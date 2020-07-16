Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorClienteLista
    Inherits Window

#Region "constantes"

    Private MSTR_RECURSO_TIPO_SELECCION As String = "A2.OYD_TIPO_SEL_BUSCADOR"

#End Region

#Region "Propiedades"

    Private _mobjComitente As OYDUtilidades.BuscadorClientes = Nothing
    Public ReadOnly Property ComitenteSeleccionado As OYDUtilidades.BuscadorClientes
        Get
            Return (_mobjComitente)
        End Get
    End Property

    Private _mstrCampoBusqueda As String = String.Empty
    Public ReadOnly Property CampoBusqueda As String
        Get
            Return (_mstrCampoBusqueda)
        End Get
    End Property

#End Region

#Region "Inicializacion"

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pstrCampoBusqueda As String,
                   ByVal pstrEtiqueta As String,
                   ByVal pintEstado As BuscadorClienteViewModel.EstadosComitente,
                   ByVal pintTipoVinculacion As BuscadorClienteViewModel.TiposVinculacion,
                   ByVal pstrAgrupamiento As String,
                   ByVal plogExcluirCodigosCompania As Boolean,
                   ByVal pintIDCompania As Nullable(Of Integer),
                   ByVal plogConFiltro As Boolean,
                   ByVal pstrfiltroAdicional1 As String,
                   ByVal pstrfiltroAdicional2 As String,
                   ByVal pstrfiltroAdicional3 As String)
        InitializeComponent()

        Try
            If pstrCampoBusqueda Is Nothing Then
                pstrCampoBusqueda = String.Empty
            End If

            If pstrEtiqueta Is Nothing Then
                pstrEtiqueta = String.Empty
            End If

            If pstrAgrupamiento Is Nothing Then
                pstrAgrupamiento = String.Empty
            End If

            If pstrCampoBusqueda Is Nothing Then
                Me._mstrCampoBusqueda = pstrCampoBusqueda
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
            Me.ctlBuscador.TipoVinculacion = pintTipoVinculacion
            Me.ctlBuscador.EstadoComitente = pintEstado
            Me.ctlBuscador.Agrupamiento = pstrAgrupamiento
            Me.ctlBuscador.BuscarAlIniciar = True
            Me.ctlBuscador.ExcluirCodigosCompania = plogExcluirCodigosCompania
            Me.ctlBuscador.IDCompania = pintIDCompania
            Me.ctlBuscador.ConFiltro = plogConFiltro
            Me.ctlBuscador.filtroAdicional1 = pstrfiltroAdicional1
            Me.ctlBuscador.filtroAdicional2 = pstrfiltroAdicional2
            Me.ctlBuscador.filtroAdicional3 = pstrfiltroAdicional3

            If Not pstrEtiqueta.Equals(String.Empty) Then
                Me.lblEtiqueta.Content = pstrEtiqueta
                Me.Title = "Buscar: " & pstrEtiqueta
            End If
        Catch ex As Exception
            Me.chkCierre.IsChecked = False
        End Try
    End Sub

    Public Sub New(ByVal pstrCampoBusqueda As String,
                   ByVal pstrEtiqueta As String,
                   ByVal pintEstado As BuscadorClienteViewModel.EstadosComitente,
                   ByVal pintTipoVinculacion As BuscadorClienteViewModel.TiposVinculacion,
                   ByVal pstrAgrupamiento As String,
                   ByVal plogClienteRestrinccion As Boolean,
                   ByVal pvisCargarClientesTercero As Visibility,
                   ByVal plogClientesXTipoProductoPerfil As Boolean,
                   ByVal pstrIdReceptor As String,
                   ByVal pstrTipoNegocio As String,
                   ByVal pstrTipoProducto As String,
                   ByVal pstrPerfilRiesgo As String,
                   ByVal plogExcluirCodigosCompania As Boolean,
                   ByVal pintIDCompania As Nullable(Of Integer),
                   ByVal plogConFiltro As Boolean,
                   ByVal pstrfiltroAdicional1 As String,
                   ByVal pstrfiltroAdicional2 As String,
                   ByVal pstrfiltroAdicional3 As String)
        InitializeComponent()

        Try
            If pstrCampoBusqueda Is Nothing Then
                pstrCampoBusqueda = String.Empty
            End If

            If pstrEtiqueta Is Nothing Then
                pstrEtiqueta = String.Empty
            End If

            If pstrAgrupamiento Is Nothing Then
                pstrAgrupamiento = String.Empty
            End If

            If pstrCampoBusqueda Is Nothing Then
                Me._mstrCampoBusqueda = pstrCampoBusqueda
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
            Me.ctlBuscador.TipoVinculacion = pintTipoVinculacion
            Me.ctlBuscador.EstadoComitente = pintEstado
            Me.ctlBuscador.Agrupamiento = pstrAgrupamiento
            Me.ctlBuscador.BuscarAlIniciar = True
            Me.ctlBuscador.CargarClientesRestriccion = plogClienteRestrinccion
            Me.ctlBuscador.CargarClientesTercero = pvisCargarClientesTercero
            Me.ctlBuscador.CargarClientesXTipoProductoPerfil = plogClientesXTipoProductoPerfil
            Me.ctlBuscador.IDReceptor = pstrIdReceptor
            Me.ctlBuscador.TipoNegocio = pstrTipoNegocio
            Me.ctlBuscador.TipoProducto = pstrTipoProducto
            Me.ctlBuscador.PerfilRiesgo = pstrPerfilRiesgo
            Me.ctlBuscador.ExcluirCodigosCompania = plogExcluirCodigosCompania
            Me.ctlBuscador.IDCompania = pintIDCompania
            Me.ctlBuscador.ConFiltro = plogConFiltro
            Me.ctlBuscador.filtroAdicional1 = pstrfiltroAdicional1
            Me.ctlBuscador.filtroAdicional2 = pstrfiltroAdicional2
            Me.ctlBuscador.filtroAdicional3 = pstrfiltroAdicional3

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

    Private Sub ctlBuscador_comitenteAsignado(ByVal pstrIdComitente As String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes) Handles ctlBuscador.comitenteAsignado
        guardarSeleccion()

        Try
            _mobjComitente = Me.ctlBuscador.ComitenteActivo
            If Me.chkCierre.IsChecked Then
                Me.Close()
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un comitente", Me.Name, "ctlBuscador_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        guardarSeleccion()

        Try
            If Me.ctlBuscador.ComitenteActivo Is Nothing Then
                Mensajes.mostrarMensaje("Debe seleccionar un comitente", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                Exit Sub
            End If
            _mobjComitente = Me.ctlBuscador.ComitenteActivo
            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un comitente", Me.Name, "cmdAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            _mobjComitente = Nothing
            If Not IsNothing(Me.ctlBuscador) Then
                If Not IsNothing(Me.ctlBuscador.mobjVM) Then
                    If Not IsNothing(Me.ctlBuscador.mobjVM.mdcProxy) Then
                        Me.ctlBuscador.mobjVM.mdcProxy.RejectChanges()
                    End If
                    Me.ctlBuscador.acbClientes.ItemsSource = Nothing
                    Me.ctlBuscador.acbClientes.IsDropDownOpen = False
                End If
            End If

            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un comitente", Me.Name, "cmdCancelar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
