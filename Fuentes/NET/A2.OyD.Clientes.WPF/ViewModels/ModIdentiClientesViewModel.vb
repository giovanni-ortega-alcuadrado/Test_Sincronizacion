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
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes

Public Class ModIdentiClientesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As ClientesDomainContext
    Dim dcProxy2 As MaestrosDomainContext
    Private objProxy As UtilidadesDomainContext
    Public CambioNroDoc As Boolean = False
    Public UtilizandoBuscador As Boolean = False

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
            objProxy = New UtilidadesDomainContext()
            dcProxy2 = New MaestrosDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            dcProxy2 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
        End If
        Try
            Modificar = False
            Consultar = False
            ClienteSelected.Habilitar = False
            ClienteSelected.HabilitarTextoNroNuevo = False
            Texto = "Códigos con el Número Identificación"
            IsBusy = True
            dcProxy2.Load(dcProxy2.ClasificacionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                     Me.ToString(), "ModIdentiClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"

    Sub ConsultarIdentificacionCliente()
        'Se hace la siguiente consulta  select strnombre, lngid from tblclientes where strnrodocumento ='5014' order by strnombre
        Try
            IsBusy = True
            Modificar = True
            Texto = "Códigos con el Número Identificación: " + ClienteSelected.NumeroDocumentoActual
            dcProxy.Clientes.Clear()
            dcProxy.Load(dcProxy.ConsultarClienteCodigoPorIdentificacionQuery(ClienteSelected.NumeroDocumentoActual, ClienteSelected.TipoIdentificacionIdActual, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteCodigo, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar codigos asociados del cliente actual", _
                Me.ToString(), "ModIdentiClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub
    Sub ModificarIdentificacionCliente()
        Try
            'Llamar el procedimiento usp_ClientesIdentificacion_Consultar y Llamar el procedimiento usp_ClientesIdentificacion_Modificar
            If (ClienteSelected.NumeroDocumentoActual = ClienteSelected.NumeroDocumentoNuevo) And (ClienteSelected.TipoIdentificacionIdActual = ClienteSelected.TipoIdentificacionIdNuevo) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Número de Identificación Acutal no puede ser igual al Número de Identificación Nuevo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ClienteSelected.TipoIdentificacionIdNuevo = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el Tipo de documento Nuevo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ClienteSelected.NumeroDocumentoNuevo = "" Then
                A2Utilidades.Mensajes.mostrarMensaje("El Número de Identificación Nuevo no puede ser 0", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'If ClienteSelected.NumeroDocumentoNuevo > 999999999999999 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("El Número de documento Nuevo no puede tener mas de 15 números", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            'With ClienteSelected
            '    If .TipoIdentificacionIdActual = "c" Or .TipoIdentificacionIdActualq Then
            'End With
            If _Clasificacion = 1 Then
                If (ClienteSelected.TipoIdentificacionIdActual = ("C") Or ClienteSelected.TipoIdentificacionIdActual = ("E") Or _
                    ClienteSelected.TipoIdentificacionIdActual = ("I") Or ClienteSelected.TipoIdentificacionIdActual = ("P")) And
                    (ClienteSelected.TipoIdentificacionIdNuevo = "T" Or ClienteSelected.TipoIdentificacionIdNuevo = "G") Then
                    'C1.Silverlight.C1MessageBox.Show("¿Esta de acuerdo en cambiar un documento de Mayor de Edad en otro de Menor de Edad?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf ResultadoCambiar)
                    mostrarMensajePregunta("¿Esta de acuerdo en cambiar un documento de Mayor de Edad en otro de Menor de Edad?", _
                                           Program.TituloSistema, _
                                           "CAMBIARDOCUMENTO", _
                                           AddressOf ResultadoCambiar, False)
                Else
                    ConsultarClientesIdentificacion()
                End If
            Else
                ConsultarClientesIdentificacion()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar clientes identificación", _
                Me.ToString(), "ModIdentiClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub ConsultarClientesIdentificacion()
        Try
            IsBusy = True
            dcProxy.ConsultarClientesIdentificacin(ClienteSelected.NumeroDocumentoNuevo, ClienteSelected.TipoIdentificacionIdNuevo, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarClienteIdentificacion, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar clientes identificacion", _
                Me.ToString(), "ModIdentiClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub ModificarClientesIdentificacion()
        Try
            IsBusy = True
            dcProxy.ModificarClientesIdentificacion(ClienteSelected.TipoIdentificacionIdActual, ClienteSelected.NumeroDocumentoActual, ClienteSelected.TipoIdentificacionIdNuevo, ClienteSelected.NumeroDocumentoNuevo, Program.Usuario, Program.HashConexion, AddressOf TerminoModificarIdentificacionCliente, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar clientes identificacion", _
                Me.ToString(), "ModIdentiClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Sub DeshabilitarControles()
        Modificar = False
        Consultar = True
        ClienteSelected.Habilitar = False
        ClienteSelected.HabilitarTextoNroNuevo = False
    End Sub

    Private Sub ResultadoCambiar(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            ConsultarClientesIdentificacion()
        End If
    End Sub

    Private Sub ResultadoConfirmar(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            ModificarClientesIdentificacion()
        End If
    End Sub

    Private Sub _ClienteSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClienteSelected.PropertyChanged
        If CambioNroDoc Then
            CambioNroDoc = False
            Exit Sub
        End If
        If e.PropertyName.Equals("NumeroDocumentoActual") Then
            'If Not Versioned.IsNumeric(_ClienteSelected.NumeroDocumentoActual) Then
            '    If Not (_ClienteSelected.NumeroDocumentoActual = String.Empty) Then
            '        A2Utilidades.Mensajes.mostrarMensaje("El Número Documento Actual debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        CambioNroDoc = True
            '        Consultar = False
            '        Modificar = False
            '        ClienteSelected.NumeroDocumentoActual = String.Empty
            '    Else
            '        Consultar = False
            '        Modificar = False
            '    End If
            'Else
            If Not UtilizandoBuscador Then
                If Not (_ClienteSelected.NumeroDocumentoActual = String.Empty) Then
                    buscarClienteIdentificacion(_ClienteSelected.NumeroDocumentoActual)
                End If
                'End If
            End If
        End If
        'If e.PropertyName.Equals("TipoIdentificacionIdActual") Then

        'End If
    End Sub

    Friend Sub buscarClienteIdentificacion(ByVal pstrNroDocumento As String)
        Try
            Dim Agrupamiento As String
            Agrupamiento = "id," + _ClienteSelected.TipoIdentificacionIdActual
            objProxy.BuscadorClientes.Clear()
            objProxy.Load(objProxy.buscarClientesQuery(pstrNroDocumento, "T", "", Agrupamiento, Program.Usuario, False, 0, Program.HashConexion), AddressOf buscarClienteIdentificacionCompleted, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarClienteIdentificacionCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                Consultar = True
                Modificar = False
            Else
                '    'Me.ComitenteSeleccionado = Nothing
                '    'If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                A2Utilidades.Mensajes.mostrarMensaje("El Número de Identificación actual ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Consultar = False
                Modificar = False
                ' IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
        'IsBusy = False
    End Sub

#End Region

#Region "Metodos Asicronos"

    Private Sub TerminoTraerClasificaciones(ByVal lo As LoadOperation(Of Clasificacion))
        If Not lo.HasError Then
            listaclasificacion = dcProxy2.Clasificacions
            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                ClienteSelected.objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOID")
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificaciones", _
                                             Me.ToString(), "TerminoTraerClasificacion", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoConsultarClienteIdentificacion(ByVal lo As InvokeOperation(Of String))
        IsBusy = False
        If Not lo.HasError Then
            If lo.Value.ToString = String.Empty Then
                'C1.Silverlight.C1MessageBox.Show("¿Desea guardar el nuevo documento?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf ResultadoConfirmar)
                mostrarMensajePregunta("¿Desea guardar el nuevo documento?", _
                                       Program.TituloSistema, _
                                       "NUEVODOCUMENTO", _
                                       AddressOf ResultadoConfirmar, False)
            Else
                A2Utilidades.Mensajes.mostrarMensaje(lo.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarMensaje("Se presento un problema al cambiar la identificación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    Private Sub TerminoConsultarClienteCodigo(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente))
        If Not lo.HasError Then
            CodigoCliente = dcProxy.Clientes
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes", _
                                 Me.ToString(), "TerminoTraerCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoModificarIdentificacionCliente(ByVal lo As InvokeOperation(Of String))
        IsBusy = False
        If Not lo.HasError Then
            'MessageBox.Show("Se ha modifica correctamete la identificacion", "Cambio Identificacion", MessageBoxButton.OK)
            A2Utilidades.Mensajes.mostrarMensaje("Se ha modificado correctamete la identificación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            DeshabilitarControles()
        Else
            'MessageBox.Show("Se presento un problema al cambiar la identificacion", "Cambio Identificacion", MessageBoxButton.OK)
            A2Utilidades.Mensajes.mostrarMensaje("Se presento un problema al cambiar la identificación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _Modificar As Boolean
    Public Property Modificar() As Boolean
        Get
            Return _Modificar
        End Get
        Set(ByVal value As Boolean)
            _Modificar = value
            MyBase.CambioItem("Modificar")
        End Set
    End Property

    Private _Consultar As Boolean
    Public Property Consultar() As Boolean
        Get
            Return _Consultar
        End Get
        Set(ByVal value As Boolean)
            _Consultar = value
            MyBase.CambioItem("Consultar")
        End Set
    End Property

    Private _Clasificacion As String
    Public Property Clasificacion() As String
        Get
            Return _Clasificacion
        End Get
        Set(ByVal value As String)
            _Clasificacion = value
            ClienteSelected.Habilitar = True
            ClienteSelected.LimpiarCombo = String.Empty
            ClienteSelected.LimpiarComboActual = String.Empty
            ClienteSelected.NumeroDocumentoNuevo = String.Empty
            Consultar = False
            Modificar = False
            If value = 1 Then
                ClienteSelected.objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1")
                'ClienteSelected.objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOID").Where(Function(li) Not li.ID.Equals("R") And Not li.ID.Equals("O") And Not li.ID.Equals("N")).ToList
            Else
                ClienteSelected.objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2")
                'ClienteSelected.objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOID").Where(Function(li) li.ID.Equals("R") Or li.ID.Equals("O") Or li.ID.Equals("N")).ToList
            End If
        End Set
    End Property

    Private _Texto As String
    Public Property Texto() As String
        Get
            Return _Texto
        End Get
        Set(ByVal value As String)
            _Texto = value
            MyBase.CambioItem("Texto")
        End Set
    End Property


    Private _listaclasificacion As EntitySet(Of Clasificacion)
    Public Property listaclasificacion As EntitySet(Of Clasificacion)
        Get
            Return _listaclasificacion
        End Get
        Set(ByVal value As EntitySet(Of Clasificacion))
            _listaclasificacion = value
            MyBase.CambioItem("listaclasificacion")
        End Set
    End Property


    Private WithEvents _ClienteSelected As ClienteSeleccionado = New ClienteSeleccionado
    Public Property ClienteSelected() As ClienteSeleccionado
        Get
            Return _ClienteSelected
        End Get
        Set(ByVal value As ClienteSeleccionado)
            _ClienteSelected = value
            MyBase.CambioItem("ClienteSeleccionado")
        End Set
    End Property

    Private _CodigoCliente As EntitySet(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
    Public Property CodigoCliente() As EntitySet(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente)
        Get
            Return _CodigoCliente
        End Get
        Set(ByVal value As EntitySet(Of A2.OyD.OYDServer.RIA.Web.OyDClientes.Cliente))
            _CodigoCliente = value
            MyBase.CambioItem("CodigoCliente")

        End Set
    End Property

#End Region

#Region "Clases"
    Public Class ClienteSeleccionado
        Implements INotifyPropertyChanged

        Private _TipoIdentificacionIdNuevo As String
        Public Property TipoIdentificacionIdNuevo As String
            Get
                Return _TipoIdentificacionIdNuevo
            End Get
            Set(ByVal value As String)
                _TipoIdentificacionIdNuevo = value
            End Set
        End Property

        Private _TipoIdentificacionIdActual As String
        Public Property TipoIdentificacionIdActual As String
            Get
                Return _TipoIdentificacionIdActual
            End Get
            Set(ByVal value As String)
                _TipoIdentificacionIdActual = value
                HabilitarTextoNroNuevo = True
                NumeroDocumentoActual = String.Empty
            End Set
        End Property

        Private _NumeroDocumentoActual As String
        Public Property NumeroDocumentoActual As String
            Get
                Return _NumeroDocumentoActual
            End Get
            Set(ByVal value As String)
                _NumeroDocumentoActual = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroDocumentoActual"))
            End Set
        End Property

        Private _NumeroDocumentoNuevo As String
        Public Property NumeroDocumentoNuevo As String
            Get
                Return _NumeroDocumentoNuevo
            End Get
            Set(ByVal value As String)
                _NumeroDocumentoNuevo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NumeroDocumentoNuevo"))
            End Set
        End Property

        Private _LimpiarCombo As String
        Public Property LimpiarCombo As String
            Get
                Return _LimpiarCombo
            End Get
            Set(ByVal value As String)
                _LimpiarCombo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LimpiarCombo"))
            End Set
        End Property

        Private _LimpiarComboActual As String
        Public Property LimpiarComboActual As String
            Get
                Return _LimpiarComboActual
            End Get
            Set(ByVal value As String)
                _LimpiarComboActual = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LimpiarComboActual"))
            End Set
        End Property

        Private _objTipoId As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
        Public Property objTipoId() As List(Of OYDUtilidades.ItemCombo)
            Get
                Return _objTipoId
            End Get
            Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
                _objTipoId = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("objTipoId"))
            End Set
        End Property

        Private _Habilitar As Boolean
        Public Property Habilitar() As Boolean
            Get
                Return _Habilitar
            End Get
            Set(ByVal value As Boolean)
                _Habilitar = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Habilitar"))
            End Set
        End Property

        Private _HabilitarTextoNroNuevo As Boolean
        Public Property HabilitarTextoNroNuevo() As Boolean
            Get
                Return _HabilitarTextoNroNuevo
            End Get
            Set(ByVal value As Boolean)
                _HabilitarTextoNroNuevo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarTextoNroNuevo"))
            End Set
        End Property

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

#End Region

End Class
