Imports System

Partial Public Class ucBtnAyuda
    Inherits UserControl

    Public Sub New()
        InitializeComponent()

    End Sub

#Region "Eventos"

    Public Event Clic_Presionado(sender As Object, e As EventArgs)

    Private Sub btn_Click(sender As Object, e As System.Windows.RoutedEventArgs)

        Try
            Program.VisorArchivosWeb_CargarURL(URLAyuda)

            'Dim op As New System.Windows.Browser.HtmlPopupWindowOptions
            'op.Toolbar = False
            'op.Scrollbars = False
            'op.Resizeable = True
            'op.Location = False
            'op.Status = False
            'op.Menubar = False
            'op.Directories = False
            'op.Height = CInt(_bolAltoVentanaAyuda)
            'op.Width = CInt(_bolAnchoVentanaAyuda)
            'Windows.Browser.HtmlPage.PopupWindow(New Uri(URLAyuda), "_blank", op)

            'Dim ucPopup As New ucPopupAyuda(URLAyuda)
            'ucPopup.Height = _bolAltoVentanaAyuda
            'ucPopup.Width = _bolAnchoVentanaAyuda
            'ucPopup.Show()

            RaiseEvent Clic_Presionado(sender, e)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "EL error se presenta cuando se intenta mostrar la ayuda", "ucBtnAyuda", "btn_Click", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region
#Region "Propiedades"

    Public ReadOnly Property URLAyuda As String
        Get
            Return String.Format("{0}{1}", Program.ClaveEspecifica(_strAyuda), _strTopicoHtm)
        End Get
    End Property

    ''' <summary>
    ''' Esta es la ruta web donde se encuentra el sitio de ayudas, ejemplo: http://muservidor/sitioWeb/CarpetaAyudas/Documents
    ''' </summary>
    ''' <remarks></remarks>
    Private _strAyuda As String
    Public Property Ayuda() As String
        Get
            Return _strAyuda
        End Get
        Set(ByVal value As String)
            _strAyuda = value
        End Set
    End Property

    ''' <summary>
    ''' Página web htm del topico especifico, ejemplo: contenido.htm, al tomar el resultado concatena el valor de la propiedad Ayuda con esta y el resultado seria http://muservidor/sitioWeb/CarpetaAyudas/Documents + / + contenido.htm
    ''' </summary>
    ''' <remarks></remarks>
    Private _strTopicoHtm As String
    Public Property TopicoHtm() As String
        Get
            Return _strTopicoHtm
        End Get
        Set(ByVal value As String)
            _strTopicoHtm = value
        End Set
    End Property

    Private _bolAnchoVentanaAyuda As Double
    ''' <summary>
    ''' Permite establecer el ancho para la ventana donde se muestra la ayuda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AnchoVentanaAyuda() As Double
        Get
            Return _bolAnchoVentanaAyuda
        End Get
        Set(ByVal value As Double)
            _bolAnchoVentanaAyuda = value
        End Set
    End Property

    Private _bolAltoVentanaAyuda As Double
    ''' <summary>
    ''' Permite establecer el alto para la ventana donde se muestra la ayuda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AltoVentanaAyuda() As Double
        Get
            Return _bolAltoVentanaAyuda
        End Get
        Set(ByVal value As Double)
            _bolAltoVentanaAyuda = value
        End Set
    End Property

    ''' <summary>
    ''' Permite establecer el ancho para el boton que permite mostrar la ayuda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Ancho() As Double
        Get
            Return btn.Width
        End Get
        Set(ByVal value As Double)
            btn.Width = value
            Me.Width = value
        End Set
    End Property

    ''' <summary>
    ''' Permite establecer el alto para el boton que permite mostrar la ayuda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Alto() As Double
        Get
            Return btn.Height
        End Get
        Set(ByVal value As Double)
            btn.Height = value
            Me.Height = value
        End Set
    End Property

#End Region
End Class
