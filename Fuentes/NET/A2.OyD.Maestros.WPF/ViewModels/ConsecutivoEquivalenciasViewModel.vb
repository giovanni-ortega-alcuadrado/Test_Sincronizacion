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
Imports Microsoft.VisualBasic.CompilerServices

Public Class ConsecutivoEquivalenciasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaConsecutivosDocumento
    Private ConsecutivosEquivalenciasPorDefecto As TesoreriaConsecutivosEquivalencias
    Private ConsecutivosEquivalenciasAnterior As TesoreriaConsecutivosEquivalencias
    Public Property _mlogNuevo As Boolean = False
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)


#Region "Variables"

#End Region


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.TesoreriaConsecutivosEquivalenciasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosEquivalencias, "FiltroInicial")
                editar = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsecutivosDocumentosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"


    Private Sub TerminoTraerConsecutivosEquivalencias(ByVal lo As LoadOperation(Of TesoreriaConsecutivosEquivalencias))
        If Not lo.HasError Then
            ListaConsecutivosEquivalencias = dcProxy.TesoreriaConsecutivosEquivalencias
            If dcProxy.TesoreriaConsecutivosEquivalencias.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConsecutivosEquivalenciasSelected = ListaConsecutivosEquivalencias.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConsecutivosEquivalencias", _
                                             Me.ToString(), "TerminoTraerConsecutivosEquivalencias", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaConsecutivosEquivalencias As EntitySet(Of TesoreriaConsecutivosEquivalencias)
    Public Property ListaConsecutivosEquivalencias() As EntitySet(Of TesoreriaConsecutivosEquivalencias)
        Get
            Return _ListaConsecutivosEquivalencias
        End Get
        Set(ByVal value As EntitySet(Of TesoreriaConsecutivosEquivalencias))
            _ListaConsecutivosEquivalencias = value

            MyBase.CambioItem("ListaConsecutivosEquivalencias")
            MyBase.CambioItem("ListaConsecutivosEquivalenciasPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConsecutivosEquivalenciasAnterior) Then
                    ConsecutivosEquivalenciasSelected = _ListaConsecutivosEquivalencias.FirstOrDefault
                Else
                    ConsecutivosEquivalenciasSelected = ConsecutivosEquivalenciasAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConsecutivosEquivalenciasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConsecutivosEquivalencias) Then
                Dim view = New PagedCollectionView(_ListaConsecutivosEquivalencias)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ConsecutivosEquivalenciasSelected As TesoreriaConsecutivosEquivalencias
    Public Property ConsecutivosEquivalenciasSelected() As TesoreriaConsecutivosEquivalencias
        Get
            Return _ConsecutivosEquivalenciasSelected
        End Get
        Set(ByVal value As TesoreriaConsecutivosEquivalencias)
            _ConsecutivosEquivalenciasSelected = value
            MyBase.CambioItem("ConsecutivosEquivalenciasSelected")
        End Set
    End Property

    Private _editar As Boolean = False
    Public Property editar() As Boolean
        Get
            Return _editar
        End Get
        Set(ByVal value As Boolean)
            _editar = value
            MyBase.CambioItem("editar")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Dim NewConsecutivosEquivalencias As New TesoreriaConsecutivosEquivalencias
        NewConsecutivosEquivalencias.NombreConsecutivoRC = Nothing
        NewConsecutivosEquivalencias.NombreConsecutivoCE = Nothing
        NewConsecutivosEquivalencias.NombreConsecutivoNotas = Nothing
        NewConsecutivosEquivalencias.UsuarioInsercion = Program.Usuario
        ConsecutivosEquivalenciasAnterior = ConsecutivosEquivalenciasSelected
        ConsecutivosEquivalenciasSelected = NewConsecutivosEquivalencias
        editar = True
        MyBase.CambioItem("TesoreriaConsecutivosEquivalencias")
        _mlogNuevo = True
        Editando = True

    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            If ValidarRegistro() Then
                ConsecutivosEquivalenciasAnterior = ConsecutivosEquivalenciasSelected
                If Not ListaConsecutivosEquivalencias.Contains(ConsecutivosEquivalenciasSelected) Then
                    origen = "insert"
                    ListaConsecutivosEquivalencias.Add(ConsecutivosEquivalenciasSelected)
                End If

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
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

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            Else
                _mlogNuevo = False
                If So.UserState = "BorrarRegistro" Then
                    A2Utilidades.Mensajes.mostrarMensaje("Se elimino correctamente el registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If



            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ConsecutivosEquivalenciasSelected) Then
            Editando = True
            editar = True
            _mlogNuevo = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConsecutivosEquivalenciasSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ConsecutivosEquivalenciasSelected.EntityState = EntityState.Detached Then
                    ConsecutivosEquivalenciasSelected = ConsecutivosEquivalenciasAnterior
                End If
            End If
            editar = False
            _mlogNuevo = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ConsecutivosEquivalenciasSelected) Then
                dcProxy.TesoreriaConsecutivosEquivalencias.Remove(_ConsecutivosEquivalenciasSelected)
                ConsecutivosEquivalenciasSelected = _ListaConsecutivosEquivalencias.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
            editar = False
            _mlogNuevo = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
    '    If DicCamposTab.ContainsKey(pstrNombreCampo) Then
    '        Dim miTab = DicCamposTab(pstrNombreCampo)
    '        TabSeleccionadaFinanciero = miTab
    '    End If
    'End Sub

    Public Function ValidarRegistro() As Boolean
        Try
            Dim logValidacion As Boolean = True
            Dim strMensajeValidacion As String = String.Empty

            If IsNothing(ConsecutivosEquivalenciasSelected) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1}- Para grabar debes completar los campos Requeridos", strMensajeValidacion, vbCrLf)
            Else
                '----------------------------------------------------------------------------------
                'Validaciones de Actualizacion Registro
                If ConsecutivosEquivalenciasSelected.NombreConsecutivoCE = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- Debe escoger un consecutivo de comprobantes de egreso.", strMensajeValidacion, vbCrLf)
                End If

                If ConsecutivosEquivalenciasSelected.NombreConsecutivoRC = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- Debe escoger un consecutivo de recibos de caja.", strMensajeValidacion, vbCrLf)
                End If

            End If
            If logValidacion = False Then
                IsBusy = False

                A2Utilidades.Mensajes.mostrarMensaje("No es posible continuar se encontraron las siguientes validaciones:" & vbCrLf & strMensajeValidacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If

        Catch ex As Exception
            Return False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de validaciones.", Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Function
#End Region

#Region "Propiedades que definen atributos de la orden"

   
   
#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"




#End Region


End Class






