Imports Telerik.Windows.Controls
Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports A2Utilidades.Recursos
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Net
Imports System.ServiceModel
Imports System.Net.Security

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Public Class Program
    Inherits A2Utilidades.Program

    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_PARAM_RUTA_SERV_IMPORTACION As String = "URLServicioImportaciones"
    Friend Const NOMBRE_PARAM_RUTA_SERV_CLIENTES As String = "URLServicioClientes"
    Friend Const NOMBRE_PARAM_RUTA_SERV_UTIL_OYD As String = "URLServicioUtilidadesOYD"
    Friend Const NOMBRE_LISTA_COMBOS As String = "ListaCombosOYD"
    Friend Const NOMBRE_DICCIONARIO_COMBOS As String = "DiccionarioCombosA2"
    Friend Const NOMBRE_PARAM_RUTA_SERV_MAESTROS As String = "URLServicioMaestros"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS As String = "DiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"
    'Descripción:   Desarollo para adjuntar documentos del cliente en físico. Caso 5133
    'Responsable:   Jorge Peña (Alcuadrado S.A.)
    'Fecha:         18 de octubre 2013
    Friend Const NOMBRE_PARAM_RUTA_SERV_DOCUMENTOS As String = "URLServicioDocumentos"
    Friend Const NOMBRE_PARAM_CODAPLICACION_SERV_DOCUMENTOS As String = "CodAplicacionServicioDocumentos"
    Friend Const NOMBRE_PARAM_CODCATEGORIA_SERV_DOCUMENTOS As String = "CodCategoriaServicioDocumentos"
    Friend Const NOMBRE_PARAM_CODPERMISO_CARGAR_SERV_DOCUMENTOS As String = "CodPermisoCargarServicioDocumentos"
    Friend Const NOMBRE_PARAM_CODPERMISO_ELIMINAR_SERV_DOCUMENTOS As String = "CodPermisoEliminarServicioDocumentos"
    Friend Const NOMBRE_PARAM_CODPERMISO_VER_SERV_DOCUMENTOS As String = "CodPermisoVerServicioDocumentos"
    Friend Const DESCRIPCIONPAIS As String = "FatcaDescripcionPais"
    Friend Const EXPRESIONEMAIL As String = "ExpresionEmail"

    Friend Const NOMBRE_PARAM_RUTA_CARGA_ARCHIVO As String = "A2RutaFisicaGeneracion"


    Public Shared ReadOnly Property RutafisicaArchivo() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_CARGA_ARCHIVO) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_CARGA_ARCHIVO).ToString()
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


    Public Shared ReadOnly Property RutaServicioNegocio() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_CLIENTES) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_CLIENTES).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_CLIENTES))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_CLIENTES))
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
    Public Shared ReadOnly Property RutaServicioImportaciones() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(NOMBRE_PARAM_RUTA_SERV_IMPORTACION) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(NOMBRE_PARAM_RUTA_SERV_IMPORTACION).ToString()
                Else
                    Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_IMPORTACION))
                End If
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(NOMBRE_PARAM_RUTA_SERV_IMPORTACION))
            End If
        End Get
    End Property

    'Descripción:   Desarollo para adjuntar documentos del cliente en físico. Caso 5133
    'Responsable:   Jorge Peña (Alcuadrado S.A.)
    'Fecha:         18 de octubre 2013
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
    ' Indica las posibles opciones para la descripcion delnombre de estados unidos JBT20140403
    Public Shared ReadOnly Property FatcaDescripcionPais() As String
        Get
            If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
                If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(DESCRIPCIONPAIS) Then
                    Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(DESCRIPCIONPAIS).ToString()
                Else
                    Return ("")
                End If
            Else
                Return ("")
            End If
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
                        'se realiza el replace debido a que en el web.config cuando un value contiene un valor() lo interpreta como dos parametros independientes por eso en la expresion regular se colaca un (#) cuando es () y luego se reemplaza.
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
    Public Shared ReadOnly Property doc_url_servicio_documentos() As String
        Get
            Return RetornarClave(NOMBRE_PARAM_RUTA_SERV_DOCUMENTOS)
        End Get
    End Property

    Public Shared ReadOnly Property doc_aplicacion_servicio_documentos() As String
        Get
            Return RetornarClave(NOMBRE_PARAM_CODAPLICACION_SERV_DOCUMENTOS)
        End Get
    End Property

    Public Shared ReadOnly Property doc_categoria_servicio_documentos() As String
        Get
            Return RetornarClave(NOMBRE_PARAM_CODCATEGORIA_SERV_DOCUMENTOS)
        End Get
    End Property


    Public Shared ReadOnly Property doc_permiso_cargar_servicio_documentos() As String
        Get
            Return RetornarClave(NOMBRE_PARAM_CODPERMISO_CARGAR_SERV_DOCUMENTOS)
        End Get
    End Property

    Public Shared ReadOnly Property doc_permiso_eliminar_servicio_documentos() As String
        Get
            Return RetornarClave(NOMBRE_PARAM_CODPERMISO_ELIMINAR_SERV_DOCUMENTOS)
        End Get
    End Property

    Public Shared ReadOnly Property doc_permiso_ver_servicio_documentos() As String
        Get
            Return RetornarClave(NOMBRE_PARAM_CODPERMISO_VER_SERV_DOCUMENTOS)
        End Get
    End Property

    Private _mobjInfoAuditoria As Auditoria = New A2.OyD.OYDServer.RIA.Web.Auditoria
    Public ReadOnly Property InfoAuditoria As Auditoria
        Get
            _mobjInfoAuditoria.Usuario = Program.Usuario
            _mobjInfoAuditoria.Maquina = Program.Maquina
            _mobjInfoAuditoria.DirIPMaquina = Program.DireccionIP
            _mobjInfoAuditoria.Browser = Program.Browser
            _mobjInfoAuditoria.ErrorPersonalizado = 170
            Return (_mobjInfoAuditoria)
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
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    '''
    Friend Shared ReadOnly Property ejecutarAppSegunAmbiente() As Boolean
        Get
            Return (A2Ctl.FuncionesCompartidas.ejecutarAppSegunAmbiente())
        End Get
    End Property

    Public Shared Sub VerificarCambiosProxyServidor(ByRef pobjProxy As ClientesDomainContext)
        If Not IsNothing(pobjProxy) Then
            If pobjProxy.Clientes.HasChanges Then
                For Each li In pobjProxy.Clientes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.tblConsultaSaldoRes.HasChanges Then
                For Each li In pobjProxy.tblConsultaSaldoRes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.EntidadesClientes.HasChanges Then
                For Each li In pobjProxy.EntidadesClientes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.CuentasClientes.HasChanges Then
                For Each li In pobjProxy.CuentasClientes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesReceptores.HasChanges Then
                For Each li In pobjProxy.ClientesReceptores
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesOrdenantes.HasChanges Then
                For Each li In pobjProxy.ClientesOrdenantes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesBeneficiarios.HasChanges Then
                For Each li In pobjProxy.ClientesBeneficiarios
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesAficiones.HasChanges Then
                For Each li In pobjProxy.ClientesAficiones
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesDeportes.HasChanges Then
                For Each li In pobjProxy.ClientesDeportes
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesDirecciones.HasChanges Then
                For Each li In pobjProxy.ClientesDirecciones
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesDocumentosRequeridos.HasChanges Then
                For Each li In pobjProxy.ClientesDocumentosRequeridos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesLOGHistoriaIRs.HasChanges Then
                For Each li In pobjProxy.ClientesLOGHistoriaIRs
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesConocimientoEspecificos.HasChanges Then
                For Each li In pobjProxy.ClientesConocimientoEspecificos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.TipoClients.HasChanges Then
                For Each li In pobjProxy.TipoClients
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesAccionistas.HasChanges Then
                For Each li In pobjProxy.ClientesAccionistas
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesFichas.HasChanges Then
                For Each li In pobjProxy.ClientesFichas
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesPersonasParaConfirmars.HasChanges Then
                For Each li In pobjProxy.ClientesPersonasParaConfirmars
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesPersonasDepEconomicas.HasChanges Then
                For Each li In pobjProxy.ClientesPersonasDepEconomicas
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesProductos.HasChanges Then
                For Each li In pobjProxy.ClientesProductos
                    If li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Deleted _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.[New] _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Modified _
                        Or li.EntityState = OpenRiaServices.DomainServices.Client.EntityState.Detached Then
                        li.pstrUsuarioConexion = Program.Usuario
                        li.pstrInfoConexion = Program.HashConexion
                    End If
                Next
            End If
            If pobjProxy.ClientesPaisesFATCAs.HasChanges Then
                For Each li In pobjProxy.ClientesPaisesFATCAs
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

    Public Shared Sub ServicioGenerales(ByRef pobjProxy As WSPhoenix.ClientesPhClient, ByVal pstrUrlServicio As String)
        Try
            Dim logEsHttps As Boolean = False
            If Not String.IsNullOrEmpty(pstrUrlServicio) Then
                If Left(pstrUrlServicio, 5).ToLower = "https" Then
                    logEsHttps = True
                End If
            End If

            If logEsHttps Then
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(Function() True)
            End If

            Dim objBinding As BasicHttpBinding = New BasicHttpBinding()
            If logEsHttps Then
                objBinding.Security.Mode = BasicHttpSecurityMode.Transport
            Else
                objBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly
            End If
            objBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm
            objBinding.SendTimeout = New TimeSpan(0, 10, 0)
            objBinding.ReceiveTimeout = New TimeSpan(0, 10, 0)
            objBinding.MaxBufferSize = 2147483647
            objBinding.MaxReceivedMessageSize = 2147483647
            Dim objEndPoint As EndpointAddress = New EndpointAddress(pstrUrlServicio)

            pobjProxy = New WSPhoenix.ClientesPhClient(objBinding, objEndPoint)
            pobjProxy.InnerChannel.OperationTimeout = New TimeSpan(0, 30, 0)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al establecer la comunicación con el proxy.", "Program", "ServicioGenerales", "Clientes", String.Empty, ex)
        End Try
    End Sub

End Class