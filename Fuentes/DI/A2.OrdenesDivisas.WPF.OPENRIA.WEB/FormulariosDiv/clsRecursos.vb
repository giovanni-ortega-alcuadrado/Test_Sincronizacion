Imports System.Security.Principal
Imports A2Utilidades
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel

Public Class clsRecursos
	Public Shared Async Function Crear(Args As String()) As Task(Of Boolean)
		Dim logRetorno As Boolean = False
		Try
			Dim objAplicacion As New Aplicacion
			Dim objDiccionarioUsuario As New Dictionary(Of String, String)

			objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UL.ToString(), WindowsIdentity.GetCurrent().Name)
			objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UP.ToString(), "")
			objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UI.ToString(), "True")
			objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UM.ToString(), Environment.MachineName)
			objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UN.ToString(), WindowsIdentity.GetCurrent().Name)
			objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UW.ToString(), WindowsIdentity.GetCurrent().Name)


			If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString) Then
				Application.Current.Resources.Remove(Recursos.RecursosApp.A2Consola_AppActiva.ToString)
			End If

			objAplicacion.Parametros = New Dictionary(Of String, Object)



			'objAplicacion.Parametros.Add("URLServicioEjemploPractico", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/EjemploPractico-OPENRIA-WEB-EjemploPracticoDomainServices.svc")
			objAplicacion.Parametros.Add("URLServicio", "http://a2webint:6565/A2-OyD-OYDServer-RIA-Web-FormulariosDivisasDomainServices.svc")
			objAplicacion.Parametros.Add("EJECUTAR_APP_SEGUN_AMBIENTE", False)
			objAplicacion.Parametros.Add("URL_SERVICIO_UPLOADS", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/Uploads.aspx")
			objAplicacion.Parametros.Add("URL_SERVICIO_REPORTING", "http://a2sqldllo/Reportserver_SQLReports/Pages/ReportViewer.aspx?/Divisas/")

			If System.Diagnostics.Debugger.IsAttached Then
                'objAplicacion.Parametros("URLServicio") = "http://localhost:56065/A2-OyD-OYDServer-RIA-Web-FormulariosDivisasDomainServices.svc"

            End If

			'Si cargar los argumentos
			If Args IsNot Nothing AndAlso Args.Count > 1 Then
				objAplicacion.Parametros("URLServicio") = Args(1).ToString
				objAplicacion.Parametros("URL_SERVICIO_REPORTING") = Args(2).ToString
				objDiccionarioUsuario(Recursos.ParametrosUsuario.UL.ToString()) = Args(3).ToString
				objDiccionarioUsuario(Recursos.ParametrosUsuario.UN.ToString()) = Args(3).ToString
				objDiccionarioUsuario(Recursos.ParametrosUsuario.UW.ToString()) = Args(3).ToString

			End If

			Dim objUsuario As New Usuario(objDiccionarioUsuario)

			Dim permisosBotones As New List(Of String)
			permisosBotones.Add("Forma|Forma|Pasar a modo forma")
			permisosBotones.Add("Buscar|Buscar|Buscar un registro")
			permisosBotones.Add("Editar|Editar|Editar registro seleccionado")
			permisosBotones.Add("Nuevo|Nuevo|Ingresar nuevo registro")
			permisosBotones.Add("Borrar|Borrar|Borrar el registro seleccionado")
			permisosBotones.Add("Aprobar|Aprobar|Aprobar modificaciones al registro")
			permisosBotones.Add("Rechazar|Rechazar|Rechazar modificaciones al registro")
			permisosBotones.Add("Version|Versión|Versión")

			Application.Current.Resources.Add("A2Consola_ToolbarActivo", permisosBotones)

			objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Mensajes.ToString, ArmarMensajesPantalla())
			objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Etiquetas.ToString, ArmarEtiquetasPantalla())
			Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_AppActiva.ToString, objAplicacion)
			objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Combos.ToString, Await ConsultarCombosAplicacion())

			'Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_ServicioSeguridad.ToString, "http://localhost:1020/A2ServiciosUtilidades_Local/Servicios/SeguridadApp.svc")
			Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_MostrarLog.ToString, "0")
			Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_UsuarioActivo.ToString, objUsuario)
			Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_UsaUsuarioSinDominio.ToString, True)

			logRetorno = True
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "Crear", Application.Current.ToString(), Program.Maquina, ex)
			logRetorno = False
		End Try
		Return logRetorno
	End Function

	Private Shared Async Function ConsultarCombosAplicacion() As Task(Of Dictionary(Of String, Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos))))
		Try
			Dim Uri = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString), Aplicacion).Parametros("URLServicio")
			Dim mdcProxy = New FormulariosDivisasDomainServices(New System.Uri(Uri))
			DirectCast(mdcProxy.DomainClient, WebDomainClient(Of FormulariosDivisasDomainServices.IFormulariosDivisasDomainServicesContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)

			Dim ListaEncabezado As New List(Of CPX_tblCombos)
			Dim ListaCombosServidor As New ObservableCollection(Of A2Utilidades.ProductoCombos)
			Dim dicListaCombosRetornoCompletos As New Dictionary(Of String, Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos)))
			Dim dicListaCombosRetorno As New Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos))
			Dim objRespuesta As InvokeResult(Of List(Of CPX_tblCombos)) = Nothing
			objRespuesta = Await mdcProxy.Utilitarios_ConsultarCombosAsync(Program.Usuario)

			If Not IsNothing(objRespuesta) Then
				If Not IsNothing(objRespuesta.Value) Then
					ListaEncabezado = objRespuesta.Value

					For Each li In ListaEncabezado
						ListaCombosServidor.Add(New A2Utilidades.ProductoCombos With {
						.ID = li.intID,
						.Topico = li.strTopico,
						.Retorno = li.strRetorno,
						.Descripcion = li.strDescripcion,
						.Dependencia1 = li.strDependencia1,
						.Dependencia2 = li.strDependencia2,
						.IDDependencia1 = li.intIDDependencia1,
						.IDDependencia2 = li.intIDDependencia2
						})
					Next

					Dim lstCombos = From lc In ListaEncabezado Select lc.strTopico Distinct
					Dim objNodosCategoria As ObservableCollection(Of A2Utilidades.ProductoCombos) = Nothing
					Dim strNombreCategoria As String
					dicListaCombosRetorno = New Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos))

					For Each NombreCategoria As String In lstCombos
						strNombreCategoria = NombreCategoria
						objNodosCategoria = New ObservableCollection(Of A2Utilidades.ProductoCombos)((From ln In ListaCombosServidor Where ln.Topico = strNombreCategoria))

						dicListaCombosRetorno.Add(strNombreCategoria, objNodosCategoria)
					Next

					dicListaCombosRetornoCompletos.Add("GENERICO", dicListaCombosRetorno)
				Else
					A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar las listas.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
				End If
			Else
				A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar las listas.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
			End If

			Return dicListaCombosRetornoCompletos
			'Dim dicListaCombosRetornoCompletos As New Dictionary(Of String, Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos)))

			'Dim objDiccionarioGenerico As New Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos))
			'Dim objListaCombos As New ObservableCollection(Of A2Utilidades.ProductoCombos)
			'objListaCombos.Add(New ProductoCombos With {.ID = 1,
			'                   .Descripcion = "Prueba1",
			'                   .Retorno = "1",
			'                   .Topico = "COMBOGENERICO1"})
			'objListaCombos.Add(New ProductoCombos With {.ID = 2,
			'                   .Descripcion = "Prueba2",
			'                   .Retorno = "2",
			'                   .Topico = "COMBOGENERICO1"})

			'objDiccionarioGenerico.Add("COMBOGENERICO1", objListaCombos)

			'dicListaCombosRetornoCompletos.Add("GENERICO", objDiccionarioGenerico)

			'Return dicListaCombosRetornoCompletos
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ConsultarCombosAplicacion", Application.Current.ToString(), Program.Maquina, ex)
			Return Nothing
		End Try
	End Function

	Private Shared Function ArmarEtiquetasPantalla() As Dictionary(Of String, Dictionary(Of String, A2Utilidades.ProductoEtiquetas))
        Try
            Dim ListaRetorno As New Dictionary(Of String, Dictionary(Of String, A2Utilidades.ProductoEtiquetas))
			Dim ListaEtiquetasGenerico As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            Dim ListaEtiquetas1 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas2 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas3 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            Dim ListaEtiquetas4 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas5 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas6 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas7 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim objProductoEtiquetas As New A2Utilidades.ProductoEtiquetas
#Region "Generico"

			'GENERICO
			ListaEtiquetasGenerico.Add("GENERICO_BOTON_FILTRAR", New ProductoEtiquetas With {.Titulo = "Filtrar", .Tooltip = "Filtrar", .TooltipOriginal = "Filtrar"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_QUITARFILTRO", New ProductoEtiquetas With {.Titulo = "Quitar filtro", .Tooltip = "Quitar filtro", .TooltipOriginal = "Quitar filtro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_BUSQUEDAAVANZADA", New ProductoEtiquetas With {.Titulo = "Busqueda avanzada", .Tooltip = "Busqueda avanzada", .TooltipOriginal = "Busqueda avanzada"})
			ListaEtiquetasGenerico.Add("GENERICO_BOTON_NUEVO", New ProductoEtiquetas With {.Titulo = "Nueva", .Tooltip = "Nuevo registro", .TooltipOriginal = "Nuevo registro"})
			ListaEtiquetasGenerico.Add("GENERICO_BOTON_EDITAR", New ProductoEtiquetas With {.Titulo = "Editar", .Tooltip = "Editar registro", .TooltipOriginal = "Editar registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_BORRAR", New ProductoEtiquetas With {.Titulo = "Borrar", .Tooltip = "Borrar registro", .TooltipOriginal = "Borrar registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_ANULAR", New ProductoEtiquetas With {.Titulo = "Anular", .Tooltip = "Anular registro", .TooltipOriginal = "Anular registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_CANCELAR", New ProductoEtiquetas With {.Titulo = "Cancelar", .Tooltip = "Cancelar edicion registro", .TooltipOriginal = "Cancelar edicion registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDAR", New ProductoEtiquetas With {.Titulo = "Guardar", .Tooltip = "Guardar registro", .TooltipOriginal = "Guardar registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDARYCERRAR", New ProductoEtiquetas With {.Titulo = "Guardar y cerrar", .Tooltip = "Guardar registro y cerrar", .TooltipOriginal = "Guardar registro y cerrar"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDARYCOPIARANTERIOR", New ProductoEtiquetas With {.Titulo = "Guardar y copiar anterior", .Tooltip = "Guardar registro y copiar anterior", .TooltipOriginal = "Guardar registro y copiar anterior"})
			ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDARYCREARNUEVO", New ProductoEtiquetas With {.Titulo = "Guardar y crear nuevo", .Tooltip = "Guardar registro y crear nuevo", .TooltipOriginal = "Guardar registro y crear nuevo"})
			ListaEtiquetasGenerico.Add("CODIGOOYD", New ProductoEtiquetas With {.Titulo = "Codigo OyD: ", .Tooltip = "Codigo OyD: ", .TooltipOriginal = "Codigo OyD: "})
			ListaEtiquetasGenerico.Add("MOSTRARCONSULTANDO", New ProductoEtiquetas With {.Titulo = "Consultando...", .Tooltip = "Consultando...", .TooltipOriginal = "Consultando..."})
			ListaEtiquetasGenerico.Add("BUSCARCLIENTE", New ProductoEtiquetas With {.Titulo = "Buscar Cliente", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaRetorno.Add("GENERICO", ListaEtiquetasGenerico)
#End Region

#Region "Formulario 1"

			'ETIQUETAS FORMULARIO1
			ListaEtiquetas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 1", .Tooltip = "Formulario No. 1", .TooltipOriginal = "Formulario No. 1"})
			ListaEtiquetas.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Declaración de Cambio por Importacion de Bienes", .Tooltip = "Formulario No. 1", .TooltipOriginal = "Formulario No. 1"})
			ListaEtiquetas.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			ListaEtiquetas.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Orden y Fecha", .Tooltip = "Filtrar por Nro Orden y Fecha", .TooltipOriginal = "Filtrar por Nro Orden y Fecha"})
			ListaEtiquetas.Add("ID", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
			ListaEtiquetas.Add("LIMPIAR", New ProductoEtiquetas With {.Titulo = "Limpiar selección", .Tooltip = "Limpiar selección", .TooltipOriginal = "Limpiar selección"})
			ListaEtiquetas.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del formulario", .TooltipOriginal = "Estado del formulario"})
			ListaEtiquetas.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
			ListaEtiquetas.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
			ListaEtiquetas.Add("ORDEN", New ProductoEtiquetas With {.Titulo = "Orden", .Tooltip = "Numero de la orden", .TooltipOriginal = "Numero de la orden"})
			ListaEtiquetas.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .Tooltip = "Nro identificación del cliente", .TooltipOriginal = "Nro identificación del cliente"})
			ListaEtiquetas.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del cliente", .TooltipOriginal = "Usuario creación del cliente"})
			ListaEtiquetas.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación", .TooltipOriginal = "Fecha creación"})
			ListaEtiquetas.Add("INGRESEFECHA", New ProductoEtiquetas With {.Titulo = "Ingrese una fecha", .Tooltip = "Ingrese una fecha", .TooltipOriginal = "Ingrese una fecha"})
			ListaEtiquetas.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del cliente", .TooltipOriginal = "Fecha ultima actualización del cliente"})
			ListaEtiquetas.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
			ListaEtiquetas.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})
			ListaEtiquetas.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
			ListaEtiquetas.Add("2NIT", New ProductoEtiquetas With {.Titulo = "2. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
			ListaEtiquetas.Add("3FECHA", New ProductoEtiquetas With {.Titulo = "3. Fecha AAAA-MM-DD", .Tooltip = "3. Fecha AAAA-MM-DD", .TooltipOriginal = "3. Fecha AAAA-MM-DD"})
			ListaEtiquetas.Add("4NUMERO", New ProductoEtiquetas With {.Titulo = "4. Número", .Tooltip = "4. Número", .TooltipOriginal = "4. Número"})
			ListaEtiquetas.Add("IIIIDENTIFICACIONDECLANT", New ProductoEtiquetas With {.Titulo = "III. Identificación de la declaración de cambio anterior", .Tooltip = "III. Identificación de la declaración de cambio anterior", .TooltipOriginal = "III. Identificación de la declaración de cambio anterior"})
			ListaEtiquetas.Add("5NIT", New ProductoEtiquetas With {.Titulo = "5. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
			ListaEtiquetas.Add("6FECHA", New ProductoEtiquetas With {.Titulo = "6. Fecha AAAA-MM-DD", .Tooltip = "3. Fecha AAAA-MM-DD", .TooltipOriginal = "3. Fecha AAAA-MM-DD"})
			ListaEtiquetas.Add("7NUMERO", New ProductoEtiquetas With {.Titulo = "7. Número", .Tooltip = "7. Número", .TooltipOriginal = "7. Número"})
			ListaEtiquetas.Add("IVDENTIFICACIONIMPORTADOR", New ProductoEtiquetas With {.Titulo = "IV. Identificación del importador", .Tooltip = "IV. Identificación del importador", .TooltipOriginal = "IV. Identificación del importador"})
			ListaEtiquetas.Add("8TIPO", New ProductoEtiquetas With {.Titulo = "8. Tipo", .Tooltip = "8. Tipo", .TooltipOriginal = "8. Tipo"})
			ListaEtiquetas.Add("9NUMEROID", New ProductoEtiquetas With {.Titulo = "9. Número de identificación", .Tooltip = "9. Número de identificación", .TooltipOriginal = "9. Número de identificación"})
			ListaEtiquetas.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
			ListaEtiquetas.Add("10NOMBRE", New ProductoEtiquetas With {.Titulo = "10. Nombre o razón social", .Tooltip = "10. Nombre o razón social", .TooltipOriginal = "10. Nombre o razón social"})
			ListaEtiquetas.Add("VDESCRIPCIONOP", New ProductoEtiquetas With {.Titulo = "V. Descripción de la operación", .Tooltip = "V. Descripción de la operación", .TooltipOriginal = "V. Descripción de la operación"})
			ListaEtiquetas.Add("11CODMONEDA", New ProductoEtiquetas With {.Titulo = "11. Código moneda de giro", .Tooltip = "11. Código moneda de giro", .TooltipOriginal = "11. Código moneda de giro"})
			ListaEtiquetas.Add("12TIPOCAMBIO", New ProductoEtiquetas With {.Titulo = "12. Tipo de cambio a USD", .Tooltip = "12. Tipo de cambio a USD", .TooltipOriginal = "12. Tipo de cambio a USD"})
			ListaEtiquetas.Add("13NUMERAL", New ProductoEtiquetas With {.Titulo = "13. Numeral", .Tooltip = "13. Numeral", .TooltipOriginal = "13. Numeral"})
			ListaEtiquetas.Add("14VALORMONEDA", New ProductoEtiquetas With {.Titulo = "14. Valor moneda giro", .Tooltip = "14. Valor moneda giro", .TooltipOriginal = "14. Valor moneda giro"})
			ListaEtiquetas.Add("15VALORUSD", New ProductoEtiquetas With {.Titulo = "15. Valor USD", .Tooltip = "15. Valor USD", .TooltipOriginal = "15. Valor USD"})
			ListaEtiquetas.Add("VIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VI. Identificación del declarante", .Tooltip = "VI. Identificación del declarante", .TooltipOriginal = "VI. Identificación del declarante"})
			ListaEtiquetas.Add("16NOMBRE", New ProductoEtiquetas With {.Titulo = "16. Nombre", .Tooltip = "16. Nombre", .TooltipOriginal = "16. Nombre"})
			ListaEtiquetas.Add("17NUMID", New ProductoEtiquetas With {.Titulo = "17. Número de identificación", .Tooltip = "17. Número de identificación", .TooltipOriginal = "17. Número de identificación"})
			ListaEtiquetas.Add("18FIRMA", New ProductoEtiquetas With {.Titulo = "18. Firma", .Tooltip = "18. Firma", .TooltipOriginal = "18. Firma"})
			ListaEtiquetas.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})

			ListaEtiquetas.Add("VIIINFDOCIMP", New ProductoEtiquetas With {.Titulo = "VII. Información documentos de importación", .Tooltip = "VII. Información documentos de importación", .TooltipOriginal = "VII. Información documentos de importación"})
			ListaEtiquetas.Add("19NUMERO", New ProductoEtiquetas With {.Titulo = "19. Numero", .Tooltip = "19. Numero", .TooltipOriginal = "19. Numero"})
			ListaEtiquetas.Add("20VALORUSD", New ProductoEtiquetas With {.Titulo = "20. Valor USD", .Tooltip = "20. Valor USD", .TooltipOriginal = "20. Valor USD"})
			ListaEtiquetas.Add("FECHAPRESENTACION", New ProductoEtiquetas With {.Titulo = "Fecha de Presentación", .Tooltip = "Fecha de Presentación", .TooltipOriginal = "Fecha de Presentación"})


			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario1View", ListaEtiquetas)
#End Region

#Region "Formulario 2"

			'ETIQUETAS FORMULARIO2
			ListaEtiquetas2.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 2", .Tooltip = "Formulario No. 2", .TooltipOriginal = "Formulario No. 2"})
			ListaEtiquetas2.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Declaración de Cambio por Exportaciones de Bienes", .Tooltip = "Formulario No. 2", .TooltipOriginal = "Formulario No. 2"})
			ListaEtiquetas2.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			ListaEtiquetas2.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Orden y Fecha", .Tooltip = "Filtrar por Nro Orden y Fecha", .TooltipOriginal = "Filtrar por Nro Orden y Fecha"})
			ListaEtiquetas2.Add("ID", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})

			ListaEtiquetas2.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
			ListaEtiquetas2.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del formulario", .TooltipOriginal = "Estado del formulario"})
			ListaEtiquetas2.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
			ListaEtiquetas2.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
			ListaEtiquetas2.Add("ORDEN", New ProductoEtiquetas With {.Titulo = "Orden", .Tooltip = "Numero de la orden", .TooltipOriginal = "Numero de la orden"})
			ListaEtiquetas2.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .Tooltip = "Nro identificación del cliente", .TooltipOriginal = "Nro identificación del cliente"})
			ListaEtiquetas2.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del cliente", .TooltipOriginal = "Usuario creación del cliente"})
			ListaEtiquetas2.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación", .TooltipOriginal = "Fecha creación"})
			ListaEtiquetas2.Add("INGRESEFECHA", New ProductoEtiquetas With {.Titulo = "Ingrese una fecha", .Tooltip = "Ingrese una fecha", .TooltipOriginal = "Ingrese una fecha"})
			ListaEtiquetas2.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del cliente", .TooltipOriginal = "Fecha ultima actualización del cliente"})

			ListaEtiquetas2.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
			ListaEtiquetas2.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})

			ListaEtiquetas2.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
			ListaEtiquetas2.Add("2NIT", New ProductoEtiquetas With {.Titulo = "2. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
			ListaEtiquetas2.Add("3FECHA", New ProductoEtiquetas With {.Titulo = "3. Fecha AAAA-MM-DD", .Tooltip = "3. Fecha AAAA-MM-DD", .TooltipOriginal = "3. Fecha AAAA-MM-DD"})
			ListaEtiquetas2.Add("4NUMERO", New ProductoEtiquetas With {.Titulo = "4. Número", .Tooltip = "4. Número", .TooltipOriginal = "4. Número"})

			ListaEtiquetas2.Add("IIIIDENTIFICACIONDECLANT", New ProductoEtiquetas With {.Titulo = "III. Identificación de la declaración de cambio anterior", .Tooltip = "III. Identificación de la declaración de cambio anterior", .TooltipOriginal = "III. Identificación de la declaración de cambio anterior"})
			ListaEtiquetas2.Add("5NIT", New ProductoEtiquetas With {.Titulo = "5. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
			ListaEtiquetas2.Add("6FECHA", New ProductoEtiquetas With {.Titulo = "6. Fecha AAAA-MM-DD", .Tooltip = "6. Fecha AAAA-MM-DD", .TooltipOriginal = "6. Fecha AAAA-MM-DD"})
			ListaEtiquetas2.Add("7NUMERO", New ProductoEtiquetas With {.Titulo = "7. Número", .Tooltip = "7. Número", .TooltipOriginal = "7. Número"})

			ListaEtiquetas2.Add("IVDENTIFICACIONEXPORTADOR", New ProductoEtiquetas With {.Titulo = "IV. Identificación del exportador", .Tooltip = "IV. Identificación del exportador", .TooltipOriginal = "IV. Identificación del exportador"})
			ListaEtiquetas2.Add("8TIPO", New ProductoEtiquetas With {.Titulo = "8. Tipo", .Tooltip = "8. Tipo", .TooltipOriginal = "8. Tipo"})
			ListaEtiquetas2.Add("9NUMEROID", New ProductoEtiquetas With {.Titulo = "9. Número de identificación", .Tooltip = "9. Número de identificación", .TooltipOriginal = "9. Número de identificación"})
			ListaEtiquetas2.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
			ListaEtiquetas2.Add("10NOMBRE", New ProductoEtiquetas With {.Titulo = "10. Nombre o razón social", .Tooltip = "10. Nombre o razón social", .TooltipOriginal = "10. Nombre o razón social"})

			ListaEtiquetas2.Add("VDESCRIPCIONOP", New ProductoEtiquetas With {.Titulo = "V. Descripción de la operación", .Tooltip = "V. Descripción de la operación", .TooltipOriginal = "V. Descripción de la operación"})
			ListaEtiquetas2.Add("11CODMONEDA", New ProductoEtiquetas With {.Titulo = "11. Código moneda de reintegro", .Tooltip = "11. Código moneda de giro", .TooltipOriginal = "11. Código moneda de giro"})
			ListaEtiquetas2.Add("12VALORMONEDA", New ProductoEtiquetas With {.Titulo = "12. Valor moneda de reintegro", .Tooltip = "12. Valor moneda de reintegro", .TooltipOriginal = "12. Valor moneda de reintegro"})
			ListaEtiquetas2.Add("13TIPOCAMBIO", New ProductoEtiquetas With {.Titulo = "13. Tipo de cambio a USD", .Tooltip = "13. Tipo de cambio a USD", .TooltipOriginal = "13. Tipo de cambio a USD"})

			ListaEtiquetas2.Add("VIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VI. Identificación del declarante", .Tooltip = "VI. Identificación del declarante", .TooltipOriginal = "VI. Identificación del declarante"})
			ListaEtiquetas2.Add("14NOMBRE", New ProductoEtiquetas With {.Titulo = "14. Nombre", .Tooltip = "14. Nombre", .TooltipOriginal = "14. Nombre"})
			ListaEtiquetas2.Add("15NUMID", New ProductoEtiquetas With {.Titulo = "15. Número de identificación", .Tooltip = "15. Número de identificación", .TooltipOriginal = "15. Número de identificación"})
			ListaEtiquetas2.Add("16FIRMA", New ProductoEtiquetas With {.Titulo = "16. Firma", .Tooltip = "16. Firma", .TooltipOriginal = "16. Firma"})

			ListaEtiquetas2.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})

			ListaEtiquetas2.Add("14NUMERAL", New ProductoEtiquetas With {.Titulo = "14. Numeral", .Tooltip = "14. Numeral", .TooltipOriginal = "14. Numeral"})
			ListaEtiquetas2.Add("NUMERAL", New ProductoEtiquetas With {.Titulo = "Numeral", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
			ListaEtiquetas2.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "Valor reintegrado USD", .Tooltip = "Valor reintegrado USD", .TooltipOriginal = "Valor reintegrado USD"})
			ListaEtiquetas2.Add("15VALORUSD", New ProductoEtiquetas With {.Titulo = "15. Valor reintegrado USD", .Tooltip = "15. Valor reintegrado USD", .TooltipOriginal = "15. Valor reintegrado USD"})

			ListaEtiquetas2.Add("VIIINFDECLEXDEF", New ProductoEtiquetas With {.Titulo = "VII.  Información declaraciones de exportación definitivas", .Tooltip = "VII.  Información decalraciones de exportación definitivas", .TooltipOriginal = "VII.  Información decalraciones de exportación definitivas"})
			ListaEtiquetas2.Add("17NUMERO", New ProductoEtiquetas With {.Titulo = "  Número", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("18FECHA", New ProductoEtiquetas With {.Titulo = "Fecha AAAA-MM-DD", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("19CIUDAD", New ProductoEtiquetas With {.Titulo = "Ciudad aduana", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("20NUMERAL", New ProductoEtiquetas With {.Titulo = "20.  Numeral", .Tooltip = "20.  Numeral", .TooltipOriginal = "20.  Numeral"})
			ListaEtiquetas2.Add("21VALORUSD", New ProductoEtiquetas With {.Titulo = "15.  Valor reintegrado USD", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("22TOTALFOB", New ProductoEtiquetas With {.Titulo = "16. Total valor FOB reintegrado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("23TOTALGASTOSEX", New ProductoEtiquetas With {.Titulo = "17. Total gastos de exportación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("24DEDUCCIONES", New ProductoEtiquetas With {.Titulo = "18. Deducciones", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("25REINTEGRONETO", New ProductoEtiquetas With {.Titulo = "19. Reintegro neto", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas2.Add("FECHAPRESENTACION", New ProductoEtiquetas With {.Titulo = "Fecha de Presentación", .Tooltip = "Fecha de Presentación", .TooltipOriginal = "Fecha de Presentación"})

			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2View", ListaEtiquetas2)
#End Region
#Region "Campos Formularios 3"
			'ETIQUETAS FORMULARIO3
			ListaEtiquetas3.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 3", .Tooltip = "Formulario No. 3", .TooltipOriginal = "Formulario No. 3"})
			ListaEtiquetas3.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Declaración de Cambio por Endeudamiento Externo o Avales y Garantías Formulario No. 3", .Tooltip = "Formulario No. 3", .TooltipOriginal = "Formulario No. 3"})
			ListaEtiquetas3.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			ListaEtiquetas3.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Orden y Fecha", .Tooltip = "Filtrar por Nro Orden y Fecha", .TooltipOriginal = "Filtrar por Nro Orden y Fecha"})
			ListaEtiquetas3.Add("ID", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
			ListaEtiquetas3.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
			ListaEtiquetas3.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del formulario", .TooltipOriginal = "Estado del formulario"})
			ListaEtiquetas3.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
			ListaEtiquetas3.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
			ListaEtiquetas3.Add("ORDEN", New ProductoEtiquetas With {.Titulo = "Orden", .Tooltip = "Numero de la orden", .TooltipOriginal = "Numero de la orden"})
			ListaEtiquetas3.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .Tooltip = "Nro identificación del cliente", .TooltipOriginal = "Nro identificación del cliente"})
			ListaEtiquetas3.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del cliente", .TooltipOriginal = "Usuario creación del cliente"})
			ListaEtiquetas3.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación", .TooltipOriginal = "Fecha creación"})
			ListaEtiquetas3.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del cliente", .TooltipOriginal = "Fecha ultima actualización del cliente"})
            ListaEtiquetas3.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
            ListaEtiquetas3.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número:", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})
			ListaEtiquetas3.Add("2OPERACIONDE", New ProductoEtiquetas With {.Titulo = "2. Operación de:", .Tooltip = "2. Operación de", .TooltipOriginal = "2. Operación de"})
            ListaEtiquetas3.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
            ListaEtiquetas3.Add("3CIUDAD", New ProductoEtiquetas With {.Titulo = "3. Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
			ListaEtiquetas3.Add("4NITIMC", New ProductoEtiquetas With {.Titulo = "4. Nit del I.M.C. o código cuenta de compensación", .Tooltip = "Nit del I.M.C", .TooltipOriginal = "Nit del I.M.C"})
			ListaEtiquetas3.Add("5FECHA", New ProductoEtiquetas With {.Titulo = "5. Fecha AAAA-MM-DD", .Tooltip = "Fecha AAAA-MM-DD", .TooltipOriginal = "Fecha AAAA-MM-DD"})
			ListaEtiquetas3.Add("6NUMERO", New ProductoEtiquetas With {.Titulo = "6. Número", .Tooltip = "6. Número", .TooltipOriginal = "6. Número"})

            ListaEtiquetas3.Add("IIIIDENTIFICACIONDECLANT", New ProductoEtiquetas With {.Titulo = "III. Identificación de la declaración de cambio anterior", .Tooltip = "III. Identificación de la declaración de cambio anterior", .TooltipOriginal = "III. Identificación de la declaración de cambio anterior"})
            ListaEtiquetas3.Add("7NIT", New ProductoEtiquetas With {.Titulo = "7. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
			ListaEtiquetas3.Add("8FECHA", New ProductoEtiquetas With {.Titulo = "8. Fecha AAAA-MM-DD", .Tooltip = "8. Fecha AAAA-MM-DD", .TooltipOriginal = "8. Fecha AAAA-MM-DD"})
			ListaEtiquetas3.Add("9NUMERO", New ProductoEtiquetas With {.Titulo = "9. Número", .Tooltip = "9. Número", .TooltipOriginal = "9. Número"})

            ListaEtiquetas3.Add("IVDESCRIPCIONOPERACION", New ProductoEtiquetas With {.Titulo = "IV. Descripción de la operación", .Tooltip = "IV. Descripción de la operación", .TooltipOriginal = "IV. Descripción de la operación"})
            ListaEtiquetas3.Add("10NUMEROPRESTAMOAVAL", New ProductoEtiquetas With {.Titulo = "10. Número de préstamo o aval", .Tooltip = "10. Número de préstamo o aval", .TooltipOriginal = "10. Número de préstamo o aval"})
			ListaEtiquetas3.Add("11TIPO", New ProductoEtiquetas With {.Titulo = "11. Tipo", .Tooltip = "11. Tipo", .TooltipOriginal = "11. Tipo"})
			ListaEtiquetas3.Add("12NUMEROIDENTIFIACION", New ProductoEtiquetas With {.Titulo = "12. Número de identificación", .Tooltip = "12. Número de identificación", .TooltipOriginal = "12. Número de identificación"})
			ListaEtiquetas3.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
			ListaEtiquetas3.Add("13NOMBREDEUDOR", New ProductoEtiquetas With {.Titulo = "13. Nombre del deudor o acreedor / avalado o beneficiario residente", .Tooltip = "Nombre del deudor o acreedor / avalado o beneficiario residente", .TooltipOriginal = "Nombre del deudor o acreedor / avalado o beneficiario residente"})
			ListaEtiquetas3.Add("14CODMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "14. Código moneda contratada", .Tooltip = "Código moneda contratada", .TooltipOriginal = "Código moneda contratada"})
			ListaEtiquetas3.Add("15VALORTOTALMONCONTRATADA", New ProductoEtiquetas With {.Titulo = "15. Valor total moneda contratada", .Tooltip = "Valor total moneda contratada", .TooltipOriginal = "Valor total moneda contratada"})
			ListaEtiquetas3.Add("16CODMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "16. Código moneda negociación", .Tooltip = "Código moneda negociación", .TooltipOriginal = "Código moneda negociación"})
			ListaEtiquetas3.Add("17VALORTOTALMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "17. Valor total moneda negociación", .Tooltip = "Valor total moneda negociación", .TooltipOriginal = "Valor total moneda negociación"})
            ListaEtiquetas3.Add("18TIPOCAMBIOMONEDANEG", New ProductoEtiquetas With {.Titulo = "18.Tipo de cambio moneda negociación", .Tooltip = "Tipo de cambio moneda negociación", .TooltipOriginal = "Tipo de cambio moneda negociación"})
            ListaEtiquetas3.Add("19VALORTOTALDOLARES", New ProductoEtiquetas With {.Titulo = "19. Valor total en dólares", .Tooltip = "Valor total en dólares", .TooltipOriginal = "Valor total en dólares"})
            ListaEtiquetas3.Add("20NOMBREACREEDOR", New ProductoEtiquetas With {.Titulo = "20. Nombre del acreedor (créditos pasivos) o el deudor (créditos activos) o avalista", .Tooltip = "Nombre del acreedor", .TooltipOriginal = "Nombre del acreedor"})

            ListaEtiquetas3.Add("VINFNUMERALESLIQUIDACIONINTERESES", New ProductoEtiquetas With {.Titulo = "V. Información de numerales y liquidación de intereses", .Tooltip = "V. Información de numerales y liquidación de intereses", .TooltipOriginal = "V. Información de numerales y liquidación de intereses"})
            ListaEtiquetas3.Add("21NUMERAL", New ProductoEtiquetas With {.Titulo = "21. Numeral", .Tooltip = "21. Numeral", .TooltipOriginal = "21. Numeral"})
            ListaEtiquetas3.Add("NUMERAL", New ProductoEtiquetas With {.Titulo = "Numeral", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
            ListaEtiquetas3.Add("22VALORMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "22. Valor moneda negociación", .Tooltip = "22. Valor moneda negociación", .TooltipOriginal = "22. Valor moneda negociación"})
            ListaEtiquetas3.Add("VALORMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "Valor moneda negociación", .Tooltip = "Valor moneda negociación", .TooltipOriginal = "23. Valor moneda negociación"})
            ListaEtiquetas3.Add("23VALORMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "23. Valor moneda contratada", .Tooltip = "23. Valor moneda contratada", .TooltipOriginal = "24. Valor moneda contratada"})
            ListaEtiquetas3.Add("VALORMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "Valor moneda contratada", .Tooltip = "24. Valor moneda contratada", .TooltipOriginal = "24. Valor moneda contratada"})
            ListaEtiquetas3.Add("24VALORUSD", New ProductoEtiquetas With {.Titulo = "24. Valor USD", .Tooltip = "24. Valor USD", .TooltipOriginal = "24. Valor USD"})
            ListaEtiquetas3.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "Valor USD", .Tooltip = "24. Valor USD", .TooltipOriginal = "24. Valor USD"})

            ListaEtiquetas3.Add("VIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VI. Identificación del declarante", .Tooltip = "VI. Identificación del declarante", .TooltipOriginal = "VI. Identificación del declarante"})
            ListaEtiquetas3.Add("25NOMBREDECLARANTE", New ProductoEtiquetas With {.Titulo = "25. Nombre", .Tooltip = "31. Nombre", .TooltipOriginal = "31. Nombre"})
            ListaEtiquetas3.Add("26NUMID", New ProductoEtiquetas With {.Titulo = "26. Número de identificación", .Tooltip = "32. Número de identificación", .TooltipOriginal = "32. Número de identificación"})
            ListaEtiquetas3.Add("27FIRMA", New ProductoEtiquetas With {.Titulo = "27. Firma", .Tooltip = "33. Firma", .TooltipOriginal = "33. Firma"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario3View", ListaEtiquetas3)
#End Region
#Region "Formulario 6"
			ListaEtiquetas6.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "No.", .Tooltip = "No.", .TooltipOriginal = "No."})
			ListaEtiquetas6.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 6", .Tooltip = "Formulario No. 6", .TooltipOriginal = "Formulario No. 6"})
			ListaEtiquetas6.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Información de endeudamiento externo otorgado a residentes", .Tooltip = "Formulario No. 6", .TooltipOriginal = "Formulario No. 6"})
			ListaEtiquetas6.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			ListaEtiquetas6.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por ID y Fecha", .Tooltip = "Filtrar por ID y Fecha", .TooltipOriginal = "Filtrar por ID y Fecha"})
			ListaEtiquetas6.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
			ListaEtiquetas6.Add("LIMPIAR", New ProductoEtiquetas With {.Titulo = "Limpiar selección", .Tooltip = "Limpiar selección", .TooltipOriginal = "Limpiar selección"})

			ListaEtiquetas6.Add("ID", New ProductoEtiquetas With {.Titulo = "Número declaración", .Tooltip = "Número declaración", .TooltipOriginal = "Número declaración"})
			ListaEtiquetas6.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del declarante", .TooltipOriginal = "Nombre del declarante"})
			ListaEtiquetas6.Add("IDENTIFICACION", New ProductoEtiquetas With {.Titulo = "Número de identificación", .Tooltip = "Número de identificación del declarante", .TooltipOriginal = "Número de identificación del declarante"})
			ListaEtiquetas6.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "Valor USD", .Tooltip = "Valor USD", .TooltipOriginal = "Valor USD"})

			ListaEtiquetas6.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
			ListaEtiquetas6.Add("INGRESEFECHA", New ProductoEtiquetas With {.Titulo = "Ingrese una fecha", .Tooltip = "Ingrese una fecha", .TooltipOriginal = "Ingrese una fecha"})

			ListaEtiquetas6.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
			ListaEtiquetas6.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})
			ListaEtiquetas6.Add("2FECHA", New ProductoEtiquetas With {.Titulo = "2. Fecha AAAA-MM-DD", .Tooltip = "2. Fecha AAAA-MM-DD", .TooltipOriginal = "2. Fecha AAAA-MM-DD"})
			ListaEtiquetas6.Add("3NUMEROPRESTAMO", New ProductoEtiquetas With {.Titulo = "3. Número préstamo", .Tooltip = "3. Número préstamo", .TooltipOriginal = "3. Número préstamo"})
			ListaEtiquetas6.Add("3.1NUMEROPRESTAMO", New ProductoEtiquetas With {.Titulo = "3.1 Número de identificación deudor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas6.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
			ListaEtiquetas6.Add("1DESEMBOLSO", New ProductoEtiquetas With {.Titulo = "1. Desembolso", .Tooltip = "1. Desembolso", .TooltipOriginal = "1. Desembolso"})
			ListaEtiquetas6.Add("2NUMERODEC", New ProductoEtiquetas With {.Titulo = "2. Número declaración", .Tooltip = "2. Número declaración", .TooltipOriginal = "2. Número declaración"})
			ListaEtiquetas6.Add("3NUMERAL", New ProductoEtiquetas With {.Titulo = "3. Numeral", .Tooltip = "3. Numeral, si esta lista esta vacia debe agregar los numerales para el formulario 6", .TooltipOriginal = "3. Numeral"})
			ListaEtiquetas6.Add("4CODMONNEG", New ProductoEtiquetas With {.Titulo = "4. Cod. Mon. Neg.", .Tooltip = "4. Codigo Moneda Negegociación", .TooltipOriginal = "4. Codigo Moneda Negegociación"})
			ListaEtiquetas6.Add("5VALORMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "5. Valor moneda negociación", .Tooltip = "5. Valor moneda negociación", .TooltipOriginal = "5. Valor moneda negociación"})
			ListaEtiquetas6.Add("6VALORUSD", New ProductoEtiquetas With {.Titulo = "6. Valor USD", .Tooltip = "6. Valor USD", .TooltipOriginal = "6. Valor USD"})

			ListaEtiquetas6.Add("IIIDENTIFICACIONDEUDOR", New ProductoEtiquetas With {.Titulo = "III. Identificación del prestatario o deudor", .Tooltip = "III. Identificación del prestatario o deudor", .TooltipOriginal = "III. Identificación del prestatario o deudor"})
			ListaEtiquetas6.Add("1TIPO", New ProductoEtiquetas With {.Titulo = "1. Tipo", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("2NUMEROID", New ProductoEtiquetas With {.Titulo = "2. Numero de identificación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("3NOMBRE", New ProductoEtiquetas With {.Titulo = "3. Nombre o razón social", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("4CIUDAD", New ProductoEtiquetas With {.Titulo = "4. Código ciudad", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("5DIRECCION", New ProductoEtiquetas With {.Titulo = "5. Dirección", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("6TELEFONO", New ProductoEtiquetas With {.Titulo = "6. Telefono", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("7EMAIL", New ProductoEtiquetas With {.Titulo = "7. Correo electrónico", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("8CODIGOCIIU", New ProductoEtiquetas With {.Titulo = "8. Código CIIU", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("BUSCARCODIGOCIIU", New ProductoEtiquetas With {.Titulo = "Buscar un código CIIU", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas6.Add("IVDENTIFICACIONACREEDOR", New ProductoEtiquetas With {.Titulo = "IV. Identificación del prestamista o acreedor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("1CODIGOASIGNADO", New ProductoEtiquetas With {.Titulo = "1. Código asignado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("BUSCARCODIGOASIGNADO", New ProductoEtiquetas With {.Titulo = "Buscar código asignado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("2NOMBRE", New ProductoEtiquetas With {.Titulo = "2. Nombre o razón social", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("3PAIS", New ProductoEtiquetas With {.Titulo = "3. Pais", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("4TIPOACREEDOR", New ProductoEtiquetas With {.Titulo = "4. Tipo de prestamista o acreedor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas6.Add("VDESCRIPCIONPRESTAMO", New ProductoEtiquetas With {.Titulo = "V. Descripción del préstamo", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("1CODIGOPRESTAMO", New ProductoEtiquetas With {.Titulo = "1. Código proposito del préstamo", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("2CODIGOMONEDA", New ProductoEtiquetas With {.Titulo = "2. Código moneda", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("3MONTOCONTRA", New ProductoEtiquetas With {.Titulo = "3. Monto contratado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("4TASAINTERES", New ProductoEtiquetas With {.Titulo = "4. Tasa de interés", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("SPREAD", New ProductoEtiquetas With {.Titulo = "Spread o valor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("5NUMERODEPOSITO", New ProductoEtiquetas With {.Titulo = "5. Número de depósito por financiación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("6INDEXACION", New ProductoEtiquetas With {.Titulo = "6. Indexación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("7CODIGOMONINDEX", New ProductoEtiquetas With {.Titulo = "7. Código de moneda de indexación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas6.Add("VIPLANAMORTIZACION", New ProductoEtiquetas With {.Titulo = "VI. Plan de amortización", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("VALORMONEDACON", New ProductoEtiquetas With {.Titulo = "Valor moneda contratada", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("FECHAAAMMDD", New ProductoEtiquetas With {.Titulo = "Fecha AAAA-MM", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})


			ListaEtiquetas6.Add("VIINUMEROCREANTERIORES", New ProductoEtiquetas With {.Titulo = "VII. Número de creditos anteriores", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("1SUSTITUCION", New ProductoEtiquetas With {.Titulo = "1. Sustitución", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("2FRACCIONAMIENTO", New ProductoEtiquetas With {.Titulo = "2. Fraccionamiento", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("3CONSOLIDACION", New ProductoEtiquetas With {.Titulo = "3. Consolidación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("ANUMEROIDCREDANT", New ProductoEtiquetas With {.Titulo = "A. Número de identificación crédito anterior", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("BCODMON", New ProductoEtiquetas With {.Titulo = "B. Código moneda", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("CVALOR", New ProductoEtiquetas With {.Titulo = "C. Valor a sustituir, fraccionar o consolidar", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas6.Add("VIIIIDENTDECLA", New ProductoEtiquetas With {.Titulo = "VIII. Identificación del declarante", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("1NOMBRE", New ProductoEtiquetas With {.Titulo = "1. Nombre", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("2NUMID", New ProductoEtiquetas With {.Titulo = "2. Número de identificación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas6.Add("3FIRMA", New ProductoEtiquetas With {.Titulo = "3. Firma", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})


			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario6View", ListaEtiquetas6)
#End Region
#Region "Formulario 7"
			ListaEtiquetas7.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "No.", .Tooltip = "No.", .TooltipOriginal = "No."})
			ListaEtiquetas7.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 7", .Tooltip = "Formulario No. 7", .TooltipOriginal = "Formulario No. 7"})
			ListaEtiquetas7.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Información de endeudamiento externo otorgado a no residentes", .Tooltip = "Formulario No. 7", .TooltipOriginal = "Formulario No. 7"})
			ListaEtiquetas7.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			ListaEtiquetas7.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por ID y Fecha", .Tooltip = "Filtrar por ID y Fecha", .TooltipOriginal = "Filtrar por ID y Fecha"})
			ListaEtiquetas7.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
			ListaEtiquetas7.Add("LIMPIAR", New ProductoEtiquetas With {.Titulo = "Limpiar selección", .Tooltip = "Limpiar selección", .TooltipOriginal = "Limpiar selección"})

			ListaEtiquetas7.Add("ID", New ProductoEtiquetas With {.Titulo = "Número declaración", .Tooltip = "Número declaración", .TooltipOriginal = "Número declaración"})
			ListaEtiquetas7.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del declarante", .TooltipOriginal = "Nombre del declarante"})
			ListaEtiquetas7.Add("IDENTIFICACION", New ProductoEtiquetas With {.Titulo = "Número de identificación", .Tooltip = "Número de identificación del declarante", .TooltipOriginal = "Número de identificación del declarante"})
			ListaEtiquetas7.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "Valor USD", .Tooltip = "Valor USD", .TooltipOriginal = "Valor USD"})

			ListaEtiquetas7.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
			ListaEtiquetas7.Add("INGRESEFECHA", New ProductoEtiquetas With {.Titulo = "Ingrese una fecha", .Tooltip = "Ingrese una fecha", .TooltipOriginal = "Ingrese una fecha"})

			ListaEtiquetas7.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
			ListaEtiquetas7.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})
			ListaEtiquetas7.Add("2FECHA", New ProductoEtiquetas With {.Titulo = "2. Fecha AAAA-MM-DD", .Tooltip = "2. Fecha AAAA-MM-DD", .TooltipOriginal = "2. Fecha AAAA-MM-DD"})
			ListaEtiquetas7.Add("3NUMEROPRESTAMO", New ProductoEtiquetas With {.Titulo = "3. Número préstamo", .Tooltip = "3. Número préstamo", .TooltipOriginal = "3. Número préstamo"})
			ListaEtiquetas7.Add("3.1NUMEROPRESTAMO", New ProductoEtiquetas With {.Titulo = "3.1 Número de identificación deudor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas7.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
			ListaEtiquetas7.Add("1DESEMBOLSO", New ProductoEtiquetas With {.Titulo = "1. Desembolso", .Tooltip = "1. Desembolso", .TooltipOriginal = "1. Desembolso"})
			ListaEtiquetas7.Add("2NUMERODEC", New ProductoEtiquetas With {.Titulo = "2. Número declaración", .Tooltip = "2. Número declaración", .TooltipOriginal = "2. Número declaración"})
			ListaEtiquetas7.Add("3NUMERAL", New ProductoEtiquetas With {.Titulo = "3. Numeral", .Tooltip = "3. Numeral, si esta lista esta vacia debe agregar los numerales para el formulario 7", .TooltipOriginal = "3. Numeral"})
			ListaEtiquetas7.Add("4CODMONNEG", New ProductoEtiquetas With {.Titulo = "4. Cod. Mon. Neg.", .Tooltip = "4. Codigo Moneda Negegociación", .TooltipOriginal = "4. Codigo Moneda Negegociación"})
			ListaEtiquetas7.Add("5VALORMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "5. Valor moneda negociación", .Tooltip = "5. Valor moneda negociación", .TooltipOriginal = "5. Valor moneda negociación"})
			ListaEtiquetas7.Add("6VALORUSD", New ProductoEtiquetas With {.Titulo = "6. Valor USD", .Tooltip = "6. Valor USD", .TooltipOriginal = "6. Valor USD"})

			ListaEtiquetas7.Add("IIIDENTIFICACIONPRESTAMISTA", New ProductoEtiquetas With {.Titulo = "III. Identificación del prestamista o acreedor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("1TIPO", New ProductoEtiquetas With {.Titulo = "1. Tipo", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("2NUMEROID", New ProductoEtiquetas With {.Titulo = "2. Numero de identificación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("3NOMBRE", New ProductoEtiquetas With {.Titulo = "3. Nombre o razón social", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("4CIUDAD", New ProductoEtiquetas With {.Titulo = "4. Código ciudad", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("5DIRECCION", New ProductoEtiquetas With {.Titulo = "5. Dirección", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("6TELEFONO", New ProductoEtiquetas With {.Titulo = "6. Telefono", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("7EMAIL", New ProductoEtiquetas With {.Titulo = "7. Correo electrónico", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("8CODIGOCIIU", New ProductoEtiquetas With {.Titulo = "8. Código CIIU", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("BUSCARCODIGOCIIU", New ProductoEtiquetas With {.Titulo = "Buscar un código CIIU", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas7.Add("IVDENTIFICACIONDEUDOR", New ProductoEtiquetas With {.Titulo = "IV. Identificación del prestatario o deudor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("1CODIGOASIGNADO", New ProductoEtiquetas With {.Titulo = "1. Código asignado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("BUSCARCODIGOASIGNADO", New ProductoEtiquetas With {.Titulo = "Buscar código asignado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("2NOMBRE", New ProductoEtiquetas With {.Titulo = "2. Nombre o razón social", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("3PAIS", New ProductoEtiquetas With {.Titulo = "3. Pais", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("4TIPOACREEDOR", New ProductoEtiquetas With {.Titulo = "4. Tipo de prestatario o deudor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas7.Add("VDESCRIPCIONPRESTAMO", New ProductoEtiquetas With {.Titulo = "V. Descripción del préstamo", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("1CODIGOPRESTAMO", New ProductoEtiquetas With {.Titulo = "1. Código proposito del préstamo", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("2CODIGOMONEDA", New ProductoEtiquetas With {.Titulo = "2. Código moneda", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("3MONTOCONTRA", New ProductoEtiquetas With {.Titulo = "3. Monto contratado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("4TASAINTERES", New ProductoEtiquetas With {.Titulo = "4. Tasa de interés", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("SPREAD", New ProductoEtiquetas With {.Titulo = "Spread o valor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("5NUMERODEPOSITO", New ProductoEtiquetas With {.Titulo = "5. Número de depósito por financiación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("6INDEXACION", New ProductoEtiquetas With {.Titulo = "6. Indexación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("7CODIGOMONINDEX", New ProductoEtiquetas With {.Titulo = "7. Código de moneda de indexación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas7.Add("VIPLANAMORTIZACION", New ProductoEtiquetas With {.Titulo = "VI. Plan de amortización", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("VALORMONEDACON", New ProductoEtiquetas With {.Titulo = "Valor moneda contratada", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("FECHAAAMMDD", New ProductoEtiquetas With {.Titulo = "Fecha AAAA-MM", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})


			ListaEtiquetas7.Add("VIINUMEROCREANTERIORES", New ProductoEtiquetas With {.Titulo = "VII. Número de creditos anteriores", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("1SUSTITUCION", New ProductoEtiquetas With {.Titulo = "1. Sustitución", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("2FRACCIONAMIENTO", New ProductoEtiquetas With {.Titulo = "2. Fraccionamiento", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("3CONSOLIDACION", New ProductoEtiquetas With {.Titulo = "3. Consolidación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("ANUMEROIDCREDANT", New ProductoEtiquetas With {.Titulo = "A. Número de identificación crédito anterior", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("BCODMON", New ProductoEtiquetas With {.Titulo = "B. Código moneda", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("CVALOR", New ProductoEtiquetas With {.Titulo = "C. Valor a sustituir, fraccionar o consolidar", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

			ListaEtiquetas7.Add("VIIIDENTDECLA", New ProductoEtiquetas With {.Titulo = "VIII. Identificación del declarante", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("1NOMBRE", New ProductoEtiquetas With {.Titulo = "1. Nombre", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("2NUMID", New ProductoEtiquetas With {.Titulo = "2. Número de identificación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
			ListaEtiquetas7.Add("3FIRMA", New ProductoEtiquetas With {.Titulo = "3. Firma", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})


			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario7View", ListaEtiquetas7)
#End Region

			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario1DescripcionOpView", ListaEtiquetas)
			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2DescripcionOpView", ListaEtiquetas2)
			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario3DescripcionOpView", ListaEtiquetas3)
			ListaRetorno.Add("A2FormulariosDivisasWPF.PlanAmortizacionView", ListaEtiquetas6)


			Return ListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ConsultarCombosAplicacion", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Private Shared Function ArmarMensajesPantalla() As Dictionary(Of String, Dictionary(Of String, String))
        Try
            Dim ListaRetorno As New Dictionary(Of String, Dictionary(Of String, String))
            Dim ListaEtiquetas As New Dictionary(Of String, String)
            Dim objProductoEtiquetas As New A2Utilidades.ProductoEtiquetas

            'GENERICO
            ListaEtiquetas.Add("GENERICO_ERRORREGISTRO", "Ocurrio un error al momento de ejecutar la acción.")
            ListaEtiquetas.Add("GENERICO_BUSQUEDANOEXITOSA", "No se encontraron datos que concuerden con los criterios de búsqueda.")
            ListaEtiquetas.Add("GENERICO_ELIMINARREGISTRO", "Está opción elimina el registro seleccionado. ¿Confirma la eliminación de este registro?")
            ListaEtiquetas.Add("GENERICO_ANULARREGISTRO", "Está opción anula el registro seleccionado. ¿Confirma la anulación de este registro?")
			ListaEtiquetas.Add("GENERICO_TITULOADVERTENCIAS", "Nro de inconsistencias [[INCONSISTENCIAS]].")
			ListaEtiquetas.Add("DIVISAS_FORMULARIOYAENVIADO", "Si continua esta declaración se convertirá en una modificación aunque no haya sido enviada")
			ListaEtiquetas.Add("DIVISAS_FORMULARIOMODIFICACION", "Esta declaración no se ha enviado, debe estar enviada para poder cambiar el tipo de operación a modificación")
			ListaEtiquetas.Add("DIVISAS_TITULOFORMULARIOMODIFICACION", "El formulario no se ha enviado")
			ListaEtiquetas.Add("DIVISAS_TITULOFORMULARIOYAENVIADO", "Esta formulario ya ha sido enviado")
			ListaRetorno.Add("GENERICO", ListaEtiquetas)

			Return ListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ConsultarCombosAplicacion", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function
End Class
