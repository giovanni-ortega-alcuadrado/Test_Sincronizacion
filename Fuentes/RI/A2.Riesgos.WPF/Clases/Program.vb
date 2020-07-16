Imports Telerik.Windows.Controls
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports System.Globalization
Imports A2.Riesgos.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

#Region "Constantes públicas específicas"
    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    Private Enum TipoServicio
        URLServicioRiesgos
    End Enum

#End Region

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 

    Friend Const TABLA As String = "Tabla"
    Friend Const GRID As String = "GRID"
    Friend Const RANGO_BANDAS As String = "A2_TBL_BANDAS"
    Friend Const RANGO_COMPONENTES As String = "A2_TBL_COMPONENTES"
    Friend Const RANGO_ALERTAS_RANGOS As String = "TBL_ALERTAS_RANGOS"
    Friend Const RANGO_POSICIONES As String = "A2_TBL_COMPONENTES_PROPIEDADES"
    Friend Const RANGO_ACCIONES As String = "TBL_ACCIONES"
    Friend Const RANGO_METODOS As String = "A2_TBL_METODOS"
    Friend Const RANGO_VERSIONES As String = "A2_TBL_VERSIONES"
    Friend Const RANGO_CONSULTAS_TITULOS As String = "A2_TBL_CONSULTAS_TITULOS"
    Friend Const RANGO_CONSULTAS As String = "A2_TBL_CONSULTAS"
    Friend Const RANGO_NOTIFICACIONES As String = "A2_TBL_SERVICIONOTIFICACIONES"
    Friend Const RANGO_AUTORIZACIONES As String = "UrlServicioAutorizacion"
    Friend Const RANGO_REQUIEREAUTORIZACION As String = "SolicitarAutorizacion"
    Friend Const RANGO_LISTA_ALERTAS As String = "A2_LISTA_ALERTAS"
    Friend Const RANGO_LISTA_TIPO_ALERTA As String = "A2_LISTA_TIPOALERTA"
    Friend Const RANGO_LISTA_BOLEANA As String = "A2_LISTABOLEANA"
    Friend Const RANGO_LISTA_PRESENTACION As String = "A2_LISTAPRESENTACION"
    Friend Const RANGO_PARAMETROS_CONFIGURACION As String = "A2_TBL_PARAMETROS_CONFIGURACION"
    Friend Const RANGO_PARAMETROS_VISUALIZACION As String = "TBL_PARAMETROS_VISUALIZACION"
    Friend Const RANGO_PARAMETROS_CONSULTA As String = "TBL_PARAMETROS_CONSULTA"
    Friend Const RANGO_A2_PARAMETROS_GRAFICO As String = "A2_TBL_PARAMETROS_GRAFICO"
    Friend Const RANGO_PARAMETROS_GRAFICO As String = "TBL_PARAMETROS_GRAFICO"
    Friend Const HOJA_VISUALIZACION As String = "Visualización"

    '''' ************** Archivo XMLConfiguracion
    Private Const COLOR_VERDE As String = "NOMBRE_VERDE"
    Private Const COLOR_AMARILLO As String = "NOMBRE_AMARILLO"
    Private Const COLOR_ROJO As String = "NOMBRE_ROJO"
    Private Const TOPICO_NOTIFICACIONES_DATOS As String = "TOPICO_NOTIFICACIONES_DATOS"
    Private Const TOPICO_NOTIFICACIONES_ALERTAS As String = "TOPICO_NOTIFICACIONES_ALERTAS"
    Private Const INTERVAL_TIMER_PERIODICO As String = "INTERVAL_TIMER_PERIODICO"
    Private Const INTERVAL_TIMER_RAFAGA As String = "INTERVAL_TIMER_RAFAGA"
    Private Const INTERVAL_VELOCIDAD_RAFAGA As String = "INTERVAL_VELOCIDAD_RAFAGA"

    ''' *************** Visibilidad de botones del MC
    Public Const MC_SUBIR_CONFIGURACION As String = "mcSubirConfiguracion"
    Public Const MC_BAJAR_CONFIGURACION As String = "mcBajarConfiguracion"
    Public Const MC_NUEVO_METODO As String = "mcNuevoMetodo"
    Public Const MC_BORRAR_METODO As String = "mcBorrarMetodo"
    Public Const BTN_NUEVA_VERSION As String = "btnNuevaVersion"
    Public Const BTN_BORRAR_VERSION As String = "btnBorrarVersion"
    Public Const BTN_BAJAR_VERSION As String = "btnBajarVersion"
    Public Const MNU_SELECCIONAR_VERSION As String = "mnuSeleccionarVersion"
    Public Const MNU_CANCELAR_VERSION As String = "mnuCancelarVersion"
    Public Const BTN_NUEVA_CONSULTA As String = "btnNuevaConsulta"
    Public Const BTN_BORRAR_CONSULTA As String = "btnBorrarConsulta"


    Public Const FRM_ADMIN_MC As String = "frmAdminMC"
    Public Const STR_EMAIL_PATRON As String = "\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"
    Public Const GSTR_MENSAJE_CAMBIOLIBRORIESGO = "El usuario {0}, modifico el libro {1}, el proceso de actualización de datos puede tomar 5-10 minutos hasta que el motor de cálculo procesa de nuevo las consultas y se vean reflejados los cambios."
    Public Const GSTR_MENSAJE_NUEVAVERSIONRIESGO = "El usuario {0}, creo una nueva versión del libro {1}, el proceso de actualización de datos puede tomar 5-10 minutos hasta que el motor de cálculo procesa de nuevo las consultas y se vean reflejados los cambios."
    Public Const GSTR_MENSAJE_SELECCIONARVERSIONRIESGO = "El usuario {0}, cambio la versión {2} del libro {1} por la versión {3}, el proceso de actualización de datos puede tomar 5-10 minutos hasta que el motor de cálculo procesa de nuevo las consultas y se vean reflejados los cambios."

    Public Const CLAVE_PARAM_CONEXION_DATOS As String = "A2CONSOLA_CNXBASEDATOS"
    Public Const USUARIO_ACTIVO As String = "A2Consola_UsuarioActivo"

#Region "Propiedades públicas genéricos"


    Friend Shared Sub CopiarObjeto(Of T)(pobjOrigen As T, pobjDestino As T)
        If pobjDestino Is Nothing Then
            'Throws a new exception. 
            Throw New System.Exception("Debe inicializar el objeto de destino")
            Exit Sub
        End If
        FuncionesCompartidas.CopyObject(Of T)(pobjOrigen, pobjDestino)
    End Sub

    ''' <summary>
    ''' Mensaje que se presenta al usuario cuando da clic para ejecutar alguna acción y aún está pendiente algún proceso asincrónico
    ''' </summary>
    Friend Shared ReadOnly Property MensajeEsperaOperacion() As String
        Get
            Return (FuncionesCompartidas.MensajeEsperaOperacion())
        End Get
    End Property

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    ''' <summary>
    ''' Funcion para retornar clave del archivo de configuracion
    ''' </summary>
    ''' <param name="pstrConstante">cave en el archivo</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function RetornarClave(ByVal pstrConstante As String) As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrConstante) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrConstante).ToString()
            Else
                Return ("")
            End If
        Else
            Return ("")
        End If
    End Function

    Public Shared ReadOnly Property RutaServicioMotorCalculo() As String
        Get
            Return Program.URLMotordeCalculos
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioNotificaciones() As String
        Get
            Return Program.URLNotificaciones
        End Get
    End Property

    Public Shared ReadOnly Property ColorVerde As String
        Get
            Return RetornarClave(COLOR_VERDE)
        End Get
    End Property

    Public Shared ReadOnly Property ColorAmarillo As String
        Get
            Return RetornarClave(COLOR_AMARILLO)
        End Get
    End Property

    Public Shared ReadOnly Property ColorRojo As String
        Get
            Return RetornarClave(COLOR_ROJO)
        End Get
    End Property

    Public Shared ReadOnly Property TopicoNotificacionesTablero As String
        Get
            Return RetornarClave(TOPICO_NOTIFICACIONES_DATOS)
        End Get
    End Property

    Public Shared ReadOnly Property TopicoNotificacionesAlertas As String
        Get
            Return RetornarClave(TOPICO_NOTIFICACIONES_ALERTAS)
        End Get
    End Property

    Public Shared ReadOnly Property IntervalTimerPeriodico As String
        Get
            Return RetornarClave(INTERVAL_TIMER_PERIODICO)
        End Get
    End Property

    Public Shared ReadOnly Property IntervalTimerRafaga As String
        Get
            Return RetornarClave(INTERVAL_TIMER_RAFAGA)
        End Get
    End Property

    Public Shared ReadOnly Property IntervalTimerVelocidadRafaga As String
        Get
            Return RetornarClave(INTERVAL_VELOCIDAD_RAFAGA)
        End Get
    End Property

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            If Program.UsaUsuarioSinDomino Then
                Return Program.UsuarioSinDomino
            Else
                Return A2Utilidades.Program.Usuario
            End If
        End Get
    End Property

    Public Overloads Shared ReadOnly Property ClaveUsuario As String
        Get
            If String.IsNullOrEmpty(Program.Password) Then
                Return "123"
            Else
                Return Program.Password
            End If
        End Get
    End Property

    Public Shared ReadOnly Property UsuarioAutenticado As String
        Get
            If Application.Current.Resources.Contains(USUARIO_ACTIVO) Then
                Dim objUsuario As A2Utilidades.Usuario = CType(Application.Current.Resources(USUARIO_ACTIVO), A2Utilidades.Usuario)
                If objUsuario.SeguridadIntegrada Then
                    Return objUsuario.UsuarioWindows
                Else
                    Return Program.Usuario
                End If
            Else
                Return String.Empty
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioRiesgos As String
        Get
            Return RetornarClave(Program.TipoServicio.URLServicioRiesgos.ToString())
        End Get
    End Property


#End Region

#Region "Métodos públicos genéricos"

    ''' <summary>
    ''' Valdar si un string enviado en el primer parámetro es nothing o vacio y retornar el valor enviado en el segundo parámetro
    ''' </summary>
    Friend Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
        Return (FuncionesCompartidas.validarValorString(pstrValor, pstrRetornoNothingOVacio))
    End Function

    Friend Shared ReadOnly Property obtenerMensajeValidacion(ByVal pstrMsg As String, ByVal pstrAccion As String, ByVal plogError As Boolean) As String
        Get
            Return (FuncionesCompartidas.obtenerMensajeValidacion(pstrMsg, pstrAccion, plogError))
        End Get
    End Property


    Public Shared Function colorFromHex(pstrHex As String) As Color
        Try
            pstrHex = pstrHex.Replace("#", String.Empty)
            Dim r, g, b As Byte

            r = Byte.Parse(pstrHex.Substring(0, 2), NumberStyles.HexNumber)
            g = Byte.Parse(pstrHex.Substring(2, 2), NumberStyles.HexNumber)
            b = Byte.Parse(pstrHex.Substring(4, 2), NumberStyles.HexNumber)
            Return Color.FromArgb(255, r, g, b)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de asignar color desde hexadecimal", Program.TituloSistema, "colorFromHex", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    Public Shared Function TiposGrafico() As List(Of OYDUtilidades.ItemCombo)
        Dim listaTiposGrafico As New List(Of OYDUtilidades.ItemCombo)
        listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 0, .ID = "Columna", .Descripcion = "Columnas", .Categoria = "Columna"})
        listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 0, .ID = "Linea", .Descripcion = "Líneas", .Categoria = "Linea"})
        listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 0, .ID = "Circular", .Descripcion = "Circular", .Categoria = "Circular"})
        listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 0, .ID = "Barra", .Descripcion = "Barra", .Categoria = "Barra"})
        listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 0, .ID = "Area", .Descripcion = "Área", .Categoria = "Area"})

        Return listaTiposGrafico
    End Function

    Public Shared Function TiposGraficoXCategoria(ByVal Categoria As String) As List(Of OYDUtilidades.ItemCombo)

        Dim listaTiposGrafico As New List(Of OYDUtilidades.ItemCombo)
        Dim ListaTiposGraficoXCategoria As New List(Of OYDUtilidades.ItemCombo)

        If Not String.IsNullOrEmpty(Categoria) Then

            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 16, .ID = "ColumnClustered", .Descripcion = "Columna", .Categoria = "Columna"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 18, .ID = "ColumnStacked", .Descripcion = "Columna apilada", .Categoria = "Columna"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 19, .ID = "ColumnStacked100", .Descripcion = "Columna 100% apilada", .Categoria = "Columna"})

            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 39, .ID = "Line", .Descripcion = "Líneas", .Categoria = "Linea"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 41, .ID = "LineMarkers", .Descripcion = "Línea con marcadores", .Categoria = "Linea"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 42, .ID = "LineMarkersStacked", .Descripcion = "Línea apilada con marcadores", .Categoria = "Linea"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 43, .ID = "LineMarkersStacked100", .Descripcion = "Línea 100% apilada con marcadores", .Categoria = "Linea"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 44, .ID = "LineStacked", .Descripcion = "Línea apilada", .Categoria = "Linea"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 45, .ID = "LineStacked100", .Descripcion = "Línea 100% apilada", .Categoria = "Linea"})

            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 46, .ID = "Pie", .Descripcion = "Circular", .Categoria = "Circular"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 48, .ID = "PieExploded", .Descripcion = "Gráfico circular seccionado", .Categoria = "Circular"})

            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 6, .ID = "BarClustered", .Descripcion = "Barra agrupada", .Categoria = "Barra"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 8, .ID = "BarStacked", .Descripcion = "Barra apilada", .Categoria = "Barra"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 9, .ID = "BarStacked100", .Descripcion = "Barra 100% apilada", .Categoria = "Barra"})

            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 0, .ID = "Area", .Descripcion = "Área agrupada", .Categoria = "Area"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 2, .ID = "AreaStacked", .Descripcion = "Área apilada", .Categoria = "Area"})
            listaTiposGrafico.Add(New OYDUtilidades.ItemCombo With {.intID = 3, .ID = "AreaStacked100", .Descripcion = "Área 100% apilada", .Categoria = "Area"})

            ListaTiposGraficoXCategoria = (From item In listaTiposGrafico Where item.Categoria.ToString().Trim() = Categoria.Trim()).ToList()
        End If

        Return ListaTiposGraficoXCategoria

    End Function

#End Region


#Region "Consola"



    ''' <summary>
    ''' Consulta los parámetros definidos en los servicios de RIA y en base de datos que deben ser presentados en el acerca de
    ''' </summary>
    ''' 
    Public Shared Async Function leerParametroAppConsola(ByVal pstrAplicacion As String, ByVal pstrVersion As String, Optional ByVal pstrDivision As String = "", Optional ByVal pstrGrupo As String = "", Optional ByVal pstrSubgrupo As String = "", Optional ByVal pstrClasificacion As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objApp As A2Utilidades.Aplicacion
        Dim dcProxy As RiesgosDomainContext
        Dim objParametros As LoadOperation(Of A2.Riesgos.RIA.Web.ItemCombo) = Nothing

        Try
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString) Then ' Verificar que exista la colección de aplicaciones
                ' Obtener la referencia a la aplicación solicitada
                objApp = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(pstrAplicacion, pstrVersion, pstrDivision)

                If Not objApp Is Nothing Then
                    If Not objApp.Parametros.ContainsKey(CLAVE_PARAM_CONEXION_DATOS) Then ' Verificar que no exista ya el parámetro de conexión a base de datos en los parámetros de la aplicación
                        ' Si no existe se consulta, si ya existe no se hace nada
                        'dcProxy = inicializarProxy()
                        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                            dcProxy = New RiesgosDomainContext()
                        Else
                            dcProxy = New RiesgosDomainContext(New System.Uri(Program.RutaServicioRiesgos))
                        End If

                        objParametros = Await dcProxy.Load(dcProxy.leerParametrosAppConsolaSyncQuery(pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

                        If Not objParametros Is Nothing Then
                            If objParametros.HasError Then
                                If objParametros.Error Is Nothing Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación pero no se recibió detalle del error.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                Else
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación.", "clsFuncionesCompartidas", "leerParametroAppConsola", Program.TituloSistema, Program.Maquina, objParametros.Error)
                                End If

                                objParametros.MarkErrorAsHandled()
                            Else
                                If objParametros.Entities.Count > 0 Then
                                    For Each objParam In objParametros.Entities
                                        objApp.Parametros.Add(objParam.ID, objParam.Descripcion)
                                    Next objParam
                                End If
                            End If
                        End If ' If Not objParametros Is Nothing Then
                    End If ' If Not objApplication.Parametros.ContainsKey(CLAVE_PARAM_CONEXION_DATOS) Then
                End If

                logResultado = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para configurar los parámetros de la aplicación.", "clsFuncionesCompartidas", "leerParametroAppConsola", Program.TituloSistema, Program.Maquina, ex)
        Finally
            dcProxy = Nothing
        End Try

        Return (logResultado)
    End Function


#End Region

End Class
