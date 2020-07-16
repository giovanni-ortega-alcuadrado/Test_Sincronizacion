Imports Telerik.Windows.Controls
'-------------------------------------------------------------------------------------
'Descripción:       ViewModel para la pantalla de asignación de DTS a usuarios
'Desarrollado por:  Santiago Alexander Vergara Orrego
'Fecha:             Octubre 30/2013
'--------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports System.Text
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Public Class AutorizarUsuariosDTSViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As UtilidadesDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.cargarCombosEspecificosQuery("AUTORIZARDTS", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "AUTORIZARDTS")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "AutorizarUsuariosDTSViewModel.New", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub

#Region "Declaraciones y Propiedades"

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            CambioPropiedad("IsBusy")
        End Set
    End Property

    Private _intIdDTS As Integer
    Public Property intIdDTS() As Integer
        Get
            Return _intIdDTS
        End Get
        Set(ByVal value As Integer)
            _intIdDTS = value

            If Not IsNothing(value) Then
                ConsultarUsuariosAutorizados()
            End If

            CambioPropiedad("intIdDTS")
        End Set
    End Property

    Private _objUsuario As OYDUtilidades.ItemCombo
    Public Property objUsuario() As OYDUtilidades.ItemCombo
        Get
            Return _objUsuario
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _objUsuario = value
            CambioPropiedad("objUsuario")
        End Set
    End Property

    Private _objUsuarioAutorizado As OYDUtilidades.ItemCombo
    Public Property objUsuarioAutorizado() As OYDUtilidades.ItemCombo
        Get
            Return _objUsuarioAutorizado
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _objUsuarioAutorizado = value
            CambioPropiedad("objUsuarioAutorizado")
        End Set
    End Property

    Private _lstUsuariosInicial As New List(Of OYDUtilidades.ItemCombo)

    Private _lstUsuarios As New ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property lstUsuarios() As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _lstUsuarios
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _lstUsuarios = value
            CambioPropiedad("lstUsuarios")
        End Set
    End Property

    Private _lstUsuariosAutorizados As New ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property lstUsuariosAutorizados() As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _lstUsuariosAutorizados
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _lstUsuariosAutorizados = value
            CambioPropiedad("lstUsuariosAutorizados")
        End Set
    End Property

    Private _lstDTS As New List(Of OYDUtilidades.ItemCombo)
    Public Property lstDTS() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _lstDTS
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _lstDTS = value
            CambioPropiedad("lstDTS")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Function AddRange(ByVal objListaInicial As ObservableCollection(Of OYDUtilidades.ItemCombo), ByVal objListaRango As ObservableCollection(Of OYDUtilidades.ItemCombo)) As ObservableCollection(Of OYDUtilidades.ItemCombo)

        For Each Objeto In objListaRango
            objListaInicial.Add(Objeto)
        Next

        Return objListaInicial

    End Function


    Public Function ListToObservableCollection(ByVal objLista As List(Of OYDUtilidades.ItemCombo)) As ObservableCollection(Of OYDUtilidades.ItemCombo)

        Dim objCollection As New ObservableCollection(Of OYDUtilidades.ItemCombo)

        For Each Objeto In objLista
            objCollection.Add(Objeto)
        Next

        Return objCollection

    End Function

    Public Sub ConsultarUsuariosAutorizados()
        Try
            IsBusy = True
            dcProxy.Load(dcProxy.cargarCombosCondicionalQuery("USUARIOS_DTS_AUTORIZADOS", Nothing, _intIdDTS, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "USUARIOS_DTS_AUTORIZADOS")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llamar la consulta de usuarios autorizados", Me.ToString(), "ConsultarUsuariosAutorizados", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub GuardarCambios()
        Dim strUsuariosConcatenados As String = String.Empty

        Try

            For Each objeto In lstUsuariosAutorizados

                If strUsuariosConcatenados = String.Empty Then
                    strUsuariosConcatenados = objeto.Descripcion
                Else
                    strUsuariosConcatenados = strUsuariosConcatenados & "|" & objeto.Descripcion
                End If
            Next

            IsBusy = True
            dcProxy.UsuariosDTS_Actualizar(_intIdDTS, strUsuariosConcatenados, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizar_UsuariosDTS, "grabar")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los usuarios autorizados", Me.ToString(), "GuardarCambios", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Public Sub AgregarUno()
        Try
            If Not IsNothing(objUsuario) Then
                lstUsuariosAutorizados.Add(objUsuario)
                lstUsuarios.Remove(objUsuario)
                objUsuario = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al agregar un registro", Me.ToString(), "AgregarUno", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub QuitarUno()
        Try
            If Not IsNothing(objUsuarioAutorizado) Then
                lstUsuarios.Add(objUsuarioAutorizado)
                lstUsuariosAutorizados.Remove(objUsuarioAutorizado)
                objUsuarioAutorizado = Nothing
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al quitar un registro", Me.ToString(), "QuitarUno", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub AgregarTodos()
        Try
            If Not IsNothing(lstUsuarios) Then
                lstUsuariosAutorizados = AddRange(lstUsuariosAutorizados, lstUsuarios)
                lstUsuarios = New ObservableCollection(Of OYDUtilidades.ItemCombo)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al agregar todos los registros a la lista", Me.ToString(), "AgregarTodos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub QuitarTodos()
        Try
            If Not IsNothing(lstUsuariosAutorizados) Then
                lstUsuarios = AddRange(lstUsuarios, lstUsuariosAutorizados)
                lstUsuariosAutorizados = New ObservableCollection(Of OYDUtilidades.ItemCombo)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al quitar todos los registros de la lista", Me.ToString(), "QuitarTodos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoActualizar_UsuariosDTS(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not lo.HasError Then
                A2Utilidades.Mensajes.mostrarMensaje("Los cambios se grabaron exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los usuarios DTS", _
                                                   Me.ToString(), "TerminoActualizar_UsuariosDTS" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de actualizar los usuarios DTS", Me.ToString(), "TerminoActualizar_UsuariosDTS", Application.Current.ToString(), Program.Maquina, ex)

        End Try
        IsBusy = False
    End Sub


    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Dim _lstUsuariosTemporal As New List(Of OYDUtilidades.ItemCombo)
        Try
            If Not lo.HasError Then


                Select Case lo.UserState
                    Case "AUTORIZARDTS"
                        If dcProxy.ItemCombos.Count > 0 Then
                            lstDTS = (From c In dcProxy.ItemCombos Where c.Categoria = "DTS" Select c).ToList

                            intIdDTS = (From c In dcProxy.ItemCombos Where c.Categoria = "DTS" Select c.ID).ToList.FirstOrDefault

                            _lstUsuariosInicial = (From c In dcProxy.ItemCombos Where c.Categoria = "USUARIOS_APLICACION" Select c).ToList
                        End If

                    Case "USUARIOS_DTS_AUTORIZADOS"

                        If dcProxy.ItemCombos.Count > 0 Then
                            lstUsuariosAutorizados = ListToObservableCollection((From c In dcProxy.ItemCombos Where c.Categoria = "USUARIOS_DTS_AUTORIZADOS" Select c).ToList)
                        Else
                            lstUsuariosAutorizados = New ObservableCollection(Of OYDUtilidades.ItemCombo)

                        End If

                        _lstUsuariosTemporal = _lstUsuariosInicial

                        For Each objeto In (From c In dcProxy.ItemCombos Where c.Categoria = "USUARIOS_DTS_AUTORIZADOS" Select c).ToList

                            _lstUsuariosTemporal = (From o In _lstUsuariosTemporal Where o.Descripcion <> objeto.Descripcion Select o).ToList


                        Next

                        lstUsuarios = ListToObservableCollection(_lstUsuariosTemporal)

                End Select

                dcProxy.ItemCombos.Clear()

            Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                 Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                       Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

#Region "Notificar Cambio"

    Public Sub CambioPropiedad(ByVal strPropiedad As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(strPropiedad))

    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

End Class






