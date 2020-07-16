
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
'Modificación: Se cambia el mensaje que se muestra al final de la carga para que se muestre tambien el poup de importación
'Responsable: Santiago Vergara
'Fecha: Noviembre 14/2013
'---------------------------------------------------------------------
'Modificación: Se quita de la vista el combo de comitente para que este sea cargado tambient en el archivo
'Responsable: Santiago Vergara
'Fecha: Marzo 07/2014


Imports OpenRiaServices.DomainServices.Client
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2ComunesImportaciones
Imports System.Text

Partial Public Class LiquidacionesOTC_SEN
    Inherits UserControl
    Implements INotifyPropertyChanged

    Dim dcProxy As OTCDomainContext
    Private Const _STR_NOMBRE_PROCESO As String = "OTC_SEN"
    Private bitTerminoCarga As Boolean = False
    Dim sb As New StringBuilder
    Dim sbGuardado As StringBuilder
    Private intContActualizados As Integer

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New()
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        ucbtnCargar.Proceso = _STR_NOMBRE_PROCESO
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New OTCDomainContext()
        Else
            dcProxy = New OTCDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OTCDomainContext.IOTCDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
    End Sub

#Region "Propiedades"

    Private _IsBusy As Boolean
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(value As Boolean)
            _IsBusy = value
            BI.IsBusy = _IsBusy
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Public _ListaOperaciones As New EntitySet(Of tblImportacionLiqSEN)
    Public Property ListarOperaciones As EntitySet(Of tblImportacionLiqSEN)
        Get
            Return _ListaOperaciones
        End Get
        Set(value As EntitySet(Of tblImportacionLiqSEN))
            _ListaOperaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListarOperacionesSEN"))
        End Set
    End Property

    Public _listaResultado As New ObservableCollection(Of tmpResultado)
    Public Property ListaResultados As ObservableCollection(Of tmpResultado)
        Get
            Return _listaResultado
        End Get
        Set(value As ObservableCollection(Of tmpResultado))
            _listaResultado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaResultados"))
        End Set
    End Property

    Private _ListaParaMostrar As New List(Of tblImportacionLiqSEN)
    Public ReadOnly Property ListaParaMostrar() As List(Of tblImportacionLiqSEN)
        Get
            Return _ListaParaMostrar
        End Get
    End Property

    Private _ListaOperacionesActualizadas As New List(Of tblImportacionLiqSEN)
    Public Property ListaOperacionesActualizadas As List(Of tblImportacionLiqSEN)
        Get
            Return _ListaOperacionesActualizadas
        End Get
        Set(value As List(Of tblImportacionLiqSEN))
            _ListaOperacionesActualizadas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOperacionesActualizadas"))
        End Set
    End Property

    Private _strMensajeActualizacion As String
    Public Property MensajeActualizacion As String
        Get
            Return _strMensajeActualizacion
        End Get
        Set(value As String)
            _strMensajeActualizacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MensajeActualizacion"))
        End Set
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

    Private _intRegistrosActualizados As Integer = 0
    Public Property RegistrosActualizados() As Integer
        Get
            Return _intRegistrosActualizados
        End Get
        Set(ByVal value As Integer)
            _intRegistrosActualizados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistrosActualizados"))
        End Set
    End Property


#End Region

#Region "Asíncronos"

    ''' <summary>
    ''' Método que recibe el resultado del archivo importado.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    Public Sub Terminocargararchivo(ByVal lo As LoadOperation(Of tblImportacionLiqSEN))

        Dim intNoValidos As Integer = 0
        Try
            If Not lo.HasError Then
                IsBusy = False
                sb.Clear()
                Dim cfe As New LogImportacion
                For Each mensaje In lo.Entities
                    If mensaje.intResultado < 0 Then
                        cfe.texto = String.Empty
                        sb.AppendLine(mensaje.ListaComentario)
                        Program.Modal_OwnerMainWindowsPrincipal(cfe)
                        cfe.texto = sb.ToString

                        intNoValidos += 1
                    End If
                Next
                cfe.ShowDialog()

                ListarOperaciones = dcProxy.tblImportacionLiqSENs
                Dim qryOperaciones = (From q2 In ListarOperaciones
                                      Where q2.intResultado = 0).ToList
                _ListaParaMostrar.Clear()
                _ListaParaMostrar = qryOperaciones
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaRegPaged"))
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListarOperaciones"))

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo",
                                                 Me.ToString(), "Terminocargararchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo",
            Me.ToString(), "Terminocargararchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Metodo para limpiar los objetos del Grid.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LimpiarObjetosLista()
        txtArchivoSeleccionado.Text = String.Empty

        If Not IsNothing(ListaParaMostrar) Then
            ListaParaMostrar.Clear()
        End If
        If Not IsNothing(ListarOperaciones) Then
            ListarOperaciones.Clear()
        End If
        dcProxy.tblImportacionLiqSENs.Clear()
        dcProxy.tmpResultados.Clear()
        ListaOperacionesActualizadas.Clear()
        sb.Clear()
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaRegPaged"))
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListarOperaciones"))
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaOperacionesActualizadas"))
    End Sub

    ''' <summary>
    ''' Función encargada de verificar los datos validos.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DatosValidos() As Boolean
        Dim bitDatosValidos As Boolean = True
        Dim strMensaje = String.Empty
        'Santiago Vergara - Marzo 07/2014 - Se quitan las validacines del comitente
        If String.IsNullOrEmpty(txtArchivoSeleccionado.Text) Then
            strMensaje = strMensaje & vbCrLf & "** Debe escoger la ruta y el nombre del archivo a subir "
            bitDatosValidos = False
        End If
        If ListaParaMostrar.Count = 0 Or IsNothing(ListaParaMostrar) Then
            strMensaje = strMensaje & vbCrLf & "** Debe cargar el archivo con las liquidaciones validas "
            bitDatosValidos = False
        End If

        If Not bitDatosValidos Then
            Dim strMensajeMostrar = "Se presentarón las siguientes inconsistencias: " & vbCrLf & strMensaje
            A2Utilidades.Mensajes.mostrarMensaje(strMensajeMostrar, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
        End If
        Return bitDatosValidos
    End Function

    ''' <summary>
    ''' Metodo encargado de recibir la respuesta de la pregunta de generación de operaciones OTC.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Terminopreguntaactualizar(ByVal sender As Object, ByVal e As EventArgs)
        Dim intNroRegistros As Integer = 0
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        Dim strRegistrosEnviar As String = String.Empty
        Try
            If objResultado.DialogResult Then
                If Not IsNothing(ListaParaMostrar) Then
                    ListaOperacionesActualizadas.Clear()
                    MensajeActualizacion = String.Empty
                End If

                IsBusy = True

                sbGuardado = New StringBuilder
                intContActualizados = 0

                For Each objDato In ListaParaMostrar
                    If String.IsNullOrEmpty(strRegistrosEnviar) Then
                        strRegistrosEnviar = String.Format("{0:MM/dd/yyyy}**{1}**{2}**{3}**{4}**{5}**{6}**{7}**{8}**{9}**{10}**{11:MM/dd/yyyy}**{12:MM/dd/yyyy}**{13:MM/dd/yyyy}**{14}**{15}**{16}**{17}",
                                                           objDato.dtmFechaImportacion,
                                                           objDato.dtmHoraImportacion,
                                                           objDato.lngIDOperacion,
                                                           objDato.lngIdComitente,
                                                           objDato.strTipo,
                                                           objDato.strEspecie,
                                                           objDato.dblCantidad,
                                                           objDato.lngDiasVencimiento,
                                                           objDato.curEquivalente,
                                                           objDato.curTotal,
                                                           objDato.curPrecio,
                                                           objDato.dtmEmision,
                                                           objDato.dtmVencimiento,
                                                           objDato.dtmLiquidacion,
                                                           objDato.strTipoNegociacion,
                                                           objDato.strISIN,
                                                           objDato.dblCantidadGarantia,
                                                           objDato.strRueda)
                    Else
                        strRegistrosEnviar = String.Format("{0}|{1:MM/dd/yyyy}**{2}**{3}**{4}**{5}**{6}**{7}**{8}**{9}**{10}**{11}**{12:MM/dd/yyyy}**{13:MM/dd/yyyy}**{14:MM/dd/yyyy}**{15}**{16}**{17}**{18}",
                                                           strRegistrosEnviar,
                                                           objDato.dtmFechaImportacion,
                                                           objDato.dtmHoraImportacion,
                                                           objDato.lngIDOperacion,
                                                           objDato.lngIdComitente,
                                                           objDato.strTipo,
                                                           objDato.strEspecie,
                                                           objDato.dblCantidad,
                                                           objDato.lngDiasVencimiento,
                                                           objDato.curEquivalente,
                                                           objDato.curTotal,
                                                           objDato.curPrecio,
                                                           objDato.dtmEmision,
                                                           objDato.dtmVencimiento,
                                                           objDato.dtmLiquidacion,
                                                           objDato.strTipoNegociacion,
                                                           objDato.strISIN,
                                                           objDato.dblCantidadGarantia,
                                                           objDato.strRueda)
                    End If
                Next

                If Not IsNothing(dcProxy.tblImportacionLiqSENs) Then
                    dcProxy.tblImportacionLiqSENs.Clear()
                End If

                dcProxy.Load(dcProxy.ActualizarOperacionesSENMasivoQuery(strRegistrosEnviar, Program.Usuario, Program.HashConexion), AddressOf TerminoActualizarEstados, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema",
         Me.ToString(), "Terminopreguntaactualizar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que recibe la respuesta de los estados que se actualizaron.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminoActualizarEstados(ByVal obj As LoadOperation(Of tblImportacionLiqSEN))

        Try
            If Not obj.HasError Then
                RegistrosActualizados += 1
                ListarOperaciones = dcProxy.tblImportacionLiqSENs
                Dim qryOperaciones = (From q2 In ListarOperaciones
                                      Where Not IsNothing(q2.ListaComentario)).ToList

                ListaOperacionesActualizadas = qryOperaciones

                For Each objItem In ListaOperacionesActualizadas
                    sbGuardado.AppendLine(objItem.ListaComentario)
                    If objItem.intResultado = 0 Then
                        intContActualizados += 1
                    End If
                Next

                sbGuardado.AppendLine("Proceso Finalizado...Registros Actualizados: " & intContActualizados.ToString)
                IsBusy = False
                Dim cfe As New LogImportacion
                Program.Modal_OwnerMainWindowsPrincipal(cfe)
                cfe.texto = sbGuardado.ToString
                cfe.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar las operaciones", _
                                               Me.ToString(), "TerminoActualizarEstados", Application.Current.ToString(), Program.Maquina, obj.Error)
                obj.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar las operaciones", _
                Me.ToString(), "TerminoActualizarEstados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            LimpiarObjetosLista()
            txtArchivoSeleccionado.Text = String.Empty
        End Try
    End Sub

#End Region

#Region "Eventos Controles"

    ''' <summary>
    ''' Este evento se lanza al terminar de subir el archivo al servidor.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ucbtnCargar_CargarArchivo(sender As OpenFileDialog, e As System.IO.Stream) Handles ucbtnCargar.CargarArchivo
        Try
            'metodo que se ejecuta cuando se cierra el explorador de archivos y termina de subir el archivo al servidor
            LimpiarObjetosLista()
            IsBusy = True
            Dim objDialog = CType(sender, OpenFileDialog)
            txtArchivoSeleccionado.Text = Path.GetFileName(objDialog.FileName)
            'Santiago Vergara - Marzo 07/2014 - Se quita l llamado a las valizaciones de comitente y el parametro comitente en el llamado al servicio
            dcProxy.Load(dcProxy.LeerArchivoOperaciones_SENQuery(txtArchivoSeleccionado.Text, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf Terminocargararchivo, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar cargar el archivo", _
                     Me.ToString(), "ucbtnCargar_CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se encargado de cargar los datos a operaciones OTC.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs) Handles btnAceptar.Click
        Try
            If DatosValidos() Then
                'C1.Silverlight.C1MessageBox.Show("Esta seguro de ingresar las operaciones a OTC? ", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminopreguntaactualizar)
                mostrarMensajePregunta("Esta seguro de ingresar las operaciones a OTC? ", _
                                       Program.TituloSistema, _
                                       "ACEPTAR", _
                                       AddressOf Terminopreguntaactualizar, False)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", _
                Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs) Handles btnCancelar.Click
        LimpiarObjetosLista()
    End Sub

    Private Sub btnAyuda_Click(sender As Object, e As RoutedEventArgs) Handles btnAyuda.Click
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Formato Operaciones OTC - SEN -->" + vbCrLf + "Fecha(DateTime), Hora(DateTime), Secuencia(Entero), Tipo Operación(Texto), Especie(Texto), Cantidad(Numérico), Días Vencimiento Título(Entero), Equivalente(Entero), Total(Numérico), Precio/Tasa(Decimal), Fecha Emisión(DateTime), Fecha Vencimiento Título(Datetime), Fecha Liquidación(DateTime), Tipo Negociación(Texto), Isin título(Texto), Rueda(Texto), Cantidad Garantía(Numérico), Comitente(Texto)", _
                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar la ventana de ayuda", _
                Me.ToString(), "btnAyuda_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

End Class
