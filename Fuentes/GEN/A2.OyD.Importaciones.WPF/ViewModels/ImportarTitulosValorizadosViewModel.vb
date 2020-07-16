'Codigo Creado Por: Rafael Cordero
'Archivo: ImportarTitulosValorizadosViewModel.vb
'Generado el : 07/31/2011 
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Windows.Browser
Imports A2ControlMenu
Imports Microsoft.Practices.Composite.Events
Imports Microsoft.Practices.Unity
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones


Public Class ImportarTitulosValorizadosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"

    Public Enum FormatoImportacion
        EspecieVencimiento
        IsinAnna
    End Enum

    Private _strNombreArchivoImportado As String = String.Empty
    Public Const _STR_NOMBRE_PROCESO As String = "TitulosValorizados"

    Private dcProxy As ImportacionesDomainContext
    Dim objProxyUtil As UtilidadesDomainContext

    Private Const STR_RUTA_DEFECTO As String = "C:\"
    Private Const STR_ARCHIVO_DEFECTO As String = ".txt"

#End Region

#Region "Procedimientos"
    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
            objProxyUtil = New UtilidadesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyUtil = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

        Try
            If Not Program.IsDesignMode() Then
                MostrarProgreso = Visibility.Collapsed
                IsBusy = True
                dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
                objProxyUtil.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportarTitulosValorizadosViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub MostrarCargadorArchivos()
    '    Try
    '        Dim cwCar As New CargarArchivosView(_STR_NOMBRE_PROCESO)
    '        AddHandler cwCar.Closed, AddressOf VentanaCargaArchivoCerro
    '        cwCar.Show()
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
    '                             Me.ToString(), "ImportarTitulosValorizadosViewModel.MostrarCargadorArchivos", Program.TituloSistema, Program.Maquina, ex)
    '    End Try
    'End Sub

    Private Sub VentanaCargaArchivoCerro(sender As System.Object, e As EventArgs)

        Try

            ListaArchivos.Clear()
            dcProxy.Archivos.Clear()
            dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            'If CType(sender, CargarArchivosView).DialogResult = True And Not IsNothing(CType(sender, CargarArchivosView).ArchivoSeleccionado) Then
            '    _strNombreArchivoImportado = CType(sender, CargarArchivosView).ArchivoSeleccionado.Nombre
            '    ArchivoSeleccionado = CType(sender, CargarArchivosView).ArchivoSeleccionado
            'End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportarTitulosValorizadosViewModel.VentanaCargaArchivoCerro", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub LimpiarObjetosLista()
        ListaTitulosValorizar = Nothing
        ListaParaMostrar.Clear()
        MyBase.CambioItem("ListaParaMostrar")
        MyBase.CambioItem("ListaRegPaged")
    End Sub


    Public Sub CambioDeArchivo()
        If Not ListaTitulosValorizar Is Nothing Then
            FormatoEspecieVencimiento = False
            FormatoIsinANNA = False
            Call LimpiarObjetosLista()
        End If
    End Sub

    Public Sub SubirArchivo()

        If DatosValidos() Then

            Select Case FormatoEspecieVencimiento
                Case True 'Especie y Vencimiento
                    Call PrSubirArchivo(FormatoImportacion.EspecieVencimiento)
                Case False 'Isin ANNA
                    Call PrSubirArchivo(FormatoImportacion.IsinAnna)
                Case Else
            End Select

        End If

    End Sub

    Private Sub PrSubirArchivo(pTipoFormato As FormatoImportacion)
        IsBusy = True
        dcProxy.ListaTitulosValorizados.Clear()
        dcProxy.Load(dcProxy.CargarArchivoValorizacionTitulosQuery(ArchivoSeleccionado.Ruta, True, FechaDesde, _STR_NOMBRE_PROCESO, Program.Usuario, pTipoFormato, Program.HashConexion), AddressOf TerminaCargarArchivo, "")
    End Sub


    ''' <summary>
    '''    /******************************************************************************************
    '''    /* INICIO DOCUMENTO
    '''    /* Function        DatosValidos
    '''    /* Alcance     :   Private
    '''    /* Descripción :   Valida que se escoja una ruta donde se encuentra el archivo de excel
    '''    /*                 de  baseivas de contraparte bajado de datatec y valida que no se hayan
    '''    /*                 subido datos de esa fecha.
    '''    /* Parámetros  :
    '''    /* Por
    '''    /* Referencia
    '''    /* Valores de retorno:
    '''    /* ConsultaBolsa:    Devuelve True si todo esta correcto
    '''    /*                   Devuelve false si no se cumplen alguna de las validaciones
    '''    /* FIN DOCUMENTO
    '''    /******************************************************************************************
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DatosValidos() As Boolean

        Dim lngContar As Long = 0, strTabla As String = String.Empty
        Dim strMensaje = String.Empty
        Dim intPosicion As Long

        DatosValidos = True

        If String.IsNullOrEmpty(ArchivoSeleccionado.Ruta) Then
            strMensaje = strMensaje & vbCrLf & "** Debe escoger la ruta y el nombre del archivo a subir "
            DatosValidos = False
        End If

        'intPosicion = InStr(1, TxtRuta, ".")

        If intPosicion = 1 Then
            strMensaje = strMensaje & vbCrLf & "** Debe escoger la ruta y el nombre del archivo a subir  "
            DatosValidos = False
        End If

        If Not FormatoEspecieVencimiento And Not FormatoIsinANNA Then
            strMensaje = strMensaje & vbCrLf & "** Se debe seleccionar un formato de importación para subir el archivo   "
            DatosValidos = False
        End If

        If Not DatosValidos Then
            Dim strMensajeMostrar = "Se presentarón las siguientes inconsistencias: " & vbCrLf & vbCrLf & strMensaje
            A2Utilidades.Mensajes.mostrarMensaje(strMensajeMostrar, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
        End If

        Return DatosValidos

    End Function

#End Region

#Region "Asincronos"
    Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of Archivo))
        If IsNothing(lo.Error) Then
            ListaArchivos = dcProxy.Archivos.ToList
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault()
        Else
            MessageBox.Show(lo.Error.Message)
        End If
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        IsBusy = False
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Program.Usuario, "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = FormatDateTime(obj.Value, Microsoft.VisualBasic.DateFormat.ShortDate)
        End If

        FechaActual = Now.Date
    End Sub
    Private Sub TerminaCargarArchivo(lo As LoadOperation(Of ListaTitulosValorizados))
        Try
            If Not lo.HasError Then
                ListaTitulosValorizar = dcProxy.ListaTitulosValorizados
                _ListaParaMostrar.Clear()
                _ListaParaMostrar = ListaTitulosValorizar.ToList()

                MyBase.CambioItem("ListaRegPaged")
                MyBase.CambioItem("ListaTitulosValorizar")

                IsBusy = False

                A2Utilidades.Mensajes.mostrarMensaje("Grabación exitosa (" & CStr(_ListaParaMostrar.Count) & " registros)", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

            Else
                IsBusy = False
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminaCargarArchivo", _
                '                                 Me.ToString(), "TerminaCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error)



                A2Utilidades.Mensajes.mostrarMensaje(SplitMensaje(lo.Error.Message.ToString()), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "ImportarTitulosValorizadosViewModel.TerminaCargarArchivo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades"


    Private _dtmFechaCierre As Date
    Public Property FechaCierre() As Date
        Get
            Return _dtmFechaCierre
        End Get
        Set(ByVal value As Date)
            If Not IsNothing(value) Then
                _dtmFechaCierre = value
                MyBase.CambioItem("FechaCierre")
            End If
        End Set
    End Property

    Private _ArchivoSeleccionado As New Archivo
    Public Property ArchivoSeleccionado() As Archivo
        Get
            Return _ArchivoSeleccionado
        End Get
        Set(ByVal value As Archivo)
            If Not IsNothing(value) Then
                _ArchivoSeleccionado = value
                MyBase.CambioItem("ArchivoSeleccionado")
            End If
        End Set
    End Property

    Private _ListaTitulosValorizar As EntitySet(Of ListaTitulosValorizados)
    Public Property ListaTitulosValorizar() As EntitySet(Of ListaTitulosValorizados)
        Get
            Return _ListaTitulosValorizar
        End Get
        Set(ByVal value As EntitySet(Of ListaTitulosValorizados))
            _ListaTitulosValorizar = value
            MyBase.CambioItem("ListaTitulosValorizar")
        End Set
    End Property

    Private _ListaParaMostrar As New List(Of ListaTitulosValorizados)
    Public ReadOnly Property ListaParaMostrar() As List(Of ListaTitulosValorizados)
        Get
            Return _ListaParaMostrar
        End Get
    End Property

    Public ReadOnly Property ListaRegPaged() As PagedCollectionView
        Get
            If Not IsNothing(ListaParaMostrar) Then
                Dim view = New PagedCollectionView(ListaParaMostrar)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


    Private _ListaArchivos As List(Of Archivo)
    Public Property ListaArchivos() As List(Of Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As List(Of Archivo))
            _ListaArchivos = value
            MyBase.CambioItem("ListaArchivos")
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault
        End Set
    End Property

    Private _enFormatoImportacionSel As FormatoImportacion
    Public Property FormatoImportacionSel() As FormatoImportacion
        Get
            Return _enFormatoImportacionSel
        End Get
        Set(ByVal value As FormatoImportacion)
            _enFormatoImportacionSel = value
            MyBase.CambioItem("FormatoImportacionSel")
        End Set
    End Property


    Private _dtmFechaDesde As Date
    Public Property FechaDesde() As Date
        Get
            Return _dtmFechaDesde
        End Get
        Set(ByVal value As Date)
            _dtmFechaDesde = value
            MyBase.CambioItem("FechaDesde")
        End Set
    End Property

    Private _dtmFechaActual As Date
    Public Property FechaActual() As Date
        Get
            Return _dtmFechaActual
        End Get
        Set(ByVal value As Date)
            _dtmFechaActual = value
            MyBase.CambioItem("FechaActual")
        End Set
    End Property

    Private _bolFormatoEspecieVencimiento As Boolean
    Public Property FormatoEspecieVencimiento() As Boolean
        Get
            Return _bolFormatoEspecieVencimiento
        End Get
        Set(ByVal value As Boolean)
            _bolFormatoEspecieVencimiento = value
            MyBase.CambioItem("FormatoEspecieVencimiento")
            LimpiarObjetosLista()
        End Set
    End Property

    Private _bolFormatoIsinANNA As Boolean
    Public Property FormatoIsinANNA() As Boolean
        Get
            Return _bolFormatoIsinANNA
        End Get
        Set(ByVal value As Boolean)
            _bolFormatoIsinANNA = value
            MyBase.CambioItem("FormatoIsinANNA")
            LimpiarObjetosLista()
        End Set
    End Property

    Private _MostrarProgreso As Visibility
    Public Property MostrarProgreso() As Visibility
        Get
            Return _MostrarProgreso
        End Get
        Set(ByVal value As Visibility)
            _MostrarProgreso = value
            MyBase.CambioItem("MostrarProgreso")
        End Set
    End Property

    Private _TotalRegistros As Integer
    Public Property TotalRegistros() As Integer
        Get
            Return _TotalRegistros
        End Get
        Set(ByVal value As Integer)
            _TotalRegistros = value
            MyBase.CambioItem("TotalRegistros")
        End Set
    End Property

    Private _PorcProgreso As Double
    Public Property PorcProgreso() As Double
        Get
            Return _PorcProgreso
        End Get
        Set(ByVal value As Double)
            _PorcProgreso = value
            MyBase.CambioItem("PorcProgreso")
        End Set
    End Property

#End Region

#Region "Funciones y Mtodos Generales"


    Private Function SplitMensaje(pstrMsg As String) As String
        Dim strMsjSpa As String = "Mensaje de InnerException: "
        Dim strMsjEng As String = "Inner exception message: "


        Dim strMensajeReemplazar As String = String.Empty

        If pstrMsg.Contains(strMsjSpa) Then
            strMensajeReemplazar = strMsjSpa
        ElseIf pstrMsg.Contains(strMsjEng) Then
            strMensajeReemplazar = strMsjEng
        End If

        Dim strMensajeFinal As String = String.Empty
        Dim intPuntoInicio As Integer = pstrMsg.IndexOf(strMensajeReemplazar)
        If intPuntoInicio > 0 Then
            strMensajeFinal = pstrMsg.Substring(intPuntoInicio, (Len(pstrMsg)) - intPuntoInicio).Replace(strMensajeReemplazar, "")
        Else
            strMensajeFinal = pstrMsg & vbCrLf & Program.TituloSistema
        End If

        Return strMensajeFinal
    End Function

    Private Function ConvStr(ByVal pstrVble As Object) As String

        If Not IsNothing(pstrVble) Then
            ConvStr = CStr(pstrVble)
        Else
            ConvStr = ""
        End If

    End Function
    Private Function ConvLng(ByVal pstrVble As Object) As Long
        If Not IsNothing(pstrVble) Then
            ConvLng = CLng(pstrVble)
        Else
            ConvLng = -1
        End If
    End Function

#End Region

End Class




