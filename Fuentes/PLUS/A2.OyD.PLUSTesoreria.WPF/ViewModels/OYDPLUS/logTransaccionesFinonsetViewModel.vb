Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TesoreriaViewModel.vb
'Generado el : 07/08/2011 15:52:29
'Propiedad de Alcuadrado S.A. 2010

#Region "Bitacora de Modificaciones"
'Modificado Por : Juan David Correa
'Fecha          : 16/Agosto 4:15 pm
'Descripción    : Se agrego el metodo ValidarEdicion y el metodo asincronico TerminoValidarEdicion.

'Modificaciones
'Modificado Por : Juan David Correa
'Fecha          : 17/Agosto 8:20 am
'Descripción    : Se agrego la propiedad Read para controlar la edición del grid y habilitar los scroll.

'Modificado Por : Juan David Correa
'Fecha          : 17/Agosto 9:50 am
'Descripción    : Se modifico el evento de VesionRegistro al codigo se le asigna el ID del documento 
'                 codigo = TesoreriSelected.IDDocumento.

'Modificado Por : Juan David Correa
'Fecha          : 17/Agosto 9:50 am
'Descripción    : Se modifico la propiedad cmbNombreConsecutivoHabilitado para que cuando se le de clic al boton
'                 nuevo la propiedad este en true y cuando se le de editar cambie a false

'Modificado Por : Juan David Correa
'Fecha          : 18/Agosto 8:50 am
'Descripción    : Se agrego la propiedad contador para controlar la vista del detalle cuando se crea un nuevo registro

'Modificado Por : Juan Carlos Soto Cruz
'Fecha          : 18/Agosto 10:24 a.m
'Descripción    : 

'Modificado Por : Juan Carlos Soto Cruz
'Fecha          : 04 Septiembre/2011
'Descripción    : Se

#End Region

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
Imports Microsoft.VisualBasic.CompilerServices
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Text
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Public Class logTransaccionesFinonsetViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Declaraciones"
    Private str_NombreHojaExcel As String = "logTransacciones"
    Private Const STR_TIPOEXPORTACION As String = "EXCELVIEJO"
    Private Const STR_NOMBREPROCESO As String = "LOGTRANSACCIONES_FINONSET"
    Private Const STR_PARAMETROSGENERACION As String = "[DTMFECHAINICIAL]=[[DTMFECHAINICIAL]]|[DTMFECHAFINAL]=[[DTMFECHAFINAL]]|[USUARIO]=[[USUARIO]]"

    Dim dcProxy As OYDPLUSTesoreriaDomainContext
    Dim dcProxy1 As OYDPLUSTesoreriaDomainContext
    Dim dcProxy2 As OYDPLUSTesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim objProxyPLUS As OyDPLUSutilidadesDomainContext
    Dim Usuario As String = String.Empty
    Private Enum ParametrosGeneracion
        DTMFECHAINICIAL
        DTMFECHAFINAL
        USUARIO
    End Enum
#End Region



#Region "Inicializacion"
    Public Sub New()
        Try
            'ListaTopicos = New List(Of String)
            'ListaTopicos.Add(STR_TOPICO_NOTIFICACION_TESORERO)
            'ListaTopicos.Add(STR_TOPICO_NOTIFICACION_ORDENESTESORERIA)

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSTesoreriaDomainContext()
                dcProxy1 = New OYDPLUSTesoreriaDomainContext()
                dcProxy2 = New OYDPLUSTesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()
                objProxyPLUS = New OyDPLUSutilidadesDomainContext()

            Else
                dcProxy = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy1 = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy2 = New OYDPLUSTesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                objProxyPLUS = New OyDPLUSutilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OYDPLUSTesoreriaDomainContext.IOYDPLUSTesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then

                Usuario = Program.Usuario

            End If
        Catch ex As Exception

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TesoreriaViewModelOyDPlus.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Propiedades"
    Private _dtmFechaInicial As DateTime = Now
    Public Property dtmFechaInicial() As DateTime
        Get
            Return _dtmFechaInicial
        End Get
        Set(ByVal value As DateTime)
            _dtmFechaInicial = value
            MyBase.CambioItem("dtmFechaInicial")
        End Set
    End Property
    Private _dtmFechaFinal As DateTime = Now
    Public Property dtmFechaFinal() As DateTime
        Get
            Return _dtmFechaFinal
        End Get
        Set(ByVal value As DateTime)
            _dtmFechaFinal = value
            MyBase.CambioItem("dtmFechaFinal")
        End Set
    End Property


    Private _ListaResultadosDocumentos As List(Of OyDPLUSTesoreria.LogTransaccionesFINONSET)
    Public Property ListaResultadosDocumentos() As List(Of OyDPLUSTesoreria.LogTransaccionesFINONSET)
        Get
            Return _ListaResultadosDocumentos
        End Get
        Set(ByVal value As List(Of OyDPLUSTesoreria.LogTransaccionesFINONSET))
            _ListaResultadosDocumentos = value
            If Not IsNothing(_ListaResultadosDocumentos) Then
                SelectedDocumentos = _ListaResultadosDocumentos.FirstOrDefault
            End If

            MyBase.CambioItem("ListaResultadosDocumentos")
            MyBase.CambioItem("ListaResultadosDocumentos_Paged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadosDocumentos_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadosDocumentos) Then
                Dim view = New PagedCollectionView(_ListaResultadosDocumentos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _SelectedDocumentos As OyDPLUSTesoreria.LogTransaccionesFINONSET
    Public Property SelectedDocumentos As OyDPLUSTesoreria.LogTransaccionesFINONSET
        Get
            Return _SelectedDocumentos
        End Get
        Set(ByVal value As OyDPLUSTesoreria.LogTransaccionesFINONSET)
            _SelectedDocumentos = value
            MyBase.CambioItem("SelectedDocumentos")
        End Set
    End Property


#End Region
#Region "Metodos"
    Public Sub Consultar()
        Try
            IsBusy = True
            If Not IsNothing(dcProxy.LogTransaccionesFINONSETs) Then
                dcProxy.LogTransaccionesFINONSETs.Clear()
            End If
            dcProxy.Load(dcProxy.LogTransaccionesFinonsetQuery(dtmFechaInicial.ToString("yyyyMMdd"), dtmFechaFinal.ToString("yyyyMMdd"), Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarDocumentos, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Consultar", _
                                     Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Sub TerminoConsultarDocumentos(lo As LoadOperation(Of OyDPLUSTesoreria.LogTransaccionesFINONSET))
        Try

            If lo.HasError = False Then
                ListaResultadosDocumentos = dcProxy.LogTransaccionesFINONSETs.ToList


            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos", _
                                                 Me.ToString(), "TerminoConsultarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Documentos", _
                                                             Me.ToString(), "TerminoConsultarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub Exportar()
        Try
            Dim strNombreArchivo As String = String.Empty

            Dim strParametrosEnviar As String = STR_PARAMETROSGENERACION
            If Not IsNothing(dcProxy.Tesoreros) Then
                dcProxy.Tesoreros.Clear()
            End If


            strParametrosEnviar = strParametrosEnviar.Replace(String.Format("[[{0}]]", ParametrosGeneracion.DTMFECHAINICIAL.ToString), dtmFechaInicial.ToString("yyyyMMdd"))
            strParametrosEnviar = strParametrosEnviar.Replace(String.Format("[[{0}]]", ParametrosGeneracion.DTMFECHAFINAL.ToString), dtmFechaFinal.ToString("yyyyMMdd"))
            strParametrosEnviar = strParametrosEnviar.Replace(String.Format("[[{0}]]", ParametrosGeneracion.USUARIO.ToString), Usuario)
            strNombreArchivo = String.Format("LogTransaccionesFinonset_{0:yyyyMMddHHmmss}", Now)
            objProxy.Load(objProxy.GenerarArchivoPlanoQuery(STR_NOMBREPROCESO, STR_NOMBREPROCESO, strParametrosEnviar, strNombreArchivo,
                                                            str_NombreHojaExcel, STR_TIPOEXPORTACION, Program.Maquina, Program.Usuario, Program.HashConexion),
                                                        AddressOf TerminoGenerarArchivoPlanoFondos, "")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Exportar", _
                                     Me.ToString(), "Exportar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoGenerarArchivoPlanoFondos(ByVal lo As LoadOperation(Of OYDUtilidades.GenerarArchivosPlanos))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso Then


                        System.Diagnostics.Process.Start(lo.Entities.First.RutaArchivoPlano)


                    Else
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el archivo.", Me.ToString(), "TerminoGenerarArchivoPlanoFondos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
#End Region
End Class