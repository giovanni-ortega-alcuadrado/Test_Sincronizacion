'Manejo de multimonedas
'Ricardo Barrientos Pérez
'Julio 19 de 2019

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports A2ControlMenu

Public Class MultimonedaViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CPX_tblOrdenesDivisasMultimoneda
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    ''---------------------------------------------------------------------------------------------------------------------------------------------------
    '' Para manejar la forma principal 
    ''---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CPX_tblOrdenesDivisasMultimoneda
    Private mobjEncabezadoGuardadoAnterior As CPX_tblOrdenesDivisasMultimoneda
    Public viewListaPrincipal As MultimonedaView
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
    ''' Lista de Parametro que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblOrdenesDivisasMultimoneda)
    Public Property ListaEncabezado() As List(Of CPX_tblOrdenesDivisasMultimoneda)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesDivisasMultimoneda))
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
    Private _EncabezadoSeleccionado As CPX_tblOrdenesDivisasMultimoneda
    Public Property EncabezadoSeleccionado() As CPX_tblOrdenesDivisasMultimoneda
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesDivisasMultimoneda)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As CPX_tblOrdenesDivisasMultimoneda
    Public Property EncabezadoEdicionSeleccionado() As CPX_tblOrdenesDivisasMultimoneda
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesDivisasMultimoneda)
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

    Private _DatosAdicionalesOrden As clsDatosAdicionalesOrden
    Public Property DatosAdicionalesOrden() As clsDatosAdicionalesOrden
        Get
            Return _DatosAdicionalesOrden
        End Get
        Set(ByVal value As clsDatosAdicionalesOrden)
            _DatosAdicionalesOrden = value
            MyBase.CambioItem("DatosAdicionalesOrden")
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
            Dim objNvoEncabezado As New CPX_tblOrdenesDivisasMultimoneda
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


            Dim objListaEncabezado As New List(Of CPX_tblOrdenesDivisasMultimoneda)
            If Not IsNothing(_ListaEncabezado) Then
                For Each li In _ListaEncabezado
                    objListaEncabezado.Add(li)
                Next
            End If

            'OJO Como se deberia guardar aqui 
            If objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).Count > 0 Then
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDMonedaM = _EncabezadoEdicionSeleccionado.intIDMonedaM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDMonedaIntermediaM = _EncabezadoEdicionSeleccionado.intIDMonedaIntermediaM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblCantidadMultimoneda = _EncabezadoEdicionSeleccionado.dblCantidadMultimoneda
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblPrecioIntermedioM = _EncabezadoEdicionSeleccionado.dblPrecioIntermedioM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblSpreadComisionM = _EncabezadoEdicionSeleccionado.dblSpreadComisionM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblValorBrutoMultimoneda = _EncabezadoEdicionSeleccionado.dblValorBrutoMultimoneda
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblPrecioMonedaNegociadaM = _EncabezadoEdicionSeleccionado.dblPrecioMonedaNegociadaM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblComisionUSD = _EncabezadoEdicionSeleccionado.dblComisionUSD
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblCantidadM = _EncabezadoEdicionSeleccionado.dblCantidadM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblPrecioM = _EncabezadoEdicionSeleccionado.dblPrecioM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblValorBrutoM = _EncabezadoEdicionSeleccionado.dblValorBrutoM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblValorNetoM = _EncabezadoEdicionSeleccionado.dblValorNetoM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblComisionCOP = _EncabezadoEdicionSeleccionado.dblComisionCOP
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblValorRteFuenteM = _EncabezadoEdicionSeleccionado.dblValorRteFuenteM
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion
            Else
                Dim objNvoEncabezado As New CPX_tblOrdenesDivisasMultimoneda

                objNvoEncabezado.intID = _EncabezadoEdicionSeleccionado.intID
                objNvoEncabezado.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objNvoEncabezado.intIDMonedaM = _EncabezadoEdicionSeleccionado.intIDMonedaM
                objNvoEncabezado.intIDMonedaIntermediaM = _EncabezadoEdicionSeleccionado.intIDMonedaIntermediaM
                objNvoEncabezado.dblCantidadMultimoneda = _EncabezadoEdicionSeleccionado.dblCantidadMultimoneda
                objNvoEncabezado.dblPrecioIntermedioM = _EncabezadoEdicionSeleccionado.dblPrecioIntermedioM
                objNvoEncabezado.dblSpreadComisionM = _EncabezadoEdicionSeleccionado.dblSpreadComisionM
                objNvoEncabezado.dblValorBrutoMultimoneda = _EncabezadoEdicionSeleccionado.dblValorBrutoMultimoneda
                objNvoEncabezado.dblPrecioMonedaNegociadaM = _EncabezadoEdicionSeleccionado.dblPrecioMonedaNegociadaM
                objNvoEncabezado.dblComisionUSD = _EncabezadoEdicionSeleccionado.dblComisionUSD
                objNvoEncabezado.dblCantidadM = _EncabezadoEdicionSeleccionado.dblCantidadM
                objNvoEncabezado.dblPrecioM = _EncabezadoEdicionSeleccionado.dblPrecioM
                objNvoEncabezado.dblValorBrutoM = _EncabezadoEdicionSeleccionado.dblValorBrutoM
                objNvoEncabezado.dblValorNetoM = _EncabezadoEdicionSeleccionado.dblValorNetoM
                objNvoEncabezado.dblComisionCOP = _EncabezadoEdicionSeleccionado.dblComisionCOP
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
                    Dim objListaDetalle As New List(Of CPX_tblOrdenesDivisasMultimoneda)

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
                Dim objViewDetalle As New MultimonedaModalView(Me)
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

    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As CPX_tblOrdenesDivisasMultimoneda, ByRef pobjRegistroSalvar As CPX_tblOrdenesDivisasMultimoneda)
        Dim objEncabezado As CPX_tblOrdenesDivisasMultimoneda = New CPX_tblOrdenesDivisasMultimoneda
        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.intIDMonedaM = pobjRegistro.intIDMonedaM
                objEncabezado.intIDOrden = pobjRegistro.intIDOrden
                objEncabezado.intIDMonedaIntermediaM = pobjRegistro.intIDMonedaIntermediaM
                objEncabezado.dblCantidadMultimoneda = pobjRegistro.dblCantidadMultimoneda
                objEncabezado.dblPrecioIntermedioM = pobjRegistro.dblPrecioIntermedioM
                objEncabezado.dblSpreadComisionM = pobjRegistro.dblSpreadComisionM
                objEncabezado.dblValorBrutoMultimoneda = pobjRegistro.dblValorBrutoMultimoneda
                objEncabezado.dblPrecioMonedaNegociadaM = pobjRegistro.dblPrecioMonedaNegociadaM
                objEncabezado.dblComisionUSD = pobjRegistro.dblComisionUSD
                objEncabezado.dblCantidadM = pobjRegistro.dblCantidadM
                objEncabezado.dblPrecioM = pobjRegistro.dblPrecioM
                objEncabezado.dblValorBrutoM = pobjRegistro.dblValorBrutoM
                objEncabezado.dblValorNetoM = pobjRegistro.dblValorNetoM
                objEncabezado.dblComisionCOP = pobjRegistro.dblComisionCOP
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
            Dim objRespuesta As InvokeResult(Of CPX_tblOrdenesDivisasMultimoneda)
            If RegistroEncabezado IsNot Nothing Then

                objRespuesta = Await mdcProxy.OrdenesDivisasMultimoneda_DefectoAsync(Program.Usuario)

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


            Else
                Await OrdenesCombos()


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarInformacionEncabezado", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarEncabezado(Optional ByVal pstrOpcion As String = "", Optional ByVal pintIDRegistro As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenesDivisasMultimoneda)) = Nothing

            ErrorForma = String.Empty
            If RegistroEncabezado IsNot Nothing Then

                If pstrOpcion = "ID" Then
                    objRespuesta = Await mdcProxy.OrdenesDivisasMultimonedas_ConsultarAsync(pintIDRegistro, Program.Usuario)
                Else
                    objRespuesta = Await mdcProxy.OrdenesDivisasMultimonedas_ConsultarAsync(0, Program.Usuario)
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
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenesDivisasMultimoneda)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.OrdenesDivisasMultimonedas_ConsultarAsync(pintID, Program.Usuario)

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
