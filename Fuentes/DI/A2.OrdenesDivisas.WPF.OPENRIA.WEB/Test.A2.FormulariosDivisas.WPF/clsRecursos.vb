Imports System.Security.Principal
Imports A2Utilidades
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel

Public Class clsRecursos
    Public Shared Async Function Crear() As Task(Of Boolean)
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

			Dim objUsuario As New Usuario(objDiccionarioUsuario)

            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString) Then
                Application.Current.Resources.Remove(Recursos.RecursosApp.A2Consola_AppActiva.ToString)
            End If

            objAplicacion.Parametros = New Dictionary(Of String, Object)

			'objAplicacion.Parametros.Add("URLServicioEjemploPractico", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/EjemploPractico-OPENRIA-WEB-EjemploPracticoDomainServices.svc")
			objAplicacion.Parametros.Add("URLServicio", "http://localhost:56065/A2-OyD-OYDServer-RIA-Web-FormulariosDivisasDomainServices.svc")
			objAplicacion.Parametros.Add("EJECUTAR_APP_SEGUN_AMBIENTE", False)
			objAplicacion.Parametros.Add("URL_SERVICIO_UPLOADS", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/Uploads.aspx")
			objAplicacion.Parametros.Add("URL_SERVICIO_REPORTING", "http://a2sqldllo/ReportServer_SQL2008/Pages/ReportViewer.aspx?%2fDivisas%2f")

			objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Mensajes.ToString, ArmarMensajesPantalla())
            objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Etiquetas.ToString, ArmarEtiquetasPantalla())
			objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Combos.ToString, Await ConsultarCombosAplicacion())

			Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_AppActiva.ToString, objAplicacion)
            Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_ServicioSeguridad.ToString, "http://localhost:1020/A2ServiciosUtilidades_Local/Servicios/SeguridadApp.svc")
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
			Dim mdcProxy = New FormulariosDivisasDomainServices(New System.Uri("http://localhost:56065/A2-OyD-OYDServer-RIA-Web-FormulariosDivisasDomainServices.svc"))
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
			Dim ListaEtiquetas3DescOp As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas4 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim ListaEtiquetas5 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
			Dim objProductoEtiquetas As New A2Utilidades.ProductoEtiquetas

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

            ListaRetorno.Add("GENERICO", ListaEtiquetasGenerico)

			'ETIQUETAS FORMULARIO1
			ListaEtiquetas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 1", .Tooltip = "Formulario No. 1", .TooltipOriginal = "Formulario No. 1"})
			ListaEtiquetas.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Declaración de Cambio por Importacion de Bienes", .Tooltip = "Formulario No. 1", .TooltipOriginal = "Formulario No. 1"})
			ListaEtiquetas.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			ListaEtiquetas.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Orden y Fecha", .Tooltip = "Filtrar por Nro Orden y Fecha", .TooltipOriginal = "Filtrar por Nro Orden y Fecha"})
			ListaEtiquetas.Add("ID", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
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
			ListaEtiquetas.Add("7NUMERO", New ProductoEtiquetas With {.Titulo = "4. Número", .Tooltip = "4. Número", .TooltipOriginal = "4. Número"})
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

			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario1View", ListaEtiquetas)


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
			ListaEtiquetas2.Add("IVDENTIFICACIONEXPORTADOR", New ProductoEtiquetas With {.Titulo = "IV. Identificación del importador", .Tooltip = "IV. Identificación del importador", .TooltipOriginal = "IV. Identificación del importador"})
			ListaEtiquetas2.Add("8TIPO", New ProductoEtiquetas With {.Titulo = "8. Tipo", .Tooltip = "8. Tipo", .TooltipOriginal = "8. Tipo"})
			ListaEtiquetas2.Add("9NUMEROID", New ProductoEtiquetas With {.Titulo = "9. Número de identificación", .Tooltip = "9. Número de identificación", .TooltipOriginal = "9. Número de identificación"})
			ListaEtiquetas2.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
			ListaEtiquetas2.Add("10NOMBRE", New ProductoEtiquetas With {.Titulo = "10. Nombre o razón social", .Tooltip = "10. Nombre o razón social", .TooltipOriginal = "10. Nombre o razón social"})
			ListaEtiquetas2.Add("VDESCRIPCIONOP", New ProductoEtiquetas With {.Titulo = "V. Descripción de la operación", .Tooltip = "V. Descripción de la operación", .TooltipOriginal = "V. Descripción de la operación"})
			ListaEtiquetas2.Add("11CODMONEDA", New ProductoEtiquetas With {.Titulo = "11. Código moneda de reintegro", .Tooltip = "11. Código moneda de giro", .TooltipOriginal = "11. Código moneda de giro"})
			ListaEtiquetas2.Add("12VALORMONEDA", New ProductoEtiquetas With {.Titulo = "12. Valor moneda de reintegro", .Tooltip = "12. Valor moneda de reintegro", .TooltipOriginal = "12. Valor moneda de reintegro"})
			ListaEtiquetas2.Add("13TIPOCAMBIO", New ProductoEtiquetas With {.Titulo = "13. Tipo de cambio a USD", .Tooltip = "13. Tipo de cambio a USD", .TooltipOriginal = "13. Tipo de cambio a USD"})
			ListaEtiquetas2.Add("14NUMERAL", New ProductoEtiquetas With {.Titulo = "14. Numeral", .Tooltip = "14. Numeral", .TooltipOriginal = "14. Numeral"})
			ListaEtiquetas2.Add("NUMERAL", New ProductoEtiquetas With {.Titulo = "Numeral", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
			ListaEtiquetas2.Add("15VALORUSD", New ProductoEtiquetas With {.Titulo = "15. Valor reintegrado USD", .Tooltip = "15. Valor reintegrado USD", .TooltipOriginal = "15. Valor reintegrado USD"})
			ListaEtiquetas2.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "Valor reintegrado USD", .Tooltip = "Valor reintegrado USD", .TooltipOriginal = "Valor reintegrado USD"})
			ListaEtiquetas2.Add("DETALLES", New ProductoEtiquetas With {.Titulo = "Detalles", .Tooltip = "Detalles", .TooltipOriginal = "Detalles"})
			ListaEtiquetas2.Add("16TOTALFOB", New ProductoEtiquetas With {.Titulo = "16. Total valor FOB reintegrado", .Tooltip = "16. Total valor FOB reintegrado", .TooltipOriginal = "16. Total valor FOB reintegrado"})
			ListaEtiquetas2.Add("17TOTALGASTOSEX", New ProductoEtiquetas With {.Titulo = "17. Total gastos de exportación", .Tooltip = "17. Total gastos de exportación", .TooltipOriginal = "17. Total gastos de exportación"})
			ListaEtiquetas2.Add("18DEDUCCIONES", New ProductoEtiquetas With {.Titulo = "18. Deducciones", .Tooltip = "18. Deducciones", .TooltipOriginal = "18. Deducciones"})
			ListaEtiquetas2.Add("19REINTEGRONETO", New ProductoEtiquetas With {.Titulo = "19. Reintegro neto", .Tooltip = "19. Reintegro neto", .TooltipOriginal = "19. Reintegro neto"})
			ListaEtiquetas2.Add("VIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VI. Identificación del declarante", .Tooltip = "VI. Identificación del declarante", .TooltipOriginal = "VI. Identificación del declarante"})
			ListaEtiquetas2.Add("20NOMBRE", New ProductoEtiquetas With {.Titulo = "20. Nombre", .Tooltip = "20. Nombre", .TooltipOriginal = "20. Nombre"})
			ListaEtiquetas2.Add("21NUMID", New ProductoEtiquetas With {.Titulo = "21. Número de identificación", .Tooltip = "21. Número de identificación", .TooltipOriginal = "21. Número de identificación"})
			ListaEtiquetas2.Add("22FIRMA", New ProductoEtiquetas With {.Titulo = "22. Firma", .Tooltip = "22. Firma", .TooltipOriginal = "22. Firma"})
			ListaEtiquetas2.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2View", ListaEtiquetas2)

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
			ListaEtiquetas3.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. TIPO DE OPERACIÓN", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
			ListaEtiquetas3.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número:", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})
			ListaEtiquetas3.Add("2OPERACIONDE", New ProductoEtiquetas With {.Titulo = "2. Operación de:", .Tooltip = "2. Operación de", .TooltipOriginal = "2. Operación de"})
			ListaEtiquetas3.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. IDENTIFICACIÓN DE LA DECLARACIÓN", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
			ListaEtiquetas3.Add("3CIUDAD", New ProductoEtiquetas With {.Titulo = "3. Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
			ListaEtiquetas3.Add("4NITIMC", New ProductoEtiquetas With {.Titulo = "4. Nit del I.M.C. o código cuenta de compensación", .Tooltip = "Nit del I.M.C", .TooltipOriginal = "Nit del I.M.C"})
			ListaEtiquetas3.Add("5FECHA", New ProductoEtiquetas With {.Titulo = "5. Fecha AAAA-MM-DD", .Tooltip = "Fecha AAAA-MM-DD", .TooltipOriginal = "Fecha AAAA-MM-DD"})
			ListaEtiquetas3.Add("6NUMERO", New ProductoEtiquetas With {.Titulo = "6. Número", .Tooltip = "6. Número", .TooltipOriginal = "6. Número"})

			ListaEtiquetas3.Add("IIIIDENTIFICACIONDECLANT", New ProductoEtiquetas With {.Titulo = "III. IDENTIFICACIÓN DE LA DECLARACIÓN DE CAMBIO ANTERIOR", .Tooltip = "III. Identificación de la declaración de cambio anterior", .TooltipOriginal = "III. Identificación de la declaración de cambio anterior"})
			ListaEtiquetas3.Add("7NIT", New ProductoEtiquetas With {.Titulo = "7. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
			ListaEtiquetas3.Add("8FECHA", New ProductoEtiquetas With {.Titulo = "8. Fecha AAAA-MM-DD", .Tooltip = "8. Fecha AAAA-MM-DD", .TooltipOriginal = "8. Fecha AAAA-MM-DD"})
			ListaEtiquetas3.Add("9NUMERO", New ProductoEtiquetas With {.Titulo = "9. Número", .Tooltip = "9. Número", .TooltipOriginal = "9. Número"})

			ListaEtiquetas3.Add("IVDESCRIPCIONOPERACION", New ProductoEtiquetas With {.Titulo = "IV. DESCRIPCIÓN DE LA OPERACIÓN", .Tooltip = "IV. DESCRIPCIÓN DE LA OPERACIÓN", .TooltipOriginal = "IV. DESCRIPCIÓN DE LA OPERACIÓN"})
			ListaEtiquetas3.Add("10NUMEROPRESTAMOAVAL", New ProductoEtiquetas With {.Titulo = "10. Número de préstamo o aval", .Tooltip = "10. Número de préstamo o aval", .TooltipOriginal = "10. Número de préstamo o aval"})
			ListaEtiquetas3.Add("11TIPO", New ProductoEtiquetas With {.Titulo = "11. Tipo", .Tooltip = "11. Tipo", .TooltipOriginal = "11. Tipo"})
			ListaEtiquetas3.Add("12NUMEROIDENTIFIACION", New ProductoEtiquetas With {.Titulo = "12. Número de identificación", .Tooltip = "12. Número de identificación", .TooltipOriginal = "12. Número de identificación"})
			ListaEtiquetas3.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
			ListaEtiquetas3.Add("13NOMBREDEUDOR", New ProductoEtiquetas With {.Titulo = "13. Nombre del deudor o acreedor / avalado o beneficiario residente", .Tooltip = "Nombre del deudor o acreedor / avalado o beneficiario residente", .TooltipOriginal = "Nombre del deudor o acreedor / avalado o beneficiario residente"})
			ListaEtiquetas3.Add("14CODMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "14. Código moneda contratada", .Tooltip = "Código moneda contratada", .TooltipOriginal = "Código moneda contratada"})
			ListaEtiquetas3.Add("15VALORTOTALMONCONTRATADA", New ProductoEtiquetas With {.Titulo = "15. Valor total moneda contratada", .Tooltip = "Valor total moneda contratada", .TooltipOriginal = "Valor total moneda contratada"})
			ListaEtiquetas3.Add("16CODMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "16. Código moneda negociación", .Tooltip = "Código moneda negociación", .TooltipOriginal = "Código moneda negociación"})
			ListaEtiquetas3.Add("17VALORTOTALMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "17. Valor total moneda negociación", .Tooltip = "Valor total moneda negociación", .TooltipOriginal = "Valor total moneda negociación"})
			ListaEtiquetas3.Add("18BASEINTERES", New ProductoEtiquetas With {.Titulo = "18. Base de interés (días por año)", .Tooltip = "Base de interés", .TooltipOriginal = "Base de interés"})
			ListaEtiquetas3.Add("BASEINTERES360", New ProductoEtiquetas With {.Titulo = "360", .Tooltip = "360", .TooltipOriginal = "360"})
			ListaEtiquetas3.Add("BASEINTERES365", New ProductoEtiquetas With {.Titulo = "365", .Tooltip = "365", .TooltipOriginal = "365"})
			ListaEtiquetas3.Add("19TIPOCAMBIOMONEDANEG", New ProductoEtiquetas With {.Titulo = "19.Tipo de cambio moneda negociación", .Tooltip = "Tipo de cambio moneda negociación", .TooltipOriginal = "Tipo de cambio moneda negociación"})
			ListaEtiquetas3.Add("20VALORTOTALDOLARES", New ProductoEtiquetas With {.Titulo = "20. Valor total en dólares", .Tooltip = "Valor total en dólares", .TooltipOriginal = "Valor total en dólares"})
			ListaEtiquetas3.Add("21NOMBREACREEDOR", New ProductoEtiquetas With {.Titulo = "21. Nombre del acreedor (créditos pasivos) o el deudor (créditos activos) o avalista", .Tooltip = "Nombre del acreedor", .TooltipOriginal = "Nombre del acreedor"})

			ListaEtiquetas3.Add("VINFNUMERALESLIQUIDACIONINTERESES", New ProductoEtiquetas With {.Titulo = "V. INFORMACIÓN DE NUMERALES Y LIQUIDACIÓN DE INTERESES", .Tooltip = "V. INFORMACIÓN DE NUMERALES Y LIQUIDACIÓN DE INTERESES", .TooltipOriginal = "V. INFORMACIÓN DE NUMERALES Y LIQUIDACIÓN DE INTERESES"})
			ListaEtiquetas3.Add("22NUMERAL", New ProductoEtiquetas With {.Titulo = "22. Numeral", .Tooltip = "22. Numeral", .TooltipOriginal = "22. Numeral"})
			ListaEtiquetas3.Add("23VALORMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "23. Valor moneda negociación", .Tooltip = "23. Valor moneda negociación", .TooltipOriginal = "23. Valor moneda negociación"})
			ListaEtiquetas3.Add("24VALORMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "24. Valor moneda contratada", .Tooltip = "24. Valor moneda contratada", .TooltipOriginal = "24. Valor moneda contratada"})
			ListaEtiquetas3.Add("25VALORUSD", New ProductoEtiquetas With {.Titulo = "25. Valor USD", .Tooltip = "25. Valor USD", .TooltipOriginal = "25. Valor USD"})
			ListaEtiquetas3.Add("26VALORBASEMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "26. Valor base moneda contratada", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
			ListaEtiquetas3.Add("27FECHAINICIO", New ProductoEtiquetas With {.Titulo = "27. Fecha inicio AAAA-MM-DD", .Tooltip = "Fecha inicio AAAA-MM-DD", .TooltipOriginal = "Fecha inicio AAAA-MM-DD"})
			ListaEtiquetas3.Add("28FECHAFINAL", New ProductoEtiquetas With {.Titulo = "28. Fecha final AAAA-MM-DD", .Tooltip = "Fecha final AAAA-MM-DD", .TooltipOriginal = "Fecha final AAAA-MM-DD"})
			ListaEtiquetas3.Add("29DIAS", New ProductoEtiquetas With {.Titulo = "29. Días", .Tooltip = "29. Días", .TooltipOriginal = "29. Días"})
			ListaEtiquetas3.Add("30TASA", New ProductoEtiquetas With {.Titulo = "30. Tasa", .Tooltip = "30. Tasa", .TooltipOriginal = "30. Tasa"})

			ListaEtiquetas3.Add("VIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VI. IDENTIFICACIÓN DEL DECLARANTE", .Tooltip = "VI. IDENTIFICACIÓN DEL DECLARANTE", .TooltipOriginal = "VI. IDENTIFICACIÓN DEL DECLARANTE"})
			ListaEtiquetas3.Add("31NOMBREDECLARANTE", New ProductoEtiquetas With {.Titulo = "31. Nombre", .Tooltip = "31. Nombre", .TooltipOriginal = "31. Nombre"})
			ListaEtiquetas3.Add("32NUMID", New ProductoEtiquetas With {.Titulo = "32. Número de identificación", .Tooltip = "32. Número de identificación", .TooltipOriginal = "32. Número de identificación"})
			ListaEtiquetas3.Add("33FIRMA", New ProductoEtiquetas With {.Titulo = "33. Firma", .Tooltip = "33. Firma", .TooltipOriginal = "33. Firma"})

			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario3View", ListaEtiquetas3)
#End Region

			ListaEtiquetas3DescOp.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
			ListaEtiquetas3DescOp.Add("NUMERAL", New ProductoEtiquetas With {.Titulo = "14. Numeral", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
			ListaEtiquetas3DescOp.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "15. Valor reintegrado USD", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})

			ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2DescripcionOpView", ListaEtiquetas2)


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
			ListaEtiquetas.Add("DIVISAS_FORMULARIOYAENVIADO", "Esta formulario ya ha sido enviado")
			ListaEtiquetas.Add("DIVISAS_TITULOFORMULARIOYAENVIADO", "Esta formulario ya ha sido enviado")
			ListaRetorno.Add("GENERICO", ListaEtiquetas)

            Return ListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ConsultarCombosAplicacion", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function
End Class
