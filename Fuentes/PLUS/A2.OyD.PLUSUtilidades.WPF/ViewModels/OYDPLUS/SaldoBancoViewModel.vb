Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class SaldoBancoViewModel
    Implements INotifyPropertyChanged
    Private dcProxy As OYDPLUSUtilidadesDomainContext

    Public viewSaldoBanco As SaldoBancoView

#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)
            LimpiarSaldoBanco()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "SaldoClienteViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    Public Sub ConsultarSaldoBanco()
        Try
            If ConsultarSaldo Then
                If Not IsNothing(IdBanco) Then
                    LimpiarSaldoBanco()
                    MostrarConsultandoSaldo = Visibility.Visible
                    IsBusySaldoBanco = True
                    If Not IsNothing(dcProxy.tblSaldosClientes) Then
                        dcProxy.tblSaldosClientes.Clear()
                    End If
                    If Not IsNothing(dcProxy.tblSaldosBancos) Then
                        dcProxy.tblSaldosBancos.Clear()
                    End If
                    dcProxy.Load(dcProxy.OYDPLUS_ConsultarSaldoBancoQuery(IdBanco, IdBanco, Date.Today, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarSaldoBanco, String.Empty)
                Else
                    LimpiarSaldoBanco()
                    MostrarConsultandoSaldo = Visibility.Collapsed
                End If
            Else
                LimpiarSaldoBanco()
                MostrarConsultandoSaldo = Visibility.Collapsed
            End If
        Catch ex As Exception
            IsBusySaldoBanco = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el saldo del cliente.", _
                                 Me.ToString(), "ConsultarSaldoCliente", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoSaldo = Visibility.Collapsed
        End Try
    End Sub

    Public Sub LimpiarSaldoBanco()
        Try
            If Not IsNothing(ListaSaldoBanco) Then
                ListaSaldoBanco.Clear()
            End If
            SaldoActual = Nothing
            SaldoBancoSelected = Nothing

            Dim objListaSaldoBanco As New List(Of OYDPLUSUtilidades.tblSaldosBancos)

            InsertarRegistro(objListaSaldoBanco, 0)

            ListaSaldoBanco = objListaSaldoBanco

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los saldos de banco.", _
                                 Me.ToString(), "LimpiarSaldoBanco", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub InsertarRegistro(ByVal objListaSaldo As List(Of OYDPLUSUtilidades.tblSaldosBancos), ByVal dblValor As Double)
        Try
            If IsNothing(objListaSaldo) Then
                objListaSaldo = New List(Of OYDPLUSUtilidades.tblSaldosBancos)
            End If

            objListaSaldo.Add(New OYDPLUSUtilidades.tblSaldosBancos With {.lngIdBanco = -1, _
                                                                              .strNombre = String.Empty, _
                                                                              .strNombreSucursal = String.Empty, _
                                                                              .strNroCuenta = String.Empty, _
                                                                              .curValor = dblValor})
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los saldos del cliente.", _
                                 Me.ToString(), "InsertarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"
    Private _HabilitarSaldoBanco As Boolean
    Public Property HabilitarSaldoBanco() As Boolean
        Get
            Return _HabilitarSaldoBanco
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSaldoBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarSaldoBanco"))
        End Set
    End Property
    Private _IsBusySaldoBanco As Boolean
    Public Property IsBusySaldoBanco() As Boolean
        Get
            Return _IsBusySaldoBanco
        End Get
        Set(ByVal value As Boolean)
            _IsBusySaldoBanco = value
            If Not IsNothing(viewSaldoBanco) Then
                viewSaldoBanco.myBusyIndicator.IsBusy = _IsBusySaldoBanco
            End If

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusySaldoBanco"))
        End Set
    End Property

    Private _SaldoActual As Double = 0
    Public Property SaldoActual() As Double
        Get
            Return _SaldoActual

        End Get
        Set(ByVal value As Double)
            _SaldoActual = value
            If Not IsNothing(viewSaldoBanco) Then
                viewSaldoBanco.txtSaldo.Text = _SaldoActual.ToString("$ 0,#,#0.00")
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SaldoActual"))
        End Set
    End Property
    Private _VerEncabezadoSaldoBanco As Visibility
    Public Property VerEncabezadoSaldoBanco() As Visibility
        Get
            Return _VerEncabezadoSaldoBanco
        End Get
        Set(ByVal value As Visibility)
            _VerEncabezadoSaldoBanco = value
            If Not IsNothing(viewSaldoBanco) Then
                viewSaldoBanco.txtSaldo.Visibility = _VerEncabezadoSaldoBanco
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VerEncabezadoSaldoBanco"))
        End Set
    End Property

    Private _ListaSaldoBanco As List(Of OYDPLUSUtilidades.tblSaldosBancos)
    Public Property ListaSaldoBanco() As List(Of OYDPLUSUtilidades.tblSaldosBancos)
        Get
            Return _ListaSaldoBanco
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblSaldosBancos))
            _ListaSaldoBanco = value
            If Not IsNothing(_ListaSaldoBanco) Then
                If _ListaSaldoBanco.FirstOrDefault.lngIdBanco > 0 Then
                    SaldoBancoSelected = ListaSaldoBanco.FirstOrDefault
                    SaldoActual = SaldoBancoSelected.curValor
                End If

            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaSaldoBanco"))
        End Set
    End Property

    Private _SaldoBancoSelected As OYDPLUSUtilidades.tblSaldosBancos
    Public Property SaldoBancoSelected() As OYDPLUSUtilidades.tblSaldosBancos
        Get
            Return _SaldoBancoSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblSaldosBancos)
            _SaldoBancoSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SaldoBancoSelected"))
        End Set
    End Property

    Private _MostrarConsultandoSaldo As Visibility = Visibility.Collapsed
    Public Property MostrarConsultandoSaldo() As Visibility
        Get
            Return _MostrarConsultandoSaldo
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultandoSaldo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarConsultandoSaldo"))
        End Set
    End Property

    Private _IdBanco As Integer
    Public Property IdBanco() As Integer
        Get
            Return _IdBanco
        End Get
        Set(ByVal value As Integer)
            _IdBanco = value
        End Set
    End Property

    Private _ConsultarSaldo As Boolean
    Public Property ConsultarSaldo() As Boolean
        Get
            Return _ConsultarSaldo
        End Get
        Set(ByVal value As Boolean)
            _ConsultarSaldo = value
            ConsultarSaldoBanco()
        End Set
    End Property



#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarSaldoBanco(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblSaldosBancos))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaSaldoBanco = lo.Entities.ToList

                Else
                    ListaSaldoBanco = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el saldo del Banco.", Me.ToString(), "TerminoConsultarSaldoBanco", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                MostrarConsultandoSaldo = Visibility.Collapsed
            End If
            IsBusySaldoBanco = False
        Catch ex As Exception
            IsBusySaldoBanco = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el saldo del Banco.", Me.ToString(), "TerminoConsultarSaldoBanco", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoSaldo = Visibility.Collapsed
        End Try
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
