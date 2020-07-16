Public Class ReporteMensualUIAFView
    Inherits UserControl
#Region "Variables"

    Private mobjVM As ReporteMensualUIAFViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    Public Sub New()

        InitializeComponent()

        mobjVM = New ReporteMensualUIAFViewModel
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
            mobjVM = CType(Me.DataContext, ReporteMensualUIAFViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.lstTopico = mobjVM.DiccionarioCombosPantalla("RutaUIAFDivisas").ToList
            mobjVM.Topicoselected = mobjVM.lstTopico.FirstOrDefault
            Await mobjVM.inicializar()

        End If
    End Sub

#End Region

#Region "Eventos de controles"




#End Region
End Class
