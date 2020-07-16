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
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTILIDADES As String = "URLServicioUtilidades"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES As String = "URLServicioImportaciones"
    Friend Const TIME_OUT_REPORTES_EN_MINUTOS As String = "TimeOutReportesEnMinutos" 'SLB20140428 Manejo del time out con parámetro.
    Friend Const TIME_OUT_DTS_EN_MINUTOS As String = "TimeOutDTSEnMinutos"  'SLB20140428 Manejo del time out con parámetro.

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

    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTILIDADES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTILIDADES).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTILIDADES))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTILIDADES))
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

    'Private _mobjInfoAuditoria As OyDUtilidades.Auditoria = New A2.OyD.OYDServer.RIA.Web.OyDTesoreria.Auditoria
    'Public ReadOnly Property InfoAuditoria As OyDTesoreria.Auditoria
    '    Get
    '        _mobjInfoAuditoria.Usuario = Program.Usuario
    '        _mobjInfoAuditoria.Maquina = Program.Maquina
    '        _mobjInfoAuditoria.DirIPMaquina = Program.DireccionIP
    '        _mobjInfoAuditoria.Browser = Program.Browser
    '        _mobjInfoAuditoria.ErrorPersonalizado = 170
    '        Return (_mobjInfoAuditoria)
    '    End Get
    'End Property

    Public Shared ReadOnly Property Consultar_TIME_OUT_REPORTES_EN_MINUTOS() As String
        Get
            Return RetornarClave(TIME_OUT_REPORTES_EN_MINUTOS)
        End Get
    End Property

    Public Shared ReadOnly Property Consultar_TIME_OUT_DTS_EN_MINUTOS() As String
        Get
            Return RetornarClave(TIME_OUT_DTS_EN_MINUTOS)
        End Get
    End Property

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
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (A2Ctl.FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As UtilidadesDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.CamposObligatorios.HasChanges Then
                For Each li In pobjProxy.CamposObligatorios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.DTs.HasChanges Then
                For Each li In pobjProxy.DTs
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.AuditoriaTablas.HasChanges Then
                For Each li In pobjProxy.AuditoriaTablas
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
