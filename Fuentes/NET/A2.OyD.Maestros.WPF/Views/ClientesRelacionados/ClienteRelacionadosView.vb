Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class ClienteRelacionadosView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorClienteListaButon

    Public Sub New()
        Me.DataContext = New ClienteRelacionadosViewModel
InitializeComponent()
    End Sub

    Private Sub ClienteResponsable_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, ClienteRelacionadosViewModel).NombreView = Me.ToString
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteEncabezadoSelected.IdEncabezado = pobjItem.CodigoOYD
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteEncabezadoSelected.Nombre = pobjItem.Nombre
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteEncabezadoSelected.nrodocumento = pobjItem.NroDocumento
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteEncabezadoSelected.TipoIdentificacion = pobjItem.TipoIdentificacion
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteEncabezadoSelected.EstadoClienteD = pobjItem.CodEstado
            CType(Me.DataContext, ClienteRelacionadosViewModel).ConsultarComitente(pobjItem.CodigoOYD)
        End If
    End Sub

    Private Sub Buscador_Cliente_GotFocus(sender As Object, e As RoutedEventArgs)
        CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = "id*" + CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteEncabezadoSelected.IdEncabezado
    End Sub

    Private Sub Buscador_finalizoBusquedaClientesdetalles(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteRelacionadoSelected.IDComitente_Relacionado = pobjComitente.CodigoOYD
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteRelacionadoSelected.Nombre_Relacionado = pobjComitente.Nombre
            CType(Me.DataContext, ClienteRelacionadosViewModel).ClienteRelacionadoSelected.EstadoCliente_Relacionado = pobjComitente.Estado
            'vm.strCodigoOyDCargarPagosAFondos = pobjComitente.CodigoOYD
        End If
    End Sub


    Private Sub BuscadorGenerico_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "IDOrdenante"
                        'CType(Me.DataContext, ClientesViewModel).ClientesOrdenantesSelected.IDOrdenante = pobjItem.IdComitente
                        'CType(Me.DataContext, ClientesViewModel).ClientesOrdenantesSelected.Nombre = pobjItem.Nombre
                        'CType(Me.DataContext, ClientesViewModel).validaordenantes()

                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del Buscador clientes",
                                 Me.ToString(), "BuscadorGenerico_finalizoBusquedaClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'Private Sub Buscador_Cliente_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    'df.FindName("Buscador_Cliente").TipoVinculacion = "CA"
    '    'df.FindName("Buscador_Cliente").Agrupamiento = "CA," + CType(Me.DataContext, ClienteResponsableViewModel).ClienteAgrupadoSelected.TipoIdentificacion + "." + CType(Me.DataContext, ClienteResponsableViewModel).ClienteAgrupadoSelected.NroDocumento + "-"

    'End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcomitente"
                    CType(Me.DataContext, ClienteRelacionadosViewModel).cb.Comitente = pobjItem.CodigoOYD
            End Select
        End If
    End Sub

End Class


