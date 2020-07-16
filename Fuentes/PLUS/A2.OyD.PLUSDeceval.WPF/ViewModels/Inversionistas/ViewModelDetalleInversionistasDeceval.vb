Imports Telerik.Windows.Controls
'Codigo Creado Por: Carlos Andres Toro
'Archivo: DetalleInversionistasDeceval.vb
'Generado el : 05/03/2015 
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
Imports OpenRiaServices.DomainServices.Client

Public Class ViewModelDetalleInversionistasDeceval
    Inherits A2ControlMenu.A2ViewModel
    Private dcProxy As OYDPLUSDecevalDomainContext
#Region "Metodos"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New OYDPLUSDecevalDomainContext
        Else
            dcProxy = New OYDPLUSDecevalDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        Try
            If Not Program.IsDesignMode() Then

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "DetalleInversionistas.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Consultar()
        Try
            IsBusy = True
            If Not IsNothing(dcProxy.DetalleInversionistas) Then
                dcProxy.DetalleInversionistas.Clear()
            End If

            dcProxy.Load(dcProxy.DetalleInversionistasConsultarQuery(IDInversionista, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRegistros, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista.",
                                             Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


#Region "Propiedades"
    Private _IDInversionista As Integer
    Public Property IDInversionista() As Integer
        Get
            Return _IDInversionista
        End Get
        Set(ByVal value As Integer)
            _IDInversionista = value
            MyBase.CambioItem("IDInversionista")
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

    Private _ListaDetalleInversionistas As List(Of DetalleInversionistas)
    Public Property ListaDetalleInversionistas() As List(Of DetalleInversionistas)
        Get
            Return _ListaDetalleInversionistas
        End Get
        Set(ByVal value As List(Of DetalleInversionistas))
            _ListaDetalleInversionistas = value
            MyBase.CambioItem("ListaDetalleInversionistas")
        End Set
    End Property

    Private _DetalleInversionistasSelected As DetalleInversionistas
    Public Property DetalleInversionistasSelected() As DetalleInversionistas
        Get
            Return _DetalleInversionistasSelected
        End Get
        Set(ByVal value As DetalleInversionistas)
            _DetalleInversionistasSelected = value
            MyBase.CambioItem("DetalleInversionistasSelected")
        End Set
    End Property

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoTraerRegistros(lo As LoadOperation(Of DetalleInversionistas))
        Try
            If Not lo.HasError Then
                ListaDetalleInversionistas = lo.Entities.ToList()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes a Inactivar",
                                                     Me.ToString(), "TerminaCargarListaClientesInactivar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes a Inactivar",
                                             Me.ToString(), "TerminaCargarListaClientesInactivar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub





#End Region

End Class
