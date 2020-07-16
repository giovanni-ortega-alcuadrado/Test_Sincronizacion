Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel
Imports SpreadsheetGear
Imports System.IO
Imports A2MCCoreWPF
Imports GalaSoft.MvvmLight.Threading
Imports A2.Notificaciones.Cliente
Imports A2.OYD.OYDServer.RIA
Imports A2.OYD.OYDServer.RIA.Web.CFUtilidades
Imports A2CFUtilitarios


Public Class Omnibus_GenerarArchivosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Declaracion de Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidades As UtilidadesDomainContext
#End Region

    Public Async Function inicializar() As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                If IsNothing(mdcProxy) Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If
                MostrarInversionista = Visibility.Collapsed
                Await ConsultarGestores()
                Await ConsultarCombosGenerico()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Async Function ConsultarGestores() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Omnibus_Configuracion_Generacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            mdcProxy.Omnibus_Configuracion_Generacions.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.Omnibus_ConsultarConfiguracionSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarGestores", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaGestores = objRet.Entities.ToList

                    If ListaGestores.Count = 1 Then
                        intIDGestor = ListaGestores.First.intIDEntidad
                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarGestores", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Async Sub HabilitarConfiguracionGestor()
        Try
            SeleccionarTodosFondos = False
            SeleccionarTodosTiposMovimientos = False

            If intIDGestor > 0 Then
                If ListaGestores.Where(Function(i) CInt(i.intIDEntidad) = CInt(intIDGestor)).Count() > 0 Then
                    Dim objRegistroSeleccionadoGestor As Omnibus_Configuracion_Generacion = Nothing
                    objRegistroSeleccionadoGestor = ListaGestores.Where(Function(i) CInt(i.intIDEntidad) = CInt(intIDGestor)).First

                    If objRegistroSeleccionadoGestor.logMultiseleccionCompania Then
                        MostrarMultiseleccionCompanias = True
                    Else
                        MostrarMultiseleccionCompanias = False
                    End If

                    If objRegistroSeleccionadoGestor.logHabilitarOpcionTodos_TipoMovimiento Then
                        MostrarMultiseleccionTipoMovimiento = True
                    Else
                        MostrarMultiseleccionTipoMovimiento = False
                    End If

                    VerificarHabilitarBanco()
                    VerificarHabilitarInversionista()


                    HabilitarSeleccionExportacion = True

                    Await ConsultarCombosGestor()
                End If
            Else
                HabilitarSeleccionExportacion = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar la configuración del gestor.", Me.ToString(), "HabilitarConfiguracionGestor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarTiposInvesionistas() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Omnibus_Combos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            mdcProxy.Omnibus_Combos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.Omnibus_ConsultarCombosSyncQuery(0, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaComboModulos = Nothing
                    If objRet.Entities.Where(Function(i) i.Categoria = "CARGASARCHIVOS").Count > 0 Then
                        ListaComboModulos = objRet.Entities.Where(Function(i) i.Categoria = "CARGASARCHIVOS").ToList
                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

  
    Private Sub VerificarHabilitarBanco()
        Try
            MostrarSeleccionBanco = False

            If Not String.IsNullOrEmpty(strTipoMovimiento) Then
                If Not IsNothing(_ListaComboTipoMovimientos) Then
                    If _ListaComboTipoMovimientos.Where(Function(i) i.ID = strTipoMovimiento).Count > 0 Then
                        If _ListaComboTipoMovimientos.Where(Function(i) i.ID = strTipoMovimiento).First.Retorno = "1" Then
                            MostrarSeleccionBanco = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar la configuración del gestor.", Me.ToString(), "VerificarHabilitarBanco", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub VerificarHabilitarInversionista()
        Try


            MostrarInversionista = Visibility.Collapsed

            If Not IsNothing(strCodigoFondo) Then
                mdcProxy.ConsultarCompaniaHabilitarInversionista(strCodigoFondo, Program.Usuario, Program.HashConexion, AddressOf ConsultarCompaniaHabilitarInversionistaCompleted, "")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el tipo de inversionista.",
                                 Me.ToString(), "VerificarHabilitarInversionista", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub ConsultarCompaniaHabilitarInversionistaCompleted(ByVal obj As InvokeOperation(Of String))
        Try
            Dim strMsg As String = String.Empty
            If obj.HasError Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validación para el tipo de inversionista", Me.ToString(), "ConsultarCompaniaHabilitarInversionistaCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

            Else
                If Not IsNothing(ListaComboTipoMovimientos) Then
                    If Not String.IsNullOrEmpty(strTipoMovimiento) Then
                        If Not String.IsNullOrEmpty(obj.Value.ToString()) And
                            _ListaComboTipoMovimientos.Where(Function(i) i.ID = strTipoMovimiento).Count > 0 Then
                            If _ListaComboTipoMovimientos.Where(Function(i) i.ID = strTipoMovimiento).First.ID = "CONSTITUCION" Then
                                MostrarInversionista = Visibility.Visible
                            Else
                                MostrarInversionista = Visibility.Collapsed
                            End If
                        Else
                            MostrarInversionista = Visibility.Collapsed
                        End If
                    Else
                        MostrarInversionista = Visibility.Collapsed

                    End If

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion del tipo de inversionista",
                                                 Me.ToString(), "ConsultarCompaniaHabilitarInversionistaCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub ConcatenarFondos()
        Try
            If MostrarMultiseleccionCompanias Then
                FondosConcatenados = String.Empty
                If Not IsNothing(ListaFondosMultiseleccion) Then
                    For Each li In ListaFondosMultiseleccion
                        If li.Seleccionado Then
                            If String.IsNullOrEmpty(FondosConcatenados) Then
                                FondosConcatenados = li.ID
                            Else
                                FondosConcatenados = String.Format("{0}*{1}", FondosConcatenados, li.ID)
                            End If
                        End If
                    Next
                End If
            Else
                FondosConcatenados = strCodigoFondo

                If Not IsNothing(_ListaComboFondos) Then
                    If _ListaComboFondos.Where(Function(i) i.ID = strCodigoFondo).Count > 0 Then
                        If Not IsNothing(_ListaComboFondos.Where(Function(i) i.ID = strCodigoFondo).First.intID) Then
                            intCodigoFondo = CInt(_ListaComboFondos.Where(Function(i) i.ID = strCodigoFondo).First.intID)
                        End If
                    End If
                End If
                
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los fondos.", Me.ToString(), "ConcatenarFondos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConcatenarTiposMovimientos()
        Try
            If MostrarMultiseleccionTipoMovimiento Then
                TipoMovimientoConcatenado = String.Empty
                If Not IsNothing(ListaTipoMovimientoMultiseleccion) Then
                    For Each li In ListaTipoMovimientoMultiseleccion
                        If li.Seleccionado Then
                            If String.IsNullOrEmpty(TipoMovimientoConcatenado) Then
                                TipoMovimientoConcatenado = li.ID
                            Else
                                TipoMovimientoConcatenado = String.Format("{0}*{1}", TipoMovimientoConcatenado, li.ID)
                            End If
                        End If
                    Next
                End If
            Else
                TipoMovimientoConcatenado = strTipoMovimiento
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los fondos.", Me.ToString(), "ConcatenarTiposMovimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarCombosGestor() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Omnibus_Combos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            mdcProxy.Omnibus_Combos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.Omnibus_ConsultarCombosSyncQuery(CInt(intIDGestor), Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaCombosCompleta = objRet.Entities.ToList

                    If MostrarMultiseleccionCompanias Then
                        Dim objListaMultiseleccionFondo As New List(Of clsOmnibus_SeleccionRegistros)
                        If ListaCombosCompleta.Where(Function(i) i.Categoria = "FONDOSMULTISELECCION").Count > 0 Then
                            For Each li In ListaCombosCompleta.Where(Function(i) i.Categoria = "FONDOSMULTISELECCION").ToList
                                objListaMultiseleccionFondo.Add(New clsOmnibus_SeleccionRegistros With {.ID = li.ID,
                                                                                                        .Descripcion = li.Descripcion,
                                                                                                        .Seleccionado = False})
                            Next
                        End If

                        ListaFondosMultiseleccion = objListaMultiseleccionFondo
                    Else
                        ListaComboFondos = Nothing
                        If objRet.Entities.Where(Function(i) i.Categoria = "FONDOS").Count > 0 Then
                            ListaComboFondos = objRet.Entities.Where(Function(i) i.Categoria = "FONDOS").ToList

                            If ListaComboFondos.Count = 1 Then
                                strCodigoFondo = ListaComboFondos.First.ID
                            End If
                        End If
                    End If

                    If MostrarMultiseleccionTipoMovimiento Then
                        Dim objListaMultiseleccionTipoMovimiento As New List(Of clsOmnibus_SeleccionRegistros)
                        If ListaCombosCompleta.Where(Function(i) i.Categoria = "TIPOMOVIMIENTOMULTISELECCION").Count > 0 Then
                            For Each li In ListaCombosCompleta.Where(Function(i) i.Categoria = "TIPOMOVIMIENTOMULTISELECCION").ToList
                                objListaMultiseleccionTipoMovimiento.Add(New clsOmnibus_SeleccionRegistros With {.ID = li.ID,
                                                                                                        .Descripcion = li.Descripcion,
                                                                                                        .Seleccionado = False})
                            Next
                        End If

                        ListaTipoMovimientoMultiseleccion = objListaMultiseleccionTipoMovimiento

                    Else
                        ListaComboTipoMovimientos = Nothing
                        If objRet.Entities.Where(Function(i) i.Categoria = "TIPOMOVIMIENTO").Count > 0 Then
                            ListaComboTipoMovimientos = objRet.Entities.Where(Function(i) i.Categoria = "TIPOMOVIMIENTO").ToList
                        End If
                        ListaComboTipoInversionista = Nothing
                        If objRet.Entities.Where(Function(i) i.Categoria = "CATEGORIACLIENTE").Count > 0 Then
                            ListaComboTipoInversionista = objRet.Entities.Where(Function(i) i.Categoria = "CATEGORIACLIENTE").ToList
                        End If

                    End If

                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Async Function ConsultarCombosGenerico() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Omnibus_Combos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            mdcProxy.Omnibus_Combos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.Omnibus_ConsultarCombosSyncQuery(0, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaComboModulos = Nothing
                    If objRet.Entities.Where(Function(i) i.Categoria = "CARGASARCHIVOS").Count > 0 Then
                        ListaComboModulos = objRet.Entities.Where(Function(i) i.Categoria = "CARGASARCHIVOS").ToList
                    End If
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub GenerarArchivo()
        Try
            ConcatenarFondos()
            ConcatenarTiposMovimientos()

            If IsNothing(intIDGestor) Or intIDGestor = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el gestor.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(FondosConcatenados) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un fondo.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(TipoMovimientoConcatenado) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un tipo de movimiento.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If MostrarSeleccionBanco Then
                If IsNothing(BancoSeleccionado) Or BancoSeleccionado = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el banco.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If
          
            IsBusy = True

            Dim strFiltrosAdicionales As String = String.Empty

            If MostrarSeleccionBanco Then
                strFiltrosAdicionales = String.Format("BANCO*{0}", BancoSeleccionado)
            End If

            If MostrarInversionista = Visibility.Visible Then
                If Not IsNothing(strTipoInversionista) Then
                    If strFiltrosAdicionales <> String.Empty Then
                        strFiltrosAdicionales = strFiltrosAdicionales + "|" + String.Format("INVERSIONISTAS*{0}", strTipoInversionista)
                    Else
                        strFiltrosAdicionales = String.Format("INVERSIONISTAS*{0}", strTipoInversionista)
                    End If
                End If
            End If


            mdcProxy.RetornoInformacionArchivos.Clear()
            mdcProxy.Load(mdcProxy.Omnibus_ExportarArchivoQuery(CInt(intIDGestor), FondosConcatenados, TipoMovimientoConcatenado, strFiltrosAdicionales, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarArchivos, String.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar los registros.", Me.ToString(), "GenerarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarArchivos(ByVal lo As LoadOperation(Of RetornoInformacionArchivo))
        Try
            If lo.HasError = False Then
                If lo.Entities.ToList.Count > 0 Then
                    Dim objRespuesta As RetornoInformacionArchivo = lo.Entities.First

                    If objRespuesta.Exitoso Then
                        A2Utilidades.Mensajes.mostrarMensaje(objRespuesta.strInformacionGenerar, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        If Not String.IsNullOrEmpty(objRespuesta.URLArchivo) Then
                            Program.VisorArchivosWeb_DescargarURL(objRespuesta.URLArchivo)
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(objRespuesta.strInformacionGenerar, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El servicio no retorno respuesta por favor comuniquese con el administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar los archivos.", Me.ToString(), "TerminoGenerarArchivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar los archivos.", Me.ToString(), "TerminoGenerarArchivos", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Métodos publicos del encabezado - REQUERIDOS"

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _ListaGestores As List(Of Omnibus_Configuracion_Generacion)
    Public Property ListaGestores() As List(Of Omnibus_Configuracion_Generacion)
        Get
            Return _ListaGestores
        End Get
        Set(ByVal value As List(Of Omnibus_Configuracion_Generacion))
            _ListaGestores = value
            MyBase.CambioItem("ListaGestores")
        End Set
    End Property

    Private _intIDGestor As Nullable(Of Integer)
    Public Property intIDGestor() As Nullable(Of Integer)
        Get
            Return _intIDGestor
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDGestor = value
            HabilitarConfiguracionGestor()
            MyBase.CambioItem("intIDGestor")
        End Set
    End Property

    Private _MostrarMultiseleccionCompanias As Boolean = False
    Public Property MostrarMultiseleccionCompanias() As Boolean
        Get
            Return _MostrarMultiseleccionCompanias
        End Get
        Set(ByVal value As Boolean)
            _MostrarMultiseleccionCompanias = value
            MyBase.CambioItem("MostrarMultiseleccionCompanias")
        End Set
    End Property

    Private _MostrarSeleccionBanco As Boolean = False
    Public Property MostrarSeleccionBanco() As Boolean
        Get
            Return _MostrarSeleccionBanco
        End Get
        Set(ByVal value As Boolean)
            _MostrarSeleccionBanco = value
            MyBase.CambioItem("MostrarSeleccionBanco")
        End Set
    End Property

    Private _MostrarInversionista As Visibility = Visibility.Collapsed
    Public Property MostrarInversionista() As Visibility
        Get
            Return _MostrarInversionista

        End Get
        Set(value As Visibility)
            _MostrarInversionista = value
            MyBase.CambioItem("MostrarInversionista")
        End Set
    End Property


    Private _MostrarMultiseleccionTipoMovimiento As Boolean = False
    Public Property MostrarMultiseleccionTipoMovimiento() As Boolean
        Get
            Return _MostrarMultiseleccionTipoMovimiento
        End Get
        Set(ByVal value As Boolean)
            _MostrarMultiseleccionTipoMovimiento = value
            MyBase.CambioItem("MostrarMultiseleccionTipoMovimiento")
        End Set
    End Property

    Private _HabilitarSeleccionExportacion As Boolean = False
    Public Property HabilitarSeleccionExportacion() As Boolean
        Get
            Return _HabilitarSeleccionExportacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionExportacion = value
            MyBase.CambioItem("HabilitarSeleccionExportacion")
        End Set
    End Property

    Private _ListaFondosMultiseleccion As List(Of clsOmnibus_SeleccionRegistros)
    Public Property ListaFondosMultiseleccion() As List(Of clsOmnibus_SeleccionRegistros)
        Get
            Return _ListaFondosMultiseleccion
        End Get
        Set(ByVal value As List(Of clsOmnibus_SeleccionRegistros))
            _ListaFondosMultiseleccion = value
            MyBase.CambioItem("ListaFondosMultiseleccion")
        End Set
    End Property

    Private _ListaTipoMovimientoMultiseleccion As List(Of clsOmnibus_SeleccionRegistros)
    Public Property ListaTipoMovimientoMultiseleccion() As List(Of clsOmnibus_SeleccionRegistros)
        Get
            Return _ListaTipoMovimientoMultiseleccion
        End Get
        Set(ByVal value As List(Of clsOmnibus_SeleccionRegistros))
            _ListaTipoMovimientoMultiseleccion = value
            MyBase.CambioItem("ListaTipoMovimientoMultiseleccion")
        End Set
    End Property

    Private _ListaCombosCompleta As List(Of Omnibus_Combos)
    Public Property ListaCombosCompleta() As List(Of Omnibus_Combos)
        Get
            Return _ListaCombosCompleta
        End Get
        Set(ByVal value As List(Of Omnibus_Combos))
            _ListaCombosCompleta = value
            MyBase.CambioItem("ListaCombosCompleta")
        End Set
    End Property

    Private _ListaComboTipoMovimientos As List(Of Omnibus_Combos)
    Public Property ListaComboTipoMovimientos() As List(Of Omnibus_Combos)
        Get
            Return _ListaComboTipoMovimientos
        End Get
        Set(ByVal value As List(Of Omnibus_Combos))
            _ListaComboTipoMovimientos = value
            MyBase.CambioItem("ListaComboTipoMovimientos")
        End Set
    End Property


    Private _ListaComboTipoInversionista As List(Of Omnibus_Combos)
    Public Property ListaComboTipoInversionista() As List(Of Omnibus_Combos)
        Get
            Return _ListaComboTipoInversionista
        End Get
        Set(ByVal value As List(Of Omnibus_Combos))
            _ListaComboTipoInversionista = value
            MyBase.CambioItem("ListaComboTipoInversionista")
        End Set
    End Property

    Private _ListaComboFondos As List(Of Omnibus_Combos)
    Public Property ListaComboFondos() As List(Of Omnibus_Combos)
        Get
            Return _ListaComboFondos
        End Get
        Set(ByVal value As List(Of Omnibus_Combos))
            _ListaComboFondos = value
            MyBase.CambioItem("ListaComboFondos")
        End Set
    End Property

    Private _ListaComboModulos As List(Of Omnibus_Combos)
    Public Property ListaComboModulos() As List(Of Omnibus_Combos)
        Get
            Return _ListaComboModulos
        End Get
        Set(ByVal value As List(Of Omnibus_Combos))
            _ListaComboModulos = value
            MyBase.CambioItem("ListaComboModulos")
        End Set
    End Property

    Private _strCodigoFondo As String
    Public Property strCodigoFondo() As String
        Get
            Return _strCodigoFondo
        End Get
        Set(ByVal value As String)
            _strCodigoFondo = value
            ConcatenarFondos()
            VerificarHabilitarInversionista()
            MyBase.CambioItem("strCodigoFondo")
        End Set
    End Property

    Private _intCodigoFondo As Integer
    Public Property intCodigoFondo() As Integer
        Get
            Return _intCodigoFondo
        End Get
        Set(ByVal value As Integer)
            _intCodigoFondo = value
            MyBase.CambioItem("intCodigoFondo")
        End Set
    End Property

    Private _FondosConcatenados As String
    Public Property FondosConcatenados() As String
        Get
            Return _FondosConcatenados
        End Get
        Set(ByVal value As String)
            _FondosConcatenados = value
            MyBase.CambioItem("FondosConcatenados")
        End Set
    End Property

    Private _strTipoMovimiento As String
    Public Property strTipoMovimiento() As String
        Get
            Return _strTipoMovimiento
        End Get
        Set(ByVal value As String)
            _strTipoMovimiento = value
            VerificarHabilitarBanco()
            ConcatenarTiposMovimientos()
            VerificarHabilitarInversionista()
            MyBase.CambioItem("strTipoMovimiento")
        End Set
    End Property

    Private _strTipoInversionista As String
    Public Property strTipoInversionista() As String
        Get
            Return _strTipoInversionista
        End Get
        Set(ByVal value As String)
            _strTipoInversionista = value
            MyBase.CambioItem("strTipoInversionista")
        End Set
    End Property

    Private _TipoMovimientoConcatenado As String
    Public Property TipoMovimientoConcatenado() As String
        Get
            Return _TipoMovimientoConcatenado
        End Get
        Set(ByVal value As String)
            _TipoMovimientoConcatenado = value
            MyBase.CambioItem("TipoMovimientoConcatenado")
        End Set
    End Property

    Private _strDescripcionBanco As String
    Public Property strDescripcionBanco() As String
        Get
            Return _strDescripcionBanco
        End Get
        Set(ByVal value As String)
            _strDescripcionBanco = value
            MyBase.CambioItem("strDescripcionBanco")
        End Set
    End Property

    Private _BancoSeleccionado As Integer
    Public Property BancoSeleccionado() As Integer
        Get
            Return _BancoSeleccionado
        End Get
        Set(ByVal value As Integer)
            _BancoSeleccionado = value
            MyBase.CambioItem("BancoSeleccionado")
        End Set
    End Property

    Private _SeleccionarTodosFondos As Boolean = False
    Public Property SeleccionarTodosFondos() As Boolean
        Get
            Return _SeleccionarTodosFondos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodosFondos = value
            If Not IsNothing(_ListaFondosMultiseleccion) Then
                Dim objListaComboMultiseleccion As New List(Of clsOmnibus_SeleccionRegistros)
                For Each li In _ListaFondosMultiseleccion
                    objListaComboMultiseleccion.Add(li)
                Next

                For Each li In objListaComboMultiseleccion
                    li.Seleccionado = _SeleccionarTodosFondos
                Next

                ListaFondosMultiseleccion = Nothing
                ListaFondosMultiseleccion = objListaComboMultiseleccion
            End If
            MyBase.CambioItem("SeleccionarTodosFondos")
        End Set
    End Property

    Private _SeleccionarTodosTiposMovimientos As Boolean = False
    Public Property SeleccionarTodosTiposMovimientos() As Boolean
        Get
            Return _SeleccionarTodosTiposMovimientos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodosTiposMovimientos = value
            If Not IsNothing(_ListaTipoMovimientoMultiseleccion) Then
                Dim objListaComboMultiseleccion As New List(Of clsOmnibus_SeleccionRegistros)
                For Each li In _ListaTipoMovimientoMultiseleccion
                    objListaComboMultiseleccion.Add(li)
                Next

                For Each li In objListaComboMultiseleccion
                    li.Seleccionado = _SeleccionarTodosTiposMovimientos
                Next

                ListaTipoMovimientoMultiseleccion = Nothing
                ListaTipoMovimientoMultiseleccion = objListaComboMultiseleccion
            End If
            MyBase.CambioItem("SeleccionarTodosTiposMovimientos")
        End Set
    End Property

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"


#End Region

End Class

Public Class clsOmnibus_SeleccionRegistros
    Implements INotifyPropertyChanged

    Private _Seleccionado As Boolean
    Public Property Seleccionado() As Boolean
        Get
            Return _Seleccionado
        End Get
        Set(ByVal value As Boolean)
            _Seleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Seleccionado"))
        End Set
    End Property

    Private _ID As String
    Public Property ID() As String
        Get
            Return _ID
        End Get
        Set(ByVal value As String)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class