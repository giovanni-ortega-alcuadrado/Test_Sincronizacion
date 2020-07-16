Imports A2Utilidades.Mensajes
Imports System.Globalization
Imports A2.OyD.OYDServer.RIA.Web


Public Class BuscadorPersona

#Region "Constantes"

	Private Const STR_NOMBRE_VIEW_MODEL As String = "VM"
	Private Const STR_TEXTO_ABRIR_POPUP As String = " " '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda

#End Region

#Region "Eventos"
	Public Event personaAsignado(ByVal pintIdPersona As Integer, ByVal pstrCodigoOyD As String, ByVal objPersona As CPX_BuscadorPersonas)
#End Region

#Region "Variables"

	Public WithEvents VM As BuscadorPersonasViewModel
	Private mlogInicializar As Boolean = True
	Private mlogDigitandoFiltro As Boolean = False

#End Region

#Region "Inicializacion"

	Public Sub New()
		InitializeComponent()
		VM = New BuscadorPersonasViewModel
		Me.DataContext = VM
	End Sub

	Private Sub inicializar()
		If Not Me.DataContext Is Nothing Then
			VM = CType(Me.DataContext, BuscadorPersonasViewModel)
			VM.NombreView = Me.ToString

		End If
	End Sub

	Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
		Try
			Dim logBuscar As Boolean = False
			If Not Program.IsDesignMode() Then
				If VM.Inicializado = False Then
					'// Validar si el control ha sido inicializado es la primera ejecución para asegurar que no se vuelvan a inicializar los controles
					'   Se valida el valor de la propiedad en el VM porque cuando la propiedad en el control ha sido asignada por Binding se 
					'   dispara en momento diferente a cuando se asigna un valor fijo. Si se asigna un valor fijo y se deja esta instrucción
					'   se lanzaría dos veces la asignación de la propiedad IdItem del VM 
					VM.inicializar()
					If Me.GetValue(intIDPersonaDep) IsNot Nothing And VM.intIDPersona Is Nothing Then
						VM.intIDPersona = Me.GetValue(intIDPersonaDep)
					ElseIf Me.GetValue(intIDPersonaDep) IsNot Nothing Then
						logBuscar = True
						VM.consultarPersonas()
					End If

					Me.grDatosClt.Visibility = Visibility.Collapsed

					VM.NombreView = Me.ToString
					VM.Inicializado = True
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, VM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub cmdBuscar_Click(sender As Object, e As RoutedEventArgs)
		Try
			VM.CondicionFiltro = acbPersonas.SearchText
			VM.consultarPersonas()
		Catch ex As Exception
			Me.cmdBuscar.IsEnabled = True
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar personas que cumplan con el filtro indicado.", Me.Name, "cmdBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub acbPersonas_SearchTextChanged(sender As Object, e As EventArgs) Handles acbPersonas.SearchTextChanged

	End Sub
#End Region

#Region "Propiedades"
	Private Shared intIDPersonaDep As DependencyProperty = DependencyProperty.Register("intIDPersona", GetType(Integer?), GetType(BuscadorPersona), New PropertyMetadata(AddressOf CambioPropiedadDep))
	Private Shared EstadoPersonaDep As DependencyProperty = DependencyProperty.Register("EstadoPersona", GetType(BuscadorPersonasViewModel.EstadosPersona), GetType(BuscadorPersona), New PropertyMetadata(AddressOf CambioPropiedadDep))
	Private Shared ReadOnly IDReceptorDep As DependencyProperty = DependencyProperty.Register("IDReceptor", GetType(Integer?), GetType(BuscadorPersona), New PropertyMetadata(AddressOf CambioPropiedadDep))
	Private Shared ReadOnly PersonaBuscarDep As DependencyProperty = DependencyProperty.Register("PersonaBuscar", GetType(String), GetType(BuscadorPersona), New PropertyMetadata("", New PropertyChangedCallback(AddressOf PersonaBuscarChanged)))

	Public Property PersonaBuscar() As String
		Get
			Return CStr(GetValue(PersonaBuscarDep))
		End Get
		Set(ByVal value As String)
			SetValue(PersonaBuscarDep, value)
		End Set
	End Property

	Public Property EstadoPersona() As BuscadorPersonasViewModel.EstadosPersona
		Get
			Return (CType(Me.GetValue(EstadoPersonaDep), BuscadorPersonasViewModel.EstadosPersona))
		End Get
		Set(ByVal value As BuscadorPersonasViewModel.EstadosPersona)
			Me.SetValue(EstadoPersonaDep, value)
			Me.VM.EstadoComitente = value
		End Set
	End Property

	Private _RolPersona As BuscadorPersonasViewModel.RolesPersona
	Public Property RolPersona() As BuscadorPersonasViewModel.RolesPersona
		Get
			Return _RolPersona
		End Get
		Set(ByVal value As BuscadorPersonasViewModel.RolesPersona)
			_RolPersona = value
			VM.RolPersona = value
		End Set
	End Property

	Public Property IDReceptor() As Integer?
		Get
			Return Me.GetValue(IDReceptorDep)
		End Get
		Set(ByVal value As Integer?)
			SetValue(IDReceptorDep, value)
			VM.IDReceptor = value
		End Set
	End Property

	Public Property intIDPersona As Integer?
		Get
			Return (Me.GetValue(intIDPersonaDep))
		End Get
		Set(value As Integer?)
			SetValue(intIDPersonaDep, value)
			VM.intIDPersona = value
		End Set
	End Property

	Private _mlogVerDetalle As Boolean = True
	''' <summary>
	''' Indica si en la parte inferior del buscador se depliega el detalle del elemento seleccionado
	''' </summary>
	Public Property VerDetalle As Boolean
		Get
			Return (_mlogVerDetalle)
		End Get
		Set(ByVal value As Boolean)
			_mlogVerDetalle = value
		End Set
	End Property

	Private _BuscarAlIniciar As Boolean = False
	''' <summary>
	''' Indica si al iniciar el control lanza una consulta de comitentes. Solamente aplica si IdComitente no se envía
	''' </summary>
	Public Property BuscarAlIniciar As Boolean
		Get
			Return (_BuscarAlIniciar)
		End Get
		Set(ByVal value As Boolean)
			_BuscarAlIniciar = value
		End Set
	End Property
#End Region

#Region "Eventos controles"
	Private Sub VM_CargaClientesCompleta(ByVal plogNroComitentes As Integer, ByVal plogBusquedaComitenteEspecifico As Boolean) Handles VM.CargaPersonasCompleta

		VM.Activar = True
		cmdBuscar.IsEnabled = True
		acbPersonas.IsEnabled = True

		Try
			If plogNroComitentes = 0 Then
				mostrarMensaje("No hay personas que cumplan con la condición de búsqueda", Program.TituloSistema)
			Else
				'Modificado por Juan David Correa
				'Descripción se adiciona la condición para cargar el cliente por defecto si solo se encuentra un registro.
				If Not IsNothing(VM.ListaPersonas) Then
					If VM.ListaPersonas.Count = 1 Then
						VM.PersonaSeleccionado = VM.ListaPersonas.FirstOrDefault
						acbPersonas.SearchText = Trim(VM.PersonaSeleccionado.strSeleccion)
						PersonaBuscar = VM.PersonaSeleccionado.strSeleccion
						acbPersonas.IsDropDownOpen = False
					Else
						If plogBusquedaComitenteEspecifico = False Or Me._BuscarAlIniciar Then
							'If acbPersonas.SearchText.Trim().Equals(String.Empty) Then
							'	acbPersonas.SearchText = STR_TEXTO_ABRIR_POPUP '// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda - Asignar la constante y no el valor fijo " "
							'End If

							acbPersonas.Focus()

							'acbPersonas.IsDropDownOpen = True

						End If
					End If
				End If

			End If

			'// Desactivar indicador que permite identificar que el usuario digitó una condición de filtro
			mlogDigitandoFiltro = False
		Catch ex As Exception
			mlogDigitandoFiltro = False
			mostrarErrorAplicacion(Program.Usuario, "Falló la actualización del cliente seleccionado", Me.Name, "VM_CargaClientesCompleta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub
	Private Function aplicarfiltroClientes(ByVal pstrFiltro As String, ByVal pobjItem As Object) As Boolean

		Dim logResultado As Boolean = False
		Dim objItem As CPX_BuscadorPersonas = Nothing

		Try
			Dim strNroDocumento As String = ""

			'// CCM20120305 - Eliminar espacio en blanco asignado para que abra el popup cuando carga la búsqueda
			If pstrFiltro.Equals(STR_TEXTO_ABRIR_POPUP) = False Then
				pstrFiltro = pstrFiltro.Trim()
			End If

			pstrFiltro = pstrFiltro.ToUpper(CultureInfo.InvariantCulture)

			If Not pobjItem Is Nothing Then
				objItem = CType(pobjItem, CPX_BuscadorPersonas)
				strNroDocumento = IIf(IsNothing(objItem.strNombre), "", objItem.strNroDocumento).ToString()

				If Len(pstrFiltro) > 0 Then
					Try
						If pstrFiltro = STR_TEXTO_ABRIR_POPUP Then
							logResultado = True
						Else
							logResultado = CBool(objItem.strNombre.ToUpper(CultureInfo.InvariantCulture).Contains(pstrFiltro) Or
									strNroDocumento.ToUpper(CultureInfo.InvariantCulture).Contains(pstrFiltro) Or
									objItem.intID.ToString().Contains(pstrFiltro))
						End If
					Catch ex As Exception
						A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la validación de las condiciones de búsqueda del cliente.", Me.ToString(), "aplicarfiltroClientes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
					End Try
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la validación de las condiciones de búsqueda del cliente(2).", Me.ToString(), "aplicarfiltroClientes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try

		Return (logResultado)
	End Function

	Private Sub acbPersonas_SelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Controls.SelectionChangedEventArgs)
		Dim objSeleccion As CPX_BuscadorPersonas
		Try
			objSeleccion = CType(CType(sender, Telerik.Windows.Controls.RadAutoCompleteBox).SelectedItem, CPX_BuscadorPersonas)
			If objSeleccion Is Nothing Then
				Me.grDatosClt.Visibility = Visibility.Collapsed
			Else
				If Me.VerDetalle Then
					Me.grDatosClt.Visibility = Visibility.Visible
				End If
				RaiseEvent personaAsignado(objSeleccion.intID, objSeleccion.strCodigoOyD, objSeleccion)
				'/
				mlogDigitandoFiltro = False
			End If
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la persona seleccionado.", Me.Name, "acbClientes_SelectionChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub
#End Region

#Region "Callback"

	''' <summary>
	''' Procedimiento de Call back que se lanza cuando alguna de las dependency properties se modifica
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="args"></param>
	''' <remarks></remarks>
	Private Shared Sub CambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)

	End Sub

	Private Shared Sub PersonaBuscarChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
		Try
			Dim obj As BuscadorPersona = DirectCast(d, BuscadorPersona)
			If Not String.IsNullOrEmpty(obj.PersonaBuscar) Then
				If obj.VM IsNot Nothing Then
					obj.acbPersonas.SearchText = Trim(obj.PersonaBuscar)
					obj.acbPersonas.IsDropDownOpen = False
				End If
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "BuscadorClientes", "ClienteBuscarChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

#End Region

End Class
