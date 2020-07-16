
Imports System.IO

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


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDClientes
Imports Microsoft.VisualBasic.CompilerServices
Imports A2ComunesImportaciones
Imports System.Text
Imports A2Utilidades.Mensajes

Partial Public Class ActualizarInformacionFinanciera
    Inherits UserControl
    Implements INotifyPropertyChanged
    Dim dcProxy As ClientesDomainContext
    Private Const _STR_NOMBRE_PROCESO As String = "LeerArchivoDatosFinancieros"
    Dim sb As New StringBuilder

    Public Sub New()
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
    End Sub
#Region "Metodos"
    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs) Handles btnAceptar.Click
        Try
            If ListaDatosFinancieros.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe selecccionar el archivo", _
                            Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            'C1.Silverlight.C1MessageBox.Show("Esta seguro de realizar la actualización de los clientes", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminopreguntaactualizar)
            mostrarMensajePregunta("¿Esta seguro de realizar la actualización de los clientes?", _
                                   Program.TituloSistema, _
                                   "ACTUALIZARCLIENTE", _
                                   AddressOf Terminopreguntaactualizar, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
                Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub btnAyuda_Click(sender As Object, e As RoutedEventArgs) Handles btnAyuda.Click
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Formato de archivo Actualización Financiera clientes-->" + vbCrLf + "Nro Documento (Alfanumerico de 15),Ingresos(Númerico),Egresos(Númerico),Activos(Númerico),Pasivos(Númerico),Codigo CIIU(Númerico),Segmento Comercial(Númerico)", _
                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
                Me.ToString(), "btnAyuda_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Try
            'metodo que se ejecuta cuando se cierra el explorador de archivos y termina de subir el archivo al servidor
            Dim objDialog = CType(sender, OpenFileDialog)
            txtNombrearchivo.Text = Path.GetFileName(objDialog.FileName)
            IsBusy = True
            dcProxy.Load(dcProxy.LeerArchvioDatosFinancierosQuery(txtNombrearchivo.Text, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf Terminocargararchivo, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
                     Me.ToString(), "ucbtnCargar_CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Propiedades"
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

    Public _ListaDatosFinancieros As New ObservableCollection(Of OyDClientes.tblConsultaDatosfinancieros)
    Public Property ListaDatosFinancieros As ObservableCollection(Of OyDClientes.tblConsultaDatosfinancieros)
        Get
            Return _ListaDatosFinancieros
        End Get
        Set(value As ObservableCollection(Of OyDClientes.tblConsultaDatosfinancieros))
            _ListaDatosFinancieros = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaDatosFinancieros"))
        End Set
    End Property


#End Region
#Region "Asincronicos"
    Public Sub Terminocargararchivo(ByVal lo As LoadOperation(Of OyDClientes.tblConsultaDatosfinancieros))
        Try
            If Not lo.HasError Then
                IsBusy = False
                'ListaDatosFinancieros = dcProxy.tblConsultaDatosfinancieros.ToList
                For Each comentario In dcProxy.tblConsultaDatosfinancieros
                    If comentario.ListaComentario <> String.Empty Then
                        sb.AppendLine(comentario.ListaComentario)
                    Else
                        ListaDatosFinancieros.Add(comentario)
                    End If
                Next
                'AddHandler cfe.Closed, AddressOf CerroVentana
                Dim cfe As New LogImportacion
                Program.Modal_OwnerMainWindowsPrincipal(cfe)
                cfe.texto = String.Empty
                cfe.texto = sb.ToString
                cfe.ShowDialog()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
            Me.ToString(), "Terminocargararchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    'Private Sub CerroVentana()
    '    If cfe.DialogResult = True Then
    '        'Dim ce As New ViewClientes_Exento
    '        'ce.Comitente = cfe.txtFiltro.Text
    '        'ce.Nombre = cfe.nombre
    '        'vm.ViewClientesExentoSelected = ce
    '        'vm.ViewClientesExentoSelected.Comitente = cfe.txtFiltro.Text
    '        'vm.ViewClientesExentoSelected.Nombre = cfe.nombre
    '        'vm.CambioViewClientesExentoSelected()
    '    End If
    'End Sub
    Private Sub Terminopreguntaactualizar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                Dim xmlResultado = ""
                For Each li In ListaDatosFinancieros
                    xmlResultado = xmlResultado + "<Datos "
                    xmlResultado = xmlResultado + " NroIdentificacion=" + ChrW(34) + li.Nrodocumento + ChrW(34)
                    xmlResultado = xmlResultado + " Ingresos=" + ChrW(34) + li.Ingresos.ToString + ChrW(34)
                    xmlResultado = xmlResultado + " Egresos=" + ChrW(34) + li.Egresos.ToString + ChrW(34)
                    xmlResultado = xmlResultado + " Activos=" + ChrW(34) + li.Activos.ToString + ChrW(34)
                    xmlResultado = xmlResultado + " Pasivos=" + ChrW(34) + li.Pasivos.ToString + ChrW(34)
                    xmlResultado = xmlResultado + " CodigoCiiu=" + ChrW(34) + li.CodigoCIIU + ChrW(34)
                    xmlResultado = xmlResultado + " SegComercial=" + ChrW(34) + li.SegmentoComercial + ChrW(34)
                    xmlResultado = xmlResultado + "></Datos>"
                Next
                xmlResultado = "<Root>" + xmlResultado + "</Root>"
                Dim strxmlseguro As String = System.Web.HttpUtility.HtmlEncode(xmlResultado)
                dcProxy.Load(dcProxy.ActualizarEstadosFinancierosQuery(strxmlseguro, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarEstados, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
         Me.ToString(), "Terminopreguntaactualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub TerminoActualizarEstados(ByVal obj As LoadOperation(Of OyDClientes.tblConsultaDatosfinancieros))
        Try
            If Not obj.HasError Then
                A2Utilidades.Mensajes.mostrarMensaje("Proceso Finalizado...Registros Actualizados:" + ListaDatosFinancieros.Count.ToString, _
                          Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ListaDatosFinancieros.Clear()
                txtNombrearchivo.Text = String.Empty
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
                Me.ToString(), "TerminoActualizarEstados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class
