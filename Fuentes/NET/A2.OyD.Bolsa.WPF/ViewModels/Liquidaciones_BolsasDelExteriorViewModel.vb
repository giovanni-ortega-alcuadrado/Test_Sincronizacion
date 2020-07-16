Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.OyDBolsa

Public Class Liquidaciones_BolsasDelExteriorViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private dcProxy As BolsaDomainContext
    Dim dcProxy1 As BolsaDomainContext
    Private objProxy As BolsaDomainContext
    Dim selected As Boolean
    Public Property cb As New CamposLiquidaciones_BolsasDelExterior


#Region "Propiedades"

    Private _ListaLiquidaciones As EntitySet(Of Liquidaciones_BolsasDelExteriorEntidad)
    Public Property ListaLiquidaciones() As EntitySet(Of Liquidaciones_BolsasDelExteriorEntidad)
        Get
            Return _ListaLiquidaciones
        End Get
        Set(ByVal value As EntitySet(Of Liquidaciones_BolsasDelExteriorEntidad))
            _ListaLiquidaciones = value
            'If Not IsNothing(value) Then
            '    LiquidacionSelected = ListaLiquidaciones.FirstOrDefault
            'End If
            MyBase.CambioItem("ListaLiquidaciones")
            MyBase.CambioItem("ListaLiquidacionesPaged")
        End Set
    End Property

    Private WithEvents _LiquidacionSelected As Liquidaciones_BolsasDelExteriorEntidad
    Public Property LiquidacionSelected() As Liquidaciones_BolsasDelExteriorEntidad
        Get
            Return _LiquidacionSelected
        End Get
        Set(ByVal value As Liquidaciones_BolsasDelExteriorEntidad)
            _LiquidacionSelected = value
            If Not IsNothing(LiquidacionSelected) Then
                If Not IsNothing(LiquidacionSelected.IdEspecie) Then
                    Try
                        NombreEspecie = LiquidacionSelected.IdEspecie
                    Catch ex As Exception
                        NombreEspecie = Nothing
                    End Try
                End If

                If Not IsNothing(LiquidacionSelected.IdReceptor) Then
                    Try
                        NombreReceptor = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("RECECUENTAPROPIA").Where(Function(li) li.ID = LiquidacionSelected.IdReceptor).First.Descripcion
                    Catch ex As Exception
                        NombreReceptor = Nothing
                    End Try
                End If
            End If
            MyBase.CambioItem("LiquidacionSelected")
        End Set
    End Property

    Private _LiquidacionAnterior As Liquidaciones_BolsasDelExteriorEntidad
    Public Property LiquidacionAnterior() As Liquidaciones_BolsasDelExteriorEntidad
        Get
            Return _LiquidacionAnterior
        End Get
        Set(ByVal value As Liquidaciones_BolsasDelExteriorEntidad)
            _LiquidacionAnterior = value
            MyBase.CambioItem("LiquidacionAnterior")
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

    Private _logBloqueado As Boolean
    Public Property logBloqueado() As Boolean
        Get
            Return _logBloqueado
        End Get
        Set(ByVal value As Boolean)
            _logBloqueado = value
            MyBase.CambioItem("logBloqueado")
        End Set
    End Property

    Private _NombreEspecie As String
    Public Property NombreEspecie() As String
        Get
            Return _NombreEspecie
        End Get
        Set(ByVal value As String)
            _NombreEspecie = value
            MyBase.CambioItem("NombreEspecie")
        End Set
    End Property

    Private _NombreReceptor As String
    Public Property NombreReceptor() As String
        Get
            Return _NombreReceptor
        End Get
        Set(ByVal value As String)
            _NombreReceptor = value
            MyBase.CambioItem("NombreReceptor")
        End Set
    End Property

    Private _NombreComitente As String
    Public Property NombreComitente() As String
        Get
            Return _NombreComitente
        End Get
        Set(ByVal value As String)
            _NombreComitente = value
            MyBase.CambioItem("NombreComitente")
        End Set
    End Property

    Private _NombreOrdenante As String
    Public Property NombreOrdenante() As String
        Get
            Return _NombreOrdenante
        End Get
        Set(ByVal value As String)
            _NombreOrdenante = value
            MyBase.CambioItem("NombreOrdenante")
        End Set
    End Property
#End Region

#Region "Inicio"
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
            dcProxy1 = New BolsaDomainContext()
            objProxy = New BolsaDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            mdcProxyUtilidad02 = New UtilidadesDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.Liquidaciones_BolsasDelExteriorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Inicial")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Liquidaciones_BolsasDelExteriorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Metodos"
    Public Overrides Sub Filtrar()
        Try
            selected = True
            dcProxy.Liquidaciones_BolsasDelExteriorEntidads.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.Liquidaciones_BolsasDelExteriorFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.Liquidaciones_BolsasDelExteriorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Dim NuevaLiquidacion As New Liquidaciones_BolsasDelExteriorEntidad
            NuevaLiquidacion.Clase = "A"
            NuevaLiquidacion.Liquidacion = Now.Date
            NuevaLiquidacion.Cumplimiento = Now.Date
            NuevaLiquidacion.Usuario = Program.Usuario
            LiquidacionAnterior = LiquidacionSelected
            LiquidacionSelected = NuevaLiquidacion
            Editando = True
            logBloqueado = True
            LiquidacionSelected.IDComitente = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("COMITENTECUENTAPROPIA").FirstOrDefault.ID
            NombreComitente = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("COMITENTECUENTAPROPIA").FirstOrDefault.Descripcion
            LiquidacionSelected.IdOrdenante = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ORDENANTECUENTAPROPIA").FirstOrDefault.ID
            NombreOrdenante = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ORDENANTECUENTAPROPIA").FirstOrDefault.Descripcion
            NombreEspecie = Nothing
            NombreReceptor = Nothing
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            selected = True
            If cb.lngIDLiquidacion <> 0 Or Not IsNothing(cb.dtmLiquidacion) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                IsBusy = True
                dcProxy.Liquidaciones_BolsasDelExteriorEntidads.Clear()
                dcProxy.Load(dcProxy.ConsultarLiquidaciones_BolsasDelExteriorQuery(cb.lngIDLiquidacion, cb.dtmLiquidacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                'IsBusy = True
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " IDComitente = " & cb.IDComitente.ToString()
                'dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, IIf(CamposBusquedaTesoreria.Documento.Year < 1900, Nothing, CamposBusquedaTesoreria.Documento), estadoMC,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                'dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion.Date.Year < 1800, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo.Date.Year < 1800, "1799/01/01", cb.CumplimientoTitulo), cb.IDOrden,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposLiquidaciones_BolsasDelExterior
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
            If validaciones Then
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            LiquidacionAnterior = LiquidacionSelected
            If Not ListaLiquidaciones.Contains(LiquidacionSelected) Then
                origen = "insert"
                ListaLiquidaciones.Add(LiquidacionSelected)
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
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        If Not IsNothing(_LiquidacionSelected) Then
            'Fin Jsoto 2011-08-10
            Editando = True
            logBloqueado = False
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_LiquidacionSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                logBloqueado = False
                'LiquidacionSelected = ListaLiquidaciones.FirstOrDefault
                If _LiquidacionSelected.EntityState = EntityState.Detached Then
                    LiquidacionSelected = LiquidacionAnterior
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
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'Try
        '    selected = True
        '    If Not IsNothing(EspecieSelected) Then
        '        dcProxy.EspeciesRelacionLocalExteriors.Remove(EspecieSelected)
        '        selected = True
        '        EspecieSelected = ListaEspecies.LastOrDefault
        '        IsBusy = True
        '        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '    End If
        'Catch ex As Exception
        '    IsBusy = False
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub

    Public Overrides Sub Buscar()
        'cb.Liquidacion = Nothing
        'cb.CumplimientoTitulo = Nothing
        cb.lngIDLiquidacion = Nothing
        cb.dtmLiquidacion = Nothing
        cb.dtmLiquidacionInicio = Date.Now
        MyBase.Buscar()
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    Private Sub _LiquidacionSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _LiquidacionSelected.PropertyChanged
        Select Case e.PropertyName
            Case "IdReceptor"
                If Not IsNothing(LiquidacionSelected.IdReceptor) Then
                    Try
                        NombreReceptor = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("RECECUENTAPROPIA").Where(Function(li) li.ID = LiquidacionSelected.IdReceptor).First.Descripcion
                    Catch ex As Exception
                        NombreReceptor = Nothing
                    End Try
                End If
            Case "Precio"
                If LiquidacionSelected.Cantidad <> 0 Then
                    LiquidacionSelected.TotalLiq = LiquidacionSelected.Precio * LiquidacionSelected.Cantidad
                End If
            Case "Cantidad"
                If LiquidacionSelected.Precio <> 0 Then
                    LiquidacionSelected.TotalLiq = LiquidacionSelected.Precio * LiquidacionSelected.Cantidad
                End If
        End Select

    End Sub

    Public Function validaciones() As Boolean
        If LiquidacionSelected.Liquidacion > LiquidacionSelected.Cumplimiento Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de cumplimiento no puede ser menor a la de la liquidacion", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        If LiquidacionSelected.Liquidacion Is Nothing Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de liquidación es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        If LiquidacionSelected.Precio = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El precio de la liquidacion es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        If LiquidacionSelected.Cantidad = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("La cantidad de la liquidacion es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        Return False
    End Function
#End Region

#Region "Asincrónico"
    Private Sub TerminoTraerLiquidaciones(ByVal lo As LoadOperation(Of Liquidaciones_BolsasDelExteriorEntidad))
        If Not lo.HasError Then
            IsBusy = False
            ListaLiquidaciones = dcProxy.Liquidaciones_BolsasDelExteriorEntidads
            If ListaLiquidaciones.Count > 0 Then
                If lo.UserState = "insert" Then
                    LiquidacionSelected = ListaLiquidaciones.LastOrDefault
                End If
            ElseIf ListaLiquidaciones.Count <= 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las Liquidaciones", _
                                             Me.ToString(), "TerminoTraerLiquidaciones_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaEmpleados.Remove(EmpleadoSelected)
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

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
            If So.UserState = "insert" Then
                dcProxy.Liquidaciones_BolsasDelExteriorEntidads.Clear()
                dcProxy.Load(dcProxy.Liquidaciones_BolsasDelExteriorFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, So.UserState) ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class

Public Class CamposLiquidaciones_BolsasDelExterior
    Implements INotifyPropertyChanged

    Private _ID As Integer
    <Display(Name:="ID")> _
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _lngIDLiquidacion As Integer
    <Display(Name:="Liquidación")> _
    Public Property lngIDLiquidacion() As Integer
        Get
            Return _lngIDLiquidacion
        End Get
        Set(ByVal value As Integer)
            _lngIDLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDLiquidacion"))
        End Set
    End Property

    Private _dtmLiquidacion As System.Nullable(Of Date)
    <Display(Name:="Fecha de Liquidación")> _
    Public Property dtmLiquidacion() As System.Nullable(Of Date)
        Get
            Return _dtmLiquidacion
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _dtmLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmLiquidacion"))
        End Set
    End Property

    Private _dtmLiquidacionInicio As DateTime
    <Display(Name:="Fecha de Liquidación")> _
    Public Property dtmLiquidacionInicio() As DateTime
        Get
            Return _dtmLiquidacionInicio
        End Get
        Set(ByVal value As DateTime)
            _dtmLiquidacionInicio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmLiquidacionInicio"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
