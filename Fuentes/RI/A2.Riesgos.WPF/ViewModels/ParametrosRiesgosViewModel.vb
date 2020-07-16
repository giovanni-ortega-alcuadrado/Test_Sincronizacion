Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.Riesgos.RIA.Web
Imports SpreadsheetGear
Imports A2MCCoreWPF
Imports System.Globalization
Imports SpreadsheetGear.Windows.Controls
Imports GalaSoft.MvvmLight.Messaging
Imports A2.Notificaciones.Cliente

Public Class ParametrosRiesgosViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim objMotorSL As clsMotorCliente = New clsMotorCliente(Program.RutaServicioMotorCalculo)


#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"
    Private _HabilitarDetalle As Visibility = Visibility.Collapsed
    Public Property HabilitarDetalle() As Visibility
        Get
            Return _HabilitarDetalle
        End Get
        Set(ByVal value As Visibility)
            _HabilitarDetalle = value
            MyBase.CambioItem("HabilitarDetalle")
        End Set
    End Property

    Private _LibroRiesgos As IWorkbook
    Public Property LibroRiesgos() As IWorkbook
        Get
            Return _LibroRiesgos
        End Get
        Set(ByVal value As IWorkbook)
            _LibroRiesgos = value
            MyBase.CambioItem("LibroRiesgos")
        End Set
    End Property

    Private _IDCategoriaSeleccionada As String
    Public Property IDCategoriaSeleccionada() As String
        Get
            Return _IDCategoriaSeleccionada
        End Get
        Set(ByVal value As String)
            _IDCategoriaSeleccionada = value
            MyBase.CambioItem("IDCategoriaSeleccionada")
        End Set
    End Property

    Private _IDRiesgoSeleccionado As String
    Public Property IDRiesgoSeleccionado() As String
        Get
            Return _IDRiesgoSeleccionado
        End Get
        Set(ByVal value As String)
            _IDRiesgoSeleccionado = value
            MyBase.CambioItem("IDRiesgoSeleccionado")
        End Set
    End Property

    Private _ListaRiesgos As List(Of Riesgo) = New List(Of Riesgo)
    Public Property ListaRiesgos() As List(Of Riesgo)
        Get
            Return _ListaRiesgos
        End Get
        Set(ByVal value As List(Of Riesgo))
            _ListaRiesgos = value
            MyBase.CambioItem("ListaRiesgos")
        End Set
    End Property

    Private _ListaCategorias As List(Of ParametrosConfiguracion) = New List(Of ParametrosConfiguracion)
    Public Property ListaCategorias() As List(Of ParametrosConfiguracion)
        Get
            Return _ListaCategorias
        End Get
        Set(ByVal value As List(Of ParametrosConfiguracion))
            _ListaCategorias = value
            MyBase.CambioItem("ListaCategorias")
        End Set
    End Property

    Private _ListaVariables As List(Of RangoConNombre) = New List(Of RangoConNombre)
    Public Property ListaVariables() As List(Of RangoConNombre)
        Get
            Return _ListaVariables
        End Get
        Set(ByVal value As List(Of RangoConNombre))
            _ListaVariables = value
            MyBase.CambioItem("ListaVariables")
        End Set
    End Property

    Private _ListaVariablesFiltro As List(Of RangoConNombre) = New List(Of RangoConNombre)
    Public Property ListaVariablesFiltro() As List(Of RangoConNombre)
        Get
            Return _ListaVariablesFiltro
        End Get
        Set(ByVal value As List(Of RangoConNombre))
            _ListaVariablesFiltro = value
            MyBase.CambioItem("ListaVariablesFiltro")
        End Set
    End Property

    Private _RiesgoSeleccionado As New Riesgo
    Public Property RiesgoSeleccionado() As Riesgo
        Get
            Return _RiesgoSeleccionado
        End Get
        Set(ByVal value As Riesgo)
            _RiesgoSeleccionado = value
            MyBase.CambioItem("RiesgoSeleccionado")
        End Set
    End Property

    Private _IsBusyFiltro As New Boolean
    Public Property IsBusyFiltro() As Boolean
        Get
            Return _IsBusyFiltro
        End Get
        Set(ByVal value As Boolean)
            _IsBusyFiltro = value
            MyBase.CambioItem("IsBusyFiltro")
        End Set
    End Property

    Private _FiltroVariables As String = String.Empty
    Public Property FiltroVariables() As String
        Get
            Return _FiltroVariables
        End Get
        Set(ByVal value As String)
            _FiltroVariables = value
            MyBase.CambioItem("FiltroVariables")
        End Set
    End Property
#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"


    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    Public Overrides Async Sub ActualizarRegistro()
        Try
            ErrorForma = String.Empty
            IsBusy = True

            If (LibroRiesgos) IsNot Nothing Then
                MostrarOcultarPropiedadesDelLibro(True)
                Dim result = Await objMotorSL.ActualizarLibroRiesgoTaskAsync(IDRiesgoSeleccionado, LibroRiesgos)
                If result Then
                    A2Utilidades.Mensajes.mostrarMensaje("Parámetros actualizados correctamente", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                    Dim notifica As New clsNotificacion
                    notifica.dtmFechaEnvio = DateTime.Now
                    notifica.strMaquina = Program.Maquina
                    notifica.strUsuario = Program.UsuarioAutenticado
                    notifica.strMensajeConsola = String.Format(Program.GSTR_MENSAJE_CAMBIOLIBRORIESGO, Program.Usuario, RiesgoSeleccionado.Titulo)
                    Messenger.Default.Send(New clsNotificacionCliente() With {.objInfoNotificacion = notifica})
                End If
                MostrarOcultarPropiedadesDelLibro(False)
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    Private Sub MostrarOcultarPropiedadesDelLibro(ByVal mostrar As Boolean)
        Try

            LibroRiesgos.WorkbookSet.GetLock()
            Try
                LibroRiesgos.ActiveWorksheet.WindowInfo.DisplayHeadings = mostrar

                LibroRiesgos.WindowInfo.DisplayHorizontalScrollBar = mostrar
                LibroRiesgos.WindowInfo.DisplayVerticalScrollBar = mostrar
                LibroRiesgos.WindowInfo.DisplayWorkbookTabs = mostrar
            Finally
                LibroRiesgos.WorkbookSet.ReleaseLock()
            End Try

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar/ocultar las propiedades del libro.", Me.ToString(), "MostrarOcultarPropiedadesDelLibro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta una consulta para llenar la lista de riesgos.
    ''' </summary>
    Public Async Function ConsultarLibroRiesgoVM() As System.Threading.Tasks.Task
        Try
            LibroRiesgos = Factory.GetWorkbook()

            If IDRiesgoSeleccionado IsNot Nothing Then

                If ListaRiesgos IsNot Nothing Then
                    Dim objRiesgo As Riesgo = (From item In ListaRiesgos.ToList() Where item.ID = IDRiesgoSeleccionado.ToString() Select item).FirstOrDefault()

                    If objRiesgo IsNot Nothing Then

                        Dim objLibroRiesgo As IWorkbook = Factory.GetWorkbook
                        Try
                            objLibroRiesgo = Await objMotorSL.ObtenerLibroDeRiesgoTaskAsync(objRiesgo.ID)
                        Catch ex As Exception
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el libro de riesgo", Me.ToString(), "ConsultarLibroRiesgoVM", Application.Current.ToString(), Program.Maquina, ex)
                        End Try

                        RiesgoSeleccionado = objRiesgo

                        If objLibroRiesgo IsNot Nothing Then
                            LibroRiesgos = objLibroRiesgo
                            LibroRiesgos.FullName = RiesgoSeleccionado.ID
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el libro de riesgo", Me.ToString(), "ConsultarLibroRiesgoVM", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Ejecuta una consulta para llenar la lista de variables.
    ''' </summary>
    Public Async Function ObtenerListadoDeVariables() As Task
        Try
            IsBusy = True

            Dim resultado As List(Of RangoConNombre) = Await objMotorSL.ObtenerNombresDefinidosLibroDeRiesgoTaskAsync(IDRiesgoSeleccionado)

            If resultado IsNot Nothing Then
                ListaVariables = (From variables In resultado Where Not variables.Nombre.StartsWith("A2") And
                                                                Not variables.Nombre.Contains("_FilterDatabase") And
                                                                Not variables.Nombre.Contains(".FilterData") And
                                                                Not variables.Nombre.Contains(".Cols") And
                                                                Not variables.Nombre.Contains(".Rows") And
                                                                Not variables.Nombre.Contains(".IFERROR")).ToList
                ListaVariablesFiltro = ListaVariables
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el listado de variables", Me.ToString(), "ObtenerListadoDeVariables", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Ejecuta una consulta para llenar la lista de variables.
    ''' </summary>
    Public Sub FiltrarListadoDeVariables()
        Try
            IsBusyFiltro = True

            If ListaVariables IsNot Nothing Then
                ListaVariablesFiltro = (From item In ListaVariables Where
                                                            item.Nombre.ToUpper().Contains(FiltroVariables.ToUpper()) Or
                                                            item.Comentario.ToUpper().Contains(FiltroVariables.ToUpper())).ToList
            End If

            IsBusyFiltro = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar el listado de variables", Me.ToString(), "FiltrarListadoDeVariables", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta una consulta para llenar el listado de riesgos.
    ''' </summary>
    Public Async Function ObtenerListadoDeRiesgos(strUsuario As String) As System.Threading.Tasks.Task
        Try
            IsBusy = True

            Dim resultado As List(Of Riesgo) = Await objMotorSL.ObtenerListadoDeRiesgosTaskAsync(strUsuario.Replace("\", "@"), Program.ClaveUsuario)

            If resultado IsNot Nothing Then
                ListaRiesgos = resultado
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema el listado de riesgos", Me.ToString(), "ObtenerListadoDeRiesgos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Ejecuta una consulta para llenar el listado de categorias.
    ''' </summary>
    Public Sub ObtenerListadoDeCategorias()
        Try
            IsBusy = True

            Dim categorias As List(Of ParametrosConfiguracion)
            Dim lstRiesgo As List(Of Riesgo)
            Dim rangoParametrosVisualizacion As String = String.Empty

            LibroRiesgos.WorkbookSet.GetLock()
            Try
                Dim miLibro = New clsCORE(LibroRiesgos, CultureInfo.CurrentCulture)
                categorias = ParametrosConfiguracion.ConvertirALista(miLibro.ObtenerValorRango(Program.RANGO_PARAMETROS_CONFIGURACION, Program.TABLA, False))
                rangoParametrosVisualizacion = miLibro.ObtenerValorRango(Program.RANGO_PARAMETROS_VISUALIZACION, Program.TABLA, False)
                lstRiesgo = Riesgo.ConvertirALista(rangoParametrosVisualizacion)
            Finally
                LibroRiesgos.WorkbookSet.ReleaseLock()
            End Try

            If categorias IsNot Nothing And lstRiesgo IsNot Nothing Then

                Dim objRiesgo As Riesgo = lstRiesgo(0)

                Dim categoriasGauge As New List(Of ParametrosConfiguracion)
                If objRiesgo.Gauge Then

                    categoriasGauge = (From catego In categorias Where catego.Gauge = True
                                             Select catego Order By catego.Categoria).ToList
                End If

                Dim categoriasGrid As New List(Of ParametrosConfiguracion)
                If objRiesgo.Grid Then
                    categoriasGrid = (From catego In categorias Where catego.Grid = True
                                            Select catego Order By catego.Categoria).ToList
                End If

                Dim categoriasGrafico As New List(Of ParametrosConfiguracion)
                If objRiesgo.Grafico Then
                    categoriasGrafico = (From catego In categorias Where catego.Grafico = True
                                            Select catego Order By catego.Categoria).ToList
                End If


                Dim lstCategorias = categoriasGauge.Union(categoriasGrid).Union(categoriasGrafico)
                ListaCategorias = lstCategorias.ToList()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el listado de categorias", Me.ToString(), "ObtenerListadoDeCategorias", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que retorna la informacion de la version del libro
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ObtenerInformacionVersion() As Task(Of clsInfoVersion)
        Try
            IsBusy = True
            'valido el riesgo seleccionado
            If IDRiesgoSeleccionado Is Nothing Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un riesgo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return Nothing
            End If
            Dim objInfo As clsInfoVersion
            objInfo = Await objMotorSL.ObtenerInformacionVersionTaskAsync(RiesgoSeleccionado.ID)
            IsBusy = False
            Return objInfo
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en obtener la informacion de la version", Me.ToString(), "ObtenerInformacionVersion", Application.Current.ToString(), Program.Maquina, ex)
            Return Nothing
        End Try
    End Function

#End Region

    

End Class




