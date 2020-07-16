
Partial Public Class ConsultasView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases ConsultasView y ConsultasViewModel
    ''' Pantalla ChoquesTasasInteres (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As ConsultasViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & Environment.NewLine & Environment.NewLine & "Se generó el error ''" & ex.Message & "''" & Environment.NewLine & Environment.NewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.DataContext = New ConsultasViewModel
            'Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & Environment.NewLine & Environment.NewLine & "Se generó el error ''" & ex.Message & "''" & Environment.NewLine & Environment.NewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma

                If Application.Current.Resources.Contains("QueryString") Then
                    For Each item In Application.Current.Resources("QueryString").ToString().Split("&")
                        If item.ToUpper.StartsWith("H") Then
                            Me.Height = item.Split("=").Last
                        ElseIf item.ToUpper.StartsWith("W") Then
                            Me.Width = item.Split("=").Last
                        End If
                    Next
                    Me.UpdateLayout()
                End If


                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ConsultasViewModel)
            mobjVM.NombreView = Me.ToString

            'Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

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
            End If
        Catch ex As Exception

        End Try
    End Sub
#End Region

End Class

