Imports A2.OyD.OYDServer.RIA.Web


Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl


Partial Public Class FlujoOrdenesView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mobjVM As FlujoOrdenesViewModel

#End Region

#Region "Inicializaciones"

    Public Sub New()

        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New FlujoOrdenesViewModel
InitializeComponent()
    End Sub

    Private Sub FlujoOrdenes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mobjVM Is Nothing Then
                mobjVM = CType(Me.DataContext, FlujoOrdenesViewModel)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "FlujoOrdenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

#End Region

#Region "Eventos controles"

    

#End Region

#Region "Eventos para asignar cliente y especie"

    ''' <summary>
    ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ''' </summary>
    ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ''' <remarks></remarks>
    Private Sub BuscadorClienteListaButon_comitenteAsignado(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Select Case pstrClaseControl.ToLower()
                    Case "idcomitente"

                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el comitente seleccionado", Me.Name, "BuscadorClienteLista_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl.ToLower()
                    Case "nemotecnico"

                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl.ToLower()
                    Case "nemotecnico"

                End Select
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarOrdenesBroken()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        mobjVM.GenerarTradeConfirmation()
    End Sub

    Private Sub Button_Click_2(sender As Object, e As RoutedEventArgs)
        mobjVM.EditarFraccionamientoOrden()
    End Sub

    Private Sub Button_Click_3(sender As Object, e As RoutedEventArgs)
        mobjVM.GenerarFraccionamiento()
    End Sub

    Private Sub Button_Click_4(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarFraccionamientoOrdenBlotter()
    End Sub

    Private Sub Button_Click_5(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarFraccionamientoOrdenPreMatched()
    End Sub

    Private Sub Button_Click_6(sender As Object, e As RoutedEventArgs)
        mobjVM.GenerarPreMATCHED()
    End Sub

    Private Sub Button_Click_7(sender As Object, e As RoutedEventArgs)
        mobjVM.DeshacerPreMATCHED()
    End Sub

Private Sub btnImportarArchivoRespuesta_CargarArchivo_1(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog.FileName) Then
                If Not IsNothing(Path.GetExtension(objDialog.FileName)) Then
                    If Path.GetExtension(objDialog.FileName).ToLower.Equals(".csv") Or Path.GetExtension(objDialog.FileName).ToLower.Equals(".xls") Or Path.GetExtension(objDialog.FileName).ToLower.Equals(".xlsx") Then
                        mobjVM.NombreArchivo = Path.GetFileName(objDialog.FileName)
                        mobjVM.ImportarArchivoRespueta()
                    Else
                        mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirRecibos_CargarArchivo_1", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region


    
End Class


