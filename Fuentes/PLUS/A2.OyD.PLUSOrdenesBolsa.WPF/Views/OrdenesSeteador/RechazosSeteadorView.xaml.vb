Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'Imports C1.Silverlight
Imports System.ComponentModel

Partial Public Class RechazosSeteadorView
    Inherits UserControl
    Implements INotifyPropertyChanged

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        InitializeComponent()
        Me.DataContext = Me
    End Sub

#Region "Declaración Eventos"

    ''' -----------------------------------------------------------------------------------------------
    ''' -- DECLARACIONES DE EVENTOS GLOBALES AL CONTROL
    ''' -----------------------------------------------------------------------------------------------
    ''' 
    Public Event mensajeAsignado(ByVal pstrMensaje As String)

#End Region

#Region "Declaración Variables"
    ''' -----------------------------------------------------------------------------------------------
    ''' -- DECLARACIONES DE VARIABLES GLOBALES AL CONTROL
    ''' -----------------------------------------------------------------------------------------------

    Private _mstrUsuario As String = ""
    Private _MostrarLista As Boolean = False
    Private _MostrarObservaciones As Boolean = False
    Private _PermitirGuardarSinItemLista As Boolean = True
    Private _PermitirGuardarSinObservacion As Boolean = True

#End Region

#Region "Propiedades"

    ''' -----------------------------------------------------------------------------------------------
    ''' -- DECLARACIONES DE PROPIEDADES
    ''' -----------------------------------------------------------------------------------------------

    Private ReadOnly Property usuarioActivo() As String
        Get
            Return (_mstrUsuario)
        End Get
    End Property

    Public WriteOnly Property MensajeUsuario() As String
        Set(ByVal value As String)
            Me.txtMsgUsuario.Text = value
            MensajeRegla = value
        End Set
    End Property

    'Public WriteOnly Property Imagen() As String
    '    Set(ByVal value As String)
    '        Dim SourceUri As Uri = New Uri(value, UriKind.Relative)
    '        imgMensaje.SetValue(Image.SourceProperty, New System.Windows.Media.Imaging.BitmapImage(SourceUri))
    '    End Set
    'End Property

    Public WriteOnly Property MostrarListaObservaciones() As Boolean
        Set(ByVal value As Boolean)
            If value Then
                stackJustificacion.Visibility = Visibility.Visible
                _MostrarLista = True
                PermitirGuardarSinItem = False
            Else
                stackJustificacion.Visibility = Visibility.Collapsed
                _MostrarLista = False
                PermitirGuardarSinItem = True
            End If
        End Set
    End Property

    Public WriteOnly Property MostrarTextoObservaciones() As Boolean
        Set(ByVal value As Boolean)
            If value Then
                stackObservaciones.Visibility = Visibility.Visible
                _MostrarObservaciones = True
                PermitirGuardarSinObservacion = False
            Else
                stackObservaciones.Visibility = Visibility.Collapsed
                _MostrarObservaciones = False
                PermitirGuardarSinObservacion = True
            End If
        End Set
    End Property

    Public WriteOnly Property PermitirGuardarSinItem() As Boolean
        Set(ByVal value As Boolean)
            If value Then
                _PermitirGuardarSinItemLista = True
            Else
                _PermitirGuardarSinItemLista = False
            End If
        End Set
    End Property

    Public WriteOnly Property PermitirGuardarSinObservacion() As Boolean
        Set(ByVal value As Boolean)
            If value Then
                _PermitirGuardarSinObservacion = True
            Else
                _PermitirGuardarSinObservacion = False
            End If
        End Set
    End Property

    Private _ListaCausas As List(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo) = New List(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
    Public Property ListaCausas() As List(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
        Get
            Return _ListaCausas
        End Get
        Set(ByVal value As List(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo))
            _ListaCausas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaCausas"))
        End Set
    End Property

    Private _TextoConfirmacion As String = String.Empty
    Public Property TextoConfirmacion() As String
        Get
            Return _TextoConfirmacion
        End Get
        Set(ByVal value As String)
            _TextoConfirmacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TextoConfirmacion"))
        End Set
    End Property

    Private _Observaciones As String
    Public Property Observaciones() As String
        Get
            Return _Observaciones
        End Get
        Set(ByVal value As String)
            _Observaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Observaciones"))
        End Set
    End Property

    Private _CodConfirmacion As String
    Public Property CodConfirmacion() As String
        Get
            Return _CodConfirmacion
        End Get
        Set(ByVal value As String)
            _CodConfirmacion = value
        End Set
    End Property

    Private _CodRegla As String
    Public Property CodRegla() As String
        Get
            Return _CodRegla
        End Get
        Set(ByVal value As String)
            _CodRegla = value
        End Set
    End Property

    Private _NombreRegla As String
    Public Property NombreRegla() As String
        Get
            Return _NombreRegla
        End Get
        Set(ByVal value As String)
            _NombreRegla = value
        End Set
    End Property

    Private _MensajeRegla As String
    Public Property MensajeRegla() As String
        Get
            Return _MensajeRegla
        End Get
        Set(ByVal value As String)
            _MensajeRegla = value
        End Set
    End Property



#End Region

#Region "Evento de controles"

    Private Sub btnSI_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If _MostrarLista Then
            TextoConfirmacion = String.Empty
            If lstCausas.SelectedItems.Count > 0 Then
                For Each li In lstCausas.SelectedItems
                    If String.IsNullOrEmpty(TextoConfirmacion) Then
                        TextoConfirmacion = ListaCausas.Where(Function(i) i.ID = CType(li, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo).ID).Single.Descripcion
                    Else
                        TextoConfirmacion = String.Format("{0}|{1}", TextoConfirmacion, ListaCausas.Where(Function(i) i.ID = CType(li, A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo).ID).Single.Descripcion)
                    End If
                Next
            End If
        End If

        Dim logCerrar As Boolean = True
        Dim strMensajeValidacion As String = String.Empty

        If _PermitirGuardarSinItemLista = False Then
            If String.IsNullOrEmpty(TextoConfirmacion) Then
                logCerrar = False
                strMensajeValidacion = "    - Debe de seleccionar al menos un item de la lista."
            End If
        End If

        If _PermitirGuardarSinObservacion = False Then
            If String.IsNullOrEmpty(Observaciones) Then
                logCerrar = False
                If String.IsNullOrEmpty(strMensajeValidacion) Then
                    strMensajeValidacion = "    - Debe ingresar la observación."
                Else
                    strMensajeValidacion = String.Format("{0}{1}    - Debe ingresar la observación.", strMensajeValidacion, vbCrLf)
                End If
            End If
        End If

        If logCerrar Then
            'Me.DialogResult = CType(True, MessageBoxResult)
            'Me.Close()
            RaiseEvent mensajeAsignado(strMensajeValidacion)
        Else
            'clMensajes.mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            A2Utilidades.Mensajes.mostrarMensaje(strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If

    End Sub

    Private Sub btnNO_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        'Me.DialogResult = CType(False, MessageBoxResult)
        'Me.Close()
        RaiseEvent mensajeAsignado("CANCEL")
    End Sub

    'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

#End Region



    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class








