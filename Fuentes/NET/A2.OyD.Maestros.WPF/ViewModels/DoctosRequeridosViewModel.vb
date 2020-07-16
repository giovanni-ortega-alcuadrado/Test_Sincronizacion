Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: DoctosRequeridosViewModel.vb
'Generado el : 01/25/2011 14:23:03
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web

Public Class DoctosRequeridosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaDoctosRequerido
    Private DoctosRequeridoPorDefecto As DoctosRequerido
    Private DoctosRequeridoAnterior As DoctosRequerido

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
                dcProxy.Load(dcProxy.DoctosRequeridosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridos, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerDoctosRequeridoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  DoctosRequeridosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "DoctosRequeridosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerDoctosRequeridosPorDefecto_Completed(ByVal lo As LoadOperation(Of DoctosRequerido))
        If Not lo.HasError Then
            DoctosRequeridoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DoctosRequerido por defecto", _
                                             Me.ToString(), "TerminoTraerDoctosRequeridoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDoctosRequeridos(ByVal lo As LoadOperation(Of DoctosRequerido))
        If Not lo.HasError Then
            ListaDoctosRequeridos = dcProxy.DoctosRequeridos
            If dcProxy.DoctosRequeridos.Count > 0 Then
                If lo.UserState = "insert" Then
                    DoctosRequeridoSelected = ListaDoctosRequeridos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro")
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DoctosRequeridos", _
                                             Me.ToString(), "TerminoTraerDoctosRequerido", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaDoctosRequeridos As EntitySet(Of DoctosRequerido)
    Public Property ListaDoctosRequeridos() As EntitySet(Of DoctosRequerido)
        Get
            Return _ListaDoctosRequeridos
        End Get
        Set(ByVal value As EntitySet(Of DoctosRequerido))
            _ListaDoctosRequeridos = value

            MyBase.CambioItem("ListaDoctosRequeridos")
            MyBase.CambioItem("ListaDoctosRequeridosPaged")
            If Not IsNothing(value) Then
                If IsNothing(DoctosRequeridoAnterior) Then
                    DoctosRequeridoSelected = _ListaDoctosRequeridos.FirstOrDefault
                Else
                    DoctosRequeridoSelected = DoctosRequeridoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDoctosRequeridosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDoctosRequeridos) Then
                Dim view = New PagedCollectionView(_ListaDoctosRequeridos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DoctosRequeridoSelected As DoctosRequerido
    Public Property DoctosRequeridoSelected() As DoctosRequerido
        Get
            Return _DoctosRequeridoSelected
        End Get
        Set(ByVal value As DoctosRequerido)
            _DoctosRequeridoSelected = value
            'If Not value Is Nothing Then
            '    End If
            MyBase.CambioItem("DoctosRequeridoSelected")
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
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewDoctosRequerido As New DoctosRequerido
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewDoctosRequerido.IDDocumento = DoctosRequeridoPorDefecto.IDDocumento
            NewDoctosRequerido.CodigoDocto = DoctosRequeridoPorDefecto.CodigoDocto
            NewDoctosRequerido.NombreDocumento = DoctosRequeridoPorDefecto.NombreDocumento
            NewDoctosRequerido.Requerido = False
            NewDoctosRequerido.FechaIniVigencia = False
            NewDoctosRequerido.FechaFinVigencia = False
            NewDoctosRequerido.DocuActivo = False
            NewDoctosRequerido.Usuario = Program.Usuario
            NewDoctosRequerido.Actualizacion = DoctosRequeridoPorDefecto.Actualizacion
            DoctosRequeridoAnterior = DoctosRequeridoSelected
            DoctosRequeridoSelected = NewDoctosRequerido
            MyBase.CambioItem("DoctosRequeridos")
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
            dcProxy.DoctosRequeridos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.DoctosRequeridosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.DoctosRequeridosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridos, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.CodigoDocto <> String.Empty Or cb.NombreDocumento <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.DoctosRequeridos.Clear()
                DoctosRequeridoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " CodigoDocto = " &  cb.CodigoDocto.ToString() & " NombreDocumento = " &  cb.NombreDocumento.ToString() 
                dcProxy.Load(dcProxy.DoctosRequeridosConsultarQuery(cb.CodigoDocto, cb.NombreDocumento,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaDoctosRequerido
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
            If IsNothing(DoctosRequeridoSelected.Requerido) Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione si el documento es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                Exit Sub
            End If
            If IsNothing(DoctosRequeridoSelected.FechaIniVigencia) Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione si la fecha de inicio de vigencia es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                Exit Sub
            End If
            If IsNothing(DoctosRequeridoSelected.FechaFinVigencia) Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione si la fecha de fin de vigencia es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            'DoctosRequeridoAnterior = DoctosRequeridoSelected
            If Not ListaDoctosRequeridos.Contains(DoctosRequeridoSelected) Then
                origen = "insert"
                ListaDoctosRequeridos.Add(DoctosRequeridoSelected)
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

                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaDoctosRequeridos.Remove(DoctosRequeridoSelected)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                    DoctosRequeridoSelected = ListaDoctosRequeridos.Last
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '	dcProxy.DoctosRequeridos.Clear()
            '      	dcProxy.Load(dcProxy.DoctosRequeridosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridos, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_DoctosRequeridoSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_DoctosRequeridoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _DoctosRequeridoSelected.EntityState = EntityState.Detached Then
                    DoctosRequeridoSelected = DoctosRequeridoAnterior
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
            If Not IsNothing(_DoctosRequeridoSelected) Then
                dcProxy.DoctosRequeridos.Remove(_DoctosRequeridoSelected)
                DoctosRequeridoSelected = _ListaDoctosRequeridos.LastOrDefault()
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("CodigoDocto", 1)
        DicCamposTab.Add("NombreDocumento", 1)
    End Sub
    Public Overrides Sub Buscar()
        cb.CodigoDocto = String.Empty
        cb.NombreDocumento = String.Empty
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaDoctosRequerido
    Implements INotifyPropertyChanged
    Private _CodigoDocto As String
    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
     <Display(Name:="Código")> _
    Public Property CodigoDocto As String
        Get
            Return _CodigoDocto
        End Get
        Set(ByVal value As String)
            _CodigoDocto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoDocto"))
        End Set
    End Property
    Private _NombreDocumento As String
    <StringLength(100, ErrorMessage:="La longitud máxima es de 100")> _
     <Display(Name:="Nombre")> _
    Public Property NombreDocumento As String
        Get
            Return _NombreDocumento
        End Get
        Set(ByVal value As String)
            _NombreDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreDocumento"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




