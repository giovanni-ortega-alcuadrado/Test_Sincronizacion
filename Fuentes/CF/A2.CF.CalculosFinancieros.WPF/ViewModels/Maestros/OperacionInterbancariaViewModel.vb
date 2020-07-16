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

Public Class OperacionInterbancariaViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As OperacionInterbancaria
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As OperacionInterbancariaViewModel
    Dim cwOperacionInterbancariaView As cwOperacionInterbancariaView

    Dim logGeneroDatosCorrectamente As Boolean
    Dim NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO As String = "Código"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As OperacionInterbancaria
    Private mobjDetallePorDefecto As OperacionInterbancariaDetalle
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
    ''' <history>
    ''' ID caso de prueba: CP0001
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()
                If String.IsNullOrEmpty(NOMBRE_ETIQUETA_COMITENTE) Then
                    NOMBRE_ETIQUETA_COMITENTE = NOMBRE_ETIQUETA_COMITENTE_X_DEFECTO
                End If

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
    ''' Lista de OperacionInterbancaria que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of OperacionInterbancaria)
    Public Property ListaEncabezado() As EntitySet(Of OperacionInterbancaria)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of OperacionInterbancaria))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de OperacionInterbancaria para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de OperacionInterbancaria que se encuentra seleccionado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0002
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
    Private WithEvents _EncabezadoSeleccionado As OperacionInterbancaria
    Public Property EncabezadoSeleccionado() As OperacionInterbancaria
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As OperacionInterbancaria)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDOperacionInterbancaria > 0 Then
                    ConsultarDetalle(_EncabezadoSeleccionado.intIDOperacionInterbancaria)
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

    Private Async Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        If e.PropertyName = "strTipoTasaFija" Then
            If EncabezadoSeleccionado.strTipoTasaFija = "F" Then
                EncabezadoSeleccionado.strIndicadorEconomico = "0"
                HabilitarIndicador = False
            Else
                EncabezadoSeleccionado.strIndicadorEconomico = String.Empty
                HabilitarIndicador = True
            End If
        End If

        If e.PropertyName = "lngIDComitente" Then
            Await ConsultarDatosPortafolio()
        End If

        If e.PropertyName = "dblValorInicial" Then
            EncabezadoSeleccionado.dblSaldo = EncabezadoSeleccionado.dblValorInicial
        End If
    End Sub

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of OperacionInterbancariaDetalle)
    Public Property ListaDetalle() As List(Of OperacionInterbancariaDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of OperacionInterbancariaDetalle))
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
    Private WithEvents _DetalleSeleccionado As OperacionInterbancariaDetalle
    Public Property DetalleSeleccionado() As OperacionInterbancariaDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As OperacionInterbancariaDetalle)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaOperacionInterbancaria
    Public Property cb() As CamposBusquedaOperacionInterbancaria
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaOperacionInterbancaria)
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

    Private _HabilitarIndicador As Boolean = False
    Public Property HabilitarIndicador() As Boolean
        Get
            Return _HabilitarIndicador
        End Get
        Set(ByVal value As Boolean)
            _HabilitarIndicador = value
            MyBase.CambioItem("HabilitarIndicador")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _NOMBRE_ETIQUETA_COMITENTE As String = Program.STR_NOMBRE_ETIQUETA_COMITENTE
    Public Property NOMBRE_ETIQUETA_COMITENTE() As String
        Get
            Return _NOMBRE_ETIQUETA_COMITENTE
        End Get
        Set(ByVal value As String)
            _NOMBRE_ETIQUETA_COMITENTE = value
            MyBase.CambioItem("NOMBRE_ETIQUETA_COMITENTE")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New OperacionInterbancaria

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of OperacionInterbancaria)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDOperacionInterbancaria = -1
                objNvoEncabezado.strIDOperacion = String.Empty
                objNvoEncabezado.lngIDComitente = String.Empty
                objNvoEncabezado.strNombreComitente = String.Empty
                objNvoEncabezado.strDescripcionBanco = String.Empty
                objNvoEncabezado.lngIDBanco = -1
                objNvoEncabezado.dtmFechaInicial = Nothing
                objNvoEncabezado.dblValorInicial = 0
                objNvoEncabezado.strTipoTasaFija = String.Empty
                objNvoEncabezado.strBase = String.Empty
                objNvoEncabezado.strIndicadorEconomico = String.Empty
                objNvoEncabezado.dblTasaPuntos = 0
                objNvoEncabezado.strModalidad = String.Empty
                objNvoEncabezado.strMoneda = String.Empty
                objNvoEncabezado.strPosicion = String.Empty
                objNvoEncabezado.strTipo = String.Empty
                objNvoEncabezado.dtmFechaFinal = Nothing
                objNvoEncabezado.dblSaldo = 0
                objNvoEncabezado.strNotas = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            'Campos seleccionados por default
            EncabezadoSeleccionado.strPosicion = "A"
            EncabezadoSeleccionado.strTipo = "B"
            EncabezadoSeleccionado.strBase = "1"

            HabilitarEncabezado = True
            HabilitarIndicador = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0003
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
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
    ''' <history>
    ''' ID caso de prueba: CP0004
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
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
            If Not IsNothing(cb.strIDOperacion) Or Not IsNothing(cb.lngIDComitente) Or Not IsNothing(cb.lngIDBanco) Or Not IsNothing(cb.dtmFechaInicial) Or
                Not IsNothing(cb.dtmFechaFinal) Or Not IsNothing(cb.strTipoTasaFija) Or Not IsNothing(cb.strTasaReferencia) Or Not IsNothing(cb.strBase) Or
                Not IsNothing(cb.strIndicadorEconomico) Or Not IsNothing(cb.strModalidad) Or Not IsNothing(cb.strMoneda) Or Not IsNothing(cb.strPosicion) Or
                Not IsNothing(cb.strTipo) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.strIDOperacion, cb.lngIDComitente, CInt(cb.lngIDBanco), cb.dtmFechaInicial, cb.strTipoTasaFija, cb.strBase, cb.strIndicadorEconomico, cb.strModalidad, cb.strMoneda, cb.strPosicion, cb.strTipo, cb.dtmFechaFinal)
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

                    'Realizamos el llamado al método "Generar" en el caso de que modifiquen cualquier campo del encabezado 
                    'sin que se haya presionado previamente el botón "Generar". Es decir, que recalcule antes de guardar.
                    Await Generar()
                    If Not logGeneroDatosCorrectamente Then
                        Exit Sub
                    End If

                    xmlCompleto = "<OperacionInterbancaria>"

                    For Each objeto In (From c In ListaDetalle)

                        xmlDetalle = "<Detalle intIDPago=""" & objeto.intIDPago &
                                    """ dtmFechaPago=""" & Format(objeto.dtmFechaPago, "yyyy/MM/dd") & """ dblTasaPago=""" & objeto.dblTasaPago &
                                    """ intDias=""" & objeto.intDias & """ dblValorPago=""" & objeto.dblValorPago & """ dblValorPagoAdicional=""" & objeto.dblValorPagoAdicional &
                                    """ dtmCalculo=""" & Format(objeto.dtmCalculo, "yyyy/MM/dd") & """ intDiasEntreFlujos=""" & objeto.intDiasEntreFlujos &
                                    """ logPagado=""" & objeto.logPagado & """ dblSaldo=""" & objeto.dblSaldo & """></Detalle>"

                        xmlCompleto = xmlCompleto & xmlDetalle
                    Next

                    xmlCompleto = xmlCompleto & "</OperacionInterbancaria>"

                    IsBusy = True

                    Dim strMsg As String = ""
                    Dim objRet As InvokeOperation(Of String)

                    objRet = Await mdcProxy.ActualizarOperacionInterbancariaSync(EncabezadoSeleccionado.intIDOperacionInterbancaria, EncabezadoSeleccionado.strIDOperacion, EncabezadoSeleccionado.lngIDComitente, CInt(EncabezadoSeleccionado.lngIDBanco), _
                                                             EncabezadoSeleccionado.dtmFechaInicial, EncabezadoSeleccionado.dblValorInicial, EncabezadoSeleccionado.strTipoTasaFija, _
                                                            EncabezadoSeleccionado.strBase, EncabezadoSeleccionado.strIndicadorEconomico, EncabezadoSeleccionado.dblTasaPuntos, EncabezadoSeleccionado.strModalidad, _
                                                            EncabezadoSeleccionado.strMoneda, EncabezadoSeleccionado.strPosicion, EncabezadoSeleccionado.strTipo, _EncabezadoSeleccionado.dtmFechaFinal,
                                                            EncabezadoSeleccionado.dblSaldo, EncabezadoSeleccionado.strNotas, xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                        strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                        If Not String.IsNullOrEmpty(strMsg) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarIndicador = False
                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        Await ConsultarEncabezado(True, String.Empty)
                    End If

                End If

            Else
                HabilitarEncabezado = True
                If EncabezadoSeleccionado.strTipoTasaFija = "F" Then
                    HabilitarIndicador = False
                Else
                    HabilitarIndicador = True
                End If
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
    ''' <history>
    ''' ID caso de prueba: CP0006
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
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
            If EncabezadoSeleccionado.strTipoTasaFija = "F" Then
                HabilitarIndicador = False
            Else
                HabilitarIndicador = True
            End If

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
                HabilitarIndicador = False
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
    ''' <history>
    ''' ID caso de prueba: CP0008
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra la operación interbancaria seleccionada. ¿Confirma el borrado de este operación interbancaria?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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

                    If mdcProxy.OperacionInterbancarias.Where(Function(i) i.intIDOperacionInterbancaria = EncabezadoSeleccionado.intIDOperacionInterbancaria).Count > 0 Then
                        mdcProxy.OperacionInterbancarias.Remove(mdcProxy.OperacionInterbancarias.Where(Function(i) i.intIDOperacionInterbancaria = EncabezadoSeleccionado.intIDOperacionInterbancaria).First)
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

    Public Async Function Generar() As Task
        Try
            Dim objOperacionInterbancariaDetalle As LoadOperation(Of OperacionInterbancariaDetalle)
            Dim strDetalleGridIDPago As String = String.Empty
            Dim strDetalleGridValorPagoAdicional As String = String.Empty
            Dim strDetalleGridPagado As String = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.OperacionInterbancariaDetalles.Clear()

            If ValidarRegistro() Then

                IsBusy = True

                If IsNothing(DetalleSeleccionado) Then
                    objOperacionInterbancariaDetalle = Await mdcProxy.Load(mdcProxy.CalculoPagosInterbancarioDetalle_GenerarSyncQuery(EncabezadoSeleccionado.dtmFechaInicial, EncabezadoSeleccionado.dblValorInicial, EncabezadoSeleccionado.strTipoTasaFija, EncabezadoSeleccionado.strBase, EncabezadoSeleccionado.strIndicadorEconomico, EncabezadoSeleccionado.dblTasaPuntos, EncabezadoSeleccionado.strModalidad, EncabezadoSeleccionado.strMoneda, EncabezadoSeleccionado.strPosicion, EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.dtmFechaFinal, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion)).AsTask
                Else
                    For Each li In ListaDetalle
                        If li.dblValorPagoAdicional <> 0 Or li.logPagado Then
                            strDetalleGridIDPago = strDetalleGridIDPago + CType(li.intIDPago, String) + "|"

                            strDetalleGridValorPagoAdicional = strDetalleGridValorPagoAdicional + CType(li.dblValorPagoAdicional, String) + "|"

                            strDetalleGridPagado = strDetalleGridPagado + CType(IIf(li.logPagado = True, 1, 0), String) + "|"
                        End If
                    Next
                    objOperacionInterbancariaDetalle = Await mdcProxy.Load(mdcProxy.CalculoPagosInterbancarioDetalle_GenerarSyncQuery(EncabezadoSeleccionado.dtmFechaInicial, EncabezadoSeleccionado.dblValorInicial, EncabezadoSeleccionado.strTipoTasaFija, EncabezadoSeleccionado.strBase, EncabezadoSeleccionado.strIndicadorEconomico, EncabezadoSeleccionado.dblTasaPuntos, EncabezadoSeleccionado.strModalidad, EncabezadoSeleccionado.strMoneda, EncabezadoSeleccionado.strPosicion, EncabezadoSeleccionado.strTipo, EncabezadoSeleccionado.dtmFechaFinal, strDetalleGridIDPago, strDetalleGridValorPagoAdicional, strDetalleGridPagado, Program.Usuario, Program.HashConexion)).AsTask
                End If

                If Not objOperacionInterbancariaDetalle Is Nothing Then
                    If objOperacionInterbancariaDetalle.HasError Then
                        If objOperacionInterbancariaDetalle.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al generar el detalle de la operación interbancaria.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar el detalle de la operación interbancaria.", Me.ToString(), "Generar", Program.TituloSistema, Program.Maquina, objOperacionInterbancariaDetalle.Error)
                        End If

                        objOperacionInterbancariaDetalle.MarkErrorAsHandled()
                    Else
                        ListaDetalle = objOperacionInterbancariaDetalle.Entities.ToList
                        logGeneroDatosCorrectamente = True
                    End If
                Else
                    ListaDetalle = Nothing
                End If
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            'Se cambia el tipo de mensaje cuando el motor genera algún error en el cálculo.
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar liquidaciones de compra", Me.ToString, "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            A2Utilidades.Mensajes.mostrarMensaje("No fue posible realizar el cálculo del pago de los flujos, por favor verifique la información.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            logGeneroDatosCorrectamente = False
        End Try
    End Function

    Public Sub ActualizarDetalle()
        Try
            mobjVM = New OperacionInterbancariaViewModel
            cwOperacionInterbancariaView = New cwOperacionInterbancariaView(mobjVM, DetalleSeleccionado, HabilitarEncabezado)
            AddHandler cwOperacionInterbancariaView.Closed, AddressOf CerroVentanaCW
            Program.Modal_OwnerMainWindowsPrincipal(cwOperacionInterbancariaView)
            cwOperacionInterbancariaView.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "ActualizarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function ConsultarDatosPortafolio() As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDatosPortafolioSyncQuery(_EncabezadoSeleccionado.lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        EncabezadoSeleccionado.strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        EncabezadoSeleccionado.strNombreComitente = Nothing
                    End If
                End If
            Else
                EncabezadoSeleccionado.strNombreComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaOperacionInterbancaria
            objCB.strIDOperacion = String.Empty
            objCB.lngIDComitente = String.Empty
            objCB.strNombreComitente = String.Empty
            objCB.lngIDBanco = -1
            objCB.dtmFechaInicial = Nothing
            objCB.dtmFechaFinal = Nothing
            objCB.strPosicion = String.Empty
            objCB.strTipo = String.Empty
            objCB.strTipoTasaFija = String.Empty
            objCB.strTasaReferencia = String.Empty
            objCB.strBase = String.Empty
            objCB.strIndicadorEconomico = String.Empty
            objCB.strModalidad = String.Empty
            objCB.strMoneda = String.Empty
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
    Private Function ObtenerRegistroAnterior() As OperacionInterbancaria
        Dim objEncabezado As OperacionInterbancaria = New OperacionInterbancaria

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of OperacionInterbancaria)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDOperacionInterbancaria = _EncabezadoSeleccionado.intIDOperacionInterbancaria
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <history>
    ''' ID caso de prueba: CP0007
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida la operación
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strIDOperacion) Then
                    strMsg = String.Format("{0}{1} + La operación es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el código
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.lngIDComitente) And String.IsNullOrEmpty(_EncabezadoSeleccionado.strNombreComitente) Then
                    strMsg = String.Format("{0}{1} + El " + NOMBRE_ETIQUETA_COMITENTE.ToLower + " es un campo requerido.", strMsg, vbCrLf)
                End If

                If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.lngIDComitente) And String.IsNullOrEmpty(_EncabezadoSeleccionado.strNombreComitente) Then
                    strMsg = String.Format("{0}{1} + El " + NOMBRE_ETIQUETA_COMITENTE.ToLower + " no existe o no es válido.", strMsg, vbCrLf)
                End If

                'Valida el banco
                If (_EncabezadoSeleccionado.lngIDBanco) = -1 Then
                    strMsg = String.Format("{0}{1} + El banco es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha inicial
                If IsNothing(_EncabezadoSeleccionado.dtmFechaInicial) Then
                    strMsg = String.Format("{0}{1} + La fecha inicial es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha final
                If IsNothing(_EncabezadoSeleccionado.dtmFechaFinal) Then
                    strMsg = String.Format("{0}{1} + La fecha final es un campo requerido.", strMsg, vbCrLf)
                End If

                If Not IsNothing(_EncabezadoSeleccionado.dtmFechaInicial) And Not IsNothing(_EncabezadoSeleccionado.dtmFechaFinal) Then
                    If _EncabezadoSeleccionado.dtmFechaInicial > _EncabezadoSeleccionado.dtmFechaFinal Then
                        strMsg = String.Format("{0}{1} + La fecha inicial debe ser igual o menor a la fecha final.", strMsg, vbCrLf)
                    End If
                End If

                'Valida la posición
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strPosicion) Then
                    strMsg = String.Format("{0}{1} + La posición es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipo) Then
                    strMsg = String.Format("{0}{1} + El tipo es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo tasa fija
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoTasaFija) Then
                    strMsg = String.Format("{0}{1} + El tipo tasa fija es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la referencia tasa variable
                'If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTasaReferencia) Then
                '    strMsg = String.Format("{0}{1} + La referencia tasa variable es un campo requerido.", strMsg, vbCrLf)
                'End If

                'Valida la base
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strBase) Then
                    strMsg = String.Format("{0}{1} + La base es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el indicador
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strIndicadorEconomico) And (_EncabezadoSeleccionado.strTipoTasaFija).ToUpper = "F" Then '"F" es TASA FIJA
                    strMsg = String.Format("{0}{1} + El indicador es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la modalidad
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strModalidad) Then
                    strMsg = String.Format("{0}{1} + La modalidad es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la moneda
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strMoneda) Then
                    strMsg = String.Format("{0}{1} + La moneda es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el valor inicial
                If (_EncabezadoSeleccionado.dblValorInicial) = 0 Then
                    strMsg = String.Format("{0}{1} + El valor inicial es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la tasa - puntos
                If (_EncabezadoSeleccionado.dblTasaPuntos) = 0 Then
                    strMsg = String.Format("{0}{1} + La tasa - puntos es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el saldo
                If (_EncabezadoSeleccionado.dblSaldo) = 0 Then
                    strMsg = String.Format("{0}{1} + El saldo es un campo requerido.", strMsg, vbCrLf)
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

    Private Async Sub CerroVentanaCW(sender As Object, e As EventArgs)
        Try
            If (cwOperacionInterbancariaView.DialogResult) Then
                Await Generar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana emergente", _
                     Me.ToString(), "CerroVentanaCW", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
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
                HabilitarIndicador = False

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
        Dim objRet As LoadOperation(Of OperacionInterbancaria)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarOperacionInterbancariaPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de OperacionInterbancaria
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrIDOperacion As String = "",
                                               Optional ByVal plngIDComitente As String = "",
                                               Optional ByVal plngIDBanco As Integer = 0,
                                               Optional ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime) = Nothing,
                                               Optional ByVal pstrTipoTasaFija As String = "",
                                               Optional ByVal pstrBase As String = "",
                                               Optional ByVal pstrIndicadorEconomico As String = "",
                                               Optional ByVal pstrModalidad As String = "",
                                               Optional ByVal pstrMoneda As String = "",
                                               Optional ByVal pstrPosicion As String = "",
                                               Optional ByVal pstrTipo As String = "",
                                               Optional ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime) = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of OperacionInterbancaria)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.OperacionInterbancarias.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarOperacionInterbancariaSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarOperacionInterbancariaSyncQuery(pstrIDOperacion:=pstrIDOperacion,
                                                                                               plngIDComitente:=plngIDComitente,
                                                                                               plngIDBanco:=plngIDBanco,
                                                                                               pdtmFechaInicial:=pdtmFechaInicial,
                                                                                               pstrTipoTasaFija:=pstrTipoTasaFija,
                                                                                               pstrBase:=pstrBase,
                                                                                               pstrIndicadorEconomico:=pstrIndicadorEconomico,
                                                                                               pstrModalidad:=pstrModalidad,
                                                                                               pstrMoneda:=pstrMoneda,
                                                                                               pstrPosicion:=pstrPosicion,
                                                                                               pstrTipo:=pstrTipo,
                                                                                               pdtmFechaFinal:=pdtmFechaFinal,
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
                    ListaEncabezado = mdcProxy.OperacionInterbancarias

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de OperacionInterbancaria ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
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

            Dim objRet As LoadOperation(Of OperacionInterbancariaDetalle)

            objRet = Await dcProxy.Load(dcProxy.ConsultarOperacionInterbancariaDetallePorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
        Dim objRet As LoadOperation(Of OperacionInterbancariaDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.OperacionInterbancariaDetalles Is Nothing Then
                mdcProxy.OperacionInterbancariaDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarOperacionInterbancariaDetalleSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

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
    ''' <history>
    ''' ID caso de prueba: CP0007
    ''' Creado por:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             Abril 5/2015
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - Abril 5/2015 - Resultado Ok 
    ''' </history>
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
Public Class CamposBusquedaOperacionInterbancaria
    Implements INotifyPropertyChanged

    Private _strIDOperacion As String
    Public Property strIDOperacion() As String
        Get
            Return _strIDOperacion
        End Get
        Set(ByVal value As String)
            _strIDOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDOperacion"))
        End Set
    End Property

    Private _lngIDComitente As String
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private Async Sub _lngIDComitente_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        If e.PropertyName = "lngIDComitente" Then
            Await ConsultarDatosPortafolioBusqueda()
        End If
    End Sub

    Private _strNombreComitente As String
    Public Property strNombreComitente() As String
        Get
            Return _strNombreComitente
        End Get
        Set(ByVal value As String)
            _strNombreComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreComitente"))
        End Set
    End Property

    Private _lngIDBanco As System.Nullable(Of Integer)
    Public Property lngIDBanco() As System.Nullable(Of Integer)
        Get
            Return _lngIDBanco
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDBanco"))
        End Set
    End Property

    Private _strDescripcionBanco As String
    Public Property strDescripcionBanco() As String
        Get
            Return _strDescripcionBanco
        End Get
        Set(ByVal value As String)
            _strDescripcionBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionBanco"))
        End Set
    End Property

    Private _dtmFechaInicial As System.Nullable(Of System.DateTime)
    Public Property dtmFechaInicial() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaInicial
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaInicial"))
        End Set
    End Property

    Private _dtmFechaFinal As System.Nullable(Of System.DateTime)
    Public Property dtmFechaFinal() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaFinal
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaFinal"))
        End Set
    End Property

    Private _strTipoTasaFija As String
    Public Property strTipoTasaFija() As String
        Get
            Return _strTipoTasaFija
        End Get
        Set(ByVal value As String)
            _strTipoTasaFija = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoTasaFija"))
        End Set
    End Property

    Private _strTasaReferencia As String
    Public Property strTasaReferencia() As String
        Get
            Return _strTasaReferencia
        End Get
        Set(ByVal value As String)
            _strTasaReferencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTasaReferencia"))
        End Set
    End Property

    Private _strBase As String
    Public Property strBase() As String
        Get
            Return _strBase
        End Get
        Set(ByVal value As String)
            _strBase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strBase"))
        End Set
    End Property

    Private _strIndicadorEconomico As String
    Public Property strIndicadorEconomico() As String
        Get
            Return _strIndicadorEconomico
        End Get
        Set(ByVal value As String)
            _strIndicadorEconomico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIndicadorEconomico"))
        End Set
    End Property

    Private _strModalidad As String
    Public Property strModalidad() As String
        Get
            Return _strModalidad
        End Get
        Set(ByVal value As String)
            _strModalidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strModalidad"))
        End Set
    End Property

    Private _strMoneda As String
    Public Property strMoneda() As String
        Get
            Return _strMoneda
        End Get
        Set(ByVal value As String)
            _strMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strMoneda"))
        End Set
    End Property

    Private _strPosicion As String
    Public Property strPosicion() As String
        Get
            Return _strPosicion
        End Get
        Set(ByVal value As String)
            _strPosicion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPosicion"))
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

    Public Async Function ConsultarDatosPortafolioBusqueda() As Task
        Try
            Dim mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

            Dim objRet As LoadOperation(Of DatosPortafolios)

            mdcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDatosPortafolioSyncQuery(lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        strNombreComitente = Nothing
                    End If
                End If
            Else
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class




