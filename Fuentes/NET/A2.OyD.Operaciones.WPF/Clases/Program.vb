Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Friend Class Program
    Inherits A2Utilidades.Program

#Region "Constantes públicas específicas"
    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const HABILITAR_RECARGA As String = "HabilitarRecargaAutomaticaProcesarPortafolio"
    Friend Const LAPSO_RECARGA As String = "LapsoRecargaAutomaticaProcesarPortafolio"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"

    Friend Enum TipoServicio
        'URLServicioMaestrosCalculosFinancieros
        URLServicioOperaciones
    End Enum

#End Region

#Region "Métodos públicos genéricos"
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

#Region "ValidarFechaCierre"

    Public Shared Async Function ValidarFechaCierre(ByVal pstrModulo As String, ByVal pstrAccionCliente As String, ByVal pstrDescripcionModulo As String, ByVal pdtmFechaAValidar As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, Optional ByVal plogMostrarMensajeUsuario As Boolean = False) As Task(Of Boolean)

        Dim logResultado As Boolean = True
        Dim dcProxy As UtilidadesDomainContext
        Dim objParametros As LoadOperation(Of OYDUtilidades.tblvalidarFechaCierre) = Nothing


        Try
            ' Si no existe se consulta, si ya existe no se hace nada
            'dcProxy = inicializarProxy()
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New UtilidadesDomainContext()
            Else
                dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidades))
            End If

            objParametros = Await dcProxy.Load(dcProxy.ValidarFechaCierreSyncQuery(pstrModulo, pstrAccionCliente, pstrDescripcionModulo, pdtmFechaAValidar, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objParametros Is Nothing Then
                If objParametros.HasError Then
                    If objParametros.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación pero no se recibió detalle del error.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta para obtener los parámetros de la aplicación.", "clsFuncionesCompartidas", "ValidarFechaCierre", Program.TituloSistema, Program.Maquina, objParametros.Error)
                    End If

                    objParametros.MarkErrorAsHandled()
                Else
                    If dcProxy.tblvalidarFechaCierres.Count > 0 Then
                        logResultado = CBool(dcProxy.tblvalidarFechaCierres.First.exitoso)

                        If logResultado = False Then
                            If plogMostrarMensajeUsuario Then
                                A2Utilidades.Mensajes.mostrarMensaje(dcProxy.tblvalidarFechaCierres.First.mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
            End If ' If Not objParametros Is Nothing Then
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para configurar los parámetros de la aplicación.", "clsFuncionesCompartidas", "ValidarFechaCierre", Program.TituloSistema, Program.Maquina, ex)
        Finally
            dcProxy = Nothing
        End Try

        Return (logResultado)
    End Function

#End Region

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As OperacionesDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.ConsultarCantidads.HasChanges Then
                For Each li In pobjProxy.ConsultarCantidads
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.LiquidacionesConsultars.HasChanges Then
                For Each li In pobjProxy.LiquidacionesConsultars
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.LiquidacionesOrdenes.HasChanges Then
                For Each li In pobjProxy.LiquidacionesOrdenes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ModificacionValidars.HasChanges Then
                For Each li In pobjProxy.ModificacionValidars
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Operaciones.HasChanges Then
                For Each li In pobjProxy.Operaciones
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OperacionesAplazamientos.HasChanges Then
                For Each li In pobjProxy.OperacionesAplazamientos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OperacionesBeneficiarios.HasChanges Then
                For Each li In pobjProxy.OperacionesBeneficiarios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OperacionesCustodias.HasChanges Then
                For Each li In pobjProxy.OperacionesCustodias
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OperacionesEspecies.HasChanges Then
                For Each li In pobjProxy.OperacionesEspecies
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OperacionesReceptores.HasChanges Then
                For Each li In pobjProxy.OperacionesReceptores
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
