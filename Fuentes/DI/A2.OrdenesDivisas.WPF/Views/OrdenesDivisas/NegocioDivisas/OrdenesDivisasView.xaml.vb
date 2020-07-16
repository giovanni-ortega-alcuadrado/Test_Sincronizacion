' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2.OyD.OYDServer.RIA.WEB

Public Class OrdenesDivisasView
    Inherits UserControl

    Public Event JsonConcatenadoDetalle(ByVal pstrDetalleJson As String, ByVal pstrLlamado As String)

    'SV20181023_AJUSTESORDENES evento para notificar cambio de propiedad,  'SV20190503
    Public Event NotificarPropiedad(ByVal strPropiedad As String, ByVal strValor As String, ByVal dblValor As Double?)


#Region "Variables"

    Private mobjVM As OrdenesDivisasViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Propiedades"

    Private Shared ReadOnly EntregarJsonGuardadoDep As DependencyProperty = DependencyProperty.Register("EntregarJsonGuardado", GetType(Boolean), GetType(OrdenesDivisasView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf EntregarJsonGuardadoChanged)))
    Public Property EntregarJsonGuardado As Boolean
        Get
            Return CBool(GetValue(EntregarJsonGuardadoDep))
        End Get
        Set(ByVal value As Boolean)
            SetValue(EntregarJsonGuardadoDep, value)
        End Set
    End Property


    '' <summary>
    '' JAPC20200408_CC20200306-04
    '' Propiedad dependiente para enviar dias cumplimiento forward desde encabezado orden a detalle
    '' </summary>
    Private Shared ReadOnly EntregarDiasCumplimientoForwardDep As DependencyProperty = DependencyProperty.Register("DiasCumplimientoForward", GetType(Int32), GetType(OrdenesDivisasView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf EntregarDiasCumplimientoForwardChanged)))
    Public Property DiasCumplimientoForward As Int32
        Get
            Return GetValue(EntregarDiasCumplimientoForwardDep)
        End Get
        Set(ByVal value As Int32)
            SetValue(EntregarDiasCumplimientoForwardDep, value)
        End Set
    End Property


    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Propiedad que cambia de valor cuando se desea que devuelva el valor de una propiedad del detalle a la entidad padre
    ''' </summary>
    Private Shared ReadOnly strEntregarValorPropiedadDep As DependencyProperty = DependencyProperty.Register("strEntregarValorPropiedad", GetType(String), GetType(OrdenesDivisasView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf EntregarValorPropiedadChanged)))
    Public Property strEntregarValorPropiedad As String
        Get
            Return GetValue(strEntregarValorPropiedadDep)
        End Get
        Set(ByVal value As String)
            SetValue(strEntregarValorPropiedadDep, value)
        End Set
    End Property

    Private Shared ReadOnly EditarRegistroDetalleProperty As DependencyProperty = DependencyProperty.Register("EditarRegistroDetalle", GetType(Boolean), GetType(OrdenesDivisasView), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf EditarRegistroDetalleChanged)))
    Public Property EditarRegistroDetalle As Boolean
        Get
            Return CBool(GetValue(EditarRegistroDetalleProperty))
        End Get
        Set(ByVal value As Boolean)
            SetValue(EditarRegistroDetalleProperty, value)
        End Set
    End Property



    Private Shared Sub EntregarJsonGuardadoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OrdenesDivisasView = DirectCast(d, OrdenesDivisasView)
        If Not IsNothing(obj.mobjVM) Then
            If obj.EntregarJsonGuardado Then
                obj.mobjVM.EntregarDetalleJson()
                obj.EntregarJsonGuardado = False
            End If
        End If
    End Sub


    ''' <summary>
    ''' JAPC20200408_CC20200306-04
    ''' Call back de Propiedad dependiente para enviar dias cumplimiento forward desde encabezado orden a detalle
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="e"></param>
    Private Shared Sub EntregarDiasCumplimientoForwardChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OrdenesDivisasView = DirectCast(d, OrdenesDivisasView)
        If Not IsNothing(obj.mobjVM) Then
            If obj.DiasCumplimientoForward > 0 Then
                obj.mobjVM.EntregarDiasCumplimientoForward(obj.DiasCumplimientoForward)
            End If
        End If
    End Sub

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Método que se ejecuta para indicar que se debe devolver el valor de una propiedad al encabezado
    ''' </summary>
    ''' <param name="d"></param>
    ''' <param name="e"></param>
    Private Shared Sub EntregarValorPropiedadChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OrdenesDivisasView = DirectCast(d, OrdenesDivisasView)
        If Not IsNothing(obj.mobjVM) Then
            If obj.strEntregarValorPropiedad <> "" Then
                obj.NotificarValorPropiedad(obj.strEntregarValorPropiedad)
            End If
        End If
    End Sub


    Private Shared Sub EditarRegistroDetalleChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim obj As OrdenesDivisasView = DirectCast(d, OrdenesDivisasView)
        If Not IsNothing(obj.mobjVM) Then
            If Not IsNothing(obj.EditarRegistroDetalle) Then
                If obj.EditarRegistroDetalle Then
                    obj.mobjVM.IniciarEditarRegistro()
                Else
                    obj.mobjVM.CancelarEditarRegistroDivisas()
                End If
            End If
        End If
    End Sub

    Public Sub EjecutarEventoDetalle()
        RaiseEvent JsonConcatenadoDetalle(mobjVM.JsonDetalle, Me.ToString)
    End Sub

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Lanzar el evento para notificar el cambio de propiedad
    ''' </summary>
    ''' <param name="strPropiedad"></param>
    Public Sub NotificarValorPropiedad(ByVal strPropiedad As String)
        Dim strValor As String = String.Empty, dblValor As Double
        If Not IsNothing(mobjVM.EncabezadoEdicionSeleccionado) Then
            If strPropiedad = "Moneda" Then
                strValor = mobjVM.EncabezadoEdicionSeleccionado.intIDMoneda.ToString
            End If
            'SV20190503
            If strPropiedad = "ValorNeto" Then
                dblValor = mobjVM.EncabezadoEdicionSeleccionado.dblValorNeto
            End If
            'RABP20190614
            If strPropiedad = "strReferencia" Then
                strValor = mobjVM.EncabezadoEdicionSeleccionado.strTipoReferencia
            End If

            If strPropiedad = "Multimoneda" Then
                strValor = mobjVM.logActivarMultimoneda
            End If

            'JAPC20200408_CC20200306-03_Ajuste divisas forward tipo de cumplimiento DELIVERY, el campo de FIXING debe estar fijo en T + 0, si el tipo de cumplimiento es NONDELIVERY si se debe activar el campo de FIXING
            If strPropiedad = "TipoCumplimiento" Then
                strValor = mobjVM.EncabezadoEdicionSeleccionado.strTipoCumplimiento
            End If


        End If
        RaiseEvent NotificarPropiedad(strPropiedad, strValor, dblValor)
    End Sub

    Public Shared ReadOnly ListaDetalleProperty As DependencyProperty =
                        DependencyProperty.Register("ListaDetalle", GetType(List(Of tblOrdenesDivisas)), GetType(OrdenesDivisasView), New FrameworkPropertyMetadata() With {.BindsTwoWayByDefault = True, .PropertyChangedCallback = (AddressOf CambioListaDetalle)})
    Public Property ListaDetalle() As List(Of tblOrdenesDivisas)
        Get
            Return CType(GetValue(ListaDetalleProperty), List(Of tblOrdenesDivisas))
        End Get
        Set(value As List(Of tblOrdenesDivisas))
            SetValue(ListaDetalleProperty, value)
        End Set
    End Property

    Private Shared Sub CambioListaDetalle(doj As DependencyObject, dp As DependencyPropertyChangedEventArgs)
        Dim myObject = DirectCast(doj, OrdenesDivisasView)
        If Not IsNothing(myObject) Then

        End If
    End Sub


#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New(objRegistroEncabezadoPadre As tblOrdenes)
        InitializeComponent()
        mobjVM = New OrdenesDivisasViewModel
        If objRegistroEncabezadoPadre.strProducto = "DERIVADOS" Then
            mobjVM.HabilitarPrecio = False
        End If
        mobjVM.RegistroEncabezadoPadre = objRegistroEncabezadoPadre

        Me.DataContext = mobjVM
        inicializar()
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

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, OrdenesDivisasViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.viewPrincipal = Me

            Await mobjVM.inicializar()
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
            ' Control de error del binding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
