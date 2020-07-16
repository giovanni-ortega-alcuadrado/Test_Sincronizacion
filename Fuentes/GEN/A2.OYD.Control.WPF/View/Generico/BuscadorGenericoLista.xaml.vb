Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorGenericoLista
    Inherits Window

#Region "constantes"

    Private MSTR_RECURSO_TIPO_SELECCION As String = "A2.OYD_TIPO_SEL_BUSCADOR"

#End Region

#Region "Propiedades"

    Private _mobjItem As OYDUtilidades.BuscadorGenerico = Nothing
    Public ReadOnly Property ItemSeleccionado As OYDUtilidades.BuscadorGenerico
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

    Public Sub New(ByVal pstrCampoBusqueda As String, ByVal pstrEtiqueta As String, ByVal pstrTipoItem As String, ByVal pintEstado As BuscadorGenericoViewModel.EstadosItem, ByVal pstrAgrupamiento As String, ByVal pstrCondicion1 As String, ByVal pstrCondicion2 As String)
        InitializeComponent()

        Try
            If pstrCampoBusqueda Is Nothing Then
                pstrCampoBusqueda = String.Empty
            End If

            If pstrEtiqueta Is Nothing Then
                pstrEtiqueta = String.Empty
            End If

            If pstrTipoItem Is Nothing Then
                pstrTipoItem = String.Empty
            End If

            If pstrAgrupamiento Is Nothing Then
                pstrAgrupamiento = String.Empty
            End If
            'SV20160203
            If pstrCondicion1 Is Nothing Then
                pstrCondicion1 = String.Empty
            End If

            If pstrCondicion2 Is Nothing Then
                pstrCondicion2 = String.Empty
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
            Me._mstrTipoItem = pstrTipoItem
            Me.ctlBuscador.TipoItem = pstrTipoItem
            Me.ctlBuscador.EstadoItem = pintEstado
            Me.ctlBuscador.Agrupamiento = pstrAgrupamiento
            'SV20160203
            Me.ctlBuscador.Condicion1 = pstrCondicion1
            Me.ctlBuscador.Condicion2 = pstrCondicion2
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

    Private Sub ctlBuscador_itemAsignado(ByVal pstrIdItem As String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico) Handles ctlBuscador.itemAsignado
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
        CancelarBusqueda()
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

    Private Sub CancelarBusqueda()
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

#End Region

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

    Private Sub Window_KeyDonw(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.KeyDown
        If CType(e, KeyEventArgs).Key = Key.Escape Then
            CancelarBusqueda()
        End If
    End Sub

End Class
