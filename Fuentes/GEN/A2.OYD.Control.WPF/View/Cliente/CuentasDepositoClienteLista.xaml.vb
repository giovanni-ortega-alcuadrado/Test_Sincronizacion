Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class CuentasDepositoClienteLista
    Inherits Window

#Region "constantes"

    Private MSTR_RECURSO_TIPO_SELECCION As String = "A2.OYD_TIPO_SEL_BUSCADOR"

#End Region

#Region "Propiedades"

    Private _mobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito = Nothing
    Public ReadOnly Property CuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito
        Get
            Return (_mobjCuentaDeposito)
        End Get
    End Property

    Private _mNroCuentaDeposito As Integer = 0
    Public ReadOnly Property NroCuentaDeposito As Integer
        Get
            Return (_mNroCuentaDeposito)
        End Get
    End Property

#End Region

#Region "Inicializacion"

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(ByVal pstrIDComitente As String, ByVal pintIDCuentaDeposito As Integer, ByVal pstrEtiqueta As String, ByVal plogTraerTodasCuentasDeposito As Boolean)
        InitializeComponent()

        Try
            If pstrIDComitente Is Nothing Then
                pstrIDComitente = String.Empty
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

            Me.ctlBuscadorCuentasDeposito.IdComitente = pstrIDComitente
            Me.ctlBuscadorCuentasDeposito.CuentaDeposito = pintIDCuentaDeposito
            Me.ctlBuscadorCuentasDeposito.TraerTodasCuentaDeposito = plogTraerTodasCuentasDeposito

            'Me.ctlBuscadorCuentasDeposito.BuscarAlIniciar = True
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

    Private Sub ctlBuscador_cuentaDepositoSeleccionada(ByVal pintCuentaDeposito As Integer, ByVal pobjCuentaDeposito As OYDUtilidades.BuscadorCuentasDeposito) Handles ctlBuscadorCuentasDeposito.cuentaDepositoSeleccionada
        guardarSeleccion()

        Try
            _mobjCuentaDeposito = Me.ctlBuscadorCuentasDeposito.mobjCuentaDeposito
            _mNroCuentaDeposito = Me.ctlBuscadorCuentasDeposito.mobjNroCuentaDeposito

            If Me.chkCierre.IsChecked Then
                Me.Close()
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la cuenta deposito", Me.Name, "ctlBuscador_cuentaDepositoSeleccionada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        guardarSeleccion()

        Try
            If Me.ctlBuscadorCuentasDeposito.mobjCuentaDeposito Is Nothing Then
                Mensajes.mostrarMensaje("Debe seleccionar una cuenta deposito", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                Exit Sub
            End If
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
            '_mobjComitente = Nothing
            If Not IsNothing(Me.ctlBuscadorCuentasDeposito) Then
                If Not IsNothing(Me.ctlBuscadorCuentasDeposito.mobjVM) Then
                    If Not IsNothing(Me.ctlBuscadorCuentasDeposito.mobjVM.mdcProxy) Then
                        Me.ctlBuscadorCuentasDeposito.mobjVM.mdcProxy.RejectChanges()
                    End If
                    Me.ctlBuscadorCuentasDeposito.cboCuentasDeposito.ItemsSource = Nothing
                End If
            End If
            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar un comitente", Me.Name, "cmdCancelar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Window_KeyDonw(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.KeyDown
        If CType(e, KeyEventArgs).Key = Key.Escape Then
            CancelarBusqueda()
        End If
    End Sub

#End Region

End Class
