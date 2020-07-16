Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConceptosTesoreriaViewModel.vb
'Generado el : 02/15/2011 13:33:49
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
Imports System.Threading.Tasks


Public Class ConceptosTesoreriaViewModel

    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaConceptosTesoreri
    Private ConceptosTesoreriPorDefecto As ConceptosTesoreri
    Private ConceptosTesoreriAnterior As ConceptosTesoreri
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim sw As Integer
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Public CambioNroDoc As Boolean = False
    Dim logUtilizaPasiva As Boolean = False
    Dim strCuentasContable_Balance As String = String.Empty
    Dim strCuentasContable_PYG As String = String.Empty

    Dim ListaCuentasBalance As List(Of String) = Nothing
    Dim ListaCuentasPYG As List(Of String) = Nothing
    Dim logCuentaContable, logCuentaContableAux, logCuentaContableCRDiferido, logCuentaContableDBDiferido As Boolean

    Private Enum TipoMvtoTesoreria
        BABA    'BALANCE X BALANCE
        BAP     'BALANCE X PYG
        BB      'BANCO X BANCO
        BBA     'BANCO X BALANCE
        BP      'BANCO X PYG
        PP      'PYG X PYG
    End Enum

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            objProxy = New UtilidadesDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConceptosTesoreriaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Public Async Sub inicializar()
        Try
            Dim strUtilizaPasiva As String = Await ConsultarParametro("CF_UTILIZAPASIVA_A2")
            strCuentasContable_Balance = Await ConsultarParametro("CUENTASCONTABLES_BALANCE")
            strCuentasContable_PYG = Await ConsultarParametro("CUENTASCONTABLES_PYG")

            If strUtilizaPasiva = "SI" Then
                logUtilizaPasiva = True
                MostrarNitTerceroParametroContable = Visibility.Collapsed
                strTipoItem = "ConsultaCuentasContables"

            Else
                logUtilizaPasiva = False
                MostrarNitTerceroParametroContable = Visibility.Visible
                strTipoItem = "CuentasContables"
            End If

            ListaCuentasBalance = New List(Of String)
            ListaCuentasPYG = New List(Of String)

            If Not String.IsNullOrEmpty(strCuentasContable_Balance) Then
                For Each li In strCuentasContable_Balance.Split(",")
                    ListaCuentasBalance.Add(li)
                Next
            End If

            If Not String.IsNullOrEmpty(strCuentasContable_PYG) Then
                For Each li In strCuentasContable_PYG.Split(",")
                    ListaCuentasPYG.Add(li)
                Next
            End If

            objProxy.Load(objProxy.cargarCombosCondicionalQuery("MAESTRO_CONCEPTOSTESORERIA", String.Empty, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombos, String.Empty)


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Inicializacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                Dim strNombreCategoria As String = String.Empty
                Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
                Dim objDiccionario As New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
                Dim objDiccionarioCamposEditables As New Dictionary(Of String, Visibility)
                Dim objListaNotas As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaTipoPago As New List(Of OYDUtilidades.ItemCombo)

                Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                For Each li In listaCategorias
                    strNombreCategoria = li
                    objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                    objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                Next

                DiccionarioCombos = objDiccionario

                dcProxy.Load(dcProxy.ConceptosTesoreriaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosTesoreria, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConceptosTesoreriPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosTesoreriaPorDefecto_Completed, "Default")
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "TerminoGenerarNotaContable", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "TerminoGenerarNotaContable", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Public Async Function ConsultarParametro(ByVal pstrParametro As String) As Task(Of String)
        Dim strValorParametro As String = String.Empty

        Try
            Dim objRet As InvokeOperation(Of String)

            objRet = Await objProxy.VerificaparametroSync(pstrParametro, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "ConsultarParametro", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    strValorParametro = objRet.Value
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "ConsultarParametro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return strValorParametro
    End Function

    Private Sub TerminoTraerConceptosTesoreriaPorDefecto_Completed(ByVal lo As LoadOperation(Of ConceptosTesoreri))
        If Not lo.HasError Then
            ConceptosTesoreriPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConceptosTesoreri por defecto", _
                                             Me.ToString(), "TerminoTraerConceptosTesoreriPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConceptosTesoreria(ByVal lo As LoadOperation(Of ConceptosTesoreri))
        If Not lo.HasError Then
            ListaConceptosTesoreria = dcProxy.ConceptosTesoreris
            If dcProxy.ConceptosTesoreris.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConceptosTesoreriSelected = ListaConceptosTesoreria.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosTesoreria", _
                                             Me.ToString(), "TerminoTraerConceptosTesoreri", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaConceptosTesoreria As EntitySet(Of ConceptosTesoreri)
    Public Property ListaConceptosTesoreria() As EntitySet(Of ConceptosTesoreri)
        Get
            Return _ListaConceptosTesoreria
        End Get
        Set(ByVal value As EntitySet(Of ConceptosTesoreri))
            _ListaConceptosTesoreria = value

            MyBase.CambioItem("ListaConceptosTesoreria")
            MyBase.CambioItem("ListaConceptosTesoreriaPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConceptosTesoreriAnterior) Then
                    ConceptosTesoreriSelected = _ListaConceptosTesoreria.FirstOrDefault
                Else
                    ConceptosTesoreriSelected = ConceptosTesoreriAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConceptosTesoreriaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConceptosTesoreria) Then
                Dim view = New PagedCollectionView(_ListaConceptosTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ConceptosTesoreriSelected As ConceptosTesoreri
    Public Property ConceptosTesoreriSelected() As ConceptosTesoreri
        Get
            Return _ConceptosTesoreriSelected
        End Get
        Set(ByVal value As ConceptosTesoreri)
            _ConceptosTesoreriSelected = value
            If Not value Is Nothing Then
                If Not IsNothing(_ConceptosTesoreriSelected.AplicaA) Then
                    HabilitarCamposFondos(False)
                End If
            End If
            MyBase.CambioItem("ConceptosTesoreriSelected")
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

    'JFGB MostrarCamposNota'
    Private _MostrarCamposNota As Visibility = Visibility.Collapsed
    Public Property MostrarCamposNota() As Visibility
        Get
            Return _MostrarCamposNota
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposNota = value
            MyBase.CambioItem("MostrarCamposNota")
        End Set
    End Property

    Private _MostrarCuentaContable As Visibility = Visibility.Collapsed
    Public Property MostrarCuentaContable() As Visibility
        Get
            Return _MostrarCuentaContable
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaContable = value
            MyBase.CambioItem("MostrarCuentaContable")
        End Set
    End Property

    Private _MostrarCuentaContableNotas As Visibility = Visibility.Collapsed
    Public Property MostrarCuentaContableNotas() As Visibility
        Get
            Return _MostrarCuentaContableNotas
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaContableNotas = value
            MyBase.CambioItem("MostrarCuentaContableNotas")
        End Set
    End Property

    Private _MostrarCuentaContableNotasDiferidos As Visibility = Visibility.Collapsed
    Public Property MostrarCuentaContableNotasDiferidos() As Visibility
        Get
            Return _MostrarCuentaContableNotasDiferidos
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaContableNotasDiferidos = value
            MyBase.CambioItem("MostrarCuentaContableNotasDiferidos")
        End Set
    End Property

    Private _DiccionarioCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombos() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
        End Set
    End Property

    Private _MostrarNitTerceroParametroContable As Visibility = Visibility.Visible
    Public Property MostrarNitTerceroParametroContable() As Visibility
        Get
            Return _MostrarNitTerceroParametroContable
        End Get
        Set(ByVal value As Visibility)
            _MostrarNitTerceroParametroContable = value
            MyBase.CambioItem("MostrarNitTerceroParametroContable")
        End Set
    End Property

    Private _MostrarCamposCRDB As Visibility = Visibility.Collapsed
    Public Property MostrarCamposCRDB() As Visibility
        Get
            Return _MostrarCamposCRDB
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposCRDB = value
            MyBase.CambioItem("MostrarCamposCRDB")
        End Set
    End Property

    Private _MostrarCamposDiferido As Visibility = Visibility.Collapsed
    Public Property MostrarCamposDiferido() As Visibility
        Get
            Return _MostrarCamposDiferido
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposDiferido = value
            MyBase.CambioItem("MostrarCamposDiferido")
        End Set
    End Property

    Private _MostrarAplica As Boolean
    Public Property MostrarAplica As Boolean
        Get
            Return _MostrarAplica
        End Get
        Set(value As Boolean)
            _MostrarAplica = value
            MyBase.CambioItem("MostrarAplica")
        End Set
    End Property

    Private _HabilitarActivo As Boolean
    Public Property HabilitarActivo As Boolean
        Get
            Return _HabilitarActivo
        End Get
        Set(value As Boolean)
            _HabilitarActivo = value
            MyBase.CambioItem("HabilitarActivo")
        End Set
    End Property

    'JCM20180712
    Private _strTipoItem As String
    Public Property strTipoItem As String
        Get
            Return _strTipoItem
        End Get
        Set(value As String)
            _strTipoItem = value
            MyBase.CambioItem("strTipoItem")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            'JFSB 20160930
            HabilitarActivo = False
            MostrarAplica = True
            Dim NewConceptosTesoreri As New ConceptosTesoreri
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewConceptosTesoreri.IDComisionista = ConceptosTesoreriPorDefecto.IDComisionista
            NewConceptosTesoreri.IDSucComisionista = ConceptosTesoreriPorDefecto.IDSucComisionista
            NewConceptosTesoreri.IDConcepto = ConceptosTesoreriPorDefecto.IDConcepto
            NewConceptosTesoreri.Detalle = ConceptosTesoreriPorDefecto.Detalle
            NewConceptosTesoreri.AplicaA = ConceptosTesoreriPorDefecto.AplicaA
            NewConceptosTesoreri.Actualizacion = ConceptosTesoreriPorDefecto.Actualizacion
            NewConceptosTesoreri.Usuario = Program.Usuario
            NewConceptosTesoreri.CuentaContable = ConceptosTesoreriPorDefecto.CuentaContable
            NewConceptosTesoreri.Activo = True      'JFSB 20160930
            NewConceptosTesoreri.ParametroContable = ConceptosTesoreriPorDefecto.ParametroContable
            NewConceptosTesoreri.CuentaContableAux = ConceptosTesoreriPorDefecto.CuentaContableAux
            NewConceptosTesoreri.NitTercero = ConceptosTesoreriPorDefecto.NitTercero
            NewConceptosTesoreri.ManejaCliente = "N"
            NewConceptosTesoreri.TipoMovimientoTesoreria = ConceptosTesoreriPorDefecto.TipoMovimientoTesoreria
            NewConceptosTesoreri.Retencion = ConceptosTesoreriPorDefecto.Retencion
            NewConceptosTesoreri.ManejaDiferido = False
            NewConceptosTesoreri.TipoMovimientoDiferido = ConceptosTesoreriPorDefecto.TipoMovimientoDiferido
            NewConceptosTesoreri.CuentaContableCRDiferido = ConceptosTesoreriPorDefecto.CuentaContableCRDiferido
            NewConceptosTesoreri.CuentaContableDBDiferido = ConceptosTesoreriPorDefecto.CuentaContableDBDiferido
            NewConceptosTesoreri.TipoNitCredito = "N/A"
            NewConceptosTesoreri.TipoNitDebito = "N/A"
            NewConceptosTesoreri.ManejaNotaDBCR = False
            NewConceptosTesoreri.TipoMovimientoCRDB = "N/A"
            NewConceptosTesoreri.TipoNitDiferidoDebito = "N/A"
            NewConceptosTesoreri.TipoNitDiferidoCredito = "N/A"
            ConceptosTesoreriAnterior = ConceptosTesoreriSelected
            ConceptosTesoreriSelected = NewConceptosTesoreri
            MostrarCamposNota = Visibility.Collapsed 'JFGB MostrarCamposNota'
            MostrarCamposCRDB = Visibility.Collapsed
            MostrarCamposDiferido = Visibility.Collapsed
            MyBase.CambioItem("ConceptosTesoreria")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
            MostrarCamposNota = Visibility.Collapsed 'JFGB MostrarCamposNota'
            MostrarCamposCRDB = Visibility.Collapsed
            MostrarCamposDiferido = Visibility.Collapsed
        End Try
    End Sub

    Private Function CuentaBalanceValida(ByVal pstrCuenta As String) As Boolean
        Dim logCuentaValida As Boolean = False

        For Each li In ListaCuentasBalance
            If Left(pstrCuenta, 2) = li Then
                logCuentaValida = True
                Exit For
            End If
        Next

        Return logCuentaValida
    End Function

    Private Function CuentaPYGValida(ByVal pstrCuenta As String) As Boolean
        Dim logCuentaValida As Boolean = False

        For Each li In ListaCuentasPYG
            If Left(pstrCuenta, 2) = li Then
                logCuentaValida = True
                Exit For
            End If
        Next

        Return logCuentaValida
    End Function

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ConceptosTesoreris.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConceptosTesoreriaFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosTesoreria, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConceptosTesoreriaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosTesoreria, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDConcepto <> 0 Or cb.Detalle <> String.Empty Or cb.AplicaA <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ConceptosTesoreris.Clear()
                ConceptosTesoreriAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDConcepto = " & cb.IDConcepto.ToString() & " Detalle = " & cb.Detalle.ToString() & " AplicaA = " & cb.AplicaA.ToString()
                dcProxy.Load(dcProxy.ConceptosTesoreriaConsultarQuery(cb.IDConcepto, cb.Detalle, cb.AplicaA, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosTesoreria, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConceptosTesoreri
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
            'validar
            If MostrarCamposNota = Visibility.Visible Then
                If String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoTesoreria) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El campo tipo movimiento tesoreria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If _ConceptosTesoreriSelected.ManejaDiferido And String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoDiferido) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El campo tipo movimiento diferido tesoreria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If _ConceptosTesoreriSelected.ManejaDiferido Then
                    Dim logManejaBancoTipoMovimiento As Boolean = False
                    Dim logManejaBancoTipoDiferido As Boolean = False

                    If _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BB.ToString _
                        Or _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BBA.ToString _
                        Or _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BP.ToString Then
                        logManejaBancoTipoMovimiento = True
                    End If

                    If _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BB.ToString _
                       Or _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BBA.ToString _
                       Or _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BP.ToString Then
                        logManejaBancoTipoDiferido = True
                    End If

                    If logManejaBancoTipoMovimiento And logManejaBancoTipoDiferido Then
                        A2Utilidades.Mensajes.mostrarMensaje("Cuando Maneja Diferido los dos tipos de movimiento no pueden mover bancos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    If _ConceptosTesoreriSelected.ManejaNotaDBCR Then
                        A2Utilidades.Mensajes.mostrarMensaje("Cuando Maneja Diferido no se permite manejar la opción Maneja NC/ND.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                If _ConceptosTesoreriSelected.ManejaNotaDBCR Then
                    If String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoCRDB) Or _ConceptosTesoreriSelected.TipoMovimientoCRDB = "N/A" Then
                        A2Utilidades.Mensajes.mostrarMensaje("Cuando se Maneja DB/CR se debe de seleccionar el Tipo movimiento NC/ND.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                If _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BABA.ToString _
                    Or _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BAP.ToString _
                    Or _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.PP.ToString Then
                    If String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) Or String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El tipo movimiento del concepto exige que se encuentren matriculadas las dos cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BABA.ToString Then
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContable) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableAux) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.PP.ToString Then
                        If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContable) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableAux) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BAP.ToString Then
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContable) = False And CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContable) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las cuentas contables matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableAux) = False And CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableAux) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las cuentas contables matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContable) And _
                            CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableAux) Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Una de las cuentas matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContable) And _
                            CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableAux) Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Una de las cuentas matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                ElseIf _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BBA.ToString Then
                    If String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) And String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                        A2Utilidades.Mensajes.mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas de tipo balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) Then
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContable) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                        If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableAux) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                ElseIf _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BP.ToString Then
                    If String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) And String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                        A2Utilidades.Mensajes.mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) Then
                        If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContable) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                        If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableAux) = False Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                ElseIf _ConceptosTesoreriSelected.TipoMovimientoTesoreria = TipoMvtoTesoreria.BB.ToString Then
                    If Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) Or Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Para tipo movimiento no se puede tener registrada cuenta contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If

                If _ConceptosTesoreriSelected.ManejaDiferido Then
                    If _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BABA.ToString _
                    Or _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BAP.ToString _
                    Or _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.PP.ToString Then
                        If String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) Or String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El tipo movimiento diferido del concepto exige que se encuentren matriculadas las dos cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BABA.ToString Then
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        ElseIf _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.PP.ToString Then
                            If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                            If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        ElseIf _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BAP.ToString Then
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) = False And CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las cuentas contables diferido matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) = False And CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Las cuentas contables diferido matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) And _
                                CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Una de las cuentas diferido matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                            If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) And _
                                CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Una de las cuentas diferido matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        End If
                    ElseIf _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BBA.ToString Then
                        If String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) And String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas diferido de tipo balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) Then
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                            If CuentaBalanceValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        End If
                    ElseIf _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BP.ToString Then
                        If String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) And String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                            A2Utilidades.Mensajes.mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas diferido de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) Then
                            If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableCRDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        ElseIf Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                            If CuentaPYGValida(_ConceptosTesoreriSelected.CuentaContableDBDiferido) = False Then
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        End If
                    ElseIf _ConceptosTesoreriSelected.TipoMovimientoDiferido = TipoMvtoTesoreria.BB.ToString Then
                        If Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) Or Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Para tipo movimiento diferido no se puede tener registrada cuenta contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                End If
            End If

            If MostrarCuentaContable = Visibility.Visible Then
                If _ConceptosTesoreriSelected.CuentaContable <> String.Empty Then
                    If Not Versioned.IsNumeric(_ConceptosTesoreriSelected.CuentaContable) Then
                        A2Utilidades.Mensajes.mostrarMensaje("La Cuenta Contable debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        CambioNroDoc = True
                        _ConceptosTesoreriSelected.CuentaContable = String.Empty
                    End If
                End If
            End If

            Dim origen = "update"
            ErrorForma = ""

            ConceptosTesoreriAnterior = ConceptosTesoreriSelected
            If Not ListaConceptosTesoreria.Contains(ConceptosTesoreriSelected) Then
                origen = "insert"
                ListaConceptosTesoreria.Add(ConceptosTesoreriSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'JFSB 20171018 Se agrega validación para que asocie solo cuando tenga datos la lista
                    If _ListaConceptosTesoreria.Count > 0 Then
                        ConceptosTesoreriSelected = _ListaConceptosTesoreria.Last
                    End If
                    'ConceptosTesoreriSelected = 
                    'If So.UserState = "insert" Then
                    '    ListaEmpleados.Remove(EmpleadoSelected)
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            logCuentaContable = False
            logCuentaContableAux = False
            logCuentaContableCRDiferido = False
            logCuentaContableDBDiferido = False
            'If So.UserState = "insert" Then
            '    dcProxy.Bolsas.Clear()
            '    dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaConceptosTesoreri
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ConceptosTesoreriSelected) Then
            IsBusy = True
            dcProxy1.ConsultarConcepto(_ConceptosTesoreriSelected.IDConcepto, Program.Usuario, Program.HashConexion, AddressOf TerminoCargarConcepto, String.Empty)
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Private Sub TerminoCargarConcepto(ByVal lo As InvokeOperation(Of Integer))
        Try
            If lo.HasError Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el concepto",
                Me.ToString(), "AddressOf TerminoCargarConcepto", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            Else
                Editando = True
                HabilitarActivo = True
                If lo.Value = 1 Then
                    MostrarAplica = False
                Else
                    MostrarAplica = True
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de generación de órdenes.", Me.ToString(), "TerminoCargarLlamadaJobLEO", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConceptosTesoreriSelected) Then
                Editando = False
                dcProxy.RejectChanges()
                HabilitarActivo = False
                MostrarAplica = False
                If _ConceptosTesoreriSelected.EntityState = EntityState.Detached Then
                    ConceptosTesoreriSelected = ConceptosTesoreriAnterior
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
            If Not IsNothing(_ConceptosTesoreriSelected) Then
                dcProxy.ConceptosTesoreris.Remove(_ConceptosTesoreriSelected)
                ConceptosTesoreriSelected = _ListaConceptosTesoreria.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
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
        DicCamposTab.Add("AplicaA", 1)
        DicCamposTab.Add("Detalle", 1)
    End Sub

    Private Sub _ConceptosTesoreriSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ConceptosTesoreriSelected.PropertyChanged
        Try
            If CambioNroDoc Then
                CambioNroDoc = False
                Exit Sub
            End If
            If e.PropertyName.Equals("NitTercero") And _ConceptosTesoreriSelected.NitTercero <> String.Empty Then
                If Not Versioned.IsNumeric(_ConceptosTesoreriSelected.NitTercero) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El Nit del Tercero debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CambioNroDoc = True
                    _ConceptosTesoreriSelected.NitTercero = String.Empty
                End If
            ElseIf e.PropertyName.Equals("AplicaA") Then
                HabilitarCamposFondos(True)
            ElseIf e.PropertyName.Equals("TipoMovimientoTesoreria") Then
                If String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoTesoreria) Then
                    MostrarCuentaContableNotas = Visibility.Collapsed
                Else
                    If _ConceptosTesoreriSelected.TipoMovimientoTesoreria <> TipoMvtoTesoreria.BB.ToString Then
                        MostrarCuentaContableNotas = Visibility.Visible
                    Else
                        MostrarCuentaContableNotas = Visibility.Collapsed
                        _ConceptosTesoreriSelected.CuentaContable = String.Empty
                        _ConceptosTesoreriSelected.CuentaContableAux = String.Empty
                    End If
                End If
            ElseIf e.PropertyName.Equals("ManejaDiferido") Then
                If _ConceptosTesoreriSelected.ManejaDiferido = False Then
                    _ConceptosTesoreriSelected.TipoMovimientoDiferido = String.Empty
                    _ConceptosTesoreriSelected.CuentaContableCRDiferido = String.Empty
                    _ConceptosTesoreriSelected.CuentaContableDBDiferido = String.Empty
                    MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                    MostrarCamposCRDB = Visibility.Visible
                Else
                    _ConceptosTesoreriSelected.ManejaNotaDBCR = False
                    _ConceptosTesoreriSelected.TipoMovimientoCRDB = "N/A"
                    MostrarCamposCRDB = Visibility.Collapsed
                End If
            ElseIf e.PropertyName.Equals("TipoMovimientoDiferido") Then
                If _ConceptosTesoreriSelected.ManejaDiferido Then
                    If String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoDiferido) Then
                        MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                    Else
                        If _ConceptosTesoreriSelected.TipoMovimientoDiferido <> TipoMvtoTesoreria.BB.ToString Then
                            MostrarCuentaContableNotasDiferidos = Visibility.Visible
                        Else
                            MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                        End If
                    End If
                Else
                    MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                End If
            ElseIf e.PropertyName.Equals("ManejaNotaDBCR") Then
                If _ConceptosTesoreriSelected.ManejaNotaDBCR = False Then
                    MostrarCamposDiferido = Visibility.Visible
                Else
                    _ConceptosTesoreriSelected.ManejaDiferido = False
                    _ConceptosTesoreriSelected.TipoMovimientoDiferido = String.Empty
                    _ConceptosTesoreriSelected.CuentaContableCRDiferido = String.Empty
                    _ConceptosTesoreriSelected.CuentaContableDBDiferido = String.Empty
                    MostrarCamposDiferido = Visibility.Collapsed
                End If

                'DEMC20170914  INICIO
            ElseIf e.PropertyName.Equals("CuentaContable") Then
                If Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContable) Then
                    logCuentaContable = True
                    logCuentaContableAux = False
                    logCuentaContableCRDiferido = False
                    logCuentaContableDBDiferido = False

                    'JCM20180711 En caso de tener el parametro prendido
                    'se realiza una busqueda distinta
                    If logUtilizaPasiva = True Then
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContable, "ConsultaCuentasContables")
                    Else
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContable, "CuentasContables")
                    End If


                End If

            ElseIf e.PropertyName.Equals("CuentaContableAux") Then
                If Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableAux) Then
                    logCuentaContableAux = True
                    logCuentaContable = False
                    logCuentaContableCRDiferido = False
                    logCuentaContableDBDiferido = False

                    'JCM20180711 En caso de tener el parametro prendido
                    'se realiza una busqueda distinta
                    If logUtilizaPasiva = True Then
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContableAux, "ConsultaCuentasContables")
                    Else
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContableAux, "CuentasContables")
                    End If


                End If

            ElseIf e.PropertyName.Equals("CuentaContableCRDiferido") Then
                If Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableCRDiferido) Then
                    logCuentaContableCRDiferido = True
                    logCuentaContable = False
                    logCuentaContableDBDiferido = False
                    logCuentaContableAux = False

                    'JCM20180711 En caso de tener el parametro prendido
                    'se realiza una busqueda distinta
                    If logUtilizaPasiva = True Then
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContableCRDiferido, "ConsultaCuentasContables")
                    Else
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContableCRDiferido, "CuentasContables")
                    End If


                End If

            ElseIf e.PropertyName.Equals("CuentaContableDBDiferido") Then
                If Not String.IsNullOrEmpty(_ConceptosTesoreriSelected.CuentaContableDBDiferido) Then
                    logCuentaContableDBDiferido = True
                    logCuentaContable = False
                    logCuentaContableAux = False
                    logCuentaContableCRDiferido = False
                    'JCM20180711 En caso de tener el parametro prendido
                    'se realiza una busqueda distinta
                    If logUtilizaPasiva = True Then
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContableDBDiferido, "ConsultaCuentasContables")
                    Else
                        buscarGenerico(_ConceptosTesoreriSelected.CuentaContableDBDiferido, "CuentasContables")
                    End If

                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la propiedad.",
                                                         Me.ToString(), "_ConceptosTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Friend Sub buscarGenerico(Optional ByVal pstrCentroCostos As String = "", Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery(pstrBusqueda, pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "CuentasContables", "ConsultaCuentasContables"
                        If Editando Then
                            If lo.Entities.ToList.Count > 0 Then
                                If logCuentaContable Then
                                    _ConceptosTesoreriSelected.CuentaContable = lo.Entities.First.IdItem
                                ElseIf logCuentaContableAux Then
                                    _ConceptosTesoreriSelected.CuentaContableAux = lo.Entities.First.IdItem
                                ElseIf logCuentaContableCRDiferido Then
                                    _ConceptosTesoreriSelected.CuentaContableCRDiferido = lo.Entities.First.IdItem
                                ElseIf logCuentaContableDBDiferido Then
                                    _ConceptosTesoreriSelected.CuentaContableDBDiferido = lo.Entities.First.IdItem
                                End If
                            Else
                                sw = 1
                                A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                If logCuentaContable Then
                                    _ConceptosTesoreriSelected.CuentaContable = Nothing
                                ElseIf logCuentaContableAux Then
                                    _ConceptosTesoreriSelected.CuentaContableAux = Nothing
                                ElseIf logCuentaContableCRDiferido Then
                                    _ConceptosTesoreriSelected.CuentaContableCRDiferido = Nothing
                                ElseIf logCuentaContableDBDiferido Then
                                    _ConceptosTesoreriSelected.CuentaContableDBDiferido = Nothing
                                End If

                            End If
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos",
                                             Me.ToString(), "TerminoTraerBuscadorGenerico", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerBuscadorGenerico", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub
    'DEMC20170914 FIN



    Private Sub HabilitarCamposFondos(ByVal plogLimpiarCampos As Boolean)
        Try
            If Not IsNothing(_ConceptosTesoreriSelected.AplicaA) Then

                If logUtilizaPasiva And (_ConceptosTesoreriSelected.AplicaA.ToUpper = "NOTAS" _
                                                 Or _ConceptosTesoreriSelected.AplicaA.ToUpper = "TODOS") Then

                    MostrarCamposNota = Visibility.Visible
                    MostrarCuentaContable = Visibility.Collapsed
                    MostrarCamposCRDB = Visibility.Collapsed
                    MostrarCamposDiferido = Visibility.Collapsed

                    If String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoTesoreria) Then
                        MostrarCuentaContableNotas = Visibility.Collapsed
                    Else
                        If _ConceptosTesoreriSelected.TipoMovimientoTesoreria <> TipoMvtoTesoreria.BB.ToString Then
                            MostrarCuentaContableNotas = Visibility.Visible
                        Else
                            MostrarCuentaContableNotas = Visibility.Collapsed
                        End If
                    End If

                    If _ConceptosTesoreriSelected.ManejaNotaDBCR Then
                        MostrarCamposCRDB = Visibility.Visible
                    Else
                        MostrarCamposDiferido = Visibility.Visible
                        If _ConceptosTesoreriSelected.ManejaDiferido Then
                            If String.IsNullOrEmpty(_ConceptosTesoreriSelected.TipoMovimientoDiferido) Then
                                MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                            Else
                                If _ConceptosTesoreriSelected.TipoMovimientoDiferido <> TipoMvtoTesoreria.BB.ToString Then
                                    MostrarCuentaContableNotasDiferidos = Visibility.Visible
                                Else
                                    MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                                End If
                            End If
                        Else
                            MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                            MostrarCamposCRDB = Visibility.Visible
                        End If
                    End If
                Else
                    MostrarCamposNota = Visibility.Collapsed
                    MostrarCuentaContable = Visibility.Visible
                    MostrarCuentaContableNotas = Visibility.Collapsed
                    MostrarCuentaContableNotasDiferidos = Visibility.Collapsed
                    MostrarCamposCRDB = Visibility.Collapsed
                    MostrarCamposDiferido = Visibility.Collapsed

                    If plogLimpiarCampos Then
                        _ConceptosTesoreriSelected.ManejaDiferido = False
                        _ConceptosTesoreriSelected.TipoMovimientoDiferido = String.Empty
                        _ConceptosTesoreriSelected.CuentaContableCRDiferido = String.Empty
                        _ConceptosTesoreriSelected.CuentaContableDBDiferido = String.Empty
                        _ConceptosTesoreriSelected.TipoNitCredito = String.Empty
                        _ConceptosTesoreriSelected.TipoNitDebito = String.Empty
                        _ConceptosTesoreriSelected.TipoNitDiferidoDebito = String.Empty
                        _ConceptosTesoreriSelected.TipoNitDiferidoCredito = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la propiedad.", _
                                                         Me.ToString(), "HabilitarCamposFondos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaConceptosTesoreri

    <Display(Name:="Código")> _
    Public Property IDConcepto As Integer

    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")> _
     <Display(Name:="Detalle")> _
    Public Property Detalle As String

    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
     <Display(Name:="Aplica A")> _
    Public Property AplicaA As String

End Class