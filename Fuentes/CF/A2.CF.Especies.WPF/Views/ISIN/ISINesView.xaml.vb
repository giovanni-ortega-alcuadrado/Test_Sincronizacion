Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports A2.OyD.OYDServer.RIA.Web
Imports A2ComunesControl
Imports A2ComunesImportaciones

Partial Public Class ISINesView
    Inherits Window

    Public _vm As ISINesViewModel
    Property blnRespuesta As Boolean
    Dim logCambioValor As Boolean = False
    Dim logDigitoTexto As Boolean = False

    Private _isinIsinFungible As EspeciesISINFungible
    Public Property isinIsinFungible() As EspeciesISINFungible
        Get
            Return _isinIsinFungible
        End Get
        Set(ByVal value As EspeciesISINFungible)
            _isinIsinFungible = value
        End Set
    End Property

    Private _isinAmortizaciones As AmortizacionesEspeci
    Public Property isinAmortizaciones() As AmortizacionesEspeci
        Get
            Return _isinAmortizaciones
        End Get
        Set(ByVal value As AmortizacionesEspeci)
            _isinAmortizaciones = value
        End Set
    End Property

    Public Sub New()
        Try
            _vm = New ISINesViewModel()
            Me.DataContext = _vm
            InitializeComponent()

            inicializar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ISINesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub New(pstrIdEspecie As String, pblnEsAccion As Boolean, pstrTipotasa As String, yaExiste As Boolean, pstrAmortiza As String, pblnNuevoregistro As Boolean, pBaseCalculoInteres As Integer, pstrNombreEspecie As String, pstrIndicador As String, plogEditar As Boolean, pintIDConceptoRetencion As Integer, ByVal logTituloCarteraColectiva As Boolean, ByVal pstrTipoEspecie As String)
        Try
            Dim logAmortiza As Boolean = False

            If Not New String() {"0", "1", "3", Nothing, ""}.Contains(pstrAmortiza) Then
                logAmortiza = True
            Else
                logAmortiza = False
            End If

            _vm = New ISINesViewModel(pstrIdEspecie, pstrNombreEspecie, pstrIndicador, logAmortiza, pintIDConceptoRetencion, pstrTipoEspecie, pblnEsAccion)
            _vm.Editando = True
            _vm.ConEspecie = yaExiste
            establecerVisivilidad(pblnEsAccion, pstrTipotasa)
            Me.DataContext = _vm
            InitializeComponent()

            If Not New String() {"0", "1", "3", Nothing, ""}.Contains(pstrAmortiza) Then
                _vm.blnHabilitarAmortizaciones = True
            Else
                _vm.blnHabilitarAmortizaciones = False
            End If

            _vm.blnNuevoRegistro = pblnNuevoregistro
            _vm.intBaseCalculoInteres = pBaseCalculoInteres
            _vm.strNombreEspecie = pstrNombreEspecie
            _vm.logEditarEspecie = plogEditar

            _vm.strTipoEspecie = pstrTipoEspecie  'JAEZ 20161101
            _vm.logEsAccion = pblnEsAccion  'JAEZ 20161101
            _vm.logTituloCarteraColectiva = logTituloCarteraColectiva

            '_vm.ConsultarValoresDefectoTituloParticipativoISIN(logTituloCarteraColectiva)
            inicializar()
            logCambioValor = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ISINesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Descripción:         Desde aquí se conservan los datos para los campos intIdCalificacionInversionC y intIdCalificacionInversionL (estos campos no se guardan en la base de datos), 
    '''                      en base al dato del campo intIDCalificacionInversion
    ''' Responsable:         Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:               18 de Abril/2016
    ''' </history>
    Public Sub New(pisnISINSelected As EspeciesISINFungible, pblnEsAccion As Boolean, pstrTipotasa As String, yaExiste As Boolean, pstrAmortiza As String, pBaseCalculoInteres As Integer, pstrIndicador As String, plogEditar As Boolean, pintIDConceptoRetencion As Integer, ByVal logTituloCarteraColectiva As Boolean, ByVal pstrTipoEspecie As String)
        Try
            pisnISINSelected.ConEspecie = yaExiste
            pisnISINSelected.strTipoEspecie = pstrTipoEspecie 'JAEZ 20161101
            pisnISINSelected.logEsAccion = pblnEsAccion 'JAEZ 20161103
            pisnISINSelected.logTituloCarteraColectiva = logTituloCarteraColectiva

            _vm = New ISINesViewModel(pisnISINSelected, pblnEsAccion, pstrIndicador)
            _vm.Editando = True
            _vm.blnDesdeEspecies = True
            _vm.ConEspecie = yaExiste
            _vm.strTipoEspecie = pstrTipoEspecie  'JAEZ 20161101
            _vm.logEsAccion = pblnEsAccion 'JAEZ 20161101

            establecerVisivilidad(pblnEsAccion, pstrTipotasa)
            Me.DataContext = _vm
            InitializeComponent()

            If Not New String() {"0", "1", "3", Nothing, ""}.Contains(pstrAmortiza) Then
                _vm.EspeciesISINFungibleSelected.Amortizada = True
                _vm.blnHabilitarAmortizaciones = True
            Else
                _vm.EspeciesISINFungibleSelected.Amortizada = False
                _vm.blnHabilitarAmortizaciones = False
            End If

            _vm.intBaseCalculoInteres = pBaseCalculoInteres
            _vm.logEditarEspecie = plogEditar
            _vm.EspeciesISINFungibleSelected.logTituloCarteraColectiva = logTituloCarteraColectiva

            If Not IsNothing(pisnISINSelected.intIDCalificacionInversion) Then
                _vm.EspeciesISINFungibleSelected.intIdCalificacionInversionC = pisnISINSelected.intIDCalificacionInversion
                _vm.EspeciesISINFungibleSelected.intIdCalificacionInversionL = pisnISINSelected.intIDCalificacionInversion
                '_vm.EspeciesISINFungibleSelected.logTituloCarteraColectiva = logTituloCarteraColectiva
            End If
            '_vm.ConsultarValoresDefectoTituloParticipativoISIN(logTituloCarteraColectiva)

            inicializar()
            logCambioValor = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ISINesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
        End If
    End Sub

    Private Sub establecerVisivilidad(pblnEsAccion As Boolean, pstrTipotasa As String)
        _vm.blnDesdeEspecies = True
        If pblnEsAccion Then
            _vm.visRentaFija = Visibility.Collapsed
            _vm.visRentaFijaTF = Visibility.Collapsed
            _vm.visRentaFijaTV = Visibility.Collapsed
            _vm.visCF = Visibility.Collapsed
        Else
            _vm.visRentaFija = Visibility.Visible
            _vm.visCF = Visibility.Visible
            If pstrTipotasa = "F" Then
                _vm.visRentaFijaTF = Visibility.Visible
                _vm.visRentaFijaTV = Visibility.Collapsed
            Else
                _vm.visRentaFijaTV = Visibility.Visible
                _vm.visRentaFijaTF = Visibility.Collapsed
            End If
        End If
    End Sub

    Private Sub ctlrEspecies_nemotecnicoAsignado(pstrNemotecnico As String, pstrNombreEspecie As String)
        CType(Me.DataContext, ISINesViewModel).strEspecie = pstrNemotecnico
        CType(Me.DataContext, ISINesViewModel).strNombreEspecie = pstrNombreEspecie
    End Sub

    Private Sub HyperlinkButton_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, ISINesViewModel).CambiarAForma()
    End Sub

    Private Sub ISINesView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        _vm.vistaIsines = Me
        '_vm.ConsultarValoresDefectoTituloParticipativoISIN()
        If Not IsNothing(_vm.EspeciesISINFungibleSelected) Then
            _vm.CargarCalificacion()
        End If
    End Sub


    Private Sub fieldEspecies_especieAsignada(pstrNemotecnico As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            _vm.strEspecie = pstrNemotecnico
            _vm.EspeciesISINFungibleSelected.IDEspecie = pstrNemotecnico
            '_vm.EspeciesISINFungibleSelected.Descripcion = pstrNombre
            _vm.EspeciesISINFungibleSelected.IDIsinFungible = -1
            '_vm.EspeciesISINFungibleSelected.Fecha_Emision = pobjEspecie.Emision
            '_vm.EspeciesISINFungibleSelected.Fecha_Vencimiento = pobjEspecie.Vencimiento
            If pobjEspecie.EsAccion Then
                _vm.visRentaFija = Visibility.Collapsed
                _vm.visRentaFijaTF = Visibility.Collapsed
                _vm.visRentaFijaTV = Visibility.Collapsed
            Else
                _vm.visRentaFija = Visibility.Visible
                If pobjEspecie.TipoTasa = "F" Then
                    _vm.visRentaFijaTF = Visibility.Visible
                    _vm.visRentaFijaTV = Visibility.Collapsed
                Else
                    _vm.visRentaFijaTV = Visibility.Visible
                    _vm.visRentaFijaTF = Visibility.Collapsed
                End If
            End If
            _vm.IsBusy = False
        End If
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivo(sender As ObjetoInformacionArchivo, strProceso As String)
        Try
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)

            If Not IsNothing(objDialog.pFile) Then
                'If objDialog.pFile.File.Extension.Equals(".csv") Or objDialog.pFile.File.Extension.Equals(".xls") Or objDialog.pFile.File.Extension.Equals(".xlsx") Then
                'Dim strRutaArchivo As String = objDialog.pFile.FileName
                Dim strRutaArchivo As String = System.IO.Path.GetFileName(objDialog.pFile.FileName)

                If Not IsNothing(_vm) Then

                    If IsNothing(_vm.EspeciesISINFungibleSelected.Fecha_Emision) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Para importar amortizaciones debe seleccionar la fecha de emisión.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    If IsNothing(_vm.EspeciesISINFungibleSelected.Fecha_Vencimiento) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Para importar amortizaciones debe seleccionar la fecha de vencimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                    If _vm.EspeciesISINFungibleSelected.Fecha_Emision > _vm.EspeciesISINFungibleSelected.Fecha_Vencimiento Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión no puede ser superior a la fecha de vencimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If

                End If

                Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, ISINesViewModel), strRutaArchivo, strProceso)
                Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                viewImportacion.ShowDialog()

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub C1NumericBox_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If Not IsNothing(_vm) Then
                If _vm.Editando Then
                    logDigitoTexto = True
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(_vm) Then
                If _vm.Editando And logDigitoTexto Then
                    Dim strTipoControl As String = String.Empty

                    strTipoControl = CType(sender, A2Utilidades.A2NumericBox).Tag

                    logCambioValor = True
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub CalculoNominalEfectiva(sender As Object, e As RoutedEventArgs)
        Try
            Dim logCalcularSinMostrarPregunta As Boolean = False
            Dim dblTasa_Facial As Double
            Dim dblTasaEfectiva As Double

            If IsNothing(_vm.EspeciesISINFungibleSelected.Tasa_Facial) Then
                dblTasa_Facial = 0
            ElseIf _vm.EspeciesISINFungibleSelected.Tasa_Facial = 0 Then
                dblTasa_Facial = 0
            Else
                dblTasa_Facial = _vm.EspeciesISINFungibleSelected.Tasa_Facial
            End If

            If IsNothing(_vm.EspeciesISINFungibleSelected.dblTasaEfectiva) Then
                dblTasaEfectiva = 0
            ElseIf _vm.EspeciesISINFungibleSelected.dblTasaEfectiva = 0 Then
                dblTasaEfectiva = 0
            Else
                dblTasaEfectiva = _vm.EspeciesISINFungibleSelected.dblTasaEfectiva
            End If

            If dblTasa_Facial > 0 And dblTasaEfectiva = 0 Then
                _vm.strTipoCalculo = "TASANOMINALEFECTIVA"
                logCalcularSinMostrarPregunta = True
            End If

            If dblTasa_Facial = 0 And dblTasaEfectiva > 0 Then
                _vm.strTipoCalculo = "TASAEFECTIVANOMINAL"
                logCalcularSinMostrarPregunta = True
            End If

            If logCalcularSinMostrarPregunta Then
                logCambioValor = False
                logDigitoTexto = False
                Await _vm.CalcularValorRegistro()
            End If

            If logCalcularSinMostrarPregunta = False And dblTasa_Facial > 0 And dblTasaEfectiva > 0 Then
                A2Utilidades.Mensajes.mostrarMensajePregunta(vbTab & vbTab & vbTab & vbTab & vbTab & vbTab & vbTab & vbTab & "    ¿ Qué tasa desea calcular ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf TerminaPregunta, True, "Presione SI para Nominal a Efectiva o NO para Efectiva a Nominal")
                'C1.Silverlight.C1MessageBox.Show("Que tasa desea calcular? " + vbLf + "Yes--> Nominal a Efectiva" + vbLf + "No --> Efectiva a Nominal", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminaPregunta)
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub



    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        End If
    End Sub

    Private Async Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not IsNothing(_vm) Then
                If _vm.Editando Then

                    If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                        _vm.strTipoCalculo = "TASANOMINALEFECTIVA"
                    Else
                        _vm.strTipoCalculo = "TASAEFECTIVANOMINAL"
                    End If

                    logCambioValor = False
                    logDigitoTexto = False
                    Await _vm.CalcularValorRegistro()
                End If
            End If
            'End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGrabar_Click(sender As Object, e As RoutedEventArgs)
        _vm.ActualizarRegistro()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        _vm.CancelarEditarRegistro()
    End Sub
End Class
