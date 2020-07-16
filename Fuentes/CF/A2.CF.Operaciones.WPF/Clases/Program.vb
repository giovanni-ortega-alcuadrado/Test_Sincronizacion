Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports System.Globalization
Imports A2.OyD.OYDServer.RIA.Web

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

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
    Friend Const NOMBRE_PARAM_RUTA_SERV_OPERACIONES_CF As String = "URLServicioCFOperaciones"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACIONES As String = "URLServicioImportaciones"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const REPORTE_ENTREGA_CUSTODIAS As String = "A2VReporteEntregaCustodias"
    Friend Const REPORTE_ENTREGA_CUSTODIAS_LIN As String = "A2VReporteEntregaCustodiasLin"
    Friend Const SERVICIO_REPORTE As String = "A2VServicioRS"
    Friend Const SERVICIO_VISOR_REPORTE As String = "A2VServicioParam"
    Friend Const SERVICIO_RUTA_GENERACION As String = "A2RutaFisicaGeneracion"
    Friend Const TIME_OUT_REPORTES_EN_MINUTOS As String = "TimeOutReportesEnMinutos"
    Friend Const HABILITAR_RECARGA As String = "HabilitarRecargaAutomatica"
    Friend Const LAPSO_RECARGA As String = "LapsoRecargaAutomatica"
    Friend Const USUARIO_ACTIVO As String = "A2Consola_UsuarioActivo"

    Friend Enum TipoServicio
        URLServicioCFCalculosFinancieros
        URLServicioCFCodificacionContable
        URLServicioCFEspecies
        URLServicioCFMaestros
        URLServicioCFPortafolio
        URLServicioCFTitulosNet
        URLServicioCFUtilidades
        URLServicioCFOperaciones
        URLServicioUtilidadesOYD
        URLServicioImportaciones
    End Enum

    Public Shared ReadOnly Property RutaServicioCalculosFinancieros() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_CALCULOSFINANCIEROS))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioCodificacionContable() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_CODIFICACIONCONTABLE))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioEspecies() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_ESPECIES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_ESPECIES).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_ESPECIES))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_ESPECIES))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioMaestros() As String
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

    Public Shared ReadOnly Property RutaServicioPortafolio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_PORTAFOLIO))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioTitulosNET() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_TITULOSNET) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_TITULOSNET).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_TITULOSNET))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_TITULOSNET))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioUtilidadesCF() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_CF))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioOperacionesCF() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_OPERACIONES_CF) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_OPERACIONES_CF).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_OPERACIONES_CF))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_OPERACIONES_CF))
            End If
        End Get
    End Property

    Public Shared ReadOnly Property RutaServicioUtilidadesOYD() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_UTILIDADES_OYD))
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

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            If Program.UsaUsuarioSinDomino Then
                Return Program.UsuarioSinDomino
            Else
                Return A2Utilidades.Program.Usuario
            End If
        End Get
    End Property

    Public Shared ReadOnly Property DatosUsuario_SeguridadIntegrada As Boolean
        Get
            If Application.Current.Resources.Contains(USUARIO_ACTIVO) Then
                Dim objUsuario As A2Utilidades.Usuario = CType(Application.Current.Resources(USUARIO_ACTIVO), A2Utilidades.Usuario)
                Return objUsuario.SeguridadIntegrada
            Else
                Return True
            End If
        End Get
    End Property

    Public Shared ReadOnly Property DatosUsuario_PasswordUsuario As String
        Get
            If Application.Current.Resources.Contains(USUARIO_ACTIVO) Then
                Dim objUsuario As A2Utilidades.Usuario = CType(Application.Current.Resources(USUARIO_ACTIVO), A2Utilidades.Usuario)
                Return objUsuario.Password
            Else
                Return String.Empty
            End If
        End Get
    End Property

    Public Shared ReadOnly Property DatosUsuario_Usuario As String
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

    Friend Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
        Return (A2Ctl.FuncionesCompartidas.validarValorString(pstrValor, pstrRetornoNothingOVacio))
    End Function

    Public Shared ReadOnly Property Consultar_TIME_OUT_REPORTES_EN_MINUTOS() As String
        Get
            Return RetornarClave(TIME_OUT_REPORTES_EN_MINUTOS)
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

    Public Shared Function CifrarTexto(ByVal pstrTexto As String) As String
        Return A2Utilidades.CifrarSL.cifrar(pstrTexto)
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

    Public Shared ReadOnly Property Recarga_Automatica_Activa() As Boolean
        Get
            Return IIf(RetornarClave(HABILITAR_RECARGA) = "1", True, False)
        End Get
    End Property

    Public Shared ReadOnly Property Par_lapso_recarga() As Integer
        Get
            Return Integer.Parse(RetornarClave(LAPSO_RECARGA))
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

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As OperacionesCFDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.HomologacionTipoOrigens.HasChanges Then
                For Each li In pobjProxy.HomologacionTipoOrigens
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
