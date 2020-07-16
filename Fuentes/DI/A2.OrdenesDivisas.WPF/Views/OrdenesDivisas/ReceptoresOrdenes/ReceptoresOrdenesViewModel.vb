' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel

Public Class ReceptoresOrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CPX_tblOrdenesReceptores
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CPX_tblOrdenesReceptores
    Private mobjEncabezadoGuardadoAnterior As CPX_tblOrdenesReceptores
    Public viewListaPrincipal As ReceptoresOrdenesView
    Private logVentanaDetalleActiva As Boolean = False
    Private strNombreProducto As String = String.Empty

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                'Await ConsultarNumerales()
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
            'ConsultarReceptoresInterbancarias()

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

    ''' <summary>
    ''' RABP20190614_Receptores de tipo formulario 5 Interbancaria
    ''' Lista de combo de receptores cuando las ordenes son interbancarias
    ''' </summary>
    Private _lstReceptoresInterbancarias As ObservableCollection(Of CPX_ComboOrdenes)
    Public Property lstReceptoresInterbancarias() As ObservableCollection(Of CPX_ComboOrdenes)
        Get
            Return _lstReceptoresInterbancarias
        End Get
        Set(ByVal value As ObservableCollection(Of CPX_ComboOrdenes))
            _lstReceptoresInterbancarias = value
            CambioItem("lstReceptoresInterbancarias")
        End Set
    End Property

    Private _dicCombosReceptoresInterbancarios As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosReceptoresInterbancarios() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosReceptoresInterbancarios
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosReceptoresInterbancarios = value
            CambioItem("dicCombosReceptoresInterbancarios")
        End Set
    End Property

    ''' <summary>
    ''' Lista de Parametro que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblOrdenesReceptores)
    Public Property ListaEncabezado() As List(Of CPX_tblOrdenesReceptores)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesReceptores))
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
    Private _EncabezadoSeleccionado As CPX_tblOrdenesReceptores
    Public Property EncabezadoSeleccionado() As CPX_tblOrdenesReceptores
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesReceptores)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As CPX_tblOrdenesReceptores
    Public Property EncabezadoEdicionSeleccionado() As CPX_tblOrdenesReceptores
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesReceptores)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private _RegistroEncabezado As tblOrdenes
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


    'RABP20190614
    Private _DatosAdicionalesOrden As clsDatosAdicionalesOrden
    Public Property DatosAdicionalesOrden() As clsDatosAdicionalesOrden
        Get
            Return _DatosAdicionalesOrden
        End Get
        Set(ByVal value As clsDatosAdicionalesOrden)
            _DatosAdicionalesOrden = value
            If DatosAdicionalesOrden.strReferencia = "IN" Then
                ConsultarReceptoresInterbancarias()
            End If
            MyBase.CambioItem("DatosAdicionalesOrden")
        End Set
    End Property

    Private _HabilitarCombosReceptores As Boolean = True
    Public Property HabilitarCombosReceptores() As Boolean
        Get
            Return _HabilitarCombosReceptores
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCombosReceptores = value
            MyBase.CambioItem("HabilitarCombosReceptores")
        End Set
    End Property

    Private _HabilitarCombosReceptoresSETFX As Boolean = True
    Public Property HabilitarCombosReceptoresSETFX() As Boolean
        Get
            Return _HabilitarCombosReceptoresSETFX
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCombosReceptoresSETFX = value
            MyBase.CambioItem("HabilitarCombosReceptoresSETFX")
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
            Dim objNvoEncabezado As New CPX_tblOrdenesReceptores
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

            If DatosAdicionalesOrden.strReferencia = "IN" Then
                HabilitarCombosReceptoresSETFX = True
                HabilitarCombosReceptores = False
                ConsultarReceptoresInterbancarias()
            Else
                HabilitarCombosReceptoresSETFX = False
                HabilitarCombosReceptores = True
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


            Dim objListaEncabezado As New List(Of CPX_tblOrdenesReceptores)
            If Not IsNothing(_ListaEncabezado) Then
                For Each li In _ListaEncabezado
                    objListaEncabezado.Add(li)
                Next
            End If

            'OJO Como se deberia guardar aqui 
            If objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).Count > 0 Then
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDReceptor = _EncabezadoEdicionSeleccionado.intIDReceptor
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.logLider = _EncabezadoEdicionSeleccionado.logLider
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblPorcentaje = _EncabezadoEdicionSeleccionado.dblPorcentaje
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion
            Else
                Dim objNvoEncabezado As New CPX_tblOrdenesReceptores

                objNvoEncabezado.intID = _EncabezadoEdicionSeleccionado.intID
                objNvoEncabezado.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objNvoEncabezado.intIDReceptor = _EncabezadoEdicionSeleccionado.intIDReceptor
                objNvoEncabezado.logLider = _EncabezadoEdicionSeleccionado.logLider
                objNvoEncabezado.dblPorcentaje = _EncabezadoEdicionSeleccionado.dblPorcentaje
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
                    Dim objListaDetalle As New List(Of CPX_tblOrdenesReceptores)

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
                Dim objViewDetalle As New ReceptoresOrdenesModalView(Me)
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

    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As CPX_tblOrdenesReceptores, ByRef pobjRegistroSalvar As CPX_tblOrdenesReceptores)
        Dim objEncabezado As CPX_tblOrdenesReceptores = New CPX_tblOrdenesReceptores
        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.intIDReceptor = pobjRegistro.intIDReceptor
                objEncabezado.intIDOrden = pobjRegistro.intIDOrden
                objEncabezado.logLider = pobjRegistro.logLider
                objEncabezado.dblPorcentaje = pobjRegistro.dblPorcentaje
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
    '    ''' <summary>
    '    ''' Consulta los valores por defecto para un nuevo encabezado
    '    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefecto()
        Try
            Dim objRespuesta As InvokeResult(Of CPX_tblOrdenesReceptores)
            If RegistroEncabezado IsNot Nothing Then

                objRespuesta = Await mdcProxy.OrdenesReceptores_DefectoAsync(Program.Usuario)

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

                Await ConsultarEncabezado("ID", RegistroEncabezado.intID)
                ConsultarReceptoresInterbancarias()


            Else
                Await OrdenesCombos()


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarInformacionEncabezado", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ConsultarReceptoresInterbancarias()
        Try
            Await OrdenesCombosEspecificosInterbancaria(_RegistroEncabezado.strProducto, "INTERBANCARIAS", "IN", Nothing, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarReceptoresInterbancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' RABP20190614_Nuevo combo para consultar los receptores cuando las ordenes son interbacarias Formulario 5
    ''' Carga de combos específicos de ordenes
    ''' </summary>
    ''' <param name="pstrComitente"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesCombosEspecificosInterbancaria(ByVal pstrComitente As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuestaReceptores As InvokeResult(Of List(Of CPX_ComboOrdenes))

            'If DatosAdicionalesOrden.strReferencia = "IN" Then
            objRespuestaReceptores = Await mdcProxy.OrdenesCombosEspecificosAsync(pstrComitente, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            dicCombosReceptoresInterbancarios = clsGenerales.CargarListas(objRespuestaReceptores.Value)

            If Not IsNothing(objRespuestaReceptores) Then
                If Not IsNothing(objRespuestaReceptores.Value) Then

                    Dim objListaComboOrdenes As New ObservableCollection(Of CPX_ComboOrdenes)
                    If pstrCondicionTexto1 = "INTERBANCARIAS" Then
                        For Each item In objRespuestaReceptores.Value
                            objListaComboOrdenes.Add(item)
                        Next

                        lstReceptoresInterbancarias = objListaComboOrdenes
                    End If


                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            'End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombosEspecificosCuentasInstrucciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Private Async Function ConsultarEncabezado(Optional ByVal pstrOpcion As String = "", Optional ByVal pintIDRegistro As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenesReceptores)) = Nothing

            ErrorForma = String.Empty
            If RegistroEncabezado IsNot Nothing Then

                If pstrOpcion = "ID" Then
                    objRespuesta = Await mdcProxy.OrdenesReceptores_ConsultarAsync(pintIDRegistro, Program.Usuario)
                Else
                    objRespuesta = Await mdcProxy.OrdenesReceptores_ConsultarAsync(0, Program.Usuario)
                End If
                ConsultarReceptoresInterbancarias()

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
                EncabezadoEdicionSeleccionado = _EncabezadoSeleccionado
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
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenesReceptores)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.OrdenesReceptores_ConsultarAsync(pintID, Program.Usuario)
            ConsultarReceptoresInterbancarias()

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value
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
