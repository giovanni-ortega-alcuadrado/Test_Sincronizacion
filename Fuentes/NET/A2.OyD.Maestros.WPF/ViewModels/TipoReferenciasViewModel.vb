Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TipoReferenciasViewModel.vb
'Generado el : 01/26/2011 13:19:15
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
Imports System.Text.RegularExpressions

Public Class TipoReferenciasViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaTipoReferencia
	Private TipoReferenciaPorDefecto As TipoReferencia
    Private TipoReferenciaAnterior As TipoReferencia

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
                dcProxy.Load(dcProxy.TipoReferenciasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferencias, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerTipoReferenciaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferenciasPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  TipoReferenciasViewModel)(Me)
                TipoReferencia.Add(New tipoclasificacion With {.ID = "C", .Descripcion = "Compra"})
                TipoReferencia.Add(New tipoclasificacion With {.ID = "V", .Descripcion = "Venta"})
                TipoReferencia.Add(New tipoclasificacion With {.ID = "A", .Descripcion = "Ambos"})
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TipoReferenciasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTipoReferenciasPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoReferencia))
        If Not lo.HasError Then
            TipoReferenciaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoReferencia por defecto", _
                                             Me.ToString(), "TerminoTraerTipoReferenciaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTipoReferencias(ByVal lo As LoadOperation(Of TipoReferencia))
        If Not lo.HasError Then
            ListaTipoReferencias= dcProxy.TipoReferencias
           	If dcProxy.TipoReferencias.Count > 0 Then
				If lo.UserState = "insert" Then
                    TipoReferenciaSelected = ListaTipoReferencias.Last
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoReferencias", _
                                             Me.ToString(), "TerminoTraerTipoReferencia", Application.Current.ToString(), Program.Maquina, lo.Error)
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
                    dcProxy.TipoReferencias.Clear()
                    dcProxy.Load(dcProxy.TipoReferenciasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferencias, "insert")
                End If
            End If
        End If
        IsBusy = False
    End Sub
	

#End Region

#Region "Propiedades"

    Private _ListaTipoReferencias As EntitySet(Of TipoReferencia)
    Public Property ListaTipoReferencias() As EntitySet(Of TipoReferencia)
        Get
            Return _ListaTipoReferencias
        End Get
        Set(ByVal value As EntitySet(Of TipoReferencia))
            _ListaTipoReferencias = value

            MyBase.CambioItem("ListaTipoReferencias")
            MyBase.CambioItem("ListaTipoReferenciasPaged")
            If Not IsNothing(value) Then
                If IsNothing(TipoReferenciaAnterior) Then
                    TipoReferenciaSelected = _ListaTipoReferencias.FirstOrDefault
                Else
                    TipoReferenciaSelected = TipoReferenciaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTipoReferenciasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoReferencias) Then
                Dim view = New PagedCollectionView(_ListaTipoReferencias)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoReferenciaSelected As TipoReferencia
    Public Property TipoReferenciaSelected() As TipoReferencia
        Get
            Return _TipoReferenciaSelected
        End Get
        Set(ByVal value As TipoReferencia)
            _TipoReferenciaSelected = value
            If Not value Is Nothing Then
				            End If
			MyBase.CambioItem("TipoReferenciaSelected")
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


    Private _habilitar As Boolean = False
    Public Property habilitar As Boolean
        Get
            Return _habilitar
        End Get
        Set(ByVal value As Boolean)
            _habilitar = value
            MyBase.CambioItem("habilitar")
        End Set
    End Property

    Private _TipoReferenciacla As List(Of tipoclasificacion) = New List(Of tipoclasificacion)
    Public Property TipoReferencia As List(Of tipoclasificacion)
        Get

            Return _TipoReferenciacla
        End Get
        Set(ByVal value As List(Of tipoclasificacion))
            _TipoReferenciacla = value
            MyBase.CambioItem("TipoReferencia")
        End Set
    End Property


#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewTipoReferencia As New TipoReferencia
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewTipoReferencia.IDComisionista = TipoReferenciaPorDefecto.IDComisionista
            NewTipoReferencia.IDSucComisionista = TipoReferenciaPorDefecto.IDSucComisionista
            NewTipoReferencia.IDCodigo = TipoReferenciaPorDefecto.IDCodigo
            NewTipoReferencia.IDCodigoRetorno = TipoReferenciaPorDefecto.IDCodigoRetorno
            NewTipoReferencia.Descripcion = TipoReferenciaPorDefecto.Descripcion
            NewTipoReferencia.tipoClasificacion = TipoReferenciaPorDefecto.tipoClasificacion
            NewTipoReferencia.Formulario1 = TipoReferenciaPorDefecto.Formulario1
            NewTipoReferencia.Formulario2 = TipoReferenciaPorDefecto.Formulario2
            NewTipoReferencia.Formulario3 = TipoReferenciaPorDefecto.Formulario3
            NewTipoReferencia.Formulario4 = TipoReferenciaPorDefecto.Formulario4
            NewTipoReferencia.Formulario5 = TipoReferenciaPorDefecto.Formulario5
            NewTipoReferencia.CalculaIVA = TipoReferenciaPorDefecto.CalculaIVA
            NewTipoReferencia.Mensajes = TipoReferenciaPorDefecto.Mensajes
            NewTipoReferencia.CalculaRetencion = TipoReferenciaPorDefecto.CalculaRetencion
            NewTipoReferencia.CantidadNegociada = TipoReferenciaPorDefecto.CantidadNegociada
            NewTipoReferencia.NroMesesDctoTransporte = TipoReferenciaPorDefecto.NroMesesDctoTransporte
            NewTipoReferencia.Consecutivo = TipoReferenciaPorDefecto.Consecutivo
            NewTipoReferencia.Usuario = Program.Usuario
            NewTipoReferencia.Actualizacion = TipoReferenciaPorDefecto.Actualizacion
            TipoReferenciaAnterior = TipoReferenciaSelected
            TipoReferenciaSelected = NewTipoReferencia
            PropiedadTextoCombos = ""
            MyBase.CambioItem("TipoReferencias")
            Editando = True
            MyBase.CambioItem("Editando")
            habilitar = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.TipoReferencias.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TipoReferenciasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferencias, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.TipoReferenciasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferencias, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDCodigoRetorno <> String.Empty Or cb.Descripcion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.TipoReferencias.Clear()
                TipoReferenciaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDCodigoRetorno = " &  cb.IDCodigoRetorno.ToString() & " Descripcion = " &  cb.Descripcion.ToString() 
                dcProxy.Load(dcProxy.TipoReferenciasConsultarQuery(cb.IDCodigoRetorno, cb.Descripcion,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferencias, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaTipoReferencia
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
            Dim regex As Regex = New Regex("[a-z|A-Z|ñ|Ñ]{3}[0-9]{4}[0-9]{2}")

            If Not IsNothing(TipoReferenciaSelected) Then
                If TipoReferenciaSelected.Formulario1 = False And TipoReferenciaSelected.Formulario2 = False And TipoReferenciaSelected.Formulario3 = False And TipoReferenciaSelected.Formulario4 = False And TipoReferenciaSelected.Formulario5 = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debes elegir al menos un formulario.", Program.Usuario, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If Not (regex.IsMatch(TipoReferenciaSelected.Consecutivo)) Then
                A2Utilidades.Mensajes.mostrarMensaje("El consecutivo debe estar conformado por 3 letras y 6 números de los cuales se recomienda que los 2 últimos hagan referencia al año. Ej. abc123411 donde 11 equivale al año 2011.", Program.Usuario, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            TipoReferenciaAnterior = TipoReferenciaSelected
            If Not ListaTipoReferencias.Contains(TipoReferenciaSelected) Then
                origen = "insert"
                ListaTipoReferencias.Add(TipoReferenciaSelected)
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
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        strMsg = Mensaje(0)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                So.MarkErrorAsHandled()

                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                '                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.TipoReferencias.Clear()
            '    dcProxy.Load(dcProxy.TipoReferenciasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoReferencias, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_TipoReferenciaSelected) Then
            Editando = True
            habilitar = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TipoReferenciaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _TipoReferenciaSelected.EntityState = EntityState.Detached Then
                    TipoReferenciaSelected = TipoReferenciaAnterior
                End If
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_TipoReferenciaSelected) Then
                'dcProxy.TipoReferencias.Remove(_TipoReferenciaSelected)
                'TipoReferenciaSelected = _ListaTipoReferencias.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")                
                dcProxy.EliminarTipoReferencias(TipoReferenciaSelected.IDCodigo, TipoReferenciaSelected.Usuario, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
            habilitar = False
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
        DicCamposTab.Add("Descripcion", 1)
        DicCamposTab.Add("IDCodigoRetorno", 1)
        DicCamposTab.Add("Consecutivo", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaTipoReferencia
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
Public Class CamposBusquedaTipoReferencia
 	
    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")> _
     <Display(Name:="Código Retorno")> _
    Public Property IDCodigoRetorno As String
 	
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Descripción")> _
    Public Property Descripcion As String
End Class

Public Class tipoclasificacion
    Public Property ID As String
    Public Property Descripcion As String

End Class




