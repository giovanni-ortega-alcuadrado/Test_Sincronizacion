
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
Public Class FormaPreordenesViewModel
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
    Public logConsultarPortafolioCliente As Boolean = True
    Public viewPreOrdenes As FormaPreordenesView = Nothing


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
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                Await consultarEncabezadoPorDefecto()
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
    ''' <summary>
    ''' ID del elemento a buscar
    ''' </summary>
    Private _intIDPreOrden As Integer
    Public Property intIDPreOrden() As Integer
        Get
            Return _intIDPreOrden
        End Get
        Set(ByVal value As Integer)
            _intIDPreOrden = value
            MyBase.CambioItem("intIDPreOrden")
        End Set
    End Property

    Private _MostrarBotones As Visibility = Visibility.Visible
    Public Property MostrarBotones() As Visibility
        Get
            Return _MostrarBotones
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotones = value
            MyBase.CambioItem("MostrarBotones")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblPreOrdenes
    Public Property EncabezadoEdicionSeleccionado() As tblPreOrdenes
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblPreOrdenes)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If _EncabezadoEdicionSeleccionado.intID > 0 Then
                    If _EncabezadoEdicionSeleccionado.logActivo Then
                        HabilitarEdicionDetalle = True
                    Else
                        HabilitarEdicionDetalle = False
                    End If
                Else
                    HabilitarEdicionDetalle = False
                End If

                If _EncabezadoEdicionSeleccionado.strTipoPreOrden = "V" And _EncabezadoEdicionSeleccionado.logActivo Then
                    HabilitarPortafolio = True
                Else
                    HabilitarPortafolio = False
                End If

                If _EncabezadoEdicionSeleccionado.strTipoInversion = "A" Then
                    HabilitarPrecio = True
                Else
                    HabilitarPrecio = False
                End If

				If _EncabezadoEdicionSeleccionado.strIntencion = "N" Then
					HabilitarValorNominal = True
				Else
					HabilitarValorNominal = False
				End If

				Encabezado_HabilitarEdicionRegistro()
			Else
				HabilitarEdicionDetalle = False
				Editando = False
				MyBase.CambioItem("Editando")
			End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private _HabilitarEdicionDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property

    Private _PreOrdenes_Portafolio As CPX_tblClientes_Portafolio
    Public Property PreOrdenes_Portafolio() As CPX_tblClientes_Portafolio
        Get
            Return _PreOrdenes_Portafolio
        End Get
        Set(ByVal value As CPX_tblClientes_Portafolio)
            _PreOrdenes_Portafolio = value
            MyBase.CambioItem("PreOrdenes_Portafolio")
        End Set
    End Property

    Private _HabilitarEdicion As Boolean = False
    Public Property HabilitarEdicion() As Boolean
        Get
            Return _HabilitarEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicion = value
            MyBase.CambioItem("HabilitarEdicion")
        End Set
    End Property

    Private _intIDPersona As Nullable(Of Integer)
    Public Property intIDPersona() As Nullable(Of Integer)
        Get
            Return _intIDPersona
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDPersona = value
            MyBase.CambioItem("intIDPersona")
        End Set
    End Property

    Private _strIDComitente As String
    Public Property strIDComitente() As String
        Get
            Return _strIDComitente
        End Get
        Set(ByVal value As String)
            _strIDComitente = value
            MyBase.CambioItem("strIDComitente")
        End Set
    End Property

    Private _strNroDocumento As String
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            MyBase.CambioItem("strNroDocumento")
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            MyBase.CambioItem("strNombre")
        End Set
    End Property

    Private _strNombreEspecie As String
    Public Property strNombreEspecie() As String
        Get
            Return _strNombreEspecie
        End Get
        Set(ByVal value As String)
            _strNombreEspecie = value
            MyBase.CambioItem("strNombreEspecie")
        End Set
    End Property

    Private _strNroDocumentoEntidad As String
    Public Property strNroDocumentoEntidad() As String
        Get
            Return _strNroDocumentoEntidad
        End Get
        Set(ByVal value As String)
            _strNroDocumentoEntidad = value
            MyBase.CambioItem("strNroDocumentoEntidad")
        End Set
    End Property

    Private _strNombreEntidad As String
    Public Property strNombreEntidad() As String
        Get
            Return _strNombreEntidad
        End Get
        Set(ByVal value As String)
            _strNombreEntidad = value
            MyBase.CambioItem("strNombreEntidad")
        End Set
    End Property

    Private _ListaCliente_Portafolio As List(Of CPX_tblClientes_Portafolio)
    Public Property ListaCliente_Portafolio() As List(Of CPX_tblClientes_Portafolio)
        Get
            Return _ListaCliente_Portafolio
        End Get
        Set(ByVal value As List(Of CPX_tblClientes_Portafolio))
            _ListaCliente_PortafolioPaginada = Nothing
            _ListaCliente_Portafolio = value
            MyBase.CambioItem("ListaCliente_Portafolio")
            MyBase.CambioItem("ListaCliente_PortafolioPaginada")
        End Set
    End Property

    Private _ListaCliente_PortafolioPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Registros para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaCliente_PortafolioPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCliente_Portafolio) Then
                If IsNothing(_ListaCliente_PortafolioPaginada) Then
                    Dim view = New PagedCollectionView(_ListaCliente_Portafolio)
                    _ListaCliente_PortafolioPaginada = view
                    Return view
                Else
                    Return (_ListaCliente_PortafolioPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TituloValor As String
    Public Property TituloValor() As String
        Get
            Return _TituloValor
        End Get
        Set(ByVal value As String)
            _TituloValor = value
            MyBase.CambioItem("TituloValor")
        End Set
    End Property

    Private _HabilitarPrecio As Boolean = True
    Public Property HabilitarPrecio() As Boolean
        Get
            Return _HabilitarPrecio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarPrecio = value
            MyBase.CambioItem("HabilitarPrecio")
        End Set
    End Property

    Private _HabilitarPortafolio As Boolean = False
    Public Property HabilitarPortafolio() As Boolean
        Get
            Return _HabilitarPortafolio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarPortafolio = value
            MyBase.CambioItem("HabilitarPortafolio")
        End Set
    End Property

    Private _HabilitarValorNominal As Boolean = True
    Public Property HabilitarValorNominal() As Boolean
        Get
            Return _HabilitarValorNominal
        End Get
        Set(ByVal value As Boolean)
            _HabilitarValorNominal = value
            MyBase.CambioItem("HabilitarValorNominal")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _BorrarEntidad As Boolean = False
    Public Property BorrarEntidad() As Boolean
        Get
            Return _BorrarEntidad
        End Get
        Set(ByVal value As Boolean)
            _BorrarEntidad = value
            MyBase.CambioItem("BorrarEntidad")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Sub Encabezado_NuevoRegistro()
        Try
            Dim objNvoEncabezado As New tblPreOrdenes
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'Obtiene el registro por defecto
            ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

			BorrarBuscadores()
			strIDComitente = String.Empty
            strNroDocumento = String.Empty
            strNombre = String.Empty
            strNombreEspecie = String.Empty
            strNroDocumentoEntidad = String.Empty
            strNombreEntidad = String.Empty

			EncabezadoEdicionSeleccionado = objNvoEncabezado

			Editando = True
			HabilitarEdicion = True
			MyBase.CambioItem("Editando")
			MyBase.ActualizarListaInconsistencias(Nothing)
		Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' </history>
    ''' 
    Public Async Sub Encabezado_ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            MyBase.ActualizarListaInconsistencias(Nothing)

            Dim strXMLDetalle As String = "<Detalles>"

            IsBusy = True

            If _EncabezadoEdicionSeleccionado.strTipoPreOrden = "V" Then
                If Not IsNothing(ListaCliente_Portafolio) Then
                    If ListaCliente_Portafolio.Where(Function(i) i.logSeleccionado).Count > 0 Then
                        Dim objItemSeleccionadoPortafolio = ListaCliente_Portafolio.Where(Function(i) i.logSeleccionado).First
                        strXMLDetalle = String.Format("{0}<Detalle intIDRegistro=""{1}"" lngIDRecibo=""{2}"" lngSecuencia=""{3}"" strNroTitulo=""{4}"" strInstrumento=""{5}"" dblTasaReferencia=""{6}"" intIDEntidad=""{7}"" dblValorNominal=""{8:n}"" dblValorCompra=""{9:n}"" dblVPNMercado=""{10:n}"" dtmFechaCompra=""{11:yyyy-MM-dd}"" />", strXMLDetalle,
                                                                     0,
                                                                     objItemSeleccionadoPortafolio.lngIDRecibo,
                                                                     objItemSeleccionadoPortafolio.lngSecuencia,
                                                                     objItemSeleccionadoPortafolio.strNroTitulo,
                                                                     objItemSeleccionadoPortafolio.strInstrumento,
                                                                     objItemSeleccionadoPortafolio.dblTasaReferencia,
                                                                     objItemSeleccionadoPortafolio.intIDEntidad,
                                                                     objItemSeleccionadoPortafolio.dblValorNominal,
                                                                     objItemSeleccionadoPortafolio.dblValorCompra,
                                                                     objItemSeleccionadoPortafolio.dblVPNMercado,
                                                                     objItemSeleccionadoPortafolio.dtmFechaCompra)
                    End If
                ElseIf Not IsNothing(PreOrdenes_Portafolio) Then
                    strXMLDetalle = String.Format("{0}<Detalle intIDRegistro=""{1}"" lngIDRecibo=""{2}"" lngSecuencia=""{3}"" strNroTitulo=""{4}"" strInstrumento=""{5}"" dblTasaReferencia=""{6}"" intIDEntidad=""{7}"" dblValorNominal=""{8:n}"" dblValorCompra=""{9:n}"" dblVPNMercado=""{10:n}"" dtmFechaCompra=""{11:yyyy-MM-dd}"" />", strXMLDetalle,
                                                  PreOrdenes_Portafolio.intID,
                                                  PreOrdenes_Portafolio.lngIDRecibo,
                                                  PreOrdenes_Portafolio.lngSecuencia,
                                                  PreOrdenes_Portafolio.strNroTitulo,
                                                  PreOrdenes_Portafolio.strInstrumento,
                                                  PreOrdenes_Portafolio.dblTasaReferencia,
                                                  PreOrdenes_Portafolio.intIDEntidad,
                                                  PreOrdenes_Portafolio.dblValorNominal,
                                                  PreOrdenes_Portafolio.dblValorCompra,
                                                  PreOrdenes_Portafolio.dblVPNMercado,
                                                  PreOrdenes_Portafolio.dtmFechaCompra)
                End If
            End If

            strXMLDetalle &= "</Detalles>"
            ErrorForma = String.Empty

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.PreOrdenes_ValidarAsync(_EncabezadoEdicionSeleccionado.intID,
                                                                                                                                _EncabezadoEdicionSeleccionado.dtmFechaInversion,
                                                                                                                                _EncabezadoEdicionSeleccionado.dtmFechaVigencia,
                                                                                                                                intIDPersona,
                                                                                                                                strIDComitente,
                                                                                                                                _EncabezadoEdicionSeleccionado.strTipoPreOrden,
                                                                                                                                _EncabezadoEdicionSeleccionado.strTipoInversion,
                                                                                                                                _EncabezadoEdicionSeleccionado.intIDEntidad,
                                                                                                                                _EncabezadoEdicionSeleccionado.strInstrumento,
                                                                                                                                _EncabezadoEdicionSeleccionado.strIntencion,
                                                                                                                                _EncabezadoEdicionSeleccionado.dblValor,
                                                                                                                                _EncabezadoEdicionSeleccionado.dblPrecio,
                                                                                                                                _EncabezadoEdicionSeleccionado.dblRentabilidadMinima,
                                                                                                                                _EncabezadoEdicionSeleccionado.dblRentabilidadMaxima,
                                                                                                                                _EncabezadoEdicionSeleccionado.strInstrucciones,
                                                                                                                                _EncabezadoEdicionSeleccionado.logActivo,
                                                                                                                                strXMLDetalle,
                                                                                                                                Program.Usuario, Program.HashConexion, Program.Maquina)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1

                    For Each li In objListaResultado
                        objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                    Next

                    If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        intIDRegistroActualizado = objListaResultado.First.intIDRegistro

                        HabilitarEdicion = False
						MyBase.ActualizarListaInconsistencias(Nothing)
						MyBase.FinalizoGuardadoRegistros()

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        viewPreOrdenes.EjecutarEventoGuardarRegistro(intIDRegistroActualizado, _EncabezadoEdicionSeleccionado)
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

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
	''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
	''' </summary>
	Public Async Sub Encabezado_HabilitarEdicionRegistro()
		Try
			If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

				HabilitarEdicion = False

				If _EncabezadoEdicionSeleccionado.logActivo Then
					If _EncabezadoEdicionSeleccionado.logEsParcial Then
						Editando = False
						MyBase.CambioItem("Editando")
					Else
						Editando = True
						MyBase.CambioItem("Editando")
					End If
				Else
					Editando = False
					MyBase.CambioItem("Editando")
				End If

				IsBusy = True

				ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)
				MyBase.ActualizarListaInconsistencias(Nothing)

				BorrarBuscadores()
				Await Consultar_Cliente_Portafolio()

				IsBusy = False
			End If
		Catch ex As Exception
			IsBusy = False
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

	''' <summary>
	''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
	''' </summary>
	Public Sub Encabezado_CancelarEditarRegistro(Optional ByVal plogNoConsultarEncabezado As Boolean = True)
        Try
            ErrorForma = String.Empty

            mdcProxy.RejectChanges()
			Editando = True
			HabilitarEdicion = False
            MyBase.CambioItem("Editando")
            MyBase.ActualizarListaInconsistencias(Nothing)

            BorrarBuscadores()

            If plogNoConsultarEncabezado Then
                ConsultarEncabezadoEdicion()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Encabezado_CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

	Public Sub BorrarBuscadores()
		Try
			If BorrarEspecie Then
				BorrarEspecie = False
			End If
			BorrarEspecie = True

			If BorrarEntidad Then
				BorrarEntidad = False
			End If
			BorrarEntidad = True

			viewPreOrdenes.ctlBuscadorCliente.BorrarPersonaSeleccionada()
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarBuscadores", Application.Current.ToString(), Program.Maquina, ex)
		End Try
	End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
	''' <summary>
	''' Retorna una copia del encabezado activo. 
	''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
	''' </summary>
	''' <history>
	''' </history>
	Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblPreOrdenes, ByRef pobjRegistroSalvar As tblPreOrdenes)
        Dim objEncabezado As tblPreOrdenes = New tblPreOrdenes

        Try
            If Not IsNothing(pobjRegistro) Then

                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.dblPrecio = pobjRegistro.dblPrecio
                objEncabezado.dblRentabilidadMaxima = pobjRegistro.dblRentabilidadMaxima
                objEncabezado.dblRentabilidadMinima = pobjRegistro.dblRentabilidadMinima
                objEncabezado.dblValor = pobjRegistro.dblValor
                objEncabezado.dtmFechaInversion = pobjRegistro.dtmFechaInversion
                objEncabezado.dtmFechaVigencia = pobjRegistro.dtmFechaVigencia
                objEncabezado.intIDCodigoPersona = pobjRegistro.intIDCodigoPersona
                objEncabezado.intIDEntidad = pobjRegistro.intIDEntidad
                objEncabezado.strInstrucciones = pobjRegistro.strInstrucciones
                objEncabezado.strInstrumento = pobjRegistro.strInstrumento
                objEncabezado.strIntencion = pobjRegistro.strIntencion
                objEncabezado.strTipoInversion = pobjRegistro.strTipoInversion
                objEncabezado.strTipoPreOrden = pobjRegistro.strTipoPreOrden
                objEncabezado.logActivo = pobjRegistro.logActivo
                objEncabezado.dtmFechaCreacion = pobjRegistro.dtmFechaCreacion
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.strUsuarioInsercion = pobjRegistro.strUsuarioInsercion

                pobjRegistroSalvar = objEncabezado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Modificación de las propeiedades del encabezado.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName = "strTipoPreOrden" Then
                    If _EncabezadoEdicionSeleccionado.strTipoPreOrden = "V" And _EncabezadoEdicionSeleccionado.logActivo Then
                        HabilitarPortafolio = True
                    Else
                        HabilitarPortafolio = False
                    End If

                    ListaCliente_Portafolio = Nothing
                    PreOrdenes_Portafolio = Nothing
                ElseIf e.PropertyName = "strTipoInversion" Then
                    If _EncabezadoEdicionSeleccionado.strTipoInversion = "A" Then
                        HabilitarPrecio = True
                        _EncabezadoEdicionSeleccionado.dblRentabilidadMinima = 0
                        _EncabezadoEdicionSeleccionado.dblRentabilidadMaxima = 0
                    Else
                        HabilitarPrecio = False
                        _EncabezadoEdicionSeleccionado.dblPrecio = 0
                    End If

                    If BorrarEspecie Then
                        BorrarEspecie = False
                    End If
                    BorrarEspecie = True

                    _EncabezadoEdicionSeleccionado.strInstrumento = String.Empty
                    strNombreEspecie = String.Empty

                    Await Consultar_Cliente_Portafolio()
                ElseIf e.PropertyName = "strIntencion" Then
                    If _EncabezadoEdicionSeleccionado.strIntencion = "N" Then
                        HabilitarValorNominal = True
                    Else
                        HabilitarValorNominal = False
                    End If

                    _EncabezadoEdicionSeleccionado.dblValor = 0
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Function consultarEncabezadoPorDefecto() As Task(Of Boolean)
        Try
            Dim objRespuesta As InvokeResult(Of tblPreOrdenes)

            objRespuesta = Await mdcProxy.PreOrdenes_ConsultarDefectoAsync(Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    mobjEncabezadoPorDefecto = objRespuesta.Value
                    Return True
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    Return False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                Return False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try
    End Function
    Public Async Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing

            If Not IsNothing(intIDPreOrden) Then
                If intIDPreOrden > 0 Then

                    If Editando Then
                        Encabezado_CancelarEditarRegistro(False)
                    End If

                    Await ConsultarEncabezadoEdicion(intIDPreOrden)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos del Registro a editar
    ''' </summary>
    ''' <param name="pintID">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuestaDatosAdicionales As InvokeResult(Of List(Of CPX_tblPreOrdenes)) = Nothing
            objRespuestaDatosAdicionales = Await mdcProxy.PreOrdenes_ConsultarAsync(intIDPreOrden, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuestaDatosAdicionales) Then
                If Not IsNothing(objRespuestaDatosAdicionales.Value) Then
                    If objRespuestaDatosAdicionales.Value.Count > 0 Then
                        Dim objDatosAdicionales = objRespuestaDatosAdicionales.Value.First

                        Dim objRespuesta As InvokeResult(Of tblPreOrdenes) = Nothing

                        ErrorForma = String.Empty

                        intIDPersona = objDatosAdicionales.intIDPersona
                        strIDComitente = objDatosAdicionales.intIDComitente
                        strNroDocumento = objDatosAdicionales.strNroDocumento
                        strNombre = objDatosAdicionales.strNombre
                        strNombreEspecie = objDatosAdicionales.strDescripcionInstrumento
                        strNroDocumentoEntidad = objDatosAdicionales.strNroDocumentoEntidad
                        strNombreEntidad = objDatosAdicionales.strNombreEntidad

                        objRespuesta = Await mdcProxy.PreOrdenes_ConsultarIDAsync(pintID, Program.Usuario, Program.HashConexion)

                        If Not IsNothing(objRespuesta) Then
                            If Not IsNothing(objRespuesta.Value) Then
                                EncabezadoEdicionSeleccionado = objRespuesta.Value
                                Await Consultar_PortafolioPreOrden(_EncabezadoEdicionSeleccionado.intID)
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar los datos del portafolio guardado
    ''' </summary>
    ''' <param name="pintID">Indica el ID de la preorden</param>
    Private Async Function Consultar_PortafolioPreOrden(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblClientes_Portafolio)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.PreOrdenesPortafolio_ConsultarAsync(pintID, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    If objRespuesta.Value.Count > 0 Then
                        PreOrdenes_Portafolio = objRespuesta.Value.First
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar los datos del portafolio guardado
    ''' </summary>
    Public Async Function Consultar_Cliente_Portafolio() As Task(Of Boolean)

        If Editando And logConsultarPortafolioCliente And Not String.IsNullOrEmpty(strIDComitente) And Not IsNothing(_EncabezadoEdicionSeleccionado) Then
            If _EncabezadoEdicionSeleccionado.strTipoPreOrden = "V" Then
                Dim logResultado As Boolean = False

                Try
                    IsBusy = True
                    Dim objRespuesta As InvokeResult(Of List(Of CPX_tblClientes_Portafolio)) = Nothing

                    ErrorForma = String.Empty

                    objRespuesta = Await mdcProxy.PreOrdenesClientes_ConsultarPortafolioAsync(strIDComitente, _EncabezadoEdicionSeleccionado.strTipoInversion, _EncabezadoEdicionSeleccionado.strInstrumento, Program.Usuario, Program.HashConexion)

                    If Not IsNothing(objRespuesta) Then
                        ListaCliente_Portafolio = objRespuesta.Value
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If

                    logResultado = True
                Catch ex As Exception
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
                Finally
                    IsBusy = False
                End Try

                Return (logResultado)
            Else
                ListaCliente_Portafolio = Nothing
                Return Nothing
            End If
        Else
            ListaCliente_Portafolio = Nothing
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Consultar los datos del portafolio guardado
    ''' </summary>
    Public Async Function Consultar_EntidadInstrumento() As Task(Of Boolean)

        If Editando And Not IsNothing(_EncabezadoEdicionSeleccionado) Then
            If Not String.IsNullOrEmpty(_EncabezadoEdicionSeleccionado.strInstrumento) Then
                Dim logResultado As Boolean = False

                Try
                    IsBusy = True
                    Dim objRespuesta As InvokeResult(Of CPX_tblEntidadEspecie) = Nothing

                    ErrorForma = String.Empty

                    objRespuesta = Await mdcProxy.PreOrdenesEspecie_EntidadAsync(_EncabezadoEdicionSeleccionado.strInstrumento, Program.Usuario, Program.HashConexion)

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            _EncabezadoEdicionSeleccionado.intIDEntidad = objRespuesta.Value.intIDEntidad
                            strNroDocumentoEntidad = objRespuesta.Value.strNroDocumento
                            strNombreEntidad = objRespuesta.Value.strNombre
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If

                    logResultado = True
                Catch ex As Exception
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
                Finally
                    IsBusy = False
                End Try

                Return (logResultado)
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

#End Region

#Region "Notificaciones"

    Private Const TIPOMENSAJE_NOTIFICACIONPREORDEN = "PREORDENES_NOTIFICACION"

    Dim NroOrdenEditar As Integer = 0

    Public Overrides Sub LlegoNotificacion(pobjInfoNotificacion As A2.Notificaciones.Cliente.clsNotificacion)
        Try

            Dim objNotificacion As List(Of clsNotificacionPreordenes)

            If Not String.IsNullOrEmpty(pobjInfoNotificacion.strTipoMensaje) Then

                If pobjInfoNotificacion.strTipoMensaje.ToUpper = TIPOMENSAJE_NOTIFICACIONPREORDEN Then

                    If Not IsNothing(pobjInfoNotificacion.strInfoMensaje) Then

                        objNotificacion = New List(Of clsNotificacionPreordenes)

                        objNotificacion = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of clsNotificacionPreordenes))(pobjInfoNotificacion.strInfoMensaje)

                        If Not IsNothing(objNotificacion) Then
                            If objNotificacion.Count > 0 Then
                                If Editando Then
                                    If EncabezadoEdicionSeleccionado.intID = objNotificacion.First.ID Then
                                        If pobjInfoNotificacion.strUsuario <> Program.Usuario And pobjInfoNotificacion.strMaquina = Program.Maquina Then
                                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("PREORDENES_MODPREORDEN"), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                        End If
                                        Encabezado_CancelarEditarRegistro()
                                    End If
                                End If
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
