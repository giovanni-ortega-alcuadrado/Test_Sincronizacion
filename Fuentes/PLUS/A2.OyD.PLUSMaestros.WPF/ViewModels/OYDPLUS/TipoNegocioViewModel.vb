Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TipoNegocioViewModel.vb
'Generado el : 11/07/2012 18:36:05
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class TipoNegocioViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private TipoNegociPorDefecto As TipoNegocio
    Private TipoNegociAnterior As TipoNegocio
    Private CertificacionXTipoNegociPorDefecto As CertificacionXTipoNegocio
    Private DocumentoXTipoNegociPorDefecto As DocumentoXTipoNegocio
    Private TipoNegocioXEspeciPorDefecto As TipoNegocioXEspecie
    Private TipoNegocioXTipoProductPorDefecto As TipoNegocioXTipoProducto
    Private TipoNegocioXDistribucionPorDefecto As DistribucionComisionXTipoNegocio

    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            If Not Program.IsDesignMode() Then

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "TipoNegocioViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Inicializar()
        Try
            IsBusy = True
            dcProxy.Load(dcProxy.TipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocio, "")

            dcProxy1.Load(dcProxy1.TraerTipoNegociPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioPorDefecto_Completed, "Default")
            dcProxy1.Load(dcProxy1.TraerCertificacionXTipoNegociPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCertificacionXTipoNegocioPorDefecto_Completed, "Default")
            dcProxy1.Load(dcProxy1.TraerDocumentoXTipoNegociPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDocumentoXTipoNegocioPorDefecto_Completed, "Default")
            dcProxy1.Load(dcProxy1.TraerTipoNegocioXEspeciPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXEspeciePorDefecto_Completed, "Default")
            dcProxy1.Load(dcProxy1.TraerTipoNegocioXTipoProductPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXTipoProductoPorDefecto_Completed, "Default")
            dcProxy1.Load(dcProxy1.TraerDistribucionPortafolioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXDistribucionPorDefecto_Completed, "Default")
            dcProxy1.Load(dcProxy1.DoctosRequeridosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDoctosRequeridos, "Default")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "TipoNegocioViewModel.Inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTipoNegocioPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoNegocio))
        Try
            If Not lo.HasError Then
                TipoNegociPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoNegoci por defecto",
                                                 Me.ToString(), "TerminoTraerTipoNegociPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoNegoci por defecto",
                                                 Me.ToString(), "TerminoTraerTipoNegociPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerTipoNegocio(ByVal lo As LoadOperation(Of TipoNegocio))
        Try
            If Not lo.HasError Then
                ListaTipoNegocioCompleta = dcProxy.TipoNegocios.ToList
                ListaTipoNegocio = dcProxy.TipoNegocios

                If dcProxy.TipoNegocios.Count > 0 Then
                    If lo.UserState = "insert" Then
                        TipoNegociSelected = ListaTipoNegocio.Last
                    End If
                Else
                    If lo.UserState = "Busqueda" Then
                        mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocio",
                                                 Me.ToString(), "TerminoTraerTipoNegoci", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocio",
                                                 Me.ToString(), "TerminoTraerTipoNegoci", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoTraerDoctosRequeridos(ByVal lo As LoadOperation(Of DoctosRequerido))
        Try
            If lo.HasError = False Then
                ListaDocRequeridos = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los documentos requeridos",
                                             Me.ToString(), "TerminoTraerDoctosRequeridos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los documentos requeridos",
                                             Me.ToString(), "TerminoTraerDoctosRequeridos", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

#End Region

#Region "Propiedades"
    Private _HabilitarTipoOperacion As Boolean = False
    Public Property HabilitarTipoOperacion() As Boolean
        Get
            Return _HabilitarTipoOperacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoOperacion = value
            MyBase.CambioItem("HabilitarTipoOperacion")
        End Set
    End Property



    Private _DiccionarioCombosCompletos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosCompletos() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosCompletos
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosCompletos = value
            MyBase.CambioItem("DiccionarioCombosCompletos")
        End Set
    End Property
    Private _DiccionarioCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombos() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombos
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombos = value
            MyBase.CambioItem("DiccionarioCombos")
        End Set
    End Property

    Private _ListaTipoNegocio As EntitySet(Of TipoNegocio)
    Public Property ListaTipoNegocio() As EntitySet(Of TipoNegocio)
        Get
            Return _ListaTipoNegocio
        End Get
        Set(ByVal value As EntitySet(Of TipoNegocio))
            _ListaTipoNegocio = value

            MyBase.CambioItem("ListaTipoNegocio")
            MyBase.CambioItem("ListaTipoNegocioPaged")
            If Not IsNothing(_ListaTipoNegocio) Then
                If _ListaTipoNegocio.Count > 0 Then
                    TipoNegociSelected = _ListaTipoNegocio.FirstOrDefault
                Else
                    TipoNegociSelected = Nothing
                End If
            Else
                TipoNegociSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTipoNegocioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoNegocio) Then
                Dim view = New PagedCollectionView(_ListaTipoNegocio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoNegociSelected As TipoNegocio
    Public Property TipoNegociSelected() As TipoNegocio
        Get
            Return _TipoNegociSelected
        End Get
        Set(ByVal value As TipoNegocio)
            _TipoNegociSelected = value
            MyBase.CambioItem("TipoNegociSelected")
            If Not IsNothing(_TipoNegociSelected) Then
                'Valida la clase del tipo de negocio
                If _TipoNegociSelected.Codigo = "C" Or _TipoNegociSelected.Codigo = "S" Or _TipoNegociSelected.Codigo = "CO" Or
                    _TipoNegociSelected.Codigo = "RC" Or _TipoNegociSelected.Codigo = "TTVC" Then
                    ClaseTipoNegocio = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                ElseIf _TipoNegociSelected.Codigo = "A" Or _TipoNegociSelected.Codigo = "R" Or
                    _TipoNegociSelected.Codigo = "AO" Or _TipoNegociSelected.Codigo = "TTV" Then
                    ClaseTipoNegocio = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                Else
                    ClaseTipoNegocio = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                End If

                ObtenerValoresCombos()


                'Consulta los detalles del tipo de negocio
                Dim strCodigo As String = _TipoNegociSelected.Codigo
                ConsultarCertificaciones_TipoNegocio(strCodigo)

                ConsultarDocumentos_TipoNegocio(strCodigo)

                ConsultarEspecie_TipoNegocio(strCodigo)

                ConsultarTipoProducto_TipoNegocio(strCodigo)

                ConsultarDistribucionComision_TipoNegocio(strCodigo)

                TabSeleccionado = 0
            End If
        End Set
    End Property

    Private _cb As CamposBusquedaTipoNegocio
    Public Property cb() As CamposBusquedaTipoNegocio
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaTipoNegocio)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _ListaTipoNegocioCompleta As List(Of TipoNegocio)
    Public Property ListaTipoNegocioCompleta() As List(Of TipoNegocio)
        Get
            Return _ListaTipoNegocioCompleta
        End Get
        Set(ByVal value As List(Of TipoNegocio))
            _ListaTipoNegocioCompleta = value
            MyBase.CambioItem("ListaTipoNegocioCompleta")
        End Set
    End Property

    Private _ListaDocRequeridos As List(Of DoctosRequerido)
    Public Property ListaDocRequeridos() As List(Of DoctosRequerido)
        Get
            Return _ListaDocRequeridos
        End Get
        Set(ByVal value As List(Of DoctosRequerido))
            _ListaDocRequeridos = value
            MyBase.CambioItem("ListaDocRequeridos")
        End Set
    End Property

    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado() As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _ClaseTipoNegocio As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
    Public Property ClaseTipoNegocio() As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie
        Get
            Return _ClaseTipoNegocio
        End Get
        Set(ByVal value As A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie)
            _ClaseTipoNegocio = value
            MyBase.CambioItem("ClaseTipoNegocio")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub ObtenerValoresCombos()
        Try
            Dim objDiccionario As New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
            Dim objListaCategoria As New List(Of OYDUtilidades.ItemCombo)
            If Editando Then
                HabilitarTipoOperacion = True
            End If

            If Not IsNothing(DiccionarioCombosCompletos) Then
                If _TipoNegociSelected.Codigo.ToUpper = "R" Or _TipoNegociSelected.Codigo.ToUpper = "RC" Then
                    For Each li In DiccionarioCombosCompletos.Where(Function(i) i.Key = "TIPONEGOCIO_TIPOOPERACIONREPO_CVRS")
                        objDiccionario.Add("TIPOOPERACIONDOC", li.Value)
                    Next
                ElseIf _TipoNegociSelected.Codigo.ToUpper = "TTV" Or _TipoNegociSelected.Codigo.ToUpper = "TTVC" Then
                    'For Each li In DiccionarioCombosCompletos.Where(Function(i) i.Key = "TIPONEGOCIO_TIPOOPERACIONDOC_OR")
                    '    objDiccionario.Add("TIPOOPERACIONDOC", li.Value)
                    'Next
                    For Each li In DiccionarioCombosCompletos.Where(Function(i) i.Key = "TIPONEGOCIO_TIPOOPERACIONTTV_CV")
                        objDiccionario.Add("TIPOOPERACIONDOC", li.Value)
                    Next
                ElseIf _TipoNegociSelected.Codigo.ToUpper = "S" Then
                    For Each li In DiccionarioCombosCompletos.Where(Function(i) i.Key = "TIPONEGOCIO_TIPOOPERACIONSIMULTANEA_CV")
                        objDiccionario.Add("TIPOOPERACIONDOC", li.Value)
                    Next
                ElseIf _TipoNegociSelected.Codigo.ToUpper = "A" Or _TipoNegociSelected.Codigo.ToUpper = "C" Then
                    For Each li In DiccionarioCombosCompletos.Where(Function(i) i.Key = "TIPONEGOCIO_TIPOOPERACIONGENERAL_CV")
                        objDiccionario.Add("TIPOOPERACIONDOC", li.Value)
                    Next
                Else
                    HabilitarTipoOperacion = False
                End If
            End If

            DiccionarioCombos = objDiccionario
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener valores combos", _
                                                         Me.ToString(), "ObtenerValoresCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            'Dim NewTipoNegoci As New TipoNegocio
            ''TODO: Verificar cuales son los campos que deben inicializarse
            'NewTipoNegoci.ID = TipoNegociPorDefecto.ID
            'NewTipoNegoci.Codigo = TipoNegociPorDefecto.Codigo
            'NewTipoNegoci.Descripcion = TipoNegociPorDefecto.Descripcion
            'NewTipoNegoci.Usuario = Program.Usuario
            'NewTipoNegoci.ConfiguracionMenu = TipoNegociPorDefecto.ConfiguracionMenu
            'TipoNegociAnterior = _TipoNegociSelected
            '_TipoNegociSelected = NewTipoNegoci
            'MyBase.CambioItem("TipoNegociSelected")
            'Editando = True
            'MyBase.CambioItem("Editando")

            mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.TipoNegocios) Then
                dcProxy.TipoNegocios.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TipoNegocioFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocio, Nothing)
            Else
                dcProxy.Load(dcProxy.TipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocio, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Codigo <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                If Not IsNothing(dcProxy.TipoNegocios) Then
                    dcProxy.TipoNegocios.Clear()
                End If
                TipoNegociAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString()    'Dic202011 quitar
                Dim TextoFiltroSeguroNombre = System.Web.HttpUtility.UrlEncode(cb.Codigo)
                dcProxy.Load(dcProxy.TipoNegocioConsultarQuery(TextoFiltroSeguroNombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocio, "Busqueda")
                MyBase.ConfirmarBuscar()
                PrepararNuevaBusqueda()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim logGuardar As Boolean = True
            Dim strEspecieRepetida As String = String.Empty

            If Not IsNothing(ListaTipoNegocioXEspecie) Then
                For Each li In ListaTipoNegocioXEspecie
                    If ListaTipoNegocioXEspecie.Where(Function(i) i.IDEspecie = li.IDEspecie).Count > 1 Then
                        strEspecieRepetida = li.IDEspecie
                        logGuardar = False
                        Exit For
                    End If
                Next
            End If

            If logGuardar = False Then
                mostrarMensaje("No se puede tener especies repetidas. Especie: " & strEspecieRepetida, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 0
                Exit Sub
            End If

            If Not IsNothing(ListaTipoNegocioXTipoProducto) Then
                For Each li In ListaTipoNegocioXTipoProducto
                    If ListaTipoNegocioXTipoProducto.Where(Function(i) i.TipoNegocio = li.TipoNegocio And i.TipoProducto = li.TipoProducto And Trim(i.IDComitente) = Trim(li.IDComitente) And i.Perfil = li.Perfil).Count > 1 Then
                        logGuardar = False
                        Exit For
                    End If
                Next
            End If

            If logGuardar = False Then
                mostrarMensaje("No se puede tener tipos de producto repetidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 1
                Exit Sub
            End If


            If Not IsNothing(ListaDocumentoXTipoNegocio) Then
                For Each li In ListaDocumentoXTipoNegocio
                    If ListaDocumentoXTipoNegocio.Where(Function(i) IsNothing(i.TipoOperacion)).Count > 0 Then
                        logGuardar = False
                        Exit For
                    End If
                Next
            End If

            If logGuardar = False Then
                mostrarMensaje("Tipo de Operación no puede ser vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 3
                Exit Sub
            End If

            Dim strMensajeCer = "No se puede tener certificaciones repetidas."

            If Not IsNothing(ListaCertificacionXTipoNegocio) Then
                For Each li In ListaCertificacionXTipoNegocio
                    If String.IsNullOrEmpty(li.CodigoCertificacion) Then
                        logGuardar = False
                        strMensajeCer = "Uno o varias certificaciones no han sido seleccionados."
                        Exit For
                    End If
                    If ListaCertificacionXTipoNegocio.Where(Function(i) i.CodigoCertificacion = li.CodigoCertificacion).Count > 1 Then
                        logGuardar = False
                        Exit For
                    End If
                Next
            End If


            If logGuardar = False Then
                mostrarMensaje(strMensajeCer, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 2
                Exit Sub
            End If

            Dim strMensajeDoc = "No se puede tener documentos repetidos."

            If Not IsNothing(ListaDocumentoXTipoNegocio) Then
                For Each li In ListaDocumentoXTipoNegocio
                    If IsNothing(li.CodDocumento) Or li.CodDocumento = 0 Then
                        logGuardar = False
                        strMensajeDoc = "Uno o varios tipos de documentos no han sido seleccionados."
                        Exit For
                    End If
                    If ListaDocumentoXTipoNegocio.Where(Function(i) i.CodDocumento = li.CodDocumento And i.TipoOperacion = li.TipoOperacion).Count > 1 Then
                        logGuardar = False
                        Exit For
                    End If
                Next
            End If


            If logGuardar = False Then
                mostrarMensaje(strMensajeDoc, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 3
                Exit Sub
            End If

            Dim strMensajeDis = "No se puede tener en la distribución dos configuraciones repetidas."

            If Not IsNothing(ListaTipoNegocioXDistribucion) Then
                For Each li In ListaTipoNegocioXDistribucion
                    If ListaTipoNegocioXDistribucion.Where(Function(i) i.TipoNegocio = li.TipoNegocio And _
                                                                       i.TipoProducto = li.TipoProducto And _
                                                                       i.CodigoOYD = li.CodigoOYD And _
                                                                       i.PerfilRiesgo = li.PerfilRiesgo And _
                                                                       i.LimiteInferior = li.LimiteInferior And _
                                                                       i.LimiteSuperior = li.LimiteSuperior And _
                                                                       i.ComisionEnPorcentaje = li.ComisionEnPorcentaje And _
                                                                       i.ComisionMinima = li.ComisionMinima And _
                                                                       i.ComisionMaxima = li.ComisionMaxima).Count > 1 Then
                        strMensajeDis = "Hay dos configuraciones repetidas."
                        logGuardar = False
                        Exit For
                    End If
                    For Each liValidacion In ListaTipoNegocioXDistribucion.Where(Function(i) i.TipoNegocio = li.TipoNegocio And _
                                                                                     i.TipoProducto = li.TipoProducto And _
                                                                                     i.CodigoOYD = li.CodigoOYD And _
                                                                                     i.PerfilRiesgo = li.PerfilRiesgo And _
                                                                                     i.ComisionEnPorcentaje = li.ComisionEnPorcentaje And _
                                                                                     i.ID <> li.ID)
                        If (liValidacion.LimiteInferior <= li.LimiteInferior And liValidacion.LimiteSuperior >= li.LimiteSuperior) Or _
                            (li.LimiteInferior <= liValidacion.LimiteInferior And li.LimiteSuperior = liValidacion.LimiteSuperior) Or _
                            (liValidacion.LimiteInferior <= li.LimiteInferior And liValidacion.LimiteSuperior >= li.LimiteInferior) Or _
                            (li.LimiteInferior <= liValidacion.LimiteInferior And li.LimiteSuperior >= liValidacion.LimiteInferior) Or _
                            (liValidacion.LimiteSuperior <= li.LimiteSuperior And liValidacion.LimiteSuperior >= li.LimiteSuperior) Or _
                            (li.LimiteInferior <= liValidacion.LimiteSuperior And li.LimiteSuperior >= liValidacion.LimiteSuperior) Then
                            strMensajeDis = "Hay una o mas configuraciones que no se permiten porque los valores limites estan dentro de otro rango."
                            logGuardar = False
                            Exit For
                        End If
                    Next

                    If li.LimiteInferior >= li.LimiteSuperior Then
                        strMensajeDis = "El limite inferior no puede ser mayor o igual al limite superior."
                        logGuardar = False
                        Exit For
                    End If
                    If li.ComisionMinima >= li.ComisionMaxima Then
                        strMensajeDis = "La comisión minima no puede ser mayor o igual a la comisión máxima."
                        logGuardar = False
                        Exit For
                    End If
                Next
            End If

            If logGuardar = False Then
                mostrarMensaje(strMensajeDis, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 4
                Exit Sub
            End If

            If logGuardar Then
                IsBusy = True
                'DEMC20180126 INICIO
                If IsNothing(_TipoNegociSelected.PorcentajeMinComision) Then
                    _TipoNegociSelected.PorcentajeMinComision = 0
                End If
                'DEMC20180126 FIN
                dcProxy1.Load(dcProxy1.TipoNegocioActualizarQuery(_TipoNegociSelected.ID, _TipoNegociSelected.Codigo, _TipoNegociSelected.PorcentajeMinComision, Program.Usuario), AddressOf TerminarEncabezadoTipoNegocio, "")
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "update")
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
                'TODO: Pendiente garantizar que Userstate no venga vacío
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            BorrarClienteBuscador()
            BorrarEspecieBuscador()

            'Valida la clase del tipo de negocio
            If Not IsNothing(_TipoNegociSelected) Then
                If _TipoNegociSelected.Codigo = "C" Or _TipoNegociSelected.Codigo = "S" Or
                    _TipoNegociSelected.Codigo = "CO" Or _TipoNegociSelected.Codigo = "RC" Or _TipoNegociSelected.Codigo = "TTVC" Then
                    ClaseTipoNegocio = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.C
                ElseIf _TipoNegociSelected.Codigo = "A" Or _TipoNegociSelected.Codigo = "R" Or
                    _TipoNegociSelected.Codigo = "AO" Or _TipoNegociSelected.Codigo = "TTV" Or _TipoNegociSelected.Codigo = "ADR" Then
                    ClaseTipoNegocio = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
                Else
                    ClaseTipoNegocio = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
                End If

                'Consulta los detalles del tipo de negocio
                Dim strCodigo As String = _TipoNegociSelected.Codigo
                ConsultarCertificaciones_TipoNegocio(strCodigo)

                ConsultarDocumentos_TipoNegocio(strCodigo)

                ConsultarEspecie_TipoNegocio(strCodigo)

                ConsultarTipoProducto_TipoNegocio(strCodigo)

                ConsultarDistribucionComision_TipoNegocio(strCodigo)
            End If
            HabilitarTipoOperacion = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminarEncabezadoTipoNegocio(ByVal So As LoadOperation(Of TipoNegocio))
        Try
            IsBusy = False
            If So.HasError Then
                mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                So.MarkErrorAsHandled()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        If Not IsNothing(_TipoNegociSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
            ObtenerValoresCombos()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TipoNegociSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                TipoNegociSelected = TipoNegociAnterior
            End If
            HabilitarTipoOperacion = False
            BorrarClienteBuscador()
            BorrarEspecieBuscador()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_TipoNegociSelected) Then

                'QUITAR TODOS LOS DETALLES DEL TIPO DE NEGOCIO
                Dim objListaCertificaciones As New List(Of CertificacionXTipoNegocio)
                Dim objListaDocumentos As New List(Of DocumentoXTipoNegocio)
                Dim objListaEspecies As New List(Of TipoNegocioXEspecie)
                Dim objListaTipoProducto As New List(Of TipoNegocioXTipoProducto)
                Dim objListaDistribucion As New List(Of DistribucionComisionXTipoNegocio)
                'se pregunto a luis rivera y esta funcionalidad no debe de estar habilitada JBT
                Exit Sub
                objListaCertificaciones = ListaCertificacionXTipoNegocio.ToList
                objListaDocumentos = ListaDocumentoXTipoNegocio.ToList
                objListaEspecies = ListaTipoNegocioXEspecie.ToList
                objListaTipoProducto = ListaTipoNegocioXTipoProducto.ToList
                objListaDistribucion = ListaTipoNegocioXDistribucion.ToList

                For Each li In objListaCertificaciones
                    ListaCertificacionXTipoNegocio.Remove(li)
                Next

                For Each li In objListaDocumentos
                    ListaDocumentoXTipoNegocio.Remove(li)
                Next

                For Each li In objListaEspecies
                    ListaTipoNegocioXEspecie.Remove(li)
                Next

                For Each li In objListaTipoProducto
                    ListaTipoNegocioXTipoProducto.Remove(li)
                Next

                For Each li In objListaDistribucion
                    ListaTipoNegocioXDistribucion.Remove(li)
                Next

                'dcProxy.TipoNegocios.Remove(_TipoNegociSelected)
                '_TipoNegociSelected = _ListaTipoNegocio.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
            'mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaTipoNegocio
            objCB.Codigo = String.Empty

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objTipoNegocio As New TipoNegocio
            If Not IsNothing(_TipoNegociSelected) Then
                objTipoNegocio.ID = _TipoNegociSelected.ID
                objTipoNegocio.Codigo = _TipoNegociSelected.Codigo
                objTipoNegocio.ConfiguracionMenu = _TipoNegociSelected.ConfiguracionMenu
                objTipoNegocio.Descripcion = _TipoNegociSelected.Descripcion
                objTipoNegocio.Usuario = _TipoNegociSelected.Usuario
            End If
            TipoNegociAnterior = Nothing
            TipoNegociAnterior = objTipoNegocio
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarClienteBuscador()
        Try
            If BorrarCliente Then
                BorrarCliente = False
            End If

            BorrarCliente = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar borrar el cliente seleccionado.", _
             Me.ToString(), "BorrarClienteBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarEspecieBuscador()
        Try
            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar borrar la especie seleccionada.", _
             Me.ToString(), "BorrarEspecieBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Certificaciones Tipo Negocio"

    Private _ListaCertificacionXTipoNegocio As EntitySet(Of CertificacionXTipoNegocio)
    Public Property ListaCertificacionXTipoNegocio() As EntitySet(Of CertificacionXTipoNegocio)
        Get
            Return _ListaCertificacionXTipoNegocio
        End Get
        Set(ByVal value As EntitySet(Of CertificacionXTipoNegocio))
            _ListaCertificacionXTipoNegocio = value
            If Not IsNothing(_ListaCertificacionXTipoNegocio) Then
                If _ListaCertificacionXTipoNegocio.Count > 0 Then
                    _CertificacionXTipoNegociSelected = _ListaCertificacionXTipoNegocio.First
                Else
                    _CertificacionXTipoNegociSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaCertificacionXTipoNegocio")
            MyBase.CambioItem("ListaCertificacionXTipoNegocioPaged")
        End Set
    End Property

    Public ReadOnly Property ListaCertificacionXTipoNegocioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCertificacionXTipoNegocio) Then
                Dim view = New PagedCollectionView(_ListaCertificacionXTipoNegocio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CertificacionXTipoNegociSelected As CertificacionXTipoNegocio
    Public Property CertificacionXTipoNegociSelected() As CertificacionXTipoNegocio
        Get
            Return _CertificacionXTipoNegociSelected
        End Get
        Set(ByVal value As CertificacionXTipoNegocio)
            _CertificacionXTipoNegociSelected = value
            MyBase.CambioItem("CertificacionXTipoNegociSelected")
        End Set
    End Property

    Private Sub TerminoTraerCertificacionXTipoNegocioPorDefecto_Completed(ByVal lo As LoadOperation(Of CertificacionXTipoNegocio))
        If Not lo.HasError Then
            CertificacionXTipoNegociPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CertificacionXTipoNegoci por defecto", _
                                             Me.ToString(), "TerminoTraerCertificacionXTipoNegociPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Sub ConsultarCertificaciones_TipoNegocio(ByVal pstrTipoNegocio As String)
        Try
            If Not IsNothing(dcProxy.CertificacionXTipoNegocios) Then
                dcProxy.CertificacionXTipoNegocios.Clear()
            End If

            dcProxy.Load(dcProxy.CertificacionXTipoNegocioConsultarQuery(pstrTipoNegocio, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCertificacionXTipoNegocio, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las certificaciones del tipo de negocio.", _
                                             Me.ToString(), "ConsultarCertificaciones_TipoNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerCertificacionXTipoNegocio(ByVal lo As LoadOperation(Of CertificacionXTipoNegocio))
        If Not lo.HasError Then
            ListaCertificacionXTipoNegocio = dcProxy.CertificacionXTipoNegocios
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CertificacionXTipoNegocio", _
                                             Me.ToString(), "TerminoTraerCertificacionXTipoNegoci", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Documentos x Tipo negocio"

    Private _ListaDocumentoXTipoNegocio As EntitySet(Of DocumentoXTipoNegocio)
    Public Property ListaDocumentoXTipoNegocio() As EntitySet(Of DocumentoXTipoNegocio)
        Get
            Return _ListaDocumentoXTipoNegocio
        End Get
        Set(ByVal value As EntitySet(Of DocumentoXTipoNegocio))
            _ListaDocumentoXTipoNegocio = value
            If Not IsNothing(_ListaDocumentoXTipoNegocio) Then
                If _ListaDocumentoXTipoNegocio.Count > 0 Then
                    _DocumentoXTipoNegociSelected = ListaDocumentoXTipoNegocio.First
                Else
                    _DocumentoXTipoNegociSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaDocumentoXTipoNegocio")
            MyBase.CambioItem("ListaDocumentoXTipoNegocioPaged")
        End Set
    End Property

    Public ReadOnly Property ListaDocumentoXTipoNegocioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDocumentoXTipoNegocio) Then
                Dim view = New PagedCollectionView(_ListaDocumentoXTipoNegocio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DocumentoXTipoNegociSelected As DocumentoXTipoNegocio
    Public Property DocumentoXTipoNegociSelected() As DocumentoXTipoNegocio
        Get
            Return _DocumentoXTipoNegociSelected
        End Get
        Set(ByVal value As DocumentoXTipoNegocio)
            _DocumentoXTipoNegociSelected = value
            MyBase.CambioItem("DocumentoXTipoNegociSelected")
        End Set
    End Property

    Private Sub TerminoTraerDocumentoXTipoNegocioPorDefecto_Completed(ByVal lo As LoadOperation(Of DocumentoXTipoNegocio))
        If Not lo.HasError Then
            DocumentoXTipoNegociPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DocumentoXTipoNegoci por defecto", _
                                             Me.ToString(), "TerminoTraerDocumentoXTipoNegociPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Sub ConsultarDocumentos_TipoNegocio(ByVal pstrTipoNegocio As String)
        Try
            If Not IsNothing(dcProxy.DocumentoXTipoNegocios) Then
                dcProxy.DocumentoXTipoNegocios.Clear()
            End If

            dcProxy.Load(dcProxy.DocumentoXTipoNegocioConsultarQuery(pstrTipoNegocio, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDocumentoXTipoNegocio, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los documentos del tipo de negocio.", _
                                             Me.ToString(), "ConsultarDocumentos_TipoNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerDocumentoXTipoNegocio(ByVal lo As LoadOperation(Of DocumentoXTipoNegocio))
        If Not lo.HasError Then
            ListaDocumentoXTipoNegocio = dcProxy.DocumentoXTipoNegocios
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DocumentoXTipoNegocio", _
                                             Me.ToString(), "TerminoTraerDocumentoXTipoNegoci", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Especies x Tipo negocio"

    Private _ListaTipoNegocioXEspecie As EntitySet(Of TipoNegocioXEspecie)
    Public Property ListaTipoNegocioXEspecie() As EntitySet(Of TipoNegocioXEspecie)
        Get
            Return _ListaTipoNegocioXEspecie
        End Get
        Set(ByVal value As EntitySet(Of TipoNegocioXEspecie))
            _ListaTipoNegocioXEspecie = value
            If Not IsNothing(_ListaTipoNegocioXEspecie) Then
                If _ListaTipoNegocioXEspecie.Count > 0 Then
                    _TipoNegocioXEspeciSelected = ListaTipoNegocioXEspecie.First
                Else
                    _TipoNegocioXEspeciSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTipoNegocioXEspecie")
            MyBase.CambioItem("ListaTipoNegocioXEspeciePaged")
        End Set
    End Property

    Public ReadOnly Property ListaTipoNegocioXEspeciePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoNegocioXEspecie) Then
                Dim view = New PagedCollectionView(_ListaTipoNegocioXEspecie)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoNegocioXEspeciSelected As TipoNegocioXEspecie
    Public Property TipoNegocioXEspeciSelected() As TipoNegocioXEspecie
        Get
            Return _TipoNegocioXEspeciSelected
        End Get
        Set(ByVal value As TipoNegocioXEspecie)
            _TipoNegocioXEspeciSelected = value
            MyBase.CambioItem("TipoNegocioXEspeciSelected")
        End Set
    End Property

    Private Sub TerminoTraerTipoNegocioXEspeciePorDefecto_Completed(ByVal lo As LoadOperation(Of TipoNegocioXEspecie))
        If Not lo.HasError Then
            TipoNegocioXEspeciPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoNegocioXEspeci por defecto", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXEspeciPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Sub ConsultarEspecie_TipoNegocio(ByVal pstrTipoNegocio As String)
        Try
            If Not IsNothing(dcProxy.TipoNegocioXEspecies) Then
                dcProxy.TipoNegocioXEspecies.Clear()
            End If

            dcProxy.Load(dcProxy.TipoNegocioXEspecieConsultarQuery(pstrTipoNegocio, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXEspecie, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las especies del tipo de negocio.", _
                                             Me.ToString(), "ConsultarEspecie_TipoNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerTipoNegocioXEspecie(ByVal lo As LoadOperation(Of TipoNegocioXEspecie))
        If Not lo.HasError Then
            ListaTipoNegocioXEspecie = dcProxy.TipoNegocioXEspecies
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocioXEspecie", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXEspeci", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Tipo Producto Tipo Negocio"

    Private _ListaTipoNegocioXTipoProducto As EntitySet(Of TipoNegocioXTipoProducto)
    Public Property ListaTipoNegocioXTipoProducto() As EntitySet(Of TipoNegocioXTipoProducto)
        Get
            Return _ListaTipoNegocioXTipoProducto
        End Get
        Set(ByVal value As EntitySet(Of TipoNegocioXTipoProducto))
            _ListaTipoNegocioXTipoProducto = value
            If Not IsNothing(_ListaTipoNegocioXTipoProducto) Then
                If _ListaTipoNegocioXTipoProducto.Count > 0 Then
                    _TipoNegocioXTipoProductSelected = _ListaTipoNegocioXTipoProducto.First
                Else
                    _TipoNegocioXTipoProductSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTipoNegocioXTipoProducto")
            MyBase.CambioItem("ListaTipoNegocioXTipoProductoPaged")
        End Set
    End Property

    Public ReadOnly Property ListaTipoNegocioXTipoProductoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoNegocioXTipoProducto) Then
                Dim view = New PagedCollectionView(_ListaTipoNegocioXTipoProducto)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoNegocioXTipoProductSelected As TipoNegocioXTipoProducto
    Public Property TipoNegocioXTipoProductSelected() As TipoNegocioXTipoProducto
        Get
            Return _TipoNegocioXTipoProductSelected
        End Get
        Set(ByVal value As TipoNegocioXTipoProducto)
            _TipoNegocioXTipoProductSelected = value
            MyBase.CambioItem("TipoNegocioXTipoProductSelected")
        End Set
    End Property

    Private Sub TerminoTraerTipoNegocioXTipoProductoPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoNegocioXTipoProducto))
        If Not lo.HasError Then
            TipoNegocioXTipoProductPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoNegocioXTipoProduct por defecto", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXTipoProductPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Sub ConsultarTipoProducto_TipoNegocio(ByVal pstrTipoNegocio As String)
        Try
            If Not IsNothing(dcProxy.TipoNegocioXTipoProductos) Then
                dcProxy.TipoNegocioXTipoProductos.Clear()
            End If

            dcProxy.Load(dcProxy.TipoNegocioXTipoProductoConsultarQuery(pstrTipoNegocio, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXTipoProducto, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los tipos de producto del tipo de negocio.", _
                                             Me.ToString(), "ConsultarTipoProducto_TipoNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerTipoNegocioXTipoProducto(ByVal lo As LoadOperation(Of TipoNegocioXTipoProducto))
        If Not lo.HasError Then
            ListaTipoNegocioXTipoProducto = dcProxy.TipoNegocioXTipoProductos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocioXTipoProducto", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXTipoProduct", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Distribución comisión"

    Private _ListaTipoNegocioXDistribucion As EntitySet(Of DistribucionComisionXTipoNegocio)
    Public Property ListaTipoNegocioXDistribucion() As EntitySet(Of DistribucionComisionXTipoNegocio)
        Get
            Return _ListaTipoNegocioXDistribucion
        End Get
        Set(ByVal value As EntitySet(Of DistribucionComisionXTipoNegocio))
            _ListaTipoNegocioXDistribucion = value
            If Not IsNothing(_ListaTipoNegocioXDistribucion) Then
                If _ListaTipoNegocioXDistribucion.Count > 0 Then
                    TipoNegocioXDistribucionSelected = _ListaTipoNegocioXDistribucion.First
                Else
                    TipoNegocioXDistribucionSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTipoNegocioXDistribucion")
            MyBase.CambioItem("ListaTipoNegocioXDistribucionPaged")
        End Set
    End Property

    Public ReadOnly Property ListaTipoNegocioXDistribucionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoNegocioXDistribucion) Then
                Dim view = New PagedCollectionView(_ListaTipoNegocioXDistribucion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TipoNegocioXDistribucionSelected As DistribucionComisionXTipoNegocio
    Public Property TipoNegocioXDistribucionSelected() As DistribucionComisionXTipoNegocio
        Get
            Return _TipoNegocioXDistribucionSelected
        End Get
        Set(ByVal value As DistribucionComisionXTipoNegocio)
            _TipoNegocioXDistribucionSelected = value
            MyBase.CambioItem("TipoNegocioXDistribucionSelected")
        End Set
    End Property

    Private Sub TerminoTraerTipoNegocioXDistribucionPorDefecto_Completed(ByVal lo As LoadOperation(Of DistribucionComisionXTipoNegocio))
        Try
            If Not lo.HasError Then
                TipoNegocioXDistribucionPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoNegocioXDistribucion por defecto", _
                                                 Me.ToString(), "TerminoTraerTipoNegocioXDistribucionPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la distribución comisión del tipo de negocio x defecto.", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXDistribucionPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDistribucionComision_TipoNegocio(ByVal pstrTipoNegocio As String)
        Try
            If Not IsNothing(dcProxy.DistribucionComisionXTipoNegocios) Then
                dcProxy.DistribucionComisionXTipoNegocios.Clear()
            End If

            dcProxy.Load(dcProxy.DistribucionPortafolioConsultarQuery(pstrTipoNegocio, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXDistribucion, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la distribución comisión del tipo de negocio.", _
                                             Me.ToString(), "ConsultarDistribucionComision_TipoNegocio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerTipoNegocioXDistribucion(ByVal lo As LoadOperation(Of DistribucionComisionXTipoNegocio))
        Try
            If Not lo.HasError Then
                ListaTipoNegocioXDistribucion = dcProxy.DistribucionComisionXTipoNegocios
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocioXDistribucion", _
                                                 Me.ToString(), "TerminoTraerTipoNegocioXDistribucion", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocioXDistribucion", _
                                                 Me.ToString(), "TerminoTraerTipoNegocioXDistribucion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Detalles Maestros del Tipo negocio"

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmCertificaciones"
                    Dim newCertificacion As New CertificacionXTipoNegocio
                    newCertificacion.ID = -New Random().Next()
                    newCertificacion.CodigoCertificacion = CertificacionXTipoNegociPorDefecto.CodigoCertificacion
                    newCertificacion.CodigoTipoNegocio = _TipoNegociSelected.Codigo
                    newCertificacion.Usuario = Program.Usuario

                    ListaCertificacionXTipoNegocio.Add(newCertificacion)
                    CertificacionXTipoNegociSelected = newCertificacion

                    MyBase.CambioItem("CertificacionXTipoNegociSelected")
                    MyBase.CambioItem("ListaCertificacionXTipoNegocio")
                    MyBase.CambioItem("Editando")
                Case "cmDocumentos"
                    Dim newDocumento As New DocumentoXTipoNegocio
                    newDocumento.ID = -New Random().Next()
                    newDocumento.CodDocumento = DocumentoXTipoNegociPorDefecto.CodDocumento
                    newDocumento.TipoNegocio = _TipoNegociSelected.Codigo
                    newDocumento.Usuario = Program.Usuario

                    ListaDocumentoXTipoNegocio.Add(newDocumento)
                    DocumentoXTipoNegociSelected = newDocumento

                    MyBase.CambioItem("DocumentoXTipoNegociSelected")
                    MyBase.CambioItem("ListaDocumentoXTipoNegocio")
                    MyBase.CambioItem("Editando")
                Case "cmEspecies"
                    Dim newEspecie As New TipoNegocioXEspecie
                    newEspecie.ID = -New Random().Next()
                    newEspecie.IDEspecie = TipoNegocioXEspeciPorDefecto.IDEspecie
                    newEspecie.ManejaISIN = TipoNegocioXEspeciPorDefecto.ManejaISIN
                    newEspecie.MaxValorCantidad = 0
                    newEspecie.PermiteNegociar = TipoNegocioXEspeciPorDefecto.PermiteNegociar
                    newEspecie.TipoNegocio = _TipoNegociSelected.Codigo
                    newEspecie.Usuario = Program.Usuario
                    newEspecie.PorcentajeGarantia = 0

                    ListaTipoNegocioXEspecie.Add(newEspecie)
                    TipoNegocioXEspeciSelected = newEspecie

                    MyBase.CambioItem("TipoNegocioXEspeciSelected")
                    MyBase.CambioItem("ListaTipoNegocioXEspecie")
                    MyBase.CambioItem("Editando")

                    BorrarEspecieBuscador()

                Case "cmTiposProducto"
                    Dim newTipoProducto As New TipoNegocioXTipoProducto
                    newTipoProducto.ID = -New Random().Next()
                    newTipoProducto.TipoNegocio = _TipoNegociSelected.Codigo
                    newTipoProducto.IDComitente = TipoNegocioXTipoProductPorDefecto.IDComitente
                    newTipoProducto.Perfil = "TOD"
                    newTipoProducto.TipoProducto = "TOD"
                    newTipoProducto.ValorMaxNegociacion = 0
                    newTipoProducto.Usuario = Program.Usuario
                    newTipoProducto.PermiteNegociar = TipoNegocioXTipoProductPorDefecto.PermiteNegociar
                    newTipoProducto.Nombre = "TODOS"
                    newTipoProducto.PorcentajePatrimonioTecnico = 0

                    ListaTipoNegocioXTipoProducto.Add(newTipoProducto)
                    TipoNegocioXTipoProductSelected = newTipoProducto

                    MyBase.CambioItem("TipoNegocioXTipoProductSelected")
                    MyBase.CambioItem("ListaTipoNegocioXTipoProducto")
                    MyBase.CambioItem("Editando")

                    BorrarClienteBuscador()
                Case "cmDistribucion"
                    Dim newDistribucionComision As New DistribucionComisionXTipoNegocio
                    newDistribucionComision.ID = -New Random().Next()
                    newDistribucionComision.TipoNegocio = _TipoNegociSelected.Codigo
                    newDistribucionComision.ComisionMaxima = 0
                    newDistribucionComision.ComisionMaxima = 0
                    newDistribucionComision.ComisionEnPorcentaje = False
                    newDistribucionComision.LimiteInferior = 0
                    newDistribucionComision.LimiteSuperior = 0
                    newDistribucionComision.NombreCodigoOYD = "Todos"
                    newDistribucionComision.TipoProducto = "TOD"
                    newDistribucionComision.PerfilRiesgo = "TOD"
                    newDistribucionComision.Usuario = Program.Usuario

                    ListaTipoNegocioXDistribucion.Add(newDistribucionComision)
                    TipoNegocioXDistribucionSelected = newDistribucionComision

                    MyBase.CambioItem("TipoNegocioXDistribucionSelected")
                    MyBase.CambioItem("ListaTipoNegocioXDistribucion")
                    MyBase.CambioItem("Editando")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar el nuevo detalle.", _
                                                         Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmCertificaciones"
                    If Not IsNothing(ListaCertificacionXTipoNegocio) And Not IsNothing(_CertificacionXTipoNegociSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(CertificacionXTipoNegociSelected, ListaCertificacionXTipoNegocio.ToList)

                        ListaCertificacionXTipoNegocio.Remove(CertificacionXTipoNegociSelected)

                        If ListaCertificacionXTipoNegocio.Count > 0 Then
                            Program.PosicionarItemLista(CertificacionXTipoNegociSelected, ListaCertificacionXTipoNegocio.ToList, intRegistroPosicionar)
                        Else
                            CertificacionXTipoNegociSelected = Nothing
                        End If
                        MyBase.CambioItem("CertificacionXTipoNegociSelected")
                        MyBase.CambioItem("ListaCertificacionXTipoNegocio")

                    End If
                Case "cmDocumentos"
                    If Not IsNothing(ListaDocumentoXTipoNegocio) And Not IsNothing(_DocumentoXTipoNegociSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DocumentoXTipoNegociSelected, ListaDocumentoXTipoNegocio.ToList)

                        ListaDocumentoXTipoNegocio.Remove(DocumentoXTipoNegociSelected)

                        If ListaDocumentoXTipoNegocio.Count > 0 Then
                            Program.PosicionarItemLista(DocumentoXTipoNegociSelected, ListaDocumentoXTipoNegocio.ToList, intRegistroPosicionar)
                        Else
                            DocumentoXTipoNegociSelected = Nothing
                        End If
                        MyBase.CambioItem("DocumentoXTipoNegociSelected")
                        MyBase.CambioItem("ListaDocumentoXTipoNegocio")

                    End If
                Case "cmEspecies"
                    If Not IsNothing(ListaTipoNegocioXEspecie) And Not IsNothing(_TipoNegocioXEspeciSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(TipoNegocioXEspeciSelected, ListaTipoNegocioXEspecie.ToList)

                        ListaTipoNegocioXEspecie.Remove(TipoNegocioXEspeciSelected)

                        If ListaTipoNegocioXEspecie.Count > 0 Then
                            Program.PosicionarItemLista(TipoNegocioXEspeciSelected, ListaTipoNegocioXEspecie.ToList, intRegistroPosicionar)
                        Else
                            TipoNegocioXEspeciSelected = Nothing
                        End If
                        MyBase.CambioItem("TipoNegocioXEspeciSelected")
                        MyBase.CambioItem("ListaTipoNegocioXEspecie")

                    End If
                Case "cmTiposProducto"
                    If Not IsNothing(ListaTipoNegocioXTipoProducto) And Not IsNothing(_TipoNegocioXTipoProductSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(TipoNegocioXTipoProductSelected, ListaTipoNegocioXTipoProducto.ToList)

                        ListaTipoNegocioXTipoProducto.Remove(TipoNegocioXTipoProductSelected)

                        If ListaTipoNegocioXTipoProducto.Count > 0 Then
                            Program.PosicionarItemLista(TipoNegocioXTipoProductSelected, ListaTipoNegocioXTipoProducto.ToList, intRegistroPosicionar)
                        Else
                            TipoNegocioXTipoProductSelected = Nothing
                        End If
                        MyBase.CambioItem("TipoNegocioXTipoProductSelected")
                        MyBase.CambioItem("ListaTipoNegocioXTipoProducto")

                    End If
                Case "cmDistribucion"
                    If Not IsNothing(ListaTipoNegocioXDistribucion) And Not IsNothing(_TipoNegocioXDistribucionSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(TipoNegocioXDistribucionSelected, ListaTipoNegocioXDistribucion.ToList)

                        ListaTipoNegocioXDistribucion.Remove(TipoNegocioXDistribucionSelected)

                        If ListaTipoNegocioXTipoProducto.Count > 0 Then
                            Program.PosicionarItemLista(TipoNegocioXDistribucionSelected, ListaTipoNegocioXDistribucion.ToList, intRegistroPosicionar)
                        Else
                            TipoNegocioXDistribucionSelected = Nothing
                        End If
                        MyBase.CambioItem("TipoNegocioXDistribucionSelected")
                        MyBase.CambioItem("ListaTipoNegocioXDistribucion")

                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", _
                                                         Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaTipoNegocio
    Implements INotifyPropertyChanged

    Private _Codigo As String
    <Display(Name:="Código", Description:="Código")> _
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class