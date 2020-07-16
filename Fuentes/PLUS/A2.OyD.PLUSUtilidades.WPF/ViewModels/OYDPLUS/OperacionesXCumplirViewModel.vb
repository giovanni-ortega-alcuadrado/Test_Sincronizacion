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

Public Class OperacionesXCumplirViewModel
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
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "OperacionesXCumplirViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    Public Sub ConsultarOperacionesXCumplir()
        Try
            If ConsultarOperaciones Then
                If Not IsNothing(TipoNegocio) And Not IsNothing(TipoOperacion) And (Not IsNothing(CodigoOYD) Or Not IsNothing(Especie)) Then
                    If Not String.IsNullOrEmpty(TipoNegocio) And Not String.IsNullOrEmpty(TipoOperacion) And _
                        ((Not String.IsNullOrEmpty(CodigoOYD) And CodigoOYD <> "-9999999999") Or (Not IsNothing(Especie) And Especie <> "(No Seleccionado)")) Then
                        If Not IsNothing(dcProxy.tblPortafolioClientes) Then
                            dcProxy.tblOperacionesCumplirs.Clear()
                        End If
                        LimpiarOperacionesXCumplir()
                        MostrarConsultandoOperaciones = Visibility.Visible

                        dcProxy.Load(dcProxy.OYDPLUS_ConsultarOperacionesCumplirQuery(TipoNegocio, TipoOperacion, CodigoOYD, Especie, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarOperaciones, String.Empty)
                    Else
                        LimpiarOperacionesXCumplir()
                        MostrarConsultandoOperaciones = Visibility.Collapsed
                    End If
                    LimpiarOperacionesXCumplir()
                    MostrarConsultandoOperaciones = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el portafolio del cliente.", _
                                 Me.ToString(), "ConsultarPortafolioCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarOperacionesXCumplir()
        Try
            ListaOperacionesXCumplir = Nothing
            OperacionXCumplirSelected = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los controles.", _
                                 Me.ToString(), "LimpiarOperacionesXCumplir", Application.Current.ToString(), Program.Maquina, ex)
            MostrarConsultandoOperaciones = Visibility.Collapsed
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaOperacionesXCumplir As List(Of OYDPLUSUtilidades.tblOperacionesCumplir)
    Public Property ListaOperacionesXCumplir() As List(Of OYDPLUSUtilidades.tblOperacionesCumplir)
        Get
            Return _ListaOperacionesXCumplir
        End Get
        Set(ByVal value As List(Of OYDPLUSUtilidades.tblOperacionesCumplir))
            _ListaOperacionesXCumplir = value
            If Not IsNothing(ListaOperacionesXCumplir) Then
                OperacionXCumplirSelected = ListaOperacionesXCumplir.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOperacionesXCumplir"))
        End Set
    End Property

    Private WithEvents _OperacionXCumplirSelected As OYDPLUSUtilidades.tblOperacionesCumplir
    Public Property OperacionXCumplirSelected() As OYDPLUSUtilidades.tblOperacionesCumplir
        Get
            Return _OperacionXCumplirSelected
        End Get
        Set(ByVal value As OYDPLUSUtilidades.tblOperacionesCumplir)
            _OperacionXCumplirSelected = value
            ViewOperacionXCumplir.OperacionXCumplirSeleccionada = OperacionXCumplirSelected

            If Not IsNothing(OperacionXCumplirSelected) Then
                If OperacionXCumplirSelected.Seleccionada Then
                    ViewOperacionXCumplir.OperacionSeleccionada = True
                Else
                    ViewOperacionXCumplir.OperacionSeleccionada = False
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("OperacionXCumplirSelected"))
        End Set
    End Property

    Private _MostrarConsultandoOperaciones As Visibility = Visibility.Collapsed
    Public Property MostrarConsultandoOperaciones() As Visibility
        Get
            Return _MostrarConsultandoOperaciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultandoOperaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarConsultandoOperaciones"))
        End Set
    End Property

    Private _ViewOperacionXCumplir As OperacionesXCumplirView
    Public Property ViewOperacionXCumplir() As OperacionesXCumplirView
        Get
            Return _ViewOperacionXCumplir
        End Get
        Set(ByVal value As OperacionesXCumplirView)
            _ViewOperacionXCumplir = value
        End Set
    End Property

    Private _CodigoOYD As String
    Public Property CodigoOYD() As String
        Get
            Return _CodigoOYD
        End Get
        Set(ByVal value As String)
            _CodigoOYD = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoOYD"))
        End Set
    End Property

    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property


    Private _TipoNegocio As String
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value
            If Not IsNothing(TipoNegocio) Then
                If TipoNegocio.ToUpper = "C" Then
                    MostrarCamposAccionesRentaFija = True
                    MostrarFaciales = True
                    MostrarOtrosCampos = False
                ElseIf TipoNegocio.ToUpper = "TTV" Or TipoNegocio.ToUpper = "R" Or TipoNegocio.ToUpper = "S" Then
                    MostrarFaciales = True
                    MostrarOtrosCampos = True
                    MostrarCamposAccionesRentaFija = False
                Else
                    MostrarCamposAccionesRentaFija = True
                    MostrarFaciales = False
                    MostrarOtrosCampos = False
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    Private _TipoOperacion As String
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _MostrarFaciales As Boolean
    Public Property MostrarFaciales() As Boolean
        Get
            Return _MostrarFaciales
        End Get
        Set(ByVal value As Boolean)
            _MostrarFaciales = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarFaciales"))
        End Set
    End Property

    Private _MostrarCamposAccionesRentaFija As Boolean
    Public Property MostrarCamposAccionesRentaFija() As Boolean
        Get
            Return _MostrarCamposAccionesRentaFija
        End Get
        Set(ByVal value As Boolean)
            _MostrarCamposAccionesRentaFija = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarCamposAccionesRentaFija"))
        End Set
    End Property

    Private _MostrarOtrosCampos As Boolean
    Public Property MostrarOtrosCampos() As Boolean
        Get
            Return _MostrarOtrosCampos
        End Get
        Set(ByVal value As Boolean)
            _MostrarOtrosCampos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarOtrosCampos"))
        End Set
    End Property

    Private _ConsultarOperaciones As Boolean
    Public Property ConsultarOperaciones() As Boolean
        Get
            Return _ConsultarOperaciones
        End Get
        Set(ByVal value As Boolean)
            _ConsultarOperaciones = value
            ConsultarOperacionesXCumplir()
        End Set
    End Property


#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarOperaciones(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.tblOperacionesCumplir))
        Try
            If lo.HasError = False Then
                ListaOperacionesXCumplir = lo.Entities.ToList
                MostrarConsultandoOperaciones = Visibility.Collapsed
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener las operaciones x cumplir.", Me.ToString(), "TerminoConsultarOperaciones", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                MostrarConsultandoOperaciones = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener las operaciones x cumplir.", Me.ToString(), "TerminoConsultarOperaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoOperaciones = Visibility.Collapsed
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la orden activa cambia
    ''' Desarrollado por Juan David Correa
    ''' Fecha 14 de Agosto del 2012
    ''' </summary>
    Private Sub _OperacionXCumplirSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _OperacionXCumplirSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "seleccionada"
                    If OperacionXCumplirSelected.Seleccionada Then
                        ViewOperacionXCumplir.OperacionSeleccionada = True
                    Else
                        ViewOperacionXCumplir.OperacionSeleccionada = False
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesSAESelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            MostrarConsultandoOperaciones = Visibility.Collapsed
        End Try

    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
