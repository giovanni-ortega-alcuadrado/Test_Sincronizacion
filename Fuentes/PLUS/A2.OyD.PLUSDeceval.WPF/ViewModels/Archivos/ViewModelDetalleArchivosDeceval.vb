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

Public Class ViewModelDetalleArchivosDeceval
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "DetalleArchivos.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Consultar()
        Try
            IsBusy = True
            If Not IsNothing(dcProxy.DetalleArchivos) Then
                dcProxy.DetalleArchivos.Clear()
            End If

            'modificar aca metodo en el domain services
            dcProxy.Load(dcProxy.DetalleArchivosConsultarQuery(intIDArchivo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRegistros, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista.", _
                                             Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


#Region "Propiedades"
    Private _intIDArchivo As Integer
    Public Property intIDArchivo() As Integer
        Get
            Return _intIDArchivo
        End Get
        Set(ByVal value As Integer)
            _intIDArchivo = value
            MyBase.CambioItem("intIDArchivo")
        End Set
    End Property

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private _UsuarioArchivo As String = String.Empty
    Public Property UsuarioArchivo() As String
        Get
            Return _UsuarioArchivo
        End Get
        Set(ByVal value As String)
            _UsuarioArchivo = value
            MyBase.CambioItem("UsuarioArchivo")
        End Set
    End Property

    Private _GeneracionAutomatica As Boolean = False
    Public Property GeneracionAutomatica() As Boolean
        Get
            Return _GeneracionAutomatica
        End Get
        Set(ByVal value As Boolean)
            _GeneracionAutomatica = value
            MyBase.CambioItem("GeneracionAutomatica")
        End Set
    End Property

    Private _ListaDetalleArchivos As List(Of DetalleArchivos)
    Public Property ListaDetalleArchivos() As List(Of DetalleArchivos)
        Get
            Return _ListaDetalleArchivos
        End Get
        Set(ByVal value As List(Of DetalleArchivos))
            _ListaDetalleArchivos = value
            MyBase.CambioItem("ListaDetalleArchivos")
        End Set
    End Property

    Private _DetalleArchivosSelected As DetalleArchivos
    Public Property DetalleArchivosSelected() As DetalleArchivos
        Get
            Return _DetalleArchivosSelected
        End Get
        Set(ByVal value As DetalleArchivos)
            _DetalleArchivosSelected = value
            MyBase.CambioItem("DetalleArchivosSelected")
        End Set
    End Property

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoTraerRegistros(lo As LoadOperation(Of DetalleArchivos))
        Try
            If Not lo.HasError Then
                ListaDetalleArchivos = lo.Entities.ToList()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos", _
                                                     Me.ToString(), "TerminaCargarListaClientesInactivar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos", _
                                             Me.ToString(), "TerminaCargarListaClientesInactivar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub





#End Region

End Class
