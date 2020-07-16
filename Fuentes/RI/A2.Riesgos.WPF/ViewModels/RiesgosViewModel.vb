Imports Telerik.Windows.Controls
Imports A2ControlMenu
Imports A2MCCOREWPF
Imports Newtonsoft.Json
Imports SpreadsheetGear
Imports System.Globalization
Imports System.Web
Imports A2Utilidades.Mensajes
Imports A2.Riesgos.RIA.Web
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web

Public Class RiesgosViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim objMotorSL As clsMotorCliente = New clsMotorCliente(Program.RutaServicioMotorCalculo)
    Private mdcProxy As RiesgosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

#Region "Propiedades"

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

    Private _ListaAccionesRiesgo As List(Of Accion) = New List(Of Accion)
    Public Property ListaAccionesRiesgo() As List(Of Accion)
        Get
            Return _ListaAccionesRiesgo
        End Get
        Set(ByVal value As List(Of Accion))
            _ListaAccionesRiesgo = value
            MyBase.CambioItem("ListaAccionesRiesgo")
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

    Private _TipoGridVisibility As Visibility = Visibility.Collapsed
    Public Property TipoGridVisibility() As Visibility
        Get
            Return _TipoGridVisibility
        End Get
        Set(ByVal value As Visibility)
            _TipoGridVisibility = value
            MyBase.CambioItem("TipoGridVisibility")
        End Set
    End Property

    Private _TipoGaugeVisibility As Visibility = Visibility.Collapsed
    Public Property TipoGaugeVisibility() As Visibility
        Get
            Return _TipoGaugeVisibility
        End Get
        Set(ByVal value As Visibility)
            _TipoGaugeVisibility = value
            MyBase.CambioItem("TipoGaugeVisibility")
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

    Private _FechaUltimaActualizacion As DateTime
    Public Property FechaUltimaActualizacion() As DateTime
        Get
            Return _FechaUltimaActualizacion
        End Get
        Set(ByVal value As DateTime)
            _FechaUltimaActualizacion = value
            MyBase.CambioItem("FechaUltimaActualizacion")
        End Set
    End Property

    Private _FechaUltimaActualizacionVisibility As Visibility = Visibility.Collapsed
    Public Property FechaUltimaActualizacionVisibility() As Visibility
        Get
            Return _FechaUltimaActualizacionVisibility
        End Get
        Set(ByVal value As Visibility)
            _FechaUltimaActualizacionVisibility = value
            MyBase.CambioItem("FechaUltimaActualizacionVisibility")
        End Set
    End Property

    Private _ListaAcciones As List(Of Accion)
    Public Property ListaAcciones As List(Of Accion)
        Get
            Return _ListaAcciones
        End Get
        Set(ByVal value As List(Of Accion))
            _ListaAcciones = value
            MyBase.CambioItem("ListaAcciones")
        End Set
    End Property


    ''' <summary>
    ''' Lista de alertas 
    ''' </summary>
    Private _ListaAlertas As EntitySet(Of Alertas)
    Public Property ListaAlertas() As EntitySet(Of Alertas)
        Get
            Return _ListaAlertas
        End Get
        Set(ByVal value As EntitySet(Of Alertas))
            _ListaAlertas = value
            MyBase.CambioItem("ListaAlertas")
        End Set
    End Property

    ''' <summary>
    ''' Visibilidad del icono de alertas
    ''' </summary>
    Private _AlertaVisibility As Boolean = False
    Public Property AlertaVisibility() As Boolean
        Get
            Return _AlertaVisibility
        End Get
        Set(ByVal value As Boolean)
            _AlertaVisibility = value
            MyBase.CambioItem("AlertaVisibility")
        End Set
    End Property

    ''' <summary>
    ''' Visibilidad de los botones del tablero de control
    ''' </summary>
    Private _BotonesVisibility As Boolean = False
    Public Property BotonesVisibility() As Boolean
        Get
            Return _BotonesVisibility
        End Get
        Set(ByVal value As Boolean)
            _BotonesVisibility = value
            MyBase.CambioItem("BotonesVisibility")
        End Set
    End Property

    ''' <summary>
    ''' Visibilidad del control isBusy para las alertas
    ''' </summary>
    Private _IsBusyAlerta As Boolean = False
    Public Property IsBusyAlerta As Boolean
        Get
            Return _IsBusyAlerta
        End Get
        Set(ByVal value As Boolean)
            _IsBusyAlerta = value
            MyBase.CambioItem("IsBusyAlerta")
        End Set
    End Property

    ''' <summary>
    ''' Visibilidad del combo de selección del tipo de gráfico
    ''' </summary>
    Private _MostrarComboTiposGraficoVisibility As Boolean = False
    Public Property MostrarComboTiposGraficoVisibility As Boolean
        Get
            Return _MostrarComboTiposGraficoVisibility
        End Get
        Set(ByVal value As Boolean)
            _MostrarComboTiposGraficoVisibility = value
            MyBase.CambioItem("MostrarComboTiposGraficoVisibility")
        End Set
    End Property


    Private _listaTiposGrafico As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)()
    Public Property ListaTiposGrafico As List(Of OYDUtilidades.ItemCombo)
        Get
            TiposGraficoSeleccionado = Program.TiposGrafico()(0)
            Return Program.TiposGrafico()
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTiposGrafico = value
            MyBase.CambioItem("ListaTiposGrafico")
        End Set
    End Property

    Private _TiposGraficoSeleccionado As OYDUtilidades.ItemCombo = New OYDUtilidades.ItemCombo()
    Public Property TiposGraficoSeleccionado As OYDUtilidades.ItemCombo
        Get
            Return _TiposGraficoSeleccionado
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _TiposGraficoSeleccionado = value
            MyBase.CambioItem("TiposGraficoSeleccionado")
            MyBase.CambioItem("listaTiposGraficoXCategoria")
        End Set
    End Property

    Private _listaTiposGraficoXCategoria As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)()
    Public Property listaTiposGraficoXCategoria As List(Of OYDUtilidades.ItemCombo)
        Get
            Return Program.TiposGraficoXCategoria(TiposGraficoSeleccionado.Categoria)
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaTiposGraficoXCategoria = value
            MyBase.CambioItem("listaTiposGraficoXCategoria")
        End Set
    End Property

    ''' <summary>
    ''' Visibilidad del botón grid
    ''' </summary>
    Private _MostrarBotonGridVisibility As Boolean = False
    Public Property MostrarBotonGridVisibility As Boolean
        Get
            Return _MostrarBotonGridVisibility
        End Get
        Set(ByVal value As Boolean)
            _MostrarBotonGridVisibility = value
            MyBase.CambioItem("MostrarBotonGridVisibility")
        End Set
    End Property

    ''' <summary>
    ''' Visibilidad del botón grid
    ''' </summary>
    Private _MostrarBotonGaugeVisibility As Boolean = False
    Public Property MostrarBotonGaugeVisibility As Boolean
        Get
            Return _MostrarBotonGaugeVisibility
        End Get
        Set(ByVal value As Boolean)
            _MostrarBotonGaugeVisibility = value
            MyBase.CambioItem("MostrarBotonGaugeVisibility")
        End Set
    End Property


    ''' <summary>
    ''' Visibilidad del botón grid
    ''' </summary>
    Private _MostrarBotonGraficoVisibility As Boolean = False
    Public Property MostrarBotonGraficoVisibility As Boolean
        Get
            Return _MostrarBotonGraficoVisibility
        End Get
        Set(ByVal value As Boolean)
            _MostrarBotonGraficoVisibility = value
            MyBase.CambioItem("MostrarBotonGraficoVisibility")
        End Set
    End Property
#End Region

#Region "Métodos"

    Public Sub New()

    End Sub

    Public Async Function CargarRiesgos(strUsuario As String) As System.Threading.Tasks.Task

        Try

            IsBusy = True

            Dim resultado As List(Of Riesgo) = Await objMotorSL.ObtenerListadoDeRiesgosTaskAsync(strUsuario.Replace("\", "@"), Program.ClaveUsuario)

            If resultado IsNot Nothing Then
                ListaRiesgos = resultado
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los riesgos del usuario.", Me.ToString(), "CargarRiesgos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Async Function CargarAccionesRiesgo(strUsuario As String, strRiesgo As String, strClave As String) As System.Threading.Tasks.Task

        Dim resultado As List(Of Accion) = Await objMotorSL.ObtenerListadoAccionesDelRiesgosTaskAsync(strUsuario.Replace("\", "@"), strRiesgo, strClave) 'strRiesgo)

        If resultado IsNot Nothing Then
            ListaAccionesRiesgo = resultado
        End If

    End Function

    Public Async Function EjecutarAccionRiesgo(strSp As String, strParametros As String, strUsuario As String) As System.Threading.Tasks.Task

        Dim resultado As String = Await objMotorSL.EjecutarConsultaAccionesRiesgoTaskAsync(strSp, strParametros, strUsuario.Replace("\", "@"))

        If resultado IsNot Nothing Then
            mostrarMensaje("Acción ejecutada exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
        Else
            mostrarMensaje("Ocurrió un error al ejecutar la acción", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If

    End Function

    Public Async Function ConsultarParametrizacionRiesgo() As System.Threading.Tasks.Task

        'LibroRiesgos = SpreadsheetGear.Factory.GetWorkbookSet(CultureInfo.CurrentCulture).Workbooks
        LibroRiesgos = Factory.GetWorkbook() 'GetWorkbookSet(System.Globalization.CultureInfo.CurrentCulture)


        If IDRiesgoSeleccionado IsNot Nothing Then

            If ListaRiesgos IsNot Nothing Then
                Dim objRiesgo As Riesgo = (From item In ListaRiesgos.ToList() Where item.ID = IDRiesgoSeleccionado.ToString() Select item).FirstOrDefault()

                If objRiesgo IsNot Nothing Then

                    Dim objLibroRiesgo As IWorkbook = Factory.GetWorkbook(CultureInfo.CurrentCulture)
                    Try
                        objLibroRiesgo = Await objMotorSL.ObtenerParametrosDeRiesgoTaskAsync(objRiesgo.ID)
                    Catch ex As Exception
                        'TODO:
                    End Try

                    RiesgoSeleccionado = objRiesgo

                    If objLibroRiesgo IsNot Nothing Then
                        LibroRiesgos = objLibroRiesgo
                        FechaUltimaActualizacion = DateTime.Now
                        FechaUltimaActualizacionVisibility = Visibility.Visible
                    End If

                    MostrarBotonGridVisibility = False
                    MostrarBotonGaugeVisibility = False
                    MostrarBotonGraficoVisibility = False

                    If objRiesgo.Grid Then
                        MostrarBotonGridVisibility = True
                    End If
                    If objRiesgo.Gauge Then
                        MostrarBotonGaugeVisibility = True
                    End If
                    If objRiesgo.Grafico Then
                        MostrarBotonGraficoVisibility = True
                    End If

                End If

            End If
        End If

    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de las alertas del día
    ''' </summary>
    Public Async Function ConsultarAlertasDelDia() As Task(Of Boolean)
        Return Await ConsultarAlertas(DateTime.Now)
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de las alertas para una fecha especifica
    ''' </summary>
    ''' <param name="pdtmFecha">Indica la fecha de las consultas a filtrar)</param>
    Private Async Function ConsultarAlertas(ByVal pdtmFecha As DateTime) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of Alertas)

        Try
            IsBusy = True
            IsBusyAlerta = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyRiesgos()
            End If

            mdcProxy.Alertas.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarAlertasSyncQuery(pdtmFecha:=pdtmFecha, pstrUsuario:=Program.UsuarioAutenticado, pstrInfoConexion:=Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar las alertas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las alertas.", Me.ToString(), "ConsultarAlertas", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaAlertas = mdcProxy.Alertas

                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en al cargar la lista de alertas ", Me.ToString(), "ConsultarAlertas", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusyAlerta = False
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Obtiene el libro del riesgos sólo con la hoja del riesgo
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DescargarVisualizacion() As IWorkbook

        LibroRiesgos.WorkbookSet.GetLock()
        Dim objCopiaLibro As IWorkbook = Factory.GetWorkbook()
        Try
            objCopiaLibro = SpreadsheetGear.Factory.GetWorkbookSet(CultureInfo.CurrentCulture).
                                                            Workbooks.OpenFromStream(LibroRiesgos.SaveToStream(FileFormat.OpenXMLWorkbook))
        Finally
            LibroRiesgos.WorkbookSet.ReleaseLock()
        End Try


        Try
            objCopiaLibro.WorkbookSet.GetLock()

            Try
                Dim objMotorSL As clsCORE = New clsCORE(objCopiaLibro, System.Globalization.CultureInfo.CurrentCulture)

                Dim tblBandas As String = String.Empty
                Dim lstBandas As List(Of Banda) = New List(Of Banda)()
                tblBandas = objMotorSL.ObtenerValorRango(Program.RANGO_BANDAS, Program.TABLA, False)
                lstBandas = Banda.ConvertirALista(tblBandas)


                'Desbloqueo las hojas del libro
                For Each Item As IWorksheet In objCopiaLibro.Sheets
                    Item.ProtectContents = False
                    Item.Visible = SheetVisibility.Visible
                Next

                'Copiar los valores de los rangos de visualización y pegarlos como valores
                For Each item In lstBandas
                    If Not item.EsGrafico Then
                        objCopiaLibro.Names(item.RangoDeDatos).RefersToRange().Copy(objCopiaLibro.Names(item.RangoDeDatos).RefersToRange(), PasteType.Values, PasteOperation.None, False, False)
                    End If
                Next

                'Elimino el resto de hojas diferentes a la de visualización
                For Each item As ISheet In objCopiaLibro.Sheets

                    If item.Name <> Program.HOJA_VISUALIZACION Then
                        item.Delete()
                    End If

                Next

                'Activo el tab de la hoja, los scrollbar y los encabezados
                objCopiaLibro.WindowInfo.ActiveWorksheet.WindowInfo.DisplayHeadings = True
                objCopiaLibro.WindowInfo.DisplayHorizontalScrollBar = True
                objCopiaLibro.WindowInfo.DisplayVerticalScrollBar = True
                objCopiaLibro.WindowInfo.DisplayWorkbookTabs = True

            Finally
                objCopiaLibro.WorkbookSet.ReleaseLock()
            End Try
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al descargar el archivo de visualización", Me.ToString(), "DescargarVisualizacion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusyAlerta = False
            IsBusy = False
        End Try

        Return objCopiaLibro
    End Function

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

#Region "Resultados Asíncronos"

#End Region

#Region "Commands"

#End Region


End Class

Public Class TiposGrafico
    Public Nombre As String
    Public Valor As String
End Class
