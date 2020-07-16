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
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports A2Utilidades.Mensajes

Public Class MensajeriaCadenaViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ConfiguracionAnterior As ConfiguracionMensajeriaCadena
    Dim dcProxy As BolsaDomainContext
    Dim IdItemActualizar As Integer
    Dim logActualizarSelected As Boolean
    Dim logCambiarPropiedades As Boolean = True

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New BolsaDomainContext()
            Else
                dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.MensajeriaCadenaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoMensajeriaCadena, " ")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                Me.ToString(), "MensajeriaCadenaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados asincronicos"
    Public Sub TerminoMensajeriaCadena(ByVal lo As LoadOperation(Of ConfiguracionMensajeriaCadena))
        Try
            If Not lo.HasError Then

                If lo.UserState = "TerminoGuardar" Then
                    logActualizarSelected = False
                End If

                ListaConfiguracion = dcProxy.ConfiguracionMensajeriaCadenas

                logActualizarSelected = True

                If dcProxy.ConfiguracionMensajeriaCadenas.Count > 0 Then
                    If lo.UserState = "TerminoGuardar" Then
                        If ListaConfiguracion.Where(Function(i) i.IDConfiguracionMensajeria = IdItemActualizar).Count > 0 Then
                            ConfiguracionSelected = ListaConfiguracion.Where(Function(i) i.IDConfiguracionMensajeria = IdItemActualizar).FirstOrDefault
                        Else
                            ConfiguracionSelected = ListaConfiguracion.First
                        End If
                    End If
                Else

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de clientes", _
                                                 Me.ToString(), "TerminoTraerClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "SSe presentó un problema en la obtención de la lista de Configuracion Facturas", _
                                Me.ToString(), "TerminoTraerConfiguracionFacturas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades"
    Private _ListaConfiguracion As EntitySet(Of ConfiguracionMensajeriaCadena)
    Public Property ListaConfiguracion() As EntitySet(Of ConfiguracionMensajeriaCadena)
        Get
            Return _ListaConfiguracion
        End Get
        Set(ByVal value As EntitySet(Of ConfiguracionMensajeriaCadena))
            _ListaConfiguracion = value

            MyBase.CambioItem("ListaConfiguracion")
            MyBase.CambioItem("ListaConfiguracionPaged")
            If Not IsNothing(_ListaConfiguracion) Then
                If ListaConfiguracion.Count > 0 And logActualizarSelected Then
                    _ConfiguracionSelected = ListaConfiguracion.FirstOrDefault
                Else
                    _ConfiguracionSelected = Nothing
                End If
            Else
                _ConfiguracionSelected = Nothing
            End If
        End Set
    End Property
    Public ReadOnly Property ListaConfiguracionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConfiguracion) Then
                Dim view = New PagedCollectionView(_ListaConfiguracion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ConfiguracionSelected As ConfiguracionMensajeriaCadena
    Public Property ConfiguracionSelected() As ConfiguracionMensajeriaCadena
        Get
            Return _ConfiguracionSelected
        End Get
        Set(ByVal value As ConfiguracionMensajeriaCadena)
            _ConfiguracionSelected = value
            MyBase.CambioItem("ConfiguracionSelected")
        End Set
    End Property


#End Region

#Region "Metodos"

    'Private Function Validaciones() As Boolean
    '    Try

    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición de un registro", _
    '                                                     Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
    '        Validaciones = True
    '    End Try
    'End Function

    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_ConfiguracionSelected) Then
                Editando = True
                MyBase.CambioItem("Editando")
                ObtenerRegistroAnterior()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la edición de un registro", _
                                                         Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConfiguracionSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                ConfiguracionSelected = ConfiguracionAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la cancelacion de un registro", _
                                                         Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            Dim origen = "ActualizarRegistro"
            ErrorForma = ""
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            IsBusy = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objConfiguracion As New ConfiguracionMensajeriaCadena
            If Not IsNothing(_ConfiguracionSelected) Then
                objConfiguracion.Completa = _ConfiguracionSelected.Completa
                objConfiguracion.Ninguna = _ConfiguracionSelected.Ninguna
                objConfiguracion.Parcial = _ConfiguracionSelected.Parcial
                objConfiguracion.VIP = _ConfiguracionSelected.VIP
            End If

            ConfiguracionAnterior = Nothing
            ConfiguracionAnterior = objConfiguracion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la configuracion anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("Errorpersonalizado,") = True) Then
                        Dim Mensaje1 = Split(So.Error.Message, "Errorpersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                        Exit Sub
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
            IdItemActualizar = 0
            Editando = False


            If So.UserState = "ActualizarRegistro" Then
                If Not IsNothing(_ConfiguracionSelected) Then
                    IdItemActualizar = _ConfiguracionSelected.IDConfiguracionMensajeria
                End If
            End If

            dcProxy.ConfiguracionFacturas.Clear()
            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.MensajeriaCadenaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoMensajeriaCadena, "TerminoGuardar")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub __ConfiguracionSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ConfiguracionSelected.PropertyChanged


    End Sub
#End Region
End Class
