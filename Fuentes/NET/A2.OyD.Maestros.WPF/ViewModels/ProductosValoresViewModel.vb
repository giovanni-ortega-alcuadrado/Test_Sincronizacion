Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ProductosValoresViewModel.vb
'Generado el : 01/25/2011 11:38:47
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

Public Class ProductosValoresViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaProductosValore
    Private ProductosValorePorDefecto As ProductosValore
    Private ProductosValoreAnterior As ProductosValore

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
                dcProxy.Load(dcProxy.ProductosValoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProductosValores, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerProductosValorePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProductosValoresPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ProductosValoresViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerProductosValoresPorDefecto_Completed(ByVal lo As LoadOperation(Of ProductosValore))
        If Not lo.HasError Then
            ProductosValorePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ProductosValore por defecto",
                                             Me.ToString(), "TerminoTraerProductosValorePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerProductosValores(ByVal lo As LoadOperation(Of ProductosValore))
        If Not lo.HasError Then
            ListaProductosValores = dcProxy.ProductosValores
            If dcProxy.ProductosValores.Count > 0 Then
                If lo.UserState = "insert" Then
                    ProductosValoreSelected = ListaProductosValores.Last
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ProductosValores",
                                             Me.ToString(), "TerminoTraerProductosValore", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaProductosValores As EntitySet(Of ProductosValore)
    Public Property ListaProductosValores() As EntitySet(Of ProductosValore)
        Get
            Return _ListaProductosValores
        End Get
        Set(ByVal value As EntitySet(Of ProductosValore))
            _ListaProductosValores = value

            MyBase.CambioItem("ListaProductosValores")
            MyBase.CambioItem("ListaProductosValoresPaged")
            If Not IsNothing(value) Then
                If IsNothing(ProductosValoreAnterior) Then
                    ProductosValoreSelected = _ListaProductosValores.FirstOrDefault
                Else
                    ProductosValoreSelected = ProductosValoreAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaProductosValoresPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaProductosValores) Then
                Dim view = New PagedCollectionView(_ListaProductosValores)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ProductosValoreSelected As ProductosValore
    Public Property ProductosValoreSelected() As ProductosValore
        Get
            Return _ProductosValoreSelected
        End Get
        Set(ByVal value As ProductosValore)
            _ProductosValoreSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("ProductosValoreSelected")
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
            Dim NewProductosValore As New ProductosValore
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewProductosValore.IDTipoProducto = ProductosValorePorDefecto.IDTipoProducto
            NewProductosValore.Descripcion = ProductosValorePorDefecto.Descripcion
            NewProductosValore.Orden = ProductosValorePorDefecto.Orden
            NewProductosValore.Usuario = Program.Usuario
            NewProductosValore.Actualizacion = ProductosValorePorDefecto.Actualizacion
            NewProductosValore.IdProductoValores = ProductosValorePorDefecto.IdProductoValores
            ProductosValoreAnterior = ProductosValoreSelected
            ProductosValoreSelected = NewProductosValore
            MyBase.CambioItem("ProductosValores")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ProductosValores.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ProductosValoresFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProductosValores, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ProductosValoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProductosValores, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.IDTipoProducto) Or cb.Descripcion <> String.Empty Or Not IsNothing(cb.Orden) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                If IsNothing(cb.IDTipoProducto) Then
                    cb.IDTipoProducto = -1
                End If
                If IsNothing(cb.Orden) Then
                    cb.Orden = -1
                End If
                dcProxy.ProductosValores.Clear()
                ProductosValoreAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDTipoProducto = " &  cb.IDTipoProducto.ToString() & " Descripcion = " &  cb.Descripcion.ToString() & " Orden = " &  cb.Orden.ToString() 
                dcProxy.Load(dcProxy.ProductosValoresConsultarQuery(cb.IDTipoProducto, cb.Descripcion, cb.Orden, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProductosValores, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaProductosValore
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            ProductosValoreAnterior = ProductosValoreSelected
            If Not ListaProductosValores.Contains(ProductosValoreSelected) Then
                origen = "insert"
                ListaProductosValores.Add(ProductosValoreSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As OpenRiaServices.DomainServices.Client.SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '	dcProxy.ProductosValores.Clear()
            '      	dcProxy.Load(dcProxy.ProductosValoresFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerProductosValores, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ProductosValoreSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ProductosValoreSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ProductosValoreSelected.EntityState = EntityState.Detached Then
                    ProductosValoreSelected = ProductosValoreAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ProductosValoreSelected) Then
                dcProxy.ProductosValores.Remove(_ProductosValoreSelected)
                ProductosValoreSelected = _ListaProductosValores.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
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
        DicCamposTab.Add("Descripción", 1)
    End Sub

    Public Overrides Sub Buscar()
        cb.Descripcion = String.Empty
        cb.Orden = Nothing
        cb.IDTipoProducto = Nothing

        MyBase.Buscar()
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaProductosValore
    Implements INotifyPropertyChanged


    Private _IDTipoProducto As Nullable(Of Integer)
    <Display(Name:="Código")>
    Public Property IDTipoProducto() As Nullable(Of Integer)
        Get
            Return _IDTipoProducto
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDTipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDTipoProducto"))
        End Set
    End Property

    Private _Descripcion As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Descripción")>
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property


    Private _Orden As Nullable(Of Integer)
    <Display(Name:="Orden")>
    Public Property Orden() As Nullable(Of Integer)
        Get
            Return _Orden
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Orden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Orden"))
        End Set

    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class




