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
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio

Public Class FraccionarCustodiasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged
    Dim dcProxy As PortafolioDomainContext
    Private objProxy As UtilidadesDomainContext
    Public _mlogBuscarCliente As Boolean = True
    Public _mlogBuscarEspecie As Boolean = True
    Dim mlogANNA As String

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New PortafolioDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        IsBusy = True
        objProxy.Verificaparametro("ISIN_ANNA", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, Nothing)
        objProxy.Load(objProxy.listaVerificaparametroQuery("", "Custodias", Program.Usuario, Program.HashConexion), AddressOf Terminotraerparametrolista, Nothing) 'SV20150305
        FechaBusqueda = Now.Date
        NombreColumna = "Cantidad a Bloquear"
        HabilitadoModificar = False
        logActivo = True
        IsBusy = False

    End Sub


#Region "Metodos"

    Sub BuscarCustadias()
        IsBusy = True
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("ESPECIEISIN", CamposBusquedaSelected.Nemotecnico, Program.Usuario, Program.HashConexion), AddressOf Terminotraerespecieitem, Nothing)
            dcProxy.CustodiasFraccionars.Clear()
            dcProxy.Load(dcProxy.ConsultarCustodiasClienteFraccionarQuery(Now.Date, CamposBusquedaSelected.CodigoCliente, CamposBusquedaSelected.Nemotecnico, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodiasCliente, "Consultar")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar las custodias asociadas al cliente", _
                Me.ToString(), "BloquearTitulosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Sub CambiarEstadoCustodias()
        CambiarEstadoCusto()
    End Sub
    Sub CambiarEstadoCusto()
        IsBusy = True
        Program.VerificarCambiosProxyServidor(dcProxy)
        dcProxy.SubmitChanges(AddressOf TerminoSubmitChangesGrabar, "insert")
    End Sub
    Private Sub _CamposBusquedaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CamposBusquedaSelected.PropertyChanged
        Try
            Select Case e.PropertyName

                Case "CodigoCliente"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected.CodigoCliente) And _mlogBuscarCliente Then
                        buscarComitente(_CamposBusquedaSelected.CodigoCliente)
                    End If
                Case "Nemotecnico"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected.Nemotecnico) And _mlogBuscarEspecie Then
                        BuscarEspecie(_CamposBusquedaSelected.Nemotecnico)
                    End If

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub _CustodiasClienteSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CustodiasClienteSelected.PropertyChanged
        Try
            If e.PropertyName = "NuevaCantidad" Or e.PropertyName = "NuevoFungible" Or e.PropertyName = "NuevoIsin" Then
                _CustodiasClienteSelected.NombreConsecutivo = "ENTCUS"
                _CustodiasClienteSelected.TipoDocumentos = "ET"
                _CustodiasClienteSelected.Comitente = _CamposBusquedaSelected.CodigoCliente
                _CustodiasClienteSelected.NombreCliente = _CamposBusquedaSelected.NombreCliente
                _CustodiasClienteSelected.TipoIdentificacion = _CamposBusquedaSelected.TipoDcto
                _CustodiasClienteSelected.NroDocumento = _CamposBusquedaSelected.NroDcto
                _CustodiasClienteSelected.Telefono1 = _CamposBusquedaSelected.TelefonoCliente
                _CustodiasClienteSelected.Direccion = _CamposBusquedaSelected.DireccionCliente
                _CustodiasClienteSelected.IdEspecie = _CamposBusquedaSelected.Nemotecnico
                _CustodiasClienteSelected.Usuario = Program.Usuario
                _CustodiasClienteSelected.Notas = "Custodia Generada después de fraccionar el recibo" + _CustodiasClienteSelected.IdRecibo.ToString
                _CustodiasClienteSelected.EstadoCus = "P"
                If _CustodiasClienteSelected.EstadoActual = String.Empty Then
                    _CustodiasClienteSelected.Estado = "S"
                Else
                    _CustodiasClienteSelected.Estado = "F"
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_CustodiasClienteSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos Autocompletar Cliente"
    ''' <summary>
    ''' Buscar los datos del comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : 
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty
        Try
            If (_mlogBuscarCliente) Then
                If Not Me.CamposBusquedaSelected Is Nothing Then
                    If Not strIdComitente.Equals(Me.CamposBusquedaSelected.CodigoCliente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.CamposBusquedaSelected.CodigoCliente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If

            If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                objProxy.BuscadorClientes.Clear()
                objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Abril 17/2013 - Resultado Ok 
    ''' </summary>
    ''' <param name="lo"></param>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If (_mlogBuscarCliente) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCamposBusqueda()
                    Else
                        Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusqueda()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>Autocompletar información del cliente</summary>
    ''' <param name="pobjComitente"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : Según el código del cliente digitado por el usuario, se autocompleta el nombre.
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub ComitenteSeleccionadoM(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            If (_mlogBuscarCliente) Then
                _CamposBusquedaSelected.NombreCliente = pobjComitente.Nombre
                CamposBusquedaSelected.TipoDcto = pobjComitente.CodTipoIdentificacion
                CamposBusquedaSelected.NroDcto = pobjComitente.NroDocumento
                CamposBusquedaSelected.TelefonoCliente = pobjComitente.Telefono
                CamposBusquedaSelected.DireccionCliente = pobjComitente.DireccionEnvio
            End If
        End If
    End Sub

    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : LimpiarCamposBusqueda()
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub LimpiarCamposBusqueda()
        CamposBusquedaSelected.CodigoCliente = String.Empty
        CamposBusquedaSelected.NombreCliente = String.Empty
        CamposBusquedaSelected.Nemotecnico = String.Empty
        CamposBusquedaSelected.Especie = String.Empty
        CamposBusquedaSelected.TipoDcto = String.Empty
        CamposBusquedaSelected.NroDcto = String.Empty
        CamposBusquedaSelected.TelefonoCliente = String.Empty
        CamposBusquedaSelected.DireccionCliente = String.Empty
        ListaCustodiasCliente = Nothing
        HabilitadoModificar = False
    End Sub
#End Region

#Region "Métodos Autocompletar Especie"

    ''' <summary>
    ''' Buscar los datos de la especie según el ID digitado por el usuario.
    ''' </summary>
    ''' <param name="pstrIdEspecie">ID de la Especie que se debe buscar.</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : En el método buscarNemotecnicoEspecificoQuery se envía el valor "T" en el parametro mstrAdmonValores
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Friend Sub BuscarEspecie(Optional ByVal pstrIdEspecie As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdEspecie As String = String.Empty
        Try
            If Not Me.CamposBusquedaSelected Is Nothing Then
                If Not strIdEspecie.Equals(Me.CamposBusquedaSelected.Nemotecnico) Then
                    If pstrIdEspecie.Trim.Equals(String.Empty) Then
                        strIdEspecie = Me.CamposBusquedaSelected.Nemotecnico
                    Else
                        strIdEspecie = pstrIdEspecie
                    End If

                    If Not strIdEspecie Is Nothing AndAlso Not strIdEspecie.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorEspecies.Clear()

                        'ID Modificación  : 000001
                        objProxy.Load(objProxy.buscarNemotecnicoEspecificoQuery("T", strIdEspecie, Program.Usuario, Program.HashConexion), AddressOf BuscarEspecieCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la especie", Me.ToString(), "BuscarEspecie", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar la especie cuando se digita el ID en el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Private Sub BuscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.Entities.ToList.Count > 0 Then
                Me.EspecieSeleccionadaM(lo.Entities.ToList.Item(0))
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La especie ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CamposBusquedaSelected.Nemotecnico = String.Empty
                CamposBusquedaSelected.Especie = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "BuscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se valida si la especie existe y se autocompleta el campo Especie.
    ''' </summary>
    ''' <param name="pobjEspecie"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub EspecieSeleccionadaM(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                CamposBusquedaSelected.Especie = pobjEspecie.Especie
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la especie", _
                                 Me.ToString(), "EspecieSeleccionadaM", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
#End Region

#Region "Metodos Asincronos"

    Private Sub TerminoTraerCustodiasCliente(ByVal lo As LoadOperation(Of CustodiasFraccionar))
        If Not lo.HasError Then
            ListaCustodiasCliente = dcProxy.CustodiasFraccionars
            If ListaCustodiasCliente.Count = 0 Then
                HabilitadoModificar = False
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron Títulos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                HabilitadoModificar = True
            End If
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes Custodias", _
                                 Me.ToString(), "TerminoTraerCustodiasCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            mlogANNA = obj.Value
            If mlogANNA = "NO" Then
                VisibleFungible = Visibility.Visible
            End If
        End If
    End Sub

    Private Sub Terminotraerespecieitem(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If Not IsNothing(ListaComboIsin) Then
                    ListaComboIsin.Clear()
                End If
                For Each li In lo.Entities.ToList
                    ListaComboIsin.Add(New CamposComboIsin With {.ID = li.IdItem, .Descripcion = li.Descripcion})
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "BuscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoSubmitChangesGrabar(ByVal So As SubmitOperation)
        Try
            If So.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se fraccionaron correctamente las custodias", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                IsBusy = True
                BuscarCustadias()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de parámetros
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks>SV20150305</remarks>
    Private Sub Terminotraerparametrolista(ByVal obj As LoadOperation(Of OYDUtilidades.valoresparametro))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            'Dim lista = obj.Entities.ToList
            For Each li In obj.Entities.ToList
                Select Case li.Parametro
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES"
                        If li.Valor = "SI" Then
                            logVisualizarMasDecimalesCantidad = True
                        Else
                            logVisualizarMasDecimalesCantidad = False
                        End If
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES_CONSULTA"
                        If li.Valor = "SI" Then
                            logVisualizarMasDecimalesCantidadConsulta = True
                        Else
                            logVisualizarMasDecimalesCantidadConsulta = False
                        End If
                End Select
            Next
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _FechaBusqueda As Date
    Public Property FechaBusqueda() As Date
        Get
            Return _FechaBusqueda
        End Get
        Set(ByVal value As Date)
            _FechaBusqueda = value
            MyBase.CambioItem("FechaBusqueda")
        End Set
    End Property

    Private _ListaCustodiasCliente As EntitySet(Of CustodiasFraccionar)
    Public Property ListaCustodiasCliente() As EntitySet(Of CustodiasFraccionar)
        Get
            Return _ListaCustodiasCliente
        End Get
        Set(ByVal value As EntitySet(Of CustodiasFraccionar))
            _ListaCustodiasCliente = value
            MyBase.CambioItem("ListaCustodiasCliente")
        End Set
    End Property
    Private WithEvents _CustodiasClienteSelected As CustodiasFraccionar
    Public Property CustodiasClienteSelected() As CustodiasFraccionar
        Get
            Return _CustodiasClienteSelected
        End Get
        Set(ByVal value As CustodiasFraccionar)
            _CustodiasClienteSelected = value
            MyBase.CambioItem("CustodiasClienteSelected")
        End Set
    End Property


    Private _logActivo As Boolean
    Public Property logActivo() As Boolean
        Get
            Return _logActivo
        End Get
        Set(ByVal value As Boolean)
            _logActivo = value
        End Set
    End Property

    Private _NombreColumna As String
    Public Property NombreColumna() As String
        Get
            Return _NombreColumna
        End Get
        Set(ByVal value As String)
            _NombreColumna = value
            MyBase.CambioItem("NombreColumna")
        End Set
    End Property

    Private _HabilitadoModificar As Boolean
    Public Property HabilitadoModificar() As Boolean
        Get
            Return _HabilitadoModificar
        End Get
        Set(ByVal value As Boolean)
            _HabilitadoModificar = value
            MyBase.CambioItem("HabilitadoModificar")
        End Set
    End Property



    Private _VisibleFungible As Visibility = Visibility.Collapsed
    Public Property VisibleFungible() As Visibility
        Get
            Return _VisibleFungible
        End Get
        Set(ByVal value As Visibility)
            _VisibleFungible = value
            MyBase.CambioItem("VisibleFungible")
        End Set
    End Property



    Private WithEvents _CamposBusquedaSelected As New CamposBusquedafraccionar
    Public Property CamposBusquedaSelected() As CamposBusquedafraccionar
        Get
            Return _CamposBusquedaSelected
        End Get
        Set(ByVal value As CamposBusquedafraccionar)
            _CamposBusquedaSelected = value
            MyBase.CambioItem("CamposBusquedaSelected")
        End Set
    End Property

    Private _ListaComboIsin As New List(Of CamposComboIsin)
    Public Property ListaComboIsin() As List(Of CamposComboIsin)
        Get
            Return _ListaComboIsin
        End Get
        Set(ByVal value As List(Of CamposComboIsin))
            _ListaComboIsin = value
            MyBase.CambioItem("ListaComboIsin")
        End Set
    End Property

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para la edición y cuando está prendido el parámetro CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES
    Private _logVisualizarMasDecimalesCantidad As Boolean = False
    Public Property logVisualizarMasDecimalesCantidad() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidad
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidad = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidad")
        End Set
    End Property

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para el modo de consulta CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES_CONSULTA
    Private _logVisualizarMasDecimalesCantidadConsulta As Boolean = False
    Public Property logVisualizarMasDecimalesCantidadConsulta() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidadConsulta
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidadConsulta = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidadConsulta")
        End Set
    End Property

#End Region

End Class

#Region "Propiedades Campos Búsqueda"
Public Class CamposBusquedafraccionar
    Implements INotifyPropertyChanged

    Private _CodigoCliente As String
    Public Property CodigoCliente() As String
        Get
            Return _CodigoCliente
        End Get
        Set(ByVal value As String)
            _CodigoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoCliente"))
        End Set
    End Property


    Private _NombreCliente As String
    Public Property NombreCliente() As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property

    Private _Nemotecnico As String
    Public Property Nemotecnico() As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
        End Set
    End Property


    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _TelefonoCliente As String
    Public Property TelefonoCliente() As String
        Get
            Return _TelefonoCliente
        End Get
        Set(ByVal value As String)
            _TelefonoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TelefonoCliente "))
        End Set
    End Property
    Private _NroDcto As String
    Public Property NroDcto() As String
        Get
            Return _NroDcto
        End Get
        Set(ByVal value As String)
            _NroDcto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroDcto"))
        End Set
    End Property

    Private _TipoDcto As String
    Public Property TipoDcto() As String
        Get
            Return _TipoDcto
        End Get
        Set(ByVal value As String)
            _TipoDcto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoDcto"))
        End Set
    End Property

    Private _DireccionCliente As String
    Public Property DireccionCliente() As String
        Get
            Return _DireccionCliente
        End Get
        Set(ByVal value As String)
            _DireccionCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DireccionCliente"))
        End Set
    End Property

    'Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class CamposComboIsin
    Implements INotifyPropertyChanged

    Private _ID As String
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property


    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
#End Region