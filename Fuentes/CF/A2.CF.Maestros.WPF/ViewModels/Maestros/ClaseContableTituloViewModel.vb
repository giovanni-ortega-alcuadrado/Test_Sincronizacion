Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports System.Threading.Tasks

Public Class ClaseContableTituloViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblClaseContableTitulo
    Private mdcProxyActualizar As MaestrosCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblClaseContableTitulo
    Private mobjDetallePorDefecto As ConfiguracionContableMultimoneda

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
    ''' <history>
    ''' Creado por       : Yessid Andrés Paniagua Pabón (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 28/2016
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón (Alcuadrado S.A.) - Abril 28/2016 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()
                ConsultarDetalleMultimonedaPorDefectoSync()

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
    Private _DiccionarioCombosA2 As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosA2() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosA2
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosA2 = value
        End Set
    End Property

    ''' <summary>
    ''' Lista de clase contable titulo que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of tblClaseContableTitulo)
    Public Property ListaEncabezado() As EntitySet(Of tblClaseContableTitulo)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of tblClaseContableTitulo))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de clase contable titulo para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de clase contable titulo que se encuentra seleccionado para editar y nuevo
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As tblClaseContableTitulo
    Public Property EncabezadoSeleccionado() As tblClaseContableTitulo
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As tblClaseContableTitulo)
            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDClaseContableTitulo > 0 Then
                    ConsultarDetalleMultimoneda(_EncabezadoSeleccionado.intIDClaseContableTitulo)
                Else
                    logIncializarDet = True
                End If
            End If

            If logIncializarDet Then
                ListaDetalleMultimoneda = Nothing
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    'Propiedad para obtener correctamente el registro que se esta modificando
    Private WithEvents _EncabezadoAnterior As tblClaseContableTitulo
    Public Property EncabezadoAnterior() As tblClaseContableTitulo
        Get
            Return _EncabezadoAnterior
        End Get
        Set(ByVal value As tblClaseContableTitulo)
            _EncabezadoAnterior = value
            MyBase.CambioItem("EncabezadoAnterior")
        End Set
    End Property




    ''' <summary>
    ''' Elemento de la lista de clase contable titulo que se encuentra seleccionado para la busqueda avanzada
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionadoBusqueda As tblClaseContableTitulo
    Public Property EncabezadoSeleccionadoBusqueda() As tblClaseContableTitulo
        Get
            Return _EncabezadoSeleccionadoBusqueda
        End Get
        Set(ByVal value As tblClaseContableTitulo)
            _EncabezadoSeleccionadoBusqueda = value

            MyBase.CambioItem("EncabezadoSeleccionadoBusqueda")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private WithEvents _cb As CamposBusquedaClaseContableTitulo
    Public Property cb() As CamposBusquedaClaseContableTitulo
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaClaseContableTitulo)
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

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' 
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New tblClaseContableTitulo

        Try
            If mdcProxyActualizar.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of tblClaseContableTitulo)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.strReferencia = String.Empty
                objNvoEncabezado.strTipoTitulo = String.Empty
                objNvoEncabezado.logGravado = False
                objNvoEncabezado.logNCRNGO = False
                objNvoEncabezado.dtmActualizacion = Date.Now()
                objNvoEncabezado.strUsuario = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()
            HabilitarEdicionDetalleMultimoneda = False

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
    ''' 
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
    ''' 
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' 
    Public Overrides Async Sub ConfirmarBuscar()
        Try

            Await ConsultarEncabezado(False, String.Empty, cb.strReferenciaDescripcion, cb.strTipoTituloDescripcion)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' 
    Public Overrides Sub ActualizarRegistro()

        Dim intNroOcurrencias As Integer
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()
        Dim xmlDetalle As String

        Try
            ErrorForma = String.Empty
            IsBusy = True

            If ValidarRegistro() Then
                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty
                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.intIDClaseContableTitulo = _EncabezadoSeleccionado.intIDClaseContableTitulo Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

                xmlDetalle = "<DetalleMultimoneda>"

                If Not IsNothing(ListaDetalleMultimoneda) Then
                    For Each objeto In (From c In ListaDetalleMultimoneda)

                        xmlDetalle += "<Detalle intIDContableListas = """ & objeto.intIDContableListas &
                                     """ strCuentaContable=""" & objeto.strCuentaContable &
                                     """ strIDMoneda=""" & objeto.strIDMoneda & """></Detalle>"
                    Next
                End If
                xmlDetalle += "</DetalleMultimoneda>"


                _EncabezadoSeleccionado.dtmActualizacion = Now
                _EncabezadoSeleccionado.strInfoSesion = String.Empty
                EncabezadoSeleccionado.xmlDetalleGrid = xmlDetalle

                ' Enviar cambios al servidor
                Program.VerificarCambiosProxyServidor(mdcProxyActualizar)
                mdcProxyActualizar.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' 
    Dim RegresarReg As Boolean
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then

            If mdcProxyActualizar.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            _EncabezadoSeleccionado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            EncabezadoAnterior = _EncabezadoSeleccionado

            Editando = True

            RegresarReg = True '/**/
            HabilitarEdicionDetalleMultimoneda = EncabezadoSeleccionado.logMultimoneda

            MyBase.CambioItem("Editando")
            HabilitarEncabezado = True

            IsBusy = False
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxyActualizar.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarEdicionDetalleMultimoneda = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' 
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxyActualizar.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra la clase contable titulo seleccionada. ¿Confirma el borrado de esta clase contable titulo?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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
    ''' 
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxyActualizar.tblClaseContableTitulos.Where(Function(i) i.intIDClaseContableTitulo = EncabezadoSeleccionado.intIDClaseContableTitulo).Count > 0 Then
                        mdcProxyActualizar.tblClaseContableTitulos.Remove(mdcProxyActualizar.tblClaseContableTitulos.Where(Function(i) i.intIDClaseContableTitulo = EncabezadoSeleccionado.intIDClaseContableTitulo).First)
                    End If


                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxyActualizar)
                    mdcProxyActualizar.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region
#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    ''' 
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaClaseContableTitulo
            objCB.strTipoTituloDescripcion = String.Empty
            objCB.strReferenciaDescripcion = String.Empty

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
    ''' 
    Private Function ObtenerRegistroAnterior() As tblClaseContableTitulo
        Dim objEncabezado As tblClaseContableTitulo = New tblClaseContableTitulo

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of tblClaseContableTitulo)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDClaseContableTitulo = _EncabezadoSeleccionado.intIDClaseContableTitulo
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
                'Valida el tipo de indicador
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoTitulo) Or IsNothing(_EncabezadoSeleccionado.strTipoTitulo) Then
                    strMsg = String.Format("{0}{1} + El tipo título es un campo requerido.", strMsg, vbCrLf)
                End If

                If _EncabezadoSeleccionado.logMultimoneda Then


                    If IsNothing(_ListaDetalleMultimoneda) Then
                        strMsg = String.Format("{0}{1} + Ha indicado que la clase contable es multimoneda, por lo cual es necesario que exista por lo menos un detalle.", strMsg, vbCrLf)
                    ElseIf _ListaDetalleMultimoneda.Count = 0 Then
                        strMsg = String.Format("{0}{1} + Ha indicado que la clase contable es multimoneda, por lo cual es necesario que exista por lo menos un detalle.", strMsg, vbCrLf)
                    End If

                    If Not IsNothing(DetalleMultimonedaSeleccionado) Then

                        If ListaDetalleMultimoneda.Where(Function(i) i.intIDContableListas = DetalleMultimonedaSeleccionado.intIDContableListas And
                                                         i.strIDMoneda = DetalleMultimonedaSeleccionado.strIDMoneda).Count > 1 Then
                            strMsg = String.Format("{0}{1} + No se permite asociar un concepto contable y una moneda más de una vez", strMsg, vbCrLf)
                        End If
                    End If

                    For Each objeto In ListaDetalleMultimoneda
                            If objeto.intIDContableListas = 0 Then
                                strMsg = String.Format("{0}{1} + Debe asignar un valor para el campo Concepto contable.", strMsg, vbCrLf)
                            End If
                            If objeto.strIDMoneda = "" Then
                                strMsg = String.Format("{0}{1} + Debe asignar un valor para el campo Moneda.", strMsg, vbCrLf)
                            End If
                            If objeto.strCuentaContable = "" Then
                                strMsg = String.Format("{0}{1} + Debe asignar un valor para el campo Cuenta contable.", strMsg, vbCrLf)
                            End If

                            If Not strMsg.Equals(String.Empty) Then
                                Exit For
                            End If

                        Next


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

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        Try
            If Editando Then
                Select Case e.PropertyName
                    Case "logMultimoneda"
                        If Not IsNothing(DetalleMultimonedaSeleccionado) Then
                            If DetalleMultimonedaSeleccionado.intIDClaseContableTitulo >= 1 And Not EncabezadoSeleccionado.logMultimoneda Then
                                ValidarBorrarConfirmacionDetalle()
                            End If
                        End If
                        HabilitarEdicionDetalleMultimoneda = EncabezadoSeleccionado.logMultimoneda
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la propiedad del encabezado seleccionado", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
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
                mdcProxyActualizar.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False
                HabilitarEdicionDetalleMultimoneda = False
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
        Dim objRet As LoadOperation(Of tblClaseContableTitulo)
        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarClaseContableTituloPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de clase contable titulo
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrTipoTitulo As String = "",
                                               Optional ByVal pstrReferencia As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of tblClaseContableTitulo)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyMaestros()
            End If

            mdcProxyActualizar.tblClaseContableTitulos.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.FiltrarClaseContableTituloSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.ConsultarClaseContableTituloSyncQuery(pstrTipoTitulo:=pstrTipoTitulo,
                                                                                                        pstrReferencia:=pstrReferencia,
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
                    ListaEncabezado = mdcProxyActualizar.tblClaseContableTitulos

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

            'If Not IsNothing(EncabezadoSeleccionado) And RegresarReg = True And Not IsNothing(EncabezadoAnterior) Then 'Se encarga de colocar el registro que se esta editando
            '    EncabezadoSeleccionado = EncabezadoAnterior
            '    RegresarReg = False
            'End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Indicadores ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub BorrarReferencia()
        EncabezadoSeleccionado.strReferencia = ""
    End Sub

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"
    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetalleMultimonedaPorDefectoSync()

        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            Dim objRet As LoadOperation(Of ConfiguracionContableMultimoneda)

            objRet = Await dcProxy.Load(dcProxy.ConsultarConfiguracionContableMultimonedaPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' <param name="pintIDClaseContableTitulo">Id del encabezado</param>
    ''' <remarks></remarks>
    Public Async Function ConsultarDetalleMultimoneda(ByVal pintIDClaseContableTitulo As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As MaestrosCFDomainContext = inicializarProxyMaestros()
        Dim objRet As LoadOperation(Of ConfiguracionContableMultimoneda)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.ConfiguracionContableMultimonedas Is Nothing Then
                mdcProxy.ConfiguracionContableMultimonedas.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionContableMultimonedaSyncQuery(pintIDClaseContableTitulo, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la clase contable pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la clase contable.", Me.ToString(), "ConsultarDetalleMultimoneda", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalleMultimoneda = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la configuración contable multimoneda seleccionada.", Me.ToString(), "ConsultarDetalleMultimoneda", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function


#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Detalles.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  
    ''' Creado por:       Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:            Agosto 5 de 2016
    ''' Pruebas CB:       Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) -Agosto 5 de 2016 - Resultado Ok 
    ''' </history> 
    Public Sub ValidarBorrarConfirmacionDetalle()
        Dim strMsg As String
        strMsg = "Si continua los detalles para la configuración contable multimoneda quedaran deshabilitados"
        A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, "", AddressOf TerminoPreguntarDetalle)
    End Sub



    ''' <summary>
    ''' Se pregunta si en realidad se desea eliminar el detalle del Grid de Detalles.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:
    ''' Creado por:       Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:            Agosto 5 de 2016
    ''' Pruebas CB:       Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) -Agosto 5 de 2016 - Resultado Ok 
    ''' </history> 
    Private Sub TerminoPreguntarDetalle(ByVal sender As Object, e As System.EventArgs)
        Try
            If Not IsNothing(sender) Then
                If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                    HabilitarEdicionDetalleMultimoneda = False
                    EncabezadoSeleccionado.logMultimoneda = False
                Else
                    HabilitarEdicionDetalleMultimoneda = True
                    EncabezadoSeleccionado.logMultimoneda = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la respuesta en TerminoPreguntarDetalle", _
             Me.ToString(), "TerminoPreguntarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


#Region "Métodos públicos del detalle - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' Creado por:       Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:            Agosto 5 de 2016
    ''' Pruebas CB:       Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) -Agosto 5 de 2016 - Resultado Ok 
    ''' </history> 

    Public Sub BorrarDetalleMultimoneda()
        Try
            If Not IsNothing(DetalleMultimonedaSeleccionado) Then

                If _ListaDetalleMultimoneda.Where(Function(i) i.intIDContableMultimoneda = _DetalleMultimonedaSeleccionado.intIDContableMultimoneda).Count > 0 Then
                    _ListaDetalleMultimoneda.Remove(_ListaDetalleMultimoneda.Where(Function(i) i.intIDContableMultimoneda = _DetalleMultimonedaSeleccionado.intIDContableMultimoneda).First)
                End If

                Dim objNuevaListaDetalle As New List(Of ConfiguracionContableMultimoneda)

                For Each li In _ListaDetalleMultimoneda
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDContableMultimoneda = _DetalleMultimonedaSeleccionado.intIDContableMultimoneda).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDContableMultimoneda = _DetalleMultimonedaSeleccionado.intIDContableMultimoneda).First)
                End If

                ListaDetalleMultimoneda = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalleMultimoneda) Then
                    If _ListaDetalleMultimoneda.Count > 0 Then
                        DetalleMultimonedaSeleccionado = _ListaDetalleMultimoneda.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la compañía", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub IngresarDetalleMultimoneda()
        Try
            'Validar que exista un cliente seleccionado en el encabezado
            If Not IsNothing(_EncabezadoSeleccionado.intIDClaseContableTitulo) Then
                Dim objNvoDetalle As New ConfiguracionContableMultimoneda
                Dim objNuevaLista As New List(Of ConfiguracionContableMultimoneda)

                Program.CopiarObjeto(Of ConfiguracionContableMultimoneda)(mobjDetallePorDefecto, objNvoDetalle)

                objNvoDetalle.intIDContableMultimoneda = -New Random().Next(0, 1000000)

                If IsNothing(ListaDetalleMultimoneda) Then
                    ListaDetalleMultimoneda = New List(Of ConfiguracionContableMultimoneda)
                End If

                objNuevaLista = ListaDetalleMultimoneda
                objNuevaLista.Add(objNvoDetalle)
                ListaDetalleMultimoneda = objNuevaLista
                DetalleMultimonedaSeleccionado = _ListaDetalleMultimoneda.First

                MyBase.CambioItem("ListaDetalleMultimoneda")
                MyBase.CambioItem("ListaDetalleMultimonedaPaginada")
                MyBase.CambioItem("DetalleMultimonedaSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información de la clase contable para poder agregar un detalle. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle", Me.ToString(), "IngresarDetalleMultimoneda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#End Region

#Region "Propiedades del detalle - REQUERIDO"

    ''' <summary>
    ''' Propiedad Habilitar la edicion del detalle
    ''' </summary>
    Private _HabilitarEdicionDetalleMultimoneda As Boolean = False
    Public Property HabilitarEdicionDetalleMultimoneda() As Boolean
        Get
            Return _HabilitarEdicionDetalleMultimoneda
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalleMultimoneda = value
            MyBase.CambioItem("HabilitarEdicionDetalleMultimoneda")
        End Set
    End Property


    Private _ListaDetalleMultimoneda As List(Of ConfiguracionContableMultimoneda)
    Public Property ListaDetalleMultimoneda() As List(Of ConfiguracionContableMultimoneda)
        Get
            Return _ListaDetalleMultimoneda
        End Get
        Set(ByVal value As List(Of ConfiguracionContableMultimoneda))
            _ListaDetalleMultimoneda = value
            MyBase.CambioItem("ListaDetalleMultimoneda")
            MyBase.CambioItem("ListaDetalleMultimonedaPaginada")
        End Set
    End Property


    Public ReadOnly Property ListaDetalleMultimonedaPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleMultimoneda) Then
                Dim view = New PagedCollectionView(_ListaDetalleMultimoneda)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleMultimonedaSeleccionado As ConfiguracionContableMultimoneda
    Public Property DetalleMultimonedaSeleccionado() As ConfiguracionContableMultimoneda
        Get
            Return _DetalleMultimonedaSeleccionado
        End Get
        Set(ByVal value As ConfiguracionContableMultimoneda)
            _DetalleMultimonedaSeleccionado = value
            MyBase.CambioItem("DetalleMultimonedaSeleccionado")
        End Set
    End Property


#End Region
End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' 
Public Class CamposBusquedaClaseContableTitulo
    Implements INotifyPropertyChanged

    Private _strTipoTituloDescripcion As String
    Public Property strTipoTituloDescripcion() As String
        Get
            Return _strTipoTituloDescripcion
        End Get
        Set(ByVal value As String)
            _strTipoTituloDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoTituloDescripcion"))
        End Set
    End Property

    Private _strReferenciaDescripcion As String
    Public Property strReferenciaDescripcion() As String
        Get
            Return _strReferenciaDescripcion
        End Get
        Set(ByVal value As String)
            _strReferenciaDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strReferenciaDescripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
