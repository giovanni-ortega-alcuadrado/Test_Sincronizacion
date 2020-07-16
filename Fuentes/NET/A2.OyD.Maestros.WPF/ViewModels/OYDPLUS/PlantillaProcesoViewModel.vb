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
Imports System.Exception

Public Class PlantillaProcesoViewModel

    Inherits A2ControlMenu.A2ViewModel
    Private PlantillaBancoPorDefecto As PlantillaBanco
    Private PlantillaBancoAnterior As PlantillaBanco
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim IdItemActualizar As Integer = 0

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If


        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.PlantillaBancoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillaBanco, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerPlantillaBancoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillaBancoPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.PlantillasFiltrarQuery(Nothing, Program.Usuario, Program.HashConexion), AddressOf terminoConsultaPlantillas, "terminoConsultarBancos")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PlantillaProcesoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#Region "Propiedades"

    ''' <summary>
    ''' tabla con datos de la plantilla
    ''' </summary>
    ''' <remarks></remarks>
    Private _listaPlantillaBancos As EntitySet(Of PlantillaBanco)
    Public Property ListaPlantillaBancos As EntitySet(Of PlantillaBanco)
        Get
            Return _listaPlantillaBancos
        End Get
        Set(ByVal value As EntitySet(Of PlantillaBanco))
            _listaPlantillaBancos = value
            If Not IsNothing(_listaPlantillaBancos) Then
                If _listaPlantillaBancos.Count > 0 Then
                    _plantillaBancoSelected = _listaPlantillaBancos.FirstOrDefault
                Else
                    _plantillaBancoSelected = Nothing
                End If
            Else
                _plantillaBancoSelected = Nothing
            End If
            MyBase.CambioItem("ListaPlantillaBancos")
            MyBase.CambioItem("ListaPlantillaBancosPaged")
        End Set
    End Property

    ''' <summary>
    ''' Lista paginada para la carga del grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ListaPlantillaBancosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_listaPlantillaBancos) Then
                Dim view = New PagedCollectionView(_listaPlantillaBancos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Registro seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private _plantillaBancoSelected As PlantillaBanco
    Public Property PlantillaBancoSelected As PlantillaBanco
        Get
            Return _plantillaBancoSelected
        End Get
        Set(ByVal value As PlantillaBanco)
            _plantillaBancoSelected = value
            MyBase.CambioItem("PlantillaBancoSelected")
        End Set
    End Property

    ''' <summary>
    ''' Tabla con los datos de los bancos
    ''' </summary>
    ''' <remarks></remarks>
    Private _listaBancos As EntitySet(Of Banco)
    Public Property ListaBancos As EntitySet(Of Banco)
        Get
            Return _listaBancos
        End Get
        Set(ByVal value As EntitySet(Of Banco))
            _listaBancos = value
            MyBase.CambioItem("ListaBancos")
        End Set
    End Property

    ''' <summary>
    ''' Banco seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private _bancoSelected As Banco
    Public Property BancoSelected As Banco
        Get
            Return _bancoSelected
        End Get
        Set(ByVal value As Banco)
            _bancoSelected = value
            If Not IsNothing(value) And Not IsNothing(PlantillaBancoSelected) Then
                PlantillaBancoSelected.strNombre = value.Nombre
                PlantillaBancoSelected.IdBanco = value.IDBanco
                idBanco = value.IDBanco
            End If
            MyBase.CambioItem("BancoSelected")
        End Set
    End Property

    ''' <summary>
    ''' id de banco seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private _idBanco As Integer
    Public Property idBanco() As Integer
        Get
            Return _idBanco
        End Get
        Set(ByVal value As Integer)
            _idBanco = value
            MyBase.CambioItem("idBanco")
        End Set
    End Property

    ''' <summary>
    ''' Tabla con los datos de la tabla de plantillas
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaPlantillas As List(Of Plantilla)
    Public Property ListaPlantillas() As List(Of Plantilla)
        Get
            Return _ListaPlantillas
        End Get
        Set(ByVal value As List(Of Plantilla))
            _ListaPlantillas = value
            MyBase.CambioItem("ListaPlantillas")
        End Set
    End Property

    ''' <summary>
    ''' Plantilla seleccionada
    ''' </summary>
    ''' <remarks></remarks>
    Private _PlantillaSelected As Plantilla
    Public Property PlantillaSelected() As Plantilla
        Get
            Return _PlantillaSelected
        End Get
        Set(ByVal value As Plantilla)
            _PlantillaSelected = value
            If Not IsNothing(value) And Not IsNothing(PlantillaBancoSelected) Then
                PlantillaBancoSelected.strCodigo = value.strCodigo
                PlantillaBancoSelected.IdPlantilla = value.intID
                idPlantilla = value.intID
            End If
            MyBase.CambioItem("PlantillaSelected")
        End Set
    End Property

    Private _idPlantilla As Integer
    Public Property idPlantilla() As Integer
        Get
            Return _idPlantilla
        End Get
        Set(ByVal value As Integer)
            _idPlantilla = value
            MyBase.CambioItem("idPlantilla")
        End Set
    End Property

    Private _cb As CamposBusquedaPlantillaProceso = New CamposBusquedaPlantillaProceso
    Public Property cb() As CamposBusquedaPlantillaProceso
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaPlantillaProceso)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' termina consulta de tabla de plantillas
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerPlantillaBanco(ByVal lo As LoadOperation(Of PlantillaBanco))
        If Not lo.HasError Then
            ListaPlantillaBancos = dcProxy.PlantillaBancos
            If dcProxy.PlantillaBancos.Count > 0 Then
                If lo.UserState = "insert" Then
                    PlantillaBancoSelected = _listaPlantillaBancos.Last
                ElseIf lo.UserState = "update" Then
                    If _listaPlantillaBancos.Where(Function(i) i.Id = IdItemActualizar).Count > 0 Then
                        PlantillaBancoSelected = _listaPlantillaBancos.Where(Function(i) i.Id = IdItemActualizar).FirstOrDefault
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
            dcProxy.Load(dcProxy.BancosFiltrarQuery(Nothing, Program.Usuario, Program.HashConexion), AddressOf terminoConsultarBancos, "TerminoTraerPlantillaBanco")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de plantillas por proceso", _
                                             Me.ToString(), "TerminoTraerPlantillaBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Termina consulta de plantilla por defecto
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerPlantillaBancoPorDefecto_Completed(ByVal lo As LoadOperation(Of PlantillaBanco))
        If Not lo.HasError Then
            PlantillaBancoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la plantilla por proceso por defecto", _
                                             Me.ToString(), "TerminoTraerPlantillaBancoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub terminoConsultarBancos(ByVal lo As LoadOperation(Of Banco))
        If Not lo.HasError Then
            ListaBancos = dcProxy.Bancos
            If dcProxy.BancosNacionales.Count > 0 Then
                'BancoSelected = ListaBancos.Last
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de bancos", _
                                             Me.ToString(), "terminoConsultarBancos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub

    Private Sub terminoConsultaPlantillas(ByVal lo As LoadOperation(Of Plantilla))
        If Not lo.HasError Then
            ListaPlantillas = lo.Entities.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de plantillas por proceso", _
                                             Me.ToString(), "TerminoTraerPlantillaBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Nueva plantilla
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NuevaPlantillaBanco As New PlantillaBanco
            'TODO: Verificar cuales son los campos que deben inicializarse
            NuevaPlantillaBanco.strUsuario = Program.Usuario
            ObtenerRegistroAnterior()
            PlantillaBancoSelected = NuevaPlantillaBanco
            MyBase.CambioItem("PlantillaBancoSelected")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta de plantilla con filtro
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub Filtrar()
        Try
            dcProxy.PlantillaBancos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.PlantillaBancoFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillaBanco, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.PlantillaBancoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillaBanco, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' consulta de plantillas
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ConfirmarBuscar()
        Try
            If _cb.Banco <> 0 Or _cb.Plantilla <> 0 Or _cb.Descripcion <> String.Empty Or _cb.Extension <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.PlantillaBancos.Clear()
                PlantillaBancoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.PlantillaBancoConsultarQuery(0, _cb.Banco, _cb.Plantilla, _cb.Descripcion, _cb.Extension, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillaBanco, "Busqueda")
                MyBase.ConfirmarBuscar()
                PrepararNuevaBusqueda()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Actulización de plantillas
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            PlantillaBancoAnterior = PlantillaBancoSelected
            If Not ListaPlantillaBancos.Contains(PlantillaBancoSelected) Then
                origen = "insert"
                ListaPlantillaBancos.Add(PlantillaBancoSelected)
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
    ''' Termina actualización de datos de plantillas
    ''' </summary>
    ''' <param name="So"></param>
    ''' <remarks></remarks>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "BorrarRegistro") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            If So.UserState = "update" Then
                IdItemActualizar = _plantillaBancoSelected.Id
            End If

            If Not IsNothing(dcProxy.PlantillaBancos) Then
                dcProxy.PlantillaBancos.Clear()
            End If

            MyBase.QuitarFiltroDespuesGuardar()

            dcProxy.Load(dcProxy.PlantillaBancoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillaBanco, So.UserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' indica si está en modo de edición
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_plantillaBancoSelected) Then
            Editando = True
            ObtenerRegistroAnterior()
            'HabilitarDeshabilitarControles(_ControlHorarioSelected.Modulo)
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    ''' <summary>
    ''' devuelve los cambios realizados
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_plantillaBancoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                PlantillaBancoSelected = PlantillaBancoAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' elimina un registro de plantilla
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_plantillaBancoSelected) Then
                dcProxy.PlantillaBancos.Remove(_plantillaBancoSelected)
                PlantillaBancoSelected = _listaPlantillaBancos.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ' ''' <summary>
    ' ''' Se cancela la vista de búsqueda
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Overrides Sub CancelarBuscar()
    '    Try
    '        cb = New CamposBusquedaPlantillaProceso
    '        CambioItem("cb")
    '        MyBase.CancelarBuscar()
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la búsqueda", _
    '                 Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            'Dim objcb As New CamposBusquedaPlantillaProceso
            cb.Descripcion = String.Empty
            cb.Extension = String.Empty
            cb.Banco = 0
            cb.Plantilla = 0

            MyBase.CambioItem("cb")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

    Private Sub ObtenerRegistroAnterior()
        Try
            Dim objPlantillaBanco As New PlantillaBanco
            If Not IsNothing(_plantillaBancoSelected) Then
                objPlantillaBanco.Id = _plantillaBancoSelected.Id
                objPlantillaBanco.IdBanco = _plantillaBancoSelected.IdBanco
                objPlantillaBanco.IdPlantilla = _plantillaBancoSelected.IdPlantilla
                objPlantillaBanco.strCodigo = _plantillaBancoSelected.strCodigo
                objPlantillaBanco.strDescripcion = _plantillaBancoSelected.strDescripcion
                objPlantillaBanco.strExtension = _plantillaBancoSelected.strExtension
                objPlantillaBanco.strNombre = _plantillaBancoSelected.strNombre
                objPlantillaBanco.strUsuario = _plantillaBancoSelected.strUsuario
            End If

            PlantillaBancoAnterior = Nothing
            PlantillaBancoAnterior = objPlantillaBanco
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaPlantillaProceso
    Implements INotifyPropertyChanged

    Private _Descripcion As String
    <Display(Name:="Proceso")> _
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Private _Extension As String
    <Display(Name:="Extensión (tipo de documento)")> _
    Public Property Extension() As String
        Get
            Return _Extension
        End Get
        Set(ByVal value As String)
            _Extension = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Extension"))
        End Set
    End Property

    Private _Banco As Integer
    <Display(Name:="Banco")> _
    Public Property Banco() As Integer
        Get
            Return _Banco
        End Get
        Set(ByVal value As Integer)
            _Banco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Banco"))
        End Set
    End Property

    Private _Plantilla As Integer
    <Display(Name:="Plantilla")> _
    Public Property Plantilla() As Integer
        Get
            Return _Plantilla
        End Get
        Set(ByVal value As Integer)
            _Plantilla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Plantilla"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

