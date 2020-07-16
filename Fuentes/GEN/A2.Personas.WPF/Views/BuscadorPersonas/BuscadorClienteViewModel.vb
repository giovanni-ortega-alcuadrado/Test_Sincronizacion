Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class BuscadorClienteViewModel
    Implements INotifyPropertyChanged

#Region "Eventos"
    'JAPC20180926
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE EVENTOS
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cualquier cambio en una propiedad del objeto
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- Indicar cuando finaliza el servicio que carga la lista de clientes
    Public Event CargaPersonasCompleta(ByVal plogNroPersonas As Integer, ByVal plogBusquedaPersonaEspecifico As Boolean)
#End Region

#Region "Variables"
    'JAPC20180926
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE VARIABLES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private mlogMostrarMensajeLog As Boolean = False
    Public mdcProxy As PersonasDomainServices

#End Region

#Region "Propiedades"

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' PROPIEDADES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    ''' <summary>
    ''' JAPC20180926: propiedad para manejar la condicion de filtro ingresada desde el control
    ''' </summary>
    Private _mstrCondicionFiltro As String = ""
    Public Property CondicionFiltro() As String
        Get
            Return (_mstrCondicionFiltro)
        End Get
        Set(ByVal value As String)
            _mstrCondicionFiltro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CondicionFiltro"))
        End Set
    End Property


    ''' <summary>
    ''' JAPC20180926: Propiedad para origen de datos del buscador
    ''' </summary>
    Private _ListaBusquedaControl As List(Of clsClientes_Buscador)
    Public Property ListaBusquedaControl() As List(Of clsClientes_Buscador)
        Get
            Return _ListaBusquedaControl
        End Get
        Set(ByVal value As List(Of clsClientes_Buscador))
            _ListaBusquedaControl = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaBusquedaControl"))
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: propiedad para manejar el item seleccionado del buscador
    ''' </summary>
    Private _ItemSeleccionadoBuscador As clsClientes_Buscador
    Public Property ItemSeleccionadoBuscador() As clsClientes_Buscador
        Get
            Return _ItemSeleccionadoBuscador
        End Get
        Set(ByVal value As clsClientes_Buscador)
            _ItemSeleccionadoBuscador = value
            'le asigna valor al comitente seleccionado el cual controla el resto del codigo
            If Not IsNothing(value) Then
                PersonaSeleccionada = value.ItemBusqueda
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemSeleccionadoBuscador"))
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: propiedad para listar cantidad de personas
    ''' </summary>
    Private _mobjPersonas As List(Of CPX_BuscadorPersonas) = Nothing
    Public Property Personas() As List(Of CPX_BuscadorPersonas)
        Get
            Return (_mobjPersonas)
        End Get
        Set(ByVal value As List(Of CPX_BuscadorPersonas))
            _mobjPersonas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Personas"))
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: propiedad para manejar la persona selecciona  de ItemSeleccionadoBuscador
    ''' </summary>
    Private _mobjPersonaSeleccionada As CPX_BuscadorPersonas = Nothing
    Public Property PersonaSeleccionada() As CPX_BuscadorPersonas
        Get
            Return (_mobjPersonaSeleccionada)
        End Get
        Set(ByVal value As CPX_BuscadorPersonas)
            _mobjPersonaSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PersonaSeleccionada"))
        End Set
    End Property


    ''' <summary>
    ''' JAPC20180926: propiedad booleana para indicar si esta buscando
    ''' </summary>
    Private _mlogBuscandoPersona As Boolean = False
    Public Property Buscando As Boolean
        Get
            Return (_mlogBuscandoPersona)
        End Get
        Set(ByVal value As Boolean)
            _mlogBuscandoPersona = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Buscando"))
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: para indicar que se ha inicizalizado
    ''' </summary>
    Private _mlogInicializado As Boolean = False
    Public Property Inicializado As Boolean
        Get
            Return (_mlogInicializado)
        End Get
        Set(ByVal value As Boolean)
            _mlogInicializado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Inicializado"))
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: propiedad para mostrar texto de consultar persona..
    ''' </summary>
    Private _MostrarConsultando As Visibility = Visibility.Collapsed
    Public Property MostrarConsultando() As Visibility
        Get
            Return _MostrarConsultando
        End Get
        Set(ByVal value As Visibility)
            _MostrarConsultando = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarConsultando"))
        End Set
    End Property


    ''' <summary>
    ''' JAPC20180926: propiedad para saber por que tkipo de persona se realiza la busqueda se trae desde 
    ''' propiedad dependiente del control
    ''' </summary>
    Private _TipoPersona As String = String.Empty
    Public Property TipoPersona() As String
        Get
            Return _TipoPersona
        End Get
        Set(ByVal value As String)
            _TipoPersona = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoPersona"))
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: para identificar si se esta editando
    ''' </summary>
    Private _Editando As Boolean = False
    Public Property Editando() As Boolean
        Get
            Return _Editando
        End Get
        Set(ByVal value As Boolean)
            _Editando = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Editando"))
        End Set
    End Property
    '**************************************************************************************************************************************************************************************


    '----------------------------------------------------------------------------------------------------------------------------------
    'JAPC20180926: propiedades para filtros adicionales (actualmente no se usan se dejan por si se necesitan en algun momento)
    '----------------------------------------------------------------------------------------------------------------------------------

    Private _mstrfiltroAdicional1 As String = String.Empty '// Por defecto consultar sobre todos los clientes
    Public Property filtroAdicional1() As String
        Get
            Return (_mstrfiltroAdicional1)
        End Get
        Set(ByVal value As String)
            _mstrfiltroAdicional1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("filtroAdicional1"))
        End Set
    End Property
    Private _mstrfiltroAdicional2 As String = String.Empty '// Por defecto consultar sobre todos los clientes
    Public Property filtroAdicional2() As String
        Get
            Return (_mstrfiltroAdicional2)
        End Get
        Set(ByVal value As String)
            _mstrfiltroAdicional2 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("filtroAdicional2"))
        End Set
    End Property
    Private _mstrfiltroAdicional3 As String = String.Empty '// Por defecto consultar sobre todos los clientes
    Public Property filtroAdicional3() As String
        Get
            Return (_mstrfiltroAdicional3)
        End Get
        Set(ByVal value As String)
            _mstrfiltroAdicional3 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("filtroAdicional3"))
        End Set
    End Property
    Private _mlogConFiltro As Boolean = False
    Public Property ConFiltro As Boolean
        Get
            Return (_mlogConFiltro)
        End Get
        Set(ByVal value As Boolean)
            _mlogConFiltro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ConFiltro"))
        End Set
    End Property
#End Region

#Region "Inicializaciones"
    'JAPC20180926
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- PROCESOS DE INICIALIZACIÓN
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Public Sub New()

        Try

            mdcProxy = inicializarProxy()
            'Inicializar servicios
            inicializarServicios()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try

    End Sub

    ''' <summary>
    ''' JAPC20180926: Inicializa los proxies para acceder a los servicios web y configura los manejadores de evento de los diferentes métodos asincrónicos disponibles
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Private Sub inicializarServicios()
        Try
            mlogMostrarMensajeLog = CBool(IIf(Program.MostrarMensajeLog.ToUpper = "S", True, False))
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region


#Region "Métodos públicos"

    ''' <summary>
    ''' JAPC20180926: metodo para consultar personas fitrando por el tipo de persona y la condicion de filtro todo se trae desde el control
    ''' </summary>
    Friend Async Sub consultarPersonas()
        Try
            Dim result As InvokeResult(Of List(Of CPX_BuscadorPersonas)) = Nothing

            MostrarConsultando = Visibility.Visible
            Dim TextoSeguro As String = System.Web.HttpUtility.UrlEncode(Me.CondicionFiltro)


            result = Await mdcProxy.Personas_BuscarAsync(TextoSeguro, Me.TipoPersona, Program.Usuario)

            Dim logBusquedaEspecifica As Boolean = False


            _mlogInicializado = True

            Personas = result.Value.ToList


            Dim objListaRespuesta As New List(Of CPX_BuscadorPersonas)
            Dim objListaBuscador As New List(Of clsClientes_Buscador)

            If Not IsNothing(Personas) Then
                For Each li In Personas
                    objListaRespuesta.Add(li)
                Next
            End If

            For Each li In objListaRespuesta
                objListaBuscador.Add(New clsClientes_Buscador With {
                    .ItemBusqueda = li,
                    .DescripcionBuscador = LTrim(RTrim(li.strCodigoOyD))
                    })
            Next

            ListaBusquedaControl = objListaBuscador



            RaiseEvent CargaPersonasCompleta(_mobjPersonas.Count, logBusquedaEspecifica)


            mdcProxy.RejectChanges()



        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de personas", Me.ToString(), "consultarPersonas", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        Finally
            MostrarConsultando = Visibility.Collapsed
        End Try
    End Sub





#End Region



End Class
