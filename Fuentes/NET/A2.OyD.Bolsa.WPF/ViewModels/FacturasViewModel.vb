Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: FacturasViewModel.vb
'Generado el : 06/13/2011 12:09:03
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
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports A2Utilidades.Mensajes

Public Class FacturasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaFactura
    Public Property TotalFactu As New TotalFactura
    Private FacturaPorDefecto As Factura
    Private FacturaAnterior As Factura
    Dim dcProxy As BolsaDomainContext
    Dim dcProxy1 As BolsaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim FechaCierre As DateTime
    Dim intIDRegistroActualizado As Integer = 0
    Dim logRecargarDetalle As Boolean = True
    Dim Observacion As ObservacionFacturas

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
            dcProxy1 = New BolsaDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.FacturasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerFacturaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturasPorDefecto_Completed, "Default")
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  FacturasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "FacturasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerFacturasPorDefecto_Completed(ByVal lo As LoadOperation(Of Factura))
        If Not lo.HasError Then
            FacturaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Factura por defecto", _
                                             Me.ToString(), "TerminoTraerFacturaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerFacturas(ByVal lo As LoadOperation(Of Factura))
        If Not lo.HasError Then
            If lo.UserState = "anular" Or lo.UserState = "insert" Then
                logRecargarDetalle = False
            End If

            ListaFacturas = dcProxy.Facturas

            logRecargarDetalle = True
            If dcProxy.Facturas.Count > 0 Then
                If lo.UserState = "insert" Then
                    FacturaSelected = ListaFacturas.Last
                ElseIf lo.UserState = "anular" Then
                    If ListaFacturas.Where(Function(i) i.IDfacturas = intIDRegistroActualizado).Count > 0 Then
                        FacturaSelected = ListaFacturas.Where(Function(i) i.IDfacturas = intIDRegistroActualizado).First
                    Else
                        If ListaFacturas.Count > 0 Then
                            FacturaSelected = ListaFacturas.First
                        End If
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TotalFactu.Texto = "Total a Favor"
                    TotalFactu.TotalFactura = 0
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Facturas", _
                                             Me.ToString(), "TerminoTraerFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

    Private Sub TerminoTraerLiquidaciones(ByVal lo As LoadOperation(Of Liquidacione))
        If Not lo.HasError Then
            ListaLiquidaciones = dcProxy.Liquidaciones
            If ListaLiquidaciones.Count > 0 Then
                dcProxy.Consultar_TotalFactura(FacturaSelected.Numero, FacturaSelected.Prefijo, 0, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarTotalFactura, "consulta")
            Else
                TotalFactu.Texto = "Total a Favor"
                TotalFactu.TotalFactura = 0
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Liquidaciones", _
                                             Me.ToString(), "TerminoTraerLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' Retorna el valor del Total de la Factura (A Favor o A Cargo)
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20121005</remarks>
    Private Sub TerminoConsultarTotalFactura(ByVal lo As InvokeOperation(Of Double))
        If Not lo.HasError Then
            Dim ValorFactura = lo.Value
            If (ValorFactura >= 0) Then
                TotalFactu.Texto = "Total a Favor"
                TotalFactu.TotalFactura = ValorFactura
            Else
                TotalFactu.Texto = "Total a Cargo"
                TotalFactu.TotalFactura = ValorFactura * -1
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del Total de la Factura", _
                                 Me.ToString(), "TerminoConsultarTotalFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
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

    ''' <summary>
    ''' Se anula correctamente la factura, se cambia el estado de la factura, esto para no recargar la lista. 
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20121005</remarks>
    Private Sub TerminoAnularFactura(ByVal lo As InvokeOperation(Of Boolean))
        Dim strMsg As String = String.Empty
        If Not lo.HasError Then
            If Not IsNothing(_FacturaSelected) Then
                intIDRegistroActualizado = FacturaSelected.IDfacturas
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            FacturaAnterior = Nothing
            dcProxy.Facturas.Clear()
            dcProxy.Load(dcProxy.FacturasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturas, "anular") ' Recarga la lista para que carguen los include
            'FacturaSelected.Estado = "A"
            'EstadoAnulada = True
            'EstadoImpresa = False
        Else
            If lo.Error.Message.ToString.Contains("Debe apagar el parametro ANULACION_FACTURAS_BANCOLOMBIA") Then
                Dim intPosIni As Integer = lo.Error.Message.ToString.IndexOf("Debe apagar ")
                Dim intPosFin As Integer = lo.Error.Message.ToString.IndexOf("|")
                strMsg = lo.Error.Message.ToString.Substring(intPosIni, intPosFin - intPosIni)
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la Factura", _
                                                 Me.ToString(), "TerminoAnularFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaFacturas As EntitySet(Of Factura)
    Public Property ListaFacturas() As EntitySet(Of Factura)
        Get
            Return _ListaFacturas
        End Get
        Set(ByVal value As EntitySet(Of Factura))
            _ListaFacturas = value

            MyBase.CambioItem("ListaFacturas")
            MyBase.CambioItem("ListaFacturasPaged")
            If Not IsNothing(value) Then
                If IsNothing(FacturaAnterior) Then
                    FacturaSelected = _ListaFacturas.FirstOrDefault
                Else
                    FacturaSelected = FacturaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaFacturasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaFacturas) Then
                Dim view = New PagedCollectionView(_ListaFacturas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _FacturaSelected As Factura
    Public Property FacturaSelected() As Factura
        Get
            Return _FacturaSelected
        End Get
        Set(ByVal value As Factura)
            _FacturaSelected = value

            dcProxy.Liquidaciones.Clear()
            If Not value Is Nothing Then
                If logRecargarDetalle Then
                    'FacturaSelected.Prefijo_Numero = FacturaSelected.Prefijo + "-" + Format(FacturaSelected.Numero, "000000")
                    If FacturaSelected.Estado.Equals("A") Then
                        EstadoAnulada = True
                        EstadoImpresa = False
                    ElseIf FacturaSelected.Estado.Equals("I") Then
                        EstadoAnulada = False
                        EstadoImpresa = True
                    End If
                    'dcProxy.Liquidaciones.Clear()
                    dcProxy.Load(dcProxy.Traer_Liquidaciones_FacturaQuery(FacturaSelected.Numero, FacturaSelected.Prefijo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, Nothing)
                End If
            Else
                ListaLiquidaciones = dcProxy.Liquidaciones
            End If
            MyBase.CambioItem("FacturaSelected")
        End Set
    End Property
    Private _EstadoAnulada As Boolean
    Public Property EstadoAnulada As Boolean
        Get
            Return _EstadoAnulada
        End Get
        Set(ByVal value As Boolean)
            _EstadoAnulada = value
            MyBase.CambioItem("EstadoAnulada")
        End Set
    End Property
    Private _EstadoImpresa As Boolean
    Public Property EstadoImpresa As Boolean
        Get
            Return _EstadoImpresa
        End Get
        Set(ByVal value As Boolean)
            _EstadoImpresa = value
            MyBase.CambioItem("EstadoImpresa")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        'MessageBox.Show("Esta funcionalidad no esta habilitada para este maestro", "Funcionalidad", MessageBoxButton.OK)
        A2Utilidades.Mensajes.mostrarMensaje("Está funcionalidad no esta habilitada para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'Try
        '    Dim NewFactura As New Factura
        '    'TODO: Verificar cuales son los campos que deben inicializarse
        '    NewFactura.IDComisionista = FacturaPorDefecto.IDComisionista
        '    NewFactura.IDSucComisionista = FacturaPorDefecto.IDSucComisionista
        '    NewFactura.Prefijo = FacturaPorDefecto.Prefijo
        '    NewFactura.Numero = FacturaPorDefecto.Numero
        '    NewFactura.Comitente = FacturaPorDefecto.Comitente
        '    NewFactura.Fecha_Documento = FacturaPorDefecto.Fecha_Documento
        '    NewFactura.Estado = FacturaPorDefecto.Estado
        '    NewFactura.Estado = FacturaPorDefecto.Estado
        '    NewFactura.Impresiones = FacturaPorDefecto.Impresiones
        '    NewFactura.Actualizacion = FacturaPorDefecto.Actualizacion
        '    NewFactura.Usuario = Program.Usuario
        '    NewFactura.IDCodigoResolucion = FacturaPorDefecto.IDCodigoResolucion
        '    NewFactura.IDfacturas = FacturaPorDefecto.IDfacturas
        '    FacturaAnterior = FacturaSelected
        '    FacturaSelected = NewFactura
        '    MyBase.CambioItem("Facturas")
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
            dcProxy.Facturas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.FacturasFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.FacturasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Numero <> 0 Or cb.Comitente <> String.Empty Or (Not IsNothing(cb.Fecha)) Or cb.NombreComitente <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Facturas.Clear()
                FacturaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Numero = " & cb.Numero.ToString() & " Comitente = " & cb.Comitente.ToString()
                dcProxy.Load(dcProxy.FacturasConsultarQuery(cb.Numero, cb.Comitente, cb.Fecha, System.Web.HttpUtility.UrlEncode(cb.NombreComitente), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaFactura
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
            Dim origen = "update"
            ErrorForma = ""
            FacturaAnterior = FacturaSelected
            If Not ListaFacturas.Contains(FacturaSelected) Then
                origen = "insert"
                ListaFacturas.Add(FacturaSelected)
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

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "BorrarRegistro" Then
                MyBase.QuitarFiltroDespuesGuardar()
                FacturaAnterior = Nothing
                dcProxy.Facturas.Clear()
                dcProxy.Load(dcProxy.FacturasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFacturas, "insert") ' Recarga la lista para que carguen los include

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
        A2Utilidades.Mensajes.mostrarMensaje("Está funcionalidad no esta habilitada para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'If Not IsNothing(_FacturaSelected) Then
        '    Editando = True
        'End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_FacturaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _FacturaSelected.EntityState = EntityState.Detached Then
                    FacturaSelected = FacturaAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            'Se valida que la factura no se encuentre anulada, adicional se verifica antes de anular la factura, como en VB.6 (SLB20121005)
            If Not IsNothing(_FacturaSelected) Then
                If FacturaSelected.Estado.Equals("I") Then
                    'C1.Silverlight.C1MessageBox.Show("¿Esta seguro que desea anular la factura?", "Preacución", C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaAnular)
                    mostrarMensajePregunta("¿Está seguro que desea anular la factura?", _
                                           Program.TituloSistema, _
                                           "BORRARREGISTRO", _
                                           AddressOf TerminoPreguntaAnular, False)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La Factura ya se encuentra anulada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPreguntaAnular(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If validaFechaCierre() Then
                IsBusy = True
                Observacion = New ObservacionFacturas()
                AddHandler Observacion.Closed, AddressOf CerroVentana
                Program.Modal_OwnerMainWindowsPrincipal(Observacion)
                Observacion.ShowDialog()
                'If Not IsNothing(_FacturaSelected) Then
                '    dcProxy.AnularFacturas(FacturaSelected.IDfacturas, FacturaSelected.Numero, FacturaSelected.Prefijo, Program.Usuario, Program.HashConexion, AddressOf TerminoAnularFactura, "anular")
                'dcProxy.Facturas.Remove(_FacturaSelected)
                'FacturaSelected = ListaFacturas.LastOrDefault
                'IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        End If
        'End If
    End Sub

    Private Sub CerroVentana()
        Try

            If Not IsNothing(_FacturaSelected) Then
                dcProxy.AnularFacturas(FacturaSelected.IDfacturas, FacturaSelected.Numero, FacturaSelected.Prefijo, Observacion.strObservaciones, Program.Usuario, Program.HashConexion, AddressOf TerminoAnularFactura, "anular")
                'dcProxy.Facturas.Remove(_FacturaSelected)
                'FacturaSelected = ListaFacturas.LastOrDefault
                IsBusy = False
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas", _
                                 Me.ToString(), "CerroVentana", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Valida la Fecha de Cierre del Sistema
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Private Function validaFechaCierre() As Boolean
        validaFechaCierre = True
        If Format(FacturaSelected.Fecha_Documento.Date, "yyyy/MM/dd") <= Format(FechaCierre, "yyyy/MM/dd") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
            If Format(FechaCierre, "yyyy/MM/dd") <> "1900/01/01" Then
                '    If Format(FacturaSelected.Fecha_Documento.Date, "MM/dd/yyyy") <> Format(FechaCierre, "MM/dd/yyyy") Then
                '        A2Utilidades.Mensajes.mostrarMensaje("La factura con fecha (" & FacturaSelected.Fecha_Documento.Date.ToLongDateString & ") no puede ser anulada porque su fecha no es igual a la fecha abierta para el usuario (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        validaFechaCierre = False
                '    End If
                'Else
                A2Utilidades.Mensajes.mostrarMensaje("La factura con fecha (" & FacturaSelected.Fecha_Documento.Date.ToLongDateString & ") no puede ser anulada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validaFechaCierre = False
            End If
        End If
        Return validaFechaCierre
    End Function

    Public Overrides Sub Buscar()
        cb.Fecha = Nothing
        cb.Numero = Nothing
        cb.Comitente = Nothing
        cb.NombreComitente = Nothing
        MyBase.Buscar()
    End Sub

#End Region

#Region "Tablas hijas"

    '******************************************************** Liquidaciones 
    Private _ListaLiquidaciones As EntitySet(Of Liquidacione)
    Public Property ListaLiquidaciones() As EntitySet(Of Liquidacione)
        Get
            Return _ListaLiquidaciones
        End Get
        Set(ByVal value As EntitySet(Of Liquidacione))
            _ListaLiquidaciones = value
            MyBase.CambioItem("ListaLiquidaciones")
            MyBase.CambioItem("ListaLiquidacionesPaged")
        End Set
    End Property

    Public ReadOnly Property LiquidacionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidaciones) Then
                Dim view = New PagedCollectionView(_ListaLiquidaciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _LiquidacioneSelected As Liquidacione
    Public Property LiquidacioneSelected() As Liquidacione
        Get
            Return _LiquidacioneSelected
        End Get
        Set(ByVal value As Liquidacione)
            _LiquidacioneSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("LiquidacioneSelected")
            End If
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmLiquidacione"
                Dim NewLiquidacione As New Liquidacione
                NewLiquidacione.IDFactura = FacturaSelected.Numero
                ListaLiquidaciones.Add(NewLiquidacione)
                LiquidacioneSelected = NewLiquidacione
                MyBase.CambioItem("LiquidacioneSelected")
                MyBase.CambioItem("ListaLiquidacione")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmLiquidacione"
                If Not IsNothing(ListaLiquidaciones) Then
                    If Not IsNothing(ListaLiquidaciones) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(LiquidacioneSelected, ListaLiquidaciones.ToList)
                        ListaLiquidaciones.Remove(_LiquidacioneSelected)
                        If ListaLiquidaciones.Count > 0 Then
                            Program.PosicionarItemLista(LiquidacioneSelected, ListaLiquidaciones.ToList, intRegistroPosicionar)
                        End If
                        MyBase.CambioItem("LiquidacioneSelected")
                        MyBase.CambioItem("ListaLiquidaciones")
                    End If
                End If

        End Select
    End Sub
#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaFactura
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
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

    Private _Numero As Integer
    <Display(Name:="Número")> _
    Public Property Numero As Integer
        Get
            Return _Numero
        End Get
        Set(value As Integer)
            _Numero = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Numero"))
        End Set
    End Property

    Private _Fecha As DateTime?
    <Display(Name:="F. Documento")> _
    Public Property Fecha As DateTime?
        Get
            Return _Fecha
        End Get
        Set(value As DateTime?)
            _Fecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fecha"))
        End Set
    End Property

    Private _NombreComitente As String
    <Display(Name:="")> _
    Public Property NombreComitente As String
        Get
            Return _NombreComitente
        End Get
        Set(value As String)
            _NombreComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreComitente"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


''' <summary>
''' Clase mas mostrar el Total de la Factura
''' </summary>
''' <remarks>SLB20121005</remarks>
Public Class TotalFactura
    Implements INotifyPropertyChanged

    Private _Texto As String = "Total a Favor"
    Public Property Texto() As String
        Get
            Return _Texto
        End Get
        Set(ByVal value As String)
            _Texto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Texto"))
        End Set
    End Property

    Private _TotalFactura As Double = 0
    Public Property TotalFactura() As Double
        Get
            Return _TotalFactura
        End Get
        Set(ByVal value As Double)
            _TotalFactura = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TotalFactura"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





