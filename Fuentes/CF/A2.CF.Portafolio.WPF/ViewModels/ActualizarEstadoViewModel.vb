Imports Telerik.Windows.Controls
' Desarrollo       : Clase ActualizarEstadoViewModel, Clase xxx
' Creado por       : Juan Carlos Soto Cruz.
' Fecha            : Agosto 24/2011 4:30 p.m   

#Region "Librerias"

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports System.Windows.Data
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
#End Region

#Region "Clases"

''' <summary>
''' View model
''' </summary>
''' <remarks>
''' Nombre	        :	ActualizarEstadoViewModel
''' Creado por	    :	Juan Carlos Soto Cruz.
''' Fecha			:	Agosto 24/2011
''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 24/2011 - Resultado Ok
''' </remarks>
Public Class ActualizarEstadoViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"

    Dim dcProxy As PortafolioDomainContext
    Dim intTitulos As Integer
    Dim _mlogSeRealizoPregunta As Boolean = False
    Public _mlogBuscarClienteInicial As Boolean = True
    Public _mlogBuscarClienteFinal As Boolean = True
    Private objProxyUtilidades As UtilidadesDomainContext
    Private logSeleccionados As Boolean = True

#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Se realizan acciones iniciales del proceso y se inicializa el DomainContext dependiendo de si se esta en ambiente de desarrollo o no.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	Sub New
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 26/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 26/2011 - Resultado Ok
    ''' </remarks>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New PortafolioDomainContext
                objProxyUtilidades = New UtilidadesDomainContext()
            Else
                dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                objProxyUtilidades = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ActualizarEstadoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega al case la opcion Comitente, para permitir digitar el código del cliente
    ''' Fecha            : Junio 05/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 05/2013 - Resultado Ok 
    ''' 
    ''' </history>

    Private Sub _actualizarEstado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _actualizarEstado.PropertyChanged
        Try
            Select Case e.PropertyName

                Case "IdComitente"
                    If Not String.IsNullOrEmpty(_actualizarEstado.IdComitente) And _mlogBuscarClienteInicial Then
                        _mlogBuscarClienteFinal = False
                        dcProxy.CustodiasObtenerTitulos.Clear()
                        buscarComitente(_actualizarEstado.IdComitente)
                    End If
                Case "IdComitenteFinal"
                    If Not String.IsNullOrEmpty(_actualizarEstado.IdComitenteFinal) And _mlogBuscarClienteFinal Then
                        _mlogBuscarClienteInicial = False
                        dcProxy.CustodiasObtenerTitulos.Clear()
                        buscarComitente(_actualizarEstado.IdComitenteFinal)
                    End If
                Case "estadoInicial", "estadofinal"
                    dcProxy.CustodiasObtenerTitulos.Clear()
                Case "Seleccionar"
                    SeleccionarTodos(_actualizarEstado.Seleccionar)

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Métodos Autocompletar Cliente"
    ''' <summary>
    ''' Buscar los datos del comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : 
    ''' Fecha            : Junio 05/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 05/2013 - Resultado Ok 
    ''' </history>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitenteInicial As String = String.Empty
        Dim strIdComitenteFinal As String = String.Empty
        Try
            If (_mlogBuscarClienteInicial) Then
                If Not Me.actualizarEstado Is Nothing Then
                    If Not strIdComitenteInicial.Equals(Me.actualizarEstado.IdComitente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitenteInicial = Me.actualizarEstado.IdComitente
                        Else
                            strIdComitenteInicial = pstrIdComitente
                        End If
                    End If
                End If
                If Not strIdComitenteInicial Is Nothing AndAlso Not strIdComitenteInicial.Trim.Equals(String.Empty) Then
                    objProxyUtilidades.BuscadorClientes.Clear()
                    objProxyUtilidades.Load(objProxyUtilidades.buscarClienteEspecificoQuery(strIdComitenteInicial, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                End If
            End If

            If (_mlogBuscarClienteFinal) Then
                If Not Me.actualizarEstado Is Nothing Then
                    If Not strIdComitenteFinal.Equals(Me.actualizarEstado.IdComitenteFinal) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitenteFinal = Me.actualizarEstado.IdComitenteFinal
                        Else
                            strIdComitenteFinal = pstrIdComitente
                        End If
                    End If
                End If
                If Not strIdComitenteFinal Is Nothing AndAlso Not strIdComitenteFinal.Trim.Equals(String.Empty) Then
                    objProxyUtilidades.BuscadorClientes.Clear()
                    objProxyUtilidades.Load(objProxyUtilidades.buscarClienteEspecificoQuery(strIdComitenteFinal, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' Fecha            : Junio 05/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 05/2013 - Resultado Ok 
    ''' </summary>
    ''' <param name="lo"></param>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If (_mlogBuscarClienteInicial) Then
                If lo.Entities.ToList.Count > 0 Then
                    'If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("El código ingresado en el campo: Del cliente, se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'Else
                    _mlogBuscarClienteInicial = False
                    'actualizarEstado.IdComitente = lo.Entities.ToList.Item(0).CodigoOYD
                    actualizarEstado.IdComitente = lo.Entities.ToList.Item(0).IdComitente
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El código ingresado en el campo: Del cliente, no existe ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

            If (_mlogBuscarClienteFinal) Then
                If lo.Entities.ToList.Count > 0 Then
                    'If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("El código ingresado en el campo: Al cliente, se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'Else
                    _mlogBuscarClienteFinal = False
                    'actualizarEstado.IdComitenteFinal = lo.Entities.ToList.Item(0).CodigoOYD
                    actualizarEstado.IdComitenteFinal = lo.Entities.ToList.Item(0).IdComitente
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El código ingresado en el campo: Al cliente, no existe ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades"

    Private WithEvents _actualizarEstado As actualizarEstado = New actualizarEstado
    Public Property actualizarEstado() As actualizarEstado
        Get
            Return _actualizarEstado
        End Get
        Set(ByVal value As actualizarEstado)
            _actualizarEstado = value
            MyBase.CambioItem("actualizarEstado")
        End Set
    End Property

    Private _ListaTitulosCustodias As EntitySet(Of CustodiasObtenerTitulos)
    Public Property ListaTitulosCustodias() As EntitySet(Of CustodiasObtenerTitulos)
        Get
            Return _ListaTitulosCustodias
        End Get
        Set(ByVal value As EntitySet(Of CustodiasObtenerTitulos))
            _ListaTitulosCustodias = value
            MyBase.CambioItem("ListaTitulosCustodias")
            MyBase.CambioItem("ListaTitulosCustodiasPaged")
        End Set
    End Property

    Public ReadOnly Property ListaTitulosCustodiasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTitulosCustodias) Then
                Dim view = New PagedCollectionView(_ListaTitulosCustodias)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaTitulosCustodiasSelected As CustodiasObtenerTitulos
    Public Property ListaTitulosCustodiasSelected() As CustodiasObtenerTitulos
        Get
            Return _ListaTitulosCustodiasSelected
        End Get
        Set(ByVal value As CustodiasObtenerTitulos)
            If Not value Is Nothing Then
                _ListaTitulosCustodiasSelected = value
            End If
            MyBase.CambioItem("ListaTitulosCustodiasSelected")
        End Set
    End Property

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Metodo encargado de obtener los titulos custodias a las cuales se les actualizara el estado.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	ConsultarTitulosCustodias
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 26/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 26/2011 - Resultado Ok
    ''' </remarks>
    Public Sub ConsultarTitulosCustodias()
        Try
            Dim strEstadoInicial As String = " "
            Dim strEstadoFinal As String = " "

            If actualizarEstado.estadoInicial <> "(PENDIENTE)" Then
                strEstadoInicial = actualizarEstado.estadoInicial
            End If
            If actualizarEstado.estadofinal <> "(PENDIENTE)" Then
                strEstadoFinal = actualizarEstado.estadofinal
            End If

            If String.IsNullOrEmpty(strEstadoInicial) Or
                String.IsNullOrEmpty(strEstadoFinal) Then

                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar todos los criterios para generar el listado de custodias para actualizar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                ErrorForma = ""
                IsBusy = True
                dcProxy.CustodiasObtenerTitulos.Clear()
                'actualizarEstado.IdComitente = actualizarEstado.IdComitente.PadLeft(17, " ")
                'actualizarEstado.IdComitenteFinal = actualizarEstado.IdComitenteFinal.PadLeft(17, " ")
                dcProxy.Load(dcProxy.CustodiasConsultarTitulosQuery(actualizarEstado.IdComitente, actualizarEstado.IdComitenteFinal, strEstadoInicial, strEstadoFinal, Program.Usuario, Program.HashConexion), AddressOf CustodiasConsultarTitulosCompleted, "Consultar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Consulta de ", Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo encargado de actualizar el estado de los titulos custodias.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	ActualizarEstadoOperaciones
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 29/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 29/2011 - Resultado Ok
    ''' </remarks>
    ''' 
    ''' Se modifica el método para que actualice el estado, solo de las custodias seleccionadas por el usuario.
    ''' ID              :   000001
    ''' Nombre	        :	ActualizarEstadoOperaciones
    ''' Creado por	    :	Yeiny Adenis Marín Zapata.
    ''' Fecha			:	Febrero 26/2013
    ''' Pruebas CB		:	Yeiny Adenis Marín Zapata - Febrero 26/2013 - Resultado Ok
    ''' 
    Public Sub ActualizarEstadoOperaciones()
        Try
            If Not IsNothing(ListaTitulosCustodias) Then
                If actualizarEstado.estadoInicial = actualizarEstado.estadofinal Then
                    A2Utilidades.Mensajes.mostrarMensaje("Estado final igual a estado actual, revise por favor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not _mlogSeRealizoPregunta Then
                    'C1.Silverlight.C1MessageBox.Show("¿Está seguro de ejecutar esta acción?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminoPregunta)
                    mostrarMensajePregunta("¿Está seguro de ejecutar esta acción?", _
                                           Program.TituloSistema, _
                                           "ESTADOOPERACION", _
                                           AddressOf TerminoPregunta, False)
                    Exit Sub
                Else
                    _mlogSeRealizoPregunta = False
                End If

                'If MessageBox.Show("¿Está seguro de ejecutar esta acción? ", Program.TituloSistema, MessageBoxButton.OKCancel) = MessageBoxResult.Cancel Then
                '    Exit Sub
                'End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se ha generado listado de custodias para actualizar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            intTitulos = 0

            For Each LTC In ListaTitulosCustodias
                'ID:000001
                'If LTC.HasChanges Then  
                If LTC.ActualizarEstado.Value = True Then
                    Dim strEstadoFinal As String = " "

                    If actualizarEstado.estadofinal <> "(PENDIENTE)" Then
                        strEstadoFinal = actualizarEstado.estadofinal
                    End If


                    If strEstadoFinal = "R" Then
                        LTC.NombreConsecutivo = "CANAUTO"
                        LTC.TipoDcto = "CA"
                        LTC.Estado = strEstadoFinal
                    Else
                        LTC.NombreConsecutivo = "PENAUTO"
                        LTC.TipoDcto = "PA"
                        LTC.Estado = strEstadoFinal
                    End If
                    intTitulos = intTitulos + 1
                    LTC.Posicion = intTitulos
                    LTC.Usuario = Program.Usuario
                End If
            Next

            ErrorForma = ""
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "update")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarEstadoOperaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            _mlogSeRealizoPregunta = True
            ActualizarEstadoOperaciones()
        End If
    End Sub

    Public Sub SeleccionarTodos(ByVal Seleccionar As Boolean)
        If Not IsNothing(ListaTitulosCustodias) And logSeleccionados Then
            If ListaTitulosCustodias.Count > 0 Then
                For Each objLista In ListaTitulosCustodias
                    objLista.ActualizarEstado = Seleccionar
                Next
            End If
        End If
    End Sub

    'Private Sub TerminoPreguntarConfirmacion(sender As Object, e As EventArgs)
    '    Try

    '        Dim objParentresultado As cwMensajePregunta
    '        Dim objResultado As wppMensajePregunta
    '        objParentresultado = CType(sender, cwMensajePregunta)
    '        objResultado = CType(objParentresultado.Content, wppMensajePregunta)

    '        If objResultado.cwParent.DialogResult Then

    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de la validación.", Me.ToString(), "TerminoPreguntarConfirmacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '        IsBusy = False
    '    End Try
    'End Sub

#End Region

#Region "Resultados Asincronicos"

    ''' <summary>
    ''' Metodo Asincrono lanzado al finalizar la ejecucion de la operacion CustodiasConsultar.
    ''' </summary>
    ''' <param name="lo">
    ''' ByVal lo As LoadOperation(Of CustodiasObtenerTitulos)
    ''' </param>
    ''' <remarks>
    ''' Nombre	        :	CustodiasConsultarTitulosCompleted
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 26/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 26/2011 - Resultado Ok
    ''' </remarks>
    Private Sub CustodiasConsultarTitulosCompleted(ByVal lo As LoadOperation(Of CustodiasObtenerTitulos))
        If Not lo.HasError Then
            logSeleccionados = False
            actualizarEstado.Seleccionar = True
            logSeleccionados = True
            ListaTitulosCustodias = dcProxy.CustodiasObtenerTitulos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se consultaron las custodias.", Me.ToString(), "CustodiasConsultarCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    '''  Metodo Asincrono lanzado al finalizar la ejecucion de la operacion de Actualizacion del Estado de custodias.
    ''' </summary>
    ''' <param name="So"></param>
    ''' <remarks></remarks>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then

                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío

                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                So.MarkErrorAsHandled()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se actualizaron " & intTitulos & " custodias.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                MyBase.QuitarFiltroDespuesGuardar()
                ConsultarTitulosCustodias()
            End If

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

''' <summary>
''' Clase y Propiedades para el uso en el dataform dfActualizarEstado del view "ActualizarEstadoView.xaml".
''' </summary>
''' <remarks>
''' Nombre	        :	actualizarEstado
''' Creado por	    :	Juan Carlos Soto Cruz.
''' Fecha			:	Agosto 24/2011
''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 24/2011 - Resultado Ok
''' </remarks>
Public Class actualizarEstado
    Implements INotifyPropertyChanged

    Private _IdComitente As String = "0"
    <Display(Name:="Del Cliente")> _
    Public Property IdComitente As String
        Get
            Return _IdComitente
        End Get
        Set(ByVal value As String)
            _IdComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdComitente"))
        End Set
    End Property

    Private _IdComitenteFinal As String = "99999999999999999"
    <Display(Name:="Al Cliente")> _
    Public Property IdComitenteFinal As String
        Get
            Return _IdComitenteFinal
        End Get
        Set(ByVal value As String)
            _IdComitenteFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdComitenteFinal"))
        End Set
    End Property

    Private _estadoInicial As String = "(PENDIENTE)"
    <Display(Name:="")> _
    Public Property estadoInicial As String
        Get
            Return _estadoInicial
        End Get
        Set(ByVal value As String)
            _estadoInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("estadoInicial"))
        End Set
    End Property

    Private _estadofinal As String = "(PENDIENTE)"
    <Display(Name:="")> _
    Public Property estadofinal As String
        Get
            Return _estadofinal
        End Get
        Set(ByVal value As String)
            _estadofinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("estadofinal"))
        End Set
    End Property

    Private _Seleccionar As Boolean = True
    Public Property Seleccionar As Boolean
        Get
            Return _Seleccionar
        End Get
        Set(ByVal value As Boolean)
            _Seleccionar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Seleccionar"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

#End Region