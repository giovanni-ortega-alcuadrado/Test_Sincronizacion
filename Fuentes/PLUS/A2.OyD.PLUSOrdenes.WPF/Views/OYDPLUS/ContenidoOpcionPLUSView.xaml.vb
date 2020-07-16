Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports A2OYDPLUSUtilidades

Partial Public Class ContenidoOpcionPLUSView
    Inherits UserControl
    Dim objVMContenido As ContenidoOpcionPLUSViewModel
    Dim _objModulo As Utilidades_ModulosUsuario
    Dim _mlogInicializacion As Boolean = False

    Public Sub New(ByVal pobjOpcionModulo As Utilidades_ModulosUsuario)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            InitializeComponent()

            objVMContenido = CType(Me.Resources("VMContenido"), ContenidoOpcionPLUSViewModel)
            objVMContenido.ModuloSeleccionado = pobjOpcionModulo
            objVMContenido.CrearControlModulo()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "ContenidoOpcionPLUSView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContenidoOpcionPLUSView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If _mlogInicializacion = False Then
                objVMContenido.viewContenido = Me
                objVMContenido.InicializarControles()
                _mlogInicializacion = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el componente del tab.", Me.Name, "", "ContenidoOpcionPLUSView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
