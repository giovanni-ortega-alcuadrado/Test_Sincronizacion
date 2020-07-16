Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel

Public Class Formulario1ViewModel
	Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblFormulariosBcoRepublica1
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public mobjEncabezadoAnterior As tblFormulariosBcoRepublica1
    Dim strTipoFiltroBusqueda As String = String.Empty

	Public Enum TipoOperacion
		Inicial = 1
		Devolucion = 2
		Cambio = 3
		Modificacion = 4
	End Enum

	Private strNombreReporte As String = "Formulario1VB"

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



				Await consultarEncabezadoPorDefecto()
				Await ConsultarNumerales()
				ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISAS").ToList

				' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
				Await ConsultarEncabezado("FILTRAR", String.Empty)
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
	''' 
	''' 
	''' </summary>
	''' 
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
	Private _ListaNumerales As List(Of BusquedaNumeralesCambiarios)
	Public Property ListaNumerales() As List(Of BusquedaNumeralesCambiarios)
		Get
			Return _ListaNumerales
		End Get
		Set(ByVal value As List(Of BusquedaNumeralesCambiarios))
			_ListaNumerales = value
			MyBase.CambioItem("ListaNumerales")
		End Set
	End Property
	Private _ListaEncabezado As List(Of CPX_Formulario1)
	Public Property ListaEncabezado() As List(Of CPX_Formulario1)
		Get
			Return _ListaEncabezado
		End Get
		Set(ByVal value As List(Of CPX_Formulario1))
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
	Private _EncabezadoSeleccionado As CPX_Formulario1
	Public Property EncabezadoSeleccionado() As CPX_Formulario1
		Get
			Return _EncabezadoSeleccionado
		End Get
		Set(ByVal value As CPX_Formulario1)
			_EncabezadoSeleccionado = value
			'Llamado de metodo para realizar la consulta del encabezado editable
			ConsultarEncabezadoEdicion()
			MyBase.CambioItem("EncabezadoSeleccionado")
		End Set
	End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblFormulariosBcoRepublica1
    Public Property EncabezadoEdicionSeleccionado() As tblFormulariosBcoRepublica1
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblFormulariosBcoRepublica1)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If _EncabezadoEdicionSeleccionado.intNumeroDecl > 0 Then
                    HabilitarEdicionDetalle = True
                Else
                    HabilitarEdicionDetalle = False
                End If
                'ModificarListaOperacion()
            Else
                HabilitarEdicionDetalle = False
            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaFormulario1
	Public Property cb() As CamposBusquedaFormulario1
		Get
			Return _cb
		End Get
		Set(ByVal value As CamposBusquedaFormulario1)
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

	Private _ListaDetalle As List(Of CPX_tblFormulario1DetalleDian)
	Public Property ListaDetalle() As List(Of CPX_tblFormulario1DetalleDian)
		Get
			Return _ListaDetalle
		End Get
		Set(ByVal value As List(Of CPX_tblFormulario1DetalleDian))
			_ListaDetalle = value
			MyBase.CambioItem("ListaDetalle")
		End Set
	End Property

	Private _ListaDetalleEliminar As List(Of CPX_tblFormulario1DetalleDian)
	Public Property ListaDetalleEliminar() As List(Of CPX_tblFormulario1DetalleDian)
		Get
			Return _ListaDetalleEliminar
		End Get
		Set(ByVal value As List(Of CPX_tblFormulario1DetalleDian))
			_ListaDetalleEliminar = value
			MyBase.CambioItem("ListaDetalleEliminar")
		End Set
	End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
	Public Sub Imprimir()
		'Dim URL As String = My.Settings.SERVIDOR_REPORTES & "/Pages/ReportViewer.aspx?/" & My.Settings.CARPETA_REPORTES & "/" & strNombreReporte
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
            Dim objNvoEncabezado As New tblFormulariosBcoRepublica1
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
			Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.ID, cb.Fecha, cb.Nombre, cb.Orden, cb.Documento)
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

			If Not IsNothing(_ListaDetalle) Then
				strDetalle = "<Detalles>  "
				For Each objDet In _ListaDetalle
					Dim strXMLDetalle = <Detalle intID=<%= objDet.intID %>
											intIDDecl=<%= objDet.intIDDecl %>
											dtmFechaDecl=<%= objDet.dtmFechaDecl %>
											dblValorUSD=<%= objDet.dblValorUSD %>
											strNumero=<%= objDet.strNumero %>
											dtmFechaPresentacion=<%= objDet.dtmFechaPresentacion %>
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
			Dim strXMLDetFormulario1 As String = System.Web.HttpUtility.UrlEncode(strDetalle)
			Dim strXMLDetEliminarFormulario1 As String = System.Web.HttpUtility.UrlEncode(strDetalleEliminar)

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.Formulario1_ActualizarAsync(_EncabezadoEdicionSeleccionado.intNumeroDecl,
                                                                                _EncabezadoEdicionSeleccionado.intFormulario,
                                                                                _EncabezadoEdicionSeleccionado.strCondicionNegocio,
                                                                                _EncabezadoEdicionSeleccionado.intTipoOperacion,
                                                                                _EncabezadoEdicionSeleccionado.intCiudad,
                                                                                _EncabezadoEdicionSeleccionado.strNit,
                                                                                _EncabezadoEdicionSeleccionado.dtmFecha,
                                                                                _EncabezadoEdicionSeleccionado.strNitAnterior,
                                                                                _EncabezadoEdicionSeleccionado.dtmFechaAnterior,
                                                                                _EncabezadoEdicionSeleccionado.intNumeroDeclAnterior,
                                                                                _EncabezadoEdicionSeleccionado.strTipoDocumento,
                                                                                _EncabezadoEdicionSeleccionado.intNumeroidentificacion,
                                                                                _EncabezadoEdicionSeleccionado.intDigitoVerificacion,
                                                                                _EncabezadoEdicionSeleccionado.strRazonSocial,
                                                                                _EncabezadoEdicionSeleccionado.intCiudadDomicilio,
                                                                                _EncabezadoEdicionSeleccionado.strCondiccionespago,
                                                                                _EncabezadoEdicionSeleccionado.strCondiccionesDespacho,
                                                                                _EncabezadoEdicionSeleccionado.strNombreDeclarante,
                                                                                _EncabezadoEdicionSeleccionado.strNumeroIdentDeclarante,
                                                                                _EncabezadoEdicionSeleccionado.logEnviado,
                                                                                _EncabezadoEdicionSeleccionado.strObservaciones,
                                                                                _EncabezadoEdicionSeleccionado.dblTotalvalorFOB,
                                                                                _EncabezadoEdicionSeleccionado.dblTotalGastosExportacion,
                                                                                _EncabezadoEdicionSeleccionado.dblDeducciones,
                                                                                _EncabezadoEdicionSeleccionado.dblReintegroNeto,
                                                                                _EncabezadoEdicionSeleccionado.strTelefono,
                                                                                _EncabezadoEdicionSeleccionado.strDireccion,
                                                                                _EncabezadoEdicionSeleccionado.strCodigoMoneda1,
                                                                                _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1,
                                                                                _EncabezadoEdicionSeleccionado.intNumeralcambiario1,
                                                                                _EncabezadoEdicionSeleccionado.dblValormoneda1,
                                                                                _EncabezadoEdicionSeleccionado.dblValorUSD1,
                                                                                _EncabezadoEdicionSeleccionado.strCodigoMoneda2,
                                                                                _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda2,
                                                                                _EncabezadoEdicionSeleccionado.intNumeralcambiario2,
                                                                                _EncabezadoEdicionSeleccionado.dblValormoneda2,
                                                                                _EncabezadoEdicionSeleccionado.dblValorUSD2,
                                                                                _EncabezadoEdicionSeleccionado.strGlobal,
                                                                                _EncabezadoEdicionSeleccionado.strCorreccion,
                                                                                _EncabezadoEdicionSeleccionado.strIngresoEgreso,
                                                                                _EncabezadoEdicionSeleccionado.intIDOperacion,
                                                                                _EncabezadoEdicionSeleccionado.strNombreDeclaranteFirma,
                                                                                _EncabezadoEdicionSeleccionado.intNumeroDeclAnteriorFirma,
                                                                                _EncabezadoEdicionSeleccionado.strEstado,
                                                                                _EncabezadoEdicionSeleccionado.strTipoReferencia,
                                                                                _EncabezadoEdicionSeleccionado.dtmFechaActualizacion,
                                                                                _EncabezadoEdicionSeleccionado.logEnviadoDian,
                                                                                _EncabezadoEdicionSeleccionado.strCorreoElectronicoDecl,
                                                                                _EncabezadoEdicionSeleccionado.strCodigoCiudadDecl,
                                                                                _EncabezadoEdicionSeleccionado.logFirmaFormPend,
                                                                                _EncabezadoEdicionSeleccionado.intDeclaracionConsolidado,
                                                                                _EncabezadoEdicionSeleccionado.intNumeroBcoRep,
                                                                                _EncabezadoEdicionSeleccionado.dtmFechaBcoRep,
                                                                                strDetalle,
                                                                                strDetalleEliminar,
                                                                                False,
                                                                                Program.Usuario)

            '_EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior,
            '_EncabezadoEdicionSeleccionado.dtmFechaBcoRepAnterior,


            If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
					Dim objListaMensajes As New List(Of ProductoValidaciones)
					Dim intIDRegistroActualizado As Integer = -1
					Dim dtmRegistroActualizado As Date = Now

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
						dtmRegistroActualizado = objListaResultado.First.dtmRegistro
						MyBase.FinalizoGuardadoRegistros()

						' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
						If strTipoFiltroBusqueda = "FILTRAR" Then
							Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado, dtmRegistroActualizado)
						ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
							Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.ID, cb.Fecha)
						Else
							Await ConsultarEncabezado("FILTRAR", String.Empty, intIDRegistroActualizado, dtmRegistroActualizado)
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
				'ModificarListaOperacion()
				If mdcProxy.IsLoading Then
					A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Exit Sub
				End If
				IsBusy = True

				'If _EncabezadoEdicionSeleccionado.blnEnviado Then
				'	A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("DIVISAS_FORMULARIOYAENVIADO"), DiccionarioMensajesPantalla("DIVISAS_TITULOFORMULARIOYAENVIADO"), "1", AddressOf TerminoPregunta)
				'Else
				IniciaEditar()
				'End If

				'ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

				'Editando = True
				'MyBase.CambioItem("Editando")
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

	Public Async Sub ModificarFormulario(ByVal sender As Object, ByVal e As EventArgs)


		Try
			Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
			If objResultado.DialogResult Then
				If Editando And _EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion Then
					_EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion
					Dim Consecutivo = Await mdcProxy.ConsecutivoFormularioAsync()
                    _EncabezadoEdicionSeleccionado.strNitAnterior = _EncabezadoEdicionSeleccionado.strNit
                    '               _EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior = _EncabezadoEdicionSeleccionado.intNumeroBcoRep
                    '_EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior = _EncabezadoEdicionSeleccionado.intNumeroBcoRep
                    '_EncabezadoEdicionSeleccionado.dtmFechaBcoRepAnterior = _EncabezadoEdicionSeleccionado.dtmFechaBcoRep
                    _EncabezadoEdicionSeleccionado.intNumeroBcoRep = Consecutivo.Value
					_EncabezadoEdicionSeleccionado.dtmFechaBcoRep = Today
                    _EncabezadoEdicionSeleccionado.logEnviado = False

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


	Public Sub IniciaEditar()
		ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)
		'If _EncabezadoEdicionSeleccionado.blnEnviado Then
		'	_EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion
		'	Dim Consecutivo = Await mdcProxy.ConsecutivoFormularioAsync()
		'	_EncabezadoEdicionSeleccionado.lngNitAnterior = _EncabezadoEdicionSeleccionado.lngNit
		'	_EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior = _EncabezadoEdicionSeleccionado.intNumeroBcoRep
		'	_EncabezadoEdicionSeleccionado.intNumeroBcoRepAnterior = _EncabezadoEdicionSeleccionado.intNumeroBcoRep
		'	_EncabezadoEdicionSeleccionado.dtmFechaBcoRepAnterior = _EncabezadoEdicionSeleccionado.dtmFechaBcoRep
		'	_EncabezadoEdicionSeleccionado.intNumeroBcoRep = Consecutivo.Value
		'	_EncabezadoEdicionSeleccionado.dtmFechaBcoRep = Today
		'	_EncabezadoEdicionSeleccionado.blnEnviado = False

		'End If
		Editando = True
		MyBase.CambioItem("Editando")
		MyBase.CambioItem("HabilitarEdicionDetalle")
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

			EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
	''' <summary>
	''' Consulta los valores por defecto para un nuevo encabezado
	''' </summary>
	Private Async Function consultarEncabezadoPorDefecto() As Task
		Try
            Dim objRespuesta As InvokeResult(Of tblFormulariosBcoRepublica1)

            objRespuesta = Await mdcProxy.Formulario1_DefectoAsync(Program.Usuario)

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
	End Function

	''' <summary>
	''' Consultar de forma sincrónica los datos de Registro
	''' </summary>
	''' <param name="pstrOpcion">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
	Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
											   Optional ByVal pstrFiltro As String = "",
											   Optional ByVal plngNumeroDecl As String = Nothing,
											   Optional ByVal pdtmFecha As Date? = Nothing,
											   Optional ByVal pstrNombre As String = Nothing,
											   Optional ByVal pintOrden As String = Nothing,
											   Optional ByVal pstrNumeroIdentificacion As String = Nothing) As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
			Dim objRespuesta As InvokeResult(Of List(Of CPX_Formulario1)) = Nothing

			ErrorForma = String.Empty

			If pstrOpcion = "FILTRAR" Then
				pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
				objRespuesta = Await mdcProxy.Formulario1_FiltrarAsync(pstrFiltro, Program.Usuario)
			ElseIf pstrOpcion = "ID" Then
				objRespuesta = Await mdcProxy.Formulario1_ConsultarAsync(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, Program.Usuario)
			Else
				objRespuesta = Await mdcProxy.Formulario1_ConsultarAsync(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, Program.Usuario)
			End If

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					ListaEncabezado = objRespuesta.Value



                    If plngNumeroDecl > 0 Then
                        If pstrOpcion = "FILTRAR" Then
                            If ListaEncabezado.Where(Function(i) i.intNumeroDecl = plngNumeroDecl And i.dtmFecha = pdtmFecha).Count > 0 Then
                                EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intNumeroDecl = plngNumeroDecl And i.dtmFecha = pdtmFecha).First
                            End If
                        ElseIf pstrOpcion = "ID" Then
                            If ListaEncabezado.Where(Function(i) i.intNumeroDecl = plngNumeroDecl And i.dtmFecha = pdtmFecha).Count > 0 Then
                                EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intNumeroDecl = plngNumeroDecl And i.dtmFecha = pdtmFecha).First
                            End If
                        Else
                            If ListaEncabezado.Where(Function(i) i.intNumeroDecl = plngNumeroDecl).Count > 0 Then
                                EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intNumeroDecl = plngNumeroDecl).First
                            End If
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
                If _EncabezadoSeleccionado.intNumeroDecl > 0 Then
                    Await ConsultarEncabezadoEdicion(_EncabezadoSeleccionado.intNumeroDecl, _EncabezadoSeleccionado.dtmFecha)
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
	''' <param name="plngNumeroDecl">Indica el ID de la entidad a consultar</param>
	''' <param name="pdtmFecha">La fecha de la entidad a consultar y hace parte de la clave primaria</param>
	Private Async Function ConsultarEncabezadoEdicion(ByVal plngNumeroDecl As Integer, ByVal pdtmFecha As Date) As Task(Of Boolean)

		Dim logResultado As Boolean = False

		Try
			IsBusy = True
            Dim objRespuesta As InvokeResult(Of tblFormulariosBcoRepublica1) = Nothing

            ErrorForma = String.Empty

			objRespuesta = Await mdcProxy.Formulario1_IDAsync(plngNumeroDecl, pdtmFecha, Program.Usuario)

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
	Private Async Function ConsultarNumerales() As Task


		Try
			IsBusy = True
			Dim objRespuesta As InvokeResult(Of List(Of CPX_NumeralesCambiarios)) = Nothing

			ErrorForma = String.Empty

			objRespuesta = Await mdcProxy.NumeralesCambiarios_FiltrarAsync(String.Empty, 1, Program.Usuario)

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

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

	End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
	''' <summary>
	''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
	''' </summary>
	''' 
	'Private Sub ModificarListaOperacion()
	'	Try
	'		ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISAS").ToList
	'		If _EncabezadoEdicionSeleccionado IsNot Nothing Then
	'			If Not (_EncabezadoEdicionSeleccionado.blnEnviado Or _EncabezadoEdicionSeleccionado.intTipoOperacion = TipoOperacion.Modificacion) Then
	'				ListaTipoOperacion = DiccionarioCombosPantalla("TIPODIVISAS").Where(Function(i) i.Retorno <> TipoOperacion.Modificacion).ToList()
	'			End If
	'		End If

	'	Catch ex As Exception
	'		IsBusy = False
	'		A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ModificarListaOperacion", Application.Current.ToString(), Program.Maquina, ex)
	'	End Try
	'End Sub
	Private Sub PrepararNuevaBusqueda()
		Try
			Dim objCB As New CamposBusquedaFormulario1
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
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblFormulariosBcoRepublica1, ByRef pobjRegistroSalvar As tblFormulariosBcoRepublica1)
        Dim objEncabezado As tblFormulariosBcoRepublica1 = New tblFormulariosBcoRepublica1

        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intNumeroDecl = pobjRegistro.intNumeroDecl
                objEncabezado.intFormulario = pobjRegistro.intFormulario
                objEncabezado.strCondicionNegocio = pobjRegistro.strCondicionNegocio
                objEncabezado.intTipoOperacion = pobjRegistro.intTipoOperacion
                objEncabezado.intCiudad = pobjRegistro.intCiudad
                objEncabezado.strNit = pobjRegistro.strNit
                objEncabezado.dtmFecha = pobjRegistro.dtmFecha
                objEncabezado.strNitAnterior = pobjRegistro.strNitAnterior
                objEncabezado.dtmFechaAnterior = pobjRegistro.dtmFechaAnterior
                objEncabezado.intNumeroDeclAnterior = pobjRegistro.intNumeroDeclAnterior
                objEncabezado.strTipoDocumento = pobjRegistro.strTipoDocumento
                objEncabezado.intNumeroidentificacion = pobjRegistro.intNumeroidentificacion
                objEncabezado.intDigitoVerificacion = pobjRegistro.intDigitoVerificacion
                objEncabezado.strRazonSocial = pobjRegistro.strRazonSocial
                objEncabezado.intCiudadDomicilio = pobjRegistro.intCiudadDomicilio
                objEncabezado.strCondiccionespago = pobjRegistro.strCondiccionespago
                objEncabezado.strCondiccionesDespacho = pobjRegistro.strCondiccionesDespacho
                objEncabezado.strNombreDeclarante = pobjRegistro.strNombreDeclarante
                objEncabezado.strNumeroIdentDeclarante = pobjRegistro.strNumeroIdentDeclarante
                objEncabezado.logEnviado = pobjRegistro.logEnviado
                objEncabezado.strObservaciones = pobjRegistro.strObservaciones
                objEncabezado.dblTotalvalorFOB = pobjRegistro.dblTotalvalorFOB
                objEncabezado.dblTotalGastosExportacion = pobjRegistro.dblTotalGastosExportacion
                objEncabezado.dblDeducciones = pobjRegistro.dblDeducciones
                objEncabezado.dblReintegroNeto = pobjRegistro.dblReintegroNeto
                objEncabezado.strTelefono = pobjRegistro.strTelefono
                objEncabezado.strDireccion = pobjRegistro.strDireccion
                objEncabezado.strCodigoMoneda1 = pobjRegistro.strCodigoMoneda1
                objEncabezado.dblTipoCambioMoneda1 = pobjRegistro.dblTipoCambioMoneda1
                objEncabezado.intNumeralcambiario1 = pobjRegistro.intNumeralcambiario1
                objEncabezado.dblValormoneda1 = pobjRegistro.dblValormoneda1
                objEncabezado.dblValorUSD1 = pobjRegistro.dblValorUSD1
                objEncabezado.strCodigoMoneda2 = pobjRegistro.strCodigoMoneda2
                objEncabezado.dblTipoCambioMoneda2 = pobjRegistro.dblTipoCambioMoneda2
                objEncabezado.intNumeralcambiario2 = pobjRegistro.intNumeralcambiario2
                objEncabezado.dblValormoneda2 = pobjRegistro.dblValormoneda2
                objEncabezado.dblValorUSD2 = pobjRegistro.dblValorUSD2
                objEncabezado.strGlobal = pobjRegistro.strGlobal
                objEncabezado.strCorreccion = pobjRegistro.strCorreccion
                objEncabezado.strIngresoEgreso = pobjRegistro.strIngresoEgreso
                objEncabezado.intIDOperacion = pobjRegistro.intIDOperacion
                objEncabezado.strNombreDeclaranteFirma = pobjRegistro.strNombreDeclaranteFirma
                objEncabezado.intNumeroDeclAnteriorFirma = pobjRegistro.intNumeroDeclAnteriorFirma
                objEncabezado.strEstado = pobjRegistro.strEstado
                objEncabezado.strTipoReferencia = pobjRegistro.strTipoReferencia
                objEncabezado.dtmFechaActualizacion = pobjRegistro.dtmFechaActualizacion
                objEncabezado.logEnviadoDian = pobjRegistro.logEnviadoDian
                objEncabezado.strCorreoElectronicoDecl = pobjRegistro.strCorreoElectronicoDecl
                objEncabezado.strCodigoCiudadDecl = pobjRegistro.strCodigoCiudadDecl
                objEncabezado.logFirmaFormPend = pobjRegistro.logFirmaFormPend
                objEncabezado.intDeclaracionConsolidado = pobjRegistro.intDeclaracionConsolidado
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.intNumeroBcoRep = pobjRegistro.intNumeroBcoRep
                'objEncabezado.intNumeroBcoRepAnterior = pobjRegistro.intNumeroBcoRepAnterior
                objEncabezado.dtmFechaBcoRep = pobjRegistro.dtmFechaBcoRep
                'objEncabezado.dtmFechaBcoRepAnterior = pobjRegistro.dtmFechaBcoRepAnterior





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
				If e.PropertyName = "dblValormoneda1" Then
					_EncabezadoEdicionSeleccionado.dblValorUSD1 = _EncabezadoEdicionSeleccionado.dblValormoneda1 * _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1
				ElseIf e.PropertyName = "dblValormoneda2" Then
					_EncabezadoEdicionSeleccionado.dblValorUSD2 = _EncabezadoEdicionSeleccionado.dblValormoneda2 * _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1
				ElseIf e.PropertyName = "dblValormoneda2" Then
					_EncabezadoEdicionSeleccionado.dblValorUSD2 = _EncabezadoEdicionSeleccionado.dblValormoneda2 * _EncabezadoEdicionSeleccionado.dblTipoCambioMoneda1
				ElseIf e.PropertyName = "intTipoOperacion" Then

				End If
			End If

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

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
Public Class CamposBusquedaFormulario1
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

	Private _Orden As String
	Public Property Orden() As String
		Get
			Return _Orden
		End Get
		Set(ByVal value As String)
			_Orden = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Orden"))
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
