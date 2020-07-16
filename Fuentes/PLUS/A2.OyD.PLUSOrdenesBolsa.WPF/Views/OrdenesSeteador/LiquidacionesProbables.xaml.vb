Imports Telerik.Windows.Controls
Imports C1.Silverlight
Imports System.ComponentModel
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesBolsa
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class LiquidacionesProbables
    Inherits UserControl
    Implements INotifyPropertyChanged

#Region "Variables privadas"

    Private _mobjVM As OrdenSeteadorViewModel

    Private chkCheckActual As CheckBox

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

#Region "Propiedades"

    Private _cwParent As Window
    Public Property cwParent() As Window
        Get
            Return _cwParent
        End Get
        Set(ByVal value As Window)
            _cwParent = value
        End Set
    End Property

    Private _lstLiquidaciones As New List(Of OyDPLUSOrdenesBolsa.Liquidacion)
    Public Property LiquidacionesProbables() As List(Of OyDPLUSOrdenesBolsa.Liquidacion)
        Get
            Return _lstLiquidaciones
        End Get
        Set(ByVal value As List(Of OyDPLUSOrdenesBolsa.Liquidacion))
            _lstLiquidaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LiquidacionesProbables"))
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        InitializeComponent()
        _mobjVM = Application.Current.Resources("VM")
        Me.DataContext = _mobjVM
    End Sub

#End Region

#Region "Eventos"


    Private Sub btnAceptar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        Dim cantidadFoliosSeleccionados As Double = 0
        If Not IsNothing(LiquidacionesProbables) Then
            cantidadFoliosSeleccionados = LiquidacionesProbables.Sum(Function(i) i.numCantidad).Value
        End If

        If _mobjVM.subTotal > 0 Then

            If _mobjVM.OrdenSeteadorSelected.strTipoNegocio = Program.TN_Simultaneas Then
                If _mobjVM.OrdenSeteadorSelected.dblCantidad <> cantidadFoliosSeleccionados Then
                    A2Utilidades.Mensajes.mostrarMensaje("En una orden simultanea el valor nominal no puede ser menor a la suma de las cantidades de los folios seleccionados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            ElseIf _mobjVM.OrdenSeteadorSelected.strTipoNegocio = Program.TN_REPO And _mobjVM.OrdenSeteadorSelected.strTipo = "V" Then
                If _mobjVM.OrdenSeteadorSelected.dblCantidad <> cantidadFoliosSeleccionados Then
                    A2Utilidades.Mensajes.mostrarMensaje("En una orden Repo venta la cantidad no puede ser diferente a la suma de las cantidades de los folios seleccionados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If
            'If _mobjVM.subTotal >= _mobjVM.OrdenSeteadorSelected.dblCantidad Then
            cwParent.DialogResult = True
            'Else
            '    Dim strMensaje As String = String.Format("El monto total de las operaciones {0:n2} debe ser igual o superar el monto de la orden {1:n2}.", _mobjVM.subTotal, _mobjVM.OrdenSeteadorSelected.dblCantidad)
            '    A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'End If
        Else
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar al menos una operación que tenga monto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancelar.Click
        'Me.DialogResult = CType(False, MessageBoxResult)
        'Me.Close()
        'cwParent = CType(Me.Parent, Window)
        _mobjVM.subTotal = 0
        cwParent.DialogResult = False
    End Sub

    Private Sub chkIncluir_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Dim chkIncluir As CheckBox = CType(sender, CheckBox)
        chkCheckActual = chkIncluir
        Dim liq As OyDPLUSOrdenesBolsa.Liquidacion = CType(dgLiquidacionesDisponibles.SelectedItem, OyDPLUSOrdenesBolsa.Liquidacion)
        Dim res As Boolean? = chkIncluir.IsChecked

        If (CBool(res)) Then

            If _mobjVM.subTotal >= _mobjVM.OrdenSeteadorSelected.dblCantidad Then
                mostrarMensaje("La cantidad de la orden ya esta cubierta por las operaciones seleccionadas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                chkCheckActual.IsChecked = False
            ElseIf ((_mobjVM.subTotal + liq.numCantidad) > (_mobjVM.OrdenSeteadorSelected.dblCantidad)) Then
                mostrarMensajePregunta("La suma de las operaciones excederá la Cantidad de la orden", Program.TituloSistema, "APROBARSOBREVALORLIQUIDACION", AddressOf terminoPreguntarAprobacionMonto, True, "¿Desea continuar?")
            Else
                LiquidacionesProbables.Add(liq)
            End If
        Else
            LiquidacionesProbables.Remove(liq)
        End If

        If LiquidacionesProbables.Sum(Function(i) i.numCantidad).Value > _mobjVM.OrdenSeteadorSelected.dblCantidad Then
            _mobjVM.subTotal = _mobjVM.OrdenSeteadorSelected.dblCantidad
        Else
            _mobjVM.subTotal = LiquidacionesProbables.Sum(Function(i) i.numCantidad).Value
        End If
    End Sub

    Private Sub dgLiquidacionesDisponibles_RowEditEnded(sender As Object, e As GridViewRowEditEndedEventArgs)
        Dim liq As OyDPLUSOrdenesBolsa.Liquidacion = CType(dgLiquidacionesDisponibles.SelectedItem, OyDPLUSOrdenesBolsa.Liquidacion)
        _mobjVM.subTotal = LiquidacionesProbables.Sum(Function(i) i.numCantidad)
    End Sub

    Private Sub LiquidacionesProbables_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cwParent = CType(Me.Parent, Window)
    End Sub

#End Region

#Region "Resultados asíncronos"

    Private Sub terminoPreguntarAprobacionMonto(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            Dim liq As OyDPLUSOrdenesBolsa.Liquidacion = CType(dgLiquidacionesDisponibles.SelectedItem, OyDPLUSOrdenesBolsa.Liquidacion)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    If objResultado.CodigoLlamado.ToUpper = "APROBARSOBREVALORLIQUIDACION" Then

                        If objResultado.DialogResult Then

                            liq.numMonto = _mobjVM.OrdenSeteadorSelected.dblCantidad - _mobjVM.subTotal

                            LiquidacionesProbables.Add(liq)

                            If LiquidacionesProbables.Sum(Function(i) i.numCantidad).Value > _mobjVM.OrdenSeteadorSelected.dblCantidad Then
                                _mobjVM.subTotal = _mobjVM.OrdenSeteadorSelected.dblCantidad
                            Else
                                _mobjVM.subTotal = LiquidacionesProbables.Sum(Function(i) i.numCantidad).Value
                            End If

                        Else
                            'decide devolver la inclusión de la actual operación, se quita el check
                            chkCheckActual.IsChecked = False
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al en la respuesta de confirmación asignando liquidaciones probables.", Me.ToString(), "terminoPreguntarAprobacionMonto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class