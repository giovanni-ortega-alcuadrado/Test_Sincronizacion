Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports A2.OyD.OYDServer.RIA.Web

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_MAESTROS As String = "URLServicioPLUSMaestros"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS As String = "URLServicioPLUSUtilidades"
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES As String = "URLServicioImportaciones"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const VERSION_APLICACION_CLIENTE As String = "VersionAplicacionCliente"
    Friend Const KEY_TIPOCHEQUE_CHEQUE As String = "TipoCheque_Cheque"
    Friend Const KEY_TIPOCHEQUE_CHEQUEGERENCIA As String = "TipoCheque_Gerencia"
    Friend Const KEY_FORMAPAGO_CHEQUE As String = "FormaPago_Cheque"
    Friend Shared KEY_HORAINICIO_HORARIO As String = "HoraInicioHorario"
    Friend Shared KEY_HORAFIN_HORARIO As String = "HoraFinHorario"
    Friend Shared KEY_MODULO_ORDENES As String = "ModuloOrdenes"
    Friend Shared KEY_MODULO_ORDENESTESORERIA As String = "ModuloOrdenesTesoreria"
    Friend Const EXPRESIONEMAIL As String = "ExpresionEmail"

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

    Public Shared ReadOnly Property RutaServicioUtilidadesOYDPLUS() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTIL_OYDPLUS))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_MAESTROS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_MAESTROS))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_MAESTROS))
            End If
        End Get
    End Property
    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de negocio para un módulo específico del sistema
    ''' </summary>
    ''' 
    Friend Shared ReadOnly Property RutaServicioPLUSMaestros(ByVal pstrTipoServicio As String) As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrTipoServicio) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrTipoServicio).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(pstrTipoServicio))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(pstrTipoServicio))
            End If

        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioImportaciones() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES))
            End If
        End Get
    End Property

    'Private _mobjInfoAuditoria As Auditoria = New A2.OyD.OYDServer.RIA.Web.Auditoria
    'Public ReadOnly Property InfoAuditoria As Auditoria
    '    Get
    '        _mobjInfoAuditoria.Usuario = Program.Usuario
    '        _mobjInfoAuditoria.Maquina = Program.Maquina
    '        _mobjInfoAuditoria.DirIPMaquina = Program.DireccionIP
    '        _mobjInfoAuditoria.Browser = Program.Browser
    '        _mobjInfoAuditoria.ErrorPersonalizado = 170
    '        Return (_mobjInfoAuditoria)
    '    End Get
    'End Property

    ' Indica la versión o cliente de la aplicación para determinar que opcioines cambian en las pantallas
    Public Shared ReadOnly Property VersionAplicacionCliente() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(VERSION_APLICACION_CLIENTE) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(VERSION_APLICACION_CLIENTE).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            Return Program.UsuarioSinDomino
        End Get
    End Property

    Public Shared ReadOnly Property TipoCheque_Cheque() As String
        Get
            Return RetornarClave(KEY_TIPOCHEQUE_CHEQUE)
        End Get
    End Property

    Public Shared ReadOnly Property TipoCheque_ChequeGerencia() As String
        Get
            Return RetornarClave(KEY_TIPOCHEQUE_CHEQUEGERENCIA)
        End Get
    End Property

    Public Shared ReadOnly Property FormaPago_Cheque() As String
        Get
            Return RetornarClave(KEY_FORMAPAGO_CHEQUE)
        End Get
    End Property

    Public Shared ReadOnly Property HoraInicio_Horario() As String
        Get
            Return RetornarClave(KEY_HORAINICIO_HORARIO)
        End Get
    End Property

    Public Shared ReadOnly Property HoraFin_Horario() As String
        Get
            Return RetornarClave(KEY_HORAFIN_HORARIO)
        End Get
    End Property

    Public Shared ReadOnly Property Modulo_Ordenes() As String
        Get
            Return RetornarClave(KEY_MODULO_ORDENES)
        End Get
    End Property

    Public Shared ReadOnly Property Modulo_OrdenesTesoreria() As String
        Get
            Return RetornarClave(KEY_MODULO_ORDENESTESORERIA)
        End Get
    End Property


    Public Shared ReadOnly Property RetornarParametroApp(ByVal strParametro As String) As String
        Get
            Return RetornarClave(strParametro)
        End Get
    End Property

    Public Shared Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

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

    'Expresion regular correo electronico JBT20140507
    Public Shared ReadOnly Property ExpresionRegularEmail() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(EXPRESIONEMAIL) Then
                    If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(EXPRESIONEMAIL).ToString() = String.Empty Then
                        'Expresion regular por defecto JBT20140507
                        Return ("^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
                    Else
                        'se realiza el replace debido a que en el web.config cuando un value contiene un valor(,) lo interpreta como dos parametros independientes por eso en la expresion regular se colaca un (#) cuando es (,) y luego se reemplaza.
                        Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(EXPRESIONEMAIL).ToString().Replace("#", ",")
                    End If
                Else
                    'Expresion regular por defecto JBT20140507
                    Return ("^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
                End If
            Else
                'Expresion regular por defecto JBT20140507
                Return ("^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
            End If
        End Get
    End Property
    Friend Enum TipoServicio
        URLServicioPLUSMaestros
    End Enum

    ''' <summary>
    ''' Valdar si un string enviado en el primer parámetro es nothing o vacio y retornar el valor enviado en el segundo parámetro
    ''' </summary>
    ''' 
    Friend Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
        Return (A2Ctl.FuncionesCompartidas.validarValorString(pstrValor, pstrRetornoNothingOVacio))
    End Function

    ''' <summary>
    ''' Mensaje que se presenta al usuario cuando da clic para ejecutar alguna acción y aún está pendiente algún proceso asincrónico
    ''' </summary>
    '''
    Friend Shared ReadOnly Property MensajeEsperaOperacion() As String
        Get
            Return (A2Ctl.FuncionesCompartidas.MensajeEsperaOperacion())
        End Get
    End Property

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

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As OyDPLUSMaestrosDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.Mensajes.HasChanges Then
                For Each li In pobjProxy.Mensajes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Reglas.HasChanges Then
                For Each li In pobjProxy.Reglas
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.BancosNacionalesFondos.HasChanges Then
                For Each li In pobjProxy.BancosNacionalesFondos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.AliasEspecies.HasChanges Then
                For Each li In pobjProxy.AliasEspecies
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.MensajesReglas.HasChanges Then
                For Each li In pobjProxy.MensajesReglas
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.CertificacionXTipoNegocios.HasChanges Then
                For Each li In pobjProxy.CertificacionXTipoNegocios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.DocumentoXTipoNegocios.HasChanges Then
                For Each li In pobjProxy.DocumentoXTipoNegocios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.TipoNegocioXEspecies.HasChanges Then
                For Each li In pobjProxy.TipoNegocioXEspecies
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.TipoNegocioXTipoProductos.HasChanges Then
                For Each li In pobjProxy.TipoNegocioXTipoProductos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.DistribucionComisionXTipoNegocios.HasChanges Then
                For Each li In pobjProxy.DistribucionComisionXTipoNegocios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.CertificacionesXReceptos.HasChanges Then
                For Each li In pobjProxy.CertificacionesXReceptos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ConceptosXReceptos.HasChanges Then
                For Each li In pobjProxy.ConceptosXReceptos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ConfiguracionesAdicionalesReceptos.HasChanges Then
                For Each li In pobjProxy.ConfiguracionesAdicionalesReceptos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ParametrosReceptos.HasChanges Then
                For Each li In pobjProxy.ParametrosReceptos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ReceptoresClientesAutorizados.HasChanges Then
                For Each li In pobjProxy.ReceptoresClientesAutorizados
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.TipoNegocioXReceptos.HasChanges Then
                For Each li In pobjProxy.TipoNegocioXReceptos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.TipoProductoXReceptos.HasChanges Then
                For Each li In pobjProxy.TipoProductoXReceptos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.RN_ConfiguracionNivelAtribucios.HasChanges Then
                For Each li In pobjProxy.RN_ConfiguracionNivelAtribucios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Costos.HasChanges Then
                For Each li In pobjProxy.Costos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.TipoProductoXEspecis.HasChanges Then
                For Each li In pobjProxy.TipoProductoXEspecis
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ControlHorarios.HasChanges Then
                For Each li In pobjProxy.ControlHorarios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesRelacionados.HasChanges Then
                For Each li In pobjProxy.ClientesRelacionados
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.PortalioClienteXTipoNegocioPrincipals.HasChanges Then
                For Each li In pobjProxy.PortalioClienteXTipoNegocioPrincipals
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.PreciosTicks.HasChanges Then
                For Each li In pobjProxy.PreciosTicks
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.CupoReceptorXTipoNegocios.HasChanges Then
                For Each li In pobjProxy.CupoReceptorXTipoNegocios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.PortalioClienteXTipoNegocios.HasChanges Then
                For Each li In pobjProxy.PortalioClienteXTipoNegocios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
        End If
    End Sub

End Class
