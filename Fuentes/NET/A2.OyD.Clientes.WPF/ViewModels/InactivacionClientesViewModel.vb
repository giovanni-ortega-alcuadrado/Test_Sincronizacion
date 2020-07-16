Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: InactivacionClientesViewModel.vb
'Generado el : 07/28/2011 07:51:00AM
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes


Public Class InactivacionClientesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"
    Private ClientePorDefecto As OyDClientes.ListaClientesInactivar
    Private GrupoDefecto As Integer = -1
    Private SucursalDefecto As Integer = -1
    Private Const TextoDefectoTodos As String = "(Todos)"
    Private dcProxy As ClientesDomainContext
#End Region

#Region "Procedimientos"
    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Try
            If Not Program.IsDesignMode() Then
                GrupoSeleccionado.ID = -1
                SucursalSeleccionada.ID = -1
                _GrupoSeleccionado.Descripcion = TextoDefectoTodos
                MostrarProgreso = Visibility.Collapsed
                Call ConsultarListado()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "InactivacionClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaCargarListaClientesInactivar(lo As LoadOperation(Of ListaClientesInactivar))
        Try
            If Not lo.HasError Then
                ListaClientes = dcProxy.ListaClientesInactivars
                ClientePorDefecto = lo.Entities.FirstOrDefault



                If GrupoSeleccionado.ID = -1 Or SucursalSeleccionada.ID = -1 Then
                    _ListaParaMostrar.Clear()
                    For Each licli In _ListaClientes
                        _ListaParaMostrar.Add(New ListaClientesParaInactivar With {.Borrar = True,
                                                                               .Codigo = licli.lngId,
                                                                               .Nombre = licli.strNombre,
                                                                               .Grupo = licli.Grupo})
                    Next
                Else
                    _ListaParaMostrar.Clear()
                    For Each licli In _ListaClientes
                        _ListaParaMostrar.Add(New ListaClientesParaInactivar With {.Borrar = False,
                                                                                   .Codigo = licli.lngId,
                                                                                   .Nombre = licli.strNombre,
                                                                                   .Grupo = licli.Grupo})
                    Next
                End If

                MyBase.CambioItem("ListaClientesPaged")
                MyBase.CambioItem("ListaClientes")
                MyBase.CambioItem("GrupoSeleccionado")
                MyBase.CambioItem("SucursalSeleccionada")

                IsBusy = False
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes a Inactivar", _
                                                     Me.ToString(), "TerminaCargarListaClientesInactivar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes a Inactivar", _
                                             Me.ToString(), "TerminaCargarListaClientesInactivar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarListado()
        IsBusy = True
        dcProxy.ListaClientesInactivars.Clear()
        dcProxy.Load(dcProxy.ListaClientesInactivarQuery(GrupoSeleccionado.ID, SucursalSeleccionada.ID, Program.Usuario, Program.HashConexion), AddressOf TerminaCargarListaClientesInactivar, "")
    End Sub

    Public Sub EjecutarActualizacion(pbolTodos As Boolean)
        Try
            Dim strClientesInactivar As String = String.Empty

            If pbolTodos Then
                IsBusy = True
                TotalRegistros = ListaClientes.Count
                For Each Cliente In ListaClientes
                    If String.IsNullOrEmpty(strClientesInactivar) Then
                        strClientesInactivar = Cliente.lngId
                    Else
                        strClientesInactivar = String.Format("{0},{1}", strClientesInactivar, Cliente.lngId)
                    End If
                Next

                If Not IsNothing(dcProxy.RespuestaInactivacionClientes) Then
                    dcProxy.RespuestaInactivacionClientes.Clear()
                End If

                dcProxy.Load(dcProxy.InactivarClienteQuery(strClientesInactivar, Program.Usuario, Program.HashConexion), AddressOf TerminoInactivarClientes, String.Empty)
            Else

                Dim intRegSeleccionados = Aggregate nreg In _ListaParaMostrar Where nreg.Borrar = True Into Count(nreg.Borrar)

                If intRegSeleccionados <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No ha seleccionado ningún cliente para su inactivación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    IsBusy = True
                    TotalRegistros = _ListaParaMostrar.Count
                    For Each Cliente In _ListaParaMostrar
                        If Cliente.Borrar Then
                            If String.IsNullOrEmpty(strClientesInactivar) Then
                                strClientesInactivar = Cliente.Codigo
                            Else
                                strClientesInactivar = String.Format("{0},{1}", strClientesInactivar, Cliente.Codigo)
                            End If
                        End If
                    Next

                    If Not IsNothing(dcProxy.RespuestaInactivacionClientes) Then
                        dcProxy.RespuestaInactivacionClientes.Clear()
                    End If

                    dcProxy.Load(dcProxy.InactivarClienteQuery(strClientesInactivar, Program.Usuario, Program.HashConexion), AddressOf TerminoInactivarClientes, String.Empty)

                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar los clientes.", _
                                             Me.ToString(), "EjecutarActualizacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub LimpiarGrid()
        If Not IsNothing(_ListaParaMostrar) Then
            IsBusy = True
            _ListaParaMostrar.Clear()
            MyBase.CambioItem("ListaClientesPaged")
            IsBusy = False
        End If
    End Sub

    Private Sub TerminoInactivarClientes(ByVal lo As LoadOperation(Of OyDClientes.RespuestaInactivacionClientes))
        Try
            If lo.HasError = False Then
                A2Utilidades.Mensajes.mostrarMensaje("El proceso de inactivación de clientes terminó satisfactoriamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                Call ConsultarListado()
                MostrarProgreso = Visibility.Collapsed
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inactivación de clientes", _
                                                 Me.ToString(), "TerminoInactivarClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inactivación de clientes", _
                                             Me.ToString(), "TerminoInactivarClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades"

    Private _GrupoSeleccionado As New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo
    Public Property GrupoSeleccionado() As OYDUtilidades.ItemCombo
        Get
            If _GrupoSeleccionado.ID Is Nothing Then
                _GrupoSeleccionado.ID = GrupoDefecto

            End If
            Return _GrupoSeleccionado
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _GrupoSeleccionado = value
            MyBase.CambioItem("GrupoSeleccionado")
        End Set
    End Property

    Private _SucursalSeleccionada As New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo
    Public Property SucursalSeleccionada() As OYDUtilidades.ItemCombo
        Get
            If _SucursalSeleccionada.ID Is Nothing Then
                _SucursalSeleccionada.ID = SucursalDefecto
                _SucursalSeleccionada.Descripcion = TextoDefectoTodos
            End If
            Return _SucursalSeleccionada
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _SucursalSeleccionada = value
            MyBase.CambioItem("SucursalSeleccionada")
        End Set
    End Property

    Private _MostrarProgreso As Visibility
    Public Property MostrarProgreso() As Visibility
        Get
            Return _MostrarProgreso
        End Get
        Set(ByVal value As Visibility)
            _MostrarProgreso = value
            MyBase.CambioItem("MostrarProgreso")
        End Set
    End Property

    Private _TotalRegistros As Integer
    Public Property TotalRegistros() As Integer
        Get
            Return _TotalRegistros
        End Get
        Set(ByVal value As Integer)
            _TotalRegistros = value
            MyBase.CambioItem("TotalRegistros")
        End Set
    End Property

    Private _PorcProgreso As Double
    Public Property PorcProgreso() As Double
        Get
            Return _PorcProgreso
        End Get
        Set(ByVal value As Double)
            _PorcProgreso = value
            MyBase.CambioItem("PorcProgreso")
        End Set
    End Property

    Private _ListaClientes As EntitySet(Of ListaClientesInactivar)
    Public Property ListaClientes() As EntitySet(Of ListaClientesInactivar)
        Get
            Return _ListaClientes
        End Get
        Set(ByVal value As EntitySet(Of ListaClientesInactivar))
            _ListaClientes = value
            MyBase.CambioItem("ListaClientes")
        End Set
    End Property

    Private _ListaParaMostrar As New List(Of ListaClientesParaInactivar)
    Public ReadOnly Property ListaParaMostrar() As List(Of ListaClientesParaInactivar)
        Get
            Return _ListaParaMostrar
        End Get
    End Property

    Public ReadOnly Property ListaClientesPaged() As PagedCollectionView
        Get
            If Not IsNothing(ListaParaMostrar) Then
                Dim view = New PagedCollectionView(ListaParaMostrar)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

End Class

Public Class ListaClientesParaInactivar


    <Display(Name:="Borrar", Description:="Borrar")> _
    Public Property Borrar As String

    <StringLength(30, ErrorMessage:="La longitud máxima es de 30")> _
     <Display(Name:="Código", Description:="Código")> _
    Public Property Codigo As String

    <Display(Name:="Nombre", Description:="Nombre")> _
    Public Property Nombre As String

    <Display(Name:="Grupo", Description:="Grupo")> _
    Public Property Grupo As String


End Class
