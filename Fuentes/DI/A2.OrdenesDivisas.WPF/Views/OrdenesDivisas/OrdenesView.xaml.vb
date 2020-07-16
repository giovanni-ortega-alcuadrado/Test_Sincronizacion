' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2.OyD.OYDServer.RIA.WEB
Imports System.ComponentModel

Public Class OrdenesView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As OrdenesViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    Public Sub New()

        InitializeComponent()
        'mobjVM = Me.DataContext
        'mobjVM.IsBusy = False

        mobjVM = New OrdenesViewModel
        Me.DataContext = mobjVM

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, OrdenesViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.objViewPrincipal = Me

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

    Private Sub NavegarAForma(sender As Object, e As RoutedEventArgs)
        'JAPC20180926: se incluye logica para actializar el nombre de la persona a mostrar en pantalla
        If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
            mobjVM.strNombreCompleto = mobjVM.EncabezadoSeleccionado.strNombre
            If mobjVM.EncabezadoSeleccionado.intID.ToString <> CType(sender, Button).Tag Then
                mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First
                mobjVM.strNombreCompleto = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First.strNombre
            End If
        Else
            mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First
            mobjVM.strNombreCompleto = mobjVM.ListaEncabezado.Where(Function(i) i.intID.ToString = CType(sender, Button).Tag).First.strNombre
        End If

        mobjVM.CambiarAForma()
    End Sub

    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub

    Private Sub cmdRefrescar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Me.mobjVM.RefrescarOrden()
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

    ''' <summary>
    ''' JAPC20180926: evento de respuesta del buscador para notificar que finalizao la buesqueda e
    ''' ingresar el codigo OyD en el EncabezadoEdicionSeleccionado
    ''' </summary>
    ''' <param name="pobjComitente"></param>
    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pobjComitente As CPX_BuscadorPersonas)
        'mobjVM.EncabezadoSeleccionado.intIDComitente = pobjComitente.strCodigoOyD
        mobjVM.EncabezadoEdicionSeleccionado.intIDComitente = pobjComitente.strCodigoOyD
        mobjVM.strNombreCompleto = pobjComitente.strNombreCompleto & pobjComitente.strCodigoOyD
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda_busquedaavanzada(pobjPersona As CPX_BuscadorPersonas)
        mobjVM.cb.intIDComitente = pobjPersona.strCodigoOyD
        mobjVM.cb.strNombre = pobjPersona.strNombreCompleto & pobjPersona.strCodigoOyD
    End Sub


#End Region
End Class

