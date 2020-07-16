Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports A2Utilidades.Mensajes
Imports A2ComunesImportaciones

Public Class ImportarRecaudosXReferenciaViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Procesar Portafolio perteneciente al proyecto de Cálculos Financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Catalina Dávila (IoSoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 26 de Junio/2016
    ''' Pruebas CB       : Jorge Peña - 26 de Junio/2016 - Resultado Ok 
    ''' </history>

#Region "Variables - REQUERIDO"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext
    Dim objProxyCarga As ImportacionesDomainContext
    Dim dcProxy As TesoreriaDomainContext

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                objProxyCarga = New ImportacionesDomainContext()
                dcProxy = New TesoreriaDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxyCarga = New ImportacionesDomainContext(New System.Uri((Program.RutaServicioImportaciones)))
                dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(objProxyCarga.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)

            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "ImportarRecaudosXReferenciaViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function inicializar() As Boolean
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

            End If

            FechaRegistro = Now
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
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    ''' 
    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private WithEvents _ListaEncabezado As List(Of ImportarRecaudosXReferencia)
    Public Property ListaDetalle() As List(Of ImportarRecaudosXReferencia)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of ImportarRecaudosXReferencia))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
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

    Private _intSucursal As Integer
    Public Property intSucursal() As Integer
        Get
            Return _intSucursal
        End Get
        Set(ByVal value As Integer)
            _intSucursal = value
            MyBase.CambioItem("intSucursal")
        End Set
    End Property

    Private _Total As Double
    Public Property Total() As Double
        Get
            Return _Total
        End Get
        Set(ByVal value As Double)
            _Total = value
            MyBase.CambioItem("Total")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As ImportarRecaudosXReferencia
    Public Property DetalleSeleccionado() As ImportarRecaudosXReferencia
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ImportarRecaudosXReferencia)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _ViewImportarArchivo As cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private _FechaRegistro As Nullable(Of DateTime) = Now
    Public Property FechaRegistro() As Nullable(Of DateTime)
        Get
            Return FechaRegistro
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _FechaRegistro = value
            If IsNothing(_FechaRegistro) Then
                _FechaRegistro = Now
            End If

            MyBase.CambioItem("FechaRegistro")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub ImportarArchivo()
        Try
            If String.IsNullOrEmpty(_NombreArchivo) Then
                mostrarMensaje("Debe de seleccionar un archivo para la importación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            If Not IsNothing(objProxyCarga.RespuestaArchivoImportacions) Then
                objProxyCarga.RespuestaArchivoImportacions.Clear()
            End If

            objProxyCarga.Load(objProxyCarga.RecaudosXReferencia_ImportarQuery("RecaudosXReferencia", _NombreArchivo, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Async Function TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion)) As Task
        Try
            Dim objRet As LoadOperation(Of ImportarRecaudosXReferencia)

            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim logContinuar As Boolean = False

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    Dim logContinuarConsultando As Boolean = False
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count = 0 Then
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                        logContinuarConsultando = True
                    Else
                        Dim objTipo As String = String.Empty

                        If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                            objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")
                        End If

                        For Each li In objListaRespuesta.OrderBy(Function(o) o.Tipo)
                            If objTipo <> li.Tipo And li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                                objTipo = li.Tipo
                                objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
                            ElseIf li.Tipo = "REGISTROSIMPORTADOS" Then
                                If li.Columna > 0 Then
                                    logContinuar = True
                                End If
                            End If

                            If li.Tipo <> "REGISTROSIMPORTADOS" And li.Tipo <> "REGISTROSLEIDOS" And li.Tipo <> "REGISTROSINCONSISTENTES" Then
                                objListaMensajes.Add(String.Format("Fila: {0} - Campo {1} - Validación: {2}", li.Fila, li.Campo, li.Mensaje))
                            Else
                                objListaMensajes.Add(li.Mensaje)
                            End If
                        Next
                    End If

                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"

                    If logContinuarConsultando Then

                        dcProxy.ImportarRecaudosXReferencias.Clear()
                        objRet = Await dcProxy.Load(dcProxy.RecaudosXReferencia_ConsultarQuery(Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()

                        If Not objRet Is Nothing Then
                            If objRet.HasError Then
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "ConsultarDocumentos", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If
                        End If

                        ListaDetalle = dcProxy.ImportarRecaudosXReferencias.ToList
                        CalcularTotalRegistros()
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.Title = "Subir archivo"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()

                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Function

    Public Sub GenerarRC()
        Try
            If ValidarDatos() Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Está seguro de generar los recibos de caja ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarGenerarRC, False)
            Else
                Exit Sub
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar RC", Me.ToString(), "GenerarRC", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    Public Sub ConfirmarGenerarRC(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Está totalmente seguro de generar los recibos de caja ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarTotalmenteGenerarRC, False)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar los Recibos de Caja", _
                                                             Me.ToString(), "ConfirmarGenerarRC", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Async Sub ConfirmarTotalmenteGenerarRC(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            End If

            If Not IsNothing(ListaDetalle) Then
                IsBusy = True
                GenerarTotalmenteGenerarRC()
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "ConfirmarTotalmenteGenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Async Sub GenerarTotalmenteGenerarRC()
        Try
            If Not IsNothing(ListaDetalle) Then
                Dim objRet As LoadOperation(Of ImportarRecaudosXReferencia)

                If mdcProxy Is Nothing Then
                    mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                End If

                IsBusy = True

                mdcProxy.ImportarRecaudosXReferencias.Clear()

                objRet = Await mdcProxy.Load(mdcProxy.RecaudosXReferencia_GenerarSyncQuery(_FechaRegistro, Program.Usuario, Program.UsuarioWindows, Program.Maquina, Program.HashConexion)).AsTask()


                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "SeleccionarTodos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaDetalle = mdcProxy.ImportarRecaudosXReferencias.ToList

                GenerarRecaudosXReferencia_Resultados()

                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "GenerarTotalmenteGenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub GenerarRecaudosXReferencia_Resultados()
        Try
            Dim objRet As LoadOperation(Of ResultadosGenerarRCIntermedia)
            Dim viewImportacion As New cwCargaArchivos(CType(Me, ImportarRecaudosXReferenciaViewModel), String.Empty, String.Empty)

            ViewImportarArchivo.IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If

            mdcProxy.ResultadosGenerarRCIntermedias.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.GenerarRecaudosXReferencia_ResultadosSyncQuery(Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of ResultadosGenerarRCIntermedia)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = objRet.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            For Each li In objListaRespuesta
                                objListaMensajes.Add(li.Mensaje)
                            Next
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                        End If
                    End If
                End If
            End If

            ViewImportarArchivo.IsBusy = False
            Total = 0

            Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
            viewImportacion.ShowDialog()



        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el resultado de validaciones.", Me.ToString(), "GenerarCE_Intermedia_Resultados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarArchivo(ByVal pstrModulo As String, ByVal _NombreArchivo As String)
        Try
            'Este método se utiliza vacío para que la pantalla de resultados no falle, ya que valida que este método exista en el ViewModel.
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método CargarArchivo. ", Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------


            If IsNothing(ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe cargar primero los documentos para poder generar.", strMsg, vbCrLf)
            ElseIf ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe cargar primero los documentos para poder generar.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                IsBusy = False
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Sub CalcularTotalRegistros()
        Try
            If Not IsNothing(_ListaEncabezado) Then
                Total = 0
                For Each objeto In (From c In _ListaEncabezado)
                    Total = Total + objeto.dblValorFormateado
                Next
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar todos los registros.", _
                                                             Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _DetalleSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        Try
            If e.PropertyName = "logGenerar" Then

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_TesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

    'Public Event PropertyChanged1(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class

