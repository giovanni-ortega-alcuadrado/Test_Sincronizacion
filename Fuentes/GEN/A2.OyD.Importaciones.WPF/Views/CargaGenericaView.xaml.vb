Imports System.IO
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Windows.Data
Imports System.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2ComunesControl

Partial Public Class CargaGenericaView
    Inherits UserControl
    Public _vm As ImportacionArchivosGenericoViewModel
    Dim BloqueoDesbloqueoClientes As BloqueoDesbloqueoClientesView
    Dim ObjetoInformacion As ObjetoInformacionArchivo
    Dim strproceso As String

    Public Sub New()
        Me.Resources.Add("A2VM", New A2UtilsViewModel)
        InitializeComponent()
        _vm = New ImportacionArchivosGenericoViewModel
        Me.DataContext = _vm
    End Sub

    Private Sub ucBtnDialogoImportar_CargarArchivoGenerico(sender As ObjetoInformacionArchivo, pProceso As String)
        Try
            'se llama el popup especifico para esta importacion
            'SM20150916 Se corrige el error al seleccionar un archivo en una carga el sistema se queda cargando, no quita el indicador
            _vm.IsBusy = True
            If _vm.CargasArchivoSeleccionado.strModulo = "BloqueoDesbloqueoClientes" Then
                ObjetoInformacion = sender
                strproceso = pProceso
                BloqueoDesbloqueoClientes = New BloqueoDesbloqueoClientesView()
                AddHandler BloqueoDesbloqueoClientes.Closed, AddressOf CerroVentana
                Program.Modal_OwnerMainWindowsPrincipal(BloqueoDesbloqueoClientes)
                BloqueoDesbloqueoClientes.ShowDialog()
                Exit Sub
            End If
            Dim objDialog = CType(sender, ObjetoInformacionArchivo)

            If Not IsNothing(objDialog.pFile) Then
                'If objDialog.pFile.Extension.Equals(".csv") Or objDialog.pFile.Extension.Equals(".xls") Or objDialog.pFile.Extension.Equals(".xlsx") Then
                Dim strRutaArchivo As String = Path.GetFileName(objDialog.pFile.FileName)

                Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, ImportacionArchivosGenericoViewModel), strRutaArchivo, pProceso)
                Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
                viewImportacion.ShowDialog()
                'Else
                '    A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores, Program.Usuario)
                'End If
            End If
            _vm.IsBusy = False
        Catch ex As Exception
            _vm.IsBusy = False
            'SM20150916 fin
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "CargarArchivoGenerico", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ucBtnDialogoImportar_SubirArchivo(sender As Object, e As RoutedEventArgs)
        'SM20150916 _vm.IsBusy = True
    End Sub
    Private Sub CerroVentana()
        Try
            If BloqueoDesbloqueoClientes.DialogResult = True Then
                Dim objDialog = CType(ObjetoInformacion, ObjetoInformacionArchivo)

                If Not IsNothing(objDialog.pFile) Then
                    _vm.IsBusy = False
                    'If objDialog.pFile.Extension.Equals(".csv") Or objDialog.pFile.Extension.Equals(".xls") Or objDialog.pFile.Extension.Equals(".xlsx") Then
                    Dim strRutaArchivo As String = Path.GetFileName(objDialog.pFile.FileName)
                    CType(Me.DataContext, ImportacionArchivosGenericoViewModel).paccion = IIf(BloqueoDesbloqueoClientes.logBloqueo = False, "A", "B")
                    CType(Me.DataContext, ImportacionArchivosGenericoViewModel).lngidconcepto = BloqueoDesbloqueoClientes.IDMotivo
                    'Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, ImportacionArchivosGenericoViewModel), strRutaArchivo, strproceso)
                    'viewImportacion.Show()
                    CType(Me.DataContext, ImportacionArchivosGenericoViewModel).CargarArchivo(strproceso, strRutaArchivo)
                    'Else
                    '    A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores, Program.Usuario)
                    'End If
                Else
                    _vm.IsBusy = False
                End If
                'BloqueoDesbloqueoClientes.DialogResult = False
            Else
                _vm.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando la ventana", _
                                      Me.ToString(), "CerroVentana", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub
End Class
