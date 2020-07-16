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
Imports System.Exception
Imports C1.Silverlight.RichTextBox

Public Class PlantillasViewModel

    Inherits A2ControlMenu.A2ViewModel
    Private PlantillPorDefecto As Plantilla
    Private PlantillAnterior As Plantilla
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

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
                dcProxy.Load(dcProxy.PlantillasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerPlantillaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillasPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PlantillasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Propiedades"

    ''' <summary>
    ''' Tabla con los datos de la tabla de plantillas
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaPlantillas As EntitySet(Of Plantilla)
    Public Property ListaPlantillas() As EntitySet(Of Plantilla)
        Get
            Return _ListaPlantillas
        End Get
        Set(ByVal value As EntitySet(Of Plantilla))
            _ListaPlantillas = value
            If Not IsNothing(_ListaPlantillas) Then
                If _ListaPlantillas.Count > 0 Then
                    PlantillaSelected = _ListaPlantillas.FirstOrDefault
                Else
                    PlantillaSelected = Nothing
                End If
            Else
                PlantillaSelected = Nothing
            End If
            MyBase.CambioItem("ListaPlantillas")
            MyBase.CambioItem("ListaPlantillasPaged")
        End Set
    End Property

    ''' <summary>
    ''' Misma lista de plantillas pero lista para el páginado en el grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ListaPlantillasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPlantillas) Then
                Dim view = New PagedCollectionView(_ListaPlantillas)
                Return view
            Else
                Return Nothing
            End If
        End Get
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
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("PlantillaSelected")
        End Set
    End Property

    ''' <summary>
    ''' Lista de metapalabras
    ''' </summary>
    ''' <remarks></remarks>
    Private _metaPalabras As EntitySet(Of tblMetapalabras)
    Public Property MetaPalabras() As EntitySet(Of tblMetapalabras)
        Get
            Return _metaPalabras
        End Get
        Set(ByVal value As EntitySet(Of tblMetapalabras))
            _metaPalabras = value
        End Set
    End Property

    ''' <summary>
    ''' Listado de metapalabras que se puede filtrar por sitema
    ''' </summary>
    ''' <remarks></remarks>
    Private _lstmetapalabras As List(Of tblMetapalabras)
    Public Property lstMetaPalabras() As List(Of tblMetapalabras)
        Get
            Return _lstmetapalabras
        End Get
        Set(ByVal value As List(Of tblMetapalabras))
            _lstmetapalabras = value
            MyBase.CambioItem("lstMetaPalabras")
        End Set
    End Property

    ''' <summary>
    ''' Sistemas para el filtrado de las metapalbras
    ''' </summary>
    ''' <remarks></remarks>
    Private _sistemas As List(Of Sistema)
    Public Property sistemas() As List(Of Sistema)
        Get
            Return _sistemas
        End Get
        Set(ByVal value As List(Of Sistema))
            _sistemas = value
            MyBase.CambioItem("sistemas")
        End Set
    End Property

    ''' <summary>
    ''' Sistema seleccionado
    ''' </summary>
    ''' <remarks></remarks>
    Private _sistema As String
    Public Property Sistema() As String
        Get
            Return _sistema
        End Get
        Set(ByVal value As String)
            _sistema = value
            FiltrarMetapalabras()
            MyBase.CambioItem("Sistema")
        End Set
    End Property

    Private _cb As CamposBusquedaPlantilla = New CamposBusquedaPlantilla
    Public Property cb() As CamposBusquedaPlantilla
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaPlantilla)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEdicion As Boolean
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property


#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' termina consulta de tabla de plantillas
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerPlantillas(ByVal lo As LoadOperation(Of Plantilla))
        If Not lo.HasError Then
            ListaPlantillas = dcProxy.Plantillas
            If dcProxy.Plantillas.Count > 0 Then
                If lo.UserState = "insert" Then
                    PlantillaSelected = ListaPlantillas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
            dcProxy.Load(dcProxy.MetapalabrasConsultarQuery(Nothing,Program.Usuario, Program.HashConexion), AddressOf terminoConsultarMetapalabras, "TerminoTraerPlantillas")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Plantillas", _
                                             Me.ToString(), "TerminoTraerPlantillas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Termina consulta de plantilla por defecto
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub TerminoTraerPlantillasPorDefecto_Completed(ByVal lo As LoadOperation(Of Plantilla))
        If Not lo.HasError Then
            PlantillPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la plantilla por defecto", _
                                             Me.ToString(), "TerminoTraerPlantillasPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <summary>
    ''' termina consulta de tabla de palabras clave
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Private Sub terminoConsultarMetapalabras(ByVal lo As LoadOperation(Of tblMetapalabras))
        If Not lo.HasError Then
            MetaPalabras = dcProxy.tblMetapalabras
            Dim q = (From mp In MetaPalabras Select mp.strSistema Distinct)
            sistemas = (From l In q Select New Sistema(l.ToString)).ToList
            Sistema = sistemas.FirstOrDefault.Nombre
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las palabras clave", _
                                             Me.ToString(), "terminoConsultarMetapalabras", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            dcProxy.Plantillas.Remove(_PlantillaSelected)
                            PlantillaSelected = _ListaPlantillas.LastOrDefault
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                        Else
                            IsBusy = False
                            A2Utilidades.Mensajes.mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "insert")
                        Else
                            IsBusy = False
                            A2Utilidades.Mensajes.mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Nueva plantilla
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NuevaPlantilla As New Plantilla
            'TODO: Verificar cuales son los campos que deben inicializarse
            NuevaPlantilla.strMensaje = "Texto para la plantilla."
            PlantillAnterior = PlantillaSelected
            PlantillaSelected = NuevaPlantilla
            MyBase.CambioItem("Plantillas")
            Editando = True
            HabilitarEdicion = True
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
            dcProxy.Plantillas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.PlantillasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.PlantillasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillas, "Filtrar")
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
            If (Not IsNothing(cb.ID) And cb.ID <> 0) Or cb.Codigo <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Plantillas.Clear()
                PlantillAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.PlantillasConsultarQuery(cb.ID, cb.Codigo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPlantillas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaPlantilla
                CambioItem("cb")
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
            PlantillAnterior = PlantillaSelected

            ' transforma el html para que los estilos queden en las etiquetas y no en el head
                        'Dim rcgtxt As C1RichTextBox = New C1RichTextBox
            'rcgtxt.Html = PlantillaSelected.strMensaje
            'PlantillaSelected.strMensaje = rcgtxt.GetHtml(Documents.HtmlEncoding.Inline)

            If Not ListaPlantillas.Contains(PlantillaSelected) Then
                origen = "insert"
                ListaPlantillas.Add(PlantillaSelected)
            End If

            IsBusy = True

            If origen = "insert" Then
                If Not IsNothing(dcProxy.ValidacionEliminarRegistros) Then
                    dcProxy.ValidacionEliminarRegistros.Clear()
                End If

                dcProxy.Load(dcProxy.ValidarDuplicidadRegistroQuery("Notificacion.tblPlantillas", "'strCodigo'", String.Format("'{0}'", _PlantillaSelected.strCodigo), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
            Else
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If

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
            HabilitarEdicion = False

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
        If Not IsNothing(_PlantillaSelected) Then
            Editando = True
            HabilitarEdicion = False
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
            If Not IsNothing(_PlantillaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                HabilitarEdicion = False
                PlantillaSelected = PlantillAnterior
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
            If Not IsNothing(_PlantillaSelected) Then
                IsBusy = True
                If Not IsNothing(dcProxy.ValidacionEliminarRegistros) Then
                    dcProxy.ValidacionEliminarRegistros.Clear()
                End If

                dcProxy.Load(dcProxy.ValidarEliminarRegistroQuery("'Notificacion.tblPlantillaBanco'|'Notificacion.tblMensajesProcesos'|'Notificacion.tblMensajesProcesos'", "'IdPlantilla'|'intIDPlantilla1'|'intIDPlantilla2'", String.Format("'{0}'|'{0}'|'{0}'", _PlantillaSelected.intID), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ELIMINAR")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se cancela la vista de búsqueda
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaPlantilla
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filtra palabras clave, al cambiar la selección del combo de sistema
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FiltrarMetapalabras()
        lstMetaPalabras = _metaPalabras.Where(Function(i) i.strSistema = _sistema).ToList
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaPlantilla
            objCB.ID = 0
            objCB.Codigo = String.Empty

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaPlantilla
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


    Private _Codigo As String
    <Display(Name:="Código")> _
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

''' <summary>
''' Clase para carga de sistemas
''' </summary>
''' <remarks></remarks>
Public Class Sistema
    Public Sub New(strSistema As String)
        _strNombre = strSistema
        _strDescripcion = strSistema
    End Sub

    Private _strNombre As String
    Public Property Nombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
        End Set
    End Property
    Private _strDescripcion As String
    Public Property Descripcion() As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
        End Set
    End Property
End Class

