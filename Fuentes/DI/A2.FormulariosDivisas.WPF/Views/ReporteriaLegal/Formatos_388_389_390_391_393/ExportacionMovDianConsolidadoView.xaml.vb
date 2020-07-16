'RABP20191002
Imports System.ComponentModel
Public Class ExportacionMovDianConsolidadoView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As ExportacionMovDianConsolidadoViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    Public Sub New()

        InitializeComponent()

        mobjVM = New ExportacionMovDianConsolidadoViewModel
        Me.DataContext = mobjVM
        mobjVM.IsBusy = True
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
            mobjVM = CType(Me.DataContext, ExportacionMovDianConsolidadoViewModel)
            mobjVM.NombreView = Me.ToString

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region


End Class
