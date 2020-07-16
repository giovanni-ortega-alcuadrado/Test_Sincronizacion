Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConsecutivosDocumentosViewModel.vb
'Generado el : 04/05/2011 13:47:03
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
Imports Microsoft.VisualBasic.CompilerServices

Public Class ConsecutivosDocumentosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaConsecutivosDocumento
    Private ConsecutivosDocumentoPorDefecto As ConsecutivosDocumento
    Private ConsecutivosDocumentoAnterior As ConsecutivosDocumento
    Public Property _mlogNuevo As Boolean = False
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim sw As Integer
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Public intIDCompaniaFirma As Integer
    Public strNombreCompaniaFirma As String

#Region "Variables"
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private _mobjCompaniaSeleccionadoAntes As OYDUtilidades.BuscadorGenerico
#End Region


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxy = New UtilidadesDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ConsecutivosDocumentosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosDocumentos, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConsecutivosDocumentoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosDocumentosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ConsecutivosDocumentosViewModel)(Me)
                mdcProxyUtilidad01.ItemCombos.Clear()
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosCondicionalQuery("COMPANIA_FIRMA", Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoConultarCompaniaFirma, String.Empty)

                mdcProxyUtilidad01.Verificaparametro("GMFCOMPENSACION_VB",Program.Usuario, Program.HashConexion, AddressOf TerminoTraerParametro, Nothing)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConsecutivosDocumentosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoConultarCompaniaFirma(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    intIDCompaniaFirma = lo.Entities.First.intID
                    strNombreCompaniaFirma = lo.Entities.First.Descripcion
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de la compañia",
                                                 Me.ToString(), "TerminoConultarCompaniaFirma", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de compañia",
                                                 Me.ToString(), "TerminoConultarCompaniaFirma", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerConsecutivosDocumentosPorDefecto_Completed(ByVal lo As LoadOperation(Of ConsecutivosDocumento))
        If Not lo.HasError Then
            ConsecutivosDocumentoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConsecutivosDocumento por defecto",
                                             Me.ToString(), "TerminoTraerConsecutivosDocumentoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConsecutivosDocumentos(ByVal lo As LoadOperation(Of ConsecutivosDocumento))
        If Not lo.HasError Then
            ListaConsecutivosDocumentos = dcProxy.ConsecutivosDocumentos
            If dcProxy.ConsecutivosDocumentos.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConsecutivosDocumentoSelected = ListaConsecutivosDocumentos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConsecutivosDocumentos",
                                             Me.ToString(), "TerminoTraerConsecutivosDocumento", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaConsecutivosDocumentos As EntitySet(Of ConsecutivosDocumento)
    Private WithEvents _ConsecutivosDocumentoSelected As ConsecutivosDocumento
    Public Property ListaConsecutivosDocumentos() As EntitySet(Of ConsecutivosDocumento)
        Get
            Return _ListaConsecutivosDocumentos
        End Get
        Set(ByVal value As EntitySet(Of ConsecutivosDocumento))
            _ListaConsecutivosDocumentos = value

            MyBase.CambioItem("ListaConsecutivosDocumentos")
            MyBase.CambioItem("ListaConsecutivosDocumentosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConsecutivosDocumentoAnterior) Then
                    ConsecutivosDocumentoSelected = _ListaConsecutivosDocumentos.FirstOrDefault
                Else
                    ConsecutivosDocumentoSelected = ConsecutivosDocumentoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConsecutivosDocumentosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConsecutivosDocumentos) Then
                Dim view = New PagedCollectionView(_ListaConsecutivosDocumentos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    'Private _ConsecutivosDocumentoSelected As ConsecutivosDocumento
    Public Property ConsecutivosDocumentoSelected() As ConsecutivosDocumento
        Get
            Return _ConsecutivosDocumentoSelected
        End Get
        Set(ByVal value As ConsecutivosDocumento)
            _ConsecutivosDocumentoSelected = value
            If Not value Is Nothing Then
                CompaniaConDoc = _ConsecutivosDocumentoSelected.Compania
            End If
            MyBase.CambioItem("ConsecutivosDocumentoSelected")
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


    Private _habilitar As Boolean = False
    Public Property habilitar() As Boolean
        Get
            Return _habilitar
        End Get
        Set(ByVal value As Boolean)
            _habilitar = value
            MyBase.CambioItem("habilitar")
        End Set
    End Property

    Private _habilitaMoneda As Boolean = False
    Public Property habilitaMoneda() As Boolean
        Get
            Return _habilitaMoneda
        End Get
        Set(ByVal value As Boolean)
            _habilitaMoneda = value
            MyBase.CambioItem("habilitaMoneda")
        End Set
    End Property

    Private _habilitarCompania As Boolean = False
    Public Property habilitarCompania() As Boolean
        Get
            Return _habilitarCompania
        End Get
        Set(ByVal value As Boolean)
            _habilitarCompania = value
            MyBase.CambioItem("habilitarCompania")
        End Set
    End Property

    Private _GMFCOMPENSACION_VB As Visibility = Visibility.Collapsed
    Public Property GMFCOMPENSACION_VB As Visibility
        Get
            Return _GMFCOMPENSACION_VB
        End Get
        Set(value As Visibility)
            _GMFCOMPENSACION_VB = value
            MyBase.CambioItem("GMFCOMPENSACION_VB")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewConsecutivosDocumento As New ConsecutivosDocumento
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewConsecutivosDocumento.IDComisionista = ConsecutivosDocumentoPorDefecto.IDComisionista
            NewConsecutivosDocumento.IDSucComisionista = ConsecutivosDocumentoPorDefecto.IDSucComisionista
            NewConsecutivosDocumento.Documento = ConsecutivosDocumentoPorDefecto.Documento
            NewConsecutivosDocumento.NombreConsecutivo = ConsecutivosDocumentoPorDefecto.NombreConsecutivo
            NewConsecutivosDocumento.Descripcion = ConsecutivosDocumentoPorDefecto.Descripcion
            NewConsecutivosDocumento.Cliente = ConsecutivosDocumentoPorDefecto.Cliente
            NewConsecutivosDocumento.CuentaContable = True
            NewConsecutivosDocumento.Actualizacion = ConsecutivosDocumentoPorDefecto.Actualizacion
            NewConsecutivosDocumento.Usuario = Program.Usuario
            NewConsecutivosDocumento.PermiteCliente = ConsecutivosDocumentoPorDefecto.PermiteCliente
            NewConsecutivosDocumento.TipoCuenta = ConsecutivosDocumentoPorDefecto.TipoCuenta
            NewConsecutivosDocumento.IdTarifa = Nothing
            NewConsecutivosDocumento.Signo = "$"
            NewConsecutivosDocumento.sucursalConciliacion = ConsecutivosDocumentoPorDefecto.sucursalConciliacion
            NewConsecutivosDocumento.IdSucursalSuvalor = ConsecutivosDocumentoPorDefecto.IdSucursalSuvalor
            NewConsecutivosDocumento.Concepto = False
            NewConsecutivosDocumento.ComprobanteContable = ConsecutivosDocumentoPorDefecto.ComprobanteContable
            NewConsecutivosDocumento.IncluidoEnExtractoBanco = ConsecutivosDocumentoPorDefecto.IncluidoEnExtractoBanco
            NewConsecutivosDocumento.IncluidoEnExtractoCliente = True
            NewConsecutivosDocumento.IDConsecutivoDocumento = ConsecutivosDocumentoPorDefecto.IDConsecutivoDocumento
            NewConsecutivosDocumento.IdMoneda = ConsecutivosDocumentoPorDefecto.IdMoneda
            NewConsecutivosDocumento.Compania = intIDCompaniaFirma
            NewConsecutivosDocumento.NombreCompania = strNombreCompaniaFirma
            NewConsecutivosDocumento.TipoCuenta = "N"
            ConsecutivosDocumentoAnterior = ConsecutivosDocumentoSelected
            ConsecutivosDocumentoSelected = NewConsecutivosDocumento
            'ConsecutivosDocumentoSelected.IdMoneda = 2
            PropiedadTextoCombos = ""
            MyBase.CambioItem("ConsecutivosDocumentos")
            Editando = True
            MyBase.CambioItem("Editando")
            habilitar = True
            habilitaMoneda = True
            '_CompaniaSeleccionado = Nothing
            'If NewConsecutivosDocumento.Compania = 0 Then
            habilitarCompania = True
            'Else
            '    habilitarCompania = False
            'End If
            _mlogNuevo = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    'DEMC20171004 INICIO
    Private Sub _ConsecutivosDocumentoSelected_propertychanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ConsecutivosDocumentoSelected.PropertyChanged
        Try
            If e.PropertyName.Equals("CuentaContable1") Then
                If Not String.IsNullOrEmpty(_ConsecutivosDocumentoSelected.CuentaContable1) Then
                    buscarGenerico(_ConsecutivosDocumentoSelected.CuentaContable1, "CuentasContables")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la propiedad.",
                                                         Me.ToString(), "_ConsecutivosDocumentoSelected_propertychanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Friend Sub buscarGenerico(Optional ByVal pstrCentroCostos As String = "", Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery(pstrBusqueda, pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cuenta contable", Me.ToString(), "buscarGenerico", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "CuentasContables"
                        If Editando Then
                            If lo.Entities.ToList.Count > 0 Then
                                _ConsecutivosDocumentoSelected.CuentaContable1 = lo.Entities.First.IdItem
                            Else
                                sw = 1
                                A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _ConsecutivosDocumentoSelected.CuentaContable1 = Nothing
                            End If
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de cuentas contables",
                                             Me.ToString(), "TerminoTraerBuscadorGenerico", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la cuenta contable", Me.ToString(),
                                                             "TerminoTraerBuscadorGenerico", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub
    'DEMC20171004 FIN

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ConsecutivosDocumentos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConsecutivosDocumentosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosDocumentos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConsecutivosDocumentosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosDocumentos, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Documento <> String.Empty Or
                cb.NombreConsecutivo <> String.Empty Or
                cb.Descripcion <> String.Empty Or
                cb.CuentaContable1 <> String.Empty Or
                cb.CuentaContable <> 0 Or
                cb.PermiteCliente <> String.Empty Or
                cb.TipoCuenta <> String.Empty Or
                cb.sucursalConciliacion <> String.Empty Or
                cb.IdSucursalSuvalor <> 0 Or
                cb.Concepto <> 0 Or
                cb.ComprobanteContable <> String.Empty Or
                cb.IncluidoEnExtractoBanco <> 0 Or
                cb.IncluidoEnExtractoCliente <> 0 Or
                cb.IdMoneda <> 0 Then 'Validar que ingresó algo en los campos de búsqueda

                If IsNothing(cb.Concepto) Then
                    cb.Concepto = 0
                End If
                If IsNothing(cb.IdSucursalSuvalor) Then
                    cb.IdSucursalSuvalor = 0
                End If
                If IsNothing(cb.IdMoneda) Then
                    cb.IdMoneda = 0
                End If

                ErrorForma = ""
                dcProxy.ConsecutivosDocumentos.Clear()
                ConsecutivosDocumentoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Documento = " &  cb.Documento.ToString() & " NombreConsecutivo = " &  cb.NombreConsecutivo.ToString() & " Descripcion = " &  cb.Descripcion.ToString() & " CuentaContable = " &  cb.CuentaContable.ToString() & " CuentaContable = " &  cb.CuentaContable.ToString() & " PermiteCliente = " &  cb.PermiteCliente.ToString() & " TipoCuenta = " &  cb.TipoCuenta.ToString() & " sucursalConciliacion = " &  cb.sucursalConciliacion.ToString() & " IdSucursalSuvalor = " &  cb.IdSucursalSuvalor.ToString() & " Concepto = " &  cb.Concepto.ToString() & " ComprobanteContable = " &  cb.ComprobanteContable.ToString() & " IncluidoEnExtractoBanco = " &  cb.IncluidoEnExtractoBanco.ToString() & " IdMoneda = " &  cb.IdMoneda.ToString() 
                dcProxy.Load(dcProxy.ConsecutivosDocumentosConsultarQuery(cb.Documento, cb.NombreConsecutivo, cb.Descripcion, cb.CuentaContable, cb.CuentaContable1,
                                                                          cb.PermiteCliente, cb.TipoCuenta, cb.sucursalConciliacion, cb.IdSucursalSuvalor, cb.Concepto,
                                                                          cb.ComprobanteContable, cb.IncluidoEnExtractoBanco, cb.IncluidoEnExtractoCliente, cb.IdMoneda,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivosDocumentos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConsecutivosDocumento
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim strmensaje As String
            strmensaje = ""
            'strmensaje = "Se creará el código de compañia: " + ConsecutivosDocumentoSelected.Compania.ToString() + " - " + ConsecutivosDocumentoSelected.NombreCompania + ", Confirma la grabación de la compañia?"
            Dim origen = "update"
            ErrorForma = ""
            If ValidarRegistro() Then
                ConsecutivosDocumentoAnterior = ConsecutivosDocumentoSelected
                If Not ListaConsecutivosDocumentos.Contains(ConsecutivosDocumentoSelected) Then
                    For Each i In ListaConsecutivosDocumentos
                        If i.Documento.Equals(ConsecutivosDocumentoSelected.Documento) And i.NombreConsecutivo.Equals(ConsecutivosDocumentoSelected.NombreConsecutivo) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El Documento y el Nombre de consecutivo ya existen", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                    origen = "insert"
                    ListaConsecutivosDocumentos.Add(ConsecutivosDocumentoSelected)
                End If

                'Dim Result As MessageBoxResult

                'If _mlogNuevo And habilitarCompania Then
                '    Result = MessageBox.Show(strmensaje, "Advertencia", MessageBoxButton.OKCancel)
                '    If Result = MessageBoxResult.Cancel Then
                '        A2Utilidades.Mensajes.mostrarMensaje("Se cancela la grabación de la compañia", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '        Exit Sub
                '    End If
                'End If

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
                habilitarCompania = False
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ConsecutivosDocumentoSelected) Then
            Editando = True
            habilitar = False
            habilitaMoneda = False
            habilitarCompania = False
            _mlogNuevo = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConsecutivosDocumentoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ConsecutivosDocumentoSelected.EntityState = EntityState.Detached Then
                    ConsecutivosDocumentoSelected = ConsecutivosDocumentoAnterior
                End If
            End If
            habilitar = False
            habilitaMoneda = False
            habilitarCompania = False
            _mlogNuevo = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaConsecutivosDocumento
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'If Not IsNothing(_ConsecutivosDocumentoSelected) Then
            '    dcProxy.ConsecutivosDocumentos.Remove(_ConsecutivosDocumentoSelected)
            '    ConsecutivosDocumentoSelected = _ListaConsecutivosDocumentos.LastOrDefault
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            'End If
            'habilitar = False
            _mlogNuevo = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("NombreConsecutivo", 1)
        DicCamposTab.Add("TipoCuenta", 1)
        DicCamposTab.Add("Descripcion", 1)
        DicCamposTab.Add("Documento", 1)
    End Sub

    Public Function ValidarRegistro() As Boolean
        Try
            Dim logValidacion As Boolean = True
            Dim strMensajeValidacion As String = String.Empty

            If IsNothing(ConsecutivosDocumentoSelected) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1}- Para grabar debes completar los campos Requeridos", strMensajeValidacion, vbCrLf)
            Else
                '----------------------------------------------------------------------------------
                'Validaciones de Actualizacion Registro
                If ConsecutivosDocumentoSelected.Compania <= 0 Or IsNothing(ConsecutivosDocumentoSelected.Compania) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- Debe elegir la compañía de las listadas.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.Documento) Or ConsecutivosDocumentoSelected.Documento = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El documento es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.TipoCuenta) Or ConsecutivosDocumentoSelected.TipoCuenta = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El SubMódulo es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.NombreConsecutivo) Or ConsecutivosDocumentoSelected.NombreConsecutivo = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El nombre del consecutivo es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.IdTarifa) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- La Tarifa es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.Signo) Or ConsecutivosDocumentoSelected.Signo = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El Símbolo Contable es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.IdMoneda) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- La sucursal Conciliacion es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.CuentaContable) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- La Cuenta Contable es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.Concepto) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El Concepto es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.IdSucursalSuvalor) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- La sucursal de la firma es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.IncluidoEnExtractoBanco) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El Extracto Banco es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.IncluidoEnExtractoCliente) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El Extracto Cliente es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If


                If IsNothing(ConsecutivosDocumentoSelected.PermiteCliente) Or ConsecutivosDocumentoSelected.PermiteCliente = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- El campo cliente  es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If IsNothing(ConsecutivosDocumentoSelected.IdMoneda) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- La Moneda es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If


                If IsNothing(ConsecutivosDocumentoSelected.Descripcion) Or ConsecutivosDocumentoSelected.Descripcion = "" Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1}- La descripción es un campo requerido.", strMensajeValidacion, vbCrLf)
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

    Private Sub TerminoTraerParametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminoTraerParametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "SI" Then
                GMFCOMPENSACION_VB = Visibility.Visible
            Else
                GMFCOMPENSACION_VB = Visibility.Collapsed
            End If
        End If
    End Sub

#End Region

#Region "Propiedades que definen atributos de la orden"

    Private _mCompania As Integer
    <Display(Name:="Compañía")>
    Public Property CompaniaConDoc() As Integer
        Get
            Return (_mCompania)
        End Get
        Set(ByVal value As Integer)
            If value.Equals(String.Empty) Then
                _mCompania = value
                _CompaniaSeleccionado = Nothing
            ElseIf Not Versioned.IsNumeric(value) Then
                A2Utilidades.Mensajes.mostrarMensaje("La compañìa debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _CompaniaSeleccionado Is Nothing Then
                    _mCompania = String.Empty
                Else
                    _mCompania = _CompaniaSeleccionado.IdItem
                End If
            ElseIf value.ToString.Length() > 7 Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima de la compañìa es de 7 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                If _CompaniaSeleccionado Is Nothing Then
                    _mCompania = String.Empty
                Else
                    _mCompania = _CompaniaSeleccionado.IdItem
                End If
            Else
                _mCompania = value

                ' If Not _ConsecutivosDocumentoSelected Is Nothing AndAlso (_CompaniaSeleccionado Is Nothing OrElse Not value.ToString.Equals(_CompaniaSeleccionado.IdItem)) Then
                If value.ToString IsNot Nothing AndAlso value > 0 Then
                    buscarCompania(value, "buscarCompaniaConDoc")
                End If
            End If
            MyBase.CambioItem("CompaniaConDoc")
        End Set
    End Property

    Private _CompaniaSeleccionado As OYDUtilidades.BuscadorGenerico
    Public Property CompaniaSeleccionado As OYDUtilidades.BuscadorGenerico
        Get
            Return (_CompaniaSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            Dim logIgual As Boolean = False

            If Not IsNothing(_CompaniaSeleccionado) AndAlso _CompaniaSeleccionado.Equals(value) Then
                Exit Property
            End If

            _CompaniaSeleccionado = value

            If Not value Is Nothing Then

                If _mlogNuevo Then
                    '_ConsecutivosDocumentoSelected.Compania = 0
                    _ConsecutivosDocumentoSelected.Compania = _CompaniaSeleccionado.IdItem
                End If

                '// Actualizar el campo para que se vea la compañia seleccionado
                'CompaniaConDoc = _ConsecutivosDocumentoSelected.Compania
                _mCompania = _CompaniaSeleccionado.IdItem

            End If
            MyBase.CambioItem("CompaniaSeleccionado")
        End Set
    End Property
#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"

    ''' <summary>
    ''' Buscar los datos de la compañia que tiene asignada el consecutivo documento.
    ''' </summary>
    ''' <param name="pintCompania">Compania que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    Friend Sub buscarCompania(Optional ByVal pintCompania As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Dim intCompania As Integer = 1

        Try
            If Not Me.ConsecutivosDocumentoSelected Is Nothing Then
                If Not Me.CompaniaSeleccionado Is Nothing And pintCompania.Equals(String.Empty) Then
                    intCompania = Me.CompaniaSeleccionado.IdItem
                End If

                If Not intCompania.Equals(Me.ConsecutivosDocumentoSelected.Compania) Then
                    If pintCompania = 0 Then
                        intCompania = Me.ConsecutivosDocumentoSelected.Compania
                    Else
                        intCompania = pintCompania
                    End If

                    If Not intCompania = 0 Then
                        mdcProxyUtilidad01.BuscadorGenericos.Clear()
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemsQuery(intCompania, "compania", "T", "soloposicionpropia", String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf buscarCompaniaCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarCompania", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Buscar los datos de la compañia que tiene asignado el consecutivo documento
    ''' Se dispara cuando la busqueda de la compañia iniciada desde el procedimiento buscarCompania finaliza
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub buscarCompaniaCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.UserState.ToString = "buscarCompaniaConDoc" Then
                    If lo.Entities.ToList.Where(Function(i) i.IdItem = CompaniaConDoc).Count > 0 Then
                        Me.CompaniaSeleccionado = lo.Entities.ToList.Where(Function(i) i.IdItem = CompaniaConDoc).First
                        ConsecutivosDocumentoSelected.Compania = CompaniaConDoc
                        ConsecutivosDocumentoSelected.NombreCompania = Me.CompaniaSeleccionado.CodigoAuxiliar
                    Else
                        Me.CompaniaSeleccionado = Nothing
                        If _mlogNuevo Then
                            A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                        CompaniaConDoc = 0
                    End If
                Else
                    Me.CompaniaSeleccionado = lo.Entities.ToList.Item(0)

                    If lo.UserState.ToString = "buscar" Then
                        _mobjCompaniaSeleccionadoAntes = _CompaniaSeleccionado
                    End If
                End If
            Else
                Me.CompaniaSeleccionado = Nothing
                If _mlogNuevo Then
                    A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                CompaniaConDoc = 0
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del consecutivo documento", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaConsecutivosDocumento

    <Display(Name:="Documento")> _
    Public Property Documento As String

    <Display(Name:="Nombre")> _
    Public Property NombreConsecutivo As String

    <Display(Name:="Descripción")> _
    Public Property Descripcion As String

    <Display(Name:="Cta Contable")> _
    Public Property CuentaContable1 As String

    <Display(Name:="Submódulo")> _
    Public Property TipoCuenta As String

    <Display(Name:="Tarifa")> _
    Public Property IdTarifa As Nullable(Of Integer)

    <Display(Name:="Comprobante Contable")> _
    Public Property ComprobanteContable As String

    <Display(Name:="Símbolo")> _
    Public Property Signo As String

    <Display(Name:="Sucursal Conciliación")> _
    Public Property sucursalConciliacion As String

    <Display(Name:="Cuenta Contable")> _
    Public Property CuentaContable As Boolean

    <Display(Name:="Concepto")> _
    Public Property Concepto As Nullable(Of Boolean)

    <Display(Name:="Id Sucursal Firma")> _
    Public Property IdSucursalSuvalor As Nullable(Of Integer)

    <Display(Name:="Extracto Banco")> _
    Public Property IncluidoEnExtractoBanco As Boolean

    <Display(Name:="Extracto Cliente")> _
    Public Property IncluidoEnExtractoCliente As Boolean

    <Display(Name:="Cliente")> _
    Public Property PermiteCliente As String

    <Display(Name:="Moneda")> _
    Public Property IdMoneda As Nullable(Of Integer)
End Class

