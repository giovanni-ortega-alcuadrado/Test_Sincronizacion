Imports Telerik.Windows.Controls
Imports A2Utilidades
Imports OyDCtl = A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports GalaSoft.MvvmLight.Messaging


Partial Public Class EjecutarScriptsView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mobjVM As EjecutarScriptsViewModel
    Private mlogInicializar As Boolean = True
    Private GrupoScriptFiltro As String = String.Empty

#End Region

#Region "Inicializacion"

    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.DataContext = New EjecutarScriptsViewModel
InitializeComponent()

            If Application.Current.Resources.Contains("QueryString") Then
                For Each item In Application.Current.Resources("QueryString").ToString().Split(CChar("&"))
                    If item.ToUpper.StartsWith("GRUPO") Then
                        GrupoScriptFiltro = item.Split(CChar("=")).Last
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mobjVM = CType(Me.DataContext, EjecutarScriptsViewModel)

                mlogInicializar = False

                If Not String.IsNullOrEmpty(Me.GrupoScriptFiltro) Then
                    mobjVM.logEsFiltroGrupo = True
                    mobjVM.GrupoScriptFiltro = Me.GrupoScriptFiltro
                End If

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
            mobjVM.NombreView = Me.ToString
            mobjVM.viewEjecutarScript = Me

            Await mobjVM.inicializar()
            mobjVM.iniciarFormularioGenerador()
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


#End Region


End Class
