Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorReceptorLista
    Inherits Window

#Region "constantes"

    Private MSTR_RECURSO_TIPO_SELECCION As String = "A2.OYD_TIPO_SEL_BUSCADOR"

#End Region

#Region "Propiedades"

    Private _mobjItem As OyDPLUSOrdenesDerivados.ReceptoresBusqueda = Nothing
    Public ReadOnly Property ItemSeleccionado As OyDPLUSOrdenesDerivados.ReceptoresBusqueda
        Get
            Return (_mobjItem)
        End Get
    End Property

    Private _mstrCampoBusqueda As String = String.Empty
    Public ReadOnly Property CampoBusqueda As String
        Get
            Return (_mstrCampoBusqueda)
        End Get
    End Property

    Private _mstrTipoItem As String = String.Empty
    Public ReadOnly Property TipoItem As String
        Get
            Return (_mstrTipoItem)
        End Get
    End Property
#End Region

#Region "Inicializacion"

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pstrCampoBusqueda As String, ByVal pstrEtiqueta As String)
        InitializeComponent()

        Try
            If pstrCampoBusqueda Is Nothing Then
                pstrCampoBusqueda = String.Empty
            End If

            If pstrEtiqueta Is Nothing Then
                pstrEtiqueta = String.Empty
            End If

            '// Verificar si el usurio tiene una preferencia para el comportamiento del buscador cuando se selecciona un elemento
            If Application.Current.Resources.Contains(MSTR_RECURSO_TIPO_SELECCION) Then
                If Application.Current.Resources(MSTR_RECURSO_TIPO_SELECCION).ToString = "S" Then
                    Me.chkCierre.IsChecked = True
                Else
                    Me.chkCierre.IsChecked = False
                End If
            Else
                Me.chkCierre.IsChecked = False
            End If

            Me._mstrCampoBusqueda = pstrCampoBusqueda
            Me.ctlBuscador.BuscarAlIniciar = True
            If Not pstrEtiqueta.Equals(String.Empty) Then
                Me.lblEtiqueta.Content = pstrEtiqueta
                Me.Title = "Buscar: " & pstrEtiqueta
            End If
        Catch ex As Exception
            Me.chkCierre.IsChecked = False
        End Try
    End Sub

#End Region

#Region "Eventos controles"

    Private Sub ctlBuscador_itemAsignado(ByVal pstrIdItem As String, ByVal pobjItem As OyDPLUSOrdenesDerivados.ReceptoresBusqueda) Handles ctlBuscador.itemAsignado
        guardarSeleccion()

        Try
            _mobjItem = Me.ctlBuscador.ItemActivo
            If Me.chkCierre.IsChecked Then
                Me.Close()
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un elemento de la lista", Me.Name, "ctlBuscador_itemAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        guardarSeleccion()

        Try
            If Me.ctlBuscador.ItemActivo Is Nothing Then
                Mensajes.mostrarMensaje("Debe seleccionar un elemento de la lista", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                Exit Sub
            End If
            _mobjItem = Me.ctlBuscador.ItemActivo
            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enviar el elemento seleccionado", Me.Name, "cmdAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        guardarSeleccion()

        Try
            _mobjItem = Nothing
            If Not IsNothing(Me.ctlBuscador) Then
                If Not IsNothing(Me.ctlBuscador.mobjVM) Then
                    If Not IsNothing(Me.ctlBuscador.mobjVM.mdcProxy) Then
                        Me.ctlBuscador.mobjVM.mdcProxy.RejectChanges()
                    End If
                    Me.ctlBuscador.acbItems.ItemsSource = Nothing
                    Me.ctlBuscador.acbItems.IsDropDownOpen = False
                End If
            End If
            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la selección", Me.Name, "cmdCancelar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub guardarSeleccion()
        Try
            If Application.Current.Resources.Contains(MSTR_RECURSO_TIPO_SELECCION) Then
                Application.Current.Resources.Remove(MSTR_RECURSO_TIPO_SELECCION)
            End If

            '// Se guarda la preferencia del usuario.
            If Me.chkCierre.IsChecked Then
                Application.Current.Resources.Add(MSTR_RECURSO_TIPO_SELECCION, "S")
            Else
                Application.Current.Resources.Add(MSTR_RECURSO_TIPO_SELECCION, "N")
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

End Class
