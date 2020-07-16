Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: MesasViewModel.vb
'Generado el : 04/20/2011 11:16:04
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

Public Class MesasViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaMesa
	Private MesaPorDefecto As Mesa
	Private MesaAnterior As Mesa
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext
            mdcProxyUtilidad02 = New UtilidadesDomainContext
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.MesasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerMesaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesasPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  MesasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "MesasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerMesasPorDefecto_Completed(ByVal lo As LoadOperation(Of Mesa))
        If Not lo.HasError Then
            MesaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Mesa por defecto", _
                                             Me.ToString(), "TerminoTraerMesaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerMesas(ByVal lo As LoadOperation(Of Mesa))
        If Not lo.HasError Then
            ListaMesas = dcProxy.Mesas
            'Dim q = From e1 In ListaMesas
            'Join e2 In ListaMesas On e1.Ccostos Equals e2.Ccostos
            'Where e1.Ccostos <> ""
            'Select e1.Ccostos, e2.Actualizacion

            If dcProxy.Mesas.Count > 0 Then
                If lo.UserState = "insert" Then
                    MesaSelected = ListaMesas.LastOrDefault
                    End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Mesas", _
                                             Me.ToString(), "TerminoTraerMesa", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.Mesas.Clear()
                    dcProxy.Load(dcProxy.MesasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesas, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaMesas As EntitySet(Of Mesa)
    Public Property ListaMesas() As EntitySet(Of Mesa)
        Get
            Return _ListaMesas
        End Get
        Set(ByVal value As EntitySet(Of Mesa))
            _ListaMesas = value

            MyBase.CambioItem("ListaMesas")
            MyBase.CambioItem("ListaMesasPaged")
            If Not IsNothing(value) Then
                If IsNothing(MesaAnterior) Then
                    MesaSelected = _ListaMesas.FirstOrDefault
                Else
                    MesaSelected = MesaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaMesasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaMesas) Then
                Dim view = New PagedCollectionView(_ListaMesas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _MesaSelected As Mesa
    Public Property MesaSelected() As Mesa
        Get
            Return _MesaSelected
        End Get
        Set(ByVal value As Mesa)
            _MesaSelected = value
            buscarItem("ciudades")
            buscarItem("cuentascontables")
            MyBase.CambioItem("MesaSelected")
        End Set
    End Property

    Private _CiudadesClaseSelected As CiudadesClases2 = New CiudadesClases2
    Public Property CiudadesClaseSelected As CiudadesClases2
        Get
            Return _CiudadesClaseSelected
        End Get
        Set(ByVal value As CiudadesClases2)
            _CiudadesClaseSelected = value
            MyBase.CambioItem("CiudadesClaseSelected")
        End Set
    End Property

    Private _CuentascontablesClasesSelected As CuentascontablesClases = New CuentascontablesClases
    Public Property CuentascontablesClasesSelected As CuentascontablesClases
        Get
            Return _CuentascontablesClasesSelected
        End Get
        Set(ByVal value As CuentascontablesClases)
            _CuentascontablesClasesSelected = value
            MyBase.CambioItem("CuentascontablesClasesSelected")
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
            Dim NewMesa As New Mesa
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewMesa.IDComisionista = MesaPorDefecto.IDComisionista
            NewMesa.IDSucComisionista = MesaPorDefecto.IDSucComisionista
            NewMesa.ID = MesaPorDefecto.ID
            NewMesa.Nombre = MesaPorDefecto.Nombre
            NewMesa.Ccostos = MesaPorDefecto.Ccostos
            NewMesa.CuentaContable = MesaPorDefecto.CuentaContable
            NewMesa.IdPoblacion = MesaPorDefecto.IdPoblacion
            NewMesa.Actualizacion = MesaPorDefecto.Actualizacion
            NewMesa.Usuario = Program.Usuario
            NewMesa.IdMesa = MesaPorDefecto.IdMesa
            MesaAnterior = MesaSelected
            MesaSelected = NewMesa
            MyBase.CambioItem("Mesas")
            Editando = True
            MyBase.CambioItem("Editando")
            CiudadesClaseSelected.Ciudad = String.Empty
            CuentascontablesClasesSelected.CuentaContable = String.Empty
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Mesas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.MesasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.MesasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Mesas.Clear()
                MesaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.MesasConsultarQuery(cb.ID, cb.Nombre,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaMesa
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
            MesaAnterior = MesaSelected
            If Not ListaMesas.Contains(MesaSelected) Then
                origen = "insert"
                ListaMesas.Add(MesaSelected)
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

            'If So.UserState = "insert" Then
            '    dcProxy.Mesas.Clear()
            '    dcProxy.Load(dcProxy.MesasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMesas, "insert") ' Recarga la lista para que carguen los include
            'End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_MesaSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_MesaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _MesaSelected.EntityState = EntityState.Detached Then
                    MesaSelected = MesaAnterior
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
            If Not IsNothing(_MesaSelected) Then
                'dcProxy.Mesas.Remove(_MesaSelected)
                'MesaSelected = _ListaMesas.LastOrDefault
                IsBusy = True
                dcProxy.EliminarMesa(MesaSelected.IdMesa, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.MesaSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "ciudades"


                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then

                            If Not IsNothing(Me.MesaSelected.IdPoblacion) Then
                                strIdItem = Me.MesaSelected.IdPoblacion
                            End If
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not strIdItem.Equals(String.Empty) Then
                            logConsultar = True
                        End If
                        If logConsultar Then
                            mdcProxyUtilidad01.BuscadorGenericos.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        End If

                    Case "cuentascontables"
                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then

                            If Not IsNothing(Me.MesaSelected.CuentaContable) Then
                                strIdItem = Me.MesaSelected.CuentaContable
                            End If
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not strIdItem.Equals(String.Empty) Then
                            logConsultar = True
                        End If
                        If logConsultar Then
                            mdcProxyUtilidad02.BuscadorGenericos.Clear()
                            mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        Else
                            Me.CuentascontablesClasesSelected.CuentaContable = Nothing
                        End If
                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Select Case strTipoItem.ToLower()
                    Case "ciudades"
                        Me.CiudadesClaseSelected.Ciudad = lo.Entities.ToList.Item(0).Nombre
                    Case "cuentascontables"
                        'Me.CuentascontablesClasesSelected.CuentaContable = lo.Entities.ToList.Item(0).Nombre
                        Me.CuentascontablesClasesSelected.CuentaContable = lo.Entities.ToList.Item(0).IdItem
                End Select
            Else
                Select Case strTipoItem.ToLower()
                    Case "ciudades"
                        Me.CiudadesClaseSelected.Ciudad = Nothing
                    Case "cuentascontables"
                        Me.CuentascontablesClasesSelected.CuentaContable = Nothing
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
    End Sub


    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaMesa
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

    



End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaMesa
 	
    <Display(Name:="Código")> _
    Public Property ID As Integer
 	
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String
End Class

Public Class CiudadesClases2
    Implements INotifyPropertyChanged


    Private _Ciudad As String
    <Display(Name:="Ciudad")> _
    Public Property Ciudad As String
        Get
            Return _Ciudad
        End Get
        Set(ByVal value As String)
            _Ciudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ciudad"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class CuentascontablesClases
    Implements INotifyPropertyChanged


    Private _CuentaContable As String
    <Display(Name:="CuentaContable")> _
    Public Property CuentaContable As String
        Get
            Return _CuentaContable
        End Get
        Set(ByVal value As String)
            _CuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaContable"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
