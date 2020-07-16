Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2CFUtilitarios
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.Net.Http
Imports Newtonsoft
Imports Newtonsoft.Json
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Net.Http.Headers
Imports System.Data
Imports System.Collections.ObjectModel
Imports System.Dynamic
Imports OpenRiaServices.DomainServices.Client

''' <summary>
''' Métodos creados para la comunicación con el RIA (MaestrosDomainService.vb y OyD_Maestros.dbml)
''' Pantalla Configuración de Parámetros (Maestros)
''' </summary>
Public Class EjecutarScriptGridPantallaViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As UtilidadesCFDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Dim plogCargarDisenoDefecto As Boolean = True

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusyDiseno = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                If intIDScript > 0 And
                    Not String.IsNullOrEmpty(strNombreScript) Then
                    IsBusyDiseno = True

                    'obtiene los parametros enviados como filtro y los lleva a una varible para mostrar en pantalla
                    If Not String.IsNullOrEmpty(strParametrosVisualizar) Then
                        Dim intContador As Integer = 1
                        For Each li In strParametrosVisualizar.Split(",")
                            CType(View_EjecutarScriptGridPantalla.FindName(String.Format("txtParametro{0}", intContador)), TextBlock).Text = li.Split("=")(0) & "="
                            CType(View_EjecutarScriptGridPantalla.FindName(String.Format("txtParametro{0}", intContador)), TextBlock).Visibility = Visibility.Visible
                            intContador += 1
                            CType(View_EjecutarScriptGridPantalla.FindName(String.Format("txtParametro{0}", intContador)), TextBlock).Text = li.Split("=")(1)
                            CType(View_EjecutarScriptGridPantalla.FindName(String.Format("txtParametro{0}", intContador)), TextBlock).Visibility = Visibility.Visible
                            intContador += 1
                        Next

                        MostrarSinParametros = Visibility.Collapsed
                        MostrarParametrosFiltros = Visibility.Visible
                    Else
                        MostrarSinParametros = Visibility.Visible
                        MostrarParametrosFiltros = Visibility.Collapsed
                    End If

                    'consulta los diseños
                    Await ConsultarListaDisenos()
                    VerificarAdministradorScript()

                    IsBusyDiseno = False

                    'Inicia timer para consultar la información del grid, se realiza en este proceso para no bloquear la pantalla inicial de carga
                    ReiniciaTimer()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se recibieron los parametros del script a procesar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    IsBusyDiseno = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyDiseno = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _ExpanderTitulo As Boolean = True
    Public Property ExpanderTitulo() As Boolean
        Get
            Return _ExpanderTitulo
        End Get
        Set(ByVal value As Boolean)
            _ExpanderTitulo = value
            MyBase.CambioItem("ExpanderTitulo")
            View_EjecutarScriptGridPantalla.EjecutarAltoControl()
        End Set
    End Property

    Private _ExpanderDiseno As Boolean = False
    Public Property ExpanderDiseno() As Boolean
        Get
            Return _ExpanderDiseno
        End Get
        Set(ByVal value As Boolean)
            _ExpanderDiseno = value
            MyBase.CambioItem("ExpanderDiseno")
            View_EjecutarScriptGridPantalla.EjecutarAltoControl()
        End Set
    End Property

    Public Property View_EjecutarScriptGridPantalla() As EjecutarScriptGridPantallaView
    Public Property View_EjecutarScriptGridPantalla_Diseno() As EjecutarScriptGridPantalla_DisenoView

    ''' <summary>
    ''' Lista de ScriptsA2Diseno que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of ScriptsA2Diseno)
    Public Property ListaEncabezado() As List(Of ScriptsA2Diseno)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of ScriptsA2Diseno))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ScriptsA2Diseno para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de ScriptsA2Diseno que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As ScriptsA2Diseno
    Public Property EncabezadoSeleccionado() As ScriptsA2Diseno
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As ScriptsA2Diseno)
            _EncabezadoSeleccionado = value
            If Not IsNothing(_EncabezadoSeleccionado) Then
                View_EjecutarScriptGridPantalla.ReorganizarColoresGridDisenos(ObtenerContadorSeleccionado())
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private _NombreCompletoDisenoSeleccionado As String = "Ningún diseño seleccionado"
    Public Property NombreCompletoDisenoSeleccionado() As String
        Get
            Return _NombreCompletoDisenoSeleccionado
        End Get
        Set(ByVal value As String)
            _NombreCompletoDisenoSeleccionado = value
            MyBase.CambioItem("NombreCompletoDisenoSeleccionado")
        End Set
    End Property

    Private _intIDScript As Integer
    Public Property intIDScript() As Integer
        Get
            Return _intIDScript
        End Get
        Set(ByVal value As Integer)
            _intIDScript = value
            MyBase.CambioItem("intIDScript")
        End Set
    End Property

    Private _intIDCompania As Nullable(Of Integer)
    Public Property intIDCompania() As Nullable(Of Integer)
        Get
            Return _intIDCompania
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDCompania = value
            MyBase.CambioItem("intIDCompania")
        End Set
    End Property

    Private _strNombreExportacion As String
    Public Property strNombreExportacion() As String
        Get
            Return _strNombreExportacion
        End Get
        Set(ByVal value As String)
            _strNombreExportacion = value
            MyBase.CambioItem("strNombreExportacion")
        End Set
    End Property

    Private _strNombreScript As String
    Public Property strNombreScript() As String
        Get
            Return _strNombreScript
        End Get
        Set(ByVal value As String)
            _strNombreScript = value
            MyBase.CambioItem("strNombreScript")
        End Set
    End Property

    Private _strDescripcionScript As String
    Public Property strDescripcionScript() As String
        Get
            Return _strDescripcionScript
        End Get
        Set(ByVal value As String)
            _strDescripcionScript = value
            MyBase.CambioItem("strDescripcionScript")
        End Set
    End Property

    Private _strParametrosFiltro As String
    Public Property strParametrosFiltro() As String
        Get
            Return _strParametrosFiltro
        End Get
        Set(ByVal value As String)
            _strParametrosFiltro = value
            MyBase.CambioItem("strParametrosFiltro")
        End Set
    End Property

    Private _strParametrosVisualizar As String
    Public Property strParametrosVisualizar() As String
        Get
            Return _strParametrosVisualizar
        End Get
        Set(ByVal value As String)
            _strParametrosVisualizar = value
            MyBase.CambioItem("strParametrosVisualizar")
        End Set
    End Property

    Private _intIDDisenoDefecto As Integer
    Public Property intIDDisenoDefecto() As Integer
        Get
            Return _intIDDisenoDefecto
        End Get
        Set(ByVal value As Integer)
            _intIDDisenoDefecto = value
            MyBase.CambioItem("intIDDisenoDefecto")
        End Set
    End Property

    Private _IsBusyDiseno As Boolean = False
    Public Property IsBusyDiseno() As Boolean
        Get
            Return _IsBusyDiseno
        End Get
        Set(ByVal value As Boolean)
            _IsBusyDiseno = value
            MyBase.CambioItem("IsBusyDiseno")
        End Set
    End Property

    Private _IsBusyControlGrid As Boolean = False
    Public Property IsBusyControlGrid() As Boolean
        Get
            Return _IsBusyControlGrid
        End Get
        Set(ByVal value As Boolean)
            _IsBusyControlGrid = value
            MyBase.CambioItem("IsBusyControlGrid")
        End Set
    End Property

    Private _ID_Edicion As Integer
    Public Property ID_Edicion() As Integer
        Get
            Return _ID_Edicion
        End Get
        Set(ByVal value As Integer)
            _ID_Edicion = value
            MyBase.CambioItem("ID_Edicion")
        End Set
    End Property

    Private _Diseno_Edicion As String
    Public Property Diseno_Edicion() As String
        Get
            Return _Diseno_Edicion
        End Get
        Set(ByVal value As String)
            _Diseno_Edicion = value
            MyBase.CambioItem("Diseno_Edicion")
        End Set
    End Property

    Private _DescripcionDiseno_Edicion As String
    Public Property DescripcionDiseno_Edicion() As String
        Get
            Return _DescripcionDiseno_Edicion
        End Get
        Set(ByVal value As String)
            _DescripcionDiseno_Edicion = value
            MyBase.CambioItem("DescripcionDiseno_Edicion")
        End Set
    End Property

    Private _FiltroDiseno_Edicion As String
    Public Property FiltroDiseno_Edicion() As String
        Get
            Return _FiltroDiseno_Edicion
        End Get
        Set(ByVal value As String)
            _FiltroDiseno_Edicion = value
            MyBase.CambioItem("FiltroDiseno_Edicion")
        End Set
    End Property

    Private _MostrarTodosUsuarios As Visibility = Visibility.Collapsed
    Public Property MostrarTodosUsuarios() As Visibility
        Get
            Return _MostrarTodosUsuarios
        End Get
        Set(ByVal value As Visibility)
            _MostrarTodosUsuarios = value
            MyBase.CambioItem("MostrarTodosUsuarios")
        End Set
    End Property

    Private _HabilitarTodosUsuarios As Boolean
    Public Property HabilitarTodosUsuarios() As Boolean
        Get
            Return _HabilitarTodosUsuarios
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTodosUsuarios = value
            MyBase.CambioItem("HabilitarTodosUsuarios")
        End Set
    End Property

    Private _MostrarActualizarDiseno As Visibility
    Public Property MostrarActualizarDiseno() As Visibility
        Get
            Return _MostrarActualizarDiseno
        End Get
        Set(ByVal value As Visibility)
            _MostrarActualizarDiseno = value
            MyBase.CambioItem("MostrarActualizarDiseno")
        End Set
    End Property

    Private _AdministradorDisenosScript As Boolean
    Public Property AdministradorDisenosScript() As Boolean
        Get
            Return _AdministradorDisenosScript
        End Get
        Set(ByVal value As Boolean)
            _AdministradorDisenosScript = value
            MyBase.CambioItem("AdministradorDisenosScript")
        End Set
    End Property

    Private _Diseno_Usuario As String
    Public Property Diseno_Usuario() As String
        Get
            Return _Diseno_Usuario
        End Get
        Set(ByVal value As String)
            _Diseno_Usuario = value
            MyBase.CambioItem("Diseno_Usuario")
        End Set
    End Property

    Private _MostrarSinParametros As Visibility = Visibility.Collapsed
    Public Property MostrarSinParametros() As Visibility
        Get
            Return _MostrarSinParametros
        End Get
        Set(ByVal value As Visibility)
            _MostrarSinParametros = value
            MyBase.CambioItem("MostrarSinParametros")
        End Set
    End Property

    Private _MostrarParametrosFiltros As Visibility = Visibility.Collapsed
    Public Property MostrarParametrosFiltros() As Visibility
        Get
            Return _MostrarParametrosFiltros
        End Get
        Set(ByVal value As Visibility)
            _MostrarParametrosFiltros = value
            MyBase.CambioItem("MostrarParametrosFiltros")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub AbrirActualizarDiseno(ByVal pintIDRegistro As Integer, ByVal pstrNombreDiseno As String, ByVal pstrFiltroDiseno As String)
        Try
            FiltroDiseno_Edicion = pstrFiltroDiseno
            HabilitarTodosUsuarios = False
            Diseno_Usuario = Program.Usuario

            If pintIDRegistro = 0 Then
                ID_Edicion = -1
                Diseno_Edicion = String.Empty
                DescripcionDiseno_Edicion = String.Empty
                MostrarActualizarDiseno = Visibility.Collapsed
                HabilitarTodosUsuarios = False

                If AdministradorDisenosScript Then
                    MostrarTodosUsuarios = Visibility.Visible
                Else
                    MostrarTodosUsuarios = Visibility.Collapsed
                End If
            Else
                ID_Edicion = pintIDRegistro
                Diseno_Edicion = pstrNombreDiseno
                DescripcionDiseno_Edicion = _EncabezadoSeleccionado.Descripcion

                If _EncabezadoSeleccionado.PermitirEdicion Then
                    MostrarActualizarDiseno = Visibility.Visible

                    If AdministradorDisenosScript Then
                        MostrarTodosUsuarios = Visibility.Visible
                        If _EncabezadoSeleccionado.UsuarioApp = "TODOS" Then
                            HabilitarTodosUsuarios = True
                        End If
                    Else
                        MostrarTodosUsuarios = Visibility.Collapsed
                        HabilitarTodosUsuarios = False
                    End If
                Else
                    MostrarActualizarDiseno = Visibility.Collapsed
                    HabilitarTodosUsuarios = False
                End If
            End If

			Application.Current.Dispatcher.Invoke(New Action(Function()
																 View_EjecutarScriptGridPantalla_Diseno = New EjecutarScriptGridPantalla_DisenoView(Me)
                                                                 View_EjecutarScriptGridPantalla_Diseno.Owner = View_EjecutarScriptGridPantalla
                                                                 Return View_EjecutarScriptGridPantalla_Diseno.ShowDialog()
															 End Function))
		Catch ex As Exception
			A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el registro. ", Me.ToString(), "AbrirActualizarDiseno", Application.Current.ToString(), Program.Maquina, ex)
			IsBusyDiseno = False
		End Try
	End Sub

    Public Sub Nuevo_Actualizacion_Diseno(ByVal plogNuevoDiseno As Boolean)
        Try
            If String.IsNullOrEmpty(Diseno_Edicion) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Nombre Diseño es un campo requerido.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(DescripcionDiseno_Edicion) Then
                A2Utilidades.Mensajes.mostrarMensaje("La Descripción es un campo requerido.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusyDiseno = True

            Dim strUsuarioApp As String = String.Empty

            If plogNuevoDiseno Then
                If AdministradorDisenosScript Then
                    If HabilitarTodosUsuarios Then
                        strUsuarioApp = "TODOS"
                    Else
                        strUsuarioApp = Program.Usuario
                    End If
                Else
                    strUsuarioApp = Program.Usuario
                End If

                ActualizarDiseno(-1,
                                 intIDScript,
                                 Diseno_Edicion,
                                 DescripcionDiseno_Edicion,
                                 FiltroDiseno_Edicion,
                                 strUsuarioApp,
                                 Program.Usuario)
            Else
                If AdministradorDisenosScript Then
                    If HabilitarTodosUsuarios Then
                        strUsuarioApp = "TODOS"
                    Else
                        strUsuarioApp = Program.Usuario
                    End If
                Else
                    strUsuarioApp = Program.Usuario
                End If

                ActualizarDiseno(_EncabezadoSeleccionado.IDScriptDiseno,
                                 _EncabezadoSeleccionado.IDScript,
                                 Diseno_Edicion,
                                 DescripcionDiseno_Edicion,
                                 FiltroDiseno_Edicion,
                                 strUsuarioApp,
                                 _EncabezadoSeleccionado.UsuarioCreacion)
            End If

            IsBusyDiseno = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el registro. ", Me.ToString(), "Nuevo_Actualizacion_Diseno", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyDiseno = False
        End Try
    End Sub

    Private Async Sub ActualizarDiseno(ByVal pintIDScriptDiseno As Integer,
                                 ByVal pintIDScript As Integer,
                                 ByVal pstrDiseno As String,
                                 ByVal pstrDescripcion As String,
                                 ByVal pstrFiltrosDiseno As String,
                                 ByVal pstrUsuarioApp As String,
                                 ByVal pstrUsuarioCreacion As String)
        Try

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyUtilidades()
            End If

            mdcProxy.ScriptsA2ResultadosGenericos.Clear()

            Dim objRet As LoadOperation(Of ScriptsA2ResultadosGenericos) = Await mdcProxy.Load(mdcProxy.ejecutarScriptDiseno_ActualizarSyncQuery(pintIDScriptDiseno,
                                                                                                                   pintIDScript,
                                                                                                                   pstrDiseno,
                                                                                                                   pstrDescripcion,
                                                                                                                   pstrFiltrosDiseno,
                                                                                                                   pstrUsuarioApp,
                                                                                                                   pstrUsuarioCreacion,
                                                                                                                   Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If Not objRet.Entities Is Nothing Then
                    If objRet.Entities.ToList.Count > 0 Then
                        If objRet.Entities.First.Exitoso Then
                            A2Utilidades.Mensajes.mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                            View_EjecutarScriptGridPantalla_Diseno.DialogResult = True
                            Await ConsultarListaDisenos()

                            If objRet.Entities.First.IDRegistro > 0 Then
                                Dim intIDRegistroGuardado As Integer = CInt(objRet.Entities.First.IDRegistro)
                                If Not IsNothing(_ListaEncabezado) Then
                                    If _ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDRegistroGuardado).Count > 0 Then
                                        EncabezadoSeleccionado = _ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDRegistroGuardado).First
                                        SeleccionarDiseno()
                                    End If
                                End If
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            IsBusyDiseno = False
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        IsBusyDiseno = False
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    IsBusyDiseno = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                IsBusyDiseno = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el registro. ", Me.ToString(), "ActualizarDiseno", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyDiseno = False
        End Try
    End Sub

    Private Async Sub ActualizarFechaSeleccionDiseno(ByVal pintIDScriptDiseno As Integer,
                                 ByVal pintIDScript As Integer,
                                 ByVal pstrUsuarioApp As String)
        Try

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyUtilidades()
            End If

            mdcProxy.ScriptsA2ResultadosGenericos.Clear()

            Dim objRet As LoadOperation(Of ScriptsA2ResultadosGenericos) = Await mdcProxy.Load(mdcProxy.ejecutarScriptDiseno_ActualizarFechaSeleccionSyncQuery(pintIDScriptDiseno,
                                                                                                                                 pintIDScript,
                                                                                                                                 pstrUsuarioApp,
                                                                                                                                 Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If Not objRet.Entities Is Nothing Then
                    If objRet.Entities.ToList.Count > 0 Then
                        If objRet.Entities.First.Exitoso Then
                            'A2Utilidades.Mensajes.mostrarMensaje(objRet.RootResults.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el registro. ", Me.ToString(), "ActualizarFechaSeleccionDiseno", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarDiseno()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                NombreCompletoDisenoSeleccionado = String.Format("Seleccionado #{0}: {1}", _EncabezadoSeleccionado.IDScriptDiseno, _EncabezadoSeleccionado.Diseno)
                View_EjecutarScriptGridPantalla.ctlControlFiltro.IDRegistro = _EncabezadoSeleccionado.IDScriptDiseno
                View_EjecutarScriptGridPantalla.ctlControlFiltro.NombreDiseno = _EncabezadoSeleccionado.Diseno
                View_EjecutarScriptGridPantalla.ctlControlFiltro.FiltrosDefecto = _EncabezadoSeleccionado.FiltrosDiseno

                ActualizarFechaSeleccionDiseno(_EncabezadoSeleccionado.IDScriptDiseno,
                                 intIDScript,
                                 Program.Usuario)
            Else
                NombreCompletoDisenoSeleccionado = "Ningun diseño seleccionado"
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el registro. ", Me.ToString(), "EliminarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyDiseno = False
        End Try
    End Sub

    Public Sub EliminarDiseno()
        Try
            IsBusyDiseno = True
            A2Utilidades.Mensajes.mostrarMensajePregunta("¿Confirma el borrado de este Diseño?", Program.TituloSistema, "Borrar", AddressOf BorrarRegistroConfirmado)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el registro. ", Me.ToString(), "EliminarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusyDiseno = False
        End Try
    End Sub

    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusyDiseno = True
                    If mdcProxy Is Nothing Then
                        mdcProxy = inicializarProxyUtilidades()
                    End If

                    mdcProxy.ScriptsA2ResultadosGenericos.Clear()

                    Dim objRet As LoadOperation(Of ScriptsA2ResultadosGenericos) = Await mdcProxy.Load(mdcProxy.ejecutarScriptDiseno_EliminarSyncQuery(_EncabezadoSeleccionado.IDScriptDiseno, Program.Usuario, Program.HashConexion)).AsTask

                    If Not objRet Is Nothing Then
                        If Not objRet.Entities Is Nothing Then
                            If objRet.Entities.ToList.Count > 0 Then
                                If objRet.Entities.First.Exitoso Then
                                    A2Utilidades.Mensajes.mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                                    Await ConsultarListaDisenos()
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje(objRet.Entities.First.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                    IsBusyDiseno = False
                                End If
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                                IsBusyDiseno = False
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            IsBusyDiseno = False
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        IsBusyDiseno = False
                    End If
                End If
            Else
                IsBusyDiseno = False
            End If
        Catch ex As Exception
            IsBusyDiseno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"


#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consultar de forma sincrónica los datos de ScriptsA2Diseno
    ''' </summary>
    Public Async Function ConsultarListaDisenos() As Task(Of Boolean)
        Try
            IsBusyDiseno = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyUtilidades()
            End If

            mdcProxy.ScriptsA2Disenos.Clear()

            Dim objRet As LoadOperation(Of ScriptsA2Diseno) = Await mdcProxy.Load(mdcProxy.ejecutarScriptDiseno_ConsultarSyncQuery(intIDScript, Program.Usuario, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If Not objRet.Entities Is Nothing Then
                    ListaEncabezado = objRet.Entities.ToList

                    'establece los valores por defecto para los diseños
                    If plogCargarDisenoDefecto Then
                        plogCargarDisenoDefecto = False

                        If Not IsNothing(_ListaEncabezado) Then
                            If intIDDisenoDefecto > 0 Then
                                If _ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDDisenoDefecto).Count > 0 Then
                                    EncabezadoSeleccionado = _ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDDisenoDefecto).First

                                    SeleccionarDiseno()
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de ScriptsA2Diseno ", Me.ToString(), "ConsultarListaDisenos", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusyDiseno = False
        End Try
        Return True
    End Function

    Public Async Sub VerificarAdministradorScript()
        Try
            IsBusyDiseno = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyUtilidades()
            End If

            Dim objRet As InvokeResult(Of Boolean) = Await mdcProxy.ejecutarScriptDiseno_ValidarPermisosSyncAsync(intIDScript, Program.Usuario, Program.Usuario, Program.HashConexion)

            If Not objRet Is Nothing Then
                If Not IsNothing(objRet.Value) Then
                    AdministradorDisenosScript = objRet.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    IsBusyDiseno = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se recibio respuesta del servidor. Por favor comunicar al administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                IsBusyDiseno = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al verificar los permisos del ScriptsA2Diseno.", Me.ToString(), "VerificarAdministradorScript", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusyDiseno = False
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de ScriptsA2Diseno
    ''' </summary>
    Public Async Sub EjecucionDatosConsulta()
        Try
            IsBusyControlGrid = True

            ErrorForma = String.Empty
            Dim objHandler As New HttpClientHandler
            objHandler.UseDefaultCredentials = True

            Dim strParametrosEnviar As String = String.Empty

            If Not String.IsNullOrEmpty(strParametrosFiltro) And strParametrosFiltro <> " " Then
                strParametrosEnviar = System.Web.HttpUtility.UrlEncode(strParametrosFiltro)
            End If

            Dim objClient As New HttpClient(objHandler)
            Dim strParametros As String = String.Format("pintIdCompania={0}&pintIDScript={1}&pstrNombreScript={2}&pstrParametros={3}&pstrUsuario={4}&pstrMaquina={5}&pstrInfoConexion={6}",
                                                        intIDCompania,
                                                        intIDScript,
                                                        strNombreScript,
                                                        strParametrosEnviar,
                                                        Program.Usuario,
                                                        Program.Maquina,
                                                        System.Web.HttpUtility.UrlEncode(Program.HashConexion))
            objClient.BaseAddress = New Uri(Program.RutaServicioBase_Api)
            objClient.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
            objClient.Timeout = TimeSpan.FromHours(2)
            Dim strUrlCompleta As String = Program.RutaServicioBase_Api_DirectorioVirtual & "/api/A2_Utilidades/Scripts_EjecutarConsulta?" & strParametros
            Dim objResponse As HttpResponseMessage = Await objClient.GetAsync(strUrlCompleta)
            If objResponse.IsSuccessStatusCode Then
                Dim strRespuestaJson As String = Await objResponse.Content.ReadAsStringAsync()
                Dim objDataSetRespuesta As DataSet = JsonConvert.DeserializeObject(Of DataSet)(strRespuestaJson)
                View_EjecutarScriptGridPantalla.ctlControlFiltro.NombreExportacion = strNombreExportacion
                If objDataSetRespuesta.Tables.Count > 0 Then
                    View_EjecutarScriptGridPantalla.ctlControlFiltro.DataViewSP = objDataSetRespuesta.Tables(0).DefaultView
                End If
            Else
                Dim objExcepcionLlamado As New Exception(objResponse.ReasonPhrase)
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de ScriptsA2Diseno ", Me.ToString(), "EjecucionDatosConsulta", Application.Current.ToString(), Program.Maquina, objExcepcionLlamado)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de ScriptsA2Diseno ", Me.ToString(), "EjecucionDatosConsulta", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusyControlGrid = False
        End Try
    End Sub

    Public Function ObtenerContadorSeleccionado() As Integer
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Dim intContador As Integer = 0
                For Each li In _ListaEncabezado
                    If li.IDScriptDiseno = _EncabezadoSeleccionado.IDScriptDiseno Then
                        Exit For
                    End If
                    intContador += 1
                Next
                Return intContador
            Else
                Return -1
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el id de la lista", Me.ToString(), "ObtenerContadorSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
            Return -1
        End Try
    End Function

#End Region

#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimer As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If _myDispatcherTimer Is Nothing Then
                _myDispatcherTimer = New System.Windows.Threading.DispatcherTimer
                _myDispatcherTimer.Interval = New TimeSpan(0, 0, 1)
                AddHandler _myDispatcherTimer.Tick, AddressOf Me.Each_Tick
            End If
            _myDispatcherTimer.Start()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusyControlGrid = False
        End Try

    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimer) Then
                _myDispatcherTimer.Stop()
                RemoveHandler _myDispatcherTimer.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimer = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        'consulta el set de datos principal para enviar al modulo
        pararTemporizador()
        EjecucionDatosConsulta()
    End Sub

#End Region

End Class

Public Class clsParam_Scripts

    <JsonProperty("IDC", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIdCompania As Nullable(Of Integer)

    <JsonProperty("IDS", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIDScript As Integer

    <JsonProperty("NOM", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrNombreScript As String

    <JsonProperty("PAR", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrParametros As String

    <JsonProperty("US", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrUsuario As String

    <JsonProperty("MA", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrMaquina As String

    <JsonProperty("INF", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrInfoConexion As String

End Class
