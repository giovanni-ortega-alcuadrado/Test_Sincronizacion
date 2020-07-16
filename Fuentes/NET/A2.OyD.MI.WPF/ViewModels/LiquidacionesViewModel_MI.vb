Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: LiquidacionesViewModel.vb
'Generado el : 05/30/2011 09:18:58
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
Imports A2.OyD.OYDServer.RIA.Web.OyDMILA
Imports A2Utilidades.Mensajes

Public Class LiquidacionesViewModel_MI
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaLiquidacione
    Private LiquidacionePorDefecto As Liquidacione_MI
    Private LiquidacioneAnterior As Liquidacione_MI
    Private ReceptoresOrdenesAnterior As ReceptoresOrdene_MI
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Dim objProxy1 As UtilidadesDomainContext
    Private dcProxy As MILADomainContext
    Dim dcProxy1 As MILADomainContext
    Private objProxy As MILADomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim strTipoAplazamiento As String = ""
    Dim Userstate As String = ""
    Dim aplazamientoserie As Aplazamiento_En_Serie_MI
    Const strtitulo As String = "Titulo"
    Const strefectivo As String = "Efectivo"
    Dim strAplazamiento As String = ""
    Dim strerror As String
    Dim strnroaplazamiento As Integer
    Dim selected As Boolean
    Dim fechaCierre As DateTime
    Dim listaliquidacionesvalidar As New List(Of LiquidacionesConsultar_MI)
    Dim mlogRepo As Boolean
    Dim consultarcantidades As New consultarcantidad_MI
    Dim mcursaldoOrden As Double
    Dim LiquidacionesOrden As New LiquidacionesOrdenes_MI
    Const BOLSACOLOMBIA = 4
    Dim numeroliq As Integer
    Dim duplica, validaactualizarespecie As Boolean
    Public estadoregistro, mlogCantidad As Boolean
    Dim mdblCantidad, CANTIDADLIQ As Double
    Dim validafiltro, disparafocus, disparaguardar, esnuevo, ESTADO_EDICION As Boolean
    Private _mlogDuplicando As Boolean = False
    Private mlogRecalculaValoresAlDuplicar As Boolean = False


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MILADomainContext()
            dcProxy1 = New MILADomainContext()
            objProxy = New MILADomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            mdcProxyUtilidad02 = New UtilidadesDomainContext()
            objProxy1 = New UtilidadesDomainContext()
        Else
            dcProxy = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxy1 = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.LiquidacionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerLiquidacionePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesPorDefecto_Completed, "Default")
                objProxy1.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  LiquidacionesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#Region "Resultados Asincrónicos"
#Region "Tablas Padres"
    Private Sub TerminoTraerLiquidacionesPorDefecto_Completed(ByVal lo As LoadOperation(Of Liquidacione_MI))
        If Not lo.HasError Then
            LiquidacionePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidacione por defecto", _
                                             Me.ToString(), "TerminoTraerLiquidacionePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerLiquidaciones(ByVal lo As LoadOperation(Of Liquidacione_MI))
        If Not lo.HasError Then
            ListaLiquidaciones = dcProxy.Liquidacione_MIs
            If dcProxy.Liquidacione_MIs.Count > 0 Then
                If lo.UserState = "insert" Then
                    'LiquidacioneSelected = LiquidacioneAnterior JABG20160318
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones", _
                                             Me.ToString(), "TerminoTraerLiquidacione", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub TerminotraerliquidacinesValidar(ByVal lo As LoadOperation(Of LiquidacionesConsultar_MI))
        If Not lo.HasError Then
            listaliquidacionesvalidar = dcProxy.LiquidacionesConsultar_MIs.ToList

            If listaliquidacionesvalidar.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No existe la orden con esas caracteristicas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                LiquidacioneSelected.IDOrden = Nothing

                Exit Sub
            End If

            If listaliquidacionesvalidar.First.Estado.Equals("A") Then
                A2Utilidades.Mensajes.mostrarMensaje("La orden está cancelada. No es posible ingresar la liquidación  ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                LiquidacioneSelected.IDOrden = Nothing
                Exit Sub
            ElseIf listaliquidacionesvalidar.First.Estado.Equals("C") Then
                A2Utilidades.Mensajes.mostrarMensaje("La orden está cumplida. No es posible ingresar la liquidación  ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If Not duplica Then
                    LiquidacioneSelected.IDOrden = Nothing
                Else
                    duplica = False
                End If
                Exit Sub
            End If
            habilitaradio = False
            selectedindex = 0
            'si no esta duplicando realiza esta accion 
            If Not duplica Then
                mlogRepo = listaliquidacionesvalidar.First.Repo
                clientes.Comitente = listaliquidacionesvalidar.First.IDComitente.Trim
                clientes.NombreCliente = listaliquidacionesvalidar.First.Comitente
                clientesOrdenantes.Ordenante = listaliquidacionesvalidar.First.IDOrdenante.Trim
                clientesOrdenantes.NombreClienteOrdenante = listaliquidacionesvalidar.First.Ordenante
                Especie.IDEspecie = listaliquidacionesvalidar.First.IDEspecie
                Especie.NombreEspecie = listaliquidacionesvalidar.First.Especie
                LiquidacioneSelected.IDComitente = listaliquidacionesvalidar.First.IDComitente
                LiquidacioneSelected.IDOrdenante = listaliquidacionesvalidar.First.IDOrdenante
                LiquidacioneSelected.IDEspecie = listaliquidacionesvalidar.First.IDEspecie
                LiquidacioneSelected.UBICACIONTITULO = listaliquidacionesvalidar.First.UBICACIONTITULO
                LiquidacioneSelected.FactorComisionPactada = Format(listaliquidacionesvalidar.First.ComisionPactada / 100, "#0.0#####")
                LiquidacioneSelected.Emision = listaliquidacionesvalidar.First.Emision
                LiquidacioneSelected.Vencimiento = listaliquidacionesvalidar.First.Vencimiento
                LiquidacioneSelected.Modalidad = listaliquidacionesvalidar.First.Modalidad
                LiquidacioneSelected.TasaDescuento = listaliquidacionesvalidar.First.TasaInicial
                LiquidacioneSelected.IDMoneda = listaliquidacionesvalidar.First.IDMoneda
                LiquidacioneSelected.IDBolsa = listaliquidacionesvalidar.First.IDBolsa
                LiquidacioneSelected.TasaEfectiva = IIf(Not IsNothing(listaliquidacionesvalidar.First.TasaNominal), listaliquidacionesvalidar.First.TasaNominal, 0.0)
                If listaliquidacionesvalidar.First.Tipo.Equals("C") Or listaliquidacionesvalidar.First.Tipo.Equals("R") Then
                    LiquidacioneSelected.Precio = IIf(Not IsNothing(listaliquidacionesvalidar.First.Inferior), listaliquidacionesvalidar.First.Inferior, 0.0)
                Else
                    LiquidacioneSelected.Precio = IIf(Not IsNothing(listaliquidacionesvalidar.First.Superior), listaliquidacionesvalidar.First.Superior, 0.0)

                End If
                LiquidacioneSelected.Swap = "N"
                LiquidacioneSelected.Reinversion = "N"
                LiquidacioneSelected.AutoRetenedor = "N"
                LiquidacioneSelected.Certificacion = "N"
                LiquidacioneSelected.ConstanciaEnajenacion = "N"
                LiquidacioneSelected.RepoTitulo = "N"
                LiquidacioneSelected.Sujeto = "N"
                LiquidacioneSelected.IDBaseDias = "N"
            End If
            SaldoOrden()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones", _
                                             Me.ToString(), "TerminotraerliquidacinesValidar", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub TerminartraerCantidad(ByVal lo As LoadOperation(Of consultarcantidad_MI))
        If Not lo.HasError Then
            'si no esta duplicando realiza esta accion
            consultarcantidades = dcProxy.consultarcantidad_MIs.First
            If Not duplica Then
                'consultarcantidades = dcProxy.consultarcantidad_MIs.First
                'cuando es edicion es esta misma formula restandole liquidacionesselected.cantidad
                If ESTADO_EDICION Then
                    mcursaldoOrden = consultarcantidades.CantidadOrden - (consultarcantidades.CantidadLiq - LiquidacioneSelected.Cantidad)
                    ESTADO_EDICION = False
                    Exit Sub
                Else
                    LiquidacioneSelected.Cantidad = consultarcantidades.CantidadOrden - consultarcantidades.CantidadLiq
                    mcursaldoOrden = LiquidacioneSelected.Cantidad
                End If


                If LiquidacioneSelected.Precio = 0 Then
                    LiquidacioneSelected.Transaccion_cur = listaliquidacionesvalidar.First.Cantidad
                    LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.Transaccion_cur
                    LiquidacioneSelected.TotalLiq = LiquidacioneSelected.Transaccion_cur
                Else
                    If LiquidacioneSelected.ClaseOrden.Equals("A") Then
                        LiquidacioneSelected.Transaccion_cur = LiquidacioneSelected.Precio * LiquidacioneSelected.Cantidad
                    Else
                        LiquidacioneSelected.Transaccion_cur = LiquidacioneSelected.Precio * LiquidacioneSelected.Cantidad / 100
                    End If
                    LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.Transaccion_cur
                    LiquidacioneSelected.TotalLiq = LiquidacioneSelected.Transaccion_cur

                End If

                If Not IsNothing(listaliquidacionesvalidar.First.ComisionPactada) Then
                    If Not IsNothing(LiquidacioneSelected.Transaccion_cur) Then
                        LiquidacioneSelected.Comision = LiquidacioneSelected.Transaccion_cur * LiquidacioneSelected.FactorComisionPactada
                        If listaliquidacionesvalidar.First.Tipo.Equals("C") Or listaliquidacionesvalidar.First.Tipo.Equals("R") Then
                            LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.SubTotalLiq + LiquidacioneSelected.Comision
                            LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq + LiquidacioneSelected.Comision
                        Else
                            LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.SubTotalLiq - LiquidacioneSelected.Comision
                            LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq - LiquidacioneSelected.Comision
                        End If
                    End If
                Else
                    LiquidacioneSelected.Comision = 0
                End If
                If Not IsNothing(listaliquidacionesvalidar.First.Objeto) Then
                    If Not listaliquidacionesvalidar.First.Ordinaria And listaliquidacionesvalidar.First.Objeto.Equals("CRR") Then
                        'HABILITAR ESTAS DOS  PROPIEDADES
                        habilitaparciald = True
                    End If
                End If
            End If
            'configurar un nuevo receptor
            NuevoReceptorliq()
            habilitamanualsi = True
            If _mlogDuplicando Then
                If LiquidacioneSelected.Plaza = "LOC" Then
                    HabilitarComisionistaLocal = True
                    HabilitarComisionistaOtraPlaza = False
                Else
                    HabilitarComisionistaLocal = False
                    HabilitarComisionistaOtraPlaza = True
                End If
                _mlogDuplicando = False
            Else
                HabilitarComisionistaLocal = False
                HabilitarComisionistaOtraPlaza = False
            End If
            HabilitarFechaLiquidacion = True
            HabilitarFechaVendido = False
            HabilitarFechaTitulo = True
            HabilitarFechaEfectivo = True

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidacione ", _
                                             Me.ToString(), "TerminartraerCantidad", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub Terminotraerreceptoresliq(ByVal lo As LoadOperation(Of ReceptoresOrdene_MI))
        If Not lo.HasError Then
            ListaReceptoresOrdenes = dcProxy.ReceptoresOrdene_MIs
            'ReceptoresOrdenesSelected = ListaReceptoresOrdenes.First
            'Dim NewReceptoresOrdenes As New ReceptoresOrdene
            'NewReceptoresOrdenes.TipoOrden = LiquidacioneSelected.Tipo
            'NewReceptoresOrdenes.ClaseOrden = LiquidacioneSelected.ClaseOrden
            'NewReceptoresOrdenes.IDOrden = LiquidacioneSelected.IDOrden
            'NewReceptoresOrdenes.Version = 0
            'NewReceptoresOrdenes.IDReceptor = receptoresordliq.IDReceptor
            'NewReceptoresOrdenes.Lider = receptoresordliq.Lider
            'NewReceptoresOrdenes.Porcentaje = receptoresordliq.Porcentaje
            'NewReceptoresOrdenes.Nombre = receptoresordliq.Nombre
            'NewReceptoresOrdenes.Usuario = Program.Usuario

            'ListaReceptoresOrdenes.Add(NewReceptoresOrdenes)
            'ReceptoresOrdenesSelected = NewReceptoresOrdenes
            'MyBase.CambioItem("ReceptoresOrdenesSelected")
            'MyBase.CambioItem("ListaReceptoresOrdenes")
            'se configura nuevo beneficiario JBT
            BeneficiariosOrdenes()

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Liquidacione ", _
                                             Me.ToString(), "Terminotraerreceptoresliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerBeneficiariosliq(ByVal lo As LoadOperation(Of BeneficiariosOrdene_MI))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenes = dcProxy.BeneficiariosOrdene_MIs
            'se configura nueva especie JBT
            Especieliq()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes", _
                                             Me.ToString(), "TerminoTraerBeneficiariosliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerEspeciesliq(ByVal lo As LoadOperation(Of EspeciesLiquidacione_MI))
        If Not lo.HasError Then
            ListaEspeciesLiquidaciones = dcProxy.EspeciesLiquidacione_MIs
            'si esta duplicando
            If duplica Then
                NuevoDuplicado()
                duplica = False
            Else
                llevarvalores()
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidaciones", _
                                             Me.ToString(), "TerminoTraerEspeciesliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminotraerOrdenesliq(ByVal lo As LoadOperation(Of LiquidacionesOrdenes_MI))
        If Not lo.HasError Then
            Try
                'trae la comision pactada,strObjeto de la orden de mila
                LiquidacionesOrden = dcProxy.LiquidacionesOrdenes_MIs.First
                If IsNothing(LiquidacionesOrden.Ordinaria) Then
                    LiquidacioneSelected.Ordinaria = False
                Else
                    LiquidacioneSelected.Ordinaria = LiquidacionesOrden.Ordinaria
                End If

                If IsNothing(LiquidacionesOrden.Objeto) Then
                    LiquidacioneSelected.ObjetoOrdenExtraordinaria = Nothing
                Else
                    LiquidacioneSelected.ObjetoOrdenExtraordinaria = LiquidacionesOrden.Objeto
                End If



                If IsNothing(LiquidacionesOrden.Objeto) Then
                    LiquidacioneSelected.NumPadre = Nothing
                Else
                    If LiquidacionesOrden.Objeto.Equals("CRR") Then
                        LiquidacioneSelected.NumPadre = LiquidacioneSelected.ID
                    Else
                        LiquidacioneSelected.NumPadre = Nothing
                    End If
                End If
                HabilitarComisionistaLocal = False
                HabilitarComisionistaOtraPlaza = False
                Guardarliq()
                HabilitarComisionistaOtraPlaza = False
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidaciones", _
                                             Me.ToString(), "TerminoTraerEspeciesliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            fechaCierre = obj.Value
        End If
    End Sub
    Private Sub Terminotraervalidar(ByVal lo As InvokeOperation(Of Integer))
        If Not lo.HasError Then
            numeroliq = lo.Value
            If numeroliq > 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La liquidación " + LiquidacioneSelected.ID.ToString + " con el parcial " + LiquidacioneSelected.Parcial.ToString + " y fecha " + LiquidacioneSelected.Liquidacion.ToShortDateString + " ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminotraervalidar", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerAplazamientosLiquidacionesvalor(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.Transaccion_cur = LiquidacioneSelected.Precio * LiquidacioneSelected.Cantidad * lo.Value
            LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.Transaccion_cur
            LiquidacioneSelected.TotalLiq = LiquidacioneSelected.Transaccion_cur
            Denominacionespecie()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraernombre(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If lo.Value.Equals("PESOS") Then
                LiquidacioneSelected.Comision = IIf(IsNothing(LiquidacioneSelected.Cantidad), 0, LiquidacioneSelected.Cantidad) * IIf(IsNothing(LiquidacioneSelected.FactorComisionPactada), 0, LiquidacioneSelected.FactorComisionPactada)
            Else
                LiquidacioneSelected.Comision = IIf(IsNothing(LiquidacioneSelected.Transaccion_cur), 0, LiquidacioneSelected.Transaccion_cur) * IIf(IsNothing(LiquidacioneSelected.FactorComisionPactada), 0, LiquidacioneSelected.FactorComisionPactada)
            End If
            dcProxy.verificadblIvacomision(Nothing, Program.Usuario, Program.HashConexion, AddressOf Terminotraeriva, Nothing)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminoTraernombre", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminotraeriva(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            If Userstate <> String.Empty Then 'JBT 20130125  cambio realizado para evitar error cuando se daba doble clik en el boton grabar
                Exit Sub
            End If
            If LiquidacioneSelected.Comision > 0 And lo.Value > 0 Then
                LiquidacioneSelected.ValorIva = ((LiquidacioneSelected.Comision) * (lo.Value / 100) - 0.005)
            Else
                LiquidacioneSelected.ValorIva = 0
            End If
            If LiquidacioneSelected.Tipo.Equals("C") Or LiquidacioneSelected.Tipo.Equals("R") Then
                LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.SubTotalLiq + LiquidacioneSelected.Comision + LiquidacioneSelected.ValorIva
                LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq + LiquidacioneSelected.Comision + LiquidacioneSelected.ValorIva
            Else
                LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.SubTotalLiq - LiquidacioneSelected.Comision - LiquidacioneSelected.ValorIva
                LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq - LiquidacioneSelected.Comision - LiquidacioneSelected.ValorIva
            End If
            If Not IsNothing(LiquidacioneSelected.ServBolsaFijo) And trasladar.Traslado Then
                If LiquidacioneSelected.Tipo.Equals("C") Or LiquidacioneSelected.Tipo.Equals("R") Then
                    LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq + LiquidacioneSelected.ServBolsaFijo
                Else
                    LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq - LiquidacioneSelected.ServBolsaFijo
                End If
            End If

            If Not IsNothing(LiquidacioneSelected.Retencion) Then
                LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq + LiquidacioneSelected.Retencion

            End If

            If Not IsNothing(LiquidacioneSelected.Intereses) Then
                LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq + LiquidacioneSelected.Intereses

            End If

            If Not IsNothing(LiquidacioneSelected.ValorTraslado) Then
                LiquidacioneSelected.TotalLiq = LiquidacioneSelected.TotalLiq + LiquidacioneSelected.ValorTraslado

            End If
            CalcularCostosMonedaOrigen()

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminoTraerAplazamientosLiquidacionesvalor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminoactualizarordenestado(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If Not lo.Value = String.Empty Then
                Dim parametrosliq = Split(lo.UserState, ",")
                dcProxy.Actualizaordenestadocumplida(parametrosliq(0), parametrosliq(1), CInt(parametrosliq(2)), 0, "P", Now, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarodencumplida, Nothing)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminoactualizarordenestado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminoactualizarodencumplida(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminoactualizarodencumplida", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerCostosMonedaOrigen(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ValorCostos = lo.Value
            CalculosValorNeto()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminoTraerCostosMonedaOrigen", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub TerminotraervalorNeto(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ValorNeto = lo.Value
            CalculaValorBruto()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminotraervalorNeto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub Terminotraervalorbruto(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ValorBruto = lo.Value
            Calculacomisionpesos()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminotraervalorbruto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub Terminotraercomisionpesos(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ComisionPesos = lo.Value
            CalculaIvacomisionpesos()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminotraercomisionpesos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub TerminotraerIvacomisionpesos(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ValorIVAComisionPesos = lo.Value
            Calculacostospesos()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "TerminotraerIvacomisionpesos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminotraercostospesos(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ValorCostosPesos = lo.Value
            Calculavalornetopesos()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminotraercostospesos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminotraervalorcostosnetopesos(ByVal lo As InvokeOperation(Of System.Nullable(Of Double)))
        If Not lo.HasError Then
            LiquidacioneSelected.ValorNetoPesos = lo.Value
            'SLB 20130111
            If disparaguardar = True And disparafocus = True Then
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")
                Userstate = "insert"
            End If
            disparafocus = False
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones", _
                                             Me.ToString(), "Terminotraervalorcostosnetopesos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub Terminoactualizarespeciesordenes(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualizacino de la especie", _
                                             Me.ToString(), "Terminoactualizarespeciesordenes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub TerminoVerificarCumplimientoOrderliq(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If Userstate <> String.Empty Then 'JBT 20130125  cambio realizado para evitar error cuando se daba doble clik en el boton grabar
                Exit Sub
            End If
            Dim pdblCantidadLiq = Split(lo.Value, ",")
            If Not ListaLiquidaciones.Contains(LiquidacioneSelected) Then

                If Not IsNothing(pdblCantidadLiq(0)) Then
                    CANTIDADLIQ = (pdblCantidadLiq(0) + LiquidacioneSelected.Cantidad)
                Else
                    CANTIDADLIQ = LiquidacioneSelected.Cantidad
                End If
                If Not ListaLiquidaciones.Contains(LiquidacioneSelected) Or mlogCantidad Then
                    If CANTIDADLIQ = pdblCantidadLiq(1) Then
                        dcProxy.Actualizaordenestadocumplida(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, 0, "C", LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarodencumplida, Nothing)
                    End If
                End If
                validaactualizarespecie = True
                ListaLiquidaciones.Add(LiquidacioneSelected)
            Else
                If mlogCantidad Then
                    If Not IsNothing(pdblCantidadLiq(0)) Then
                        CANTIDADLIQ = (pdblCantidadLiq(0) - mdblCantidad + LiquidacioneSelected.Cantidad)
                    Else
                        CANTIDADLIQ = LiquidacioneSelected.Cantidad
                    End If
                End If
            End If

            If mlogCantidad And ((CANTIDADLIQ < pdblCantidadLiq(1)) Or (CANTIDADLIQ > pdblCantidadLiq(1))) Then
                dcProxy.Actualizaordenestadocumplida(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, 0, "P", LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarodencumplida, Nothing)
            End If

            If Not disparafocus Then
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")
                Userstate = "insert"
            End If
            disparaguardar = True

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del Saldo de la Orden", _
                     Me.ToString(), "TerminoVerificarCumplimientoOrderliq", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub





#End Region
#Region "Tablas Hijas"
    Private Sub TerminoTraerReceptoresOrdenes(ByVal lo As LoadOperation(Of ReceptoresOrdene_MI))
        If Not lo.HasError Then
            ListaReceptoresOrdenes = dcProxy.ReceptoresOrdene_MIs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresOrdenes", _
                                             Me.ToString(), "TerminoTraerReceptoresOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerBeneficiariosOrdenes(ByVal lo As LoadOperation(Of BeneficiariosOrdene_MI))
        If Not lo.HasError Then
            ListaBeneficiariosOrdenes = dcProxy.BeneficiariosOrdene_MIs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BeneficiariosOrdenes", _
                                             Me.ToString(), "TerminoTraerBeneficiariosOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerEspeciesLiquidaciones(ByVal lo As LoadOperation(Of EspeciesLiquidacione_MI))
        If Not lo.HasError Then
            ListaEspeciesLiquidaciones = dcProxy.EspeciesLiquidacione_MIs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesLiquidaciones", _
                                             Me.ToString(), "TerminoTraerEspeciesLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerAplazamientosLiquidaciones(ByVal lo As LoadOperation(Of AplazamientosLiquidacione_MI))
        If Not lo.HasError Then
            ListaAplazamientosLiquidaciones = dcProxy.AplazamientosLiquidacione_MIs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de AplazamientosLiquidaciones", _
                                             Me.ToString(), "TerminoTraerAplazamientosLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Private Sub TerminoTraerCustodiasLiquidaciones(ByVal lo As LoadOperation(Of CustodiasLiquidacione_MI))
        If Not lo.HasError Then
            ListaCustodiasLiquidaciones = dcProxy.CustodiasLiquidacione_MIs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CustodiasLiquidaciones", _
                                             Me.ToString(), "TerminoTraerCustodiasLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
#End Region
#End Region
#Region "Propiedades"
    Private _ListaLiquidaciones As EntitySet(Of Liquidacione_MI)
    Public Property ListaLiquidaciones() As EntitySet(Of Liquidacione_MI)
        Get
            Return _ListaLiquidaciones
        End Get
        Set(ByVal value As EntitySet(Of Liquidacione_MI))
            _ListaLiquidaciones = value
            MyBase.CambioItem("ListaLiquidaciones")
            MyBase.CambioItem("ListaLiquidacionesPaged")

            If Not IsNothing(value) Then
                If _ListaLiquidaciones.Count > 0 Then
                    If IsNothing(LiquidacioneSelected) Then
                        LiquidacioneSelected = _ListaLiquidaciones.FirstOrDefault
                    End If
                Else
                    LiquidacioneSelected = Nothing
                End If
            Else
                LiquidacioneSelected = Nothing
            End If
        End Set
    End Property
    Public ReadOnly Property ListaLiquidacionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidaciones) Then
                Dim view = New PagedCollectionView(_ListaLiquidaciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private WithEvents _LiquidacioneSelected As Liquidacione_MI
    Public Property LiquidacioneSelected() As Liquidacione_MI
        Get
            Return _LiquidacioneSelected
        End Get
        Set(ByVal value As Liquidacione_MI)

            _LiquidacioneSelected = value
            If selected = False Then
                If Not IsNothing(LiquidacioneSelected) Then
                    If Not IsNothing(LiquidacioneSelected.Traslado) Then
                        If LiquidacioneSelected.Traslado.Equals("S") Then
                            'LiquidacioneSelected.Traslado = "1"
                            trasladar.Traslado = 1
                        Else
                            'LiquidacioneSelected.Traslado = "0"
                            trasladar.Traslado = 0
                        End If
                    End If
                End If
                If Not value Is Nothing Then
                    If Not IsNothing(LiquidacioneSelected.IDOrden) And LiquidacioneSelected.IDOrden <> 0 Then
                        VeriOrdenes.NumeroOrden = CInt(Mid$(CStr(LiquidacioneSelected.IDOrden), 1, 4))
                        VeriOrdenes.NumeroOrden1 = Mid$(CStr(LiquidacioneSelected.IDOrden), 5)
                    End If

                    numeroliq = 0
                    clientes.Comitente = value.IDComitente.Trim
                    clientesOrdenantes.Ordenante = value.IDOrdenante.Trim
                    Especie.IDEspecie = value.IDEspecie
                    buscarItem("clientes")
                    buscarItem("clientesOrdenantes")
                    buscarItem("especies")

                    dcProxy.ReceptoresOrdene_MIs.Clear()
                    dcProxy.BeneficiariosOrdene_MIs.Clear()
                    dcProxy.EspeciesLiquidacione_MIs.Clear()
                    dcProxy.AplazamientosLiquidacione_MIs.Clear()
                    dcProxy.CustodiasLiquidacione_MIs.Clear()

                    dcProxy.Load(dcProxy.Traer_ReceptoresOrdenes_LiquidacionesQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresOrdenes, "FiltroReceptoresOrdenes")
                    dcProxy.Load(dcProxy.Traer_BeneficiariosOrdenes_LiquidacionesQuery(LiquidacioneSelected.IDLiquidaciones, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosOrdenes, "FiltroBeneficiariosOrdenes")
                    dcProxy.Load(dcProxy.Traer_EspeciesLiquidacionesQuery(LiquidacioneSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesLiquidaciones, "FiltroEspeciesLiquidaciones")
                    dcProxy.Load(dcProxy.Traer_AplazamientosLiquidacionesQuery(LiquidacioneSelected.Liquidacion, LiquidacioneSelected.ID, LiquidacioneSelected.Parcial, LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAplazamientosLiquidaciones, "FiltroAplazamientosLiquidaciones")
                    dcProxy.Load(dcProxy.Traer_CustodiasLiquidacionesQuery(LiquidacioneSelected.IDComisionista,
                                                                           LiquidacioneSelected.IDSucComisionista,
                                                                           LiquidacioneSelected.IDComitente,
                                                                           LiquidacioneSelected.IDEspecie,
                                                                           LiquidacioneSelected.Tipo,
                                                                           LiquidacioneSelected.ClaseOrden,
                                                                           LiquidacioneSelected.ID,
                                                                           LiquidacioneSelected.Parcial,
                                                                           LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodiasLiquidaciones, "FiltroCustodiasLiquidaciones")
                End If

            End If
            selected = False
            MyBase.CambioItem("LiquidacioneSelected")
        End Set
    End Property
    Private _TabSeleccionada As Integer = 0
    Public Property TabSeleccionada
        Get
            Return _TabSeleccionada
        End Get
        Set(ByVal value)
            _TabSeleccionada = value
            MyBase.CambioItem("TabSeleccionada")

        End Set
    End Property
    Private _clientes As Clientesclase = New Clientesclase
    Public Property clientes() As Clientesclase
        Get
            Return _clientes
        End Get
        Set(ByVal value As Clientesclase)
            _clientes = value
            MyBase.CambioItem("clientes")
        End Set
    End Property
    Private _clientesOrdenantes As ClientesOrdenantesclase = New ClientesOrdenantesclase
    Public Property clientesOrdenantes() As ClientesOrdenantesclase
        Get
            Return _clientesOrdenantes
        End Get
        Set(ByVal value As ClientesOrdenantesclase)
            _clientesOrdenantes = value
            MyBase.CambioItem("_clientesOrdenantes")
        End Set
    End Property
    Private _Especie As Especieclase = New Especieclase
    Public Property Especie() As Especieclase
        Get
            Return _Especie
        End Get
        Set(ByVal value As Especieclase)
            _Especie = value
            MyBase.CambioItem("Especie")
        End Set
    End Property
    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property
    Private _trasladar As traslado = New traslado
    Public Property trasladar As traslado
        Get
            Return _trasladar
        End Get
        Set(ByVal value As traslado)
            _trasladar = value
            MyBase.CambioItem("trasladar")
        End Set
    End Property

    Private _habilitaboton As Boolean = True
    Public Property habilitaboton As Boolean
        Get
            Return _habilitaboton
        End Get
        Set(ByVal value As Boolean)
            _habilitaboton = value
            MyBase.CambioItem("habilitaboton")
        End Set
    End Property

    Private _habilitamanualsi As Boolean
    Public Property habilitamanualsi As Boolean
        Get
            Return _habilitamanualsi
        End Get
        Set(ByVal value As Boolean)
            _habilitamanualsi = value
            MyBase.CambioItem("habilitamanualsi")
        End Set
    End Property

    Private _habilitamanualno As Boolean
    Public Property habilitamanualno As Boolean
        Get
            Return _habilitamanualno
        End Get
        Set(ByVal value As Boolean)
            _habilitamanualno = value
            MyBase.CambioItem("habilitamanualno")
        End Set
    End Property
    Private _habilitaradio As Boolean
    Public Property habilitaradio As Boolean
        Get
            Return _habilitaradio
        End Get
        Set(ByVal value As Boolean)
            _habilitaradio = value
            MyBase.CambioItem("habilitaradio")
        End Set
    End Property

    Private _habilitaparciald As Boolean
    Public Property habilitaparciald As Boolean
        Get
            Return _habilitaparciald
        End Get
        Set(ByVal value As Boolean)
            _habilitaparciald = value
            MyBase.CambioItem("habilitaparciald")
        End Set
    End Property

    Private _habilitamanualdias As Boolean
    Public Property habilitamanualdias As Boolean
        Get
            Return _habilitamanualdias
        End Get
        Set(ByVal value As Boolean)
            _habilitamanualdias = value
            MyBase.CambioItem("habilitamanualdias")
        End Set
    End Property

    Private _HabilitarComisionistaLocal As Boolean = False
    Public Property HabilitarComisionistaLocal() As Boolean
        Get
            Return _HabilitarComisionistaLocal
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComisionistaLocal = value
            MyBase.CambioItem("HabilitarComisionistaLocal")
        End Set
    End Property

    Private _HabilitarComisionistaOtraPlaza As Boolean = False
    Public Property HabilitarComisionistaOtraPlaza As Boolean
        Get
            Return _HabilitarComisionistaOtraPlaza
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComisionistaOtraPlaza = value
            MyBase.CambioItem("HabilitarComisionistaOtraPlaza")
        End Set
    End Property

    Private _HabilitarFechaLiquidacion As Boolean = False
    Public Property HabilitarFechaLiquidacion As Boolean
        Get
            Return _HabilitarFechaLiquidacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaLiquidacion = value
            MyBase.CambioItem("HabilitarFechaLiquidacion")
        End Set
    End Property

    Private _HabilitarFechaVendido As Boolean = False
    Public Property HabilitarFechaVendido As Boolean
        Get
            Return _HabilitarFechaVendido
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaVendido = value
            MyBase.CambioItem("HabilitarFechaVendido")
        End Set
    End Property

    Private _HabilitarFechaTitulo As Boolean = False
    Public Property HabilitarFechaTitulo As Boolean
        Get
            Return _HabilitarFechaTitulo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaTitulo = value
            MyBase.CambioItem("HabilitarFechaTitulo")
        End Set
    End Property

    Private _HabilitarFechaEfectivo As Boolean = False
    Public Property HabilitarFechaEfectivo As Boolean
        Get
            Return _HabilitarFechaEfectivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaEfectivo = value
            MyBase.CambioItem("HabilitarFechaEfectivo")
        End Set
    End Property

    Private WithEvents _VeriOrdenes As New VeriOrden
    Public Property VeriOrdenes As VeriOrden
        Get
            Return _VeriOrdenes
        End Get
        Set(ByVal value As VeriOrden)
            _VeriOrdenes = value
            MyBase.CambioItem("VeriOrdenes")
        End Set
    End Property
    Private _VmLiquidaciones As LiquidacionesView_MI
    Public Property VmLiquidaciones As LiquidacionesView_MI
        Get
            Return _VmLiquidaciones
        End Get
        Set(ByVal value As LiquidacionesView_MI)
            _VmLiquidaciones = value
            MyBase.CambioItem("VmLiquidaciones")
        End Set
    End Property
    Private _selectedindex As Integer
    Public Property selectedindex As Integer
        Get
            Return _selectedindex
        End Get
        Set(ByVal value As Integer)
            _selectedindex = value
            MyBase.CambioItem("selectedindex")
        End Set
    End Property
#End Region
#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            estadoregistro = True

            Dim NewLiquidacione As New Liquidacione_MI
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewLiquidacione.IDComisionista = LiquidacionePorDefecto.IDComisionista
            NewLiquidacione.IDSucComisionista = LiquidacionePorDefecto.IDSucComisionista
            NewLiquidacione.ID = LiquidacionePorDefecto.ID
            NewLiquidacione.Parcial = Nothing
            NewLiquidacione.Tipo = "C"
            NewLiquidacione.ClaseOrden = "A"
            NewLiquidacione.IDEspecie = LiquidacionePorDefecto.IDEspecie
            NewLiquidacione.IDOrden = LiquidacionePorDefecto.IDOrden

            'SLB20131120 Para el manejo de la facturación
            'NewLiquidacione.Prefijo = LiquidacionePorDefecto.Prefijo
            'NewLiquidacione.IDFactura = LiquidacionePorDefecto.IDFactura
            'NewLiquidacione.Facturada = LiquidacionePorDefecto.Facturada
            NewLiquidacione.Prefijo = Nothing
            NewLiquidacione.IDFactura = Nothing
            NewLiquidacione.Facturada = Nothing

            NewLiquidacione.IDComitente = String.Empty
            NewLiquidacione.IDOrdenante = String.Empty
            NewLiquidacione.IDBolsa = LiquidacionePorDefecto.IDBolsa
            NewLiquidacione.ValBolsa = LiquidacionePorDefecto.ValBolsa
            NewLiquidacione.IDRueda = LiquidacionePorDefecto.IDRueda
            NewLiquidacione.TasaDescuento = LiquidacionePorDefecto.TasaDescuento
            NewLiquidacione.TasaCompraVende = LiquidacionePorDefecto.TasaCompraVende
            NewLiquidacione.Modalidad = LiquidacionePorDefecto.Modalidad
            NewLiquidacione.IndicadorEconomico = LiquidacionePorDefecto.IndicadorEconomico
            NewLiquidacione.PuntosIndicador = LiquidacionePorDefecto.PuntosIndicador
            NewLiquidacione.Plazo = LiquidacionePorDefecto.Plazo
            NewLiquidacione.Liquidacion = Now.Date
            NewLiquidacione.Cumplimiento = Now.Date
            NewLiquidacione.Emision = LiquidacionePorDefecto.Emision
            NewLiquidacione.Vencimiento = LiquidacionePorDefecto.Vencimiento
            NewLiquidacione.OtraPlaza = LiquidacionePorDefecto.OtraPlaza
            NewLiquidacione.Plaza = LiquidacionePorDefecto.Plaza
            NewLiquidacione.IDComisionistaLocal = LiquidacionePorDefecto.IDComisionistaLocal
            NewLiquidacione.IDComisionistaOtraPlaza = LiquidacionePorDefecto.IDComisionistaOtraPlaza
            NewLiquidacione.IDCiudadOtraPlaza = LiquidacionePorDefecto.IDCiudadOtraPlaza
            NewLiquidacione.TasaEfectiva = LiquidacionePorDefecto.TasaEfectiva
            NewLiquidacione.Cantidad = LiquidacionePorDefecto.Cantidad
            NewLiquidacione.Precio = LiquidacionePorDefecto.Precio
            NewLiquidacione.Transaccion = LiquidacionePorDefecto.Transaccion
            NewLiquidacione.SubTotalLiq = LiquidacionePorDefecto.SubTotalLiq
            NewLiquidacione.TotalLiq = LiquidacionePorDefecto.TotalLiq
            NewLiquidacione.Comision = LiquidacionePorDefecto.Comision
            NewLiquidacione.Retencion = LiquidacionePorDefecto.Retencion
            NewLiquidacione.Intereses = LiquidacionePorDefecto.Intereses
            NewLiquidacione.ValorIva = LiquidacionePorDefecto.ValorIva
            NewLiquidacione.DiasIntereses = LiquidacionePorDefecto.DiasIntereses
            NewLiquidacione.FactorComisionPactada = LiquidacionePorDefecto.FactorComisionPactada
            NewLiquidacione.Mercado = LiquidacionePorDefecto.Mercado
            NewLiquidacione.NroTitulo = LiquidacionePorDefecto.NroTitulo
            NewLiquidacione.IDCiudadExpTitulo = LiquidacionePorDefecto.IDCiudadExpTitulo
            NewLiquidacione.PlazoOriginal = LiquidacionePorDefecto.PlazoOriginal
            NewLiquidacione.Aplazamiento = LiquidacionePorDefecto.Aplazamiento
            NewLiquidacione.VersionPapeleta = LiquidacionePorDefecto.VersionPapeleta
            NewLiquidacione.EmisionOriginal = LiquidacionePorDefecto.EmisionOriginal
            NewLiquidacione.VencimientoOriginal = LiquidacionePorDefecto.VencimientoOriginal
            NewLiquidacione.Impresiones = LiquidacionePorDefecto.Impresiones
            NewLiquidacione.FormaPago = LiquidacionePorDefecto.FormaPago
            NewLiquidacione.CtrlImpPapeleta = LiquidacionePorDefecto.CtrlImpPapeleta
            NewLiquidacione.DiasVencimiento = LiquidacionePorDefecto.DiasVencimiento
            NewLiquidacione.PosicionPropia = LiquidacionePorDefecto.PosicionPropia
            NewLiquidacione.Transaccion = LiquidacionePorDefecto.Transaccion
            NewLiquidacione.TipoOperacion = LiquidacionePorDefecto.TipoOperacion
            NewLiquidacione.DiasContado = LiquidacionePorDefecto.DiasContado
            NewLiquidacione.Ordinaria = LiquidacionePorDefecto.Ordinaria
            NewLiquidacione.ObjetoOrdenExtraordinaria = LiquidacionePorDefecto.ObjetoOrdenExtraordinaria
            NewLiquidacione.NumPadre = LiquidacionePorDefecto.NumPadre
            NewLiquidacione.ParcialPadre = LiquidacionePorDefecto.ParcialPadre
            NewLiquidacione.OperacionPadre = LiquidacionePorDefecto.OperacionPadre
            NewLiquidacione.DiasTramo = LiquidacionePorDefecto.DiasTramo
            NewLiquidacione.Vendido = LiquidacionePorDefecto.Vendido
            NewLiquidacione.Vendido = LiquidacionePorDefecto.Vendido
            NewLiquidacione.Manual = True
            NewLiquidacione.ValorTraslado = LiquidacionePorDefecto.ValorTraslado
            NewLiquidacione.ValorBrutoCompraVencida = LiquidacionePorDefecto.ValorBrutoCompraVencida
            NewLiquidacione.AutoRetenedor = LiquidacionePorDefecto.AutoRetenedor
            NewLiquidacione.Sujeto = LiquidacionePorDefecto.Sujeto
            NewLiquidacione.PcRenEfecCompraRet = LiquidacionePorDefecto.PcRenEfecCompraRet
            NewLiquidacione.PcRenEfecVendeRet = LiquidacionePorDefecto.PcRenEfecVendeRet
            NewLiquidacione.Reinversion = LiquidacionePorDefecto.Reinversion
            NewLiquidacione.Swap = LiquidacionePorDefecto.Swap
            NewLiquidacione.NroSwap = LiquidacionePorDefecto.NroSwap
            NewLiquidacione.Certificacion = LiquidacionePorDefecto.Certificacion
            NewLiquidacione.DescuentoAcumula = LiquidacionePorDefecto.DescuentoAcumula
            NewLiquidacione.PctRendimiento = LiquidacionePorDefecto.PctRendimiento
            NewLiquidacione.FechaCompraVencido = LiquidacionePorDefecto.FechaCompraVencido
            NewLiquidacione.PrecioCompraVencido = LiquidacionePorDefecto.PrecioCompraVencido
            NewLiquidacione.ConstanciaEnajenacion = LiquidacionePorDefecto.ConstanciaEnajenacion
            NewLiquidacione.RepoTitulo = LiquidacionePorDefecto.RepoTitulo
            NewLiquidacione.ServBolsaVble = LiquidacionePorDefecto.ServBolsaVble
            NewLiquidacione.ServBolsaFijo = LiquidacionePorDefecto.ServBolsaFijo
            NewLiquidacione.Traslado = LiquidacionePorDefecto.Traslado
            NewLiquidacione.UBICACIONTITULO = LiquidacionePorDefecto.UBICACIONTITULO
            NewLiquidacione.HoraGrabacion = LiquidacionePorDefecto.HoraGrabacion
            NewLiquidacione.OrigenOperacion = LiquidacionePorDefecto.OrigenOperacion
            NewLiquidacione.CodigoOperadorCompra = LiquidacionePorDefecto.CodigoOperadorCompra
            NewLiquidacione.CodigoOperadorVende = LiquidacionePorDefecto.CodigoOperadorVende
            NewLiquidacione.IdentificacionRemate = LiquidacionePorDefecto.IdentificacionRemate
            NewLiquidacione.ModalidaOperacion = LiquidacionePorDefecto.ModalidaOperacion
            NewLiquidacione.IndicadorPrecio = LiquidacionePorDefecto.IndicadorPrecio
            NewLiquidacione.PeriodoExdividendo = LiquidacionePorDefecto.PeriodoExdividendo
            NewLiquidacione.PlazoOperacionRepo = LiquidacionePorDefecto.PlazoOperacionRepo
            NewLiquidacione.ValorCaptacionRepo = LiquidacionePorDefecto.ValorCaptacionRepo
            NewLiquidacione.VolumenCompraRepo = LiquidacionePorDefecto.VolumenCompraRepo
            NewLiquidacione.PrecioNetoFraccion = LiquidacionePorDefecto.PrecioNetoFraccion
            NewLiquidacione.VolumenNetoFraccion = LiquidacionePorDefecto.VolumenNetoFraccion
            NewLiquidacione.CodigoContactoComercial = LiquidacionePorDefecto.CodigoContactoComercial
            NewLiquidacione.NroFraccionOperacion = LiquidacionePorDefecto.NroFraccionOperacion
            NewLiquidacione.IdentificacionPatrimonio1 = LiquidacionePorDefecto.IdentificacionPatrimonio1
            NewLiquidacione.TipoidentificacionCliente2 = LiquidacionePorDefecto.TipoidentificacionCliente2
            NewLiquidacione.NitCliente2 = LiquidacionePorDefecto.NitCliente2
            NewLiquidacione.IdentificacionPatrimonio2 = LiquidacionePorDefecto.IdentificacionPatrimonio2
            NewLiquidacione.TipoIdentificacionCliente3 = LiquidacionePorDefecto.TipoIdentificacionCliente3
            NewLiquidacione.NitCliente3 = LiquidacionePorDefecto.NitCliente3
            NewLiquidacione.IdentificacionPatrimonio3 = LiquidacionePorDefecto.IdentificacionPatrimonio3
            NewLiquidacione.IndicadorOperacion = LiquidacionePorDefecto.IndicadorOperacion
            NewLiquidacione.BaseRetencion = LiquidacionePorDefecto.BaseRetencion
            NewLiquidacione.PorcRetencion = LiquidacionePorDefecto.PorcRetencion
            NewLiquidacione.BaseRetencionTranslado = LiquidacionePorDefecto.BaseRetencionTranslado
            NewLiquidacione.PorcRetencionTranslado = LiquidacionePorDefecto.PorcRetencionTranslado
            NewLiquidacione.PorcIvaComision = LiquidacionePorDefecto.PorcIvaComision
            NewLiquidacione.IndicadorAcciones = LiquidacionePorDefecto.IndicadorAcciones
            NewLiquidacione.OperacionNegociada = LiquidacionePorDefecto.OperacionNegociada
            NewLiquidacione.FechaConstancia = LiquidacionePorDefecto.FechaConstancia
            NewLiquidacione.ValorConstancia = LiquidacionePorDefecto.ValorConstancia
            NewLiquidacione.GeneraConstancia = LiquidacionePorDefecto.GeneraConstancia
            NewLiquidacione.Cargado = LiquidacionePorDefecto.Cargado
            NewLiquidacione.Actualizacion = LiquidacionePorDefecto.Actualizacion
            NewLiquidacione.Usuario = Program.Usuario
            NewLiquidacione.CumplimientoTitulo = Now.Date

            'SLB20131120 Para el manejo de la facturación
            'NewLiquidacione.NroLote = LiquidacionePorDefecto.NroLote
            NewLiquidacione.NroLote = 0

            NewLiquidacione.ValorEntregaContraPago = LiquidacionePorDefecto.ValorEntregaContraPago
            NewLiquidacione.AquienSeEnviaRetencion = LiquidacionePorDefecto.AquienSeEnviaRetencion
            NewLiquidacione.IDBaseDias = LiquidacionePorDefecto.IDBaseDias
            NewLiquidacione.TipoDeOferta = LiquidacionePorDefecto.TipoDeOferta
            NewLiquidacione.NroLoteENC = LiquidacionePorDefecto.NroLoteENC
            NewLiquidacione.ContabilidadENC = LiquidacionePorDefecto.ContabilidadENC
            NewLiquidacione.IDLiquidaciones = -1
            NewLiquidacione.CodigoIntermediario = LiquidacionePorDefecto.CodigoIntermediario
            NewLiquidacione.OperacionEnMercadoExtranjero = String.Empty
            LiquidacioneAnterior = LiquidacioneSelected
            clientes.Comitente = ""
            clientes.NombreCliente = ""
            clientesOrdenantes.Ordenante = ""
            clientesOrdenantes.NombreClienteOrdenante = ""
            Especie.IDEspecie = ""
            Especie.NombreEspecie = ""
            habilitaboton = False
            habilitaradio = True
            LiquidacioneSelected = NewLiquidacione
            VeriOrdenes.NumeroOrden = Now.Date.Year
            VeriOrdenes.NumeroOrden1 = String.Empty


            MyBase.CambioItem("Liquidaciones")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Filtrar()
        Try
            selected = True
            dcProxy.Liquidacione_MIs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.LiquidacionesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.LiquidacionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub ConfirmarBuscar()
        Try
            selected = True
            If cb.ID <> 0 Or cb.IDComitente <> String.Empty Or cb.Tipo <> String.Empty Or cb.ClaseOrden <> String.Empty Or cb.NumeroOrden1 <> String.Empty Or Not IsNothing(cb.Liquidacion) Or Not IsNothing(cb.CumplimientoTitulo) Or cb.Parcial <> 0 Or cb.IDOrdenante <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Liquidacione_MIs.Clear()
                LiquidacioneAnterior = Nothing

                Dim orden As String
                If Not IsNumeric(cb.NumeroOrden1) And Not IsNothing(cb.NumeroOrden1) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El número de la orden debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    cb.NumeroOrden1 = Nothing
                    Exit Sub
                End If
                IsBusy = True
                If cb.NumeroOrden1 Is Nothing Then
                    orden = "0"
                Else
                    orden = CStr(cb.NumeroOrden) + Format(CInt(cb.NumeroOrden1), "000000")
                End If
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " IDComitente = " & cb.IDComitente.ToString()
                dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, "A", IIf(cb.Liquidacion Is Nothing, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo Is Nothing, "1799/01/01", cb.CumplimientoTitulo), CInt(orden), cb.Parcial, cb.IDOrdenante, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaLiquidacione
                CambioItem("cb")
            Else
                ErrorForma = ""
                dcProxy.Liquidacione_MIs.Clear()
                LiquidacioneAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, "C", "A", IIf(cb.Liquidacion Is Nothing, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo Is Nothing, "1799/01/01", cb.CumplimientoTitulo), cb.NumeroOrden, cb.Parcial, cb.IDOrdenante, cb.NumeroOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaLiquidacione
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Function ValidarReceptoresOrdenes() As Boolean
        Dim retorno As Boolean = False
        Try
            Dim existeLider As Integer
            Dim sumaPorcentaje As Integer

            If ListaReceptoresOrdenes.Count <> 0 Then
                For Each detalleReceptor In ListaReceptoresOrdenes
                    If IsNothing(detalleReceptor.Porcentaje) Or detalleReceptor.Porcentaje = 0 Then
                        Throw New ValidationException("El porcentaje no puede ser vacío.")
                    End If
                    If detalleReceptor.Lider Then
                        existeLider = existeLider + 1
                    End If
                    If IsNothing(detalleReceptor.IDReceptor) Then
                        Throw New ValidationException("El código de receptor no puede ser vacío.")
                    End If
                    If IsNothing(detalleReceptor.Nombre) Then
                        Throw New ValidationException("El nombre del receptor no puede ser vacío, este se establece seleccionando el código del receptor.")
                    End If
                    sumaPorcentaje = sumaPorcentaje + detalleReceptor.Porcentaje
                Next

                If existeLider = 0 Then
                    Throw New ValidationException("Debe haber un receptor líder.")
                End If
                If existeLider > 1 Then
                    Throw New ValidationException("Sólo debe existir un líder.")
                End If
                If sumaPorcentaje <> 100 Then
                    Throw New ValidationException("El porcentaje es: " & sumaPorcentaje & ", el total debe ser 100.")
                End If

                Dim numeroErrores = (From lr In ListaReceptoresOrdenes Where lr.HasValidationErrors = True).Count
                If numeroErrores <> 0 Then
                    Throw New ValidationException("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.")
                End If
            Else
                Throw New ValidationException("Debe existir al menos un receptor.")
            End If
            retorno = True
        Catch ex As ValidationException
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return retorno
    End Function
    Public Overrides Sub ActualizarRegistro()
        Try
            If validafechacierre("ingresada o modificada") Then
                If ValidarReceptoresOrdenes() Then
                    If validaciones() Then

                        If LiquidacioneSelected.Manual Then
                            If LiquidacioneSelected.Cantidad > mcursaldoOrden Then
                                disparaguardar = False
                                'C1.Silverlight.C1MessageBox.Show("La cantidad de la operación supera el saldo de la Orden " _
                                '                                 & "Desea continuar?", _
                                'Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPreguntaSobregiro)
                                mostrarMensajePregunta("La cantidad de la operación supera el saldo de la Orden.", _
                                                       Program.TituloSistema, _
                                                       "ACTUALIZARREGISTRO", _
                                                       AddressOf TerminaPreguntaSobregiro, True, "¿Desea continuar?")
                                Exit Sub
                            End If
                        End If

                        Ordenesliq()
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            disparaguardar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPreguntaSobregiro(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            Ordenesliq()
        End If
    End Sub


    Private Sub Guardarliq()
        Dim origen = "update"
        ErrorForma = ""
        LiquidacioneAnterior = LiquidacioneSelected
        dcProxy.CumplimientoOrden_liq(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, _
                            LiquidacioneSelected.IDEspecie, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarCumplimientoOrderliq, Nothing)
        IsBusy = True
    End Sub
    Public Sub actualizaespecie()
        Try
            'si es edicion no entra 
            If ListaEspeciesLiquidaciones.Count > 0 Then
                dcProxy.ActualizaEspecieOrdenes(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, 0, ListaEspeciesLiquidaciones.First.IDEspecie, LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarespeciesordenes, Nothing)
            Else
                dcProxy.ActualizaEspecieOrdenes(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, 0, "", LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarespeciesordenes, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End Try


    End Sub
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                ' A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                ' Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)


                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                Dim strMsg As String = String.Empty
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" or So.UserState = "BorrarRegistro"  then
            If validaactualizarespecie Then
                actualizaespecie()
                validaactualizarespecie = False
            End If
            If So.UserState = "insert" Or So.UserState = "BorrarRegistro" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.Liquidacione_MIs.Clear()
                dcProxy.Load(dcProxy.LiquidacionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, So.UserState) ' Recarga la lista para que carguen los include
            End If
            'If So.UserState = "BorrarRegistro" Then
            '    ListaLiquidaciones = _ListaLiquidaciones
            'End If
            habilitaboton = True
            habilitaradio = False
            mlogCantidad = False
            habilitamanualsi = False
            disparafocus = False
            disparaguardar = False
            Userstate = ""
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Metodo encargado de lanzar la edicion del formulario.
    ''' </summary>
    ''' <remarks>        
    ''' Modificado por:	Juan Carlos Soto Cruz.
    ''' Descripción:    Se inicializan algunas propiedades de la liquidacion seleccionada de manera provisional para poder realizar los metodos CRUD de la pestaña
    '''                 Receptores. Estos se deben quitar una vez se realicen los metodos CRUD para la liquidacion. Se encuentran entre las etiquetas Jsoto 2011-08-10 y Fin Jsoto 2011-08-10 
    ''' Fecha:			Agosto 10/2011        
    ''' </remarks>
    Public Overrides Sub EditarRegistro()
        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        estadoregistro = False
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        If Not IsNothing(_LiquidacioneSelected) Then

            'Jsoto 2011-08-10
            'Retirar este codigo segun condiciones del Summary
            LiquidacioneSelected.AutoRetenedor = "N"
            LiquidacioneSelected.Certificacion = "N"
            LiquidacioneSelected.ConstanciaEnajenacion = "N"
            LiquidacioneSelected.RepoTitulo = "N"
            LiquidacioneSelected.Sujeto = "N"
            LiquidacioneSelected.Traslado = "1"
            LiquidacioneSelected.Swap = "N"
            LiquidacioneSelected.Reinversion = "N"
            LiquidacioneSelected.Usuario = Program.Usuario
            trasladar.Traslado = 1
            LiquidacioneSelected.Actualizacion = Now
            'Fin Jsoto 2011-08-10
            Editando = True
            habilitaboton = False
            habilitaradio = False
            mdblCantidad = LiquidacioneSelected.Cantidad
            'JBT 20121019
            ESTADO_EDICION = True
            SaldoOrden()
            'mcursaldoOrden = LiquidacioneSelected.Cantidad
            If LiquidacioneSelected.ClaseOrden.Equals("C") Then
                habilitamanualno = True
                habilitamanualdias = True
            End If
            If LiquidacioneSelected.Manual Then
                If LiquidacioneSelected.IDFactura = 0 Then
                    habilitamanualsi = True
                    If LiquidacioneSelected.Plaza = "LOC" Then
                        HabilitarComisionistaLocal = True
                        HabilitarComisionistaOtraPlaza = False
                    Else
                        HabilitarComisionistaLocal = False
                        HabilitarComisionistaOtraPlaza = True
                    End If
                    HabilitarFechaLiquidacion = True
                    HabilitarFechaVendido = True
                    HabilitarFechaTitulo = False
                    HabilitarFechaEfectivo = False
                Else
                    HabilitarFechaLiquidacion = False
                    HabilitarFechaVendido = True
                    HabilitarFechaTitulo = False
                    HabilitarFechaEfectivo = False
                End If
            End If
            If LiquidacioneSelected.Tipo.Equals("C") Or LiquidacioneSelected.Tipo.Equals("R") Then
                habilitamanualno = True
            End If
        End If
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            habilitaboton = True
            HabilitarComisionistaLocal = False
            HabilitarComisionistaOtraPlaza = False
            HabilitarFechaLiquidacion = False
            HabilitarFechaVendido = False
            HabilitarFechaTitulo = False
            HabilitarFechaEfectivo = False
            ErrorForma = ""
            If Not IsNothing(_LiquidacioneSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                habilitamanualsi = False
                If _LiquidacioneSelected.EntityState = EntityState.Detached Then
                    LiquidacioneSelected = LiquidacioneAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub BorrarRegistro()
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(LiquidacioneSelected) Then   'JABG20160318
                If LiquidacioneSelected.IDFactura > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("La liquidación está facturada, no se puede borrar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If validafechacierre("borrada") Then

                    selected = True
                    If Not IsNothing(LiquidacioneSelected) Then
                        dcProxy.Actualizaordenestado(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarordenestado, _
                                                     LiquidacioneSelected.Tipo + "," + LiquidacioneSelected.ClaseOrden + "," + LiquidacioneSelected.IDOrden.ToString)
                        If ListaEspeciesLiquidaciones.Count > 0 Then
                            dcProxy.ActualizaEspecieOrdenes(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, 0, ListaEspeciesLiquidaciones.First.IDEspecie, LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarespeciesordenes, Nothing)
                        Else
                            dcProxy.ActualizaEspecieOrdenes(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, 0, "", LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminoactualizarespeciesordenes, Nothing)
                        End If

                        If dcProxy.Liquidacione_MIs.Where(Function(i) i.ID = LiquidacioneSelected.ID).Count > 0 Then
                            dcProxy.Liquidacione_MIs.Remove(dcProxy.Liquidacione_MIs.Where(Function(i) i.ID = LiquidacioneSelected.ID).First)
                        End If
                        'ListaLiquidaciones.Remove(LiquidacioneSelected)
                        selected = True
                        LiquidacioneSelected = _ListaLiquidaciones.LastOrDefault
                        'LiquidacioneSelected = Nothing
                        IsBusy = True
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                    End If
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb.Liquidacion = Nothing
        cb.CumplimientoTitulo = Nothing
        cb.DisplayDate = Date.Now
        cb.DisplayDate2 = Date.Now
        cb.IDComitente = String.Empty
        cb.IDOrdenante = String.Empty
        cb.Parcial = Nothing
        cb.ID = Nothing
        cb.NumeroOrden1 = Nothing
        'CType(VmLiquidaciones.dfBuscar.FindName("txtnOrden"), TextBox).Focus()
        'CType(VmLiquidaciones.dfBuscar.FindName("Cumplimiento"), DatePicker).ClearValue(DatePicker.SelectedDateProperty)
        'CType(VmLiquidaciones.dfBuscar.FindName("FLiquidacion"), DatePicker).ClearValue(DatePicker.SelectedDateProperty)
        MyBase.Buscar()

        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub
    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.LiquidacioneSelected Is Nothing Then
                Select Case pstrTipoItem
                    Case "clientes"
                        If Not IsNothing(LiquidacioneSelected.IDComitente) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.LiquidacioneSelected.IDComitente
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad01.BuscadorClientes.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(LiquidacioneSelected.IDComitente, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarClienteCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "clientesOrdenantes"
                        If Not IsNothing(LiquidacioneSelected.IDOrdenante) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.LiquidacioneSelected.IDOrdenante
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad02.BuscadorClientes.Clear()
                                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarOrdenantesComitenteQuery(LiquidacioneSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf buscarOrdenanteCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "especies"
                        If Not IsNothing(LiquidacioneSelected.IDEspecie) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.LiquidacioneSelected.IDEspecie
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad02.BuscadorEspecies.Clear()
                                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarNemotecnicoEspecificoQuery("", LiquidacioneSelected.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted, pstrTipoItem)
                                'SE ENVIA EL PARAMETRO PSTRMERCADO VACIO A LA FUNCION buscarNemotecnicoEspecifico EN EL DOMAINSERVICES DE UTILIDADES
                            End If
                        End If
                    Case Else
                        logConsultar = False
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la ciudad", Me.ToString(), "Buscar ciudad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarClienteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Me.clientes.NombreCliente = lo.Entities.ToList.Item(0).Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarClienteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarOrdenanteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorOrdenantes))
        Dim strTipoItem As String
        Dim IdOrdenante As String
        Dim cont As Integer

        Try

            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            'RABP20191022_Se adiciona lógica para poder mostrar correctamente el nombre del ordenante, ya que estaba seleccionanado el primero que encontrara
            cont = 0
            IdOrdenante = Me.LiquidacioneSelected.IDOrdenante

            If lo.Entities.ToList.Count > 0 Then
                While lo.Entities.ToList.Count <> cont

                    If IdOrdenante = lo.Entities.ToList.Item(cont).IdOrdenante Then

                        Me.clientesOrdenantes.NombreClienteOrdenante = lo.Entities.ToList.Item(cont).Nombre

                    End If
                    cont = cont + 1

                End While

            End If

            'If lo.Entities.ToList.Count > 0 Then
            '    Me.clientesOrdenantes.NombreClienteOrdenante = lo.Entities.ToList.Item(0).Nombre
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarOrdenanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Me.Especie.NombreEspecie = lo.Entities.ToList.Item(0).Especie
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarOrdenanteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub Aplazamientos()
        If IsNothing(LiquidacioneSelected) Then
            Exit Sub
        End If
        If LiquidacioneSelected.Tipo.Equals("R") Or LiquidacioneSelected.Tipo.Equals("S") Then
            strTipoAplazamiento = "I"
        Else
            strTipoAplazamiento = "S"
        End If
        aplazamientoserie = New Aplazamiento_En_Serie_MI(strTipoAplazamiento, LiquidacioneSelected.Liquidacion, LiquidacioneSelected.CumplimientoTitulo)
        AddHandler aplazamientoserie.Closed, AddressOf CerroVentana
        Program.Modal_OwnerMainWindowsPrincipal(aplazamientoserie)
        aplazamientoserie.ShowDialog()

    End Sub
    Public Sub Duplicar()
        If IsNothing(LiquidacioneSelected) Then
            Exit Sub
        End If
        estadoregistro = True
        _mlogDuplicando = True
        listaliquidacionesvalidar.Clear()
        dcProxy.LiquidacionesConsultar_MIs.Clear()
        dcProxy.Load(dcProxy.LiquidacionesConsultarvalidarQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerliquidacinesValidar, Nothing)
        MyBase.CambioItem("Editando")
        duplica = True
    End Sub
    Private Sub NuevoDuplicado()
        Try
            'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Dim NewLiquidacione As New Liquidacione_MI
            'TODO: Verificar cuales son los campos que deben inicializarse 
            NewLiquidacione.IDComisionista = LiquidacioneSelected.IDComisionista
            NewLiquidacione.IDSucComisionista = LiquidacioneSelected.IDSucComisionista
            NewLiquidacione.ID = LiquidacioneSelected.ID
            'NewLiquidacione.Parcial = Nothing
            NewLiquidacione.Parcial = -1
            NewLiquidacione.Tipo = LiquidacioneSelected.Tipo
            NewLiquidacione.ClaseOrden = LiquidacioneSelected.ClaseOrden
            NewLiquidacione.IDEspecie = LiquidacioneSelected.IDEspecie
            NewLiquidacione.IDOrden = LiquidacioneSelected.IDOrden

            'SLB20131120 Para el manejo de la facturación
            'NewLiquidacione.Prefijo = LiquidacioneSelected.Prefijo
            'NewLiquidacione.IDFactura = LiquidacioneSelected.IDFactura
            'NewLiquidacione.Facturada = LiquidacioneSelected.Facturada
            NewLiquidacione.Prefijo = Nothing
            NewLiquidacione.IDFactura = Nothing
            NewLiquidacione.Facturada = Nothing

            NewLiquidacione.IDComitente = LiquidacioneSelected.IDComitente
            NewLiquidacione.IDOrdenante = LiquidacioneSelected.IDOrdenante
            NewLiquidacione.IDBolsa = LiquidacioneSelected.IDBolsa
            NewLiquidacione.ValBolsa = LiquidacioneSelected.ValBolsa
            NewLiquidacione.IDRueda = LiquidacioneSelected.IDRueda
            NewLiquidacione.TasaDescuento = LiquidacioneSelected.TasaDescuento
            NewLiquidacione.TasaCompraVende = LiquidacioneSelected.TasaCompraVende
            NewLiquidacione.Modalidad = LiquidacioneSelected.Modalidad
            NewLiquidacione.IndicadorEconomico = LiquidacioneSelected.IndicadorEconomico
            NewLiquidacione.PuntosIndicador = LiquidacioneSelected.PuntosIndicador
            NewLiquidacione.Plazo = LiquidacioneSelected.Plazo
            NewLiquidacione.Liquidacion = LiquidacioneSelected.Liquidacion
            NewLiquidacione.Cumplimiento = LiquidacioneSelected.Cumplimiento
            NewLiquidacione.Emision = LiquidacioneSelected.Emision
            NewLiquidacione.Vencimiento = LiquidacioneSelected.Vencimiento
            NewLiquidacione.OtraPlaza = LiquidacioneSelected.OtraPlaza
            NewLiquidacione.Plaza = LiquidacioneSelected.Plaza
            NewLiquidacione.IDComisionistaLocal = LiquidacioneSelected.IDComisionistaLocal
            NewLiquidacione.IDComisionistaOtraPlaza = LiquidacioneSelected.IDComisionistaOtraPlaza
            NewLiquidacione.IDCiudadOtraPlaza = LiquidacioneSelected.IDCiudadOtraPlaza
            NewLiquidacione.TasaEfectiva = LiquidacioneSelected.TasaEfectiva
            'NewLiquidacione.Cantidad = LiquidacioneSelected.Cantidad
            NewLiquidacione.Cantidad = consultarcantidades.CantidadOrden - consultarcantidades.CantidadLiq
            mcursaldoOrden = NewLiquidacione.Cantidad
            NewLiquidacione.Precio = 0
            NewLiquidacione.FactorComisionPactada = 0
            If NewLiquidacione.Precio = 0 Then
                NewLiquidacione.Transaccion_cur = listaliquidacionesvalidar.First.Cantidad
                NewLiquidacione.SubTotalLiq = NewLiquidacione.Transaccion_cur
                NewLiquidacione.TotalLiq = NewLiquidacione.Transaccion_cur
            Else
                If NewLiquidacione.ClaseOrden.Equals("A") Then
                    NewLiquidacione.Transaccion_cur = NewLiquidacione.Precio * NewLiquidacione.Cantidad
                Else
                    NewLiquidacione.Transaccion_cur = NewLiquidacione.Precio * NewLiquidacione.Cantidad / 100
                End If
                NewLiquidacione.SubTotalLiq = NewLiquidacione.Transaccion_cur
                NewLiquidacione.TotalLiq = NewLiquidacione.Transaccion_cur

            End If

            If Not IsNothing(listaliquidacionesvalidar.First.ComisionPactada) Then
                If Not IsNothing(NewLiquidacione.Transaccion_cur) Then
                    NewLiquidacione.Comision = NewLiquidacione.Transaccion_cur * NewLiquidacione.FactorComisionPactada
                    If listaliquidacionesvalidar.First.Tipo.Equals("C") Or listaliquidacionesvalidar.First.Tipo.Equals("R") Then
                        NewLiquidacione.SubTotalLiq = NewLiquidacione.SubTotalLiq + NewLiquidacione.Comision
                        NewLiquidacione.TotalLiq = NewLiquidacione.TotalLiq + NewLiquidacione.Comision
                    Else
                        NewLiquidacione.SubTotalLiq = NewLiquidacione.SubTotalLiq - NewLiquidacione.Comision
                        NewLiquidacione.TotalLiq = NewLiquidacione.TotalLiq - NewLiquidacione.Comision
                    End If
                End If
            Else
                NewLiquidacione.Comision = 0
            End If
            If Not IsNothing(listaliquidacionesvalidar.First.Objeto) Then
                If Not listaliquidacionesvalidar.First.Ordinaria And listaliquidacionesvalidar.First.Objeto.Equals("CRR") Then
                    'HABILITAR ESTAS DOS  PROPIEDADES
                    habilitaparciald = True
                End If
            End If
            NewLiquidacione.Transaccion = LiquidacioneSelected.Transaccion
            'NewLiquidacione.SubTotalLiq = LiquidacioneSelected.SubTotalLiq
            'NewLiquidacione.TotalLiq = LiquidacioneSelected.TotalLiq
            'NewLiquidacione.Comision = LiquidacioneSelected.Comision
            NewLiquidacione.Retencion = LiquidacioneSelected.Retencion
            NewLiquidacione.Intereses = LiquidacioneSelected.Intereses
            NewLiquidacione.ValorIva = LiquidacioneSelected.ValorIva
            NewLiquidacione.DiasIntereses = LiquidacioneSelected.DiasIntereses
            NewLiquidacione.FactorComisionPactada = LiquidacioneSelected.FactorComisionPactada
            NewLiquidacione.Mercado = LiquidacioneSelected.Mercado
            NewLiquidacione.NroTitulo = LiquidacioneSelected.NroTitulo
            NewLiquidacione.IDCiudadExpTitulo = LiquidacioneSelected.IDCiudadExpTitulo
            NewLiquidacione.PlazoOriginal = LiquidacioneSelected.PlazoOriginal
            NewLiquidacione.Aplazamiento = LiquidacionePorDefecto.Aplazamiento
            NewLiquidacione.VersionPapeleta = LiquidacioneSelected.VersionPapeleta
            NewLiquidacione.EmisionOriginal = LiquidacioneSelected.EmisionOriginal
            NewLiquidacione.VencimientoOriginal = LiquidacioneSelected.VencimientoOriginal
            NewLiquidacione.Impresiones = LiquidacioneSelected.Impresiones
            NewLiquidacione.FormaPago = LiquidacioneSelected.FormaPago
            NewLiquidacione.CtrlImpPapeleta = LiquidacioneSelected.CtrlImpPapeleta
            NewLiquidacione.DiasVencimiento = LiquidacioneSelected.DiasVencimiento
            NewLiquidacione.PosicionPropia = LiquidacioneSelected.PosicionPropia
            NewLiquidacione.Transaccion = LiquidacioneSelected.Transaccion
            NewLiquidacione.TipoOperacion = LiquidacioneSelected.TipoOperacion
            NewLiquidacione.DiasContado = LiquidacioneSelected.DiasContado
            NewLiquidacione.Ordinaria = LiquidacioneSelected.Ordinaria
            NewLiquidacione.ObjetoOrdenExtraordinaria = LiquidacioneSelected.ObjetoOrdenExtraordinaria
            NewLiquidacione.NumPadre = LiquidacioneSelected.NumPadre
            NewLiquidacione.ParcialPadre = LiquidacioneSelected.ParcialPadre
            NewLiquidacione.OperacionPadre = LiquidacioneSelected.OperacionPadre
            NewLiquidacione.DiasTramo = LiquidacioneSelected.DiasTramo
            NewLiquidacione.Vendido = LiquidacioneSelected.Vendido
            NewLiquidacione.Vendido = LiquidacioneSelected.Vendido
            NewLiquidacione.Manual = LiquidacioneSelected.Manual
            NewLiquidacione.ValorTraslado = LiquidacioneSelected.ValorTraslado
            NewLiquidacione.ValorBrutoCompraVencida = LiquidacioneSelected.ValorBrutoCompraVencida
            NewLiquidacione.AutoRetenedor = LiquidacioneSelected.AutoRetenedor
            NewLiquidacione.Sujeto = LiquidacioneSelected.Sujeto
            NewLiquidacione.PcRenEfecCompraRet = LiquidacioneSelected.PcRenEfecCompraRet
            NewLiquidacione.PcRenEfecVendeRet = LiquidacioneSelected.PcRenEfecVendeRet
            NewLiquidacione.Reinversion = LiquidacioneSelected.Reinversion
            NewLiquidacione.Swap = LiquidacioneSelected.Swap
            NewLiquidacione.NroSwap = LiquidacioneSelected.NroSwap
            NewLiquidacione.Certificacion = LiquidacioneSelected.Certificacion
            NewLiquidacione.DescuentoAcumula = LiquidacioneSelected.DescuentoAcumula
            NewLiquidacione.PctRendimiento = LiquidacioneSelected.PctRendimiento
            NewLiquidacione.FechaCompraVencido = LiquidacioneSelected.FechaCompraVencido
            NewLiquidacione.PrecioCompraVencido = LiquidacioneSelected.PrecioCompraVencido
            NewLiquidacione.ConstanciaEnajenacion = LiquidacioneSelected.ConstanciaEnajenacion
            NewLiquidacione.RepoTitulo = LiquidacioneSelected.RepoTitulo
            NewLiquidacione.ServBolsaVble = LiquidacioneSelected.ServBolsaVble
            NewLiquidacione.ServBolsaFijo = LiquidacioneSelected.ServBolsaFijo
            NewLiquidacione.Traslado = LiquidacioneSelected.Traslado
            NewLiquidacione.UBICACIONTITULO = LiquidacioneSelected.UBICACIONTITULO
            NewLiquidacione.HoraGrabacion = LiquidacioneSelected.HoraGrabacion
            NewLiquidacione.OrigenOperacion = LiquidacioneSelected.OrigenOperacion
            NewLiquidacione.CodigoOperadorCompra = LiquidacioneSelected.CodigoOperadorCompra
            NewLiquidacione.CodigoOperadorVende = LiquidacioneSelected.CodigoOperadorVende
            NewLiquidacione.IdentificacionRemate = LiquidacioneSelected.IdentificacionRemate
            NewLiquidacione.ModalidaOperacion = LiquidacioneSelected.ModalidaOperacion
            NewLiquidacione.IndicadorPrecio = LiquidacioneSelected.IndicadorPrecio
            NewLiquidacione.PeriodoExdividendo = LiquidacioneSelected.PeriodoExdividendo
            NewLiquidacione.PlazoOperacionRepo = LiquidacioneSelected.PlazoOperacionRepo
            NewLiquidacione.ValorCaptacionRepo = LiquidacioneSelected.ValorCaptacionRepo
            NewLiquidacione.VolumenCompraRepo = LiquidacioneSelected.VolumenCompraRepo
            NewLiquidacione.PrecioNetoFraccion = LiquidacioneSelected.PrecioNetoFraccion
            NewLiquidacione.VolumenNetoFraccion = LiquidacioneSelected.VolumenNetoFraccion
            NewLiquidacione.CodigoContactoComercial = LiquidacioneSelected.CodigoContactoComercial
            NewLiquidacione.NroFraccionOperacion = LiquidacioneSelected.NroFraccionOperacion
            NewLiquidacione.IdentificacionPatrimonio1 = LiquidacioneSelected.IdentificacionPatrimonio1
            NewLiquidacione.TipoidentificacionCliente2 = LiquidacioneSelected.TipoidentificacionCliente2
            NewLiquidacione.NitCliente2 = LiquidacioneSelected.NitCliente2
            NewLiquidacione.IdentificacionPatrimonio2 = LiquidacioneSelected.IdentificacionPatrimonio2
            NewLiquidacione.TipoIdentificacionCliente3 = LiquidacioneSelected.TipoIdentificacionCliente3
            NewLiquidacione.NitCliente3 = LiquidacioneSelected.NitCliente3
            NewLiquidacione.IdentificacionPatrimonio3 = LiquidacioneSelected.IdentificacionPatrimonio3
            NewLiquidacione.IndicadorOperacion = LiquidacioneSelected.IndicadorOperacion
            NewLiquidacione.BaseRetencion = LiquidacioneSelected.BaseRetencion
            NewLiquidacione.PorcRetencion = LiquidacioneSelected.PorcRetencion
            NewLiquidacione.BaseRetencionTranslado = LiquidacioneSelected.BaseRetencionTranslado
            NewLiquidacione.PorcRetencionTranslado = LiquidacioneSelected.PorcRetencionTranslado
            NewLiquidacione.PorcIvaComision = LiquidacioneSelected.PorcIvaComision
            NewLiquidacione.IndicadorAcciones = LiquidacioneSelected.IndicadorAcciones
            NewLiquidacione.OperacionNegociada = LiquidacioneSelected.OperacionNegociada
            NewLiquidacione.FechaConstancia = LiquidacioneSelected.FechaConstancia
            NewLiquidacione.ValorConstancia = LiquidacioneSelected.ValorConstancia
            NewLiquidacione.GeneraConstancia = LiquidacioneSelected.GeneraConstancia
            NewLiquidacione.Cargado = LiquidacioneSelected.Cargado
            NewLiquidacione.Actualizacion = Now
            NewLiquidacione.Usuario = Program.Usuario
            NewLiquidacione.CumplimientoTitulo = LiquidacioneSelected.CumplimientoTitulo

            'SLB20131120 Para el manejo de la facturación
            'NewLiquidacione.NroLote = LiquidacioneSelected.NroLote
            NewLiquidacione.NroLote = 0

            NewLiquidacione.ValorEntregaContraPago = LiquidacioneSelected.ValorEntregaContraPago
            NewLiquidacione.AquienSeEnviaRetencion = LiquidacioneSelected.AquienSeEnviaRetencion
            NewLiquidacione.IDBaseDias = LiquidacioneSelected.IDBaseDias
            NewLiquidacione.TipoDeOferta = LiquidacioneSelected.TipoDeOferta
            NewLiquidacione.NroLoteENC = LiquidacioneSelected.NroLoteENC
            NewLiquidacione.ContabilidadENC = LiquidacioneSelected.ContabilidadENC
            NewLiquidacione.IDMoneda = LiquidacioneSelected.IDMoneda
            NewLiquidacione.ValorCostos = LiquidacioneSelected.ValorCostos
            NewLiquidacione.ValorNeto = LiquidacioneSelected.ValorNeto
            NewLiquidacione.ValorBruto = LiquidacioneSelected.ValorBruto
            NewLiquidacione.ComisionPesos = LiquidacioneSelected.ComisionPesos
            NewLiquidacione.ValorIVAComisionPesos = LiquidacioneSelected.ValorIVAComisionPesos
            NewLiquidacione.ValorCostosPesos = LiquidacioneSelected.ValorCostosPesos
            NewLiquidacione.ValorNetoPesos = LiquidacioneSelected.ValorNetoPesos
            NewLiquidacione.IDLiquidaciones = -1
            NewLiquidacione.CodigoIntermediario = LiquidacioneSelected.CodigoIntermediario
            LiquidacioneAnterior = LiquidacioneSelected
            habilitaboton = False
            habilitaradio = False
            LiquidacioneSelected = NewLiquidacione
            MyBase.CambioItem("Liquidaciones")
            Editando = True
            MyBase.CambioItem("Editando")
            IsBusy = False
            mlogRecalculaValoresAlDuplicar = True
            validaorden()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub CerroVentana(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If aplazamientoserie.DialogResult = True Then
                Select Case aplazamientoserie.TipoSelected.Descripcion
                    Case strtitulo
                        strAplazamiento = "T"
                    Case strefectivo
                        strAplazamiento = "A"
                End Select

                If strTipoAplazamiento.Equals("S") Then
                    'C1.Silverlight.C1MessageBox.Show("Se aplazarán todos los parciales de compra o venta de la operación: " & LiquidacioneSelected.ID _
                    '                                 & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Se aplazarán todos los parciales de compra o venta de la operación: " & LiquidacioneSelected.ID _
                                                     & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion,
                                            Program.TituloSistema,
                                            "CERROVENTANA",
                                            AddressOf TerminaPregunta, True, "¿Desea continuar?")

                Else
                    'C1.Silverlight.C1MessageBox.Show("Se aplazará la operación: " & LiquidacioneSelected.ID _
                    '                                              & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPregunta)
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Se aplazará la operación: " & LiquidacioneSelected.ID _
                                                    & vbCr & " con la siguiente Fecha: " & aplazamientoserie.TipoSelected.FechaAplazamiento & " de " & aplazamientoserie.TipoSelected.Descripcion,
                                            Program.TituloSistema,
                                            "CERROVENTANA",
                                            AddressOf TerminaPregunta, True, "¿Desea continuar?")
                End If
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana emergente", Me.ToString(), "CerroVentana", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                If Not IsNothing(LiquidacioneSelected) Then
                    objProxy.Aplazamiento(strTipoAplazamiento, strAplazamiento, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.Tipo, LiquidacioneSelected.ID _
                    , LiquidacioneSelected.Parcial, LiquidacioneSelected.Liquidacion, aplazamientoserie.TipoSelected.FechaAplazamiento, Program.Usuario, strerror, strnroaplazamiento, Program.HashConexion, AddressOf TerminoactualizarAplazamiento, "")
                End If
            Else
                IsBusy = False
                Exit Sub

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar pregunta", Me.ToString(), "TerminaPregunta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub TerminoactualizarAplazamiento(ByVal obj As InvokeOperation(Of String))
        IsBusy = False
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else

            If Not IsNothing(obj.Value) Then
                If (IsNumeric(obj.Value)) Then
                    If CInt(obj.Value) = -5 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La operación tiene titulos descargados" & vbNewLine & " No puede ser aplazada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Se aplazaron " & obj.Value & " Operaciones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                        LiquidacioneAnterior = Nothing
                        dcProxy.Liquidacione_MIs.Clear()
                        dcProxy.Load(dcProxy.LiquidacionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "") ' Recarga la lista para que carguen los include
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(obj.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else

            End If
        End If
    End Sub
    Public Function [IsNumeric](ByVal str As String) As Boolean
        Dim result As Decimal = 0
        Return Decimal.TryParse(str, result)
    End Function
    Private Sub _LiquidacioneSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _LiquidacioneSelected.PropertyChanged
        Select Case e.PropertyName
            'Case "IDOrden"
            '    If LiquidacioneSelected.IDOrden = Nothing Then
            '        Exit Sub
            '    End If
            '    listaliquidacionesvalidar.Clear()
            '    dcProxy.LiquidacionesConsultars.Clear()
            '    dcProxy.Load(dcProxy.LiquidacionesConsultarvalidarQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden,Program.Usuario, Program.HashConexion), AddressOf TerminotraerliquidacinesValidar, Nothing)

            Case "DiasVencimiento"
                If Not IsNothing(LiquidacioneSelected.Plazo) And Not IsNothing(LiquidacioneSelected.DiasVencimiento) _
          And LiquidacioneSelected.DiasVencimiento > LiquidacioneSelected.Plazo Then
                    A2Utilidades.Mensajes.mostrarMensaje("Los días al vencimiento deben ser menores o iguales que el plazo del título ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If Not IsNothing(LiquidacioneSelected.DiasVencimiento) Then
                    If IsNothing(LiquidacioneSelected.Vencimiento) Then
                        LiquidacioneSelected.Vencimiento = DateAdd("d", LiquidacioneSelected.DiasVencimiento, LiquidacioneSelected.Liquidacion)
                    End If
                    If Not IsNothing(LiquidacioneSelected.Plazo) Then
                        If IsNothing(LiquidacioneSelected.Emision) Then
                            LiquidacioneSelected.Emision = DateAdd("d", LiquidacioneSelected.Plazo * (1 - 2), LiquidacioneSelected.Vencimiento)
                        End If
                    End If
                End If
            Case "Vencimiento"
                If LiquidacioneSelected.Vencimiento < LiquidacioneSelected.Emision Then
                    A2Utilidades.Mensajes.mostrarMensaje("La Fecha de vencimiento debe ser mayor que la de emisión ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LiquidacioneSelected.Vencimiento = Now
                    Exit Sub
                End If
            Case "Cantidad"
                If ListaLiquidaciones.Contains(LiquidacioneSelected) Then
                    mlogCantidad = True
                End If
            Case "Plaza" 'SLB 20130109 Se adiciona para el manejo del Comisionista Local o Otra Plaza como VB.6
                If _LiquidacioneSelected.Plaza = "LOC" Then
                    HabilitarComisionistaLocal = True
                    HabilitarComisionistaOtraPlaza = False
                    _LiquidacioneSelected.IDComisionistaOtraPlaza = 0
                    _LiquidacioneSelected.IDCiudadOtraPlaza = 0
                Else
                    HabilitarComisionistaLocal = False
                    HabilitarComisionistaOtraPlaza = True
                    _LiquidacioneSelected.IDComisionistaLocal = 0
                End If
            Case "Tipo"
                If Me._LiquidacioneSelected.IDOrden <> 0 And Not IsNothing(Me._LiquidacioneSelected.IDOrden) Then
                    validaorden()
                End If
            Case "Clase"
                If Me._LiquidacioneSelected.IDOrden <> 0 And Not IsNothing(Me._LiquidacioneSelected.IDOrden) Then
                    validaorden()
                End If

        End Select

    End Sub
    Public Sub validaorden()

        If VeriOrdenes.NumeroOrden1 = Nothing Then
            Exit Sub
        End If
        If Not IsNumeric(VeriOrdenes.NumeroOrden1) Then
            A2Utilidades.Mensajes.mostrarMensaje("La orden debe ser un valor númerico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            VeriOrdenes.NumeroOrden1 = Nothing
            Exit Sub
        End If
        Dim a = CStr(VeriOrdenes.NumeroOrden) + Format(CInt(VeriOrdenes.NumeroOrden1), "000000")
        LiquidacioneSelected.IDOrden = CInt(a)
        listaliquidacionesvalidar.Clear()
        dcProxy.LiquidacionesConsultar_MIs.Clear()
        dcProxy.Load(dcProxy.LiquidacionesConsultarvalidarQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerliquidacinesValidar, Nothing)

    End Sub

    Private Sub _NumeroOrden1_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _VeriOrdenes.PropertyChanged
        If LiquidacioneSelected.IDOrden <> 0 And Not IsNothing(LiquidacioneSelected.IDOrden) Then
            Exit Sub
        End If
        If e.PropertyName.Equals("NumeroOrden1") Then
            validaorden()
        End If
    End Sub
    Public Sub validaliq()
        If LiquidacioneSelected.ID <> 0 Then
            dcProxy.verilifaliq(LiquidacioneSelected.ID, LiquidacioneSelected.Parcial, LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.Liquidacion, Program.Usuario, Program.HashConexion, AddressOf Terminotraervalidar, Nothing)
        End If
    End Sub
    Public Sub llevarvalores()
        disparafocus = True
        LiquidacioneSelected.Comision = LiquidacioneSelected.Transaccion_cur * IIf(IsNothing(LiquidacioneSelected.FactorComisionPactada), 0, LiquidacioneSelected.FactorComisionPactada)
        'LiquidacioneSelected.TotalLiq = IIf(IsNothing(LiquidacioneSelected.TotalLiq), 0, LiquidacioneSelected.TotalLiq)
        'LiquidacioneSelected.TotalLiq = IIf(IsNothing(LiquidacioneSelected.SubTotalLiq), 0, LiquidacioneSelected.SubTotalLiq)
        If (Not IsNothing(LiquidacioneSelected.IDFactura) And LiquidacioneSelected.IDFactura <> 0) Or (LiquidacioneSelected.Manual = False) Then
            Exit Sub
        End If
        If LiquidacioneSelected.Precio = 0 Then
            LiquidacioneSelected.Transaccion_cur = LiquidacioneSelected.Cantidad
            LiquidacioneSelected.SubTotalLiq = LiquidacioneSelected.Transaccion_cur
            LiquidacioneSelected.TotalLiq = LiquidacioneSelected.Transaccion_cur
            Denominacionespecie()
        Else
            dcProxy.verilifavalor(LiquidacioneSelected.IDEspecie, LiquidacioneSelected.Liquidacion, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerAplazamientosLiquidacionesvalor, Nothing)
        End If
    End Sub
    Private Sub SaldoOrden()
        dcProxy.consultarcantidad_MIs.Clear()
        dcProxy.Load(dcProxy.LiquidacionesConsultarcantidadQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, LiquidacioneSelected.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminartraerCantidad, Nothing)
    End Sub
    Private Sub NuevoReceptorliq()
        dcProxy.ReceptoresOrdene_MIs.Clear()
        ListaReceptoresOrdenes.Clear()
        dcProxy.Load(dcProxy.ReceptoresOrdenesliqQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf Terminotraerreceptoresliq, Nothing)
    End Sub
    Private Sub BeneficiariosOrdenes()
        dcProxy.BeneficiariosOrdene_MIs.Clear()
        dcProxy.Load(dcProxy.BeneficiariosOrdenesliqQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiariosliq, Nothing)
    End Sub
    Private Sub Especieliq()
        dcProxy.EspeciesLiquidacione_MIs.Clear()
        dcProxy.Load(dcProxy.EspeciesOrdenesliqQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesliq, Nothing)
    End Sub
    Private Function validafechacierre(ByVal opcion As String) As Boolean

        validafechacierre = True
        If LiquidacioneSelected.Liquidacion.ToShortDateString <= fechaCierre Then
            If fechaCierre <> "1900/01/01" Then
                If LiquidacioneSelected.Liquidacion <> fechaCierre Then
                    A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & LiquidacioneSelected.Liquidacion.ToShortDateString() & "). no puede ser " & opcion & " porque su fecha no es igual a la fecha abierta para el usuario ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    validafechacierre = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & LiquidacioneSelected.Liquidacion.ToShortDateString() & "). no puede ser " & opcion & " porque su fecha es inferior a la fecha de cierre", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validafechacierre = False
            End If
        End If
        Return validafechacierre
    End Function
    Private Sub Ordenesliq()
        dcProxy.LiquidacionesOrdenes_MIs.Clear() 'SLB20131127
        dcProxy.Load(dcProxy.OrdenesliqQuery(LiquidacioneSelected.Tipo, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.IDOrden, Program.Usuario, Program.HashConexion), AddressOf TerminotraerOrdenesliq, Nothing)
    End Sub
    Private Function validaciones() As Boolean
        Dim a As Boolean
        a = True
        If LiquidacioneSelected.ID = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar un número de  liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If IsNothing(LiquidacioneSelected.Parcial) Or LiquidacioneSelected.Parcial = -1 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Ingresar un número de  parcial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If (LiquidacioneSelected.CumplimientoTitulo < LiquidacioneSelected.Liquidacion) Or (LiquidacioneSelected.Cumplimiento < LiquidacioneSelected.Liquidacion) Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de cumplimiento debe ser mayor o igual que la de liquidación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacioneSelected.Precio = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El precio no debe ser cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacioneSelected.Cantidad = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La cantidad no debe ser cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If numeroliq > 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La liquidación " + LiquidacioneSelected.ID.ToString + " con el parcial " + LiquidacioneSelected.Parcial.ToString + " y fecha " + LiquidacioneSelected.Liquidacion.ToShortDateString + " ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        If LiquidacioneSelected.IDOrden = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La Orden no debe ser cero", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            a = False
            Return a
        End If
        'If LiquidacioneSelected.Comision > LiquidacioneSelected.Transaccion_cur Then
        '    A2Utilidades.Mensajes.mostrarMensaje("La Comisión no debe ser mayor que el valor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '    a = False
        '    Return a
        'End If
        If (LiquidacioneSelected.Plaza <> "LOC") And (LiquidacioneSelected.Plaza <> String.Empty) And (Not IsNothing(LiquidacioneSelected.Plaza)) Then
            If IsNothing(LiquidacioneSelected.IDComisionistaOtraPlaza) Or LiquidacioneSelected.IDComisionistaOtraPlaza = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el comisionista de la otra plaza", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If

            If IsNothing(LiquidacioneSelected.IDCiudadOtraPlaza) Or LiquidacioneSelected.IDCiudadOtraPlaza = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la ciudad de la otra plaza", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If
        ElseIf LiquidacioneSelected.Plaza = "LOC" Then
            If IsNothing(LiquidacioneSelected.IDComisionistaLocal) Or LiquidacioneSelected.IDComisionistaLocal = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el comisionista local", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If

        End If

        If LiquidacioneSelected.ClaseOrden = "R" Then
            If IsNothing(LiquidacioneSelected.Plazo) Or LiquidacioneSelected.Plazo = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el plazo del título", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                a = False
                Return a
            End If

        End If

        Return a
    End Function
    Private Sub Denominacionespecie()
        dcProxy.verificanombretarifa(LiquidacioneSelected.IDEspecie, "", Program.Usuario, Program.HashConexion, AddressOf TerminoTraernombre, Nothing)
    End Sub
    Private Sub CalcularCostosMonedaOrigen()
        dcProxy.CalculaCostoMonedaOrigen(LiquidacioneSelected.IDBolsa, LiquidacioneSelected.Transaccion_cur, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerCostosMonedaOrigen, Nothing)
    End Sub
    Private Sub CalculosValorNeto()
        dcProxy.CalculaValorNeto(LiquidacioneSelected.Tipo, LiquidacioneSelected.Transaccion_cur, LiquidacioneSelected.ValorCostos, Program.Usuario, Program.HashConexion, AddressOf TerminotraervalorNeto, Nothing)
    End Sub
    Private Sub CalculaValorBruto()
        dcProxy.CalculaValorBruto(LiquidacioneSelected.IDOrden, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.Tipo, LiquidacioneSelected.Transaccion_cur, Program.Usuario, Program.HashConexion, AddressOf Terminotraervalorbruto, Nothing)
    End Sub
    Private Sub Calculacomisionpesos()
        dcProxy.Calculacomisionpesos(LiquidacioneSelected.IDOrden, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.Tipo, LiquidacioneSelected.ValorBruto, Program.Usuario, Program.HashConexion, AddressOf Terminotraercomisionpesos, Nothing)
    End Sub
    Private Sub CalculaIvacomisionpesos()
        dcProxy.CalculaIvacomisionpesos(LiquidacioneSelected.ComisionPesos, Program.Usuario, Program.HashConexion, AddressOf TerminotraerIvacomisionpesos, Nothing)
    End Sub
    Private Sub Calculacostospesos()
        dcProxy.Calculacostospesos(LiquidacioneSelected.IDOrden, LiquidacioneSelected.ClaseOrden, LiquidacioneSelected.Tipo, LiquidacioneSelected.ValorCostos, Program.Usuario, Program.HashConexion, AddressOf Terminotraercostospesos, Nothing)
    End Sub
    Private Sub Calculavalornetopesos()
        dcProxy.Calculavalornetopesos(LiquidacioneSelected.Tipo, LiquidacioneSelected.ValorBruto, LiquidacioneSelected.ComisionPesos, LiquidacioneSelected.ValorIVAComisionPesos, LiquidacioneSelected.ValorCostosPesos, Program.Usuario, Program.HashConexion, AddressOf Terminotraervalorcostosnetopesos, Nothing)
    End Sub
    'Public Sub mostrarmensaje()
    '    A2Utilidades.Mensajes.mostrarMensaje("Los días al vencimiento deben ser menores o iguales que el plazo del título ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    'End Sub


#End Region
#Region "Tablas hijas"
#Region "ReceptoresOrdenes"
    Private _ListaReceptoresOrdenes As EntitySet(Of ReceptoresOrdene_MI)
    Public Property ListaReceptoresOrdenes() As EntitySet(Of ReceptoresOrdene_MI)
        Get
            Return _ListaReceptoresOrdenes
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresOrdene_MI))
            _ListaReceptoresOrdenes = value
            MyBase.CambioItem("ListaReceptoresOrdenes")
        End Set
    End Property

    Private _ReceptoresOrdenesSelected As ReceptoresOrdene_MI
    Public Property ReceptoresOrdenesSelected() As ReceptoresOrdene_MI
        Get
            Return _ReceptoresOrdenesSelected
        End Get
        Set(ByVal value As ReceptoresOrdene_MI)
            If Not value Is Nothing Then
                _ReceptoresOrdenesSelected = value
                MyBase.CambioItem("ReceptoresOrdenesSelected")
            End If
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresOrdenes"
                Dim NewReceptoresOrdenes As New ReceptoresOrdene_MI
                NewReceptoresOrdenes.TipoOrden = LiquidacioneSelected.Tipo
                NewReceptoresOrdenes.ClaseOrden = LiquidacioneSelected.ClaseOrden
                NewReceptoresOrdenes.IDOrden = LiquidacioneSelected.IDOrden
                NewReceptoresOrdenes.Version = 0
                'NewReceptoresOrdenes.IDReceptor = LiquidacioneSelected.PorcRetencionTranslado
                NewReceptoresOrdenes.Usuario = Program.Usuario

                ListaReceptoresOrdenes.Add(NewReceptoresOrdenes)
                ReceptoresOrdenesSelected = NewReceptoresOrdenes
                MyBase.CambioItem("ReceptoresOrdenesSelected")
                MyBase.CambioItem("ListaReceptoresOrdenes")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmReceptoresOrdenes"
                If Not IsNothing(ListaReceptoresOrdenes) Then
                    If Not IsNothing(ListaReceptoresOrdenes) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresOrdenesSelected, ListaReceptoresOrdenes.ToList)

                        ListaReceptoresOrdenes.Remove(_ReceptoresOrdenesSelected)
                        If ListaReceptoresOrdenes.Count > 0 Then
                            Program.PosicionarItemLista(ReceptoresOrdenesSelected, ListaReceptoresOrdenes.ToList, intRegistroPosicionar)
                        Else
                            ReceptoresOrdenesSelected = Nothing
                        End If
                        MyBase.CambioItem("ReceptoresOrdenesSelected")
                        MyBase.CambioItem("ListaReceptoresOrdenes")
                    End If
                End If

        End Select
    End Sub

#End Region
#Region "BeneficiariosOrdenes"
    Private _ListaBeneficiariosOrdenes As EntitySet(Of BeneficiariosOrdene_MI)
    Public Property ListaBeneficiariosOrdenes() As EntitySet(Of BeneficiariosOrdene_MI)
        Get
            Return _ListaBeneficiariosOrdenes
        End Get
        Set(ByVal value As EntitySet(Of BeneficiariosOrdene_MI))
            _ListaBeneficiariosOrdenes = value
            MyBase.CambioItem("ListaBeneficiariosOrdenes")
        End Set
    End Property

    Private _BeneficiariosOrdenesSelected As BeneficiariosOrdene_MI
    Public Property BeneficiariosOrdenesSelected() As BeneficiariosOrdene_MI
        Get
            Return _BeneficiariosOrdenesSelected
        End Get
        Set(ByVal value As BeneficiariosOrdene_MI)
            _BeneficiariosOrdenesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosOrdenesSelected")
            End If
        End Set
    End Property
#End Region
#Region "EspeciesLiquidaciones"
    Private _ListaEspeciesLiquidaciones As EntitySet(Of EspeciesLiquidacione_MI)
    Public Property ListaEspeciesLiquidaciones() As EntitySet(Of EspeciesLiquidacione_MI)
        Get
            Return _ListaEspeciesLiquidaciones
        End Get
        Set(ByVal value As EntitySet(Of EspeciesLiquidacione_MI))
            _ListaEspeciesLiquidaciones = value
            MyBase.CambioItem("ListaEspeciesLiquidaciones")
        End Set
    End Property

    Private _EspeciesLiquidacionesSelected As EspeciesLiquidacione_MI
    Public Property EspeciesLiquidacionesSelected() As EspeciesLiquidacione_MI
        Get
            Return _EspeciesLiquidacionesSelected
        End Get
        Set(ByVal value As EspeciesLiquidacione_MI)
            _EspeciesLiquidacionesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("EspeciesLiquidacionesSelected")
            End If
        End Set
    End Property
#End Region
#Region "AplazamientosLiquidaciones"
    Private _ListaAplazamientosLiquidaciones As EntitySet(Of AplazamientosLiquidacione_MI)
    Public Property ListaAplazamientosLiquidaciones() As EntitySet(Of AplazamientosLiquidacione_MI)
        Get
            Return _ListaAplazamientosLiquidaciones
        End Get
        Set(ByVal value As EntitySet(Of AplazamientosLiquidacione_MI))
            _ListaAplazamientosLiquidaciones = value
            MyBase.CambioItem("ListaAplazamientosLiquidaciones")
        End Set
    End Property

    Private _AplazamientosLiquidacionesSelected As AplazamientosLiquidacione_MI
    Public Property AplazamientosLiquidacionesSelected() As AplazamientosLiquidacione_MI
        Get
            Return _AplazamientosLiquidacionesSelected
        End Get
        Set(ByVal value As AplazamientosLiquidacione_MI)
            _AplazamientosLiquidacionesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("AplazamientosLiquidacionesSelected")
            End If
        End Set
    End Property
#End Region
#Region "CustodiasLiquidaciones"
    Private _ListaCustodiasLiquidaciones As EntitySet(Of CustodiasLiquidacione_MI)
    Public Property ListaCustodiasLiquidaciones() As EntitySet(Of CustodiasLiquidacione_MI)
        Get
            Return _ListaCustodiasLiquidaciones
        End Get
        Set(ByVal value As EntitySet(Of CustodiasLiquidacione_MI))
            _ListaCustodiasLiquidaciones = value
            MyBase.CambioItem("ListaCustodiasLiquidaciones")
        End Set
    End Property

    Private _CustodiasLiquidacionesSelected As CustodiasLiquidacione_MI
    Public Property CustodiasLiquidacionesSelected() As CustodiasLiquidacione_MI
        Get
            Return _CustodiasLiquidacionesSelected
        End Get
        Set(ByVal value As CustodiasLiquidacione_MI)
            _CustodiasLiquidacionesSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("CustodiasLiquidacionesSelected")
            End If
        End Set
    End Property
#End Region
#End Region
End Class
'Clase base para forma de búsquedas
Public Class CamposBusquedaLiquidacione
    Implements INotifyPropertyChanged
    Private _ID As Integer
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
    Private _Parcial As Integer
    <Display(Name:="Parcial")> _
    Public Property Parcial As Integer
        Get
            Return _Parcial
        End Get
        Set(ByVal value As Integer)
            _Parcial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Parcial"))
        End Set
    End Property

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
    Private _IDOrdenante As String
    <Display(Name:="Ordenante")> _
    Public Property IDOrdenante As String
        Get
            Return _IDOrdenante
        End Get
        Set(ByVal value As String)
            _IDOrdenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDOrdenante"))
        End Set
    End Property

    <Display(Name:="Tipo")> _
    Public Property Tipo As String

    <Display(Name:="Clase")> _
    Public Property ClaseOrden As String

    Private _Liquidacion As System.Nullable(Of DateTime)
    <Display(Name:="Fecha Liquidación")> _
    Public Property Liquidacion As System.Nullable(Of DateTime)
        Get
            Return _Liquidacion
        End Get
        Set(value As System.Nullable(Of DateTime))
            _Liquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Liquidacion"))
        End Set
    End Property

    Private _CumplimientoTitulo As System.Nullable(Of DateTime)
    <Display(Name:="Cumplimiento Título")> _
    Public Property CumplimientoTitulo As System.Nullable(Of DateTime)
        Get
            Return _CumplimientoTitulo
        End Get
        Set(value As System.Nullable(Of DateTime))
            _CumplimientoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CumplimientoTitulo"))
        End Set
    End Property

    <Display(Name:="Cumplimiento Titulo")> _
    Public Property DisplayDate As DateTime

    <Display(Name:="Liquidación")> _
    Public Property DisplayDate2 As DateTime
    Private _NumeroOrden As Integer = Now.Year
    <Display(Name:="Orden")> _
    Public Property NumeroOrden As Integer
        Get
            Return _NumeroOrden
        End Get
        Set(ByVal value As Integer)
            _NumeroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden"))
        End Set
    End Property
    Private _NumeroOrden1 As String
    <Display(Name:="")> _
    Public Property NumeroOrden1 As String
        Get
            Return _NumeroOrden1
        End Get
        Set(ByVal value As String)
            _NumeroOrden1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden1"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class Clientesclase
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <Display(Name:=" ")> _
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _NombreCliente As String
    <Display(Name:=" ")> _
    Public Property NombreCliente As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class ClientesOrdenantesclase
    Implements INotifyPropertyChanged

    Private _Ordenante As String
    <Display(Name:=" ")> _
    Public Property Ordenante As String
        Get
            Return _Ordenante
        End Get
        Set(ByVal value As String)
            _Ordenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ordenante"))
        End Set
    End Property

    Private _NombreClienteOrdenante As String
    <Display(Name:=" ")> _
    Public Property NombreClienteOrdenante As String
        Get
            Return _NombreClienteOrdenante
        End Get
        Set(ByVal value As String)
            _NombreClienteOrdenante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreClienteOrdenante"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class Especieclase
    Implements INotifyPropertyChanged

    Private _IDEspecie As String
    <Display(Name:=" ")> _
    Public Property IDEspecie As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecie"))
        End Set
    End Property

    Private _NombreEspecie As String
    <Display(Name:=" ")> _
    Public Property NombreEspecie As String
        Get
            Return _NombreEspecie
        End Get
        Set(ByVal value As String)
            _NombreEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class traslado
    Implements INotifyPropertyChanged

    Private _Traslado As Boolean
    <Display(Name:="Traslado")> _
    Public Property Traslado As Boolean
        Get
            Return _Traslado
        End Get
        Set(ByVal value As Boolean)
            _Traslado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Traslado"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class VeriOrden

    Implements INotifyPropertyChanged
    Private _NumeroOrden As Integer
    Public Property NumeroOrden As Integer
        Get
            Return _NumeroOrden
        End Get
        Set(ByVal value As Integer)
            _NumeroOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden"))
        End Set
    End Property
    Private _NumeroOrden1 As String
    Public Property NumeroOrden1 As String
        Get
            Return _NumeroOrden1
        End Get
        Set(ByVal value As String)
            _NumeroOrden1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroOrden1"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


