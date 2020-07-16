Imports Telerik.Windows.Controls
'Codigo Creado Por: Carlos Andres Toro
'Archivo: ProcesarArchivosDeceval.vb
'Generado el : 19/03/2015 
'Propiedad de Alcuadrado S.A. 
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSDeceval
Imports System.Text.RegularExpressions
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports OpenRiaServices.DomainServices.Client

Public Class ViewModelProcesarArchivosDeceval
    Inherits A2ControlMenu.A2ViewModel
    Private dcProxy As OYDPLUSDecevalDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSDecevalDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                dcProxy = New OYDPLUSDecevalDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ConfiguracionArchivoConsultarQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionArchivo, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ViewModelProcesarArchivosDeceval.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

#Region "Metodos"

    Private Sub consultarCuentasDepositoOYDPLUS(ByVal pstrIdComitente As String, Optional ByVal pstrUserState As String = "")
        Try
            If Not IsNothing(mdcProxyUtilidad.BuscadorCuentasDepositos) Then
                mdcProxyUtilidad.BuscadorCuentasDepositos.Clear()
            End If

            Dim strClienteABuscar = Right(Space(17) & pstrIdComitente, 17)
            CtaDepositoSeleccionada = Nothing

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarCuentasDepositoComitenteQuery(strClienteABuscar, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeposito, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar las cuentas deposito del cliente.", Me.ToString, "consultarCuentasDepositoOYDPLUS", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub ProcesarArchivoDeceval()
        Try
            If RegistroValido() Then
                If Not IsNothing(dcProxy.tblResultadoEnvioArchivos) Then
                    dcProxy.tblResultadoEnvioArchivos.Clear()
                End If

                Dim intCuentaDeposito As Nullable(Of Integer) = Nothing

                If CuentaDeposito >= 0 Then
                    intCuentaDeposito = CuentaDeposito
                End If
                IsBusy = True

                dcProxy.Load(dcProxy.ConfiguracionArchivoSolicitarQuery(ConfiguracionArchivoSeleccionado.Codigo, ConfiguracionArchivoSeleccionado.TipoArchivo, ISIN, intCuentaDeposito, CodigoDeposito, Program.Usuario, Program.HashConexion), AddressOf TerminoProcesarArchivoDeceval, "")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al procesasar archivo.", Me.ToString, " ProcesarArchivoDeceval", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub



    Public Function RegistroValido()

        If IsNothing(ConfiguracionArchivoSeleccionado) Then
            mostrarMensaje("El tipo de archivo es requerido ", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If

        Return True
    End Function

    Public Function LimpiarVariables()
        ComitenteSeleccionado = Nothing
        IDComitente = String.Empty
        CtaDepositoSeleccionada = Nothing
        CuentaDeposito = -1
        NemotecnicoSeleccionado = Nothing
        Especie = String.Empty
        ISIN = String.Empty
        Return True
    End Function

#End Region

#Region "Propiedades"

    Private _ListaConfiguracionArchivos As List(Of OyDPLUSDeceval.ConfiguracionArchivo)
    Public Property ListaConfiguracionArchivos() As List(Of OyDPLUSDeceval.ConfiguracionArchivo)
        Get
            Return _ListaConfiguracionArchivos
        End Get
        Set(ByVal value As List(Of OyDPLUSDeceval.ConfiguracionArchivo))
            _ListaConfiguracionArchivos = value
            MyBase.CambioItem("ListaConfiguracionArchivos")
        End Set
    End Property

    Private _ListaProcesarArchivos As List(Of OyDPLUSDeceval.tblResultadoEnvioArchivo)
    Public Property ListaProcesarArchivos() As List(Of OyDPLUSDeceval.tblResultadoEnvioArchivo)
        Get
            Return _ListaProcesarArchivos
        End Get
        Set(ByVal value As List(Of OyDPLUSDeceval.tblResultadoEnvioArchivo))
            _ListaProcesarArchivos = value
            MyBase.CambioItem("ListaProcesarArchivos")
        End Set
    End Property

    Private _ConfiguracionArchivoSeleccionado As OyDPLUSDeceval.ConfiguracionArchivo
    Public Property ConfiguracionArchivoSeleccionado() As OyDPLUSDeceval.ConfiguracionArchivo
        Get
            Return _ConfiguracionArchivoSeleccionado
        End Get
        Set(ByVal value As OyDPLUSDeceval.ConfiguracionArchivo)
            _ConfiguracionArchivoSeleccionado = value
            If _ConfiguracionArchivoSeleccionado.HabilitarSeleccionCliente = True Then
                HabilitarSeleccionCliente = True

            Else
                HabilitarSeleccionCliente = False
            End If

            If _ConfiguracionArchivoSeleccionado.HabilitarSeleccionEspecie = True Then
                HabilitarSeleccionEspecie = True

            Else
                HabilitarSeleccionEspecie = False
            End If

            MyBase.CambioItem("ConfiguracionArchivoSeleccionado")

        End Set
    End Property

    Private _IDComitente As String
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            MyBase.CambioItem("IDComitente")
        End Set
    End Property

    Private _ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
    Public Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_ComitenteSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorClientes)
            _ComitenteSeleccionado = value
            Try
                If Not IsNothing(_ComitenteSeleccionado) Then
                    IDComitente = _ComitenteSeleccionado.IdComitente
                    consultarCuentasDepositoOYDPLUS(_ComitenteSeleccionado.IdComitente)
                End If

            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad del cliente seleccionado.", Me.ToString, "ComitenteSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try

            MyBase.CambioItem("ComitenteSeleccionado")
        End Set
    End Property

    Private _ListaCuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
    Public Property ListaCuentasDeposito As List(Of OYDUtilidades.BuscadorCuentasDeposito)
        Get
            Return (_ListaCuentasDeposito)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorCuentasDeposito))
            _ListaCuentasDeposito = value
            MyBase.CambioItem("ListaCuentasDeposito")

            If Not IsNothing(_ListaCuentasDeposito) Then
                If _ListaCuentasDeposito.Count = 1 Then
                    CtaDepositoSeleccionada = _ListaCuentasDeposito.FirstOrDefault

                End If
            End If
        End Set
    End Property

    Private _mobjCtaDepositoSeleccionada As OYDUtilidades.BuscadorCuentasDeposito
    Public Property CtaDepositoSeleccionada() As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCtaDepositoSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorCuentasDeposito)
            _mobjCtaDepositoSeleccionada = value
            MyBase.CambioItem("CtaDepositoSeleccionada")
            If Not IsNothing(_mobjCtaDepositoSeleccionada) Then
                CuentaDeposito = _mobjCtaDepositoSeleccionada.NroCuentaDeposito
            End If
        End Set
    End Property

    Private _CuentaDeposito As Integer = -1
    Public Property CuentaDeposito() As Integer
        Get
            Return _CuentaDeposito
        End Get
        Set(ByVal value As Integer)
            _CuentaDeposito = value
            MyBase.CambioItem("CuentaDeposito")
        End Set
    End Property

    Private _CodigoDeposito As String = "DECEVAL"
    Public Property CodigoDeposito() As String
        Get
            Return _CodigoDeposito
        End Get
        Set(ByVal value As String)
            _CodigoDeposito = value
            MyBase.CambioItem("CodigoDeposito")
        End Set
    End Property

    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            Try
                If Not IsNothing(_NemotecnicoSeleccionado) Then
                    Especie = _NemotecnicoSeleccionado.Especie
                    ISIN = _NemotecnicoSeleccionado.ISIN
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al obtener la propiedad de la especie seleccionada.", Me.ToString, "NemotecnicoSeleccionado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            End Try
            MyBase.CambioItem("NemotecnicoSeleccionado")
        End Set
    End Property

    Private _Especie As String = String.Empty
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            MyBase.CambioItem("Especie")
        End Set
    End Property

    Private _ISIN As String = String.Empty
    Public Property ISIN() As String
        Get
            Return _ISIN
        End Get
        Set(ByVal value As String)
            _ISIN = value
            MyBase.CambioItem("ISIN")
        End Set
    End Property

#End Region


    Private _HabilitarSeleccionCliente As Boolean
    Public Property HabilitarSeleccionCliente() As Boolean
        Get
            Return _HabilitarSeleccionCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionCliente = value
            MyBase.CambioItem("HabilitarSeleccionCliente")
        End Set
    End Property

    Private _HabilitarSeleccionEspecie As Boolean
    Public Property HabilitarSeleccionEspecie() As Boolean
        Get
            Return _HabilitarSeleccionEspecie
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionEspecie = value
            MyBase.CambioItem("HabilitarSeleccionEspecie")
        End Set
    End Property

#Region "Resuiltados asincronicos"

    Private Sub TerminoTraerCuentasDeposito(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorCuentasDeposito))
        Try
            If Not lo.HasError Then
                ListaCuentasDeposito = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas de depósito", _
                                                 Me.ToString(), "TerminoTraerCuentasDeposito", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoProcesarArchivoDeceval(ByVal lo As LoadOperation(Of OyDPLUSDeceval.tblResultadoEnvioArchivo))
        Try
            If Not lo.HasError Then
                ListaProcesarArchivos = lo.Entities.ToList

                Dim strValidacionesProcesar As String = String.Empty
                Dim intIDArchivoInsertado As Integer = 0
                Dim strNombreConfiguracion As String = String.Empty
                Dim logEjecucionAutomatica As Boolean = False

                For Each li In ListaProcesarArchivos
                    If li.logExitoso = False Then
                        If String.IsNullOrEmpty(strValidacionesProcesar) Then
                            strValidacionesProcesar = li.Mensaje
                        Else
                            strValidacionesProcesar = String.Format("{0}{1}{2}", strValidacionesProcesar, vbCrLf, li.Mensaje)
                        End If
                    Else
                        intIDArchivoInsertado = li.IDArchivoInsertado
                        strNombreConfiguracion = li.NombreArchivo
                        logEjecucionAutomatica = li.EjecucionAutomatica
                    End If
                Next

                If String.IsNullOrEmpty(strValidacionesProcesar) Then
                    mostrarMensaje("Se realizo la solicitud del archivo exitosamente al sistema destino.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    LimpiarVariables()

                    Dim objviewDetalleArchivo As New DetalleArchivosDeceval(intIDArchivoInsertado, strNombreConfiguracion, Program.Usuario, logEjecucionAutomatica)
                    Program.Modal_OwnerMainWindowsPrincipal(objviewDetalleArchivo)
                    objviewDetalleArchivo.ShowDialog()
                Else
                    mostrarMensaje(String.Format("Al realizar la solicitud del archivo ocurrieron las siguientes validaciones:{0}{1}", vbCrLf, strValidacionesProcesar), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de archivos procesados", _
                                                 Me.ToString(), "TerminoProcesarArchivoDeceval", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de archivos procesados", _
                                                 Me.ToString(), "TerminoProcesarArchivoDeceval", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoTraerConfiguracionArchivo(ByVal lo As LoadOperation(Of OyDPLUSDeceval.ConfiguracionArchivo))
        Try
            If Not lo.HasError Then
                ListaConfiguracionArchivos = lo.Entities.ToList

                If _ListaConfiguracionArchivos.Count = 1 Then
                    ConfiguracionArchivoSeleccionado = _ListaConfiguracionArchivos.First
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de la configuración del archivo", _
                                                 Me.ToString(), "TerminoTraerConfiguracionArchivo", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de la configuración del archivo", _
                                                 Me.ToString(), "TerminoTraerConfiguracionArchivo", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

#End Region

End Class
