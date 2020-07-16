Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: EspeciesView.xaml.vb
'Generado el : 06/30/2011 10:47:24
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web
Imports GalaSoft.MvvmLight.Messaging


Partial Public Class EspeciesView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Private mobjVM As EspeciesViewModel
    Dim logInicializar As Boolean = True

    Public Sub New()
        Me.DataContext = New EspeciesViewModel
InitializeComponent()
        ' El mensaje se utiliza para saber si se está duplicando la especie en caso tal que si, realizar el focus al textbox del nemotecnico 
        Messenger.Default.Register(Of CFEspecies.Especi)(Me, AddressOf LlegoMensaje)
    End Sub


    Private Sub Especies_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If logInicializar Then
            cm.GridTransicion = grdGridForma
            cm.GridViewRegistros = datapager1
            CType(Me.DataContext, EspeciesViewModel).NombreView = Me.ToString
            mobjVM = CType(Me.DataContext, EspeciesViewModel)
            CType(Me.DataContext, EspeciesViewModel).viewEspecie = Me
            mobjVM.visNavegando = "Collapsed"
            Me.dtpEmision.DisplayDateEnd = mobjVM.FechaActual
            inicializar()
        End If
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            If logInicializar Then
                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
                logInicializar = False
            End If
        End If
    End Sub

    

    Public Sub Rfocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'gridEd.ValidateItem()
        'If gridEd.ValidationSummary.HasErrors Then
        '    gridEd.CancelEdit()
        'Else
        '    gridEd.CommitEdit()
        'End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idemisor"
                    CType(Me.DataContext, EspeciesViewModel).EspecieSelected.IdEmisor = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EspeciesViewModel).EmisorClaseSelected.Emisor = pobjItem.Nombre
                Case "idemisorbuscar"
                    CType(Me.DataContext, EspeciesViewModel).cb.plngIdEmisor = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EspeciesViewModel).cb.Emisor = pobjItem.Nombre

                Case "idclase"
                    CType(Me.DataContext, EspeciesViewModel).EspecieSelected.IDClase = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EspeciesViewModel).EspecieSelected.strClase = pobjItem.Nombre
                    CType(Me.DataContext, EspeciesViewModel).ClaseClaseSelected.Clase = pobjItem.Nombre

                    CType(Me.DataContext, EspeciesViewModel).EspecieSelected.ClaseContableTitulo = pobjItem.InfoAdicional01

                    'CType(Me.DataContext, EspeciesViewModel).EspecieSelected.TituloParticipativo = pobjItem.InfoAdicional02
                    If CType(Me.DataContext, EspeciesViewModel).EspecieSelected.TituloParticipativo Then
                        'CType(Me.DataContext, EspeciesViewModel).HabilitarCamposTituloParticipativo = False
                        'CType(Me.DataContext, EspeciesViewModel).HabilitarTipo = False
                        CType(Me.DataContext, EspeciesViewModel).HabilitarCamposTabCFTituloParticipativo = False
                    Else
                        'CType(Me.DataContext, EspeciesViewModel).HabilitarCamposTituloParticipativo = True
                        'CType(Me.DataContext, EspeciesViewModel).HabilitarTipo = True
                        CType(Me.DataContext, EspeciesViewModel).HabilitarCamposTabCFTituloParticipativo = True
                        'CType(Me.DataContext, EspeciesViewModel).logLimpiarTituloParticipativo = True
                    End If
                    'CType(Me.DataContext, EspeciesViewModel).ConsultarValoresDefectoTituloParticipativo()

                Case "idclasebuscar"
                    CType(Me.DataContext, EspeciesViewModel).cb.plngIDClase = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EspeciesViewModel).cb.Clase = pobjItem.Nombre

                Case "admonemision"
                    CType(Me.DataContext, EspeciesViewModel).EspecieSelected.IDAdmonEmision = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, EspeciesViewModel).EmisoresClaseSelected.Emisores = pobjItem.Nombre
                    CType(Me.DataContext, EspeciesViewModel).EmisoresClaseSelected.NombreAdmonEmision = pobjItem.Nombre

                Case "claseinversion"
                    CType(Me.DataContext, EspeciesViewModel).EspecieSelected.ClaseInversion = pobjItem.IdItem
                    If IsNothing(CType(Me.DataContext, EspeciesViewModel).claseinversionSelected) Then
                        CType(Me.DataContext, EspeciesViewModel).claseinversionSelected = New claseinversion() With {.strcodigo = pobjItem.IdItem}
                    End If

                    CType(Me.DataContext, EspeciesViewModel).claseinversionSelected.strDescripcion = pobjItem.Descripcion
                    mobjVM.DescripcionClaseInversion = pobjItem.IdItem & ", " & pobjItem.Nombre & Environment.NewLine & pobjItem.Descripcion
            End Select
        End If
    End Sub

    Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, EspeciesViewModel).EspecieSelected) Then
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("ISIN", "ISIN", "ISIN", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, EspeciesViewModel).EspecieSelected.Id, "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()

        End If
    End Sub

    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then

            Select Case mobjBuscadorLst.CampoBusqueda.ToLower
                Case "isin"
                    CType(Me.DataContext, EspeciesViewModel).EspeciesISINFungibleSelected.ISIN = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case Else
            End Select
        End If
    End Sub

    Private Sub Buscadorclaseespecie_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, EspeciesViewModel).Clase()
        'If CType(Me.DataContext, EspeciesViewModel).EspecieSelected.EsAccion Then
        '    df.FindName("Buscadorclaseespecie").Agrupamiento = "1"
        'Else
        '    df.FindName("Buscadorclaseespecie").Agrupamiento = "0"
        'End If
    End Sub

    Private Sub BusquedaRapida_LostFocus(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Select Case DirectCast(sender, A2Utilidades.A2NumericBox).Name.ToString
            Case "IdEmisor"
                CType(Me.DataContext, EspeciesViewModel).buscarItem("emisor,property")
            Case "IdClase"
                CType(Me.DataContext, EspeciesViewModel).buscarItem("clase,property")
            Case "IDAdmonEmision"
                CType(Me.DataContext, EspeciesViewModel).buscarItem("admonemision,property")
        End Select

    End Sub
    ''' <summary>
    ''' Se lanza maestro de ISIN fungibles en modo de edición -- EOMC -- 08/08/2013
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub HyperlinkButton_Click(sender As Object, e As RoutedEventArgs)

        CType(Me.DataContext, EspeciesViewModel).ValidarEspecieLanzarISIN(sender.datacontext)
    End Sub

    Private Sub btnBuscarISIN_Click(sender As Object, e As RoutedEventArgs) Handles btnBuscarISIN.Click
        CType(Me.DataContext, EspeciesViewModel).FiltrarListaISIN()
    End Sub

    Private Sub btnNuevoISIN_Click(sender As Object, e As RoutedEventArgs) Handles btnNuevoIsin.Click
        CType(Me.DataContext, EspeciesViewModel).DetalleIsinFungible_NuevoRegistro()
    End Sub

    Private Sub btnBorrarIsin_Click(sender As Object, e As RoutedEventArgs) Handles btnBorrarIsin.Click
        CType(Me.DataContext, EspeciesViewModel).DetallleIsinFungible_BorrarRegistro()
    End Sub

    Private Sub btnDuplicarIsin_Click(sender As Object, e As RoutedEventArgs) Handles btnDuplicarIsin.Click
        Try
            CType(Me.DataContext, EspeciesViewModel).DetallleIsinFungible_DuplicarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para duplicar el ISIN", Me.Name, "btnDuplicarIsin_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ComboBoxBursatilidad_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            Me.mobjVM.ConsultarValoresDefectoBursatilidad()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la calificadora.", _
                                Me.ToString(), "ComboBox_SelectionChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LlegoMensaje()
        txtNemotecnico.Focus()
    End Sub

    Private Sub EspeciesView_Unloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        Messenger.Default.Unregister(Me)
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                mobjVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub cm_EventoCancelarBuscar(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, EspeciesViewModel).AltoBotonDuplicar = 19
        CType(Me.DataContext, EspeciesViewModel).HabilitarDuplicarRegistro = True
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, EspeciesViewModel).DetalleClaseInversion()
    End Sub

Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, EspeciesViewModel).PreguntarDuplicarRegistro()
    End Sub

    Private Sub cmbConceptoRetencionGravados_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub Buscador_Especie_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(sender, A2ComunesControl.BuscadorEspecieListaButon).ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.A
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en Buscador_Especie_GotFocus",
                                 Me.ToString(), "Buscador_Especie_GotFocus", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Buscador_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, EspeciesViewModel).ArbitrajeADRSelected.strIDEspecie = pobjItem.Nemotecnico
        End If
    End Sub

    Private Sub btnBuscarADR_Click(sender As Object, e As RoutedEventArgs) Handles btnBuscarADR.Click
        CType(Me.DataContext, EspeciesViewModel).DetalleADR_FiltrarLista()
    End Sub

    Private Sub btnLimpiarFiltroADR_Click(sender As Object, e As RoutedEventArgs) Handles btnLimpiarFiltroADR.Click
        CType(Me.DataContext, EspeciesViewModel).DetalleADR_LimpiarFiltro()
    End Sub

    Private Sub btnNuevoADR_Click(sender As Object, e As RoutedEventArgs) Handles btnNuevoADR.Click
        CType(Me.DataContext, EspeciesViewModel).DetalleADR_NuevoRegistro()
    End Sub

    Private Sub btnBorrarADR_Click(sender As Object, e As RoutedEventArgs) Handles btnBorrarADR.Click
        CType(Me.DataContext, EspeciesViewModel).DetalleADR_BorrarRegistro()
    End Sub

End Class





