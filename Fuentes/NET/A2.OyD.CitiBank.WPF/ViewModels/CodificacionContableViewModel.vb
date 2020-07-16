Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CodificacionContableViewModel.vb
'Generado el : 09/01/2011 11:00:17
'Propiedad de Alcuadrado S.A. 2010
'Fecha: Sep-9-2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCodificacionContable
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDCitiBank

Public Class CodificacionContableViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCodificacionContabl
    Private CodificacionContablPorDefecto As CodificacionContabl
    Private CodificacionContablAnterior As CodificacionContabl
    Private _ListaConsecutivos As New List(Of A2.OyD.OYDServer.RIA.Web.OyDCitiBank.ConsecutivosDocumento)

    Dim dcProxy As CitiBankDomainContext
    Dim dcProxy1 As CitiBankDomainContext
    Dim dcProxy2 As CitiBankDomainContext
    Public Property DiccionarioCombosA2() As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Dim dicListaCombos As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Private _strTipoOperDoc As String = String.Empty
    Private _strConsecDoc As String = String.Empty
    Dim DicCamposTab As New Dictionary(Of String, Integer)




#Region "Declaraciones"

    Private Const TIPODOCCODIFICACION As String = "TIPODOCCODIFICACION"
    Private Const TIPOOPERCODIFICACION As String = "TIPOOPERCODIFICACION"
    Private Const TIPOOPEROTCCONTABLE As String = "TIPOOPEROTCCONTABLE"
    Private Const TIPOOPERCUSTCODIFICA As String = "TIPOOPERCUSTCODIFICA"

#End Region

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New CitiBankDomainContext()
            dcProxy1 = New CitiBankDomainContext()
            dcProxy2 = New CitiBankDomainContext()
        Else
            dcProxy = New CitiBankDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New CitiBankDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy2 = New CitiBankDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Try

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CodificacionContableFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodificacionContable, "")
                dcProxy1.Load(dcProxy1.TraerCodificacionContablPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodificacionContablePorDefecto_Completed, "Default")
                dcProxy2.Load(dcProxy2.ListarConsecutivosDocumentosQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListaConsecutivo, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CodificacionContableViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try

            ErrorForma = ""
            dcProxy.CodificacionContabls.Clear()
            CodificacionContablAnterior = Nothing
            IsBusy = True
            If cb.IDCodificacion = 0 Then
                cb.IDCodificacion = Nothing
            End If

            If Not cb.Modulo Is Nothing Then
                If cb.Modulo.Equals("-1") Then
                    cb.Modulo = Nothing
                End If
            End If

            dcProxy.Load(dcProxy.CodificacionContableConsultarQuery(cb.IDCodificacion, cb.Modulo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodificacionContable, "Busqueda")
            MyBase.ConfirmarBuscar()
            cb = New CamposBusquedaCodificacionContabl
            CambioItem("cb")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCodificacionContablePorDefecto_Completed(ByVal lo As LoadOperation(Of CodificacionContabl))

        If Not lo.HasError Then
            CodificacionContablPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CodificacionContabl por defecto", _
                                             Me.ToString(), "TerminoTraerCodificacionContablPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerListaConsecutivo(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OyDCitiBank.ConsecutivosDocumento))

        If Not lo.HasError Then
            _ListaConsecutivos = dcProxy2.ConsecutivosDocumentos.ToList()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Consecutivos", _
                                             Me.ToString(), "TerminoTraerListaConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub

    Private Sub TerminoTraerCodificacionContable(ByVal lo As LoadOperation(Of CodificacionContabl))
        If Not lo.HasError Then
            ListaCodificacionContable = dcProxy.CodificacionContabls
            If dcProxy.CodificacionContabls.Count > 0 Then
                If lo.UserState = "insert" Then
                    CodificacionContablSelected = ListaCodificacionContable.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MessageBox.Show("No se encontro ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CodificacionContable", _
                                             Me.ToString(), "TerminoTraerCodificacionContabl", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Public Sub SeleccionarRegistro()
        Call Seleccion("Modulo")
        Call Seleccion("TipoOperacion")
        CambiarAForma()
    End Sub


#End Region

#Region "Propiedades"

    Private _LabelConsecutivo As String = "Consecutivo"
    Public Property LabelConsecutivo() As String
        Get
            Return _LabelConsecutivo
        End Get
        Set(ByVal value As String)
            _LabelConsecutivo = value
            MyBase.CambioItem("LabelConsecutivo")
        End Set
    End Property

    Private _ListaCodificacionContable As EntitySet(Of CodificacionContabl)
    Public Property ListaCodificacionContable() As EntitySet(Of CodificacionContabl)
        Get
            Return _ListaCodificacionContable
        End Get
        Set(ByVal value As EntitySet(Of CodificacionContabl))
            _ListaCodificacionContable = value

            MyBase.CambioItem("ListaCodificacionContable")
            MyBase.CambioItem("ListaCodificacionContablePaged")
            If Not IsNothing(value) Then
                If IsNothing(CodificacionContablAnterior) Then
                    CodificacionContablSelected = _ListaCodificacionContable.FirstOrDefault
                Else
                    CodificacionContablSelected = CodificacionContablAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCodificacionContablePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCodificacionContable) Then
                Dim view = New PagedCollectionView(_ListaCodificacionContable)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CodificacionContablSelected As CodificacionContabl

    Public Property CodificacionContablSelected() As CodificacionContabl
        Get
            Return _CodificacionContablSelected
        End Get
        Set(ByVal value As CodificacionContabl)
            _CodificacionContablSelected = value
            If Not IsNothing(_CodificacionContablSelected) Then
                Call Seleccion("Modulo")
                Call Seleccion("TipoOperacion")
            End If
            MyBase.CambioItem("CodificacionContablSelected")
        End Set
    End Property


    Public ReadOnly Property ListaComboValorRegistrar() As ObservableCollection(Of ListaPorOperacion)
        Get
            Dim objPorOperacion As New ObservableCollection(Of ListaPorOperacion)
            objPorOperacion.Add(New ListaPorOperacion With {.ID = True, .Descripcion = "Operación"})
            objPorOperacion.Add(New ListaPorOperacion With {.ID = False, .Descripcion = "Sumatoria"})

            Return objPorOperacion
        End Get

    End Property

    Public Sub ConsultarRecurso()
        If DiccionarioCombosA2 Is Nothing Or DiccionarioCombosA2.Count <= 0 Then
            If Not Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS) Is Nothing Then
                DiccionarioCombosA2 = Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS)
            End If
        End If
    End Sub

    Private WithEvents objListOperDoc As New ObservableCollection(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)

    Public Property ListaTipoOperacionDocumento() As ObservableCollection(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
        Get
            Return objListOperDoc
        End Get
        Set(ByVal value As ObservableCollection(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo))
            MyBase.CambioItem("ListaConsecutivoDocumento")
        End Set
    End Property

    Public Sub SeleccionaOperDoc()
        MyBase.CambioItem("ListaConsecutivoDocumento")
    End Sub

    Private WithEvents objList As New ObservableCollection(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)

    Public ReadOnly Property ListaConsecutivoDocumento() As ObservableCollection(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo)
        Get
            Return objList
        End Get
    End Property

    Private _strLabelTipoOperacion As String
    Public Property LabelTipoOperacion() As String
        Get
            Return _strLabelTipoOperacion
        End Get
        Set(ByVal value As String)
            _strLabelTipoOperacion = value
            MyBase.CambioItem("LabelTipoOperacion")
        End Set
    End Property

    Private _bolVisibilityConsecutivo As Visibility
    Public Property VisibilityConsecutivo() As Visibility
        Get
            Return _bolVisibilityConsecutivo
        End Get
        Set(ByVal value As Visibility)
            _bolVisibilityConsecutivo = value
            If _bolVisibilityConsecutivo = Visibility.Visible Then
                LabelConsecutivo = "Consecutivo"
            Else
                LabelConsecutivo = String.Empty
            End If
            MyBase.CambioItem("VisibilityConsecutivo")
        End Set
    End Property

    Private _bolHabilitaConSucursal As Boolean
    Public Property HabilitaConSucursal() As Boolean
        Get
            Return _bolHabilitaConSucursal
        End Get
        Set(ByVal value As Boolean)
            _bolHabilitaConSucursal = value
            MyBase.CambioItem("HabilitaConSucursal")
        End Set
    End Property


    Private _bolHabilitaUsarFecha As Boolean
    Public Property HabilitaUsarFecha() As Boolean
        Get
            Return _bolHabilitaUsarFecha
        End Get
        Set(ByVal value As Boolean)
            _bolHabilitaUsarFecha = value
            MyBase.CambioItem("HabilitaUsarFecha")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")

        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewCodificacionContabl As New CodificacionContabl
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCodificacionContabl.IDCodificacion = CodificacionContablPorDefecto.IDCodificacion
            NewCodificacionContabl.IDComisionista = CodificacionContablPorDefecto.IDComisionista
            NewCodificacionContabl.IDSucComisionista = CodificacionContablPorDefecto.IDSucComisionista
            NewCodificacionContabl.Modulo = CodificacionContablPorDefecto.Modulo
            NewCodificacionContabl.TipoOperacion = CodificacionContablPorDefecto.TipoOperacion
            NewCodificacionContabl.UsarFecha = CodificacionContablPorDefecto.UsarFecha
            NewCodificacionContabl.TipoCliente = CodificacionContablPorDefecto.TipoCliente
            NewCodificacionContabl.Branch = CodificacionContablPorDefecto.Branch
            NewCodificacionContabl.CuentaCosmos = CodificacionContablPorDefecto.CuentaCosmos
            NewCodificacionContabl.CodigoTransaccion = CodificacionContablPorDefecto.CodigoTransaccion
            NewCodificacionContabl.IndicadorMvto = CodificacionContablPorDefecto.IndicadorMvto
            NewCodificacionContabl.NroLote = CodificacionContablPorDefecto.NroLote
            NewCodificacionContabl.DetalleAdicional = CodificacionContablPorDefecto.DetalleAdicional
            NewCodificacionContabl.TextoDetalle = CodificacionContablPorDefecto.TextoDetalle
            NewCodificacionContabl.NroReferencia = CodificacionContablPorDefecto.NroReferencia
            NewCodificacionContabl.PorOperacion = CodificacionContablPorDefecto.PorOperacion
            NewCodificacionContabl.VlrAReportar = CodificacionContablPorDefecto.VlrAReportar
            NewCodificacionContabl.Producto = CodificacionContablPorDefecto.Producto
            NewCodificacionContabl.NroBase = CodificacionContablPorDefecto.NroBase
            NewCodificacionContabl.SucursalContable = CodificacionContablPorDefecto.SucursalContable
            NewCodificacionContabl.ConsecutivoTesoreria = CodificacionContablPorDefecto.ConsecutivoTesoreria
            NewCodificacionContabl.Actualizacion = CodificacionContablPorDefecto.Actualizacion
            NewCodificacionContabl.Usuario = Program.Usuario
            CodificacionContablAnterior = CodificacionContablSelected
            CodificacionContablSelected = NewCodificacionContabl
            MyBase.CambioItem("CodificacionContable")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CodificacionContabls.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CodificacionContableFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodificacionContable, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CodificacionContableFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodificacionContable, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            If DatosValidos() Then


                CodificacionContablAnterior = CodificacionContablSelected

                Dim origen = "update"
                ErrorForma = ""
                If Not ListaCodificacionContable.Contains(CodificacionContablSelected) Then
                    origen = "insert"
                    'CodificacionContablSelected.IDCodificacion = Nothing
                    ListaCodificacionContable.Add(CodificacionContablSelected)
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

    Public Function DatosValidos() As Boolean
        Dim objRetorno As Boolean = True
        Dim strMsg As String = String.Empty

        If String.IsNullOrEmpty(CodificacionContablSelected.Modulo) Then
            strMsg = strMsg & "* El campo <Módulo> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If Not String.IsNullOrEmpty(CodificacionContablSelected.Modulo) And String.IsNullOrEmpty(CodificacionContablSelected.TipoOperacion) Then
            If VisibilityConsecutivo = Visibility.Visible Then
                strMsg = strMsg & "* El campo <Tipo documento> es requerido. Por favor digítelo" & vbCrLf
                objRetorno = False
            Else
                strMsg = strMsg & "* El campo <Tipo operación> de la consulta es requerido. Por favor digítelo" & vbCrLf
                objRetorno = False
            End If
        End If

        If Not String.IsNullOrEmpty(CodificacionContablSelected.Modulo) And Not String.IsNullOrEmpty(CodificacionContablSelected.TipoOperacion) And String.IsNullOrEmpty(CodificacionContablSelected.ConsecutivoTesoreria) Then
            If VisibilityConsecutivo = Visibility.Visible Then
                strMsg = strMsg & "* El campo <Consecutivo> es requerido. Por favor digítelo" & vbCrLf
                objRetorno = False
            End If
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.Modulo) OrElse (Not CodificacionContablSelected.Modulo.Equals("T") And Not CodificacionContablSelected.Modulo.Equals("CT")) Then
            If String.IsNullOrEmpty(CodificacionContablSelected.UsarFecha) Then
                strMsg = strMsg & "* El campo <Usar Fecha> es requerido. Por favor digítelo" & vbCrLf
                objRetorno = False
            End If
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.TipoCliente) Then
            strMsg = strMsg & "* El campo <Tipo Cliente> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If CodificacionContablSelected.Branch <= 0 Then
            strMsg = strMsg & "* El campo <Branch> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If CodificacionContablSelected.CuentaCosmos Is Nothing Then
            strMsg = strMsg & "* El campo <Cuenta Cosmos> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If CodificacionContablSelected.CodigoTransaccion <= 0 Then
            strMsg = strMsg & "* El campo <Cod. de la transacción> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.Producto) Then
            strMsg = strMsg & "* El campo <Código del producto> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.NroBase) Then
            strMsg = strMsg & "* El campo <Nro. Base> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.IndicadorMvto) Then
            strMsg = strMsg & "* El campo <Indicador Mvto> es requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If Len(CodificacionContablSelected.NroLote) <> 6 Then
            strMsg = strMsg & "* El campo <Nro. lote> debe ser de 6 dígitos, los 3 primeros para el departamento y los 3 restantes para lote, Por favor rectifique" & vbCrLf
            objRetorno = False
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.DetalleAdicional) Then
            strMsg = strMsg & "* El campo <Detalle adicional> es un campo requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If String.IsNullOrEmpty(CodificacionContablSelected.NroReferencia) Then
            strMsg = strMsg & "* El campo <Nro de referencia> es un campo requerido. Por favor digítelo" & vbCrLf
            objRetorno = False
        End If

        If CodificacionContablSelected.PorOperacion Is Nothing Then
            strMsg = strMsg & "* El valor a registrar es requerido. Por favor escoja la opción (por operación, por sumatoria)" & vbCrLf
            objRetorno = False
        End If

        If Not String.IsNullOrEmpty(CodificacionContablSelected.Modulo) AndAlso
            (CodificacionContablSelected.Modulo.Equals("F") Or _
            CodificacionContablSelected.Modulo.Equals("A") Or _
            CodificacionContablSelected.Modulo.Equals("O") Or _
            CodificacionContablSelected.Modulo.Equals("AE") Or _
            CodificacionContablSelected.Modulo.Equals("FE")) Then
            If String.IsNullOrEmpty(CodificacionContablSelected.VlrAReportar) Then
                strMsg = strMsg & "* El campo <Valor a reportar> es un campo requerido. Por favor dígitelo" & vbCrLf
                objRetorno = False
            End If
        End If

        If Not objRetorno Then
            A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.Usuario, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If

        Return objRetorno
    End Function

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then

                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "borrar" Then
                    CodificacionContablSelected = ListaCodificacionContable.Last
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
            SeleccionarRegistro()
            MyBase.CambioItem("CodificacionContablSelected")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CodificacionContablSelected) Then
            'SeleccionarRegistro()
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CodificacionContablSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _CodificacionContablSelected.EntityState = EntityState.Detached Then
                    CodificacionContablSelected = CodificacionContablAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_CodificacionContablSelected) Then
                dcProxy.CodificacionContabls.Remove(_CodificacionContablSelected)
                CodificacionContablSelected = _ListaCodificacionContable.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _CodificacionContablSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CodificacionContablSelected.PropertyChanged
        Call Seleccion(e.PropertyName)
    End Sub

    Private Sub _objListOperDoc_PropertyChanged(ByVal sender As Object, ByVal e As System.Collections.Specialized.NotifyCollectionChangedEventArgs) Handles objListOperDoc.CollectionChanged
        MyBase.CambioItem("ListaConsecutivoDocumento")
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Modulo", 1)
        DicCamposTab.Add("TipoOperacion", 1)
        DicCamposTab.Add("UsarFecha", 1)
        DicCamposTab.Add("TipoCliente", 1)
    End Sub

#End Region

    Private Sub Seleccion(pPropertyName As String)

        Select Case pPropertyName
            Case "Modulo"
                Call SeleccionMod()
            Case "TipoOperacion"
                Select Case CodificacionContablSelected.TipoOperacion
                    Case "RC"
                        _strConsecDoc = "CAJA"
                    Case "CE"
                        _strConsecDoc = "EGRESOS"
                    Case "N"
                        _strConsecDoc = "NOTAS"
                    Case Else
                        _strConsecDoc = ""
                End Select

                objList.Clear()

                ConsultarRecurso()

                For Each lis In _ListaConsecutivos
                    If lis.strDocumento.ToUpper.Equals(_strConsecDoc.ToUpper()) Then
                        objList.Add(New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo With {.ID = lis.strDocumento, .Descripcion = lis.strNombreConsecutivo})
                    End If
                Next

                Dim op = CodificacionContablSelected.TipoOperacion
                CodificacionContablSelected.TipoOperacion = op
                If Editando = True Then
                    CodificacionContablSelected.ConsecutivoTesoreria = String.Empty
                End If
                CodificacionContablSelected.Usuario = Program.Usuario

            Case Else
        End Select


    End Sub

    Private Sub SeleccionMod()
        Select Case _CodificacionContablSelected.Modulo
            Case "F", "A", "AE", "FE"
                _strTipoOperDoc = TIPOOPERCODIFICACION
                TipoOperacion(True, True, False)
            Case "T"
                _strTipoOperDoc = TIPODOCCODIFICACION
                TipoOperacion(False, False, False)
            Case "O"
                _strTipoOperDoc = TIPOOPEROTCCONTABLE
                TipoOperacion(True, True, False)
            Case "C"
                _strTipoOperDoc = TIPOOPERCUSTCODIFICA
                TipoOperacion(True, False, True)
        End Select

        ConsultarRecurso()

        objListOperDoc.Clear()

        Try

            Dim objListaNodosCategoria = (From ob In DiccionarioCombosA2 Where ob.Key = _strTipoOperDoc).ToList()

            For Each lis In objListaNodosCategoria
                For Each sublis In lis.Value
                    objListOperDoc.Add(New A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo With {.ID = sublis.ID, .Descripcion = sublis.Descripcion})
                Next
            Next

            If Editando = True Then
                CodificacionContablSelected.TipoOperacion = String.Empty
                CodificacionContablSelected.ConsecutivoTesoreria = String.Empty
            End If

        Catch ex As Exception
            objListOperDoc = Nothing
        End Try


        MyBase.CambioItem("ListaTipoOperacionDocumento")
        MyBase.CambioItem("ListaConsecutivoDocumento")
    End Sub

    Private Sub TipoOperacion(pbolEsOper As Boolean, pbolConSucursal As Boolean, pbolCustodia As Boolean)

        If pbolEsOper Then
            LabelTipoOperacion = "Tipo Operación"
            VisibilityConsecutivo = Visibility.Collapsed
            HabilitaUsarFecha = True
        ElseIf Not pbolEsOper Then
            CodificacionContablSelected.UsarFecha = ""
            LabelTipoOperacion = "Tipo Documento"
            VisibilityConsecutivo = Visibility.Visible
            HabilitaUsarFecha = False
        End If

        HabilitaConSucursal = pbolConSucursal


    End Sub

End Class


Public Class ListaPorOperacion
    Public Property ID As Boolean
    Public Property Descripcion As String
End Class

Public Class CamposBusquedaCodificacionContabl
    Implements INotifyPropertyChanged


    Private _lngIDCodificacion As Integer
    <Display(Name:="Indicador de Codificación")> _
    Public Property IDCodificacion As Integer
        Get
            Return _lngIDCodificacion
        End Get
        Set(ByVal value As Integer)
            _lngIDCodificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCodificacion"))
        End Set
    End Property


    Private _strModulo As String
    <Display(Name:="Módulos")> _
    Public Property Modulo As String
        Get
            Return _strModulo
        End Get
        Set(ByVal value As String)
            _strModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modulo"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

