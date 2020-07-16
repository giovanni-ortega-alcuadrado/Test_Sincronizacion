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

            objAplicacion.Parametros.Add("URLServicioEjemploPractico", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/EjemploPractico-OPENRIA-WEB-EjemploPracticoDomainServices.svc")

            objAplicacion.Parametros.Add("EJECUTAR_APP_SEGUN_AMBIENTE", False)
            objAplicacion.Parametros.Add("URL_SERVICIO_UPLOADS", "http://localhost:1020/EJEMPLOPRACTICO_OPENRIA/Uploads.aspx")

            objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Mensajes.ToString, ArmarMensajesPantalla())
            objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Etiquetas.ToString, ArmarEtiquetasPantalla())
            objAplicacion.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Combos.ToString, ConsultarCombosAplicacion())

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

    Private Shared Function ConsultarCombosAplicacion() As Dictionary(Of String, Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos)))
        Try
            Dim dicListaCombosRetornoCompletos As New Dictionary(Of String, Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos)))

            Dim objDiccionarioGenerico As New Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos))
            Dim objListaCombos As New ObservableCollection(Of A2Utilidades.ProductoCombos)
            objListaCombos.Add(New ProductoCombos With {.ID = 1,
                               .Descripcion = "Prueba1",
                               .Retorno = "1",
                               .Topico = "COMBOGENERICO1"})
            objListaCombos.Add(New ProductoCombos With {.ID = 2,
                               .Descripcion = "Prueba2",
                               .Retorno = "2",
                               .Topico = "COMBOGENERICO1"})

            objDiccionarioGenerico.Add("COMBOGENERICO1", objListaCombos)

            dicListaCombosRetornoCompletos.Add("GENERICO", objDiccionarioGenerico)

            Return dicListaCombosRetornoCompletos
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
			Dim objProductoEtiquetas As New A2Utilidades.ProductoEtiquetas

            'GENERICO
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_FILTRAR", New ProductoEtiquetas With {.Titulo = "Filtrar", .Tooltip = "Filtrar", .TooltipOriginal = "Filtrar"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_QUITARFILTRO", New ProductoEtiquetas With {.Titulo = "Quitar filtro", .Tooltip = "Quitar filtro", .TooltipOriginal = "Quitar filtro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_BUSQUEDAAVANZADA", New ProductoEtiquetas With {.Titulo = "Busqueda avanzada", .Tooltip = "Busqueda avanzada", .TooltipOriginal = "Busqueda avanzada"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_NUEVO", New ProductoEtiquetas With {.Titulo = "Nuevo", .Tooltip = "Nuevo registro", .TooltipOriginal = "Nuevo registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_EDITAR", New ProductoEtiquetas With {.Titulo = "Editar", .Tooltip = "Editar registro", .TooltipOriginal = "Editar registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_BORRAR", New ProductoEtiquetas With {.Titulo = "Borrar", .Tooltip = "Borrar registro", .TooltipOriginal = "Borrar registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_ANULAR", New ProductoEtiquetas With {.Titulo = "Anular", .Tooltip = "Anular registro", .TooltipOriginal = "Anular registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_CANCELAR", New ProductoEtiquetas With {.Titulo = "Cancelar", .Tooltip = "Cancelar edicion registro", .TooltipOriginal = "Cancelar edicion registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDAR", New ProductoEtiquetas With {.Titulo = "Guardar", .Tooltip = "Guardar registro", .TooltipOriginal = "Guardar registro"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDARYCERRAR", New ProductoEtiquetas With {.Titulo = "Guardar y cerrar", .Tooltip = "Guardar registro y cerrar", .TooltipOriginal = "Guardar registro y cerrar"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDARYCOPIARANTERIOR", New ProductoEtiquetas With {.Titulo = "Guardar y copiar anterior", .Tooltip = "Guardar registro y copiar anterior", .TooltipOriginal = "Guardar registro y copiar anterior"})
            ListaEtiquetasGenerico.Add("GENERICO_BOTON_GUARDARYCREARNUEVO", New ProductoEtiquetas With {.Titulo = "Guardar y crear nuevo", .Tooltip = "Guardar registro y crear nuevo", .TooltipOriginal = "Guardar registro y crear nuevo"})

            ListaRetorno.Add("GENERICO", ListaEtiquetasGenerico)

			''ETIQUETAS CLIENTES
			'ListaEtiquetas.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Clientes", .Tooltip = "Clientes", .TooltipOriginal = "Clientes"})
			'ListaEtiquetas.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			'         ListaEtiquetas.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Documento y Nombre", .Tooltip = "Filtrar por Nro Documento y Nombre", .TooltipOriginal = "Filtrar por Nro Documento y Nombre"})
			'         ListaEtiquetas.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
			'         ListaEtiquetas.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del cliente", .TooltipOriginal = "Estado del cliente"})
			'         ListaEtiquetas.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
			'         ListaEtiquetas.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro Documento", .Tooltip = "Nro Documento del cliente", .TooltipOriginal = "Nro Documento del cliente"})
			'         ListaEtiquetas.Add("TELEFONO", New ProductoEtiquetas With {.Titulo = "Teléfono", .Tooltip = "Teléfono del cliente", .TooltipOriginal = "Teléfono del cliente"})
			'         ListaEtiquetas.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del cliente", .TooltipOriginal = "Usuario creación del cliente"})
			'         ListaEtiquetas.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario ultima actualización", .Tooltip = "Usuario ultima modificación del cliente", .TooltipOriginal = "Usuario ultima modificación del cliente"})
			'         ListaEtiquetas.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación del beneficiario", .TooltipOriginal = "Fecha creación del beneficiario"})
			'         ListaEtiquetas.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del cliente", .TooltipOriginal = "Fecha ultima actualización del cliente"})
			'         ListaEtiquetas.Add("DETALLE_BENEFICIARIOS", New ProductoEtiquetas With {.Titulo = "Beneficiarios", .Tooltip = "Beneficiarios", .TooltipOriginal = "Beneficiarios"})

			'         ListaRetorno.Add("EjemploPracticoWPF.ClientesView", ListaEtiquetas)

			'         'ETIQUETAS CLIENTES BENEFICIARIOS
			'         ListaEtiquetas1.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Beneficiario", .Tooltip = "Beneficiario"})
			'         ListaEtiquetas1.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
			'         ListaEtiquetas1.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro Documento", .Tooltip = "Nro Documento del beneficiario", .TooltipOriginal = "Nro Documento del beneficiario"})
			'         ListaEtiquetas1.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del beneficiario", .TooltipOriginal = "Nombre del beneficiario"})
			'         ListaEtiquetas1.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del beneficiario", .TooltipOriginal = "Usuario creación del beneficiario"})
			'         ListaEtiquetas1.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario ultima actualización", .Tooltip = "Usuario ultima modificación del beneficiario", .TooltipOriginal = "Usuario ultima modificación del beneficiario"})
			'         ListaEtiquetas1.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación del beneficiario", .TooltipOriginal = "Fecha creación del beneficiario"})
			'         ListaEtiquetas1.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del beneficiario", .TooltipOriginal = "Fecha ultima actualización del beneficiario"})

			'         ListaRetorno.Add("EjemploPracticoWPF.Clientes_BeneficiariosView", ListaEtiquetas1)

			'         'ETIQUETAS PRODUCTOS
			'         ListaEtiquetas2.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Productos", .Tooltip = "Productos", .TooltipOriginal = "Productos"})
			'         ListaEtiquetas2.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			'         ListaEtiquetas2.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nombre", .Tooltip = "Filtrar por Nombre", .TooltipOriginal = "Filtrar por Nombre"})
			'         ListaEtiquetas2.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
			'         ListaEtiquetas2.Add("ESTADO", New ProductoEtiquetas With {.Titulo = "Estado", .Tooltip = "Estado del cliente", .TooltipOriginal = "Estado del cliente"})
			'         ListaEtiquetas2.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del producto", .TooltipOriginal = "Nombre del producto"})
			'         ListaEtiquetas2.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad del producto", .TooltipOriginal = "Cantidad del producto"})
			'         ListaEtiquetas2.Add("CANTIDADVENDIDO", New ProductoEtiquetas With {.Titulo = "Cantidad Vendido", .Tooltip = "Cantidad Vendida del producto", .TooltipOriginal = "Cantidad Vendida del producto"})
			'         ListaEtiquetas2.Add("VALOR", New ProductoEtiquetas With {.Titulo = "Valor", .Tooltip = "Valor del producto", .TooltipOriginal = "Valor del producto"})
			'         ListaEtiquetas2.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del producto", .TooltipOriginal = "Usuario creación del producto"})
			'         ListaEtiquetas2.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario ultima actualización", .Tooltip = "Usuario ultima modificación del producto", .TooltipOriginal = "Usuario ultima modificación del producto"})
			'         ListaEtiquetas2.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación del producto", .TooltipOriginal = "Fecha creación del producto"})
			'         ListaEtiquetas2.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del producto", .TooltipOriginal = "Fecha ultima actualización del producto"})

			'         ListaRetorno.Add("EjemploPracticoWPF.ProductosView", ListaEtiquetas2)

			'         'VENTAS
			'         ListaEtiquetas3.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Ventas", .Tooltip = "Ventas", .TooltipOriginal = "Ventas"})
			'         ListaEtiquetas3.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})
			'         ListaEtiquetas3.Add("FILTRO", New ProductoEtiquetas With {.Titulo = "Filtrar por Nro Documento y Nombre", .Tooltip = "Filtrar por Nro Documento y Nombre", .TooltipOriginal = "Filtrar por Nro Documento y Nombre"})
			'         ListaEtiquetas3.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
			'         ListaEtiquetas3.Add("CLIENTE", New ProductoEtiquetas With {.Titulo = "Cliente", .Tooltip = "Cliente", .TooltipOriginal = "Cliente"})
			'         ListaEtiquetas3.Add("NRODOCUMENTO", New ProductoEtiquetas With {.Titulo = "Nro documento", .Tooltip = "Nro documento cliente", .TooltipOriginal = "Nro documento cliente"})
			'         ListaEtiquetas3.Add("NOMBRE", New ProductoEtiquetas With {.Titulo = "Nombre", .Tooltip = "Nombre del cliente", .TooltipOriginal = "Nombre del cliente"})
			'         ListaEtiquetas3.Add("FECHA", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha de la venta", .TooltipOriginal = "Fecha de la venta"})
			'         ListaEtiquetas3.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del producto", .TooltipOriginal = "Usuario creación del producto"})
			'         ListaEtiquetas3.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario ultima actualización", .Tooltip = "Usuario ultima modificación del producto", .TooltipOriginal = "Usuario ultima modificación del producto"})
			'         ListaEtiquetas3.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación del producto", .TooltipOriginal = "Fecha creación del producto"})
			'         ListaEtiquetas3.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del producto", .TooltipOriginal = "Fecha ultima actualización del producto"})
			'         ListaEtiquetas3.Add("DETALLE_BENEFICIARIOS", New ProductoEtiquetas With {.Titulo = "Detalle Ventas", .Tooltip = "Detalle Ventas", .TooltipOriginal = "Detalle Ventas"})

			'         ListaRetorno.Add("EjemploPracticoWPF.VentasView", ListaEtiquetas3)

			'         'VENTAS DETALLE
			'         ListaEtiquetas4.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Detalle Ventas", .Tooltip = "Detalle Ventas", .TooltipOriginal = "Detalle Ventas"})
			'         ListaEtiquetas4.Add("ID", New ProductoEtiquetas With {.Titulo = "ID", .Tooltip = "ID del registro", .TooltipOriginal = "ID del registro"})
			'         ListaEtiquetas4.Add("PRODUCTO", New ProductoEtiquetas With {.Titulo = "Producto", .Tooltip = "Producto", .TooltipOriginal = "Producto"})
			'         ListaEtiquetas4.Add("CANTIDAD", New ProductoEtiquetas With {.Titulo = "Cantidad", .Tooltip = "Cantidad", .TooltipOriginal = "Cantidad"})
			'         ListaEtiquetas4.Add("VALORPRODUCTO", New ProductoEtiquetas With {.Titulo = "Valor producto", .Tooltip = "Valor producto", .TooltipOriginal = "Valor producto"})
			'         ListaEtiquetas4.Add("VALORTOTAL", New ProductoEtiquetas With {.Titulo = "Valor total", .Tooltip = "Valor total", .TooltipOriginal = "Valor total"})
			'         ListaEtiquetas4.Add("USUARIOCREACION", New ProductoEtiquetas With {.Titulo = "Usuario creación", .Tooltip = "Usuario creación del beneficiario", .TooltipOriginal = "Usuario creación del beneficiario"})
			'         ListaEtiquetas4.Add("USUARIOACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Usuario ultima actualización", .Tooltip = "Usuario ultima modificación del beneficiario", .TooltipOriginal = "Usuario ultima modificación del beneficiario"})
			'         ListaEtiquetas4.Add("FECHACREACION", New ProductoEtiquetas With {.Titulo = "Fecha", .Tooltip = "Fecha creación del beneficiario", .TooltipOriginal = "Fecha creación del beneficiario"})
			'         ListaEtiquetas4.Add("FECHAACTUALIZACION", New ProductoEtiquetas With {.Titulo = "Fecha ultima actualización", .Tooltip = "Fecha ultima actualización del beneficiario", .TooltipOriginal = "Fecha ultima actualización del beneficiario"})

			'ListaRetorno.Add("EjemploPracticoWPF.Ventas_DetalleView", ListaEtiquetas4)


			'ORDENES
			ListaEtiquetas4.Add("TITULO", New ProductoEtiquetas With {.Titulo = "Ordenes Divisas", .Tooltip = "Ordenes Divisas", .TooltipOriginal = "Ordenes Divisas"})
			ListaEtiquetas4.Add("REGISTROS", New ProductoEtiquetas With {.Titulo = "registros", .Tooltip = "registros", .TooltipOriginal = "registros"})

			ListaRetorno.Add("A2OrdenesDivisasWPF.OrdenesView", ListaEtiquetas4)



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
            ListaRetorno.Add("GENERICO", ListaEtiquetas)

            Return ListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización los recursos.", "Test_Recursos", "ConsultarCombosAplicacion", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function
End Class
