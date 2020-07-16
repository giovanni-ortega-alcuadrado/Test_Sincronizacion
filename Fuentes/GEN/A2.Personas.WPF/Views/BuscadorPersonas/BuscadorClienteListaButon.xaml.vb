Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorClienteListaButon
    Inherits UserControl

#Region "Eventos"

    ''' <summary>
    ''' JAPC20180926: evento para notificar donde se utilize el control que la busqueda finalizo y enviar la persona seleccionada
    ''' </summary>
    ''' <param name="pobjPersona"></param>
    Public Event finalizoBusqueda(ByVal pobjPersona As CPX_BuscadorPersonas)

#End Region

#Region "Variables"

    ''' <summary>
    ''' JAPC20180926: BuscadorClienteLista con eventos para identificar cuando cierren el modal buscador
    ''' </summary>
    Private WithEvents mobjBuscador As BuscadorClienteLista

#End Region

#Region "Propiedades"
    ''' <summary>
    ''' JAPC20180926: propiedad dependiente para indicar el tipo de busqueda al control
    ''' </summary>
    Private Shared ReadOnly RolPersonaDep As DependencyProperty = DependencyProperty.Register("RolPersona", GetType(String), GetType(BuscadorClienteListaButon), New PropertyMetadata(""))


    ''' <summary>
    ''' JAPC20180926:propiedad para almacenar la persona seleccionada 
    ''' </summary>
    Private _mobjPersona As CPX_BuscadorPersonas = Nothing
    Public ReadOnly Property PersonaSeleccionada As CPX_BuscadorPersonas
        Get
            Return (_mobjPersona)
        End Get
    End Property



    ''' <summary>
    ''' JAPC20180926:Indica el rol por el cual se realizara el filtro de personas.
    ''' </summary>
    Public Property RolPersona As String
        Get
            Return CStr(GetValue(RolPersonaDep))
        End Get
        Set(ByVal value As String)
            SetValue(RolPersonaDep, value)
        End Set
    End Property




#End Region

#Region "Inicialización"

    Public Sub New()
        InitializeComponent()
    End Sub

#End Region

#Region "Eventos controles"


    ''' <summary>
    ''' JAPC20180926: evento cuando se da click al simbolo buscador para instanciar modal BuscadorClienteLista
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjBuscador = New BuscadorClienteLista(RolPersona)
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscador)
            mobjBuscador.ShowDialog()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al buscar una persona", Me.Name, "Button_Click_buscar_persona", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20180926: metodo que maneja evento de cierre modal buscador para traer persona seleccionada y activar evento finalizoBusqueda para notificar la persona seleccionda donde se utilize el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub mobjBuscador_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscador.Closed
        Try
            Me._mobjPersona = mobjBuscador.PersonaSeleccionada
            If Not IsNothing(mobjBuscador.PersonaSeleccionada) Then
                RaiseEvent finalizoBusqueda(mobjBuscador.PersonaSeleccionada)
            End If

        Catch ex As Exception
            Me._mobjPersona = Nothing
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al cerrar el buscador de personas", Me.Name, "mobjBuscador_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub



#End Region


End Class
