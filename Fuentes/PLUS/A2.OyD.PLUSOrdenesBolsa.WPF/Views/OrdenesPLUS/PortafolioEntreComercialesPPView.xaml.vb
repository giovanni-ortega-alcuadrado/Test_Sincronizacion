Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'View para el registro portafolio entre receptores
'Mayo 19 de 2014
'Giovanny Velez
'-------------------------------------------------------------------------
Imports A2Utilidades.Mensajes
Imports A2ComunesControl


Partial Public Class PortafolioEntreComercialesPPView
    Inherits UserControl

#Region "Constantes y definición de variables"

    Dim objVMA2Utils As A2UtilsViewModel
    Dim objVMPortafolio As PortafolioEntreComercialesPPViewModel
    Dim logCambioValor As Boolean = False

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Método que se lanza cuando se inicializa el View
    ''' </summary>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Public Sub New()

        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Dim strClaseCombos As String = "Ord_OyDPLUS"

        objVMA2Utils = New A2UtilsViewModel(strClaseCombos, strClaseCombos)
        Me.Resources.Add("A2VM", objVMA2Utils)

        Me.DataContext = New PortafolioEntreComercialesPPViewModel
InitializeComponent()

    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se carga el View
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub PortafolioEntreComercialesPPView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        objVMPortafolio = CType(Me.DataContext, PortafolioEntreComercialesPPViewModel)

        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df

    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se cancela la edición de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se confirma el guardado de un registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        If Not IsNothing(df.ValidationSummary) Then
            If df.ValidationSummary.HasErrors Then
                df.CancelEdit()
            Else
                df.CommitEdit()
            End If
        End If
    End Sub

    ''' <summary>
    '''  Método que se lanza cuando se posiciona en un control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

        logCambioValor = False
    End Sub

    ''' <summary>
    '''  Método que se lanza cuando se tiene un cliente asignado
    ''' </summary>
    ''' <param name="pstrIdComitente"></param>
    ''' <param name="pobjComitente"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As String, pobjComitente AS OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMPortafolio) Then
                Me.objVMPortafolio.ComitenteSeleccionadoOYDPLUS = pobjComitente
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se borra o se limpia la información de un cliente
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMPortafolio) Then
                Me.objVMPortafolio.ComitenteSeleccionadoOYDPLUS = Nothing
                Me.objVMPortafolio.BorrarCliente = True
                Me.objVMPortafolio.BorrarCliente = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_EspecieAgrupadaAsignado(pobjNemotecnico As EspeciesAgrupadas, pstrNemotecnico As String)
        Try
            If Not IsNothing(objVMPortafolio) Then
                If Not IsNothing(pobjNemotecnico) Then
                    objVMPortafolio.OperacionesPorReceptorSelected.IdEspecie = pstrNemotecnico
                    objVMPortafolio.OperacionesPorReceptorSelected.TipoTasaFija = pobjNemotecnico.CodTipoTasaFija
                    objVMPortafolio.OperacionesPorReceptorSelected.EspecieEsAccion = pobjNemotecnico.EsAccion
                    objVMPortafolio.HabilitarControlesEspecieOYDPLUS(pobjNemotecnico)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se tiene una especie asignado
    ''' </summary>
    ''' <param name="pstrNemotecnico"></param>
    ''' <param name="pobjEspecie"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub ctlrEspecies_especieAsignada(pstrNemotecnico As String, pobjEspecie AS OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(objVMPortafolio) Then
                If Not IsNothing(pobjEspecie) Then
                    Me.objVMPortafolio.NemotecnicoSeleccionadoOYDPLUS = pobjEspecie
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se lanza cuando se borra o se limpia la información de una especie
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Por:Giovanny Velez Bolivar - 19/05/2014</remarks>
    Private Sub btnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMPortafolio) Then
                Me.objVMPortafolio.NemotecnicoSeleccionadoOYDPLUS = Nothing
                Me.objVMPortafolio.BorrarEspecie = True
                Me.objVMPortafolio.BorrarEspecie = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objVMPortafolio) Then
                    Select Case pstrClaseControl
                        Case "ReceptorB"
                            objVMPortafolio.OperacionesPorReceptorSelected.ReceptorB = pobjItem.IdItem
                            objVMPortafolio.OperacionesPorReceptorSelected.NombreReceptorB = pobjItem.IdItem & " - " & pobjItem.Nombre
                        Case "ReceptorB_Busqueda"
                            objVMPortafolio.cb.ReceptorB = pobjItem.IdItem
                            objVMPortafolio.cb.NombreReceptorB = pobjItem.IdItem & " - " & pobjItem.Nombre
                        Case "ReceptorA_Busqueda"
                            objVMPortafolio.cb.ReceptorA = pobjItem.IdItem
                            objVMPortafolio.cb.NombreReceptorA = pobjItem.IdItem & " - " & pobjItem.Nombre
                    End Select
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCalculo_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))
        Try
            If Not IsNothing(objVMPortafolio) Then
                logCambioValor = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar el valor del control.", Me.Name, "txtCalculo_ValueChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtCalculo_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMPortafolio) Then
                If logCambioValor Then
                    logCambioValor = False
                    objVMPortafolio.logCalcularValores = True
                    Await objVMPortafolio.CalcularValorOrden(objVMPortafolio.OperacionesPorReceptorSelected)
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cambiar de control.", Me.Name, "txtCalculo_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                objVMPortafolio.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                objVMPortafolio.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtValorNominal_ValueChanged(sender As Object, e As C1.WPF.PropertyChangedEventArgs(Of Double))

    End Sub


#End Region


End Class


