Imports Telerik.Windows.Controls
'Descripción:   Desarollo para adjuntar documentos del cliente en físico. Caso 5133
'Responsable:   Jorge Peña (Alcuadrado S.A.)
'Fecha:         18 de octubre 2013

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web

Public Class DocumentosRecibidosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxyUtilidades As UtilidadesDomainContext

#Region "Declaraciones"
    Dim objServicioDocumentador As ServicioDocumentador.ServicioDocumentadorClient
    Dim _strURL As String
    Dim _strRutaServicioDoc As String
    Dim _strCodAplicacionServicioDoc As String
    Dim _strCodCategoriaServicioDoc As String
    Dim _strCodPermisoCargarServicioDoc As String
    Dim _strCodPermisoEliminarServicioDoc As String
    Dim _strCodPermisoVerServicioDoc As String

    Dim dcProxy As MaestrosDomainContext

#End Region

#Region "Procedimientos"

    Public Sub New()

        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxyUtilidades = New UtilidadesDomainContext()
            Else
                dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            _strRutaServicioDoc = Program.doc_url_servicio_documentos
            _strCodAplicacionServicioDoc = Program.doc_aplicacion_servicio_documentos
            _strCodCategoriaServicioDoc = Program.doc_categoria_servicio_documentos
            _strCodPermisoCargarServicioDoc = Program.doc_permiso_cargar_servicio_documentos
            _strCodPermisoEliminarServicioDoc = Program.doc_permiso_eliminar_servicio_documentos
            _strCodPermisoVerServicioDoc = Program.doc_permiso_ver_servicio_documentos

            objServicioDocumentador = New ServicioDocumentador.ServicioDocumentadorClient

            objServicioDocumentador.Endpoint.Address = New System.ServiceModel.EndpointAddress(_strRutaServicioDoc)

            


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "DocumentosRecibidosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDocumentos()
        Try
            IsBusy = True
            dcProxy.DoctosRequeridos.Clear()
            dcProxy.Load(dcProxy.DocumentosRecibidosConsultarQuery(_lngIdComitente, strTipoIdentificacion, Program.Usuario, Program.HashConexion), AddressOf TerminaCargarListaDocumentosRecibidos, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema consultando los documentos recibidos", _
                                 Me.ToString(), "ConsultarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub LlamarServicio()
        Try
            IsBusy = True
            Dim objParametrosDocumentador As New ServicioDocumentador.RegistrarIngresoDocumentadorRequest
            objParametrosDocumentador.pstrUsuario = Program.Usuario
            objParametrosDocumentador.pstrCodigoAplicacion = _strCodAplicacionServicioDoc
            objParametrosDocumentador.pstrCodigoCategoria = _strCodCategoriaServicioDoc
            objParametrosDocumentador.pstrTipoDocumento = _DocumentoSeleccionado.IDDocumento
            objParametrosDocumentador.pstrTipoIdCliente = _strTipoIdentificacion
            objParametrosDocumentador.pstrNroDocumentoCliente = _strNroDocumento
            objParametrosDocumentador.pstrCodigoExterno = _lngIdComitente
            objParametrosDocumentador.pstrPermiso = _strCodPermisoCargarServicioDoc & "&" & _strCodPermisoEliminarServicioDoc & "&" & _strCodPermisoVerServicioDoc
            objParametrosDocumentador.pstrCodigoIngreso = ""

            Dim objResponse As ServicioDocumentador.RegistrarIngresoDocumentadorResponse = Await objServicioDocumentador.RegistrarIngresoDocumentadorAsync(objParametrosDocumentador)
            If Not IsNothing(objResponse) Then
                If Not IsNothing(objResponse.RegistrarIngresoDocumentadorResult) Then
                    If objResponse.RegistrarIngresoDocumentadorResult.Count > 0 Then
                        Dim strUrl As String = objResponse.RegistrarIngresoDocumentadorResult.First.ValorRetorno
                        Program.VisorArchivosWeb_DescargarURL(strUrl)
                    End If
                End If
            End If

            'objServicioDocumentador.RegistrarIngresoDocumentadorAsync(Program.Usuario, "OyD", "TES", "COPIAC", _strTipoIdentificacion, _strNroDocumento, _lngIdComitente, "Cargar&Eliminar&Ver", "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema invocando el servicio",
                                 Me.ToString(), "LlamarServicio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosCliente()
        Try
            IsBusy = True

            If Not IsNothing(dcProxyUtilidades.BuscadorClientes) Then
                dcProxyUtilidades.BuscadorClientes.Clear()
            End If

            dcProxyUtilidades.Load(dcProxyUtilidades.buscarClientesQuery(lngIdComitente, "A", String.Empty, "idcomitentelectura", Program.Usuario, False, 0, Program.HashConexion), AddressOf TerminoConsultarDatosCliente, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente.", _
                                 Me.ToString(), "ConsultarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LimpiarDatosCliente()
        Try
            lngIdComitente = String.Empty
            strNombre = String.Empty
            strNroDocumento = String.Empty
            strTipoIdentificacion = String.Empty
            ListaDocumentosRecibidos = New List(Of DoctosRequerido)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos del cliente.", _
                                Me.ToString(), "LimpiarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaDocumentosRecibidos As List(Of DoctosRequerido)
    Public Property ListaDocumentosRecibidos() As List(Of DoctosRequerido)
        Get
            Return _ListaDocumentosRecibidos
        End Get
        Set(ByVal value As List(Of DoctosRequerido))
            _ListaDocumentosRecibidos = value
            MyBase.CambioItem("ListaDocumentosRecibidos")
            MyBase.CambioItem("ListaDocumentosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaDocumentosPaged() As PagedCollectionView
        Get
            If Not IsNothing(ListaDocumentosRecibidos) Then
                Dim view = New PagedCollectionView(ListaDocumentosRecibidos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DocumentoSeleccionado As DoctosRequerido
    Public Property DocumentoSeleccionado() As DoctosRequerido
        Get
            Return _DocumentoSeleccionado
        End Get
        Set(ByVal value As DoctosRequerido)
            _DocumentoSeleccionado = value
            MyBase.CambioItem("DocumentoSeleccionado")
        End Set
    End Property

    Private _lngIdComitente As String
    Public Property lngIdComitente() As String
        Get
            Return _lngIdComitente
        End Get
        Set(ByVal value As String)
            _lngIdComitente = value
            MyBase.CambioItem("lngIdComitente")
        End Set
    End Property

    Private _strTipoIdentificacion As String
    Public Property strTipoIdentificacion() As String
        Get
            Return _strTipoIdentificacion
        End Get
        Set(ByVal value As String)
            _strTipoIdentificacion = value
            MyBase.CambioItem("strTipoIdentificacion")
        End Set
    End Property

    Private _strNroDocumento As String
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            MyBase.CambioItem("strNroDocumento")
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            MyBase.CambioItem("strNombre")
        End Set
    End Property


#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminaCargarListaDocumentosRecibidos(lo As LoadOperation(Of DoctosRequerido))
        Try
            If Not lo.HasError Then
                ListaDocumentosRecibidos = dcProxy.DoctosRequeridos.ToList
                IsBusy = False
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de documentos recibidos", _
                                                 Me.ToString(), "TerminaCargarListaDocumentosRecibidos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de documentos recibidos", _
                                 Me.ToString(), "TerminaCargarListaDocumentosRecibidos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    

    Private Sub TerminoConsultarDatosCliente(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningun registro con estas caracteristicas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarDatosCliente()
                Else
                    If Not IsNothing(lo.Entities) Then
                        lngIdComitente = lo.Entities(0).IdComitente
                        strTipoIdentificacion = lo.Entities(0).CodTipoIdentificacion
                        strNroDocumento = lo.Entities(0).NroDocumento
                        strNombre = lo.Entities(0).Nombre
                        ConsultarDocumentos()
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos del cliente.", _
                                 Me.ToString(), "TerminoConsultarDatosCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos del cliente.", _
                                 Me.ToString(), "TerminoConsultarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

End Class

