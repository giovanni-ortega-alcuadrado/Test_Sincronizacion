Imports A2Utilidades

Partial Public Class upgNavegacion
    Inherits UserControl

    Public Event TerminoCargarVentana(ByVal pstrURLCargada As String, ByVal plogCargoExitoso As Boolean)

#Region "Tipos"
    Private Enum NavegarPor
        PorCancelacion
        PorError
    End Enum
#End Region

#Region "Variables"
    '------------------------------------------------------------------------------------------------------
    '------------------------------------------------------------------------------------------------------
    '-- Declaración de variables
    '------------------------------------------------------------------------------------------------------
    '------------------------------------------------------------------------------------------------------

    '------------------------------------------------------------------------------------------------------
    '-- Otras variables
    '------------------------------------------------------------------------------------------------------
    Private mlogErrorReincidente As Boolean
#End Region

#Region "Propiedades"

    ''' Propiedad urlNavegacionRelativa
    ''' 
    ''' <summary>
    ''' Permite asignar y navegar al URL que corresponde a un control XAML de usuario. 
    ''' También permite conocer el URL asignado.
    ''' </summary>
    ''' <value>URL del control XAML que debe ser cargado. Se asume que es un URL relativo.</value>
    ''' <returns>El URL del control XAML cargado mediante esta página</returns>
    ''' <remarks></remarks>
    Public Property urlNavegacionRelativa() As String
        Get
            'Return (Me.wfrContenido.Source.OriginalString)
            Return (Me.wfrContenido.Source.OriginalString)
        End Get
        Set(ByVal value As String)
            Dim objURI As Uri

            Try
                myBusyIndicator.IsBusy = True
                mlogErrorReincidente = False
                value = ReemplazarURLInterna(value)
                objURI = New Uri(value, UriKind.Relative)

                Task.Run(Function()
                             Dispatcher.Invoke(
                                Function()
                                    Try
                                        If Me.wfrContenido.Navigate(objURI) = False Then
                                            navegarA(NavegarPor.PorCancelacion, value)
                                        End If
                                        Return True
                                    Catch ex As Exception
                                        Mensajes.mostrarErrorAplicacion(Program.Usuario, "La página solicitada generó un error al ser cargada y no se pudo completar su despliegue.", Me.Name, "", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                                        myBusyIndicator.IsBusy = False
                                        Return False
                                    End Try
                                End Function
                                )
                             Return True
                         End Function)
            Catch ex As Exception
                navegarA(NavegarPor.PorError, value)
                myBusyIndicator.IsBusy = False
            End Try

        End Set
    End Property

    ''' Propiedad urlNavegacionAbsoluta
    ''' 
    ''' <summary>
    ''' Permite asignar y navegar al URL que corresponde a un control XAML de usuario. 
    ''' También permite conocer el URL asignado.
    ''' </summary>
    ''' <value>URL del control XAML que debe ser cargado. Se asume que es un URL absoluto.</value>
    ''' <returns>El URL del control XAML cargado mediante esta página</returns>
    ''' <remarks></remarks>
    Public Property urlNavegacionAbsoluta() As String
        Get
            Return (Me.wfrContenido.Source.OriginalString)
        End Get
        Set(ByVal value As String)

            Dim objURI As Uri

            Try
                myBusyIndicator.IsBusy = True
                mlogErrorReincidente = False
                value = ReemplazarURLInterna(value)
                objURI = New Uri(value, UriKind.Absolute)

                Task.Run(Function()
                             Dispatcher.Invoke(Function()
                                                   Try
                                                       If Me.wfrContenido.Navigate(objURI) = False Then
                                                           navegarA(NavegarPor.PorCancelacion, value)
                                                       End If
                                                       Return True
                                                   Catch ex As Exception
                                                       Mensajes.mostrarErrorAplicacion(Program.Usuario, "La página solicitada generó un error al ser cargada y no se pudo completar su despliegue.", Me.Name, "", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                                                       myBusyIndicator.IsBusy = False
                                                       Return False
                                                   End Try
                                               End Function)
                             Return True
                         End Function)


            Catch ex As Exception
                navegarA(NavegarPor.PorError, value)
                myBusyIndicator.IsBusy = False
            End Try

        End Set
    End Property

#End Region

    Public Sub New()
        InitializeComponent()
        AddHandler Me.SizeChanged, AddressOf CambioDePantalla
        Me.ScrollPrincipal.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.98
        If Application.Current.MainWindow.ActualHeight - 100 < 0 Then
            Me.ScrollPrincipal.MaxHeight = 100
        Else
            Me.ScrollPrincipal.MaxHeight = Application.Current.MainWindow.ActualHeight - 100
        End If
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.ScrollPrincipal.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.98
        If Application.Current.MainWindow.ActualHeight - 100 < 0 Then
            Me.ScrollPrincipal.MaxHeight = 100
        Else
            Me.ScrollPrincipal.MaxHeight = Application.Current.MainWindow.ActualHeight - 100
        End If
    End Sub

#Region "Manejadores de eventos"

    ''' <summary>
    ''' Este evento se lanza cuando se presenta un error en la navegación a un URL al utilizar el método Navigate
    ''' del frame.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub wfrContenido_NavigationFailed(ByVal sender As System.Object, ByVal e As System.Windows.Navigation.NavigationFailedEventArgs)
        Dim strMsg As String = "La página solicitada no respondió y no se pudo completar su despliegue."
        Try
            If Not e.Exception Is Nothing Then
                strMsg &= vbNewLine & vbNewLine & e.Exception.Message
            End If

            e.Handled = True

            Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, wppMensajes.TiposMensaje.Personalizado)

            mlogErrorReincidente = True
            myBusyIndicator.IsBusy = False
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "La página solicitada generó un error al ser cargada y no se pudo completar su despliegue.", Me.Name, "", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            myBusyIndicator.IsBusy = False
        End Try
        RaiseEvent TerminoCargarVentana(Me.wfrContenido.Source.OriginalString, False)
    End Sub

    Private Sub wfrContenido_Navigated(sender As Object, e As NavigationEventArgs)
        Try
            myBusyIndicator.IsBusy = False
            RaiseEvent TerminoCargarVentana(Me.wfrContenido.Source.OriginalString, True)
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "La página solicitada generó un error al ser cargada y no se pudo completar su despliegue.", Me.Name, "", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            RaiseEvent TerminoCargarVentana(Me.wfrContenido.Source.OriginalString, False)
        End Try


    End Sub

#End Region

#Region "Métodos privados"

    ''' <summary>
    ''' Este método permite ir a un control que se encarga de presentar un mensaje específico
    ''' </summary>
    ''' <param name="pintNavegarA"></param>
    ''' <param name="pstrURL"></param>
    ''' <remarks></remarks>
    Private Sub navegarA(ByVal pintNavegarA As NavegarPor, ByVal pstrURL As String)
        Dim objURI As Uri
        Dim strURLNav As String = "/Navegacion/upgNavegacionError.xaml"

        Try
            Select Case pintNavegarA
                Case NavegarPor.PorCancelacion
                    strURLNav = "/Navegacion/upgNavegacionCancelacion.xaml"
                Case NavegarPor.PorError
                    strURLNav = "/Navegacion/upgNavegacionError.xaml"
                Case Else
                    strURLNav = "/Navegacion/upgNavegacionError.xaml"
            End Select

            pstrURL = IIf(pstrURL.Trim() = String.Empty, "", "?URL=" & pstrURL).ToString()
            objURI = New Uri(strURLNav & pstrURL, UriKind.Relative)
        Catch ex As Exception
            objURI = New Uri(strURLNav, UriKind.Relative)
        End Try

        Try
            Me.wfrContenido.Navigate(objURI)
        Catch ex As Exception

        End Try
    End Sub

    Private Function ReemplazarURLInterna(ByVal pstrURL As String)
        Dim strUrl As String = pstrURL
        strUrl = strUrl.Replace("ReportViewerSL.xaml", "ReportViewer.xaml")
        strUrl = strUrl.Replace("A2.VisorReportes;component", "A2.VisorReportes.WPF;component")
        strUrl = strUrl.Replace(".SL;component", ".WPF;component")
        Return strUrl
    End Function




#End Region

End Class
