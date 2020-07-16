Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OYD.OYDServer.RIA.Web.OyDClientes
Imports A2ComunesControl
Imports System.Web



Public Class InhabilitarClientesViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private dcProxyImportaciones As ImportacionesDomainContext
    Private dcProxyCliente As ClientesDomainContext
    Public Const INCL_NOMBRE_PROCESO As String = "ClientesInhabilitados"
    Private mdcProxyUtilidad01 As UtilidadesDomainContext

#Region "Properties"

    Private _ListaInhabilitados As List(Of ClientesInhabilitados)
    Public Property ListaInhabilitados() As List(Of ClientesInhabilitados)
        Get
            Return _ListaInhabilitados
        End Get
        Set(ByVal value As List(Of ClientesInhabilitados))
            _ListaInhabilitados = value
            MyBase.CambioItem("ListaInhabilitados")
            MyBase.CambioItem("ListaInhabilitadosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaInhabilitadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaInhabilitados) Then
                Dim view = New PagedCollectionView(_ListaInhabilitados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _listaConceptoInhabilitados As New List(Of OYDUtilidades.ItemCombo)
    Public Property listaConceptoInhabilitados() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaConceptoInhabilitados
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaConceptoInhabilitados = value
            MyBase.CambioItem("listaConceptoInhabilitados")
        End Set
    End Property


    Private _strTipoConcepto As String
    Public Property strTipoConcepto() As String
        Get
            Return _strTipoConcepto
        End Get
        Set(ByVal value As String)
            _strTipoConcepto = value
        End Set
    End Property


    Private _NombreArchivo As String
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property



    Private _strRutaFormato As String
    Public Property strRutaFormato() As String
        Get
            Return _strRutaFormato
        End Get
        Set(ByVal value As String)
            _strRutaFormato = value
            MyBase.CambioItem("strRutaFormato")
        End Set
    End Property

    Private _TipoCargaInhabilitado As Boolean = True
    Public Property TipoCargaInhabilitado() As Boolean
        Get
            Return _TipoCargaInhabilitado
        End Get
        Set(ByVal value As Boolean)
            _TipoCargaInhabilitado = value
        End Set
    End Property

#Region "Commands"

    Private WithEvents _CargarInhabilitados As RelayCommand
    Public ReadOnly Property CargarInhabilitados() As RelayCommand
        Get
            If _CargarInhabilitados Is Nothing Then
                _CargarInhabilitados = New RelayCommand(AddressOf CargarClientesInhabilitados)
            End If
            Return _CargarInhabilitados
        End Get
    End Property

    Private WithEvents _ExportarInhabilitados As RelayCommand
    Public ReadOnly Property ExportarInhabilitados() As RelayCommand
        Get
            If _ExportarInhabilitados Is Nothing Then
                _ExportarInhabilitados = New RelayCommand(AddressOf ExportarClientesInhabilitados)
            End If
            Return _ExportarInhabilitados
        End Get
    End Property

    Private WithEvents _GuardarInhabilitados As RelayCommand
    Public ReadOnly Property GuardarInhabilitados() As RelayCommand
        Get
            If _GuardarInhabilitados Is Nothing Then
                _GuardarInhabilitados = New RelayCommand(AddressOf GuardarClientesInhabilitados)
            End If
            Return _GuardarInhabilitados
        End Get
    End Property

#End Region


#End Region

#Region "Metodos"
    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxyImportaciones = New ImportacionesDomainContext()
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
                dcProxyCliente = New ClientesDomainContext
            Else
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyCliente = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))

            End If

            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

            dcProxyImportaciones.ObtenerURLClientesInhabilitados(Program.Usuario, Program.HashConexion, AddressOf TerminoCargarURLInhabilitados, String.Empty)
            ConsultarConceptosInhabilitados()
            dcProxyImportaciones.ConsultarRutaFormatos(Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarFormatos, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "InhabilitarClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub GuardarClientesInhabilitados()
        Try
            IsBusy = True
            Dim Concepto = strTipoConcepto

            If IsNothing(Concepto) Then
                A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar el concepto por el cual se guardaran los clientes inhabilitados", "Clientes Inhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If IsNothing(ListaInhabilitados) Then
                A2Utilidades.Mensajes.mostrarMensaje("Primero debe de cargar la información del archivo.", "Clientes Inhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If ListaInhabilitados.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Primero debe de cargar la información del archivo.", "Clientes Inhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If Not IsNothing(Concepto) Then
                dcProxyCliente.GrabarClientesInhabilitados(ListaInhabilitados.First.RutaArchivoCarga, strRutaFormato & "ClientesInhabilitados.fmt", Concepto, Program.Usuario, Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarInhabilitados, String.Empty)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar el concepto por el cual se guardaran los clientes inhabilitados", "Clientes Inhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de grabar clientes inhabilitados", Me.ToString(), "TerminoGrabarInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    'Private Sub GuardarClientesInhabilitados()
    '    Try
    '        IsBusy = True
    '        Dim Concepto = strTipoConcepto
    '        If Not IsNothing(Concepto) Then
    '            Dim CadenaRegistros As String = String.Empty
    '            If ListaInhabilitados.Count > 0 Then
    '                For Each objCliente In ListaInhabilitados
    '                    CadenaRegistros = CadenaRegistros & objCliente.TipoIdentificacion & _
    '                                      "," & objCliente.NumeroDocumento & _
    '                                      "," & objCliente.NombreCompleto & "|"
    '                Next
    '                dcProxyCliente.GrabarClientesInhabilitados(Concepto, CadenaRegistros, Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarInhabilitados, String.Empty)

    '                'CadenaRegistros = CadenaRegistros.Remove(CadenaRegistros.Length - 1, 1)

    '            End If
    '        Else
    '            A2Utilidades.Mensajes.mostrarMensaje("Se debe seleccionar el concepto por el cual se guardaran los clientes inhabilitados", "Clientes Inhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '            IsBusy = False
    '        End If

    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de grabar clientes inhabilitados", Me.ToString(), "TerminoGrabarInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try

    'End Sub


    Private Sub ExportarClientesInhabilitados()
        Try
            If Not IsNothing(ListaInhabilitados) Then
                If ListaInhabilitados.Count > 0 Then
                    Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                    Program.VisorArchivosWeb_DescargarURL(ListaInhabilitados.First.RutaArchivo)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar archivo de clientes inhabilitados", Me.ToString(), "ExportarClientesInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
      
    End Sub

    Private Sub ExportarClientesInsertados()
        Try
            If Not IsNothing(ListaInhabilitados) Then
                If ListaInhabilitados.Count > 0 Then
                    Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                    Program.VisorArchivosWeb_DescargarURL(ListaInhabilitados.First.RutaArchivoInsertado)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al exportar archivo de clientes inhabilitados", Me.ToString(), "ExportarClientesInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub CargarClientesInhabilitados()

        Try
            IsBusy = True

            If Not IsNothing(NombreArchivo) Then
                dcProxyImportaciones.ClientesInhabilitados.Clear()
                If TipoCargaInhabilitado = True Then
                    dcProxyImportaciones.Load(dcProxyImportaciones.ClientesInhablitadosQuery(NombreArchivo, INCL_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarClientesInhabilitados, String.Empty)
                Else
                    dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionInhabilitadosQuery(NombreArchivo, INCL_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarArchivoClientesInhabilitados, "")
                End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se seleccionó ningún archivo", "CargarClientesInhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de carga de clientes inhabilitados", Me.ToString(), "CargarClientesInhabilitados", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarConceptosInhabilitados()
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad01.ItemCombos) Then
                mdcProxyUtilidad01.ItemCombos.Clear()
            End If
            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosCondicionalQuery("TIPO_CONCEPTO_INHABILITADO", Nothing, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "TIPO_CONCEPTO_INHABILITADO")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llamar la consulta de tipos de Concepto Inhabilitados", Me.ToString(), "ConsultarConceptosInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados asíncronos"

    Private Sub TerminoConsultarFormatos(ByVal lo As InvokeOperation(Of String))
        Try
            If lo.HasError = False Then
                strRutaFormato = lo.Value
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la ruta de los formatos", _
                                                Me.ToString(), "TerminoConsultarFormatos.New", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la ruta de los formatos", _
                                Me.ToString(), "TerminoConsultarFormatos.New", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoCargarURLInhabilitados(ByVal lo As InvokeOperation(Of String))
        Try
            If lo.HasError = False Then
                NombreArchivo = lo.Value
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga de URL Inhabilitados", _
                                                Me.ToString(), "TerminoCargarURLInhabilitados.New", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga de URL Inhabilitados", _
                                Me.ToString(), "TerminoCargarURLInhabilitados.New", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoGrabarInhabilitados(ByVal lo As InvokeOperation(Of Integer))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar los clientes inhabilitados", _
                Me.ToString(), "AddressOf TerminoGrabarInhabilitados", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se guardaron los clientes inhabilitados satisfactoriamente", "Clientes Inhabilitados", A2Utilidades.wppMensajes.TiposMensaje.Exito)
            End If
            ExportarClientesInsertados()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de grabar clientes inhabilitados", Me.ToString(), "TerminoGrabarInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))

        Try

            If Not lo.HasError Then

                Select Case lo.UserState
                    Case "TIPO_CONCEPTO_INHABILITADO"
                        listaConceptoInhabilitados = lo.Entities.ToList
                End Select

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                       Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub


    Private Sub TerminoCargarClientesInhabilitados(ByVal lo As LoadOperation(Of OyDImportaciones.ClientesInhabilitados))
        Try
            If lo.HasError = False Then

                Dim objListaRespuesta As List(Of OyDImportaciones.ClientesInhabilitados)

                objListaRespuesta = lo.Entities.ToList

                ListaInhabilitados = objListaRespuesta

                MyBase.CambioItem("ListaInhabilitados")
                MyBase.CambioItem("ListaInhabilitadosPaged")


            Else
                If Not IsNothing(lo.Error) Then
                    If lo.Error.Message.Contains("ERRORLECTURAURL:") Then
                        Dim strMensajeUsuario As String = lo.Error.Message.Split("|")(1)
                        A2Utilidades.Mensajes.mostrarMensaje(strMensajeUsuario, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        lo.MarkErrorAsHandled() '????
                        IsBusy = False
                        Exit Sub
                    End If
                End If

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los clientes inhabilitados", _
                                 Me.ToString(), "TerminoCargarClientesInhabilitados", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled() '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de cargar los clientes inhabilitados", Me.ToString(), "TerminoCargarClientesInhabilitados", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try
    End Sub

    Sub TerminoCargarArchivoClientesInhabilitados(lo As LoadOperation(Of ClientesInhabilitados))
        Try
            If lo.HasError = False Then

                Dim objListaRespuesta As List(Of OyDImportaciones.ClientesInhabilitados)

                objListaRespuesta = lo.Entities.ToList

                If Not IsNothing(objListaRespuesta) Then
                    ListaInhabilitados = objListaRespuesta
                    MyBase.CambioItem("ListaInhabilitados")
                    MyBase.CambioItem("ListaInhabilitadosPaged")
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga de los clientes inhabilitados", _
                                 Me.ToString(), "TerminoCargarArchivoClientesInhabilitados", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled() '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga de los clientes inhabilitados", _
                                                 Me.ToString(), "TerminoCargarArchivoClientesInhabilitados", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

#End Region


End Class

