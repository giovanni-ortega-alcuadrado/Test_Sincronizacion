Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web.CFMaestros
Imports A2.OYD.OYDServer.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades

''' <summary>
''' ViewModel para la pantalla Especies Excluidas perteneciente al proyecto de Maestros.
''' </summary>
''' <history>
''' Creado por       : Javier Pardo  - Alcuadrado S.A.
''' Descripción      : Creación.
''' Fecha            : Enero 19/2016
''' Pruebas CB       : Javier Pardo - Enero 19/2016 - Resultado Ok 
''' </history>

Public Class EspeciesExcluidasViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Proxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As EspeciesExcluidasEncabezado
    Private mdcProxy As MaestrosCFDomainContext    ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As EspeciesExcluidasViewModel

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As EspeciesExcluidasEncabezado
    Private mobjDetallePorDefecto As EspeciesExcluidasDetalles
    Public objProxy As UtilidadesDomainContext

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                objProxy = New UtilidadesDomainContext()
            Else
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If
            If Not Program.IsDesignMode() Then
                DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la función Nuevo.", Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <history>
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Creación.
    ''' Fecha            : Enero 19/2016
    ''' Pruebas CB       : Javier Pardo (Alcuadrado S.A.) - Enero 19/2016 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarValoresListaFormatos()
                consultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()

                'Cargar los motivos de exclusión desde cargar combos con la categoría EXCLUSION_F
                Await CargarMotivosExclusion()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

                'Consultar los formatos previamente guardados en la tabla tblEspeciesExcluidas
                ConsultarListaFormatosRegistrados()
                

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
    ''' Lista de Especies Excluidas Encabezado que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of EspeciesExcluidasEncabezado)
    Public Property ListaEncabezado() As EntitySet(Of EspeciesExcluidasEncabezado)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of EspeciesExcluidasEncabezado))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de EspeciesExcluidasEncabezado para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de EspeciesExcluidasEncabezado que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As EspeciesExcluidasEncabezado
    Public Property EncabezadoSeleccionado() As EspeciesExcluidasEncabezado
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As EspeciesExcluidasEncabezado)

            Dim logIncializarDet As Boolean = False

           _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intID > 0 Then
                    Call FiltrarMotivosExclusion(EncabezadoSeleccionado.strFormato, "Forma")
                    ConsultarDetalle(_EncabezadoSeleccionado.strFormato)
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
            If Editando Then

                Select Case e.PropertyName
                    'Case "strNumeroDocumento"
                    '    _mlogBuscarClienteEncabezado = True
                    '    _mLogBuscarClienteDetalle = False

                    '    If Not String.IsNullOrEmpty(_EncabezadoSeleccionado.strNumeroDocumento) Then
                    '        buscarComitente(_EncabezadoSeleccionado.strNumeroDocumento)
                    '    End If
                    'Case Else
                    Case "strFormato"
                        Call FiltrarMotivosExclusion(EncabezadoSeleccionado.strFormato, "Forma")
                End Select

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_EncabezadoSeleccionado.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Lista de Especies Excluidas Encabezado copia que será
    ''' </summary>
    Private _ListaEncabezadoInicial As EntitySet(Of EspeciesExcluidasEncabezado)
    Public Property ListaEncabezadoInicial() As EntitySet(Of EspeciesExcluidasEncabezado)
        Get
            Return _ListaEncabezadoInicial
        End Get
        Set(ByVal value As EntitySet(Of EspeciesExcluidasEncabezado))
            _ListaEncabezadoInicial = value

            MyBase.CambioItem("ListaEncabezadoInicial")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private WithEvents _cb As CamposBusquedaEspeciesExcluidas
    Public Property cb() As CamposBusquedaEspeciesExcluidas
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaEspeciesExcluidas)
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

    Private _HabilitarCamposEdicion As Boolean = False
    Public Property HabilitarCamposEdicion() As Boolean
        Get
            Return _HabilitarCamposEdicion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposEdicion = value
            MyBase.CambioItem("HabilitarCamposEdicion")
        End Set
    End Property

    Private _EspeciesExcluidasEncabezado As List(Of EspeciesExcluidasEncabezado)
    Public Property EspeciesExcluidasEncabezado() As List(Of EspeciesExcluidasEncabezado)
        Get
            Return _EspeciesExcluidasEncabezado
        End Get
        Set(ByVal value As List(Of EspeciesExcluidasEncabezado))
            _EspeciesExcluidasEncabezado = value
            MyBase.CambioItem("EspeciesExcluidasEncabezado")
        End Set
    End Property

    Private _HabilitarEdicionDetalle As Boolean
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property

    Private _HabilitarBoton As System.Boolean = True
    Public Property HabilitarBoton() As System.Boolean
        Get
            Return _HabilitarBoton
        End Get
        Set(ByVal value As System.Boolean)
            _HabilitarBoton = value
            MyBase.CambioItem("HabilitarBoton")
        End Set
    End Property

    Private _HabilitarCampoFormato As Boolean
    Public Property HabilitarCampoFormato() As Boolean
        Get
            Return _HabilitarCampoFormato
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCampoFormato = value
            MyBase.CambioItem("HabilitarCampoFormato")
        End Set
    End Property

    ''' <summary>
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Descripción:    Propiedad para cargar la lista del tópico FORMATOS_SUPERFINANC y poder manipular los items desde la pantalla
    '''                 Se filtra la información necesaria en la propiedad ListaFormatosFiltrados
    ''' Fecha:          Enero 21/2015
    ''' ID del cambio:  JEPM20160121
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaFormatos As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaFormatos As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaFormatos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaFormatos = value
            MyBase.CambioItem("ListaFormatos")
        End Set
    End Property

    ''' <summary>
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Descripción:    Propiedad para filtrar ListaFormatos y eliminar los formatos ya registrados cuando se realiza un nuevo registro
    ''' Fecha:          Enero 21/2015
    ''' ID del cambio:  JEPM20160121
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaFormatosFiltrados As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaFormatosFiltrados As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaFormatosFiltrados
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaFormatosFiltrados = value
            MyBase.CambioItem("ListaFormatosFiltrados")
        End Set
    End Property

    ''' <summary>
    ''' lista de ItemsCombo con los formatos que ya están almacenados en la base de datos para no mostrarlos en la lista al realizar nuevo registro
    ''' </summary>
    ''' <remarks></remarks>
    Private _itemsFormatos As List(Of OYDUtilidades.ItemCombo)
    Public Property itemsFormatos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _itemsFormatos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _itemsFormatos = value
            MyBase.CambioItem("itemsFormatos")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para determinar si se permite eliminar cuando el usuario elimina el ultimo registro de los detalles
    ''' </summary>
    ''' <remarks></remarks>
    Private _HabilitarEliminar As Boolean = False
    Public Property HabilitarEliminar() As Boolean
        Get
            Return _HabilitarEliminar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEliminar = value
            MyBase.CambioItem("HabilitarEliminar")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New EspeciesExcluidasEncabezado

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of EspeciesExcluidasEncabezado)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intID = -1
                objNvoEncabezado.strFormato = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True
            HabilitarCamposEdicion = True
            HabilitarEdicionDetalle = True
            HabilitarCampoFormato = True

            FiltrarFormatos(True)

            HabilitarEliminar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' TODO CP
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
    ''' TODO CP
    Public Overrides Sub Buscar()
        Try
            PrepararNuevaBusqueda()
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al buscar.", Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.strFormato) Or Not IsNothing(cb.strIDEspecie) Or
                Not IsNothing(cb.strExclusionFormato) Then 'Validar que ingresó algo en los campos de búsqueda

                Await ConsultarEncabezado(False, String.Empty, cb.strFormato, cb.strIDEspecie, cb.strExclusionFormato)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' ID caso de prueba:  CP008 Guardar Formato
    ''' </summary>
    Public Overrides Async Sub ActualizarRegistro()

        Dim strAccion As String = ValoresUserState.Actualizar.ToString()
        Dim EncabezadoActual As EspeciesExcluidasEncabezado = New EspeciesExcluidasEncabezado()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            ErrorForma = String.Empty
            IsBusy = True

            If ValidarRegistro() Then

                If ValidarDetalle() Then

                    ' Incializar los mensajes de validación
                    _EncabezadoSeleccionado.strMsgValidacion = String.Empty

                    If Not IsNothing(ListaDetalle) Then

                        'Construir el XML de los detalles
                        xmlCompleto = "<EspeciesExcluidas>"

                        For Each objeto In (From c In ListaDetalle)

                            xmlDetalle = "<Detalle strFormato=""" & objeto.strFormato &
                                        """ strIDEspecie=""" & objeto.strIDEspecie &
                                        """ strExclusionFormato=""" & objeto.strExclusionFormato & """></Detalle>"

                            xmlCompleto = xmlCompleto & xmlDetalle
                        Next

                        xmlCompleto = xmlCompleto & "</EspeciesExcluidas>"
                    Else
                        xmlDetalle = String.Empty
                    End If

                    Dim strMsg As String = String.Empty
                    Dim objRet As InvokeOperation(Of String)

                    objRet = Await mdcProxy.ActualizarEspeciesExcluidasEncabezadoSync(EncabezadoSeleccionado.strFormato, xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                        strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                        If Not String.IsNullOrEmpty(strMsg) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        EncabezadoActual = EncabezadoSeleccionado
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarCamposEdicion = False
                        HabilitarEdicionDetalle = False
                        HabilitarCampoFormato = False

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        Await ConsultarEncabezado(True, String.Empty)

                        ConsultarListaFormatosRegistrados()

                        FiltrarFormatos(False)

                        'Si está guardando un nuevo encabezado seleccionarlo nuevamente y no el que es por defecto
                        If EncabezadoActual.intID = 0 Then
                            'Asignar el ID para que pueda buscar los detalles al asignar el EncabezadoSeleccionado
                            EncabezadoActual.intID = ListaEncabezado.Where(Function(i) i.strFormato = EncabezadoActual.strFormato).FirstOrDefault.intID
                            EncabezadoSeleccionado = EncabezadoActual
                        End If

                    End If 'If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                End If 'If ValidarDetalle() Then

            Else
                HabilitarEncabezado = True
                HabilitarCamposEdicion = True
                HabilitarEdicionDetalle = True
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
    ''' ID caso de prueba:  CP009 No habilitar los campos tipo documento ni numero documento en edición
    ''' ID caso de prueba:  CP012 que no permita modificar si esta inactiva
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                IsBusy = True

                _EncabezadoSeleccionado.strUsuario = Program.Usuario

                mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                'JEPM20160125 Almacenar el valor del formato, ya que al filtrar se pierde
                'Dim strFormatoSeleccionado As String = EncabezadoSeleccionado.strFormato
                FiltrarFormatos(False)
                'EncabezadoSeleccionado.strFormato = strFormatoSeleccionado

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarEncabezado = True
                HabilitarCamposEdicion = False
                HabilitarEdicionDetalle = True

                HabilitarBoton = False
                HabilitarCampoFormato = False
                HabilitarEliminar = True

                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el registro.", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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

                FiltrarFormatos(False)

                EncabezadoSeleccionado = mobjEncabezadoAnterior

                'JEPM20160125 En el grid se pierde la propiedad strFormato y se vuelve a asignar manualmente
                If _ListaEncabezado.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count > 0 Then
                    _ListaEncabezado.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).First.strFormato = mobjEncabezadoAnterior.strFormato
                End If
                HabilitarEncabezado = False
                HabilitarCamposEdicion = False
                HabilitarEdicionDetalle = False
                HabilitarCampoFormato = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            MyBase.CancelarBuscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar búsqueda.", Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' ID caso de prueba:  CP011
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            HabilitarEliminar = True 'Se vuelve a dejar en falso en el Finally en BorrarRegistroConfirmado
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                'Preguntar que si desea borrar el formato, el cual elimina todos los detalles
                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción elimina toda la configuración de especies excluidas relacionada al Formato " & _EncabezadoSeleccionado.strFormato & ". ¿Confirma el borrado de la configuración?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            HabilitarEliminar = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Elimina todos los detalles de la ListaDetalles y ejecuta el proceso de actualizar. 
    ''' Para el usuario es como si se eliminara un encabezado y detalles, pero en este caso se eliminan los detalles.
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    ListaDetalle.Clear()
                    ActualizarRegistro()

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            HabilitarEliminar = False
        End Try
    End Sub

    ''' <summary>
    ''' JEPM20160125 Método para Refrescar Combos y cargar las listas manualmente
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function RefresacarCache() As Task
        Try
            IsBusy = True

            If Not IsNothing(EncabezadoSeleccionado) Then
                Dim strFormatoSeleccionado As String = EncabezadoSeleccionado.strFormato

                Dim A2VM As New A2UtilsViewModel
                Await A2VM.inicializarCombos(String.Empty, String.Empty, True)
                Await CargarMotivosExclusion() 'JEPM20160202

                consultarValoresListaFormatos()
                FiltrarFormatos(HabilitarCampoFormato)

                EncabezadoSeleccionado.strFormato = strFormatoSeleccionado
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Refresacar Cache. ", Me.ToString(), "RefresacarCache", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function
#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaEspeciesExcluidas
            objCB.strFormato = String.Empty
            objCB.strIDEspecie = String.Empty
            objCB.strExclusionFormato = String.Empty
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
    Private Function ObtenerRegistroAnterior() As EspeciesExcluidasEncabezado
        Dim objEncabezado As EspeciesExcluidasEncabezado = New EspeciesExcluidasEncabezado

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of EspeciesExcluidasEncabezado)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intID = _EncabezadoSeleccionado.intID
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <summary>
    ''' Validar campos obligatorios antes de actualizar o insertar.
    ''' ID caso de prueba:  CP010
    ''' </summary>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Formato
                If (_EncabezadoSeleccionado.strFormato) = "" Then
                    strMsg = String.Format("{0}{1} + El formato es un campo requerido.", strMsg, vbCrLf)
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
                HabilitarCamposEdicion = False
                HabilitarEdicionDetalle = False
                HabilitarCampoFormato = False

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

    Private Sub TerminoTraerEspeciesExcluidas(ByVal lo As LoadOperation(Of EspeciesExcluidasEncabezado))
        Try
            If Not lo.HasError Then
                If mdcProxy.EspeciesExcluidasEncabezados.Count > 0 Then
                    EspeciesExcluidasEncabezado = mdcProxy.EspeciesExcluidasEncabezados.ToList
                    IsBusy = False
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro de especies excluidas, por favor ingrese por lo menos un registro en el maestro de especies excluidas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de especies excluidas", _
                                                 Me.ToString(), "TerminoTraerEspeciesExcluidas", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener información.", Me.ToString(), "TerminoTraerEspeciesExcluidas", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub
#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of EspeciesExcluidasEncabezado)
        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarEspeciesExcluidasEncabezadoPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de EspeciesExcluidasEncabezado
    ''' ID caso de prueba:  CP003, CP004
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrFormato As String = "",
                                               Optional ByVal pstrIDEspecie As String = "",
                                               Optional ByVal pstrExclusionFormato As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of EspeciesExcluidasEncabezado)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyMaestros()
            End If

            mdcProxy.EspeciesExcluidasEncabezados.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarEspeciesExcluidasEncabezadoSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarEspeciesExcluidasEncabezadoSyncQuery(pstrFormato:=pstrFormato,
                                                                                  pstrIDEspecie:=pstrIDEspecie,
                                                                                  pstrExclusionFormato:=pstrExclusionFormato,
                                                                                  pstrUsuario:=Program.Usuario,
                                                                                  pstrInfoConexion:=Program.HashConexion)).AsTask()
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
                    ListaEncabezado = mdcProxy.EspeciesExcluidasEncabezados

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

            'Dim strFormatoSeleccionado As String = EncabezadoSeleccionado.strFormato
            FiltrarFormatos(False)
            'EncabezadoSeleccionado.strFormato = strFormatoSeleccionado
            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de especies excluidas ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function


    Private Sub consultarValoresListaFormatos()
        Try
            If Application.Current.Resources.Contains(Program.NombreListaCombos) Then
                If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("FORMATOS_SUPERFINANC") Then
                    If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("FORMATOS_SUPERFINANC")) Then
                        Dim objListaFormatos As List(Of OYDUtilidades.ItemCombo)
                        objListaFormatos = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
                                     Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("FORMATOS_SUPERFINANC"))
                        Me.ListaFormatos = objListaFormatos 'Almacenar los tipos de formatos

                        Dim objListaFormatosFiltrados As New List(Of OYDUtilidades.ItemCombo)

                        For Each li In ListaFormatos
                            objListaFormatosFiltrados.Add(li)
                        Next

                        Me.ListaFormatosFiltrados = objListaFormatosFiltrados

                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores de la lista de formatos.", Me.ToString(), "consultarValoresListaFormatos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filtrar la lista de formatos que se le muestran al usuario para que al agregar una nueva exclusión no repita un formato previamente guardado
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FiltrarFormatos(ByVal blnNuevoRegistro As Boolean)
        Try
            Dim strFormatoSeleccionado As String = String.Empty

            If Not IsNothing(_ListaEncabezado) Then

                'Determinar si la lista se debe filtrar o debe contener todos los items
                Dim objListaFormatosFiltrados As New List(Of OYDUtilidades.ItemCombo)

                For Each li In ListaFormatos
                    objListaFormatosFiltrados.Add(li)
                Next

                If blnNuevoRegistro Then
                    For Each item In itemsFormatos.ToList()
                        objListaFormatosFiltrados.Remove(objListaFormatosFiltrados.Where(Function(e) e.ID = CType(item, ItemCombo).ID).FirstOrDefault())
                    Next
                Else
                    If Not IsNothing(EncabezadoSeleccionado) Then
                        strFormatoSeleccionado = EncabezadoSeleccionado.strFormato
                    End If
                End If 'If blnNuevoRegistro Then

                Me.ListaFormatosFiltrados = objListaFormatosFiltrados
                If Not blnNuevoRegistro And Not IsNothing(EncabezadoSeleccionado) Then EncabezadoSeleccionado.strFormato = strFormatoSeleccionado

            End If 'If Not IsNothing(_ListaEncabezado) Then

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al filtrar los valores de la lista de formatos.", Me.ToString(), "FiltrarFormatos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' consulta la lista de formatos registrados previamente en la base de datos. Esto lo realiza de la carga inicial para realizar filtro de formatos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConsultarListaFormatosRegistrados()
        Try
            Dim objListItemCombo As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)

            If Not IsNothing(ListaEncabezado) Then

                For Each item As EspeciesExcluidasEncabezado In ListaEncabezado
                    objListItemCombo.Add(New ItemCombo With {.Categoria = item.strFormatoDescripcion,
                                                             .Descripcion = "FORMATOS_SUPERFINANC",
                                                             .ID = item.strFormato
                                                            })
                Next
                itemsFormatos = objListItemCombo
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub _cb_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _cb.PropertyChanged
        Try
            If e.PropertyName = "strFormato" Then
                Call FiltrarMotivosExclusion(cb.strFormato, "cb")
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub
#End Region

#Region "Propiedades para Detalles"

    ''' <summary>
    ''' Lista de detalles del formato. En este caso la misma tabla de Especies excluidas solamente que filtradas por formato
    ''' </summary>
    Private _ListaDetalle As List(Of EspeciesExcluidasDetalles)
    Public Property ListaDetalle() As List(Of EspeciesExcluidasDetalles)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of EspeciesExcluidasDetalles))
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
    Private WithEvents _DetalleSeleccionado As EspeciesExcluidasDetalles
    Public Property DetalleSeleccionado() As EspeciesExcluidasDetalles
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As EspeciesExcluidasDetalles)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:      Lista de motivos de exclusión filtrados por formato para cargar combo de los detalles
    ''' Desarrollado por: Javier Eduardo Pardo Moreno
    ''' Fecha:            Febrero 01/2011
    ''' ID del cambio:    JEPM20160201
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaComboMotivosExclusion_Filtrada As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboMotivosExclusion_Filtrada As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboMotivosExclusion_Filtrada
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboMotivosExclusion_Filtrada = value
            MyBase.CambioItem("ListaComboMotivosExclusion_Filtrada")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:      Lista de motivos de exclusión filtrados por formato para cargar combo de la búsqueda avanzada
    ''' Desarrollado por: Javier Eduardo Pardo Moreno
    ''' Fecha:            Febrero 02/2011
    ''' ID del cambio:    JEPM20160202
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaComboMotivosExclusion_Filtrada_Busqueda As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboMotivosExclusion_Filtrada_Busqueda As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboMotivosExclusion_Filtrada_Busqueda
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboMotivosExclusion_Filtrada_Busqueda = value
            MyBase.CambioItem("ListaComboMotivosExclusion_Filtrada_Busqueda")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:      Lista de motivos de exclusión para relacionar los formatos con la categoría EXCLUSION_F
    ''' Desarrollado por: Javier Eduardo Pardo Moreno
    ''' Fecha:            Febrero 01/2016
    ''' ID del cambio:    JEPM20160201
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaComboMotivosExclusion As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboMotivosExclusion As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboMotivosExclusion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboMotivosExclusion = value
            MyBase.CambioItem("ListaComboMotivosExclusion")
        End Set
    End Property
#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"
    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetallePorDefectoSync()

        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            Dim objRet As LoadOperation(Of EspeciesExcluidasDetalles)

            objRet = Await dcProxy.Load(dcProxy.ConsultarEspeciesExcluidasDetallesPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' <param name="pstrFormato">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal pstrFormato As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As MaestrosCFDomainContext = inicializarProxyMaestros()
        Dim objRet As LoadOperation(Of EspeciesExcluidasDetalles)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.EspeciesExcluidasDetalles Is Nothing Then
                mdcProxy.EspeciesExcluidasDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarEspeciesExcluidasDetallesSyncQuery(pstrFormato, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle del formato, pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle del formato.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle del formato seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
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
            If Not _HabilitarEliminar Then
                If IsNothing(_ListaDetalle) Then
                    strMsg = String.Format("{0}{1} + El formato debe tener por lo menos un una especie y un motivo de exclusión asociado.", strMsg, vbCrLf)
                ElseIf _ListaDetalle.Count = 0 Then
                    strMsg = String.Format("{0}{1} + El formato debe tener por lo menos un una especie y un motivo de exclusión asociado.", strMsg, vbCrLf)
                End If
            End If

            'Validar que se haya asignado una especie en el detalle
            If _ListaDetalle IsNot Nothing Then
                Dim intCantidadDetallesSinAsignar = (From item In _ListaDetalle Select item.strIDEspecie Where strIDEspecie = "").Count

                If intCantidadDetallesSinAsignar > 0 Then
                    strMsg = String.Format("{0}{1} + Debe asisgnar correctamente las especies en el detalle.", strMsg, vbCrLf)
                End If
            End If

            'Validar que se haya asignado motivos de exclusión
            If _ListaDetalle IsNot Nothing Then
                Dim intCantidadDetallesSinAsignar = (From item In _ListaDetalle Select item.strExclusionFormato Where strExclusionFormato = "").Count

                If intCantidadDetallesSinAsignar > 0 Then
                    strMsg = String.Format("{0}{1} + Debe asisgnar correctamente los motivos de exclusión de cada especie.", strMsg, vbCrLf)
                End If
            End If

            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que no existan especies repetidas en el detalle
            'ID caso de prueba:  CP016
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If _ListaDetalle IsNot Nothing Then

                Dim intCantidadDetalles As Integer = _ListaDetalle.Count
                Dim ValidarDetallesRepetidos = (From item In _ListaDetalle Select item.strIDEspecie.Trim()).Distinct '.OrderBy(Function(x As EspeciesExcluidasDetalles) x.lngIDComitente) 
                Dim intCantidadDetallesRepetidos As Integer = ValidarDetallesRepetidos.Count

                If intCantidadDetalles <> intCantidadDetallesRepetidos Then
                    strMsg = String.Format("{0}{1} + No se permite asociar la especie más de una vez al mismo formato.", strMsg, vbCrLf)
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

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' ID caso de prueba:  CP007
    ''' </summary>
    Public Sub IngresarDetalle()
        Try
            'Validar que exista un formato seleccionado en el encabezado
            If Not IsNothing(EncabezadoSeleccionado.strFormato) Then
                Dim objNvoDetalle As New CFMaestros.EspeciesExcluidasDetalles
                Dim objNuevaLista As New List(Of CFMaestros.EspeciesExcluidasDetalles)

                Program.CopiarObjeto(Of CFMaestros.EspeciesExcluidasDetalles)(mobjDetallePorDefecto, objNvoDetalle)

                objNvoDetalle.intIDEspecieExcluida = -New Random().Next(0, 1000000)
                objNvoDetalle.strFormato = EncabezadoSeleccionado.strFormato

                If IsNothing(ListaDetalle) Then
                    '' ''If Not Await LlenarComboValor() Then
                    '' ''    Exit Sub
                    '' ''End If
                    ListaDetalle = New List(Of CFMaestros.EspeciesExcluidasDetalles)
                End If

                objNuevaLista = ListaDetalle
                objNuevaLista.Add(objNvoDetalle)
                ListaDetalle = objNuevaLista
                DetalleSeleccionado = _ListaDetalle.First

                MyBase.CambioItem("ListaDetalle")
                MyBase.CambioItem("ListaDetallesPaginada")
                MyBase.CambioItem("DetalleSeleccionado")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la información del formato para poder agregar un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

            ' ''EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle", Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Try

            If Not IsNothing(ListaDetalle) Then
                'TODO
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle del formato", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' ID caso de prueba:  CP007
    ''' </summary>
    Public Sub BorrarDetalle()
        Try
            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intIDEspecieExcluida = _DetalleSeleccionado.intIDEspecieExcluida).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDEspecieExcluida = _DetalleSeleccionado.intIDEspecieExcluida).First)
                End If

                Dim objNuevaListaDetalle As New List(Of EspeciesExcluidasDetalles)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDEspecieExcluida = _DetalleSeleccionado.intIDEspecieExcluida).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDEspecieExcluida = _DetalleSeleccionado.intIDEspecieExcluida).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle del formato", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _DetalleSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        Try
            'If Editando Then
            '    Select Case e.PropertyName
            '        Case "strIDEspecie"
            '            _mlogBuscarClienteEncabezado = False
            '            _mLogBuscarClienteDetalle = True
            '            If Not String.IsNullOrEmpty(CStr(_DetalleSeleccionado.lngIDComitente)) And _mLogBuscarClienteDetalle Then
            '                If _DetalleSeleccionado.lngIDComitente <> "0" Then buscarComitente(CStr(_DetalleSeleccionado.lngIDComitente))
            '            End If
            '    End Select
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la lista de Motivos de Exclusion desde el sp cargar combos con la categoría EXCLUSION_F, para luego ser filtrados según el formato seleccionado.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Javier Eduardo Pardo Moreno - Asesorado por Walter Sierra.
    ''' Fecha            : Febrero 02/2016
    ''' Pruebas CB       : Javier Eduardo Pardo Moreno - Febrero 02/2016 - Resultado OK
    ''' ID del cambio    : JEPM20160202 
    ''' </history>
    Private Async Function CargarMotivosExclusion() As Task
        Try

            Dim A2VM As New A2UtilsViewModel

            'Cargar de la caché en caso que ya haya sido cargada
            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Where(Function(item) item.Key.StartsWith("EXCLUSION_F")).Count() > 0 Then

                Dim objListaMotivos = CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))) _
                                            .Where(Function(item) item.Key.StartsWith("EXCLUSION_F")).Select(Function(items) items.Value).ToList
                Dim objItemsListaMotivos As New List(Of OYDUtilidades.ItemCombo)
                objListaMotivos.ForEach(Sub(lista) objItemsListaMotivos.AddRange(lista))
                ListaComboMotivosExclusion = objItemsListaMotivos
            Else
                'Consultar directamente de la base de datos
                Dim objListaMotivos = Await A2VM.CargarCombosAsync("EXCLUSION_F", "")
                ListaComboMotivosExclusion = objListaMotivos.ToList
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los motivos de exclusión", _
             Me.ToString(), "CargarMotivosExclusion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Filtra los motivos de exclusión de acuerdo al Formato seleccionado. Se discrimina por encabezado y por búsqueda avanzada, según de donde se llame.
    ''' </summary>
    ''' <param name="strFormatoSeleccionado">Formato que el usuario elige en el combo</param>
    ''' <param name="strMetodo">"Forma" o "cb", según de donde se llame</param>
    ''' <remarks></remarks>
    Private Sub FiltrarMotivosExclusion(ByVal strFormatoSeleccionado As String, ByVal strMetodo As String)
        Try

            If Not IsNothing(ListaComboMotivosExclusion) Then

                'Esta categoría se conforma por el prefijo EXCLUSION_F y la unión con el número del formato. 
                'Se deben crear tópicos con este prefijo para que se mantenga el estandar
                Dim strCategoria As String = "EXCLUSION_F" & strFormatoSeleccionado

                Select Case strMetodo
                    Case "Forma"

                        Dim ListaMotivosExclusionSeleccionados As New List(Of String)

                        'JEPM20160204 Almacenar los valores del grid para que al refrescar se vuelvan a asignar, ya que estos se pierden por actualizar el itemsource
                        If Not IsNothing(ListaDetalle) Then
                            For Each item As EspeciesExcluidasDetalles In ListaDetalle
                                ListaMotivosExclusionSeleccionados.Add(item.strExclusionFormato)
                            Next
                        End If

                        'Limpiar valor anterior
                        ListaComboMotivosExclusion_Filtrada = Nothing

                        ListaComboMotivosExclusion_Filtrada = ListaComboMotivosExclusion.Where(Function(i) i.Categoria = strCategoria).ToList

                        'Volver a asignar los valores 
                        If Not IsNothing(ListaDetalle) Then
                            Dim intContador As Int16 = 0
                            For Each item As EspeciesExcluidasDetalles In ListaDetalle
                                item.strExclusionFormato = ListaMotivosExclusionSeleccionados(intContador)
                                intContador = intContador + 1
                            Next

                            MyBase.CambioItem("ListaDetalle")
                            MyBase.CambioItem("ListaDetallePaginada")
                        End If

                    Case "cb"
                        'Limpiar valor anterior
                        ListaComboMotivosExclusion_Filtrada_Busqueda = Nothing

                        ListaComboMotivosExclusion_Filtrada_Busqueda = ListaComboMotivosExclusion.Where(Function(i) i.Categoria = strCategoria).ToList

                    Case Else

                End Select

               
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al fitrar los motivos de exclusión", _
             Me.ToString(), "FiltrarMotivosExclusion", Application.Current.ToString(), Program.Maquina, ex)
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
''' 
Public Class CamposBusquedaEspeciesExcluidas
    Implements INotifyPropertyChanged

    Private _strFormato As String
    Public Property strFormato() As String
        Get
            Return _strFormato
        End Get
        Set(ByVal value As String)
            _strFormato = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strFormato"))
        End Set
    End Property

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

    Private _strExclusionFormato As String
    Public Property strExclusionFormato() As String
        Get
            Return _strExclusionFormato
        End Get
        Set(ByVal value As String)
            _strExclusionFormato = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strExclusionFormato"))
        End Set
    End Property

    Private _strEspecie As String
    Public Property strEspecie() As String
        Get
            Return _strEspecie
        End Get
        Set(ByVal value As String)
            _strEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class