Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: InactivacionClientesViewModel.vb
'Generado el : 07/28/2011 07:51:00AM
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSDeceval
Imports A2.OyD.OYDServer.RIA.Web.OYDPLUSUtilidades
Imports A2Utilidades.Mensajes
Imports OpenRiaServices.DomainServices.Client

Public Class ViewModelArchivosDeceval
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"
    Public Const GSTR_PENDIENTES = "Pendientes"
    Public Const GSTR_XConfirmar = "Por confirmar"
    Public Const GSTR_PROCESADAS = "Procesadas"
    Dim strAccion As String = ""
    Private dcProxy As OYDPLUSDecevalDomainContext
#End Region

#Region "Metodos"
    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New OYDPLUSDecevalDomainContext()
        Else
            dcProxy = New OYDPLUSDecevalDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        Dim objListaDatos As New List(Of String)

        objListaDatos = New List(Of String)
        objListaDatos.Add(GSTR_PENDIENTES)
        objListaDatos.Add(GSTR_XConfirmar)
        objListaDatos.Add(GSTR_PROCESADAS)

        Try
            If Not Program.IsDesignMode() Then
                Consultar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "Inversionistas.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub VerDetalle()
        Try
            If Not IsNothing(LogArchivo) Then
                Dim objviewDetalleArchivo As New DetalleArchivosDeceval(LogArchivo.ID, LogArchivo.NombreArchivo, Program.Usuario, LogArchivo.EjecucionAutomatica)
                Program.Modal_OwnerMainWindowsPrincipal(objviewDetalleArchivo)
                objviewDetalleArchivo.ShowDialog()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar detalle",
                               Me.ToString(), "VerDetalle.New", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub
    Public Sub Consultar()
        Try

            IsBusy = True
            If Not IsNothing(dcProxy.Archivos) Then
                dcProxy.Archivos.Clear()
            End If
            If Not IsNothing(dcProxy.DetalleArchivos) Then
                dcProxy.DetalleArchivos.Clear()
            End If
            dcProxy.Load(dcProxy.ArchivosConsultarQuery(Fecha.ToShortDateString, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRegistros, "")


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista.",
                                             Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoTraerRegistros(lo As LoadOperation(Of Archivos))
        Try
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    ListaArchivos = lo.Entities.ToList

                Else

                    ListaArchivos = Nothing
                End If


            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista ",
                                                     Me.ToString(), "TerminoTraerRegistros", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista r",
                                             Me.ToString(), "TerminoTraerRegistros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub






#End Region
#Region "Metodos Respuesta"

    Private Sub TerminoActualizarInversionistas(ByVal lo As LoadOperation(Of tblResultadoEnvio))
        Try
            If lo.HasError = False Then
                Dim strMensajeErrores As String = String.Empty


                If lo.Entities.Count > 0 Then
                    For Each li In lo.Entities
                        If li.logExitoso = False Then
                            If String.IsNullOrEmpty(strMensajeErrores) Then
                                strMensajeErrores = li.Mensaje
                            Else
                                strMensajeErrores = String.Format("{0}{1}{2}", strMensajeErrores, vbCrLf, li.Mensaje)
                            End If
                        End If
                    Next
                Else
                    IsBusy = False
                End If

                If Not String.IsNullOrEmpty(strMensajeErrores) Then
                    strMensajeErrores = String.Format("Se presentaron unas validaciones al momento de guardar:{0}{1}", vbCrLf, strMensajeErrores)
                    mostrarMensaje(strMensajeErrores, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                Else
                    IsBusy = False
                    For Each li In lo.Entities
                        If li.logExitoso = True Then
                            If String.IsNullOrEmpty(strMensajeErrores) Then
                                strMensajeErrores = li.Mensaje
                            Else
                                strMensajeErrores = String.Format("{0}{1}{2}", strMensajeErrores, vbCrLf, li.Mensaje)
                            End If
                        End If
                    Next
                    strMensajeErrores = String.Format("{0}{1}", vbCrLf, strMensajeErrores)
                    mostrarMensaje(strMensajeErrores, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    IsBusy = False
                    Consultar()
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Procesar los Registros",
                                            Me.ToString(), "TerminoActualizarInversionistas", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Procesar los Registros",
                                                             Me.ToString(), "TerminoActualizarInversionistas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Propiedades"
    Private _Fecha As DateTime = Date.Now
    Public Property Fecha() As DateTime
        Get
            Return _Fecha
        End Get
        Set(ByVal value As DateTime)
            _Fecha = value
            MyBase.CambioItem("Fecha")
        End Set
    End Property


    Private _ListaArchivos As List(Of Archivos)
    Public Property ListaArchivos() As List(Of Archivos)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As List(Of Archivos))
            _ListaArchivos = value

            MyBase.CambioItem("ListaArchivos")
            MyBase.CambioItem("ListaArchivosPaged")
            If Not IsNothing(_ListaArchivos) Then
                LogArchivo = _ListaArchivos.FirstOrDefault
            End If
        End Set
    End Property

    Public ReadOnly Property ListaArchivosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaArchivos) Then
                Dim view = New PagedCollectionView(_ListaArchivos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _LogArchivo As Archivos
    Public Property LogArchivo() As Archivos
        Get
            Return _LogArchivo
        End Get
        Set(ByVal value As Archivos)
            _LogArchivo = value
            MyBase.CambioItem("LogArchivo")
        End Set
    End Property


#End Region



End Class

