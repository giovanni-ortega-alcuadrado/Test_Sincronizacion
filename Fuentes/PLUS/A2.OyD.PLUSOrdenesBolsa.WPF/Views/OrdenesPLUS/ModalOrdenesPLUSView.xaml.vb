Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class ModalOrdenesPLUSView
	Dim objVMOrdenes As OrdenesOYDPLUSViewModel
	Dim objFormulario As FormaOrdenesView
	Dim objDatosModalOrdenesPLUSView As Datos_ModalOrdenesPLUSView
    Public intNroOrden As Integer
    Public strTipoOrden As String
    Public strTipoOperacion As String

    Public Sub New(ByVal pobjDatosModalOrdenesPLUSView As Datos_ModalOrdenesPLUSView)
        Try
            objDatosModalOrdenesPLUSView = pobjDatosModalOrdenesPLUSView
            ' This call is required by the designer.
            InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "ModalOrdenesAcciones", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ModalOrdenesAcciones_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objVMOrdenes = New OrdenesOYDPLUSViewModel()
            Me.DataContext = objVMOrdenes
            objVMOrdenes.logEsModal = True
            objVMOrdenes.logCargoForma = True

			objFormulario = New FormaOrdenesView(objVMOrdenes)
			AddHandler objVMOrdenes.TerminoConfigurarNuevoRegistro, AddressOf TerminoConfigurarNuevoRegistro
			AddHandler objVMOrdenes.TerminoConfigurarReceptor, AddressOf TerminoConfigurarReceptor
			AddHandler objVMOrdenes.TerminoConfigurarCliente, AddressOf TerminoConfigurarCliente
			AddHandler objVMOrdenes.TerminoConfigurarEspecie, AddressOf TerminoConfigurarEspecie
			AddHandler objVMOrdenes.TerminoConfigurarConErrores, AddressOf TerminoConfigurarConErrores
			AddHandler objVMOrdenes.TerminoGuardarRegistro, AddressOf TerminoGuardarRegistro
			GridPrincipal.Children.Clear()
			GridPrincipal.Children.Add(objFormulario)
			GridPrincipal.Height = objFormulario.Height
			GridPrincipal.Width = Application.Current.MainWindow.ActualWidth * 0.96
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "ModalOrdenesAcciones_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoConfigurarNuevoRegistro()
		Try
			If objVMOrdenes.ListaReceptoresUsuario.Where(Function(i) i.CodigoReceptor = objDatosModalOrdenesPLUSView.Receptor).Count > 0 Then
				objVMOrdenes.OrdenOYDPLUSSelected.Receptor = objDatosModalOrdenesPLUSView.Receptor
			Else
				A2Utilidades.Mensajes.mostrarMensaje(String.Format("El receptor enviado '{0}' no esta dentro de los receptores que puede operar el usuario.", objDatosModalOrdenesPLUSView.Receptor), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Me.DialogResult = False
			End If
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarNuevoRegistro", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoConfigurarReceptor()
		Try
			If objVMOrdenes.ListaTipoNegocio.Where(Function(i) i.CodigoTipoNegocio = objDatosModalOrdenesPLUSView.TipoNegocio).Count > 0 Then
				objVMOrdenes.OrdenOYDPLUSSelected.TipoNegocio = objDatosModalOrdenesPLUSView.TipoNegocio

				If objVMOrdenes.DiccionarioCombosOYDPlus.ContainsKey("TIPOPRODUCTO") Then
					If objVMOrdenes.DiccionarioCombosOYDPlus("TIPOPRODUCTO").Where(Function(i) i.Retorno = objDatosModalOrdenesPLUSView.TipoProducto).Count > 0 Then
						objVMOrdenes.OrdenOYDPLUSSelected.TipoProducto = objDatosModalOrdenesPLUSView.TipoProducto
						objVMOrdenes.OrdenOYDPLUSSelected.TipoOperacion = objDatosModalOrdenesPLUSView.TipoOperacion

						objVMOrdenes.logCalcularValores = False
						objVMOrdenes.BuscarClienteOYDPLUSRestriccion(objDatosModalOrdenesPLUSView.IDComitente.Trim, objDatosModalOrdenesPLUSView.Receptor, objDatosModalOrdenesPLUSView.TipoNegocio, objDatosModalOrdenesPLUSView.TipoProducto)
						objVMOrdenes.logCalcularValores = True
					Else
						A2Utilidades.Mensajes.mostrarMensaje(String.Format("El receptor '{0}' no tiene autorizado el tipo de producto enviado '{1}'.", objDatosModalOrdenesPLUSView.Receptor, objDatosModalOrdenesPLUSView.TipoProducto), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
						Me.DialogResult = False
					End If
				End If
			Else
				A2Utilidades.Mensajes.mostrarMensaje(String.Format("El receptor '{0}' no tiene autorizado el tipo de negocio enviado '{1}'.", objDatosModalOrdenesPLUSView.Receptor, objDatosModalOrdenesPLUSView.TipoNegocio), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
				Me.DialogResult = False
			End If
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarNuevoRegistro", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoConfigurarCliente()
		Try
			objVMOrdenes.logCalcularValores = False
			objVMOrdenes.BuscarEspecieOYDPLUSRestriccion(objDatosModalOrdenesPLUSView.Especie, objDatosModalOrdenesPLUSView.TipoNegocio, objDatosModalOrdenesPLUSView.TipoNegocio, objDatosModalOrdenesPLUSView.TipoProducto)
			objVMOrdenes.logCalcularValores = True
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarNuevoRegistro", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoConfigurarEspecie()
		Try
			objVMOrdenes.logCalcularValores = False
			If String.IsNullOrEmpty(objVMOrdenes.OrdenOYDPLUSSelected.TipoOrden) Then
				objVMOrdenes.OrdenOYDPLUSSelected.TipoOrden = "I"
			End If
			objVMOrdenes.OrdenOYDPLUSSelected.Cantidad = objDatosModalOrdenesPLUSView.Nominal
			objVMOrdenes.OrdenOYDPLUSSelected.Precio = objDatosModalOrdenesPLUSView.Precio

			objFormulario.ctrlCliente.Visibility = Visibility.Collapsed
			objFormulario.btnLimpiarCliente.Visibility = Visibility.Collapsed
			objFormulario.ctlrEspecies.Visibility = Visibility.Collapsed
			objFormulario.btnLimpiarEspecie.Visibility = Visibility.Collapsed
			objFormulario.cboReceptores.IsEnabled = False
			objFormulario.cboTipoOrden.IsEnabled = False
			objFormulario.cboTipoProducto.IsEnabled = False
			objFormulario.cboTipoNegocio.IsEnabled = False
			objFormulario.cboTipoOperacion.IsEnabled = False
			objFormulario.txtCantidadAcciones.IsEnabled = False
			objFormulario.txtCantidadRentaFija.IsEnabled = False
			objFormulario.txtPrecioAcciones.IsEnabled = False
			objFormulario.txtPrecioRentaFija.IsEnabled = False

			objVMOrdenes.logCalcularValores = True
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarNuevoRegistro", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoConfigurarConErrores(ByVal pstrTipoError As String)
		Try
			If pstrTipoError = "CODIGOOYD" Then
				A2Utilidades.Mensajes.mostrarMensaje(String.Format("El código OYD '{0}' no esta configurado para el tipo de negocio y tipo de producto seleccionado.", objDatosModalOrdenesPLUSView.IDComitente.Trim), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			ElseIf pstrTipoError = "ESPECIE" Then
				A2Utilidades.Mensajes.mostrarMensaje(String.Format("La Especie '{0}' no esta configurado para el tipo de negocio y tipo de producto seleccionado.", objDatosModalOrdenesPLUSView.Especie), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
			End If
			Me.DialogResult = False
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoConfigurarConErrores", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub TerminoGuardarRegistro(ByVal plogGuardoRegistro As Boolean, ByVal plngIDOrden As Integer)
		Try
			If plogGuardoRegistro Then
				intNroOrden = plngIDOrden
				strTipoOrden = objDatosModalOrdenesPLUSView.TipoNegocio
				strTipoOperacion = objDatosModalOrdenesPLUSView.TipoOperacion
				Me.DialogResult = True
			Else
				Me.DialogResult = False
			End If
		Catch ex As Exception
			mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "TerminoGuardarRegistro", Program.Maquina, ex, Program.RutaServicioLog)
		End Try
	End Sub

	Private Sub BtnGuardar_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenes.ActualizarRegistro()
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub
End Class

Public Class Datos_ModalOrdenesPLUSView
	Public Property Receptor As String
	Public Property TipoNegocio As String
	Public Property TipoProducto As String
	Public Property TipoOperacion As String
	Public Property IDComitente As String
	Public Property Especie As String
	Public Property Precio As Double
	Public Property Nominal As Double
End Class
