Imports System.Security.Principal
Imports A2Utilidades
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web
Imports System.Net

Public Class clsRecursos
    Public Shared Async Function Crear(Args As String(), ByVal Optional pstrArchivoConfiguracion As String = "", ByVal Optional pstrContenidoConfiguracion As String = "") As Task(Of Boolean)
        Dim logRetorno As Boolean = False
        Try
            If Not Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_HAK.ToString) Then
                Application.Current.Resources.Add(Recursos.RecursosApp.A2Consola_HAK.ToString, "@2sEG")
            End If

            Dim objAplicacion As New Aplicacion
            Dim objDiccionarioUsuario As New Dictionary(Of String, String)
            Dim strTokenSeguridadUsuario As String = Await CrearTokenSeguridad()
            'Dim strTokenSeguridadUsuario As String = String.Empty


            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UL.ToString(), WindowsIdentity.GetCurrent().Name)
            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UP.ToString(), "")
            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UI.ToString(), "True")
            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UM.ToString(), Environment.MachineName)
            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UN.ToString(), WindowsIdentity.GetCurrent().Name)
            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.UW.ToString(), WindowsIdentity.GetCurrent().Name)
            objDiccionarioUsuario.Add(Recursos.ParametrosUsuario.HA.ToString(), strTokenSeguridadUsuario)


            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString) Then
                Application.Current.Resources.Remove(Recursos.RecursosApp.A2Consola_AppActiva.ToString)
            End If

            objAplicacion.Parametros = New Dictionary(Of String, Object)


            objAplicacion.Parametros.Add("URLServicio", My.Settings.RutaServicioFormularios)
            objAplicacion.Parametros.Add("URLServicioOrdenesDivisas", My.Settings.RutaServicioOrdenes)
            objAplicacion.Parametros.Add("URLServicioPersonas", My.Settings.RutaServicioPersonas)

            objAplicacion.Parametros.Add("EJECUTAR_APP_SEGUN_AMBIENTE", False)
            objAplicacion.Parametros.Add("URL_SERVICIO_UPLOADS", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/Uploads.aspx")
            objAplicacion.Parametros.Add("URL_SERVICIO_REPORTING", "http://a2sqldllo:81/Reportserver_SQL2008/Pages/ReportViewer.aspx?/Divisas/")

            If System.Diagnostics.Debugger.IsAttached Then
                objAplicacion.Parametros("URLServicio") = "http://localhost:1020/DIV_FORMULARIOS/A2-OyD-OYDServer-RIA-Web-FormulariosDivisasDomainServices.svc"
                objAplicacion.Parametros("URLServicioOrdenesDivisas") = "http://localhost:1020/DIV_ORDENES/A2-OyD-OYDServer-RIA-Web-OrdenesDivisasDomainServices.svc"
                objAplicacion.Parametros("URLServicioPersonas") = "http://localhost:1020/Personas/A2-OyD-OYDServer-RIA-Web-PersonasDomainServices.svc"
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

            Dim strUsuario As String = WindowsIdentity.GetCurrent().Name
            Dim strUsuarioSinDominio As String = WindowsIdentity.GetCurrent().Name.ToLower.Replace("alcuadrado\", "")
            Dim strMaquina As String = Environment.MachineName

            Application.Current.Resources.Add("A2VMensajesLog", "0")
            Application.Current.Resources.Add("A2VReporteDemo", "")
            Application.Current.Resources.Add("A2VArchivoConfigRpts", pstrArchivoConfiguracion)
            Application.Current.Resources.Add("A2VXMLConfigRpts", pstrContenidoConfiguracion)
            Application.Current.Resources.Add("A2VUsrNet", WindowsIdentity.GetCurrent().Name)
            Application.Current.Resources.Add("A2VUsrWin", WindowsIdentity.GetCurrent().Name)
            Application.Current.Resources.Add("A2VUsr", strUsuario)
            Application.Current.Resources.Add("A2VSinDominio", "1")
            Application.Current.Resources.Add("A2VMaquina", strMaquina)
            Application.Current.Resources.Add("A2VUsrSinDominio", strUsuarioSinDominio)
            Application.Current.Resources.Add("A2VOpcVisualizacion", "Pantalla;Excel;PDF;Excel - Solo Datos")
            Application.Current.Resources.Add("A2VCultura", "en-US")
            Application.Current.Resources.Add("A2VServicioParam", "http://a2webdllo:5025/OYDServiciosVisor/Servicios/Generales.svc")
            Application.Current.Resources.Add("A2VServicioRS", "http://a2sqldllo:81/Reportserver_SQL2008/ReportService2005.asmx")
            Application.Current.Resources.Add("OYDPlusReporte", "http://a2sqlpru:81/Reportserver_SQL2008/ReportExecution2005.asmx")
            Application.Current.Resources.Add("A2RutaFisicaGeneracion", "\\A2WEBDLLO\SitiosWebDllo\OyDPlat_WPF\OYDServiciosRIA\Uploads")
            Application.Current.Resources.Add("A2RutaWebGeneracion", "http://a2webdllo:5025/OyDServiciosRIA/Uploads/")
            Application.Current.Resources.Add("A2CarpetaReportes", "/Divisas/")
            Application.Current.Resources.Add("A2RutaImprimirCheques", "C:\Alcuadrado\ImprimirCheques\ImprimirReporte.exe")
            Application.Current.Resources.Add("A2VReporteImprimirNotaContableYankees", "/Divisas/Repetir Notas Contables Yankees")
            Application.Current.Resources.Add("A2VFechaLocal", "1")

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
            '#Region "Generico"

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
            '#End Region

            '#Region "Formulario 1"

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
            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario1DescripcionOpView", ListaEtiquetas)
            '#End Region

            '#Region "Formulario 2"

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
            ListaEtiquetas2.Add("17NUMERO", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número", .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("18FECHA", New ProductoEtiquetas With {.Titulo = "Fecha AAAA-MM-DD", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("19CIUDAD", New ProductoEtiquetas With {.Titulo = "Ciudad aduana", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("20NUMERAL", New ProductoEtiquetas With {.Titulo = "20.  Numeral", .Tooltip = "20.  Numeral", .TooltipOriginal = "20.  Numeral"})
            ListaEtiquetas2.Add("21VALORUSD", New ProductoEtiquetas With {.Titulo = "15.  Valor reintegrado USD", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("22TOTALFOB", New ProductoEtiquetas With {.Titulo = "16. Total valor FOB reintegrado", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("23TOTALGASTOSEX", New ProductoEtiquetas With {.Titulo = "17. Total gastos de exportación", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("24DEDUCCIONES", New ProductoEtiquetas With {.Titulo = "18. Deducciones", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("25REINTEGRONETO", New ProductoEtiquetas With {.Titulo = "19. Reintegro neto", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})
            ListaEtiquetas2.Add("FECHAPRESENTACION", New ProductoEtiquetas With {.Titulo = "Fecha de Presentación", .Tooltip = "Fecha de Presentación", .TooltipOriginal = "Fecha de Presentación"})
            ListaEtiquetas2.Add("NUMERALDIAN", New ProductoEtiquetas With {.Titulo = "Numeral", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
            ListaEtiquetas2.Add("VALORDIAN", New ProductoEtiquetas With {.Titulo = "Valor", .Tooltip = .Titulo, .TooltipOriginal = .Titulo})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2View", ListaEtiquetas2)
            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2DescripcionOpView", ListaEtiquetas2)
            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario2DescripcionOpDIANView", ListaEtiquetas2)
            '#End Region
            '#Region "Campos Formularios 3"
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
            ListaEtiquetas3.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})
            ListaEtiquetas3.Add("2OPERACIONDE", New ProductoEtiquetas With {.Titulo = "2. Operación de", .Tooltip = "2. Operación de", .TooltipOriginal = "2. Operación de"})
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
            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario3DescripcionOpView", ListaEtiquetas3)

            '#Region "Campos Formularios 4"
            'ETIQUETAS FORMULARIO4 RABP20180921
            ListaEtiquetas4.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 4", .Tooltip = "Formulario No. 5", .TooltipOriginal = "Formulario No. 4"})
            ListaEtiquetas4.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Declaración de Cambio por inversiones internacionales. Formulario No. 4", .Tooltip = "Formulario No. 4", .TooltipOriginal = "Formulario No. 4"})
            ListaEtiquetas4.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetas4.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Orden y Fecha", .Tooltip = "Filtrar por Nro Orden y Fecha", .TooltipOriginal = "Filtrar por Nro Orden y Fecha"})
            ListaEtiquetas4.Add("ID", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetas4.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetas4.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del formulario", .TooltipOriginal = "Estado del formulario"})
            ListaEtiquetas4.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
            ListaEtiquetas4.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
            ListaEtiquetas4.Add("ORDEN", New ProductoEtiquetas With {.Titulo = "Orden", .Tooltip = "Numero de la orden", .TooltipOriginal = "Numero de la orden"})
            ListaEtiquetas4.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .Tooltip = "Nro identificación del cliente", .TooltipOriginal = "Nro identificación del cliente"})
            ListaEtiquetas4.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del cliente", .TooltipOriginal = "Usuario creación del cliente"})
            ListaEtiquetas4.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación", .TooltipOriginal = "Fecha creación"})
            ListaEtiquetas4.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del cliente", .TooltipOriginal = "Fecha ultima actualización del cliente"})
            ListaEtiquetas4.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
            ListaEtiquetas4.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})

            ListaEtiquetas4.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
            ListaEtiquetas4.Add("3CIUDAD", New ProductoEtiquetas With {.Titulo = "3. Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetas4.Add("4NITIMC", New ProductoEtiquetas With {.Titulo = "4. Nit del I.M.C. o código cuenta de compensación", .Tooltip = "Nit del I.M.C", .TooltipOriginal = "Nit del I.M.C"})
            ListaEtiquetas4.Add("5FECHA", New ProductoEtiquetas With {.Titulo = "5. Fecha AAAA-MM-DD", .Tooltip = "Fecha AAAA-MM-DD", .TooltipOriginal = "Fecha AAAA-MM-DD"})
            ListaEtiquetas4.Add("6NUMERO", New ProductoEtiquetas With {.Titulo = "6. Número", .Tooltip = "6. Número", .TooltipOriginal = "6. Número"})
            ListaEtiquetas4.Add("7OPERACIONDE", New ProductoEtiquetas With {.Titulo = "2. Operación de", .Tooltip = "2. Operación de", .TooltipOriginal = "2. Operación de"})

            ListaEtiquetas4.Add("IIIIDENTIFICACIONDECLANT", New ProductoEtiquetas With {.Titulo = "III. Identificación de la declaración de cambio anterior (solo para los tipos op. 3 y 4)", .Tooltip = "III. Identificación de la declaración de cambio anterior", .TooltipOriginal = "III. Identificación de la declaración de cambio anterior"})
            ListaEtiquetas4.Add("8NIT", New ProductoEtiquetas With {.Titulo = "8. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
            ListaEtiquetas4.Add("9FECHA", New ProductoEtiquetas With {.Titulo = "9. Fecha AAAA-MM-DD", .Tooltip = "9. Fecha AAAA-MM-DD", .TooltipOriginal = "9. Fecha AAAA-MM-DD"})
            ListaEtiquetas4.Add("10NUMERO", New ProductoEtiquetas With {.Titulo = "10. Número", .Tooltip = "10. Número", .TooltipOriginal = "10. Número"})

            ListaEtiquetas4.Add("IVDESTINOINVERSION", New ProductoEtiquetas With {.Titulo = "IV. Destino inversión", .Tooltip = "IV. Destino inversión", .TooltipOriginal = "IV. Destino inversión"})
            ListaEtiquetas4.Add("DESTINOINVERSION", New ProductoEtiquetas With {.Titulo = "Destino inversión", .Tooltip = "Destino inversión", .TooltipOriginal = "Destino inversión"})

            ListaEtiquetas4.Add("VIDENTIFICACIONEMPRESA", New ProductoEtiquetas With {.Titulo = "V. Identificación de la empresa receptora o fondo de inversión (Portafolio).", .Tooltip = "V. Identificación de la empresa receptora o fondo de inversión (Portafolio).", .TooltipOriginal = "V. Identificación de la empresa receptora o fondo de inversión (Portafolio)."})
            ListaEtiquetas4.Add("11TIPO", New ProductoEtiquetas With {.Titulo = "11. Tipo", .Tooltip = "11. Tipo", .TooltipOriginal = "11. Tipo"})
            ListaEtiquetas4.Add("12NUMEROIDENTIFIACION", New ProductoEtiquetas With {.Titulo = "12. Número de identificación", .Tooltip = "12. Número de identificación", .TooltipOriginal = "12. Número de identificación"})
            ListaEtiquetas4.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
            ListaEtiquetas4.Add("13NOMBREORAZONSOCIAL", New ProductoEtiquetas With {.Titulo = "13. Nombre o razón social", .Tooltip = "Nombre o razón social", .TooltipOriginal = "Nombre o razón social"})
            ListaEtiquetas4.Add("14PAIS", New ProductoEtiquetas With {.Titulo = "14. País", .Tooltip = "País", .TooltipOriginal = "País"})
            ListaEtiquetas4.Add("15CIUDAD", New ProductoEtiquetas With {.Titulo = "15. Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetas4.Add("16TELEFONO", New ProductoEtiquetas With {.Titulo = "16. Teléfono", .Tooltip = "Teléfono", .TooltipOriginal = "Teléfono"})
            ListaEtiquetas4.Add("17CIIU", New ProductoEtiquetas With {.Titulo = "17. Ciiu", .Tooltip = "Ciiu", .TooltipOriginal = "Ciiu"})

            ListaEtiquetas4.Add("VIIDENTIFICACIONINVERSIONISTA", New ProductoEtiquetas With {.Titulo = "VI. Identificación inversionista", .Tooltip = "VI. Identificación inversionista", .TooltipOriginal = "VI. Identificación inversionista"})
            ListaEtiquetas4.Add("18TIPO", New ProductoEtiquetas With {.Titulo = "18. Tipo", .Tooltip = "Tipo de indentificación", .TooltipOriginal = "Tipo de indentificación"})
            ListaEtiquetas4.Add("19NROIDENTIFICACION", New ProductoEtiquetas With {.Titulo = "19. Nro. identificación", .Tooltip = "Nro. identificación", .TooltipOriginal = "Nro. identificación"})
            ListaEtiquetas4.Add("19DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
            ListaEtiquetas4.Add("20NOMBRE", New ProductoEtiquetas With {.Titulo = "20. Nombre", .Tooltip = "20. Nombre", .TooltipOriginal = "20. Nombre"})
            ListaEtiquetas4.Add("21PAIS", New ProductoEtiquetas With {.Titulo = "21. País", .Tooltip = "21. País", .TooltipOriginal = "21. País"})
            ListaEtiquetas4.Add("22CIIU", New ProductoEtiquetas With {.Titulo = "22. Ciiu", .Tooltip = "22. Ciiu", .TooltipOriginal = "22. Ciiu"})

            ListaEtiquetas4.Add("VIIDESCRIPCIONOPERACION", New ProductoEtiquetas With {.Titulo = "VII. Descripción operación", .Tooltip = "VII. Descripción operación", .TooltipOriginal = "VII. Descripción operación"})
            ListaEtiquetas4.Add("23NUMERAL", New ProductoEtiquetas With {.Titulo = "23. Numeral", .Tooltip = "23. Numeral", .TooltipOriginal = "23. Numeral"})
            ListaEtiquetas4.Add("24NOMBREMONEDAGIRO", New ProductoEtiquetas With {.Titulo = "24.Nombre moneda giro", .Tooltip = "24.Nombre moneda giro", .TooltipOriginal = "24.Nombre moneda giro"})
            ListaEtiquetas4.Add("25VALORMONEDAGIRO", New ProductoEtiquetas With {.Titulo = "25.Valor moneda giro", .Tooltip = "25.Valor moneda giro", .TooltipOriginal = "25.Valor moneda giro"})
            ListaEtiquetas4.Add("26TIPOCAMBIOUSD", New ProductoEtiquetas With {.Titulo = "26.Tipo cambio a USD", .Tooltip = "26.Tipo cambio a USD", .TooltipOriginal = "26.Tipo cambio a USD"})
            ListaEtiquetas4.Add("27VALORUSD", New ProductoEtiquetas With {.Titulo = "27.Valor USD", .Tooltip = "27.Valor USD", .TooltipOriginal = "27.Valor USD"})
            ListaEtiquetas4.Add("28TIPOCAMBIOPESOS", New ProductoEtiquetas With {.Titulo = "28.Tipo cambio a pesos", .Tooltip = "28.Tipo cambio a pesos", .TooltipOriginal = "28.Tipo cambio a pesos"})
            ListaEtiquetas4.Add("29VALORMONEDAPESOS", New ProductoEtiquetas With {.Titulo = "29.Valor moneda en pesos", .Tooltip = "29.Valor moneda en pesos", .TooltipOriginal = "29.Valor moneda en pesos"})
            ListaEtiquetas4.Add("30ACCIONES", New ProductoEtiquetas With {.Titulo = "30.Acciones", .Tooltip = "30.Acciones", .TooltipOriginal = "30.Acciones"})
            ListaEtiquetas4.Add("31INVERSIONPLAZOS", New ProductoEtiquetas With {.Titulo = "31.Inversión a plazos", .Tooltip = "31.Inversión a plazos", .TooltipOriginal = "31.Inversión a plazos"})

            ListaEtiquetas4.Add("VIIIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VIII. Identificación del declarante", .Tooltip = "VIII. Identificación del declarante", .TooltipOriginal = "VIII. Identificación del declarante"})
            ListaEtiquetas4.Add("32TIPO", New ProductoEtiquetas With {.Titulo = "32. Tipo", .Tooltip = "32. Tipo", .TooltipOriginal = "32. Tipo"})
            ListaEtiquetas4.Add("33NROIDENTIFICACION", New ProductoEtiquetas With {.Titulo = "33. Número de identificación", .Tooltip = "33. Número de identificación", .TooltipOriginal = "33. Número de identificación"})
            ListaEtiquetas4.Add("33DV", New ProductoEtiquetas With {.Titulo = "D.V.", .Tooltip = "D.V.", .TooltipOriginal = "D.V."})
            ListaEtiquetas4.Add("34NOMBERAZONSOIAL", New ProductoEtiquetas With {.Titulo = "34. Nombre o razón social", .Tooltip = "34. Nombre o razón social", .TooltipOriginal = "34. Nombre o razón social"})
            ListaEtiquetas4.Add("35TELEFONO", New ProductoEtiquetas With {.Titulo = "35. Teléfono", .Tooltip = "35. Teléfono", .TooltipOriginal = "35. Teléfono"})
            ListaEtiquetas4.Add("36DIRECCION", New ProductoEtiquetas With {.Titulo = "36. Dirección", .Tooltip = "36. Dirección", .TooltipOriginal = "36. Dirección"})
            ListaEtiquetas4.Add("37CIUDADDECLARANTE", New ProductoEtiquetas With {.Titulo = "37. Ciudad del declarante", .Tooltip = "37. Ciudad del declarante", .TooltipOriginal = "37. Ciudad del declarante"})
            ListaEtiquetas4.Add("38CORREOELECTRONICO", New ProductoEtiquetas With {.Titulo = "38. Correo electrónico", .Tooltip = "38. Correo electrónico", .TooltipOriginal = "38. Correo electrónico"})

            ListaEtiquetas4.Add("XOBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})
            ListaEtiquetas4.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario4View", ListaEtiquetas4)

            '#Region "Campos Formularios 5"
            'ETIQUETAS FORMULARIO5
            ListaEtiquetas5.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 5", .Tooltip = "Formulario No. 5", .TooltipOriginal = "Formulario No. 5"})
            ListaEtiquetas5.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Declaración de Cambio por servicios, transferencias y otros. Formulario No. 5", .Tooltip = "Formulario No. 5", .TooltipOriginal = "Formulario No. 5"})
            ListaEtiquetas5.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetas5.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Orden y Fecha", .Tooltip = "Filtrar por Nro Orden y Fecha", .TooltipOriginal = "Filtrar por Nro Orden y Fecha"})
            ListaEtiquetas5.Add("ID", New ProductoEtiquetas With {.Titulo = "Número", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetas5.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetas5.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del formulario", .TooltipOriginal = "Estado del formulario"})
            ListaEtiquetas5.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
            ListaEtiquetas5.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
            ListaEtiquetas5.Add("ORDEN", New ProductoEtiquetas With {.Titulo = "Orden", .Tooltip = "Numero de la orden", .TooltipOriginal = "Numero de la orden"})
            ListaEtiquetas5.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .Tooltip = "Nro identificación del cliente", .TooltipOriginal = "Nro identificación del cliente"})
            ListaEtiquetas5.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del cliente", .TooltipOriginal = "Usuario creación del cliente"})
            ListaEtiquetas5.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación", .TooltipOriginal = "Fecha creación"})
            ListaEtiquetas5.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del cliente", .TooltipOriginal = "Fecha ultima actualización del cliente"})
            ListaEtiquetas5.Add("ITIPOOPERACION", New ProductoEtiquetas With {.Titulo = "I. Tipo de operación", .Tooltip = "I. Tipo de operación", .TooltipOriginal = "I. Tipo de operación"})
            ListaEtiquetas5.Add("1NUMERO", New ProductoEtiquetas With {.Titulo = "1. Número", .Tooltip = "1. Número", .TooltipOriginal = "1. Número"})

            ListaEtiquetas5.Add("IIIDENTIFICACIONDECL", New ProductoEtiquetas With {.Titulo = "II. Identificación de la declaración", .Tooltip = "II. Identificación de la declaración", .TooltipOriginal = "II. Identificación de la declaración"})
            ListaEtiquetas5.Add("3CIUDAD", New ProductoEtiquetas With {.Titulo = "3. Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetas5.Add("4NITIMC", New ProductoEtiquetas With {.Titulo = "4. Nit del I.M.C. o código cuenta de compensación", .Tooltip = "Nit del I.M.C", .TooltipOriginal = "Nit del I.M.C"})
            ListaEtiquetas5.Add("5FECHA", New ProductoEtiquetas With {.Titulo = "5. Fecha AAAA-MM-DD", .Tooltip = "Fecha AAAA-MM-DD", .TooltipOriginal = "Fecha AAAA-MM-DD"})
            ListaEtiquetas5.Add("6NUMERO", New ProductoEtiquetas With {.Titulo = "6. Número", .Tooltip = "6. Número", .TooltipOriginal = "6. Número"})
            ListaEtiquetas5.Add("7OPERACIONDE", New ProductoEtiquetas With {.Titulo = "2. Operación de", .Tooltip = "2. Operación de", .TooltipOriginal = "2. Operación de"})

            ListaEtiquetas5.Add("IIIIDENTIFICACIONDECLANT", New ProductoEtiquetas With {.Titulo = "III. Identificación de la declaración de cambio anterior (solo para los tipos op. 3 y 4)", .Tooltip = "III. Identificación de la declaración de cambio anterior", .TooltipOriginal = "III. Identificación de la declaración de cambio anterior"})
            ListaEtiquetas5.Add("8NIT", New ProductoEtiquetas With {.Titulo = "8. NIT del I.M.C o Código cuenta de compensación", .Tooltip = "NIT del I.M.C o Código cuenta de compensación", .TooltipOriginal = "NIT del I.M.C o Código cuenta de compensación"})
            ListaEtiquetas5.Add("9FECHA", New ProductoEtiquetas With {.Titulo = "9. Fecha AAAA-MM-DD", .Tooltip = "9. Fecha AAAA-MM-DD", .TooltipOriginal = "9. Fecha AAAA-MM-DD"})
            ListaEtiquetas5.Add("10NUMERO", New ProductoEtiquetas With {.Titulo = "10. Número", .Tooltip = "10. Número", .TooltipOriginal = "10. Número"})

            ListaEtiquetas5.Add("IVIDENTIFICACIONEMPRESA", New ProductoEtiquetas With {.Titulo = "IV. Identificación de la empresa o persona natural que compra o vende.", .Tooltip = "IV. Identificación de la empresa o persona natural que compra o vende.", .TooltipOriginal = "IV. Identificación de la empresa o persona natural que compra o vende."})
            ListaEtiquetas5.Add("11TIPO", New ProductoEtiquetas With {.Titulo = "11. Tipo", .Tooltip = "11. Tipo", .TooltipOriginal = "11. Tipo"})
            ListaEtiquetas5.Add("12NUMEROIDENTIFIACION", New ProductoEtiquetas With {.Titulo = "12. Número de identificación", .Tooltip = "12. Número de identificación", .TooltipOriginal = "12. Número de identificación"})
            ListaEtiquetas5.Add("DV", New ProductoEtiquetas With {.Titulo = "DV", .Tooltip = "Digito de verificacón", .TooltipOriginal = "Digito de verificacón"})
            ListaEtiquetas5.Add("13NOMBREORAZONSOCIAL", New ProductoEtiquetas With {.Titulo = "13. Nombre o razón social", .Tooltip = "Nombre o razón social", .TooltipOriginal = "Nombre o razón social"})
            ListaEtiquetas5.Add("14CIUDAD", New ProductoEtiquetas With {.Titulo = "14. Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetas5.Add("15TELEFONO", New ProductoEtiquetas With {.Titulo = "15. Teléfono", .Tooltip = "Teléfono", .TooltipOriginal = "Teléfono"})
            ListaEtiquetas5.Add("16DIRECCION", New ProductoEtiquetas With {.Titulo = "16. Dirección", .Tooltip = "Dirección", .TooltipOriginal = "Dirección"})

            ListaEtiquetas5.Add("VDESCRIPCIONOPERACION", New ProductoEtiquetas With {.Titulo = "V. Descripción de la operación", .Tooltip = "V. Descripción de la operación", .TooltipOriginal = "V. Descripción de la operación"})
            ListaEtiquetas5.Add("17CODMONEDACONTRATADA", New ProductoEtiquetas With {.Titulo = "17. Código moneda", .Tooltip = "Código moneda", .TooltipOriginal = "Código moneda"})
            ListaEtiquetas5.Add("18TIPOCAMBIOMONEDAUSD", New ProductoEtiquetas With {.Titulo = "18.Tipo de cambio USD", .Tooltip = "Tipo de cambio moneda negociación", .TooltipOriginal = "Tipo de cambio moneda negociación"})
            ListaEtiquetas5.Add("19VALORMONEDANEGOCIACION", New ProductoEtiquetas With {.Titulo = "19. Valor moneda", .Tooltip = "19. Valor moneda", .TooltipOriginal = "19. Valor moneda"})
            ListaEtiquetas5.Add("20VALORUSD", New ProductoEtiquetas With {.Titulo = "20. Valor USD", .Tooltip = "20. Valor USD", .TooltipOriginal = "20. Valor USD"})

            ListaEtiquetas5.Add("VINFORMACIONOPERACION", New ProductoEtiquetas With {.Titulo = "VI. Información de la operación", .Tooltip = "VI. Información de la operación", .TooltipOriginal = "VI. Información de la operación"})
            ListaEtiquetas5.Add("21NUMERAL", New ProductoEtiquetas With {.Titulo = "21. Numeral", .Tooltip = "21. Numeral", .TooltipOriginal = "21. Numeral"})
            ListaEtiquetas5.Add("VALORUSD", New ProductoEtiquetas With {.Titulo = "22.Valor USD", .Tooltip = "22. Valor USD", .TooltipOriginal = "22. Valor USD"})

            ListaEtiquetas5.Add("VIIDDECLARANTE", New ProductoEtiquetas With {.Titulo = "VII. Identificación del declarante", .Tooltip = "VII. Identificación del declarante", .TooltipOriginal = "VII. Identificación del declarante"})
            ListaEtiquetas5.Add("22NOMBRE", New ProductoEtiquetas With {.Titulo = "22. Nombre", .Tooltip = "22. Nombre", .TooltipOriginal = "22. Nombre"})
            ListaEtiquetas5.Add("23NUMID", New ProductoEtiquetas With {.Titulo = "23. Número de identificación", .Tooltip = "23. Número de identificación", .TooltipOriginal = "23. Número de identificación"})
            ListaEtiquetas5.Add("24FIRMA", New ProductoEtiquetas With {.Titulo = "24. Firma", .Tooltip = "24. Firma", .TooltipOriginal = "24. Firma"})
            ListaEtiquetas5.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario5View", ListaEtiquetas5)
            ListaRetorno.Add("A2FormulariosDivisasWPF.Formulario5DescripcionOpView", ListaEtiquetas5)



            '#End Region
            '#Region "Formulario 6"
            ListaEtiquetas6.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "No.", .Tooltip = "No.", .TooltipOriginal = "No."})
            ListaEtiquetas6.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 6", .Tooltip = "Formulario No. 6", .TooltipOriginal = "Formulario No. 6"})
            ListaEtiquetas6.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Información de endeudamiento externo otorgado a residentes", .Tooltip = "Formulario No. 6", .TooltipOriginal = "Formulario No. 6"})
            ListaEtiquetas6.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetas6.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por ID y Fecha", .Tooltip = "Filtrar por ID y Fecha", .TooltipOriginal = "Filtrar por ID y Fecha"})
            ListaEtiquetas6.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
            ListaEtiquetas6.Add("LIMPIAR", New ProductoEtiquetas With {.Titulo = "Limpiar selección", .Tooltip = "Limpiar selección", .TooltipOriginal = "Limpiar selección"})

            ListaEtiquetas6.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número declaración", .TooltipOriginal = "Número declaración"})
            ListaEtiquetas6.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre Deudor", .Tooltip = "Nombre del deudor", .TooltipOriginal = "Nombre del deudor"})
            ListaEtiquetas6.Add("IDENTIFICACION", New ProductoEtiquetas With {.Titulo = "Número del prestamo", .Tooltip = "Número del prestamo", .TooltipOriginal = "Número de identificación del declarante"})
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
            ListaRetorno.Add("A2FormulariosDivisasWPF.PlanAmortizacionView", ListaEtiquetas6)
            '#End Region
            '#Region "Formulario 7"
            ListaEtiquetas7.Add("IDDESC", New ProductoEtiquetas With {.Titulo = "No.", .Tooltip = "No.", .TooltipOriginal = "No."})
            ListaEtiquetas7.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Formulario No. 7", .Tooltip = "Formulario No. 7", .TooltipOriginal = "Formulario No. 7"})
            ListaEtiquetas7.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Información de endeudamiento externo otorgado a no residentes", .Tooltip = "Formulario No. 7", .TooltipOriginal = "Formulario No. 7"})
            ListaEtiquetas7.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetas7.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por ID y Fecha", .Tooltip = "Filtrar por ID y Fecha", .TooltipOriginal = "Filtrar por ID y Fecha"})
            ListaEtiquetas7.Add("IMPRIMIR", New ProductoEtiquetas With {.Titulo = "Imprimir", .Tooltip = "Imprimir", .TooltipOriginal = "Imprimir"})
            ListaEtiquetas7.Add("LIMPIAR", New ProductoEtiquetas With {.Titulo = "Limpiar selección", .Tooltip = "Limpiar selección", .TooltipOriginal = "Limpiar selección"})

            ListaEtiquetas7.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número ID", .TooltipOriginal = "Número ID"})
            ListaEtiquetas7.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre del deudor", .Tooltip = "Nombre del deudor", .TooltipOriginal = "Nombre del deudor"})
            ListaEtiquetas7.Add("IDENTIFICACION", New ProductoEtiquetas With {.Titulo = "Número del prestamo", .Tooltip = "Número del prestamo", .TooltipOriginal = "Número del prestamo"})
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

            'ETIQUETAS Ordenes
            Dim ListaEtiquetasOrdenes As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas) 'Etiquetas encabezado órdenes 

            ListaEtiquetasOrdenes.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Órdenes", .Tooltip = "Órdenes", .TooltipOriginal = "Órdenes"})
            ListaEtiquetasOrdenes.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Órdenes del mercado", .Tooltip = "mercado", .TooltipOriginal = "mercado"})
            ListaEtiquetasOrdenes.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasOrdenes.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Id, consecutivo, versión, producto, comitente, tipo y fecha de orden",
                                      .Tooltip = "Filtrar por Id, consecutivo, versión, producto, comitente, tipo y fecha de orden",
                                      .TooltipOriginal = "Filtrar por Id, consecutivo, versión, producto, comitente, tipo y fecha de orden"})
            ListaEtiquetasOrdenes.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasOrdenes.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Consecutivo", .Tooltip = "Consecutivo", .TooltipOriginal = "Consecutivo"})
            ListaEtiquetasOrdenes.Add("VERSION", New ProductoEtiquetas With {.Titulo = "Versión", .Tooltip = "Versión", .TooltipOriginal = "Versión"})
            ListaEtiquetasOrdenes.Add("COMITENTE", New ProductoEtiquetas With {.Titulo = "Comitente", .Tooltip = "Comitente", .TooltipOriginal = "Comitente"})
            ListaEtiquetasOrdenes.Add("TIPOORDEN", New ProductoEtiquetas With {.Titulo = "Tipo de orden", .Tooltip = "Tipo de orden", .TooltipOriginal = "Tipo de orden"})

            ListaEtiquetasOrdenes.Add("FECHAORDEN", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha de la orden", .TooltipOriginal = "Fecha de la orden"})
            ListaEtiquetasOrdenes.Add("FECHAVIGENCIAHASTA", New ProductoEtiquetas With {.Titulo = "Fecha de vigencia", .Tooltip = "Fecha de vigencia", .TooltipOriginal = "Fecha de vigencia"})

            ListaEtiquetasOrdenes.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado",
                                      .Tooltip = "Estado",
                                      .TooltipOriginal = "Estado"})
            ListaEtiquetasOrdenes.Add("FECHAESTADO", New ProductoEtiquetas With {.Titulo = "Fecha estado", .Tooltip = "Fecha estado", .TooltipOriginal = "Fecha estado"})
            ListaEtiquetasOrdenes.Add("PRODUCTO", New ProductoEtiquetas With {.Titulo = "Producto", .Tooltip = "Producto", .TooltipOriginal = "Producto"})
            ListaEtiquetasOrdenes.Add("CLASIFICACIONNEGOCIO", New ProductoEtiquetas With {.Titulo = "Clasficación", .Tooltip = "Clasficación", .TooltipOriginal = "Clasficación"})

            ListaEtiquetasOrdenes.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación", .TooltipOriginal = "Usuario creación"})
            ListaEtiquetasOrdenes.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Fecha creación", .Tooltip = "Fecha creación", .TooltipOriginal = "Fecha creación"})
            ListaEtiquetasOrdenes.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario actualización", .Tooltip = "Usuario actualización", .TooltipOriginal = "Usuario actualización"})
            ListaEtiquetasOrdenes.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})
            ListaEtiquetasOrdenes.Add("DIASCUMPLIMIENTO", New ProductoEtiquetas With {.Titulo = "Dias", .Tooltip = "Dias", .TooltipOriginal = "Dias"})


            ListaRetorno.Add("A2OrdenesDivisasWPF.OrdenesView", ListaEtiquetasOrdenes)

            'Santiago Alexander Vergara Orrego 
            'SV20180711_ORDENES

            'ETIQUETAS Ordenes Receptores
            Dim ListaEtiquetasOrdenesReceptores As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas) 'Etiquetas pantalla Ordenes Receptores

            ListaEtiquetasOrdenesReceptores.Add("TITULORECEPTORES", New ProductoEtiquetas With {.Titulo = "Receptores ordenes", .Tooltip = "Receptores ordenes", .TooltipOriginal = "Receptores ordenes"})
            ListaEtiquetasOrdenesReceptores.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetasOrdenesReceptores.Add("RECEPTOR", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasOrdenesReceptores.Add("CODIGORECEPTOR", New ProductoEtiquetas With {.Titulo = "Código", .Tooltip = "Código receptor", .TooltipOriginal = "Código receptor"})
            ListaEtiquetasOrdenesReceptores.Add("LOGLIDER", New ProductoEtiquetas With {.Titulo = "Líder", .Tooltip = "Líder", .TooltipOriginal = "Líder"})
            ListaEtiquetasOrdenesReceptores.Add("PORCENTAJE", New ProductoEtiquetas With {.Titulo = "Porcentaje", .Tooltip = "Porcentaje participación", .TooltipOriginal = "Porcentaje participación"})
            ListaEtiquetasOrdenesReceptores.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ReceptoresOrdenesView", ListaEtiquetasOrdenesReceptores)


            'ETIQUETAS Ordenes Instrucciones


            Dim ListaEtiquetasOrdenesInstrucciones As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasOrdenesInstrucciones.Add("TITULOINSTRUCCIONES", New ProductoEtiquetas With {.Titulo = "Instrucciones ordenes", .Tooltip = "Instrucciones ordenes", .TooltipOriginal = "Instrucciones ordenes"})
            ListaEtiquetasOrdenesInstrucciones.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetasOrdenesInstrucciones.Add("INSTRUCCION", New ProductoEtiquetas With {.Titulo = "Instrucción", .Tooltip = "Instrucción", .TooltipOriginal = "Instrucción"})
            ListaEtiquetasOrdenesInstrucciones.Add("DETALLE", New ProductoEtiquetas With {.Titulo = "Detalle", .Tooltip = "Detalle", .TooltipOriginal = "Detalle"})
            ListaEtiquetasOrdenesInstrucciones.Add("VALOR", New ProductoEtiquetas With {.Titulo = "Valor", .Tooltip = "Valor", .TooltipOriginal = "Valor"})
            ListaEtiquetasOrdenesInstrucciones.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})
            ListaEtiquetasOrdenesInstrucciones.Add("NATURALEZAOP", New ProductoEtiquetas With {.Titulo = "NaturalezaOP", .Tooltip = "NaturalezaOP", .TooltipOriginal = "NaturalezaOP"})
            ListaEtiquetasOrdenesInstrucciones.Add("COBRAGMF", New ProductoEtiquetas With {.Titulo = "Cobra GMF", .Tooltip = "Cobra GMF", .TooltipOriginal = "Cobra GMF"})
            ListaEtiquetasOrdenesInstrucciones.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})
            ListaEtiquetasOrdenesInstrucciones.Add("CUENTA", New ProductoEtiquetas With {.Titulo = "CUENTA", .Tooltip = "CUENTA", .TooltipOriginal = "CUENTA"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.InstruccionesOrdenesView", ListaEtiquetasOrdenesInstrucciones)


            'ETIQUETAS Ordenes Divisas
            Dim ListaEtiquetasOrdenesDivisas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas) 'Etiquetas encabezado órdenes de divisas

            ListaEtiquetasOrdenesDivisas.Add("MONEDA", New ProductoEtiquetas With {.Titulo = "Moneda",
                                             .Tooltip = "Moneda", .TooltipOriginal = "Moneda"})
            ListaEtiquetasOrdenesDivisas.Add("TIPOREFERENCIA", New ProductoEtiquetas With {.Titulo = "Tipo referencia",
                                             .Tooltip = "Tipo referencia", .TooltipOriginal = "Tipo referencia"})
            ListaEtiquetasOrdenesDivisas.Add("COMPENSACION", New ProductoEtiquetas With {.Titulo = "Compensación",
                                             .Tooltip = "Compensación", .TooltipOriginal = "Compensación"})
            ListaEtiquetasOrdenesDivisas.Add("FORMULARIO", New ProductoEtiquetas With {.Titulo = "Formiulario",
                                      .Tooltip = "Formiulario",
                                      .TooltipOriginal = "Formiulario"})
            ListaEtiquetasOrdenesDivisas.Add("NUMERAL", New ProductoEtiquetas With {.Titulo = "Numeral",
                                             .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
            ListaEtiquetasOrdenesDivisas.Add("MESA", New ProductoEtiquetas With {.Titulo = "Mesa",
                                             .Tooltip = "Mesa", .TooltipOriginal = "Mesa"})
            ListaEtiquetasOrdenesDivisas.Add("PRODUCTO", New ProductoEtiquetas With {.Titulo = "Producto",
                                             .Tooltip = "Producto", .TooltipOriginal = "Producto"})
            ListaEtiquetasOrdenesDivisas.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad",
                                             .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasOrdenesDivisas.Add("SPREADCOMISION", New ProductoEtiquetas With {.Titulo = "Spread comisión",
                                             .Tooltip = "Spread comisión", .TooltipOriginal = "Spread comisión"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio parcial",
                                             .Tooltip = "Precio parcial", .TooltipOriginal = "Precio parcial"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIOTOTAL", New ProductoEtiquetas With {.Titulo = "Precio total",
                                             .Tooltip = "Precio total", .TooltipOriginal = "Precio total"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIOINFERIOR", New ProductoEtiquetas With {.Titulo = "Precio inferior",
                                             .Tooltip = "Precio inferior", .TooltipOriginal = "Precio inferior"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIOSUPERIOR", New ProductoEtiquetas With {.Titulo = "Precio superior",
                                             .Tooltip = "Precio superior", .TooltipOriginal = "Precio superior"})
            ListaEtiquetasOrdenesDivisas.Add("VALORBRUTO", New ProductoEtiquetas With {.Titulo = "Valor bruto",
                                             .Tooltip = "Valor brutor", .TooltipOriginal = "Valor bruto"})
            ListaEtiquetasOrdenesDivisas.Add("RTEFUENTE", New ProductoEtiquetas With {.Titulo = "Rte fuente",
                                             .Tooltip = "Rte fuente", .TooltipOriginal = "Rte fuente"})
            ListaEtiquetasOrdenesDivisas.Add("VALORTRM", New ProductoEtiquetas With {.Titulo = "TRM",
                                             .Tooltip = "TRM", .TooltipOriginal = "TRM"})
            ListaEtiquetasOrdenesDivisas.Add("BASEIVA", New ProductoEtiquetas With {.Titulo = "Base IVA",
                                             .Tooltip = "Base IVA", .TooltipOriginal = "Base IVA"})
            ListaEtiquetasOrdenesDivisas.Add("VALORIVA", New ProductoEtiquetas With {.Titulo = "Valor IVA",
                                             .Tooltip = "Valor IVA", .TooltipOriginal = "Valor IVA"})
            ListaEtiquetasOrdenesDivisas.Add("VALORNETO", New ProductoEtiquetas With {.Titulo = "Valor neto",
                                             .Tooltip = "Valor neto", .TooltipOriginal = "Valor neto"})
            ListaEtiquetasOrdenesDivisas.Add("VALORGMF", New ProductoEtiquetas With {.Titulo = "Valor GMF",
                                             .Tooltip = "Valor GMF", .TooltipOriginal = "Valor GMF"})
            ListaEtiquetasOrdenesDivisas.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones",
                                             .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})
            ListaEtiquetasOrdenesDivisas.Add("PAIS", New ProductoEtiquetas With {.Titulo = "Pais",
                                             .Tooltip = "Pais", .TooltipOriginal = "Pais"})
            ListaEtiquetasOrdenesDivisas.Add("CIUDAD", New ProductoEtiquetas With {.Titulo = "Ciudad",
                                             .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetasOrdenesDivisas.Add("CODSWIFT", New ProductoEtiquetas With {.Titulo = "Código SWIFT",
                                             .Tooltip = "Código SWIFT", .TooltipOriginal = "Código SWIFT"})
            ListaEtiquetasOrdenesDivisas.Add("FOLIO", New ProductoEtiquetas With {.Titulo = "Folio",
                                             .Tooltip = "Folio", .TooltipOriginal = "Folio"})
            ListaEtiquetasOrdenesDivisas.Add("CUMPLIR", New ProductoEtiquetas With {.Titulo = "Cumplir",
                                             .Tooltip = "Cumplir la orden", .TooltipOriginal = "Cumplir la orden"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.OrdenesDivisasView", ListaEtiquetasOrdenesDivisas)



            Dim ListaEtiquetasCumplirOrden As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasCumplirOrden.Add("TITULOCUMPLIRORDEN", New ProductoEtiquetas With {.Titulo = "Cumplir orden",
                                                .Tooltip = "Cumplir orden", .TooltipOriginal = "Cumplir orden"})
            ListaEtiquetasCumplirOrden.Add("FOLIO", New ProductoEtiquetas With {.Titulo = "Folio",
                                             .Tooltip = "Folio liquidación", .TooltipOriginal = "Folio"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.CumplirOrdenModalView", ListaEtiquetasCumplirOrden)


            'ETIQUETAS Importación de operaciones SET FX
            'Dim ListaEtiquetasImportacionOperacionesSETFX As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            'ListaEtiquetasImportacionOperacionesSETFX.Add("TIPO", New ProductoEtiquetas With {.Titulo = "Tipo", .Tooltip = "Tipo: Compra o Venta", .TooltipOriginal = "Tipo"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("ORDINARIA", New ProductoEtiquetas With {.Titulo = "Ordinaria", .Tooltip = "Ordinaria", .TooltipOriginal = "Ordinaria"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("CLIENTE", New ProductoEtiquetas With {.Titulo = "Cliente", .Tooltip = "Cliente", .TooltipOriginal = "Cliente"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("NEGOCIACION", New ProductoEtiquetas With {.Titulo = "Negociacion", .Tooltip = "Negociacion", .TooltipOriginal = "Negociacion"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("LIMITE", New ProductoEtiquetas With {.Titulo = "Limite", .Tooltip = "Limite", .TooltipOriginal = "Limite"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("REFERENCIA", New ProductoEtiquetas With {.Titulo = "Referencia", .Tooltip = "Referencia", .TooltipOriginal = "Referencia"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("SPREAD", New ProductoEtiquetas With {.Titulo = "Spred", .Tooltip = "Spred", .TooltipOriginal = "Spred"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("PRODUCTO", New ProductoEtiquetas With {.Titulo = "Producto", .Tooltip = "Producto", .TooltipOriginal = "Producto"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("RECEPTOR", New ProductoEtiquetas With {.Titulo = "Receptor", .Tooltip = "Receptor", .TooltipOriginal = "Receptor"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("INSTRUCCION1", New ProductoEtiquetas With {.Titulo = "Instrucción", .Tooltip = "Instrucción", .TooltipOriginal = "Instrucción"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("DETALLE1", New ProductoEtiquetas With {.Titulo = "Detalle", .Tooltip = "Detalle", .TooltipOriginal = "Detalle"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("VALORINSTRUCCION", New ProductoEtiquetas With {.Titulo = "Valor Instrucción", .Tooltip = "Valor Instrucción", .TooltipOriginal = "Valor Instrucción"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("OBSERVACION1", New ProductoEtiquetas With {.Titulo = "Observación", .Tooltip = "Observación", .TooltipOriginal = "Observación"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("FORMULARIO", New ProductoEtiquetas With {.Titulo = "Formulario", .Tooltip = "Formulario", .TooltipOriginal = "Formulario"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("NUMERAL", New ProductoEtiquetas With {.Titulo = "Numeral", .Tooltip = "Numeral", .TooltipOriginal = "Numeral"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("VLRBRUTO", New ProductoEtiquetas With {.Titulo = "Valor Bruto", .Tooltip = "Valor Bruto", .TooltipOriginal = "Valor Bruto"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("VLRNETO", New ProductoEtiquetas With {.Titulo = "Valor Neto", .Tooltip = "Valor Neto", .TooltipOriginal = "Valor Neto"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("VLRIVA", New ProductoEtiquetas With {.Titulo = "Valor IVA", .Tooltip = "Valor IVA", .TooltipOriginal = "Valor IVA"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("COMPENSACION", New ProductoEtiquetas With {.Titulo = "Compensación", .Tooltip = "Compensación", .TooltipOriginal = "Compensación"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("GMF", New ProductoEtiquetas With {.Titulo = "GMF", .Tooltip = "GMF", .TooltipOriginal = "GMF"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("PAIS", New ProductoEtiquetas With {.Titulo = "País", .Tooltip = "País", .TooltipOriginal = "País"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("CIUDAD", New ProductoEtiquetas With {.Titulo = "Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("VALORGIRO", New ProductoEtiquetas With {.Titulo = "Valor Giro", .Tooltip = "Valor Giro", .TooltipOriginal = "Valor Giro"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("FOLIOLIQUIDACION", New ProductoEtiquetas With {.Titulo = "Folio de la operación", .Tooltip = "Folio de la operación", .TooltipOriginal = "Folio de la operación"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("FECHAENTREGA", New ProductoEtiquetas With {.Titulo = "Fecha Entrega", .Tooltip = "Fecha Entrega", .TooltipOriginal = "Fecha Entrega"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("FECHAVIGENCIA", New ProductoEtiquetas With {.Titulo = "Fecha vigencia", .Tooltip = "Fecha vigencia", .TooltipOriginal = "Fecha vigencia"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("SISTEMAOPERACION", New ProductoEtiquetas With {.Titulo = "Sistema Operación", .Tooltip = "Sistema Operación", .TooltipOriginal = "Sistema Operación"})
            'ListaEtiquetasImportacionOperacionesSETFX.Add("HORA", New ProductoEtiquetas With {.Titulo = "Hora", .Tooltip = "Hora", .TooltipOriginal = "Hora"})

            'ListaRetorno.Add("A2OrdenesDivisasWPF.ImportacionOperacionesSETFXView", ListaEtiquetasImportacionOperacionesSETFX)


            'ETIQUETAS Modulos
            Dim ListaEtiquetasModulos As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasModulos.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Módulos", .Tooltip = "Módulos", .TooltipOriginal = "Módulos"})
            ListaEtiquetasModulos.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Configuración de módulos de la aplicación", .Tooltip = "Configuración de módulos de la aplicación",
                                      .TooltipOriginal = "Configuración de módulos de la aplicación"})
            ListaEtiquetasModulos.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasModulos.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Id, módulo y submódulo",
                                      .Tooltip = "Filtrar por Id, módulo y submódulo",
                                      .TooltipOriginal = "Filtrar por Id, módulo y submódulo"})
            ListaEtiquetasModulos.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasModulos.Add("MODULO", New ProductoEtiquetas With {.Titulo = "Módulo", .Tooltip = "Módulo", .TooltipOriginal = "Módulo"})
            ListaEtiquetasModulos.Add("SUBMODULO", New ProductoEtiquetas With {.Titulo = "Submódulo", .Tooltip = "Submódulo", .TooltipOriginal = "Submódulo"})
            ListaEtiquetasModulos.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Descripción", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasModulos.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Consecutivo", .Tooltip = "Consecutivo", .TooltipOriginal = "Consecutivo"})
            ListaEtiquetasModulos.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasModulos.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ModulosView", ListaEtiquetasModulos)


            'ETIQUETAS Modulos estados
            Dim ListaEtiquetasModulosEstados As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasModulosEstados.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Módulos estados", .Tooltip = "Módulos estados", .TooltipOriginal = "Módulos estados"})
            ListaEtiquetasModulosEstados.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Configuración de módulos de la aplicación", .Tooltip = "Configuración de módulos de la aplicación",
                                      .TooltipOriginal = "Configuración de módulos de la aplicación"})
            ListaEtiquetasModulosEstados.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasModulosEstados.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado", .TooltipOriginal = "Estado"})
            ListaEtiquetasModulosEstados.Add("RESTRICTIVO", New ProductoEtiquetas With {.Titulo = "Restrictivo", .Tooltip = "Restrictivo", .TooltipOriginal = "Restrictivo"})
            ListaEtiquetasModulosEstados.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasModulosEstados.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ModulosEstadosView", ListaEtiquetasModulosEstados)
            ListaRetorno.Add("A2OrdenesDivisasWPF.ModulosEstadosModalView", ListaEtiquetasModulosEstados)



            'ETIQUETAS Modulos estados configuración
            Dim ListaEtiquetasModulosEstadosConfiguracion As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasModulosEstadosConfiguracion.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Configuración estados módulo", .Tooltip = "Configuración estados módulo", .TooltipOriginal = "Configuración estados módulo"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Configuración de la secuencia de estados según módulo",
                                                          .Tooltip = "Configuración de la secuencia de estados según módulo",
                                                          .TooltipOriginal = "Configuración de la secuencia de estados según módulo"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Id, módulo, estado inicial y estado permitido",
                                      .Tooltip = "Filtrar por Id, módulo, estado inicial y estado permitido",
                                      .TooltipOriginal = "Filtrar por Id, módulo, estado inicial y estado permitido"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("MODULO", New ProductoEtiquetas With {.Titulo = "Módulo", .Tooltip = "Módulo", .TooltipOriginal = "Módulo"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("ESTADOINICIAL", New ProductoEtiquetas With {.Titulo = "Estado inicial", .Tooltip = "Estado inicial", .TooltipOriginal = "Estado inicial"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("ESTADOPERMITIDO", New ProductoEtiquetas With {.Titulo = "Estado permitido", .Tooltip = "Estado permitido", .TooltipOriginal = "Estado permitido"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})
            ListaEtiquetasModulosEstadosConfiguracion.Add("ESTADOREGISTRO", New ProductoEtiquetas With {.Titulo = "Estado registro", .Tooltip = "Estado registro", .TooltipOriginal = "Estado registro"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ModulosEstadosConfiguracionView", ListaEtiquetasModulosEstadosConfiguracion)


            'ETIQUETAS Ajustes Mesas
            'Santiago Alexander Vergara Orrego
            'Julio 11/2018
            'SV20180710_AJUSTESMESAS

            Dim ListaEtiquetasAjustesMesas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasAjustesMesas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Ajustes de mesas", .Tooltip = "Ajustes de mesas", .TooltipOriginal = "Ajustes de mesas"})
            ListaEtiquetasAjustesMesas.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Ajustes entre mesas y ajustes desde bancos",
                                           .Tooltip = "Ajustes entre mesas y ajustes desde bancos",
                                      .TooltipOriginal = "Ajustes entre mesas y ajustes desde bancos"})
            ListaEtiquetasAjustesMesas.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasAjustesMesas.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Id, Tipo, De la mesa, A la mesa",
                                      .Tooltip = "Filtrar por Id, Tipo, De la mesa, A la mesa",
                                      .TooltipOriginal = "Filtrar por Id, Tipo, De la mesa, A la mesa"})
            ListaEtiquetasAjustesMesas.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasAjustesMesas.Add("FECHAAJUSTE", New ProductoEtiquetas With {.Titulo = "Fecha ajuste", .Tooltip = "Fecha ajuste", .TooltipOriginal = "Fecha ajuste"})
            ListaEtiquetasAjustesMesas.Add("TIPOMOVIMIENTO", New ProductoEtiquetas With {.Titulo = "Tipo de movimiento", .Tooltip = "Tipo de movimiento", .TooltipOriginal = "Tipo de movimiento"})
            ListaEtiquetasAjustesMesas.Add("NATURALEZA", New ProductoEtiquetas With {.Titulo = "Naturaleza", .Tooltip = "Naturaleza", .TooltipOriginal = "Naturaleza"})
            ListaEtiquetasAjustesMesas.Add("MESADESDE", New ProductoEtiquetas With {.Titulo = "De la mesa", .Tooltip = "De la mesa", .TooltipOriginal = "De la mesa"})
            ListaEtiquetasAjustesMesas.Add("MESAHACIA", New ProductoEtiquetas With {.Titulo = "A la mesa", .Tooltip = "A la mesa", .TooltipOriginal = "A la mesa"})
            ListaEtiquetasAjustesMesas.Add("BANCO", New ProductoEtiquetas With {.Titulo = "Banco", .Tooltip = "Banco", .TooltipOriginal = "Banco"})
            ListaEtiquetasAjustesMesas.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasAjustesMesas.Add("TASAPROMEDIO", New ProductoEtiquetas With {.Titulo = "Tasa promedio", .Tooltip = "Tasa promedio", .TooltipOriginal = "Tasa promedio"})
            ListaEtiquetasAjustesMesas.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado registro", .Tooltip = "Estado registro", .TooltipOriginal = "Estado registro"})
            ListaEtiquetasAjustesMesas.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})
            ListaEtiquetasAjustesMesas.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasAjustesMesas.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.AjustesMesasView", ListaEtiquetasAjustesMesas)



            Dim ListaEtiquetasCierreDivisas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasCierreDivisas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Cierre divisas", .Tooltip = "Cierre divisas", .TooltipOriginal = "Ajustes de mesas"})
            ListaEtiquetasCierreDivisas.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Cierre de divisas por mesa y moneda",
                                           .Tooltip = "Cierre de divisas por mesa y moneda",
                                      .TooltipOriginal = "Cierre de divisas por mesa y moneda"})
            ListaEtiquetasCierreDivisas.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasCierreDivisas.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por mesa",
                                      .Tooltip = "Filtrar por mesa",
                                      .TooltipOriginal = "Filtrar por mesa"})
            ListaEtiquetasCierreDivisas.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasCierreDivisas.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha cierre", .TooltipOriginal = "Fecha cierre"})
            ListaEtiquetasCierreDivisas.Add("MESA", New ProductoEtiquetas With {.Titulo = "Mesa", .Tooltip = "Mesa", .TooltipOriginal = "Mesa"})
            ListaEtiquetasCierreDivisas.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasCierreDivisas.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})
            ListaEtiquetasCierreDivisas.Add("MONEDA", New ProductoEtiquetas With {.Titulo = "Moneda", .Tooltip = "Moneda", .TooltipOriginal = "Moneda"})
            ListaEtiquetasCierreDivisas.Add("CIERRE", New ProductoEtiquetas With {.Titulo = "Cierre", .Tooltip = "Cierre", .TooltipOriginal = "Cierre"})
            ListaEtiquetasCierreDivisas.Add("CERRAR", New ProductoEtiquetas With {.Titulo = "Cerrar", .Tooltip = "Cerrar", .TooltipOriginal = "Cerrar"})
            ListaEtiquetasCierreDivisas.Add("DESHACER", New ProductoEtiquetas With {.Titulo = "Deshacer", .Tooltip = "Deshacer", .TooltipOriginal = "Deshacer"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.CierreDivisasView", ListaEtiquetasCierreDivisas)


            Dim ListaEtiquetasDestinosInverForm As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasDestinosInverForm.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Configuración destinos inversión",
                                                .Tooltip = "Configuración destinos inversión", .TooltipOriginal = "Configuración destinos inversión"})
            ListaEtiquetasDestinosInverForm.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Configuración de campos según destinos de inversión",
                                           .Tooltip = "Configuración de campos según destinos de inversión",
                                           .TooltipOriginal = "Configuración de campos según destinos de inversión"})
            ListaEtiquetasDestinosInverForm.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasDestinosInverForm.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar",
                                      .Tooltip = "Filtrar",
                                      .TooltipOriginal = "Filtrar"})
            ListaEtiquetasDestinosInverForm.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasDestinosInverForm.Add("FORMULARIO", New ProductoEtiquetas With {.Titulo = "Formulario", .Tooltip = "Formulario", .TooltipOriginal = "Formulario"})
            ListaEtiquetasDestinosInverForm.Add("DESTINOINVERSION", New ProductoEtiquetas With {.Titulo = "Destino inversión", .Tooltip = "Destino inversión", .TooltipOriginal = "Destino inversión"})
            ListaEtiquetasDestinosInverForm.Add("GRUPOFORMULARIO", New ProductoEtiquetas With {.Titulo = "Grupo formulario", .Tooltip = "Grupo formulario", .TooltipOriginal = "Grupo formulario"})
            ListaEtiquetasDestinosInverForm.Add("NUMEROCAMPOFORMULARIO", New ProductoEtiquetas With {.Titulo = "Número campo formulario", .Tooltip = "Número campo formulario", .TooltipOriginal = "Número campo formulario"})
            ListaEtiquetasDestinosInverForm.Add("NOMBRECAMPO", New ProductoEtiquetas With {.Titulo = "Nombre campo", .Tooltip = "Nombre campo", .TooltipOriginal = "Nombre campo"})
            ListaEtiquetasDestinosInverForm.Add("NUMERALCAMBIARIO", New ProductoEtiquetas With {.Titulo = "Numeral cambiario", .Tooltip = "Numeral cambiario", .TooltipOriginal = "Numeral cambiario"})
            ListaEtiquetasDestinosInverForm.Add("EDITABLE", New ProductoEtiquetas With {.Titulo = "Es editable", .Tooltip = "Es editable", .TooltipOriginal = "Es editable"})
            ListaEtiquetasDestinosInverForm.Add("NOMBRECAMPOFORMULARIO", New ProductoEtiquetas With {.Titulo = "Nombre campo formulario", .Tooltip = "Nombre campo formulario", .TooltipOriginal = "Nombre campo formulario"})
            ListaEtiquetasDestinosInverForm.Add("REQUERIDO", New ProductoEtiquetas With {.Titulo = "Es requerido", .Tooltip = "Es requerido", .TooltipOriginal = "Es requerido"})
            ListaEtiquetasDestinosInverForm.Add("APLICATODOSDESTINOSINVERSION", New ProductoEtiquetas With {.Titulo = "Aplica todos destinos inversión", .Tooltip = "Aplica todos destinos inversió", .TooltipOriginal = "Aplica todos destinos inversió"})
            ListaEtiquetasDestinosInverForm.Add("APLICATODOSFORMULARIOS", New ProductoEtiquetas With {.Titulo = "Aplica todos formularios", .Tooltip = "Aplica todos formularios", .TooltipOriginal = "Aplica todos formularios"})
            ListaEtiquetasDestinosInverForm.Add("PERMITIDO", New ProductoEtiquetas With {.Titulo = "Es permitido", .Tooltip = "Es permitido", .TooltipOriginal = "Es permitido"})
            ListaEtiquetasDestinosInverForm.Add("APLICATODOSNUMERALES", New ProductoEtiquetas With {.Titulo = "Aplica todos numerales", .Tooltip = "Aplica todos numerales", .TooltipOriginal = "Aplica todos numerales"})
            ListaEtiquetasDestinosInverForm.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})
            ListaEtiquetasDestinosInverForm.Add("USUARIO", New ProductoEtiquetas With {.Titulo = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})



            ListaRetorno.Add("A2FormulariosDivisasWPF.DestinosInvFormulariosView", ListaEtiquetasDestinosInverForm)

            'ETIQUETAS Exportacion movimientos DIAN
            'Jose Alejandro Pineda Castañeda
            'JAPC20181009          

            Dim ListaEtiquetasExportacionMovDian As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)


            ListaEtiquetasExportacionMovDian.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Exportación Movimiento DIAN Divisas", .Tooltip = "Exportación Movimiento DIAN Divisas", .TooltipOriginal = "Exportación Movimiento DIAN Divisas"})
            ListaEtiquetasExportacionMovDian.Add("FORMATO", New ProductoEtiquetas With {.Titulo = "Formato", .Tooltip = "Formato movimiento exportación", .TooltipOriginal = "Formato movimiento exportación"})
            ListaEtiquetasExportacionMovDian.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Descripción", .Tooltip = "Descripción formato", .TooltipOriginal = "Descripción formato"})
            ListaEtiquetasExportacionMovDian.Add("CODIGO", New ProductoEtiquetas With {.Titulo = "Código", .Tooltip = "Código OyD", .TooltipOriginal = "Código OyD"})
            ListaEtiquetasExportacionMovDian.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Consecutivo año", .Tooltip = "Consecutivo año", .TooltipOriginal = "Consecutivo año"})
            ListaEtiquetasExportacionMovDian.Add("FECHAINICIO", New ProductoEtiquetas With {.Titulo = "Fecha inicial", .Tooltip = "Fecha inicial movimiento", .TooltipOriginal = "Fecha inicial movimiento"})
            ListaEtiquetasExportacionMovDian.Add("FECHAFIN", New ProductoEtiquetas With {.Titulo = "Fecha final", .Tooltip = "Fecha final movimiento", .TooltipOriginal = "Fecha final movimiento"})
            ListaEtiquetasExportacionMovDian.Add("CARGAR", New ProductoEtiquetas With {.Titulo = "Cargar", .Tooltip = "Carga movimientos DIAN", .TooltipOriginal = "Carga movimientos DIAN"})
            ListaEtiquetasExportacionMovDian.Add("EXPORTAR", New ProductoEtiquetas With {.Titulo = "Exportar", .Tooltip = "Exportar movimientos DIAN", .TooltipOriginal = "Exportar movimientos DIAN"})
            ListaEtiquetasExportacionMovDian.Add("TINF", New ProductoEtiquetas With {.Titulo = "tinf", .Tooltip = "tinf", .TooltipOriginal = "tinf"})
            ListaEtiquetasExportacionMovDian.Add("TOP", New ProductoEtiquetas With {.Titulo = "top", .Tooltip = "top", .TooltipOriginal = "top"})
            ListaEtiquetasExportacionMovDian.Add("CCOM", New ProductoEtiquetas With {.Titulo = "ccom", .Tooltip = "ccom", .TooltipOriginal = "ccom"})
            ListaEtiquetasExportacionMovDian.Add("FDEC", New ProductoEtiquetas With {.Titulo = "fdec", .Tooltip = "fdec", .TooltipOriginal = "fdec"})
            ListaEtiquetasExportacionMovDian.Add("NDEC", New ProductoEtiquetas With {.Titulo = "ndec", .Tooltip = "ndec", .TooltipOriginal = "ndec"})
            ListaEtiquetasExportacionMovDian.Add("FDECA", New ProductoEtiquetas With {.Titulo = "fdeca", .Tooltip = "fdeca", .TooltipOriginal = "fdeca"})
            ListaEtiquetasExportacionMovDian.Add("NDECA", New ProductoEtiquetas With {.Titulo = "ndeca", .Tooltip = "ndeca", .TooltipOriginal = "ndeca"})
            ListaEtiquetasExportacionMovDian.Add("TDOC", New ProductoEtiquetas With {.Titulo = "tdoc", .Tooltip = "tdoc", .TooltipOriginal = "tdoc"})
            ListaEtiquetasExportacionMovDian.Add("NID", New ProductoEtiquetas With {.Titulo = "nid", .Tooltip = "nid", .TooltipOriginal = "nid"})
            ListaEtiquetasExportacionMovDian.Add("DV", New ProductoEtiquetas With {.Titulo = "dv", .Tooltip = "dv", .TooltipOriginal = "dv"})
            ListaEtiquetasExportacionMovDian.Add("APL1", New ProductoEtiquetas With {.Titulo = "apl1", .Tooltip = "apl1", .TooltipOriginal = "apl1"})
            ListaEtiquetasExportacionMovDian.Add("APL2", New ProductoEtiquetas With {.Titulo = "apl2", .Tooltip = "apl2", .TooltipOriginal = "apl2"})
            ListaEtiquetasExportacionMovDian.Add("NOM1", New ProductoEtiquetas With {.Titulo = "nom1", .Tooltip = "nom1", .TooltipOriginal = "nom1"})
            ListaEtiquetasExportacionMovDian.Add("NOM2", New ProductoEtiquetas With {.Titulo = "nom2", .Tooltip = "nom2", .TooltipOriginal = "nom2"})
            ListaEtiquetasExportacionMovDian.Add("RAZ", New ProductoEtiquetas With {.Titulo = "raz", .Tooltip = "raz", .TooltipOriginal = "raz"})
            ListaEtiquetasExportacionMovDian.Add("DIR", New ProductoEtiquetas With {.Titulo = "dir", .Tooltip = "dir", .TooltipOriginal = "dir"})
            ListaEtiquetasExportacionMovDian.Add("MUN", New ProductoEtiquetas With {.Titulo = "mun", .Tooltip = "mun", .TooltipOriginal = "mun"})
            ListaEtiquetasExportacionMovDian.Add("CIUD", New ProductoEtiquetas With {.Titulo = "ciud", .Tooltip = "ciud", .TooltipOriginal = "ciud"})
            ListaEtiquetasExportacionMovDian.Add("CMON", New ProductoEtiquetas With {.Titulo = "cmon", .Tooltip = "cmon", .TooltipOriginal = "cmon"})
            ListaEtiquetasExportacionMovDian.Add("VMON", New ProductoEtiquetas With {.Titulo = "vmon", .Tooltip = "vmon", .TooltipOriginal = "vmon"})
            ListaEtiquetasExportacionMovDian.Add("VTUSD", New ProductoEtiquetas With {.Titulo = "vtusd", .Tooltip = "vtusd", .TooltipOriginal = "vtusd"})
            ListaEtiquetasExportacionMovDian.Add("ROW", New ProductoEtiquetas With {.Titulo = "Row Number", .Tooltip = "Row Number", .TooltipOriginal = "Row Number"})
            ListaEtiquetasExportacionMovDian.Add("TODOS", New ProductoEtiquetas With {.Titulo = "Todos", .Tooltip = "Todos", .TooltipOriginal = "Todos"})
            ListaEtiquetasExportacionMovDian.Add("ANO", New ProductoEtiquetas With {.Titulo = "Año", .Tooltip = "Año", .TooltipOriginal = "Año"})
            ListaEtiquetasExportacionMovDian.Add("CDES", New ProductoEtiquetas With {.Titulo = "cdes", .Tooltip = "cdes", .TooltipOriginal = "cdes"})
            ListaEtiquetasExportacionMovDian.Add("CPAG", New ProductoEtiquetas With {.Titulo = "cpag", .Tooltip = "cpag", .TooltipOriginal = "cpag"})
            ListaEtiquetasExportacionMovDian.Add("USD", New ProductoEtiquetas With {.Titulo = "Valor USD", .Tooltip = "Valor USD", .TooltipOriginal = "Valor USD"})
            ListaEtiquetasExportacionMovDian.Add("FECHADEC", New ProductoEtiquetas With {.Titulo = "Fecha Decl", .Tooltip = "Fecha Decl", .TooltipOriginal = "Fecha Decl"})
            ListaEtiquetasExportacionMovDian.Add("IDDEC", New ProductoEtiquetas With {.Titulo = "ID Decl", .Tooltip = "ID Decl", .TooltipOriginal = "ID Decl"})
            ListaEtiquetasExportacionMovDian.Add("FORMULARIO", New ProductoEtiquetas With {.Titulo = "Formulario", .Tooltip = "Formulario", .TooltipOriginal = "Formulario"})
            ListaEtiquetasExportacionMovDian.Add("NUMDEC", New ProductoEtiquetas With {.Titulo = "Numero Decl", .Tooltip = "Numero Decl", .TooltipOriginal = "Numero Decl"})
            ListaEtiquetasExportacionMovDian.Add("OBS", New ProductoEtiquetas With {.Titulo = "obs", .Tooltip = "obs", .TooltipOriginal = "obs"})
            ListaEtiquetasExportacionMovDian.Add("NUM", New ProductoEtiquetas With {.Titulo = "Numero", .Tooltip = "Numero", .TooltipOriginal = "Numero"})
            ListaEtiquetasExportacionMovDian.Add("MON", New ProductoEtiquetas With {.Titulo = "mon", .Tooltip = "mon", .TooltipOriginal = "mon"})
            ListaEtiquetasExportacionMovDian.Add("CAM", New ProductoEtiquetas With {.Titulo = "cam", .Tooltip = "cam", .TooltipOriginal = "cam"})
            ListaEtiquetasExportacionMovDian.Add("FOB", New ProductoEtiquetas With {.Titulo = "fob", .Tooltip = "fob", .TooltipOriginal = "fob"})
            ListaEtiquetasExportacionMovDian.Add("GAS", New ProductoEtiquetas With {.Titulo = "gas", .Tooltip = "gas", .TooltipOriginal = "gas"})
            ListaEtiquetasExportacionMovDian.Add("DED", New ProductoEtiquetas With {.Titulo = "Ded", .Tooltip = "Ded", .TooltipOriginal = "Ded"})
            ListaEtiquetasExportacionMovDian.Add("REIN", New ProductoEtiquetas With {.Titulo = "rein", .Tooltip = "rein", .TooltipOriginal = "rein"})


            ListaRetorno.Add("A2FormulariosDivisasWPF.ExportacionMovDIANView", ListaEtiquetasExportacionMovDian)

            'End JAPC20181009

            'SV20181009_BCOREPUBLICA
            Dim ListaEtiquetasBcoRepublica As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasBcoRepublica.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Generar archivos Banco República", .Tooltip = "Generar archivos Banco República", .TooltipOriginal = "Generar archivos Banco República"})
            ListaEtiquetasBcoRepublica.Add("FORMATO", New ProductoEtiquetas With {.Titulo = "Formato", .Tooltip = "Formato movimiento exportación", .TooltipOriginal = "Formato movimiento exportación"})
            ListaEtiquetasBcoRepublica.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Generación de archivos para reportería legal Banco de la República", .Tooltip = "Generación de archivos para reportería legal Banco de la República", .TooltipOriginal = "Generación de archivos para reportería legal Banco de la República"})
            ListaEtiquetasBcoRepublica.Add("FECHADESDE", New ProductoEtiquetas With {.Titulo = "Fecha desde", .Tooltip = "Fecha desde", .TooltipOriginal = "Fecha desde"})
            ListaEtiquetasBcoRepublica.Add("FECHAHASTA", New ProductoEtiquetas With {.Titulo = "Fecha hasta", .Tooltip = "Fecha hasta", .TooltipOriginal = "Fecha hasta"})
            ListaEtiquetasBcoRepublica.Add("TIPOREGISTRO", New ProductoEtiquetas With {.Titulo = "Tipo registro", .Tooltip = "Tipo registro", .TooltipOriginal = "Tipo registro"})
            ListaEtiquetasBcoRepublica.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasBcoRepublica.Add("FALTANFILTROS", New ProductoEtiquetas With {.Titulo = "No se han digitado todos los campos de busqueda", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasBcoRepublica.Add("ARCHIVO", New ProductoEtiquetas With {.Titulo = "Archivo", .Tooltip = "Archivo", .TooltipOriginal = "Archivo"})
            ListaEtiquetasBcoRepublica.Add("SININFORMACIONFORMULARIOS", New ProductoEtiquetas With {.Titulo = "No hay información para generar el archivo", .Tooltip = "", .TooltipOriginal = ""})

            ListaRetorno.Add("A2FormulariosDivisasWPF.GenerarBcoRepublicaView", ListaEtiquetasBcoRepublica)
            'End SV20181009_BCOREPUBLICA


            'Reporte UIAF
            'Ricardo Barrientos Perez
            'RABP20181017          

            Dim ListaEtiquetasReporteUIAF As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasReporteUIAF.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Reporte mensual de compra y venta UIAF", .Tooltip = "Reporte mensual de compra y venta UIAF", .TooltipOriginal = "Reporte mensual de compra y venta UIAF"})
            ListaEtiquetasReporteUIAF.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Descripción", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasReporteUIAF.Add("FECHAINICIO", New ProductoEtiquetas With {.Titulo = "Fecha inicial", .Tooltip = "Fecha inicial", .TooltipOriginal = "Fecha inicial"})
            ListaEtiquetasReporteUIAF.Add("FECHAFIN", New ProductoEtiquetas With {.Titulo = "Fecha final", .Tooltip = "Fecha final", .TooltipOriginal = "Fecha final"})
            ListaEtiquetasReporteUIAF.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasReporteUIAF.Add("ENVIAR", New ProductoEtiquetas With {.Titulo = "Enviar", .Tooltip = "Envia los datos para ser generados en un archivo plano", .TooltipOriginal = "Envia los datos para ser generados en un archivo plano"})
            ListaEtiquetasReporteUIAF.Add("RUTA", New ProductoEtiquetas With {.Titulo = "Ruta", .Tooltip = "Muestra la ruta donde esta configurado la generación del archivo", .TooltipOriginal = "Muestra la ruta donde esta configurado la generación del archivo"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.ReporteMensualUIAFView", ListaEtiquetasReporteUIAF)

            'SV20181025_SETFX: 
            'Etiquetas para la pantalla de importación de operaciones

            Dim ListaEtiquetasImportacionOperaciones As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasImportacionOperaciones.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Importación operaciones SETFX", .Tooltip = "Importación operaciones SETFX", .TooltipOriginal = "Importación operaciones SETFX"})
            ListaEtiquetasImportacionOperaciones.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Pantalla de importación de archivo de operaciones masivas", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasImportacionOperaciones.Add("IMPORTAR", New ProductoEtiquetas With {.Titulo = "Importar", .Tooltip = "Importar", .TooltipOriginal = "Importar"})
            'ListaEtiquetasImportacionOperaciones.Add("FECHAFIN", New ProductoEtiquetas With {.Titulo = "Fecha final", .Tooltip = "Fecha final", .TooltipOriginal = "Fecha final"})
            'ListaEtiquetasImportacionOperaciones.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            'ListaEtiquetasImportacionOperaciones.Add("ENVIAR", New ProductoEtiquetas With {.Titulo = "Enviar", .Tooltip = "Envia los datos para ser generados en un archivo plano", .TooltipOriginal = "Envia los datos para ser generados en un archivo plano"})
            'ListaEtiquetasImportacionOperaciones.Add("RUTA", New ProductoEtiquetas With {.Titulo = "Ruta", .Tooltip = "Muestra la ruta donde esta configurado la generación del archivo", .TooltipOriginal = "Muestra la ruta donde esta configurado la generación del archivo"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ImportacionOperacionesSETFXView", ListaEtiquetasImportacionOperaciones)


            'SV20181031_CUMPLIMIENTOOPERACIONES
            'Pantalla de cumplimiento de operaciones

            Dim ListaEtiquetasCumplimientoOperaciones As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasCumplimientoOperaciones.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Cumplimiento de operaciones", .Tooltip = "Cumplimiento de operaciones", .TooltipOriginal = "Cumplimiento de operaciones"})
            ListaEtiquetasCumplimientoOperaciones.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Pantalla para el cumplimiento de las operaciones", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasCumplimientoOperaciones.Add("CUMPLIR", New ProductoEtiquetas With {.Titulo = "Cumplir", .Tooltip = "Cumplir", .TooltipOriginal = "Cumplir"})
            ListaEtiquetasCumplimientoOperaciones.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasCumplimientoOperaciones.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Consecutivo", .Tooltip = "Consecutivo", .TooltipOriginal = "Consecutivo"})
            ListaEtiquetasCumplimientoOperaciones.Add("COMITENTE", New ProductoEtiquetas With {.Titulo = "Comitente", .Tooltip = "Comitente", .TooltipOriginal = "Comitente"})
            ListaEtiquetasCumplimientoOperaciones.Add("TIPOORDEN", New ProductoEtiquetas With {.Titulo = "Tipo de orden", .Tooltip = "Tipo de orden", .TooltipOriginal = "Tipo de orden"})
            ListaEtiquetasCumplimientoOperaciones.Add("FECHAORDEN", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha de la orden", .TooltipOriginal = "Fecha de la orden"})
            ListaEtiquetasCumplimientoOperaciones.Add("CLASIFICACION", New ProductoEtiquetas With {.Titulo = "Clasifiación", .Tooltip = "Clasifiación", .TooltipOriginal = "Clasifiación"})
            ListaEtiquetasCumplimientoOperaciones.Add("FOLIO", New ProductoEtiquetas With {.Titulo = "Folio", .Tooltip = "Folio", .TooltipOriginal = "Folio"})
            ListaEtiquetasCumplimientoOperaciones.Add("CONFIRMACION", New ProductoEtiquetas With {.Titulo = "Esta seguro que desea cumplir las operaciones con folios: ", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasCumplimientoOperaciones.Add("VALIDACION", New ProductoEtiquetas With {.Titulo = "Debe digitar un folio para generar el cumplimiento", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasCumplimientoOperaciones.Add("ACTUALIZACIONEXITOSA", New ProductoEtiquetas With {.Titulo = "Se generó el cumplimiento de las operaciones exitosamente", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasCumplimientoOperaciones.Add("VALORNETO", New ProductoEtiquetas With {.Titulo = "Valor neto", .Tooltip = "Valor neto", .TooltipOriginal = "Valor neto"})


            ListaRetorno.Add("A2OrdenesDivisasWPF.CumplimientoView", ListaEtiquetasCumplimientoOperaciones)

            Return ListaRetorno

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ArmarEtiquetasPantalla", Application.Current.ToString(), Program.Maquina, ex)
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
            'JAPC20181009: se añaden mensajes para pantalla ExportacionMovDIAN            
            ListaEtiquetas.Add("DIVISAS_FORMATODIAN", "Debe seleccionar un formato para la exportación movimiento DIAN")
            ListaEtiquetas.Add("DIVISAS_EXPORTACION", "Se realizo el proceso de exportación con exito")
            'END JAPC20181009
            ListaRetorno.Add("GENERICO", ListaEtiquetas)

            Return ListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ArmarMensajesPantalla", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Private Shared Async Function CrearTokenSeguridad() As Task(Of String)
        Try
            Dim strip = IPV4()
            Dim objHandler As New HttpClientHandler
            objHandler.UseDefaultCredentials = True

            Dim objClient As New HttpClient(objHandler)
            Dim pstrToken As String = Program.HashConexionApi()
            pstrToken = HttpUtility.UrlEncode(pstrToken)
            Dim strParametros As String = String.Format("pstrUsuario={0}&pstrInfoConexion={1}",
                                                        WindowsIdentity.GetCurrent().Name,
                                                        pstrToken)
            objClient.BaseAddress = New Uri(My.Settings.RutaServicioUtilidadesApi)
            objClient.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
            Dim strUrlCompleta As String = "/api/A2_Utilidades/ValidacionSeguridad?" & strParametros
            'Dim strUrlCompleta As String = "/api/A2_Utilidades/GetAll?" & strParametros
            Dim objResponse As HttpResponseMessage = Await objClient.GetAsync(strUrlCompleta)
            If objResponse.IsSuccessStatusCode Then
                Dim strRespuestaJson As String = Await objResponse.Content.ReadAsStringAsync()
                Return strRespuestaJson
            Else
                MessageBox.Show("Seguridad Invalida.El usuario ingresado no es válido.")
                Return Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "CrearTokenSeguridad", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    Public Shared Function IPV4() As String
        Try
            Dim strIP As String = String.Empty


            Dim objListaIP = Net.Dns.GetHostEntry(System.Net.Dns.GetHostName())
            If Not objListaIP Is Nothing Then
                Dim listAdress = objListaIP.AddressList
                If Not listAdress Is Nothing Then
                    For Each IPA As IPAddress In listAdress
                        If IPA.AddressFamily.ToString() = "InterNetwork" Then
                            If IPA.ToString().Length <= 15 Then
                                strIP = IPA.ToString()
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If


            Return strIP
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Return Nothing
        End Try
    End Function


End Class
