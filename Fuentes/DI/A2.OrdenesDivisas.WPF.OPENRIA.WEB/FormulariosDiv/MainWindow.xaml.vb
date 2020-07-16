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
		IsBusy = True
		'creación recursos
		Await clsRecursos.Crear(Environment.GetCommandLineArgs())
		IsBusy = False
	End Sub

	Private Sub CargarMenus(plogOpcionNuevoBorrar As Boolean)

		Dim permisosBotones As New List(Of String)
		permisosBotones.Add("Forma|Forma|Pasar a modo forma")
		permisosBotones.Add("Buscar|Buscar|Buscar un registro")
		permisosBotones.Add("Editar|Editar|Editar registro seleccionado")

		permisosBotones.Add("Aprobar|Aprobar|Aprobar modificaciones al registro")
		permisosBotones.Add("Rechazar|Rechazar|Rechazar modificaciones al registro")
		permisosBotones.Add("Version|Versión|Versión")

		If plogOpcionNuevoBorrar Then
			permisosBotones.Add("Nuevo|Nuevo|Ingresar nuevo registro")
			permisosBotones.Add("Borrar|Borrar|Borrar el registro seleccionado")

		End If
		Application.Current.Resources("A2Consola_ToolbarActivo") = permisosBotones
	End Sub

	Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
		Dim pstrOpcionMenu As String = CType(sender, MenuItem).Tag

		GridPrincipal.Children.Clear()
		objPantallaVisualizar = Nothing

		If pstrOpcionMenu = "Formulario1" Then
			CargarMenus(False)
			objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario1View()
		ElseIf pstrOpcionMenu = "Formulario2" Then
			CargarMenus(False)

			objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario2View()
		ElseIf pstrOpcionMenu = "Formulario3" Then
			CargarMenus(False)

			objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario3View()
		ElseIf pstrOpcionMenu = "Formulario6" Then
			CargarMenus(True)

			objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario6View()
		ElseIf pstrOpcionMenu = "Formulario7" Then
			CargarMenus(True)

			objPantallaVisualizar = New A2FormulariosDivisasWPF.Formulario7View()
		End If

		GridPrincipal.Children.Add(objPantallaVisualizar)
	End Sub
End Class
