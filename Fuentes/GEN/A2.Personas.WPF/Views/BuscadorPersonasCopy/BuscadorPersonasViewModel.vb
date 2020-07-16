Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web

Public Class BuscadorPersonasViewModel
	Inherits A2ControlMenu.A2ViewModel


#Region "Constantes"

	Public Enum RolesPersona As Byte
		'TODO definir los roles de la persona y los nombres del enum

		Todos 'Todos
		Comitente 'Comitente
		Ordenante 'Ordenante
		Receptor 'Receptor
		Tercero 'Terceros
	End Enum
	Public Enum EstadosComitente As Byte
		T '// Todos
		A '// Activos
		B '// Bloqueados
		I '// Inactivos
	End Enum

	Public Enum TiposVinculacion As Byte
		T '// Todas
		C '// Clientes
		O '// Ordenantes
	End Enum

	Public Enum EstadosPersona As Byte
		'TODO definier los estados de las personas
		T '// Todos
		A '// Activos
		I '// Inactivos
	End Enum

#End Region

#Region "Eventos"


	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-- DEFINICIÓN DE EVENTOS
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------

	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-- Indicar cualquier cambio en una propiedad del objeto
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-- Indicar cuando finaliza el servicio que carga la lista de personas
	Public Event CargaPersonasCompleta(ByVal plogNroComitentes As Integer, ByVal plogBusquedaComitenteEspecifico As Boolean)
	Public Event PersonaSeleccionada()
#End Region

#Region "Variables"
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-- DEFINICIÓN DE VARIABLES
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------

	Private mlogMostrarMensajeLog As Boolean = False
	Public mdcProxy As PersonasDomainServices

#End Region

#Region "Propiedades"

	Private _Inicializado As Boolean
	Public Property Inicializado() As Boolean
		Get
			Return _Inicializado
		End Get
		Set(ByVal value As Boolean)
			_Inicializado = value
			CambioItem("Inicializado")
		End Set
	End Property

	Private _IDReceptor As Integer?
	Public Property IDReceptor() As Integer?
		Get
			Return _IDReceptor
		End Get
		Set(ByVal value As Integer?)
			_IDReceptor = value
			CambioItem("IDReceptor")
		End Set
	End Property

	Private _Activar As Boolean = True
	Public Property Activar() As Boolean
		Get
			Return _Activar
		End Get
		Set(ByVal value As Boolean)
			_Activar = value
			CambioItem("Activar")
		End Set
	End Property

	Private _MostrarConsultando As Visibility = Visibility.Collapsed
	Public Property MostrarConsultando() As Visibility
		Get
			Return _MostrarConsultando
		End Get
		Set(ByVal value As Visibility)
			_MostrarConsultando = value
			CambioItem("MostrarConsultando")
		End Set
	End Property

	Private _ListaPersonas As ObservableCollection(Of CPX_BuscadorPersonas)
	Public Property ListaPersonas() As ObservableCollection(Of CPX_BuscadorPersonas)
		Get
			Return _ListaPersonas
		End Get
		Set(ByVal value As ObservableCollection(Of CPX_BuscadorPersonas))
			_ListaPersonas = value
			CambioItem("ListaPersonas")
		End Set
	End Property

	Private _CondicionFiltro As String = ""
	Public Property CondicionFiltro() As String
		Get
			Return _CondicionFiltro
		End Get
		Set(ByVal value As String)
			_CondicionFiltro = value
			CambioItem("CondicionFiltro")
		End Set
	End Property

	Private _PersonaSeleccionado As CPX_BuscadorPersonas
	Public Property PersonaSeleccionado() As CPX_BuscadorPersonas
		Get
			Return _PersonaSeleccionado
		End Get
		Set(ByVal value As CPX_BuscadorPersonas)
			_PersonaSeleccionado = value
			CambioItem("PersonaSeleccionado")

		End Set
	End Property

	Private _Buscando As Boolean = False
	Public Property Buscando() As Boolean
		Get
			Return _Buscando
		End Get
		Set(ByVal value As Boolean)
			_Buscando = value
			CambioItem("Buscando")
		End Set
	End Property

	Private _intIDPersona As Integer?
	Public Property intIDPersona() As Integer?
		Get
			Return _intIDPersona
		End Get
		Set(ByVal value As Integer?)
			_intIDPersona = value
			CambioItem("intIDPersona")
		End Set
	End Property

	Private _EstadoComitente As EstadosComitente = EstadosComitente.T
	Public Property EstadoComitente() As String
		Get
			Return _EstadoComitente
		End Get
		Set(ByVal value As String)
			_EstadoComitente = value
			CambioItem("EstadoComitente")
		End Set
	End Property

	Private _EstadoPersona As EstadosPersona = EstadosPersona.T
	Public Property EstadoPersona() As EstadosPersona
		Get
			Return _EstadoPersona
		End Get
		Set(ByVal value As EstadosPersona)
			_EstadoPersona = value
			CambioItem("EstadoPersona")
		End Set
	End Property

	Private _TipoVinculacion As TiposVinculacion = TiposVinculacion.T
	Public Property TipoVinculacion() As TiposVinculacion
		Get
			Return _TipoVinculacion
		End Get
		Set(ByVal value As TiposVinculacion)
			_TipoVinculacion = value
		End Set
	End Property

	Private _RolPersona As RolesPersona = RolesPersona.Comitente
	Public Property RolPersona() As RolesPersona
		Get
			Return _RolPersona
		End Get
		Set(ByVal value As RolesPersona)
			_RolPersona = value
			CambioItem("RolPersona")
		End Set
	End Property

#End Region

#Region "Inicializaciones"
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-- PROCESOS DE INICIALIZACIÓN
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------
	'-------------------------------------------------------------------------------------------------------------------------------------------------------------

	Public Sub New()
		Try

		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
		End Try

	End Sub

	Public Function inicializar() As Boolean

		Dim logResultado As Boolean = False

		Try
			mdcProxy = inicializarProxy()
			'consultarPersonas()
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
		Finally
			IsBusy = False
		End Try

		Return (logResultado)

	End Function



#End Region

#Region "Métodos públicos"
	Friend Async Sub consultarPersonas()
		Try
			Dim logBusquedaEspecifica As Boolean = False

			If MostrarConsultando = Visibility.Collapsed Then
				Activar = False
				'ListaPersonas = Nothing
				Dim objRespuesta As InvokeResult(Of List(Of CPX_BuscadorPersonas)) = Nothing

				MostrarConsultando = Visibility.Visible
				Dim TextoSeguro As String = System.Web.HttpUtility.UrlEncode(CondicionFiltro)

				objRespuesta = Await mdcProxy.Personas_BuscarAsync(TextoSeguro, RolPersona.ToString, Program.Usuario)
				If objRespuesta IsNot Nothing Then
					ListaPersonas = New ObservableCollection(Of CPX_BuscadorPersonas)(objRespuesta.Value)

					If Buscando Then
						_Buscando = False '// Desactivar la busqueda del comitente
						logBusquedaEspecifica = True
						seleccionarPersona(intIDPersona)
						Activar = True
					End If

					Activar = True

				End If
				RaiseEvent CargaPersonasCompleta(ListaPersonas.Count, logBusquedaEspecifica)
				MostrarConsultando = Visibility.Collapsed
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Comitentes", Me.ToString(), "consultarComitentes", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
			Activar = True
		End Try
	End Sub
#End Region

#Region "Métodos privados"
	Public Sub seleccionarPersona(ByVal pstrIDPersona As Integer?)
		Dim logBusquedaEspecifica As Boolean = False
		Dim objRes As CPX_BuscadorPersonas = Nothing
		Try
			If pstrIDPersona Is Nothing Then
				PersonaSeleccionado = Nothing
			Else
				If ListaPersonas IsNot Nothing Then
					objRes = (From per In ListaPersonas Where per.intID = pstrIDPersona Select per).ToList.FirstOrDefault
				End If
				'// mlogBuscandoPersona se inicializa en true cuando se asigna un valor a la propiedad intID
				If objRes Is Nothing Then
					'// Buscar una persona específica
					Activar = False
					ListaPersonas = Nothing
					MostrarConsultando = Visibility.Visible
					consultarPersonas()




				Else
					Buscando = False
					PersonaSeleccionado = objRes
					Activar = True
				End If

			End If
		Catch ex As Exception
			PersonaSeleccionado = Nothing
			Activar = True
			_Buscando = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el comitente seleccionado", Me.ToString(), "seleccionarComitente", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
			MostrarConsultando = Visibility.Collapsed
		End Try
	End Sub
#End Region

End Class
