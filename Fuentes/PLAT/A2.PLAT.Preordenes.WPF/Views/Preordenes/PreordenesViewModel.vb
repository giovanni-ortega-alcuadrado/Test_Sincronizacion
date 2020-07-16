
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web

''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (PLAT_PreordenesDomainServices.vb y dbPLAT_Preordenes.edmx)
''' Pantalla País (Maestros)
''' </summary>
''' <remarks>Natalia Andrea Otalvaro (Alcuadrado S.A.) - 21 de Febrero 2019</remarks>
''' <history>
'''
'''</history>
Public Class PreordenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblPreOrdenes
    Private mdcProxy As PLAT_PreordenesDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblPreOrdenes
    Dim strTipoFiltroBusqueda As String = String.Empty
    Public viewPreOrdenes As PreordenesView = Nothing

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                PrepararNuevaBusqueda()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado("FILTRAR", String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"
	Private _ListaEncabezadoCompleta As List(Of CPX_tblPreOrdenes)

	''' <summary>
	''' Lista de Registros que se encuentran cargadas en el grid del formulario
	''' </summary>
	Private _ListaEncabezado As List(Of CPX_tblPreOrdenes)
    Public Property ListaEncabezado() As List(Of CPX_tblPreOrdenes)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblPreOrdenes))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Registros para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private _EncabezadoSeleccionado As CPX_tblPreOrdenes
    Public Property EncabezadoSeleccionado() As CPX_tblPreOrdenes
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblPreOrdenes)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                intIDPreOrdenSeleccionado = _EncabezadoSeleccionado.intID
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private _intIDPreOrdenSeleccionado As Integer
    Public Property intIDPreOrdenSeleccionado() As Integer
        Get
            Return _intIDPreOrdenSeleccionado
        End Get
        Set(ByVal value As Integer)
            _intIDPreOrdenSeleccionado = value
            MyBase.CambioItem("intIDPreOrdenSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaPreordenes
    Public Property cb() As CamposBusquedaPreordenes
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaPreordenes)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _FiltroUsuario As String
    Public Property FiltroUsuario() As String
        Get
            Return _FiltroUsuario
        End Get
        Set(ByVal value As String)
            _FiltroUsuario = value
            MyBase.CambioItem("FiltroUsuario")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

	''' <summary>
	''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
	''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
	''' </summary>
	''' <history>
	''' </history>
	Public Sub Encabezado_Filtrar()
		Try
			strTipoFiltroBusqueda = "FILTRAR"
			ObtenerInformacionLista()
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_Filtrar", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Public Async Sub Encabezado_QuitarFiltro()
		Try
			FiltroUsuario = String.Empty
			strTipoFiltroBusqueda = "FILTRAR"
			Await ConsultarEncabezado("FILTRAR", FiltroUsuario)
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_QuitarFiltro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Sub ObtenerInformacionLista()
		Dim objListaFiltrada As New List(Of CPX_tblPreOrdenes)

		If Not IsNothing(_ListaEncabezadoCompleta) Then
			For Each li In _ListaEncabezadoCompleta
				If Not String.IsNullOrEmpty(FiltroUsuario) Then
					If li.intID.ToString.Contains(FiltroUsuario) Or
						li.intIDComitente.Contains(FiltroUsuario) Or
						li.strDescripcionTipoPreOrden.Contains(FiltroUsuario) Or
						li.strDescripcionTipoInversion.Contains(FiltroUsuario) Or
						li.strDescripcionIntencion.Contains(FiltroUsuario) Then
						objListaFiltrada.Add(li)
					End If
				Else
					objListaFiltrada.Add(li)
				End If
			Next
		End If

		ListaEncabezado = objListaFiltrada
	End Sub

	''' <summary>
	''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
	''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
	''' </summary>
	''' <history>
	''' </history>
	Public Async Sub Encabezado_Buscar()
        Try
			If Not cb.intID > 0 Or Not IsNothing(cb.dtmFechaInversion) Or Not IsNothing(cb.dtmFechaVigencia) Or cb.intIDCodigoPersona > 0 Or Not String.IsNullOrEmpty(cb.strTipoPreOrden) Or Not String.IsNullOrEmpty(cb.strTipoInversion) Or Not String.IsNullOrEmpty(cb.strIntencion) Or Not String.IsNullOrEmpty(cb.strEstado) Then
				strTipoFiltroBusqueda = "BUSQUEDA"
				Await ConsultarEncabezado("BUSQUEDA", String.Empty, 0, cb.intID, cb.dtmFechaInversion, cb.dtmFechaVigencia, cb.intIDCodigoPersona, cb.strIDComitente, cb.strTipoPreOrden, cb.strTipoInversion, Nothing, cb.strIntencion, cb.strEstado)
			End If
		Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
	''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
	''' </summary>
	''' <history>
	''' </history>
	''' 

	Public Async Sub Encabezado_ActualizarRegistro(ByVal pintIDRegistroActualizado As Integer)
		Dim strAccion As String = ValoresUserState.Actualizar.ToString()

		Try
			' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
			If strTipoFiltroBusqueda = "FILTRAR" Then
				Await ConsultarEncabezado("FILTRAR", FiltroVM, pintIDRegistroActualizado)
			ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
				Await ConsultarEncabezado("BUSQUEDA", String.Empty, Nothing, cb.strNombre)
			Else
				Await ConsultarEncabezado("FILTRAR", String.Empty, pintIDRegistroActualizado)
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
	''' </summary>
	''' <history>
	''' </history>
	Public Sub Encabezado_BorrarRegistro()
		Try
			If Not IsNothing(_EncabezadoSeleccionado) Then
				If mdcProxy.IsLoading Then
					A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
					Exit Sub
				End If

				If _EncabezadoSeleccionado.logActivo Then
					A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ANULARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado, False, String.Empty, False, True)
				Else
					A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_NOPERMITE_ANULARREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
		Try
			Dim strAccion As String = ValoresUserState.Actualizar.ToString()

			If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
				If Not IsNothing(_EncabezadoSeleccionado) Then
					IsBusy = True

					Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.PreOrdenes_EliminarIDAsync(_EncabezadoSeleccionado.intID, CType(sender, A2Utilidades.wppMensajePregunta).Observaciones, Program.Usuario, Program.HashConexion, Program.Maquina)

					If Not IsNothing(objRespuesta) Then
						If Not IsNothing(objRespuesta.Value) Then
							Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
							Dim objListaMensajes As New List(Of ProductoValidaciones)

							For Each li In objListaResultado
								objListaMensajes.Add(New ProductoValidaciones With {
											 .Campo = li.strCampo,
											 .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
											 .Mensaje = li.strMensaje
											 })
							Next
							If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
								A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

								Encabezado_BorrarRegistro(_EncabezadoSeleccionado.intID)
								IsBusy = False
							Else
								MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
								IsBusy = False
							End If
						Else
							A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
							IsBusy = False
						End If
					Else
						A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
						IsBusy = False
					End If
				End If
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
	''' </summary>
	''' <history>
	''' </history>
	Private Async Sub Encabezado_BorrarRegistro(ByVal pintIDRegistroBorrado As Integer)
		Try
			Await ConsultarEncabezado("FILTRAR", String.Empty, pintIDRegistroBorrado)
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
	''' <summary>
	''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
	''' </summary>
	Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaPreordenes
            objCB.dtmFechaInversion = Nothing
            objCB.dtmFechaVigencia = Nothing
            objCB.intID = 0
            objCB.intIDCodigoPersona = 0
            objCB.strNroDocumento = String.Empty
            objCB.strNombre = String.Empty
            objCB.strIntencion = String.Empty
            objCB.strTipoInversion = String.Empty
            objCB.strTipoPreOrden = String.Empty
            objCB.strEstado = "NINGUNO"
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Registro
    ''' </summary>
    ''' <param name="pstrOpcion">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
                                               Optional ByVal pstrFiltro As String = "",
                                               Optional ByVal pintIDRegistro As Integer = 0,
                                               Optional ByVal pintID As Nullable(Of Integer) = 0,
                                               Optional ByVal pdtmFechaInversion As Nullable(Of DateTime) = Nothing,
                                               Optional ByVal pdtmFechaVigencia As Nullable(Of DateTime) = Nothing,
                                               Optional ByVal pintIDCodigoPersona As Nullable(Of Integer) = Nothing,
                                               Optional ByVal pstrIDComitente As String = Nothing,
                                               Optional ByVal pstrTipoPreOrden As String = "",
                                               Optional ByVal pstrTipoInversion As String = "",
                                               Optional ByVal pintIDEntidad As Nullable(Of Integer) = Nothing,
                                               Optional ByVal pstrIntencion As String = "",
                                               Optional ByVal pstrEstado As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblPreOrdenes)) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.PreOrdenes_FiltrarAsync(pstrFiltro, Program.Usuario, Program.HashConexion)

            Else
                Dim logEstado As Nullable(Of Boolean) = Nothing

                If pstrEstado = "ACTIVO" Then
                    logEstado = True
                ElseIf pstrEstado = "INACTIVO" Then
                    logEstado = False
                End If

                objRespuesta = Await mdcProxy.PreOrdenes_ConsultarAsync(pintID, pdtmFechaInversion, pdtmFechaVigencia, pintIDCodigoPersona, pstrIDComitente, pstrTipoPreOrden, pstrTipoInversion, pintIDEntidad, pstrIntencion, logEstado, Program.Usuario, Program.HashConexion)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
					_ListaEncabezadoCompleta = objRespuesta.Value

					ObtenerInformacionLista()

					If pintIDRegistro > 0 Then
                        If ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).First
                        End If
                    End If

                    If ListaEncabezado.Count > 0 Then
                        If pstrOpcion = "BUSQUEDA" Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If (pstrOpcion = "FILTRAR" And Not String.IsNullOrEmpty(pstrFiltro)) Or (pstrOpcion = "BUSQUEDA") Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_BUSQUEDANOEXITOSA"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Notificaciones"

    Private Const TIPOMENSAJE_NOTIFICACIONPREORDENES = "PREORDENES_CRUZADAS_NOTIFICACION"

    Dim NroOrdenEditar As Integer = 0

    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try

            Dim objNotificacion As List(Of clsNotificacionPreordenes)

            If Not String.IsNullOrEmpty(pobjInfoNotificacion.strTipoMensaje) Then

                If pobjInfoNotificacion.strTipoMensaje.ToUpper = TIPOMENSAJE_NOTIFICACIONPREORDENES Then

                    If Not IsNothing(pobjInfoNotificacion.strInfoMensaje) Then

                        objNotificacion = New List(Of clsNotificacionPreordenes)

                        objNotificacion = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of clsNotificacionPreordenes))(pobjInfoNotificacion.strInfoMensaje)

                        If Not IsNothing(objNotificacion) Then
                            If objNotificacion.Count > 0 Then
								For Each li In objNotificacion
									If _ListaEncabezado.Where(Function(i) i.intIDPreOrdenOrigen = li.ID).Count > 0 Then
										Encabezado_Buscar()
										Encabezado_Filtrar()
										Exit For
									End If
								Next
							End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "LlegoNotificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' <history>
''' 
''' </history>
Public Class CamposBusquedaPreordenes
    Implements INotifyPropertyChanged

    Private _intID As Nullable(Of Integer)
    Public Property intID() As Nullable(Of Integer)
        Get
            Return _intID
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intID"))
        End Set
    End Property

    Private _dtmFechaInversion As Nullable(Of DateTime)
    Public Property dtmFechaInversion() As Nullable(Of DateTime)
        Get
            Return _dtmFechaInversion
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmFechaInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaInversion"))
        End Set
    End Property

    Private _dtmFechaVigencia As Nullable(Of DateTime)
    Public Property dtmFechaVigencia() As Nullable(Of DateTime)
        Get
            Return _dtmFechaVigencia
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _dtmFechaVigencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaVigencia"))
        End Set
    End Property

    Private _intIDCodigoPersona As Nullable(Of Integer)
    Public Property intIDCodigoPersona() As Nullable(Of Integer)
        Get
            Return _intIDCodigoPersona
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDCodigoPersona = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDCodigoPersona"))
        End Set
    End Property

    Private _strIDComitente As String
    Public Property strIDComitente() As String
        Get
            Return _strIDComitente
        End Get
        Set(ByVal value As String)
            _strIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDComitente"))
        End Set
    End Property

    Private _strNroDocumento As String
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Private _strTipoPreOrden As String
    Public Property strTipoPreOrden() As String
        Get
            Return _strTipoPreOrden
        End Get
        Set(ByVal value As String)
            _strTipoPreOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoPreOrden"))
        End Set
    End Property

    Private _strTipoInversion As String
    Public Property strTipoInversion() As String
        Get
            Return _strTipoInversion
        End Get
        Set(ByVal value As String)
            _strTipoInversion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoInversion"))
        End Set
    End Property

    Private _strIntencion As String
    Public Property strIntencion() As String
        Get
            Return _strIntencion
        End Get
        Set(ByVal value As String)
            _strIntencion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIntencion"))
        End Set
    End Property

    Private _strEstado As String
    Public Property strEstado() As String
        Get
            Return _strEstado
        End Get
        Set(ByVal value As String)
            _strEstado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class clsNotificacionPreordenes
    Public ID As Integer
    Public Accion As String
    Public Estado As String
End Class