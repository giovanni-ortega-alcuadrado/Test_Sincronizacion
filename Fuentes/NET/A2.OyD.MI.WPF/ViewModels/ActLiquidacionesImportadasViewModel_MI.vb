Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
'Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web.OyDMILA
Imports A2Utilidades.Mensajes

Public Class ActLiquidacionesImportadasViewModel_MI
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaImportacionLi
    Private ImportacionLiPorDefecto As ImportacionLiq_MI
    Private ImportacionLiAnterior As ImportacionLiq_MI
    Dim dcProxy As MILADomainContext
    Dim dcProxy1 As MILADomainContext
    Dim dcProxy2 As MaestrosDomainContext
    Dim mdcProxyUtilidad01 As UtilidadesDomainContext
    'Dim ValorParametroUBICACIONTITULO As List(Of Parametro)
    'Dim ValorParametroPatrimonioTecnicoFirma As List(Of Parametro)
    Dim strValidarDeposito As String = "NO" 'SLB20131022 Validar Deposito
    Private dblPatrimonioTecnico As Double = 0 'SLB20131022 Validar el patrimonio técnico de la firma
    'Dim ListaPatrimonioTecnico As List(Of PatrimonioTecnico)
    Dim EstadoConsulta As Boolean = True


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MILADomainContext
            dcProxy1 = New MILADomainContext
            dcProxy2 = New MaestrosDomainContext
            mdcProxyUtilidad01 = New UtilidadesDomainContext
        Else
            dcProxy = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy2 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.ImportacionLiq_MIs.Clear()
                'dcProxyImportacionLis.Clear()
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerImportacionLiPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiqPorDefecto_Completed, "Default")
                'dcProxy2.Load(dcProxy2.ParametrosConsultarQuery(0, "IMPLIQ_VALIDARDEPOSITO", "",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametros, "Busqueda")
                'dcProxy2.Load(dcProxy2.ParametrosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametros, "")
                ConsultarParametros()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportacionLiqViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerImportacionLiqPorDefecto_Completed(ByVal lo As LoadOperation(Of ImportacionLiq_MI))
        If Not lo.HasError Then
            ImportacionLiPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ImportacionLi por defecto", _
                                             Me.ToString(), "TerminoTraerImportacionLiPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerImportacionLiq(ByVal lo As LoadOperation(Of ImportacionLiq_MI))
        If Not lo.HasError Then
            ListaImportacionLiq = dcProxy.ImportacionLiq_MIs
            If dcProxy.ImportacionLiq_MIs.Count > 0 Then
                If lo.UserState = "insert" Then
                    ImportacionLiSelected = ListaImportacionLiq.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
            MyBase.CambioItem("ListaImportacionLiqPaged")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ImportacionLis", _
                                             Me.ToString(), "TerminoTraerImportacionLi", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerVerficarOrden(ByVal lo As LoadOperation(Of VerificarOrdenLiq_MI))
        If Not lo.HasError Then
            ListaVerificarOrdenLiq = dcProxy.VerificarOrdenLiq_MIs.ToList
            If ListaVerificarOrdenLiq.Count > 0 Then
                VerificarEstadoOrden()
            Else
                'C1.Silverlight.C1MessageBox.Show("La orden no cumple con las características de la liquidación", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Information)
                A2Utilidades.Mensajes.mostrarMensaje("La orden no cumple con las características de la liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Verificacion de la Orden", _
                     Me.ToString(), "TerminoTraerVerficarOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
    End Sub

    Private Sub TerminoVerificarCumplimientoOrder(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Dim pdblCantidadLiq = Split(lo.Value, ",")
            Dim SaldoOrden = (pdblCantidadLiq(1) - pdblCantidadLiq(0) - pdblCantidadLiq(2)) - ImportacionLiSelected.Cantidad
            If SaldoOrden < 0 Then
                'C1.Silverlight.C1MessageBox.Show("La cantidad de la operación supera el Saldo de la Orden." + vbCr + "Desea Continuar?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
                '                                 C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaValidacionSaldo)
                mostrarMensajePregunta("La cantidad de la operación supera el Saldo de la Orden.", _
                                       Program.TituloSistema, _
                                       "CUMPLIMIENTOORDEN", _
                                       AddressOf TerminoPreguntaValidacionSaldo, True, "¿Desea continuar?")
            ElseIf SaldoOrden > 0 Then
                'C1.Silverlight.C1MessageBox.Show("La cantidad de la operación es menor al Saldo disponible de la Orden. Al importar la Liquidación la Orden quedara en Estado Pendiente y con un Saldo de: " _
                '                                 + SaldoOrden.ToString + vbCrLf + "Desea Continuar?", Program.TituloSistema, _
                '                                 C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaValidacionSaldo)
                mostrarMensajePregunta("La cantidad de la operación es menor al Saldo disponible de la Orden. Al importar la Liquidación la Orden quedara en Estado Pendiente y con un Saldo de: " + SaldoOrden.ToString, _
                                       Program.TituloSistema, _
                                       "CUMPLIMIENTOORDEN", _
                                       AddressOf TerminoPreguntaValidacionSaldo, True, "¿Desea continuar?")
            Else
                Validaciones()
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del Saldo de la Orden", _
                     Me.ToString(), "TerminoVerificarCumplimientoOrder", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
    End Sub

    Private Sub TerminoPreguntaValidacionSaldo(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            Validaciones()
        Else
            IsBusy = False
        End If
    End Sub

    'Private Sub TerminoVerificarPatrimonioTecnico(ByVal lo As LoadOperation(Of PatrimonioTecnico))
    '    If Not lo.HasError Then
    '        ListaPatrimonioTecnico = dcProxy1.PatrimonioTecnicos.ToList
    '        If ListaPatrimonioTecnico.Count > 0 Then
    '            EstadoConsulta = False
    '            Validaciones()
    '        Else
    '            C1.Silverlight.C1MessageBox.Show("La orden(Renta Fija) de tipo Recompra no se pudo comparar con el Patrimonio Tecnico", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Information)
    '        End If
    '    Else
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del patrimonio tecnico de la firma", _
    '                             Me.ToString(), "TerminoVerificarPatrimonioTecnico", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        lo.MarkErrorAsHandled()   '????
    '    End If
    '    IsBusy = False
    'End Sub

    Private Sub TerminoPreguntaValidarCadena(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            GuardarImportacion()
        Else
            IsBusy = False
        End If
    End Sub

    Private Sub buscarOrdenSeleccionada(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Me._ordenselec.Cantidad = lo.Entities.ToList.Item(0).InfoAdicional01
                Me._ordenselec.Comitente = lo.Entities.ToList.Item(0).InfoAdicional02
                Me._ordenselec.Nombre = lo.Entities.ToList.Item(0).Clasificacion
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarOrdenSeleccionada", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaImportacionLiq As EntitySet(Of ImportacionLiq_MI)
    Public Property ListaImportacionLiq() As EntitySet(Of ImportacionLiq_MI)
        Get
            Return _ListaImportacionLiq
        End Get
        Set(ByVal value As EntitySet(Of ImportacionLiq_MI))
            _ListaImportacionLiq = value

            MyBase.CambioItem("ListaImportacionLiq")
            MyBase.CambioItem("ListaImportacionLiqPaged")
            If Not IsNothing(value) Then
                If IsNothing(ImportacionLiAnterior) Then
                    ImportacionLiSelected = _ListaImportacionLiq.FirstOrDefault
                Else
                    ImportacionLiSelected = ImportacionLiAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaImportacionLiqPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaImportacionLiq) Then
                Dim view = New PagedCollectionView(_ListaImportacionLiq)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ImportacionLiSelected As ImportacionLiq_MI
    Public Property ImportacionLiSelected() As ImportacionLiq_MI
        Get
            Return _ImportacionLiSelected
        End Get
        Set(ByVal value As ImportacionLiq_MI)
            _ImportacionLiSelected = value
            MyBase.CambioItem("ImportacionLiSelected")
            buscarItem("ordenselec")
        End Set
    End Property

    Private _Editarcampos As Boolean = False
    Public Property Editarcampos As Boolean
        Get
            Return _Editarcampos
        End Get
        Set(ByVal value As Boolean)
            _Editarcampos = value
            MyBase.CambioItem("Editarcampos")
        End Set
    End Property

    Private _ListaVerificarOrdenLiq As List(Of VerificarOrdenLiq_MI)
    Public Property ListaVerificarOrdenLiq() As List(Of VerificarOrdenLiq_MI)
        Get
            Return _ListaVerificarOrdenLiq
        End Get
        Set(ByVal value As List(Of VerificarOrdenLiq_MI))
            _ListaVerificarOrdenLiq = value
        End Set
    End Property

    'Public Property ordenselec As New OrdenSeleccionada
    Private _ordenselec As New OrdenSeleccionada
    Public Property ordenselec() As OrdenSeleccionada
        Get
            Return _ordenselec
        End Get
        Set(ByVal value As OrdenSeleccionada)
            _ordenselec = value
            MyBase.CambioItem("ordenselec")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no aplica para esta forma.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'Try
        '    Dim NewImportacionLi As New ImportacionLi
        '    'TODO: Verificar cuales son los campos que deben inicializarse
        '    NewImportacionLi.IDComisionista = ImportacionLiPorDefecto.IDComisionista
        '    NewImportacionLi.IDSucComisionista = ImportacionLiPorDefecto.IDSucComisionista
        '    NewImportacionLi.ID = ImportacionLiPorDefecto.ID
        '    NewImportacionLi.Parcial = ImportacionLiPorDefecto.Parcial
        '    NewImportacionLi.Tipo = ImportacionLiPorDefecto.Tipo
        '    NewImportacionLi.ClaseOrden = ImportacionLiPorDefecto.ClaseOrden
        '    NewImportacionLi.IDEspecie = ImportacionLiPorDefecto.IDEspecie
        '    NewImportacionLi.IDOrden = ImportacionLiPorDefecto.IDOrden
        '    NewImportacionLi.IDComitente = ImportacionLiPorDefecto.IDComitente
        '    NewImportacionLi.IDOrdenante = ImportacionLiPorDefecto.IDOrdenante
        '    NewImportacionLi.IDBolsa = ImportacionLiPorDefecto.IDBolsa
        '    NewImportacionLi.IDRueda = ImportacionLiPorDefecto.IDRueda
        '    NewImportacionLi.ValBolsa = ImportacionLiPorDefecto.ValBolsa
        '    NewImportacionLi.TasaDescuento = ImportacionLiPorDefecto.TasaDescuento
        '    NewImportacionLi.TasaCompraVende = ImportacionLiPorDefecto.TasaCompraVende
        '    NewImportacionLi.Modalidad = ImportacionLiPorDefecto.Modalidad
        '    NewImportacionLi.IndicadorEconomico = ImportacionLiPorDefecto.IndicadorEconomico
        '    NewImportacionLi.PuntosIndicador = ImportacionLiPorDefecto.PuntosIndicador
        '    NewImportacionLi.Plazo = ImportacionLiPorDefecto.Plazo
        '    NewImportacionLi.Liquidacion = ImportacionLiPorDefecto.Liquidacion
        '    NewImportacionLi.Cumplimiento = ImportacionLiPorDefecto.Cumplimiento
        '    NewImportacionLi.Emision = ImportacionLiPorDefecto.Emision
        '    NewImportacionLi.Vencimiento = ImportacionLiPorDefecto.Vencimiento
        '    NewImportacionLi.OtraPlaza = ImportacionLiPorDefecto.OtraPlaza
        '    NewImportacionLi.Plaza = ImportacionLiPorDefecto.Plaza
        '    NewImportacionLi.IDComisionistaLocal = ImportacionLiPorDefecto.IDComisionistaLocal
        '    NewImportacionLi.IDComisionistaOtraPlaza = ImportacionLiPorDefecto.IDComisionistaOtraPlaza
        '    NewImportacionLi.IDCiudadOtraPlaza = ImportacionLiPorDefecto.IDCiudadOtraPlaza
        '    NewImportacionLi.TasaEfectiva = ImportacionLiPorDefecto.TasaEfectiva
        '    NewImportacionLi.Cantidad = ImportacionLiPorDefecto.Cantidad
        '    NewImportacionLi.Precio = ImportacionLiPorDefecto.Precio
        '    NewImportacionLi.Transaccion_str = ImportacionLiPorDefecto.Transaccion_str
        '    NewImportacionLi.SubTotalLiq = ImportacionLiPorDefecto.SubTotalLiq
        '    NewImportacionLi.TotalLiq = ImportacionLiPorDefecto.TotalLiq
        '    NewImportacionLi.Comision = ImportacionLiPorDefecto.Comision
        '    NewImportacionLi.Retencion = ImportacionLiPorDefecto.Retencion
        '    NewImportacionLi.Intereses = ImportacionLiPorDefecto.Intereses
        '    NewImportacionLi.ValorIva = ImportacionLiPorDefecto.ValorIva
        '    NewImportacionLi.DiasIntereses = ImportacionLiPorDefecto.DiasIntereses
        '    NewImportacionLi.FactorComisionPactada = ImportacionLiPorDefecto.FactorComisionPactada
        '    NewImportacionLi.Mercado = ImportacionLiPorDefecto.Mercado
        '    NewImportacionLi.NroTitulo = ImportacionLiPorDefecto.NroTitulo
        '    NewImportacionLi.IDCiudadExpTitulo = ImportacionLiPorDefecto.IDCiudadExpTitulo
        '    NewImportacionLi.PlazoOriginal = ImportacionLiPorDefecto.PlazoOriginal
        '    NewImportacionLi.Aplazamiento = ImportacionLiPorDefecto.Aplazamiento
        '    NewImportacionLi.VersionPapeleta = ImportacionLiPorDefecto.VersionPapeleta
        '    NewImportacionLi.EmisionOriginal = ImportacionLiPorDefecto.EmisionOriginal
        '    NewImportacionLi.VencimientoOriginal = ImportacionLiPorDefecto.VencimientoOriginal
        '    NewImportacionLi.Impresiones = ImportacionLiPorDefecto.Impresiones
        '    NewImportacionLi.FormaPago = ImportacionLiPorDefecto.FormaPago
        '    NewImportacionLi.CtrlImpPapeleta = ImportacionLiPorDefecto.CtrlImpPapeleta
        '    NewImportacionLi.DiasVencimiento = ImportacionLiPorDefecto.DiasVencimiento
        '    NewImportacionLi.PosicionPropia = ImportacionLiPorDefecto.PosicionPropia
        '    NewImportacionLi.Transaccion_cur = ImportacionLiPorDefecto.Transaccion_cur
        '    NewImportacionLi.TipoOperacion = ImportacionLiPorDefecto.TipoOperacion
        '    NewImportacionLi.DiasContado = ImportacionLiPorDefecto.DiasContado
        '    NewImportacionLi.Ordinaria = ImportacionLiPorDefecto.Ordinaria
        '    NewImportacionLi.ObjetoOrdenExtraordinaria = ImportacionLiPorDefecto.ObjetoOrdenExtraordinaria
        '    NewImportacionLi.NumPadre = ImportacionLiPorDefecto.NumPadre
        '    NewImportacionLi.ParcialPadre = ImportacionLiPorDefecto.ParcialPadre
        '    NewImportacionLi.OperacionPadre = ImportacionLiPorDefecto.OperacionPadre
        '    NewImportacionLi.DiasTramo = ImportacionLiPorDefecto.DiasTramo
        '    NewImportacionLi.Vendido_log = ImportacionLiPorDefecto.Vendido_log
        '    NewImportacionLi.Vendido_dtm = ImportacionLiPorDefecto.Vendido_dtm
        '    NewImportacionLi.ValorTraslado = ImportacionLiPorDefecto.ValorTraslado
        '    NewImportacionLi.ValorBrutoCompraVencida = ImportacionLiPorDefecto.ValorBrutoCompraVencida
        '    'NewImportacionLi.AutoRetenedor = ImportacionLiPorDefecto.AutoRetenedor
        '    'NewImportacionLi.Sujeto = ImportacionLiPorDefecto.Sujeto
        '    NewImportacionLi.PcRenEfecCompraRet = ImportacionLiPorDefecto.PcRenEfecCompraRet
        '    NewImportacionLi.PcRenEfecVendeRet = ImportacionLiPorDefecto.PcRenEfecVendeRet
        '    NewImportacionLi.Reinversion = ImportacionLiPorDefecto.Reinversion
        '    NewImportacionLi.Swap = ImportacionLiPorDefecto.Swap
        '    NewImportacionLi.NroSwap = ImportacionLiPorDefecto.NroSwap
        '    'NewImportacionLi.Certificacion = ImportacionLiPorDefecto.Certificacion
        '    NewImportacionLi.DescuentoAcumula = ImportacionLiPorDefecto.DescuentoAcumula
        '    NewImportacionLi.PctRendimiento = ImportacionLiPorDefecto.PctRendimiento
        '    NewImportacionLi.FechaCompraVencido = ImportacionLiPorDefecto.FechaCompraVencido
        '    NewImportacionLi.PrecioCompraVencido = ImportacionLiPorDefecto.PrecioCompraVencido
        '    'NewImportacionLi.ConstanciaEnajenacion = ImportacionLiPorDefecto.ConstanciaEnajenacion
        '    'NewImportacionLi.RepoTitulo = ImportacionLiPorDefecto.RepoTitulo
        '    NewImportacionLi.ServBolsaVble = ImportacionLiPorDefecto.ServBolsaVble
        '    NewImportacionLi.ServBolsaFijo = ImportacionLiPorDefecto.ServBolsaFijo
        '    NewImportacionLi.Traslado = ImportacionLiPorDefecto.Traslado
        '    NewImportacionLi.UBICACIONTITULO = ImportacionLiPorDefecto.UBICACIONTITULO
        '    NewImportacionLi.TipoIdentificacion = ImportacionLiPorDefecto.TipoIdentificacion
        '    NewImportacionLi.NroDocumento = ImportacionLiPorDefecto.NroDocumento
        '    NewImportacionLi.ValorEntregaContraPago = ImportacionLiPorDefecto.ValorEntregaContraPago
        '    NewImportacionLi.AquienSeEnviaRetencion = ImportacionLiPorDefecto.AquienSeEnviaRetencion
        '    NewImportacionLi.IDBaseDias = ImportacionLiPorDefecto.IDBaseDias
        '    NewImportacionLi.TipoDeOferta = ImportacionLiPorDefecto.TipoDeOferta
        '    NewImportacionLi.HoraGrabacion = ImportacionLiPorDefecto.HoraGrabacion
        '    NewImportacionLi.OrigenOperacion = ImportacionLiPorDefecto.OrigenOperacion
        '    NewImportacionLi.CodigoOperadorCompra = ImportacionLiPorDefecto.CodigoOperadorCompra
        '    NewImportacionLi.CodigoOperadorVende = ImportacionLiPorDefecto.CodigoOperadorVende
        '    NewImportacionLi.IdentificacionRemate = ImportacionLiPorDefecto.IdentificacionRemate
        '    NewImportacionLi.ModalidaOperacion = ImportacionLiPorDefecto.ModalidaOperacion
        '    NewImportacionLi.IndicadorPrecio = ImportacionLiPorDefecto.IndicadorPrecio
        '    NewImportacionLi.PeriodoExdividendo = ImportacionLiPorDefecto.PeriodoExdividendo
        '    NewImportacionLi.PlazoOperacionRepo = ImportacionLiPorDefecto.PlazoOperacionRepo
        '    NewImportacionLi.ValorCaptacionRepo = ImportacionLiPorDefecto.ValorCaptacionRepo
        '    NewImportacionLi.VolumenCompraRepo = ImportacionLiPorDefecto.VolumenCompraRepo
        '    NewImportacionLi.PrecioNetoFraccion = ImportacionLiPorDefecto.PrecioNetoFraccion
        '    NewImportacionLi.VolumenNetoFraccion = ImportacionLiPorDefecto.VolumenNetoFraccion
        '    NewImportacionLi.CodigoContactoComercial = ImportacionLiPorDefecto.CodigoContactoComercial
        '    NewImportacionLi.NroFraccionOperacion = ImportacionLiPorDefecto.NroFraccionOperacion
        '    NewImportacionLi.IdentificacionPatrimonio1 = ImportacionLiPorDefecto.IdentificacionPatrimonio1
        '    NewImportacionLi.TipoidentificacionCliente2 = ImportacionLiPorDefecto.TipoidentificacionCliente2
        '    NewImportacionLi.NitCliente2 = ImportacionLiPorDefecto.NitCliente2
        '    NewImportacionLi.IdentificacionPatrimonio2 = ImportacionLiPorDefecto.IdentificacionPatrimonio2
        '    NewImportacionLi.TipoIdentificacionCliente3 = ImportacionLiPorDefecto.TipoIdentificacionCliente3
        '    NewImportacionLi.NitCliente3 = ImportacionLiPorDefecto.NitCliente3
        '    NewImportacionLi.IdentificacionPatrimonio3 = ImportacionLiPorDefecto.IdentificacionPatrimonio3
        '    NewImportacionLi.IndicadorOperacion = ImportacionLiPorDefecto.IndicadorOperacion
        '    NewImportacionLi.BaseRetencion = ImportacionLiPorDefecto.BaseRetencion
        '    NewImportacionLi.PorcRetencion = ImportacionLiPorDefecto.PorcRetencion
        '    NewImportacionLi.BaseRetencionTranslado = ImportacionLiPorDefecto.BaseRetencionTranslado
        '    NewImportacionLi.PorcRetencionTranslado = ImportacionLiPorDefecto.PorcRetencionTranslado
        '    NewImportacionLi.PorcIvaComision = ImportacionLiPorDefecto.PorcIvaComision
        '    NewImportacionLi.IndicadorAcciones = ImportacionLiPorDefecto.IndicadorAcciones
        '    NewImportacionLi.OperacionNegociada = ImportacionLiPorDefecto.OperacionNegociada
        '    NewImportacionLi.FechaConstancia = ImportacionLiPorDefecto.FechaConstancia
        '    NewImportacionLi.ValorConstancia = ImportacionLiPorDefecto.ValorConstancia
        '    NewImportacionLi.GeneraConstancia = ImportacionLiPorDefecto.GeneraConstancia
        '    NewImportacionLi.Actualizacion = ImportacionLiPorDefecto.Actualizacion
        '    NewImportacionLi.Usuario = Program.Usuario
        '    NewImportacionLi.CodigoIntermediario = ImportacionLiPorDefecto.CodigoIntermediario
        '    ImportacionLiAnterior = ImportacionLiSelected
        '    ImportacionLiSelected = NewImportacionLi
        '    MyBase.CambioItem("ImportacionLis")
        '    Editando = True
        '    MyBase.CambioItem("Editando")
        'Catch ex As Exception
        '    IsBusy = False
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
        '                                                 Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.ImportacionLiq_MIs.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.ID = 0
        cb.Parcial = Nothing
        cb.Liquidacion = Nothing
        cb.IDComitente = String.Empty
        cb.IDEspecie = String.Empty
        MyBase.Buscar()

    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or Not IsNothing(cb.Parcial) Or cb.IDComitente <> String.Empty Or cb.IDEspecie <> String.Empty Or Not IsNothing(cb.Liquidacion) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ImportacionLiq_MIs.Clear()
                ImportacionLiAnterior = Nothing
                IsBusy = True
                cb.Parcial = IIf(IsNothing(cb.Parcial), -1, cb.Parcial)
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " Parcial = " & cb.Parcial.ToString() & " dtmLiquidacion = " & cb.Liquidacion.ToString() & " IDcomitente = " & cb.IDComitente.ToString() & " IDEspecie = " & cb.IDEspecie.ToString()

                dcProxy.Load(dcProxy.ImportacionLiqConsultarQuery(cb.ID, cb.Parcial, cb.Liquidacion, cb.IDComitente, cb.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaImportacionLi
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If Not ImportacionLiSelected.IDOrden = 0 Then
                IsBusy = True
                dcProxy.VerificarOrdenLiq_MIs.Clear()
                dcProxy.Load(dcProxy.VerificarOrdenLiqQuery(ImportacionLiSelected.Tipo, ImportacionLiSelected.ClaseOrden, ImportacionLiSelected.IDOrden, _
                                                            ImportacionLiSelected.IDEspecie, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerVerficarOrden, "Consulta")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número de orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Verificacion de la Orden", _
                    Me.ToString(), "TerminoTraerVerficarOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                MyBase.QuitarFiltroDespuesGuardar()
                Editarcampos = False
                dcProxy.ImportacionLiq_MIs.Clear()
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "")
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ImportacionLiSelected) Then
            'If _ImportacionLiSelected.IDOrden <= 0 Then
            Editando = True
            Editarcampos = True
            'Else
            ' C1.Silverlight.C1MessageBox.Show("La liquidación ya tiene asociada una orden, no puede realizar esta operación.", "Se canceló la operación solicitada", C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
            'A2Utilidades.Mensajes.mostrarMensaje("La liquidación ya tiene asociada una orden, no puede realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'End If
        Else
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("No hay registro seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ImportacionLiSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editarcampos = False
                If _ImportacionLiSelected.EntityState = EntityState.Detached Then
                    ImportacionLiSelected = ImportacionLiAnterior
                End If
                LimpiarOrdenSeleccionada()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para esta forma.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Try
            If Not IsNothing(_ImportacionLiSelected) Then
                dcProxy.ImportacionLiq_MIs.Remove(_ImportacionLiSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                dcProxy.ImportacionLiq_MIs.Clear()
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "update")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub GuardarImportacion()
        Try
            Dim origen = "update"
            ErrorForma = ""
            ImportacionLiSelected.Usuario = Program.Usuario
            ImportacionLiAnterior = ImportacionLiSelected
            If Not ListaImportacionLiq.Contains(ImportacionLiSelected) Then
                origen = "insert"
                ListaImportacionLiq.Add(ImportacionLiSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerificarEstadoOrden()
        If ListaVerificarOrdenLiq.Count > 0 Then
            If ListaVerificarOrdenLiq.ToList.Item(0).strEstado = "A" Then
                'C1.Silverlight.C1MessageBox.Show("La Orden está Cancelada. No es posible modificar la liquidación", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
                mostrarMensaje("La Orden está Cancelada. No es posible modificar la liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If ListaVerificarOrdenLiq.ToList.Item(0).strEstado = "C" Then
                'C1.Silverlight.C1MessageBox.Show("La Orden está Cumplida. No es posible modificar la liquidación", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
                mostrarMensaje("La Orden está Cumplida. No es posible modificar la liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
        End If
        'Lanzo el SP sp_CumplimientoOrden
        Try
            dcProxy.CumplimientoOrden(ImportacionLiSelected.Tipo, ImportacionLiSelected.ClaseOrden, ImportacionLiSelected.IDOrden, _
                                        ImportacionLiSelected.IDEspecie, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarCumplimientoOrder, "Consulta")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del cumplimiento de la Orden", _
            Me.ToString(), "sp_CumplimientoOrden_OyDNet", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Function ValidarUBICACIONTITULO() As Boolean
        Try
            ValidarUBICACIONTITULO = True
            If strValidarDeposito.Equals("SI") And EstadoConsulta Then 'ValorParametroUBICACIONTITULO.Item(0).Valor
                If ImportacionLiSelected.UBICACIONTITULO <> ListaVerificarOrdenLiq.Item(0).strUBICACIONTITULO Then
                    'C1.Silverlight.C1MessageBox.Show("La ubicación del titulo de la orden es diferente al de la liquidacion, por favor verifique!", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
                    A2Utilidades.Mensajes.mostrarMensaje("La ubicación del titulo de la orden es diferente al de la liquidacion, por favor verifique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ValidarUBICACIONTITULO = False
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificacion de la ubicacion del Titulo", _
                Me.ToString(), "ValidarUBICACIONTITULO", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
        'Return ValidarUBICACIONTITULO
    End Function

    Public Function ExistenDiferencias(ByRef strCadena As String) As Boolean
        Try
            ExistenDiferencias = False
            strCadena = "Existen diferencias entre:" + vbCr

            'Comparamos el ID del cliente que envia la bolsa, con el ID del cliente en la orden
            'If ImportacionLiSelected.TipoIdentificacion <> ListaVerificarOrdenLiq.Item(0).strTipoIdentificacion Then
            '    strCadena = String.Format("{0}El tipo de ID Bolsa [{1}] y la Orden [{2}] {3}", strCadena, ImportacionLiSelected.TipoIdentificacion, ListaVerificarOrdenLiq.Item(0).strTipoIdentificacion, vbCrLf)
            '    ExistenDiferencias = True
            'End If

            'Comparamos el Nro de ID del cliente que envia la bolsa, con el Nro de ID del cliente en la orden
            If ImportacionLiSelected.NroDocumento <> ListaVerificarOrdenLiq.Item(0).strNroDocumento Then
                strCadena = String.Format("{0}El Nro del Documento de Identificación de la Bolsa [{1}] y la Orden [{2}] {3}", strCadena, ImportacionLiSelected.NroDocumento, ListaVerificarOrdenLiq.Item(0).strNroDocumento, vbCrLf)
                ExistenDiferencias = True
            End If

            'Comparamos la fecha de emision que envia la bolsa con la fecha de emisión en la orden
            If IsDate(ImportacionLiSelected.Emision) And IsDate(ListaVerificarOrdenLiq.Item(0).dtmEmision) Then
                If CDate(ImportacionLiSelected.Emision) <> CDate(ListaVerificarOrdenLiq.Item(0).dtmEmision) Then
                    strCadena = strCadena + "La fecha de emisión de la Bolsa [" + ImportacionLiSelected.Emision + "] y la Orden [" + ListaVerificarOrdenLiq.Item(0).dtmEmision + "]" + vbCrLf
                    'strCadena = String.Format("{0]La fecha de emisión de la Bolsa [{1}] y la Orden [{2}] {3}", strCadena, ImportacionLiSelected.Emision, ListaVerificarOrdenLiq.Item(0).dtmEmision, vbCrLf)
                    ExistenDiferencias = True
                End If
            End If

            'Comparamos la fecha de vencimiento que envia la bolsa con la fecha de vencimiento en la orden
            If IsDate(ImportacionLiSelected.Vencimiento) And IsDate(ListaVerificarOrdenLiq.Item(0).dtmVencimiento) Then
                If CDate(ImportacionLiSelected.Vencimiento) <> CDate(ListaVerificarOrdenLiq.Item(0).dtmVencimiento) Then
                    strCadena = strCadena + "La fecha de emisión de la Bolsa [" + ImportacionLiSelected.Vencimiento + "] y la Orden [" + ListaVerificarOrdenLiq.Item(0).dtmVencimiento + "]" + vbCrLf
                    'strCadena = String.Format("{0}La fecha de vencimiento de la Bolsa [{1}] y la Orden [{2}] {3}", strCadena, ImportacionLiSelected.Vencimiento, ListaVerificarOrdenLiq.Item(0).dtmVencimiento, vbCrLf)
                    ExistenDiferencias = True
                End If
            End If
            strCadena = strCadena & "Desea Continuar? "
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificacion de la Liquidacion", _
                    Me.ToString(), "sp_CumplimientoOrden_OyDNet", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
    End Function

    Public Function ValidarPatrimonioTecnico() As Boolean
        Try
            ValidarPatrimonioTecnico = True

            'If ImportacionLiSelected.ClaseOrden = "C" And ImportacionLiSelected.Tipo = "R" And ValorParametroPatrimonioTecnicoFirma.Item(0).Valor > 0 And EstadoConsulta Then
            '    ValidarPatrimonioTecnico = False
            '    dcProxy1.PatrimonioTecnicos.Clear()
            '    dcProxy1.Load(dcProxy1.PatrimonioTecnicoQuery(ImportacionLiSelected.IDComitente, ImportacionLiSelected.Cumplimiento, _
            '                                                  ImportacionLiSelected.TotalLiq,Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarPatrimonioTecnico, "Consulta")
            'End If
            'If Not EstadoConsulta Then
            '    ValidarPatrimonioTecnico = True
            '    EstadoConsulta = True
            '    Dim dblTotalFuturoRepo = ListaPatrimonioTecnico.Item(0).TotalRecompras + _
            '                ListaPatrimonioTecnico.Item(0).TotalFuturoRepoOrdenes + _
            '                ListaPatrimonioTecnico.Item(0).TotalRecomprasImportadas
            '    If ListaPatrimonioTecnico.Item(0).SuperaPatrimonioTecnico > 0 Then
            '        ValidarPatrimonioTecnico = False
            '        C1.Silverlight.C1MessageBox.Show(String.Format("La orden no puede ser asignada, el acumulado de la operacion {0} supera el valor del patrimonio técnico de la firma: {1}", _
            '                                                       Format(ImportacionLiSelected.TotalLiq, "$##,##0.00"), Format(CDbl(ValorParametroPatrimonioTecnicoFirma.Item(0).Valor), "$##,##0.00")), _
            '                                                   Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
            '        IsBusy = False
            '    End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validacion del Patrimonio Tecnico", _
        Me.ToString(), "ValidarPatrimonioTecnico", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
    End Function

    Public Sub Validaciones()
        If ValidarUBICACIONTITULO() Then
            If ValidarPatrimonioTecnico() Then
                Dim strCadena As String = String.Empty
                If ExistenDiferencias(strCadena) Then
                    'C1.Silverlight.C1MessageBox.Show(strCadena, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
                    '                                 C1.Silverlight.C1MessageBoxIcon.Error,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaValidarCadena)
                    mostrarMensajePregunta(strCadena, _
                                           Program.TituloSistema, _
                                           "VALIDACIONES", _
                                           AddressOf TerminoPreguntaValidarCadena, False)
                Else
                    GuardarImportacion()
                End If
            End If
        End If
    End Sub

    Public Sub LimpiarOrdenSeleccionada()
        If ImportacionLiSelected.IDOrden.Equals(0) Then
            Me._ordenselec.Cantidad = Nothing
            Me._ordenselec.Nombre = String.Empty
            Me._ordenselec.Comitente = String.Empty
        End If
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False
        Try
            If Not Me.ImportacionLiSelected Is Nothing Then
                Select Case pstrTipoItem
                    Case "ordenselec"
                        If (ImportacionLiSelected.IDOrden <> 0) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ImportacionLiSelected.IDOrden
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                Dim strAgrupacion = ImportacionLiSelected.ClaseOrden + "," + ImportacionLiSelected.Tipo + "." + ImportacionLiSelected.IDEspecie
                                mdcProxyUtilidad01.BuscadorGenericos.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemsQuery(ImportacionLiSelected.IDOrden, "BuscarOrdenMILA", "A", strAgrupacion, "", "", Program.Usuario, Program.HashConexion), AddressOf buscarOrdenSeleccionada, pstrTipoItem)
                            End If
                        Else
                            LimpiarOrdenSeleccionada()
                        End If
                    Case Else
                        logConsultar = False
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la Orden", Me.ToString(), "buscarItem", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region
#Region "Consultar parametros y campos obligatorios"

    ''' <summary>
    ''' Método para consultar los parámetros que se Utilizan en Tesoreria
    ''' </summary>
    ''' <remarks>SLB20130204</remarks>
    Private Sub ConsultarParametros()
        Try
            mdcProxyUtilidad01.Verificaparametro("IMPLIQ_VALIDARDEPOSITO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "IMPLIQ_VALIDARDEPOSITO")
            mdcProxyUtilidad01.Verificaparametro("PATRIMONIOTECNICO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "PATRIMONIOTECNICO")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de Tesorería", _
                                 Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados de Tesoreria
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    ''' <remarks>SLB20130204</remarks>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState
                Case "IMPLIQ_VALIDARDEPOSITO"
                    strValidarDeposito = lo.Value.ToString
                    '_mlogActivarListaClinton = IIf(lo.Value.ToString.Equals("SI"), True, False)
                Case "PATRIMONIOTECNICO"
                    dblPatrimonioTecnico = CDbl(lo.Value)
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region
End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaImportacionLi
    Implements INotifyPropertyChanged

    Private _ID As Integer = 0
    <Display(Name:="Liquidación")> _
    Public Property ID As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Parcial As Integer? = Nothing
    <Display(Name:="Parcial")> _
    Public Property Parcial As Integer?
        Get
            Return _Parcial
        End Get
        Set(ByVal value As Integer?)
            _Parcial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Parcial"))
        End Set
    End Property

    '<Display(Name:="Parcial")> _
    'Public Property Parcial As Integer? = Nothing

    Private _Liquidacion As DateTime? = Nothing
    <Display(Name:="Fecha Liquidación")> _
    Public Property Liquidacion As DateTime?
        Get
            Return _Liquidacion
        End Get
        Set(ByVal value As DateTime?)
            _Liquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Liquidacion"))
        End Set
    End Property

    '<Display(Name:="Fecha Liquidación")> _
    'Public Property Liquidacion As DateTime?

    '<Display(Name:="Comitente")> _
    'Public Property IDComitente As String

    Private _IDComitente As String
    <Display(Name:="Comitente")> _
    Public Property IDComitente As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    '<Display(Name:="Especie")> _
    'Public Property IDEspecie As String

    Private _IDEspecie As String
    <Display(Name:="Especie")> _
    Public Property IDEspecie As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

'Clase para mostrar mas información de la orden seleccionada
Public Class OrdenSeleccionada
    Implements INotifyPropertyChanged

    Private _Cantidad As Decimal?
    <Display(Name:="Cantidad")> _
    Public Property Cantidad() As Decimal?
        Get
            Return _Cantidad
        End Get
        Set(ByVal value As Decimal?)
            _Cantidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cantidad"))
        End Set
    End Property

    Private _Comitente As String
    <Display(Name:="Comitente")> _
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _Nombre As String
    <Display(Name:="Nombre")> _
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




