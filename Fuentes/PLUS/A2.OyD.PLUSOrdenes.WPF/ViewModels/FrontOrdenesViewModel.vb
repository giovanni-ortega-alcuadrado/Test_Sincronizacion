Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2MCCoreWPF
Imports A2OYDPLUSUtilidades
Imports OpenRiaServices.DomainServices.Client

Public Class FrontOrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Inicialización"

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
                mdcProxyUtilidadPLUS = New OYDPLUSUtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidadPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OYDPLUSOrdenesDomainContext.IOYDPLUSOrdenesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(mdcProxyUtilidadPLUS.DomainClient, WebDomainClient(Of OYDPLUSUtilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If System.Diagnostics.Debugger.IsAttached Then

            End If

            '---------------------------------------------------------------------------------------------------------------------
            '-- Definir tipo de orden que manejará el control
            '---------------------------------------------------------------------------------------------------------------------
            If Not Program.IsDesignMode() Then
                ConsultarModulosUsuario()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "FrontOrdenesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Constantes"

    Private Const STR_NOMBREGENERALTAB As String = "tabItemOpcion{0}"

#End Region

#Region "Variables"

    Private dcProxy As OYDPLUSOrdenesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private mdcProxyUtilidadPLUS As OYDPLUSUtilidadesDomainContext

    Public viewFrontOrdenes As FrontOrdenesPLUSView
#End Region

#Region "Propiedades"

    Private _IsBusyFront As Boolean
    Public Property IsBusyFront() As Boolean
        Get
            Return _IsBusyFront
        End Get
        Set(ByVal value As Boolean)
            _IsBusyFront = value
            MyBase.CambioItem("IsBusyFront")
        End Set
    End Property

    Private _TabSeleccionado As Integer = -1
    Public Property TabSeleccionado() As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            If Not IsNothing(_TabSeleccionado) Then
                CargarInformacionTab()
            End If
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _ListaModulosUsuario As List(Of Utilidades_ModulosUsuario)
    Public Property ListaModulosUsuario() As List(Of Utilidades_ModulosUsuario)
        Get
            Return _ListaModulosUsuario
        End Get
        Set(ByVal value As List(Of Utilidades_ModulosUsuario))
            _ListaModulosUsuario = value
            MyBase.CambioItem("ListaModulosUsuario")
        End Set
    End Property

    Private _ModuloSeleccionado As Utilidades_ModulosUsuario
    Public Property ModuloSeleccionado() As Utilidades_ModulosUsuario
        Get
            Return _ModuloSeleccionado
        End Get
        Set(ByVal value As Utilidades_ModulosUsuario)
            _ModuloSeleccionado = value
            MyBase.CambioItem("ModuloSeleccionado")
        End Set
    End Property

#End Region

#Region "Metodos"

    Public Sub ConsultarModulosUsuario()
        Try
            IsBusyFront = True
            If Not IsNothing(dcProxy.ModulosUsuarios) Then
                dcProxy.ModulosUsuarios.Clear()
            End If

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy.Load(dcProxy.cargarModulosUsuarioQuery(Program.CifrarTexto(Program.Aplicacion), Program.CifrarTexto(Program.VersionAplicacion), Program.CifrarTexto(Program.UsuarioWindows), String.Empty, Program.CifrarTexto(Program.Usuario), Program.CifrarTexto(Program.Maquina), Program.HashConexion), AddressOf TerminoConsultarModulosUsuario, String.Empty)
            Else
                dcProxy.Load(dcProxy.cargarModulosUsuarioQuery(Program.CifrarTexto(Program.Aplicacion), Program.CifrarTexto(Program.VersionAplicacion), Program.CifrarTexto(Program.UsuarioAutenticado), Program.CifrarTexto(Program.Password), Program.CifrarTexto(Program.Usuario), Program.CifrarTexto(Program.Maquina), Program.HashConexion), AddressOf TerminoConsultarModulosUsuario, String.Empty)
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los modulos del usuario.", _
                                 Me.ToString(), "ConsultarModulosUsuario", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyFront = False
        End Try
    End Sub

    Public Sub CargarControlesPantalla()
        Try
            If Not IsNothing(viewFrontOrdenes) Then
                Dim intPrimerModulo As Integer = 100

                If Not IsNothing(_ListaModulosUsuario) Then
                    For Each li In _ListaModulosUsuario
                        Dim objTabItem = CType(viewFrontOrdenes.FindName(String.Format(STR_NOMBREGENERALTAB, li.Orden)), System.Windows.Controls.TabItem)

                        If Not IsNothing(objTabItem) Then
                            objTabItem.Header = li.TituloModulo
                            objTabItem.Visibility = Visibility.Visible

                            If li.Orden < intPrimerModulo Then
                                intPrimerModulo = li.Orden
                            End If
                        End If
                    Next

                    TabSeleccionado = intPrimerModulo
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los permitidos de los usuarios.",
                                 Me.ToString(), "CargarControlesPantalla", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusyFront = False
    End Sub

    Private Sub CargarInformacionTab()
        Try
            If Not IsNothing(viewFrontOrdenes) And Not IsNothing(_ListaModulosUsuario) Then
                Dim objModulo As Utilidades_ModulosUsuario = Nothing

                For Each li In _ListaModulosUsuario
                    If li.Orden = TabSeleccionado Then
                        objModulo = li
                        Exit For
                    End If
                Next

                If Not IsNothing(objModulo) Then
                    If objModulo.TieneContenido = False Then
                        Dim objNuevoControl As New ContenidoOpcionPLUSView(objModulo)

                        CType(viewFrontOrdenes.FindName(String.Format(STR_NOMBREGENERALTAB, objModulo.Orden)), System.Windows.Controls.TabItem).Content = objNuevoControl
                        objModulo.TieneContenido = True
                    End If
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los permitidos de los usuarios.", _
                                 Me.ToString(), "CargarControlesPantalla", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados asincronicos"

    Private Sub TerminoConsultarModulosUsuario(ByVal lo As LoadOperation(Of OyDPLUSOrdenes.ModulosUsuario))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim objListaModulos As New List(Of Utilidades_ModulosUsuario)

                    For Each liModulo In lo.Entities.ToList
                        If objListaModulos.Where(Function(i) i.Modulo = liModulo.Modulo).Count = 0 Then
                            Dim objListaBotones As New Dictionary(Of String, A2ControlMenu.BotonMenu)
                            For Each liBoton In lo.Entities.ToList.Where(Function(i) i.Modulo = liModulo.Modulo)
                                If objListaBotones.Where(Function(i) i.Key = liBoton.AccionPermitida).Count = 0 Then
                                    objListaBotones.Add(liBoton.AccionPermitida, New A2ControlMenu.BotonMenu With {.Texto = liBoton.AccionPermitida,
                                                                                                                   .IsVisible = True,
                                                                                                                   .IsEnabled = True,
                                                                                                                   .ToolTip = liBoton.DescripcionAccion})
                                End If
                            Next

                            objListaModulos.Add(New Utilidades_ModulosUsuario With {.Modulo = liModulo.Modulo,
                                                                         .TituloModulo = liModulo.TituloModulo,
                                                                         .TituloVistaModulo = liModulo.TituloVistaModulo,
                                                                         .Orden = liModulo.Prioridad,
                                                                         .CamposControlMenu = objListaBotones})
                        End If
                    Next

                    ListaModulosUsuario = objListaModulos

                    If _ListaModulosUsuario.Count = 0 Then
                        IsBusyFront = False
                        mostrarMensaje("Señor usuario, usted no tiene configurado ningún receptor con permisos en los tipos de negocio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        CargarControlesPantalla()
                    End If
                Else
                    IsBusyFront = False
                    mostrarMensaje("Señor usuario, usted no tiene configurado ningún receptor con permisos en los tipos de negocio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de los modulos del usuario.", _
                                 Me.ToString(), "TerminoConsultarModulosUsuario", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusyFront = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de los modulos del usuario.", _
                                 Me.ToString(), "TerminoConsultarModulosUsuario", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyFront = False
        End Try

    End Sub

#End Region

End Class