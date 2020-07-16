Imports System.ComponentModel

Public Class ExportacionMovDIANView
    Inherits UserControl
#Region "Variables"

    Private mobjVM As ExportacionMovDIANViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    Public Sub New()

        InitializeComponent()

        mobjVM = New ExportacionMovDIANViewModel
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
            mobjVM = CType(Me.DataContext, ExportacionMovDIANViewModel)
            mobjVM.NombreView = Me.ToString

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region



#Region "Eventos de controles"



    Private Sub RadComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        Dim obj = cbFormato.SelectedValue
        mobjVM.strDescripcion = (From r In mobjVM.DiccionarioCombosPantalla("FORMATO") Where r.Retorno = obj Select r.Descripcion).FirstOrDefault
    End Sub


    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pobjPersona As A2.OyD.OYDServer.RIA.Web.CPX_BuscadorPersonas)
        mobjVM.strCodigo = LTrim(RTrim(pobjPersona.strCodigoOyD))
        If Not IsNothing(mobjVM.strCodigo) Then
            mobjVM.logTodos = False
        End If
    End Sub


#End Region
End Class