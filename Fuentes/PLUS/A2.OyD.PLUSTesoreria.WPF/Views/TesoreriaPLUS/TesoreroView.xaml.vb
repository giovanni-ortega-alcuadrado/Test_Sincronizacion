Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.ComponentModel


Partial Public Class TesoreroView
    Inherits UserControl

    Dim objVMA2Utils As A2UtilsViewModel
    Private WithEvents objVMTesorero As TesoreroViewModel_OYDPLUS
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private strClaseCombos As String = "Plus_Tesorero"
    Private mstrDicCombosEspecificos As String = String.Empty
    Public ListaFiltrada As New List(Of OyDPLUSTesoreria.tblTesorero_Registros_CE)

#Region "Inicializaciones"

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)

            Me.Resources.Add("A2VM", objA2VM)

            mlogInicializado = True

            Me.DataContext = New TesoreroViewModel_OYDPLUS
            InitializeComponent()

            objVMTesorero = Me.LayoutRoot.DataContext

            AddHandler Me.Unloaded, AddressOf View_Unloaded

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


#End Region
    Private Sub Tesorero_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If objVMTesorero Is Nothing Then
                objVMTesorero = CType(Me.DataContext, TesoreroViewModel_OYDPLUS)
                objVMTesorero.objTesorero = Me
                mlogInicializado = True
                objVMTesorero.ViewTesoreroOYDPLUS = Me
                objVMTesorero.visNavegando = "Collapse"
                objVMTesorero.ReiniciaTimer()
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "Tesorero_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.LayoutRoot.Width = Application.Current.MainWindow.ActualWidth * 0.96
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrClaseControl = "banconacionalfondos" Then
                    objVMTesorero.DescripcionBancoFondo = pobjItem.Nombre + " | " + pobjItem.CodItem
                    objVMTesorero.IdBancoFondo = pobjItem.IdItem
                ElseIf pstrClaseControl = "cuentasbancarias" Then
                    objVMTesorero.ConsultarSaldoBanco = False
                    objVMTesorero.DescripcionBanco = pobjItem.Descripcion
                    objVMTesorero.IdBanco = pobjItem.IdItem
                    objVMTesorero.ConsultarSaldoBanco = True
                ElseIf pstrClaseControl = "cuentasbancariasdestino" Then
                    objVMTesorero.ConsultarSaldoBanco_FondosOYD = False
                    objVMTesorero.DescripcionBanco_FondosOYD = pobjItem.Descripcion
                    objVMTesorero.IdBanco_FondosOYD = pobjItem.IdItem
                    objVMTesorero.ConsultarSaldoBanco_FondosOYD = True
                ElseIf pstrClaseControl = "receptores" Then
                    objVMTesorero.ItemReceptor = pobjItem.CodItem
                    objVMTesorero.NombreReceptor = pobjItem.Nombre
                ElseIf pstrClaseControl = "carterascolectivas" Then
                    objVMTesorero.CarteraColectiva = pobjItem.IdItem
                    objVMTesorero.DescripcionCarteraColectiva = pobjItem.Nombre
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.Name, "New", "BuscadorGenerico_finalizoBusqueda", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de tesorero
            If Not IsNothing(objVMTesorero) Then
                Me.objVMTesorero.pararTemporizador()

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla del tesorero.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarReceptor_Click_1(sender As Object, e As RoutedEventArgs)
        objVMTesorero.ItemReceptor = GSTR_ID_TODOS
        objVMTesorero.NombreReceptor = GSTR_TODOS
    End Sub

    Private Sub VerOrden(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                Dim strTexto As String = CType(CType(sender, Button).Content, TextBlock).Text
                If Not IsNothing(strTexto) Then
                    Dim intOrden As Integer = 0
                    Dim logEditarOrden As Boolean = False
                    Dim logPorAprobar As Boolean = False

                    If strTexto.Contains("Confirmar") Then
                        intOrden = Integer.Parse(strTexto.Replace(" - Confirmar", ""))
                        logEditarOrden = True
                    ElseIf strTexto.Contains("Por aprobar") Then
                        intOrden = Integer.Parse(strTexto.Replace(" - Por aprobar", ""))
                        logEditarOrden = False
                        logPorAprobar = True
                    Else
                        intOrden = Integer.Parse(strTexto)
                    End If
                    objVMTesorero.VerOrdenTesoreria(intOrden, logEditarOrden, logPorAprobar)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "VerOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub EditarDetalle(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                Dim objRegistroSeleccionado As OyDPLUSTesoreria.tblTesorero_Registros_CE = CType(CType(sender, Button).Tag, OyDPLUSTesoreria.tblTesorero_Registros_CE)
                If Not IsNothing(objRegistroSeleccionado) Then
                    objVMTesorero.EditarDetalle(objRegistroSeleccionado.lngID, objRegistroSeleccionado.Generar)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "VerOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub HyperlinkButton_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                objVMTesorero.MostrarCartaGenerencia()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "HyperlinkButton_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub EditarRecibo(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                Dim strTexto As String = CType(CType(sender, Button).Content, TextBlock).Text
                If Not IsNothing(strTexto) Then
                    objVMTesorero.EditarRecibo(Integer.Parse(strTexto))
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "HyperlinkButton_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dgppal_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgppal_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    Private Sub dgTransferencia_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgTransferencia_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dgCarterasColectivas_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgCarterasColectivas_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dgOYD_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgOYD_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dgInternos_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgInternos_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dgBloqueos_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgBloqueos_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub dgOperacionesEspeciales_FilterChanged(sender As Object, e As GridView.GridViewFilteredEventArgs)
        Try
            If Not IsNothing(ListaFiltrada) Then
                ListaFiltrada.Clear()
                For Each li In CType(sender, Telerik.Windows.Controls.RadGridView).ItemsSource
                    ListaFiltrada.Add(CType(li, OyDPLUSTesoreria.tblTesorero_Registros_CE))
                Next
            End If
            If Not IsNothing(objVMTesorero) Then
                If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                    If ListaFiltrada.Count <> objVMTesorero.ListaResultadosDocumentos.Count Then
                        objVMTesorero.logListaFiltrada_OrdenGiro = True
                    Else
                        objVMTesorero.logListaFiltrada_OrdenGiro = False
                    End If
                    For Each li In objVMTesorero.ListaResultadosDocumentos
                        li.Generar = False
                    Next
                End If
                objVMTesorero.SeleccionarTodos = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Filtrar Grid", Me.Name, "dgOperacionesEspeciales_FilterChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorCuentasBancariasCarteras_GotFocus_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                If objVMTesorero.strTipoNegocio = GSTR_ORDENFONDOS.ToUpper Then
                    If objVMTesorero.strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or
                                    objVMTesorero.strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Then
                        If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count > 0 Then
                            If Not IsNothing(objVMTesorero.ListaResultadosDocumentosRecibo) Then
                                If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count > 0 Then
                                    Dim strFondoIncial As String = objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).FirstOrDefault.strCarteraColectivaFondos

                                    If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True And i.strCarteraColectivaFondos <> strFondoIncial).Count > 0 Then
                                        ctlBuscadorCuentasFondos.Agrupamiento = String.Empty
                                        mostrarMensaje("Para seleccionar un banco de fondos los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        ctlBuscadorCuentasFondos.Agrupamiento = strFondoIncial
                                    End If
                                End If
                            End If
                        Else
                            mostrarMensaje("¡Para consultar el banco fondos debe seleccionar como minímo un registro!.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                            If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count > 0 Then
                                Dim strFondoIncial As String = String.Empty
                                If objVMTesorero.strTipoPagoPlus = GSTR_TRASLADOFONDOS And objVMTesorero.logEsFondosOYD Then
                                    strFondoIncial = objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).FirstOrDefault.strNombreCarteraColectivaDetalle
                                    If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strNombreCarteraColectivaDetalle <> strFondoIncial).Count > 0 Then
                                        ctlBuscadorCuentasFondos.Agrupamiento = String.Empty
                                        mostrarMensaje("Para seleccionar un banco de fondos los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        ctlBuscadorCuentasFondos.Agrupamiento = strFondoIncial
                                    End If
                                Else
                                    strFondoIncial = objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).FirstOrDefault.strNombreCarteraColectivaEncabezado
                                    If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strNombreCarteraColectivaEncabezado <> strFondoIncial).Count > 0 Then
                                        ctlBuscadorCuentasFondos.Agrupamiento = String.Empty
                                        mostrarMensaje("Para seleccionar un banco de fondos los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        ctlBuscadorCuentasFondos.Agrupamiento = strFondoIncial
                                    End If
                                End If
                            Else
                                mostrarMensaje("¡Para consultar el banco fondos debe seleccionar como minímo un registro!.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                Else
                    If objVMTesorero.strTipoNegocio = GSTR_ORDENGIRO.ToUpper And objVMTesorero.strTipoPagoPlus = GSTR_CARTERASCOLECTIVAS Then
                        If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                            If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count > 0 Then
                                Dim strFondoIncial = objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).FirstOrDefault.strNombreCarteraColectivaDetalle
                                If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strNombreCarteraColectivaDetalle <> strFondoIncial).Count > 0 Then
                                    ctlBuscadorCuentasFondos.Agrupamiento = String.Empty
                                    mostrarMensaje("Para seleccionar un banco de fondos los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                                Else
                                    ctlBuscadorCuentasFondos.Agrupamiento = strFondoIncial
                                End If
                            Else
                                mostrarMensaje("¡Para consultar el banco fondos debe seleccionar como minímo un registro!.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al seleccionar el banco de cuentas bancarias cartera", Me.Name, "BuscadorCuentasBancariasCarteras_GotFocus_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCarteraColectiva_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                objVMTesorero.CarteraColectiva = String.Empty
                objVMTesorero.DescripcionCarteraColectiva = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar la cartera colectiva", Me.Name, "btnLimpiarCarteraColectiva_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtReceptor1_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorReceptor1.AbrirBuscador()
    End Sub

    Private Sub txtCartera1_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorCartera1.AbrirBuscador()
    End Sub

    Private Sub txtReceptor2_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorReceptor2.AbrirBuscador()
    End Sub

    Private Sub txtReceptor3_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorReceptor3.AbrirBuscador()
    End Sub

    Private Sub txtCartera3_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorCartera3.AbrirBuscador()
    End Sub

    Private Sub txtBanco1_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorBanco1.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarBanco1_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                objVMTesorero.ConsultarSaldoBanco = False
                objVMTesorero.DescripcionBanco = String.Empty
                objVMTesorero.IdBanco = Nothing
                objVMTesorero.ConsultarSaldoBanco = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name, "btnLimpiarBanco1_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtBanco2_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        If Not IsNothing(objVMTesorero) Then
            If objVMTesorero.logEsFondosOYD Then
                ctlBuscadorCuentasFondos.AbrirBuscador()

            End If
        End If
    End Sub

    Private Sub btnLimpiarBanco2_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                objVMTesorero.DescripcionBancoFondo = String.Empty
                objVMTesorero.IdBancoFondo = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name, "btnLimpiarBanco2_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtBanco3_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        CuentasBancariasDestino.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarBanco3_Click_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                objVMTesorero.ConsultarSaldoBanco_FondosOYD = False
                objVMTesorero.DescripcionBanco_FondosOYD = String.Empty
                objVMTesorero.IdBanco_FondosOYD = Nothing
                objVMTesorero.ConsultarSaldoBanco_FondosOYD = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name, "btnLimpiarBanco3_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorCuentasBancariasCarteras_GotFocus_2(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                If objVMTesorero.strTipoNegocio = GSTR_ORDENFONDOS.ToUpper Then
                    If objVMTesorero.strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION Or
                                    objVMTesorero.strTipoPagoPlus = GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION Then
                        If Not IsNothing(objVMTesorero.ListaResultadosDocumentosRecibo) Then
                            If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count > 0 Then
                                If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).Count > 0 Then
                                    Dim strFondoFinal As String = objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True).FirstOrDefault.strCarteraColectivaFondos

                                    If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.Generar = True And i.strCarteraColectivaFondos <> strFondoFinal).Count > 0 Then
                                        ctlBuscadorCuentasFondos.Agrupamiento = String.Empty
                                        mostrarMensaje("Para seleccionar un banco de fondos los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        ctlBuscadorCuentasFondos.Agrupamiento = strFondoFinal
                                    End If
                                End If
                            Else
                                mostrarMensaje("¡Para consultar el banco fondos debe seleccionar como minímo un registro!.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    Else
                        If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                            If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count > 0 Then
                                Dim strFondoFinal As String = String.Empty
                                If objVMTesorero.strTipoPagoPlus = GSTR_TRASLADOFONDOS And objVMTesorero.logEsFondosOYD Then
                                    strFondoFinal = objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).FirstOrDefault.strNombreCarteraColectivaDetalle
                                    If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strNombreCarteraColectivaDetalle <> strFondoFinal).Count > 0 Then
                                        ctlBuscadorCuentasFondosDestino.Agrupamiento = String.Empty
                                        mostrarMensaje("Para seleccionar  banco de fondos destino los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        ctlBuscadorCuentasFondosDestino.Agrupamiento = strFondoFinal
                                    End If
                                Else
                                    strFondoFinal = objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).FirstOrDefault.strNombreCarteraColectivaDetalle
                                    If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strNombreCarteraColectivaDetalle <> strFondoFinal).Count > 0 Then
                                        ctlBuscadorCuentasFondosDestino.Agrupamiento = String.Empty
                                        mostrarMensaje("Para seleccionar  banco de fondos destino los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        ctlBuscadorCuentasFondosDestino.Agrupamiento = strFondoFinal
                                    End If
                                End If
                            Else
                                mostrarMensaje("¡Para consultar el banco fondos debe seleccionar como minímo un registro!.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                Else
                    If Not IsNothing(objVMTesorero.ListaResultadosDocumentos) Then
                        If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).Count > 0 Then
                            Dim strFondoIncial = objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True).FirstOrDefault.strNombreCarteraColectivaDetalle
                            If objVMTesorero.ListaResultadosDocumentos.Where(Function(i) i.Generar = True And i.strNombreCarteraColectivaDetalle <> strFondoIncial).Count > 0 Then
                                ctlBuscadorCuentasFondos.Agrupamiento = String.Empty
                                mostrarMensaje("Para seleccionar un banco de fondos los registros a generar deben tener la misma cartera colectiva.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                ctlBuscadorCuentasFondosDestino.Agrupamiento = strFondoIncial
                            End If
                        Else
                            mostrarMensaje("¡Para consultar el banco fondos debe seleccionar como minímo un registro!.", Application.Current.ToString, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If

            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al seleccionar el banco de cuentas bancarias cartera", Me.Name, "BuscadorCuentasBancariasCarteras_GotFocus_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda2(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrClaseControl = "banconacionalfondos" Then
                    objVMTesorero.DescripcionBancoFondoDestino = pobjItem.Nombre + " | " + pobjItem.CodItem
                    objVMTesorero.IdBancoFondoDestino = pobjItem.IdItem
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.Name, "New", "BuscadorGenerico_finalizoBusqueda", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarBanco2_Click_Destino(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMTesorero) Then
                objVMTesorero.DescripcionBancoFondoDestino = String.Empty
                objVMTesorero.IdBancoFondoDestino = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar", Me.Name, "btnLimpiarBanco2_Click_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'DEMC20180413
    Private Sub VerDetalleRecibo(sender As Object, e As RoutedEventArgs)
        Try
            Dim intTag As Integer = CInt(CType(sender, Button).Tag)
            ' If intTag <> objVMTesorero.ListaResultadosDocumentosRecibo.Count Then
            If IsNothing(objVMTesorero.SelectedDocumentosRecibo) Then
                If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intTag).Count() > 0 Then
                    objVMTesorero.SelectedDocumentosRecibo = objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intTag).First
                End If
                'objVMTesorero.SelectedDocumentosReciboDetalles.lngIDEncabezado = intTag.ToString
            ElseIf objVMTesorero.SelectedDocumentosRecibo.lngIDEncabezado <> intTag Then
                objVMTesorero.SelectedDocumentosRecibo = objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intTag).First
            End If

            objVMTesorero.VerDetalles("RECIBO")
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "HyperlinkButton_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'DEMC20180706
    Private Sub VerDetalleRecibofondos(sender As Object, e As RoutedEventArgs)
        Try
            Dim intTag As Integer = CInt(CType(sender, Button).Tag)
            If IsNothing(objVMTesorero.SelectedDocumentosRecibo) Then
                If objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intTag).Count() > 0 Then
                    objVMTesorero.SelectedDocumentosRecibo = objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intTag).First
                End If
            ElseIf objVMTesorero.SelectedDocumentosRecibo.lngIDEncabezado <> intTag Then
                objVMTesorero.SelectedDocumentosRecibo = objVMTesorero.ListaResultadosDocumentosRecibo.Where(Function(i) i.lngIDEncabezado = intTag).First
            End If

            objVMTesorero.VerDetalles("FONDOS")
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "HyperlinkButton_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        objVMTesorero.MostrarCartaGenerencia()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        Dim strUrlArchivo As String = CType(sender, Button).Tag
        Program.VisorArchivosWeb_DescargarURL(strUrlArchivo)
    End Sub

    Private Sub ComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMTesorero.ConsultarDocumentos()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "btnConsultar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMTesorero.ValidarGeneracionDocumentos()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "btnConsultar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnRechazar_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVMTesorero.RechazarDocumentos()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ejecutar el evento", Me.Name, "btnConsultar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
