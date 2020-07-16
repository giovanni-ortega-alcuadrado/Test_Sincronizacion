Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class ModalOrdenesRentaFija
    Dim objFormulario As OrdenesRFView
    Dim objDatosModalOrdenesAcciones As Datos_ModalOrdenesRentaFija
    Public intNroOrden As Integer
    Public strTipoOrden As String
    Public strTipoOperacion As String

    Public Sub New(ByVal pobjDatosModalOrdenesAcciones As Datos_ModalOrdenesRentaFija)
        Try
            objDatosModalOrdenesAcciones = pobjDatosModalOrdenesAcciones
            ' This call is required by the designer.
            InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "ModalOrdenesAcciones", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ModalOrdenesAcciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objFormulario = New OrdenesRFView(True)
            AddHandler objFormulario.mobjVM.TerminoConfigurarNuevoRegistro, AddressOf TerminoConfigurarNuevoRegistro
            AddHandler objFormulario.mobjVM.TerminoGuardarRegistro, AddressOf TerminoGuardarRegistro
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
            objFormulario.mobjVM.OrdenSelected.Tipo = objDatosModalOrdenesAcciones.TipoOperacion
            objFormulario.mobjVM.ComitenteOrden = objDatosModalOrdenesAcciones.IDComitente.Trim
            objFormulario.mobjVM.NemotecnicoOrden = objDatosModalOrdenesAcciones.Especie
            objFormulario.mobjVM.OrdenSelected.Precio = objDatosModalOrdenesAcciones.Precio
            objFormulario.mobjVM.OrdenSelected.PrecioStop = objDatosModalOrdenesAcciones.Precio
            objFormulario.mobjVM.OrdenSelected.Cantidad = objDatosModalOrdenesAcciones.Nominal
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarNuevoRegistro", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoGuardarRegistro(ByVal plogGuardoRegistro As Boolean, ByVal plngIDOrden As Integer, ByVal pstrTipoOrden As String, ByVal pstrTipoOperacion As String)
        Try
            If plogGuardoRegistro Then
                intNroOrden = plngIDOrden
                strTipoOrden = pstrTipoOrden
                strTipoOperacion = pstrTipoOperacion
                Me.DialogResult = True
            Else
                Me.DialogResult = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoGuardarRegistro", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class

Public Class Datos_ModalOrdenesRentaFija
    Public Property TipoOperacion As String
    Public Property IDComitente As String
    Public Property Especie As String
    Public Property Precio As Double
    Public Property Nominal As Double

End Class
