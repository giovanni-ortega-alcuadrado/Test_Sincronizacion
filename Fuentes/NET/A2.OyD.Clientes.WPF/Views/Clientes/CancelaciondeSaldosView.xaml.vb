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
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports Microsoft.VisualBasic.CompilerServices
Imports A2ComunesControl
Imports A2Utilidades.Mensajes

Partial Public Class CancelaciondeSaldosView
    Inherits UserControl
    Implements INotifyPropertyChanged
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Dim dcProxy As ClientesDomainContext
    Dim mdcProxy As UtilidadesDomainContext
    Dim mdcProxy1 As UtilidadesDomainContext
    Dim mdcProxy2 As UtilidadesDomainContext
    Dim mdcProxy3 As UtilidadesDomainContext
    Public _mlogBuscarBancos As Boolean = True
    Private mdcProxyMaestro As MaestrosDomainContext
    Private mdcProxyMaestro1 As MaestrosDomainContext
    Private DiccionarioCombosEspecificos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Dim consultaresumido As ConsultaRelacionSaldo
    Dim ultimoregistro As Integer
    Dim intSecuencia As Integer
    Dim intConsecutivo As Integer
    Dim listatrue As List(Of tblConsultaSaldoRes)
    Private objDetalleNota As tblConsultaSaldoRes
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty
    Dim CuentaConstable As String = String.Empty
    Dim NombreConsecutivo As String = String.Empty
    Dim logConcepto As Boolean = False
    Private logConsecutivoBanco As String = "NO"
    Private CuentaContableConsecutivo As String = String.Empty
    Private CodigoBanco As String = String.Empty
    Dim logRecargarLista As Boolean = True
    'Public objItemSelecionado As New ItemModulo



    Public Sub New()
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            Me.DataContext = Me


            strClaseCombos = "clientes_SaldosMenores"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            SaldosSelected.ExporContabilidad = True

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New ClientesDomainContext()
                mdcProxy = New UtilidadesDomainContext()
                mdcProxy1 = New UtilidadesDomainContext()
                mdcProxy2 = New UtilidadesDomainContext()
                mdcProxy3 = New UtilidadesDomainContext()
                mdcProxyMaestro = New MaestrosDomainContext()
                mdcProxyMaestro1 = New MaestrosDomainContext()
            Else
                dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                mdcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxy1 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxy2 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxy3 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyMaestro = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
                mdcProxyMaestro1 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ClientesDomainContext.IClientesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 450)
            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 450)
            DirectCast(mdcProxy1.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 450)
            DirectCast(mdcProxy2.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 450)
            DirectCast(mdcProxy3.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 450)
            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdcProxy.Load(mdcProxy.cargarCombosEspecificos_SinUsuarioQuery("Tesoreria_Notas", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarConsecutivosNotas, String.Empty)
            End If

            IsBusy = True
            System.Threading.Thread.Sleep(1000)

            mdcProxy2.Load(mdcProxy2.cargarCombosEspecificosQuery("clientes_SaldosMenores", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CancelacionSaldos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"
    ''' <summary>
    ''' Evento que marca todas los chekbox de la lista de notas de tesoreria
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RadioButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(ListaGenerarNotas) Then
            If ListaGenerarNotas.Count > 0 Then
                For Each li In ListaGenerarNotas
                    li.Chequear = True
                Next
            End If
        End If
    End Sub
    Private Sub Desmarcar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(ListaGenerarNotas) Then
            If ListaGenerarNotas.Count > 0 Then
                For Each li In ListaGenerarNotas
                    li.Chequear = False
                Next
            End If
        End If
    End Sub
    ''' <summary>
    ''' Evento que marca todas los chekbox nota debito de notas de tesoreria
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RadioButton_Click_1(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(ListaGenerarNotas) Then
            If ListaGenerarNotas.Count > 0 Then
                For Each li In ListaGenerarNotas
                    If li.Tipo = "DB" Then
                        li.Chequear = True
                    Else
                        li.Chequear = False
                    End If

                Next
            End If
        End If
    End Sub
    ''' <summary>
    ''' Evento que marca todas los chekbox nota credito de notas de tesoreria
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RadioButton_Click_2(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(ListaGenerarNotas) Then
            If ListaGenerarNotas.Count > 0 Then
                For Each li In ListaGenerarNotas
                    If li.Tipo = "CR" Then
                        li.Chequear = True
                    Else
                        li.Chequear = False
                    End If

                Next
            End If
        End If
    End Sub
    ''' <summary>
    ''' Evento que guarda las notas de tesorera marcadas en true
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Button_Click_4(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If IsNothing(ListaGenerarNotas) Then
            Exit Sub
        Else
            If ListaGenerarNotas.Count = 0 Then
                Exit Sub
            End If
        End If
        If validar() Then
            'C1.Silverlight.C1MessageBox.Show("Desea Generar las Notas de Tesoreria", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaGenerar)
            mostrarMensajePregunta("¿Desea Generar las Notas de Tesoreria?", _
                                   Program.TituloSistema, _
                                   "GENERARNOTAS", _
                                   AddressOf TerminoPreguntaGenerar, False)
        End If

    End Sub



    ''' <summary>
    ''' metodo de validaciones propias del formulario
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Public Function validar() As Boolean
        If SaldosSelected.NombreConsecutivo Is Nothing Or SaldosSelected.NombreConsecutivo = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe Seleccionar un consecutivo de Notas de Tesoreria", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If
        If SaldosSelected.Detalle Is Nothing Or SaldosSelected.Detalle = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe digitar la descripción de los detalles de Notas de Tesoreria", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If
        If IsNothing(SaldosSelected.FechaElaboracion) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar una fecha de registro para la Nota de Tesoreria", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If
        listatrue = ListaGenerarNotas.Where(Function(ic) ic.Chequear = True).ToList
        If listatrue.Count = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar una registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
            Exit Function
        End If
        Return True
    End Function
    Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
        If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then

            Select Case mobjBuscadorLst.CampoBusqueda
                Case "CentrosCosto"
                    SaldosSelected.CentroCostos = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case "IDCuentaContable"
                    SaldosSelected.CuentaContableClientes = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case "IDCuentaContableContraparte"
                    SaldosSelected.CuentaContableContraparte = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case "ConceptoTeso"
                    CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected.Detalle = mobjBuscadorLst.ItemSeleccionado.Nombre
                    CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected.IdConcepto = mobjBuscadorLst.ItemSeleccionado.IdItem
                Case Else
            End Select
        End If
    End Sub
    Private Sub Button_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("IDCuentaContable", "Cuentas Contables", "CuentasContables", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "", "", "")
        Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
        mobjBuscadorLst.ShowDialog()

    End Sub
    Private Sub Button_Click_1(sender As System.Object, e As System.Windows.RoutedEventArgs)
        mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("IDCuentaContableContraparte", "Cuentas Contables", "CuentasContables", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "", "", "")
        Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
        mobjBuscadorLst.ShowDialog()

    End Sub
    Private Sub Button_Click_2(sender As System.Object, e As System.Windows.RoutedEventArgs)
        'If Not IsNothing(CType(Me.DataContext, EspeciesViewModel).EspecieSelected) Then
        mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("CentrosCosto", "Centros de costo", "CentrosCosto", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, "", "", "")
        Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
        mobjBuscadorLst.ShowDialog()

        'End If
    End Sub
    ''' <summary>
    ''' evento para llamar el popup en el que se envian los parametros de la consulta
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>

    Private Sub Button_Click_3(sender As System.Object, e As System.Windows.RoutedEventArgs)
        IsBusy = True
        If Not IsNothing(ListaGenerarNotas) Then
            ListaGenerarNotas.Clear()
        End If
        If ListaModulosSeleccionadosUsuario.Count = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe elegir un módulo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            IsBusy = False
            Exit Sub
        End If
        consultaresumido = New ConsultaRelacionSaldo()
        consultaresumido.Owner = Owner
        consultaresumido.NombreConsecutivo = SaldosSelected.NombreConsecutivo
        consultaresumido.ListaModulosSeleccionadosUsuario = ListaModulosSeleccionadosUsuario
        AddHandler consultaresumido.Closed, AddressOf CerroVentana
        Program.Modal_OwnerMainWindowsPrincipal(consultaresumido)
        consultaresumido.ShowDialog()
        IsBusy = False

    End Sub
    Private Sub CerroVentana()
        Try
            If consultaresumido.DialogResult = True Then
                If consultaresumido.listasaldo.Count > 0 Then
                    If ListaGenerarNotas.Count > 0 Then
                        Exit Sub
                    End If
                    For Each li In consultaresumido.listasaldo
                        ListaGenerarNotas.Add(li)
                    Next
                End If
            End If
            mdcProxyMaestro.Load(mdcProxyMaestro.ConceptosConsecutivosFiltrarQuery(NombreConsecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarConcepto, String.Empty)


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas",
                                 Me.ToString(), "CerroVentana", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub chkResgistradas_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(ListaModulo) Then
                If ListaModulo.Where(Function(i) i.ID = CType(sender, CheckBox).Tag).Count > 0 Then
                    listamodulos.SelectedItem = ListaModulo.Where(Function(i) i.ID = CType(sender, CheckBox).Tag).First
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema",
                                                Me.ToString(), "chkResgistradas_GotFocus", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub

    Private Sub chkResgistradas1_Checked(sender As Object, e As RoutedEventArgs)
        Try
            listamodulos.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource()
            CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()
            If Not IsNothing(ListaModulo) Then
                If ListaModulo.Where(Function(i) i.ID = CType(sender, CheckBox).Tag).Count > 0 Then
                    listamodulos.SelectedItem = ListaModulo.Where(Function(i) i.ID = CType(sender, CheckBox).Tag).First
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema",
                                                Me.ToString(), "chkResgistradas1_Checked", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub
    Private Sub chkResgistradas1_Unchecked(sender As Object, e As RoutedEventArgs)
        Try
            listamodulos.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource()
            CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()
            listamodulos.SelectedItem = ListaModulo.Where(Function(i) i.ID = CType(sender, CheckBox).Tag).First
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema",
                                                Me.ToString(), "chkResgistradas1_Unchecked", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub

    Private Sub Button_Click_BuscadorLista(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected) Then
            mobjBuscadorLst = New A2ComunesControl.BuscadorGenericoLista("ConceptoTeso", "Conceptos Asociados", "ConceptoTeso", A2ComunesControl.BuscadorGenericoViewModel.EstadosItem.A, CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected.NombreConsecutivo, "", "")
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscadorLst)
            mobjBuscadorLst.ShowDialog()

        End If
    End Sub

    'Private Sub mobjBuscadorLst_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscadorLst.Closed
    '    If Not mobjBuscadorLst.ItemSeleccionado Is Nothing Then
    '        Select Case mobjBuscadorLst.CampoBusqueda.ToLower
    '            Case "conceptoteso"
    '                CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected.Detalle = mobjBuscadorLst.ItemSeleccionado.Nombre
    '            Case Else
    '        End Select
    '    End If
    'End Sub

#End Region
#Region "ResultadosAsincronicos"

    Private Sub TerminoCargarConsecutivosNotas(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As ObservableCollection(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = ""
        Try
            If Not lo.HasError Then
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New ObservableCollection(Of OYDUtilidades.ItemCombo)(mdcProxy.ItemCombos)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New ObservableCollection(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))
                        dicListaCombos.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    'If dicListaCombos.ContainsKey("ConsecutivosTesoreriaNotas") Then
                    '    Dim objListaConsecutivos As New List(Of OYDUtilidades.ItemCombo)

                    '    For Each li In dicListaCombos("ConsecutivosTesoreriaNotas")
                    '        objListaConsecutivos.Add(li)
                    '    Next
                    '    listConsecutivos = objListaConsecutivos
                    'End If
                End If

            Else
                lo.MarkErrorAsHandled()
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los consecutivos de las notas", _
                     Me.ToString(), "TerminoCargarConsecutivosNotas", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los consecutivos de las notas", _
                     Me.ToString(), "TerminoCargarConsecutivosNotas", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        Dim objListaBancos As New List(Of OYDUtilidades.ItemCombo)
        Dim objConsecutivos As New List(Of OYDUtilidades.ItemCombo)
        If Not lo.HasError Then
            Select Case lo.UserState
                Case "ModuloSubMoludoBancosTesoreria"
                    If Not String.IsNullOrEmpty(lo.Value) Then
                        logConsecutivoBanco = lo.Value
                    End If
            End Select
            If (logConsecutivoBanco = "NO") Then
                If DiccionarioCombosEspecificos.ContainsKey("BANCOSMENORES") Then
                    For Each li In DiccionarioCombosEspecificos("BANCOSMENORES")
                        objListaBancos.Add(li)
                    Next
                End If
                If DiccionarioCombosEspecificos.ContainsKey("SALDOMENORES") Then
                    For Each li In DiccionarioCombosEspecificos("SALDOMENORES")
                        objConsecutivos.Add(li)
                    Next
                End If

                listaBancos = objListaBancos
                listConsecutivos = objConsecutivos
            End If

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim strNombreCategoria As String = String.Empty

        Try
            If Not lo.HasError Then
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New List(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    DiccionarioCombosEspecificos = New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        'SLB cuando la categoria es objeto no se puede cargar la opcion de simultanea.
                        If Not strNombreCategoria.Equals("O_OBJETO") Then
                            objListaNodosCategoria = New List(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))
                        Else
                            objListaNodosCategoria = New List(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria And (Not ln.Descripcion.ToLower.Equals("simultaneas") Or Not ln.ID.Equals("SI"))))
                        End If
                        'dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                        DiccionarioCombosEspecificos.Add(NombreCategoria, objListaNodosCategoria)
                    Next
                End If

                Dim objListaModulos As New List(Of ItemModulo)

                If Not IsNothing(DiccionarioCombosEspecificos) Then

                    If DiccionarioCombosEspecificos.ContainsKey("MODULOS") Then
                        For Each li In DiccionarioCombosEspecificos("MODULOS")
                            objListaModulos.Add(New ItemModulo With {.Descripcion = li.Descripcion, .Chequear = False, .ID = li.ID})
                        Next
                    End If
                End If

                ListaModulo = objListaModulos

                mdcProxy1.Verificaparametro("ModuloSubMoludoBancosTesoreria", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ModuloSubMoludoBancosTesoreria")
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' metodo asincronico que guarda los encabezados de las notas de tesoreria
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TerminoPreguntaGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                listatrue = ListaGenerarNotas.Where(Function(ic) ic.Chequear = True And ic.Credito < 900000000000000.62 And ic.Debito < 900000000000000.62).ToList
                If listatrue.Count > 0 Then
                    ultimoregistro = listatrue.FirstOrDefault.NumeroRegistro
                End If
                For Each li In ListaGenerarNotas
                    If li.Chequear Then
                        If li.Credito > 900000000000000.62 Or li.Debito > 900000000000000.62 Then
                            A2Utilidades.Mensajes.mostrarMensaje("el débito o crédito en la fila " + li.NumeroRegistro.ToString + " excede el límite permitido para un valor monetario", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        Else
                            dcProxy.insertarNotaCli(SaldosSelected.NombreConsecutivo, SaldosSelected.FechaElaboracion, SaldosSelected.ExporContabilidad, Program.Usuario, Program.HashConexion, AddressOf Terminoguardarnota, li)
                            Exit For
                        End If
                    End If
                Next

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas", _
                        Me.ToString(), "TerminoPreguntaGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    ''' <summary>
    ''' metodo asincronico que guarda los detalles de tesoreria
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Terminoguardarnota(ByVal lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
                If lo.Value <> 0 Then
                    intSecuencia = 0
                    intConsecutivo = lo.Value
                    GuardarDetalleNota(1)
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas", _
                   Me.ToString(), "Terminoguardarnota", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas", _
                                 Me.ToString(), "Terminoguardarnota", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
        If intSecuencia = 0 Then
            IsBusy = False
        End If

    End Sub

    Private Sub TerminoConsultarConcepto(ByVal lo As LoadOperation(Of ConceptosConsecutivo))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó consecutivo documento", Me.ToString(), "TerminoConsultarConceptoCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
        Else
            If (mdcProxyMaestro.ConceptosConsecutivos.ToList.Where(Function(i) i.Consecutivo = NombreConsecutivo).Count) > 0 Then
                Editable = True
            Else
                Editable = False
            End If

        End If
    End Sub
    Private Sub TerminoBuscadorConsecutivoDocumento(ByVal lo As LoadOperation(Of ConsecutivosDocumento))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó consecutivo documento", Me.ToString(), "TerminoConsultarConsecutivoExistenteCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
        Else
            If (mdcProxyMaestro1.ConsecutivosDocumentos.ToList.Where(Function(i) i.NombreConsecutivo = SaldosSelected.NombreConsecutivo).Count > 0) Then
                CuentaContableConsecutivo = mdcProxyMaestro1.ConsecutivosDocumentos.ToList.Where(Function(i) i.NombreConsecutivo = SaldosSelected.NombreConsecutivo).FirstOrDefault.CuentaContable1
            End If
            If Not IsNothing(CuentaContableConsecutivo) Then
                SaldosSelected.CuentaContableClientes = String.Empty
                SaldosSelected.CuentaContableContraparte = String.Empty
            Else
                SaldosSelected.CuentaContableClientes = String.Empty
            End If
            SaldosSelected.NombreBanco = String.Empty
            SaldosSelected.IDBanco = Nothing


        End If
    End Sub

    Public Sub GuardarDetalleNota(intMovimiento As Integer)
        Try
            If intMovimiento = 1 Then
                objDetalleNota = (From c In ListaGenerarNotas Where c.Chequear = True Select c).FirstOrDefault
                If objDetalleNota.Credito > 900000000000000.62 Or objDetalleNota.Debito > 900000000000000.62 Then
                    A2Utilidades.Mensajes.mostrarMensaje("el débito o crédito en la fila " + objDetalleNota.NumeroRegistro.ToString + " excede el límite permitido para un valor monetario", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    intSecuencia = intSecuencia + 1
                    'JFSB 20180305 - Se omite el envío del concepto para el registro inicial
                    dcProxy.insertarDetalleNotaCli(IIf(objDetalleNota.Credito <> 0, "ND", "NC"), SaldosSelected.NombreConsecutivo, intConsecutivo, intSecuencia, objDetalleNota.IDComitente, IIf(objDetalleNota.Credito <> 0, objDetalleNota.Credito, IIf(objDetalleNota.Debito <> 0, objDetalleNota.Debito, 0)),
                                        SaldosSelected.CuentaContableClientes, SaldosSelected.Detalle, Nothing, "", SaldosSelected.CentroCostos, Program.Usuario, "", 0, Nothing, Program.Usuario, Program.HashConexion, AddressOf Terminoguardardetallenota, intMovimiento)
                End If

            Else
                intSecuencia = intSecuencia + 1

                'JFSB 20171130 Se envia el comitente en todas las ejecuciones, ya que se necesita el nit del cliente para la contrapartida
                'JFSB 20180305 - Se agrega el envío del concepto para el registro inicial

                If Not IsNothing(CodigoBanco) Then
                    dcProxy.insertarDetalleNotaCli(IIf(objDetalleNota.Credito <> 0, "NC", "ND"), SaldosSelected.NombreConsecutivo, intConsecutivo, intSecuencia, Nothing, IIf(objDetalleNota.Credito <> 0, objDetalleNota.Credito, IIf(objDetalleNota.Debito <> 0, objDetalleNota.Debito, 0)),
                                                   SaldosSelected.CuentaContableContraparte, "Contrapartida a la cancelación automática del cliente " & objDetalleNota.IDComitente.ToString, CInt(CodigoBanco), "", SaldosSelected.CentroCostos, Program.Usuario, "", 0, SaldosSelected.IdConcepto, Program.Usuario, Program.HashConexion, AddressOf Terminoguardardetallenota, intMovimiento)
                Else
                    dcProxy.insertarDetalleNotaCli(IIf(objDetalleNota.Credito <> 0, "NC", "ND"), SaldosSelected.NombreConsecutivo, intConsecutivo, intSecuencia, Nothing, IIf(objDetalleNota.Credito <> 0, objDetalleNota.Credito, IIf(objDetalleNota.Debito <> 0, objDetalleNota.Debito, 0)),
                                                   SaldosSelected.CuentaContableContraparte, "Contrapartida a la cancelación automática del cliente " & objDetalleNota.IDComitente.ToString, Nothing, "", SaldosSelected.CentroCostos, Program.Usuario, "", 0, SaldosSelected.IdConcepto, Program.Usuario, Program.HashConexion, AddressOf Terminoguardardetallenota, intMovimiento)
                End If


                ListaGenerarNotas.Remove(objDetalleNota)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el detalle de la nota", _
                       Me.ToString(), "GuardarDetalleNota", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Terminoguardardetallenota(ByVal lo As InvokeOperation(Of Integer))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas", _
                     Me.ToString(), "Terminoguardardetallenota", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            Else
                If (From c In ListaGenerarNotas Where c.Chequear = True Select c).Count = 0 Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("Se generó la Nota de Tesorería No " & intConsecutivo.ToString & ", con consecutivo " & SaldosSelected.NombreConsecutivo & " y con " & intSecuencia.ToString & " detalles, correspondientes a " & (intSecuencia / 2).ToString & " cliente(s).", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    recargarlista()
                Else
                    GuardarDetalleNota(IIf(lo.UserState = 1, 2, 1))
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la insercion de las notas", _
                                 Me.ToString(), "Terminoguardardetallenota", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()
        End Try
    End Sub
    Public Sub recargarlista()
        Try
            Dim a As List(Of tblConsultaSaldoRes)
            a = ListaGenerarNotas.Where(Function(li) li.Chequear = True And li.Credito < 900000000000000.62 And li.Debito < 900000000000000.62).ToList

            For Each li In a
                ListaGenerarNotas.Remove(li)
            Next

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recarga de la lista", _
                       Me.ToString(), "recargarlista", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
#End Region
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

    'Propiedad para cargar los Consecutivos de los usuarios.
    Private _listConsecutivos As List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("listConsecutivos"))
        End Set
    End Property

    Private _listaBancos As List(Of OYDUtilidades.ItemCombo)
    Public Property listaBancos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaBancos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaBancos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("listaBancos"))
        End Set
    End Property

    Private WithEvents _SaldosSelected As New Saldos
    Public Property SaldosSelected As Saldos
        Get
            Return _SaldosSelected
        End Get
        Set(value As Saldos)
            _SaldosSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SaldosSelected"))
        End Set
    End Property
    Private _NotasSelected As tblConsultaSaldoRes
    Public Property NotasSelected As tblConsultaSaldoRes
        Get
            Return _NotasSelected
        End Get
        Set(value As tblConsultaSaldoRes)
            _NotasSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NotasSelected"))
        End Set
    End Property
    Private _ListaGenerarNotas As New ObservableCollection(Of tblConsultaSaldoRes)
    Public Property ListaGenerarNotas As ObservableCollection(Of tblConsultaSaldoRes)
        Get
            Return _ListaGenerarNotas
        End Get
        Set(value As ObservableCollection(Of tblConsultaSaldoRes))
            _ListaGenerarNotas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaGenerarNotas"))
        End Set
    End Property


    Private _Editable As Boolean
    Public Property Editable As Boolean
        Get
            Return _Editable
        End Get
        Set(ByVal value As Boolean)
            'If Not IsNothing(value) Then
            _Editable = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Editable"))
        End Set
    End Property
    Private _Owner As String = "(Seleccione las opciones)"
    Public Property Owner As String
        Get
            Return _Owner
        End Get
        Set(value As String)
            _Owner = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Owner"))
        End Set
    End Property

    Private _ListaModulo As List(Of ItemModulo) = New List(Of ItemModulo)
    Public Property ListaModulo() As List(Of ItemModulo)
        Get

            Return _ListaModulo
        End Get
        Set(ByVal value As List(Of ItemModulo))
            _ListaModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaModulo"))
        End Set
    End Property

    Private WithEvents _ModuloSeleccionado As ItemModulo
    Public Property ModuloSeleccionado() As ItemModulo
        Get
            Return _ModuloSeleccionado
        End Get
        Set(ByVal value As ItemModulo)
            _ModuloSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ModuloSeleccionado"))
        End Set
    End Property


    Private _Chequear As Boolean
    Public Property Chequear As Boolean
        Get
            Return _Chequear
        End Get
        Set(ByVal value As Boolean)
            _Chequear = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Chequear"))
        End Set
    End Property


    Private _Descripcion As String
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))

        End Set
    End Property

    Private _ID As String
    Public Property ID As String
        Get
            Return _ID
        End Get
        Set(value As String)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _ListaModulosSeleccionadosUsuario As List(Of ItemModulo) = New List(Of ItemModulo)
    Public Property ListaModulosSeleccionadosUsuario() As List(Of ItemModulo)
        Get
            Return _ListaModulosSeleccionadosUsuario
        End Get
        Set(ByVal value As List(Of ItemModulo))
            _ListaModulosSeleccionadosUsuario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaModulosSeleccionadosUsuario"))
        End Set
    End Property

    Private _Todos As Boolean
    Public Property Todos As Boolean
        Get
            Return _Todos
        End Get
        Set(value As Boolean)
            _Todos = value
            Try
                logRecargarLista = False
                Dim objListaNueva As New List(Of ItemModulo)

                If value Then
                    For Each i In ListaModulo
                        i.Chequear = True
                    Next
                    Owner = "(Todos)"
                    For Each li In ListaModulo
                        Dim objItemSelecionado As New ItemModulo

                        objItemSelecionado.ID = li.ID
                        objItemSelecionado.Descripcion = li.Descripcion
                        objItemSelecionado.Chequear = True

                        objListaNueva.Add(objItemSelecionado)
                    Next
                Else
                    For Each i In ListaModulo
                        i.Chequear = False
                    Next
                    Owner = "(Seleccione las opciones)"
                End If

                ListaModulosSeleccionadosUsuario = objListaNueva

                logRecargarLista = True
                RecargarListaInformacion()
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar", _
                       Me.ToString(), "Todos", Application.Current.ToString(), Program.Maquina, ex)
            End Try
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Todos"))
        End Set
    End Property

#End Region
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    'Private Sub cboConsecutivos_SelectedItemChanged(sender As Object, e As C1.Silverlight.PropertyChangedEventArgs(Of Object))
    '    Try
    '        mdcProxyMaestro1.Load(mdcProxyMaestro1.BuscadorConsecutivosDocumentosQuery(SaldosSelected.NombreConsecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoBuscadorConsecutivoDocumento, String.Empty)
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el consecutivo", _
    '                   Me.ToString(), "cboConsecutivos_SelectedItemChanged", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub
    'Private Sub cboBancos_SelectedItemChanged(sender As Object, e As C1.Silverlight.PropertyChangedEventArgs(Of Object))
    '    CodigoBanco = SaldosSelected.NombreBanco
    'End Sub

    Private Sub _SaldosSelected_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _SaldosSelected.PropertyChanged
        Try
            Select Case e.PropertyName
                Case "NombreConsecutivo"
                    mdcProxyMaestro1.Load(mdcProxyMaestro1.BuscadorConsecutivosDocumentosQuery(SaldosSelected.NombreConsecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoBuscadorConsecutivoDocumento, String.Empty)
                Case "NombreBanco"
                    'JFSB 20180305 Se agrega la validación del campo IDBanco
                    If IsNothing(SaldosSelected.IDBanco) Then
                        CodigoBanco = Nothing
                    Else
                        CodigoBanco = SaldosSelected.IDBanco
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el consecutivo", _
                       Me.ToString(), "_SaldosSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ModuloSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _ModuloSeleccionado.PropertyChanged
        Try
            Select Case e.PropertyName
                Case "Chequear"
                    If logRecargarLista Then
                        If _ModuloSeleccionado.Chequear Then
                            If ListaModulosSeleccionadosUsuario.Where(Function(i) i.ID = _ModuloSeleccionado.ID).Count = 0 Then
                                Dim objItemSelecionado As New ItemModulo

                                Dim objItemSeleccionadoOriginal = ListaModulo.Where(Function(i) i.ID = _ModuloSeleccionado.ID).First

                                objItemSelecionado.ID = objItemSeleccionadoOriginal.ID
                                objItemSelecionado.Descripcion = objItemSeleccionadoOriginal.Descripcion
                                objItemSelecionado.Chequear = True

                                ListaModulosSeleccionadosUsuario.Add(objItemSelecionado)
                            End If
                        Else
                            If ListaModulosSeleccionadosUsuario.Where(Function(i) i.ID = _ModuloSeleccionado.ID).Count > 0 Then
                                ListaModulosSeleccionadosUsuario.Remove(ListaModulosSeleccionadosUsuario.Where(Function(i) i.ID = _ModuloSeleccionado.ID).First)
                            End If
                        End If

                        RecargarListaInformacion()
                    End If

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el consecutivo", _
                       Me.ToString(), "_SaldosSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub listamodulos_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

    End Sub

    Private Sub RecargarListaInformacion()
        Try
            SaldosSelected.CuentaContableClientes = String.Empty
            SaldosSelected.CuentaContableContraparte = String.Empty

            If logConsecutivoBanco = "NO" Then
                Exit Sub
            ElseIf logConsecutivoBanco = "SI" Then

                Dim objListaConsecutivo As New List(Of OYDUtilidades.ItemCombo)

                Dim objObjeto1 As New OYDUtilidades.ItemCombo
                'bancos
                Dim objListaBancos As New List(Of OYDUtilidades.ItemCombo)
                Dim objListabancos2 As New List(Of OYDUtilidades.ItemCombo)
                Dim objObjeto2 As New OYDUtilidades.ItemCombo
                Dim objListaConsecutivo2 As New List(Of OYDUtilidades.ItemCombo)

                ' Dim IdConsecutivo As String= CType(ListaModulosSeleccionadosUsuario.)
                If Not IsNothing(ListaModulosSeleccionadosUsuario) Then
                    If DiccionarioCombosEspecificos.ContainsKey("SALDOMENORES") Then
                        For Each Li In ListaModulosSeleccionadosUsuario
                            objListaConsecutivo = DiccionarioCombosEspecificos("SALDOMENORES").Where(Function(i) i.Retorno = Li.ID).ToList
                            If Not IsNothing(objListaConsecutivo) Then
                                For Each objLi In objListaConsecutivo
                                    objObjeto1 = New OYDUtilidades.ItemCombo
                                    objObjeto1.ID = objLi.ID
                                    objObjeto1.Descripcion = objLi.Descripcion
                                    objObjeto1.Retorno = objLi.Retorno
                                    objObjeto1.intID = objLi.intID
                                    objObjeto1.Categoria = objLi.Categoria
                                    objListaConsecutivo2.Add(objObjeto1)
                                Next
                            End If
                        Next
                    End If

                End If

                listConsecutivos = objListaConsecutivo2

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar las lista", _
                      Me.ToString(), "RecargarListaInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl

                Case "IDBanco"
                    CType(Me.DataContext, CancelaciondeSaldosView)._mlogBuscarBancos = False
                    CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected.IDBanco = pobjItem.IdItem
                    CType(Me.DataContext, CancelaciondeSaldosView).SaldosSelected.NombreBanco = pobjItem.Nombre
                    CType(Me.DataContext, CancelaciondeSaldosView)._mlogBuscarBancos = True
            End Select
        End If
    End Sub

End Class


Public Class Saldos
    Implements INotifyPropertyChanged

    Private _NombreConsecutivo As String
    <Display(Name:="Consecutivo")> _
    Public Property NombreConsecutivo As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _NombreConsecutivo = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))
        End Set
    End Property



    Private _Nombreconcepto As String
    <Display(Name:="Concepto")> _
    Public Property Nombreconcepto As String
        Get
            Return _Nombreconcepto
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _Nombreconcepto = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombreconcepto"))
        End Set
    End Property

    Private _NombreModulo As String
    <Display(Name:="Módulo")> _
    Public Property NombreModulo As String
        Get
            Return _NombreModulo
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _NombreModulo = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreModulo"))
        End Set
    End Property

    Private _NombreBanco As String
    <Display(Name:="")>
    Public Property NombreBanco As String
        Get
            Return _NombreBanco
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _NombreBanco = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreBanco"))
        End Set
    End Property

    'JFSB 20180305 - Se modifica la variable para que acepte valores nulos
    Private _IDBanco As Nullable(Of Integer)
    <Display(Name:="Banco")> _
    Public Property IDBanco As Nullable(Of Integer)
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Nullable(Of Integer))
            'If Not IsNothing(value) Then
            _IDBanco = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))
        End Set
    End Property

    Private _FechaElaboracion As DateTime = Now.Date
    <Display(Name:="Fecha de elaboración")> _
    Public Property FechaElaboracion As DateTime
        Get
            Return _FechaElaboracion
        End Get
        Set(ByVal value As DateTime)
            'If Not IsNothing(value) Then
            _FechaElaboracion = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaElaboracion"))
        End Set
    End Property
    Private _Detalle As String = "Cancelación automática de saldos de cartera"
    '<Display(Name:="Detalle")> _
    Public Property Detalle As String
        Get
            Return _Detalle
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _Detalle = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Detalle"))
        End Set
    End Property
    Private _CuentaContableClientes As String
    <Display(Name:=" ")> _
    Public Property CuentaContableClientes As String
        Get
            Return _CuentaContableClientes
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                _CuentaContableClientes = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaContableClientes"))
        End Set
    End Property
    Private _CuentaContableContraparte As String
    <Display(Name:=" ")> _
    Public Property CuentaContableContraparte As String
        Get
            Return _CuentaContableContraparte
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _CuentaContableContraparte = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaContableContraparte"))
        End Set
    End Property
    Private _CentroCostos As String
    <Display(Name:=" ")> _
    Public Property CentroCostos As String
        Get
            Return _CentroCostos
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _CentroCostos = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CentroCostos"))
        End Set
    End Property
    Private _ExporContabilidad As Boolean
    <Display(Name:="Exportable a contabilidad")> _
    Public Property ExporContabilidad As Boolean
        Get
            Return _ExporContabilidad
        End Get
        Set(ByVal value As Boolean)
            'If Not IsNothing(value) Then
            _ExporContabilidad = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ExporContabilidad"))
        End Set
    End Property
    Private _IdConcepto As Nullable(Of Integer)
    Public Property IdConcepto() As Nullable(Of Integer)
        Get
            Return _IdConcepto
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdConcepto"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


End Class


