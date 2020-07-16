Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.OyDTesoreria
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes
Imports System.Threading.Tasks

Public Class GenerarNotasContablesFondosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"

    Dim dcProxy As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Public viewGenerarNotas As GenerarNotasContablesFondosView

    Dim strCuentasContable_Balance As String = String.Empty
    Dim strCuentasContable_PYG As String = String.Empty

    Public URLServicioDocumento As String = String.Empty

    Dim ListaCuentasBalance As List(Of String) = Nothing
    Dim ListaCuentasPYG As List(Of String) = Nothing

    Public Enum TipoMvtoTesoreria
        BABA    'BALANCE X BALANCE
        BAP     'BALANCE X PYG
        BB      'BANCO X BANCO
        BBA     'BANCO X BALANCE
        BP      'BANCO X PYG
        PP      'PYG X PYG
    End Enum

    Public strConsecutivoGMF_Compania As String = String.Empty
    Public strConsecutivoRetencion_Compania As String = String.Empty

#End Region

#Region "Inicializacion"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            DirectCast(objProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

            If Not Program.IsDesignMode() Then
                FechaActual = Now.Date.ToString("yyyy-MM-dd")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "GenerarNotasContablesFondosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub inicializar()
        Try
            strCuentasContable_Balance = Await ConsultarParametro("CUENTASCONTABLES_BALANCE")
            strCuentasContable_PYG = Await ConsultarParametro("CUENTASCONTABLES_PYG")
            URLServicioDocumento = Await ConsultarParametro("SERVICIO_DOCUMENTOS")


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

            viewGenerarNotas.ConfigurarSubirDocumento()

            objProxy.Load(objProxy.cargarCombosEspecificosQuery("GenerarNotasFondos", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Inicializacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = String.Empty

        Try
            If Not lo.HasError Then
                'Obtiene los valores del UserState
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New List(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New List(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))

                        dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCombosNotas = dicListaCombos

                    TipoConsulta = "DIFERIDOS"

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _IDCompania As Integer
    Public Property IDCompania() As Integer
        Get
            Return _IDCompania
        End Get
        Set(ByVal value As Integer)
            _IDCompania = value
            MyBase.CambioItem("IDCompania")
        End Set
    End Property

    Private _NombreCompania As String
    Public Property NombreCompania() As String
        Get
            Return _NombreCompania
        End Get
        Set(ByVal value As String)
            _NombreCompania = value
            MyBase.CambioItem("NombreCompania")
        End Set
    End Property

    Private _NombreConsecutivo As String
    Public Property NombreConsecutivo() As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            LimpiarCampos("CONSECUTIVO")
            MyBase.CambioItem("NombreConsecutivo")
        End Set
    End Property

    Private _ListaConsecutivos As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivos = value
            MyBase.CambioItem("ListaConsecutivos")
        End Set
    End Property

    Private _IDConcepto As Integer
    Public Property IDConcepto() As Integer
        Get
            Return _IDConcepto
        End Get
        Set(ByVal value As Integer)
            _IDConcepto = value
            MyBase.CambioItem("IDConcepto")
        End Set
    End Property

    Private _DetalleConcepto As String
    Public Property DetalleConcepto() As String
        Get
            Return _DetalleConcepto
        End Get
        Set(ByVal value As String)
            _DetalleConcepto = value
            ConcatenarConceptoYDetalle(DetalleConcepto, DescripcionConcepto)
            MyBase.CambioItem("DetalleConcepto")
        End Set
    End Property

    Private _DescripcionConcepto As String
    Public Property DescripcionConcepto() As String
        Get
            Return _DescripcionConcepto
        End Get
        Set(ByVal value As String)
            _DescripcionConcepto = value
            ConcatenarConceptoYDetalle(DetalleConcepto, DescripcionConcepto)
            MyBase.CambioItem("DescripcionConcepto")
        End Set
    End Property

    Private _DescripcionConceptoCompleta As String
    Public Property DescripcionConceptoCompleta() As String
        Get
            Return _DescripcionConceptoCompleta
        End Get
        Set(ByVal value As String)
            _DescripcionConceptoCompleta = value
            MyBase.CambioItem("DescripcionConceptoCompleta")
        End Set
    End Property

    Private _TipoMovimiento As String
    Public Property TipoMovimiento() As String
        Get
            Return _TipoMovimiento
        End Get
        Set(ByVal value As String)
            _TipoMovimiento = value
            MyBase.CambioItem("TipoMovimiento")
        End Set
    End Property

    Private _DescripcionTipoMovimiento As String
    Public Property DescripcionTipoMovimiento() As String
        Get
            Return _DescripcionTipoMovimiento
        End Get
        Set(ByVal value As String)
            _DescripcionTipoMovimiento = value
            MyBase.CambioItem("DescripcionTipoMovimiento")
        End Set
    End Property

    Private _Retencion As String
    Public Property Retencion() As String
        Get
            Return _Retencion
        End Get
        Set(ByVal value As String)
            _Retencion = value
            MyBase.CambioItem("Retencion")
        End Set
    End Property

    Private _DescripcionRetencion As String
    Public Property DescripcionRetencion() As String
        Get
            Return _DescripcionRetencion
        End Get
        Set(ByVal value As String)
            _DescripcionRetencion = value
            MyBase.CambioItem("DescripcionRetencion")
        End Set
    End Property

    Private _CuentaContableCR As String
    Public Property CuentaContableCR() As String
        Get
            Return _CuentaContableCR
        End Get
        Set(ByVal value As String)
            _CuentaContableCR = value
            MyBase.CambioItem("CuentaContableCR")
        End Set
    End Property

    Private _CuentaContableDB As String
    Public Property CuentaContableDB() As String
        Get
            Return _CuentaContableDB
        End Get
        Set(ByVal value As String)
            _CuentaContableDB = value
            MyBase.CambioItem("CuentaContableDB")
        End Set
    End Property

    Private _IDBancoCR As Nullable(Of Integer)
    Public Property IDBancoCR() As Nullable(Of Integer)
        Get
            Return _IDBancoCR
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDBancoCR = value
            MyBase.CambioItem("IDBancoCR")
        End Set
    End Property

    Private _NombreBancoCR As String
    Public Property NombreBancoCR() As String
        Get
            Return _NombreBancoCR
        End Get
        Set(ByVal value As String)
            _NombreBancoCR = value
            MyBase.CambioItem("NombreBancoCR")
        End Set
    End Property

    Private _IDBancoDB As Nullable(Of Integer)
    Public Property IDBancoDB() As Nullable(Of Integer)
        Get
            Return _IDBancoDB
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDBancoDB = value
            MyBase.CambioItem("IDBancoDB")
        End Set
    End Property

    Private _NombreBancoDB As String
    Public Property NombreBancoDB() As String
        Get
            Return _NombreBancoDB
        End Get
        Set(ByVal value As String)
            _NombreBancoDB = value
            MyBase.CambioItem("NombreBancoDB")
        End Set
    End Property

    Private _Valor As Decimal
    Public Property Valor() As Decimal
        Get
            Return _Valor
        End Get
        Set(ByVal value As Decimal)
            _Valor = value
            MyBase.CambioItem("Valor")
        End Set
    End Property

    Private _HabilitarConsecutivo As Boolean = False
    Public Property HabilitarConsecutivo() As Boolean
        Get
            Return _HabilitarConsecutivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarConsecutivo = value
            MyBase.CambioItem("HabilitarConsecutivo")
        End Set
    End Property

    Private _HabilitarConcepto As Boolean = False
    Public Property HabilitarConcepto() As Boolean
        Get
            Return _HabilitarConcepto
        End Get
        Set(ByVal value As Boolean)
            _HabilitarConcepto = value
            MyBase.CambioItem("HabilitarConcepto")
        End Set
    End Property

    Private _HabilitarBancoCR As Boolean = False
    Public Property HabilitarBancoCR() As Boolean
        Get
            Return _HabilitarBancoCR
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBancoCR = value
            MyBase.CambioItem("HabilitarBancoCR")
        End Set
    End Property

    Private _HabilitarBancoDB As Boolean = False
    Public Property HabilitarBancoDB() As Boolean
        Get
            Return _HabilitarBancoDB
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBancoDB = value
            MyBase.CambioItem("HabilitarBancoDB")
        End Set
    End Property

    Private _ManejaNotaCRDB As Boolean
    Public Property ManejaNotaCRDB() As Boolean
        Get
            Return _ManejaNotaCRDB
        End Get
        Set(ByVal value As Boolean)
            _ManejaNotaCRDB = value
            MyBase.CambioItem("ManejaNotaCRDB")
        End Set
    End Property

    Private _IDNotaCRDB As String
    Public Property IDNotaCRDB() As String
        Get
            Return _IDNotaCRDB
        End Get
        Set(ByVal value As String)
            _IDNotaCRDB = value
            MyBase.CambioItem("IDNotaCRDB")
        End Set
    End Property

    Private _DescripcionNotaCRDB As String
    Public Property DescripcionNotaCRDB() As String
        Get
            Return _DescripcionNotaCRDB
        End Get
        Set(ByVal value As String)
            _DescripcionNotaCRDB = value
            MyBase.CambioItem("DescripcionNotaCRDB")
        End Set
    End Property

    Private _Nit As String
    Public Property Nit() As String
        Get
            Return _Nit
        End Get
        Set(ByVal value As String)
            _Nit = value
            MyBase.CambioItem("Nit")
        End Set
    End Property

    Private _CentroCostos As String
    Public Property CentroCostos() As String
        Get
            Return _CentroCostos
        End Get
        Set(ByVal value As String)
            _CentroCostos = value
            MyBase.CambioItem("CentroCostos")
        End Set
    End Property

    Private _FechaActual As String
    Public Property FechaActual() As String
        Get
            Return _FechaActual
        End Get
        Set(ByVal value As String)
            _FechaActual = value
            MyBase.CambioItem("FechaActual")
        End Set
    End Property

    Private _FechaInicial As Nullable(Of DateTime) = Now.Date
    Public Property FechaInicial() As Nullable(Of DateTime)
        Get
            Return _FechaInicial
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaInicial = value
            MyBase.CambioItem("FechaInicial")
        End Set
    End Property

    Private _FechaFinal As Nullable(Of DateTime) = Now.Date
    Public Property FechaFinal() As Nullable(Of DateTime)
        Get
            Return _FechaFinal
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaFinal = value
            MyBase.CambioItem("FechaFinal")
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

    Private _MostrarCamposCodigoOYD As Visibility = Visibility.Collapsed
    Public Property MostrarCamposCodigoOYD() As Visibility
        Get
            Return _MostrarCamposCodigoOYD
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposCodigoOYD = value
            MyBase.CambioItem("MostrarCamposCodigoOYD")
        End Set
    End Property

    Private _MostrarCamposRetiros As Visibility = Visibility.Collapsed
    Public Property MostrarCamposRetiros() As Visibility
        Get
            Return _MostrarCamposRetiros
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposRetiros = value
            MyBase.CambioItem("MostrarCamposRetiros")
        End Set
    End Property

    Private _ManejaDiferido As Boolean = False
    Public Property ManejaDiferido() As Boolean
        Get
            Return _ManejaDiferido
        End Get
        Set(ByVal value As Boolean)
            _ManejaDiferido = value
            MyBase.CambioItem("ManejaDiferido")
        End Set
    End Property

    Private _ListaTipoPago As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoPago() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoPago
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoPago = value
            MyBase.CambioItem("ListaTipoPago")
        End Set
    End Property

    Private _TipoPagoDiferido As String
    Public Property TipoPagoDiferido() As String
        Get
            Return _TipoPagoDiferido
        End Get
        Set(ByVal value As String)
            _TipoPagoDiferido = value
            MyBase.CambioItem("TipoPagoDiferido")
        End Set
    End Property

    Private _TipoMovimientoDiferido As String
    Public Property TipoMovimientoDiferido() As String
        Get
            Return _TipoMovimientoDiferido
        End Get
        Set(ByVal value As String)
            _TipoMovimientoDiferido = value
            MyBase.CambioItem("TipoMovimientoDiferido")
        End Set
    End Property

    Private _DescripcionTipoMovimientoDiferido As String
    Public Property DescripcionTipoMovimientoDiferido() As String
        Get
            Return _DescripcionTipoMovimientoDiferido
        End Get
        Set(ByVal value As String)
            _DescripcionTipoMovimientoDiferido = value
            MyBase.CambioItem("DescripcionTipoMovimientoDiferido")
        End Set
    End Property

    Private _CuentaContableCRDiferido As String
    Public Property CuentaContableCRDiferido() As String
        Get
            Return _CuentaContableCRDiferido
        End Get
        Set(ByVal value As String)
            _CuentaContableCRDiferido = value
            MyBase.CambioItem("CuentaContableCRDiferido")
        End Set
    End Property

    Private _CuentaContableDBDiferido As String
    Public Property CuentaContableDBDiferido() As String
        Get
            Return _CuentaContableDBDiferido
        End Get
        Set(ByVal value As String)
            _CuentaContableDBDiferido = value
            MyBase.CambioItem("CuentaContableDBDiferido")
        End Set
    End Property

    Private _DescripcionFechaInicial As String = "Fecha"
    Public Property DescripcionFechaInicial() As String
        Get
            Return _DescripcionFechaInicial
        End Get
        Set(ByVal value As String)
            _DescripcionFechaInicial = value
            MyBase.CambioItem("DescripcionFechaInicial")
        End Set
    End Property

    Private _mobjCtlSubirArchivo As A2DocumentosWPF.A2SubirDocumento

    Private _mstrArchivo As String
    Public Property mstrArchivo() As String
        Get
            Return _mstrArchivo
        End Get
        Set(ByVal value As String)
            _mstrArchivo = value
            MyBase.CambioItem("mstrArchivo")
        End Set
    End Property

    Private _mstrRuta As String
    Public Property mstrRuta() As String
        Get
            Return _mstrRuta
        End Get
        Set(ByVal value As String)
            _mstrRuta = value
            MyBase.CambioItem("mstrRuta")
        End Set
    End Property

    Private _mabytArchivo As Byte()
    Public Property mabytArchivo As Byte()
        Get
            Return _mabytArchivo
        End Get
        Set(ByVal value As Byte())
            _mabytArchivo = value
            MyBase.CambioItem("mabytArchivo")
        End Set
    End Property

    Private _TipoConsulta As String
    Public Property TipoConsulta() As String
        Get
            Return _TipoConsulta
        End Get
        Set(ByVal value As String)
            _TipoConsulta = value
            If _TipoConsulta = "DIFERIDOS" Then
                TextoColumnaCancelar = "Generar faltante"
            Else
                TextoColumnaCancelar = "Cancelar"
            End If
            ConsultarNotaContablesPendientes()
            MyBase.CambioItem("TipoConsulta")
        End Set
    End Property

    Private _ListaNotasCancelar As List(Of GenerarNotasFondosCancelacion)
    Public Property ListaNotasCancelar() As List(Of GenerarNotasFondosCancelacion)
        Get
            Return _ListaNotasCancelar
        End Get
        Set(ByVal value As List(Of GenerarNotasFondosCancelacion))
            _ListaNotasCancelar = value
            MyBase.CambioItem("ListaNotasCancelar")

            If Not IsNothing(_ListaNotasCancelar) Then
                If _ListaNotasCancelar.Count > 0 Then
                    NotaCancelar = _ListaNotasCancelar.First
                End If
            End If
        End Set
    End Property

    Private _NotaCancelar As GenerarNotasFondosCancelacion
    Public Property NotaCancelar() As GenerarNotasFondosCancelacion
        Get
            Return _NotaCancelar
        End Get
        Set(ByVal value As GenerarNotasFondosCancelacion)
            _NotaCancelar = value
            MyBase.CambioItem("NotaCancelar")
        End Set
    End Property

    Private _DiccionarioCombosNotas As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosNotas() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosNotas
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosNotas = value
            MyBase.CambioItem("DiccionarioCombosNotas")
        End Set
    End Property

    Private _SeleccionarTodosCancelacion As Boolean
    Public Property SeleccionarTodosCancelacion() As Boolean
        Get
            Return _SeleccionarTodosCancelacion
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodosCancelacion = value
            SeleccionarTodosRegistrosCancelar(_SeleccionarTodosCancelacion)
            MyBase.CambioItem("SeleccionarTodosCancelacion")
        End Set
    End Property

    Private _TextoColumnaCancelar As String = "Cancelar"
    Public Property TextoColumnaCancelar() As String
        Get
            Return _TextoColumnaCancelar
        End Get
        Set(ByVal value As String)
            _TextoColumnaCancelar = value
            MyBase.CambioItem("TextoColumnaCancelar")
        End Set
    End Property

    Private _TipoNitDB As String
    Public Property TipoNitDB() As String
        Get
            Return _TipoNitDB
        End Get
        Set(ByVal value As String)
            _TipoNitDB = value
            MyBase.CambioItem("TipoNitDB")
        End Set
    End Property

    Private _TipoNitCR As String
    Public Property TipoNitCR() As String
        Get
            Return _TipoNitCR
        End Get
        Set(ByVal value As String)
            _TipoNitCR = value
            MyBase.CambioItem("TipoNitCR")
        End Set
    End Property

    Private _HabilitarSeleccionNit As Boolean = False
    Public Property HabilitarSeleccionNit() As Boolean
        Get
            Return _HabilitarSeleccionNit
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionNit = value
            MyBase.CambioItem("HabilitarSeleccionNit")
        End Set
    End Property

    Private _CodigoOYD As String
    Public Property CodigoOYD() As String
        Get
            Return _CodigoOYD
        End Get
        Set(ByVal value As String)
            _CodigoOYD = value
            MyBase.CambioItem("CodigoOYD")
        End Set
    End Property

    Private _ListaEncargos As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaEncargos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaEncargos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaEncargos = value
            MyBase.CambioItem("ListaEncargos")
        End Set
    End Property

    Private _Encargo As Nullable(Of Integer)
    Public Property Encargo() As Nullable(Of Integer)
        Get
            Return _Encargo
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Encargo = value
            ConsultarCombosMovimientosRetiro()
            MyBase.CambioItem("Encargo")
        End Set
    End Property

    Private _FechaRetiro As DateTime = Now.Date
    Public Property FechaRetiro() As DateTime
        Get
            Return _FechaRetiro
        End Get
        Set(ByVal value As DateTime)
            _FechaRetiro = value
            ConsultarCombosMovimientosRetiro()
            MyBase.CambioItem("FechaRetiro")
        End Set
    End Property

    Private _ListaRetiros As List(Of OyDTesoreria.GenerarNotasFondos_Retiros)
    Public Property ListaRetiros() As List(Of OyDTesoreria.GenerarNotasFondos_Retiros)
        Get
            Return _ListaRetiros
        End Get
        Set(ByVal value As List(Of OyDTesoreria.GenerarNotasFondos_Retiros))
            _ListaRetiros = value
            MyBase.CambioItem("ListaRetiros")
        End Set
    End Property

    Private _RetiroSeleccionado As OyDTesoreria.GenerarNotasFondos_Retiros
    Public Property RetiroSeleccionado() As OyDTesoreria.GenerarNotasFondos_Retiros
        Get
            Return _RetiroSeleccionado
        End Get
        Set(ByVal value As OyDTesoreria.GenerarNotasFondos_Retiros)
            _RetiroSeleccionado = value
            If Not IsNothing(_RetiroSeleccionado) Then
                IDRetiroSeleccionado = _RetiroSeleccionado.IDDetalleEncargo
                Valor = _RetiroSeleccionado.ValorGMFRetencion
            Else
                IDRetiroSeleccionado = Nothing
            End If
            MyBase.CambioItem("RetiroSeleccionado")
        End Set
    End Property

    Private _IDRetiroSeleccionado As Nullable(Of Integer)
    Public Property IDRetiroSeleccionado() As Nullable(Of Integer)
        Get
            Return _IDRetiroSeleccionado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDRetiroSeleccionado = value
            MyBase.CambioItem("IDRetiroSeleccionado")
        End Set
    End Property

#End Region

#Region "Metodos"

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

    Public Sub ConcatenarConceptoYDetalle(ByVal pstrDetalleConcepto As String, ByVal pstrTextoDigitado As String)
        Try
            If Not String.IsNullOrEmpty(pstrDetalleConcepto) Then
                Dim strConceptoConcatenado As String
                Dim intCantidadPermitida As Integer
                If Len(pstrDetalleConcepto) >= 77 Then
                    DescripcionConceptoCompleta = pstrDetalleConcepto
                Else
                    If Not String.IsNullOrEmpty(pstrTextoDigitado) Then
                        strConceptoConcatenado = pstrDetalleConcepto & "("
                        intCantidadPermitida = 77 - Len(pstrDetalleConcepto)

                        If Len(pstrTextoDigitado) <= intCantidadPermitida Then
                            strConceptoConcatenado += pstrTextoDigitado
                        Else
                            strConceptoConcatenado += pstrTextoDigitado.Substring(0, intCantidadPermitida)
                        End If

                        strConceptoConcatenado += ")"

                        DescripcionConceptoCompleta = strConceptoConcatenado
                    Else
                        DescripcionConceptoCompleta = pstrDetalleConcepto
                    End If
                End If
            Else
                DescripcionConceptoCompleta = pstrTextoDigitado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concetenar el concepto.", _
                                                Me.ToString(), "ConcatenarConceptoYDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub VerificarHabilitarBancos(Optional ByVal plogMostrarMensajeValidacion As Boolean = True)
        Try
            'BABA    'BALANCE X BALANCE
            'BAP     'BALANCE X PYG
            'BB      'BANCO X BANCO
            'BBA     'BANCO X BALANCE
            'BP      'BANCO X PYG
            'PP      'PYG X PYG
            If ManejaDiferido Then
                If (TipoMovimiento = TipoMvtoTesoreria.BB.ToString Or TipoMovimiento = TipoMvtoTesoreria.BBA.ToString Or TipoMovimiento = TipoMvtoTesoreria.BP.ToString) And _
                    (TipoMovimientoDiferido = TipoMvtoTesoreria.BB.ToString Or TipoMovimientoDiferido = TipoMvtoTesoreria.BBA.ToString Or TipoMovimientoDiferido = TipoMvtoTesoreria.BP.ToString) Then
                    If plogMostrarMensajeValidacion Then
                        mostrarMensaje("La configuración de los tipos de movimientos no es valida por favor revise.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            End If

            If String.IsNullOrEmpty(TipoMovimiento) Then
                HabilitarBancoCR = False
                HabilitarBancoDB = False
                IDBancoCR = Nothing
                IDBancoDB = Nothing
                NombreBancoCR = String.Empty
                NombreBancoDB = String.Empty
                If plogMostrarMensajeValidacion Then
                    mostrarMensaje("La concepto no tiene configurado el tipo movimiento por favor revise.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                Exit Sub
            End If

            If ManejaDiferido Then
                If String.IsNullOrEmpty(TipoMovimientoDiferido) Then
                    HabilitarBancoCR = False
                    HabilitarBancoDB = False
                    IDBancoCR = Nothing
                    IDBancoDB = Nothing
                    NombreBancoCR = String.Empty
                    NombreBancoDB = String.Empty
                    If plogMostrarMensajeValidacion Then
                        mostrarMensaje("La concepto no tiene configurado el tipo movimiento diferido por favor revise.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    Exit Sub
                End If
            End If

            If TipoMovimiento = TipoMvtoTesoreria.BB.ToString Or (ManejaDiferido And TipoMovimientoDiferido = TipoMvtoTesoreria.BB.ToString) Then
                HabilitarBancoCR = True
                HabilitarBancoDB = True
            ElseIf (TipoMovimiento = TipoMvtoTesoreria.BABA.ToString Or _
                TipoMovimiento = TipoMvtoTesoreria.BAP.ToString Or _
                TipoMovimiento = TipoMvtoTesoreria.PP.ToString) And _
             (ManejaDiferido = False Or (ManejaDiferido And (TipoMovimiento = TipoMvtoTesoreria.BABA.ToString Or _
                                                             TipoMovimiento = TipoMvtoTesoreria.BAP.ToString Or _
                                                             TipoMovimiento = TipoMvtoTesoreria.PP.ToString))) Then
                HabilitarBancoCR = False
                HabilitarBancoDB = False
                IDBancoCR = Nothing
                IDBancoDB = Nothing
                NombreBancoCR = String.Empty
                NombreBancoDB = String.Empty
            Else
                If TipoMovimiento = TipoMvtoTesoreria.BBA.ToString Or TipoMovimiento = TipoMvtoTesoreria.BP.ToString Then
                    If Not String.IsNullOrEmpty(CuentaContableCR) And String.IsNullOrEmpty(CuentaContableDB) Then
                        HabilitarBancoCR = False
                        HabilitarBancoDB = True
                        IDBancoCR = Nothing
                        NombreBancoCR = String.Empty
                    ElseIf Not String.IsNullOrEmpty(CuentaContableDB) And String.IsNullOrEmpty(CuentaContableCR) Then
                        HabilitarBancoCR = True
                        HabilitarBancoDB = False
                        IDBancoDB = Nothing
                        NombreBancoDB = String.Empty
                    Else
                        HabilitarBancoCR = False
                        HabilitarBancoDB = False
                        IDBancoCR = Nothing
                        IDBancoDB = Nothing
                        NombreBancoCR = String.Empty
                        NombreBancoDB = String.Empty
                        If plogMostrarMensajeValidacion Then
                            mostrarMensaje("La configuración de las cuentas contables del concepto no son validas por favor revise.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                        Exit Sub
                    End If
                End If

                If ManejaDiferido Then
                    If TipoMovimientoDiferido = TipoMvtoTesoreria.BBA.ToString Or TipoMovimientoDiferido = TipoMvtoTesoreria.BP.ToString Then
                        If Not String.IsNullOrEmpty(CuentaContableCRDiferido) And String.IsNullOrEmpty(CuentaContableDBDiferido) Then
                            HabilitarBancoDB = True
                        ElseIf Not String.IsNullOrEmpty(CuentaContableDBDiferido) And String.IsNullOrEmpty(CuentaContableCRDiferido) Then
                            HabilitarBancoCR = True
                        Else
                            HabilitarBancoCR = False
                            HabilitarBancoDB = False
                            IDBancoCR = Nothing
                            IDBancoDB = Nothing
                            NombreBancoCR = String.Empty
                            NombreBancoDB = String.Empty
                            If plogMostrarMensajeValidacion Then
                                mostrarMensaje("La configuración de las cuentas contables diferido del concepto no son validas por favor revise.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                            Exit Sub
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los bancos.", _
                                                Me.ToString(), "VerificarHabilitarBancos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub GenerarNotaContable()
        Try
            IsBusy = True

            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compañia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(NombreConsecutivo) Then
                mostrarMensaje("Debe de seleccionar el consecutivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If IsNothing(IDConcepto) Or IDConcepto = 0 Then
                mostrarMensaje("Debe de seleccionar el concepto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(TipoMovimiento) Then
                mostrarMensaje("El concepto seleccionado no tiene el tipo movimiento configurado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If (NombreConsecutivo = strConsecutivoGMF_Compania Or NombreConsecutivo = strConsecutivoRetencion_Compania) And ManejaDiferido Then
                mostrarMensaje("No se puede seleccionar un concepto que maneje diferido cuando se selcciona un consecutivo de Retención o GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If (NombreConsecutivo = strConsecutivoGMF_Compania Or NombreConsecutivo = strConsecutivoRetencion_Compania) And ManejaNotaCRDB = False Then
                mostrarMensaje("De acuerdo al consecutivo seleccionado, se debe de seleccionar un concepto que Maneje Nota NC/ND.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If ManejaNotaCRDB Then
                If IsNothing(Encargo) Or Encargo = 0 Then
                    mostrarMensaje("Cuando el concepto maneja Nota NC/ND se debe de seleccionar el Encargo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If (NombreConsecutivo = strConsecutivoGMF_Compania Or NombreConsecutivo = strConsecutivoRetencion_Compania) And IsNothing(IDRetiroSeleccionado) Then
                mostrarMensaje("De acuerdo al consecutivo seleccionado, se debe de seleccionar el Tipo Retiro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If ManejaDiferido Then
                If String.IsNullOrEmpty(TipoMovimientoDiferido) Then
                    mostrarMensaje("El concepto seleccionado no tiene el tipo movimiento diferido configurado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
                If IsNothing(FechaInicial) Then
                    mostrarMensaje("La Fecha inicial es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
                If IsNothing(FechaFinal) Then
                    mostrarMensaje("La Fecha final es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
                If String.IsNullOrEmpty(TipoPagoDiferido) Then
                    mostrarMensaje("El Tipo de pago es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
                If FechaFinal < FechaInicial Then
                    mostrarMensaje("La fecha final no puede ser mayor a la fecha inicial.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
            Else
                If IsNothing(FechaInicial) Then
                    mostrarMensaje("La Fecha es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If TipoMovimiento = TipoMvtoTesoreria.BABA.ToString Or TipoMovimiento = TipoMvtoTesoreria.BAP.ToString Or TipoMovimiento = TipoMvtoTesoreria.PP.ToString Then
                If String.IsNullOrEmpty(CuentaContableCR) Or String.IsNullOrEmpty(CuentaContableDB) Then
                    mostrarMensaje("El tipo movimiento del concepto exige que se encuentren matriculadas las dos cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
                If TipoMovimiento = TipoMvtoTesoreria.BABA.ToString Then
                    If CuentaBalanceValida(CuentaContableCR) = False Then
                        mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If CuentaBalanceValida(CuentaContableDB) = False Then
                        mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf TipoMovimiento = TipoMvtoTesoreria.PP.ToString Then
                    If CuentaPYGValida(CuentaContableCR) = False Then
                        mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If CuentaPYGValida(CuentaContableDB) = False Then
                        mostrarMensaje(String.Format("Las dos cuentas contables matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf TipoMovimiento = TipoMvtoTesoreria.BAP.ToString Then
                    If CuentaBalanceValida(CuentaContableCR) = False And CuentaPYGValida(CuentaContableCR) = False Then
                        mostrarMensaje(String.Format("Las cuentas contables matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If CuentaBalanceValida(CuentaContableDB) = False And CuentaPYGValida(CuentaContableDB) = False Then
                        mostrarMensaje(String.Format("Las cuentas contables matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If CuentaBalanceValida(CuentaContableCR) And CuentaBalanceValida(CuentaContableDB) Then
                        mostrarMensaje(String.Format("Una de las cuentas matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If CuentaPYGValida(CuentaContableCR) And CuentaPYGValida(CuentaContableDB) Then
                        mostrarMensaje(String.Format("Una de las cuentas matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            ElseIf TipoMovimiento = TipoMvtoTesoreria.BB.ToString Then
                If IsNothing(IDBancoCR) And IsNothing(IDBancoDB) Then
                    mostrarMensaje("Se deben de seleccionar los dos bancos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
                If IDBancoCR = IDBancoDB Then
                    mostrarMensaje("Los bancos no pueden ser iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
            ElseIf TipoMovimiento = TipoMvtoTesoreria.BBA.ToString Then
                If String.IsNullOrEmpty(CuentaContableCR) And String.IsNullOrEmpty(CuentaContableDB) Then
                    mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas de tipo balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                ElseIf Not String.IsNullOrEmpty(CuentaContableCR) Then
                    If CuentaBalanceValida(CuentaContableCR) = False Then
                        mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If IsNothing(IDBancoDB) Then
                        mostrarMensaje("Se deben de seleccionar el banco débito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf Not String.IsNullOrEmpty(CuentaContableDB) Then
                    If CuentaBalanceValida(CuentaContableDB) = False Then
                        mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If IsNothing(IDBancoCR) Then
                        mostrarMensaje("Se deben de seleccionar el banco crédito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            ElseIf TipoMovimiento = TipoMvtoTesoreria.BP.ToString Then
                If String.IsNullOrEmpty(CuentaContableCR) And String.IsNullOrEmpty(CuentaContableDB) Then
                    mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                ElseIf Not String.IsNullOrEmpty(CuentaContableCR) Then
                    If CuentaPYGValida(CuentaContableCR) = False Then
                        mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If IsNothing(IDBancoDB) Then
                        mostrarMensaje("Se deben de seleccionar el banco débito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf Not String.IsNullOrEmpty(CuentaContableDB) Then
                    If CuentaPYGValida(CuentaContableDB) = False Then
                        mostrarMensaje(String.Format("La cuenta contable matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If IsNothing(IDBancoCR) Then
                        mostrarMensaje("Se deben de seleccionar el banco crédito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            End If

            If ManejaDiferido Then
                If TipoMovimientoDiferido = TipoMvtoTesoreria.BABA.ToString Or TipoMovimientoDiferido = TipoMvtoTesoreria.BAP.ToString Or TipoMovimientoDiferido = TipoMvtoTesoreria.PP.ToString Then
                    If String.IsNullOrEmpty(CuentaContableCRDiferido) Or String.IsNullOrEmpty(CuentaContableDBDiferido) Then
                        mostrarMensaje("El tipo movimiento diferido del concepto exige que se encuentren matriculadas las dos cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                    If TipoMovimientoDiferido = TipoMvtoTesoreria.BABA.ToString Then
                        If CuentaBalanceValida(CuentaContableCRDiferido) = False Then
                            mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaBalanceValida(CuentaContableDBDiferido) = False Then
                            mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo Balance comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf TipoMovimientoDiferido = TipoMvtoTesoreria.PP.ToString Then
                        If CuentaPYGValida(CuentaContableCRDiferido) = False Then
                            mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaPYGValida(CuentaContableDBDiferido) = False Then
                            mostrarMensaje(String.Format("Las dos cuentas contables diferido matriculadas tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf TipoMovimientoDiferido = TipoMvtoTesoreria.BAP.ToString Then
                        If CuentaBalanceValida(CuentaContableCRDiferido) = False And CuentaPYGValida(CuentaContableCRDiferido) = False Then
                            mostrarMensaje(String.Format("Las cuentas contables diferido matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaBalanceValida(CuentaContableDBDiferido) = False And CuentaPYGValida(CuentaContableDBDiferido) = False Then
                            mostrarMensaje(String.Format("Las cuentas contables diferido matriculadas tienen que ser de tipo Balance o PYG, las PYG comienzan con ({0}), balance con ({1}).", strCuentasContable_PYG, strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaBalanceValida(CuentaContableCRDiferido) And CuentaBalanceValida(CuentaContableDBDiferido) Then
                            mostrarMensaje(String.Format("Una de las cuentas diferido matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If CuentaPYGValida(CuentaContableCRDiferido) And CuentaPYGValida(CuentaContableDBDiferido) Then
                            mostrarMensaje(String.Format("Una de las cuentas diferido matriculadas tiene que ser de tipo balance, comienzan con ({0}) y una cuenta PYG comienzan con ({1}).", strCuentasContable_Balance, strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                ElseIf TipoMovimientoDiferido = TipoMvtoTesoreria.BB.ToString Then
                    If IsNothing(IDBancoCR) And IsNothing(IDBancoDB) Then
                        mostrarMensaje("Se deben de seleccionar los dos bancos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                ElseIf TipoMovimientoDiferido = TipoMvtoTesoreria.BBA.ToString Then
                    If String.IsNullOrEmpty(CuentaContableCRDiferido) And String.IsNullOrEmpty(CuentaContableDBDiferido) Then
                        mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas diferido de tipo balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    ElseIf Not String.IsNullOrEmpty(CuentaContableCRDiferido) Then
                        If CuentaBalanceValida(CuentaContableCRDiferido) = False Then
                            mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If IsNothing(IDBancoDB) Then
                            mostrarMensaje("Se deben de seleccionar el banco débito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf Not String.IsNullOrEmpty(CuentaContableDBDiferido) Then
                        If CuentaBalanceValida(CuentaContableDBDiferido) = False Then
                            mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo Balance, comienzan con ({0}).", strCuentasContable_Balance), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If IsNothing(IDBancoCR) Then
                            mostrarMensaje("Se deben de seleccionar el banco crédito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                ElseIf TipoMovimientoDiferido = TipoMvtoTesoreria.BP.ToString Then
                    If String.IsNullOrEmpty(CuentaContableCRDiferido) And String.IsNullOrEmpty(CuentaContableDBDiferido) Then
                        mostrarMensaje(String.Format("El concepto debe de tener matriculado una de las dos cuentas diferido de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    ElseIf Not String.IsNullOrEmpty(CuentaContableCRDiferido) Then
                        If CuentaPYGValida(CuentaContableCRDiferido) = False Then
                            mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If IsNothing(IDBancoDB) Then
                            mostrarMensaje("Se deben de seleccionar el banco débito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    ElseIf Not String.IsNullOrEmpty(CuentaContableDBDiferido) Then
                        If CuentaPYGValida(CuentaContableDBDiferido) = False Then
                            mostrarMensaje(String.Format("La cuenta contable diferido matriculada tienen que ser de tipo PYG, comienzan con ({0}).", strCuentasContable_PYG), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                        If IsNothing(IDBancoCR) Then
                            mostrarMensaje("Se deben de seleccionar el banco crédito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                End If
            End If

            If (TipoNitDB = "TE" Or TipoNitCR = "TE") And String.IsNullOrEmpty(Nit) Then
                mostrarMensaje("De acuerdo a la configuración del concepto se debe de seleccionar el Nit.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If IsNothing(Valor) Or Valor = 0 Then
                mostrarMensaje("Debe de seleccionar el valor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            dcProxy.RespuestaValidacionEstadoDocumentos.Clear()

            dcProxy.Load(dcProxy.Tesoreria_GenerarNotaContableFondosQuery(IDCompania, NombreConsecutivo, IDConcepto, DetalleConcepto, TipoMovimiento, Retencion,
                                                                          CuentaContableCR, CuentaContableDB, IDBancoCR, IDBancoDB, Nit, CentroCostos,
                                                                          FechaInicial, FechaFinal, ManejaDiferido, TipoPagoDiferido, TipoMovimientoDiferido,
                                                                          CuentaContableCRDiferido, CuentaContableDBDiferido, Valor, CodigoOYD, Encargo,
                                                                          FechaRetiro, IDRetiroSeleccionado, ManejaNotaCRDB, IDNotaCRDB, Program.Usuario, Program.HashConexion),
                                                                      AddressOf TerminoGenerarNotaContable, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "GenerarNotaContable", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoGenerarNotaContable(ByVal lo As LoadOperation(Of RespuestaValidacionEstadoDocumento))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strMensaje As String = String.Empty

                    If lo.Entities.First.Exitoso Then
                        For Each li In lo.Entities.Where(Function(i) i.Exitoso)
                            If String.IsNullOrEmpty(strMensaje) Then
                                strMensaje = li.Mensaje
                            Else
                                strMensaje = String.Format("{0}{1}{2}", strMensaje, vbCrLf, li.Mensaje)
                            End If
                        Next
                        mostrarMensajeResultadoAsincronico(strMensaje, Program.TituloSistema, AddressOf TerminoMostrarMensajeUsuario, "TERMINOGENERAR", A2Utilidades.wppMensajes.TiposMensaje.Exito)

                        If Not String.IsNullOrEmpty(mstrArchivo) Then
                            viewGenerarNotas.ctlSubirArchivo.ClaveUnica = lo.Entities.First.ID

                            If String.IsNullOrEmpty(_mstrRuta) Then
                                viewGenerarNotas.ctlSubirArchivo.subirArchivo(mstrArchivo, mabytArchivo)
                            Else
                                viewGenerarNotas.ctlSubirArchivo.subirArchivo(mstrArchivo, mstrRuta)
                            End If
                        End If
                    Else
                        For Each li In lo.Entities.Where(Function(i) i.Exitoso = False)
                            If String.IsNullOrEmpty(strMensaje) Then
                                strMensaje = li.Mensaje
                            Else
                                strMensaje = String.Format("{0}{1}{2}", strMensaje, vbCrLf, li.Mensaje)
                            End If
                        Next
                        mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
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

    Private Sub TerminoMostrarMensajeUsuario(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Valor = 0
            mstrArchivo = String.Empty
            mstrRuta = String.Empty
            mabytArchivo = Nothing
            viewGenerarNotas.ctlSubirArchivo.inicializarControl()
            viewGenerarNotas.ConfigurarSubirDocumento()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                               Me.ToString(), "TerminoMostrarMensajeUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarCombosCompania()
        Try
            If IDCompania > 0 Then
                IsBusy = True
                objProxy.ItemCombos.Clear()
                objProxy.Load(objProxy.cargarCombosCondicionalQuery("CONSECUTIVOS_FIRMA_COMPANIA", IDCompania, IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombos, String.Empty)
            Else
                ListaConsecutivos = Nothing
                ListaTipoPago = Nothing
                HabilitarConsecutivo = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los combos.", _
                                                Me.ToString(), "ConsultarCombosCompania", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Async Function ConsultarFechaDefectoCompania() As Task
        Try
            If IDCompania > 0 Then
                IsBusy = True

                Dim objRet As InvokeOperation(Of Nullable(Of Date))
                Dim objProxy As UtilidadesDomainContext

                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    objProxy = New UtilidadesDomainContext()
                Else
                    objProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                End If

                'Se realiza para aumentar el tiempo de consulta de ria y evitar el timeup en algunas consultas
                DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                ErrorForma = String.Empty

                objRet = Await objProxy.consultarFechaHabilPosteriorCierreCompaniaSync(IDCompania, Program.Usuario, Program.HashConexion).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "ObtenerCalculosMotor", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        FechaInicial = objRet.Value
                        FechaRetiro = objRet.Value
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre de la compania.", _
                                                Me.ToString(), "ConsultarFechaDefectoCompania", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Function

    Private Async Sub TerminoConsultarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
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

                If objDiccionario.ContainsKey("NOTAS_COMPANIA") Then
                    objListaNotas = objDiccionario("NOTAS_COMPANIA")
                End If

                If objDiccionario.ContainsKey("TIPOPAGO") Then
                    objListaTipoPago = objDiccionario("TIPOPAGO")
                End If

                If objDiccionario.ContainsKey("CONSECUTIVOGMF_COMPANIA") Then
                    strConsecutivoGMF_Compania = objDiccionario("CONSECUTIVOGMF_COMPANIA").First.ID
                End If
                If objDiccionario.ContainsKey("CONSECUTIVORETENCION_COMPANIA") Then
                    strConsecutivoRetencion_Compania = objDiccionario("CONSECUTIVORETENCION_COMPANIA").First.ID
                End If

                ListaConsecutivos = objListaNotas
                ListaTipoPago = objListaTipoPago

                If ListaConsecutivos.Count > 0 Then
                    If ListaConsecutivos.Count = 1 Then
                        NombreConsecutivo = ListaConsecutivos.First.ID
                    End If
                    HabilitarConsecutivo = True

                    TipoPagoDiferido = "I"
                    Await ConsultarFechaDefectoCompania()
                Else
                    HabilitarConsecutivo = False
                    mostrarMensaje("La compañia no tiene consecutivos de notas configurados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
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

    Public Sub LimpiarInformacion()
        Try
            IDCompania = Nothing
            NombreCompania = String.Empty
            Nit = String.Empty
            CentroCostos = String.Empty
            Valor = 0
            mstrArchivo = String.Empty
            mstrRuta = String.Empty
            mabytArchivo = Nothing
            CodigoOYD = String.Empty
            Encargo = Nothing
            IDRetiroSeleccionado = Nothing
            RetiroSeleccionado = Nothing

            LimpiarCampos("COMPANIA")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar la información.", _
                                               Me.ToString(), "LimpiarInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarCampos(ByVal pstrTipoRegistro As String)
        Try
            If pstrTipoRegistro = "COMPANIA" Then
                NombreConsecutivo = String.Empty
                IDConcepto = Nothing
                CuentaContableDB = String.Empty
                CuentaContableCR = String.Empty
                TipoMovimiento = String.Empty
                DescripcionTipoMovimiento = String.Empty
                Retencion = String.Empty
                DescripcionRetencion = String.Empty
                DetalleConcepto = String.Empty
                IDBancoCR = Nothing
                IDBancoDB = Nothing
                NombreBancoCR = String.Empty
                NombreBancoDB = String.Empty
                If IsNothing(IDCompania) Or IDCompania = 0 Then
                    HabilitarConsecutivo = True
                Else
                    HabilitarConsecutivo = False
                End If
                HabilitarConcepto = False
                HabilitarBancoCR = False
                HabilitarBancoDB = False
                ManejaDiferido = False
                ManejaNotaCRDB = False
                IDNotaCRDB = String.Empty
                DescripcionNotaCRDB = String.Empty
                VerificarHabilitarCodigoOYD()
            ElseIf pstrTipoRegistro = "CONSECUTIVO" Then
                IDConcepto = Nothing
                CuentaContableDB = String.Empty
                CuentaContableCR = String.Empty
                TipoMovimiento = String.Empty
                DescripcionTipoMovimiento = String.Empty
                Retencion = String.Empty
                DescripcionRetencion = String.Empty
                DetalleConcepto = String.Empty
                IDBancoCR = Nothing
                IDBancoDB = Nothing
                NombreBancoCR = String.Empty
                NombreBancoDB = String.Empty
                If Not String.IsNullOrEmpty(NombreConsecutivo) Then
                    HabilitarConcepto = True
                Else
                    HabilitarConcepto = False
                End If

                ManejaNotaCRDB = False
                IDNotaCRDB = String.Empty
                DescripcionNotaCRDB = String.Empty
                HabilitarBancoCR = False
                HabilitarBancoDB = False
                ManejaDiferido = False
                VerificarHabilitarCodigoOYD()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarCampos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosCompania(ByVal pstrIDCompania As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrIDCompania, "compania", "A", "incluircompaniasclasestodas", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCompania, pstrIDCompania)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarCompania()
        Try
            IDCompania = Nothing
            NombreCompania = String.Empty
            LimpiarCampos("COMPANIA")
            ConsultarCombosCompania()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarCompania", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarCompania(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        IDCompania = CInt(lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem)
                        NombreCompania = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.Nombre

                        LimpiarCampos("COMPANIA")
                        ConsultarCombosCompania()
                    Else
                        mostrarMensaje("La compañia no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDCompania = Nothing
                        NombreCompania = String.Empty
                        LimpiarCampos("COMPANIA")
                        ConsultarCombosCompania()
                    End If
                Else
                    mostrarMensaje("La compañia no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDCompania = Nothing
                    NombreCompania = String.Empty
                    LimpiarCampos("COMPANIA")
                    ConsultarCombosCompania()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub ConsultarDatosBancoCR(ByVal pstrIDBanco As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrIDBanco, "CuentasBancarias", "A", String.Empty, IDCompania, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarBancoCR, pstrIDBanco)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarBancoCR()
        Try
            IDBancoCR = Nothing
            NombreBancoCR = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarBancoCR(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        IDBancoCR = CInt(lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem)
                        NombreBancoCR = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.Nombre
                    Else
                        mostrarMensaje("El banco no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDBancoCR = Nothing
                        NombreBancoCR = String.Empty
                    End If
                Else
                    mostrarMensaje("El banco no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDBancoCR = Nothing
                    NombreBancoCR = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBancoCR", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBancoCR", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub ConsultarDatosBancoDB(ByVal pstrIDBanco As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrIDBanco, "CuentasBancarias", "A", String.Empty, IDCompania, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarBancoDB, pstrIDBanco)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarBancoDB()
        Try
            IDBancoDB = Nothing
            NombreBancoDB = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarBancoDB", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarBancoDB(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        IDBancoDB = CInt(lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem)
                        NombreBancoDB = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.Nombre
                    Else
                        mostrarMensaje("El banco no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDBancoDB = Nothing
                        NombreBancoDB = String.Empty
                    End If
                Else
                    mostrarMensaje("El banco no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDBancoDB = Nothing
                    NombreBancoDB = String.Empty
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBancoDB", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarBancoDB", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub ConsultarDatosNit(ByVal pstrNit As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrNit, "NITS", "A", String.Empty, String.Empty, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarNit, pstrNit)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarNit()
        Try
            Nit = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarNit", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarNit(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.CodItem = lo.UserState.ToString).Count > 0 Then
                        Nit = lo.Entities.Where(Function(i) i.CodItem = lo.UserState.ToString).First.CodItem
                    Else
                        mostrarMensaje("El nit no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Nit = Nothing
                    End If
                Else
                    mostrarMensaje("El nit no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Nit = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarNit", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarNit", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub ConsultarDatosCentroCostos(ByVal pstrCentroCosto As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrCentroCosto, "CentrosCosto", "A", String.Empty, String.Empty, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCentroCostos, pstrCentroCosto)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarCentroCosto()
        Try
            CentroCostos = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarCentroCosto", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarCentroCostos(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        CentroCostos = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First.IdItem
                    Else
                        mostrarMensaje("El centro de costos no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        CentroCostos = Nothing
                    End If
                Else
                    mostrarMensaje("El centro de costos no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CentroCostos = Nothing
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCentroCostos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCentroCostos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub ConsultarDatosConcepto(ByVal pstrIDConcepto As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrIDConcepto, "ConceptoTesoNotas", "A", NombreConsecutivo, String.Empty, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarConcepto, pstrIDConcepto)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarConcepto()
        Try
            IDConcepto = Nothing
            CuentaContableDB = String.Empty
            CuentaContableCR = String.Empty
            TipoMovimiento = String.Empty
            DescripcionTipoMovimiento = String.Empty
            Retencion = String.Empty
            DescripcionRetencion = String.Empty
            DetalleConcepto = String.Empty
            viewGenerarNotas.CantidadMaximaParaTextoDigitado(String.Empty)
            HabilitarBancoCR = False
            HabilitarBancoDB = False
            IDBancoCR = Nothing
            IDBancoDB = Nothing
            NombreBancoCR = String.Empty
            NombreBancoDB = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarConcepto", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarConcepto(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).Count > 0 Then
                        Dim pobjItem = lo.Entities.Where(Function(i) i.IdItem = lo.UserState.ToString).First

                        IDConcepto = pobjItem.IdItem
                        If pobjItem.InfoAdicional07 <> TipoMvtoTesoreria.BB.ToString Then
                            CuentaContableDB = pobjItem.InfoAdicional03
                            CuentaContableCR = pobjItem.InfoAdicional04
                        Else
                            CuentaContableDB = String.Empty
                            CuentaContableCR = String.Empty
                        End If

                        TipoMovimiento = pobjItem.InfoAdicional07
                        DescripcionTipoMovimiento = pobjItem.InfoAdicional08
                        Retencion = pobjItem.InfoAdicional09
                        DescripcionRetencion = pobjItem.InfoAdicional10
                        DetalleConcepto = pobjItem.Nombre
                        'Nit = pobjItem.InfoAdicional02

                        If pobjItem.InfoAdicional11 = "1" Then
                            ManejaDiferido = True
                        Else
                            ManejaDiferido = False
                        End If

                        TipoMovimientoDiferido = pobjItem.InfoAdicional12
                        DescripcionTipoMovimientoDiferido = pobjItem.InfoAdicional13

                        If pobjItem.InfoAdicional12 <> TipoMvtoTesoreria.BB.ToString Then
                            CuentaContableCRDiferido = pobjItem.InfoAdicional14
                            CuentaContableDBDiferido = pobjItem.InfoAdicional15
                        Else
                            CuentaContableCRDiferido = String.Empty
                            CuentaContableDBDiferido = String.Empty
                        End If

                        TipoNitDB = pobjItem.InfoAdicional16
                        TipoNitCR = pobjItem.InfoAdicional17

                        If TipoNitDB = "TE" Or TipoNitCR = "TE" Then
                            HabilitarSeleccionNit = True
                        Else
                            HabilitarSeleccionNit = False
                            Nit = String.Empty
                        End If

                        ManejaNotaCRDB = IIf(pobjItem.InfoAdicional18 = "1", True, False)
                        IDNotaCRDB = pobjItem.InfoAdicional19
                        DescripcionNotaCRDB = pobjItem.InfoAdicional20

                        viewGenerarNotas.CantidadMaximaParaTextoDigitado(pobjItem.Nombre)
                        VerificarHabilitarBancos()
                        VerificarHabilitarCodigoOYD()
                        VerificarConceptoVSConsecutvio()
                    Else
                        mostrarMensaje("El concepto no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IDConcepto = Nothing
                        CuentaContableDB = String.Empty
                        CuentaContableCR = String.Empty
                        TipoMovimiento = String.Empty
                        DescripcionTipoMovimiento = String.Empty
                        Retencion = String.Empty
                        DescripcionRetencion = String.Empty
                        DetalleConcepto = String.Empty
                        viewGenerarNotas.CantidadMaximaParaTextoDigitado(String.Empty)
                        HabilitarBancoCR = False
                        HabilitarBancoDB = False
                        IDBancoCR = Nothing
                        IDBancoDB = Nothing
                        NombreBancoCR = String.Empty
                        NombreBancoDB = String.Empty
                        TipoNitDB = String.Empty
                        TipoNitCR = String.Empty
                        ManejaDiferido = False
                        ManejaNotaCRDB = False
                        IDNotaCRDB = String.Empty
                        DescripcionNotaCRDB = String.Empty
                        VerificarHabilitarCodigoOYD()
                    End If
                Else
                    mostrarMensaje("El concepto no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IDConcepto = Nothing
                    CuentaContableDB = String.Empty
                    CuentaContableCR = String.Empty
                    TipoMovimiento = String.Empty
                    DescripcionTipoMovimiento = String.Empty
                    Retencion = String.Empty
                    DescripcionRetencion = String.Empty
                    DetalleConcepto = String.Empty
                    viewGenerarNotas.CantidadMaximaParaTextoDigitado(String.Empty)
                    HabilitarBancoCR = False
                    HabilitarBancoDB = False
                    IDBancoCR = Nothing
                    IDBancoDB = Nothing
                    NombreBancoCR = String.Empty
                    NombreBancoDB = String.Empty
                    TipoNitDB = String.Empty
                    TipoNitCR = String.Empty
                    ManejaDiferido = False
                    ManejaNotaCRDB = False
                    IDNotaCRDB = String.Empty
                    DescripcionNotaCRDB = String.Empty
                    VerificarHabilitarCodigoOYD()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCentroCostos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCentroCostos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub AbrirConsultarDocumentosPendientes()
        Try
            If IsNothing(IDCompania) Or IDCompania = 0 Then
                mostrarMensaje("Debe de seleccionar la compañia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            Dim objDocumentosPendientesCancelar As New GenerarNotasContablesFondos_CancelacionView(Me)
            Program.Modal_OwnerMainWindowsPrincipal(objDocumentosPendientesCancelar)
            objDocumentosPendientesCancelar.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                               Me.ToString(), "AbrirConsultarDocumentosPendientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarNotaContablesPendientes()
        Try
            IsBusy = True

            dcProxy.GenerarNotasFondosCancelacions.Clear()

            dcProxy.Load(dcProxy.Tesoreria_GenerarNotaContableFondos_ConsultarPendientesQuery(TipoConsulta, IDCompania, Program.Usuario, Program.Maquina, Program.HashConexion),
                                                                      AddressOf TerminoConsultarNotaContablesPendientes, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "ConsultarNotaContablesPendientes", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarNotaContablesPendientes(ByVal lo As LoadOperation(Of GenerarNotasFondosCancelacion))
        Try
            If lo.HasError = False Then
                ListaNotasCancelar = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "TerminoConsultarNotaContablesPendientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "TerminoConsultarNotaContablesPendientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Public Sub AbrirDocumentoAdjunto()
        Try
            If Not IsNothing(NotaCancelar) Then
                If Not String.IsNullOrEmpty(NotaCancelar.Adjunto) Then
                    Program.VisorArchivosWeb_DescargarURL(NotaCancelar.Adjunto)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "AbrirDocumentoAdjunto", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub SeleccionarTodosRegistrosCancelar(ByVal plogSeleccionarTodos As Boolean)
        Try
            If Not IsNothing(ListaNotasCancelar) Then
                For Each li In ListaNotasCancelar
                    li.Cancelar = plogSeleccionarTodos
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "SeleccionarTodosRegistrosCancelar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub CancelarNotasContables()
        Try
            IsBusy = True

            If IsNothing(_ListaNotasCancelar) Then
                mostrarMensaje("Debe de seleccionar al menos un registro a cancelar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If _ListaNotasCancelar.Where(Function(i) i.Cancelar).Count = 0 Then
                mostrarMensaje("Debe de seleccionar al menos un registro a cancelar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            Dim strRegistrosGenerar As String = String.Empty

            For Each li In _ListaNotasCancelar.Where(Function(i) i.Cancelar)
                If String.IsNullOrEmpty(strRegistrosGenerar) Then
                    strRegistrosGenerar = li.ID
                Else
                    strRegistrosGenerar = String.Format("{0}|{1}", strRegistrosGenerar, li.ID)
                End If
            Next

            dcProxy.RespuestaValidacionEstadoDocumentos.Clear()

            dcProxy.Load(dcProxy.Tesoreria_GenerarNotaContableFondos_CancelarPendientesQuery(TipoConsulta, strRegistrosGenerar, Program.Usuario, Program.Maquina, Program.HashConexion),
                         AddressOf TerminoCancelarNotaContable, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "GenerarNotaContable", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCancelarNotaContable(ByVal lo As LoadOperation(Of RespuestaValidacionEstadoDocumento))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then

                    If lo.Entities.First.Exitoso Then
                        Dim objListaMensajes As New List(Of String)
                        Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                        For Each li In lo.Entities.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes

                        objViewImportarArchivo.Title = "Generar notas fondos"
                        Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                        objViewImportarArchivo.ShowDialog()

                        ConsultarNotaContablesPendientes()
                    Else
                        Dim strMensaje As String = String.Empty
                        For Each li In lo.Entities.Where(Function(i) i.Exitoso = False)
                            If String.IsNullOrEmpty(strMensaje) Then
                                strMensaje = li.Mensaje
                            Else
                                strMensaje = String.Format("{0}{1}{2}", strMensaje, vbCrLf, li.Mensaje)
                            End If
                        Next
                        mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "TerminoGenerarNotaContable", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar la nota contable.", _
                                                Me.ToString(), "TerminoGenerarNotaContable", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ConsultarDatosCodigoOYD(ByVal pstrCodigoOYD As String)
        Try
            If Not IsNothing(objProxy.BuscadorGenericos) Then
                objProxy.BuscadorGenericos.Clear()
            End If

            objProxy.Load(objProxy.buscarItemsQuery(pstrCodigoOYD, "codigosoydcompania", "A", String.Empty, IDCompania, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCodigoOYD, pstrCodigoOYD)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosCompania. ", Me.ToString(), "ConsultarDatosBancoCR", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarCodigoOYOD()
        Try
            CodigoOYD = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los campos.", _
                                                Me.ToString(), "LimpiarCodigoOYOD", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarCodigoOYD(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.Where(Function(i) Trim(i.IdItem) = lo.UserState.ToString).Count > 0 Then
                        CodigoOYD = lo.Entities.Where(Function(i) Trim(i.IdItem) = lo.UserState.ToString).First.IdItem
                        If HabilitarSeleccionNit Then
                            Nit = lo.Entities.Where(Function(i) Trim(i.IdItem) = lo.UserState.ToString).First.InfoAdicional03
                        End If
                        ConsultarCombosEncargos()
                    Else
                        mostrarMensaje("El Código OYD no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCodigoOYOD()
                    End If
                Else
                    mostrarMensaje("El Código OYD no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCodigoOYOD()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCodigoOYD", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCodigoOYD", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub VerificarHabilitarCodigoOYD()
        Try
            If Not String.IsNullOrEmpty(NombreConsecutivo) Then
                If NombreConsecutivo = strConsecutivoGMF_Compania Or NombreConsecutivo = strConsecutivoRetencion_Compania Then
                    MostrarCamposCodigoOYD = Visibility.Visible
                    MostrarCamposRetiros = Visibility.Visible
                    MostrarCamposDiferido = Visibility.Collapsed
                Else
                    MostrarCamposRetiros = Visibility.Collapsed
                    IDRetiroSeleccionado = Nothing
                    RetiroSeleccionado = Nothing
                    ListaRetiros = Nothing

                    If ManejaDiferido Then
                        MostrarCamposCodigoOYD = Visibility.Collapsed
                        MostrarCamposDiferido = Visibility.Visible

                        DescripcionFechaInicial = "Fecha inicial"
                        FechaFinal = FechaInicial

                        CodigoOYD = String.Empty
                        Encargo = Nothing
                        ListaEncargos = Nothing
                    Else
                        If ManejaNotaCRDB Then
                            MostrarCamposCodigoOYD = Visibility.Visible
                        Else
                            MostrarCamposCodigoOYD = Visibility.Collapsed

                            CodigoOYD = String.Empty
                            Encargo = Nothing
                            ListaEncargos = Nothing
                        End If

                        MostrarCamposDiferido = Visibility.Collapsed

                        DescripcionFechaInicial = "Fecha"
                        FechaFinal = Nothing
                    End If
                End If
            Else
                MostrarCamposCodigoOYD = Visibility.Collapsed
                MostrarCamposDiferido = Visibility.Collapsed
                MostrarCamposRetiros = Visibility.Collapsed
                IDRetiroSeleccionado = Nothing
                RetiroSeleccionado = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los campos.", _
                                                Me.ToString(), "VerificarHabilitarCodigoOYD", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerificarConceptoVSConsecutvio()
        Try
            If Not String.IsNullOrEmpty(NombreConsecutivo) Then
                If (NombreConsecutivo = strConsecutivoGMF_Compania Or NombreConsecutivo = strConsecutivoRetencion_Compania) And ManejaDiferido Then
                    mostrarMensaje("No se puede seleccionar un concepto que maneje diferido cuando se selcciona un consecutivo de Retención o GMF.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los campos.", _
                                                Me.ToString(), "VerificarConceptoVSConsecutvio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarCombosEncargos()
        Try
            If IDCompania > 0 And Not String.IsNullOrEmpty(CodigoOYD) Then
                IsBusy = True
                objProxy.ItemCombos.Clear()
                objProxy.Load(objProxy.cargarCombosCondicionalQuery("ENCARGOSCOMITENTE", CodigoOYD, IDCompania, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEncargos, String.Empty)
            Else
                ListaEncargos = Nothing
                Encargo = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los combos.", _
                                                Me.ToString(), "ConsultarCombosMovimientosRetiro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarEncargos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                ListaEncargos = lo.Entities.ToList

                If lo.Entities.Count > 0 Then
                    If ListaEncargos.Count = 1 Then
                        Encargo = ListaEncargos.First.intID
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Public Sub ConsultarCombosMovimientosRetiro()
        Try
            If IDCompania > 0 And Not String.IsNullOrEmpty(CodigoOYD) And Encargo > 0 And Not IsNothing(FechaRetiro) And Not String.IsNullOrEmpty(NombreConsecutivo) Then
                IsBusy = True
                dcProxy.GenerarNotasFondos_Retiros.Clear()
                dcProxy.Load(dcProxy.Tesoreria_GenerarNotaContableFondos_RetirosQuery(IDCompania, Encargo, CodigoOYD, NombreConsecutivo, FechaRetiro, FechaRetiro, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarMovimientosRetiro, String.Empty)
            Else
                ListaRetiros = Nothing
                RetiroSeleccionado = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los combos.", _
                                                Me.ToString(), "ConsultarCombosMovimientosRetiro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarMovimientosRetiro(ByVal lo As LoadOperation(Of OyDTesoreria.GenerarNotasFondos_Retiros))
        Try
            If lo.HasError = False Then
                ListaRetiros = lo.Entities.ToList

                If lo.Entities.Count > 0 Then
                    If ListaRetiros.Count = 1 Then
                        RetiroSeleccionado = ListaRetiros.First
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarCombosCondicional", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

#End Region

End Class