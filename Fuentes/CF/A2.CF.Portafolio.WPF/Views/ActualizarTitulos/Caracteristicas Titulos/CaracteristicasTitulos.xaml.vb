Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class CaracteristicasTitulos
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New CaracteristicasTitulosViewModel
InitializeComponent()
    End Sub

    Private Sub CaracteristicasTitulos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        '  CType(Me.DataContext, CaracteristicasTitulosViewModel).NombreView = Me.ToString

    End Sub
    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

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

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se adiciona la linea para modificar la propiedad Nombre.
    '''                    Se identifica asi:  JCS Marzo 08/2013 Adicion.
    ''' Fecha            : Marzo 08/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 08/2013 - Resultado Ok 
    ''' </history>
    Private Sub BuscadorGenerico_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, CaracteristicasTitulosViewModel).Busqueda.Comitente = pobjItem.IdComitente
            'JCS Marzo 08/2013 Adicion.
            CType(Me.DataContext, CaracteristicasTitulosViewModel).Busqueda.Nombre = pobjItem.Nombre
            ' FIN JCS Marzo 08/2013 Adicion.
        End If
    End Sub
    Private Sub Buscador_finalizoBusquedaespecies(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, CaracteristicasTitulosViewModel).Busqueda.Especie = pobjItem.Nemotecnico

        End If
    End Sub
    


    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(pstrNemotecnico As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                CType(Me.DataContext, CaracteristicasTitulosViewModel).TraerCaracteristicasNemotecnico(pobjEspecie)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, CaracteristicasTitulosViewModel).LiquidacionesCliente()
    End Sub

Private Sub BuscadorEspecie_nemotecnicoAsignado_1(pstrNemotecnico As String, pstrNombreEspecie As String)
        Try
            'If Not IsNothing(pstrNemotecnico) Then
            '    If Not CType(Me.DataContext, CaracteristicasTitulosViewModel).IsBusy Then
            '        CType(Me.DataContext, CaracteristicasTitulosViewModel).IsBusy = True
            '    Else
            '        CType(Me.DataContext, CaracteristicasTitulosViewModel).IsBusy = False
            '    End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


End Class
