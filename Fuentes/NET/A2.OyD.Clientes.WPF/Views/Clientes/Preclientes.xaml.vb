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
Partial Public Class Preclientes
    Inherits Window
    Implements INotifyPropertyChanged

    Dim dcProxy As ClientesDomainContext
    Public mlngIDPreclientes As Integer
    Public logPrecliente As Boolean
    Public logExisteAlgunRegistroRechado As Boolean = False

    Dim contadorclic As Integer
    Dim mobjServicio As A2OYDClientes.WSPhoenix.ClientesPhClient

    Private RUTA_SERVICIO_PHOENIX As String = "RUTA_SERVICIO_PHOENIX"
    Private UTILIZA_SERVICIO_PHOENIX As String = "UTILIZA_SERVICIO_PHOENIX"
    Private VALIDACIONES_PHOENIX As String = "VALIDACIONES_PHOENIX"
    Private strUtilizaWSPhoenix As String = "NO"
    Private strRutaServicioPhoenix As String
    Private strTipoIdentificacionHomologado As String = String.Empty

    Public Sub New()

        InitializeComponent()

        Me.Preclientes.DataContext = Me

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Try
            mlngIDPreclientes = 0

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.PreClientes.Clear()
                dcProxy.Load(dcProxy.ConsultaPreclientesQuery("E", String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPreClientes, "FiltroInicial")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "Preclientes.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub OKButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles OKButton.Click
        Try
            If ListaPreClientes.Count = 0 Or IsNothing(ListaPreClientes) Then
                Exit Sub
            End If

            If PreClientesSelected.IDPreCliente <> 0 And Not IsNothing(PreClientesSelected.IDPreCliente) Then
                mlngIDPreclientes = PreClientesSelected.IDPreCliente
                If PreClientesSelected.Rechazar Then
                    logPrecliente = False
                Else
                    logPrecliente = True
                End If
                'aqui se graba
                IsBusy = True
                Await modificarestado(True)
            Else
                logPrecliente = False
            End If

            'If logPrecliente Then
            '    'llenar variable en clientes con=mlngIDPreclientes
            '    'llenar variable en clientes con=true
            '    'llenarformapreclientes
            'Else
            '    'llenar variable en clientes con=false
            'End If

            'No debe cerrar la venta si rechazó algún registro.
            If Not logExisteAlgunRegistroRechado Then
                Me.DialogResult = True
            Else
                dcProxy.PreClientes.Clear()
                dcProxy.Load(dcProxy.ConsultaPreclientesQuery("E", strNroDocumento, strTipoIdentificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPreClientes, "FiltroInicial")
            End If

            'Me.Close()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Aceptar.",
                                 Me.ToString(), "OKButton_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function modificarestado(ByVal lograchazo As Boolean) As Task
        Try
            Dim strMsg As String = ""
            Dim objRet As InvokeOperation(Of String)
            logExisteAlgunRegistroRechado = False

            If lograchazo Then
                For Each li In ListaPreClientes
                    If li.Rechazar Then

                        logExisteAlgunRegistroRechado = True

                        objRet = Await dcProxy.ModificarPreclientesSync(li.IDPreCliente, "CR", Program.Usuario, Program.HashConexion).AsTask()

                        If objRet.HasError Then
                            objRet.MarkErrorAsHandled()
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al actualizar el estado del registro PreCliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            IsBusy = False
                            logPrecliente = False
                        End If
                    End If
                Next
            Else
                objRet = Await dcProxy.ModificarPreclientesSync(mlngIDPreclientes, "C", Program.Usuario, Program.HashConexion).AsTask()

                If objRet.HasError Then
                    objRet.MarkErrorAsHandled()
                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al actualizar el estado del registro PreCliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Else
                    IsBusy = False
                    logPrecliente = False
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el estado del registro PreCliente.",
                     Me.ToString(), "modificarestado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Private Sub CancelButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Async Sub GrabarPreClientesConDatosDesdePhoenix(pobjWS_ResultadoConsultaClientes As A2OYDClientes.WSPhoenix.WS_ResultadoConsultaClientes, pobjWS_ResultadoConsultaDirecciones As A2OYDClientes.WSPhoenix.WS_ResultadoConsultaDirecciones)
        Try
            Dim xmlCompletoClientes As String
            Dim xmlCompletoDirecciones As String
            Dim xmlDetalleClientes As String
            Dim xmlDetalleDirecciones As String

            'CLIENTES
            xmlCompletoClientes = "<Phoenix>"

            xmlDetalleClientes = "<Detalle Error=""" & pobjWS_ResultadoConsultaClientes.Error &
                                    """ Descripcion = """ & pobjWS_ResultadoConsultaClientes.Descripcion &
                                    """ nRimNo=""" & pobjWS_ResultadoConsultaClientes.nRimNo &
                                    """ sRimType=""" & pobjWS_ResultadoConsultaClientes.sRimType &
                                    """ sSex=""" & pobjWS_ResultadoConsultaClientes.sSex &
                                    """ nTitleId=""" & pobjWS_ResultadoConsultaClientes.nTitleId &
                                    """ sLastName=""" & pobjWS_ResultadoConsultaClientes.sLastName &
                                    """ sFirstName=""" & pobjWS_ResultadoConsultaClientes.sFirstName &
                                    """ sMiddleInitial=""" & pobjWS_ResultadoConsultaClientes.sMiddleInitial &
                                    """ sPreferredName=""" & pobjWS_ResultadoConsultaClientes.sPreferredName &
                                    """ sSuffix=""" & pobjWS_ResultadoConsultaClientes.sSuffix &
                                    """ sTaxIdNumber=""" & pobjWS_ResultadoConsultaClientes.sTaxIdNumber &
                                    """ sTinFormat=""" & pobjWS_ResultadoConsultaClientes.sTinFormat &
                                    """ dtBirthDt=""" & pobjWS_ResultadoConsultaClientes.dtBirthDt &
                                    """ dtDeceasedDt=""" & pobjWS_ResultadoConsultaClientes.dtDeceasedDt &
                                    """ sCityOfBirth=""" & pobjWS_ResultadoConsultaClientes.sCityOfBirth &
                                    """ nIdentId=""" & pobjWS_ResultadoConsultaClientes.nIdentId &
                                    """ sIdValue=""" & pobjWS_ResultadoConsultaClientes.sIdValue &
                                    """ sMotherMaidenName=""" & pobjWS_ResultadoConsultaClientes.sMotherMaidenName &
                                    """ dtEffectiveDt=""" & pobjWS_ResultadoConsultaClientes.dtEffectiveDt &
                                    """ sStatus=""" & pobjWS_ResultadoConsultaClientes.sStatus &
                                    """ nClassCode=""" & pobjWS_ResultadoConsultaClientes.nClassCode &
                                    """ nBranchNo=""" & pobjWS_ResultadoConsultaClientes.nBranchNo &
                                    """ nRsmIdNo=""" & pobjWS_ResultadoConsultaClientes.nRsmIdNo &
                                    """ sTINRsm=""" & pobjWS_ResultadoConsultaClientes.sTINRsm &
                                    """ nRiskCode=""" & pobjWS_ResultadoConsultaClientes.nRiskCode &
                                    """ sRiskReason=""" & pobjWS_ResultadoConsultaClientes.sRiskReason &
                                    """ sTaxIdCertified=""" & pobjWS_ResultadoConsultaClientes.sTaxIdCertified &
                                    """ dtBackupStatDt=""" & pobjWS_ResultadoConsultaClientes.dtBackupStatDt &
                                    """ sTaxExempReason=""" & pobjWS_ResultadoConsultaClientes.sTaxExempReason &
                                    """ dtExempExpiryDt=""" & pobjWS_ResultadoConsultaClientes.dtExempExpiryDt &
                                    """ dtCertDate=""" & pobjWS_ResultadoConsultaClientes.dtCertDate &
                                    """ sBackupWhithold=""" & pobjWS_ResultadoConsultaClientes.sBackupWhithold &
                                    """ sBackupWhReason=""" & pobjWS_ResultadoConsultaClientes.sBackupWhReason &
                                    """ dtBackupStartDt=""" & pobjWS_ResultadoConsultaClientes.dtBackupStartDt &
                                    """ dtBackupStatChDt=""" & pobjWS_ResultadoConsultaClientes.dtBackupStatChDt &
                                    """ dtBNotice1Dt=""" & pobjWS_ResultadoConsultaClientes.dtBNotice1Dt &
                                    """ dtBNotice2Dt=""" & pobjWS_ResultadoConsultaClientes.dtBNotice2Dt &
                                    """ nRestrictId=""" & pobjWS_ResultadoConsultaClientes.nRestrictId &
                                    """ sRegOCode=""" & pobjWS_ResultadoConsultaClientes.sRegOCode &
                                    """ sRetainDebtHist=""" & pobjWS_ResultadoConsultaClientes.sRetainDebtHist &
                                    """ sPurchased=""" & pobjWS_ResultadoConsultaClientes.sPurchased &
                                    """ sCountryCode=""" & pobjWS_ResultadoConsultaClientes.sCountryCode &
                                    """ nUserIdNo=""" & pobjWS_ResultadoConsultaClientes.nUserIdNo &
                                    """ sPotential=""" & pobjWS_ResultadoConsultaClientes.sPotential &
                                    """ sEconomicGroup=""" & pobjWS_ResultadoConsultaClientes.sEconomicGroup &
                                    """ nSicCode=""" & pobjWS_ResultadoConsultaClientes.nSicCode &
                                    """ sEconomicSector=""" & pobjWS_ResultadoConsultaClientes.sEconomicSector &
                                    """ nTintypeId=""" & pobjWS_ResultadoConsultaClientes.nTintypeId &
                                    """ sCountryOfBirth=""" & pobjWS_ResultadoConsultaClientes.sCountryOfBirth &
                                    """ sTitleECode=""" & pobjWS_ResultadoConsultaClientes.sTitleECode &
                                    """ SCityOfBirthECode=""" & pobjWS_ResultadoConsultaClientes.SCityOfBirthECode &
                                    """ sStateOfBirth=""" & pobjWS_ResultadoConsultaClientes.sStateOfBirth &
                                    """ sCountyOfBirth=""" & pobjWS_ResultadoConsultaClientes.sCountyOfBirth &
                                    """ nClasifPrev=""" & pobjWS_ResultadoConsultaClientes.nClasifPrev &
                                    """ nDateClasif=""" & pobjWS_ResultadoConsultaClientes.nDateClasif &
                                    """ nDateLastClasif=""" & pobjWS_ResultadoConsultaClientes.nDateLastClasif &
                                    """ sClasifType=""" & pobjWS_ResultadoConsultaClientes.sClasifType &
                                    """ sSector_type=""" & pobjWS_ResultadoConsultaClientes.sSector_type &
                                    """ sUNBankTypeNom=""" & pobjWS_ResultadoConsultaClientes.sUNBankTypeNom &
                                    """ nRowCount1=""" & pobjWS_ResultadoConsultaClientes.nRowCount1 &
                                    """ nRowCount2=""" & pobjWS_ResultadoConsultaClientes.nRowCount2 &
                                    """ nPdreRimNo=""" & pobjWS_ResultadoConsultaClientes.nPdreRimNo &
                                    """ sPdreFirstName=""" & pobjWS_ResultadoConsultaClientes.sPdreFirstName &
                                    """ sPdreLastName=""" & pobjWS_ResultadoConsultaClientes.sPdreLastName &
                                    """ sIndcdorCbzaGrpo=""" & pobjWS_ResultadoConsultaClientes.sIndcdorCbzaGrpo &
                                    """ sAddressLine1=""" & pobjWS_ResultadoConsultaClientes.sAddressLine1 &
                                    """ sPhone1=""" & pobjWS_ResultadoConsultaClientes.sPhone1 &
                                    """ sPhone2=""" & pobjWS_ResultadoConsultaClientes.sPhone2 &
                                    """ sFaxPhone=""" & pobjWS_ResultadoConsultaClientes.sFaxPhone &
                                    """ sCrreoElctrnco=""" & pobjWS_ResultadoConsultaClientes.sCrreoElctrnco & """></Detalle>"


            xmlCompletoClientes = xmlCompletoClientes & xmlDetalleClientes & "</Phoenix>"


            'CLIENTES DIRECCIONES
            xmlCompletoDirecciones = "<Phoenix>"

            xmlDetalleDirecciones = "<Detalle Error=""" & pobjWS_ResultadoConsultaDirecciones.Error &
                                    """ Descripcion = """ & pobjWS_ResultadoConsultaDirecciones.Descripcion &
                                    """ rnAddrId=""" & pobjWS_ResultadoConsultaDirecciones.rnAddrId &
                                    """ rsAddrLine1=""" & pobjWS_ResultadoConsultaDirecciones.rsAddrLine1 &
                                    """ rsAddrLine2=""" & pobjWS_ResultadoConsultaDirecciones.rsAddrLine2 &
                                    """ rsAddrLine3=""" & pobjWS_ResultadoConsultaDirecciones.rsAddrLine3 &
                                    """ rsCity=""" & pobjWS_ResultadoConsultaDirecciones.rsCity &
                                    """ rsState=""" & pobjWS_ResultadoConsultaDirecciones.rsState &
                                    """ rsZip=""" & pobjWS_ResultadoConsultaDirecciones.rsZip &
                                    """ rsCountryCode=""" & pobjWS_ResultadoConsultaDirecciones.rsCountryCode &
                                    """ rsPhone1=""" & pobjWS_ResultadoConsultaDirecciones.rsPhone1 &
                                    """ rsPhone1Ext=""" & pobjWS_ResultadoConsultaDirecciones.rsPhone1Ext &
                                    """ rsPhone2=""" & pobjWS_ResultadoConsultaDirecciones.rsPhone2 &
                                    """ rsPhone2Ext=""" & pobjWS_ResultadoConsultaDirecciones.rsPhone2Ext &
                                    """ rsFaxPhone=""" & pobjWS_ResultadoConsultaDirecciones.rsFaxPhone &
                                    """ rsFaxPhoneExt=""" & pobjWS_ResultadoConsultaDirecciones.rsFaxPhoneExt &
                                    """ rsStatus=""" & pobjWS_ResultadoConsultaDirecciones.rsStatus &
                                    """ rnAddrTypeId=""" & pobjWS_ResultadoConsultaDirecciones.rnAddrTypeId &
                                    """ rsAddressBasis=""" & pobjWS_ResultadoConsultaDirecciones.rsAddressBasis &
                                    """ rdtStartDt=""" & pobjWS_ResultadoConsultaDirecciones.rdtStartDt &
                                    """ rdtEndDt=""" & pobjWS_ResultadoConsultaDirecciones.rdtEndDt &
                                    """ rsName1=""" & pobjWS_ResultadoConsultaDirecciones.rsName1 &
                                    """ rsName2=""" & pobjWS_ResultadoConsultaDirecciones.rsName2 &
                                    """ rsCounty=""" & pobjWS_ResultadoConsultaDirecciones.rsCounty &
                                    """ rsAddrTypeCode=""" & pobjWS_ResultadoConsultaDirecciones.rsAddrTypeCode &
                                    """ rsCityCode=""" & pobjWS_ResultadoConsultaDirecciones.rsCityCode &
                                    """ rsCountyCode=""" & pobjWS_ResultadoConsultaDirecciones.rsCountyCode &
                                    """ rsMovilPhone=""" & pobjWS_ResultadoConsultaDirecciones.rsMovilPhone & """></Detalle>"


            xmlCompletoDirecciones = xmlCompletoDirecciones & xmlDetalleDirecciones & "</Phoenix>"

            IsBusy = True

            Dim strMsg As String = ""
            Dim objRet As InvokeOperation(Of String)

            objRet = Await dcProxy.PreClientes_Phoenix_ActualizarSync(strNroDocumento, strTipoIdentificacion, xmlCompletoClientes, xmlCompletoDirecciones, Program.Usuario, Program.HashConexion).AsTask()

            If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                If Not String.IsNullOrEmpty(strMsg) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                dcProxy.PreClientes.Clear()
                dcProxy.Load(dcProxy.ConsultaPreclientesQuery("E", strNroDocumento, strTipoIdentificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPreClientes, "FiltroInicial")
            End If
            'Else
            'SI HAY ERROR MOSTRAR MENSAJE
            'End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "GrabarPreClientesConDatosDesdePhoenix", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function ConsultarInformacion(ByVal pstrClave As String) As Task
        Try
            Dim objRet As InvokeOperation(Of String)

            If dcProxy Is Nothing Then
                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    dcProxy = New ClientesDomainContext()
                Else
                    dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                End If
            End If

            objRet = Await dcProxy.PreClientes_Phoenix_ConsultarSync(pstrClave, strNroDocumento, strTipoIdentificacion, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarInformacion", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If pstrClave = UTILIZA_SERVICIO_PHOENIX Then
                        strUtilizaWSPhoenix = objRet.Value
                    ElseIf pstrClave = RUTA_SERVICIO_PHOENIX Then
                        strRutaServicioPhoenix = objRet.Value
                    ElseIf pstrClave.Substring(0, 20) = VALIDACIONES_PHOENIX Then
                        strTipoIdentificacionHomologado = objRet.Value
                    End If
                End If
                'Else
                '    lngIDComitente = Nothing
                '    strNombreComitente = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar parámetros en la base de datos.",
                                 Me.ToString(), "ConsultarInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#Region "Propiedades"

    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _ListaPreClientes As List(Of OyDClientes.PreClientes)
    Public Property ListaPreClientes As List(Of OyDClientes.PreClientes)
        Get
            Return _ListaPreClientes
        End Get
        Set(value As List(Of OyDClientes.PreClientes))
            _ListaPreClientes = value
            If Not IsNothing(value) Then
                PreClientesSelected = _ListaPreClientes.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaPreClientes"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaPreClientesPaged"))
        End Set
    End Property
    Public ReadOnly Property ListaPreClientesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPreClientes) Then
                Dim view = New PagedCollectionView(_ListaPreClientes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _PreClientesSelected As OyDClientes.PreClientes
    Public Property PreClientesSelected As OyDClientes.PreClientes
        Get
            Return _PreClientesSelected
        End Get
        Set(value As OyDClientes.PreClientes)
            _PreClientesSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PreClientesSelected"))
        End Set
    End Property

    Private _strNroDocumento As String = String.Empty
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
        End Set
    End Property

    Private _strTipoIdentificacion As String = String.Empty
    Public Property strTipoIdentificacion() As String
        Get
            Return _strTipoIdentificacion
        End Get
        Set(ByVal value As String)
            _strTipoIdentificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoIdentificacion"))
        End Set
    End Property

#End Region

#Region "Asincronicos"
    Private Sub TerminoTraerPreClientes(ByVal lo As LoadOperation(Of OyDClientes.PreClientes))
        Try
            If Not lo.HasError Then
                ListaPreClientes = dcProxy.PreClientes.ToList
                IsBusy = False
                If ListaPreClientes.Count = 0 Then
                    'MessageBox.Show("No se encontro ningún registro")
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes",
                                             Me.ToString(), "TerminoTraerPreClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema terminar de consultar PreClientes.",
                                 Me.ToString(), "TerminoTraerPreClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoModificarprecliente(ByVal lo As InvokeOperation(Of Integer))
        If Not lo.HasError Then
            IsBusy = False
            logPrecliente = False
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar la lista de preclientes",
                                            Me.ToString(), "TerminoModificarprecliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    'Private Sub dg_MouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
    '    If e.ClickCount = 2 Then
    '        MessageBox.Show("kdfvbfdk")
    '    End If
    'End Sub

    Private Sub dg_MouseLeftButtonDown(sender As System.Object, e As System.Windows.Input.MouseButtonEventArgs)
        If ListaPreClientes.Count = 0 Or IsNothing(ListaPreClientes) Then
            Exit Sub
        End If
        contadorclic = 1 + contadorclic

        If contadorclic = 2 And mlngIDPreclientes = DirectCast(DirectCast(sender, System.Windows.Controls.DataGrid).SelectedItem, A2.OyD.OYDServer.RIA.Web.OyDClientes.PreClientes).IDPreCliente Then

            contadorclic = 0
            'mlngIDPreclientes = 0
            mlngIDPreclientes = DirectCast(DirectCast(sender, System.Windows.Controls.DataGrid).SelectedItem, A2.OyD.OYDServer.RIA.Web.OyDClientes.PreClientes).IDPreCliente
            logPrecliente = True
            Me.DialogResult = True
            ' Me.Close()
        End If

        If contadorclic = 2 And mlngIDPreclientes <> DirectCast(DirectCast(sender, System.Windows.Controls.DataGrid).SelectedItem, A2.OyD.OYDServer.RIA.Web.OyDClientes.PreClientes).IDPreCliente Then
            contadorclic = 0
            mlngIDPreclientes = 0
        End If

        mlngIDPreclientes = DirectCast(DirectCast(sender, System.Windows.Controls.DataGrid).SelectedItem, A2.OyD.OYDServer.RIA.Web.OyDClientes.PreClientes).IDPreCliente
    End Sub
    Private Async Sub btnBuscar_Click(sender As Object, e As RoutedEventArgs)
        Try

            IsBusy = True
            Dim strMsg As String = ""
            dcProxy.PreClientes.Clear()

            Await ConsultarInformacion(UTILIZA_SERVICIO_PHOENIX)

            If strUtilizaWSPhoenix = "SI" Then
                Dim objConsultaClientes As New A2OYDClientes.WSPhoenix.ConsultaClientes
                Dim objConsultaDirecciones As New A2OYDClientes.WSPhoenix.ConsultaDirecciones

                Await ConsultarInformacion(RUTA_SERVICIO_PHOENIX)

                If Not strRutaServicioPhoenix.ToLower.Contains("http") Then
                    A2Utilidades.Mensajes.mostrarMensaje(strRutaServicioPhoenix, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Return
                End If

                Program.ServicioGenerales(mobjServicio, strRutaServicioPhoenix)

                'SE CONSULTA LA HOMOLOGACION DEL TIPO DE DOCUMENTO DE PHOENIX EN OYD
                Await ConsultarInformacion(VALIDACIONES_PHOENIX)

                If strTipoIdentificacionHomologado.ToUpper.Contains(VALIDACIONES_PHOENIX) Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, strTipoIdentificacionHomologado.Substring(21, strTipoIdentificacionHomologado.Length - 21).ToString())

                    If Not String.IsNullOrEmpty(strMsg) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    IsBusy = False
                    Return
                Else
                    objConsultaClientes.NumIdentificacion = strNroDocumento
                    objConsultaClientes.TipoIdentificacion = strTipoIdentificacionHomologado
                End If

                objConsultaDirecciones.NumIdentificacion = strNroDocumento
                objConsultaDirecciones.TipoIdentificacion = strTipoIdentificacionHomologado
                objConsultaDirecciones.NumeroDireccion = 1

                'SE INVOCA EL METODO DEL WEB SERVICE PARA SOLICITAR LA INFORMACION DEL CLIENTE
                Dim objRespuestaServicioClientes = mobjServicio.consultaClientes(objConsultaClientes)
                Dim objRespuestaServicioDirecciones = mobjServicio.consultaDirecciones(objConsultaDirecciones)

                GrabarPreClientesConDatosDesdePhoenix(objRespuestaServicioClientes, objRespuestaServicioDirecciones)
            Else
                dcProxy.PreClientes.Clear()
                dcProxy.Load(dcProxy.ConsultaPreclientesQuery("E", strNroDocumento, strTipoIdentificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPreClientes, "FiltroInicial")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Buscar.",
                                 Me.ToString(), "btnBuscar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class