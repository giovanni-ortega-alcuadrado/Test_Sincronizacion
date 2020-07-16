Imports System.Collections.ObjectModel
Imports System.Configuration
Imports System.Security.Principal
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Recursos

Public Class clsRecursosVisuales
    Public Shared Function CrearRecursosMensajes(ByVal pobjAplicacion As Aplicacion) As Boolean
        Dim logRetorno As Boolean = False
        Try
            If Not IsNothing(pobjAplicacion) Then
                pobjAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Mensajes.ToString, ArmarMensajesPantalla())
                pobjAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Etiquetas.ToString, ArmarEtiquetasPantalla())
            End If

            logRetorno = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "Crear", Application.Current.ToString(), Program.Maquina, ex)
            logRetorno = False
        End Try
        Return logRetorno
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
            ListaEtiquetasOrdenes.Add("FECHAVIGENCIAHASTA", New ProductoEtiquetas With {.Titulo = "Fecha de vencimiento", .Tooltip = "Fecha de vencimiento", .TooltipOriginal = "Fecha de vencimiento"})

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

            'RABP20200407: Etiquetas para los campos de FORWARD
            ListaEtiquetasOrdenes.Add("TIPODERIVADO", New ProductoEtiquetas With {.Titulo = "Tipo derivado", .Tooltip = "Tipo derivado", .TooltipOriginal = "Tipo derivado"})
            ListaEtiquetasOrdenes.Add("TIPOOPERACION", New ProductoEtiquetas With {.Titulo = "Tipo operación", .Tooltip = "Tipo derivado", .TooltipOriginal = "Tipo operación"})
            ListaEtiquetasOrdenes.Add("FIXING", New ProductoEtiquetas With {.Titulo = "Fixing", .Tooltip = "Fixing", .TooltipOriginal = "Fixing"})
            ListaEtiquetasOrdenes.Add("TIPOCONTRAPARTE", New ProductoEtiquetas With {.Titulo = "Tipo contraparte", .Tooltip = "Tipo contraparte", .TooltipOriginal = "Tipo contraparte"})
            ListaEtiquetasOrdenes.Add("OBJETIVO", New ProductoEtiquetas With {.Titulo = "Objetivo", .Tooltip = "Objetivo", .TooltipOriginal = "Objetivo"})

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
            ListaEtiquetasOrdenesInstrucciones.Add("COBRAGMF", New ProductoEtiquetas With {.Titulo = "Cobra GMF", .Tooltip = "Cobra GMF", .TooltipOriginal = "Cobra GMF"})
            ListaEtiquetasOrdenesInstrucciones.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})
            ListaEtiquetasOrdenesInstrucciones.Add("NATURALEZAOP", New ProductoEtiquetas With {.Titulo = "Naturaleza", .Tooltip = "Naturaleza", .TooltipOriginal = "Naturaleza"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.InstruccionesOrdenesView", ListaEtiquetasOrdenesInstrucciones)


            'ETIQUETAS Ordenes Divisas
            Dim ListaEtiquetasOrdenesDivisas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas) 'Etiquetas encabezado órdenes de divisas

            ListaEtiquetasOrdenesDivisas.Add("MONEDA", New ProductoEtiquetas With {.Titulo = "Moneda",
                                             .Tooltip = "Moneda", .TooltipOriginal = "Moneda"})
            ListaEtiquetasOrdenesDivisas.Add("TIPOREFERENCIA", New ProductoEtiquetas With {.Titulo = "Tipo referencia",
                                             .Tooltip = "Tipo referencia", .TooltipOriginal = "Tipo referencia"})
            ListaEtiquetasOrdenesDivisas.Add("DESTINOINVERSION", New ProductoEtiquetas With {.Titulo = "Destino inversión",
                                             .Tooltip = "Destino inversión", .TooltipOriginal = "Destino inversión"})
            ListaEtiquetasOrdenesDivisas.Add("COMPENSACION", New ProductoEtiquetas With {.Titulo = "Compensación",
                                             .Tooltip = "Compensación", .TooltipOriginal = "Compensación"})
            ListaEtiquetasOrdenesDivisas.Add("FORMULARIO", New ProductoEtiquetas With {.Titulo = "Formulario",
                                      .Tooltip = "Formulario",
                                      .TooltipOriginal = "Formulario"})
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
            ListaEtiquetasOrdenesDivisas.Add("CUMPLIR", New ProductoEtiquetas With {.Titulo = "Cumplir", .Tooltip = "Cumplir la orden", .TooltipOriginal = "Cumplir la orden"})

            'RABP20190726_Etiquetas para multimoneda
            ListaEtiquetasOrdenesDivisas.Add("TITULOMULTIMONEDA", New ProductoEtiquetas With {.Titulo = "Multimoneda", .Tooltip = "Multimoneda", .TooltipOriginal = "Multimoneda"})
            ListaEtiquetasOrdenesDivisas.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetasOrdenesDivisas.Add("CANTIDADMULTIMONEDA", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIOINTERMEDIOM", New ProductoEtiquetas With {.Titulo = "Precio intermedio", .Tooltip = "Precio intermedio", .TooltipOriginal = "Precio intermedio"})
            ListaEtiquetasOrdenesDivisas.Add("SPREADCOMISIONM", New ProductoEtiquetas With {.Titulo = "Spread", .Tooltip = "Spread", .TooltipOriginal = "Spread"})
            ListaEtiquetasOrdenesDivisas.Add("VALORBRUTOMULTIMONEDA", New ProductoEtiquetas With {.Titulo = "Valor bruto USD", .Tooltip = "Valor bruto USD", .TooltipOriginal = "Valor bruto USD"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIOMONEDANEGOCIADAM", New ProductoEtiquetas With {.Titulo = "Valor neto USD", .Tooltip = "Valor neto USD", .TooltipOriginal = "Valor neto USD"})
            ListaEtiquetasOrdenesDivisas.Add("COMISIONUSD", New ProductoEtiquetas With {.Titulo = "Comisión USD", .Tooltip = "Comisión USD", .TooltipOriginal = "Comisión USD"})
            ListaEtiquetasOrdenesDivisas.Add("CANTIDADM", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasOrdenesDivisas.Add("PRECIOM", New ProductoEtiquetas With {.Titulo = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            ListaEtiquetasOrdenesDivisas.Add("VALORBRUTOM", New ProductoEtiquetas With {.Titulo = "Valor bruto", .Tooltip = "Valor bruto", .TooltipOriginal = "Valor bruto"})
            ListaEtiquetasOrdenesDivisas.Add("VALORNETOM", New ProductoEtiquetas With {.Titulo = "Valor neto", .Tooltip = "Valor neto", .TooltipOriginal = "Valor neto"})
            ListaEtiquetasOrdenesDivisas.Add("COMISIONCOP", New ProductoEtiquetas With {.Titulo = "Comisión COP", .Tooltip = "Comisión COP", .TooltipOriginal = "Comisión COP"})
            ListaEtiquetasOrdenesDivisas.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            'RABP20200407: Etiquetas para los campos de FORWARD
            ListaEtiquetasOrdenesDivisas.Add("DEVALUACION", New ProductoEtiquetas With {.Titulo = "Devaluación", .Tooltip = "Devaluación", .TooltipOriginal = "Devaluación"})
            ListaEtiquetasOrdenesDivisas.Add("TIPOCUMPLIMIENTO", New ProductoEtiquetas With {.Titulo = "Tipo cumplimiento", .Tooltip = "Tipo cumplimiento", .TooltipOriginal = "Tipo cumplimiento"})
            ListaEtiquetasOrdenesDivisas.Add("VALORTASAFORWARD", New ProductoEtiquetas With {.Titulo = "Tasa forward", .Tooltip = "Tasa forward", .TooltipOriginal = "Tasa forward"})

            'C-20200366:JAPC20200518: Etiquetas para nuevo campo spot en el detalle de la orden
            ListaEtiquetasOrdenesDivisas.Add("VALORSPOT", New ProductoEtiquetas With {.Titulo = "Spot", .Tooltip = "Spot", .TooltipOriginal = "Spot"})


            ListaRetorno.Add("A2OrdenesDivisasWPF.OrdenesDivisasView", ListaEtiquetasOrdenesDivisas)



            Dim ListaEtiquetasCumplirOrden As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasCumplirOrden.Add("TITULOCUMPLIRORDEN", New ProductoEtiquetas With {.Titulo = "Cumplir orden",
                                                .Tooltip = "Cumplir orden", .TooltipOriginal = "Cumplir orden"})
            ListaEtiquetasCumplirOrden.Add("FOLIO", New ProductoEtiquetas With {.Titulo = "Folio",
                                             .Tooltip = "Folio liquidación", .TooltipOriginal = "Folio"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.CumplirOrdenModalView", ListaEtiquetasCumplirOrden)


            'Ricardo Barrientos Pérez 
            'RABP20190722: Etiquestas para pantallas de multimonedas

            'ETIQUETAS Ordenes Receptores
            Dim ListaEtiquetasMultimoneda As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas) 'Etiquetas pantalla Ordenes de multimonedas

            ListaEtiquetasMultimoneda.Add("TITULOMULTIMONEDA", New ProductoEtiquetas With {.Titulo = "Multimoneda", .Tooltip = "Multimoneda", .TooltipOriginal = "Multimoneda"})
            ListaEtiquetasMultimoneda.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetasMultimoneda.Add("MONEDA", New ProductoEtiquetas With {.Titulo = "Moneda", .Tooltip = "Moneda", .TooltipOriginal = "Moneda"})
            ListaEtiquetasMultimoneda.Add("MONEDAINTERMEDIA", New ProductoEtiquetas With {.Titulo = "Moneda Intermedia", .Tooltip = "Moneda Intermedia", .TooltipOriginal = "Moneda Intermedia"})
            ListaEtiquetasMultimoneda.Add("CANTIDADMULTIMONEDA", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasMultimoneda.Add("PRECIOINTERMEDIO", New ProductoEtiquetas With {.Titulo = "Precio intermedio", .Tooltip = "Precio intermedio", .TooltipOriginal = "Precio intermedio"})
            ListaEtiquetasMultimoneda.Add("SPREADCOMISION", New ProductoEtiquetas With {.Titulo = "Spread", .Tooltip = "Spread", .TooltipOriginal = "Spread"})
            ListaEtiquetasMultimoneda.Add("VALORBRUTOMULTIMONEDA", New ProductoEtiquetas With {.Titulo = "Valor bruto", .Tooltip = "Valor bruto", .TooltipOriginal = "Valor bruto"})
            ListaEtiquetasMultimoneda.Add("PRECIOMONEDANEGOCIADA", New ProductoEtiquetas With {.Titulo = "Precio moneda negociada", .Tooltip = "Precio moneda negociada", .TooltipOriginal = "Precio moneda negociada"})
            ListaEtiquetasMultimoneda.Add("COMISIONUSD", New ProductoEtiquetas With {.Titulo = "Comisión USD", .Tooltip = "Comisión USD", .TooltipOriginal = "Comisión USD"})
            ListaEtiquetasMultimoneda.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasMultimoneda.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            ListaEtiquetasMultimoneda.Add("VALORBRUTO", New ProductoEtiquetas With {.Titulo = "Valor bruto", .Tooltip = "Valor bruto", .TooltipOriginal = "Valor bruto"})
            ListaEtiquetasMultimoneda.Add("VALORNETO", New ProductoEtiquetas With {.Titulo = "Valor neto", .Tooltip = "Valor neto", .TooltipOriginal = "Valor neto"})
            ListaEtiquetasMultimoneda.Add("COMISIONCOP", New ProductoEtiquetas With {.Titulo = "Comisión COP", .Tooltip = "Comisión COP", .TooltipOriginal = "Comisión COP"})
            ListaEtiquetasMultimoneda.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.MultimonedaView", ListaEtiquetasMultimoneda)




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

            'ETIQUETAS Control de cierre de operaciones Divisas
            'RABP20200602
            Dim ListaEtiquetasControlCierreDivisas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasControlCierreDivisas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Control Cierre Operaciones Divisas", .Tooltip = "Control Cierre Operaciones Divisas", .TooltipOriginal = "Control Cierre Operaciones Divisas"})
            ListaEtiquetasControlCierreDivisas.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Control del cierre de operaciones Divisas", .Tooltip = "Control del cierre de operaciones Divisas", .TooltipOriginal = "Control del cierre de operaciones Divisas"})
            ListaEtiquetasControlCierreDivisas.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasControlCierreDivisas.Add("FECHACONTROL", New ProductoEtiquetas With {.Titulo = "Fecha control", .Tooltip = "Fecha control", .TooltipOriginal = "Fecha control"})
            ListaEtiquetasControlCierreDivisas.Add("HORACONTROL", New ProductoEtiquetas With {.Titulo = "Hora control", .Tooltip = "Hora control", .TooltipOriginal = "Hora control"})
            ListaEtiquetasControlCierreDivisas.Add("ACTUALIZAR", New ProductoEtiquetas With {.Titulo = "Actualizar", .Tooltip = "Actualizar", .TooltipOriginal = "Actualizar"})
            ListaEtiquetasControlCierreDivisas.Add("CONFIRMACION", New ProductoEtiquetas With {.Titulo = "¿Está seguro de actualizar la hora de control de cierre de Divisas?", .Tooltip = "¿Está seguro de actualizar la hora de control de cierre de Divisas?", .TooltipOriginal = "¿Está seguro de actualizar la hora de control de cierre de Divisas?"})
            ListaEtiquetasControlCierreDivisas.Add("ACTUALIZARCIONEXITOSA", New ProductoEtiquetas With {.Titulo = "Hora actualizada", .Tooltip = "Hora actualizada", .TooltipOriginal = "Hora actualizada"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ControlCierreDivisasView", ListaEtiquetasControlCierreDivisas)

            'ETIQUETAS Historial de valoración de ordenes
            'RABP20200603
            Dim ListaEtiquetasHistorialValoracionDivisas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasHistorialValoracionDivisas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Historia Valoración", .Tooltip = "Historia Valoración", .TooltipOriginal = "Historia Valoración"})
            ListaEtiquetasHistorialValoracionDivisas.Add("SUBTITULO", New ProductoEtiquetas With {.Titulo = "Lista del historial de Ordenes Divisas", .Tooltip = "Lista del historial de Ordenes Divisas", .TooltipOriginal = "Lista del historial de Ordenes Divisas"})
            ListaEtiquetasHistorialValoracionDivisas.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasHistorialValoracionDivisas.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            ListaEtiquetasHistorialValoracionDivisas.Add("VALORDERECHOCOP", New ProductoEtiquetas With {.Titulo = "Valor Derecho COP", .Tooltip = "Valor Derecho CO", .TooltipOriginal = "Valor Derecho CO"})
            ListaEtiquetasHistorialValoracionDivisas.Add("VALOROBLIGACIONCOP", New ProductoEtiquetas With {.Titulo = "Valor Obligacion COP", .Tooltip = "Valor Obligacion COP", .TooltipOriginal = "Valor Obligacion COP"})
            ListaEtiquetasHistorialValoracionDivisas.Add("VALORMERCADOCO", New ProductoEtiquetas With {.Titulo = "Valor Mercado COP", .Tooltip = "Valor Mercado COP", .TooltipOriginal = "Valor Mercado COP"})
            ListaEtiquetasHistorialValoracionDivisas.Add("VALORDERECHOUSD", New ProductoEtiquetas With {.Titulo = "Valor Derecho USD", .Tooltip = "Valor Derecho USD", .TooltipOriginal = "Valor Derecho USD"})
            ListaEtiquetasHistorialValoracionDivisas.Add("VALOROBLIGACIONUSD", New ProductoEtiquetas With {.Titulo = "Valor Obligacion USD", .Tooltip = "Valor Obligacion USa", .TooltipOriginal = "Valor Obligacion US"})
            ListaEtiquetasHistorialValoracionDivisas.Add("VALORMERCADOUSD", New ProductoEtiquetas With {.Titulo = "Valor Mercado USD", .Tooltip = "Valor Mercado USD", .TooltipOriginal = "Valor Mercado USD"})
            ListaEtiquetasHistorialValoracionDivisas.Add("FECHAVALORACION", New ProductoEtiquetas With {.Titulo = "Fecha valoración", .Tooltip = "Fecha Valoración", .TooltipOriginal = "Fecha Valoració"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.HistorialValoracionView", ListaEtiquetasHistorialValoracionDivisas)


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


            'Reporte UIAF
            'Ricardo Barrientos Perez
            'RABP20181017          

            Dim ListaEtiquetasReporteUIAF As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasReporteUIAF.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Reporte mensual de compra y venta UIAF", .Tooltip = "Reporte mensual de compra y venta UIAF", .TooltipOriginal = "Reporte mensual de compra y venta UIAF"})
            ListaEtiquetasReporteUIAF.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Descripción", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasReporteUIAF.Add("FECHAINICIO", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
            ListaEtiquetasReporteUIAF.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasReporteUIAF.Add("ENVIAR", New ProductoEtiquetas With {.Titulo = "Enviar", .Tooltip = "Envia los datos para ser generados en un archivo plano", .TooltipOriginal = "Envia los datos para ser generados en un archivo plano"})
            ListaEtiquetasReporteUIAF.Add("RUTA", New ProductoEtiquetas With {.Titulo = "Ruta", .Tooltip = "Muestra la ruta donde esta configurado la generación del archivo", .TooltipOriginal = "Muestra la ruta donde esta configurado la generación del archivo"})
            ListaEtiquetasReporteUIAF.Add("CODIGOENTIDAD", New ProductoEtiquetas With {.Titulo = "Código Entidad", .Tooltip = "Código de la Entidad registrado en parámetros", .TooltipOriginal = "Código de la Entidad registrado en parámetros"})
            ListaEtiquetasReporteUIAF.Add("FECHACORTE", New ProductoEtiquetas With {.Titulo = "Fecha corte", .Tooltip = "Fecha de consulta de la información", .TooltipOriginal = "Fecha de consulta de la información"})
            ListaEtiquetasReporteUIAF.Add("CONSECUTIVONUMEROREGISTRODET", New ProductoEtiquetas With {.Titulo = "Consecutivo Numero Registro Det", .Tooltip = "Consecutivo Numero Registro Det", .TooltipOriginal = "Consecutivo Numero Registro Det"})
            ListaEtiquetasReporteUIAF.Add("CODIGOSUCURSAL", New ProductoEtiquetas With {.Titulo = "Código sucursal", .Tooltip = "Código sucursal", .TooltipOriginal = "Código sucursal"})
            ListaEtiquetasReporteUIAF.Add("FECHAOPERACION", New ProductoEtiquetas With {.Titulo = "Fecha de operación", .Tooltip = "Fecha de operación", .TooltipOriginal = "Fecha de operación"})
            ListaEtiquetasReporteUIAF.Add("TIPOOPERACION", New ProductoEtiquetas With {.Titulo = "Tipo operación", .Tooltip = "Tipo operación", .TooltipOriginal = "Tipo operación"})
            ListaEtiquetasReporteUIAF.Add("DEVOLUCION", New ProductoEtiquetas With {.Titulo = "Devolución", .Tooltip = "Devolución", .TooltipOriginal = "Devolución"})
            ListaEtiquetasReporteUIAF.Add("VALOROPERACION", New ProductoEtiquetas With {.Titulo = "Valor operación", .Tooltip = "Valor operación", .TooltipOriginal = "Valor operación"})
            ListaEtiquetasReporteUIAF.Add("TIPODIVISA", New ProductoEtiquetas With {.Titulo = "Tipo divisa", .Tooltip = "Tipo divisa", .TooltipOriginal = "Tipo divisa"})
            ListaEtiquetasReporteUIAF.Add("FORMAPAGO", New ProductoEtiquetas With {.Titulo = "Forma de pago", .Tooltip = "Forma de pago", .TooltipOriginal = "Forma de pago"})
            ListaEtiquetasReporteUIAF.Add("TIPOPRODUCTO", New ProductoEtiquetas With {.Titulo = "Tipo de producto", .Tooltip = "Tipo de producto", .TooltipOriginal = "Tipo de producto"})
            ListaEtiquetasReporteUIAF.Add("NUMEROPRODUCTO", New ProductoEtiquetas With {.Titulo = "Número producto", .Tooltip = "Número producto", .TooltipOriginal = "Número producto"})
            ListaEtiquetasReporteUIAF.Add("NUMERODOCUMENTOIDENTIFICACION", New ProductoEtiquetas With {.Titulo = "Numero documento identificación", .Tooltip = "Numero documento identificación", .TooltipOriginal = "Numero documento identificación"})
            ListaEtiquetasReporteUIAF.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasReporteUIAF.Add("DIRECCION", New ProductoEtiquetas With {.Titulo = "Dirección", .Tooltip = "Dirección", .TooltipOriginal = "Dirección"})
            ListaEtiquetasReporteUIAF.Add("DEPARTAMENTOMUNICIPIO", New ProductoEtiquetas With {.Titulo = "Municipio", .Tooltip = "Municipio", .TooltipOriginal = "Municipio"})
            ListaEtiquetasReporteUIAF.Add("TELEFONO", New ProductoEtiquetas With {.Titulo = "Teléfono", .Tooltip = "Teléfono", .TooltipOriginal = "Teléfono"})
            ListaEtiquetasReporteUIAF.Add("TIPOIDENTIFICACIONDESC", New ProductoEtiquetas With {.Titulo = "Tipo identificacion Desc", .Tooltip = "Tipo identificacion Desc", .TooltipOriginal = "Tipo identificacion Desc"})
            ListaEtiquetasReporteUIAF.Add("NUMERODOCUMENTOIDENTIFICACION1", New ProductoEtiquetas With {.Titulo = "Número documento identificación1", .Tooltip = "Número documento identificación1", .TooltipOriginal = "Número documento identificación1"})
            ListaEtiquetasReporteUIAF.Add("DIGITOVERIFICACIONDESC", New ProductoEtiquetas With {.Titulo = "Dígito verificación Desc", .Tooltip = "Dígito verificación Desc", .TooltipOriginal = "Dígito verificación Desc"})
            ListaEtiquetasReporteUIAF.Add("NOMBREREMITENTEBENEFICIARIO", New ProductoEtiquetas With {.Titulo = "Nombre remitente beneficiario", .Tooltip = "Nombre remitente beneficiario", .TooltipOriginal = "Nombre remitente beneficiario"})
            ListaEtiquetasReporteUIAF.Add("ENTIDADTRAMITAGIRO", New ProductoEtiquetas With {.Titulo = "Entidad tramita giro", .Tooltip = "Entidad tramita giro", .TooltipOriginal = "Entidad tramita giro"})
            ListaEtiquetasReporteUIAF.Add("PAISCIUDADENTIDADTRAMITAGIRO", New ProductoEtiquetas With {.Titulo = "País ciudad entidad tramita giro", .Tooltip = "País ciudad entidad tramita giro", .TooltipOriginal = "País ciudad entidad tramita giro"})
            ListaEtiquetasReporteUIAF.Add("CIUDAD", New ProductoEtiquetas With {.Titulo = "Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetasReporteUIAF.Add("INTERMEDARIO", New ProductoEtiquetas With {.Titulo = "Intermedario", .Tooltip = "Intermedario", .TooltipOriginal = "Intermedario"})
            ListaEtiquetasReporteUIAF.Add("PAISINTERMEDARIO", New ProductoEtiquetas With {.Titulo = "País intermedario", .Tooltip = "País intermedario", .TooltipOriginal = "País intermedario"})
            ListaEtiquetasReporteUIAF.Add("CIUDADINTERMEDARIO", New ProductoEtiquetas With {.Titulo = "Ciudad intermedario", .Tooltip = "Ciudad intermedario", .TooltipOriginal = "Ciudad intermedario"})
            ListaEtiquetasReporteUIAF.Add("ENTIDAD", New ProductoEtiquetas With {.Titulo = "Entidad", .Tooltip = "Entidad", .TooltipOriginal = "Entidad"})
            ListaEtiquetasReporteUIAF.Add("PAISENTIDAD", New ProductoEtiquetas With {.Titulo = "País entidad", .Tooltip = "País entidad", .TooltipOriginal = "País entidad"})
            ListaEtiquetasReporteUIAF.Add("CIUDADENTIDAD", New ProductoEtiquetas With {.Titulo = "Ciudad entidad", .Tooltip = "Ciudad entidad", .TooltipOriginal = "Ciudad entidad"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.ReporteMensualUIAFView", ListaEtiquetasReporteUIAF)



            'SV20181025_SETFX: 
            'Etiquetas para la pantalla de importación de operaciones

            Dim ListaEtiquetasImportacionOperaciones As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasImportacionOperaciones.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Importación operaciones SETFX", .Tooltip = "Importación operaciones SETFX", .TooltipOriginal = "Importación operaciones SETFX"})
            ListaEtiquetasImportacionOperaciones.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Archivo de operaciones masivas. Haga clic en el botón importar para leer el archivo de la ruta configurada", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasImportacionOperaciones.Add("IMPORTAR", New ProductoEtiquetas With {.Titulo = "Importar", .Tooltip = "Importar", .TooltipOriginal = "Importar"})
            ListaEtiquetasImportacionOperaciones.Add("INFORMATIVO", New ProductoEtiquetas With {.Titulo = "Ver mensajes informativos", .Tooltip = "Ver mensajes informativos", .TooltipOriginal = "Ver mensajes informativos"})
            ListaEtiquetasImportacionOperaciones.Add("INCONSISTENCIA", New ProductoEtiquetas With {.Titulo = "Ver inconsistencias", .Tooltip = "Ver inconsistencias", .TooltipOriginal = "Ver inconsistencias"})
            'ListaEtiquetasImportacionOperaciones.Add("ENVIAR", New ProductoEtiquetas With {.Titulo = "Enviar", .Tooltip = "Envia los datos para ser generados en un archivo plano", .TooltipOriginal = "Envia los datos para ser generados en un archivo plano"})
            'ListaEtiquetasImportacionOperaciones.Add("RUTA", New ProductoEtiquetas With {.Titulo = "Ruta", .Tooltip = "Muestra la ruta donde esta configurado la generación del archivo", .TooltipOriginal = "Muestra la ruta donde esta configurado la generación del archivo"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.ImportacionOperacionesSETFXView", ListaEtiquetasImportacionOperaciones)


            'SV20181031_CUMPLIMIENTOOPERACIONES
            'Pantalla de cumplimiento de operaciones

            Dim ListaEtiquetasCumplimientoOperaciones As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasCumplimientoOperaciones.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Cumplimiento de operaciones pendientes", .Tooltip = "Cumplimiento de operaciones", .TooltipOriginal = "Cumplimiento de operaciones"})
            ListaEtiquetasCumplimientoOperaciones.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Debe digitar el folio en cada operación y a continuación hacer clic en el botón Cumplir", .Tooltip = "Descripción", .TooltipOriginal = "Descripción"})
            ListaEtiquetasCumplimientoOperaciones.Add("CUMPLIR", New ProductoEtiquetas With {.Titulo = "Cumplir", .Tooltip = "Cumplir", .TooltipOriginal = "Cumplir"})
            ListaEtiquetasCumplimientoOperaciones.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasCumplimientoOperaciones.Add("PRODUCTO", New ProductoEtiquetas With {.Titulo = "Producto", .Tooltip = "Producto", .TooltipOriginal = "Producto"})
            ListaEtiquetasCumplimientoOperaciones.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Consecutivo", .Tooltip = "Consecutivo", .TooltipOriginal = "Consecutivo"})
            ListaEtiquetasCumplimientoOperaciones.Add("COMITENTE", New ProductoEtiquetas With {.Titulo = "Comitente", .Tooltip = "Comitente", .TooltipOriginal = "Comitente"})
            ListaEtiquetasCumplimientoOperaciones.Add("TIPOORDEN", New ProductoEtiquetas With {.Titulo = "Tipo de orden", .Tooltip = "Tipo de orden", .TooltipOriginal = "Tipo de orden"})
            ListaEtiquetasCumplimientoOperaciones.Add("FECHAORDEN", New ProductoEtiquetas With {.Titulo = "Fecha orden", .Tooltip = "Fecha de la orden", .TooltipOriginal = "Fecha de la orden"})
            ListaEtiquetasCumplimientoOperaciones.Add("FECHAVIGENCIA", New ProductoEtiquetas With {.Titulo = "Fecha vigencia", .Tooltip = "Fecha vigencia", .TooltipOriginal = "Fecha vigencia"})
            ListaEtiquetasCumplimientoOperaciones.Add("CLASIFICACION", New ProductoEtiquetas With {.Titulo = "Clasifiación", .Tooltip = "Clasifiación", .TooltipOriginal = "Clasifiación"})
            ListaEtiquetasCumplimientoOperaciones.Add("FOLIO", New ProductoEtiquetas With {.Titulo = "Folio", .Tooltip = "Folio", .TooltipOriginal = "Folio"})
            ListaEtiquetasCumplimientoOperaciones.Add("CONFIRMACION", New ProductoEtiquetas With {.Titulo = "Esta seguro que desea cumplir las operaciones con folios: ", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasCumplimientoOperaciones.Add("VALIDACION", New ProductoEtiquetas With {.Titulo = "Debe digitar un folio para generar el cumplimiento", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasCumplimientoOperaciones.Add("ACTUALIZACIONEXITOSA", New ProductoEtiquetas With {.Titulo = "Se generó el cumplimiento de las operaciones exitosamente", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasCumplimientoOperaciones.Add("VALORNETO", New ProductoEtiquetas With {.Titulo = "Valor neto", .Tooltip = "Valor neto", .TooltipOriginal = "Valor neto"})
            ListaEtiquetasCumplimientoOperaciones.Add("VALIDACIONVIGENCIA", New ProductoEtiquetas With {.Titulo = "No se pueden cumplir ordenes por fuera de la fecha de vigencia", .Tooltip = "", .TooltipOriginal = ""})

            ListaRetorno.Add("A2OrdenesDivisasWPF.CumplimientoView", ListaEtiquetasCumplimientoOperaciones)

            'Formato 395
            'Ricardo Barrientos Perez
            'RABP20181109  

            Dim ListaEtiquetasFORMATO395 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasFORMATO395.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Generar reporte diario de compra y venta Formato 395", .Tooltip = "Generar reporte diario de compra y venta Formato 395", .TooltipOriginal = "Generar reporte diario de compra y venta Formato 395"})
            ListaEtiquetasFORMATO395.Add("FORMATO", New ProductoEtiquetas With {.Titulo = "Formato", .Tooltip = "Formato movimiento exportación", .TooltipOriginal = "Formato movimiento exportación"})
            ListaEtiquetasFORMATO395.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Generar reporte diario de compra y venta Formato 395", .Tooltip = "Generar reporte diario de compra y venta Formato 395", .TooltipOriginal = "Generar reporte diario de compra y venta Formato 395"})
            ListaEtiquetasFORMATO395.Add("FECHADESDE", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
            ListaEtiquetasFORMATO395.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasFORMATO395.Add("FALTANFILTROS", New ProductoEtiquetas With {.Titulo = "No se han digitado todos los campos de busqueda", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasFORMATO395.Add("ARCHIVO", New ProductoEtiquetas With {.Titulo = "Archivo", .Tooltip = "Archivo", .TooltipOriginal = "Archivo"})
            ListaEtiquetasFORMATO395.Add("DIANOHABIL", New ProductoEtiquetas With {.Titulo = "El día es no habil. Por favor verifique", .Tooltip = "", .TooltipOriginal = ""})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formato395View", ListaEtiquetasFORMATO395)

            'Formato 102
            'Ricardo Barrientos Perez
            'RABP20181113 

            Dim ListaEtiquetasFORMATO102 As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasFORMATO102.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Generar reporte diario de compra y venta Formato 102", .Tooltip = "Generar reporte diario de compra y venta Formato 102", .TooltipOriginal = "Generar reporte diario de compra y venta Formato 102"})
            ListaEtiquetasFORMATO102.Add("FORMATO", New ProductoEtiquetas With {.Titulo = "Formato", .Tooltip = "Formato movimiento exportación", .TooltipOriginal = "Formato movimiento exportación"})
            ListaEtiquetasFORMATO102.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Generar reporte diario de compra y venta Formato 102", .Tooltip = "Generar reporte diario de compra y venta Formato 102", .TooltipOriginal = "Generar reporte diario de compra y venta Formato 102"})
            ListaEtiquetasFORMATO102.Add("FECHADESDE", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
            ListaEtiquetasFORMATO102.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasFORMATO102.Add("FALTANFILTROS", New ProductoEtiquetas With {.Titulo = "No se han digitado todos los campos de busqueda", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasFORMATO102.Add("ARCHIVO", New ProductoEtiquetas With {.Titulo = "Archivo", .Tooltip = "Archivo", .TooltipOriginal = "Archivo"})
            ListaEtiquetasFORMATO102.Add("DIANOHABIL", New ProductoEtiquetas With {.Titulo = "El día es no habil. Por favor verifique", .Tooltip = "", .TooltipOriginal = ""})

            ListaRetorno.Add("A2FormulariosDivisasWPF.Formato102View", ListaEtiquetasFORMATO102)


            'Formato Consolidado 388,389,390 393
            'Ricardo Barrientos Perez
            'RABP20191002 

            Dim ListaEtiquetasMovimientoConsolidadoDIAN As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasMovimientoConsolidadoDIAN.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Exportación Movimiento DIAN Divisas Consolidado", .Tooltip = "Exportación Movimiento DIAN Divisas Consolidado", .TooltipOriginal = "Exportación Movimiento DIAN Divisas Consolidado5"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Exportación Movimiento DIAN Divisas Consolidado", .Tooltip = "Exportación Movimiento DIAN Divisas Consolidado", .TooltipOriginal = "Exportación Movimiento DIAN Divisas Consolidado"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("ANO", New ProductoEtiquetas With {.Titulo = "Año", .Tooltip = "Año", .TooltipOriginal = "Año para la generación de los movimiento consolidado"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("MESTRANSMITIR", New ProductoEtiquetas With {.Titulo = "Mes a transmitir", .Tooltip = "Mes a transmitir", .TooltipOriginal = "Mes a transmitir"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("METODO", New ProductoEtiquetas With {.Titulo = "Método", .Tooltip = "Método", .TooltipOriginal = "Método"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("EXCEL", New ProductoEtiquetas With {.Titulo = "Exportar Excel", .Tooltip = "Exportar Excel", .TooltipOriginal = "Exportar Excel"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("FALTANFILTROS", New ProductoEtiquetas With {.Titulo = "No se han digitado todos los campos de busqueda", .Tooltip = "Consultar", .TooltipOriginal = "Consultar"})
            ListaEtiquetasMovimientoConsolidadoDIAN.Add("ARCHIVO", New ProductoEtiquetas With {.Titulo = "Archivo", .Tooltip = "Archivo", .TooltipOriginal = "Archivo"})

            ListaRetorno.Add("A2FormulariosDivisasWPF.ExportacionMovDianConsolidadoView", ListaEtiquetasMovimientoConsolidadoDIAN)



            'ETIQUETAS Datos Giros

            Dim ListaEtiquetasDatosGiros As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasDatosGiros.Add("TITULODATOSGIROS", New ProductoEtiquetas With {.Titulo = "Datos giros", .Tooltip = "Datos giros", .TooltipOriginal = "Datos giros"})
            ListaEtiquetasDatosGiros.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Número del registro", .TooltipOriginal = "Número del registro"})
            ListaEtiquetasDatosGiros.Add("CODIGOAGENTE", New ProductoEtiquetas With {.Titulo = "Código agente", .Tooltip = "Código agente", .TooltipOriginal = "Código agente"})
            ListaEtiquetasDatosGiros.Add("CODIGOPAIS", New ProductoEtiquetas With {.Titulo = "Código país", .Tooltip = "Código país", .TooltipOriginal = "Código país"})
            ListaEtiquetasDatosGiros.Add("CODIGOMONEDA", New ProductoEtiquetas With {.Titulo = "Código moneda", .Tooltip = "Código moneda", .TooltipOriginal = "Código moneda"})
            ListaEtiquetasDatosGiros.Add("MONTOMONEDAORIGINAL", New ProductoEtiquetas With {.Titulo = "Monto moneda original", .Tooltip = "Monto moneda original", .TooltipOriginal = "Monto moneda original"})
            ListaEtiquetasDatosGiros.Add("NOMBREBENEFICIARIO", New ProductoEtiquetas With {.Titulo = "Nombre beneficiario", .Tooltip = "Nombre beneficiario", .TooltipOriginal = "Nombre beneficiario"})
            ListaEtiquetasDatosGiros.Add("NUMERODOCUMENTOBENEFICIARIO", New ProductoEtiquetas With {.Titulo = "Número documento beneficiario", .Tooltip = "Número documento beneficiario", .TooltipOriginal = "Número documento beneficiario"})
            ListaEtiquetasDatosGiros.Add("TIPOIDENTIFICACIONBENEFICIARIO", New ProductoEtiquetas With {.Titulo = "Tipo identificación beneficiario", .Tooltip = "Tipo identificación beneficiario", .TooltipOriginal = "Tipo identificación beneficiario"})
            ListaEtiquetasDatosGiros.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha actualización", .Tooltip = "Fecha actualización", .TooltipOriginal = "Fecha actualización"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.DatosGirosView", ListaEtiquetasDatosGiros)



            'RABP20181203_APROBACIONPREORDENES
            'Pantalla de aprobación de las ordenes

            Dim ListaEtiquetasAprobacionOrdenes As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasAprobacionOrdenes.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Aprobación de ordenes", .Tooltip = "Aprobación de ordenes", .TooltipOriginal = "Aprobación de ordenes"})
            ListaEtiquetasAprobacionOrdenes.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Debe seleccionar cada orden para su respectiva aprobación", .Tooltip = "Debe seleccionar cada orden para su respectiva aprobación", .TooltipOriginal = "Debe seleccionar cada orden para su respectiva aprobación"})
            ListaEtiquetasAprobacionOrdenes.Add("APROBAR", New ProductoEtiquetas With {.Titulo = "Aprobar", .Tooltip = "Aprobar", .TooltipOriginal = "Aprobar"})
            ListaEtiquetasAprobacionOrdenes.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "Id del registro", .TooltipOriginal = "Id del registro"})
            ListaEtiquetasAprobacionOrdenes.Add("PRODUCTO", New ProductoEtiquetas With {.Titulo = "Producto", .Tooltip = "Producto", .TooltipOriginal = "Producto"})
            ListaEtiquetasAprobacionOrdenes.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Consecutivo", .Tooltip = "Consecutivo", .TooltipOriginal = "Consecutivo"})
            ListaEtiquetasAprobacionOrdenes.Add("COMITENTE", New ProductoEtiquetas With {.Titulo = "Comitente", .Tooltip = "Comitente", .TooltipOriginal = "Comitente"})
            ListaEtiquetasAprobacionOrdenes.Add("TIPOORDEN", New ProductoEtiquetas With {.Titulo = "Tipo de orden", .Tooltip = "Tipo de orden", .TooltipOriginal = "Tipo de orden"})
            ListaEtiquetasAprobacionOrdenes.Add("FECHAORDEN", New ProductoEtiquetas With {.Titulo = "Fecha orden", .Tooltip = "Fecha de la orden", .TooltipOriginal = "Fecha de la orden"})
            ListaEtiquetasAprobacionOrdenes.Add("FECHAVIGENCIA", New ProductoEtiquetas With {.Titulo = "Fecha de vencimiento", .Tooltip = "Fecha de vencimiento", .TooltipOriginal = "Fecha de vencimiento"})
            ListaEtiquetasAprobacionOrdenes.Add("CLASIFICACION", New ProductoEtiquetas With {.Titulo = "Clasificación", .Tooltip = "Clasificación", .TooltipOriginal = "Clasificación"})
            ListaEtiquetasAprobacionOrdenes.Add("APROBACION", New ProductoEtiquetas With {.Titulo = "Aprobación", .Tooltip = "Aprobación", .TooltipOriginal = "Aprobación"})
            ListaEtiquetasAprobacionOrdenes.Add("CONFIRMACION", New ProductoEtiquetas With {.Titulo = "Esta seguro que desea aprobar las siguientes ordenes: ", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasAprobacionOrdenes.Add("VALIDACION", New ProductoEtiquetas With {.Titulo = "Debe seleccionar una orden para su aprobación", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasAprobacionOrdenes.Add("ACTUALIZACIONEXITOSA", New ProductoEtiquetas With {.Titulo = "Se aprobaron las ordenes exitosamente", .Tooltip = "", .TooltipOriginal = ""})
            ListaEtiquetasAprobacionOrdenes.Add("VALORNETO", New ProductoEtiquetas With {.Titulo = "Valor neto", .Tooltip = "Valor neto", .TooltipOriginal = "Valor neto"})
            ListaEtiquetasAprobacionOrdenes.Add("VALIDACIONVIGENCIA", New ProductoEtiquetas With {.Titulo = "No se pueden modificar ordenes por fuera de la fecha de vigencia", .Tooltip = "", .TooltipOriginal = ""})
            'RABP20200407: Etiquetas para Devolución de preorden 
            ListaEtiquetasAprobacionOrdenes.Add("CANTIDADDES", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasAprobacionOrdenes.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            ListaEtiquetasAprobacionOrdenes.Add("OBSERVACION", New ProductoEtiquetas With {.Titulo = "Observación", .Tooltip = "Observación", .TooltipOriginal = "Observación"})
            ListaEtiquetasAprobacionOrdenes.Add("DESAPROBACION", New ProductoEtiquetas With {.Titulo = "Devolver", .Tooltip = "Devolver", .TooltipOriginal = "Devolver"})
            ListaEtiquetasAprobacionOrdenes.Add("DESABROBAR", New ProductoEtiquetas With {.Titulo = "Devolver", .Tooltip = "Devolver", .TooltipOriginal = "Devolver"})
            ListaEtiquetasAprobacionOrdenes.Add("GRABARDESABROBACION", New ProductoEtiquetas With {.Titulo = "Grabar", .Tooltip = "Grabar", .TooltipOriginal = "Grabar"})
            ListaEtiquetasAprobacionOrdenes.Add("CANCELARDESABROBACION", New ProductoEtiquetas With {.Titulo = "Cancelar", .Tooltip = "Cancelar", .TooltipOriginal = "Cancelar"})
            ListaEtiquetasAprobacionOrdenes.Add("CONFIRMACIONDESAPROBACION", New ProductoEtiquetas With {.Titulo = "Esta seguro que desea devolver la orden:", .Tooltip = "Esta seguro que desea devolver la orden:", .TooltipOriginal = "Esta seguro que desea devolver la orden:"})
            ListaEtiquetasAprobacionOrdenes.Add("EMAILDESAPROBACION", New ProductoEtiquetas With {.Titulo = "Devolver orden divisas", .Tooltip = "Devolver orden divisas", .TooltipOriginal = "Devolver orden divisas"})
            ListaEtiquetasAprobacionOrdenes.Add("RECEPTORSINEMAIL", New ProductoEtiquetas With {.Titulo = "El receptor de la orden no tiene email configurado", .Tooltip = "El receptor de la orden no tiene email configurado", .TooltipOriginal = "El receptor de la orden no tiene email configurado"})
            ListaEtiquetasAprobacionOrdenes.Add("ACTUALIZACIONEXITOSADESABROBACION", New ProductoEtiquetas With {.Titulo = "Devuelvo y notifico estado de la orden exitosamente", .Tooltip = "Devuelvo y notifico estado de la orden exitosamente", .TooltipOriginal = "Devuelvo y notifico estado de la orden exitosamente"})


            ListaRetorno.Add("A2OrdenesDivisasWPF.AprobacionView", ListaEtiquetasAprobacionOrdenes)

            Dim ListaEtiquetasParametrizacionTributaria As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasParametrizacionTributaria.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtra por el ID, Nombre, Modulo y Funcionalidad", .Tooltip = "Filtra por el ID, Nombre, Modulo y Funcionalidad", .TooltipOriginal = "Filtra por el ID, Nombre, Modulo y Funcionalidad"})
            ListaEtiquetasParametrizacionTributaria.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Parametrización tributaria", .Tooltip = "Parametrización tributaria", .TooltipOriginal = "Parametrización tributaria"})
            ListaEtiquetasParametrizacionTributaria.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasParametrizacionTributaria.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .TituloAlterno = "ID*", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
            ListaEtiquetasParametrizacionTributaria.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre impuesto", .TituloAlterno = "Nombre impuesto*", .Tooltip = "Nombre impuesto", .TooltipOriginal = "Nombre impuesto"})
            ListaEtiquetasParametrizacionTributaria.Add("TASAIMPOSITIVA", New ProductoEtiquetas With {.Titulo = "Tasa impositiva", .TituloAlterno = "Tasa impositiva", .Tooltip = "Tasa impositiva", .TooltipOriginal = "Tasa impositiva"})
            ListaEtiquetasParametrizacionTributaria.Add("MODULO", New ProductoEtiquetas With {.Titulo = "Modulo", .TituloAlterno = "Modulo*", .Tooltip = "Modulo", .TooltipOriginal = "Modulo"})
            ListaEtiquetasParametrizacionTributaria.Add("FUNCIONALIDAD", New ProductoEtiquetas With {.Titulo = "Funcionalidad", .TituloAlterno = "Funcionalidad*", .Tooltip = "Funcionalidad", .TooltipOriginal = "Funcionalidad"})
            ListaEtiquetasParametrizacionTributaria.Add("PAIS", New ProductoEtiquetas With {.Titulo = "País", .TituloAlterno = "País", .Tooltip = "País", .TooltipOriginal = "País"})
            ListaEtiquetasParametrizacionTributaria.Add("CIUDAD", New ProductoEtiquetas With {.Titulo = "Ciudad", .TituloAlterno = "Ciudad", .Tooltip = "Ciudad", .TooltipOriginal = "Ciudad"})
            ListaEtiquetasParametrizacionTributaria.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Activo", .TituloAlterno = "Activo", .Tooltip = "Estado", .TooltipOriginal = "Activo"})

            ListaRetorno.Add("A2PLATMaestros.ParametrizacionTributariaView", ListaEtiquetasParametrizacionTributaria)

            Dim ListaEtiquetasPais As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasPais.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtra por el ID y Nombre", .Tooltip = "Filtra por el ID y Nombre", .TooltipOriginal = "Filtra por el ID y Nombre"})
            ListaEtiquetasPais.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Paises", .Tooltip = "Paises", .TooltipOriginal = "Paises"})
            ListaEtiquetasPais.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasPais.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .TituloAlterno = "ID*", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
            ListaEtiquetasPais.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre*", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasPais.Add("CODIGOISOALFA2", New ProductoEtiquetas With {.Titulo = "Código ISO alfa 2", .TituloAlterno = "Código ISO alfa 2", .Tooltip = "Código ISO alfa 2", .TooltipOriginal = "Código ISO alfa 2"})
            ListaEtiquetasPais.Add("CODIGOISOALFA3", New ProductoEtiquetas With {.Titulo = "Código ISO alfa 3", .TituloAlterno = "Código ISO alfa 3", .Tooltip = "Código ISO alfa 3", .TooltipOriginal = "Código ISO alfa 3"})
            ListaEtiquetasPais.Add("CODIGOISONUMERICO", New ProductoEtiquetas With {.Titulo = "Código ISO númerico", .TituloAlterno = "Código ISO númerico", .Tooltip = "Código ISO númerico", .TooltipOriginal = "Código ISO númerico"})
            ListaEtiquetasPais.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .TituloAlterno = "Estado", .Tooltip = "Estado", .TooltipOriginal = "Estado"})
            ListaEtiquetasPais.Add("ESTADOS", New ProductoEtiquetas With {.Titulo = "Estados", .TituloAlterno = "Estados", .Tooltip = "Estados", .TooltipOriginal = "Estados"})
            ListaEtiquetasPais.Add("ESTADOS_NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasPais.Add("ESTADOS_CODIGO", New ProductoEtiquetas With {.Titulo = "Código estado", .TituloAlterno = "Código estado", .Tooltip = "Código estado", .TooltipOriginal = "Código estado"})
            ListaEtiquetasPais.Add("MONEDAS", New ProductoEtiquetas With {.Titulo = "Monedas", .TituloAlterno = "Monedas", .Tooltip = "Monedas", .TooltipOriginal = "Monedas"})
            ListaEtiquetasPais.Add("MONEDAS_NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasPais.Add("MONEDAS_CODIGO", New ProductoEtiquetas With {.Titulo = "Código estado", .TituloAlterno = "Código estado", .Tooltip = "Código estado", .TooltipOriginal = "Código estado"})

            ListaRetorno.Add("A2PLATMaestros.PaisView", ListaEtiquetasPais)

            Dim ListaEtiquetasPaisEstados As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasPaisEstados.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Estados País", .Tooltip = "Estados País", .TooltipOriginal = "Estados País"})
            ListaEtiquetasPaisEstados.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .TituloAlterno = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
            ListaEtiquetasPaisEstados.Add("CODIGO", New ProductoEtiquetas With {.Titulo = "Código", .TituloAlterno = "Código", .Tooltip = "Código", .TooltipOriginal = "Código"})
            ListaEtiquetasPaisEstados.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})

            ListaRetorno.Add("A2PLATMaestros.Pais_EstadoView", ListaEtiquetasPaisEstados)

            Dim ListaEtiquetasPaisMonedas As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasPaisMonedas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Monedas País", .Tooltip = "Monedas País", .TooltipOriginal = "Monedas País"})
            ListaEtiquetasPaisMonedas.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .TituloAlterno = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
            ListaEtiquetasPaisMonedas.Add("CODIGO", New ProductoEtiquetas With {.Titulo = "Código", .TituloAlterno = "Código", .Tooltip = "Código", .TooltipOriginal = "Código"})
            ListaEtiquetasPaisMonedas.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})

            ListaRetorno.Add("A2PLATMaestros.Pais_MonedaView", ListaEtiquetasPaisMonedas)

            Dim ListaEtiquetasPreordenes As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasPreordenes.Add("EDICIONREGISTRO", New ProductoEtiquetas With {.Titulo = "Edición registro", .Tooltip = "Edición registro", .TooltipOriginal = "Edición registro"})
            ListaEtiquetasPreordenes.Add("PORTAFOLIOSELECCIONADO", New ProductoEtiquetas With {.Titulo = "Portafolio almacenado", .Tooltip = "Portafolio almacenado", .TooltipOriginal = "Portafolio almacenado"})
            ListaEtiquetasPreordenes.Add("BUSQUEDA", New ProductoEtiquetas With {.Titulo = "Busqueda avanzada", .Tooltip = "Busqueda avanzada", .TooltipOriginal = "Busqueda avanzada"})
            ListaEtiquetasPreordenes.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar", .Tooltip = "Filtra por el ID, Datos de Cuenta, Tipo, Tipo inversión e Intención", .TooltipOriginal = "Filtra por el ID, Datos de Cuenta, Tipo, Tipo inversión e Intención"})
            ListaEtiquetasPreordenes.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Consultar", .Tooltip = "Consulta por los criterios de busqueda", .TooltipOriginal = "Consulta por los criterios de busqueda"})
            ListaEtiquetasPreordenes.Add("NUEVO", New ProductoEtiquetas With {.Titulo = "Nuevo", .Tooltip = "Crear un nuevo registro", .TooltipOriginal = "Crear un nuevo registro"})
            ListaEtiquetasPreordenes.Add("EDITAR", New ProductoEtiquetas With {.Titulo = "Editar", .Tooltip = "Editar el registro", .TooltipOriginal = "Editar el registro"})
            ListaEtiquetasPreordenes.Add("BORRAR", New ProductoEtiquetas With {.Titulo = "Borrar", .Tooltip = "Borrar el registro", .TooltipOriginal = "Borrar el registro"})
            ListaEtiquetasPreordenes.Add("GUARDAR", New ProductoEtiquetas With {.Titulo = "Guardar", .Tooltip = "Guardar el registro", .TooltipOriginal = "Guardar el registro"})
            ListaEtiquetasPreordenes.Add("CANCELAR", New ProductoEtiquetas With {.Titulo = "Cancelar", .Tooltip = "Cancelar edición registro", .TooltipOriginal = "Cancelar edición registro"})
            ListaEtiquetasPreordenes.Add("LIMPIAR", New ProductoEtiquetas With {.Titulo = "Limpiar", .Tooltip = "Limpia los campos de busqueda", .TooltipOriginal = "Limpia los campos de busqueda"})
            ListaEtiquetasPreordenes.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Preordenes", .Tooltip = "Preordenes", .TooltipOriginal = "Preordenes"})
            ListaEtiquetasPreordenes.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
            ListaEtiquetasPreordenes.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .TituloAlterno = "ID*", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
            ListaEtiquetasPreordenes.Add("FECHAINVERSION", New ProductoEtiquetas With {.Titulo = "Fecha inversión", .TituloAlterno = "Fecha inversión", .Tooltip = "Fecha inversión", .TooltipOriginal = "Fecha inversión"})
            ListaEtiquetasPreordenes.Add("FECHAVIGENCIA", New ProductoEtiquetas With {.Titulo = "Fecha vigencia", .TituloAlterno = "Fecha vigencia", .Tooltip = "Fecha vigencia", .TooltipOriginal = "Fecha vigencia"})
            ListaEtiquetasPreordenes.Add("BUSCADORINSTRUMENTO", New ProductoEtiquetas With {.Titulo = "Instrumento", .TituloAlterno = "Instrumento", .Tooltip = "Instrumento", .TooltipOriginal = "Instrumento"})
            ListaEtiquetasPreordenes.Add("BUSCADORENTIDAD", New ProductoEtiquetas With {.Titulo = "Entidad", .TituloAlterno = "Entidad", .Tooltip = "Entidad", .TooltipOriginal = "Entidad"})
            ListaEtiquetasPreordenes.Add("BUSCADORCUENTA", New ProductoEtiquetas With {.Titulo = "Cuenta", .TituloAlterno = "Cuenta", .Tooltip = "Cuenta", .TooltipOriginal = "Cuenta"})
            ListaEtiquetasPreordenes.Add("CUENTA", New ProductoEtiquetas With {.Titulo = "Código", .TituloAlterno = "Cuenta*", .Tooltip = "Código", .TooltipOriginal = "Código"})
            ListaEtiquetasPreordenes.Add("CUENTANRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .TituloAlterno = "Nro identificación", .Tooltip = "Nro identificación", .TooltipOriginal = "Nro identificación"})
            ListaEtiquetasPreordenes.Add("CUENTANOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasPreordenes.Add("TIPOPREORDEN", New ProductoEtiquetas With {.Titulo = "Tipo", .TituloAlterno = "Tipo*", .Tooltip = "Tipo", .TooltipOriginal = "Tipo"})
            ListaEtiquetasPreordenes.Add("TIPOINVERSION", New ProductoEtiquetas With {.Titulo = "Tipo inversión", .TituloAlterno = "Tipo inversión*", .Tooltip = "Tipo inversión", .TooltipOriginal = "Tipo inversión"})
            ListaEtiquetasPreordenes.Add("ENTIDAD", New ProductoEtiquetas With {.Titulo = "Entidad", .TituloAlterno = "Entidad", .Tooltip = "Entidad", .TooltipOriginal = "Entidad"})
            ListaEtiquetasPreordenes.Add("ENTIDADNRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro identificación", .TituloAlterno = "Nro identificación", .Tooltip = "Nro identificación", .TooltipOriginal = "Nro identificación"})
            ListaEtiquetasPreordenes.Add("ENTIDADNOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .TituloAlterno = "Nombre", .Tooltip = "Nombre", .TooltipOriginal = "Nombre"})
            ListaEtiquetasPreordenes.Add("INSTRUMENTO", New ProductoEtiquetas With {.Titulo = "Código", .TituloAlterno = "Instrumento", .Tooltip = "Código instrumento", .TooltipOriginal = "Código instrumento"})
            ListaEtiquetasPreordenes.Add("INSTRUMENTODESCRIPCION", New ProductoEtiquetas With {.Titulo = "Descripción", .TituloAlterno = "Descripción", .Tooltip = "Instrumento", .TooltipOriginal = "Descripción"})
            ListaEtiquetasPreordenes.Add("INTENCION", New ProductoEtiquetas With {.Titulo = "Intención", .TituloAlterno = "Intención*", .Tooltip = "Intención", .TooltipOriginal = "Intención"})
            ListaEtiquetasPreordenes.Add("VALOR", New ProductoEtiquetas With {.Titulo = "Valor", .TituloAlterno = "Valor", .Tooltip = "Valor", .TooltipOriginal = "Valor"})
            ListaEtiquetasPreordenes.Add("VALORNOMINAL", New ProductoEtiquetas With {.Titulo = "Valor nominal", .TituloAlterno = "Valor nominal", .Tooltip = "Valor nominal", .TooltipOriginal = "Valor nominal"})
            ListaEtiquetasPreordenes.Add("VALORECONOMICO", New ProductoEtiquetas With {.Titulo = "Valor economico", .TituloAlterno = "Valor economico", .Tooltip = "Valor economico", .TooltipOriginal = "Valor economico"})
            ListaEtiquetasPreordenes.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio", .TituloAlterno = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            ListaEtiquetasPreordenes.Add("RENTABILIDADMINIMA", New ProductoEtiquetas With {.Titulo = "Rentabilidad mínima", .TituloAlterno = "Rentabilidad mínima", .Tooltip = "Rentabilidad mínima", .TooltipOriginal = "Rentabilidad mínima"})
            ListaEtiquetasPreordenes.Add("RENTABILIDADMAXIMA", New ProductoEtiquetas With {.Titulo = "Rentabilidad máxima", .TituloAlterno = "Rentabilidad máxima", .Tooltip = "Rentabilidad máxima", .TooltipOriginal = "Rentabilidad máxima"})
            ListaEtiquetasPreordenes.Add("INSTRUCCIONES", New ProductoEtiquetas With {.Titulo = "Instrucciones", .TituloAlterno = "Instrucciones", .Tooltip = "Instrucciones", .TooltipOriginal = "Instrucciones"})
            ListaEtiquetasPreordenes.Add("OBSERVACIONES", New ProductoEtiquetas With {.Titulo = "Observaciones", .TituloAlterno = "Observaciones", .Tooltip = "Observaciones", .TooltipOriginal = "Observaciones"})
            ListaEtiquetasPreordenes.Add("USUARIO", New ProductoEtiquetas With {.Titulo = "Usuario", .TituloAlterno = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasPreordenes.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Actualización", .TituloAlterno = "Actualización", .Tooltip = "Actualización", .TooltipOriginal = "Actualización"})
            ListaEtiquetasPreordenes.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Creación", .TituloAlterno = "Creación", .Tooltip = "Creación", .TooltipOriginal = "Creación"})
            ListaEtiquetasPreordenes.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .TituloAlterno = "Usuario creación", .Tooltip = "Usuario creación", .TooltipOriginal = "Usuario creación"})
            ListaEtiquetasPreordenes.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Activo", .TituloAlterno = "Activo", .Tooltip = "Activo", .TooltipOriginal = "Activo"})
            ListaEtiquetasPreordenes.Add("SELECCIONADO", New ProductoEtiquetas With {.Titulo = "Seleccionado", .TituloAlterno = "Seleccionado", .Tooltip = "Seleccionado", .TooltipOriginal = "Seleccionado"})
            ListaEtiquetasPreordenes.Add("TASAREFERENCIA", New ProductoEtiquetas With {.Titulo = "Tasa referencia", .TituloAlterno = "Tasa referencia", .Tooltip = "Tasa referencia", .TooltipOriginal = "Tasa referencia"})
            ListaEtiquetasPreordenes.Add("VALORCOMPRA", New ProductoEtiquetas With {.Titulo = "Valor compra", .TituloAlterno = "Valor compra", .Tooltip = "Valor compra", .TooltipOriginal = "Valor compra"})
            ListaEtiquetasPreordenes.Add("VPNMERCADO", New ProductoEtiquetas With {.Titulo = "VPN Mercado", .TituloAlterno = "VPN Mercado", .Tooltip = "VPN Mercado", .TooltipOriginal = "VPN Mercado"})
            ListaEtiquetasPreordenes.Add("FECHACOMPRA", New ProductoEtiquetas With {.Titulo = "Fecha compra", .TituloAlterno = "Fecha compra", .Tooltip = "Fecha compra", .TooltipOriginal = "Fecha compra"})
            ListaEtiquetasPreordenes.Add("VALORPENDIENTE", New ProductoEtiquetas With {.Titulo = "Saldo", .TituloAlterno = "Saldo", .Tooltip = "Saldo", .TooltipOriginal = "Saldo"})

            ListaRetorno.Add("A2PLATPreordenes.PreordenesView", ListaEtiquetasPreordenes)

            Dim ListaEtiquetasVisorPreordenes As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)
            ListaEtiquetasVisorPreordenes.Add("CONSULTAR", New ProductoEtiquetas With {.Titulo = "Refrescar", .Tooltip = "Refrescar compras y ventas", .TooltipOriginal = "Refrescar compras y ventas"})
            ListaEtiquetasVisorPreordenes.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar", .Tooltip = "Filtra cruces", .TooltipOriginal = "Filtra cruces"})
            ListaEtiquetasVisorPreordenes.Add("REFRESCAR", New ProductoEtiquetas With {.Titulo = "Recargar registros", .Tooltip = "Recargar registros", .TooltipOriginal = "Recargar registros"})
            ListaEtiquetasVisorPreordenes.Add("CRUZAR", New ProductoEtiquetas With {.Titulo = "Cruzar", .Tooltip = "Cruzar registros seleccionados", .TooltipOriginal = "Cruzar registros seleccionados"})
            ListaEtiquetasVisorPreordenes.Add("QUITARFILTRO", New ProductoEtiquetas With {.Titulo = "Quitar filtrar", .Tooltip = "Quitar filtro", .TooltipOriginal = "Quitar filtro"})
            ListaEtiquetasVisorPreordenes.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Visor Preordenes", .Tooltip = "Visor Preordenes", .TooltipOriginal = "Visor Preordenes"})
            ListaEtiquetasVisorPreordenes.Add("TITULOCOMPRA", New ProductoEtiquetas With {.Titulo = "Compras", .Tooltip = "Compras", .TooltipOriginal = "Compras"})
            ListaEtiquetasVisorPreordenes.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .TituloAlterno = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
            ListaEtiquetasVisorPreordenes.Add("TIPOINVERSION", New ProductoEtiquetas With {.Titulo = "Tipo inversión", .TituloAlterno = "Tipo inversión", .Tooltip = "Tipo inversión", .TooltipOriginal = "Tipo inversión"})
            ListaEtiquetasVisorPreordenes.Add("INSTRUMENTO", New ProductoEtiquetas With {.Titulo = "Instrumento", .TituloAlterno = "Instrumento", .Tooltip = "Instrumento", .TooltipOriginal = "Instrumento"})
            ListaEtiquetasVisorPreordenes.Add("VALOR", New ProductoEtiquetas With {.Titulo = "Valor Inicial Inversión/Nominal Inicial", .TituloAlterno = "Valor Inicial Inversión/Nominal Inicial", .Tooltip = "Valor Inicial Inversión/Nominal Inicial", .TooltipOriginal = "Valor Inicial Inversión/Nominal Inicial"})
            ListaEtiquetasVisorPreordenes.Add("VALORPENDIENTE", New ProductoEtiquetas With {.Titulo = "Saldo Valor Inversión/Saldo Nominal", .TituloAlterno = "Saldo Valor Inversión/Saldo Nominal", .Tooltip = "Saldo Valor Inversión/Saldo Nominal", .TooltipOriginal = "Saldo Valor Inversión/Saldo Nominal"})
            ListaEtiquetasVisorPreordenes.Add("TITULOVENTA", New ProductoEtiquetas With {.Titulo = "Ventas", .Tooltip = "Ventas", .TooltipOriginal = "Ventas"})
            ListaEtiquetasVisorPreordenes.Add("TITULOCRUCES", New ProductoEtiquetas With {.Titulo = "Cruces", .Tooltip = "Cruces", .TooltipOriginal = "Cruces"})
            ListaEtiquetasVisorPreordenes.Add("FECHAINICIAL", New ProductoEtiquetas With {.Titulo = "Fecha inicial", .TituloAlterno = "Fecha inicial", .Tooltip = "Fecha inicial", .TooltipOriginal = "Fecha inicial"})
            ListaEtiquetasVisorPreordenes.Add("FECHAFINAL", New ProductoEtiquetas With {.Titulo = "Fecha final", .TituloAlterno = "Fecha final", .Tooltip = "Fecha final", .TooltipOriginal = "Fecha final"})
            ListaEtiquetasVisorPreordenes.Add("IDPREORDEN", New ProductoEtiquetas With {.Titulo = "PreOrden", .TituloAlterno = "PreOrden", .Tooltip = "PreOrden", .TooltipOriginal = "PreOrden"})
            ListaEtiquetasVisorPreordenes.Add("SOLOUSUARIO", New ProductoEtiquetas With {.Titulo = "Solo cruces propios", .TituloAlterno = "Solo cruces propios", .Tooltip = "Solo cruces propios", .TooltipOriginal = "Solo cruces propios"})
            ListaEtiquetasVisorPreordenes.Add("IDCOMPRA", New ProductoEtiquetas With {.Titulo = "ID Compra", .TituloAlterno = "ID Compra", .Tooltip = "ID Compra", .TooltipOriginal = "ID Compra"})
            ListaEtiquetasVisorPreordenes.Add("TIPOINVERSIONCOMPRA", New ProductoEtiquetas With {.Titulo = "Inversión Compra", .TituloAlterno = "Inversión Compra", .Tooltip = "Inversión Compra", .TooltipOriginal = "Inversión Compra"})
            ListaEtiquetasVisorPreordenes.Add("INSTRUMENTOCOMPRA", New ProductoEtiquetas With {.Titulo = "Instrumento Compra", .TituloAlterno = "Instrumento Compra", .Tooltip = "Instrumento Compra", .TooltipOriginal = "Instrumento Compra"})
            ListaEtiquetasVisorPreordenes.Add("IDVENTA", New ProductoEtiquetas With {.Titulo = "ID Venta", .TituloAlterno = "ID Venta", .Tooltip = "ID Venta", .TooltipOriginal = "ID Venta"})
            ListaEtiquetasVisorPreordenes.Add("TIPOINVERSIONVENTA", New ProductoEtiquetas With {.Titulo = "Inversión Venta", .TituloAlterno = "Inversión Venta", .Tooltip = "Inversión Venta", .TooltipOriginal = "Inversión Venta"})
            ListaEtiquetasVisorPreordenes.Add("INSTRUMENTOVENTA", New ProductoEtiquetas With {.Titulo = "Instrumento Venta", .TituloAlterno = "Instrumento Venta", .Tooltip = "Instrumento Venta", .TooltipOriginal = "Instrumento Venta"})
            ListaEtiquetasVisorPreordenes.Add("FECHACRUCE", New ProductoEtiquetas With {.Titulo = "Fecha cruce", .TituloAlterno = "Fecha cruce", .Tooltip = "Fecha cruce", .TooltipOriginal = "Fecha cruce"})
            ListaEtiquetasVisorPreordenes.Add("VALORCRUZADO", New ProductoEtiquetas With {.Titulo = "Valor cruzado", .TituloAlterno = "Valor cruzado", .Tooltip = "Valor cruzado", .TooltipOriginal = "Valor cruzado"})
            ListaEtiquetasVisorPreordenes.Add("USUARIO", New ProductoEtiquetas With {.Titulo = "Usuario", .TituloAlterno = "Usuario", .Tooltip = "Usuario", .TooltipOriginal = "Usuario"})
            ListaEtiquetasVisorPreordenes.Add("VALORTOTAL", New ProductoEtiquetas With {.Titulo = "Valor total", .TituloAlterno = "Valor total", .Tooltip = "Valor total", .TooltipOriginal = "Valor total"})
            ListaEtiquetasVisorPreordenes.Add("ORDENOPERACION", New ProductoEtiquetas With {.Titulo = "Orden/Operación", .TituloAlterno = "Orden/Operación", .Tooltip = "Orden/Operación", .TooltipOriginal = "Orden/Operación"})

            ListaEtiquetasVisorPreordenes.Add("SELECTORORDEN_TITULOCOMPRA", New ProductoEtiquetas With {.Titulo = "Orden compra", .TituloAlterno = "Orden compra", .Tooltip = "Orden compra", .TooltipOriginal = "Orden compra"})
            ListaEtiquetasVisorPreordenes.Add("SELECTORORDEN_TITULOVENTA", New ProductoEtiquetas With {.Titulo = "Orden venta", .TituloAlterno = "Orden venta", .Tooltip = "Orden venta", .TooltipOriginal = "Orden venta"})
            ListaEtiquetasVisorPreordenes.Add("SELECTORORDEN_ETIQUETA", New ProductoEtiquetas With {.Titulo = "Tipo Orden", .TituloAlterno = "Tipo Orden", .Tooltip = "Tipo Orden", .TooltipOriginal = "Tipo Orden"})
            ListaEtiquetasVisorPreordenes.Add("SELECTORORDEN_MENSAJE", New ProductoEtiquetas With {.Titulo = "Seleccione el formulario en el que se desea completar el registro de la orden.", .TituloAlterno = "Tipo Orden", .Tooltip = "Tipo Orden", .TooltipOriginal = "Tipo Orden"})
            ListaEtiquetasVisorPreordenes.Add("SELECTORORDEN_ACEPTAR", New ProductoEtiquetas With {.Titulo = "Aceptar", .TituloAlterno = "Aceptar", .Tooltip = "Aceptar", .TooltipOriginal = "Aceptar"})
            ListaEtiquetasVisorPreordenes.Add("SELECTORORDEN_CANCELAR", New ProductoEtiquetas With {.Titulo = "Cancelar", .TituloAlterno = "Cancelar", .Tooltip = "Cancelar", .TooltipOriginal = "Cancelar"})

            ListaRetorno.Add("A2PLATPreordenes.VisorPreordenesView", ListaEtiquetasVisorPreordenes)


            'ETIQUETAS Exportacion ordenes divisas sector real
            'Ricardo Barrientos Pérez
            'RABP20190628          

            Dim ListaEtiquetasExportacionOrdenesDivisasSectoReal As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasExportacionOrdenesDivisasSectoReal.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Exportación ordenes divisas sector real", .Tooltip = "Exportación Ordenes divisas Sector Real", .TooltipOriginal = "Exportación Ordenes divisas Sector Real"})
            ListaEtiquetasExportacionOrdenesDivisasSectoReal.Add("FECHAINICIO", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
            ListaEtiquetasExportacionOrdenesDivisasSectoReal.Add("CARGAR", New ProductoEtiquetas With {.Titulo = "Cargar", .Tooltip = "Carga Ordenes divisas sector real", .TooltipOriginal = "Carga Ordenes divisas sector real"})
            ListaEtiquetasExportacionOrdenesDivisasSectoReal.Add("EXPORTAR", New ProductoEtiquetas With {.Titulo = "Exportar", .Tooltip = "Exportar Ordenes divisas sector real", .TooltipOriginal = "Exportar Ordenes divisas sector real"})

            ListaRetorno.Add("A2OrdenesDivisasWPF.OrdenesDivisasExportacionSectorRealXMLView", ListaEtiquetasExportacionOrdenesDivisasSectoReal)

            'End RABP20190628


            'JAPC20200630_C-20200440
            'Etiquetas cierre operaciones valoracion divisas forward

            Dim ListaEtiquetasValoracion As New Dictionary(Of String, A2Utilidades.ProductoEtiquetas)

            ListaEtiquetasValoracion.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Procesar valoración", .Tooltip = "Procesar valoración", .TooltipOriginal = "Procesar valoración"})
            ListaEtiquetasValoracion.Add("PROCESAR", New ProductoEtiquetas With {.Titulo = "Procesar", .Tooltip = "Procesar", .TooltipOriginal = "Procesar"})
            ListaEtiquetasValoracion.Add("FECHACORTE", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha", .TooltipOriginal = "Fecha"})
            ListaEtiquetasValoracion.Add("INGRESEFECHACORTE", New ProductoEtiquetas With {.Titulo = "Ingrese fecha corte", .Tooltip = "Ingrese fecha corte", .TooltipOriginal = "Ingrese fecha corte"})
            ListaEtiquetasValoracion.Add("MONEDA", New ProductoEtiquetas With {.Titulo = "Moneda", .Tooltip = "Moneda", .TooltipOriginal = "Moneda"})
            ListaEtiquetasValoracion.Add("CLASIFICACIONNEGOCIO", New ProductoEtiquetas With {.Titulo = "Tipo negocio", .Tooltip = "Tipo negocio", .TooltipOriginal = "Tipo negocio"})
            ListaEtiquetasValoracion.Add("DESHACER", New ProductoEtiquetas With {.Titulo = "Deshacer cierre", .Tooltip = "Deshacer cierre", .TooltipOriginal = "Deshacer cierre"})
            ListaEtiquetasValoracion.Add("DESCRIPCION", New ProductoEtiquetas With {.Titulo = "Proceso para cerrar y deshacer valoración de operaciones", .Tooltip = "Proceso para cerrar y deshacer operaciones", .TooltipOriginal = "Proceso para cerrar y deshacer operaciones"})
            ListaEtiquetasValoracion.Add("PENDIENTES", New ProductoEtiquetas With {.Titulo = "Pendientes", .Tooltip = "Operaciones pendientes por cerrar", .TooltipOriginal = "Operaciones pendientes por cerrar"})
            ListaEtiquetasValoracion.Add("CONSECUTIVO", New ProductoEtiquetas With {.Titulo = "Concecutivo", .Tooltip = "Concecutivo", .TooltipOriginal = "Concecutivo"})
            ListaEtiquetasValoracion.Add("FECHANEGOCIACION", New ProductoEtiquetas With {.Titulo = "Fecha negociación", .Tooltip = "Fecha negociación", .TooltipOriginal = ""})
            ListaEtiquetasValoracion.Add("FECHAVENCIMIENTO", New ProductoEtiquetas With {.Titulo = "Fecha vencimiento", .Tooltip = "Fecha vencimiento", .TooltipOriginal = "Fecha vencimiento"})
            ListaEtiquetasValoracion.Add("DIAS", New ProductoEtiquetas With {.Titulo = "Dias", .Tooltip = "Dias", .TooltipOriginal = "Dias"})
            ListaEtiquetasValoracion.Add("COMITENTE", New ProductoEtiquetas With {.Titulo = "Comitente", .Tooltip = "Comitente", .TooltipOriginal = "Comitente"})
            ListaEtiquetasValoracion.Add("CLASIFICACION", New ProductoEtiquetas With {.Titulo = "Clasificación", .Tooltip = "Clasificación", .TooltipOriginal = "Clasificación"})
            ListaEtiquetasValoracion.Add("TIPO", New ProductoEtiquetas With {.Titulo = "Tipo", .Tooltip = "Tipo", .TooltipOriginal = "Tipo"})
            ListaEtiquetasValoracion.Add("TIPOCUMPLIMIENTO", New ProductoEtiquetas With {.Titulo = "Tipo cumplimiento", .Tooltip = "Tipo cumplimiento", .TooltipOriginal = "Tipo cumplimiento"})
            ListaEtiquetasValoracion.Add("FIXING", New ProductoEtiquetas With {.Titulo = "Fixing", .Tooltip = "Fixing", .TooltipOriginal = "Fixing"})
            ListaEtiquetasValoracion.Add("MONEDA1", New ProductoEtiquetas With {.Titulo = "Moneda1", .Tooltip = "Moneda1", .TooltipOriginal = "Moneda1"})
            ListaEtiquetasValoracion.Add("MONEDA2", New ProductoEtiquetas With {.Titulo = "Moneda2", .Tooltip = "Moneda2", .TooltipOriginal = "Moneda2"})
            ListaEtiquetasValoracion.Add("OBJETIVO", New ProductoEtiquetas With {.Titulo = "Objetivo", .Tooltip = "Objetivo", .TooltipOriginal = "Objetivo"})
            ListaEtiquetasValoracion.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado", .TooltipOriginal = "Estado"})
            ListaEtiquetasValoracion.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
            ListaEtiquetasValoracion.Add("PRECIO", New ProductoEtiquetas With {.Titulo = "Precio", .Tooltip = "Precio", .TooltipOriginal = "Precio"})
            ListaEtiquetasValoracion.Add("TRM", New ProductoEtiquetas With {.Titulo = "TRM", .Tooltip = "TRM", .TooltipOriginal = "TRM"})
            ListaEtiquetasValoracion.Add("SPOT", New ProductoEtiquetas With {.Titulo = "Spot", .Tooltip = "Spot", .TooltipOriginal = "Spot"})
            ListaEtiquetasValoracion.Add("DEVALUACION", New ProductoEtiquetas With {.Titulo = "Devaluación", .Tooltip = "Devaluación", .TooltipOriginal = "Devaluación"})
            ListaEtiquetasValoracion.Add("TASAFORWARD", New ProductoEtiquetas With {.Titulo = "Tasa forward", .Tooltip = "Tasa forward", .TooltipOriginal = "Tasa forward"})
            ListaEtiquetasValoracion.Add("CERRADAS", New ProductoEtiquetas With {.Titulo = "Cerradas", .Tooltip = "Cerradas", .TooltipOriginal = "Cerradas"})
            ListaEtiquetasValoracion.Add("VALORDERECHOCOP", New ProductoEtiquetas With {.Titulo = "Valor derecho COP", .Tooltip = "Valor derecho COP", .TooltipOriginal = "Valor derecho COP"})
            ListaEtiquetasValoracion.Add("VALOROBLIGACIONCOP", New ProductoEtiquetas With {.Titulo = "Valor obligacion COP", .Tooltip = "Valor obligacion COP", .TooltipOriginal = "Valor obligacion COP"})
            ListaEtiquetasValoracion.Add("VALORMERCADOCOP", New ProductoEtiquetas With {.Titulo = "Valor mercado COP", .Tooltip = "Valor mercado COP", .TooltipOriginal = "Valor mercado COP"})
            ListaEtiquetasValoracion.Add("VALORPRESENTEPYG", New ProductoEtiquetas With {.Titulo = "Valor presente PYG", .Tooltip = "Valor presente PYG", .TooltipOriginal = "Valor presente PYG"})
            ListaEtiquetasValoracion.Add("VALORDERECHOUSD", New ProductoEtiquetas With {.Titulo = "Valor derecho USD", .Tooltip = "Valor derecho USD", .TooltipOriginal = "Valor derecho USD"})
            ListaEtiquetasValoracion.Add("VALOROBLIGACIONUSD", New ProductoEtiquetas With {.Titulo = "Valor obligacion USD", .Tooltip = "Valor obligacion USD", .TooltipOriginal = "Valor obligacion USD"})
            ListaEtiquetasValoracion.Add("VALORMERCADOUSD", New ProductoEtiquetas With {.Titulo = "Valor mercado USD", .Tooltip = "Valor mercado USD", .TooltipOriginal = "Valor mercado USD"})
            ListaEtiquetasValoracion.Add("CONFIRMACIONCIERRE", New ProductoEtiquetas With {.Titulo = "Esta seguro de realizar el cierre para las operaciones pendientes con fecha corte:", .Tooltip = "Esta seguro de realizar el cierre para las operaciones pendientes con fecha corte:", .TooltipOriginal = "Esta seguro de realizar el cierre para las operaciones pendientes con fecha corte:"})
            ListaEtiquetasValoracion.Add("CONFIRMACIONDESHACER", New ProductoEtiquetas With {.Titulo = "Esta seguro de deshacer el cierre para las operaciones valoradas con fecha corte:", .Tooltip = "Esta seguro de deshacer el cierre para las operaciones valoradas con fecha corte:", .TooltipOriginal = "Esta seguro de deshacer el cierre para las operaciones valoradas con fecha corte:"})


            ListaRetorno.Add("A2OrdenesDivisasWPF.ProcesarValoracionView", ListaEtiquetasValoracion)

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
            ListaEtiquetas.Add("GENERICO_NOPERMITE_ELIMINARREGISTRO", "El registro seleccionado no se puede eliminar.")
            ListaEtiquetas.Add("GENERICO_ANULARREGISTRO", "Está opción anula el registro seleccionado. ¿Confirma la anulación de este registro?")
            ListaEtiquetas.Add("GENERICO_NOPERMITE_EDITARREGISTRO", "El registro seleccionado no se puede editar.")
            ListaEtiquetas.Add("GENERICO_NOPERMITE_ANULARREGISTRO", "El registro seleccionado no se puede anular.")
            ListaEtiquetas.Add("GENERICO_TITULOADVERTENCIAS", "Nro de inconsistencias [[INCONSISTENCIAS]].")
            ListaEtiquetas.Add("DIVISAS_FORMULARIOYAENVIADO", "Si continua esta declaración se convertirá en una modificación aunque no haya sido enviada")
            ListaEtiquetas.Add("DIVISAS_FORMULARIOMODIFICACION", "Esta declaración no se ha enviado, debe estar enviada para poder cambiar el tipo de operación a modificación")
            ListaEtiquetas.Add("DIVISAS_TITULOFORMULARIOMODIFICACION", "El formulario no se ha enviado")
            ListaEtiquetas.Add("DIVISAS_TITULOFORMULARIOYAENVIADO", "Esta formulario ya ha sido enviado")
            'JAPC20181009: se añaden mensajes para pantalla ExportacionMovDIAN            
            ListaEtiquetas.Add("DIVISAS_FORMATODIAN", "Debe seleccionar un formato para la exportación movimiento DIAN")
            ListaEtiquetas.Add("DIVISAS_EXPORTACION", "Se realizo el proceso de exportación con exito")
            'END JAPC20181009
            'JAPC20181114 se añade mensaje cuando el comitente seleccionado no tiene cuentas cuando se realiza intrucciones en la orden
            ListaEtiquetas.Add("DIVISAS_CUENTACOMITENTE", "El comitente seleccionado no tiene cuentas para realizar este tipo de instrucción")
            'END JAPC20181114 

            'JAPC20181123_VALIDACIONESORDENES : se añade mensaje para control creacion ordenes divisas  basado en fecha control y cierre anterior
            ListaEtiquetas.Add("ORDENES_VERIFICARFECHAORDEN", "No se pueden ingresar mas ordenes en este dia. porque se pasa de la hora de control")
            ListaEtiquetas.Add("ORDENES_VERIFICARFECHACIERREANT", "No existe cierre de liquidaciones, para el dia anterior a la fecha actual. Por favor verifique y haga el cierre antes de entrar la operaciones del dia actual")
            ListaEtiquetas.Add("VISORPREORDENES_CONFIRMARCRUCE", "¿Está seguro de realizar el cruce de los registros?")
            ListaEtiquetas.Add("VISORPREORDENES_VALIDACION_SINREGISTRO", "Debe de seleccionar una compra y una venta para realizar el cruce.")
            ListaEtiquetas.Add("VISORPREORDENES_VALIDACION_SINSELECCION", "Debe seleccionar al menos 1 Venta y 1 Compra.")
            ListaEtiquetas.Add("VISORPREORDENES_VALIDACION_SOLOUNO", "Solo se puede seleccionar 1 Venta con N Compras o 1 Compra y N Ventas.")
            ListaEtiquetas.Add("VISORPREORDENES_VALIDACION_SUMAVENTAS", "La suma total de las Ventas no puede ser mayor que el valor de la Compra.")
            ListaEtiquetas.Add("VISORPREORDENES_VALIDACION_SUMACOMPRAS", "La suma total de las Compras no puede ser mayor que el valor de la Venta.")
            ListaEtiquetas.Add("VISORPREORDENES_VALIDACION_ORDENNOPERMITIDA", "Ya se creó los dos registros asociados a las PreOrdenes.")
            ListaEtiquetas.Add("VISORPREORDENES_MODPREORDEN", "Se modifico el registro [PREORDEN] desde la pantalla de PreOrdenes. Se recargara la pantalla.")
            ListaEtiquetas.Add("VISORPREORDENES_CRUZADA", "Se cruzaron registros desde otra pantalla del Visor. Se recargara la pantalla.")
            ListaEtiquetas.Add("PREORDENES_NOPERMITE_EDITARREGISTRO", "El registro seleccionado no se puede editar, dado a que fue parcialmente calzado.")
            ListaEtiquetas.Add("PREORDENES_MODPREORDEN", "Se complemento el registro [PREORDEN] desde la pantalla del Visor.")

            'END JAPC20181123_VALIDACIONESORDENES 

            'RABP20191004:Validaciones para los formatos consolidados 388,389,390,393
            ListaEtiquetas.Add("DIVISAS_CONSOLIDADO_CONSULTAR", "Es necesario seleccionar el tipo de formato a exportar.")
            ListaEtiquetas.Add("DIVISAS_CONSOLIDADO_ANO", "Es necesario seleccionar el año a generar el archivo")
            ListaEtiquetas.Add("DIVISAS_CONSOLIDADO_MES", "Es necesario seleccionar el mes a generar el archivo")
            ListaEtiquetas.Add("DIVISAS_FORMATODIANCONSOLIDADO", "No se encontraron registros para exportar.")
            ListaEtiquetas.Add("DIVISAS_FORMATODIANCONSOLIDADOEXPORTAR", "No existen datos para este dia. Por favor verifique")
            ListaEtiquetas.Add("DIVISAS_GENERACIONARCHIVOCONEXITO", "El archivo se generó con éxito.")
            ListaEtiquetas.Add("DIVISAS_BOTONEXCEL", "No se muestra información, ya que el metodo seleccionado fue Excel")
            ListaEtiquetas.Add("DIVISAS_BOTONTEXTO", "No se muestra información, ya que el metodo seleccionado fue Texto")
            ListaEtiquetas.Add("DIVISAS_FORMATODIANCONSOLIDADOTEXTO", "No se encontraron registros para exportar.")

            ListaRetorno.Add("GENERICO", ListaEtiquetas)


            Return ListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ArmarMensajesPantalla", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

End Class
