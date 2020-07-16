Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

''' <summary>
''' ViewModel para la pantalla Precios de Especies perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok ''' 
''' <remarks></remarks>

Public Class PreciosEspeciesViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As PreciosEspecies
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyActualizar As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As PreciosEspecies
    Private mlogEsAccion As Boolean
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Febrero 21/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
                ObtenerValoresCombos()


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

    Private _DiccionarioCombosOYD As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosOYD() As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosOYD
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosOYD = value
            MyBase.CambioItem("DiccionarioCombosOYD")
        End Set
    End Property

    Private _IndicadorFSPorDefecto As OYDUtilidades.ItemCombo
    Public Property IndicadorFSPorDefecto() As OYDUtilidades.ItemCombo
        Get
            Return _IndicadorFSPorDefecto
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _IndicadorFSPorDefecto = value
            MyBase.CambioItem("IndicadorFSPorDefecto")
        End Set
    End Property

    ''' <summary>
    ''' Lista de PreciosEspecies que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of PreciosEspecies)
    Public Property ListaEncabezado() As EntitySet(Of PreciosEspecies)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of PreciosEspecies))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de PreciosEspecies para navegar sobre el grid con paginación
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
    _
    ''' <summary>
    ''' Elemento de la lista de PreciosEspecies que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As PreciosEspecies
    Public Property EncabezadoSeleccionado() As PreciosEspecies
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As PreciosEspecies)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                mlogEsAccion = EncabezadoSeleccionado.logEsAccion
                HabilitarCamposXClase()
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaPreciosEspecies
    Public Property cb() As CamposBusquedaPreciosEspecies
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaPreciosEspecies)
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

    Private _HabilitarEncabezadoEspecieXClase As Visibility = Visibility.Collapsed
    Public Property HabilitarEncabezadoEspecieXClase() As Visibility
        Get
            Return _HabilitarEncabezadoEspecieXClase
        End Get
        Set(ByVal value As Visibility)
            _HabilitarEncabezadoEspecieXClase = value
            MyBase.CambioItem("HabilitarEncabezadoEspecieXClase")
        End Set
    End Property

    Private _HabilitarCodPreciosEspecies As Boolean = False
    Public Property HabilitarCodPreciosEspecies() As Boolean
        Get
            Return _HabilitarCodPreciosEspecies
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodPreciosEspecies = value
            MyBase.CambioItem("HabilitarCodPreciosEspecies")
        End Set
    End Property

    Private _HabilitarCampos As Boolean = False
    Public Property HabilitarCampos() As Boolean
        Get
            Return _HabilitarCampos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCampos = value
            MyBase.CambioItem("HabilitarCampos")
        End Set
    End Property

    Private _EstadoVisibleBuscador As Visibility = Visibility.Collapsed
    Public Property EstadoVisibleBuscador() As Visibility
        Get
            Return _EstadoVisibleBuscador
        End Get
        Set(ByVal value As Visibility)
            _EstadoVisibleBuscador = value
            MyBase.CambioItem("EstadoVisibleBuscador")
        End Set
    End Property

    Private _EstadoVisibleEspecie As Visibility = Visibility.Collapsed
    Public Property EstadoVisibleEspecie() As Visibility
        Get
            Return _EstadoVisibleEspecie
        End Get
        Set(ByVal value As Visibility)
            _EstadoVisibleEspecie = value
            MyBase.CambioItem("EstadoVisibleEspecie")
        End Set
    End Property

    Private _VisibilidadIdentityPreciosEspecies As Visibility = Visibility.Collapsed
    Public Property VisibilidadIdentityPreciosEspecies() As Visibility
        Get
            Return _VisibilidadIdentityPreciosEspecies
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadIdentityPreciosEspecies = value
            MyBase.CambioItem("VisibilidadIdentityPreciosEspecies")
        End Set
    End Property

    Private _HabilitarSeleccionISIN As Boolean = True
    ''' <summary>
    ''' Permite seleccionar el ISIN en el buscador de especies, si lo permite debe llenar las faciales de la especie.
    ''' </summary>
    Public Property HabilitarSeleccionISIN As Boolean
        Get
            Return _HabilitarSeleccionISIN
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionISIN = value
            MyBase.CambioItem("HabilitarSeleccionISIN")
        End Set
    End Property

    Private WithEvents _NemotecnicoSelected As PreciosEspecies
    Public Property NemotecnicoSelected() As PreciosEspecies
        Get
            Return _NemotecnicoSelected
        End Get
        Set(ByVal value As PreciosEspecies)
            _NemotecnicoSelected = value
        End Set
    End Property

    Private _ListaComboIsin As New ObservableCollection(Of CamposComboIsin)
    Public Property ListaComboIsin() As ObservableCollection(Of CamposComboIsin)
        Get
            Return _ListaComboIsin
        End Get
        Set(ByVal value As ObservableCollection(Of CamposComboIsin))
            _ListaComboIsin = value
            MyBase.CambioItem("ListaComboIsin")
        End Set
    End Property

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        'If e.PropertyName = "strIDEspecie" Then
        'End If
    End Sub

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            If Not IsNothing(EncabezadoSeleccionado) Then
                EncabezadoSeleccionado.strISIN = String.Empty
            End If
            If Not IsNothing(cb) Then
                cb.strISIN = String.Empty
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _EspecieBuscar As String
    Public Property EspecieBuscar() As String
        Get
            Return _EspecieBuscar
        End Get
        Set(ByVal value As String)
            _EspecieBuscar = value
            MyBase.CambioItem("EspecieBuscar")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub ObtenerValoresCombos()
        Try
            Dim objDiccionario As New Dictionary(Of String, ObservableCollection(Of OYDUtilidades.ItemCombo))

            If DiccionarioCombosOYD IsNot Nothing Then

                Dim listaIndicador = (From item In DiccionarioCombosOYD("INDICADOR")
                                      Select New OYDUtilidades.ItemCombo With {.ID = item.ID,
                                                                                .Retorno = item.Retorno,
                                                                                .Descripcion = item.Descripcion})

                If listaIndicador IsNot Nothing Then
                    If listaIndicador.Count > 0 Then
                        Dim RegistroFS = (From item In listaIndicador.ToList() Where item.Descripcion = "FS").SingleOrDefault()

                        If RegistroFS IsNot Nothing Then
                            IndicadorFSPorDefecto = RegistroFS
                        End If
                    End If
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_7, Id_10, Id_11
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New PreciosEspecies

        Try
            If mdcProxyActualizar.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of PreciosEspecies)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.lngID = -1
                objNvoEncabezado.strIDEspecie = String.Empty
                objNvoEncabezado.strISIN = ""
                objNvoEncabezado.dtmFechaArchivo = Date.Now()
                objNvoEncabezado.dtmEmision = Nothing
                objNvoEncabezado.dtmVencimiento = Nothing
                objNvoEncabezado.strMoneda = String.Empty
                objNvoEncabezado.strModalidad = String.Empty
                'objNvoEncabezado.dblSpread = 0
                objNvoEncabezado.strTasaRef = String.Empty
                objNvoEncabezado.dblPrecio = 0
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = obtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado
            HabilitarEncabezado = True
            HabilitarCodPreciosEspecies = True
            HabilitarCampos = True

            BorrarEspecie = True
            BorrarEspecie = False

            EstadoVisibleBuscador = Visibility.Visible
            EstadoVisibleEspecie = Visibility.Collapsed
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
    ''' ID caso de prueba: Id_3
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
            HabilitarCampos = False
            HabilitarEncabezado = False
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
    ''' <history>
    ''' ID caso de prueba: Id_4
    ''' Descripción      : Se agrega el parámetro pstrTipoInversion. Desarrollo autorizado por Jorge Arango.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            Dim strMsg As String = String.Empty

            If cb.strIDEspecie <> String.Empty Then
                If mlogEsAccion Then
                    cb.dtmEmision = Nothing
                    cb.dtmVencimiento = Nothing
                    cb.strModalidad = String.Empty
                    cb.dblSpread = 0
                    cb.strTasaRef = String.Empty
                    cb.strTipoInversion = String.Empty
                End If
            End If

            'Validar que ingresó algo en los campos de búsqueda
            If Not IsNothing(cb.strIDEspecie) Or Not IsNothing(cb.strISIN) Or
                Not IsNothing(cb.strModalidad) Or Not IsNothing(cb.strMoneda) Or
                Not IsNothing(cb.strTasaRef) Or Not IsNothing(cb.strTipoInversion) Then
                Await ConsultarEncabezado(False, String.Empty, 0, cb.strIDEspecie, cb.strISIN, 0, cb.dtmFechaArchivo, cb.dtmEmision, cb.dtmVencimiento, cb.strModalidad, cb.strMoneda, cb.dblSpread, cb.strTasaRef, cb.dblPrecio, cb.strTipoInversion)
            End If
            HabilitarCampos = False
            HabilitarEncabezado = False
            EstadoVisibleBuscador = Visibility.Collapsed
            EstadoVisibleEspecie = Visibility.Visible
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_6, Id_9
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub ActualizarRegistro()

        Dim intNroOcurrencias As Integer
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty
            IsBusy = True

            If ValidarRegistro() Then
                ' Incializar los mensajes de validación
                _EncabezadoSeleccionado.strMsgValidacion = String.Empty
                ' Validar si el registro ya existe en la lista
                intNroOcurrencias = (From e In ListaEncabezado Where e.lngID = _EncabezadoSeleccionado.lngID Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

                ' Enviar cambios al servidor
                Program.VerificarCambiosProxyServidor(mdcProxyActualizar)

                If Double.IsNaN(_EncabezadoSeleccionado.dblPrecio) Then
                    _EncabezadoSeleccionado.dblPrecio = 0
                End If

                If Double.IsNaN(_EncabezadoSeleccionado.dblPrecioLimpio) Then
                    _EncabezadoSeleccionado.dblPrecioLimpio = 0
                End If

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
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxyActualizar.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                _EncabezadoSeleccionado.strUsuario = Program.Usuario

                'Dim logEsAccion As Boolean
                'logEsAccion = _EncabezadoSeleccionado.logEsAccion

                mobjEncabezadoAnterior = obtenerRegistroAnterior()

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarEncabezado = True
                HabilitarCodPreciosEspecies = False

                If String.IsNullOrEmpty(EncabezadoSeleccionado.strISIN) Then
                    HabilitarCampos = True
                Else
                    HabilitarCampos = False
                End If
                EstadoVisibleBuscador = Visibility.Collapsed
                EstadoVisibleEspecie = Visibility.Visible

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de editar registro.", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxyActualizar.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarCodPreciosEspecies = False
                HabilitarCampos = False
                EstadoVisibleBuscador = Visibility.Collapsed
                EstadoVisibleEspecie = Visibility.Visible
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: Id_8
    ''' Descripción      : Se agrega el parámetro pstrTipoInversion. Desarrollo autorizado por Jorge Arango.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxyActualizar.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el Precio seleccionado. ¿Confirma el borrado de este Precio?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
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

                    mobjEncabezadoAnterior = obtenerRegistroAnterior()

                    If mdcProxyActualizar.PreciosEspecies.Where(Function(i) i.lngID = EncabezadoSeleccionado.lngID).Count > 0 Then
                        mdcProxyActualizar.PreciosEspecies.Remove(mdcProxyActualizar.PreciosEspecies.Where(Function(i) i.lngID = EncabezadoSeleccionado.lngID).First)
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
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaPreciosEspecies
            objCB.strIDEspecie = String.Empty
            objCB.strISIN = String.Empty
            objCB.dtmFechaArchivo = Nothing
            objCB.dtmEmision = Nothing
            objCB.dtmVencimiento = Nothing
            objCB.strMoneda = String.Empty
            objCB.strModalidad = String.Empty
            objCB.dblSpread = 0
            objCB.strTasaRef = String.Empty
            objCB.dblPrecio = 0
            objCB.strTipoInversion = String.Empty
            cb = objCB

            If Not IsNothing(EncabezadoSeleccionado) Then
                EncabezadoSeleccionado.strISIN = String.Empty
            End If

            If Not IsNothing(HabilitarEncabezadoEspecieXClase) Then
                HabilitarEncabezadoEspecieXClase = Visibility.Visible
            End If

            HabilitarCampos = True
            BorrarEspecie = True
            BorrarEspecie = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    Private Function obtenerRegistroAnterior() As PreciosEspecies
        Dim objEncabezado As PreciosEspecies = New PreciosEspecies

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of PreciosEspecies)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.lngID = _EncabezadoSeleccionado.lngID
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <history>
    ''' ID caso de prueba: Id_5
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If IsNothing(_EncabezadoSeleccionado) Then
                ''Valida la especie
                'If String.IsNullOrEmpty(EncabezadoSeleccionado.strIDEspecie) Then
                '    strMsg = String.Format("{0}{1} + La especie es un campo requerido.", strMsg, vbCrLf)
                'End If

                ''Valida la fecha de precio
                'If IsNothing(_EncabezadoSeleccionado.dtmFechaArchivo) Then
                '    strMsg = String.Format("{0}{1} + La fecha de precio es un campo requerido.", strMsg, vbCrLf)
                'End If


                ''Valida la moneda
                'If String.IsNullOrEmpty(_EncabezadoSeleccionado.strMoneda) Then
                '    strMsg = String.Format("{0}{1} + La moneda es un campo requerido.", strMsg, vbCrLf)
                'End If

                'If Not (mlogEsAccion) Then

                '    'Valida la modalidad
                '    If String.IsNullOrEmpty(_EncabezadoSeleccionado.strModalidad) Then
                '        strMsg = String.Format("{0}{1} + La modalidad es un campo requerido.", strMsg, vbCrLf)
                '    End If

                '    'Valida la fecha de Emision
                '    If IsNothing(_EncabezadoSeleccionado.dtmEmision) Then
                '        strMsg = String.Format("{0}{1} + La fecha de emisión es un campo requerido.", strMsg, vbCrLf)
                '    End If

                '    'Valida la fecha de Vencimiento
                '    If IsNothing(_EncabezadoSeleccionado.dtmVencimiento) Then
                '        strMsg = String.Format("{0}{1} + La fecha de vencimiento es un campo requerido.", strMsg, vbCrLf)
                '    End If

                '    'Valida la fecha de Emision
                '    If (_EncabezadoSeleccionado.dtmEmision) > (_EncabezadoSeleccionado.dtmVencimiento) Then
                '        strMsg = String.Format("{0}{1} + La fecha de emisión no puede ser mayor que la fecha de vencimiento.", strMsg, vbCrLf)
                '    End If

                '    'Valida la tasa
                '    If IsNothing(_EncabezadoSeleccionado.dblSpread) Then
                '        strMsg = String.Format("{0}{1} + La tasa es un campo requerido.", strMsg, vbCrLf)
                '    End If

                'End If


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

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ActualizarRegistro
    ''' </summary>
    ''' <history>
    ''' Modificado por   : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se ajusta el incremento a la variable numeroSeleccionadas pues no se esta realizando correctamente.
    ''' Fecha            : Septiembre 26/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Septiembre 26/2013 - Resultado Ok 
    ''' </history>
    Private Sub HabilitarCamposXClase()
        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- HABILITAR CAMPOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If mlogEsAccion Then
                HabilitarEncabezadoEspecieXClase = Visibility.Collapsed
            Else
                HabilitarEncabezadoEspecieXClase = Visibility.Visible
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los campos por clase de especie.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
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
            'Dim EncabezadoGuardado As PreciosEspecies = New PreciosEspecies() 'YAPP20160708

            'EncabezadoGuardado = _EncabezadoSeleccionado 'YAPP20160708

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
                'JP20181217 SE COMENTA PORQUE CUANDO EL SP ME DEVUELVE VALIDACIONES ME DEJA LOS CAMPOS QUE YA HABIA CAMBIADO EN NULL
                ' Marcar los cambios como rechazados
                'mdcProxyActualizar.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezado = False
                HabilitarCodPreciosEspecies = False
                HabilitarCampos = False
                HabilitarEncabezado = False

                MyBase.TerminoSubmitChanges(So)

                Editando = False
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

                'EncabezadoSeleccionado = EncabezadoGuardado 'YAPP20160708


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
        Dim objRet As LoadOperation(Of PreciosEspecies)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()
            mdcProxyUtilidad01 = inicializarProxyUtilidadesOYD()

            objRet = Await dcProxy.Load(dcProxy.ConsultarPreciosEspeciesPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de PreciosEspecies
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    ''' <history>
    ''' ID caso de prueba: Id_1
    ''' Descripción      : Se agrega el parámetro pstrTipoInversion. Desarrollo autorizado por Jorge Arango.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Junio 20/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Junio 20/2014 - Resultado Ok 
    ''' </history>
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal plngID As Integer = -1,
                                               Optional ByVal pstrIDEspecie As String = "",
                                               Optional ByVal pstrISIN As String = "",
                                               Optional ByVal pintNroEmision As Integer = 0,
                                               Optional ByVal pdtmFechaArchivo As System.Nullable(Of System.DateTime) = Nothing,
                                               Optional ByVal pdtmEmision As System.Nullable(Of System.DateTime) = Nothing,
                                               Optional ByVal pdtmVencimiento As System.Nullable(Of System.DateTime) = Nothing,
                                               Optional ByVal pstrModalidad As String = "",
                                               Optional ByVal pstrMoneda As String = "",
                                               Optional ByVal pdblSpread As Double = 0,
                                               Optional ByVal pstrTasaRef As String = "",
                                               Optional ByVal pdblPrecio As Double = 0,
                                               Optional ByVal pstrTipoInversion As String = "",
                                               Optional ByVal pdblPrecioLimpio As Double = 0,
                                               Optional ByVal pstrProveedor As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of PreciosEspecies)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxyActualizar Is Nothing Then
                mdcProxyActualizar = inicializarProxyCalculosFinancieros()
            End If

            mdcProxyActualizar.PreciosEspecies.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.FiltrarPreciosEspeciesSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxyActualizar.Load(mdcProxyActualizar.ConsultarPreciosEspeciesSyncQuery(plngID:=plngID, pstrIDEspecie:=pstrIDEspecie,
                                                                                                            pstrISIN:=pstrISIN, pintNroEmision:=pintNroEmision,
                                                                                                            pdtmFechaArchivo:=pdtmFechaArchivo, pdtmEmision:=pdtmEmision,
                                                                                                            pdtmVencimiento:=pdtmVencimiento, pstrModalidad:=pstrModalidad,
                                                                                                            pstrMoneda:=pstrMoneda, pdblSpread:=pdblSpread,
                                                                                                            pstrTasaRef:=pstrTasaRef, pdblPrecio:=pdblPrecio,
                                                                                                            pstrTipoInversion:=pstrTipoInversion,
                                                                                                            pstrUsuario:=Program.Usuario,
                                                                                                            pstrInfoConexion:=Program.HashConexion,
                                                                                                            pdblPrecioLimpio:=pdblPrecioLimpio,
                                                                                                            pstrProveedor:=pstrProveedor)).AsTask()

            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString, "ConsultarEncabezado", objRet.Error)
                        'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxyActualizar.PreciosEspecies

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

            EstadoVisibleBuscador = Visibility.Collapsed
            EstadoVisibleEspecie = Visibility.Visible

            logResultado = True
        Catch ex As Exception
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la recepción de la lista de PreciosEspecies ", Me.ToString, "ConsultarEncabezado", ex)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de PreciosEspecies ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"
    ''' <summary>
    ''' Actualizar el nemotécnico de la orden con los datos del nemotecnico recibido como parámetro
    ''' </summary>
    ''' <param name="pobjNemotecnico">Nemotécnico enviado como parámetro</param>
    Public Sub TraerCaracteristicasNemotecnico(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies)
        Try
            If Editando Then

                mlogEsAccion = pobjNemotecnico.EsAccion
                Me.EncabezadoSeleccionado.strIDEspecie = pobjNemotecnico.Nemotecnico
                Me.EncabezadoSeleccionado.strISIN = pobjNemotecnico.ISIN
                Me.EncabezadoSeleccionado.logEsAccion = pobjNemotecnico.EsAccion
                Me.EncabezadoSeleccionado.intNroEmision = 0


                If pobjNemotecnico.TipoTasa = "TASA VARIABLE" Then
                    If pobjNemotecnico.PuntosIndicador IsNot Nothing Then
                        Me.EncabezadoSeleccionado.dblSpread = CDbl(pobjNemotecnico.PuntosIndicador)
                    End If
                Else
                    If pobjNemotecnico.TasaFacial IsNot Nothing Then
                        Me.EncabezadoSeleccionado.dblSpread = CDbl(pobjNemotecnico.TasaFacial)
                    End If
                End If

                If Not mlogEsAccion Then
                    Me.EncabezadoSeleccionado.dtmEmision = pobjNemotecnico.Emision
                    Me.EncabezadoSeleccionado.dtmVencimiento = pobjNemotecnico.Vencimiento
                    Me.EncabezadoSeleccionado.strModalidad = pobjNemotecnico.CodModalidad

                    If pobjNemotecnico.TipoTasa <> "TASA VARIABLE" And String.IsNullOrEmpty(pobjNemotecnico.Indicador) Then
                        If Not IsNothing(IndicadorFSPorDefecto) Then
                            If Not IsNothing(IndicadorFSPorDefecto.Descripcion) Then
                                Me.EncabezadoSeleccionado.strTasaRef = IndicadorFSPorDefecto.Descripcion
                            End If
                        End If

                    Else
                        If Not IsNothing(pobjNemotecnico.Indicador) Then
                            Me.EncabezadoSeleccionado.strTasaRef = pobjNemotecnico.Indicador 'IIf(pobjNemotecnico.IdIndicador Is Nothing, "", pobjNemotecnico.IdIndicador).ToString ' Se hace por conversión de tipo de datos
                        End If
                    End If


                End If
                HabilitarCamposXClase()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las características del Nemotecnico.", Me.ToString(), "TraerCaracteristicasNemotecnico", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub TraerNemotecnico(ByVal pstrIDEspecie As String)
        Try
            If Editando Then
                Me.EncabezadoSeleccionado.strIDEspecie = pstrIDEspecie
                Me.EncabezadoSeleccionado.strISIN = String.Empty
                Me.EncabezadoSeleccionado.intNroEmision = 0
                If Not mlogEsAccion Then
                    Me.EncabezadoSeleccionado.dtmEmision = Nothing
                    Me.EncabezadoSeleccionado.dtmVencimiento = Nothing
                    Me.EncabezadoSeleccionado.strModalidad = String.Empty
                    Me.EncabezadoSeleccionado.strTasaRef = String.Empty
                End If
                HabilitarCamposXClase()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las características del Nemotecnico.", Me.ToString(), "TraerCaracteristicasNemotecnico", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub TraerCaracteristicasNemotecnicoBusquedaAvanzada(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies)
        Try
            mlogEsAccion = pobjNemotecnico.EsAccion
            Me.cb.strIDEspecie = pobjNemotecnico.Nemotecnico
            Me.cb.strISIN = pobjNemotecnico.ISIN

            If pobjNemotecnico.TasaFacial IsNot Nothing Then
                Me.cb.dblSpread = CDbl(pobjNemotecnico.TasaFacial)
            End If

            'Me.cb.intNroEmision = 0
            If Not mlogEsAccion Then
                Me.cb.dtmEmision = pobjNemotecnico.Emision
                Me.cb.dtmVencimiento = pobjNemotecnico.Vencimiento
                Me.cb.strModalidad = pobjNemotecnico.CodModalidad
                Me.cb.strTasaRef = pobjNemotecnico.Indicador 'IIf(pobjNemotecnico.IdIndicador Is Nothing, "", pobjNemotecnico.IdIndicador).ToString ' Se hace por conversión de tipo de datos

            End If
            HabilitarCamposXClase()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las características del Nemotecnico.", Me.ToString(), "TraerCaracteristicasNemotecnico", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaPreciosEspecies
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

    Private _strISIN As String
    Public Property strISIN() As String
        Get
            Return _strISIN
        End Get
        Set(ByVal value As String)
            _strISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strISIN"))
        End Set
    End Property

    Private _dtmFechaArchivo As System.Nullable(Of System.DateTime)
    Public Property dtmFechaArchivo() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaArchivo
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaArchivo"))
        End Set
    End Property

    Private _dtmEmision As System.Nullable(Of System.DateTime)
    Public Property dtmEmision() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmEmision
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmEmision"))
        End Set
    End Property

    Private _dtmVencimiento As System.Nullable(Of System.DateTime)
    Public Property dtmVencimiento() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmVencimiento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmVencimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmVencimiento"))
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

    Private _dblSpread As Double
    Public Property dblSpread() As Double
        Get
            Return _dblSpread
        End Get
        Set(ByVal value As Double)
            _dblSpread = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblSpread"))
        End Set
    End Property

    Private _strTasaRef As String
    Public Property strTasaRef() As String
        Get
            Return _strTasaRef
        End Get
        Set(ByVal value As String)
            _strTasaRef = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTasaRef"))
        End Set
    End Property


    Private _dblPrecio As Double
    Public Property dblPrecio() As Double
        Get
            Return _dblPrecio
        End Get
        Set(ByVal value As Double)
            _dblPrecio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblPrecio"))
        End Set
    End Property
    '<Display(Name:="Fecha Precio")> _
    'Public Property dtmFechaArchivo As Date

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

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

Public Class CamposComboIsin
    Implements INotifyPropertyChanged

    Private _ID As String
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property


    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class