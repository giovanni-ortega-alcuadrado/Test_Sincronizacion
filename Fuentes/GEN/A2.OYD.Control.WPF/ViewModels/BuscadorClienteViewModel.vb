Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class BuscadorClienteViewModel
    Implements INotifyPropertyChanged

#Region "Eventos"
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
    Public Event CargaComitentesCompleta(ByVal plogNroComitentes As Integer, ByVal plogBusquedaComitenteEspecifico As Boolean)
#End Region

#Region "Constantes"

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

#End Region

#Region "Variables"
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- DEFINICIÓN DE VARIABLES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private mlogMostrarMensajeLog As Boolean = False
    Public mdcProxy As UtilidadesDomainContext

#End Region

#Region "Propiedades"

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' PROPIEDADES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private _mstrIdComitente As String = String.Empty
    Public Property IdComitente() As String
        Get
            Return (_mstrIdComitente)
        End Get
        Set(ByVal value As String)
            _mstrIdComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdComitente"))

            If Not value.Equals(String.Empty) Then
                _mlogBuscandoComitente = True
                Activar = False
				seleccionarComitente(value)
			End If
		End Set
	End Property

	Private _mstrIdEstadoComitente As EstadosComitente = EstadosComitente.T '// Por defecto consultar sobre todos los clientes
	Public Property EstadoComitente() As EstadosComitente
		Get
			Return (_mstrIdEstadoComitente)
		End Get
		Set(ByVal value As EstadosComitente)
			_mstrIdEstadoComitente = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoComitente"))
		End Set
	End Property

	Private _mstrTipoVinculacion As TiposVinculacion = TiposVinculacion.T '// Por defecto consultar sobre todos los clientes
	Public Property TipoVinculacion() As TiposVinculacion
		Get
			Return (_mstrTipoVinculacion)
		End Get
		Set(ByVal value As TiposVinculacion)
			_mstrTipoVinculacion = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoVinculacion"))
		End Set
	End Property

	Private _mstrAgrupamiento As String = String.Empty '// Por defecto consultar sobre todos los clientes
	Public Property Agrupamiento() As String
		Get
			Return (_mstrAgrupamiento)
		End Get
		Set(ByVal value As String)
			_mstrAgrupamiento = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Agrupamiento"))
		End Set
	End Property

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

    Private _ItemSeleccionadoBuscador As clsClientes_Buscador
    Public Property ItemSeleccionadoBuscador() As clsClientes_Buscador
        Get
            Return _ItemSeleccionadoBuscador
        End Get
        Set(ByVal value As clsClientes_Buscador)
            _ItemSeleccionadoBuscador = value
            'le asigna valor al comitente seleccionado el cual controla el resto del codigo
            If Not IsNothing(value) Then
                ComitenteSeleccionado = value.ItemBusqueda
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemSeleccionadoBuscador"))
        End Set
    End Property

    Private _mobjComitentes As List(Of OYDUtilidades.BuscadorClientes) = Nothing
	Public Property Comitentes() As List(Of OYDUtilidades.BuscadorClientes)
		Get
			Return (_mobjComitentes)
		End Get
		Set(ByVal value As List(Of OYDUtilidades.BuscadorClientes))
			_mobjComitentes = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitentes"))
		End Set
	End Property

	Private _mobjComitenteSeleccionado As OYDUtilidades.BuscadorClientes = Nothing
	Public Property ComitenteSeleccionado() As OYDUtilidades.BuscadorClientes
		Get
			Return (_mobjComitenteSeleccionado)
		End Get
		Set(ByVal value As OYDUtilidades.BuscadorClientes)
			_mobjComitenteSeleccionado = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ComitenteSeleccionado"))
		End Set
	End Property

	Private _mlogActivar As Boolean = False
	Public Property Activar As Boolean
		Get
			Return (_mlogActivar)
		End Get
		Set(ByVal value As Boolean)
			_mlogActivar = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Activar"))
		End Set
	End Property

	Private _mlogBuscandoComitente As Boolean = False
	Public Property Buscando As Boolean
		Get
			Return (_mlogBuscandoComitente)
		End Get
		Set(ByVal value As Boolean)
			_mlogBuscandoComitente = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Buscando"))
		End Set
	End Property

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


	'**************************************************************************************************************************************************************************************
	''' <summary>
	''' Modificado por Juan David Correa
	''' Propiedades para habilitar la busqueda de clientes con las restricciones de OYDPLUS
	''' </summary>
	Private _CargarClientesRestricciones As Boolean = False
	Public Property CargarClientesRestricciones() As Boolean
		Get
			Return _CargarClientesRestricciones
		End Get
		Set(ByVal value As Boolean)
			_CargarClientesRestricciones = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarClientesTercero"))
		End Set
	End Property

	Private _CargarClienteXTipoProductoPerfil As Boolean
	Public Property CargarClienteXTipoProductoPerfil() As Boolean
		Get
			Return _CargarClienteXTipoProductoPerfil
		End Get
		Set(ByVal value As Boolean)
			_CargarClienteXTipoProductoPerfil = value
		End Set
	End Property

	Private _MostrarClientesTercero As Visibility = Visibility.Collapsed
	Public Property MostrarClientesTercero() As Visibility
		Get
			Return _MostrarClientesTercero
		End Get
		Set(ByVal value As Visibility)
			_MostrarClientesTercero = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarClientesTercero"))
		End Set
	End Property

	Private _ClienteTercero As Boolean = False
	Public Property ClienteTercero() As Boolean
		Get
			Return _ClienteTercero
		End Get
		Set(ByVal value As Boolean)
			_ClienteTercero = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClienteTercero"))
		End Set
	End Property

	Private _TextoFiltrado As String
	Public Property TextoFiltrado() As String
		Get
			Return _TextoFiltrado
		End Get
		Set(ByVal value As String)
			_TextoFiltrado = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TextoFiltrado"))
		End Set
	End Property


	Private _IDReceptor As String = String.Empty
	Public Property IDReceptor() As String
		Get
			Return _IDReceptor
		End Get
		Set(ByVal value As String)
			_IDReceptor = value
			'If Not String.IsNullOrEmpty(TipoNegocio) And _
			'    Not String.IsNullOrEmpty(TipoProducto) And _
			'    Not String.IsNullOrEmpty(IDReceptor) Then
			'    If Editando Then
			'        consultarComitentes()
			'    End If
			'End If
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDReceptor"))
		End Set
	End Property

	Private _TipoNegocio As String = String.Empty
	Public Property TipoNegocio() As String
		Get
			Return _TipoNegocio
		End Get
		Set(ByVal value As String)
			_TipoNegocio = value
			'If Not String.IsNullOrEmpty(TipoNegocio) And _
			'    Not String.IsNullOrEmpty(TipoProducto) And _
			'    Not String.IsNullOrEmpty(IDReceptor) Then
			'    If Editando Then
			'        consultarComitentes()
			'    End If
			'End If
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
		End Set
	End Property

	Private _TipoProducto As String = String.Empty
	Public Property TipoProducto() As String
		Get
			Return _TipoProducto
		End Get
		Set(ByVal value As String)
			_TipoProducto = value
			'If Not String.IsNullOrEmpty(TipoNegocio) And _
			'    Not String.IsNullOrEmpty(TipoProducto) And _
			'    Not String.IsNullOrEmpty(IDReceptor) Then
			'    If Editando Then
			'        consultarComitentes()
			'    End If
			'End If
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProducto"))
		End Set
	End Property

	Private _PerfilRiesgo As String = String.Empty
	Public Property PerfilRiesgo() As String
		Get
			Return _PerfilRiesgo
		End Get
		Set(ByVal value As String)
			_PerfilRiesgo = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PerfilRiesgo"))
		End Set
	End Property

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

	Private _ExcluirCodigosCompania As Boolean = False
	Public Property ExcluirCodigosCompania() As Boolean
		Get
			Return _ExcluirCodigosCompania
		End Get
		Set(ByVal value As Boolean)
			_ExcluirCodigosCompania = value
			RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ExcluirCodigosCompania"))
		End Set
	End Property

	Private _intIDCompania As Nullable(Of Integer) = Nothing
    Public Property intIDCompania() As Nullable(Of Integer)
        Get
            Return _intIDCompania
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDCompania"))
        End Set
    End Property

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
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-- PROCESOS DE INICIALIZACIÓN
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Public Sub New()

		Try
			If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
				mdcProxy = New UtilidadesDomainContext()
			Else
				mdcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
			End If

			'-- Inicializar servicios
			inicializarServicios()
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización del control", Me.ToString(), "New", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
		End Try

	End Sub

	''' <summary>
	''' Inicializa los proxies para acceder a los servicios web y configura los manejadores de evento de los diferentes métodos asincrónicos disponibles
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

#Region "Eventos respuesta de servicios"

	Private Sub buscarComitentesComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
		Dim logBusquedaEspecifica As Boolean = False

		Try
			If lo.HasError Then
				If Not lo.Error Is Nothing Then
					FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un error al ejecutar la consulta de Comitentes pero no se recibió detalle del problema generado", Me.ToString, "buscarComitentesComplete", lo.Error)
					lo.MarkErrorAsHandled()
					Exit Sub
					' Throw New Exception(lo.Error.Message, lo.Error.InnerException)
				Else
					FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un error al ejecutar la consulta de Comitentes pero no se recibió detalle del problema generado", Me.ToString, "buscarComitentesComplete", lo.Error)

					Throw New Exception("Se presentó un error al ejecutar la consulta de Comitentes pero no se recibió detalle del problema generado")
				End If
			Else
				_mlogInicializado = True

				Comitentes = lo.Entities.ToList

                Dim objListaRespuesta As New List(Of OYDUtilidades.BuscadorClientes)
                Dim objListaBuscador As New List(Of clsClientes_Buscador)

                If Not IsNothing(Comitentes) Then
                    For Each li In Comitentes
                        objListaRespuesta.Add(li)
                    Next
                End If

                For Each li In objListaRespuesta
                    objListaBuscador.Add(New clsClientes_Buscador With {
                    .ItemBusqueda = li,
                    .DescripcionBuscador = LTrim(RTrim(li.CodigoOYD))
                    })
                Next

                ListaBusquedaControl = objListaBuscador

                '// Si se está buscando un comitente específico y no estaba en la lista previamente cargada se busca en la lista actualizada
                If _mlogBuscandoComitente Then
					_mlogBuscandoComitente = False '// Desactivar la busqueda del comitente
					logBusquedaEspecifica = True

					Me.seleccionarComitente(Me.IdComitente)
					Activar = True
				End If

				RaiseEvent CargaComitentesCompleta(_mobjComitentes.Count, logBusquedaEspecifica)
			End If
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de Comitentes", Me.ToString(), "buscarComitentesComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
		Finally
			Activar = True
		End Try

		MostrarConsultando = Visibility.Collapsed
		mdcProxy.RejectChanges()
	End Sub

#End Region

#Region "Métodos públicos"

	Friend Sub consultarComitentes()
		Try
			'Modificado por Juan David Correa.
			'Se adiciona la condición para no consultar cuando ya se este consultando datos.
			If MostrarConsultando = Visibility.Collapsed Then
				Activar = False
				mdcProxy.BuscadorClientes.Clear()
                'Modificado por Juan David Correa.
                'Fecha Agosto 03 del 2012.
                'Se adiciona la busqueda de los clientes terceros siempre y cuando la funcionalidad este habilitada en el buscador.
                '****************************************************************************************************************************************************************
                MostrarConsultando = Visibility.Visible
                Dim TextoSeguro As String = System.Web.HttpUtility.UrlEncode(Me.CondicionFiltro)
                If CargarClientesRestricciones Then
                    If CargarClienteXTipoProductoPerfil Then
                        If ConFiltro = False Then
                            mdcProxy.Load(mdcProxy.buscarClientesOYDPLUSQuery(TextoSeguro, Me.EstadoComitente.ToString, Me.TipoVinculacion.ToString, Me.Agrupamiento,
                                                                     False, False, Me.IDReceptor, Me.TipoNegocio, Me.TipoProducto, Me.PerfilRiesgo, Program.Usuario,
                                                                     ExcluirCodigosCompania, intIDCompania, Program.HashConexion), AddressOf buscarComitentesComplete, "")
                        Else
                            mdcProxy.Load(mdcProxy.buscarClientesOYDPLUSconFiltrosQuery(TextoSeguro, Me.EstadoComitente.ToString, Me.TipoVinculacion.ToString, Me.Agrupamiento,
                                                                     False, False, Me.IDReceptor, Me.TipoNegocio, Me.TipoProducto, Me.PerfilRiesgo, Program.Usuario,
                                                                     ExcluirCodigosCompania, intIDCompania, Program.HashConexion, Me.filtroAdicional1, Me.filtroAdicional2, Me.filtroAdicional3), AddressOf buscarComitentesComplete, "")
                        End If

                    Else
                        If ConFiltro = False Then
                            mdcProxy.Load(mdcProxy.buscarClientesOYDPLUSQuery(TextoSeguro, Me.EstadoComitente.ToString, Me.TipoVinculacion.ToString, Me.Agrupamiento,
                                                                     Me.CargarClientesRestricciones, Me.ClienteTercero, Me.IDReceptor, Me.TipoNegocio, Me.TipoProducto, Me.PerfilRiesgo, Program.Usuario,
                                                                     ExcluirCodigosCompania, intIDCompania, Program.HashConexion), AddressOf buscarComitentesComplete, "")
                        Else
                            mdcProxy.Load(mdcProxy.buscarClientesOYDPLUSconFiltrosQuery(TextoSeguro, Me.EstadoComitente.ToString, Me.TipoVinculacion.ToString, Me.Agrupamiento,
                                                                    False, False, Me.IDReceptor, Me.TipoNegocio, Me.TipoProducto, Me.PerfilRiesgo, Program.Usuario,
                                                                    ExcluirCodigosCompania, intIDCompania, Program.HashConexion, Me.filtroAdicional1, Me.filtroAdicional2, Me.filtroAdicional3), AddressOf buscarComitentesComplete, "")
                        End If
                    End If

                Else
                    If ConFiltro = False Then
                        mdcProxy.Load(mdcProxy.buscarClientesQuery(TextoSeguro, Me.EstadoComitente.ToString, Me.TipoVinculacion.ToString, Me.Agrupamiento, Program.Usuario, ExcluirCodigosCompania, intIDCompania, Program.HashConexion), AddressOf buscarComitentesComplete, "")
                    Else
                        mdcProxy.Load(mdcProxy.buscarClientesconFiltrosQuery(TextoSeguro, Me.EstadoComitente.ToString, Me.TipoVinculacion.ToString, Me.Agrupamiento, Program.Usuario, ExcluirCodigosCompania, intIDCompania, Program.HashConexion, Me.filtroAdicional1, Me.filtroAdicional2, Me.filtroAdicional3), AddressOf buscarComitentesComplete, "")
                    End If
                End If
                '****************************************************************************************************************************************************************
            End If
		Catch ex As Exception
            MostrarConsultando = Visibility.Collapsed
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Comitentes", Me.ToString(), "consultarComitentes", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = True
		End Try
	End Sub

#End Region

#Region "Métodos privados"

	Public Sub seleccionarComitente(ByVal pstrIdComitente As String)
		Dim objRes As OYDUtilidades.BuscadorClientes = Nothing

		Try
			If pstrIdComitente.Equals(String.Empty) Then
				ComitenteSeleccionado = Nothing
			Else
				If Not Comitentes Is Nothing Then
					If (From clt In Comitentes Where clt.IdComitente = pstrIdComitente Select clt).ToList.Count > 0 Then
						objRes = (From clt In Comitentes Where clt.IdComitente = pstrIdComitente Select clt).ToList.ElementAt(0)
					Else
						objRes = Nothing
					End If
				End If

				'// mlogBuscandoComitente se inicializa en true cuando se asigna un valor a la propiedad IdComitente
				If objRes Is Nothing And _mlogBuscandoComitente Then
					'// Buscar u comitente específico
					Activar = False
					mdcProxy.BuscadorClientes.Clear()
					'Modificado por Juan David Correa.
					'Fecha Agosto 03 del 2012.
					'Se adiciona la busqueda de los clientes terceros siempre y cuando la funcionalidad este habilitada en el buscador.
					'****************************************************************************************************************************************************************
					MostrarConsultando = Visibility.Visible
					If CargarClientesRestricciones And pstrIdComitente.Equals(String.Empty) Then
						If CargarClienteXTipoProductoPerfil Then
                            mdcProxy.Load(mdcProxy.buscarClientesOYDPLUSQuery(pstrIdComitente, EstadosComitente.T.ToString, TiposVinculacion.T.ToString, "portafoliocliente",
                                                                          False, False, Me.IDReceptor, Me.TipoNegocio, Me.TipoProducto, Me.PerfilRiesgo, Program.Usuario,
                                                                          ExcluirCodigosCompania, intIDCompania, Program.HashConexion), AddressOf buscarComitentesComplete, "")
                        Else
                            mdcProxy.Load(mdcProxy.buscarClientesOYDPLUSQuery(pstrIdComitente, EstadosComitente.T.ToString, TiposVinculacion.T.ToString, "IdComitente",
                                                                          Me.CargarClientesRestricciones, Me.ClienteTercero, Me.IDReceptor, Me.TipoNegocio, Me.TipoProducto, Me.PerfilRiesgo, Program.Usuario,
                                                                          ExcluirCodigosCompania, intIDCompania, Program.HashConexion), AddressOf buscarComitentesComplete, "")
                        End If
					Else
                        mdcProxy.Load(mdcProxy.buscarClientesQuery(pstrIdComitente, EstadosComitente.T.ToString, TiposVinculacion.T.ToString, "IdComitente", Program.Usuario,
                                                                   ExcluirCodigosCompania, intIDCompania, Program.HashConexion), AddressOf buscarComitentesComplete, "")
                    End If
				Else
					_mlogBuscandoComitente = False

					ComitenteSeleccionado = objRes

					Activar = True
				End If
            End If
        Catch ex As Exception
            ComitenteSeleccionado = Nothing
            Activar = True
            _mlogBuscandoComitente = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el comitente seleccionado", Me.ToString(), "seleccionarComitente", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            MostrarConsultando = Visibility.Collapsed
        End Try
    End Sub

#End Region

End Class
