Imports A2.OyD.OYDServer.RIA.WEB
Imports A2Utilidades

Partial Public Class BuscadorClienteLista
    Inherits Window


#Region "Propiedades"

    ''' <summary>
    ''' JAPC20180926:Propiedad para almacenar el comitente seleccionado
    ''' </summary>
    Private _mobjPersona As CPX_BuscadorPersonas = Nothing
    Public ReadOnly Property PersonaSeleccionada As CPX_BuscadorPersonas
        Get
            Return (_mobjPersona)
        End Get
    End Property


#End Region

#Region "Inicializacion"

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' JAPC20180926: constructor para enviar rol persona al buscador
    ''' </summary>
    ''' <param name="pRolPersona"></param>
    Public Sub New(ByVal pRolPersona As String
                  )
        InitializeComponent()

        Try
            Me.ctlBuscador.RolPersona = pRolPersona

        Catch ex As Exception
            Me.chkCierre.IsChecked = False
        End Try
    End Sub


#End Region

#Region "Eventos controles"

    ''' <summary>
    ''' JAPC20180926: Metodo que maneja evento cuando se selecciona la persona en el buscador
    ''' </summary>
    ''' <param name="pstrIdPersona"></param>
    ''' <param name="pobjPersona"></param>
    Private Sub ctlBuscador_PersonaAsignada(ByVal pstrIdPersona As String, ByVal pobjPersona As CPX_BuscadorPersonas) Handles ctlBuscador.personaAsignada
        Try
            _mobjPersona = Me.ctlBuscador.personaActiva
            If Me.chkCierre.IsChecked Then
                Me.Close()
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar una persona", Me.Name, "ctlBuscador_PersonaAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    '''  JAPC20180926: metodo que se ejecuta cuando le dan aceptar al control  para asignar el comitente seleccionado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdAceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Me.ctlBuscador.personaActiva Is Nothing Then
                Mensajes.mostrarMensaje("Debe seleccionar un comitente", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                Exit Sub
            End If
            _mobjPersona = Me.ctlBuscador.personaActiva
            Me.Close()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar una persona", Me.Name, "cmdAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: metodo para cancelar busqueda y limpiar datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdCancelar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CancelarBusqueda()
    End Sub


    ''' <summary>
    ''' JAPC20180926: metodo para cancelar busqueda y limpiar datos
    ''' </summary>
    Private Sub CancelarBusqueda()
        Try
            _mobjPersona = Nothing
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
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar una persona", Me.Name, "cmdCancelar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    End Sub


End Class
