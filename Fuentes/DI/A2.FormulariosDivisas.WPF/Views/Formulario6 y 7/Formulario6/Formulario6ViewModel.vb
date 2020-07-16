Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel

Public Class Formulario6ViewModel
	Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	' Poxies para comunicación con el domain service
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	Private mobjEncabezadoPorDefecto As tblFormularioEndeudamientoExterno
	Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	' Para manejar la forma principal 
	'---------------------------------------------------------------------------------------------------------------------------------------------------
	Private mobjEncabezadoAnterior As tblFormularioEndeudamientoExterno
	Dim strTipoFiltroBusqueda As String = String.Empty
	Dim strCodigoAcreedorOrdiginal As String

	Public Enum TipoOperacion
		Inicial = 1
		Modificacion = 2
	End Enum

	Private strNombreReporte As String = "Formulario6VB"

	Public mobjView As Formulario6View

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
	Public Async Function inicializar() As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			mdcProxy = inicializarProxy()

			' Validación para cuando se carga en modo de diseño que no intente conectar al web service
			If Not Program.IsDesignMode() Then
				' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
				consultarEncabezadoPorDefecto()
				Await ConsultarNumerales()
				' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
				Await ConsultarEncabezado("FILTRAR", String.Empty)
				ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISASF6F7").ToList
				ListaCodigosBancRep = DiccionarioCombosPantalla("CODIGOBCOREPNORES").ToList
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return (logResultado)

	End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

	''' <summary>
	''' Lista de Registros que se encuentran cargadas en el grid del formulario
	''' </summary>
	''' 



	Private _VisibilityTxtCodigoCiiU As Visibility = Visibility.Collapsed
	Public Property VisibilityTxtCodigoCiiU() As Visibility
		Get
			Return _VisibilityTxtCodigoCiiU
		End Get
		Set(ByVal value As Visibility)
			_VisibilityTxtCodigoCiiU = value
			CambioItem("VisibilityTxtCodigoCiiU")
		End Set
	End Property

	Private _VisibilityAbcCodigoCiiU As Visibility = Visibility.Visible
	Public Property VisibilityAbcCodigoCiiU() As Visibility
		Get
			Return _VisibilityAbcCodigoCiiU
		End Get
		Set(ByVal value As Visibility)
			_VisibilityAbcCodigoCiiU = value
			CambioItem("VisibilityAbcCodigoCiiU")
		End Set
	End Property

	Private _ClienteSeleccionado As CPX_Clientes
	Public Property ClienteSeleccionado() As CPX_Clientes
		Get
			Return _ClienteSeleccionado
		End Get
		Set(ByVal value As CPX_Clientes)
			_ClienteSeleccionado = value
			If _ClienteSeleccionado IsNot Nothing And EncabezadoEdicionSeleccionado IsNot Nothing Then
				_EncabezadoEdicionSeleccionado.strTipoIdentificacionDeudor = _ClienteSeleccionado.strTipoIdentificacion
				_EncabezadoEdicionSeleccionado.strNumeroIdentificacionDeudor = _ClienteSeleccionado.strNroDocumento
				If _ClienteSeleccionado.strTipoIdentificacion = "N" Then
					_EncabezadoEdicionSeleccionado.intDigitoVerificacionDeudor = Integer.Parse(_ClienteSeleccionado.strNroDocumento.Last)
				End If

				_EncabezadoEdicionSeleccionado.strNombreDeudor = _ClienteSeleccionado.strNombre
				_EncabezadoEdicionSeleccionado.intIDCiudadDeudor = _ClienteSeleccionado.lngIdCiudadNacimiento
				_EncabezadoEdicionSeleccionado.strTelefonoDeudor = _ClienteSeleccionado.strTelefono1
				_EncabezadoEdicionSeleccionado.strEmailDeudor = _ClienteSeleccionado.strEMail
				_EncabezadoEdicionSeleccionado.strDireccionDeudor = _ClienteSeleccionado.strDireccion
				CodigoCiiuSeleccionado = DiccionarioCombosPantalla("CODIGOSCIIU").Where(Function(i) i.Descripcion = _ClienteSeleccionado.strcodigoCiiu).FirstOrDefault
				If Not (String.IsNullOrWhiteSpace(_ClienteSeleccionado.strcodigoCiiu) Or _ClienteSeleccionado.strcodigoCiiu = "0") Then
					_EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor = _ClienteSeleccionado.strcodigoCiiu
				Else
					HabilitarCodigoCIIU = True
				End If

				If String.IsNullOrWhiteSpace(_EncabezadoEdicionSeleccionado.strEmailDeudor) Then
					HabilitarCorreo = True
				End If
			ElseIf _ClienteSeleccionado Is Nothing And EncabezadoEdicionSeleccionado IsNot Nothing Then
				_EncabezadoEdicionSeleccionado.strTipoIdentificacionDeudor = Nothing
				_EncabezadoEdicionSeleccionado.strNumeroIdentificacionDeudor = Nothing
				_EncabezadoEdicionSeleccionado.intDigitoVerificacionDeudor = Nothing
				_EncabezadoEdicionSeleccionado.strNombreDeudor = Nothing
				_EncabezadoEdicionSeleccionado.intIDCiudadDeudor = Nothing
				_EncabezadoEdicionSeleccionado.strTelefonoDeudor = Nothing
				_EncabezadoEdicionSeleccionado.strEmailDeudor = Nothing
				_EncabezadoEdicionSeleccionado.strDireccionDeudor = Nothing
				CodigoCiiuSeleccionado = Nothing
				HabilitarCorreo = False
				HabilitarCodigoCIIU = False
				_EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor = Nothing
			End If


			CambioItem("ClienteSeleccionado")
		End Set
	End Property

	Private _ListaClientes As List(Of CPX_Clientes) = New List(Of CPX_Clientes)
	Public Property ListaClientes() As List(Of CPX_Clientes)
		Get
			Return _ListaClientes
		End Get
		Set(ByVal value As List(Of CPX_Clientes))
			_ListaClientes = value
			CambioItem("ListaClientes")
		End Set
	End Property

	Private _CodigoBancRepSeleccionado As ProductoCombos
	Public Property CodigoBancRepSeleccionado() As ProductoCombos
		Get
			Return _CodigoBancRepSeleccionado
		End Get
		Set(ByVal value As ProductoCombos)
			_CodigoBancRepSeleccionado = value
			If _CodigoBancRepSeleccionado IsNot Nothing And _EncabezadoEdicionSeleccionado IsNot Nothing Then
				_EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor = _CodigoBancRepSeleccionado.Retorno
				HabilitarNombreAcreedor = False
				HabilitarPaisAcreedor = False
				HabilitarTipoAcreedor = False
			ElseIf _CodigoBancRepSeleccionado Is Nothing And _EncabezadoEdicionSeleccionado IsNot Nothing Then
				_EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor = Nothing
				HabilitarNombreAcreedor = True
				HabilitarPaisAcreedor = True
				HabilitarTipoAcreedor = True
			End If
			_EncabezadoEdicionSeleccionado.strNombreAcreedor = Nothing
			_EncabezadoEdicionSeleccionado.strPais = Nothing
			_EncabezadoEdicionSeleccionado.strTipoAcreedor = Nothing
			CambioItem("CodigoBancRepSeleccionado")
		End Set
	End Property

	Private _ListaNumerales As List(Of BusquedaNumeralesCambiarios)
	Public Property ListaNumerales() As List(Of BusquedaNumeralesCambiarios)
		Get
			Return _ListaNumerales
		End Get
		Set(ByVal value As List(Of BusquedaNumeralesCambiarios))
			_ListaNumerales = value
			CambioItem("ListaNumerales")
		End Set
	End Property

	Private _ListaCodigosBancRep As List(Of ProductoCombos)
	Public Property ListaCodigosBancRep() As List(Of ProductoCombos)
		Get
			Return _ListaCodigosBancRep
		End Get
		Set(ByVal value As List(Of ProductoCombos))
			_ListaCodigosBancRep = value
			CambioItem("ListaCodigosBancRep")
		End Set
	End Property

	Private _CodigoCiiuSeleccionado As ProductoCombos
	Public Property CodigoCiiuSeleccionado() As ProductoCombos
		Get
			Return _CodigoCiiuSeleccionado
		End Get
		Set(ByVal value As ProductoCombos)
			_CodigoCiiuSeleccionado = value
			If _CodigoCiiuSeleccionado IsNot Nothing And _EncabezadoEdicionSeleccionado IsNot Nothing Then
				_EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor = _CodigoCiiuSeleccionado.Retorno
			ElseIf _CodigoCiiuSeleccionado Is Nothing And _EncabezadoEdicionSeleccionado IsNot Nothing Then
				HabilitarCodigoCIIU = True
			End If
			MyBase.CambioItem("CodigoCiiuSeleccionado")
		End Set
	End Property

	Private _ListaTipoOperacion As List(Of ProductoCombos)
	Public Property ListaTipoOperacion() As List(Of ProductoCombos)
		Get
			Return _ListaTipoOperacion
		End Get
		Set(ByVal value As List(Of ProductoCombos))
			_ListaTipoOperacion = value
			MyBase.CambioItem("ListaTipoOperacion")
		End Set
	End Property

	Private _ListaEncabezado As List(Of CPX_tblFormularioEndeudamientoExterno)
	Public Property ListaEncabezado() As List(Of CPX_tblFormularioEndeudamientoExterno)
		Get
			Return _ListaEncabezado
		End Get
		Set(ByVal value As List(Of CPX_tblFormularioEndeudamientoExterno))
			_ListaEncabezadoPaginada = Nothing
			_ListaEncabezado = value
			MyBase.CambioItem("ListaEncabezado")
			MyBase.CambioItem("ListaEncabezadoPaginada")
		End Set
	End Property

	Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
	''' <summary>
	''' Colección que pagina la lista de Registros para navegar sobre el grid con paginación
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
	''' Elemento de la lista de Registros que se encuentra seleccionado
	''' </summary>
	Private _EncabezadoSeleccionado As CPX_tblFormularioEndeudamientoExterno
	Public Property EncabezadoSeleccionado() As CPX_tblFormularioEndeudamientoExterno
		Get
			Return _EncabezadoSeleccionado
		End Get
		Set(ByVal value As CPX_tblFormularioEndeudamientoExterno)
			_EncabezadoSeleccionado = value
			'Llamado de metodo para realizar la consulta del encabezado editable
			ConsultarEncabezadoEdicion()
			MyBase.CambioItem("EncabezadoSeleccionado")
		End Set
	End Property

	Private _HabilitarNombreAcreedor As Boolean
	Public Property HabilitarNombreAcreedor() As Boolean
		Get
			Return _HabilitarNombreAcreedor And Editando
		End Get
		Set(ByVal value As Boolean)
			_HabilitarNombreAcreedor = value
			CambioItem("HabilitarNombreAcreedor")
		End Set
	End Property

	Private _HabilitarPaisAcreedor As Boolean
	Public Property HabilitarPaisAcreedor() As Boolean
		Get
			Return _HabilitarPaisAcreedor And Editando
		End Get
		Set(ByVal value As Boolean)
			_HabilitarPaisAcreedor = value
			CambioItem("HabilitarPaisAcreedor")
		End Set
	End Property

	Private _HabilitarTipoAcreedor As Boolean
	Public Property HabilitarTipoAcreedor() As Boolean
		Get
			Return _HabilitarTipoAcreedor And Editando
		End Get
		Set(ByVal value As Boolean)
			_HabilitarTipoAcreedor = value
			CambioItem("HabilitarTipoAcreedor")
		End Set
	End Property

	''' <summary>
	''' Elemento de la lista de Registros que se encuentra seleccionado
	''' </summary>
	Private WithEvents _EncabezadoEdicionSeleccionado As tblFormularioEndeudamientoExterno
	Public Property EncabezadoEdicionSeleccionado() As tblFormularioEndeudamientoExterno
		Get
			Return _EncabezadoEdicionSeleccionado
		End Get
		Set(ByVal value As tblFormularioEndeudamientoExterno)
			_EncabezadoEdicionSeleccionado = value
			If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
				HabilitarEdicionDetalle = True
				ModificarListaOperacion()
				If ListaCodigosBancRep.Any(Function(i) i.Retorno = _EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor) Then
					CodigoBancRepSeleccionado = ListaCodigosBancRep.Where(Function(i) i.Retorno = _EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor).FirstOrDefault
				Else
					strCodigoAcreedorOrdiginal = _EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor
					mobjView.acbCodigoAcreedor.SearchText = _EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor
					_EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor = Nothing
					HabilitarNombreAcreedor = True
					HabilitarPaisAcreedor = True
					HabilitarTipoAcreedor = True
				End If
				FiltrarClientes(_EncabezadoEdicionSeleccionado.strNumeroIdentifiacion)
				If _EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor Is Nothing Or _EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor = "0" Then
					VisibilityAbcCodigoCiiU = Visibility.Visible
					VisibilityTxtCodigoCiiU = Visibility.Collapsed
				Else
					VisibilityAbcCodigoCiiU = Visibility.Collapsed
					VisibilityTxtCodigoCiiU = Visibility.Visible
				End If
			Else
				HabilitarEdicionDetalle = False
			End If
			HabilitarCorreo = False
			HabilitarCodigoCIIU = False
			MyBase.CambioItem("EncabezadoEdicionSeleccionado")
		End Set
	End Property

	''' <summary>
	''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
	''' </summary>
	Private _cb As CamposBusquedaFormulario6
	Public Property cb() As CamposBusquedaFormulario6
		Get
			Return _cb
		End Get
		Set(ByVal value As CamposBusquedaFormulario6)
			_cb = value
			MyBase.CambioItem("cb")
		End Set
	End Property

	Private _HabilitarEdicionDetalle As Boolean = False
	Public Property HabilitarEdicionDetalle() As Boolean
		Get
			Return _HabilitarEdicionDetalle And Editando
		End Get
		Set(ByVal value As Boolean)
			_HabilitarEdicionDetalle = value
			MyBase.CambioItem("HabilitarEdicionDetalle")
		End Set
	End Property

	Private _HabilitarCodigoCIIU As Boolean
	Public Property HabilitarCodigoCIIU() As Boolean
		Get
			Return _HabilitarCodigoCIIU And Editando
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCodigoCIIU = value
			MyBase.CambioItem("HabilitarCodigoCIIU")
		End Set
	End Property

	Private _HabilitarCorreo As Boolean = False
	Public Property HabilitarCorreo() As Boolean
		Get
			Return _HabilitarCorreo And Editando
		End Get
		Set(ByVal value As Boolean)
			_HabilitarCorreo = value
			MyBase.CambioItem("HabilitarCorreo")
		End Set
	End Property

	Private _ListaDetalle As List(Of CPX_FormularioEndeudamientoDetalle)
	Public Property ListaDetalle() As List(Of CPX_FormularioEndeudamientoDetalle)
		Get
			Return _ListaDetalle
		End Get
		Set(ByVal value As List(Of CPX_FormularioEndeudamientoDetalle))
			_ListaDetalle = value
			MyBase.CambioItem("ListaDetalle")
			'CalcularCampos()
		End Set
	End Property

	Private _ListaDetalleEliminar As List(Of CPX_FormularioEndeudamientoDetalle)
	Public Property ListaDetalleEliminar() As List(Of CPX_FormularioEndeudamientoDetalle)
		Get
			Return _ListaDetalleEliminar
		End Get
		Set(ByVal value As List(Of CPX_FormularioEndeudamientoDetalle))
			_ListaDetalleEliminar = value
			MyBase.CambioItem("ListaDetalleEliminar")
		End Set
	End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
	Public Sub Imprimir()
		Dim URL As String = Program.RutaServicioNegocio("URL_SERVICIO_REPORTING") & strNombreReporte
		Dim wb = New WebBrowser()
		Dim ie = Activator.CreateInstance(Type.GetTypeFromProgID("InternetExplorer.Application"))
		ie.AddressBar = False
		ie.MenuBar = False
		ie.ToolBar = False

		ie.Visible = True
		ie.Navigate(URL)

	End Sub

	''' <summary>
	''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
	''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Sub NuevoRegistro()
		Try
			Dim objNvoEncabezado As New tblFormularioEndeudamientoExterno
			If mdcProxy.IsLoading Then
				A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Exit Sub
			End If


			'Obtiene el registro por defecto
			ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
			'Salva el registro anterior
			ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

			Editando = True
			MyBase.CambioItem("Editando")
			MyBase.CambioItem("HabilitarEdicionDetalle")

			EncabezadoEdicionSeleccionado = objNvoEncabezado
			ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISASF6F7").Where(Function(i) i.Retorno <> TipoOperacion.Modificacion).ToList()
			MyBase.NuevoRegistro()

		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
	''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Async Sub Filtrar()
		Try
			Await ConsultarEncabezado("FILTRAR", FiltroVM)
			strTipoFiltroBusqueda = "FILTRAR"
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Overrides Sub QuitarFiltro()
		MyBase.QuitarFiltro()
		strTipoFiltroBusqueda = String.Empty
	End Sub

	''' <summary>
	''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
	''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
	''' </summary>
	Public Overrides Sub Buscar()
		PrepararNuevaBusqueda()
		MyBase.Buscar()
	End Sub

	''' <summary>
	''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
	''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Async Sub ConfirmarBuscar()
		Try
			Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.ID, cb.Fecha, cb.Nombre, cb.Documento)
			strTipoFiltroBusqueda = "BUSQUEDA"
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
	''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Async Sub ActualizarRegistro()
		Dim strAccion As String = ValoresUserState.Actualizar.ToString()

		Try
			MyBase.ActualizarRegistro()

			ErrorForma = String.Empty
			IsBusy = True
			Dim strDetalle As String = String.Empty
			Dim strDetalleEliminar As String = String.Empty

			If CodigoBancRepSeleccionado Is Nothing And Not String.IsNullOrWhiteSpace(mobjView.acbCodigoAcreedor.SearchText) Then
				_EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor = mobjView.acbCodigoAcreedor.SearchText
			End If


			If Not IsNothing(_ListaDetalle) Then
				strDetalle = "<Detalles>  "
				For Each objDet In _ListaDetalle
					Dim strXMLDetalle = <Detalle intID=<%= objDet.intID %>
											intIDFormulario=<%= objDet.intIDFormulario %>
											intNumero=<%= objDet.intNumero %>
											dtmFecha=<%= objDet.dtmFecha %>
											dblValorMonedaContratada=<%= objDet.dblValorMonedaContratada %>
											dtmActualizacion=<%= objDet.dtmActualizacion %>
											strUsuario=<%= objDet.strUsuario %>
											>
										</Detalle>

					strDetalle = strDetalle & strXMLDetalle.ToString
				Next
				strDetalle = strDetalle & " </Detalles>"
			End If

			If Not IsNothing(_ListaDetalleEliminar) Then
				strDetalleEliminar = "<Detalles>  "
				For Each objDet In _ListaDetalleEliminar
					Dim strXMLDetalleEliminar = <Detalle intID=<%= objDet.intID %>
													>
												</Detalle>

					strDetalleEliminar = strDetalleEliminar & strXMLDetalleEliminar.ToString
				Next
				strDetalleEliminar = strDetalleEliminar & " </Detalles>"
			End If

			Dim strXMLDetFormulario2 As String = System.Web.HttpUtility.UrlEncode(strDetalle)
			Dim strXMLDetEliminarFormulario2 As String = System.Web.HttpUtility.UrlEncode(strDetalleEliminar)

			Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.Formulario6_ActualizarAsync(
																				_EncabezadoEdicionSeleccionado.intID,
																				_EncabezadoEdicionSeleccionado.intFormulario,
																				_EncabezadoEdicionSeleccionado.logEnviado,
																				_EncabezadoEdicionSeleccionado.intTipoOperacion,
																				_EncabezadoEdicionSeleccionado.dtmFecha,
																				_EncabezadoEdicionSeleccionado.intNumeroPrestamo,
																				_EncabezadoEdicionSeleccionado.strNumeroIdentifiacion,
																				_EncabezadoEdicionSeleccionado.logDesembolso,
																				_EncabezadoEdicionSeleccionado.intNumeroDeclaracion,
																				_EncabezadoEdicionSeleccionado.intNumeral,
																				_EncabezadoEdicionSeleccionado.intIDMoneda,
																				_EncabezadoEdicionSeleccionado.dblValorMonedaNegociada,
																				_EncabezadoEdicionSeleccionado.dblValorUSD,
																				_EncabezadoEdicionSeleccionado.strTipoIdentificacionDeudor,
																				_EncabezadoEdicionSeleccionado.strNumeroIdentificacionDeudor,
																				_EncabezadoEdicionSeleccionado.intDigitoVerificacionDeudor,
																				_EncabezadoEdicionSeleccionado.strNombreDeudor,
																				_EncabezadoEdicionSeleccionado.intIDCiudadDeudor,
																				_EncabezadoEdicionSeleccionado.strDireccionDeudor,
																				_EncabezadoEdicionSeleccionado.strTelefonoDeudor,
																				_EncabezadoEdicionSeleccionado.strEmailDeudor,
																				_EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor,
																				_EncabezadoEdicionSeleccionado.strCodigoBanRepAcreedor,
																				_EncabezadoEdicionSeleccionado.strNombreAcreedor,
																				_EncabezadoEdicionSeleccionado.strPais,
																				_EncabezadoEdicionSeleccionado.strTipoAcreedor,
																				_EncabezadoEdicionSeleccionado.strCodigoPropositoPrestamo,
																				_EncabezadoEdicionSeleccionado.intIDMonedaPrestamo,
																				_EncabezadoEdicionSeleccionado.dblMontoContratado,
																				_EncabezadoEdicionSeleccionado.strTasaInteres,
																				_EncabezadoEdicionSeleccionado.dblSpreadValorInteres,
																				_EncabezadoEdicionSeleccionado.strNumeroDepositoFinan,
																				_EncabezadoEdicionSeleccionado.logIndexacion,
																				_EncabezadoEdicionSeleccionado.intIDMonedaIndexacion,
																				_EncabezadoEdicionSeleccionado.logSustitucion,
																				_EncabezadoEdicionSeleccionado.logFraccionamiento,
																				_EncabezadoEdicionSeleccionado.logConsolidacion,
																				_EncabezadoEdicionSeleccionado.intNUmeroIDCreditoAnterior1,
																				_EncabezadoEdicionSeleccionado.intIDMonedaAnterior1,
																				_EncabezadoEdicionSeleccionado.dblValorAnterior1,
																				_EncabezadoEdicionSeleccionado.intNUmeroIDCreditoAnterior2,
																				_EncabezadoEdicionSeleccionado.intIDMonedaAnterior2,
																				_EncabezadoEdicionSeleccionado.dblValorAnterior2,
																				_EncabezadoEdicionSeleccionado.strNombre,
																				_EncabezadoEdicionSeleccionado.strNumeroIDDeclarante,
																				_EncabezadoEdicionSeleccionado.strFirma,
																				_EncabezadoEdicionSeleccionado.dtmActualizacion,
																				strDetalle,
																				strDetalleEliminar,
																				False,
																				Program.Usuario)

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
					Dim objListaMensajes As New List(Of ProductoValidaciones)
					Dim intIDRegistroActualizado As Integer = -1

					For Each li In objListaResultado
						objListaMensajes.Add(New ProductoValidaciones With {
											 .Campo = li.strCampo,
											 .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
											 .Mensaje = li.strMensaje
											 })
					Next


					If (objListaResultado.All(Function(i) i.logInconsitencia = False)) Then

						A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
						intIDRegistroActualizado = objListaResultado.First.intIDRegistro

						MyBase.FinalizoGuardadoRegistros()
						CambioItem("HabilitarNombreAcreedor")
						CambioItem("HabilitarPaisAcreedor")
						CambioItem("HabilitarTipoAcreedor")
						' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
						If strTipoFiltroBusqueda = "FILTRAR" Then
							Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
						ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
							Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.ID, cb.Fecha)
						Else
							Await ConsultarEncabezado("FILTRAR", String.Empty)
						End If
					Else
						Dim strMensaje As String = ""
						For Each m In objListaMensajes
							strMensaje += IIf(String.IsNullOrWhiteSpace(m.TituloCampo), "", m.TituloCampo + ":") + m.Mensaje + vbCrLf
						Next
						A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
						MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema, False)
						IsBusy = False
					End If
				Else
					A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
					IsBusy = False
				End If
			Else
				A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
				IsBusy = False
			End If

		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
	''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
	''' </summary>
	Public Overrides Sub EditarRegistro()
		Try
			If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
				ModificarListaOperacion()
				If mdcProxy.IsLoading Then
					A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Exit Sub
				End If
				IsBusy = True

				'If _EncabezadoEdicionSeleccionado.logEnviado Then
				'	A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("DIVISAS_FORMULARIOYAENVIADO"), DiccionarioMensajesPantalla("DIVISAS_TITULOFORMULARIOYAENVIADO"), "1", AddressOf TerminoPregunta)
				'Else
				IniciaEditar()
				'End If

				'ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

				'Editando = True
				'MyBase.CambioItem("Editando")
				'MyBase.CambioItem("HabilitarEdicionDetalle")
				'MyBase.EditarRegistro()

				IsBusy = False
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Sub PreguntarModificacion()
		If _EncabezadoEdicionSeleccionado IsNot Nothing Then
			If Editando And _EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion Then
				A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("DIVISAS_FORMULARIOYAENVIADO"), DiccionarioMensajesPantalla("DIVISAS_TITULOFORMULARIOYAENVIADO"), "1", AddressOf ModificarFormulario)
			End If
		End If
	End Sub

	Public Sub ModificarFormulario(ByVal sender As Object, ByVal e As EventArgs)

		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
			If objResultado.DialogResult Then
				If Editando And _EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion Then
					_EncabezadoEdicionSeleccionado.dtmFecha = Now
					_EncabezadoEdicionSeleccionado.strNumeroIdentifiacion = _EncabezadoEdicionSeleccionado.strNumeroIdentificacionDeudor & IIf(_EncabezadoEdicionSeleccionado.intDigitoVerificacionDeudor IsNot Nothing, _EncabezadoEdicionSeleccionado.intDigitoVerificacionDeudor.ToString, String.Empty)
				Else
					_EncabezadoEdicionSeleccionado.strNumeroIdentifiacion = Nothing
					If mobjEncabezadoAnterior IsNot Nothing Then
						_EncabezadoEdicionSeleccionado.dtmFecha = mobjEncabezadoAnterior.dtmFecha
					End If

				End If
			Else
				_EncabezadoEdicionSeleccionado.intTipoOperacion = mobjEncabezadoAnterior.intTipoOperacion
			End If
			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try

	End Sub

	Public Async Sub IniciaEditar()
		ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)
		'Se comenta esto ya que el formulario 6 no tiene sección de la declaracion anterior
		If _EncabezadoEdicionSeleccionado.logEnviado Then
			'	_EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion
			Dim Consecutivo = Await mdcProxy.ConsecutivoFormularioAsync()
			'	_EncabezadoEdicionSeleccionado.lngNitAnterior = _EncabezadoEdicionSeleccionado.lngNit
			'	_EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior = _EncabezadoEdicionSeleccionado.intNumeroBcoRep
			'	_EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior = _EncabezadoEdicionSeleccionado.intNumeroBcoRep
			'	_EncabezadoEdicionSeleccionado.dtmFechaBcoRepAnterior = _EncabezadoEdicionSeleccionado.dtmFechaBcoRep
			'	_EncabezadoEdicionSeleccionado.intNumeroBcoRep = Consecutivo.Value
			'	_EncabezadoEdicionSeleccionado.dtmFechaBcoRep = Today
			'	_EncabezadoEdicionSeleccionado.blnEnviado = False

		End If
		Editando = True
		MyBase.CambioItem("Editando")
		MyBase.CambioItem("HabilitarEdicionDetalle")
		CambioItem("HabilitarNombreAcreedor")
		CambioItem("HabilitarPaisAcreedor")
		CambioItem("HabilitarTipoAcreedor")
		MyBase.EditarRegistro()
	End Sub


	Public Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
			If objResultado.DialogResult Then
				IniciaEditar()
			End If
			IsBusy = False
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
	''' </summary>
	Public Overrides Sub CancelarEditarRegistro()
		Try
			ErrorForma = String.Empty
			mdcProxy.RejectChanges()
			Editando = False
			MyBase.CambioItem("Editando")
			MyBase.CambioItem("HabilitarEdicionDetalle")
			MyBase.CancelarEditarRegistro()
			mobjEncabezadoAnterior.strCodigoBanRepAcreedor = strCodigoAcreedorOrdiginal
			If mobjEncabezadoAnterior IsNot Nothing Then
				EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
	''' </summary>
	''' <history>
	''' </history>
	Public Overrides Sub BorrarRegistro()
		Try
			MyBase.BorrarRegistro()

			If Not IsNothing(_EncabezadoSeleccionado) Then
				If mdcProxy.IsLoading Then
					A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Exit Sub
				End If

				A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ELIMINARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim strAccion As String = ValoresUserState.Actualizar.ToString()

			If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
				If Not IsNothing(_EncabezadoSeleccionado) Then
					IsBusy = True

					Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.Formulario6_EliminarAsync(_EncabezadoSeleccionado.intID, Program.Usuario)

					If Not IsNothing(objRespuesta) Then
						If Not IsNothing(objRespuesta.Value) Then
							Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
							Dim objListaMensajes As New List(Of ProductoValidaciones)

							For Each li In objListaResultado
								objListaMensajes.Add(New ProductoValidaciones With {
											 .Campo = li.strCampo,
											 .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
											 .Mensaje = li.strMensaje
											 })
							Next
							If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
								A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

								EncabezadoSeleccionado = Nothing

								Await ConsultarEncabezado(True, String.Empty)
							Else
								MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
								IsBusy = False
							End If
						Else
							A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
							IsBusy = False
						End If
					Else
						A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
						IsBusy = False
					End If
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
	''' <summary>
	''' Consulta los valores por defecto para un nuevo encabezado
	''' </summary>
	''' 
	Private Sub ModificarListaOperacion()
		Try
			ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISASF6F7").ToList
			'If _EncabezadoEdicionSeleccionado IsNot Nothing Then
			'	If Not (_EncabezadoEdicionSeleccionado.logEnviado Or _EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion) Then
			'		ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISASF6F7").Where(Function(i) i.Retorno <> TipoOperacion.Modificacion).ToList()
			'	End If
			'End If

		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ModificarListaOperacion", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub


	Private Async Function ConsultarNumerales() As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
			Dim objRespuesta As InvokeResult(Of List(Of CPX_NumeralesCambiarios)) = Nothing

			ErrorForma = String.Empty

			objRespuesta = Await mdcProxy.NumeralesCambiarios_FiltrarAsync(String.Empty, 6, Program.Usuario)

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					Dim objListaProductos As List(Of CPX_NumeralesCambiarios) = objRespuesta.Value

					If Not IsNothing(objListaProductos) Then
						Dim objListaProductosCombo As New List(Of BusquedaNumeralesCambiarios)
						For Each li In objListaProductos
                            objListaProductosCombo.Add(New BusquedaNumeralesCambiarios With {
                                .lngID = li.intIDNumeral,
                                .strConcatenacion = li.strConcatenacion
                                })
                        Next

						ListaNumerales = objListaProductosCombo
					End If
				Else
					A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
				End If
			Else
				A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
			End If

			logResultado = True
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return (logResultado)
	End Function
	Private Async Sub consultarEncabezadoPorDefecto()
		Try
			Dim objRespuesta As InvokeResult(Of tblFormularioEndeudamientoExterno)

			objRespuesta = Await mdcProxy.Formulario6_DefectoAsync(Program.Usuario)

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					mobjEncabezadoPorDefecto = objRespuesta.Value
				Else
					A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
				End If
			Else
				A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Consultar de forma sincrónica los datos de Registro
	''' </summary>
	''' <param name="pstrOpcion">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
	Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
											   Optional ByVal pstrFiltro As String = "",
											   Optional ByVal pintID As String = Nothing,
											   Optional ByVal pdtmFecha As Date? = Nothing,
											   Optional ByVal pstrNombre As String = Nothing,
											   Optional ByVal pstrNumeroIDDeclarante As String = Nothing) As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
			Dim objRespuesta As InvokeResult(Of List(Of CPX_tblFormularioEndeudamientoExterno)) = Nothing

			ErrorForma = String.Empty

			If pstrOpcion = "FILTRAR" Then
				pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
				objRespuesta = Await mdcProxy.Formulario6_FiltrarAsync(pstrFiltro, Program.Usuario)
			ElseIf pstrOpcion = "ID" Then
				objRespuesta = Await mdcProxy.Formulario6_ConsultarAsync(pintID, pdtmFecha, pstrNombre, pstrNumeroIDDeclarante, Program.Usuario)
			Else
				objRespuesta = Await mdcProxy.Formulario6_ConsultarAsync(pintID, pdtmFecha, pstrNombre, pstrNumeroIDDeclarante, Program.Usuario)

			End If

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					ListaEncabezado = objRespuesta.Value

					If pintID > 0 Then
						If ListaEncabezado.Where(Function(i) i.intID = pintID).Count > 0 Then
							EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intID = pintID).First
						End If
					End If

					If ListaEncabezado.Count > 0 Then
						If pstrOpcion = "BUSQUEDA" Then
							MyBase.ConfirmarBuscar()
						End If
					Else
						If (pstrOpcion = "FILTRAR" And Not String.IsNullOrEmpty(pstrFiltro)) Or (pstrOpcion = "BUSQUEDA") Then
							' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
							A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_BUSQUEDANOEXITOSA"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return (logResultado)
	End Function

	Public Async Sub ConsultarEncabezadoEdicion()
		Try
			EncabezadoEdicionSeleccionado = Nothing

			If Not IsNothing(_EncabezadoSeleccionado) Then
				If _EncabezadoSeleccionado.intID > 0 Then
					Await ConsultarEncabezadoEdicion(_EncabezadoSeleccionado.intID)
				End If
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
	Private Async Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
			Dim objRespuesta As InvokeResult(Of tblFormularioEndeudamientoExterno) = Nothing

			ErrorForma = String.Empty

			objRespuesta = Await mdcProxy.Formulario6_IDAsync(pintID, Program.Usuario)

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					EncabezadoEdicionSeleccionado = objRespuesta.Value
				Else
					A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
				End If
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

	Public Async Sub FiltrarClientes(strFiltro As String)
		Dim objResultado = Await mdcProxy.Filtrar_ClientesAsync(strFiltro, Program.Usuario)
		ListaClientes = objResultado.Value
		If ListaClientes.Count = 1 Then
			ClienteSeleccionado = ListaClientes.First
		ElseIf ListaClientes.Any(Function(i) i.strNroDocumento = strFiltro) Then
			ClienteSeleccionado = ListaClientes.Where(Function(i) i.strNroDocumento = strFiltro).First
		End If
		'ClienteSeleccionado = ListaClientes.FirstOrDefault
	End Sub
#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
	''' <summary>
	''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
	''' </summary>
	Private Sub PrepararNuevaBusqueda()
		Try
			Dim objCB As New CamposBusquedaFormulario6
			objCB.ID = 0
			objCB.Fecha = Nothing
			cb = objCB

		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Retorna una copia del encabezado activo. 
	''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
	''' </summary>
	''' <history>
	''' </history>
	Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblFormularioEndeudamientoExterno, ByRef pobjRegistroSalvar As tblFormularioEndeudamientoExterno)
		Dim objEncabezado As tblFormularioEndeudamientoExterno = New tblFormularioEndeudamientoExterno

		Try
			If Not IsNothing(pobjRegistro) Then

				objEncabezado.dblMontoContratado = pobjRegistro.dblMontoContratado
				objEncabezado.dblSpreadValorInteres = pobjRegistro.dblSpreadValorInteres
				objEncabezado.dblValorAnterior1 = pobjRegistro.dblValorAnterior1
				objEncabezado.dblValorAnterior2 = pobjRegistro.dblValorAnterior2
				objEncabezado.dblValorMonedaNegociada = pobjRegistro.dblValorMonedaNegociada
				objEncabezado.dblValorUSD = pobjRegistro.dblValorUSD
				objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
				objEncabezado.dtmFecha = pobjRegistro.dtmFecha
				objEncabezado.strCodigoPropositoPrestamo = pobjRegistro.strCodigoPropositoPrestamo
				objEncabezado.intDigitoVerificacionDeudor = pobjRegistro.intDigitoVerificacionDeudor
				objEncabezado.intFormulario = pobjRegistro.intFormulario
				objEncabezado.intID = pobjRegistro.intID
				objEncabezado.intIDCiudadDeudor = pobjRegistro.intIDCiudadDeudor
				objEncabezado.intIDMoneda = pobjRegistro.intIDMoneda
				objEncabezado.intIDMonedaAnterior1 = pobjRegistro.intIDMonedaAnterior1
				objEncabezado.intIDMonedaAnterior2 = pobjRegistro.intIDMonedaAnterior2
				objEncabezado.intIDMonedaIndexacion = pobjRegistro.intIDMonedaIndexacion
				objEncabezado.intIDMonedaPrestamo = pobjRegistro.intIDMonedaPrestamo
				objEncabezado.strPais = pobjRegistro.strPais
				objEncabezado.strTasaInteres = pobjRegistro.strTasaInteres
				objEncabezado.intNumeral = pobjRegistro.intNumeral
				objEncabezado.intNumeroDeclaracion = pobjRegistro.intNumeroDeclaracion
				objEncabezado.strNumeroDepositoFinan = pobjRegistro.strNumeroDepositoFinan
				objEncabezado.intNUmeroIDCreditoAnterior1 = pobjRegistro.intNUmeroIDCreditoAnterior1
				objEncabezado.intNUmeroIDCreditoAnterior2 = pobjRegistro.intNUmeroIDCreditoAnterior2
				objEncabezado.intNumeroPrestamo = pobjRegistro.intNumeroPrestamo
				objEncabezado.strTipoIdentificacionDeudor = pobjRegistro.strTipoIdentificacionDeudor
				objEncabezado.intTipoOperacion = pobjRegistro.intTipoOperacion
				objEncabezado.logConsolidacion = pobjRegistro.logConsolidacion
				objEncabezado.logDesembolso = pobjRegistro.logDesembolso
				objEncabezado.logEnviado = pobjRegistro.logEnviado
				objEncabezado.logFraccionamiento = pobjRegistro.logFraccionamiento
				objEncabezado.logIndexacion = pobjRegistro.logIndexacion
				objEncabezado.logSustitucion = pobjRegistro.logSustitucion
				objEncabezado.strCodigoBanRepAcreedor = pobjRegistro.strCodigoBanRepAcreedor
				objEncabezado.strDireccionDeudor = pobjRegistro.strDireccionDeudor
				objEncabezado.strEmailDeudor = pobjRegistro.strEmailDeudor
				objEncabezado.strFirma = pobjRegistro.strFirma
				objEncabezado.strIDCodigoCIIUDeudor = pobjRegistro.strIDCodigoCIIUDeudor
				objEncabezado.strNombre = pobjRegistro.strNombre
				objEncabezado.strNombreAcreedor = pobjRegistro.strNombreAcreedor
				objEncabezado.strNombreDeudor = pobjRegistro.strNombreDeudor
				objEncabezado.strNumeroIDDeclarante = pobjRegistro.strNumeroIDDeclarante
				objEncabezado.strNumeroIdentifiacion = pobjRegistro.strNumeroIdentifiacion
				objEncabezado.strNumeroIdentificacionDeudor = pobjRegistro.strNumeroIdentificacionDeudor
				objEncabezado.strTelefonoDeudor = pobjRegistro.strTelefonoDeudor
				objEncabezado.strTipoAcreedor = pobjRegistro.strTipoAcreedor
				objEncabezado.strUsuario = pobjRegistro.strUsuario


				pobjRegistroSalvar = objEncabezado
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Modificación de las propeiedades del encabezado.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	Private Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
		Try
			If Editando Then
				If e.PropertyName = "dblDeducciones" Then
					'CalcularCampos()
				ElseIf e.PropertyName = "strIDCodigoCIIUDeudor" Then
					If _EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor Is Nothing Or _EncabezadoEdicionSeleccionado.strIDCodigoCIIUDeudor = "0" Then
						VisibilityAbcCodigoCiiU = Visibility.Visible
						VisibilityTxtCodigoCiiU = Visibility.Collapsed
					Else
						VisibilityAbcCodigoCiiU = Visibility.Collapsed
						VisibilityTxtCodigoCiiU = Visibility.Visible
					End If
				ElseIf e.PropertyName = "intTipoOperacion" Then

				End If
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	'Private Sub CalcularCampos()
	'	If _ListaDetalle IsNot Nothing Then
	'		If _EncabezadoEdicionSeleccionado IsNot Nothing Then
	'			_EncabezadoEdicionSeleccionado.dblTotalvalorFOB = (From det In ListaDetalle Where det.intNumeral <> 1510 Select det.dblValorUSD).Sum
	'			_EncabezadoEdicionSeleccionado.dblTotalGastosExportacion = (From det In ListaDetalle Where det.intNumeral = 1510 Select det.dblValorUSD).Sum
	'			_EncabezadoEdicionSeleccionado.dblReintegroNeto = _EncabezadoEdicionSeleccionado.dblTotalvalorFOB + _EncabezadoEdicionSeleccionado.dblTotalGastosExportacion - IIf(_EncabezadoEdicionSeleccionado.dblDeducciones Is Nothing, 0, _EncabezadoEdicionSeleccionado.dblDeducciones)
	'		End If
	'	End If
	'End Sub



#End Region

#Region "Comandos"
	Private WithEvents _ImprimirCmd As RelayCommand
	Public ReadOnly Property ImprimirCmd() As RelayCommand
		Get
			If _ImprimirCmd Is Nothing Then
				_ImprimirCmd = New RelayCommand(AddressOf Imprimir)
			End If
			Return _ImprimirCmd
		End Get

	End Property
#End Region

End Class


''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' <history>
''' 
''' </history>
Public Class CamposBusquedaFormulario6
	Implements INotifyPropertyChanged

	Private _ID As String
	Public Property ID() As String
		Get
			Return _ID
		End Get
		Set(ByVal value As String)
			_ID = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
		End Set
	End Property

	Private _Fecha As Nullable(Of Date)
	Public Property Fecha() As Nullable(Of Date)
		Get
			Return _Fecha
		End Get
		Set(ByVal value As Nullable(Of Date))
			_Fecha = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fecha"))
		End Set
	End Property

	Private _Nombre As String
	Public Property Nombre() As String
		Get
			Return _Nombre
		End Get
		Set(ByVal value As String)
			_Nombre = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
		End Set
	End Property

	Private _Documento As String
	Public Property Documento() As String
		Get
			Return _Documento
		End Get
		Set(ByVal value As String)
			_Documento = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Documento"))
		End Set
	End Property

	Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
