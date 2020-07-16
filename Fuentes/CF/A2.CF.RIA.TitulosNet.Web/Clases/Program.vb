Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Friend Class Program
    Inherits A2Utilidades.Program

#Region "Constantes públicas específicas"
    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS As String = "URLServicioCFCalculosFinancieros"
    Friend Const NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE As String = "URLServicioCFCodificacionContable"
    Friend Const NOMBRE_PARAM_RUTA_SERV_ESPECIES As String = "URLServicioCFEspecies"
    Friend Const NOMBRE_PARAM_RUTA_SERV_MAESTROS As String = "URLServicioCFMaestros"
    Friend Const NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO As String = "URLServicioCFPortafolio"
    Friend Const NOMBRE_PARAM_RUTA_SERV_TITULOSNET As String = "URLServicioCFTitulosNet"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF As String = "URLServicioCFUtilidades"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES As String = "URLServicioImportaciones"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const HABILITAR_RECARGA As String = "HabilitarRecargaAutomaticaProcesarPortafolio"
    Friend Const LAPSO_RECARGA As String = "LapsoRecargaAutomaticaProcesarPortafolio"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"

    Friend Enum TipoServicio
        URLServicioCFCalculosFinancieros
        URLServicioCFCodificacionContable
        URLServicioCFEspecies
        URLServicioCFMaestros
        URLServicioCFPortafolio
        URLServicioCFTitulosNet
        URLServicioCFUtilidades
        URLServicioUtilidadesOYD
        URLServicioImportaciones
    End Enum

#End Region

#Region "Métodos públicos genéricos"

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            If Program.UsaUsuarioSinDomino Then
                Return Program.UsuarioSinDomino
            Else
                Return A2Utilidades.Program.Usuario
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioCalculosFinancieros() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioCodificacionContable() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioEspecies() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_ESPECIES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_ESPECIES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioMaestros() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_MAESTROS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioPortafolio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioTitulosNET() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_TITULOSNET) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_TITULOSNET).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioUtilidadesCF() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF).ToString()
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
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioImportaciones() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    ''' <summary>
    ''' Valdar si un string enviado en el primer parámetro es nothing o vacio y retornar el valor enviado en el segundo parámetro
    ''' </summary>
    ''' 
    Friend Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
        Return (A2Ctl.FuncionesCompartidas.validarValorString(pstrValor, pstrRetornoNothingOVacio))
    End Function

    Friend Shared Sub CopiarObjeto(Of T)(pobjOrigen As T, pobjDestino As T)
        If IsNothing(pobjDestino) Then
            'Throws a new exception. 
            Throw New System.Exception("Debe inicializar el objeto de destino")
            Exit Sub
        End If
        A2Ctl.FuncionesCompartidas.CopyObject(Of T)(pobjOrigen, pobjDestino)
    End Sub

    Friend Shared ReadOnly Property obtenerMensajeValidacion(ByVal pstrMsg As String, ByVal pstrAccion As String, ByVal plogError As Boolean) As String
        Get
            Return (A2Ctl.FuncionesCompartidas.obtenerMensajeValidacion(pstrMsg, pstrAccion, plogError))
        End Get
    End Property

#End Region

#Region "Propiedades públicas genéricos"

    ''' <summary>
    ''' Verificar si se debe o no mostrar un mensaje (popup) que permite hacer seguimiento al ejecutar la aplicación.
    ''' </summary>
    ''' 
    Public Shared ReadOnly Property mostrarMensaje() As Boolean
        Get
            Return (A2Ctl.FuncionesCompartidas.mostrarMensaje())
        End Get
    End Property

    ''' <summary>
    ''' Nombre del recurso que se crea a nivel de aplicación para guardar los datos de los combos genéricos
    ''' </summary>
    ''' 
    Friend Shared ReadOnly Property NombreListaCombos() As String
        Get
            Return (A2Ctl.FuncionesCompartidas.NOMBRE_LISTA_COMBOS_APP)
        End Get
    End Property

    ''' <summary>
    ''' Nombre del recurso que se crea a nivel de aplicación para guardar los datos de la lista de combos especificos
    ''' </summary>
    ''' 
    Friend Shared ReadOnly Property NombreListaCombosEspecificos() As String
        Get
            Return (A2Ctl.FuncionesCompartidas.NOMBRE_LISTA_COMBOSESPECIFICOS_APP)
        End Get
    End Property

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de negocio para un módulo específico del sistema
    ''' </summary>
    ''' 
    Friend Shared ReadOnly Property RutaServicioNegocio(ByVal pstrTipoServicio As String) As String
        Get
            Return (A2Ctl.FuncionesCompartidas.RutaServicioNegocio(pstrTipoServicio))
        End Get
    End Property

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de utilidades generales del aplicativo
    ''' </summary>
    '''
    Friend Shared ReadOnly Property RutaServicioUtilidades() As String
        Get
            Return (A2Ctl.FuncionesCompartidas.RutaServicioUtilidades())
        End Get
    End Property

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (A2Ctl.FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    ''' <summary>
    ''' Mensaje que se presenta al usuario cuando da clic para ejecutar alguna acción y aún está pendiente algún proceso asincrónico
    ''' </summary>
    '''
    Friend Shared ReadOnly Property MensajeEsperaOperacion() As String
        Get
            Return (A2Ctl.FuncionesCompartidas.MensajeEsperaOperacion())
        End Get
    End Property


    Public Shared ReadOnly Property Recarga_Automatica_Activa() As Boolean
        Get
            Return CBool(IIf(RetornarClave(HABILITAR_RECARGA) = "1", True, False))
        End Get
    End Property


    Public Shared ReadOnly Property Par_lapso_recarga() As Integer
        Get
            Return Integer.Parse(RetornarClave(LAPSO_RECARGA))
        End Get
    End Property



#End Region

#Region "Funciones para retornar clave"

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
#End Region

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As TitulosNetDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.ConceptoRetencions.HasChanges Then
                For Each li In pobjProxy.ConceptoRetencions
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
        End If
    End Sub

End Class
