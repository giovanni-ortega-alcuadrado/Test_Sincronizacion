Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class ConsultarEstadosDocumentoViewModel
    Implements INotifyPropertyChanged
    Private dcProxy As UtilidadesCFDomainContext

#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New UtilidadesCFDomainContext()
            Else
                dcProxy = New UtilidadesCFDomainContext(New System.Uri(Program.RutaServicioUtilidadesCF))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of UtilidadesCFDomainContext.IUtilidadesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsultarEstadosDocumentoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaEstadosDocumento As List(Of CFUtilidades.tblRN_ContadorReglasAutorizacion)
    Public Property ListaEstadosDocumento() As List(Of CFUtilidades.tblRN_ContadorReglasAutorizacion)
        Get
            Return _ListaEstadosDocumento
        End Get
        Set(ByVal value As List(Of CFUtilidades.tblRN_ContadorReglasAutorizacion))
            _ListaEstadosDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaEstadosDocumento"))
        End Set
    End Property

    Private _IDNumeroUnico As Integer
    Public Property IDNumeroUnico() As Integer
        Get
            Return _IDNumeroUnico
        End Get
        Set(ByVal value As Integer)
            _IDNumeroUnico = value
            If Not IsNothing(IDNumeroUnico) And IDNumeroUnico <> 0 Then
                MostrarNumeroUnico = Visibility.Visible
            Else
                MostrarNumeroUnico = Visibility.Collapsed
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDNumeroUnico"))
        End Set
    End Property


    Private _IDDocumento As Integer
    Public Property IDDocumento() As Integer
        Get
            Return _IDDocumento
        End Get
        Set(ByVal value As Integer)
            _IDDocumento = value
            If Not IsNothing(IDDocumento) And IDDocumento <> 0 Then
                MostrarIDDocumento = Visibility.Visible
            Else
                MostrarIDDocumento = Visibility.Collapsed
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDDocumento"))
        End Set
    End Property

    Private _Modulo As String
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modulo"))
        End Set
    End Property


    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _MostrarNumeroUnico As Visibility
    Public Property MostrarNumeroUnico() As Visibility
        Get
            Return _MostrarNumeroUnico
        End Get
        Set(ByVal value As Visibility)
            _MostrarNumeroUnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarNumeroUnico"))
        End Set
    End Property

    Private _MostrarIDDocumento As Visibility
    Public Property MostrarIDDocumento() As Visibility
        Get
            Return _MostrarIDDocumento
        End Get
        Set(ByVal value As Visibility)
            _MostrarIDDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarIDDocumento"))
        End Set
    End Property


#End Region

#Region "Metodos"

    Public Sub ConsultarEstadoDocumento()
        Try
            If Not IsNothing(IDNumeroUnico) And IDNumeroUnico <> 0 And Not IsNothing(IDDocumento) And IDDocumento <> 0 Then
                IsBusy = True
                If Not IsNothing(dcProxy.tblRN_ContadorReglasAutorizacions) Then
                    dcProxy.tblRN_ContadorReglasAutorizacions.Clear()
                End If

                dcProxy.Load(dcProxy.OYDPLUS_ConsultarEstadosDocumentoQuery(IDNumeroUnico, IDDocumento, Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEstado, String.Empty)
            Else
                If Not IsNothing(IDNumeroUnico) And IDNumeroUnico <> 0 Then
                    IsBusy = True
                    If Not IsNothing(dcProxy.tblRN_ContadorReglasAutorizacions) Then
                        dcProxy.tblRN_ContadorReglasAutorizacions.Clear()
                    End If

                    dcProxy.Load(dcProxy.OYDPLUS_ConsultarEstadosDocumentoQuery(IDNumeroUnico, 0, Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEstado, String.Empty)
                ElseIf Not IsNothing(IDDocumento) And IDDocumento <> 0 Then
                    IsBusy = True
                    If Not IsNothing(dcProxy.tblRN_ContadorReglasAutorizacions) Then
                        dcProxy.tblRN_ContadorReglasAutorizacions.Clear()
                    End If

                    dcProxy.Load(dcProxy.OYDPLUS_ConsultarEstadosDocumentoQuery(0, IDDocumento, Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEstado, String.Empty)
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar el estado del documento.", Me.ToString(), "ConsultarEstadoDocumento", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Resultados asincronicos"

    Private Sub TerminoConsultarEstado(ByVal lo As LoadOperation(Of CFUtilidades.tblRN_ContadorReglasAutorizacion))
        Try
            If lo.HasError = False Then
                ListaEstadosDocumento = lo.Entities.ToList

                For Each li In ListaEstadosDocumento
                    li.ComentarioAprobador = li.ComentarioAprobador.Replace("++", String.Format("{0}      -> ", vbCrLf))
                    li.ComentarioAprobador = li.ComentarioAprobador.Replace("*|", String.Format("{0}      -> ", vbCrLf))
                    li.ComentarioAprobador = li.ComentarioAprobador.Replace("|", String.Format("{0}   -> ", vbCrLf))
                Next

                If ListaEstadosDocumento.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El documento no tiene traza en la tabla de estados.", "Estados Documento", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recebir la consulta de los estados.", Me.ToString(), "TerminoConsultarEstado", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recebir la consulta de los estados.", Me.ToString(), "TerminoConsultarEstado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        IsBusy = False
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    

End Class
