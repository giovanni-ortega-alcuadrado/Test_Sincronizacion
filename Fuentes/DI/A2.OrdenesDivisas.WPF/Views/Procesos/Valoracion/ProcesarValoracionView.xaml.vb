'Ricardo Barrientos Perez
'Diciembre 03/ 2018
'RABP20181203 _APROBACIONORDENES 
Imports DocumentFormat.OpenXml.Spreadsheet
Imports SpreadsheetLight
Imports Telerik.Windows.Controls

Public Class ProcesarValoracionView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As ProcesarValoracionViewModel
    Private mlogInicializar As Boolean = True
#End Region

#Region "Inicializacion"
    Public Sub New()

        InitializeComponent()

        mobjVM = New ProcesarValoracionViewModel
        Me.DataContext = mobjVM


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
            mobjVM = CType(Me.DataContext, ProcesarValoracionViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.objViewPrincipal = Me

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub


    Private Sub cmdRefrescar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.RefrescarOperaciones()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar las ordenes", Me.Name, "cmdRefrescar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

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

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim doc As New SLDocument
        Dim Style As New SLStyle
        Style.Font.FontSize = 10
        Style.Font.Bold = True
        Style.Fill.SetPattern(PatternValues.Solid, System.Drawing.Color.CornflowerBlue, System.Drawing.Color.Blue)
        Dim val As Integer = 9

        For Each row In dgF1.Columns
            doc.SetCellValue(1, 1, IIf(Not IsNothing(row.Header.ToString), row.Header.ToString, "Sin definición"))
            doc.Filter("A1", "G1")
            doc.SetCellStyle(1, 1, Style)

        Next

        doc.HasFilter()

        doc.SaveAs("\\Lt-jpineda\alcuadrado\prueba24.xlsx")
    End Sub

#End Region
End Class
