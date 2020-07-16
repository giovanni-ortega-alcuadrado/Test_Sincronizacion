Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ImportacionLiqViewModel.vb
'Generado el : 07/19/2011 09:26:12
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Text
Imports A2Utilidades.Mensajes

Public Class ActLiquidacionesImportadasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaImportacionLi
    Private ImportacionLiPorDefecto As ImportacionLi
    Private ImportacionLiAnterior As ImportacionLi
    Dim dcProxy As ImportacionesDomainContext
    Dim dcProxy1 As ImportacionesDomainContext
    Dim mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim ListaPatrimonioTecnico As List(Of PatrimonioTecnico)
    Dim EstadoConsulta As Boolean = True

    'Dim ValorParametroUBICACIONTITULO As List(Of Parametro)
    'Dim ValorParametroPatrimonioTecnicoFirma As List(Of Parametro)
    Dim strValidarDeposito As String = "NO" 'SLB20131022 Validar Deposito
    Private dblPatrimonioTecnico As Double = 0 'SLB20131022 Validar el patrimonio técnico de la firma
    Dim FechaCierre As DateTime

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
            dcProxy1 = New ImportacionesDomainContext
            objProxy = New UtilidadesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.ImportacionLis.Clear()
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM")
                dcProxy1.Load(dcProxy1.TraerImportacionLiPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiqPorDefecto_Completed, "Default")
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                ConsultarParametros()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ImportacionLiqViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerImportacionLiqPorDefecto_Completed(ByVal lo As LoadOperation(Of ImportacionLi))
        If Not lo.HasError Then
            ImportacionLiPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ImportacionLi por defecto", _
                                             Me.ToString(), "TerminoTraerImportacionLiPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = obj.Value
        End If
    End Sub

    Private Sub TerminoTraerImportacionLiq(ByVal lo As LoadOperation(Of ImportacionLi))
        If Not lo.HasError Then
            ListaImportacionLiq = dcProxy.ImportacionLis
            If dcProxy.ImportacionLis.Count > 0 Then
                If lo.UserState = "insert" Then
                    ImportacionLiSelected = ListaImportacionLiq.Last
                ElseIf lo.UserState.ToString.Contains("FiltroVM&") Then
                    Dim strRegistroLiquidacion As String = lo.UserState.ToString.Split("&")(1)
                    Dim strIDLiquidacion As String = strRegistroLiquidacion.Split("-")(0)
                    Dim strParcialLiquidacion As String = strRegistroLiquidacion.Split("-")(1)
                    Dim strTipoLiquidacion As String = strRegistroLiquidacion.Split("-")(2)
                    Dim strClaseLiquidacion As String = strRegistroLiquidacion.Split("-")(3)
                    Dim strIDBolsa As String = strRegistroLiquidacion.Split("-")(4)
                    Dim strFechaLiquidacion As String = strRegistroLiquidacion.Split("-")(5)

                    If ListaImportacionLiq.Where(Function(i) i.ID.ToString.Equals(strIDLiquidacion) _
                                                     And i.Parcial.ToString.Equals(strParcialLiquidacion) _
                                                     And i.Tipo.ToString.Equals(strTipoLiquidacion) _
                                                     And i.ClaseOrden.ToString.Equals(strClaseLiquidacion) _
                                                     And i.IDBolsa.ToString.Equals(strIDBolsa) _
                                                     And i.Liquidacion.Value.ToString("yyyyMMdd").Equals(strFechaLiquidacion)).Count > 0 Then
                        ImportacionLiSelected = Nothing
                        ImportacionLiSelected = ListaImportacionLiq.Where(Function(i) i.ID.ToString.Equals(strIDLiquidacion) _
                                                     And i.Parcial.ToString.Equals(strParcialLiquidacion) _
                                                     And i.Tipo.ToString.Equals(strTipoLiquidacion) _
                                                     And i.ClaseOrden.ToString.Equals(strClaseLiquidacion) _
                                                     And i.IDBolsa.ToString.Equals(strIDBolsa) _
                                                     And i.Liquidacion.Value.ToString("yyyyMMdd").Equals(strFechaLiquidacion)).First
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ImportacionLis", _
                                             Me.ToString(), "TerminoTraerImportacionLi", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Public Sub Autorefresh()
        Try
            If Not Program.IsDesignMode() Then
                If Not IsNothing(_ImportacionLiSelected) Then
                    ImportacionLiAnterior = Nothing
                    ' Santiago Vergara - Octubre 08/2014 - se ajusta lógica para que haga la consulta dependiendo de si el cliente esta por aprobar o esta aprobado
                    Dim strID As String = String.Format("{0}-{1}-{2}-{3}-{4}-{5}",
                                                        _ImportacionLiSelected.ID,
                                                        _ImportacionLiSelected.Parcial,
                                                        _ImportacionLiSelected.Tipo,
                                                        _ImportacionLiSelected.ClaseOrden,
                                                        _ImportacionLiSelected.IDBolsa,
                                                        _ImportacionLiSelected.Liquidacion.Value.ToString("yyyyMMdd"))

                    dcProxy.ImportacionLis.Clear()
                        IsBusy = True
                    If FiltroVM.Length > 0 Then
                        Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                        dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM")
                    Else
                        dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM&" & strID)
                    End If
                Else
                    dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Autorefresh", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerVerficarOrden(ByVal lo As LoadOperation(Of VerificarOrdenLiq))
        If Not lo.HasError Then
            ListaVerificarOrdenLiq = dcProxy.VerificarOrdenLiqs.ToList
            If ListaVerificarOrdenLiq.Count > 0 Then
                VerificarEstadoOrden()
            Else
                'C1.Silverlight.C1MessageBox.Show("La orden no cumple con las características de la liquidación", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Information)
                A2Utilidades.Mensajes.mostrarMensaje("La orden no cumple con las características de la liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Verificacion de la Orden",
                     Me.ToString(), "TerminoTraerVerficarOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
    End Sub
    Private Sub TerminoTraerVerficarLiqExistente(ByVal lo As LoadOperation(Of verificarLiqExistente))
        If Not lo.HasError Then
            ListaVerificarLiqExistente = dcProxy.verificarLiqExistentes.ToList
            IsBusy = False
            'If ListaVerificarOrdenLiq.Count > 0 Then
            '    VerificarEstadoOrden()
            'Else
            '    'C1.Silverlight.C1MessageBox.Show("La orden no cumple con las características de la liquidación", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Information)
            '    A2Utilidades.Mensajes.mostrarMensaje("La orden no cumple con las características de la liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    IsBusy = False
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Verificacion de la Orden",
                     Me.ToString(), "TerminoTraerVerficarOrden", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
    End Sub

    Private Sub TerminoVerificarCumplimientoOrder(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Dim pdblCantidadLiq = Split(lo.Value, ",")

            'Jorge Andres Bedoya 20150506
            'Si pdblCantidadLiq(3) = 1 la orden es en pesos y se debe evaluar contra el total de la liquidacion
            Dim SaldoOrden = (pdblCantidadLiq(1) - pdblCantidadLiq(0) - pdblCantidadLiq(2)) - IIf(pdblCantidadLiq(3) = 1, ImportacionLiSelected.TotalLiq, ImportacionLiSelected.Cantidad)
            If SaldoOrden < 0 Then
                'C1.Silverlight.C1MessageBox.Show("La cantidad de la operación supera el Saldo de la Orden." + vbCr + "Desea Continuar?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
                '                                 C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminoPreguntaValidacionSaldo)
                mostrarMensajePregunta("La cantidad de la operación supera el Saldo de la Orden.", _
                                       Program.TituloSistema, _
                                       "VERIFICARCUMPLIMIENTO", _
                                       AddressOf TerminoPreguntaValidacionSaldo, True, "¿Desea continuar?")
            ElseIf SaldoOrden > 0 Then
                'C1.Silverlight.C1MessageBox.Show("La cantidad de la operación es menor al Saldo disponible de la Orden. Al importar la Liquidación la Orden quedara en Estado Pendiente y con un Saldo de: " _
                '                                 + SaldoOrden.ToString + vbCrLf + "Desea Continuar?", Program.TituloSistema, _
                '                                 C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminoPreguntaValidacionSaldo)
                mostrarMensajePregunta("La cantidad de la operación es menor al Saldo disponible de la Orden. Al importar la Liquidación la Orden quedara en Estado Pendiente y con un Saldo de: " + SaldoOrden.ToString, _
                                       Program.TituloSistema, _
                                       "VERIFICARCUMPLIMIENTO", _
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

    Private Sub TerminoVerificarPatrimonioTecnico(ByVal lo As LoadOperation(Of PatrimonioTecnico))
        If Not lo.HasError Then
            ListaPatrimonioTecnico = dcProxy1.PatrimonioTecnicos.ToList
            If ListaPatrimonioTecnico.Count > 0 Then
                EstadoConsulta = False
                Validaciones()
            Else
                'C1.Silverlight.C1MessageBox.Show("La orden(Renta Fija) de tipo Recompra no se pudo comparar con el Patrimonio Tecnico", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Information)
                mostrarMensaje("La orden(Renta Fija) de tipo Recompra no se pudo comparar con el Patrimonio Tecnico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del patrimonio tecnico de la firma", _
                                 Me.ToString(), "TerminoVerificarPatrimonioTecnico", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

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

    Private _ListaImportacionLiq As EntitySet(Of ImportacionLi)
    Public Property ListaImportacionLiq() As EntitySet(Of ImportacionLi)
        Get
            Return _ListaImportacionLiq
        End Get
        Set(ByVal value As EntitySet(Of ImportacionLi))
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

    Private _ImportacionLiSelected As ImportacionLi
    Public Property ImportacionLiSelected() As ImportacionLi
        Get
            Return _ImportacionLiSelected
        End Get
        Set(ByVal value As ImportacionLi)
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


    Private _ListaVerificarOrdenLiq As List(Of VerificarOrdenLiq)
    Public Property ListaVerificarOrdenLiq() As List(Of VerificarOrdenLiq)
        Get
            Return _ListaVerificarOrdenLiq
        End Get
        Set(ByVal value As List(Of VerificarOrdenLiq))
            _ListaVerificarOrdenLiq = value
        End Set
    End Property
    Private _ListaVerificarLiqExistente As List(Of verificarLiqExistente)
    Public Property ListaVerificarLiqExistente() As List(Of verificarLiqExistente)
        Get
            Return _ListaVerificarLiqExistente
        End Get
        Set(ByVal value As List(Of verificarLiqExistente))
            _ListaVerificarLiqExistente = value
        End Set
    End Property

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
            dcProxy.ImportacionLis.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "FiltroVM")
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
                dcProxy.ImportacionLis.Clear()
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
                If validaFechaCierre("Actualizar") Then
                    IsBusy = True
                    dcProxy.VerificarOrdenLiqs.Clear()
                    dcProxy.Load(dcProxy.VerificarOrdenLiqQuery(ImportacionLiSelected.Tipo, ImportacionLiSelected.ClaseOrden, ImportacionLiSelected.IDOrden, _
                                                                ImportacionLiSelected.IDEspecie, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerVerficarOrden, "Consulta")
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número de orden", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Verificacion de la Orden", _
                    Me.ToString(), "TerminoTraerVerficarOrden", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Valida la Fecha de Cierre del Sistema
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Private Function validaFechaCierre(ByVal pstrAccion As String) As Boolean
        validaFechaCierre = True
        If Format(CType(_ImportacionLiSelected.Liquidacion, Date).Date, "yyyy/MM/dd") <= Format(FechaCierre, "yyyy/MM/dd") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
            If Format(FechaCierre, "yyyy/MM/dd") <> "1900/01/01" Then
                Select Case pstrAccion
                    Case "Actualizar"
                        A2Utilidades.Mensajes.mostrarMensaje("No es posible asociar la orden a la operación con fecha (" & CType(_ImportacionLiSelected.Liquidacion, Date).Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End Select
                validaFechaCierre = False
            End If
        End If
        Return validaFechaCierre
    End Function


    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                MyBase.QuitarFiltroDespuesGuardar()
                Editarcampos = False
                dcProxy.ImportacionLis.Clear()
                dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerImportacionLiq, "")
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ImportacionLiSelected) Then
            'If _ImportacionLiSelected.IDOrden <= 0 Then
            Editando = True
            Editarcampos = True
            IsBusy = True
            'dcProxy.verificar.Clear()
            dcProxy.verificarLiqExistentes.Clear()
            dcProxy.Load(dcProxy.verificarLiqExistenteQuery(ImportacionLiSelected.ID, ImportacionLiSelected.Tipo, ImportacionLiSelected.Clase, ImportacionLiSelected.Liquidacion,
                                                      ImportacionLiSelected.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerVerficarLiqExistente, "Consulta")
            'Else
            ' C1.Silverlight.C1MessageBox.Show("La liquidación ya tiene asociada una orden, no puede realizar esta operación.", "Se canceló la operación solicitada", C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
            'A2Utilidades.Mensajes.mostrarMensaje("La liquidación ya tiene asociada una orden, no puede realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'End If
        Else
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
                dcProxy.ImportacionLis.Remove(_ImportacionLiSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                'dcProxy.ImportacionLis.Clear()
                'dcProxy.Load(dcProxy.ImportacionLiqFiltrarQuery(""), AddressOf TerminoTraerImportacionLiq, "update")
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
                                        ImportacionLiSelected.IDEspecie, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarCumplimientoOrder, "Consulta")
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
            Dim TipoOfertaOrden As String = ""
            Dim NombreTipoOfertaOrden As String = ""
            Dim TipoOfertacarga As String = ""
            Dim NombreTipoOfertacarga As String = ""

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
                    strCadena = strCadena + "La fecha de vencimiento de la Bolsa [" + ImportacionLiSelected.Vencimiento + "] y la Orden [" + ListaVerificarOrdenLiq.Item(0).dtmVencimiento + "]" + vbCrLf
                    'strCadena = String.Format("{0}La fecha de vencimiento de la Bolsa [{1}] y la Orden [{2}] {3}", strCadena, ImportacionLiSelected.Vencimiento, ListaVerificarOrdenLiq.Item(0).dtmVencimiento, vbCrLf)
                    ExistenDiferencias = True
                End If
            End If

            TipoOfertaOrden = ImportacionLiSelected.TipoDeOferta
            TipoOfertacarga = ListaVerificarOrdenLiq.Item(0).strObjeto
            TipoOfertacarga = TipoOfertacarga.Trim

            If TipoOfertacarga = "NO" Then
                TipoOfertacarga = ""
            End If

            Select Case TipoOfertaOrden
                Case "N"
                    TipoOfertaOrden = " "
                    NombreTipoOfertaOrden = "Normal"
                Case "NO"
                    TipoOfertaOrden = " "
                    NombreTipoOfertaOrden = "Normal"
                Case "R"
                    TipoOfertaOrden = "RP"
                    NombreTipoOfertaOrden = "Repo Renta Fija"
                Case "A"
                    TipoOfertaOrden = "RP"
                    NombreTipoOfertaOrden = "Repo Acciones"
                Case "P"
                    TipoOfertaOrden = "OPA"
                    NombreTipoOfertaOrden = "OPA"
                Case "M"
                    TipoOfertaOrden = "MA"
                    NombreTipoOfertaOrden = "Martillo"
                Case "F"
                    TipoOfertaOrden = "FD"
                    NombreTipoOfertaOrden = "Fondeo"
                Case "S"
                    TipoOfertaOrden = "SUB"
                    NombreTipoOfertaOrden = "Subasta"
                Case "C"
                    TipoOfertaOrden = "CRR"
                    NombreTipoOfertaOrden = "Carrusel"
                Case "1"
                    TipoOfertaOrden = "1"
                    NombreTipoOfertaOrden = "Simultáneas de Salida"
                Case "2"
                    TipoOfertaOrden = "2"
                    NombreTipoOfertaOrden = "Simultánea de Regreso"
                Case "3"
                    TipoOfertaOrden = "3"
                    NombreTipoOfertaOrden = "TTV de Salida"
                Case "4"
                    TipoOfertaOrden = "4"
                    NombreTipoOfertaOrden = "TTV de Regreso"
                Case "5"
                    TipoOfertaOrden = "5"
                    NombreTipoOfertaOrden = "Simultánea camara de salida"
                Case "6"
                    TipoOfertaOrden = "6"
                    NombreTipoOfertaOrden = "Simultánea camara de Regreso"
                Case "O"
                    TipoOfertaOrden = "-1"
                    NombreTipoOfertaOrden = "Opciones"
                Case "D"
                    TipoOfertaOrden = "-1"
                    NombreTipoOfertaOrden = "Forward de registro"
                Case "I"
                    TipoOfertaOrden = "-1"
                    NombreTipoOfertaOrden = "Interbancario"
                Case "X"
                    TipoOfertaOrden = "-1"
                    NombreTipoOfertaOrden = "Otras"
            End Select

            Select Case TipoOfertacarga
                Case ""
                    NombreTipoOfertacarga = "Normal"
                Case "NO"
                    NombreTipoOfertacarga = "Normal"
                Case "RP"
                    NombreTipoOfertacarga = "Repo Renta Fija"
                Case "RP"
                    NombreTipoOfertacarga = "Repo"
                Case "OPA"
                    NombreTipoOfertacarga = "OPA"
                Case "MA"
                    NombreTipoOfertacarga = "Martillo"
                Case "FD"
                    NombreTipoOfertacarga = "Fondeo"
                Case "SUB"
                    NombreTipoOfertacarga = "Subasta"
                Case "CRR"
                    NombreTipoOfertacarga = "Carrusel"
                Case "1"
                    NombreTipoOfertacarga = "Simultáneas de Salida"
                Case "2"
                    NombreTipoOfertacarga = "Simultánea de Regreso"
                Case "3"
                    NombreTipoOfertacarga = "TTV de Salida"
                Case "4"
                    NombreTipoOfertacarga = "TTV de Regreso"
                Case "5"
                    NombreTipoOfertacarga = "Simultánea camara de salida"
                Case "6"
                    NombreTipoOfertacarga = "Simultánea camara de Regreso"
                    'Case "O"
                    '    NombreTipoOfertacarga = "Opciones"
                    'Case "D"
                    '    TipoOfertacarga = "-1"
                    '    NombreTipoOfertacarga = "Forward de registro"
                    'Case "I"
                    '    TipoOfertacarga = "-1"
                    '    NombreTipoOfertacarga = "Interbancario"
                    'Case "X"
                    '    TipoOfertacarga = "-1"
                    '    NombreTipoOfertacarga = "Otras"
            End Select

            ''Comparamos la clase de la operación sea Igual a la clase de la orden  JAEZ 20170510
            'If ImportacionLiSelected.Clase <> ListaVerificarOrdenLiq.Item(0).strClase Then
            '    strCadena = strCadena + "La clase de la Bolsa [" + ImportacionLiSelected.Clase + "] y la Orden [" + ListaVerificarOrdenLiq.Item(0).strClase + "]" + vbCrLf
            '    'strCadena = String.Format("{0}La clase de la Bolsa [{1}] y la Orden [{2}] {3}", strCadena, ImportacionLiSelected.Clase, ListaVerificarOrdenLiq.Item(0).strClase, vbCrLf)
            '    ExistenDiferencias = True
            'End If

            If TipoOfertaOrden.Trim <> TipoOfertacarga Then
                strCadena = strCadena + "La clasificación de la liquidación [" + NombreTipoOfertaOrden + "] y la clasificación de la Orden [" + NombreTipoOfertacarga + "]" + vbCrLf
                ExistenDiferencias = True
            End If

            If ListaVerificarLiqExistente.Count > 0 Then
                If (ImportacionLiSelected.Parcial = 0 And ListaVerificarLiqExistente.Last.lngParcial > ImportacionLiSelected.Parcial) Then
                    strCadena = strCadena + "El folio [" + ImportacionLiSelected.ID.ToString + "] - [" + ImportacionLiSelected.Parcial.ToString + "] de tipo " + ImportacionLiSelected.Tipo + " ya existe en OyD fraccionado, retire el folio con parcial 0, antes de intentar cargar el folio con fracciones." + vbCrLf
                    ExistenDiferencias = True
                End If

                If (ListaVerificarLiqExistente.Last.lngParcial = 0 And ImportacionLiSelected.Parcial > ListaVerificarLiqExistente.Last.lngParcial) Then
                    strCadena = strCadena + "El folio [" + ListaVerificarLiqExistente.ToList.Item(0).lngIDLiquidacion.ToString + "] - [" + ListaVerificarLiqExistente.ToList.Item(0).lngParcial.ToString + "] de tipo" + ListaVerificarLiqExistente.ToList.Item(0).strTipo + " ya existe en OyD sin fraccionar. Retire el folio con parcial 0, antes de intentar cargar el folio con fracciones. " + vbCrLf
                    ExistenDiferencias = True
                End If
            End If

            strCadena = strCadena & "Desea Continuar? "
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificacion de la Liquidacion",
                    Me.ToString(), "ExistenDiferencias", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
    End Function

    Public Function ValidarPatrimonioTecnico() As Boolean
        Try
            ValidarPatrimonioTecnico = True

            If ImportacionLiSelected.ClaseOrden = "C" And ImportacionLiSelected.Tipo = "R" And dblPatrimonioTecnico > 0 And EstadoConsulta Then 'ValorParametroPatrimonioTecnicoFirma.Item(0).Valor > 0
                ValidarPatrimonioTecnico = False
                dcProxy1.PatrimonioTecnicos.Clear()
                dcProxy1.Load(dcProxy1.PatrimonioTecnicoQuery(ImportacionLiSelected.IDComitente, ImportacionLiSelected.Cumplimiento, _
                                                              ImportacionLiSelected.TotalLiq, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificarPatrimonioTecnico, "Consulta")
            End If
            If Not EstadoConsulta Then
                ValidarPatrimonioTecnico = True
                EstadoConsulta = True
                Dim dblTotalFuturoRepo = ListaPatrimonioTecnico.Item(0).TotalRecompras + _
                            ListaPatrimonioTecnico.Item(0).TotalFuturoRepoOrdenes + _
                            ListaPatrimonioTecnico.Item(0).TotalRecomprasImportadas
                If ListaPatrimonioTecnico.Item(0).SuperaPatrimonioTecnico > 0 Then
                    ValidarPatrimonioTecnico = False

                    mostrarMensaje(String.Format("La orden no puede ser asignada, el acumulado de la operacion {0} supera el valor del patrimonio técnico de la firma: {1}", _
                                                                   Format(ImportacionLiSelected.TotalLiq, "$##,##0.00"), Format(dblPatrimonioTecnico, "$##,##0.00")), _
                                                               Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validacion del Patrimonio Tecnico", _
        Me.ToString(), "ValidarPatrimonioTecnico", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
    End Function

    Public Sub Validaciones()
        If validarTipoOferta() Then
            If ValidarUBICACIONTITULO() Then
                If ValidarPatrimonioTecnico() Then
                    Dim strCadena As String = String.Empty
                    If ExistenDiferencias(strCadena) Then
                        'C1.Silverlight.C1MessageBox.Show(strCadena, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
                        '                                 'C1.Silverlight.C1MessageBoxIcon.Error, AddressOf TerminoPreguntaValidarCadena)
                        mostrarMensajePregunta(strCadena, Program.TituloSistema, "VALIDACIONES", AddressOf TerminoPreguntaValidarCadena, False)
                    Else
                        GuardarImportacion()
                    End If
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
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemsQuery(ImportacionLiSelected.IDOrden, "BuscarOrden", "A", strAgrupacion, "", "", Program.Usuario, Program.HashConexion), AddressOf buscarOrdenSeleccionada, pstrTipoItem)
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
            'ValorParametroUBICACIONTITULO = ListaParametros.Where(Function(p) p.Parametro.Equals("IMPLIQ_VALIDARDEPOSITO")).ToList
            'ValorParametroPatrimonioTecnicoFirma = ListaParametros.Where(Function(p) p.Parametro.Equals("PATRIMONIOTECNICO")).ToList

            objProxy.Verificaparametro("IMPLIQ_VALIDARDEPOSITO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "IMPLIQ_VALIDARDEPOSITO")
            objProxy.Verificaparametro("PATRIMONIOTECNICO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "PATRIMONIOTECNICO")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de Tesorería", _
                                 Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    ''' <remarks>SLB20130204</remarks>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState
                Case "IMPLIQ_VALIDARDEPOSITO"
                    strValidarDeposito = lo.Value.ToString
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

    Public Function validarTipoOferta() As Boolean
        Try

            validarTipoOferta = True

            ''Validamos el tipo de la ofera
            If ImportacionLiSelected.TipoDeOferta.Trim = "1" And ListaVerificarOrdenLiq.Item(0).strObjeto.Trim <> "1" Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de oferta de la orden es diferente al de la liquidacion, por favor verifique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validarTipoOferta = False
                IsBusy = False

            ElseIf ImportacionLiSelected.TipoDeOferta.Trim = "2" And ListaVerificarOrdenLiq.Item(0).strObjeto.ToString.Trim <> "2" Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de oferta de la orden es diferente al de la liquidacion, por favor verifique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validarTipoOferta = False
                IsBusy = False

            ElseIf (ImportacionLiSelected.TipoDeOferta.Trim = "A" Or ImportacionLiSelected.TipoDeOferta = "R") And ListaVerificarOrdenLiq.Item(0).strObjeto.Trim <> "RP" Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de oferta de la orden es diferente al de la liquidacion, por favor verifique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validarTipoOferta = False
                IsBusy = False

            ElseIf ImportacionLiSelected.TipoDeOferta.Trim = "N" And (ListaVerificarOrdenLiq.Item(0).strObjeto.Trim = "1" Or ListaVerificarOrdenLiq.Item(0).strObjeto.Trim = "2") Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de oferta de la orden es diferente al de la liquidacion, por favor verifique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validarTipoOferta = False
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación del tipo de oferta", _
               Me.ToString(), "validarTipoOferta", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
    End Function

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





