Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwCodificacionContableValor
    Inherits Window
    Implements INotifyPropertyChanged
    Dim mdcProxy As CodificacionContableDomainContext

    Public Sub New(strChequeados As String, Habilitar As Boolean)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            IsBusy = True
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If
            strCWChequeados = strChequeados
            Editando = Habilitar
            mdcProxy.Load(mdcProxy.ConsultarConfiguracionContableXValorQuery(True, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion), AddressOf TerminoCargarChildWindow, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "cwCodificacionContableValor.New", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub

#Region "Métodos asincrónicos"

    Private Sub TerminoCargarChildWindow(ByVal lo As LoadOperation(Of CFCodificacionContable.ConfiguracionContableXValor))
        Try
            IsBusy = False
            If Not lo.HasError Then
                ListaTotalizados = mdcProxy.ConfiguracionContableXValors
                If ListaTotalizados.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron registros para totalizar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    If Not String.IsNullOrEmpty(strCWChequeados) Then
                        Dim strResultado = Split(strCWChequeados, ",")
                        For Each obj In strResultado
                            Dim strSeleccionado = ListaTotalizados.Where(Function(l) l.strRetorno = obj).FirstOrDefault
                            If Not IsNothing(strSeleccionado) Then
                                ListaTotalizados.Where(Function(l) l.Equals(strSeleccionado)).FirstOrDefault.logTotalizado = True
                            End If
                        Next
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de opciones", _
                                                 Me.ToString(), "TerminoCargarChildWindow", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cargando la lista", _
                                 Me.ToString(), "cwCodificacionContableValor.TerminoCargarChildWindow", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub

#End Region

#Region "Eventos de Controles"
    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub
#End Region

#Region "Propiedades"
    Private _IsBusy As Boolean = False
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _ListaTotalizados As EntitySet(Of CFCodificacionContable.ConfiguracionContableXValor)
    Public Property ListaTotalizados() As EntitySet(Of CFCodificacionContable.ConfiguracionContableXValor)
        Get
            Return _ListaTotalizados
        End Get
        Set(ByVal value As EntitySet(Of CFCodificacionContable.ConfiguracionContableXValor))
            _ListaTotalizados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaTotalizados"))
        End Set
    End Property

    Private _strCWChequeados As String = String.Empty
    Public Property strCWChequeados As String
        Get
            Return _strCWChequeados
        End Get
        Set(value As String)
            _strCWChequeados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCWChequeados"))
        End Set
    End Property

    Private _Editando As Boolean = False
    Public Property Editando() As Boolean
        Get
            Return _Editando
        End Get
        Set(ByVal value As Boolean)
            _Editando = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Editando"))
        End Set
    End Property

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
