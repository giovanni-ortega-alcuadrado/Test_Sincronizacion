Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TipoProductoXEspecieViewModel.vb
'Generado el : 12/10/2012 09:15:14
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class TipoProductoXEspecieViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private TipoProductoXEspeciPorDefecto As TipoProductoXEspeci
    Private TipoProductoAnterior As TipoProducto

    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If


            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.TipoProductoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProducto, "INICIO")

                dcProxy1.Load(dcProxy1.TraerTipoProductoXEspeciPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProductoXEspeciePorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "TipoProductoXEspecieViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTipoProducto(ByVal lo As LoadOperation(Of TipoProducto))
        If Not lo.HasError Then
            ListaTipoProducto = dcProxy.TipoProductos.ToList
            If lo.UserState = "INICIO" Then
                ListaTipoProdutoCompleta = dcProxy.TipoProductos.ToList
            End If

            If dcProxy.TipoProductos.Count > 0 Then
                If lo.UserState = "insert" Then
                    _TipoProductoXEspeciSelected = ListaTipoProductoXEspecie.Last
                End If
            Else
                If lo.UserState = "Busqueda" Then
                    mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoProductoXEspecie",
                                             Me.ToString(), "TerminoTraerTipoProductoXEspeci", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaTipoProdutoCompleta As List(Of TipoProducto)
    Public Property ListaTipoProdutoCompleta() As List(Of TipoProducto)
        Get
            Return _ListaTipoProdutoCompleta
        End Get
        Set(ByVal value As List(Of TipoProducto))
            _ListaTipoProdutoCompleta = value
            MyBase.CambioItem("ListaTipoProdutoCompleta")
        End Set
    End Property

    Private _ListaTipoProducto As List(Of TipoProducto)
    Public Property ListaTipoProducto() As List(Of TipoProducto)
        Get
            Return _ListaTipoProducto
        End Get
        Set(ByVal value As List(Of TipoProducto))
            _ListaTipoProducto = value

            MyBase.CambioItem("ListaTipoProducto")
            MyBase.CambioItem("ListaTipoProductoPaged")
            If Not IsNothing(_ListaTipoProducto) Then
                If _ListaTipoProducto.Count > 0 Then
                    TipoProductoSelected = _ListaTipoProducto.First
                Else
                    TipoProductoSelected = Nothing
                End If
            Else
                TipoProductoSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTipoProductoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoProducto) Then
                Dim view = New PagedCollectionView(_ListaTipoProducto)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoProductoSelected As TipoProducto
    Public Property TipoProductoSelected() As TipoProducto
        Get
            Return _TipoProductoSelected
        End Get
        Set(ByVal value As TipoProducto)
            _TipoProductoSelected = value
            If Not IsNothing(_TipoProductoSelected) Then
                ConsultarEspeciesTipoProducto(_TipoProductoSelected.Codigo)
            End If
            MyBase.CambioItem("TipoProductoSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaTipoProductoXEspeci = New CamposBusquedaTipoProductoXEspeci
    Public Property cb() As CamposBusquedaTipoProductoXEspeci
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaTipoProductoXEspeci)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            '          Dim NewTipoProductoXEspeci As New TipoProductoXEspeci
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            'NewTipoProductoXEspeci.ID = TipoProductoXEspeciPorDefecto.ID
            'NewTipoProductoXEspeci.CodigoTipoNegocio = TipoProductoXEspeciPorDefecto.CodigoTipoNegocio
            'NewTipoProductoXEspeci.IDEspecie = TipoProductoXEspeciPorDefecto.IDEspecie
            'NewTipoProductoXEspeci.ValorMaxNegociacion = TipoProductoXEspeciPorDefecto.ValorMaxNegociacion
            'NewTipoProductoXEspeci.Usuario = Program.Usuario
            '      TipoProductoXEspeciAnterior = _TipoProductoXEspeciSelected
            '      _TipoProductoXEspeciSelected = NewTipoProductoXEspeci
            '      MyBase.CambioItem("TipoProductoXEspeciSelected")
            '      Editando = True
            '      MyBase.CambioItem("Editando")

            mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.TipoProductos) Then
                dcProxy.TipoProductos.Clear()
            End If

            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TipoProductoFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProducto, Nothing)
            Else
                dcProxy.Load(dcProxy.TipoProductoFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProducto, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.CodigoTipoProducto <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                If Not IsNothing(dcProxy.TipoProductos) Then
                    dcProxy.TipoProductos.Clear()
                End If
                TipoProductoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " CodigoTipoNegocio = " &  cb.CodigoTipoNegocio.ToString()    'Dic202011 quitar
                Dim TextoFiltroSeguroNombre = System.Web.HttpUtility.UrlEncode(cb.CodigoTipoProducto)
                dcProxy.Load(dcProxy.TipoProductoConsultarQuery(TextoFiltroSeguroNombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProducto, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaTipoProductoXEspeci
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
            Dim logGuardar As Boolean = True

            For Each li In ListaTipoProductoXEspecie
                If ListaTipoProductoXEspecie.Where(Function(i) i.IDEspecie = li.IDEspecie).Count > 1 Then
                    logGuardar = False
                    Exit For
                End If
            Next

            If logGuardar = False Then
                mostrarMensaje("No se puede tener especies repetidas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If logGuardar Then
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "update")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            If Not IsNothing(_TipoProductoSelected) Then
                ConsultarEspeciesTipoProducto(_TipoProductoSelected.Codigo)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        If Not IsNothing(_TipoProductoSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
        End If
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objTipoProducto As New TipoProducto
            If Not IsNothing(_TipoProductoSelected) Then
                objTipoProducto.ID = _TipoProductoSelected.ID
                objTipoProducto.Codigo = _TipoProductoSelected.Codigo
                objTipoProducto.Descripcion = _TipoProductoSelected.Descripcion
            End If
            TipoProductoAnterior = Nothing
            TipoProductoAnterior = objTipoProducto
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.",
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TipoProductoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                TipoProductoSelected = TipoProductoAnterior
            End If
            BorrarEspecieBuscador()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_TipoProductoXEspeciSelected) Then
                Dim objListaEspecies As New List(Of TipoProductoXEspeci)
                'se valida con luis rivera y esta funcionalidad no debe de ir JBT
                Exit Sub

                objListaEspecies = ListaTipoProductoXEspecie.ToList

                For Each li In objListaEspecies
                    ListaTipoProductoXEspecie.Remove(li)
                Next

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaTipoProductoXEspeci
            objCB.CodigoTipoProducto = String.Empty

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta",
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Tipo Producto x Especie"

    Private _ListaTipoProductoXEspecie As EntitySet(Of TipoProductoXEspeci)
    Public Property ListaTipoProductoXEspecie() As EntitySet(Of TipoProductoXEspeci)
        Get
            Return _ListaTipoProductoXEspecie
        End Get
        Set(ByVal value As EntitySet(Of TipoProductoXEspeci))
            _ListaTipoProductoXEspecie = value
            If Not IsNothing(_ListaTipoProductoXEspecie) Then
                If _ListaTipoProductoXEspecie.Count > 0 Then
                    _TipoProductoXEspeciSelected = ListaTipoProductoXEspecie.First
                Else
                    _TipoProductoXEspeciSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTipoProductoXEspecie")
            MyBase.CambioItem("ListaTipoProductoXEspeciePaged")
        End Set
    End Property

    Public ReadOnly Property ListaTipoProductoXEspeciePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoProductoXEspecie) Then
                Dim view = New PagedCollectionView(_ListaTipoProductoXEspecie)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoProductoXEspeciSelected As TipoProductoXEspeci
    Public Property TipoProductoXEspeciSelected() As TipoProductoXEspeci
        Get
            Return _TipoProductoXEspeciSelected
        End Get
        Set(ByVal value As TipoProductoXEspeci)
            _TipoProductoXEspeciSelected = value
            MyBase.CambioItem("TipoProductoXEspeciSelected")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private Sub TerminoTraerTipoProductoXEspeciePorDefecto_Completed(ByVal lo As LoadOperation(Of TipoProductoXEspeci))
        If Not lo.HasError Then
            TipoProductoXEspeciPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoProductoXEspeci por defecto",
                                             Me.ToString(), "TerminoTraerTipoProductoXEspeciPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTipoProductoXEspecie(ByVal lo As LoadOperation(Of TipoProductoXEspeci))
        If Not lo.HasError Then
            ListaTipoProductoXEspecie = dcProxy.TipoProductoXEspecis
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoProductoXEspecie",
                                             Me.ToString(), "TerminoTraerTipoProductoXEspeci", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Public Sub BorrarEspecieBuscador()
        Try
            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar borrar la especie seleccionada.",
             Me.ToString(), "BorrarEspecieBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarEspeciesTipoProducto(ByVal pstrTipoProducto As String)
        Try
            If Not IsNothing(dcProxy.TipoProductoXEspecis) Then
                dcProxy.TipoProductoXEspecis.Clear()
            End If

            dcProxy.Load(dcProxy.TipoProductoXEspecieConsultarQuery(pstrTipoProducto, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProductoXEspecie, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las especies x tipo de producto",
                                            Me.ToString(), "ConsultarEspeciesTipoProducto", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Detalles Maestros del Tipo Producto"

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmTipoProductoXEspecie"
                    Dim newTipoProductoXEspecie As New TipoProductoXEspeci
                    newTipoProductoXEspecie.ID = -New Random().Next()
                    newTipoProductoXEspecie.CodigoTipoProducto = _TipoProductoSelected.Codigo
                    newTipoProductoXEspecie.IDEspecie = TipoProductoXEspeciPorDefecto.IDEspecie
                    newTipoProductoXEspecie.PermiteNegociar = TipoProductoXEspeciPorDefecto.PermiteNegociar
                    newTipoProductoXEspecie.ValorMaxNegociacion = TipoProductoXEspeciPorDefecto.ValorMaxNegociacion
                    newTipoProductoXEspecie.Usuario = Program.Usuario
                    newTipoProductoXEspecie.CrucePorFaciales = True

                    ListaTipoProductoXEspecie.Add(newTipoProductoXEspecie)
                    TipoProductoXEspeciSelected = newTipoProductoXEspecie

                    MyBase.CambioItem("TipoProductoXEspeciSelected")
                    MyBase.CambioItem("ListaTipoProductoXEspecie")
                    MyBase.CambioItem("Editando")

                    BorrarEspecieBuscador()
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar el nuevo detalle.",
                                                         Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmTipoProductoXEspecie"
                    If Not IsNothing(ListaTipoProductoXEspecie) And Not IsNothing(TipoProductoXEspeciSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(TipoProductoXEspeciSelected, ListaTipoProductoXEspecie.ToList)

                        ListaTipoProductoXEspecie.Remove(TipoProductoXEspeciSelected)

                        'TipoProductoXEspeciSelected = ListaTipoProductoXEspecie.LastOrDefault
                        If ListaTipoProductoXEspecie.Count > 0 Then
                            Program.PosicionarItemLista(TipoProductoXEspeciSelected, ListaTipoProductoXEspecie.ToList, intRegistroPosicionar)
                        Else
                            TipoProductoXEspeciSelected = Nothing
                        End If
                        MyBase.CambioItem("TipoProductoXEspeciSelected")
                        MyBase.CambioItem("ListaTipoProductoXEspecie")
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.",
                                                         Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaTipoProductoXEspeci
    Implements INotifyPropertyChanged

    Private _CodigoTipoProducto As String
    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")>
    <Display(Name:="Código tipo negocio", Description:="Código tipo negocio")>
    Public Property CodigoTipoProducto() As String
        Get
            Return _CodigoTipoProducto
        End Get
        Set(ByVal value As String)
            _CodigoTipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoTipoProducto"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




