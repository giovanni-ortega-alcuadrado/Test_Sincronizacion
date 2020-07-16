Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades

Partial Public Class BuscadorEspecieListaButon
    Inherits UserControl

#Region "Eventos"

    Public Event finalizoBusqueda(ByVal pstrClaseControl As String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)

    Public Event nemotecnicoAsignado(ByVal pstrNemotecnico As String, ByVal pstrNombreEspecie As String)

    Public Event EspecieAgrupadaAsignado(ByVal pobjNemotecnico As EspeciesAgrupadas, ByVal pstrNemotecnico As String)
#End Region

#Region "Variables"

    Private WithEvents mobjBuscador As BuscadorEspecieLista

#End Region

#Region "Propiedades"
    Private Shared ReadOnly NemotecnicoDep As DependencyProperty = DependencyProperty.Register("Nemotecnico", GetType(String), GetType(BuscadorEspecieListaButon), New PropertyMetadata("", New PropertyChangedCallback(AddressOf cambioPropiedadDep)))
    Private Shared ReadOnly EstadoEspecieDep As DependencyProperty = DependencyProperty.Register("EstadoEspecie", GetType(BuscadorEspecieViewModel.EstadosEspecie), GetType(BuscadorEspecieListaButon), New PropertyMetadata(BuscadorEspecieViewModel.EstadosEspecie.T, New PropertyChangedCallback(AddressOf cambioPropiedadDep)))
    Private Shared ReadOnly ClaseOrdenDep As DependencyProperty = DependencyProperty.Register("ClaseOrden", GetType(BuscadorEspecieViewModel.ClasesEspecie), GetType(BuscadorEspecieListaButon), New PropertyMetadata(BuscadorEspecieViewModel.ClasesEspecie.T, New PropertyChangedCallback(AddressOf cambioPropiedadDep)))
    Private Shared ReadOnly _mlogHabilitarConsultaISIN As DependencyProperty = DependencyProperty.Register("HabilitarConsultaISIN", GetType(Boolean), GetType(BuscadorEspecieListaButon), New PropertyMetadata(True, New PropertyChangedCallback(AddressOf cambioPropiedadDep)))
    Private Shared ReadOnly _mlogPermitirSeleccionarEspecieOIsin As DependencyProperty = DependencyProperty.Register("PermitirSeleccionarEspecieOIsin", GetType(Boolean), GetType(BuscadorEspecieListaButon), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf cambioPropiedadDep)))
    Private Shared ReadOnly _mlogTraerEspeciesVencidas As DependencyProperty = DependencyProperty.Register("TraerEspeciesVencidas", GetType(Boolean), GetType(BuscadorEspecieListaButon), New PropertyMetadata(False, New PropertyChangedCallback(AddressOf cambioPropiedadDep)))

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
    Public Property CampoBusqueda As String
        Get
            Return (_mstrCampoBusqueda)
        End Get
        Set(ByVal value As String)
            _mstrCampoBusqueda = value
        End Set
    End Property

    ''' <summary>
    ''' ClaseOrden al cual pertenencen las especies que se desea consultar (acciones, renta fija)
    ''' </summary>
    Public Property ClaseOrden As BuscadorEspecieViewModel.ClasesEspecie
        Get
            Return (CType(Me.GetValue(ClaseOrdenDep), BuscadorEspecieViewModel.ClasesEspecie))
        End Get
        Set(ByVal value As BuscadorEspecieViewModel.ClasesEspecie)
            Me.SetValue(ClaseOrdenDep, value)
        End Set
    End Property

    ''' <summary>
    ''' Estado de las especies que se desea consultar (activas, inactivas, todas)
    ''' </summary>
    Public Property EstadoEspecie As BuscadorEspecieViewModel.EstadosEspecie
        Get
            Return (CType(Me.GetValue(EstadoEspecieDep), BuscadorEspecieViewModel.EstadosEspecie))
        End Get
        Set(ByVal value As BuscadorEspecieViewModel.EstadosEspecie)
            Me.SetValue(EstadoEspecieDep, value)
        End Set
    End Property

    Private _mstrAgrupamiento As String = String.Empty
    Public Property Agrupamiento As String
        Get
            Return (_mstrAgrupamiento)
        End Get
        Set(ByVal value As String)
            _mstrAgrupamiento = value
        End Set
    End Property

    Private _mstrEtiqueta As String = String.Empty
    Public Property Etiqueta As String
        Get
            Return (_mstrEtiqueta)
        End Get
        Set(ByVal value As String)
            _mstrEtiqueta = value
        End Set
    End Property

    Private _mintIzquierda As Integer = 0
    Public Property Izquierda As Integer
        Get
            Return (_mintIzquierda)
        End Get
        Set(ByVal value As Integer)
            _mintIzquierda = value
        End Set
    End Property

    Private _mintSuperior As Integer = 0
    Public Property Superior As Integer
        Get
            Return (_mintSuperior)
        End Get
        Set(ByVal value As Integer)
            _mintSuperior = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se debe de mostrar la consulta de ISINES
    ''' </summary>
    ''' <value></value>
    Public Property HabilitarConsultaISIN As Boolean
        Get
            Return (CType(Me.GetValue(_mlogHabilitarConsultaISIN), Boolean))
        End Get
        Set(ByVal value As Boolean)
            Me.SetValue(_mlogHabilitarConsultaISIN, value)
        End Set
    End Property

    ''' <summary>
    ''' Indica si se puede cerrar el popup sin seleccionar el isin
    ''' </summary>
    ''' <value></value>
    Public Property PermitirSeleccionarEspecieOIsin As Boolean
        Get
            Return (CType(Me.GetValue(_mlogPermitirSeleccionarEspecieOIsin), Boolean))
        End Get
        Set(ByVal value As Boolean)
            Me.SetValue(_mlogPermitirSeleccionarEspecieOIsin, value)
        End Set
    End Property

    ''' <summary>
    ''' Indica si se consultan especies con la fecha de vencimiento menor a la actual
    ''' </summary>
    ''' <value></value>
    Public Property TraerEspeciesVencidas As Boolean
        Get
            Return (CType(Me.GetValue(_mlogTraerEspeciesVencidas), Boolean))
        End Get
        Set(ByVal value As Boolean)
            Me.SetValue(_mlogTraerEspeciesVencidas, value)
        End Set
    End Property

#End Region

#Region "Callback"

    ''' <summary>
    ''' Procedimiento de Call back que se lanza cuando alguna de las dependency properties se modifica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Shared Sub cambioPropiedadDep(ByVal sender As Object, ByVal args As DependencyPropertyChangedEventArgs)

    End Sub

#End Region

#Region "Inicialización"

    Public Sub New()
        InitializeComponent()
    End Sub

#End Region

#Region "Eventos controles"

    Private Sub cmdBuscar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            mobjBuscador = New BuscadorEspecieLista(_mstrCampoBusqueda, _mstrEtiqueta, Me.EstadoEspecie, Me.ClaseOrden, _mstrAgrupamiento, HabilitarConsultaISIN, TraerEspeciesVencidas, PermitirSeleccionarEspecieOIsin)

            If _mintIzquierda > 0 And _mintSuperior > 0 Then
                mobjBuscador.Top = _mintSuperior
                mobjBuscador.Left = _mintIzquierda
            Else
                'mobjBuscador.CenterOnScreen()
            End If
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscador)
            mobjBuscador.ShowDialog()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al buscar un cliente", Me.Name, "Button_Click_buscar_cliente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjBuscador_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobjBuscador.Closed
        Try
            Me._mobjEspecie = mobjBuscador.EspecieSeleccionada
            Me._mobjNemotecnico = mobjBuscador.NemotecnicoSeleccionado

            RaiseEvent finalizoBusqueda(_mstrCampoBusqueda, mobjBuscador.EspecieSeleccionada)

            If Not IsNothing(Me._mobjNemotecnico) Then
                RaiseEvent nemotecnicoAsignado(Me._mobjNemotecnico.Nemotecnico, Me._mobjNemotecnico.Especie)
                RaiseEvent EspecieAgrupadaAsignado(Me._mobjNemotecnico, Me._mobjNemotecnico.Nemotecnico)
            End If
        Catch ex As Exception
            Me._mobjEspecie = Nothing
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al cerrar el buscador de clientes", Me.Name, "mobjBuscador_Closed", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub AbrirBuscador()
        Try
            mobjBuscador = New BuscadorEspecieLista(_mstrCampoBusqueda, _mstrEtiqueta, Me.EstadoEspecie, Me.ClaseOrden, _mstrAgrupamiento, HabilitarConsultaISIN, TraerEspeciesVencidas, PermitirSeleccionarEspecieOIsin)

            If _mintIzquierda > 0 And _mintSuperior > 0 Then
                mobjBuscador.Top = _mintSuperior
                mobjBuscador.Left = _mintIzquierda
            Else
                'mobjBuscador.CenterOnScreen()
            End If
            Program.Modal_OwnerMainWindowsPrincipal(mobjBuscador)
            mobjBuscador.ShowDialog()
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al buscar un cliente", Me.Name, "AbrirBuscador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
