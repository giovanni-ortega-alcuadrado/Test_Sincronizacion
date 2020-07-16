Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Collections.ObjectModel

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Public Const CLAVE_PARAM_CONEXION_DATOS As String = "A2CONSOLA_CNXBASEDATOS"
    Public Const URL_SERVICIOUPLOADS As String = "URL_SERVICIO_UPLOADS"
    Private Const COMPRIMIR_ARCHIVO_IMPORTAR As String = "COMPRIMIR_ARCHIVO_IMPORTAR"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"

    ''----------------------------------------------------------------------------------------------------------------------------
    '' 
    '' Declaración de propiedades
    '' 
    ''----------------------------------------------------------------------------------------------------------------------------

    Public Shared ReadOnly Property ClaveEspecifica(pstrClave As String) As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrClave.ToString()) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrClave.ToString()).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioUtilidadesOYD() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYD))
            End If
        End Get
    End Property

    ''' <summary>
    ''' Ruta donde se encuentra alojada la pagina para subir los archivos al servidor
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property URLFileUploads() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(URL_SERVICIOUPLOADS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(URL_SERVICIOUPLOADS).ToString()
                Else
                    Dim strRuta As String = RutaServicioUtilidadesOYD.Substring(0, RutaServicioUtilidadesOYD.ToLower.ToString().IndexOf("/services")) & "/Uploads.aspx"
                    Return strRuta
                End If
            Else
                Dim strRuta As String = RutaServicioUtilidadesOYD.Substring(0, RutaServicioUtilidadesOYD.ToLower.ToString().IndexOf("/services")) & "/Uploads.aspx"
                Return strRuta
            End If
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

    ''' <summary>
    ''' Consulta los parámetros definidos en los servicios de RIA y en base de datos que deben ser presentados en el acerca de
    ''' </summary>
    ''' 
    Public Shared Async Function leerParametroAppConsola(ByVal pstrAplicacion As String, ByVal pstrVersion As String, Optional ByVal pstrDivision As String = "", Optional ByVal pstrGrupo As String = "", Optional ByVal pstrSubgrupo As String = "", Optional ByVal pstrClasificacion As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objApp As A2Utilidades.Aplicacion
        Dim dcProxy As UtilidadesDomainContext
        Dim objParametros As LoadOperation(Of OYDUtilidades.ItemCombo) = Nothing

        Try
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString) Then ' Verificar que exista la colección de aplicaciones
                ' Obtener la referencia a la aplicación solicitada
                objApp = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(pstrAplicacion, pstrVersion, pstrDivision)

                If Not objApp Is Nothing Then
                    If Not objApp.Parametros.ContainsKey(CLAVE_PARAM_CONEXION_DATOS) Then ' Verificar que no exista ya el parámetro de conexión a base de datos en los parámetros de la aplicación
                        ' Si no existe se consulta, si ya existe no se hace nada
                        'dcProxy = inicializarProxy()
                        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                            dcProxy = New UtilidadesDomainContext()
                        Else
                            dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                        End If

                        objParametros = Await dcProxy.Load(dcProxy.leerParametrosAppConsolaWPFSyncQuery(pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

                        If Not objParametros Is Nothing Then
                            If objParametros.HasError Then
                                If objParametros.Error Is Nothing Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación pero no se recibió detalle del error.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                Else
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación.", "clsFuncionesCompartidas", "leerParametroAppConsola", Program.TituloSistema, Program.Maquina, objParametros.Error)
                                End If

                                objParametros.MarkErrorAsHandled()
                            Else
                                Dim objDiccionarioDLLRIA As New Dictionary(Of String, String)

                                If objParametros.Entities.Count > 0 Then
                                    For Each objParam In objParametros.Entities
                                        If objParam.Categoria = "DLLSERVICIORIA" Then
                                            If Not objDiccionarioDLLRIA.ContainsKey(objParam.ID) Then
                                                objDiccionarioDLLRIA.Add(objParam.ID, objParam.Descripcion)
                                            End If
                                        Else
                                            If Not objApp.Parametros.ContainsKey(objParam.ID) Then
                                                objApp.Parametros.Add(objParam.ID, objParam.Descripcion)
                                            End If
                                        End If
                                    Next objParam
                                End If

                                'actualiza las versiones de los assemblies ria
                                Program.Assembly_VerificarVersionesClienteEnDiccionario(objApp.ListaDLLRIAValidacion, objDiccionarioDLLRIA)
                            End If
                        End If ' If Not objParametros Is Nothing Then
                    End If ' If Not objApp.Parametros.ContainsKey(CLAVE_PARAM_CONEXION_DATOS) Then
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

    ''' <summary>
    ''' Consulta los combos genericos para la aplicación
    ''' </summary>
    ''' 
    Public Shared Async Function Plataforma_ConsultarCombosGenericos(ByVal pstrAplicacion As String, ByVal pstrVersion As String, Optional ByVal pstrDivision As String = "", Optional ByVal pstrGrupo As String = "", Optional ByVal pstrSubgrupo As String = "", Optional ByVal pstrClasificacion As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objApp As A2Utilidades.Aplicacion
        Dim dcProxy As UtilidadesDomainContext
        Dim objParametros As LoadOperation(Of OYDUtilidades.PLATAFORMA_CombosGenericos) = Nothing
        Dim dicListaCombosRetornoCompletos As New Dictionary(Of String, Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos)))
        Dim ListaEncabezado As New List(Of OYDUtilidades.PLATAFORMA_CombosGenericos)

        Try
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString) Then ' Verificar que exista la colección de aplicaciones
                ' Obtener la referencia a la aplicación solicitada
                objApp = CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString), A2Utilidades.Aplicaciones).obtenerAplicacion(pstrAplicacion, pstrVersion, pstrDivision)

                If Not objApp Is Nothing Then
                    ' Si no existe se consulta, si ya existe no se hace nada
                    'dcProxy = inicializarProxy()
                    If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                        dcProxy = New UtilidadesDomainContext()
                    Else
                        dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                    End If

                    objParametros = Await dcProxy.Load(dcProxy.Plataforma_Util_CargarCombosSyncQuery(pstrProducto:=String.Empty, pstrCondicionTexto1:=String.Empty, pstrCondicionTexto2:=String.Empty, pstrCondicionEntero1:=Nothing, pstrCondicionEntero2:=Nothing, pstrModulo:=String.Empty, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

                    If Not objParametros Is Nothing Then
                        If objParametros.HasError Then
                            If objParametros.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación pero no se recibió detalle del error.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación.", "clsFuncionesCompartidas", "leerParametroAppConsola", Program.TituloSistema, Program.Maquina, objParametros.Error)
                            End If

                            objParametros.MarkErrorAsHandled()
                        Else
                            Dim ListaCombosServidor As New ObservableCollection(Of A2Utilidades.ProductoCombos)
                            Dim ListaCombosOYD As New ObservableCollection(Of OYDUtilidades.ItemCombo)
                            ListaEncabezado = objParametros.Entities.ToList

                            For Each li In objParametros.Entities
                                'COMBOS PARA LA MIGRACIÓN DE OYD
                                '****************************************************************
                                If li.strOrigen = "OYD" Then
                                    ListaCombosOYD.Add(New OYDUtilidades.ItemCombo With {
                                                                .intID = li.intID,
                                                                .ID = li.strRetorno,
                                                                .Categoria = li.strTopico,
                                                                .Retorno = li.strRetorno,
                                                                .Descripcion = li.strDescripcion
                                                                })
                                    '****************************************************************
                                End If

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

                            'COMBOS POR APLICACIÓN
                            Dim lstOrigen = From lc In ListaEncabezado Select lc.strOrigen Distinct

                            For Each Origen As String In lstOrigen
                                Dim lstCombos = From lc In ListaEncabezado
                                                Where lc.strOrigen = Origen
                                                Select lc.strTopico Distinct

                                Dim objNodosCategoria As ObservableCollection(Of A2Utilidades.ProductoCombos) = Nothing
                                Dim strNombreCategoria As String
                                Dim dicListaCombosRetorno = New Dictionary(Of String, ObservableCollection(Of A2Utilidades.ProductoCombos))

                                For Each NombreCategoria As String In lstCombos
                                    strNombreCategoria = NombreCategoria
                                    objNodosCategoria = New ObservableCollection(Of A2Utilidades.ProductoCombos)((From ln In ListaCombosServidor Where ln.Topico = strNombreCategoria))

                                    dicListaCombosRetorno.Add(strNombreCategoria, objNodosCategoria)
                                Next

                                dicListaCombosRetornoCompletos.Add(Origen, dicListaCombosRetorno)
                            Next

                            objApp.Parametros.Add(Recursos.RecursosProducto.A2Consola_Producto_Combos.ToString, dicListaCombosRetornoCompletos)

                            'COMBOS PARA LA MIGRACIÓN DE OYD
                            '****************************************************************
                            Dim lstCombosOYD = From lc In ListaCombosOYD Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada
                            Dim strNombreCategoriaOYD As String = String.Empty
                            Dim objNodosCategoriaOYD As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
                            ' Guardar los datos recibidos en un diccionario
                            Dim dicListaCombosOYD As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))

                            For Each NombreCategoria As String In lstCombosOYD
                                strNombreCategoriaOYD = NombreCategoria
                                objNodosCategoriaOYD = New ObservableCollection(Of OYDUtilidades.ItemCombo)((From ln In ListaCombosOYD Where ln.Categoria = strNombreCategoriaOYD))

                                dicListaCombosOYD.Add(strNombreCategoriaOYD, objNodosCategoriaOYD)
                            Next
                            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                                Application.Current.Resources.Remove(Program.NOMBRE_LISTA_COMBOS)
                            End If
                            Application.Current.Resources.Add(Program.NOMBRE_LISTA_COMBOS,
                                                                          dicListaCombosOYD.ToDictionary(Function(k) k.Key, Function(k) k.Value.ToList()))
                            '****************************************************************



                        End If
                    End If ' If Not objParametros Is Nothing Then
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

    ''' <summary>
    ''' Verifica la validación de seguridad del RIA
    ''' </summary>
    ''' 
    Public Shared Async Function Seguridad_ValidarUsuario() As Task(Of String)

        Dim strRetorno As String = String.Empty
        Dim dcProxy As UtilidadesDomainContext
        Dim objParametros As InvokeOperation(Of String) = Nothing

        Try
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString) Then ' Verificar que exista la colección de aplicaciones
                ' Si no existe se consulta, si ya existe no se hace nada
                'dcProxy = inicializarProxy()
                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    dcProxy = New UtilidadesDomainContext()
                Else
                    dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                End If

                objParametros = Await dcProxy.Seguridad_ValidacionSync(Program.Usuario, Program.HashConexion).AsTask()

                If Not objParametros Is Nothing Then
                    If objParametros.HasError Then
                        If objParametros.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se genero un problema al realizar la consulta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se genero un problema al realizar la consulta.", "clsFuncionesCompartidas", "Seguridad_ValidarUsuario", Program.TituloSistema, Program.Maquina, objParametros.Error)
                        End If

                        objParametros.MarkErrorAsHandled()
                    Else
                        strRetorno = objParametros.Value
                    End If
                End If ' If Not objParametros Is Nothing Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se genero un problema al realizar la consulta.", "clsFuncionesCompartidas", "Seguridad_ValidarUsuario", Program.TituloSistema, Program.Maquina, ex)
        Finally
            dcProxy = Nothing
        End Try

        Return (strRetorno)
    End Function

    ''' <summary>
    ''' Verifica la IP que esta dando el servidor del cliente
    ''' </summary>
    ''' 
    Public Shared Async Function Seguridad_ObtenerIP() As Task(Of String)

        Dim strRetorno As String = String.Empty
        Dim dcProxy As UtilidadesDomainContext
        Dim objParametros As InvokeOperation(Of String) = Nothing

        Try
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString) Then ' Verificar que exista la colección de aplicaciones
                ' Si no existe se consulta, si ya existe no se hace nada
                'dcProxy = inicializarProxy()
                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    dcProxy = New UtilidadesDomainContext()
                Else
                    dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                End If

                objParametros = Await dcProxy.Seguridad_ObtenerIPClienteSync().AsTask()

                If Not objParametros Is Nothing Then
                    If objParametros.HasError Then
                        If objParametros.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se genero un problema al realizar la consulta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se genero un problema al realizar la consulta.", "clsFuncionesCompartidas", "Seguridad_ObtenerIP", Program.TituloSistema, Program.Maquina, objParametros.Error)
                        End If

                        objParametros.MarkErrorAsHandled()
                    Else
                        strRetorno = objParametros.Value
                    End If
                End If ' If Not objParametros Is Nothing Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se genero un problema al realizar la consulta.", "clsFuncionesCompartidas", "Seguridad_ObtenerIP", Program.TituloSistema, Program.Maquina, ex)
        Finally
            dcProxy = Nothing
        End Try

        Return (strRetorno)
    End Function

    ''' <summary>
    ''' Verifica el Usuario que esta dando el servidor del cliente
    ''' </summary>
    ''' 
    Public Shared Async Function Seguridad_ObtenerUsuario() As Task(Of String)

        Dim strRetorno As String = String.Empty
        Dim dcProxy As UtilidadesDomainContext
        Dim objParametros As InvokeOperation(Of String) = Nothing

        Try
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_Aplicaciones.ToString) Then ' Verificar que exista la colección de aplicaciones
                ' Si no existe se consulta, si ya existe no se hace nada
                'dcProxy = inicializarProxy()
                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    dcProxy = New UtilidadesDomainContext()
                Else
                    dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                End If

                objParametros = Await dcProxy.Seguridad_ObtenerUsuarioClienteSync().AsTask()

                If Not objParametros Is Nothing Then
                    If objParametros.HasError Then
                        If objParametros.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se genero un problema al realizar la consulta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se genero un problema al realizar la consulta.", "clsFuncionesCompartidas", "Seguridad_ObtenerUsuario", Program.TituloSistema, Program.Maquina, objParametros.Error)
                        End If

                        objParametros.MarkErrorAsHandled()
                    Else
                        strRetorno = objParametros.Value
                    End If
                End If ' If Not objParametros Is Nothing Then
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se genero un problema al realizar la consulta.", "clsFuncionesCompartidas", "Seguridad_ObtenerUsuario", Program.TituloSistema, Program.Maquina, ex)
        Finally
            dcProxy = Nothing
        End Try

        Return (strRetorno)
    End Function

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    Public Shared ReadOnly Property ComprimirArchivoImportar As Boolean
        Get
            Dim logComprimir As Boolean = False
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(COMPRIMIR_ARCHIVO_IMPORTAR) Then
                    If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(COMPRIMIR_ARCHIVO_IMPORTAR).ToString() = "1" Then
                        logComprimir = True
                    End If
                End If
            End If
            Return logComprimir
        End Get
    End Property

End Class