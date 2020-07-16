Imports Telerik.Windows.Controls
Imports A2ControlMenu
Imports A2MCCoreWPF
Imports Newtonsoft.Json
Imports SpreadsheetGear
Imports System.Globalization
Imports System.Web
Imports Newtonsoft.Json.Linq
Imports A2Utilidades.Mensajes
Imports System.Threading.Tasks
Imports A2.Riesgos.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2.Notificaciones.Cliente
Imports GalaSoft.MvvmLight.Messaging

Public Class MotorCalculoViewModel
    Inherits A2ControlMenu.A2ViewModel

    Public objMotorSL As clsMotorCliente = New clsMotorCliente(Program.RutaServicioMotorCalculo)
    Private mdcProxy As RiesgosDomainContext

#Region "Propiedades"

    Private _IsBusyVersiones As Boolean = False
    Public Property IsBusyVersiones() As Boolean
        Get
            Return _IsBusyVersiones
        End Get
        Set(ByVal value As Boolean)
            _IsBusyVersiones = value
            MyBase.CambioItem("IsBusyVersiones")
            If _IsBusyVersiones Then
                VerBotonesVersiones = Visibility.Collapsed
            Else
                VerBotonesVersiones = Visibility.Visible
            End If
        End Set
    End Property

    Private _VerBotonesVersiones As Visibility = Visibility.Visible
    Public Property VerBotonesVersiones() As Visibility
        Get
            Return _VerBotonesVersiones
        End Get
        Set(ByVal value As Visibility)
            _VerBotonesVersiones = value
            MyBase.CambioItem("VerBotonesVersiones")
        End Set
    End Property


    Private _ListaMetodos As List(Of clsInfoMetodos) = New List(Of clsInfoMetodos)
    Public Property ListaMetodos() As List(Of clsInfoMetodos)
        Get
            Return _ListaMetodos
        End Get
        Set(ByVal value As List(Of clsInfoMetodos))
            _ListaMetodos = value
            MyBase.CambioItem("ListaMetodos")
        End Set
    End Property

    Private _ListaVersiones As List(Of clsInfoVersion) = New List(Of clsInfoVersion)
    Public Property ListaVersiones() As List(Of clsInfoVersion)
        Get
            Return _ListaVersiones
        End Get
        Set(ByVal value As List(Of clsInfoVersion))
            _ListaVersiones = value
            MyBase.CambioItem("ListaMetodos")
        End Set
    End Property

    Private _ListaVersionesPorMetodo As List(Of clsInfoVersion) = New List(Of clsInfoVersion)
    Public Property ListaVersionesPorMetodo() As List(Of clsInfoVersion)
        Get
            Return _ListaVersionesPorMetodo
        End Get
        Set(ByVal value As List(Of clsInfoVersion))
            _ListaVersionesPorMetodo = value
            MyBase.CambioItem("ListaVersionesPorMetodo")
        End Set
    End Property

    Private _MetodoSeleccionado As clsInfoMetodos = New clsInfoMetodos
    Public Property MetodoSeleccionado() As clsInfoMetodos
        Get
            Return _MetodoSeleccionado
        End Get
        Set(ByVal value As clsInfoMetodos)
            _MetodoSeleccionado = value
            ObtenerVersionesPorMetodo()
            MyBase.CambioItem("MetodoSeleccionado")
        End Set
    End Property

    Private _NuevoMetodo As clsInfoMetodos = New clsInfoMetodos
    Public Property NuevoMetodo() As clsInfoMetodos
        Get
            Return _NuevoMetodo
        End Get
        Set(ByVal value As clsInfoMetodos)
            _NuevoMetodo = value
            MyBase.CambioItem("NuevoMetodo")
        End Set
    End Property

    Private _NuevaVersion As clsInfoVersion = New clsInfoVersion
    Public Property NuevaVersion() As clsInfoVersion
        Get
            Return _NuevaVersion
        End Get
        Set(ByVal value As clsInfoVersion)
            _NuevaVersion = value
            MyBase.CambioItem("NuevaVersion")
        End Set
    End Property

    Private _NuevoLibroMetodo As IWorkbook
    Public Property NuevoLibroMetodo() As IWorkbook
        Get
            Return _NuevoLibroMetodo
        End Get
        Set(ByVal value As IWorkbook)
            _NuevoLibroMetodo = value
            MyBase.CambioItem("NuevoLibroMetodo")
        End Set
    End Property

    Private _NuevoLibroVersion As IWorkbook
    Public Property NuevoLibroVersion() As IWorkbook
        Get
            Return _NuevoLibroVersion
        End Get
        Set(ByVal value As IWorkbook)
            _NuevoLibroVersion = value
            MyBase.CambioItem("NuevoLibroVersion")
        End Set
    End Property

    Private _VersionSeleccionada As clsInfoVersion = Nothing
    Public Property VersionSeleccionada() As clsInfoVersion
        Get
            Return _VersionSeleccionada
        End Get
        Set(ByVal value As clsInfoVersion)
            _VersionSeleccionada = value
            MyBase.CambioItem("VersionSeleccionada")
        End Set
    End Property

    Private _RangoTexto As String
    Public Property RangoTexto() As String
        Get
            Return _RangoTexto
        End Get
        Set(ByVal value As String)
            _RangoTexto = value
        End Set
    End Property

    Private _TituloMetodo As String
    Public Property TituloMetodo() As String
        Get
            Return _TituloMetodo
        End Get
        Set(ByVal value As String)
            _TituloMetodo = value
            MyBase.CambioItem("TituloMetodo")
        End Set
    End Property

    Private _ListaBotonesMC As List(Of ToolbarsPorAplicacion)
    Public Property ListaBotonesMC() As List(Of ToolbarsPorAplicacion)
        Get
            Return _ListaBotonesMC
        End Get
        Set(ByVal value As List(Of ToolbarsPorAplicacion))
            _ListaBotonesMC = value
            MyBase.CambioItem("ListaBotonesMC")
        End Set
    End Property

    Private _mcSubirConfiguracionVisibility As Boolean
    Public Property mcSubirConfiguracionVisibility() As Boolean
        Get
            Return _mcSubirConfiguracionVisibility
        End Get
        Set(ByVal value As Boolean)
            _mcSubirConfiguracionVisibility = value
            MyBase.CambioItem("mcSubirConfiguracionVisibility")
        End Set
    End Property

    Private _mcBajarConfiguracionVisibility As Boolean = False
    Public Property mcBajarConfiguracionVisibility() As Boolean
        Get
            Return _mcBajarConfiguracionVisibility
        End Get
        Set(ByVal value As Boolean)
            _mcBajarConfiguracionVisibility = value
            MyBase.CambioItem("mcBajarConfiguracionVisibility")
        End Set
    End Property

    Private _mcNuevoMetodoVisibility As Boolean = False
    Public Property mcNuevoMetodoVisibility() As Boolean
        Get
            Return _mcNuevoMetodoVisibility
        End Get
        Set(ByVal value As Boolean)
            _mcNuevoMetodoVisibility = value
            MyBase.CambioItem("mcNuevoMetodoVisibility")
        End Set
    End Property

    Private _mcBorrarMetodoVisibility As Boolean = False
    Public Property mcBorrarMetodoVisibility() As Boolean
        Get
            Return _mcBorrarMetodoVisibility
        End Get
        Set(ByVal value As Boolean)
            _mcBorrarMetodoVisibility = value
            MyBase.CambioItem("mcBorrarMetodoVisibility")
        End Set
    End Property

    Private _btnNuevaVersionVisibility As Boolean = False
    Public Property btnNuevaVersionVisibility() As Boolean
        Get
            Return _btnNuevaVersionVisibility
        End Get
        Set(ByVal value As Boolean)
            _btnNuevaVersionVisibility = value
            MyBase.CambioItem("btnNuevaVersionVisibility")
        End Set
    End Property

    Private _btnNuevaConsultaVisibility As Boolean = False
    Public Property btnNuevaConsultaVisibility() As Boolean
        Get
            Return _btnNuevaConsultaVisibility
        End Get
        Set(ByVal value As Boolean)
            _btnNuevaConsultaVisibility = value
            MyBase.CambioItem("btnNuevaConsultaVisibility")
        End Set
    End Property

    Private _btnBorarConsultaVisibility As Boolean = False
    Public Property btnBorarConsultaVisibility() As Boolean
        Get
            Return _btnBorarConsultaVisibility
        End Get
        Set(ByVal value As Boolean)
            _btnBorarConsultaVisibility = value
            MyBase.CambioItem("btnBorarConsultaVisibility")
        End Set
    End Property

    Private _mnuSeleccionarVersionVisibility As Boolean = False
    Public Property mnuSeleccionarVersionVisibility() As Boolean
        Get
            Return _mnuSeleccionarVersionVisibility
        End Get
        Set(ByVal value As Boolean)
            _mnuSeleccionarVersionVisibility = value
            MyBase.CambioItem("mnuSeleccionarVersionVisibility")
        End Set
    End Property

    Private _mnuCancelarVersionVisibility As Boolean = False
    Public Property mnuCancelarVersionVisibility() As Boolean
        Get
            Return _mnuCancelarVersionVisibility
        End Get
        Set(ByVal value As Boolean)
            _mnuCancelarVersionVisibility = value
            MyBase.CambioItem("mnuCancelarVersionVisibility")
        End Set
    End Property

    Private _btnBorrarVersionVisibility As Boolean = False
    Public Property btnBorrarVersionVisibility() As Boolean
        Get
            Return _btnBorrarVersionVisibility
        End Get
        Set(ByVal value As Boolean)
            _btnBorrarVersionVisibility = value
            MyBase.CambioItem("btnBorrarVersionVisibility")
        End Set
    End Property

    Private _btnBajarVersionVisibility As Boolean = False
    Public Property btnBajarVersionVisibility() As Boolean
        Get
            Return _btnBajarVersionVisibility
        End Get
        Set(ByVal value As Boolean)
            _btnBajarVersionVisibility = value
            MyBase.CambioItem("btnBajarVersionVisibility")
        End Set
    End Property

    Private _BKRequiereAutorizacion As Boolean = False
    Public Property BKRequiereAutorizacion As Boolean
        Get
            Return _BKRequiereAutorizacion
        End Get
        Set(ByVal value As Boolean)
            _BKRequiereAutorizacion = value
            MyBase.CambioItem("BKRequiereAutorizacion")
        End Set
    End Property

    Private _ConfiguracionAutorizaciones As AutorizacionesConfiguracion
    Public Property ConfiguracionAutorizaciones As AutorizacionesConfiguracion
        Get
            Return _ConfiguracionAutorizaciones
        End Get
        Set(ByVal value As AutorizacionesConfiguracion)
            _ConfiguracionAutorizaciones = value
            MyBase.CambioItem("ConfiguracionAutorizaciones")
        End Set
    End Property

    Private _PrioridadPorDefecto As AutorizacionesPrioridad
    Public Property PrioridadPorDefecto As AutorizacionesPrioridad
        Get
            Return _PrioridadPorDefecto
        End Get
        Set(ByVal value As AutorizacionesPrioridad)
            _PrioridadPorDefecto = value
            MyBase.CambioItem("PrioridadPorDefecto")
        End Set
    End Property

    Private _AssemblyMC As String = String.Empty
    Public ReadOnly Property AssemblyMC As String
        Get
            If _AssemblyMC.Length = 0 Then
                _AssemblyMC = CType(GetType(A2MCCOREWPF.Accion).Assembly.GetCustomAttributes(GetType(Reflection.AssemblyFileVersionAttribute), False)(0), Reflection.AssemblyFileVersionAttribute).Version
            End If
            Return _AssemblyMC
        End Get
    End Property

#End Region


#Region "Métodos"



    Public Async Function ObtenerRangoLibroConfiguracion(strRangoID As String) As Task(Of String)

        IsBusy = True
        Dim resultado As String = String.Empty

        Try
            resultado = Await objMotorSL.ObtenerRangoLibroConfiguracionTaskAsync(strRangoID)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar los parametros.", Me.ToString(), "ObtenerRangoLibroConfiguracion", Program.TituloSistema, Program.Maquina, ex)
        End Try

        If Not String.IsNullOrEmpty(resultado) Then
            RangoTexto = resultado
        Else
            RangoTexto = String.Empty
        End If

        IsBusy = False

        Return RangoTexto

    End Function

    ''' <summary>
    ''' Obtiene los métodos del libro de configuración
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function ObtenerListaMetodos(Optional ByVal pstrMetodoASeleccionar As String = "") As Task(Of Boolean)
        Try
            ListaMetodos = Await objMotorSL.ObtenerMetodosTaskAsync()

            If ListaMetodos IsNot Nothing Then
                If ListaMetodos.Count = 1 Then
                    MetodoSeleccionado = ListaMetodos.First
                Else
                    If Not String.IsNullOrEmpty(pstrMetodoASeleccionar) Then
                        If ListaMetodos.Where(Function(i) i.Nombre = pstrMetodoASeleccionar).Count > 0 Then
                            MetodoSeleccionado = ListaMetodos.Where(Function(i) i.Nombre = pstrMetodoASeleccionar).First
                        End If
                    End If
                End If
            End If
            Return True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar los metodos.", Me.ToString(), "ObtenerListaMetodos", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Obtiene las versiones del libro del configuración
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function ObtenerListaVersiones() As Task(Of Boolean)
        Try
            ListaVersiones = Await objMotorSL.ObtenerVersionesMetodoTaskAsync(String.Empty)
            Return True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las versiones.", Me.ToString(), "ObtenerListaVersiones", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Filtra los métodos basado en la versión seleccionada
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function ObtenerVersionesPorMetodo() As Task(Of Boolean)
        Try
            If MetodoSeleccionado IsNot Nothing Then
                IsBusyVersiones = True
                ListaVersionesPorMetodo = Await objMotorSL.ObtenerVersionesMetodoTaskAsync(MetodoSeleccionado.Nombre)
                IsBusyVersiones = False
            End If
            Return True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las versiones.", Me.ToString(), "ObtenerVersionesPorMetodo", Program.TituloSistema, Program.Maquina, ex)
            IsBusyVersiones = False
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Filtra los métodos basado en la versión seleccionada
    ''' </summary>
    ''' <remarks></remarks>
    Public Async Function ObtenerVersionesMetodoEspecifico(ByVal pstrNombreMetodo As String) As Task(Of List(Of clsInfoVersion))
        Try
            Dim objListaRetorno As List(Of clsInfoVersion) = Await objMotorSL.ObtenerVersionesMetodoTaskAsync(MetodoSeleccionado.Nombre)
            Return objListaRetorno
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las versiones.", Me.ToString(), "ObtenerVersionesMetodoEspecifico", Program.TituloSistema, Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Adiciona un nuevo método en el libro de configuración del motor de cálculo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function AdicionarMetodo() As System.Threading.Tasks.Task(Of Boolean)
        IsBusy = True

        Dim resultadoConfiguracion As Boolean = False

        Try
            'Creo la estructura de carpetas para el nuevo método
            Dim resultado As Boolean = Await objMotorSL.CrearLibroRiesgoTaskAsync(NuevoMetodo.Nombre, NuevoLibroMetodo, NuevoMetodo.Version)
            'Si fue exitosa la creación de carpetas, creo el método en el archivo de configuración
            If resultado Then
                ListaMetodos = Nothing
                ListaVersiones = Nothing
                ListaVersionesPorMetodo = Nothing
                Await ObtenerListaMetodos()
                Await ObtenerListaVersiones()
                Await ObtenerVersionesPorMetodo()

                resultadoConfiguracion = True
            Else
                mostrarMensaje("No fue posible crear el libro del método. Por favor revise el log de errores con el administrador.", "AdicionarMetodo", A2Utilidades.wppMensajes.TiposMensaje.Errores)
                resultadoConfiguracion = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al adicionar el metodo.", Me.ToString(), "AdicionarMetodo", Program.TituloSistema, Program.Maquina, ex)
        End Try
        IsBusy = False

        Return resultadoConfiguracion
    End Function

    ''' <summary>
    ''' Adiciona un nuevo método en el libro de configuración del motor de cálculo y modifica la versión con la que trabaja el método
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function AdicionarVersion(ByVal strDatosVersion As String, ByVal dtFechaInicial As DateTime) As System.Threading.Tasks.Task(Of Boolean)
        IsBusy = True

        Dim resultadoConfiguracion As Boolean = False
        Dim strNombreMetodo As String = MetodoSeleccionado.Nombre
        Dim strSeguimientoError As String = String.Empty

        Try
            strSeguimientoError = "Crear libro riesgos"
            'Creo el libro de la nueva versión del método
            Dim resultado As Boolean = Await objMotorSL.CrearLibroRiesgoTaskAsync(MetodoSeleccionado.Nombre, NuevoLibroVersion, strDatosVersion)
            strSeguimientoError = "Termina crear libro riesgos"

            If resultado Then

            Else
                mostrarMensaje("No fue posible crear la versión. Por favor revise el log de errores con el administrador.", "AdicionarVersion", A2Utilidades.wppMensajes.TiposMensaje.Errores)
                resultadoConfiguracion = False
            End If

            'Si fue exitosa la creación del método en el libro, cargo nuevamente el objeto con la lista de métodos
            strSeguimientoError = "Realiza notificación"

            'envio la notificacion a los usuarios
            Dim notifica As New clsNotificacion
            notifica.dtmFechaEnvio = DateTime.Now
            notifica.strMaquina = Program.Maquina
            notifica.strUsuario = Program.UsuarioAutenticado
            notifica.strMensajeConsola = String.Format(Program.GSTR_MENSAJE_NUEVAVERSIONRIESGO, Program.Usuario, MetodoSeleccionado.Nombre)
            Messenger.Default.Send(New clsNotificacionCliente() With {.objInfoNotificacion = notifica})

            'Recarga la Cache de SQL
            strSeguimientoError = "Refresca cache de SQL"
            Await RecargarCacheSQL(strNombreMetodo)
            strSeguimientoError = "Termina refrescar cache de SQL"

            strSeguimientoError = "Termina obtener lista metodos"
            Dim strMetodoSeleccionado As String = MetodoSeleccionado.Nombre
            ListaMetodos = Nothing
            MetodoSeleccionado = Nothing

            Await ObtenerListaMetodos(strMetodoSeleccionado)
            Await ObtenerListaVersiones()

            strSeguimientoError = "Termina obtener lista versiones"

            resultadoConfiguracion = True
        Catch ex As Exception
            resultadoConfiguracion = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, strSeguimientoError & ": Se genero un problema al momento de adcionar la versión.",
                                 Me.ToString(), "NuevaVersionView.AdicionarVersion", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False

        Return resultadoConfiguracion
    End Function

    ''' <summary>
    ''' Elimina un registro de método del libro de configuración de motor de cálculo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function EliminarMetodo() As System.Threading.Tasks.Task(Of Boolean)
        IsBusy = True
        Dim resultadoConfiguracion As Boolean = False

        Try

            Dim resultado As Boolean = Await objMotorSL.EliminarRiesgoTaskAsync(MetodoSeleccionado.Nombre)

            If resultado Then
                'Elimino el riesgo en el administrador de seguridad
                Dim nombreRiesgo As String = MetodoSeleccionado.Nombre.Split(".").Last
                Dim objRet As InvokeOperation(Of Boolean)

                objRet = Await mdcProxy.EliminarRiesgoSync(Program.Aplicacion, Program.VersionAplicacion, nombreRiesgo, Program.UsuarioAutenticado, Program.HashConexion).AsTask()

                ListaVersionesPorMetodo = New List(Of clsInfoVersion)
                MetodoSeleccionado = Nothing
                ListaMetodos = Nothing
                ListaVersiones = Nothing
                ListaVersionesPorMetodo = Nothing
                Await ObtenerListaMetodos()
                Await ObtenerListaVersiones()
                Await ObtenerVersionesPorMetodo()
            Else
                mostrarMensaje("No fue posible eliminar el metodo. Por favor revise el log de errores con el administrador.", "EliminarMetodo", A2Utilidades.wppMensajes.TiposMensaje.Errores)
                resultadoConfiguracion = False
            End If
        Catch ex As Exception
            resultadoConfiguracion = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al eliminar el método.", Me.ToString(), "EliminarMetodo", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
        Return resultadoConfiguracion
    End Function


    ''' <summary>
    ''' Elimina una versión del libro de configuración del motor de cálculo y disminuye la versión con la que trabaja el método
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function EliminarVersion() As System.Threading.Tasks.Task(Of Boolean)
        IsBusy = True
        Dim resultadoConfiguracion As Boolean = False

        Try
            Dim strNombreMetodo As String = MetodoSeleccionado.Nombre

            Dim lstVersiones As List(Of clsInfoVersion) = ListaVersionesPorMetodo

            If lstVersiones IsNot Nothing Then
                Dim maximaVersion = (From item In lstVersiones
                                     Select item.Version).Max
                Dim cantidadVersionesXMetodo = lstVersiones.Count
                If cantidadVersionesXMetodo > 1 Then
                    Dim objMaximaVersionXMetodo = (From item In lstVersiones Where item.Version = maximaVersion).SingleOrDefault()

                    If objMaximaVersionXMetodo IsNot Nothing Then

                        If objMaximaVersionXMetodo.Version = VersionSeleccionada.Version Then


                            'solo se eliminan las versiones aprobadas
                            If objMaximaVersionXMetodo.Aprobado Then

                                'Verifico que la nueva version "actual" sea de la misma distribucion de la que se va a eliminar
                                Dim objProximo As clsInfoVersion = (From item In lstVersiones
                                                                    Where item.Aprobado And item.Version <> objMaximaVersionXMetodo.Version
                                                                    Select item Order By item.Version Descending).FirstOrDefault()
                                If objProximo.Assembly = objMaximaVersionXMetodo.Assembly Then
                                    Dim resultado As Boolean = Await objMotorSL.EliminarRiesgoVersionTaskAsync(MetodoSeleccionado.Nombre, JsonConvert.SerializeObject(objMaximaVersionXMetodo))

                                    If resultado Then
                                        'Recarga la Cache de SQL
                                        Await RecargarCacheSQL(strNombreMetodo)

                                        Dim strMetodoSeleccionado As String = MetodoSeleccionado.Nombre
                                        ListaMetodos = Nothing
                                        MetodoSeleccionado = Nothing

                                        Await ObtenerListaMetodos(strMetodoSeleccionado)
                                        Await ObtenerListaVersiones()

                                        resultadoConfiguracion = True
                                    Else
                                        mostrarMensaje("No fue posible eliminar la versión. Por favor revise el log de errores con el administrador.", "EliminarVersion", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        resultadoConfiguracion = False
                                    End If
                                Else
                                    resultadoConfiguracion = False
                                    mostrarMensaje("La distribucion del libro seleccionado es diferente al libro destino y no puede ser eliminado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            Else
                                resultadoConfiguracion = False
                                mostrarMensaje("La versión del libro seleccionado esta pendiente por aprobar y no puede ser eliminado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If

                        Else
                            resultadoConfiguracion = False
                            mostrarMensaje("No es posible eliminar versiones anteriores del libro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If

                    End If
                Else
                    mostrarMensaje("No es posible eliminar el registro, el método debe tener al menos una versión", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

        Catch ex As Exception
            resultadoConfiguracion = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al eliminar el método.", Me.ToString(), "EliminarVersion", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False

        Return resultadoConfiguracion
    End Function

    ''' <summary>
    ''' Adiciona un nuevo método en el libro de configuración del motor de cálculo y modifica la versión con la que trabaja el método
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function RecargarCacheSQL(ByVal pstrNombreMetodo As String) As System.Threading.Tasks.Task(Of Boolean)
        Dim logResultado As Boolean = False
        Dim objRet As InvokeOperation(Of Boolean)

        Try
            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyRiesgos()
            End If

            objRet = Await mdcProxy.RecargarCacheSQL(pstrNombreMetodo, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al recargar la cache de SQL.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al recargar la cache de SQL.", Me.ToString(), "RecargarCacheSQL", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al recargar la cache de SQL", Me.ToString(), "RecargarCacheSQL", Application.Current.ToString(), Program.Maquina, ex)
        Finally
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Obtiene el libro del método basado en la versión seleccionada
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function DecargarArchivoVersion() As System.Threading.Tasks.Task(Of IWorkbook)
        Dim libroExcel As IWorkbook = Await objMotorSL.DescargarArchivoRiesgoVersionTaskAsync(MetodoSeleccionado.Nombre.Replace(".", "@"), VersionSeleccionada.Version)

        Return libroExcel
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strNombreMetodo"></param>
    ''' <param name="iwLibroRiesgo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ValidarRangos(ByVal strNombreMetodo As String, ByVal iwLibroRiesgo As IWorkbook)
        Dim valido As Boolean = True
        Dim rangoParametrosVisualizacion As String = String.Empty

        iwLibroRiesgo.WorkbookSet.GetLock()
        Try
            Dim objCORE As clsCORE = New clsCORE(iwLibroRiesgo, CultureInfo.CurrentCulture)
            rangoParametrosVisualizacion = objCORE.ObtenerValorRango(Program.RANGO_PARAMETROS_VISUALIZACION, Program.TABLA, False)
        Finally
            iwLibroRiesgo.WorkbookSet.ReleaseLock()
        End Try

        Dim lstRiesgo As List(Of Riesgo) = Riesgo.ConvertirALista(rangoParametrosVisualizacion)

        If lstRiesgo IsNot Nothing Then

            Dim objRiesgo As Riesgo = lstRiesgo(0)

            If objRiesgo.ID.ToString().Trim.ToUpper() <> strNombreMetodo.ToString().Trim.ToUpper() Then

                valido = False
            End If
        End If

        Return valido
    End Function

    Public Function ValidarExisteRiesgoRegistrado(ByVal idRiesgo As String) As Boolean
        Dim valido As Boolean = True
        Dim rangoRiesgos As String = String.Empty

        Dim existe = (From item In ListaMetodos Where item.Nombre.Split(".").First().ToString().Trim().ToUpper() = idRiesgo.ToString().Trim().ToUpper()).SingleOrDefault()

        If existe IsNot Nothing Then
            If Not String.IsNullOrEmpty(existe.Nombre) Then
                valido = False
            End If
        End If
        Return valido
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de las alertas para una fecha especifica
    ''' </summary>
    Public Async Function ConsultarBotonesMC() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ToolbarsPorAplicacion)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyRiesgos()
            End If

            mdcProxy.ToolbarsPorAplicacions.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarBotonesXToolbarSyncQuery(Program.Aplicacion, Program.VersionAplicacion, Program.FRM_ADMIN_MC, Program.UsuarioAutenticado, Program.ClaveUsuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar los botones del toolbar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las alertas.", Me.ToString(), "ConsultarBotonesMC", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaBotonesMC = CType(objRet.Entities.ToList(), List(Of ToolbarsPorAplicacion))

                    For Each item In ListaBotonesMC

                        Select Case item.strNombreBoton

                            Case Program.MC_BAJAR_CONFIGURACION
                                mcBajarConfiguracionVisibility = True

                            Case Program.MC_BORRAR_METODO
                                mcBorrarMetodoVisibility = True

                            Case Program.MC_NUEVO_METODO
                                mcNuevoMetodoVisibility = True

                            Case Program.MC_SUBIR_CONFIGURACION
                                mcSubirConfiguracionVisibility = True

                            Case Program.BTN_BAJAR_VERSION
                                btnBajarVersionVisibility = True

                            Case Program.BTN_BORRAR_VERSION
                                btnBorrarVersionVisibility = True

                            Case Program.BTN_NUEVA_VERSION
                                btnNuevaVersionVisibility = True

                            Case Program.MNU_SELECCIONAR_VERSION
                                mnuSeleccionarVersionVisibility = True

                            Case Program.MNU_CANCELAR_VERSION
                                mnuCancelarVersionVisibility = True

                            Case Program.BTN_NUEVA_CONSULTA
                                btnNuevaConsultaVisibility = True

                            Case Program.BTN_BORRAR_CONSULTA
                                btnBorarConsultaVisibility = True

                        End Select
                    Next
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en al consultar los botones del toolbar ", Me.ToString(), "ConsultarBotonesMC", Application.Current.ToString(), Program.Maquina, ex)
        Finally
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Funcion para consultar asincronicamente la configuracion de autorizaciones
    ''' </summary>
    ''' <returns>retorna un objeto de tipo AutorizacionesConfiguracion</returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarConfiguracionAutorizaciones() As Task(Of Boolean)
        Try
            ConfiguracionAutorizaciones = Await objMotorSL.ObtenerConfiguracionAutorizacionesTaskAsync()

            If ConfiguracionAutorizaciones IsNot Nothing Then
                If ConfiguracionAutorizaciones.Prioridades IsNot Nothing Then
                    ConfiguracionAutorizaciones.Prioridades = (From item In ConfiguracionAutorizaciones.Prioridades Order By item.IDPrioridad Ascending).ToList()

                    PrioridadPorDefecto = ConfiguracionAutorizaciones.PrioridadDefecto
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la configuracion de autorizaciones", Me.ToString(), "ConsultarConfiguracionAutorizaciones", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
        Return True
    End Function


    ''' <summary>
    ''' Metodo para cancelar una version del sistema de autorizaciones
    ''' </summary>
    ''' <param name="objData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Async Function CancelarVersion(objData As clsInfoVersion) As Task(Of Boolean)
        Dim resultadoConfiguracion As Boolean = False
        Try
            IsBusy = True
            'verifico los parametros de autorizaciones
            If ConfiguracionAutorizaciones Is Nothing Then
                Dim bitResultado = Await ConsultarConfiguracionAutorizaciones()
                If Not bitResultado Then
                    Return False
                End If
            End If
            'retiro el documento
            Dim objDocumento As New AutorizacionesIngresoDocumento
            With objDocumento
                .TipoDocumento = ConfiguracionAutorizaciones.IDDocumento
                .NroDocumento = objData.Metodo
                .NroDetalleDocumento = String.Format("{0}_{1}", objData.Metodo, objData.Version)
                .VersionDocumento = objData.Version
                .InformacionDocumento = "<documento><encabezado infodocumento=""Versión cancelada desde la consola de administración del motor cálculos, Usuario: " & Program.UsuarioAutenticado & """/></documento>"
                .InsertarRetornoEnTemporal = False
                .NombreTablaTemporal = String.Empty
            End With
            Dim objResultado As AutorizacionesDocumento = Await objMotorSL.RetirarDocumentoAutorizacionesTaskAsync(objDocumento)
            If objResultado Is Nothing OrElse objResultado.Codigo > 0 Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retirar el documento del sistema de autorizaciones", Me.ToString(), "CancelarVersion", Application.Current.ToString(), Program.Maquina, If(objResultado Is Nothing, New Exception(), New Exception(objResultado.Mensaje)))
                Return False
            End If

            ListaMetodos = Nothing
            ListaVersiones = Nothing
            ListaVersionesPorMetodo = Nothing
            Await ObtenerListaMetodos()
            Await ObtenerListaVersiones()
            Await ObtenerVersionesPorMetodo()

            resultadoConfiguracion = True

            Return resultadoConfiguracion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al retirar el documento del sistema de autorizaciones.", Me.ToString(), "CancelarVersion", Program.TituloSistema, Program.Maquina, ex)
            Return False
        End Try
        IsBusy = False
    End Function

    ''' <summary>
    ''' Metodo para cambiar la version actual por la seleccionada
    ''' </summary>
    ''' <param name="objNuevaVersion">objeto de tipo clsInfoVersion</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function SeleccionarVersion(objNuevaVersion As clsInfoVersion) As Boolean
        Dim resultadoConfiguracion As Boolean = False
        Dim intVersionAnterior As Integer
        Try
            IsBusy = True

            'envio la notificacion a los usuarios
            Dim notifica As New clsNotificacion
            notifica.dtmFechaEnvio = DateTime.Now
            notifica.strMaquina = Program.Maquina
            notifica.strUsuario = Program.UsuarioAutenticado
            notifica.strMensajeConsola = String.Format(Program.GSTR_MENSAJE_SELECCIONARVERSIONRIESGO, Program.Usuario, MetodoSeleccionado.Nombre, intVersionAnterior, objNuevaVersion.Version)
            Messenger.Default.Send(New clsNotificacionCliente() With {.objInfoNotificacion = notifica})

            resultadoConfiguracion = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al cambiar la versión del metodo.", Me.ToString(), "CancelarVersion", Program.TituloSistema, Program.Maquina, ex)
            resultadoConfiguracion = False
        End Try

        IsBusy = False

        Return resultadoConfiguracion
    End Function

#End Region

#Region "Resultados Asíncronos"

#End Region

#Region "Commands"

#End Region

End Class
