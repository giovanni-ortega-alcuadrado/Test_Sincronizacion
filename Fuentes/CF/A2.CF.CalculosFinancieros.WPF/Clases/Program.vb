﻿Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports System.Globalization
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
    Friend Const NOMBRE_PARAM_RUTA_SERV_PROCESARPORTAFOLIOS As String = "URLServicioCFProcesarPortafolios"
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
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    Friend Const NOMBRE_ETIQUETA_COMITENTE As String = "NombreEtiquetaComitente"
    Friend Const STR_DESCRPCION_PASO_1 As String = "DerechosPatrimonialesDescripcionPaso1"
    Friend Const STR_DESCRPCION_PASO_2 As String = "DerechosPatrimonialesDescripcionPaso2"
    Friend Const STR_DESCRPCION_PASO_3 As String = "DerechosPatrimonialesDescripcionPaso3"
    Friend Const STR_DESCRPCION_PASO_4 As String = "DerechosPatrimonialesDescripcionPaso4"
    Friend Const NOMBRE_LISTA_COMBOSRecurso As String = "DiccionarioCombos"
    Friend Const ValorMaximoRangoAcumuladoComisiones As Double = 999999999999999


    Friend Enum TipoServicio
        URLServicioCFCalculosFinancieros
        URLServicioCFProcesarPortafolios
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

    Public Shared ReadOnly Property RutaServicioProcesarPortafolio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_PROCESARPORTAFOLIOS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_PROCESARPORTAFOLIOS).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
        End Get
    End Property

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

    Public Shared ReadOnly Property STR_NOMBRE_ETIQUETA_COMITENTE() As String
        Get
            Return RetornarClave(NOMBRE_ETIQUETA_COMITENTE)
        End Get
    End Property

    Public Shared ReadOnly Property DESCRPCION_PASO_1() As String
        Get
            Return RetornarClave(STR_DESCRPCION_PASO_1)
        End Get
    End Property

    Public Shared ReadOnly Property DESCRPCION_PASO_2() As String
        Get
            Return RetornarClave(STR_DESCRPCION_PASO_2)
        End Get
    End Property

    Public Shared ReadOnly Property DESCRPCION_PASO_3() As String
        Get
            Return RetornarClave(STR_DESCRPCION_PASO_3)
        End Get
    End Property

    Public Shared ReadOnly Property DESCRPCION_PASO_4() As String
        Get
            Return RetornarClave(STR_DESCRPCION_PASO_4)
        End Get
    End Property

#End Region

#Region "Funciones para retornar clave"

    Public Shared ReadOnly Property RetornarParametroApp(ByVal strParametro As String) As String
        Get
            Return RetornarClave(strParametro)
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
#End Region

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

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As CalculosFinancierosDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.PreciosEspecies.HasChanges Then
                For Each li In pobjProxy.PreciosEspecies
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Indicadores.HasChanges Then
                For Each li In pobjProxy.Indicadores
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.OperacionInterbancarias.HasChanges Then
                For Each li In pobjProxy.OperacionInterbancarias
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.MovimientosParticipacionesFondos.HasChanges Then
                For Each li In pobjProxy.MovimientosParticipacionesFondos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Companias.HasChanges Then
                For Each li In pobjProxy.Companias
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.MvtoCustodiasAplicados.HasChanges Then
                For Each li In pobjProxy.MvtoCustodiasAplicados
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ConfiguracionArbitrajes.HasChanges Then
                For Each li In pobjProxy.ConfiguracionArbitrajes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.Arbitrajes.HasChanges Then
                For Each li In pobjProxy.Arbitrajes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ArbitrajesDetalles.HasChanges Then
                For Each li In pobjProxy.ArbitrajesDetalles
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If

            If pobjProxy.ControlLiquidezOperaciones.HasChanges Then
                For Each li In pobjProxy.ControlLiquidezOperaciones
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If

            If pobjProxy.ParesDeDivisas.HasChanges Then
                For Each li In pobjProxy.ParesDeDivisas
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.strUsuarioConexion = Program.Usuario
                        li.strInfoConexion = Program.HashConexion
                    End If
                Next
            End If
        End If
    End Sub

End Class
