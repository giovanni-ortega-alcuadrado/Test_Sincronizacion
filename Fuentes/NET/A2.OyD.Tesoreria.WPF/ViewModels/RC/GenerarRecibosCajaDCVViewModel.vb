Imports Telerik.Windows.Controls
'Codigo Creado Por: Jhon bayron torres
'Archivo: GenerarRecibosCajaDCVViewModel.vb
'Generado el : 08/08/2011 
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Runtime.InteropServices.Automation
Imports System.Text
Imports System.Globalization
Imports System.Threading
Imports A2Utilidades.Mensajes

Public Class GenerarRecibosCajaDCVViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Const CSTR_NOMBREPROCESO_RECIBOSDECAJA = "RECIBOSDECAJA"
    'Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture().Parent.TextInfo.ListSeparator
    Dim Pagina As Integer = 1
    Const NroDeRegistro As Integer = 2000
    Dim NroFilas As Integer
    Dim NroRegistrosTotal As Integer
    Dim intIdidentitytesoreria As Integer

#Region "Declaraciones"
    Private dcProxy As TesoreriaDomainContext
    Private dcProxy1 As ImportacionesDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext

    'Constantes para indicar el tipo de filtro
    Private Const ALGUNOS_REGISTROS As Integer = 1
    Private Const TODOS_LOS_REGISTROS As Integer = 2
    Private Const REGISTRO_INICIAL As Integer = 3

    'Constante para indicar el Codigo de Comicol
    Private Const Comicol As Integer = 23

    Private Const LLAMADO_DESDE_ACEPTAR As String = "ACEPTAR"
    Private Const LLAMADO_DESDE_CONTRAPARTIDA As String = "CONTRAPARTIDA"

    'NIT DECEVAL
    Private Const Nitdeceval As Decimal = 8001820912
    Dim strFormaPagoDefecto_RCDCV As String = ""
    Dim strNombresucursal As String = ""


    Private Const STRVALORESNUMERICOS As String = "Debe ingresar valores Numericos"
    Private Const BANCO_NO_VALIDO As String = "Debe seleccionar un banco"
    Private Const SUCURSAL_NO_VALIDA As String = "Debe seleccionar una Sucursal"
    Private Const CONSECUTIVO_NO_VALIDA As String = "Debe seleccionar un Consecutivo de Tesoreria"
    Private Const CUENTACONTABLE_NO_VALIDA As String = "Debe seleccionar un Cuenta Contable"
    Private Const FECHARECIBO_NO_VALIDA As String = "Debe seleccionar una Fecha para el Recibo"
    Private Const RECIBI_NO_VALIDO As String = "El Campo Recibi no puede estar Vacio"
    Private Const TIPOID_NO_VALIDO As String = "Debe seleccionar el Tipo de Documento"
    Private Const NROID_NO_VALIDO As String = "Debe digitar el Nro de Documento"
    Private Const BANCOGIRADOR_NO_VALIDO As String = "El banco girador no puede estar vacio"
    Private Const CHEQUE_NO_VALIDO As String = "El número de Cheque no puede estar Vacio"
    Private Const FECHACONSIGNACION_NO_VALIDA As String = "Debe seleccionar una Fecha para la consignación del cheque "
    Private Const NRODETALLES_NO_VALIDO As String = "Debe digitar un número de detalles por Recibo de Caja."
    Private Const USUARIO_NO_VALIDO As String = "Debe seleccionar el usuario al cual quedaran asociados los Recibos"
    Private Const NO_EXISTEN_DETALLES As String = "No existen detalles para hacer Recibos"
    Private Const NO_SELECCIONADOS As String = "No existen detalles Seleccionados para hacer Recibos"
    Private Const NO_CONTRAPARTIDA As String = "No existen datos de sucursales diferentes a la escogida." & vbCrLf & "No puede hacerse la contrapartida"
    Private Const DETALLE_CONTRAPARTIDA As String = "Sucursales y Agencias "
    Private mdtmFechaFinVigencia As Date

    Private intSecuencia As Integer = 0
    Private intNroDetalles As Integer = 0
    Private TotalRecibo As Double = 0
    Private TotalDetallesGRID As Long = 0
    Private RegistrosInsertados As Long = 0
    Private ValorTotalRegistrosInsertados As Double = 0
    Private logSeleccionatodos As Boolean

    Public Event salvar()
    Public Event mostrarmensaje()
    Public Event mostrar()

#Region "Constantes"
    ''' <summary>
    ''' CE : Comprobantes de Egreso
    ''' RC : Recibos de Caja
    ''' N : Notas Contables
    ''' </summary>
    Public Enum ClasesTesoreria
        CE '// Comprobantes de Egreso
        RC '// Bancos
        N '// Notas Contables
    End Enum
#End Region
#End Region

#Region "Inicialización"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext
            dcProxy1 = New ImportacionesDomainContext
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))

        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        DirectCast(dcProxy1.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

        Try
            If Not Program.IsDesignMode() Then
                VisibleContraPartida = Visibility.Collapsed
                RegistroTesoreriaSeleccionado.NroDocumento = Nitdeceval
                IsBusy = True
                CargarListaPagosDCV(False)
                mdcProxyUtilidad01.Verificaparametro("FORMAPAGO_DEFECTO_RC_DCV", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, Nothing)

                'JCM20160217
                ValoresSelected = RegistroTesoreriaSeleccionado

                If Not IsNothing(dcProxy.ItemCombos) Then
                    mdcProxyUtilidad01.ItemCombos.Clear()
                End If

                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosEspecificosQuery("Tesoreria_RecibosCaja", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, Nothing)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "GenerarRecibosCajaDCVViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            strFormaPagoDefecto_RCDCV = obj.Value
        End If
    End Sub
    Public Sub CargarListaPagosDCV(ByVal pobjEstado As Boolean)
        dcProxy1.TblpagosDCVs.Clear()
        dcProxy1.Load(dcProxy1.ConsultaListaPagosDCVQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListaPagosDCV, pobjEstado)
    End Sub
    Public Sub SeleccionarTodosChecked(ByVal pbolCheked As System.Nullable(Of Boolean))
        Try
            If Not pbolCheked Is Nothing Then
                IsBusy = True
                logSeleccionatodos = True
                _numTotalSeleccionados = 0

                For Each Lista In _ListaRegDetalle
                    Lista.Seleccionado = pbolCheked
                    If pbolCheked Then
                        If Not Lista.Valor Is Nothing Then
                            _numTotalSeleccionados = _numTotalSeleccionados + Lista.Valor
                        End If
                    End If
                Next

                MyBase.CambioItem("TotalSeleccionados")
                MyBase.CambioItem("TotalGeneral")
                IsBusy = False
                logSeleccionatodos = False
            Else
                SeleccionarTodos = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar los registros", _
                                 Me.ToString(), "SeleccionarTodosChecked", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Aceptar()
        If Validaciones(LLAMADO_DESDE_ACEPTAR) Then
            'C1.Silverlight.C1MessageBox.Show("Desea Generar los recibos de la sucursal de " + strNombresucursal.ToString + "?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminoinactivo)
            mostrarMensajePregunta("Desea Generar los recibos de la sucursal de " + strNombresucursal.ToString + "?", _
                                   Program.TituloSistema, _
                                   "ACEPTAR", _
                                   AddressOf Terminoinactivo, False)
        End If
    End Sub
    Private Sub Terminoinactivo(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            intSecuencia = 0
            SalvarDatos()
        End If
    End Sub
    Public Sub SalvarDatos()
        intNroDetalles = 0
        Call GrabarEncabezado(intNroDetalles, False)
    End Sub
    Private Sub GrabarEncabezado(ByVal plngIdSecuencia As Integer, ByVal pobjEstado As Boolean)
        'se graba el encabezado del recibo de caja
        Try
            intSecuencia = intSecuencia + 1
            With _RegistroTesoreriaSeleccionado

                If Year(.Documento) <= 1950 Then
                    .Documento = Now
                End If

                If Year(.FecEstado) <= 1950 Then
                    .Estado = Now
                End If

                If Year(.Actualizacion) <= 1950 Then
                    .Actualizacion = Now
                End If

                If Not .ContabilidadEncuenta Is Nothing Then
                    If Year(.ContabilidadEncuenta) <= 1950 Then
                        .ContabilidadEncuenta = Now
                    End If
                End If


                If Not .ContabilidadENC Is Nothing Then
                    If Year(.ContabilidadENC) <= 1950 Then
                        .ContabilidadENC = Now
                    End If
                End If

                If Not .ContabilidadAntENC Is Nothing Then
                    If Year(.ContabilidadAntENC) <= 1950 Then
                        .ContabilidadAntENC = Now
                    End If
                End If

                If Not .Creacion Is Nothing Then
                    If Year(.Creacion) <= 1950 Then
                        .Creacion = Now
                    End If
                End If

                If Not .haProceso_Contabilidad Is Nothing Then
                    If Year(.haProceso_Contabilidad) <= 1950 Then
                        .haProceso_Contabilidad = Now
                    End If
                End If


                dcProxy.Tesoreris.Clear()
                dcProxy.GrabarReciboCajaEncabezado(plngIdSecuencia,
                                                                     .NombreConsecutivo,
                                                                    .TipoIdentificacion,
                                                                    .NroDocumento,
                                                                    .Nombre,
                                                                    .IDBanco,
                                                                    .NumCheque,
                                                                    .Valor,
                                                                    .Detalle,
                                                                    .Documento,
                                                                    "P",
                                                                    .Estado,
                                                                    .Impresiones,
                                                                    .FormaPagoCE,
                                                                    .NroLote,
                                                                    .Contabilidad,
                                                                    .Actualizacion,
                                                                    .Usuario,
                                                                    .ParametroContable,
                                                                    .ImpresionFisica,
                                                                    .MultiCliente,
                                                                    .CuentaCliente,
                                                                    .CodComitente,
                                                                    .ArchivoTransferencia,
                                                                    .IdNumInst,
                                                                    .DVP,
                                                                    .Instruccion,
                                                                    .IdNroOrden,
                                                                    .DetalleInstruccion,
                                                                    "N",
                                                                    .eroComprobante_Contabilidad,
                                                                    .hadecontabilizacion_Contabilidad,
                                                                    .haProceso_Contabilidad,
                                                                    "N",
                                                                    "N",
                                                                    .eroLote_Contabilidad,
                                                                    .MontoEscrito,
                                                                    .TipoIntermedia,
                                                                    .Concepto,
                                                                    .RecaudoDirecto,
                                                                    .ContabilidadEncuenta,
                                                                    .Sobregiro,
                                                                    .IdentificacionAutorizadoCheque,
                                                                    .NombreAutorizadoCheque,
                                                                    .ContabilidadENC,
                                                                    .NroLoteAntENC,
                                                                    .ContabilidadAntENC,
                                                                    .IdSucursalBancaria,
                                                                    .Creacion,
                                                                    _RegistroTesoreriaSeleccionado.IDDocumento,
                                                                     _ListaRegDetalle(plngIdSecuencia).Valor,
                                                                    _strCuentaContable,
                                                                    _strObservaciones,
                                                                    _strCentroCostos,
                                                                    Nothing,
                                                                    Nothing,
                                                                    Nothing,
                                                                    Nothing,
                                                                    Nothing,
                                                                   Nothing,.IDComisionista, Program.Usuario, Program.HashConexion, AddressOf TerminaGrabarEncabezado, pobjEstado)


                'ByVal plngIDDocumento As Integer, _
                '                                ByVal pcurValorDetalle As System.Nullable(Of Decimal), _
                '                                ByVal pstrIDCuentaContable As String, _
                '                                ByVal pstrDetalleObs As String, _
                '                                ByVal pstrCentroCosto As String, _
                '                                ByVal pstrISIN As String, _
                '                                ByVal plngFungible As System.Nullable(Of Integer), _
                '                                ByVal plngCuentaInversionista As System.Nullable(Of Integer)

                'TotalRecibo = TotalRecibo + _ListaRegDetalle(plngIdSecuencia).VlrAPagar
                'ValorTotalRegistrosInsertados = ValorTotalRegistrosInsertados + _ListaRegDetalle(plngIdSecuencia).VlrAPagar

            End With
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar los encabezados", _
                                 Me.ToString(), "GrabarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub NombreSucursal_Click(ByVal strsucursal As String)
        'se recibe el nombre de la sucursal seleccionada y se llama el metodo que filtra dependiendo de la sucursal
        Try
            If Not String.IsNullOrEmpty(strsucursal) Then

                If _ListaRegDetalle.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No hay detalles", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                strNombresucursal = strsucursal
                Call OpcionSeleccion(strsucursal)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los registros", _
                                 Me.ToString(), "NombreSucursal_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub OpcionSeleccion(ByVal strsucursal As String)
        'marca los registros dependiendo la sucursal que tenga marcada
        Try
            Dim intNroSecuencia As Long = 0
            _numTotalSeleccionados = 0
            _numTotalGeneral = 0
            Dim intTotalReg As Integer = _ListaRegDetalle.Count
            Dim intTotalSeleccionados As Integer = 0


            For Each Lista In _ListaRegDetalle

                If Lista.Sucursal = strsucursal Or _SeleccionarTodos Then
                    Lista.Seleccionado = True

                    intTotalSeleccionados = intTotalSeleccionados + 1

                    If Not Lista.Valor Is Nothing Then
                        _numTotalSeleccionados = _numTotalSeleccionados + Lista.Valor
                    End If
                End If

                _numTotalGeneral = _numTotalGeneral + Lista.Valor
            Next

            If intTotalSeleccionados = intTotalReg Then
                _SeleccionarTodos = True
            End If

            MyBase.CambioItem("SeleccionarTodos")
            MyBase.CambioItem("ListaDetallePaged")
            MyBase.CambioItem("TotalSeleccionados")
            MyBase.CambioItem("TotalGeneral")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los registros", _
                                 Me.ToString(), "OpcionSeleccion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub _SelectedDetalle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _SelectedDetalle.PropertyChanged
        If e.PropertyName.Equals("Seleccionado") And logSeleccionatodos = False Then
            If Not SelectedDetalle.Valor Is Nothing Then
                If SelectedDetalle.Seleccionado Then
                    _numTotalSeleccionados = _numTotalSeleccionados + SelectedDetalle.Valor
                Else
                    _numTotalSeleccionados = _numTotalSeleccionados - SelectedDetalle.Valor
                End If
            End If
            MyBase.CambioItem("TotalSeleccionados")
            MyBase.CambioItem("TotalGeneral")
        End If
    End Sub
#End Region

#Region "Asincronos"
    Private Sub TerminaGrabarEncabezado(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                Dim secuenciafalse = lo.UserState
                Dim intUltimoegistro As Integer = 0
                Dim retorno = lo.Value.Split(",")
                _RegistroTesoreriaSeleccionado.IDDocumento = retorno(0)
                intIdidentitytesoreria = retorno(1)
                _numTotalGeneral = 0
                intNroDetalles = 0
                RegistrosInsertados = 0
                'ValorTotalRegistrosInsertados = 0
                TotalRecibo = 0
                'TotalDetallesGRID = ListaDetallePaged.Count - 0
                'TotalSeleccionados = Aggregate li In _ListaRegDetalle Where li.Seleccionado = True Into Count(li.Seleccionado)
                Dim bolUltimoRegistro As Boolean = False
                Dim objListaFiltrada As New ObservableCollection(Of TblpagosDCV)
                For Each li In _ListaRegDetalle.Where(Function(x) x.Seleccionado).Take(NroDetalles)
                    objListaFiltrada.Add(li)
                Next
                intUltimoegistro = objListaFiltrada.Count
                For Each li In objListaFiltrada
                    intNroDetalles = intNroDetalles + 1
                    If intNroDetalles = intUltimoegistro Then
                        bolUltimoRegistro = True
                    End If
                    Call GrabarDetalle(intNroDetalles, bolUltimoRegistro, intIdidentitytesoreria, _RegistroTesoreriaSeleccionado.IDDocumento, li)
                    _ListaRegDetalle.Remove(_ListaRegDetalle.Where(Function(x) x.IdNroRegistro = li.IdNroRegistro).FirstOrDefault)
                Next

                Dim listatermina = (From sl In _ListaRegDetalle Where sl.Seleccionado = True Select sl).Take(NroDetalles)
                If listatermina.Count > 0 Then
                    SalvarDatos()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Número de Recibos de caja insertados : " & CStr(intSecuencia) & vbCrLf & "Valor de : $ " & CStr(Format(ValorTotalRegistrosInsertados, "###,###,###,###,##0.0")), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    ValorTotalRegistrosInsertados = 0
                    TotalRecibo = 0
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(lo.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el encabezado", _
                                 Me.ToString(), "TerminaGrabarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoTrasladarTesoreria(ByVal lo As InvokeOperation(Of Integer))
        Try
            IsBusy = False
            If lo.HasError Then

                dcProxy.BorrarTesoreria_TMP_TBL(intIdidentitytesoreria, _RegistroTesoreriaSeleccionado.Usuario, Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarTMP, "borrar")

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoTrasladarTesoreria" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al trasladar", _
                                 Me.ToString(), "TerminoTrasladarTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        '
    End Sub
    Private Sub TerminoEliminarTMP(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoEliminarTMP" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If '
    End Sub
    Private Sub GrabarDetalle(ByVal pintSecuencia As Integer, ByVal pobjEstado As Boolean, ByVal intIdidentity As Integer, ByVal iddocumento As Integer, ByVal itemseleccionado As Object)
        Try
            'With _ListaRegDetalle(pintSecuencia)
            TotalRecibo = TotalRecibo + itemseleccionado.Valor
            ValorTotalRegistrosInsertados = ValorTotalRegistrosInsertados + itemseleccionado.Valor
            dcProxy.GrabarReciboCajaDetalle(_RegistroTesoreriaSeleccionado.NombreConsecutivo, _
                                             iddocumento, _
                                             pintSecuencia, _
              itemseleccionado.CodigoCliente, _
            itemseleccionado.Valor, _
                                             _strCuentaContable, _
                                             itemseleccionado.Detalle, _
                                             _RegistroTesoreriaSeleccionado.IDBanco, _
                                             _RegistroTesoreriaSeleccionado.NroDocumento, _
                                             _strCentroCostos, _
                                             Now.Date, _
                                             _RegistroTesoreriaSeleccionado.Usuario, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, _
                                             Nothing, "DCV", itemseleccionado.IdNroRegistro, intIdidentitytesoreria, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarDetalles, pobjEstado.ToString + "," + intIdidentity.ToString + "," + iddocumento.ToString + "," + TotalRecibo.ToString)


            'End With
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al grabar los detalles", _
                                 Me.ToString(), "GrabarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoGrabarDetalles(ByVal lo As InvokeOperation(Of Integer))

        Try
            If Not lo.HasError Then
                Dim retorno = lo.UserState.Split(",")
                If retorno(0) = True Then
                    dcProxy.GrabarChequePagoDeceval(_RegistroTesoreriaSeleccionado.NombreConsecutivo, retorno(2), 1, False, _strIdBancoGirador, _RegistroTesoreriaSeleccionado.NumCheque, CDbl(retorno(3)), _RegistroTesoreriaSeleccionado.IDBanco, _FechaConsignar, _RegistroTesoreriaSeleccionado.Usuario, _strObservaciones, strFormaPagoDefecto_RCDCV, retorno(1), Program.Usuario, Program.HashConexion, AddressOf TerminoGuardarCheque, retorno(1))
                End If


            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al insertar los detalles", _
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó  al insertar los detalles", _
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Private Sub TerminoTraerListaPagosDCV(ByVal lo As LoadOperation(Of TblpagosDCV))
        Try
            'carga el grid con los pagos realizados en dcv
            If Not lo.HasError Then
                'If lo.UserState Then
                '    _ListaRegDetalle.Clear()
                '    NroRegistrosTotal = 0
                '    A2Utilidades.Mensajes.mostrarMensaje("Numero de Recibos de caja insertados : " & CStr(intSecuencia) & vbCrLf & "Valor de : $ " & CStr(Format(ValorTotalRegistrosInsertados, "###,###,###,###,##0.0")), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                '    ValorTotalRegistrosInsertados = 0
                '    TotalRecibo = 0
                'End If
                '_ListaRegDetalle.Clear()
                ListaDetalle.Clear()
                'ListaDetalle = dcProxy1.TblpagosDCVs
                For Each li In dcProxy1.TblpagosDCVs
                    ListaDetalle.Add(li)
                Next
                For Each Lista In _ListaDetalle
                    _ListaRegDetalle.Add(Lista)
                    _numTotalGeneral = _numTotalGeneral + Lista.Valor
                    MyBase.CambioItem("TotalGeneral")
                Next
                'NroRegistrosTotal = NroRegistrosTotal + dcProxy.ListaDetalleTesorerias.Count
                'For Each i In ListaDetalle
                '    NroFilas = i.NroFilas
                '    NroRegistrosTotal = NroRegistrosTotal + i.RegistrosporPaginaTotal
                '    Exit For
                'Next

                MyBase.CambioItem("ListaDetalle")
                MyBase.CambioItem("ListaDetallePaged")
                MyBase.CambioItem("RegistroTesoreriaSeleccionado")

            Else

                A2Utilidades.Mensajes.mostrarMensaje(lo.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            End If

            If NroFilas > NroDeRegistro Then
                Pagina = Pagina + 1
                If NroFilas = NroRegistrosTotal Then
                    Pagina = 1
                    IsBusy = False
                    Exit Sub
                ElseIf NroRegistrosTotal < NroFilas Then
                    CargarListaPagosDCV(False)
                End If


            End If

            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje(ex.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End Try
    End Sub
    Private Sub TerminoTraerrecibos(ByVal lo As InvokeOperation(Of System.Nullable(Of Boolean)))
        Try
            If Not lo.HasError Then
                TerminoCrearArchivo()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos", _
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos", _
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub TerminoGuardarCheque(ByVal lo As InvokeOperation(Of Integer))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el cheque",
                                 Me.ToString(), "TerminoGuardarCheque", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            Else
                ''' JFSB 20180102 Se agrega el parámetro de la compañía
                dcProxy.TrasladarTesoreria_TMP_TBL(lo.UserState, _RegistroTesoreriaSeleccionado.Usuario, Nothing, Program.Usuario, Program.HashConexion, AddressOf TerminoTrasladarTesoreria, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos", _
                                 Me.ToString(), "TerminoGuardarCheque", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New A2ComunesImportaciones.ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_RECIBOSDECAJA)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos", _
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    'JCM20160217
    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If mdcProxyUtilidad01.ItemCombos.Count > 0 Then
                    listConsecutivos = mdcProxyUtilidad01.ItemCombos.Where(Function(y) y.Categoria = "ConsecutivosTesoreriaRC").ToList
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.", _
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.", _
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"

    'JCM20160217
    Private WithEvents _Valoreselected As Tesoreri
    Public Property ValoresSelected() As Tesoreri
        Get
            Return _Valoreselected
        End Get
        Set(value As Tesoreri)
            _Valoreselected = value
            MyBase.CambioItem("ValoresSelected")
        End Set
    End Property


    Private _RegistroTesoreriaSeleccionado As New Tesoreri
    Public Property RegistroTesoreriaSeleccionado() As Tesoreri
        Get

            If _RegistroTesoreriaSeleccionado.TipoIdentificacion Is Nothing Then
                _RegistroTesoreriaSeleccionado.TipoIdentificacion = "N"
            End If

            If IsDate(_RegistroTesoreriaSeleccionado.Creacion) Then
                _RegistroTesoreriaSeleccionado.Creacion = Now.Date
            End If

            If _RegistroTesoreriaSeleccionado.Nombre Is Nothing Or String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.Nombre) Then
                _RegistroTesoreriaSeleccionado.Nombre = "Deceval S.A."
            End If

            Return _RegistroTesoreriaSeleccionado
        End Get
        Set(ByVal value As Tesoreri)
            _RegistroTesoreriaSeleccionado = value
            MyBase.CambioItem("RegistroTesoreriaSeleccionado")
        End Set
    End Property

    Private _strCuentaContable As String
    Public Property CuentaContable() As String
        Get
            Return _strCuentaContable
        End Get
        Set(ByVal value As String)
            _strCuentaContable = value
            MyBase.CambioItem("CuentaContable")
        End Set
    End Property

    Private _strNombreBanco As String
    Public Property NombreBanco() As String
        Get
            Return _strNombreBanco
        End Get
        Set(ByVal value As String)
            _strNombreBanco = value
            MyBase.CambioItem("NombreBanco")
        End Set
    End Property

    Private _strIdBancoGirador As String
    Public Property IDBancoGirador() As String
        Get
            Return _strIdBancoGirador
        End Get
        Set(ByVal value As String)
            _strIdBancoGirador = value
            MyBase.CambioItem("IDBancoGirador")
        End Set
    End Property

    Private _strNombreBancoGirador As String
    Public Property NombreBancoGirador() As String
        Get
            Return _strNombreBancoGirador
        End Get
        Set(ByVal value As String)
            _strNombreBancoGirador = value
            MyBase.CambioItem("NombreBancoGirador")
        End Set
    End Property

    Private _strObservaciones As String
    Public Property Observaciones() As String
        Get
            Return _strObservaciones
        End Get
        Set(ByVal value As String)
            _strObservaciones = value
            MyBase.CambioItem("Observaciones")
        End Set
    End Property

    Private _strCentroCostos As String
    Public Property CentroCostos() As String
        Get
            Return _strCentroCostos
        End Get
        Set(ByVal value As String)
            _strCentroCostos = value
            MyBase.CambioItem("CentroCostos")
        End Set
    End Property

    Private _ListaDetalle As New ObservableCollection(Of TblpagosDCV)
    Public Property ListaDetalle() As ObservableCollection(Of TblpagosDCV)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As ObservableCollection(Of TblpagosDCV))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
        End Set
    End Property

    Private WithEvents _SelectedDetalle As TblpagosDCV
    Public Property SelectedDetalle() As TblpagosDCV
        Get
            Return _SelectedDetalle
        End Get
        Set(ByVal value As TblpagosDCV)
            _SelectedDetalle = value
            MyBase.CambioItem("SelectedDetalle")
        End Set
    End Property

    Private _ListaRegDetalle As New ObservableCollection(Of TblpagosDCV)
    Public Property ListaRegDetalle() As ObservableCollection(Of TblpagosDCV)
        Get
            Return _ListaRegDetalle
        End Get
        Set(ByVal value As ObservableCollection(Of TblpagosDCV))
            _ListaRegDetalle = value
            MyBase.CambioItem("ListaRegDetalle")
        End Set
    End Property


    Public ReadOnly Property ListaDetallePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaRegDetalle) Then
                Dim view = New PagedCollectionView(_ListaRegDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _FechaConsignar As System.Nullable(Of Date)
    Public Property FechaConsignar() As System.Nullable(Of Date)
        Get
            Return _FechaConsignar
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _FechaConsignar = value
            MyBase.CambioItem("FechaConsignar")
        End Set
    End Property

    Private _SeleccionarTodos As System.Nullable(Of Boolean)
    Public Property SeleccionarTodos() As System.Nullable(Of Boolean)
        Get
            If _SeleccionarTodos Is Nothing Then
                _SeleccionarTodos = False
            End If
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _SeleccionarTodos = value
            MyBase.CambioItem("SeleccionarTodos")
            If Not IsNothing(_SeleccionarTodos) Then
                SeleccionarTodosChecked(_SeleccionarTodos)
            End If
        End Set
    End Property

    Private _numTotalSeleccionados As Decimal
    Public Property TotalSeleccionados() As Decimal
        Get
            Return _numTotalSeleccionados
        End Get
        Set(ByVal value As Decimal)
            _numTotalSeleccionados = value
            MyBase.CambioItem("TotalSeleccionados")
        End Set
    End Property

    Private _numTotalGeneral As Decimal
    Public Property TotalGeneral() As Decimal
        Get
            Return _numTotalGeneral
        End Get
        Set(ByVal value As Decimal)
            _numTotalGeneral = value
            MyBase.CambioItem("TotalGeneral")
        End Set
    End Property

    Private _intSucursal As Integer
    Public Property Sucursal() As Integer
        Get
            Return _intSucursal
        End Get
        Set(ByVal value As Integer)
            _intSucursal = value
            MyBase.CambioItem("Sucursal")
        End Set
    End Property

    Private _bolVisibleContraPartida As Visibility
    Public Property VisibleContraPartida() As Visibility
        Get
            Return _bolVisibleContraPartida
        End Get
        Set(ByVal value As Visibility)
            _bolVisibleContraPartida = value
            MyBase.CambioItem("VisibleContraPartida")
        End Set
    End Property

    Private _intNroDetalles As System.Nullable(Of Integer)
    Public Property NroDetalles() As System.Nullable(Of Integer)
        Get
            If _intNroDetalles Is Nothing Then
                _intNroDetalles = 100
            End If

            Return _intNroDetalles
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intNroDetalles = value
            MyBase.CambioItem("NroDetalles")
        End Set
    End Property


    'JCM20160217
    'Propiedad para cargar los consecutivos de los documentos
    Private _listConsecutivos As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivos = value
            MyBase.CambioItem("listConsecutivos")
        End Set
    End Property

    ' Compañía asociada al consecutivo seleccionado
    Private _strCompaniaConsecutivo As String = String.Empty
    Public Property strCompaniaConsecutivo() As String
        Get
            Return _strCompaniaConsecutivo
        End Get
        Set(ByVal value As String)
            _strCompaniaConsecutivo = value
            MyBase.CambioItem("strCompaniaConsecutivo")
        End Set
    End Property

    Public Property strCompaniaBanco As String = String.Empty



#End Region

    Private Sub _ValoresSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _Valoreselected.PropertyChanged
        If e.PropertyName = "NombreConsecutivo" Then
            If Not IsNothing(_Valoreselected.NombreConsecutivo) Then
                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _Valoreselected.NombreConsecutivo Select c).FirstOrDefault.Retorno
            Else
                strCompaniaConsecutivo = String.Empty
            End If
            If strCompaniaBanco <> strCompaniaConsecutivo Then
                LimpiarBanco()
            End If
        End If
    End Sub


#Region "Funciones y Mtodos Generales"
    Private Function Validaciones(ByVal LlamadaDesde As String) As Boolean
        '/*****************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function Validaciones  As Boolean
        '/* Alcance     : Private
        '/* Descripción : Valida el ingreso de los campos que son obligatorios en la forma
        '/* Parámetros  :
        '/* Por             Nombre      Tipo        Descripción
        '/* LlamadaDesde    Permite Saber el evento desde el cual se llamo
        '/* Valores de retorno:
        '/* devuelve  TRUE si todos los datos fueron correctos
        '/*                  FALSE si falto algun campo obligatorio
        '/* FIN DOCUMENTO
        '/******************************************************************************************


        Dim seleccionados As Boolean
        Dim strMsg As String = String.Empty
        Dim bolPresentaInconsistencia As Boolean = False

        Try

            Validaciones = False

            'Consecutivo
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.NombreConsecutivo) Then
                strMsg = strMsg & "* " & CONSECUTIVO_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Sucursal
            If String.IsNullOrEmpty(Sucursal) Then
                strMsg = strMsg & "* " & SUCURSAL_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            ElseIf Sucursal = 0 Then
                strMsg = strMsg & "* " & SUCURSAL_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Cuenta Contable
            If String.IsNullOrEmpty(_strCuentaContable) Then
                strMsg = strMsg & "* " & CUENTACONTABLE_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Fecha de Corte
            If Not IsDate(_RegistroTesoreriaSeleccionado.Creacion) Then
                strMsg = strMsg & "* " & FECHARECIBO_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Se recibe de.... Por defecto Deceval S.A.
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.Nombre) Then
                strMsg = strMsg & "* " & RECIBI_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Tipo de Identificacion
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.TipoIdentificacion) Then
                strMsg = strMsg & "* " & TIPOID_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Valida Nro de Documento que no llegue vacio.
            If _RegistroTesoreriaSeleccionado.NroDocumento Is Nothing Then
                strMsg = strMsg & "* " & NROID_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Codigo del Banco
            If _RegistroTesoreriaSeleccionado.IDBanco Is Nothing Then
                strMsg = strMsg & "* " & BANCO_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            ''Codigo banco girador
            'If String.IsNullOrEmpty(_strIdBancoGirador) Then
            '    strMsg = strMsg & "* " & BANCOGIRADOR_NO_VALIDO & vbCrLf
            '    bolPresentaInconsistencia = True
            'End If

            ''Valida Cheque
            'If IsNothing(_RegistroTesoreriaSeleccionado.NumCheque) Then
            '    strMsg = strMsg & "* " & CHEQUE_NO_VALIDO & vbCrLf
            '    bolPresentaInconsistencia = True
            'End If

            'Fecha a consignar
            If Not IsDate(_FechaConsignar) Then
                strMsg = strMsg & "* " & FECHACONSIGNACION_NO_VALIDA & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Nro de Detalles
            If NroDetalles <= 0 Then
                strMsg = strMsg & "* " & NRODETALLES_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If

            'Valida que se haya seleccionado un usuario
            If String.IsNullOrEmpty(_RegistroTesoreriaSeleccionado.Usuario) Then
                strMsg = strMsg & "* " & USUARIO_NO_VALIDO & vbCrLf
                bolPresentaInconsistencia = True
            End If


            If (_strObservaciones & "") = "" Then
                _strObservaciones = ""
            End If


            With _ListaRegDetalle

                If .Count = 0 Then
                    strMsg = strMsg & "* " & NRODETALLES_NO_VALIDO & vbCrLf
                    bolPresentaInconsistencia = True
                End If

                seleccionados = False

                For Each registro In _ListaRegDetalle

                    If SeleccionarTodos() Then registro.Seleccionado = True

                    If registro.Seleccionado Then
                        seleccionados = True
                    End If

                    'CFMA20160225 Validar primero si el registro está seleccionado
                    If registro.Seleccionado Then
                        If registro.Valor > 0 Then
                            If String.IsNullOrEmpty(registro.Detalle) Then
                                strMsg = strMsg & "* No existe detalle" & vbCrLf
                                bolPresentaInconsistencia = True
                                Exit For
                            End If
                        Else
                            strMsg = strMsg & "* No existe Valor" & vbCrLf
                            bolPresentaInconsistencia = True
                            Exit For
                        End If
                    End If

                Next

            End With


            If Not seleccionados And LlamadaDesde = LLAMADO_DESDE_ACEPTAR Then
                strMsg = strMsg & "* " & NO_SELECCIONADOS & vbCrLf
                bolPresentaInconsistencia = True
            End If

            If bolPresentaInconsistencia Then
                A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            Return Not bolPresentaInconsistencia

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de campos", _
                                 Me.ToString(), "GenerarRecibosCajaDCVViewModel.Validaciones", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try

    End Function
    Public Sub EjecutarConsulta()
        Try
            IsBusy = True
            Dim grid As New DataGrid
            grid.AutoGenerateColumns = False
            grid.HeadersVisibility = DataGridHeadersVisibility.All
            ' ListaTitulos = dcProxy.ReporteExcelTitulos
            CrearColumnasTitulos(grid)
            Dim strMensaje As String = "csv"
            exportDataGrid(grid, strMensaje.ToUpper())
        Catch ex As Exception
            'A2Utilidades.Mensajes.mostrarMensaje(ex.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)_
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al ejecutar la consulta.", _
                                 Me.ToString(), "ReporteRecibosdeCajaViewModel.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub CrearColumnasTitulos(ByVal pgrid As DataGrid)
        '<--
        Dim NombreCliente As New DataGridTextColumn
        NombreCliente.Header = "NombreCliente"
        pgrid.Columns.Add(NombreCliente)

        Dim CodigoCliente As New DataGridTextColumn
        CodigoCliente.Header = "CodigoCliente"
        pgrid.Columns.Add(CodigoCliente)

        Dim Valor As New DataGridTextColumn
        Valor.Header = "Valor"
        pgrid.Columns.Add(Valor)

        Dim NombreTituloPagado As New DataGridTextColumn
        NombreTituloPagado.Header = "NombreTituloPagado"
        pgrid.Columns.Add(NombreTituloPagado)

        Dim Detalle As New DataGridTextColumn
        Detalle.Header = "Detalle"
        pgrid.Columns.Add(Detalle)

        Dim DocumentoCliente As New DataGridTextColumn
        DocumentoCliente.Header = "DocumentoCliente"
        pgrid.Columns.Add(DocumentoCliente)

        Dim FechaPago As New DataGridTextColumn
        FechaPago.Header = "FechaPago"
        pgrid.Columns.Add(FechaPago)

        Dim CiudadTitulo As New DataGridTextColumn
        CiudadTitulo.Header = "CiudadTitulo"
        pgrid.Columns.Add(CiudadTitulo)

        Dim NroContratoTercero As New DataGridTextColumn
        NroContratoTercero.Header = "NroContratoTercero"
        pgrid.Columns.Add(NroContratoTercero)

        Dim NumeroExpedicion As New DataGridTextColumn
        NumeroExpedicion.Header = "NumeroExpedicion"
        pgrid.Columns.Add(NumeroExpedicion)

        Dim EntidadBeneficiariaPago As New DataGridTextColumn
        EntidadBeneficiariaPago.Header = "EntidadBeneficiariaPago"
        pgrid.Columns.Add(EntidadBeneficiariaPago)

        Dim TasaEfectivaAnual As New DataGridTextColumn
        TasaEfectivaAnual.Header = "TasaEfectivaAnual"
        pgrid.Columns.Add(TasaEfectivaAnual)

        Dim ValorCosto As New DataGridTextColumn
        ValorCosto.Header = "ValorCosto"
        pgrid.Columns.Add(ValorCosto)

        Dim ValorCapitalizado As New DataGridTextColumn
        ValorCapitalizado.Header = "ValorCapitalizado"
        pgrid.Columns.Add(ValorCapitalizado)

        Dim ValorCapital As New DataGridTextColumn
        ValorCapital.Header = "ValorCapital"
        pgrid.Columns.Add(ValorCapital)

        Dim ValorIntereses As New DataGridTextColumn
        ValorIntereses.Header = "ValorIntereses"
        pgrid.Columns.Add(ValorIntereses)

        Dim ValorReinvertido As New DataGridTextColumn
        ValorReinvertido.Header = "ValorReinvertido"
        pgrid.Columns.Add(ValorReinvertido)

        Dim ValorAmortizado As New DataGridTextColumn
        ValorAmortizado.Header = "ValorAmortizado"
        pgrid.Columns.Add(ValorAmortizado)

        Dim ValorDescuento As New DataGridTextColumn
        ValorDescuento.Header = "ValorDescuento"
        pgrid.Columns.Add(ValorDescuento)

        Dim ValorDiferenciaEnCambio As New DataGridTextColumn
        ValorDiferenciaEnCambio.Header = "ValorDiferenciaEnCambio"
        pgrid.Columns.Add(ValorDiferenciaEnCambio)

        Dim ValorReteFuenteIntereses As New DataGridTextColumn
        ValorReteFuenteIntereses.Header = "ValorReteFuenteIntereses"
        pgrid.Columns.Add(ValorReteFuenteIntereses)

        Dim ValorReteFuenteDiferenciaEnCambio As New DataGridTextColumn
        ValorReteFuenteDiferenciaEnCambio.Header = "ValorReteFuenteDiferenciaEnCambio"
        pgrid.Columns.Add(ValorReteFuenteDiferenciaEnCambio)
        '-->
        Dim ValorReteFuenteCapital As New DataGridTextColumn
        ValorReteFuenteCapital.Header = "ValorReteFuenteCapital"
        pgrid.Columns.Add(ValorReteFuenteCapital)

        Dim ValorPagoOtraMoneda As New DataGridTextColumn
        ValorPagoOtraMoneda.Header = "ValorPagoOtraMoneda"
        pgrid.Columns.Add(ValorPagoOtraMoneda)

        Dim NroOperacionPesos As New DataGridTextColumn
        NroOperacionPesos.Header = "NroOperacionPesos"
        pgrid.Columns.Add(NroOperacionPesos)

        Dim NroOperacionOtraMoneda As New DataGridTextColumn
        NroOperacionOtraMoneda.Header = "NroOperacionOtraMoneda"
        pgrid.Columns.Add(NroOperacionOtraMoneda)

        Dim NroOperacionCapitalizacion As New DataGridTextColumn
        NroOperacionCapitalizacion.Header = "NroOperacionCapitalizacion"
        pgrid.Columns.Add(NroOperacionCapitalizacion)

        Dim NroOperacionReinversion As New DataGridTextColumn
        NroOperacionReinversion.Header = "NroOperacionReinversion"
        pgrid.Columns.Add(NroOperacionReinversion)

        Dim Receptor As New DataGridTextColumn
        Receptor.Header = "Receptor"
        pgrid.Columns.Add(Receptor)

        Dim Sucursal As New DataGridTextColumn
        Sucursal.Header = "Sucursal"
        pgrid.Columns.Add(Sucursal)

        Dim CodigoIntermediario As New DataGridTextColumn
        CodigoIntermediario.Header = "CodigoIntermediario"
        pgrid.Columns.Add(CodigoIntermediario)

        Dim EntidadIntermediario As New DataGridTextColumn
        EntidadIntermediario.Header = "EntidadIntermediario"
        pgrid.Columns.Add(EntidadIntermediario)

        Dim IdNroRegistro As New DataGridTextColumn
        IdNroRegistro.Header = "IdNroRegistro"
        pgrid.Columns.Add(IdNroRegistro)
    End Sub
    Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String)
        Try
            'construye el archivo en formato csv
            Dim strBuilder As New StringBuilder()
            Dim strLineas As New List(Of String)

            If IsNothing(dGrid) Then Return
            Dim lstFields As List(Of String) = New List(Of String)()
            If dGrid.HeadersVisibility = DataGridHeadersVisibility.Column OrElse dGrid.HeadersVisibility = DataGridHeadersVisibility.All Then
                For Each dgcol As DataGridColumn In dGrid.Columns
                    lstFields.Add(formatField(dgcol.Header.ToString(), strFormat, True))
                Next
                buildStringOfRow(strBuilder, lstFields, strFormat)
                strLineas.Add(strBuilder.ToString())
            End If
            dGrid.ItemsSource = _ListaRegDetalle

            For a = 0 To _ListaRegDetalle.Count - 1
                lstFields.Clear()
                strBuilder.Clear()
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NombreCliente), String.Empty, _ListaRegDetalle(a).NombreCliente)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).CodigoCliente), String.Empty, _ListaRegDetalle(a).CodigoCliente)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Valor), String.Empty, _ListaRegDetalle(a).Valor)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NombreTituloPagado), String.Empty, _ListaRegDetalle(a).NombreTituloPagado)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Detalle), String.Empty, _ListaRegDetalle(a).Detalle)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).DocumentoCliente), String.Empty, _ListaRegDetalle(a).DocumentoCliente)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).FechaPago), String.Empty, _ListaRegDetalle(a).FechaPago)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).CiudadTitulo), String.Empty, _ListaRegDetalle(a).CiudadTitulo)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NroContratoTercero), String.Empty, _ListaRegDetalle(a).NroContratoTercero)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NumeroExpedicion), String.Empty, _ListaRegDetalle(a).NumeroExpedicion)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).EntidadBeneficiariaPago), String.Empty, _ListaRegDetalle(a).EntidadBeneficiariaPago)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).TasaEfectivaAnual), String.Empty, _ListaRegDetalle(a).TasaEfectivaAnual)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorCosto), String.Empty, _ListaRegDetalle(a).ValorCosto)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorCapitalizado), String.Empty, _ListaRegDetalle(a).ValorCapitalizado)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorCapital), String.Empty, _ListaRegDetalle(a).ValorCapital)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorIntereses), String.Empty, _ListaRegDetalle(a).ValorIntereses)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorReinvertido), String.Empty, _ListaRegDetalle(a).ValorReinvertido)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorAmortizado), String.Empty, _ListaRegDetalle(a).ValorAmortizado)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorDescuento), String.Empty, _ListaRegDetalle(a).ValorDescuento)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorDiferenciaEnCambio), String.Empty, _ListaRegDetalle(a).ValorDiferenciaEnCambio)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorReteFuenteIntereses), String.Empty, _ListaRegDetalle(a).ValorReteFuenteIntereses)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorReteFuenteDiferenciaEnCambio), String.Empty, _ListaRegDetalle(a).ValorReteFuenteDiferenciaEnCambio)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorReteFuenteCapital), String.Empty, _ListaRegDetalle(a).ValorReteFuenteCapital)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).ValorPagoOtraMoneda), String.Empty, _ListaRegDetalle(a).ValorPagoOtraMoneda)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NroOperacionPesos), String.Empty, _ListaRegDetalle(a).NroOperacionPesos)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NroOperacionOtraMoneda), String.Empty, _ListaRegDetalle(a).NroOperacionOtraMoneda)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NroOperacionCapitalizacion), String.Empty, _ListaRegDetalle(a).NroOperacionCapitalizacion)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).NroOperacionReinversion), String.Empty, _ListaRegDetalle(a).NroOperacionReinversion)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Receptor), String.Empty, _ListaRegDetalle(a).Receptor)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).Sucursal), String.Empty, _ListaRegDetalle(a).Sucursal)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).CodigoIntermediario), String.Empty, _ListaRegDetalle(a).CodigoIntermediario)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).EntidadIntermediario), String.Empty, _ListaRegDetalle(a).EntidadIntermediario)), strFormat, False))
                lstFields.Add(formatField(CStr(IIf(IsNothing(_ListaRegDetalle(a).IdNroRegistro), String.Empty, _ListaRegDetalle(a).IdNroRegistro)), strFormat, False))

                buildStringOfRow(strBuilder, lstFields, strFormat)
                strLineas.Add(strBuilder.ToString())
            Next

            dcProxy1.Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_RECIBOSDECAJA, Program.Usuario, String.Format("ReporteTitulosActivos{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas, Program.HashConexion, AddressOf TerminoCrearArchivo, True)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al generar el archivo csv", _
                                 Me.ToString(), "exportDataGrid", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
        'formatea las celdas dependiendo del formato que se nesesite
        Select Case format
            Case "XML"
                Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
            Case "CSV"
                If encabezado = True Then
                    Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                Else
                    Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                End If
        End Select
        Return data

    End Function
    Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
        Thread.CurrentThread.CurrentCulture = New CultureInfo("en-US")
        Select Case strFormat
            Case "XML"
                strBuilder.AppendLine("<Row>")
                strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
                strBuilder.AppendLine("</Row>")
                ' break;
            Case "CSV"
                strBuilder.AppendLine(String.Join(CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator, lstFields.ToArray()))
                ' break;

        End Select

    End Sub
    


    'JCM20160217
    Public Sub LimpiarBanco()

        RegistroTesoreriaSeleccionado.IDBanco = Nothing
        NombreBanco = Nothing
        strCompaniaBanco = String.Empty
    End Sub



#End Region
End Class
