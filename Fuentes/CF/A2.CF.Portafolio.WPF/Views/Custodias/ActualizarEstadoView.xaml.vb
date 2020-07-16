Imports Telerik.Windows.Controls
' Desarrollo       : TrasladarSaldoView
' Creado por       : Juan Carlos Soto Cruz.
' Fecha            : Agosto 17/2011 4:00 p.m  

#Region "Librerias"

Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

#End Region

#Region "Clases"


Partial Public Class ActualizarEstadoView
    Inherits UserControl

#Region "Declaraciones"

    'Dim vm As ActualizarEstadoViewModel
    'Private mlogInicializado As Boolean = False

#End Region

#Region "Inicializacion"

    Public Sub New()
        Me.DataContext = New ActualizarEstadoViewModel
InitializeComponent()
    End Sub

    'Private Sub TrasladarSaldoView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
    '    vm = CType(Me.DataContext, ActualizarEstadoViewModel)
    'End Sub

#End Region

#Region "Eventos"

    Private Sub btnCustodias_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'vm.ConsultarTitulosCustodias()
        CType(Me.DataContext, ActualizarEstadoViewModel).ConsultarTitulosCustodias()
    End Sub

    Private Sub btnAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'vm.ActualizarEstadoOperaciones()
        CType(Me.DataContext, ActualizarEstadoViewModel).ActualizarEstadoOperaciones()
    End Sub

#End Region

#Region "Buscadores"

    ''' <summary>
    ''' Se incluye modifica el método Buscar_finalizoBusqueda para que en el control Custodio Final se autocomplete con el ID Custodio Inicial.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' ID               : 000001
    ''' Descripción      : Se modifica la propiedad para que detecte el cambio de comitente que se realice mediante el buscador de clientes.
    ''' Fecha            : Febrero 19/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Febrero 22/2013 - Resultado Ok 
    ''' </history> 

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ActualizarEstadoViewModel)._mlogBuscarClienteInicial = False
            'CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitente = pobjComitente.CodigoOYD
            CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitente = pobjComitente.IdComitente
            'ID:000001
            'CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitenteFinal = pobjComitente.CodigoOYD
            CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitenteFinal = pobjComitente.IdComitente
            CType(Me.DataContext, ActualizarEstadoViewModel)._mlogBuscarClienteInicial = True
        End If
    End Sub

    ''' <summary>
    ''' Se modifica el método
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' ID               : 000002
    ''' Descripción      : Se modifica el método Buscar_finalizoBusquedaCF para validar que el comitente final sea mayor que el comitente inicial.
    ''' Fecha            : Febrero 21/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Febrero 22/2013 - Resultado Ok 
    ''' </history> 
    ''' 
    Private Sub Buscar_finalizoBusquedaCF(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ActualizarEstadoViewModel)._mlogBuscarClienteInicial = False
            'ID: 000002
            Dim IntIdComitente As Long
            IntIdComitente = CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitente

            If (IntIdComitente > pobjComitente.CodigoOYD) Then
                A2Utilidades.Mensajes.mostrarMensaje("El comitente final debe ser mayor o igual al comitente inicial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                'CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitenteFinal = pobjComitente.CodigoOYD
                CType(Me.DataContext, ActualizarEstadoViewModel).actualizarEstado.IdComitenteFinal = pobjComitente.IdComitente
            End If
            CType(Me.DataContext, ActualizarEstadoViewModel)._mlogBuscarClienteInicial = True
        End If
    End Sub

    Private Sub txtClienteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

#End Region

End Class

#End Region
