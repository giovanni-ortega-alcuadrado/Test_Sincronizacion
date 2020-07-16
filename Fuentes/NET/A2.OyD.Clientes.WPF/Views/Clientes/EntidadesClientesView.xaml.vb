Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClientesViewModel.vb
'Generado el : 07/08/2011 09:34:53
'Propiedad de Alcuadrado S.A. 2010
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes

Partial Public Class EntidadesClientesView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Dim dcProxy As ClientesDomainContext
    Dim mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim mlogValidarAutorizacionInfoClientes As Boolean


#Region "Metodos"
    Public Sub New()
        InitializeComponent()
        Me.DataContext = Me

        bitHabilitarIdComitente = True
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        VisibleClasificacionTipoCliente = Visibility.Collapsed

        'Jorge Andrés Bedoya 2013/12/18
        'Se adiciona el campo HabilitarTratamientoInfo
        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.listaVerificaparametroQuery("", "Clientes", Program.Usuario, Program.HashConexion), AddressOf Terminotraerparametrolista, Nothing)

    End Sub

    Private Sub Terminotraerparametrolista(ByVal obj As LoadOperation(Of valoresparametro))
        'Jorge Andrés Bedoya 2013/12/18
        'Se adiciona el campo HabilitarTratamientoInfo
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            For Each li In obj.Entities.ToList
                Select Case li.Parametro
                    Case "VALIDAR_AUTORIZACION_INFORMACION_CLIENTES"
                        mlogValidarAutorizacionInfoClientes = IIf(li.Valor = "SI", True, False)
                    Case "CAMPOCLASIFICACIONTIPOCLIENTE" 'Jorge Andres Bedoya 2014/03/11
                        If li.Valor = "SI" Then
                            VisibleClasificacionTipoCliente = Visibility.Visible
                            'Else
                            '   VisibleClasificacionTipoCliente = Visibility.Collapsed
                        End If
                End Select
            Next
        End If
    End Sub


    Private Sub Clientes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'objViewModel = CType(Me.DataContext, DuplicarClientesViewModel)
    End Sub
    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)

        'Validaciones de la pantalla
        If EntidadesClientesClase.lngIdComitente = "" Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un código de comitente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        If EntidadesClientesClase.strTipoPersona = "2" Then
            If IsNothing(EntidadesClientesClase.strEntidadPublica) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar una entidad pública", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
        End If

        If EntidadesClientesClase.strTipoPersona = "2" Then
            If EntidadesClientesClase.strEntidadPublica.Equals("1") And IsNothing(EntidadesClientesClase.strClaseEntidad) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe selecionar una clase de entidad", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
        End If

        'Jorge Andrés Bedoya 2013/12/18
        'Se adiciona el campo HabilitarTratamientoInfo
        If mlogValidarAutorizacionInfoClientes Then
            If EntidadesClientesClase.strAutorizarinfoCliente = Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo Autorizo Tratamiento de Información es obligatorio", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
        End If

        myBusyIndicator.IsBusy = True
        'dcProxy.modificarclientesentidades(EntidadesClientesClase.lngIdComitente, EntidadesClientesClase.strEntidadPublica, EntidadesClientesClase.strClaseEntidad, EntidadesClientesClase.bitFondoLiquidez,Program.Usuario, Program.HashConexion, AddressOf TerminoModificarCliente, Nothing)
        Program.VerificarCambiosProxyServidor(dcProxy)
        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)

    End Sub

    Public Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            myBusyIndicator.IsBusy = False
            If So.HasError Then
                'Dim sm As String
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)


                So.MarkErrorAsHandled()
                Exit Try
            Else
                ' A2Utilidades.Mensajes.mostrarMensaje("Las entidades fueron actualizadas correctamente ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                EntidadesClientesClase.lngIdComitente = ""
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub txtNroIdentificacion_LostFocus(sender As Object, e As RoutedEventArgs)
        'objViewModel.ConsultarDatosCliente()
    End Sub
    Private Sub __EntidadesClientesClase_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EntidadesClientesClase.PropertyChanged

        If e.PropertyName.Equals("lngIdComitente") Then
            If EntidadesClientesClase.lngIdComitente = "" Then
                EntidadesClientesClase.strTipoPersona = Nothing
                EntidadesClientesClase.strNombre = ""
                EntidadesClientesClase.strEntidadPublica = Nothing
                EntidadesClientesClase.strClaseEntidad = Nothing
                EntidadesClientesClase.bitFondoLiquidez = False

                'Jorge Andrés Bedoya 2013/12/18
                'Se adiciona el campo HabilitarTratamientoInfo
                EntidadesClientesClase.strAutorizarinfoCliente = Nothing

                'Jorge Andrés Bedoya 2014/03/11
                'Se adiciona el campo lngIDClasificacionTipoCliente
                EntidadesClientesClase.lngIDClasificacionTipoCliente = Nothing

                bitHabilitarEntidadesPublicas = False
                bitHabilitarFondoLiquidez = False
                bitHabilitarClaseEntidades = False
                bitHabilitarTratamientoInfo = False
                bitHabilitarClasificacionTipoCliente = False
                bitHabilitarIdComitente = True
            Else
                buscarItem("clientes")
            End If
        Else
            If e.PropertyName.Equals("strEntidadPublica") Then
                If Not EntidadesClientesClase.strEntidadPublica = Nothing Then
                    If EntidadesClientesClase.strEntidadPublica.Equals("1") Then
                        bitHabilitarClaseEntidades = True
                    Else
                        bitHabilitarClaseEntidades = False
                        EntidadesClientesClase.strClaseEntidad = Nothing
                    End If
                End If
            Else
                If e.PropertyName.Equals("strTipoPersona") Then
                    If Not EntidadesClientesClase.strTipoPersona = Nothing Then
                        If EntidadesClientesClase.strTipoPersona.Equals("1") Then
                            EntidadesClientesClase.strEntidadPublica = Nothing
                            EntidadesClientesClase.strClaseEntidad = Nothing
                            bitHabilitarClaseEntidades = False
                            bitHabilitarEntidadesPublicas = False
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente AS OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            EntidadesClientesClase.lngIdComitente = pobjComitente.IdComitente
            EntidadesClientesClase.strNombre = pobjComitente.Nombre
            Dim tipopersona = pobjComitente.CodTipoIdentificacion
            Dim objClienteNatural = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1").Where(Function(id) id.ID = tipopersona) 'natural
            If objClienteNatural.Count > 0 Then
                EntidadesClientesClase.strTipoPersona = "1"
            End If
            Dim objClienteJuridica = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2").Where(Function(id) id.ID = tipopersona) 'juridica
            If objClienteJuridica.Count > 0 Then
                EntidadesClientesClase.strTipoPersona = "2"
            End If
        End If
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        EntidadesClientesClase.lngIdComitente = ""
    End Sub
    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Try
            If Not Me.EntidadesClientesClase Is Nothing Then
                Select Case pstrTipoItem
                    Case "clientes"
                        myBusyIndicator.IsBusy = True
                        If Not IsNothing(EntidadesClientesClase.lngIdComitente) Then
                            'mdcProxyUtilidad01.BuscadorClientes.Clear()
                            'mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(EntidadesClientesClase.lngIdComitente, Program.Usuario, "IdComitente",Program.Usuario, Program.HashConexion), AddressOf buscarClienteCompleted, pstrTipoItem)
                            dcProxy.EntidadesClientes.Clear()
                            dcProxy.Load(dcProxy.ConsultaClientesEntidadesQuery(EntidadesClientesClase.lngIdComitente, Program.Usuario, Program.HashConexion), AddressOf buscarClienteCompleted, Nothing)
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del comitente", Me.ToString(), "Buscar comitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    'Private Sub buscarClienteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
    Private Sub buscarClienteCompleted(ByVal lo As LoadOperation(Of OyDClientes.EntidadesClientes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                'EntidadesClientesClase.strNombre = lo.Entities.ToList.Item(0).Nombre
                bitHabilitarEntidadesPublicas = True
                bitHabilitarFondoLiquidez = True
                EntidadesClientesClase = lo.Entities.First
                bitHabilitarIdComitente = False

                'Jorge Andrés Bedoya 2013/12/18
                'Se adiciona el campo HabilitarTratamientoInfo
                bitHabilitarTratamientoInfo = True

                'Jorge Andrés Bedoya 2014/03/11
                'Se adiciona el campo ClasificacionTipoCliente
                bitHabilitarClasificacionTipoCliente = True

                Dim tipopersona = EntidadesClientesClase.strTipoIdentificacion
                Dim objClienteNatural = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1").Where(Function(id) id.ID = tipopersona) 'natural
                If objClienteNatural.Count > 0 Then
                    EntidadesClientesClase.strTipoPersona = "1"
                End If
                Dim objClienteJuridica = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2").Where(Function(id) id.ID = tipopersona) 'juridica
                If objClienteJuridica.Count > 0 Then
                    EntidadesClientesClase.strTipoPersona = "2"
                End If
                If Not EntidadesClientesClase.strEntidadPublica = Nothing Then
                    If EntidadesClientesClase.strEntidadPublica.Equals("1") Then
                        bitHabilitarClaseEntidades = True
                    Else
                        bitHabilitarClaseEntidades = False
                        EntidadesClientesClase.strClaseEntidad = Nothing
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El código del comitente no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                bitHabilitarIdComitente = True
                EntidadesClientesClase.lngIdComitente = ""
                txtIdComitente.Focus()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarClienteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
        myBusyIndicator.IsBusy = False
    End Sub

    Private Sub txtIdComitente_KeyDown_1(sender As Object, e As KeyEventArgs)

        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If

    End Sub
#End Region
#Region "Propiedades"
    Private WithEvents _EntidadesClientesClase As New OyDClientes.EntidadesClientes
    Public Property EntidadesClientesClase As OyDClientes.EntidadesClientes
        Get
            Return _EntidadesClientesClase
        End Get
        Set(ByVal value As OyDClientes.EntidadesClientes)
            _EntidadesClientesClase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EntidadesClientesClase"))
        End Set
    End Property
    Private _bitHabilitarClaseEntidades As Boolean
    Public Property bitHabilitarClaseEntidades As Boolean
        Get
            Return _bitHabilitarClaseEntidades
        End Get
        Set(ByVal value As Boolean)
            _bitHabilitarClaseEntidades = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitHabilitarClaseEntidades"))
        End Set
    End Property
    Private _bitHabilitarEntidadesPublicas As Boolean
    Public Property bitHabilitarEntidadesPublicas As Boolean
        Get
            Return _bitHabilitarEntidadesPublicas
        End Get
        Set(ByVal value As Boolean)
            _bitHabilitarEntidadesPublicas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitHabilitarEntidadesPublicas"))
        End Set
    End Property
    Private _bitHabilitarFondoLiquidez As Boolean
    Public Property bitHabilitarFondoLiquidez As Boolean
        Get
            Return _bitHabilitarFondoLiquidez
        End Get
        Set(ByVal value As Boolean)
            _bitHabilitarFondoLiquidez = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitHabilitarFondoLiquidez"))
        End Set
    End Property

    Private _bitHabilitarIdComitente As Boolean
    Public Property bitHabilitarIdComitente As Boolean
        Get
            Return _bitHabilitarIdComitente
        End Get
        Set(ByVal value As Boolean)
            _bitHabilitarIdComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitHabilitarIdComitente"))
        End Set
    End Property

    'Jorge Andrés Bedoya 2013/12/18
    'Se adiciona el campo HabilitarTratamientoInfo
    Private _bitHabilitarTratamientoInfo As Boolean
    Public Property bitHabilitarTratamientoInfo As Boolean
        Get
            Return _bitHabilitarTratamientoInfo
        End Get
        Set(ByVal value As Boolean)
            _bitHabilitarTratamientoInfo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitHabilitarTratamientoInfo"))
        End Set
    End Property

    'Jorge Andrés Bedoya 2014/03/11
    Private _bitHabilitarClasificacionTipoCliente As Boolean
    Public Property bitHabilitarClasificacionTipoCliente As Boolean
        Get
            Return _bitHabilitarClasificacionTipoCliente
        End Get
        Set(ByVal value As Boolean)
            _bitHabilitarClasificacionTipoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitHabilitarClasificacionTipoCliente"))
        End Set
    End Property

    'Jorge Andrés Bedoya 2014/03/11
    'Se adiciona el campo Clasificacion tipo cliente
    Private _VisibleClasificacionTipoCliente As Visibility
    Public Property VisibleClasificacionTipoCliente As Visibility
        Get
            Return _VisibleClasificacionTipoCliente
        End Get
        Set(ByVal value As Visibility)
            _VisibleClasificacionTipoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleClasificacionTipoCliente"))
        End Set
    End Property

#End Region
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
