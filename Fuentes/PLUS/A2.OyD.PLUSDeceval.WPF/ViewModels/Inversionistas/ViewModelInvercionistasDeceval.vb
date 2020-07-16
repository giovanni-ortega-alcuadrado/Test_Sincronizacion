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

Public Class ViewModelInvercionistasDeceval
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

        ListaDatos = objListaDatos
        VistaSeleccionada = GSTR_PENDIENTES
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
            If Not IsNothing(InversionistasSelected) Then
                Dim objDEtalle As New DetalleInversionistasDeceval(InversionistasSelected.ID, InversionistasSelected.IDComitente)
                Program.Modal_OwnerMainWindowsPrincipal(objDEtalle)
                objDEtalle.ShowDialog()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar detalle",
                               Me.ToString(), "VerDetalle.New", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub
    Public Sub Consultar()
        Try
            SeleccionarTodos = False

            IsBusy = True
            If Not IsNothing(dcProxy.Inversionistas) Then
                dcProxy.Inversionistas.Clear()
                ListaInversionistas = Nothing

            End If
            If Not IsNothing(dcProxy.DetalleInversionistas) Then
                dcProxy.DetalleInversionistas.Clear()
            End If
            dcProxy.Load(dcProxy.InversionistasConsultarQuery(Comitente, VistaSeleccionada, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRegistros, "")


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista.",
                                             Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub ConcatenarRegistros(pstrAccion As String)
        Try

            Dim logpermitir As Boolean = True
            Dim strRegistros As String = ""
            If Not IsNothing(ListaInversionistas) Then
                If ListaInversionistas.Count > 0 Then
                    If ListaInversionistas.Where(Function(i) i.Generar = True).Count > 0 Then
                        For Each li In ListaInversionistas.Where(Function(i) i.Generar = True)
                            If li.Generar Then
                                If logpermitir Then
                                    strRegistros = String.Format("{0}", li.ID)
                                    logpermitir = False
                                Else
                                    strRegistros = String.Format("{0}|{1}", strRegistros, li.ID)
                                End If
                            End If
                        Next
                        If Not IsNothing(dcProxy.tblResultadoEnvios) Then
                            dcProxy.tblResultadoEnvios.Clear()
                        End If
                        IsBusy = True
                        dcProxy.Load(dcProxy.InversionistasActualizarQuery(pstrAccion, strRegistros, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarInversionistas, "")

                    Else
                        mostrarMensaje("No ha seleccionado ningun registro", "Validaciòn", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Concatenar Registros",
                                             Me.ToString(), "ConcatenarRegistros", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Public Sub EnviarConfirmar()
        Try
            If VistaSeleccionada = GSTR_PENDIENTES Then
                ConcatenarRegistros("CREAR")
            ElseIf VistaSeleccionada = GSTR_XConfirmar Then
                ConcatenarRegistros("CONFIRMACION")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Enviar y/o confirmar",
                                             Me.ToString(), "EnviarConfirmar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub Rechazar()
        Try
            ConcatenarRegistros("RECHAZAR")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al rechazar",
                                             Me.ToString(), "Rechazar", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Private Sub TerminoTraerRegistros(lo As LoadOperation(Of Inversionistas))
        Try
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    ListaInversionistas = lo.Entities.ToList
                    HabilitarBotones = True
                Else
                    HabilitarBotones = False
                    ListaInversionistas = Nothing
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
    Public Sub LimpiarComitente()
        Try
            Comitente = String.Empty
            DescripcionComitente = String.Empty
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el comitente",
                                             Me.ToString(), "LimpiarComitente", Application.Current.ToString(), Program.Maquina, ex)

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

                End If
                IsBusy = True
                Consultar()
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
    Private _VerPpal As Visibility = Visibility.Visible
    Public Property VerPpal() As Visibility
        Get
            Return _VerPpal
        End Get
        Set(ByVal value As Visibility)
            _VerPpal = value
            MyBase.CambioItem("VerPpal")
        End Set
    End Property
    Private _VerProcesadas As Visibility = Visibility.Collapsed
    Public Property VerProcesadas() As Visibility
        Get
            Return _VerProcesadas
        End Get
        Set(ByVal value As Visibility)
            _VerProcesadas = value
            MyBase.CambioItem("VerProcesadas")
        End Set
    End Property


    Private _SeleccionarTodos As Boolean
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            Try
                Dim Comitentes As String = String.Empty
                Dim logpermitir As Boolean = True
                If Not IsNothing(_ListaInversionistas) Then
                    If _ListaInversionistas.Count > 0 Then
                        If _SeleccionarTodos Then
                            For Each li In _ListaInversionistas.Where(Function(i) i.Generar = False)
                                li.Generar = True
                                If logpermitir Then
                                    Comitentes = String.Format("%{0}%", li.IDComitente)
                                    logpermitir = False
                                Else
                                    Comitentes = String.Format("{0}|%{1}%", Comitentes, li.IDComitente)
                                End If
                            Next
                        Else
                            For Each li In _ListaInversionistas.Where(Function(i) i.Generar = True)
                                li.Generar = False
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los documentos.",
                                                             Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
            End Try
            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property
    Private _DescripcionComitente As String = String.Empty
    Public Property DescripcionComitente() As String
        Get
            Return _DescripcionComitente
        End Get
        Set(ByVal value As String)
            _DescripcionComitente = value
            MyBase.CambioItem("DescripcionComitente")
        End Set
    End Property

    Private _Comitente As String = String.Empty
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            MyBase.CambioItem("Comitente")
        End Set
    End Property

    Private _HabilitarBotones As Boolean = False
    Public Property HabilitarBotones() As Boolean
        Get
            Return _HabilitarBotones
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotones = value
            MyBase.CambioItem("HabilitarBotones")
        End Set
    End Property



    Private _TextoBoton As String
    Public Property TextoBoton() As String
        Get
            Return _TextoBoton
        End Get
        Set(ByVal value As String)
            _TextoBoton = value
            MyBase.CambioItem("TextoBoton")
        End Set
    End Property

    Private _ListaInversionistas As List(Of Inversionistas)
    Public Property ListaInversionistas() As List(Of Inversionistas)
        Get
            Return _ListaInversionistas
        End Get
        Set(ByVal value As List(Of Inversionistas))
            _ListaInversionistas = value
            If Not IsNothing(_ListaInversionistas) Then
                InversionistasSelected = _ListaInversionistas.FirstOrDefault
            End If
            MyBase.CambioItem("ListaInversionistas")
        End Set
    End Property
    Private _InversionistasSelected As Inversionistas
    Public Property InversionistasSelected() As Inversionistas
        Get
            Return _InversionistasSelected
        End Get
        Set(ByVal value As Inversionistas)
            _InversionistasSelected = value
            MyBase.CambioItem("InversionistasSelected")
        End Set
    End Property
    Private _VerConfirmar As Visibility = Visibility.Visible
    Public Property VerConfirmar() As Visibility
        Get
            Return _VerConfirmar
        End Get
        Set(ByVal value As Visibility)
            _VerConfirmar = value
            MyBase.CambioItem("VerConfirmar")
        End Set
    End Property
    Private _VerRechazar As Visibility = Visibility.Collapsed
    Public Property VerRechazar() As Visibility
        Get
            Return _VerRechazar
        End Get
        Set(ByVal value As Visibility)
            _VerRechazar = value
            MyBase.CambioItem("VerRechazar")
        End Set
    End Property


    Private _VistaSeleccionada As String
    Public Property VistaSeleccionada() As String
        Get
            Return _VistaSeleccionada
        End Get
        Set(ByVal value As String)
            _VistaSeleccionada = value
            SeleccionarTodos = False
            If Not IsNothing(ListaInversionistas) Then
                ListaInversionistas = Nothing
            End If
            HabilitarBotones = False
            If _VistaSeleccionada = GSTR_PENDIENTES Then
                TextoBoton = "Enviar"
                VerRechazar = Visibility.Collapsed
                VerConfirmar = Visibility.Visible
                VerProcesadas = Visibility.Collapsed
                VerPpal = Visibility.Visible
            ElseIf _VistaSeleccionada = GSTR_XConfirmar Then
                TextoBoton = "Confirmar"
                VerRechazar = Visibility.Visible
                VerConfirmar = Visibility.Visible
                VerProcesadas = Visibility.Collapsed
                VerPpal = Visibility.Visible
            Else
                VerConfirmar = Visibility.Collapsed
                VerRechazar = Visibility.Collapsed
                VerProcesadas = Visibility.Visible
                VerPpal = Visibility.Collapsed
            End If
            Consultar()

            MyBase.CambioItem("VistaSeleccionada")
        End Set
    End Property

    Private _ListaDatos As List(Of String)
    Public Property ListaDatos() As List(Of String)
        Get
            Return _ListaDatos
        End Get
        Set(ByVal value As List(Of String))
            _ListaDatos = value
            MyBase.CambioItem("ListaDatos")
        End Set
    End Property

#End Region



End Class

