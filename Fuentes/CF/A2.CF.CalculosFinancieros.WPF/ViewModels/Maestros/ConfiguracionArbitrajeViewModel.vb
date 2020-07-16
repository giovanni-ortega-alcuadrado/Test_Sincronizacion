Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo
Imports A2Utilidades.Mensajes
Imports A2ComunesImportaciones
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel

Public Class ConfiguracionArbitrajeViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As ConfiguracionArbitraje
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private objProxyCarga As ImportacionesDomainContext
    Private mobjVM As ConfiguracionArbitrajeViewModel
    Dim cwConfiguracionArbitrajeView As cwConfiguracionArbitrajeView

    Dim logGeneroDatosCorrectamente As Boolean
    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As ConfiguracionArbitraje
    Private mobjDetallePorDefecto As ConfiguracionArbitrajeDetalle
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
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    objProxyCarga = New ImportacionesDomainContext()
                Else
                    objProxyCarga = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
                End If

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de ConfiguracionArbitraje que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of ConfiguracionArbitraje)
    Public Property ListaEncabezado() As EntitySet(Of ConfiguracionArbitraje)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of ConfiguracionArbitraje))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ConfiguracionArbitraje para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de ConfiguracionArbitraje que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As ConfiguracionArbitraje
    Public Property EncabezadoSeleccionado() As ConfiguracionArbitraje
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As ConfiguracionArbitraje)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDConfiguracionArbitraje > 0 Then
                    ConsultarDetalle(_EncabezadoSeleccionado.intIDConfiguracionArbitraje)
                Else
                    logIncializarDet = True
                End If
            End If

            If logIncializarDet Then
                ListaDetalle = Nothing
            End If

            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged

        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                If Not String.IsNullOrEmpty(EncabezadoSeleccionado.strIDEspecie) And Not String.IsNullOrEmpty(EncabezadoSeleccionado.strTipo) And _
                    Not IsNothing(EncabezadoSeleccionado.dtmFechaVigencia) And Editando Then
                    HabilitarEdicionDetalle = True
                    HabilitarBorrarDetalle = True
                Else
                    HabilitarEdicionDetalle = False
                    HabilitarBorrarDetalle = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los botones del detalle.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of ConfiguracionArbitrajeDetalle)
    Public Property ListaDetalle() As List(Of ConfiguracionArbitrajeDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of ConfiguracionArbitrajeDetalle))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    ''' <summary>
    ''' Pagina la lista detalles. Se presenta en el grid del detalle 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                Dim view = New PagedCollectionView(_ListaDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As ConfiguracionArbitrajeDetalle
    Public Property DetalleSeleccionado() As ConfiguracionArbitrajeDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ConfiguracionArbitrajeDetalle)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaConfiguracionArbitraje
    Public Property cb() As CamposBusquedaConfiguracionArbitraje
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaConfiguracionArbitraje)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            MyBase.CambioItem("HabilitarEncabezado")
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

    Private _HabilitarBorrarDetalle As Boolean = False
    Public Property HabilitarBorrarDetalle() As Boolean
        Get
            Return _HabilitarBorrarDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBorrarDetalle = value
            MyBase.CambioItem("HabilitarBorrarDetalle")
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

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub ImportarArchivo()
        Try
            If String.IsNullOrEmpty(_NombreArchivo) Then
                mostrarMensaje("Debe de seleccionar un archivo para la importación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            IsBusy = True
            If Not IsNothing(objProxyCarga.RespuestaArchivoImportacions) Then
                objProxyCarga.RespuestaArchivoImportacions.Clear()
            End If

            objProxyCarga.Load(objProxyCarga.ConfiguracionArbitraje_ImportarQuery("ConfiguracionArbitraje", _NombreArchivo, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Async Function TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion)) As Task
        Try
            Dim objRet As LoadOperation(Of ConfiguracionArbitrajeDetalle)

            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim logContinuar As Boolean = False

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    Dim logContinuarConsultando As Boolean = False
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count = 0 Then
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                        logContinuarConsultando = True
                    Else
                        Dim objTipo As String = String.Empty

                        If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                            objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")
                        End If

                        For Each li In objListaRespuesta.OrderBy(Function(o) o.Tipo)
                            If objTipo <> li.Tipo And li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                                objTipo = li.Tipo
                                objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
                            ElseIf li.Tipo = "REGISTROSIMPORTADOS" Then
                                If li.Columna > 0 Then
                                    logContinuar = True
                                End If
                            End If

                            If li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Campo {1} - Validación: {2}", li.Fila, li.Campo, li.Mensaje))
                            Else
                                objListaMensajes.Add(li.Mensaje)
                            End If
                        Next
                    End If

                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"

                    If logContinuarConsultando Then

                        mdcProxy.ConfiguracionArbitrajeDetalles.Clear()
                        objRet = Await mdcProxy.Load(mdcProxy.ConfiguracionArbitrajeMasiva_ConsultarQuery(Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

                        If Not objRet Is Nothing Then
                            If objRet.HasError Then
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If
                        End If

                        ListaDetalle = mdcProxy.ConfiguracionArbitrajeDetalles.ToList
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Function

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New ConfiguracionArbitraje

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of ConfiguracionArbitraje)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDConfiguracionArbitraje = -1
                objNvoEncabezado.strIDEspecie = String.Empty
                objNvoEncabezado.strTipo = String.Empty
                objNvoEncabezado.dtmFechaVigencia = Nothing
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.strIDEspecie) Or Not IsNothing(cb.strTipo) Or Not IsNothing(cb.dtmFechaVigencia) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.strIDEspecie, cb.strTipo, cb.dtmFechaVigencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            ErrorForma = String.Empty

            If ValidarRegistro() Then

                If ValidarDetalle() Then

                    xmlCompleto = "<ConfiguracionArbitraje>"

                    For Each objeto In (From c In ListaDetalle)

                        xmlDetalle = "<Detalle intIDConfiguracionArbitrajeDetalle=""" & objeto.intIDConfiguracionArbitrajeDetalle &
                                    """ intIDConfiguracionArbitraje=""" & objeto.intIDConfiguracionArbitraje & """ strDescripcionTipo=""" & objeto.strDescripcionTipo &
                                    """ strIDEspecie=""" & objeto.strIDEspecie & """ strISIN=""" & objeto.strISIN & """ dblValor=""" & objeto.dblValor &
                                    """ lngID=""" & objeto.lngIdMoneda & """></Detalle>"

                        xmlCompleto = xmlCompleto & xmlDetalle
                    Next

                    xmlCompleto = xmlCompleto & "</ConfiguracionArbitraje>"

                    IsBusy = True

                    Dim strMsg As String = ""
                    Dim objRet As InvokeOperation(Of String)

                    objRet = Await mdcProxy.ActualizarConfiguracionArbitrajeSync(EncabezadoSeleccionado.intIDConfiguracionArbitraje, EncabezadoSeleccionado.strIDEspecie, _
                                                                                 EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.dtmFechaRegistro, EncabezadoSeleccionado.dtmFechaVigencia, _
                                                                                 CDbl(EncabezadoSeleccionado.dblUnidades), Convert.ToInt32(EncabezadoSeleccionado.intIDEstadosConceptoTitulos), _
                                                                                 Convert.ToInt32(EncabezadoSeleccionado.intIDEstadosConceptoTitulosD), _
                                                                                 xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                        strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                        If Not String.IsNullOrEmpty(strMsg) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarEdicionDetalle = False
                        HabilitarBorrarDetalle = False
                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        Await ConsultarEncabezado(True, String.Empty)
                    End If
                Else
                    HabilitarEncabezado = True
                    HabilitarEdicionDetalle = True
                    HabilitarBorrarDetalle = True
                End If

            Else
                HabilitarEncabezado = True
                HabilitarEdicionDetalle = True
                HabilitarBorrarDetalle = True
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarEncabezado = True
            HabilitarEdicionDetalle = True
            HabilitarBorrarDetalle = True

            IsBusy = False
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el registro seleccionado. ¿Confirma el borrado de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.ConfiguracionArbitrajes.Where(Function(i) i.intIDConfiguracionArbitraje = EncabezadoSeleccionado.intIDConfiguracionArbitraje).Count > 0 Then
                        mdcProxy.ConfiguracionArbitrajes.Remove(mdcProxy.ConfiguracionArbitrajes.Where(Function(i) i.intIDConfiguracionArbitraje = EncabezadoSeleccionado.intIDConfiguracionArbitraje).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)

                    'Await ConsultarEncabezado(True, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ActualizarDetalle()
        Try
            If Not IsNothing(DetalleSeleccionado) Then
                cwConfiguracionArbitrajeView = New cwConfiguracionArbitrajeView(Me, DetalleSeleccionado, mobjDetallePorDefecto, HabilitarEncabezado)
                Program.Modal_OwnerMainWindowsPrincipal(cwConfiguracionArbitrajeView)
                cwConfiguracionArbitrajeView.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible editar este detalle.", "ActualizarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la configuración arbitraje.", Me.ToString(), "ActualizarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' </summary>
    Public Sub IngresarDetalle()
        Try
            If Not IsNothing(ListaDetalle) Then
                If ListaDetalle.Count > 0 And EncabezadoSeleccionado.strTipo = "A" Then 'A = ADR
                    A2Utilidades.Mensajes.mostrarMensaje("No es permitido agregar otro detalle cuando el tipo es ARBITRAJE.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            cwConfiguracionArbitrajeView = New cwConfiguracionArbitrajeView(Me, Nothing, mobjDetallePorDefecto, HabilitarEdicionDetalle)
            Program.Modal_OwnerMainWindowsPrincipal(cwConfiguracionArbitrajeView)
            cwConfiguracionArbitrajeView.ShowDialog()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    Public Sub BorrarDetalle()
        Try

            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intIDConfiguracionArbitrajeDetalle = _DetalleSeleccionado.intIDConfiguracionArbitrajeDetalle).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDConfiguracionArbitrajeDetalle = _DetalleSeleccionado.intIDConfiguracionArbitrajeDetalle).First)
                End If

                Dim objNuevaListaDetalle As New List(Of ConfiguracionArbitrajeDetalle)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDConfiguracionArbitrajeDetalle = _DetalleSeleccionado.intIDConfiguracionArbitrajeDetalle).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDConfiguracionArbitrajeDetalle = _DetalleSeleccionado.intIDConfiguracionArbitrajeDetalle).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible borrar este detalle o no existe.", "BorrarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de configuración arbitraje.", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaConfiguracionArbitraje
            objCB.strIDEspecie = String.Empty
            objCB.strTipo = String.Empty
            objCB.dtmFechaVigencia = Date.Now
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    Private Function ObtenerRegistroAnterior() As ConfiguracionArbitraje
        Dim objEncabezado As ConfiguracionArbitraje = New ConfiguracionArbitraje

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of ConfiguracionArbitraje)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDConfiguracionArbitraje = _EncabezadoSeleccionado.intIDConfiguracionArbitraje
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida el instrumento
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strIDEspecie) Then
                    strMsg = String.Format("{0}{1} + El instrumento es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipo) Then
                    strMsg = String.Format("{0}{1} + El tipo es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha inicial
                If IsNothing(_EncabezadoSeleccionado.dtmFechaVigencia) Then
                    strMsg = String.Format("{0}{1} + La fecha vigencia es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha de registro
                If IsNothing(_EncabezadoSeleccionado.dtmFechaRegistro) Then
                    strMsg = String.Format("{0}{1} + La fecha de registro es un campo requerido.", strMsg, vbCrLf)
                End If

                If IsNothing(_EncabezadoSeleccionado.dblUnidades) Or _EncabezadoSeleccionado.dblUnidades = 0 Then
                    strMsg = String.Format("{0}{1} + El campo número de unidades es un campo requerido.", strMsg, vbCrLf)
                End If

                'JAEZ 20161117 
                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipo) Then

                    If _EncabezadoSeleccionado.strTipo = "E" Then

                        If IsNothing(_EncabezadoSeleccionado.intIDEstadosConceptoTitulos) Then
                            strMsg = String.Format("{0}{1} + El concepto título construir es un campo requerido", strMsg, vbCrLf)
                        End If

                        If IsNothing(_EncabezadoSeleccionado.intIDEstadosConceptoTitulosD) Then
                            strMsg = String.Format("{0}{1} + El concepto título destruir es un campo requerido", strMsg, vbCrLf)
                        End If
                    End If

                End If

                'JAEZ 20161117
                If Not IsNothing(ListaDetalle) Then
                    If ListaDetalle.Count = 0 Then
                        strMsg = String.Format("{0}{1} + Debe ingresar un detalle.", strMsg, vbCrLf)
                    End If
                End If

                'JAEZ 20161117
                If Not IsNothing(ListaDetalle) Then
                    If ListaDetalle.Count > 0 Then
                        If (ListaDetalle.Where(Function(i) i.strDescripcionTipo = "DINERO")).Count > 1 Then
                            strMsg = String.Format("{0}{1} + No puede existir más de un detalle con el tipo DINERO.", strMsg, vbCrLf)
                        End If
                    End If
                End If


            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    Public Overrides Async Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty

        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If Not So.Error Is Nothing Then
                        strMsg = So.Error.Message
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.obtenerMensajeValidacion(strMsg, So.UserState.ToString, True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                ' Marcar los cambios como rechazados
                mdcProxy.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of ConfiguracionArbitraje)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarConfiguracionArbitrajePorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de ConfiguracionArbitraje
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrIDEspecie As String = "",
                                               Optional ByVal pstrTipo As String = "",
                                               Optional ByVal pdtmFechaVigencia As System.Nullable(Of System.DateTime) = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ConfiguracionArbitraje)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.ConfiguracionArbitrajes.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarConfiguracionArbitrajeSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionArbitrajeSyncQuery(pstrIDEspecie:=pstrIDEspecie,
                                                                                               pstrTipo:=pstrTipo,
                                                                                               pdtmFechaVigencia:=pdtmFechaVigencia,
                                                                                               pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxy.ConfiguracionArbitrajes

                    If objRet.Entities.Count > 0 Then
                        If Not plogFiltrar Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de ConfiguracionArbitraje ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetallePorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of ConfiguracionArbitrajeDetalle)

            objRet = Await dcProxy.Load(dcProxy.ConsultarConfiguracionArbitrajeDetallePorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "ConsultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjDetallePorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjDetallePorDefecto = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "consultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ConfiguracionArbitrajeDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.ConfiguracionArbitrajeDetalles Is Nothing Then
                mdcProxy.ConfiguracionArbitrajeDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionArbitrajeDetalleSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la operación interbancaria pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la operación interbancaria.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la operación interbancaria seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If IsNothing(_ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            ElseIf _ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            End If

            If Not IsNothing(ListaDetalle) Then
                If ListaDetalle.Count > 1 And EncabezadoSeleccionado.strTipo = "A" Then 'A = ADR
                    strMsg = String.Format("{0}{1} + No es permitido grabar más de un detalle cuando el tipo es ARBITRAJE.", strMsg, vbCrLf)
                End If
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "validarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
Public Class CamposBusquedaConfiguracionArbitraje
    Implements INotifyPropertyChanged

    Private _strIDEspecie As String
    Public Property strIDEspecie() As String
        Get
            Return _strIDEspecie
        End Get
        Set(ByVal value As String)
            _strIDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
        End Set
    End Property

    Private _strTipo As String
    Public Property strTipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipo"))
        End Set
    End Property

    Private _dtmFechaVigencia As System.Nullable(Of System.DateTime)
    Public Property dtmFechaVigencia() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaVigencia
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaVigencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaVigencia"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class




