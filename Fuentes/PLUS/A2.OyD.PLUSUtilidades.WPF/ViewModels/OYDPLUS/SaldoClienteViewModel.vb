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

Public Class SaldoClienteViewModel
    Implements INotifyPropertyChanged
    Private dcProxy As OyDPLUSutilidadesDomainContext


#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of OyDPLUSutilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)

            LimpiarSaldoCliente()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "SaldoClienteViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    Public Sub ConsultarSaldoCliente()
        Try
            If ConsultarSaldo Then
                If Not IsNothing(CodigoOYD) Then
                    If Not String.IsNullOrEmpty(CodigoOYD) And ConsultarSaldo Then
                        LimpiarSaldoCliente()
                        MostrarConsultandoSaldo = Visibility.Visible

                        If Not IsNothing(dcProxy.tblSaldosClientes) Then
                            dcProxy.tblSaldosClientes.Clear()
                        End If
                        dcProxy.Load(dcProxy.OYDPLUS_ConsultarSaldoClienteQuery(CodigoOYD, Program.Usuario, TipoProducto, CarteraColectivaFondos, NroEncargoFondos, Program.HashConexion), AddressOf TerminoConsultarSaldoCliente, String.Empty)
                    Else
                        LimpiarSaldoCliente()
                        MostrarConsultandoSaldo = Visibility.Collapsed
                    End If
                Else
                    LimpiarSaldoCliente()
                    MostrarConsultandoSaldo = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el saldo del cliente.", _
                                 Me.ToString(), "ConsultarSaldoCliente", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoSaldo = Visibility.Collapsed
        End Try
    End Sub

    Public Sub LimpiarSaldoCliente()
        Try
            If Not IsNothing(ListaSaldoCliente) Then
                ListaSaldoCliente.Clear()
            End If
            SaldoActual = Nothing
            SaldoClienteSelected = Nothing

            Dim objListaSaldoCliente As New List(Of OYDPLUSUtilidades.tblSaldosCliente)

            InsertarRegistro(objListaSaldoCliente, "Saldo Actual:", 0)
            InsertarRegistro(objListaSaldoCliente, "Saldo en Canje:", 0)
            InsertarRegistro(objListaSaldoCliente, "Ordenes de Tesoreria Pendientes:", 0)
            InsertarRegistro(objListaSaldoCliente, "Operaciones Pendientes por Cumplir:", 0)
            InsertarRegistro(objListaSaldoCliente, "Saldo Neto Cliente:", 0)

            ListaSaldoCliente = objListaSaldoCliente

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los saldos del cliente.", _
                                 Me.ToString(), "LimpiarSaldoCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub InsertarRegistro(ByVal objListaSaldo As List(Of OYDPLUSUtilidades.tblSaldosCliente), ByVal strDescripcion As String, ByVal dblValor As Double)
        Try
            If IsNothing(objListaSaldo) Then
                objListaSaldo = New List(Of OYDPLUSUtilidades.tblSaldosCliente)
            End If

            objListaSaldo.Add(New OYDPLUSUtilidades.tblSaldosCliente With {.ID = 1, _
                                                                              .CodigoOYD = String.Empty, _
                                                                              .Usado = True, _
                                                                              .Descripcion = strDescripcion, _
                                                                              .Valor = dblValor})
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los saldos del cliente.", _
                                 Me.ToString(), "InsertarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"
    Private _SaldoActual As Double
    Public Property SaldoActual() As Double
        Get
            Return _SaldoActual

        End Get
        Set(ByVal value As Double)
            _SaldoActual = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SaldoActual"))
        End Set
    End Property
    Private _VerEncabezadoSaldoCliente As Visibility
    Public Property VerEncabezadoSaldoCliente() As Visibility
        Get
            Return _VerEncabezadoSaldoCliente
        End Get
        Set(ByVal value As Visibility)
            _VerEncabezadoSaldoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VerEncabezadoSaldoCliente"))
        End Set
    End Property

    Private _ListaSaldoCliente As List(Of OYDPLUSUtilidades.tblSaldosCliente)
    Public Property ListaSaldoCliente() As List(Of OYDPLUSUtilidades.tblSaldosCliente)
        Get
            Return _ListaSaldoCliente
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblSaldosCliente))
            _ListaSaldoCliente = value
            If Not IsNothing(_ListaSaldoCliente) Then
                SaldoClienteSelected = ListaSaldoCliente.FirstOrDefault
                SaldoActual = SaldoClienteSelected.Valor
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaSaldoCliente"))
        End Set
    End Property

    Private _SaldoClienteSelected As OYDPLUSUtilidades.tblSaldosCliente
    Public Property SaldoClienteSelected() As OYDPLUSUtilidades.tblSaldosCliente
        Get
            Return _SaldoClienteSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblSaldosCliente)
            _SaldoClienteSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SaldoClienteSelected"))
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

    Private _CodigoOYD As String
    Public Property CodigoOYD() As String
        Get
            Return _CodigoOYD
        End Get
        Set(ByVal value As String)
            _CodigoOYD = value
        End Set
    End Property

    Private _TipoProducto As String
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
        End Set
    End Property

    Private _CarteraColectivaFondos As String
    Public Property CarteraColectivaFondos() As String
        Get
            Return _CarteraColectivaFondos
        End Get
        Set(ByVal value As String)
            _CarteraColectivaFondos = value
        End Set
    End Property

    Private _NroEncargoFondos As String
    Public Property NroEncargoFondos() As String
        Get
            Return _NroEncargoFondos
        End Get
        Set(ByVal value As String)
            _NroEncargoFondos = value
        End Set
    End Property

    Private _ConsultarSaldo As Boolean
    Public Property ConsultarSaldo() As Boolean
        Get
            Return _ConsultarSaldo
        End Get
        Set(ByVal value As Boolean)
            _ConsultarSaldo = value
            ConsultarSaldoCliente()
        End Set
    End Property

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarSaldoCliente(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblSaldosCliente))
        Try
            If lo.HasError = False Then
                ListaSaldoCliente = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el saldo del cliente.", Me.ToString(), "TerminoConsultarSaldoCliente", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                MostrarConsultandoSaldo = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener el saldo del cliente.", Me.ToString(), "TerminoConsultarSaldoCliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoSaldo = Visibility.Collapsed
        End Try
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
