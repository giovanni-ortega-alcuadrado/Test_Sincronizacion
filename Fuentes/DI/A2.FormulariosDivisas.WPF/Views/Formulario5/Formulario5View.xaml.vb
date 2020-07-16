Imports System.ComponentModel

Public Class Formulario5View
    Inherits UserControl
#Region "Variables"

    Private mobjVM As Formulario5ViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    Public Sub New()

        InitializeComponent()
        'mobjVM = Me.DataContext
        'mobjVM.IsBusy = False

        mobjVM = New Formulario5ViewModel
        Me.DataContext = mobjVM

    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
                cm.GridViewRegistros = datapager3

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, Formulario5ViewModel)
            mobjVM.NombreView = Me.ToString

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

    Private Sub NavegarAForma(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
            If mobjVM.EncabezadoSeleccionado.intNumeroDecl.ToString <> CType(sender, Button).Tag Then
                mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intNumeroDecl.ToString = CType(sender, Button).Tag).First
            End If
        Else
            mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intNumeroDecl.ToString = CType(sender, Button).Tag).First
        End If

        mobjVM.CambiarAForma()
    End Sub

    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
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

    Private Sub RadComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        mobjVM.PreguntarModificacion()
    End Sub

#End Region
End Class
