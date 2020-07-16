'Codigo Creado Por: Rafael Cordero
'Archivo: ImportacionDCVViewModel.vb
'Generado el : 08/04/2011 
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports A2ControlMenu
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Text
Imports A2Utilidades.Mensajes

Public Class ImportacionDCVViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private _strNombreArchivoImportado As String = String.Empty
    Private Const _STR_NOMBRE_PROCESO As String = "DatosDCV"
    Private dcProxy As ImportacionesDomainContext


    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)

        Try
            If Not Program.IsDesignMode() Then
                'IsBusy = True
                'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionDCVViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarResultados()
        If Not objResultadoBuilder Is Nothing Then
            objResultadoBuilder.Clear()
            Resultados = objResultadoBuilder.ToString
        End If
    End Sub

    Public Sub CargarArchivo()
        Try
            If ArchivoSeleccionado Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("No ha seleccionado el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(ArchivoSeleccionado.Nombre) Then
                A2Utilidades.Mensajes.mostrarMensaje("No ha seleccionado el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            dcProxy.Load(dcProxy.NroDeRegistrosDCVQuery(False, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaNroRegistros, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportarLiquidacionesView.btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Asincronos"
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
                                 Me.ToString(), "ImportacionDCVViewModel.VentanaCargaArchivoCerro", Program.TituloSistema, Program.Maquina, ex)
        End Try


    End Sub

    Public Sub VentanaCargaArchivoControl(ByVal strNombrearchivo As String)
        Try
            'SLB20131008 Manejo con el text box
            ArchivoSeleccionado.Nombre = strNombrearchivo
            Exit Sub

            ListaArchivos.Clear()
            dcProxy.Archivos.Clear()
            dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            _strNombreArchivoImportado = strNombrearchivo
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionDCVViewModel.VentanaCargaArchivoCerro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of Archivo))
        'metodo que se utiliza cuando termina de consultar los archivos que tiene el usuario subidos al servidor y los muestra en un combo
        If IsNothing(lo.Error) Then
            ListaArchivos = dcProxy.Archivos.ToList
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault()
        Else
            MessageBox.Show(lo.Error.Message)
        End If
        IsBusy = False
    End Sub

    Private Sub TerminaConsultaNroRegistros(lo As LoadOperation(Of LineaComentario))
        Try
            'cuando termina de consultar el numero de registros que existen en tblpagosdcv pregunta si desea eliminarlos o no
            Dim sb As New StringBuilder

            If lo.HasError = False Then
                If Not String.IsNullOrEmpty(lo.Entities(0).Texto) Then
                    If lo.Entities(0).Texto.Equals("BORRADO") Or lo.Entities(0).Texto.Equals("OK") Then
                        'se encarga de la lectura del archivo dcv
                        IsBusy = True
                        dcProxy.Load(dcProxy.CargarArchivoDCVQuery(ArchivoSeleccionado.Nombre, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminaCargaDatosDCV, Nothing)
                        'dcProxy.Load(dcProxy.CargarArchivoDCVQuery(ArchivoSeleccionado.Ruta, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminaCargaDatosDCV, Nothing)
                    Else
                        IsBusy = False
                        mostrarMensajePregunta(lo.Entities(0).Texto,
                                               Program.TituloSistema,
                                               "INFORMACIONPAGO",
                                               AddressOf TerminaPregunta, False)
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDCVViewModel.TerminoCargarArchivoLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaCargaDatosDCV(lo As LoadOperation(Of LineaComentario))
        Try
            ' muestra los resultados de la lectura del archivo 
            Dim sb As New StringBuilder

            If lo.HasError = False Then
                objResultadoBuilder.Clear()
                For Each Lista In lo.Entities
                    objResultadoBuilder.Append(Lista.FechaHora & " - " & Lista.Texto & vbCrLf)
                Next
                Resultados = objResultadoBuilder.ToString
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDCVViewModel.TerminaCargaDatosDCV", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            dcProxy.Load(dcProxy.NroDeRegistrosDCVQuery(True, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaNroRegistros, Nothing)
        Else
            objResultadoBuilder.Append(vbCrLf)
            objResultadoBuilder.Append(Now.ToString() & " - " & "Carga de archivo cancelada.")

            Resultados = objResultadoBuilder.ToString
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _objResultadoBuilder As StringBuilder = New StringBuilder
    Public Property objResultadoBuilder() As StringBuilder
        Get
            Return _objResultadoBuilder
        End Get
        Set(ByVal value As StringBuilder)
            _objResultadoBuilder = value
        End Set
    End Property

    Private _strResultados As String
    Public Property Resultados() As String
        Get
            Return _strResultados
        End Get
        Set(ByVal value As String)
            _strResultados = value
            MyBase.CambioItem("Resultados")
        End Set
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

#End Region

End Class
