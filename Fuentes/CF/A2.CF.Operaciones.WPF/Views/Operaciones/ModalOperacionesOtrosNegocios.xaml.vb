Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class ModalOperacionesOtrosNegocios
    Dim objFormulario As OperacionesOtrosNegociosView
    Dim objDatosModalOperacionesOtrosNegocios As Datos_ModalOperacionesOtrosNegocios
    Public intNroOrden As Integer
    Public strTipoOrden As String
    Public strTipoOperacion As String
    Public strTipoOrigen As String

    Public Sub New(ByVal pobjDatosModalOperacionesOtrosNegocios As Datos_ModalOperacionesOtrosNegocios)
        Try
            objDatosModalOperacionesOtrosNegocios = pobjDatosModalOperacionesOtrosNegocios
            ' This call is required by the designer.
            InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "ModalOrdenesAcciones", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ModalOrdenesAcciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objFormulario = New OperacionesOtrosNegociosView(True)
            AddHandler objFormulario.objVMOperaciones.TerminoConfigurarNuevoRegistro, AddressOf TerminoConfigurarNuevoRegistro
            AddHandler objFormulario.objVMOperaciones.TerminoGuardarRegistro, AddressOf TerminoGuardarRegistro
            GridPrincipal.Children.Clear()
            GridPrincipal.Children.Add(objFormulario)
            GridPrincipal.Height = objFormulario.Height
            GridPrincipal.Width = objFormulario.Width
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "ModalOrdenesAcciones_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoConfigurarNuevoRegistro()
        Try
            objFormulario.objVMOperaciones.EncabezadoSeleccionado.TipoNegocio = objDatosModalOperacionesOtrosNegocios.Clase
            objFormulario.objVMOperaciones.EncabezadoSeleccionado.TipoOrigen = "EX"
            objFormulario.objVMOperaciones.EncabezadoSeleccionado.TipoOperacion = objDatosModalOperacionesOtrosNegocios.TipoOperacion
            objFormulario.objVMOperaciones.BuscarCliente(objDatosModalOperacionesOtrosNegocios.IDComitente.Trim)
            objFormulario.objVMOperaciones.EncabezadoSeleccionado.Nemotecnico = objDatosModalOperacionesOtrosNegocios.Especie
            objFormulario.objVMOperaciones.BuscarEspecie(objDatosModalOperacionesOtrosNegocios.Clase, objDatosModalOperacionesOtrosNegocios.Especie)
            objFormulario.objVMOperaciones.EncabezadoSeleccionado.PrecioSucio = objDatosModalOperacionesOtrosNegocios.Precio
            objFormulario.objVMOperaciones.EncabezadoSeleccionado.Nominal = objDatosModalOperacionesOtrosNegocios.Nominal
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarNuevoRegistro", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoGuardarRegistro(ByVal plogGuardoRegistro As Boolean, ByVal plngIDOrden As Integer, ByVal pstrTipoOrden As String, ByVal pstrTipoOperacion As String, ByVal pstrTipoOrigen As String)
        Try
            If plogGuardoRegistro Then
                intNroOrden = plngIDOrden
                strTipoOrden = pstrTipoOrden
                strTipoOperacion = pstrTipoOperacion
                strTipoOrigen = pstrTipoOrigen
                Me.DialogResult = True
            Else
                Me.DialogResult = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoGuardarRegistro", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class

Public Class Datos_ModalOperacionesOtrosNegocios
    Public Property Clase As String
    Public Property TipoOperacion As String
    Public Property IDComitente As String
    Public Property Especie As String
    Public Property Precio As Double
    Public Property Nominal As Double

End Class
