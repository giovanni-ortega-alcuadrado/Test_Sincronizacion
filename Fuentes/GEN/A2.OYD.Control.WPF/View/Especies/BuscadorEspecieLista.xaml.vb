Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorEspecieLista
    Inherits Window

#Region "constantes"

    Private MSTR_RECURSO_TIPO_SELECCION As String = "A2.OYD_TIPO_SEL_BUSCADOR"

#End Region

#Region "Propiedades"

    Private _mobjEspecie As OYDUtilidades.BuscadorEspecies = Nothing
    Public ReadOnly Property EspecieSeleccionada As OYDUtilidades.BuscadorEspecies
        Get
            Return (_mobjEspecie)
        End Get
    End Property

    Private _mobjNemotecnico As EspeciesAgrupadas = Nothing
    Public ReadOnly Property NemotecnicoSeleccionado As EspeciesAgrupadas
        Get
            Return (_mobjNemotecnico)
        End Get
    End Property

    Private _mstrCampoBusqueda As String = String.Empty
    Public ReadOnly Property CampoBusqueda As String
        Get
            Return (_mstrCampoBusqueda)
        End Get
    End Property

    Private _mlogHabilitarConsultaISIN As Boolean = True
    Public ReadOnly Property HabilitarConsultaISIN As Boolean
        Get
            Return (_mlogHabilitarConsultaISIN)
        End Get
    End Property

    Private _mlogPermitirSeleccionarEspecieOIsin As Boolean = False
    Public ReadOnly Property PermitirSeleccionarEspecieOIsin As Boolean
        Get
            Return (_mlogPermitirSeleccionarEspecieOIsin)
        End Get
    End Property

#End Region

#Region "Inicializacion"

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pstrCampoBusqueda As String,
                   ByVal pstrEtiqueta As String,
                   ByVal pintEstado As BuscadorEspecieViewModel.EstadosEspecie,
                   ByVal pstrMercado As BuscadorEspecieViewModel.ClasesEspecie,
                   ByVal pstrAgrupamiento As String,
                   ByVal plogHabilitarConsultaISIN As Boolean,
                   ByVal plogTraerEspeciesVencidas As Boolean,
                   ByVal plogPermitirSeleccionarEspecieOIsin As Boolean)
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
            Me.ctlBuscador.ClaseOrden = pstrMercado
            Me.ctlBuscador.EstadoEspecie = pintEstado
            Me.ctlBuscador.Agrupamiento = pstrAgrupamiento
            Me.ctlBuscador.BuscarAlIniciar = True
            Me.ctlBuscador.HabilitarConsultaISIN = plogHabilitarConsultaISIN
            Me.ctlBuscador.TraerEspeciesVencidas = plogTraerEspeciesVencidas

            Me._mlogHabilitarConsultaISIN = plogHabilitarConsultaISIN
            Me._mlogPermitirSeleccionarEspecieOIsin = plogPermitirSeleccionarEspecieOIsin

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

    Private Sub ctlBuscador_especieAsignada(ByVal pstrNemotecnico As String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies) Handles ctlBuscador.especieAsignada
        guardarSeleccion()

        Try
            _mobjEspecie = Me.ctlBuscador.EspecieActiva
            If Me.chkCierre.IsChecked Then
                Me.Close()
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un Especie", Me.Name, "ctlBuscador_EspecieAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        guardarSeleccion()

        Try
            If Me.HabilitarConsultaISIN = False Then
                If Me.ctlBuscador.EspecieActiva Is Nothing Then
                    Mensajes.mostrarMensaje("Debe seleccionar una especie", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                    Exit Sub
                End If
            ElseIf Me.PermitirSeleccionarEspecieOIsin = False Then
                If Me.ctlBuscador.EspecieActiva Is Nothing Then
                    Mensajes.mostrarMensaje("Debe seleccionar una especie", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                    Exit Sub
                End If
            End If
            
            _mobjEspecie = Me.ctlBuscador.EspecieActiva
            _mobjNemotecnico = Me.ctlBuscador.mobjVM.NemotecnicoSeleccionado

            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar una especie", Me.Name, "cmdAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            _mobjEspecie = Nothing
            If Not IsNothing(Me.ctlBuscador) Then
                If Not IsNothing(Me.ctlBuscador.mobjVM) Then
                    If Not IsNothing(Me.ctlBuscador.mobjVM.mdcProxy) Then
                        Me.ctlBuscador.mobjVM.mdcProxy.RejectChanges()
                    End If
                    Me.ctlBuscador.acbEspecies.ItemsSource = Nothing
                    Me.ctlBuscador.acbEspecies.IsDropDownOpen = False
                End If
            End If
            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar una especie", Me.Name, "cmdCancelar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
