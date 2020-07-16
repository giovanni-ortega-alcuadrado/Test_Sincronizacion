Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Public Class EntidadesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As Entidades
    Private mdcProxy As MaestrosCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

    Private Const STR_EXPRESION_EMAIL_VALIDO As String = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
    Private mlogTerminoSubmitChanges As Boolean = False
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As Entidades

    Private ListaComboCalificacionesLarga As List(Of OYDUtilidades.ItemCombo)
    Private ListaComboCalificacionesCorta As List(Of OYDUtilidades.ItemCombo)

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
    ''' ID caso de prueba: Id_1
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 7/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 7/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00010
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Creado por       : Javier Eduardo Pardo Moreno
    ''' Descripción      : Consultar las calificaciones de tipo L y tipo C según la calificadora seleccionada
    ''' Fecha            : Octubre 06/2015
    ''' Pruebas CB       : Javier Eduardo Pardo Moreno - Octubre 06/2015 - Resultado OK
    ''' ID del cambio    : JEPM20151007 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New MaestrosCFDomainContext
            Else
                mdcProxy = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            End If
        Catch ex As Exception

        End Try

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefectoSync()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

                'Cargar lista de Calificaciones JEPM20151007
                CargarCalificacionesInversiones()

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
    ''' Lista de Entidades que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of Entidades)
    Public Property ListaEncabezado() As EntitySet(Of Entidades)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of Entidades))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Entidades para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Entidades que se encuentra seleccionado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Bloque de Validación cuando es Cartera colectiva, cambios solicitados por Jorge Arango.
    ''' Fecha            : Julio 16/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 16/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00011
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    Private WithEvents _EncabezadoSeleccionado As Entidades
    Public Property EncabezadoSeleccionado() As Entidades
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As Entidades)
            _EncabezadoSeleccionado = value
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property


    ''' <history>
    ''' Descripción      : Se realiza la lógica para habilitar y limpiar los campos lngIDGrupo, lngIDSubGrupo y logVigiladoSuper
    ''' Creado por       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha            : Mayo 20/2015
    ''' Pruebas CB       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) - Mayo 20/2015 - Resultado Ok 
    ''' </history>
    ''' ''' <history>
    ''' Descripción      : Se realiza la lógica para habilitar lngIDGrupo, lngIDSubGrupo y logVigiladoSuper sin importar si aplica a emisores, adicionalmente si se esta editando
    '''                    habilitar HabilitarEnrutamiento_Y_CodComisionista si es comisionista
    ''' Creado por       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha            : Junio 19/2015
    ''' Pruebas CB       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) - Junio 19/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se añaden condiciones segun el PropertyName para cambiar posición a campos relacionados con "Cartera colectiva", cambios solicitados por Jorge Arango.
    ''' Fecha            : Julio 16/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 16/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se elimina la lógica para ocultar y visualizar campos cuando es cartera colectiva y/o replicar a emisores.
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        If Not mlogTerminoSubmitChanges Then

            If e.PropertyName = "logEsComisionista" Then
                If EncabezadoSeleccionado.logEsComisionista And Editando Then
                    HabilitarEnrutamiento_Y_CodComisionista = True
                Else
                    HabilitarEnrutamiento_Y_CodComisionista = False
                    EncabezadoSeleccionado.logAplicaEnrutamiento = False
                    EncabezadoSeleccionado.intIDComisionista = Nothing
                End If
            ElseIf e.PropertyName = "logReplicarAEmisores" Then
                If EncabezadoSeleccionado.logReplicarAEmisores And Editando Then
                    HabilitarCodigoEmisor = True
                Else
                    HabilitarCodigoEmisor = False
                End If
            ElseIf e.PropertyName = "logCarteraColectiva" Then
                If EncabezadoSeleccionado.logCarteraColectiva And Editando Then
                    HabilitarSiEsCarteraColectiva = True
                Else
                    HabilitarSiEsCarteraColectiva = False
                    EncabezadoSeleccionado.intCodigoEntidadAdmin = Nothing
                    EncabezadoSeleccionado.strClaseInversion = Nothing
                    EncabezadoSeleccionado.strDescripcionClaseInversion = Nothing
                    EncabezadoSeleccionado.strTipoFondo = Nothing   'JEPM20160506
                End If
            End If

        End If

    End Sub

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaEntidades
    Public Property cb() As CamposBusquedaEntidades
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaEntidades)
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
    ''' <history>
    ''' Creado por       : Juan Esteban Restrepo Franco (Alcuadrado S.A.)
    ''' Descripción      : Se agrega la propiedad HabilitarDocumento que determina si el tipo y documento se puedem modificar
    ''' Fecha            : Junio 21/2018
    ''' Pruebas CB       : Juan Esteban Restrepo Franco (Alcuadrado S.A.) - Junio 06/2018 - Resultado Ok 
    ''' </history>
    Private _HabilitarDocumento As Boolean = False
    Public Property HabilitarDocumento() As Boolean
        Get
            Return _HabilitarDocumento
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDocumento = value
            MyBase.CambioItem("HabilitarDocumento")
        End Set
    End Property

    Private _HabilitarEnrutamiento_Y_CodComisionista As Boolean = False
    Public Property HabilitarEnrutamiento_Y_CodComisionista() As Boolean
        Get
            Return _HabilitarEnrutamiento_Y_CodComisionista
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEnrutamiento_Y_CodComisionista = value
            MyBase.CambioItem("HabilitarEnrutamiento_Y_CodComisionista")
        End Set
    End Property

    Private _HabilitarReplicarAEmisores As Boolean = False
    Public Property HabilitarReplicarAEmisores() As Boolean
        Get
            Return _HabilitarReplicarAEmisores
        End Get
        Set(ByVal value As Boolean)
            _HabilitarReplicarAEmisores = value
            MyBase.CambioItem("HabilitarReplicarAEmisores")
        End Set
    End Property

    Private _HabilitarReplicacionEsComisionista As Boolean = False
    Public Property HabilitarReplicacionEsComisionista() As Boolean
        Get
            Return _HabilitarReplicacionEsComisionista
        End Get
        Set(ByVal value As Boolean)
            _HabilitarReplicacionEsComisionista = value
            MyBase.CambioItem("HabilitarReplicacionEsComisionista")
        End Set
    End Property

    Private _HabilitarCodigoEmisor As Boolean = False
    Public Property HabilitarCodigoEmisor() As Boolean
        Get
            Return _HabilitarCodigoEmisor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigoEmisor = value
            MyBase.CambioItem("HabilitarCodigoEmisor")
        End Set
    End Property

    Private _HabilitarVigiladoSuperYGrupos As Boolean = False
    Public Property HabilitarVigiladoSuperYGrupos() As Boolean
        Get
            Return _HabilitarVigiladoSuperYGrupos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarVigiladoSuperYGrupos = value
            MyBase.CambioItem("HabilitarVigiladoSuperYGrupos")
        End Set
    End Property

    Private _HabilitarSiEsCarteraColectiva As Boolean = False
    Public Property HabilitarSiEsCarteraColectiva() As Boolean
        Get
            Return _HabilitarSiEsCarteraColectiva
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSiEsCarteraColectiva = value
            MyBase.CambioItem("HabilitarSiEsCarteraColectiva")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:      Lista de calificaciones inversiones para cargar combo Calificación Larga (Tipo L)
    ''' Desarrollado por: Javier Eduardo Pardo Moreno
    ''' Fecha:            Octubre 06/2015
    ''' ID del cambio:    JEPM20151006
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaComboCalificacionesLarga_Filtrada As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboCalificacionesLarga_Filtrada As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboCalificacionesLarga_Filtrada
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboCalificacionesLarga_Filtrada = value
            MyBase.CambioItem("ListaComboCalificacionesLarga_Filtrada")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:      Lista de calificaciones inversiones para cargar combo Calificación Corta (Tipo C)
    ''' Desarrollado por: Javier Eduardo Pardo Moreno
    ''' Fecha:            Octubre 06/2015
    ''' ID del cambio:    JEPM20151006
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaComboCalificacionesCorta_Filtrada As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboCalificacionesCorta_Filtrada As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboCalificacionesCorta_Filtrada
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboCalificacionesCorta_Filtrada = value
            MyBase.CambioItem("ListaComboCalificacionesCorta_Filtrada")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:      Lista de calificaciones calificadora para relacionar las calificadoras con las calificaciones
    ''' Desarrollado por: Javier Eduardo Pardo Moreno
    ''' Fecha:            Octubre 06/2015
    ''' ID del cambio:    JEPM20151006
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaCalificacionesCalificadora As List(Of CFMaestros.CalificacionesCalificadora)
    Public Property ListaCalificacionesCalificadora As List(Of CFMaestros.CalificacionesCalificadora)
        Get
            Return _ListaCalificacionesCalificadora
        End Get
        Set(ByVal value As List(Of CFMaestros.CalificacionesCalificadora))
            _ListaCalificacionesCalificadora = value
            MyBase.CambioItem("ListaCalificacionesCalificadora")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0006
    ''' Descripción      : Se agregan los valores a las propiedades logEsComisionista, strEMail, logAplicaEnrutamiento.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Noviembre 25/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Noviembre 25/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP0009
    ''' Descripción      : Se agregan las pripiedades intIDCalificacionInversionLarga, intIDCalificacionInversionCorta, strNombreCalificacionInversionLarga, strNombreCalificacionInversionCorta.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Marzo 10/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Marzo 10/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Se agregan los campos lngIDGrupo, lngIDSubGrupo y logVigiladoSuper
    ''' Creado por       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha            : Mayo 20/2015
    ''' Pruebas CB       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) - Mayo 20/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00014, CP00015
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: JEPM20160506
    ''' Descripción      : Nuevo campo strTipoFondo
    ''' Creado por       : Javier Eduardo Pardo Moreno (Alcuadrado S.A.)
    ''' Fecha            : Mayo 6/2016
    ''' Pruebas CB       : Javier Eduardo Pardo (Alcuadrado S.A.) - Mayo 6/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción:   Se agregan campos Calidad tributaria, Clasificacion del afiliado
    ''' Responsable:   Yessid Andrés Paniagua Pabón (Alcuadrado S.A.)
    ''' Fecha:         Julio 2016/18
    ''' ID del cambio: YAPP20160718
    ''' </history>

    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New Entidades

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of Entidades)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDEntidad = -1
                objNvoEncabezado.strTipoIdentificacion = String.Empty
                objNvoEncabezado.strDescripcionTipoIdentificacion = String.Empty
                objNvoEncabezado.strNroDocumento = String.Empty
                objNvoEncabezado.strNombre = String.Empty
                objNvoEncabezado.intIDTipoEntidad = 0
                objNvoEncabezado.strDescripcionTipoEntidad = String.Empty
                objNvoEncabezado.intIDCodigoCIIU = 0
                objNvoEncabezado.strDescripcionCodigoCIIU = String.Empty
                objNvoEncabezado.strTelefono = String.Empty
                objNvoEncabezado.strDireccion = String.Empty
                objNvoEncabezado.intIDCalificacionInversionLarga = 0
                objNvoEncabezado.intIDCalificacionInversionCorta = 0
                objNvoEncabezado.strNombreCalificacionInversionLarga = String.Empty
                objNvoEncabezado.strNombreCalificacionInversionCorta = String.Empty
                objNvoEncabezado.logActivo = True
                objNvoEncabezado.intIDCalificadora = 0
                objNvoEncabezado.strNomCalificadora = String.Empty
                objNvoEncabezado.dblValorCupo = 0
                objNvoEncabezado.dblValorCupoConsumido = 0
                objNvoEncabezado.lngIDPoblacion = 0
                objNvoEncabezado.strDescripcionPoblacion = String.Empty
                objNvoEncabezado.lngIDDepartamento = 0
                objNvoEncabezado.strDescripcionDepartamento = String.Empty
                objNvoEncabezado.lngIDPais = 0
                objNvoEncabezado.strDescripcionPais = String.Empty
                objNvoEncabezado.logReplicarAEmisores = False
                objNvoEncabezado.strDescripcionDepartamento = String.Empty
                objNvoEncabezado.logEsComisionista = False
                objNvoEncabezado.strEMail = String.Empty
                objNvoEncabezado.logAplicaEnrutamiento = False
                objNvoEncabezado.logVigiladoSuper = False
                objNvoEncabezado.lngIDGrupo = 0
                objNvoEncabezado.lngIDSubGrupo = 0
                objNvoEncabezado.logCarteraColectiva = False                    'JP20150716
                objNvoEncabezado.intCodigoEntidadAdmin = Nothing                'JP20150716
                objNvoEncabezado.strClaseInversion = String.Empty               'JP20150716
                objNvoEncabezado.strDescripcionClaseInversion = String.Empty    'JP20150716
                objNvoEncabezado.intDigitoVerificacion = Nothing
                objNvoEncabezado.intIDComisionista = Nothing
                objNvoEncabezado.strTipoFondo = String.Empty                    'JEPM20160506
                objNvoEncabezado.strClasificacionAfiliado = String.Empty
                objNvoEncabezado.strCalidadTributaria = String.Empty
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado
            HabilitarEncabezado = True
            HabilitarDocumento = True
            HabilitarReplicarAEmisores = True
            HabilitarReplicacionEsComisionista = True
            HabilitarVigiladoSuperYGrupos = True
            mlogTerminoSubmitChanges = False

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
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 7/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 7/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00012
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
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
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 7/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 7/2014 - Resultado Ok  
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00013
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>

    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.strNombre) Or Not IsNothing(cb.strTipoIdentificacion) Or
                Not IsNothing(cb.strNroDocumento) Or Not IsNothing(cb.intIDTipoEntidad) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.strTipoIdentificacion, cb.strNroDocumento, cb.strNombre, CInt(cb.intIDTipoEntidad))
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
    ''' <history>
    ''' ID caso de prueba: Id_5, Id_6, Id_7, Id_8
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 7/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 7/2014 - Resultado Ok 
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
                intNroOcurrencias = (From e In ListaEncabezado Where e.intIDEntidad = _EncabezadoSeleccionado.intIDEntidad Select e).Count

                If intNroOcurrencias = 0 Then
                    strAccion = ValoresUserState.Ingresar.ToString()
                    ListaEncabezado.Add(_EncabezadoSeleccionado)
                End If

                If EncabezadoSeleccionado.logReplicarAEmisores Then
                    mdcProxy.ValidarEmisor(EncabezadoSeleccionado.strNroDocumento, CInt(EncabezadoSeleccionado.intIDTipoEntidad), Program.Usuario, Program.HashConexion, AddressOf TerminoValidarEmisor, "")
                Else
                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
                End If

                ' Enviar cambios al servidor
                'mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
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
    ''' <history>
    ''' ID caso de prueba: CP0006, CP0007
    ''' Descripción      : Se agrega la variable "mlogTerminoSubmitChanges" para saber si el TerminoSubmitChanges es falso o verdadero. 
    '''                    Tambien se consulta el valor de la propiedad logEsComisionista para habilitar o deshabilitar el campo "Aplica enrutamiento".
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Noviembre 25/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Noviembre 25/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Se realiza la lógica para habilitar los chek de replicarcomisionista y replicaraemisores para permitir decidir que ya dejan de estar activos
    '''                    siempre y cuando el comisionista no sea usado en ordenes y el emisor no sea usado en especies.
    ''' Creado por       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha            : Junio 19/2015
    ''' Pruebas CB       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) - Junio 19/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00016
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EncabezadoSeleccionado) Then
            Try
                If mdcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If Not EncabezadoSeleccionado.logActivo Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("La Entidad está inactiva, no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                IsBusy = True

                _EncabezadoSeleccionado.strUsuario = Program.Usuario

                mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarDocumento = False
                HabilitarEncabezado = True
                mlogTerminoSubmitChanges = False

                If EncabezadoSeleccionado.strNroDocumento = EncabezadoSeleccionado.strIDComisionista Then
                    HabilitarReplicacionEsComisionista = False
                Else
                    HabilitarReplicacionEsComisionista = True
                End If

                If EncabezadoSeleccionado.strNroDocumento = EncabezadoSeleccionado.strIDEmisor Then
                    HabilitarReplicarAEmisores = False
                Else
                    HabilitarReplicarAEmisores = True
                End If

                If EncabezadoSeleccionado.logEsComisionista Then
                    HabilitarEnrutamiento_Y_CodComisionista = True
                Else
                    HabilitarEnrutamiento_Y_CodComisionista = False
                End If

                If EncabezadoSeleccionado.logReplicarAEmisores Then
                    HabilitarCodigoEmisor = True
                Else
                    HabilitarCodigoEmisor = False
                End If

                If EncabezadoSeleccionado.logCarteraColectiva Then
                    HabilitarSiEsCarteraColectiva = True
                Else
                    HabilitarSiEsCarteraColectiva = False
                End If

                HabilitarVigiladoSuperYGrupos = True

                IsBusy = False
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0006, CP0007
    ''' Descripción      : Se deshabilita la propiedad logAplicaEnrutamiento con la variable HabilitarEnrutamiento_Y_CodComisionista.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Noviembre 25/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Noviembre 25/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se añaden propiedades relacionadas a las columnas en que se ubican los controles y la visibilidad según "Cartera Colectiva".
    ''' Fecha            : Julio 16/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 16/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarDocumento = False
                HabilitarEnrutamiento_Y_CodComisionista = False
                HabilitarReplicarAEmisores = False
                HabilitarReplicacionEsComisionista = False
                HabilitarVigiladoSuperYGrupos = False

                HabilitarCodigoEmisor = False
                HabilitarSiEsCarteraColectiva = False
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
    ''' ID caso de prueba: Id_9
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Septiembre 7/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Septiembre 7/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00018
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If mdcProxy.Entidades.Where(Function(i) i.intIDEntidad = EncabezadoSeleccionado.intIDEntidad And CBool(EncabezadoSeleccionado.logActivo) = False).Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción activa la entidad seleccionada. ¿Confirma la activación de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)

                Else
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción inactiva la entidad seleccionada. ¿Confirma la inactivación de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
                End If
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
    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.Entidades.Where(Function(i) i.intIDEntidad = EncabezadoSeleccionado.intIDEntidad).Count > 0 Then
                        mdcProxy.Entidades.Remove(mdcProxy.Entidades.Where(Function(i) i.intIDEntidad = EncabezadoSeleccionado.intIDEntidad).First)
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConfirmarTerminoValidarEmisor(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyMaestros()
            End If

            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                EncabezadoSeleccionado.logReplicarAEmisores = False
            End If

            Program.VerificarCambiosProxyServidor(mdcProxy)
            mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el confirmar cerrar portafolio", Me.ToString(), "ConfirmarCerrarPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaEntidades
            objCB.strNombre = String.Empty
            objCB.strTipoIdentificacion = String.Empty
            objCB.strNroDocumento = String.Empty
            objCB.intIDTipoEntidad = 0
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
    Private Function ObtenerRegistroAnterior() As Entidades
        Dim objEncabezado As Entidades = New Entidades

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of Entidades)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDEntidad = _EncabezadoSeleccionado.intIDEntidad
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <history>
    ''' ID caso de prueba: CP0005
    ''' Descripción      : Se valida la propiedad strEMail.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Noviembre 25/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Noviembre 25/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Se validan los campos lngIDGrupo, lngIDSubGrupo en caso de que replique a emisores
    ''' Creado por       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha            : Mayo 20/2015
    ''' Pruebas CB       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) - Mayo 20/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Validaciones de campos "Codígo entidad admin." y "Clase inversión" de acuerdo a la opción "Cartera colectiva", cambios solicitados por Jorge Arango.
    ''' Fecha            : Julio 16/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 16/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: CP00017
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Agosto 6/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Agosto 6/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: 
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Validaciones de campos "Codígo entidad admin." y "Clase inversión" de acuerdo a la opción "Cartera colectiva", cambios solicitados por Jorge Arango.
    ''' Fecha            : Julio 16/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 16/2015 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' ID caso de prueba: YAPP20160630
    ''' Creado por       : Yessid Andrés Paniagua Pabón (Alcuadrado S.A.)
    ''' Descripción      : Validacion para controlar que campo Código del emisor sea requerido al habilitar el Check box "Es emisor"
    ''' Fecha            : Junio 30/2016
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón (Alcuadrado S.A.) - Junio 30/2016 - Resultado Ok 
    ''' </history>


    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                'Valida el nombre
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strNombre) Then
                    strMsg = String.Format("{0}{1} + El nombre es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo de identificación
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoIdentificacion) Then
                    strMsg = String.Format("{0}{1} + El tipo de identificación es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el número de documento
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strNroDocumento) Then
                    strMsg = String.Format("{0}{1} + El número de documento es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo entidad
                If (_EncabezadoSeleccionado.intIDTipoEntidad) = 0 Then
                    strMsg = String.Format("{0}{1} + El tipo entidad es un campo requerido.", strMsg, vbCrLf)
                End If

                If EncabezadoSeleccionado.logAplicaEnrutamiento Then
                    'Valida el EMail
                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.strEMail) Then
                        strMsg = String.Format("{0}{1} + El Email es un campo requerido.", strMsg, vbCrLf)
                    ElseIf Not Regex.IsMatch(_EncabezadoSeleccionado.strEMail, STR_EXPRESION_EMAIL_VALIDO) Then
                        strMsg = String.Format("{0}{1} + La dirección de correo electrónico ingresada no es válida.", strMsg, vbCrLf)
                    End If
                End If

                If EncabezadoSeleccionado.logEsComisionista Then
                    'Valida el Código comisionista
                    If IsNothing(_EncabezadoSeleccionado.intIDComisionista) Then
                        strMsg = String.Format("{0}{1} + El código comisionista es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If
                'YAPP20151230 Nueva validacion para el campo naturaleza juridica
                If IsNothing(_EncabezadoSeleccionado.intCodigoNaturaleza) Then
                    strMsg = String.Format("{0}{1} + El código naturaleza jurídica es un campo requerido.", strMsg, vbCrLf)
                End If


                If Not String.IsNullOrEmpty(EncabezadoSeleccionado.strEMail) And Not EncabezadoSeleccionado.logAplicaEnrutamiento Then
                    If Not Regex.IsMatch(_EncabezadoSeleccionado.strEMail, STR_EXPRESION_EMAIL_VALIDO) Then
                        strMsg = String.Format("{0}{1} + La dirección de correo electrónico ingresada no es válida.", strMsg, vbCrLf)
                    End If
                End If

                'Valida que al replicar a emisores se seleccione grupo y sub grupo
                If EncabezadoSeleccionado.logReplicarAEmisores Then
                    If IsNothing(EncabezadoSeleccionado.lngIDGrupo) Or EncabezadoSeleccionado.lngIDGrupo = 0 Then
                        strMsg = String.Format("{0}{1} + Grupo es un campo requerido.", strMsg, vbCrLf)
                    End If
                    If IsNothing(EncabezadoSeleccionado.lngIDSubGrupo) Or EncabezadoSeleccionado.lngIDSubGrupo = 0 Then
                        strMsg = String.Format("{0}{1} + Sub grupo es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

                'Valida que al seleccionar el campo "Cartera colectiva", los campos código entidad admin. y clase inversión sean obligatorios.
                'JEPM20160506 El campo strTipoFondo también es obligatorio si es Cartera colectiva (FIC)
                If EncabezadoSeleccionado.logCarteraColectiva Then
                    If IsNothing(EncabezadoSeleccionado.intCodigoEntidadAdmin) Then
                        strMsg = String.Format("{0}{1} + EL código entidad admin. es un campo requerido.", strMsg, vbCrLf)
                    End If
                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.strDescripcionClaseInversion) Then
                        strMsg = String.Format("{0}{1} + La clase inversión es un campo requerido.", strMsg, vbCrLf)
                    End If
                    If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoFondo) Then
                        strMsg = String.Format("{0}{1} + El tipo de fondo es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If
                'YAPP20160630 -Inicio cambio
                If (EncabezadoSeleccionado.logReplicarAEmisores = True And (EncabezadoSeleccionado.strCodigoEmisor = String.Empty Or EncabezadoSeleccionado.strCodigoEmisor = "0")) Then
                    strMsg = String.Format("{0}{1} + El código del emisor es un campo requerido.", strMsg, vbCrLf)
                End If
                'YAPP20160630 -Fin cambio
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
    ''' <history>
    ''' ID caso de prueba: CP0008
    ''' Descripción      : Se agrega la variable "mlogTerminoSubmitChanges" para asignarla en True. 
    '''                    Tambien se asigna el valor false a la variable HabilitarEnrutamiento_Y_CodComisionista.
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Noviembre 25/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Noviembre 25/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Descripción      : Se asigna el valor false a la propiedad HabilitarVigiladoSuperYGrupos.
    ''' Creado por       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha            : Mayo 20/2015
    ''' Pruebas CB       : Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.) - Mayo 20/2015 - Resultado Ok 
    ''' </history>
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
                HabilitarDocumento = False
                HabilitarEncabezado = False
                HabilitarEnrutamiento_Y_CodComisionista = False
                HabilitarReplicarAEmisores = False
                HabilitarReplicacionEsComisionista = False
                HabilitarVigiladoSuperYGrupos = False
                mlogTerminoSubmitChanges = True
                HabilitarCodigoEmisor = False
                HabilitarSiEsCarteraColectiva = False

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

    ''' <summary>
    ''' Método asincrónico que cálcula el dígito de verficación.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CalcularDigitoNIT()
        Dim mdcProxy As MaestrosCFDomainContext

        Try
            mdcProxy = inicializarProxyMaestros()

            mdcProxy.CalcularDigitoNIT(EncabezadoSeleccionado.strNroDocumento, EncabezadoSeleccionado.intDigitoVerificacion, Program.Usuario, Program.HashConexion, AddressOf TerminoCalcularDigitoNIT, "")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al calcular el dígito de verificación.", Me.ToString(), "CalcularDigitoNIT", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of Entidades)
        Dim dcProxy As MaestrosCFDomainContext

        Try
            dcProxy = inicializarProxyMaestros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarEntidadesPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
    ''' Consultar de forma sincrónica los datos de Entidades
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pstrTipoIdentificacion As String = "",
                                               Optional ByVal pstrNroDocumento As String = "",
                                               Optional ByVal pstrNombre As String = "",
                                               Optional ByVal pintIDTipoEntidad As Integer = 0,
                                               Optional ByVal pintIDCodigoCIIU As Integer = 0,
                                               Optional ByVal pintIDCalificacionInversion As Integer = 0,
                                               Optional ByVal pintIDCalificadora As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of Entidades)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyMaestros()
            End If

            mdcProxy.Entidades.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarEntidadesSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarEntidadesSyncQuery(pstrTipoIdentificacion:=pstrTipoIdentificacion,
                                                                                  pstrNroDocumento:=pstrNroDocumento,
                                                                                  pstrNombre:=pstrNombre,
                                                                                  pintIDTipoEntidad:=pintIDTipoEntidad,
                                                                                  pintIDCodigoCIIU:=pintIDCodigoCIIU,
                                                                                  pintIDCalificacionInversion:=pintIDCalificacionInversion,
                                                                                  pintIDCalificadora:=pintIDCalificadora,
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
                    ListaEncabezado = mdcProxy.Entidades

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de Entidades ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Este metodo se ejecuta al terminar de realizar la valoración
    ''' </summary>
    ''' <param name="lo">Objeto de tipo InvokeOperation(Of String)</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Descripción  : Este metodo se ejecuta al terminar de realizar la valoración
    ''' Fecha        : Abril 11/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 11/2014 - Resultado OK
    ''' </history>
    Private Sub TerminoValidarEmisor(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó validar emisor", _
                                                 Me.ToString(), "TerminoValidarEmisor", Application.Current.ToString(), Program.Maquina, lo.Error)
            ElseIf lo.Value <> String.Empty Then
                A2Utilidades.Mensajes.mostrarMensajePregunta(lo.Value, Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarTerminoValidarEmisor)
            Else
                Dim strAccion As String = ValoresUserState.Actualizar.ToString()

                Program.VerificarCambiosProxyServidor(mdcProxy)
                mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, strAccion)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó validar emisor", _
                                                             Me.ToString(), "TerminoValidarEmisor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Este metodo se ejecuta al terminar de realizar el cálculo del dpigito de verificación.
    ''' </summary>
    Private Sub TerminoCalcularDigitoNIT(lo As InvokeOperation(Of Integer))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó calcular el dígito de verificación", _
                                                 Me.ToString(), "TerminoCalcularDigitoNIT", Application.Current.ToString(), Program.Maquina, lo.Error)
            Else
                EncabezadoSeleccionado.intDigitoVerificacion = lo.Value
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó calcular el dígito de verificación.", _
                                                             Me.ToString(), "TerminoCalcularDigitoNIT", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar los subgrupos dependiente de los grupos
    ''' </summary>
    ''' <history>
    ''' Creado por   : Yessid Andres Paniagua Pabon
    ''' Descripción  : Consultar las calificaciones de tipo L y tipo C según la calificadora seleccionada
    ''' Fecha        : Diciembre 28/2015
    ''' Pruebas CB   : Yessid Andres Paniagua Pabon - Diciembre 28/2015 - Resultado OK
    ''' ID del cambio: YAPP20151228
    ''' </history>
    Public Sub ConsultarListaSubGrupos()
        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                If EncabezadoSeleccionado.lngIDGrupo <> 0 Then
                    mdcProxy.Load(mdcProxy.ListarSubgruposQuery(CType(EncabezadoSeleccionado.lngIDGrupo, Integer), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListaSubGrupos, Nothing)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la lista de subgrupos.", _
                                                             Me.ToString(), "ConsultarListaSubGrupos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Consultar los subgrupos dependiente de los grupos
    ''' </summary>
    ''' <history>
    ''' Creado por   : Yessid Andres Paniagua Pabon
    ''' Descripción  : Consultar las calificaciones de tipo L y tipo C según la calificadora seleccionada
    ''' Fecha        : Diciembre 28/2015
    ''' Pruebas CB   : Yessid Andres Paniagua Pabon - Diciembre 28/2015 - Resultado OK
    ''' ID del cambio: YAPP20151228
    ''' </history>
    Private Sub TerminoTraerListaSubGrupos(ByVal lo As LoadOperation(Of SubGrupo))
        Try
            Dim intSubGrupo As Integer? = Nothing
            If Not lo.HasError Then
                If Not IsNothing(EncabezadoSeleccionado) Then
                    intSubGrupo = EncabezadoSeleccionado.lngIDSubGrupo
                    EncabezadoSeleccionado.lngIDSubGrupo = Nothing
                    ListaComboSubGrupo_Filtrado = lo.Entities.ToList  'Entidad de CalificacionesCalificadora (Tabla de relación) 
                    EncabezadoSeleccionado.lngIDSubGrupo = intSubGrupo
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de subgrupos", _
                                                 Me.ToString(), "TerminoTraerListaSubGrupos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer la lista de subgrupos", _
             Me.ToString(), "TerminoTraerListaSubGrupos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Consultar las calificaciones de tipo LARGA y tipo CORTA según la calificadora seleccionada
    ''' </summary>
    ''' <history>
    ''' Creado por   : Javier Eduardo Pardo Moreno
    ''' Descripción  : Consultar las calificaciones de tipo L y tipo C según la calificadora seleccionada
    ''' Fecha        : Octubre 06/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Octubre 06/2015 - Resultado OK
    ''' ID del cambio: JEPM20151007 
    ''' </history>
    Public Sub ConsultarListaCalificaiones()
        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                If EncabezadoSeleccionado.intIDCalificadora <> 0 Then
                    mdcProxy.Load(mdcProxy.ConsultarCalificacionesCalificadoraQuery(0, EncabezadoSeleccionado.intIDCalificadora, 0, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificacionesCalificadora, Nothing)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las calificaciones.", _
                                                             Me.ToString(), "ConsultarListaCalificaiones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filtrar los combos de Calificación Larga y Calificación Corta
    ''' </summary>
    ''' <history>
    ''' Creado por   : Javier Eduardo Pardo Moreno
    ''' Descripción  : Filtra los combos de Calificación Larga y Calificación Corta, según la información consultada y asigna el SelectedItem en el View.
    ''' Fecha        : Octubre 07/2015
    ''' Pruebas CB   : Javier Eduardo Pardo Moreno - Octubre 06/2015 - Resultado OK
    ''' ID del cambio: JEPM20151007 
    ''' </history>
    Private Sub TerminoTraerCalificacionesCalificadora(ByVal lo As LoadOperation(Of CalificacionesCalificadora))
        Try

            If Not lo.HasError Then
                If Not IsNothing(EncabezadoSeleccionado) Then
                    ListaCalificacionesCalificadora = lo.Entities.ToList 'Entidad de CalificacionesCalificadora (Tabla de relación)

                    'Consultar las calificaciones de tipo L y tipo C según la calificadora seleccionada
                    Dim intIdCalificadoraSeleccionada As Integer
                    intIdCalificadoraSeleccionada = EncabezadoSeleccionado.intIDCalificadora
                    Dim objListaCC As New List(Of CalificacionesCalificadora)
                    objListaCC = (From c In ListaCalificacionesCalificadora Where c.intCodCalificadora = intIdCalificadoraSeleccionada).ToList()

                    'Se crean las variables intCalificacionLarga y intCalificacionCorta para almacenar el valor del item seleccionado ya que al filtrar los combos se pierde
                    'este valor y es necesario volver a asignarlos en el EncabezadoSeleccionado
                    Dim intCalificacionLarga As Nullable(Of Integer) = EncabezadoSeleccionado.intIDCalificacionInversionLarga 'JEPM20151008
                    Dim intCalificacionCorta As Nullable(Of Integer) = EncabezadoSeleccionado.intIDCalificacionInversionCorta 'JEPM20151008

                    EncabezadoSeleccionado.intIDCalificacionInversionLarga = Nothing
                    EncabezadoSeleccionado.intIDCalificacionInversionCorta = Nothing
                    ListaComboCalificacionesLarga_Filtrada = Nothing
                    ListaComboCalificacionesCorta_Filtrada = Nothing

                    Dim ListaVaciaCorto As New List(Of OYDUtilidades.ItemCombo)
                    Dim ListaVaciaLarga As New List(Of OYDUtilidades.ItemCombo)

                    ListaVaciaLarga = (From c In ListaComboCalificacionesLarga Where (From o In objListaCC Select o.intIDCalificacion).Contains(c.ID)).ToList
                    ListaVaciaCorto = (From c In ListaComboCalificacionesCorta Where (From o In objListaCC Select o.intIDCalificacion).Contains(c.ID)).ToList

                    'Dim ItemComboVacio As New OYDUtilidades.ItemCombo

                    'ItemComboVacio.Descripcion = " "
                    'ListaVaciaLarga.Add(ItemComboVacio)
                    'ListaVaciaCorto.Add(ItemComboVacio)

                    ListaComboCalificacionesLarga_Filtrada = ListaVaciaLarga
                    ListaComboCalificacionesCorta_Filtrada = ListaVaciaCorto

                    EncabezadoSeleccionado.intIDCalificacionInversionLarga = intCalificacionLarga
                    EncabezadoSeleccionado.intIDCalificacionInversionCorta = intCalificacionCorta
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Calificaciones por calificadora", _
                                             Me.ToString(), "TerminoTraerCalificacionesCalificadora", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las Calificaciones por calificadora", _
             Me.ToString(), "TerminoTraerCalificacionesCalificadora", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    '**'
    Private _ListaComboSubGrupo_Filtrado As List(Of SubGrupo)

    Public Property ListaComboSubGrupo_Filtrado As List(Of SubGrupo)
        Get
            Return _ListaComboSubGrupo_Filtrado
        End Get
        Set(ByVal value As List(Of SubGrupo))
            _ListaComboSubGrupo_Filtrado = value
            MyBase.CambioItem("ListaComboSubGrupo_Filtrado")
        End Set
    End Property

    ''' <summary>
    ''' Carga la lista de calificaciones CFCalificacionesInversionesL y CFCalificacionesInversionesC desde el SP de cargar Combos, para luego ser filtrados según la calificadora seleccionada
    ''' </summary>
    ''' <history>
    ''' Creado por       : Javier Eduardo Pardo Moreno
    ''' Fecha            : Octubre 06/2015
    ''' Pruebas CB       : Javier Eduardo Pardo Moreno - Octubre 06/2015 - Resultado OK
    ''' ID del cambio    : JEPM20151007 
    ''' </history>
    Private Sub CargarCalificacionesInversiones()
        Try
            If IsNothing(ListaComboCalificacionesLarga) And IsNothing(ListaComboCalificacionesCorta) Then
                'CFCalificacionesInversionesL
                If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CFCalificacionesInversionesL") Then
                    If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("CFCalificacionesInversionesL")) Then
                        ListaComboCalificacionesLarga = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
                                      Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("CFCalificacionesInversionesL"))
                    End If
                End If

                'CFCalificacionesInversionesC
                If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CFCalificacionesInversionesC") Then
                    If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("CFCalificacionesInversionesC")) Then
                        ListaComboCalificacionesCorta = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos), 
                                       Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("CFCalificacionesInversionesC"))
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las calificaciones de inversiones", _
             Me.ToString(), "CargarCalificacionesInversiones", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaEntidades
    Implements INotifyPropertyChanged

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

    Private _strTipoIdentificacion As String
    Public Property strTipoIdentificacion() As String
        Get
            Return _strTipoIdentificacion
        End Get
        Set(ByVal value As String)
            _strTipoIdentificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoIdentificacion"))
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

    Private _intIDTipoEntidad As System.Nullable(Of Integer)
    Public Property intIDTipoEntidad() As System.Nullable(Of Integer)
        Get
            Return _intIDTipoEntidad
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intIDTipoEntidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDTipoEntidad"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

''' <history>
''' ID caso de prueba: 
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Se utiliza de acuerdo a la opción "Cartera colectiva", cambios solicitados por Jorge Arango.
''' Fecha            : Julio 16/2015
''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Julio 16/2015 - Resultado Ok 
''' </history>
Public Class ClaseInversion
    Implements INotifyPropertyChanged

    Private _strcodigo As String
    <Display(Name:="codigo")> _
    Public Property strcodigo As String
        Get
            Return _strcodigo
        End Get
        Set(ByVal value As String)
            _strcodigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strcodigo"))
        End Set
    End Property

    Private _strDescripcion As String
    <Display(Name:="Clase Inversión")> _
    Public Property strDescripcion As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcion"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


