Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClasesEspeciesView.xaml.vb
'Generado el : 01/20/2011 09:58:14
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ClasesEspeciesView
    Inherits UserControl
    Dim logInicializar As Boolean = True

    Public Sub New()
        Me.DataContext = New ClasesEspeciesViewModel
InitializeComponent()
    End Sub

    Private Sub ClasesEspecies_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        inicializar()
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            If logInicializar Then
                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
                logInicializar = False
            End If
        End If
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
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub


    ''' <History>
    ''' Modificado por   : Juan Carlos Soto Cruz. 
    ''' Descripción      : Se adiciona la lógica necesaria para que permita ingresar los caracteres correspondientes al teclado numérico.                   
    ''' Fecha            : Julio 23/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Julio 23/2013 - Resultado Ok 
    ''' </History>
    Private Sub txtClaseEspecie_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not ((e.Key > 47 And e.Key < 58) Or (e.Key > 95 And e.Key < 106)) Then
            e.Handled = True
        End If
    End Sub


End Class


