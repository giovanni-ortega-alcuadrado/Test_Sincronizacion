Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Public Class BuscadorEspecieViewModel
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
    '-- Indicar cuando finaliza el servicio que carga la lista de especies
    Public Event CargaEspeciesCompleta(ByVal plogNroEspecies As Integer, ByVal plogBusquedaNemoEspecifico As Boolean)
#End Region

#Region "Constantes"

    ''' <summary>
    ''' T  : Todos
    ''' C  : Renta fija
    ''' A  : Renta variable
    ''' </summary>
    Public Enum ClasesEspecie As Byte
        T   '// Todos
        C  '// Renta fija
        A  '// Renta variable
    End Enum

    ''' <summary>
    ''' T  : Todas
    ''' C  : Activas
    ''' A  : Inactivas
    ''' </summary>
    Public Enum EstadosEspecie As Byte
        T  '// Todas
        A  '// Activas
        I  '// Inactivas
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

    Public mobjEspeciesConsulta As List(Of OYDUtilidades.BuscadorEspecies)

    Public ViewBuscadorEspecie As BuscadorEspecie

#End Region

#Region "Propiedades"

    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    ' PROPIEDADES
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------
    '-------------------------------------------------------------------------------------------------------------------------------------------------------------

    Private _mstrNemotecnico As String = String.Empty
    Public Property Nemotecnico() As String
        Get
            Return (_mstrNemotecnico)
        End Get
        Set(ByVal value As String)
            _mstrNemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))

            If Not value.Equals(String.Empty) Then
                _mlogBuscandoNemo = True
                Activar = True

                seleccionarEspecie(value)
            End If
        End Set
    End Property

    Private _mstrEstadoEspecie As EstadosEspecie = EstadosEspecie.T '// Por defecto consultar sobre todas las especies
    Public Property EstadoEspecie() As EstadosEspecie
        Get
            Return (_mstrEstadoEspecie)
        End Get
        Set(ByVal value As EstadosEspecie)
            _mstrEstadoEspecie = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EstadoEspecie"))
        End Set
    End Property

    Private _mstrAgrupamiento As String = String.Empty '// Por defecto consultar sobre todas las especies
    Public Property Agrupamiento() As String
        Get
            Return (_mstrAgrupamiento)
        End Get
        Set(ByVal value As String)
            _mstrAgrupamiento = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Agrupamiento"))
        End Set
    End Property

    Private _mstrClaseOrden As ClasesEspecie = ClasesEspecie.T '// Por defecto consultar sobre todas las especies
    Public Property ClaseOrden() As ClasesEspecie
        Get
            Return (_mstrClaseOrden)
        End Get
        Set(ByVal value As ClasesEspecie)
            _mstrClaseOrden = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClaseOrden"))
        End Set
    End Property

    Private _mstrCondicionFiltro As String = String.Empty
    Public Property CondicionFiltro() As String
        Get
            Return (_mstrCondicionFiltro)
        End Get
        Set(ByVal value As String)
            _mstrCondicionFiltro = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CondicionFiltro"))
        End Set
    End Property

    Private _ListaBusquedaControl As List(Of clsEspecies_Buscador)
    Public Property ListaBusquedaControl() As List(Of clsEspecies_Buscador)
        Get
            Return _ListaBusquedaControl
        End Get
        Set(ByVal value As List(Of clsEspecies_Buscador))
            _ListaBusquedaControl = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaBusquedaControl"))
        End Set
    End Property

    Private _ItemSeleccionadoBuscador As clsEspecies_Buscador
    Public Property ItemSeleccionadoBuscador() As clsEspecies_Buscador
        Get
            Return _ItemSeleccionadoBuscador
        End Get
        Set(ByVal value As clsEspecies_Buscador)
            _ItemSeleccionadoBuscador = value
            'le asigna valor al comitente seleccionado el cual controla el resto del codigo
            If Not IsNothing(value) Then
                Me.ViewBuscadorEspecie.mlogDigitandoFiltro = False
                NemotecnicoSeleccionado = value.ItemBusqueda
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ItemSeleccionadoBuscador"))
        End Set
    End Property

    Private _mobjEspecies As List(Of EspeciesAgrupadas) = Nothing
    Public Property Especies() As List(Of EspeciesAgrupadas)
        Get
            Return (_mobjEspecies)
        End Get
        Set(ByVal value As List(Of EspeciesAgrupadas))
            _mobjEspecies = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especies"))
        End Set
    End Property

    Private _mobjISIN As List(Of OYDUtilidades.BuscadorEspecies)
    Public Property Isines() As List(Of OYDUtilidades.BuscadorEspecies)
        Get
            Return (_mobjISIN)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.BuscadorEspecies))
            _mobjISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Isines"))
        End Set
    End Property

    Private _mobjNemotecnicoSeleccionado As EspeciesAgrupadas = Nothing
    Public Property NemotecnicoSeleccionado() As EspeciesAgrupadas
        Get
            Return (_mobjNemotecnicoSeleccionado)
        End Get
        Set(ByVal value As EspeciesAgrupadas)
            _mobjNemotecnicoSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NemotecnicoSeleccionado"))

            If value Is Nothing Then
                Me.Isines = Nothing
            Else
                Me.consultarIsines(value.Nemotecnico)
            End If
        End Set
    End Property

    Private _mobjEspecieSeleccionada As OYDUtilidades.BuscadorEspecies = Nothing
    Public Property EspecieSeleccionada() As OYDUtilidades.BuscadorEspecies
        Get
            Return (_mobjEspecieSeleccionada)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _mobjEspecieSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspecieSeleccionada"))
        End Set
    End Property

    Private _mlogActivar As Boolean = True
    Public Property Activar As Boolean
        Get
            Return (_mlogActivar)
        End Get
        Set(ByVal value As Boolean)
            _mlogActivar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Activar"))
        End Set
    End Property

    Private _mlogBuscandoNemo As Boolean = False
    Public Property Buscando As Boolean
        Get
            Return (_mlogBuscandoNemo)
        End Get
        Set(ByVal value As Boolean)
            _mlogBuscandoNemo = value
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

    'Modificado por Juan David Correa.
    'Se adiciona la propiedad para saber si se requiere filtrar las especies con reestricción.
    Private _CargarEspeciesConReestriccion As Boolean
    Public Property CargarEspeciesConReestriccion() As Boolean
        Get
            Return _CargarEspeciesConReestriccion
        End Get
        Set(ByVal value As Boolean)
            _CargarEspeciesConReestriccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CargarEspeciesConReestriccion"))
        End Set
    End Property


    'Modificado por Juan David Correa.
    'Se adiciona la propiedad para saber el tipo de negocio y filtrar las especies.
    Private _TipoNegocio As String = String.Empty
    Public Property TipoNegocio() As String
        Get
            Return _TipoNegocio
        End Get
        Set(ByVal value As String)
            _TipoNegocio = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoNegocio"))
        End Set
    End Property

    'Modificado por Juan David Correa.
    'Se adiciona la propiedad para saber el tipo de producto y filtrar las especies.
    Private _TipoProducto As String = String.Empty
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProducto"))
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


    Private _HabilitarConsultaISINES As Boolean = True
    Public Property HabilitarConsultaISINES() As Boolean
        Get
            Return _HabilitarConsultaISINES
        End Get
        Set(ByVal value As Boolean)
            _HabilitarConsultaISINES = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarConsultaISINES"))
        End Set
    End Property

    Private _TraerEspeciesVencidas As Boolean = False
    Public Property TraerEspeciesVencidas() As Boolean
        Get
            Return _TraerEspeciesVencidas
        End Get
        Set(ByVal value As Boolean)
            _TraerEspeciesVencidas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TraerEspeciesVencidas"))
        End Set
    End Property

    Private _FechaEmision As Nullable(Of DateTime)
    Public Property FechaEmision() As Nullable(Of DateTime)
        Get
            Return _FechaEmision
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaEmision = value
            If Not String.IsNullOrEmpty(_Modalidad) And Not IsNothing(FechaEmision) And Not IsNothing(FechaVencimiento) And Not String.IsNullOrEmpty(CondicionFiltro) Then
                consultarEspecies()
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaEmision"))
        End Set
    End Property
    Private _FechaVencimiento As Nullable(Of DateTime)
    Public Property FechaVencimiento() As Nullable(Of DateTime)
        Get
            Return _FechaVencimiento
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaVencimiento = value
            If Not String.IsNullOrEmpty(_Modalidad) And Not IsNothing(FechaEmision) And Not IsNothing(FechaVencimiento) And Not String.IsNullOrEmpty(CondicionFiltro) Then
                consultarEspecies()
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaVencimiento"))
        End Set
    End Property
    Private _Modalidad As String
    Public Property Modalidad() As String
        Get
            Return _Modalidad
        End Get
        Set(ByVal value As String)
            _Modalidad = value
            If Not String.IsNullOrEmpty(_Modalidad) And Not IsNothing(FechaEmision) And Not IsNothing(FechaVencimiento) And Not String.IsNullOrEmpty(CondicionFiltro) Then
                consultarEspecies()
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modalidad"))
        End Set
    End Property
    Private _logConsultarIsines As Boolean
    Public Property logConsultarIsines() As Boolean
        Get
            Return _logConsultarIsines
        End Get
        Set(ByVal value As Boolean)
            _logConsultarIsines = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logConsultarIsines"))
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

    Private Sub buscarEspeciesComplete(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Dim logBusquedaEspecifica As Boolean = False

        Try
            If lo.HasError Then
                If Not lo.Error Is Nothing Then
                    FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un error al ejecutar la consulta de especies pero no se recibió detalle del problema generado", Me.ToString, "buscarEspeciesComplete", lo.Error)
                    'Throw New Exception(lo.Error.Message, lo.Error.InnerException)
                    lo.MarkErrorAsHandled()
                    Exit Sub
                Else

                    Throw New Exception("Se presentó un error al ejecutar la consulta de especies pero no se recibió detalle del problema generado")
                End If
            Else
                _mlogInicializado = True

                mobjEspeciesConsulta = lo.Entities.ToList

                Especies = (From obj In (From obj In mobjEspeciesConsulta Select obj.Nemotecnico, obj.Especie, obj.Emisor, obj.Mercado, obj.EsAccion, obj.CodTipoTasaFija, obj.TipoTasa, obj.IdIndicador, obj.Indicador).Distinct
                            Select New EspeciesAgrupadas(obj.Nemotecnico, obj.Especie, obj.Emisor, obj.Mercado, obj.EsAccion, obj.CodTipoTasaFija, obj.TipoTasa, obj.IdIndicador, obj.Indicador)).ToList

                Dim objListaRespuesta As New List(Of EspeciesAgrupadas)
                Dim objListaBuscador As New List(Of clsEspecies_Buscador)

                If Not IsNothing(Especies) Then
                    For Each li In Especies
                        objListaRespuesta.Add(li)
                    Next
                End If

                For Each li In objListaRespuesta
                    objListaBuscador.Add(New clsEspecies_Buscador With {
                    .ItemBusqueda = li,
                    .DescripcionBuscador = LTrim(RTrim(li.Nemotecnico))
                    })
                Next

                ListaBusquedaControl = objListaBuscador

                If Not String.IsNullOrEmpty(_Modalidad) And Not IsNothing(FechaEmision) And Not IsNothing(FechaVencimiento) And Not String.IsNullOrEmpty(CondicionFiltro) Then
                    consultarIsinesPlus(CondicionFiltro, FechaEmision, FechaVencimiento, _Modalidad)
                End If
                '// Si se está buscando un nemotécnico específico y no estaba en la lista previamente cargada se busca en la lista actualizada
                If _mlogBuscandoNemo Then
                    _mlogBuscandoNemo = False '// Desactivar la busqueda del comitente
                    logBusquedaEspecifica = True

                    Me.seleccionarEspecie(Me.Nemotecnico)
                    Activar = True
                End If

                RaiseEvent CargaEspeciesCompleta(_mobjEspecies.Count, logBusquedaEspecifica)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la consulta de especies", Me.ToString(), "buscarEspeciesComplete", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
        Finally
            Activar = True
        End Try
        MostrarConsultando = Visibility.Collapsed
        mdcProxy.RejectChanges()
    End Sub

#End Region

#Region "Métodos públicos"

    Friend Sub consultarEspecies()
        Try
            'Modificado por Juan David Correa.
            'Se adiciona la condición para no consultar cuando ya se este consultando datos.
            If MostrarConsultando = Visibility.Collapsed Then
                Activar = False
                mdcProxy.BuscadorEspecies.Clear()
                'Modificado por Juan David Correa
                'Se llama el metodo de consulta de las especies de OYDPLUS para filtrar las especies por el tipo de negocio elegido.

                MostrarConsultando = Visibility.Visible
                Dim TextoSeguro As String = System.Web.HttpUtility.UrlEncode(Me.CondicionFiltro)

                If CargarEspeciesConReestriccion Then

                    mdcProxy.Load(mdcProxy.buscarEspeciesOyDPLUSQuery(TextoSeguro, Me.ClaseOrden.ToString, Me.EstadoEspecie.ToString, Me.Agrupamiento.ToString, Me.TipoNegocio.ToString, Me.TipoProducto.ToString, Program.Usuario, Me.TraerEspeciesVencidas, Program.HashConexion), AddressOf buscarEspeciesComplete, "")
                Else
                    mdcProxy.Load(mdcProxy.buscarEspeciesControlQuery(TextoSeguro, Me.ClaseOrden.ToString, Me.EstadoEspecie.ToString, Me.Agrupamiento.ToString, Program.Usuario, Me.TraerEspeciesVencidas, Program.HashConexion), AddressOf buscarEspeciesComplete, "")
                End If
            End If
        Catch ex As Exception
            MostrarConsultando = Visibility.Collapsed
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Comitentes", Me.ToString(), "consultarComitentes", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = True
        End Try
    End Sub

    Public Sub consultarIsines(ByVal pstrNemotecnico As String)
        Try
            Activar = False
            If Not String.IsNullOrEmpty(_Modalidad) And Not IsNothing(FechaEmision) And Not IsNothing(FechaVencimiento) And Not String.IsNullOrEmpty(CondicionFiltro) Then
                consultarIsinesPlus(CondicionFiltro, FechaEmision, FechaVencimiento, _Modalidad)
            Else
                Me.Isines = (From obj In mobjEspeciesConsulta Where obj.Nemotecnico = pstrNemotecnico Select obj).ToList
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Comitentes", Me.ToString(), "consultarComitentes", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = True
        End Try
    End Sub
    Public Sub consultarIsinesPlus(ByVal pstrNemotecnico As String, ByVal pdtmFechaEmision As Nullable(Of DateTime), pdtmFechaVencimiento As Nullable(Of DateTime), pstrModalidad As String)
        Try
            Activar = False
            Dim objISINES As New List(Of OYDUtilidades.BuscadorEspecies)

            If Not IsNothing(mobjEspeciesConsulta) Then
                If Not IsNothing(pdtmFechaEmision) And Not IsNothing(pdtmFechaVencimiento) And Not String.IsNullOrEmpty(pstrModalidad) Then

                    For Each obj In mobjEspeciesConsulta.Where(Function(i) i.Nemotecnico = pstrNemotecnico)
                        Dim logCumple As Boolean = False

                        If Not IsNothing(obj.Emision) And Not IsNothing(obj.Vencimiento) And Not String.IsNullOrEmpty(obj.CodModalidad) Then
                            If obj.Emision.Value = pdtmFechaEmision And obj.Vencimiento.Value = pdtmFechaVencimiento And obj.CodModalidad = pstrModalidad Then
                                logCumple = True
                            End If

                            If logCumple Then
                                objISINES.Add(obj)
                            End If
                        End If


                    Next
                Else
                    objISINES = (From obj In mobjEspeciesConsulta Where obj.Nemotecnico = pstrNemotecnico Select obj).ToList
                End If

            End If

            Me.Isines = objISINES
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Isines plus", Me.ToString(), "consultarIsinesPlus", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = True
        End Try
    End Sub

    Public Sub consultarIsinesFiltro(ByVal pstrNemotecnico As String, ByVal pstrFiltro As String)
        Try
            Activar = False
            Dim objISINES As New List(Of OYDUtilidades.BuscadorEspecies)

            If Not IsNothing(mobjEspeciesConsulta) Then
                If Not String.IsNullOrEmpty(pstrFiltro) Then
                    pstrFiltro = pstrFiltro.ToLower

                    For Each obj In mobjEspeciesConsulta.Where(Function(i) i.Nemotecnico = pstrNemotecnico)
                        Dim logCumple As Boolean = False

                        If Not IsNothing(obj.Emision) Then
                            If obj.Emision.Value.ToString("dd/MM/yyyy") = pstrFiltro Then
                                logCumple = True
                            End If
                        End If

                        If Not IsNothing(obj.Vencimiento) Then
                            If obj.Vencimiento.Value.ToString("dd/MM/yyyy") = pstrFiltro Then
                                logCumple = True
                            End If
                        End If

                        If Not IsNothing(obj.CodModalidad) Then
                            If obj.CodModalidad.ToLower.Contains(pstrFiltro) Then
                                logCumple = True
                            End If
                        End If

                        If Not IsNothing(obj.Indicador) Then
                            If obj.Indicador.ToLower.Contains(pstrFiltro) Then
                                logCumple = True
                            End If
                        End If

                        If Not IsNothing(obj.TasaFacial) Then
                            If obj.TasaFacial.ToString = pstrFiltro Then
                                logCumple = True
                            End If
                        End If

                        If Not IsNothing(obj.PuntosIndicador) Then
                            If obj.PuntosIndicador.ToString = pstrFiltro Then
                                logCumple = True
                            End If
                        End If

                        If Not IsNothing(obj.ISIN) Then
                            If obj.ISIN.ToLower.Contains(pstrFiltro) Then
                                logCumple = True
                            End If
                        End If

                        If logCumple Then
                            objISINES.Add(obj)
                        End If

                    Next
                Else
                    objISINES = (From obj In mobjEspeciesConsulta Where obj.Nemotecnico = pstrNemotecnico Select obj).ToList
                End If

            End If

            Me.Isines = objISINES
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de Comitentes", Me.ToString(), "consultarComitentes", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = True
        End Try
    End Sub

#End Region

#Region "Métodos privados"

    Public Sub seleccionarEspecie(ByVal pstrNemotecnico As String)
        Dim objRes As EspeciesAgrupadas = Nothing

        Try
            If pstrNemotecnico.Equals(String.Empty) Then
                NemotecnicoSeleccionado = Nothing
            Else
                If Not Especies Is Nothing Then
                    If (From obj In Especies Where obj.Nemotecnico = Nemotecnico Select obj).ToList.Count > 0 Then
                        objRes = (From obj In Especies Where obj.Nemotecnico = Nemotecnico Select obj).ToList.ElementAt(0)
                    Else
                        objRes = Nothing
                    End If
                End If

                '// mlogBuscandoNemo se inicializa en true cuando se asigna un valor a la propiedad Nemotecnico
                If objRes Is Nothing And _mlogBuscandoNemo Then
                    '// Buscar un menotécnico específico
                    Activar = False
                    mdcProxy.BuscadorEspecies.Clear()
                    'Modificado por Juan David Correa
                    'Se llama el metodo de consulta de las especies de OYDPLUS para filtrar las especies por el tipo de negocio elegido.
                    MostrarConsultando = Visibility.Visible
                    If CargarEspeciesConReestriccion And pstrNemotecnico.Equals(String.Empty) Then


                        mdcProxy.Load(mdcProxy.buscarEspeciesOyDPLUSQuery(pstrNemotecnico, ClasesEspecie.T.ToString, EstadosEspecie.T.ToString, "Nemotecnico", Me.TipoNegocio.ToString, Me.TipoProducto.ToString, Program.Usuario, Me.TraerEspeciesVencidas, Program.HashConexion), AddressOf buscarEspeciesComplete, "")
                    Else
                        mdcProxy.Load(mdcProxy.buscarEspeciesControlQuery(pstrNemotecnico, ClasesEspecie.T.ToString, EstadosEspecie.T.ToString, "Nemotecnico", Program.Usuario, Me.TraerEspeciesVencidas, Program.HashConexion), AddressOf buscarEspeciesComplete, "")
                    End If
                Else
                    Activar = True
                    _mlogBuscandoNemo = False

                    NemotecnicoSeleccionado = objRes
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la especie seleccionada", Me.ToString(), "seleccionarEspecie", Program.TituloSistema, Program.Maquina(), ex, Program.RutaServicioLog)
            Activar = True
            MostrarConsultando = Visibility.Collapsed
        End Try
    End Sub

#End Region

End Class
