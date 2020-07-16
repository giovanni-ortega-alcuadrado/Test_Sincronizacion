Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class OficinasView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases OficinasView y OficinasViewModel
    ''' Pantalla Oficinas (OyD)
    ''' </summary>
    ''' <remarks>Ricardo Barrientos Pérez (Alcuadrado S.A.) - 27 de Julio 2016</remarks>
#Region "Variables"

    Private mobjVM As OficinasViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        'Try
        '    Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        'Catch ex As Exception
        '    MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        'End Try

        Me.DataContext = New OficinasViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, OficinasViewModel)
            mobjVM.NombreView = Me.ToString

            CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializar(String.Empty, String.Empty, False)
            'CType(Me.Resources("A2VM"), A2UtilsViewModel).DiccionarioCombosA2(String.Empty)
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
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
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
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "ciudades"
                    CType(Me.DataContext, OficinasViewModel).EncabezadoSeleccionado.IdCiudad = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, OficinasViewModel).EncabezadoSeleccionado.NombreCiudad = pobjItem.Nombre
                    CType(Me.DataContext, OficinasViewModel).EncabezadoSeleccionado.Departamento = pobjItem.CodigoAuxiliar
                    CType(Me.DataContext, OficinasViewModel).EncabezadoSeleccionado.Pais = pobjItem.InfoAdicional02


                Case "centroscosto"
                    CType(Me.DataContext, OficinasViewModel).EncabezadoSeleccionado.CentroCostos = pobjItem.IdItem

            End Select
        End If
    End Sub


End Class
