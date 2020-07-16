' Desarrollo de pestaña de Datos giros, para guardar el beneficiario
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public Class DatosGirosViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CPX_DatosGiros
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CPX_DatosGiros
    Private mobjEncabezadoGuardadoAnterior As CPX_DatosGiros
    Public viewListaPrincipal As DatosGirosView
    Private logVentanaDetalleActiva As Boolean = False
    Private strNombreProducto As String = String.Empty

#End Region


#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        PrepararNuevaBusqueda()
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba: Id_1
    ''' Creado por       : Juan David Correa (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Octubre 30/2017
    ''' Pruebas CB       : Juan David Correa (Alcuadrado S.A.) - Octubre 30/2017 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                Editando = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return logResultado
    End Function

    Public Async Function OrdenesCombos() As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosAsync(Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    dicCombosGeneral = clsGenerales.CargarListas(objRespuesta.Value)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Function


#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaOrdenes
    Public Property cb() As CamposBusquedaOrdenes
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaOrdenes)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    'Persona buscador
    Private _PersonaSelected As CPX_BuscadorPersonas
    Public Property PersonaSelected() As CPX_BuscadorPersonas
        Get
            Return _PersonaSelected
        End Get
        Set(ByVal value As CPX_BuscadorPersonas)
            _PersonaSelected = value
        End Set
    End Property

    Private _dicCombosGeneral As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosGeneral() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosGeneral
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosGeneral = value
            CambioItem("dicCombosGeneral")
        End Set
    End Property


    Private _lstCodAgente As List(Of CPX_ComboOrdenes)
    Public Property lstCodAgente() As List(Of CPX_ComboOrdenes)
        Get
            Return _lstCodAgente
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenes))
            _lstCodAgente = value
            CambioItem("lstCodAgente")
        End Set
    End Property

    Private _lstCodPais As List(Of CPX_ComboOrdenes)
    Public Property lstCodPais() As List(Of CPX_ComboOrdenes)
        Get
            Return _lstCodPais
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenes))
            _lstCodPais = value
            CambioItem("lstCodPais")
        End Set
    End Property

    Private _lstMonedaGiro As List(Of CPX_ComboOrdenes)
    Public Property lstMonedaGiro() As List(Of CPX_ComboOrdenes)
        Get
            Return _lstMonedaGiro
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenes))
            _lstMonedaGiro = value
            CambioItem("lstMonedaGiro")
        End Set
    End Property


    ''' <summary>
    ''' Lista de Parametro que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_DatosGiros)
    Public Property ListaEncabezado() As List(Of CPX_DatosGiros)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_DatosGiros))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            viewListaPrincipal.ListaDetalle = _ListaEncabezado

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Parametro para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Parametro que se encuentra seleccionado
    ''' </summary>
    Private _EncabezadoSeleccionado As CPX_DatosGiros
    Public Property EncabezadoSeleccionado() As CPX_DatosGiros
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_DatosGiros)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Dim logDesdeViewModel As Boolean = False

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As CPX_DatosGiros
    Public Property EncabezadoEdicionSeleccionado() As CPX_DatosGiros
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As CPX_DatosGiros)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private WithEvents _RegistroEncabezado As tblOrdenes
    Public Property RegistroEncabezado() As tblOrdenes
        Get
            Return _RegistroEncabezado
        End Get
        Set(ByVal value As tblOrdenes)
            _RegistroEncabezado = value
            mobjEncabezadoAnterior = Nothing
            mobjEncabezadoGuardadoAnterior = Nothing
            ListaEncabezado = Nothing
            ConsultarInformacionEncabezado()
            consultarEncabezadoPorDefecto()
            MyBase.CambioItem("RegistroEncabezado")
        End Set
    End Property

    Private _DetalleGuardaIndependiente As Boolean = False
    Public Property DetalleGuardaIndependiente() As Boolean
        Get
            Return _DetalleGuardaIndependiente
        End Get
        Set(ByVal value As Boolean)
            _DetalleGuardaIndependiente = value
            If _DetalleGuardaIndependiente Then
                MostrarCamposGuardaIndependiente = Visibility.Visible
            Else
                MostrarCamposGuardaIndependiente = Visibility.Collapsed
            End If
            MyBase.CambioItem("DetalleGuardaIndependiente")
        End Set
    End Property

    Private _MostrarCamposGuardaIndependiente As Visibility = Visibility.Collapsed
    Public Property MostrarCamposGuardaIndependiente() As Visibility
        Get
            Return _MostrarCamposGuardaIndependiente
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposGuardaIndependiente = value
            MyBase.CambioItem("MostrarCamposGuardaIndependiente")
        End Set
    End Property

    Private _HabilitarBotonesAcciones As Boolean = True
    Public Property HabilitarBotonesAcciones() As Boolean
        Get
            Return _HabilitarBotonesAcciones
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonesAcciones = value
            'If _HabilitarBotonesAcciones = False Then
            '    ConsultarInformacionEncabezado()
            'End If
            MyBase.CambioItem("HabilitarBotonesAcciones")
        End Set
    End Property


    Private _Habilitar_GuardarYCopiarAnterior As Boolean = True
    Public Property Habilitar_GuardarYCopiarAnterior() As Boolean
        Get
            Return _Habilitar_GuardarYCopiarAnterior
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_GuardarYCopiarAnterior = value
            MyBase.CambioItem("Habilitar_GuardarYCopiarAnterior")
        End Set
    End Property

    Private _HabilitarBoton_GuardarYCopiarAnterior As Boolean = False
    Public Property HabilitarBoton_GuardarYCopiarAnterior() As Boolean
        Get
            Return _HabilitarBoton_GuardarYCopiarAnterior
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBoton_GuardarYCopiarAnterior = value
            MyBase.CambioItem("HabilitarBoton_GuardarYCopiarAnterior")
        End Set
    End Property

    Private _Habilitar_GuardarYCrearNuevo As Boolean = True
    Public Property Habilitar_GuardarYCrearNuevo() As Boolean
        Get
            Return _Habilitar_GuardarYCrearNuevo
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_GuardarYCrearNuevo = value
            MyBase.CambioItem("Habilitar_GuardarYCrearNuevo")
        End Set
    End Property

    Private _HabilitarBoton_GuardarYCrearNuevo As Boolean = False
    Public Property HabilitarBoton_GuardarYCrearNuevo() As Boolean
        Get
            Return _HabilitarBoton_GuardarYCrearNuevo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBoton_GuardarYCrearNuevo = value
            MyBase.CambioItem("HabilitarBoton_GuardarYCrearNuevo")
        End Set
    End Property
#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    '    ''' <summary>
    '    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    '    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    '    ''' </summary>
    '    ''' <history>
    '    ''' </history>
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            CrearNuevoDetalle()
            MyBase.NuevoRegistroDetalle()
            AbrirDetalle()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CrearNuevoDetalle(Optional ByVal plogCopiarValorRegistroAnterior As Boolean = False)
        Try

            Dim objNvoEncabezado As New CPX_DatosGiros
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If plogCopiarValorRegistroAnterior Then
                'Obtiene el registro por defecto
                ObtenerRegistroAnterior(mobjEncabezadoGuardadoAnterior, objNvoEncabezado)
            Else
                'Obtiene el registro por defecto
                ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            End If

            objNvoEncabezado.intIDOrden = RegistroEncabezado.intID

            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            If Not IsNothing(_ListaEncabezado) Then
                objNvoEncabezado.intID = -(_ListaEncabezado.Count + 1)
            End If

            Editando = True
            If _Habilitar_GuardarYCopiarAnterior Then
                HabilitarBoton_GuardarYCopiarAnterior = True
            End If
            If _Habilitar_GuardarYCrearNuevo Then
                HabilitarBoton_GuardarYCrearNuevo = True
            End If
            MyBase.CambioItem("Editando")

            EncabezadoEdicionSeleccionado = objNvoEncabezado
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CrearNuevoDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub EditarRegistroDetalle()
        Try
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

                Editando = True
                If _Habilitar_GuardarYCopiarAnterior Then
                    HabilitarBoton_GuardarYCopiarAnterior = True
                End If
                If _Habilitar_GuardarYCrearNuevo Then
                    HabilitarBoton_GuardarYCrearNuevo = True
                End If
                MyBase.CambioItem("Editando")

                IsBusy = False

                MyBase.EditarRegistro()
                AbrirDetalle()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Function ActualizarRegistroDetalle() As Boolean
        Dim plogRetorno As Boolean = False
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty
            IsBusy = True

            mobjEncabezadoGuardadoAnterior = _EncabezadoEdicionSeleccionado


            Dim objListaEncabezado As New List(Of CPX_DatosGiros)
            If Not IsNothing(_ListaEncabezado) Then
                For Each li In _ListaEncabezado
                    objListaEncabezado.Add(li)
                Next
            End If

            If objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).Count > 0 Then
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strCodAgenteOper = _EncabezadoEdicionSeleccionado.strCodAgenteOper
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIdPaisGiro = _EncabezadoEdicionSeleccionado.intIdPaisGiro
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIdMonedaGiro = _EncabezadoEdicionSeleccionado.intIdMonedaGiro
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblMontoMonedaGiro = _EncabezadoEdicionSeleccionado.dblMontoMonedaGiro
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIdComitente = _EncabezadoEdicionSeleccionado.intIDComitente
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strNombreBeneficiario = _EncabezadoEdicionSeleccionado.strNombreBeneficiario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strNumeroDocumentoBeneficiario = _EncabezadoEdicionSeleccionado.strNumeroDocumentoBeneficiario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strTipoIdentificacionBeneficiario = _EncabezadoEdicionSeleccionado.strTipoIdentificacionBeneficiario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion
            Else
                Dim objNvoEncabezado As New CPX_DatosGiros

                objNvoEncabezado.intID = _EncabezadoEdicionSeleccionado.intID
                objNvoEncabezado.strCodAgenteOper = _EncabezadoEdicionSeleccionado.strCodAgenteOper
                objNvoEncabezado.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objNvoEncabezado.intIdPaisGiro = _EncabezadoEdicionSeleccionado.intIdPaisGiro
                objNvoEncabezado.intIdMonedaGiro = _EncabezadoEdicionSeleccionado.intIdMonedaGiro
                objNvoEncabezado.dblMontoMonedaGiro = _EncabezadoEdicionSeleccionado.dblMontoMonedaGiro
                objNvoEncabezado.intIDComitente = _EncabezadoEdicionSeleccionado.intIDComitente
                objNvoEncabezado.strNombreBeneficiario = _EncabezadoEdicionSeleccionado.strNombreBeneficiario
                objNvoEncabezado.strNumeroDocumentoBeneficiario = _EncabezadoEdicionSeleccionado.strNumeroDocumentoBeneficiario
                objNvoEncabezado.strTipoIdentificacionBeneficiario = _EncabezadoEdicionSeleccionado.strTipoIdentificacionBeneficiario
                objNvoEncabezado.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario
                objNvoEncabezado.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion


                EncabezadoSeleccionado = objNvoEncabezado

                objListaEncabezado.Add(EncabezadoSeleccionado)
            End If

            ListaEncabezado = objListaEncabezado
            plogRetorno = True

            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return plogRetorno
    End Function

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Sub VisualizarRegistroDetalle()
        Try
            MyBase.ActualizarListaInconsistencias(Nothing, Program.TituloSistema, False)
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If HabilitarBotonesAcciones Then
                    Editando = True
                    If _Habilitar_GuardarYCopiarAnterior Then
                        HabilitarBoton_GuardarYCopiarAnterior = True
                    End If
                    If _Habilitar_GuardarYCrearNuevo Then
                        HabilitarBoton_GuardarYCrearNuevo = True
                    End If
                Else
                    Editando = False
                    HabilitarBoton_GuardarYCopiarAnterior = False
                    HabilitarBoton_GuardarYCrearNuevo = False
                End If
                MyBase.CambioItem("Editando")
                AbrirDetalle()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub BorrarRegistroDetalle()
        Try
            MyBase.BorrarRegistroDetalle()

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ELIMINARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    Dim objListaDetalle As New List(Of CPX_DatosGiros)

                    For Each li In ListaEncabezado
                        objListaDetalle.Add(li)
                    Next

                    If objListaDetalle.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count > 0 Then
                        objListaDetalle.Remove(objListaDetalle.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).First)
                    End If

                    ListaEncabezado = objListaDetalle
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AbrirDetalle()
        Try
            If logVentanaDetalleActiva = False Then
                Dim objViewDetalle As New DatosGirosModalView(Me)
                objViewDetalle.Owner = Window.GetWindow(Me.viewListaPrincipal)
                logVentanaDetalleActiva = True
                Program.Modal_OwnerMainWindowsPrincipal(objViewDetalle)
                objViewDetalle.ShowDialog()
                ConsultarEncabezadoEdicion()
                logVentanaDetalleActiva = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "AbrirDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Sub CancelarRegistroDetalle()
        Try
            Editando = False
            HabilitarBoton_GuardarYCopiarAnterior = False
            HabilitarBoton_GuardarYCrearNuevo = False
            MyBase.CambioItem("Editando")
            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As CPX_DatosGiros, ByRef pobjRegistroSalvar As CPX_DatosGiros)
        Dim objEncabezado As CPX_DatosGiros = New CPX_DatosGiros

        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.strCodAgenteOper = pobjRegistro.strCodAgenteOper
                objEncabezado.intIDOrden = pobjRegistro.intIDOrden
                objEncabezado.intIdPaisGiro = pobjRegistro.intIdPaisGiro
                objEncabezado.intIdMonedaGiro = pobjRegistro.intIdMonedaGiro
                objEncabezado.dblMontoMonedaGiro = pobjRegistro.dblMontoMonedaGiro
                objEncabezado.intIDComitente = pobjRegistro.intIDComitente
                objEncabezado.strNombreBeneficiario = pobjRegistro.strNombreBeneficiario
                objEncabezado.strNumeroDocumentoBeneficiario = pobjRegistro.strNumeroDocumentoBeneficiario
                objEncabezado.strTipoIdentificacionBeneficiario = pobjRegistro.strTipoIdentificacionBeneficiario
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion

                pobjRegistroSalvar = objEncabezado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    Public Overrides Sub Buscar()
        Try
            PrepararNuevaBusqueda()
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Limpia los datos de la entidad de busqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaOrdenes
            objCB.intID = Nothing
            objCB.intConsecutivo = Nothing
            objCB.intVersion = Nothing
            objCB.strProducto = Nothing
            objCB.intIDComitente = Nothing
            objCB.strNombre = Nothing
            objCB.strTipo = Nothing
            objCB.dtmOrden = Nothing
            cb = objCB

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    '    ''' <summary>
    '    ''' Consulta los valores por defecto para un nuevo encabezado
    '    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefecto()
        Try
            Dim objRespuesta As InvokeResult(Of CPX_DatosGiros)
            If RegistroEncabezado IsNot Nothing Then

                objRespuesta = Await mdcProxy.OrdenesDivisasDatosGiros_DefectoAsync(Program.Usuario)

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        mobjEncabezadoPorDefecto = objRespuesta.Value
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ConsultarInformacionEncabezado()
        Try
            If Not IsNothing(_RegistroEncabezado) AndAlso Not IsNothing(_RegistroEncabezado.intID) AndAlso _RegistroEncabezado.intID > 0 Then
                Await OrdenesCombos()
                'SV20181023_AJUSTESORDENES Se hace el el llamado para carga del combo de instrucciones según el tipo de operación de la órden
                Await OrdenesCombosEspecificos(_RegistroEncabezado.strProducto, "DATOSGIROS", _RegistroEncabezado.strTipo, Nothing, Nothing)
                Await ConsultarEncabezado("ID", _RegistroEncabezado.intID)
            Else
                Await OrdenesCombos()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarInformacionEncabezado_Proc", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Carga de combos específicos de ordenes
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesCombosEspecificos(ByVal pstrProducto As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosEspecificosAsync(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    If pstrCondicionTexto1 = "INSTRUCCIONES" Then
                        lstCodAgente = objRespuesta.Value
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' RABP20181024_SE ACIONA NUEVO COMBO DE CUENTAS para las instrucciones de pago transferencias de pago
    ''' Carga de combos específicos de ordenes
    ''' </summary>
    ''' <param name="pstrComitente"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesCombosEspecificosCuentasInstrucciones(ByVal pstrComitente As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosEspecificosAsync(pstrComitente, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    If pstrCondicionTexto1 = "CUENTASBANCARIAS" Then
                        lstCodPais = objRespuesta.Value
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombosEspecificosCuentasInstrucciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function


    Private Async Function ConsultarEncabezado(Optional ByVal pstrOpcion As String = "", Optional ByVal pintIDRegistro As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_DatosGiros)) = Nothing

            ErrorForma = String.Empty
            If RegistroEncabezado IsNot Nothing Then

                If pstrOpcion = "ID" Then
                    objRespuesta = Await mdcProxy.OrdenesDivisasDatosGiros_ConsultarAsync(pintIDRegistro, Program.Usuario)
                Else
                    objRespuesta = Await mdcProxy.OrdenesDivisasDatosGiros_ConsultarAsync(0, Program.Usuario)
                End If

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        ListaEncabezado = objRespuesta.Value
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If

                MyBase.CambioItem("ListaEncabezado")
                MyBase.CambioItem("ListaEncabezadoPaginada")

                logResultado = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Dim objEncabezadoEdicion As New CPX_DatosGiros

                objEncabezadoEdicion.intID = _EncabezadoSeleccionado.intID
                objEncabezadoEdicion.strCodAgenteOper = _EncabezadoSeleccionado.strCodAgenteOper
                objEncabezadoEdicion.intIdPaisGiro = _EncabezadoSeleccionado.intIdPaisGiro
                objEncabezadoEdicion.intIdMonedaGiro = _EncabezadoSeleccionado.intIdMonedaGiro
                objEncabezadoEdicion.intIDOrden = _EncabezadoSeleccionado.intIDOrden
                objEncabezadoEdicion.dblMontoMonedaGiro = _EncabezadoSeleccionado.dblMontoMonedaGiro
                objEncabezadoEdicion.intIDComitente = _EncabezadoSeleccionado.intIDComitente
                objEncabezadoEdicion.strNombreBeneficiario = _EncabezadoSeleccionado.strNombreBeneficiario
                objEncabezadoEdicion.strNumeroDocumentoBeneficiario = _EncabezadoSeleccionado.strNumeroDocumentoBeneficiario
                objEncabezadoEdicion.strTipoIdentificacionBeneficiario = _EncabezadoSeleccionado.strTipoIdentificacionBeneficiario
                objEncabezadoEdicion.strUsuario = _EncabezadoSeleccionado.strUsuario
                objEncabezadoEdicion.dtmActualizacion = _EncabezadoSeleccionado.dtmActualizacion

                EncabezadoEdicionSeleccionado = objEncabezadoEdicion
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
            Dim objRespuesta As InvokeResult(Of CPX_DatosGiros) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.OrdenesDivisasDatosGiros_ConsultarIDAsync(pintID, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    EncabezadoEdicionSeleccionado = objRespuesta.Value
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


#End Region

End Class



