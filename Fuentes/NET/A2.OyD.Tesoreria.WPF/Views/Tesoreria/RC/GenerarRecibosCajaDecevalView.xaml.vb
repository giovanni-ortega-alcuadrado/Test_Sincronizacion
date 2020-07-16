Imports Telerik.Windows.Controls
'Codigo Creado Por: Rafael Cordero
'Archivo: GenerarRecibosCajaDecevalView.vb
'Generado el : 08/07/2011 
'Propiedad de Alcuadrado S.A. 2011
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports Telerik.Windows.Controls.GridView

Partial Public Class GenerarRecibosCajaDecevalView
    Inherits UserControl

    Dim vm As GenerarRecibosCajaDecevalViewModel
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Try
            Me.DataContext = New GenerarRecibosCajaDecevalViewModel
            Me.Resources.Add("ViewModelPrincipal", Me.DataContext)
            InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la pantalla",
                     Me.ToString(), "GenerarRecibosCajaDecevalView", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ImportarTitulosValorizadosView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        vm = CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel)
        CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).NombreView = Me.ToString
        dtpFechaRecibo.SelectedDate = Now.Date
        dtpConsignarEl.SelectedDate = Nothing
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As System.Windows.RoutedEventArgs) Handles btnAceptar.Click
        vm.Aceptar()
    End Sub

    Private Sub BuscadorBancos_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico) Handles BuscadorBancos.finalizoBusqueda
        If Not pobjItem Is Nothing Then
            vm.RegistroTesoreriaSeleccionado.IDBanco = pobjItem.IdItem
            vm.NombreBanco = pobjItem.Nombre
        Else
            vm.RegistroTesoreriaSeleccionado.IDBanco = Nothing
            vm.NombreBanco = Nothing
        End If
    End Sub

    Private Sub btnReporte_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).EjecutarConsulta()
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcuentacontable"
                    CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).CuentaContable = pobjItem.IdItem
                Case "lngid"
                    CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).strCompaniaBanco = pobjItem.InfoAdicional03 'JCM20160216

            End Select
        End If
    End Sub

    Private Sub Buscador_Cliente_GotFocus(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If Not IsNothing(sender) Then
            CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = "id*" + CType(sender, A2ComunesControl.BuscadorClienteListaButon).Tag
        End If
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda_1(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).SelectedDetalle.lngIDComitenteAnterior = CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).SelectedDetalle.lngIDComitente
            CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).SelectedDetalle.lngIDComitente = pobjComitente.IdComitente
            CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).SelectedDetalle.Cliente = pobjComitente.Nombre
        End If
    End Sub




    Private Sub txtIDEspecie_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).Tipo = "T" Then

                ctlBuscadorEspeciesDemocratizadas.ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T

            Else
                ctlBuscadorEspeciesDemocratizadas.ClaseOrden = IIf(CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).Tipo = "C", 1, 2)
            End If

            ctlBuscadorEspeciesDemocratizadas.AbrirBuscador()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtIDEspecie_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlBuscadorEspeciesDemocratizadas_nemotecnicoAsignado(pstrNemotecnico As String, pstrNombreEspecie As String)
        Try
            CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).IDEspecie = pstrNemotecnico
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al recibir la respuesta del buscador.", Me.ToString, "ctlBuscadorEspeciesDemocratizadas_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)
        Try
            CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).IDEspecie = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al recibir la respuesta del buscador.", Me.ToString, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        Try
            CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).CargarListaPagosDeceval(True)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al consultar.", Me.ToString, "btnConsultar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlBuscadorEspeciesDemocratizadas_GotFocus(sender As Object, e As RoutedEventArgs)

        If CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).Tipo = "T" Then
            CType(sender, A2ComunesControl.BuscadorEspecieListaButon).ClaseOrden = A2ComunesControl.BuscadorEspecieViewModel.ClasesEspecie.T
        Else
            CType(sender, A2ComunesControl.BuscadorEspecieListaButon).ClaseOrden = IIf(CType(Me.DataContext, GenerarRecibosCajaDecevalViewModel).Tipo = "C", 1, 2)
        End If

    End Sub

    Private Sub RadGridView_RowLoaded(ByVal sender As Object, ByVal e As RowLoadedEventArgs)
        Try
            Dim objItemSeleccionado As OyDTesoreria.ListaDetalleTesoreria = Nothing

            Try
                objItemSeleccionado = CType(e.Row.Item, OyDTesoreria.ListaDetalleTesoreria)
            Catch ex1 As Exception

            End Try

            If Not IsNothing(objItemSeleccionado) Then

                If Program.VerificarRecurso("HabilitarResaltoColorDeceval", "SI") Then
                    If objItemSeleccionado.NroCodigosOyDxCliente > 1 Then
                        e.Row.Background = New BrushConverter().ConvertFromString("#85FFF2")
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class