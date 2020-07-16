Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OYD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.IO
Imports System.Text

Partial Public Class ImportarRecibosCajaRecaudosView
    Inherits UserControl
    Dim vm As ImportarRecibosCajaRecaudosViewModel

#Region "Inicialización"

    Public Sub New()
        InitializeComponent()
        Me.DataContext = New ImportarRecibosCajaRecaudosViewModel
        ucbtnCargar.Proceso = CSTR_NOMBREPROCESO_IMP_RC_RECAUDO
    End Sub


    Private Sub ImportarRecibosCajaRecaudosView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, ImportarRecibosCajaRecaudosViewModel)
        CType(Me.DataContext, ImportarRecibosCajaRecaudosViewModel).NombreView = Me.ToString
    End Sub

#End Region

#Region "Eventos"

    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        vm.strNombrearchivo = Path.GetFileName(objDialog.FileName)
    End Sub

    Private Sub txtBanco_KeyDown(sender As Object, e As KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtBanco_LostFocus(sender As Object, e As RoutedEventArgs)
        If Not String.IsNullOrEmpty(DirectCast(sender, TextBox).Text) Then
            CType(Me.DataContext, ImportarRecibosCajaRecaudosViewModel).BuscarBancos(DirectCast(sender, TextBox).Text)
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDBanco"
                    CType(Me.DataContext, ImportarRecibosCajaRecaudosViewModel).intCodigoBanco = pobjItem.IdItem
                    CType(Me.DataContext, ImportarRecibosCajaRecaudosViewModel).strNombreBanco = pobjItem.Nombre
            End Select
        End If
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        vm.CargarArchivo()
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Dim objArchivoSeleccionado As OyDImportaciones.Archivo = Nothing
        For Each li In vm.ListaArchivosGuardados
            If li.Ruta = CType(sender, Button).Tag Then
                objArchivoSeleccionado = li
                Exit For
            End If
        Next
        If Not IsNothing(objArchivoSeleccionado) Then
            vm.BorrarArchivo(objArchivoSeleccionado)
        End If
    End Sub

#End Region

End Class
