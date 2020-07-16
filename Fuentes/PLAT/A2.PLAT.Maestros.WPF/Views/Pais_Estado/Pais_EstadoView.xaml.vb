Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class Pais_EstadoView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases Pais_EstadoView y Pais_EstadoViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 30 de Octubre 2017</remarks>
#Region "Variables"

    Private mobjVM As Pais_EstadoViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Propiedades"

    Public Shared ReadOnly IDRegistroEncabezadoProperty As DependencyProperty =
                        DependencyProperty.Register("IDRegistroEncabezado", GetType(Integer), GetType(Pais_EstadoView), New FrameworkPropertyMetadata() With {.BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioIDRegistroEncabezado)})
    Public Property IDRegistroEncabezado() As Integer
        Get
            Return CInt(GetValue(IDRegistroEncabezadoProperty))
        End Get
        Set(value As Integer)
            SetValue(IDRegistroEncabezadoProperty, value)
        End Set
    End Property

    Private Shared Sub CambioIDRegistroEncabezado(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, Pais_EstadoView)
        If Not IsNothing(myObject) Then
            myObject.mobjVM.IDRegistroEncabezado = myObject.IDRegistroEncabezado
        End If
    End Sub

    Public Shared ReadOnly DetalleGuardaIndependienteProperty As DependencyProperty =
                        DependencyProperty.Register("DetalleGuardaIndependiente", GetType(Boolean), GetType(Pais_EstadoView), New FrameworkPropertyMetadata() With {.BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioDetalleGuardaIndependiente)})
    Public Property DetalleGuardaIndependiente() As Boolean
        Get
            Return CBool(GetValue(DetalleGuardaIndependienteProperty))
        End Get
        Set(value As Boolean)
            SetValue(DetalleGuardaIndependienteProperty, value)
        End Set
    End Property

    Private Shared Sub CambioDetalleGuardaIndependiente(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, Pais_EstadoView)
        If Not IsNothing(myObject) Then
            myObject.mobjVM.DetalleGuardaIndependiente = myObject.DetalleGuardaIndependiente
        End If
    End Sub

    Public Shared ReadOnly HabilitarEdicionRegistroProperty As DependencyProperty =
                        DependencyProperty.Register("HabilitarEdicionRegistro", GetType(Boolean), GetType(Pais_EstadoView), New FrameworkPropertyMetadata() With {.BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioHabilitarEdicionRegistro)})
    Public Property HabilitarEdicionRegistro() As Boolean
        Get
            Return CBool(GetValue(HabilitarEdicionRegistroProperty))
        End Get
        Set(value As Boolean)
            SetValue(HabilitarEdicionRegistroProperty, value)
        End Set
    End Property

    Private Shared Sub CambioHabilitarEdicionRegistro(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, Pais_EstadoView)
        If Not IsNothing(myObject) Then
            myObject.mobjVM.HabilitarBotonesAcciones = myObject.HabilitarEdicionRegistro
        End If
    End Sub

    Public Shared ReadOnly HabilitarBotonGuardarYCopiarAnteriorProperty As DependencyProperty =
                        DependencyProperty.Register("HabilitarBotonGuardarYCopiarAnterior", GetType(Boolean), GetType(Pais_EstadoView), New FrameworkPropertyMetadata() With {.DefaultValue = True, .BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioHabilitarBotonGuardarYCopiarAnterior)})
    Public Property HabilitarBotonGuardarYCopiarAnterior() As Boolean
        Get
            Return CBool(GetValue(HabilitarBotonGuardarYCopiarAnteriorProperty))
        End Get
        Set(value As Boolean)
            SetValue(HabilitarBotonGuardarYCopiarAnteriorProperty, value)
        End Set
    End Property

    Private Shared Sub CambioHabilitarBotonGuardarYCopiarAnterior(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, Pais_EstadoView)
        If Not IsNothing(myObject) Then
            myObject.mobjVM.Habilitar_GuardarYCopiarAnterior = myObject.HabilitarBotonGuardarYCopiarAnterior
        End If
    End Sub

    Public Shared ReadOnly HabilitarBotonGuardarYCrearNuevoProperty As DependencyProperty =
                        DependencyProperty.Register("HabilitarBotonGuardarYCrearNuevo", GetType(Boolean), GetType(Pais_EstadoView), New FrameworkPropertyMetadata() With {.DefaultValue = True, .BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioHabilitarBotonGuardarYCrearNuevo)})
    Public Property HabilitarBotonGuardarYCrearNuevo() As Boolean
        Get
            Return CBool(GetValue(HabilitarBotonGuardarYCrearNuevoProperty))
        End Get
        Set(value As Boolean)
            SetValue(HabilitarBotonGuardarYCrearNuevoProperty, value)
        End Set
    End Property

    Private Shared Sub CambioHabilitarBotonGuardarYCrearNuevo(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, Pais_EstadoView)
        If Not IsNothing(myObject) Then
            myObject.mobjVM.Habilitar_GuardarYCrearNuevo = myObject.HabilitarBotonGuardarYCrearNuevo
        End If
    End Sub

    Public Shared ReadOnly ListaDetalleProperty As DependencyProperty =
                        DependencyProperty.Register("ListaDetalle", GetType(List(Of CPX_tblPais_Estado)), GetType(Pais_EstadoView), New FrameworkPropertyMetadata() With {.BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioListaDetalle)})
    Public Property ListaDetalle() As List(Of CPX_tblPais_Estado)
        Get
            Return CType(GetValue(ListaDetalleProperty), List(Of CPX_tblPais_Estado))
        End Get
        Set(value As List(Of CPX_tblPais_Estado))
            SetValue(ListaDetalleProperty, value)
        End Set
    End Property

    Private Shared Sub CambioListaDetalle(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, Pais_EstadoView)
        If Not IsNothing(myObject) Then

        End If
    End Sub

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New()
        Try
            InitializeComponent()
            mobjVM = New Pais_EstadoViewModel
            Me.DataContext = mobjVM
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False

                inicializar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, Pais_EstadoViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.viewListaPrincipal = Me

            mobjVM.inicializar()
        End If
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is Telerik.Windows.Controls.RadNumericUpDown Then
                CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Select(0, CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Manejadores error"

    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Private Sub VisualizarDetalle(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
            If mobjVM.EncabezadoSeleccionado.intID.ToString <> CType(sender, Button).Tag Then
                mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First
            End If
        Else
            mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First
        End If

        mobjVM.VisualizarRegistroDetalle()
    End Sub

End Class
