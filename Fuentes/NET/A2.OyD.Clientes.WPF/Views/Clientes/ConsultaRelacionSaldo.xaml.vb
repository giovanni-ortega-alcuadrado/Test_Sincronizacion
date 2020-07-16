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
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports Microsoft.VisualBasic.CompilerServices

Partial Public Class ConsultaRelacionSaldo
    Inherits Window
    Implements INotifyPropertyChanged
    Dim dcProxy As ClientesDomainContext

    Private DiccionarioCombosEspecificos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Private mdcProxy As UtilidadesDomainContext
    Private mdcProxyMaestro As MaestrosDomainContext
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty
    Public NombreConsecutivo1 As String = String.Empty
    Public DescripcionConsecutivo As String = String.Empty
    Public DescripcionBanco As String = String.Empty
    Public CuentaContableConsecutivo As String = String.Empty
    Public CuentaContable As String = String.Empty
    Private logConsecutivoBanco As String = String.Empty

    Public listasaldo As List(Of tblConsultaSaldoRes)
    Dim Csv As CancelaciondeSaldosView = New CancelaciondeSaldosView

    'Executes when the user navigates to this page.
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()



        Me.LayoutRoot.DataContext = Me
        strClaseCombos = "clientes_SaldosMenores"
        mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()

            mdcProxy = New UtilidadesDomainContext()
            mdcProxyMaestro = New MaestrosDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))

            mdcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyMaestro = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ClientesDomainContext.IClientesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 900)
        Try
            If Not Program.IsDesignMode() Then

            End If
            IsBusy = True
            System.Threading.Thread.Sleep(1000)
            Dim e = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("MODULOS")

            For Each ll In e
                ListaModulo.Add(New ItemModulo With {.Descripcion = ll.Descripcion, .Chequear = False, .ID = ll.ID})
            Next
            IsBusy = False
            ''// Ejecutar la carga de los combos
            ''mdcProxy.Load(mdcProxy.cargarCombosEspecificosQuery("clientes_SaldosMenores", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, String.Empty)




        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Preclientes.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

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
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    

#Region "Propiedades"
    Private _Fcorte As Date = Now
    Public Property Fcorte As Date
        Get
            Return _Fcorte
        End Get
        Set(value As Date)
            _Fcorte = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fcorte"))
        End Set
    End Property
    Private _Receptor As String = "T"
    Public Property Receptor As String
        Get
            Return _Receptor
        End Get
        Set(value As String)
            _Receptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptor"))
        End Set
    End Property
    Private _Sucursal As Integer = "-1"
    Public Property Sucursal As Integer
        Get
            Return _Sucursal
        End Get
        Set(value As Integer)
            _Sucursal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Sucursal"))
        End Set
    End Property
    Private _TipoLimite As Integer = 0
    Public Property TipoLimite As Integer
        Get
            Return _TipoLimite
        End Get
        Set(value As Integer)
            _TipoLimite = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoLimite"))
        End Set
    End Property
    Private _ValorLimite As Double = 10000
    Public Property ValorLimite As Double
        Get
            Return _ValorLimite
        End Get
        Set(value As Double)
            _ValorLimite = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ValorLimite"))
        End Set
    End Property
    Private _EstadoClientes As Integer = -1
    Public Property EstadoClientes As Integer
        Get
            Return _EstadoClientes
        End Get
        Set(value As Integer)
            _EstadoClientes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoClientes"))
        End Set
    End Property
    Private _TipoCartera As String = "0"
    Public Property TipoCartera As String
        Get
            Return _TipoCartera
        End Get
        Set(value As String)
            _TipoCartera = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoCartera"))
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
    Private _ListaModulo As ObservableCollection(Of ItemModulo) = New ObservableCollection(Of ItemModulo)
    Public Property ListaModulo() As ObservableCollection(Of ItemModulo)
        Get
            Return _ListaModulo
        End Get
        Set(ByVal value As ObservableCollection(Of ItemModulo))
            _ListaModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaModulo"))
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

    Private _NombreConsecutivo As String
    Public Property NombreConsecutivo As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                _NombreConsecutivo = value
                '' mdcProxyMaestro.Load(mdcProxyMaestro.BuscadorConsecutivosDocumentosQuery(NombreConsecutivo, Program.Usuario, Program.HashConexion), AddressOf TerminoBuscadorConsecutivoDocumento, String.Empty)
            End If

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))
        End Set
    End Property
    Private _NombreBanco As String
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
  

    'propiedad que tiene los campos que se seleccionaron en la lista de modulos
    Private _Todos As Boolean
    Public Property Todos As Boolean
        Get
            Return _Todos
        End Get
        Set(value As Boolean)
            _Todos = value
            If value Then
                For Each i In ListaModulo
                    i.Chequear = True
                Next
                Owner = "(Todos)"
            Else
                For Each i In ListaModulo
                    i.Chequear = False
                Next
                Owner = "(Seleccione las opciones)"
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Todos"))
        End Set
    End Property
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
#End Region
#Region "Metodos"
    Private Sub OKButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles OKButton.Click
        Try
            
            Dim strowner As String = String.Empty
            If Owner.Equals("(Seleccione las opciones)") Then

                For Each li In ListaModulosSeleccionadosUsuario
                        strowner = strowner + "," + li.ID
                Next
                ''If strowner = String.Empty Then
                ''    A2Utilidades.Mensajes.mostrarMensaje("Debe elegir un módulo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ''    Exit Sub
                ''End If
            Else
                strowner = Owner
            End If
            listasaldo = Nothing
            dcProxy.tblConsultaSaldoRes.Clear()
            IsBusy = True

            dcProxy.Load(dcProxy.ConsultaRelacionResumidaQuery(Receptor, Fcorte, strowner, TipoLimite, ValorLimite, EstadoClientes, Sucursal, Program.Usuario, TipoCartera, NombreConsecutivo, Program.HashConexion), AddressOf TerminoConsultar, Nothing)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de saldos", _
                                                 Me.ToString(), "TerminoConsultar", Application.Current.ToString(), Program.Maquina, ex.InnerException)

            IsBusy = False
        End Try

    End Sub
    ''' <summary>
    ''' metodo asincronico que retorna la lista de saldos
    ''' JBT20130316
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TerminoConsultar(ByVal lo As LoadOperation(Of tblConsultaSaldoRes))
        Try
            If Not lo.HasError Then
                listasaldo = dcProxy.tblConsultaSaldoRes.Where(Function(i) (i.Credito = 0 And i.Debito > 0) Or (i.Debito = 0 And i.Credito > 0)).ToList
                IsBusy = False
                DialogResult = True
                Me.Close()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de saldos", _
                                                 Me.ToString(), "TerminoConsultar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de saldos", _
                                                 Me.ToString(), "TerminoConsultar", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End Try

    End Sub

    'Private Sub TerminoBuscadorConsecutivoDocumento(ByVal lo As LoadOperation(Of ConsecutivosDocumento))
    '    If lo.HasError Then
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó consecutivo documento", Me.ToString(), "TerminoConsultarConsecutivoExistenteCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
    '    Else
    '        If (mdcProxyMaestro.ConsecutivosDocumentos.ToList.Where(Function(i) i.NombreConsecutivo = NombreConsecutivo).Count > 0) Then
    '            CuentaContableConsecutivo = mdcProxyMaestro.ConsecutivosDocumentos.ToList.Where(Function(i) i.NombreConsecutivo = NombreConsecutivo).FirstOrDefault.CuentaContable1
    '        End If
    '    End If
    'End Sub

    Private Sub CancelButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub
#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    ''Private Sub chkResgistradas_Checked(sender As Object, e As RoutedEventArgs)
    ''    Try
    ''        '// Ejecutar la carga de los combos

    ''        Dim IDSeleccionado As String = CType(sender, CheckBox).Tag

    ''        If ListaModulosSeleccionadosUsuario.Where(Function(i) i.ID = IDSeleccionado).Count = 0 Then
    ''            Dim objItemSelecionado As New ItemModulo

    ''            Dim objItemSeleccionadoOriginal = ListaModulo.Where(Function(i) i.ID = IDSeleccionado).First

    ''            objItemSelecionado.ID = objItemSeleccionadoOriginal.ID
    ''            objItemSelecionado.Descripcion = objItemSeleccionadoOriginal.Descripcion
    ''            objItemSelecionado.Chequear = True

    ''            ListaModulosSeleccionadosUsuario.Add(objItemSelecionado)
    ''        End If



    ''        Dim objListaConsecutivo As New List(Of OYDUtilidades.ItemCombo)
    ''        Dim objListaConsecutivo2 As New List(Of OYDUtilidades.ItemCombo)
    ''        Dim objObjeto1 As New OYDUtilidades.ItemCombo
    ''        'bancos
    ''        Dim objListaBancos As New List(Of OYDUtilidades.ItemCombo)
    ''        Dim objListabancos2 As New List(Of OYDUtilidades.ItemCombo)
    ''        Dim objObjeto2 As New OYDUtilidades.ItemCombo
    ''        ' Dim IdConsecutivo As String= CType(ListaModulosSeleccionadosUsuario.)
    ''        If Not IsNothing(ListaModulosSeleccionadosUsuario) Then
    ''            If DiccionarioCombosEspecificos.ContainsKey("SALDOMENORES") Then
    ''                For Each Li In ListaModulosSeleccionadosUsuario
    ''                    objListaConsecutivo = DiccionarioCombosEspecificos("SALDOMENORES").Where(Function(i) i.Retorno = Li.ID).ToList
    ''                    If Not IsNothing(objListaConsecutivo) Then
    ''                        For Each objLi In objListaConsecutivo
    ''                            objObjeto1 = New OYDUtilidades.ItemCombo
    ''                            objObjeto1.ID = objLi.ID
    ''                            objObjeto1.Descripcion = objLi.Descripcion
    ''                            objObjeto1.Retorno = objLi.Retorno
    ''                            objObjeto1.intID = objLi.intID
    ''                            objObjeto1.Categoria = objLi.Categoria
    ''                            objListaConsecutivo2.Add(objObjeto1)
    ''                        Next
    ''                    End If
    ''                Next
    ''            End If
    ''            If DiccionarioCombosEspecificos.ContainsKey("BANCOSMENORES") Then
    ''                For Each Li In ListaModulosSeleccionadosUsuario
    ''                    objListaBancos = DiccionarioCombosEspecificos("BANCOSMENORES").Where(Function(i) i.Retorno = Li.ID).ToList
    ''                    If Not IsNothing(objListaBancos) Then
    ''                        For Each objLi In objListaBancos
    ''                            objObjeto2 = New OYDUtilidades.ItemCombo
    ''                            objObjeto2.ID = objLi.ID
    ''                            objObjeto2.Descripcion = objLi.Descripcion
    ''                            objObjeto2.Retorno = objLi.Retorno
    ''                            objObjeto2.intID = objLi.intID
    ''                            objObjeto2.Categoria = objLi.Categoria
    ''                            objListabancos2.Add(objObjeto2)
    ''                        Next
    ''                    End If
    ''                Next
    ''            End If
    ''        End If

    ''        listConsecutivos = objListaConsecutivo2
    ''        listaBancos = objListabancos2

    ''        'objConsecutivosDocumentos = mdcProxyMaestro.BuscadorConsecutivosDocumentosQuery(NombreConsecutivo, Program.Usuario)

    ''    Catch ex As Exception
    ''        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
    ''                                            Me.ToString(), "TerminoConsultar", Application.Current.ToString(), Program.Maquina, ex.InnerException)
    ''    End Try
    ''End Sub
    ''Private Sub chkResgistradas_Unchecked(sender As Object, e As RoutedEventArgs)
    ''    Try
    ''        Dim IDSeleccionado As String = CType(sender, CheckBox).Tag

    ''        If ListaModulosSeleccionadosUsuario.Where(Function(i) i.ID = IDSeleccionado).Count > 0 Then
    ''            ListaModulosSeleccionadosUsuario.Remove(ListaModulosSeleccionadosUsuario.Where(Function(i) i.ID = IDSeleccionado).First)
    ''        End If
    ''    Catch ex As Exception
    ''        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
    ''                                            Me.ToString(), "TerminoConsultar", Application.Current.ToString(), Program.Maquina, ex.InnerException)
    ''    End Try
    ''End Sub
End Class
Public Class ItemModulo
    Implements INotifyPropertyChanged
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



    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class