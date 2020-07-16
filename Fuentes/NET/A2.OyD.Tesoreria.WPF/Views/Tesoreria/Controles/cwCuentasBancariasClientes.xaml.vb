Imports Telerik.Windows.Controls
' Descripción:    Child Window creado para mostrar las Cuentas Bancarias de los Clientes
' Creado por:     Sebastian Londoño Benitez
' Fecha:          Junio 5/2013

Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwCuentasBancariasClientes
    Inherits Window
    Implements INotifyPropertyChanged

    Dim dcProxy As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext

    Public Sub New(ByVal strCodigoCliente As String)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            IsBusy = True
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If

            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("CuentasClientes", strCodigoCliente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasClientes, "Formatos")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "cwCuentasBancariasClientes.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#Region "Eventos de Controles"

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaCuentasClientes As List(Of OYDUtilidades.BuscadorGenerico)
    Public Property ListaCuentasClientes As List(Of OYDUtilidades.BuscadorGenerico)
        Get
            Return _ListaCuentasClientes
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorGenerico))
            _ListaCuentasClientes = value
            If Not IsNothing(value) Then
                CuentasClientesSelected = _ListaCuentasClientes.FirstOrDefault
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaCuentasClientes"))
        End Set
    End Property

    Private _CuentasClientesSelected As OYDUtilidades.BuscadorGenerico
    Public Property CuentasClientesSelected As OYDUtilidades.BuscadorGenerico
        Get
            Return _CuentasClientesSelected
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorGenerico)
            If Not IsNothing(value) Then
                _CuentasClientesSelected = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentasClientesSelected"))
        End Set
    End Property

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

#End Region

#Region "Resultados Asincronicos"

    ''' <summary>
    ''' Metodo encarga de recibir el set de datos de las cuentas bancarias de los clientes
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130604</remarks>
    Private Sub TerminoTraerCuentasClientes(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            IsBusy = False
            If Not lo.HasError Then
                ListaCuentasClientes = objProxy.BuscadorGenericos.ToList
                If ListaCuentasClientes.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron cuentas bancarias.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos", _
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(), _
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
