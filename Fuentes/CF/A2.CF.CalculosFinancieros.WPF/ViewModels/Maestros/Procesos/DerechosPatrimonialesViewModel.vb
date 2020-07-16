Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports A2ComunesImportaciones
Imports A2CFProcesarPortafolio


Public Class DerechosPatrimonialesViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla DerechosPatrimoniales perteneciente al proyecto de Cálculos Financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por:  Jorge Peña (Alcuadrado S.A.)
    ''' Descripción: Creacion. 
    ''' Fecha:       3 de Septiembre/2015
    ''' Pruebas CB:  Jorge Peña - 3 de Septiembre/2015 - Resultado Ok 
    ''' </history>

#Region "Constantes"

    Private Const STR_PARAMETROS_EXPORTAR_ACCIONES_RENTAFIJA As String = "[FECHAPROCESO]=[[FECHAPROCESO]]|[USUARIO]=[[USUARIO]]"
    Private Const STR_PARAMETROS_EXPORTAR_PASO4 As String = "[FECHAPROCESO]=[[FECHAPROCESO]]|[TIPOCLIENTE]=[[TIPOCLIENTE]]|[COMITENTE]=[[COMITENTE]]|[TIPORESULTADO]=[[TIPORESULTADO]]|[RAZONABILIDAD]=[[RAZONABILIDAD]]|[USUARIO]=[[USUARIO]]"
    Private Const STR_CARPETA As String = "DERECHOSPATRIMONIALES"
    Private Const STR_PROCESO_ACCIONES As String = "DerechosPatrimonialesAcciones"
    Private Const STR_PROCESO_RENTAFIJA As String = "DerechosPatrimonialesRentaFija"
    Private Const STR_PROCESO_PASO4 As String = "DerechosPatrimonialesGenerar"

    Private Enum PARAMETROSEXPORTAR
        FECHAPROCESO
        TIPOCLIENTE
        COMITENTE
        TIPORESULTADO
        RAZONABILIDAD
        USUARIO
    End Enum

#End Region

#Region "Variables - REQUERIDO"

    Public ViewDerechosPatrimoniales As DerechosPatrimonialesView = Nothing

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Métodos utilizados para la ventana modal con el cobro de utilidades pendientes
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjVM As ProcesoCobroUtilidadesViewModel
    Private ViewProcesoCobroUtilidades As ProcesoCobroUtilidadesView
    'Private dcProxy As ImportacionesDomainContex
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"

    Dim DESCRPCION_PASO_1 As String = "PASO_1"
    Dim DESCRPCION_PASO_2 As String = "PASO_2"
    Dim DESCRPCION_PASO_3 As String = "PASO_3"
    Dim DESCRPCION_PASO_4 As String = "PASO_4"

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
        '    strTipoResultado = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOPORTAFOLIO_VAL").FirstOrDefault.ID
        'End If
        IndexTipoCliente = 0  '(Todos)
        IndexTipoResultado = 0  '(Todos)
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0001
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Function inicializar() As Boolean
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarValoresPorDefecto()

                If String.IsNullOrEmpty(NOMBRE_ETIQUETA_COMITENTE) Then
                    NOMBRE_ETIQUETA_COMITENTE = NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO
                End If

                If String.IsNullOrEmpty(DESCRPCION_PASO_1) Then
                    STR_DESCRPCION_PASO_1 = DESCRPCION_PASO_1
                End If

                If String.IsNullOrEmpty(DESCRPCION_PASO_2) Then
                    STR_DESCRPCION_PASO_2 = DESCRPCION_PASO_2
                End If

                If String.IsNullOrEmpty(DESCRPCION_PASO_3) Then
                    STR_DESCRPCION_PASO_3 = DESCRPCION_PASO_3
                End If

                If String.IsNullOrEmpty(DESCRPCION_PASO_4) Then
                    STR_DESCRPCION_PASO_4 = DESCRPCION_PASO_4
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _dtmFechaAcciones As System.Nullable(Of System.DateTime)
    Public Property dtmFechaAcciones() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaAcciones
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaAcciones = value
            MyBase.CambioItem("dtmFechaAcciones")
        End Set
    End Property

    Private _dtmFechaRentaFija As System.Nullable(Of System.DateTime)
    Public Property dtmFechaRentaFija() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaRentaFija
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaRentaFija = value
            MyBase.CambioItem("dtmFechaRentaFija")
        End Set
    End Property

    Private _dtmFechaGenerar As System.Nullable(Of System.DateTime)
    Public Property dtmFechaGenerar() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaGenerar
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaGenerar = value
            MyBase.CambioItem("dtmFechaGenerar")
        End Set
    End Property

    Private _strTipoCliente As String = String.Empty
    Public Property strTipoCliente() As String
        Get
            Return _strTipoCliente
        End Get
        Set(ByVal value As String)
            _strTipoCliente = value
            MyBase.CambioItem("strTipoCliente")
        End Set
    End Property

    Private _strTipoResultado As String = String.Empty
    Public Property strTipoResultado() As String
        Get
            Return _strTipoResultado
        End Get
        Set(ByVal value As String)
            _strTipoResultado = value
            MyBase.CambioItem("strTipoResultado")
        End Set
    End Property

    Private _dblRazonabilidad As Double
    Public Property dblRazonabilidad() As Double
        Get
            Return _dblRazonabilidad
        End Get
        Set(ByVal value As Double)
            _dblRazonabilidad = value
            MyBase.CambioItem("dblRazonabilidad")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = LTrim(RTrim(value))

            If Not String.IsNullOrEmpty(lngIDComitente) Then
                ConsultarDatosPortafolio()
            End If

            MyBase.CambioItem("lngIDComitente")
        End Set
    End Property

    Private _strNombreComitente As String = String.Empty
    Public Property strNombreComitente() As String
        Get
            Return _strNombreComitente
        End Get
        Set(ByVal value As String)
            _strNombreComitente = value
            MyBase.CambioItem("strNombreComitente")
        End Set
    End Property

    Private _strMensaje As String = String.Empty
    Public Property strMensaje() As String
        Get
            Return _strMensaje
        End Get
        Set(ByVal value As String)
            _strMensaje = value
            MyBase.CambioItem("strMensaje")
        End Set
    End Property

    Private _IndexTipoCliente As Integer
    Public Property IndexTipoCliente() As Integer
        Get
            Return _IndexTipoCliente
        End Get
        Set(ByVal value As Integer)
            _IndexTipoCliente = value
            MyBase.CambioItem("IndexTipoCliente")
        End Set
    End Property

    Private _IndexTipoResultado As Integer
    Public Property IndexTipoResultado() As Integer
        Get
            Return _IndexTipoResultado
        End Get
        Set(ByVal value As Integer)
            _IndexTipoResultado = value
            MyBase.CambioItem("IndexTipoResultado")
        End Set
    End Property

    Private _NOMBRE_ETIQUETA_COMITENTE As String = Program.STR_NOMBRE_ETIQUETA_COMITENTE
    Public Property NOMBRE_ETIQUETA_COMITENTE() As String
        Get
            Return _NOMBRE_ETIQUETA_COMITENTE
        End Get
        Set(ByVal value As String)
            _NOMBRE_ETIQUETA_COMITENTE = value
            MyBase.CambioItem("NOMBRE_ETIQUETA_COMITENTE")
        End Set
    End Property

    Private _STR_DESCRPCION_PASO_1 As String = Program.DESCRPCION_PASO_1
    Public Property STR_DESCRPCION_PASO_1() As String
        Get
            Return _STR_DESCRPCION_PASO_1
        End Get
        Set(ByVal value As String)
            _STR_DESCRPCION_PASO_1 = value
            MyBase.CambioItem("STR_DESCRPCION_PASO_1")
        End Set
    End Property

    Private _STR_DESCRPCION_PASO_2 As String = Program.DESCRPCION_PASO_2
    Public Property STR_DESCRPCION_PASO_2() As String
        Get
            Return _STR_DESCRPCION_PASO_2
        End Get
        Set(ByVal value As String)
            _STR_DESCRPCION_PASO_2 = value
            MyBase.CambioItem("STR_DESCRPCION_PASO_2")
        End Set
    End Property

    Private _STR_DESCRPCION_PASO_3 As String = Program.DESCRPCION_PASO_3
    Public Property STR_DESCRPCION_PASO_3() As String
        Get
            Return _STR_DESCRPCION_PASO_3
        End Get
        Set(ByVal value As String)
            _STR_DESCRPCION_PASO_3 = value
            MyBase.CambioItem("STR_DESCRPCION_PASO_3")
        End Set
    End Property

    Private _STR_DESCRPCION_PASO_4 As String = Program.DESCRPCION_PASO_4
    Public Property STR_DESCRPCION_PASO_4() As String
        Get
            Return _STR_DESCRPCION_PASO_4
        End Get
        Set(ByVal value As String)
            _STR_DESCRPCION_PASO_4 = value
            MyBase.CambioItem("STR_DESCRPCION_PASO_4")
        End Set
    End Property

    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado() As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _ViewImportarArchivo As cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private _strRutaDeceval As String
    Public Property strRutaDeceval() As String
        Get
            Return _strRutaDeceval
        End Get
        Set(ByVal value As String)
            _strRutaDeceval = value
            MyBase.CambioItem("strRutaDeceval")
        End Set
    End Property

    Private _strRutaDCV As String
    Public Property strRutaDCV() As String
        Get
            Return _strRutaDCV
        End Get
        Set(ByVal value As String)
            _strRutaDCV = value
            MyBase.CambioItem("strRutaDCV")
        End Set
    End Property

    Private _strRutaOtros As String
    Public Property strRutaOtros() As String
        Get
            Return _strRutaOtros
        End Get
        Set(ByVal value As String)
            _strRutaOtros = value
            MyBase.CambioItem("strRutaOtros")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    Public Async Function ConsultarDatosPortafolio() As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(_lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        strNombreComitente = Nothing
                    End If
                End If
            Else
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Función utilizada para importar el archivo Deceval.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0009
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Function DerechosPatrimoniales_Deceval_Importar(pstrModulo As String, pstrNombreCompletoArchivo As String, plogEliminarRegistrosTodos As Boolean) As Task
        Try
            Dim objRet As LoadOperation(Of RespuestaArchivoImportacion)

            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.RespuestaArchivoImportacions.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.DerechosPatrimoniales_Deceval_ImportarSyncQuery(dtmFechaRentaFija, "DerechosPatrimonialesDeceval", pstrNombreCompletoArchivo, Program.Usuario, plogEliminarRegistrosTodos, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of RespuestaArchivoImportacion)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = objRet.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                                objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                                    If li.Tipo = "C" Then
                                        objListaMensajes.Add(li.Mensaje)
                                    Else
                                        objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                                    End If
                                Next

                                objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                                ViewImportarArchivo.IsBusy = False
                            Else
                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean))
                                    objListaMensajes.Add(li.Mensaje)
                                Next
                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                                ViewImportarArchivo.IsBusy = False
                            End If
                        Else
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                            ViewImportarArchivo.IsBusy = False
                        End If
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo Deceval.", _
                               Me.ToString(), "DerechosPatrimoniales_Deceval_Importar", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Function

    ''' <summary>
    ''' Función utilizada para importar el archivo DCV.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0010
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Function DerechosPatrimoniales_DCV_Importar(pstrModulo As String, pstrNombreCompletoArchivo As String, plogEliminarRegistrosTodos As Boolean) As Task
        Try
            Dim objRet As LoadOperation(Of RespuestaArchivoImportacion)

            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.RespuestaArchivoImportacions.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.DerechosPatrimoniales_DCV_ImportarSyncQuery(dtmFechaRentaFija, "DerechosPatrimonialesDCV", pstrNombreCompletoArchivo, Program.Usuario, plogEliminarRegistrosTodos, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of RespuestaArchivoImportacion)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = objRet.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                                objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                                    If li.Tipo = "C" Then
                                        objListaMensajes.Add(li.Mensaje)
                                    Else
                                        objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                                    End If
                                Next

                                objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                                ViewImportarArchivo.IsBusy = False
                            Else
                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean))
                                    objListaMensajes.Add(li.Mensaje)
                                Next
                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                                ViewImportarArchivo.IsBusy = False
                            End If
                        Else
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                            ViewImportarArchivo.IsBusy = False
                        End If
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo DCV.", _
                               Me.ToString(), "DerechosPatrimoniales_DCV_Importar", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Function

    ''' <summary>
    ''' Función utilizada para importar el archivo Otros.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0011
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Function DerechosPatrimoniales_Otros_Importar(pstrModulo As String, pstrNombreCompletoArchivo As String, plogEliminarRegistrosTodos As Boolean) As Task
        Try
            Dim objRet As LoadOperation(Of RespuestaArchivoImportacion)

            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.RespuestaArchivoImportacions.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.DerechosPatrimoniales_Otros_ImportarSyncQuery(dtmFechaRentaFija, "DerechosPatrimonialesOtros", pstrNombreCompletoArchivo, Program.Usuario, plogEliminarRegistrosTodos, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of RespuestaArchivoImportacion)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = objRet.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                                objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                                    If li.Tipo = "C" Then
                                        objListaMensajes.Add(li.Mensaje)
                                    Else
                                        objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                                    End If
                                Next

                                objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                                ViewImportarArchivo.IsBusy = False
                            Else
                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean))
                                    objListaMensajes.Add(li.Mensaje)
                                Next
                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                                ViewImportarArchivo.IsBusy = False
                            End If
                        Else
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                            ViewImportarArchivo.IsBusy = False
                        End If
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo de otros depósitos.", _
                               Me.ToString(), "DerechosPatrimoniales_Otros_Importar", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Function

    ''' <summary>
    ''' Función utilizada para importar el archivo Deceval.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0009, CP0010, CP0011
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Function CargarArchivo(pstrModulo As String, pstrNombreCompletoArchivo As String, plogEliminarRegistrosTodos As Boolean) As Task
        Try

            If pstrModulo = "DerechosPatrimonialesDeceval" Then
                Await DerechosPatrimoniales_Deceval_Importar(pstrModulo, pstrNombreCompletoArchivo, plogEliminarRegistrosTodos)
                strRutaDeceval = pstrNombreCompletoArchivo
            ElseIf pstrModulo = "DerechosPatrimonialesDCV" Then
                Await DerechosPatrimoniales_DCV_Importar(pstrModulo, pstrNombreCompletoArchivo, plogEliminarRegistrosTodos)
                strRutaDCV = pstrNombreCompletoArchivo
            ElseIf pstrModulo = "DerechosPatrimonialesOtros" Then
                Await DerechosPatrimoniales_Otros_Importar(pstrModulo, pstrNombreCompletoArchivo, plogEliminarRegistrosTodos)
                strRutaOtros = pstrNombreCompletoArchivo
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al subir el archivo.", _
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Sub Atras()
        Try
            If TabSeleccionado <= 3 Then
                TabSeleccionado = TabSeleccionado - 1
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Atras", Me.ToString(), "Atras", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Siguiente()
        Try
            If TabSeleccionado <= 2 Then
                TabSeleccionado = TabSeleccionado + 1
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Siguiente", Me.ToString(), "Siguiente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Realiza la exportación  de los resultados de valoración a Excel.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0006
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Sub ExportarInformeAcciones()
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR_ACCIONES_RENTAFIJA
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaAcciones As String = String.Empty

            If Not ValidarDatos() Then Exit Sub

            dcProxyUtil = inicializarProxyUtilidadesOYD()

            If (_dtmFechaAcciones.Value.Day < 10) Then
                dia = "0" + _dtmFechaAcciones.Value.Day.ToString
            Else
                dia = _dtmFechaAcciones.Value.Day.ToString
            End If

            If (_dtmFechaAcciones.Value.Month < 10) Then
                mes = "0" + _dtmFechaAcciones.Value.Month.ToString
            Else
                mes = _dtmFechaAcciones.Value.Month.ToString
            End If

            ano = _dtmFechaAcciones.Value.Year.ToString

            strFechaAcciones = ano + mes + dia

            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHAPROCESO), strFechaAcciones)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

            objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO_ACCIONES, strParametros, "DerechosPatrimonialesExdividendos", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeAcciones", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    IsBusy = False
                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objResultado = objRet.Entities.First

                        If objResultado.Exitoso Then
                            Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano & "?date=" & strFechaAcciones & DateTime.Now.ToString("HH:mm:ss"))
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarInformeAcciones", _
                                                             Me.ToString(), "ExportarInformeAcciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Realiza la exportación  del avance de procesamiento de valoración a Excel.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0008
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Sub ExportarInformeRentaFija()
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR_ACCIONES_RENTAFIJA
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaRentaFija As String = String.Empty

            If Not ValidarDatos() Then Exit Sub

            dcProxyUtil = inicializarProxyUtilidadesOYD()

            If (_dtmFechaRentaFija.Value.Day < 10) Then
                dia = "0" + _dtmFechaRentaFija.Value.Day.ToString
            Else
                dia = _dtmFechaRentaFija.Value.Day.ToString
            End If

            If (_dtmFechaRentaFija.Value.Month < 10) Then
                mes = "0" + _dtmFechaRentaFija.Value.Month.ToString
            Else
                mes = _dtmFechaRentaFija.Value.Month.ToString
            End If

            ano = _dtmFechaRentaFija.Value.Year.ToString

            strFechaRentaFija = ano + mes + dia

            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHAPROCESO), strFechaRentaFija)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

            objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO_RENTAFIJA, strParametros, "DerechosPatrimonialesPagos", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeValoracion", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    IsBusy = False
                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objResultado = objRet.Entities.First

                        If objResultado.Exitoso Then
                            Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano & "?date=" & strFechaRentaFija & DateTime.Now.ToString("HH:mm:ss"))
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarInformeRentaFija", _
                                                             Me.ToString(), "ExportarInformeRentaFija", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Realiza la exportación del proceso en el paso 4 a Excel.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0014
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Sub ExportarInformePaso4()
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR_PASO4
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaPaso4 As String = String.Empty

            If Not ValidarDatos() Then Exit Sub

            dcProxyUtil = inicializarProxyUtilidadesOYD()

            If (_dtmFechaGenerar.Value.Day < 10) Then
                dia = "0" + _dtmFechaGenerar.Value.Day.ToString
            Else
                dia = _dtmFechaGenerar.Value.Day.ToString
            End If

            If (_dtmFechaGenerar.Value.Month < 10) Then
                mes = "0" + _dtmFechaGenerar.Value.Month.ToString
            Else
                mes = _dtmFechaGenerar.Value.Month.ToString
            End If

            ano = _dtmFechaGenerar.Value.Year.ToString

            strFechaPaso4 = ano + mes + dia

            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHAPROCESO), strFechaPaso4)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPOCLIENTE), strTipoCliente)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.COMITENTE), lngIDComitente)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPORESULTADO), strTipoResultado)
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.RAZONABILIDAD), CType(dblRazonabilidad, String))
            strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

            objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO_PASO4, strParametros, "DerechosPatrimonialesResultado", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeAcciones", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    IsBusy = False
                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objResultado = objRet.Entities.First

                        If objResultado.Exitoso Then
                            Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano & "?date=" & strFechaPaso4 & DateTime.Now.ToString("HH:mm:ss"))
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarInformePaso4", _
                                                             Me.ToString(), "ExportarInformePaso4", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:  CP0003, CP0004, CP0005
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Function ValidarAcciones() As Task
        Try
            Dim objRet As InvokeOperation(Of String)

            If ValidarDatos() Then

                IsBusy = True

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If

                objRet = Await mdcProxy.DerechosPatrimoniales_ValidarSync(dtmFechaAcciones, Program.Usuario, Program.HashConexion).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        strMensaje = objRet.Value

                        If strMensaje <> String.Empty Then
                            If strMensaje.Contains("MENSAJE_SI_NO") Then

                                strMensaje = Replace(strMensaje, "MENSAJE_SI_NO", "")
                                A2Utilidades.Mensajes.mostrarMensajePregunta(strMensaje, Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConsultarAcciones, False)

                            ElseIf strMensaje.Contains("MENSAJE_DETIENE_PROCESO") Then

                                strMensaje = Replace(strMensaje, "MENSAJE_DETIENE_PROCESO", "")
                                A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)

                            End If
                        Else
                            Dim objProcesar As InvokeOperation(Of String)

                            If mdcProxy Is Nothing Then
                                mdcProxy = inicializarProxyCalculosFinancieros()
                            End If

                            objProcesar = Await mdcProxy.DerechosPatrimoniales_Acciones_ProcesarSync(dtmFechaAcciones, Program.Usuario, Program.HashConexion).AsTask

                            If Not objProcesar Is Nothing Then
                                If objProcesar.HasError Then
                                    If objProcesar.Error Is Nothing Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al proceso acciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                                    End If

                                    objProcesar.MarkErrorAsHandled()
                                Else
                                    strMensaje = objProcesar.Value

                                    If strMensaje <> String.Empty Then
                                        A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                    End If

                                End If
                            Else
                                strMensaje = Nothing
                            End If
                        End If
                    End If
                Else
                    strMensaje = Nothing
                End If

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al validar el proceso.", Me.ToString(), "ValidarAcciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Ejecuta la consulta de acciones si el usuario presiona el botón "Calcular exdividendo" en el paso 1.
    ''' </summary>
    Public Async Sub ConsultarAcciones(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objRet As InvokeOperation(Of String)

            'Cuando presiona el botón SI
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                If Not IsBusy Then
                    IsBusy = True
                End If

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If

                objRet = Await mdcProxy.DerechosPatrimoniales_Acciones_ProcesarSync(dtmFechaAcciones, Program.Usuario, Program.HashConexion).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema en el consultar datos de acciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        strMensaje = objRet.Value

                        If strMensaje <> String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If

                    End If
                Else
                    strMensaje = Nothing
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método ConsultarAcciones", Me.ToString(), "ConsultarAcciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta la consulta de renta fija si el usuario presiona el botón "Calcular pagos" en el paso 2.
    ''' </summary>
    Public Async Sub ConsultarRentaFija()
        Try
            Dim objRet As InvokeOperation(Of String)

            If ValidarDatos() Then

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If

                IsBusy = True

                objRet = Await mdcProxy.DerechosPatrimoniales_RentaFija_ProcesarSync(dtmFechaRentaFija, Program.Usuario, Program.HashConexion).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema en el consultar datos de renta fija.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        strMensaje = objRet.Value

                        If strMensaje <> String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If

                    End If
                Else
                    strMensaje = Nothing
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método ConsultarRentaFija", Me.ToString(), "ConsultarRentaFija", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta la consulta de renta fija si el usuario presiona el botón "Calcular pagos" en el paso 2.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0012
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Async Sub ConsultarCarga()
        Try
            Dim objRet As InvokeOperation(Of String)

            If ValidarDatos() Then

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If

                IsBusy = True

                objRet = Await mdcProxy.DerechosPatrimoniales_Carga_ConsultarSync(dtmFechaRentaFija, "", Program.Usuario, Program.HashConexion).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema en el consultar la carga de datos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        strMensaje = objRet.Value

                        If strMensaje <> String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If

                    End If
                Else
                    strMensaje = Nothing
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método ConsultarCarga", Me.ToString(), "ConsultarCarga", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta los pagos si el usuario presiona el botón "Pagar" en el paso 4.
    ''' </summary>
    Public Async Sub Pagar()
        Try
            Dim objRet As InvokeOperation(Of String)

            If ValidarDatos() Then

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If

                IsBusy = True

                objRet = Await mdcProxy.DerechosPatrimoniales_PagarSync(dtmFechaGenerar, strTipoCliente, lngIDComitente, strTipoResultado, dblRazonabilidad, Program.Usuario, Program.HashConexion).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema en el procesar pagos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        strMensaje = objRet.Value

                        If strMensaje <> String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If

                    End If
                Else
                    strMensaje = Nothing
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método Pagar", Me.ToString(), "Pagar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:  CP0002, CP0007, CP0013
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Septiembre/2015
    ''' </history> 
    Public Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            'Valida la fecha de ex - dividendo
            If IsNothing(dtmFechaAcciones) And TabSeleccionado = 0 Then
                strMsg = String.Format("{0}{1} + La fecha de ex - dividendo es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not IsNothing(dtmFechaAcciones) Then
                If dtmFechaAcciones.Value.Date > Now.Date And TabSeleccionado = 0 Then
                    strMsg = String.Format("{0}{1} + La fecha de ex - dividendo no puede ser mayor a la fecha actual.", strMsg, vbCrLf)
                End If
            End If

            'Valida la fecha de pago
            If IsNothing(dtmFechaRentaFija) And (TabSeleccionado = 1 Or TabSeleccionado = 2) Then
                strMsg = String.Format("{0}{1} + La fecha de pago es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not IsNothing(dtmFechaRentaFija) Then
                If dtmFechaRentaFija.Value.Date > Now.Date And (TabSeleccionado = 1 Or TabSeleccionado = 2) Then
                    strMsg = String.Format("{0}{1} + La fecha de pago no puede ser mayor a la fecha actual.", strMsg, vbCrLf)
                End If
            End If

            'Valida la fecha 
            If IsNothing(dtmFechaGenerar) And TabSeleccionado = 3 Then
                strMsg = String.Format("{0}{1} + La fecha es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not IsNothing(dtmFechaGenerar) Then
                If dtmFechaGenerar.Value.Date > Now.Date And TabSeleccionado = 3 Then
                    strMsg = String.Format("{0}{1} + La fecha no puede ser mayor a la fecha actual.", strMsg, vbCrLf)
                End If
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                IsBusy = False
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Sub ConsultarValoresPorDefecto()
        Try
            dtmFechaAcciones = Date.Now()
            dtmFechaRentaFija = Date.Now()
            dtmFechaGenerar = Date.Now()
            lngIDComitente = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarValoresPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

#End Region

End Class


