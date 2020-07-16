Imports System.ComponentModel

Class MainWindow
    Implements INotifyPropertyChanged

    Dim objPantallaVisualizar As Object = Nothing
    Private mlogInicializar As Boolean = True
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _IsBusy As Boolean = False
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Me.DataContext = Me
        IsBusy = True
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                inicializar()

            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Async Sub inicializar()
        'creación recursos
        Await clsRecursos.Crear()
        IsBusy = False
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
		Dim pstrOpcionMenu As String = CType(sender, Button).Content

		GridPrincipal.Children.Clear()
        objPantallaVisualizar = Nothing

		If pstrOpcionMenu = "Ordenes" Then
			'objPantallaVisualizar = New EjemploPracticoWPF.ProductosView()
			'objPantallaVisualizar = New A2OrdenesDivisasWPF.OrdenesView()
		End If

		GridPrincipal.Children.Add(objPantallaVisualizar)
    End Sub
End Class
