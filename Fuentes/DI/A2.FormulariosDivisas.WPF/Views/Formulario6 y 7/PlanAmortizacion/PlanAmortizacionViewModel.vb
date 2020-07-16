
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports Telerik.Windows.Controls

''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (EjemploPracticoDomainService.vb y dbEjemploPractico.edmx)
''' Pantalla Clientes Beneficiarios (Maestros)
''' </summary>
''' <remarks>Juan David Correa (Alcuadrado S.A.) - 30 de Octubre 2017</remarks>
''' <history>
'''
'''</history>
Public Class PlanAmortizacionViewModel
	Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	' Poxies para comunicación con el domain service
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	Private mobjEncabezadoPorDefecto As CPX_FormularioEndeudamientoDetalle
	Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	' Para manejar la forma principal 
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	Private mobjEncabezadoAnterior As CPX_FormularioEndeudamientoDetalle
	Private mobjEncabezadoGuardadoAnterior As CPX_FormularioEndeudamientoDetalle
	Public viewListaPrincipal As PlanAmortizacionView
	Private logVentanaDetalleActiva As Boolean = False
#End Region

#Region "Inicialización - REQUERIDO"
	''' <summary>
	''' Constructor de la clase
	''' </summary>
	Public Sub New()
		IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
	End Sub

	''' <summary>
	''' Inicalización de acceso a datos y carga inicial de datos
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	''' <history>
	''' ID caso de prueba: Id_1
	''' Creado por       : Juan David Correa (Alcuadrado S.A.)
	''' Descripción      : Creacion.
	''' Fecha            : Octubre 30/2017
	''' Pruebas CB       : Juan David Correa (Alcuadrado S.A.) - Octubre 30/2017 - Resultado Ok 
	''' </history>
	Public Function inicializar() As Boolean

		Dim logResultado As Boolean = False

		Try
			mdcProxy = inicializarProxy()

			' Validación para cuando se carga en modo de diseño que no intente conectar al web service
			If Not Program.IsDesignMode() Then
				' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
				Editando = False
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return logResultado
	End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

	''' <summary>
	''' Lista de Parametro que se encuentran cargadas en el grid del formulario
	''' </summary>
	Private _ListaEncabezado As List(Of CPX_FormularioEndeudamientoDetalle)
	Public Property ListaEncabezado() As List(Of CPX_FormularioEndeudamientoDetalle)
		Get
			Return _ListaEncabezado
		End Get
		Set(ByVal value As List(Of CPX_FormularioEndeudamientoDetalle))
			_ListaEncabezadoPaginada = Nothing
			_ListaEncabezado = value
			viewListaPrincipal.ListaDetalle = _ListaEncabezado

			MyBase.CambioItem("ListaEncabezado")
			MyBase.CambioItem("ListaEncabezadoPaginada")
		End Set
	End Property

	Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
	''' <summary>
	''' Colección que pagina la lista de Parametro para navegar sobre el grid con paginación
	''' </summary>
	Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
		Get
			If Not IsNothing(_ListaEncabezado) Then
				If IsNothing(_ListaEncabezadoPaginada) Then
					Dim view = New PagedCollectionView(_ListaEncabezado)
					_ListaEncabezadoPaginada = view
					Return view
				Else
					Return (_ListaEncabezadoPaginada)
				End If
			Else
				Return Nothing
			End If
		End Get
	End Property

	''' <summary>
	''' Elemento de la lista de Parametro que se encuentra seleccionado
	''' </summary>
	Private _EncabezadoSeleccionado As CPX_FormularioEndeudamientoDetalle
	Public Property EncabezadoSeleccionado() As CPX_FormularioEndeudamientoDetalle
		Get
			Return _EncabezadoSeleccionado
		End Get
		Set(ByVal value As CPX_FormularioEndeudamientoDetalle)
			_EncabezadoSeleccionado = value
			'Llamado de metodo para realizar la consulta del encabezado editable
			ConsultarEncabezadoEdicion()
			MyBase.CambioItem("EncabezadoSeleccionado")
		End Set
	End Property

	''' <summary>
	''' Elemento de la lista de Registros que se encuentra seleccionado
	''' </summary>
	Private WithEvents _EncabezadoEdicionSeleccionado As CPX_FormularioEndeudamientoDetalle
	Public Property EncabezadoEdicionSeleccionado() As CPX_FormularioEndeudamientoDetalle
		Get
			Return _EncabezadoEdicionSeleccionado
		End Get
		Set(ByVal value As CPX_FormularioEndeudamientoDetalle)
			_EncabezadoEdicionSeleccionado = value
			If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

			End If
			MyBase.CambioItem("EncabezadoEdicionSeleccionado")
		End Set
	End Property

	Private _ListaEncabezadoEliminar As New List(Of CPX_FormularioEndeudamientoDetalle)
	Public Property ListaEncabezadoEliminar() As List(Of CPX_FormularioEndeudamientoDetalle)
		Get
			Return _ListaEncabezadoEliminar
		End Get
		Set(ByVal value As List(Of CPX_FormularioEndeudamientoDetalle))
			_ListaEncabezadoEliminar = value
			viewListaPrincipal.ListaDetalleEliminar = _ListaEncabezadoEliminar
			MyBase.CambioItem("ListaEncabezadoEliminar")
		End Set
	End Property

	Private _IDRegistroEncabezado As tblFormularioEndeudamientoExterno
	Public Property IDRegistroEncabezado() As tblFormularioEndeudamientoExterno
		Get
			Return _IDRegistroEncabezado
		End Get
		Set(ByVal value As tblFormularioEndeudamientoExterno)
			_IDRegistroEncabezado = value
			mobjEncabezadoAnterior = Nothing
			mobjEncabezadoGuardadoAnterior = Nothing
			ConsultarInformacionEncabezado()
			consultarEncabezadoPorDefecto()
			MyBase.CambioItem("IDRegistroEncabezado")
		End Set
	End Property

	Private _DetalleGuardaIndependiente As Boolean = False
	Public Property DetalleGuardaIndependiente() As Boolean
		Get
			Return _DetalleGuardaIndependiente
		End Get
		Set(ByVal value As Boolean)
			_DetalleGuardaIndependiente = value
			If _DetalleGuardaIndependiente Then
				MostrarCamposGuardaIndependiente = Visibility.Visible
			Else
				MostrarCamposGuardaIndependiente = Visibility.Collapsed
			End If
			MyBase.CambioItem("DetalleGuardaIndependiente")
		End Set
	End Property

	Private _MostrarCamposGuardaIndependiente As Visibility = Visibility.Collapsed
	Public Property MostrarCamposGuardaIndependiente() As Visibility
		Get
			Return _MostrarCamposGuardaIndependiente
		End Get
		Set(ByVal value As Visibility)
			_MostrarCamposGuardaIndependiente = value
			MyBase.CambioItem("MostrarCamposGuardaIndependiente")
		End Set
	End Property

	Private _HabilitarBotonesAcciones As Boolean = False
	Public Property HabilitarBotonesAcciones() As Boolean
		Get
			Return _HabilitarBotonesAcciones
		End Get
		Set(ByVal value As Boolean)
			_HabilitarBotonesAcciones = value
			If _HabilitarBotonesAcciones = False Then
				ConsultarInformacionEncabezado()
			End If
			MyBase.CambioItem("HabilitarBotonesAcciones")
		End Set
	End Property

	Private _Habilitar_GuardarYCopiarAnterior As Boolean = True
	Public Property Habilitar_GuardarYCopiarAnterior() As Boolean
		Get
			Return _Habilitar_GuardarYCopiarAnterior
		End Get
		Set(ByVal value As Boolean)
			_Habilitar_GuardarYCopiarAnterior = value
			MyBase.CambioItem("Habilitar_GuardarYCopiarAnterior")
		End Set
	End Property

	Private _HabilitarBoton_GuardarYCopiarAnterior As Boolean = False
	Public Property HabilitarBoton_GuardarYCopiarAnterior() As Boolean
		Get
			Return _HabilitarBoton_GuardarYCopiarAnterior
		End Get
		Set(ByVal value As Boolean)
			_HabilitarBoton_GuardarYCopiarAnterior = value
			MyBase.CambioItem("HabilitarBoton_GuardarYCopiarAnterior")
		End Set
	End Property

	Private _Habilitar_GuardarYCrearNuevo As Boolean = True
	Public Property Habilitar_GuardarYCrearNuevo() As Boolean
		Get
			Return _Habilitar_GuardarYCrearNuevo
		End Get
		Set(ByVal value As Boolean)
			_Habilitar_GuardarYCrearNuevo = value
			MyBase.CambioItem("Habilitar_GuardarYCrearNuevo")
		End Set
	End Property

	Private _HabilitarBoton_GuardarYCrearNuevo As Boolean = False
	Public Property HabilitarBoton_GuardarYCrearNuevo() As Boolean
		Get
			Return _HabilitarBoton_GuardarYCrearNuevo
		End Get
		Set(ByVal value As Boolean)
			_HabilitarBoton_GuardarYCrearNuevo = value
			MyBase.CambioItem("HabilitarBoton_GuardarYCrearNuevo")
		End Set
	End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
	''' <summary>
	''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
	''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Sub NuevoRegistroDetalle()
		Try
			CrearNuevoDetalle()
			MyBase.NuevoRegistroDetalle()
			AbrirDetalle()
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Sub CrearNuevoDetalle(Optional ByVal plogCopiarValorRegistroAnterior As Boolean = False)
		Try
			Dim objNvoEncabezado As New CPX_FormularioEndeudamientoDetalle
			If mdcProxy.IsLoading Then
				A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Exit Sub
			End If

			If plogCopiarValorRegistroAnterior Then
				'Obtiene el registro por defecto
				ObtenerRegistroAnterior(mobjEncabezadoGuardadoAnterior, objNvoEncabezado)
			Else
				'Obtiene el registro por defecto
				ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
			End If

			'Salva el registro anterior
			ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

			If Not IsNothing(_ListaEncabezado) Then
				objNvoEncabezado.intID = -(_ListaEncabezado.Count + 1)
			End If

			Editando = True
			If _Habilitar_GuardarYCopiarAnterior Then
				HabilitarBoton_GuardarYCopiarAnterior = True
			End If
			If _Habilitar_GuardarYCrearNuevo Then
				HabilitarBoton_GuardarYCrearNuevo = True
			End If
			MyBase.CambioItem("Editando")

			EncabezadoEdicionSeleccionado = objNvoEncabezado
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CrearNuevoDetalle", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
	''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Sub EditarRegistroDetalle()
		Try
			If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

				If mdcProxy.IsLoading Then
					A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Exit Sub
				End If
				IsBusy = True

				ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

				Editando = True
				If _Habilitar_GuardarYCopiarAnterior Then
					HabilitarBoton_GuardarYCopiarAnterior = True
				End If
				If _Habilitar_GuardarYCrearNuevo Then
					HabilitarBoton_GuardarYCrearNuevo = True
				End If
				MyBase.CambioItem("Editando")

				IsBusy = False

				MyBase.EditarRegistro()
				AbrirDetalle()
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
	''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
	''' </summary>
	''' <history>
	''' </history>
	Public Function ActualizarRegistroDetalle() As Boolean
		Dim plogRetorno As Boolean = False
		Dim strAccion As String = ValoresUserState.Actualizar.ToString()

		Try
			ErrorForma = String.Empty
			IsBusy = True

			mobjEncabezadoGuardadoAnterior = _EncabezadoEdicionSeleccionado


			Dim objListaEncabezado As New List(Of CPX_FormularioEndeudamientoDetalle)
			If Not IsNothing(_ListaEncabezado) Then
				For Each li In _ListaEncabezado
					objListaEncabezado.Add(li)
				Next
			End If

			If objListaEncabezado.Any(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID) Then
				Dim objEditar As CPX_FormularioEndeudamientoDetalle = objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First
				objEditar.dblValorMonedaContratada = _EncabezadoEdicionSeleccionado.dblValorMonedaContratada
				objEditar.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion
				objEditar.dtmFecha = _EncabezadoEdicionSeleccionado.dtmFecha
				objEditar.intIDFormulario = _EncabezadoEdicionSeleccionado.intIDFormulario
				objEditar.intNumero = _EncabezadoEdicionSeleccionado.intNumero
				objEditar.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario

			Else
				Dim objNvoEncabezado As New CPX_FormularioEndeudamientoDetalle
				objNvoEncabezado.intID = _EncabezadoEdicionSeleccionado.intID
				objNvoEncabezado.dblValorMonedaContratada = _EncabezadoEdicionSeleccionado.dblValorMonedaContratada
				objNvoEncabezado.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion
				objNvoEncabezado.dtmFecha = _EncabezadoEdicionSeleccionado.dtmFecha
				objNvoEncabezado.intIDFormulario = _EncabezadoEdicionSeleccionado.intIDFormulario
				objNvoEncabezado.intNumero = _EncabezadoEdicionSeleccionado.intNumero
				objNvoEncabezado.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario


				EncabezadoSeleccionado = objNvoEncabezado

				objListaEncabezado.Add(EncabezadoSeleccionado)
			End If

			ListaEncabezado = objListaEncabezado
			plogRetorno = True

			IsBusy = False


		Catch ex As Exception
			IsBusy = False
			plogRetorno = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
		End Try

		Return plogRetorno
	End Function

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
	''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
	''' </summary>
	Public Sub VisualizarRegistroDetalle()
		Try
			MyBase.ActualizarListaInconsistencias(Nothing, Program.TituloSistema, False)
			If Not IsNothing(_EncabezadoSeleccionado) Then
				If HabilitarBotonesAcciones Then
					Editando = True
					If _Habilitar_GuardarYCopiarAnterior Then
						HabilitarBoton_GuardarYCopiarAnterior = True
					End If
					If _Habilitar_GuardarYCrearNuevo Then
						HabilitarBoton_GuardarYCrearNuevo = True
					End If
				Else
					Editando = False
					HabilitarBoton_GuardarYCopiarAnterior = False
					HabilitarBoton_GuardarYCrearNuevo = False
				End If
				MyBase.CambioItem("Editando")
				AbrirDetalle()
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Sub BorrarRegistroDetalle()
		Try
			MyBase.BorrarRegistroDetalle()

			If Not IsNothing(_EncabezadoSeleccionado) Then
				If mdcProxy.IsLoading Then
					A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Exit Sub
				End If

				A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ELIMINARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim strAccion As String = ValoresUserState.Actualizar.ToString()

			If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
				If Not IsNothing(_EncabezadoSeleccionado) Then
					Dim objListaDetalle As New List(Of CPX_FormularioEndeudamientoDetalle)

					For Each li In ListaEncabezado
						objListaDetalle.Add(li)
					Next

					If IsNothing(_ListaEncabezadoEliminar) Then
						_ListaEncabezadoEliminar = New List(Of CPX_FormularioEndeudamientoDetalle)
					End If

					If _ListaEncabezadoEliminar.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count = 0 Then
						_ListaEncabezadoEliminar.Add(_EncabezadoSeleccionado)
					End If

					If objListaDetalle.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count > 0 Then
						objListaDetalle.Remove(objListaDetalle.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).First)
					End If

					ListaEncabezado = objListaDetalle
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Sub AbrirDetalle()
		Try
			If logVentanaDetalleActiva = False Then
				Dim objViewDetalle As New PlanAmortizacionModalView(Me)
				objViewDetalle.Owner = Window.GetWindow(Me.viewListaPrincipal)
				logVentanaDetalleActiva = True
				objViewDetalle.ShowDialog()
				ConsultarEncabezadoEdicion()
				logVentanaDetalleActiva = False
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "AbrirDetalle", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
	''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
	''' </summary>
	''' <history>
	''' </history>
	Public Sub CancelarRegistroDetalle()
		Try
			Editando = False
			HabilitarBoton_GuardarYCopiarAnterior = False
			HabilitarBoton_GuardarYCrearNuevo = False
			MyBase.CambioItem("Editando")
			EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
	''' <summary>
	''' Retorna una copia del encabezado activo. 
	''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
	''' </summary>
	''' <history>
	''' </history>
	Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As CPX_FormularioEndeudamientoDetalle, ByRef pobjRegistroSalvar As CPX_FormularioEndeudamientoDetalle)
		Dim objEncabezado As CPX_FormularioEndeudamientoDetalle = New CPX_FormularioEndeudamientoDetalle

		Try
			If Not IsNothing(pobjRegistro) Then

				objEncabezado.intID = pobjRegistro.intID
				objEncabezado.dblValorMonedaContratada = pobjRegistro.dblValorMonedaContratada
				objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
				objEncabezado.dtmFecha = pobjRegistro.dtmFecha
				objEncabezado.intIDFormulario = pobjRegistro.intIDFormulario
				objEncabezado.intNumero = pobjRegistro.intNumero
				objEncabezado.strUsuario = pobjRegistro.strUsuario

				pobjRegistroSalvar = objEncabezado
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region



#Region "Métodos sincrónicos del encabezado - REQUERIDO"
	''' <summary>
	''' Consulta los valores por defecto para un nuevo encabezado
	''' </summary>

	Private Sub consultarEncabezadoPorDefecto()
		Try
			Dim objRespuesta As CPX_FormularioEndeudamientoDetalle
			If IDRegistroEncabezado IsNot Nothing Then

				objRespuesta = New CPX_FormularioEndeudamientoDetalle
				objRespuesta.dtmFecha = Now
				objRespuesta.dtmActualizacion = Now
				mobjEncabezadoPorDefecto = objRespuesta
			Else
				'A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub
	Private Async Sub ConsultarInformacionEncabezado()
		Try
			Await ConsultarEncabezado()
			ListaEncabezadoEliminar = Nothing
			ListaEncabezadoEliminar = New List(Of CPX_FormularioEndeudamientoDetalle)
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarInformacionEncabezado", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Consultar de forma sincrónica los datos de Parametro
	''' </summary>
	Private Async Function ConsultarEncabezado(Optional ByVal pstrOpcion As String = "", Optional ByVal pintIDRegistro As Integer = 0) As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
			Dim objRespuesta As InvokeResult(Of List(Of CPX_FormularioEndeudamientoDetalle)) = Nothing

			ErrorForma = String.Empty
			If IDRegistroEncabezado IsNot Nothing Then

				If pstrOpcion = "ID" Then
					objRespuesta = Await mdcProxy.Detalle6Formulario_ConsultarAsync(IDRegistroEncabezado.intID, Program.Usuario)
				Else
					objRespuesta = Await mdcProxy.Detalle6Formulario_ConsultarAsync(IDRegistroEncabezado.intID, Program.Usuario)
				End If

				If Not IsNothing(objRespuesta) Then
					If Not IsNothing(objRespuesta.Value) Then
						ListaEncabezado = objRespuesta.Value

						If pintIDRegistro > 0 Then
							If ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).Count > 0 Then
								EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).First
							End If
						Else
							If ListaEncabezado.Count = 0 Then
								EncabezadoSeleccionado = Nothing
							End If
						End If
					Else
						A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
					End If
				Else
					A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
				End If

				MyBase.CambioItem("ListaEncabezado")
				MyBase.CambioItem("ListaEncabezadoPaginada")

				logResultado = True
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return (logResultado)
	End Function

	Public Sub ConsultarEncabezadoEdicion()
		Try
			EncabezadoEdicionSeleccionado = Nothing
			If Not IsNothing(_EncabezadoSeleccionado) Then
				Dim objEncabezadoEdicion As New CPX_FormularioEndeudamientoDetalle

				objEncabezadoEdicion.intID = _EncabezadoSeleccionado.intID
				objEncabezadoEdicion.dblValorMonedaContratada = _EncabezadoSeleccionado.dblValorMonedaContratada
				objEncabezadoEdicion.dtmActualizacion = _EncabezadoSeleccionado.dtmActualizacion
				objEncabezadoEdicion.dtmFecha = _EncabezadoSeleccionado.dtmFecha
				objEncabezadoEdicion.intIDFormulario = _EncabezadoSeleccionado.intIDFormulario
				objEncabezadoEdicion.intNumero = _EncabezadoSeleccionado.intNumero
				objEncabezadoEdicion.strUsuario = _EncabezadoSeleccionado.strUsuario

				EncabezadoEdicionSeleccionado = objEncabezadoEdicion
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Consultar de forma sincrónica los datos del Registro a editar
	''' </summary>
	''' <param name="pintID">Indica el ID de la entidad a consultar</param>
	Private Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Boolean

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
			Dim objRespuesta As CPX_FormularioEndeudamientoDetalle = Nothing

			ErrorForma = String.Empty

			objRespuesta = ListaEncabezado.Where(Function(i) i.intID = pintID).First

			If Not IsNothing(objRespuesta) Then
				EncabezadoEdicionSeleccionado = objRespuesta
			Else
				A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
			End If

			logResultado = True
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return (logResultado)
	End Function

	''' <summary>
	''' Consultar de forma sincrónica los datos de Parametro
	''' </summary>
#End Region




End Class
