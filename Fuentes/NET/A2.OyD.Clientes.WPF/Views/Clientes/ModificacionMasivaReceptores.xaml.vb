
Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports Microsoft.VisualBasic.CompilerServices
Imports System.IO
Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO.IsolatedStorage
Imports A2ComunesImportaciones
Imports System.Text

Partial Public Class ModificacionMasivaReceptores
    Inherits UserControl
    Implements INotifyPropertyChanged
    Dim dcProxy As ClientesDomainContext
    Dim dcProxyImporta As ImportacionesDomainContext
    Dim strnombre As String
    Private Shared SEPARATOR_FORMAT_CVS As String = ","
    Public Const CSTR_NOMBREPROCESO_MODMASIVARECE = "MODMASIVARECEPTORES"
    Private Const _STR_NOMBRE_PROCESO As String = "LeerArchivoReceptores"

    Public Sub New()
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
            dcProxyImporta = New ImportacionesDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxyImporta = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If

    End Sub
#Region "Asincronicos"
    Private Sub Terminoactualizarreceptores(ByVal lo As LoadOperation(Of OyDClientes.tblLogReceptores))
        If Not lo.HasError Then
            Try
                IsBusy = False
                Dim list = dcProxy.tblLogReceptores.ToList
                If list.Count > 0 Then
                    If list.First.Inconsistencia = String.Empty Then
                        'strnombre = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\" + GeneraArchivoCliSelected.NombreArchivo.Trim + ".txt"
                        'A2Utilidades.Mensajes.mostrarMensaje("Actualización terminada satisfactoriamente" + vbCrLf + "A continuación se genererara el archivo de resultados" + vbCrLf + "(" + strnombre + ")" + vbCrLf + "que puede abrir y revisar desde EXCEL", _
                        '    Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        GenerarArchivoPlano(list)
                    Else
                        Dim i = 1
                        Dim strmensajeError = "Actualización NO realizada.Se presentaron estas inconsistencias:" + vbCrLf
                        For Each l In list
                            strmensajeError = strmensajeError + l.Inconsistencia + vbCrLf
                            i = i + 1
                        Next
                        If i > 10 Then
                            strmensajeError = "..."
                        End If
                        strmensajeError = strmensajeError + vbCrLf + "Revise su archivo plano Fuente" + vbCrLf + "(" + GeneraArchivoCliSelected.NombreArchivoAC + ")"
                        A2Utilidades.Mensajes.mostrarMensaje(strmensajeError, _
                           Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se realizo ninguna actualizacion verifique que los receptores se encuentren registrados", _
                       Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesReceptores", _
                                             Me.ToString(), "Terminoactualizarreceptores", Application.Current.ToString(), Program.Maquina, ex)
                lo.MarkErrorAsHandled()
            End Try
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesReceptores", _
                                         Me.ToString(), "Terminoactualizarreceptores", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub
    Private Sub Terminoactualizarreceptoresespecifico(ByVal lo As LoadOperation(Of OyDClientes.tblLogReceptores))
        If Not lo.HasError Then
            Try
                IsBusy = False
                Dim list = dcProxy.tblLogReceptores.ToList
                If list.Count > 0 Then
                    If list.First.Inconsistencia = String.Empty Then
                        'A2Utilidades.Mensajes.mostrarMensaje("Actualización terminada satisfactoriamente" + vbCrLf + "A continuación se genererara el archivo de resultados" + vbCrLf + "(" + GeneraArchivoCliSelected.NombreArchivo + ")" + vbCrLf + "que puede abrir y revisar desde EXCEL", _
                        '    Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        GenerarArchivoPlano(list)
                    Else
                        Dim i = 1
                        Dim strmensajeError = "Actualización NO realizada.Se presentaron estas inconsistencias:" + vbCrLf
                        For Each l In list
                            strmensajeError = strmensajeError + l.Inconsistencia + vbCrLf
                            i = i + 1
                        Next
                        If i > 10 Then
                            strmensajeError = "..."
                        End If
                        strmensajeError = strmensajeError + vbCrLf + "Revise su archivo plano Fuente" + vbCrLf + "(" + GeneraArchivoCliSelected.NombreArchivoAC + ")"
                        A2Utilidades.Mensajes.mostrarMensaje(strmensajeError, _
                           Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se realizo ninguna actualizacion verifique que los receptores se encuentren registrados", _
                       Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesReceptores", _
                                             Me.ToString(), "Terminoactualizarreceptoresespecifico", Application.Current.ToString(), Program.Maquina, ex)
                lo.MarkErrorAsHandled()
            End Try
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesReceptores", _
                                         Me.ToString(), "Terminoactualizarreceptoresespecifico", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub
#End Region
#Region "Metodos"

    Private Sub Button_Click3(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            Dim p As New OpenFileDialog
            p.Filter = "*.txt|*.txt"
            If p.ShowDialog Then
                GeneraArchivoCliSelected.NombreArchivoAC = p.FileName 'p.FileName
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al leer el archivo", _
                                            Me.ToString(), "LeerArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub Button_Click_1(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If tbiGeneral.SelectedIndex = 1 Then
                If GeneraArchivoCliSelected.NombreArchivoAC = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el  archivo plano Fuente de los datos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                ElseIf GeneraArchivoCliSelected.NombreArchivo = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el  archivo plano que almacenará los resultados del proceso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                ElseIf GeneraArchivoCliSelected.NombreArchivo = GeneraArchivoCliSelected.NombreArchivoAC Then
                    A2Utilidades.Mensajes.mostrarMensaje("El archivo origen y el destino no pueden ser iguales", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    IsBusy = True
                    importar()
                    'se importa el archivo

                End If
            ElseIf tbiGeneral.SelectedIndex = 0 Then
                If GeneraArchivoCliSelected.Receptorant = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el código de receptor anterior ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                ElseIf GeneraArchivoCliSelected.Receptoract = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el código de receptor actual ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    If GeneraArchivoCliSelected.Receptoract = GeneraArchivoCliSelected.Receptorant Then
                        A2Utilidades.Mensajes.mostrarMensaje("Los codigos de receptores deben ser diferentes ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                If GeneraArchivoCliSelected.NombreArchivo = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe especificar el archivo plano que almacenará los resultados del proceso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    IsBusy = True
                    dcProxy.tblLogReceptores.Clear()
                    dcProxy.Load(dcProxy.ActualizarReceptoresclienteQuery(GeneraArchivoCliSelected.Receptorant, GeneraArchivoCliSelected.Receptoract, Program.Usuario, Program.HashConexion), AddressOf Terminoactualizarreceptores, Nothing)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Generar el proceso", _
                                Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub GenerarArchivoPlano(ByVal lista As Object)
        Try
            Dim strBuilder As New StringBuilder()
            Dim strLineas As New List(Of String)
            Dim lstFields As List(Of String) = New List(Of String)()
            'Dim strruta As String
            lstFields.Add(formatField("CLIENTE", "CSV", True))
            lstFields.Add(formatField("RECEPTOR ANTEIROR", "CSV", True))
            lstFields.Add(formatField("NUEVO RECEPTOR", "CSV", True))
            lstFields.Add(formatField("FECHA PROCESO", "CSV", True))
            lstFields.Add(formatField("USUARIO", "CSV", True))
            lstFields.Add(formatField("MAQUINA", "CSV", True))
            buildStringOfRow(strBuilder, lstFields, "CSV")
            strLineas.Add(strBuilder.ToString())
            strnombre = GeneraArchivoCliSelected.NombreArchivo.Trim + ".csv"
            'strruta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), strnombre)
            'Dim s As StreamWriter = File.CreateText(strruta)

            's.WriteLine("CLIENTE, RECEPTOR ANTEIROR, NUEVO RECEPTOR, FECHA PROCESO, USUARIO, MAQUINA")
            For Each li In lista
                lstFields.Clear()
                strBuilder.Clear()
                lstFields.Add(formatField(Trim(li.IdComitente), "CSV", False))
                lstFields.Add(formatField(Trim(li.IDReceptorAnterior), "CSV", False))
                lstFields.Add(formatField(Trim(li.IDNuevoReceptor), "CSV", False))
                lstFields.Add(formatField(Trim(li.Proceso), "CSV", False))
                lstFields.Add(formatField(Trim(li.Usuario), "CSV", False))
                lstFields.Add(formatField(Trim(li.Maquina), "CSV", False))
                buildStringOfRow(strBuilder, lstFields, "CSV")
                strLineas.Add(strBuilder.ToString())
                's.WriteLine(Trim(li.IdComitente) & "," & li.IDReceptorAnterior & "," & li.IDNuevoReceptor & "," & li.Proceso & "," & li.Usuario & "," & li.Maquina)
            Next
            dcProxyImporta.Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_MODMASIVARECE, Program.Usuario, String.Format(strnombre, Now.ToString("yyyy-mm-dd")), strLineas, Program.HashConexion, AddressOf TerminoCrearArchivo, True)
            's.Close()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear el archivo", _
            Me.ToString(), "CrearArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub
    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_MODMASIVARECE)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos", _
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
        Select Case strFormat
            Case "XML"
                strBuilder.AppendLine("<Row>")
                strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
                strBuilder.AppendLine("</Row>")
                ' break;
            Case "CSV"
                strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
                ' break;

        End Select

    End Sub
    Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
        Select Case format
            Case "XML"
                Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
            Case "CSV"
                If encabezado = True Then
                    Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                Else
                    Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                End If
        End Select
        Return data

    End Function

    Private Sub importar()
        Try
            dcProxy.tblLogReceptores.Clear()
            dcProxy.Load(dcProxy.ActualizarReceptoresclienteEspecificoQuery(GeneraArchivoCliSelected.NombreArchivoAC, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf Terminoactualizarreceptoresespecifico, Nothing)
            'Dim archivo As StreamReader = File.OpenText(GeneraArchivoCliSelected.NombreArchivoAC)
            ''Recorremos el archivo de encabezados
            'Dim xmlResultado = "<Receptores>"
            'Do While archivo.Peek() <> -1
            '    Dim e = archivo.ReadLine().Split(",")
            '    xmlResultado = xmlResultado + "<actualizar codCliente=" + ChrW(34) + e(0).Trim + ChrW(34) + " receptorAnterior=" + ChrW(34) + e(1).Trim + ChrW(34) + " receptorActual=" + ChrW(34) + e(2).Trim + ChrW(34) + "/>"
            'Loop
            'xmlResultado = xmlResultado + "</Receptores>"
            'archivo.Close()
            'ActualizarEspecificos(xmlResultado)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar", _
            Me.ToString(), "importarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub
    'Private Sub ActualizarEspecificos(ByVal strxml As String)
    '    Try
    '        'Dim strxmlseguro As String = System.Web.HttpUtility.HtmlEncode(strxml)
    '        'dcProxy.tblLogReceptores.Clear()
    '        'dcProxy.Load(dcProxy.ActualizarReceptoresclienteEspecificoQuery(strxmlseguro, Program.Usuario, Program.HashConexion), AddressOf Terminoactualizarreceptoresespecifico, Nothing)
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ActualizarEspecificos", _
    '        Me.ToString(), "ActualizarEspecificos", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub
    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Dim objDialog = CType(sender, OpenFileDialog)
        GeneraArchivoCliSelected.NombreArchivoAC = Path.GetFileName(objDialog.FileName)
    End Sub
#End Region
#Region "Propiedades"

    Private _GeneraArchivoCliSelected As New GenerarArchivoCli
    Public Property GeneraArchivoCliSelected As GenerarArchivoCli
        Get
            Return _GeneraArchivoCliSelected
        End Get
        Set(value As GenerarArchivoCli)
            _GeneraArchivoCliSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("GeneraArchivoCliSelected"))
        End Set
    End Property
    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class GenerarArchivoCli
    Implements INotifyPropertyChanged
    Private _Receptorant As String
    <Display(Name:="Receptor Anterior")> _
    Public Property Receptorant As String
        Get
            Return _Receptorant
        End Get
        Set(value As String)
            _Receptorant = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptorant"))
        End Set
    End Property
    Private _Receptoract As String
    <Display(Name:="Receptor Actual    ")> _
    Public Property Receptoract As String
        Get
            Return _Receptoract
        End Get
        Set(value As String)
            _Receptoract = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptoract"))
        End Set
    End Property
    Private _NombreArchivoAC As String
    <Display(Name:=" ")> _
    Public Property NombreArchivoAC As String
        Get
            Return _NombreArchivoAC
        End Get
        Set(value As String)
            _NombreArchivoAC = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreArchivoAC"))
        End Set
    End Property
    Private _NombreArchivo As String
    <Display(Name:="Nombre de archivo")> _
    Public Property NombreArchivo As String
        Get
            Return _NombreArchivo
        End Get
        Set(value As String)
            _NombreArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreArchivo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
