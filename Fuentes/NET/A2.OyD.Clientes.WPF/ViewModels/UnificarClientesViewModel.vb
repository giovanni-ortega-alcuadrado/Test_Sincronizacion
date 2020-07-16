Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class UnificarClientesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaUnificar
    Dim dcProxy As ClientesDomainContext
    Public result As MessageBoxResult
    Dim Limpiar As Boolean = True

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ClientesDomainContext.IClientesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 480)
        Try
            If Not Program.IsDesignMode() Then
                'IsBusy = True
                'dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "FiltroInicial")
                'dcProxy1.Load(dcProxy1.TraerCuentasDecevalPorAgrupadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupadorPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "UnificarClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"
    Public Sub TerminoTraerNombre(ByVal lo As LoadOperation(Of OyDClientes.ConsultaNombresCliente))

        Try
            If Not lo.HasError Then

                ListaClientesUnificar = dcProxy.ConsultaNombresClientes
                If ListaClientesUnificar.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente No existe, se encuentra Inactivo o Pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CancelarEditarRegistro()
                    cancelar("Todos")
                Else
                    If lo.UserState = "R" Then
                        If ListaClientesUnificar(0).Respuesta = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("El cliente No existe, se encuentra Inactivo o Pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            CancelarEditarRegistro()
                            cancelar("Retira")
                        Else
                            unifica.NombreClienteR = ClientesUnificarSelected.Nombre
                            MyBase.CambioItem("unifica")
                            MyBase.CambioItem("NombreClienteR")
                        End If
                    ElseIf lo.UserState = "U" Then
                        If ListaClientesUnificar(0).Respuesta = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("El cliente No existe, se encuentra Inactivo o Pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            CancelarEditarRegistro()
                            cancelar("Unifica")
                        Else
                            unifica.NombreClienteU = ClientesUnificarSelected.Nombre
                            MyBase.CambioItem("unifica")
                            MyBase.CambioItem("NombreClienteU")
                        End If
                    End If


                    If Not IsNothing(unifica.NombreClienteR) And Not IsNothing(unifica.NombreClienteU) Then
                        Habilitaboton = True
                        MyBase.CambioItem("Habilitaboton")
                    Else
                        Habilitaboton = False
                        MyBase.CambioItem("Habilitaboton")
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del nombre del cliente.", _
                                                 Me.ToString(), "TerminoTraerNombre", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del nombre del cliente.", _
                                                 Me.ToString(), "TerminoTraerNombre", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False

    End Sub
    Public Sub TerminoUnificar(ByVal lo As LoadOperation(Of OyDClientes.ConsultaNombresCliente))

        Try
            If Not lo.HasError Then
                ListaClientesUnificar = dcProxy.ConsultaNombresClientes
                If ListaClientesUnificar.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente No existe, se encuentra Inactivo o Pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CancelarEditarRegistro()
                    cancelar("Todos")
                Else

                    If ListaClientesUnificar(0).Respuesta = -1 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La unificación del cliente fué exitosa", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    ElseIf ListaClientesUnificar(0).Respuesta = -2 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La unificación del cliente fallo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    ElseIf ListaClientesUnificar(0).Respuesta = -3 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La unificación no pudo borrar el cliente" & vbCrLf & "o no pudo actualizar ordenantes en los clientes", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    ElseIf ListaClientesUnificar(0).Respuesta = -4 Then
                        A2Utilidades.Mensajes.mostrarMensaje("La unificación no fue posible." _
                                                             & vbCrLf & _
                                                             "El cliente tiene este movimiento pendiente por aprobar:" _
                                                             & vbCrLf & _
                                                             ListaClientesUnificar(0).Nombre, _
                                                             Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("La unificación del cliente fallo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If
                    cancelar("Todos")
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de unificar los clientes.", _
                                                 Me.ToString(), "TerminoUnificar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de unificar los clientes.", _
                                             Me.ToString(), "TerminoUnificar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub
#End Region

#Region "Propiedades"

    Private _ListaClientesUnificar As EntitySet(Of OyDClientes.ConsultaNombresCliente)
    Public Property ListaClientesUnificar() As EntitySet(Of OyDClientes.ConsultaNombresCliente)
        Get
            Return _ListaClientesUnificar
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ConsultaNombresCliente))
            _ListaClientesUnificar = value
            If Not IsNothing(value) Then

                ClientesUnificarSelected = _ListaClientesUnificar.FirstOrDefault

            End If
            MyBase.CambioItem("ListaClientesUnificar")
        End Set
    End Property
    Private _ClientesUnificarSelected As OyDClientes.ConsultaNombresCliente
    Public Property ClientesUnificarSelected() As OyDClientes.ConsultaNombresCliente
        Get
            Return _ClientesUnificarSelected
        End Get
        Set(ByVal value As OyDClientes.ConsultaNombresCliente)
            _ClientesUnificarSelected = value
            If Not value Is Nothing Then


            End If
            MyBase.CambioItem("ClientesUnificarSelected")
        End Set
    End Property

    Private WithEvents _unifica As Unificar = New Unificar
    Public Property unifica() As Unificar
        Get
            Return _unifica
        End Get
        Set(ByVal value As Unificar)
            _unifica = value
            MyBase.CambioItem("unifica")
        End Set
    End Property

    Private _Habilitaboton As Boolean = False
    Public Property Habilitaboton() As Boolean
        Get
            Return _Habilitaboton
        End Get
        Set(ByVal value As Boolean)
            _Habilitaboton = value
            MyBase.CambioItem("Habilitaboton")
        End Set
    End Property

#End Region

#Region "Métodos"

    Private Sub _unifica_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _unifica.PropertyChanged
        Try
            If Limpiar = True Then
                If e.PropertyName.Equals("ClienteRetira") Then
                    If unifica.ClienteRetira.Trim() <> "0" And unifica.ClienteRetira <> String.Empty Then
                        unifica.Accion = CType("B", Char?)
                        IsBusy = True
                        dcProxy.ConsultaNombresClientes.Clear()
                        dcProxy.Load(dcProxy.TraerUnificarClientesQuery(unifica.ClienteRetira, unifica.Accion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNombre, "R")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente No existe, se encuentra Inactivo o Pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        CancelarEditarRegistro()
                        cancelar("Retira")
                    End If
                ElseIf e.PropertyName.Equals("ClienteUnifica") Then
                    If IsNothing(unifica.ClienteRetira) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Digite primero la cuenta a retirar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        CancelarEditarRegistro()
                        Exit Sub
                    End If
                    If ((unifica.ClienteUnifica.Trim().Equals("0")) And (unifica.ClienteUnifica.Trim <> "0" Or unifica.ClienteRetira.Trim = "0")) Then
                        If Editando = True Then
                            A2Utilidades.Mensajes.mostrarMensaje("Digite primero la cuenta a retirar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            CancelarEditarRegistro()
                        End If
                    ElseIf unifica.ClienteUnifica.Trim <> "0" Or unifica.ClienteUnifica <> String.Empty Then
                        unifica.Accion = CType("B", Char?)
                        IsBusy = True
                        dcProxy.ConsultaNombresClientes.Clear()
                        dcProxy.Load(dcProxy.TraerUnificarClientesQuery(unifica.ClienteUnifica, unifica.Accion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNombre, "U")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente No existe, se encuentra Inactivo o Pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        CancelarEditarRegistro()
                        cancelar("Unifica")
                    End If

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los clientes.", _
                                             Me.ToString(), "unifica_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Sub ActualizaRegistro()

        If IsNothing(unifica.ClienteRetira) Or unifica.ClienteRetira = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente a Retirar No existe o está Inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            cancelar("Retira")
            Exit Sub
        ElseIf IsNothing(unifica.ClienteUnifica) Or unifica.ClienteUnifica = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente a Unificar No existe o está Inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            cancelar("Unifica")
            Exit Sub
        Else
            Dim strMensaje As String = String.Empty
            strMensaje &= "Realmente desea unificar el cliente" & vbCrLf
            strMensaje &= unifica.NombreClienteR & vbCrLf
            strMensaje &= "con el cliente" & vbCrLf
            strMensaje &= unifica.NombreClienteU

            'C1.Silverlight.C1MessageBox.Show(strMensaje, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminopregunta)
            mostrarMensajePregunta(strMensaje, _
                                   Program.TituloSistema, _
                                   "ACTUALIZARREGISTRO", _
                                   AddressOf Terminopregunta, True, "¿Unificar clientes?")
        End If


    End Sub

    Private Sub Terminopregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If (unifica.ClienteRetira = unifica.ClienteUnifica) Then
                A2Utilidades.Mensajes.mostrarMensaje("Los códigos de los clientes deben ser diferentes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                cancelar("Todos")
                Exit Sub
            End If
            Try
                ListaClientesUnificar.Clear()
                IsBusy = True
                unifica.Accion = "U"
                dcProxy.Load(dcProxy.UnificarClientesQuery(unifica.ClienteRetira, unifica.Accion, unifica.ClienteUnifica, Program.Usuario, Program.HashConexion), AddressOf TerminoUnificar, "")
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                     Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try

        Else
            cancelar("Todos")
            IsBusy = False
        End If
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "insert" Then
                unifica.Accion = ""
                unifica.ClienteRetira = Nothing
                unifica.ClienteUnifica = Nothing
                unifica.NombreClienteR = ""
                unifica.NombreClienteU = ""

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub cancelar(ByVal accion As String)


        If accion = "Retira" Then
            Limpiar = False
            unifica.ClienteRetira = String.Empty
            unifica.NombreClienteR = String.Empty
        ElseIf accion = "Unifica" Then
            Limpiar = False
            unifica.ClienteUnifica = String.Empty
            unifica.NombreClienteU = String.Empty
        Else
            Limpiar = False
            unifica.ClienteRetira = String.Empty
            unifica.ClienteUnifica = String.Empty
            unifica.NombreClienteR = String.Empty
            unifica.NombreClienteU = String.Empty
        End If

        ErrorForma = ""
        unifica.Accion = ""
        Editando = False
        Habilitaboton = False
        Limpiar = True
        MyBase.CambioItem("ClienteRetira")
        MyBase.CambioItem("ClienteUnifica")
        MyBase.CambioItem("NombreClienteR")
        MyBase.CambioItem("NombreClienteU")
        MyBase.CambioItem("Habilitaboton")
        MyBase.CambioItem("Editando")
    End Sub
#End Region
End Class

Public Class Unificar
    Implements INotifyPropertyChanged
    '<StringLength(15, ErrorMessage:="La longitud máxima es de 17")> _
    Private _ClienteRetira As String
    <Display(Name:="Cliente a retirar")> _
    Public Property ClienteRetira As String
        Get
            Return _ClienteRetira
        End Get
        Set(ByVal value As String)
            _ClienteRetira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClienteRetira"))
        End Set
    End Property

    Private _ClienteUnifica As String
    <Display(Name:="Cliente a unificar")> _
    Public Property ClienteUnifica As String
        Get
            Return _ClienteUnifica
        End Get
        Set(ByVal value As String)
            _ClienteUnifica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClienteUnifica"))
        End Set
    End Property

    Private _NombreClienteR As String
    <Display(Name:="Nombre")> _
    Public Property NombreClienteR As String
        Get
            Return _NombreClienteR
        End Get
        Set(ByVal value As String)
            _NombreClienteR = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreClienteR"))
        End Set
    End Property

    Private _NombreClienteU As String
    <Display(Name:="Nombre")> _
    Public Property NombreClienteU As String
        Get
            Return _NombreClienteU
        End Get
        Set(ByVal value As String)
            _NombreClienteU = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreClienteU"))
        End Set
    End Property

    <Display(Name:="Accion")> _
    Public Property Accion As Char


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class


Public Class CamposBusquedaUnificar

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente", Description:="Comitente")> _
    Public Property Comitente As String
End Class