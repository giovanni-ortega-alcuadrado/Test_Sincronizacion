Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: PendientesTesoreriaViewModel.vb
'Generado el : 07/30/2012 14:53:15
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria

Public Class PendientesTesoreriaViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private WithEvents autorizaciones As Autorizaciones
    Dim dcProxy As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim logConsultarInformacion As Boolean = True
    Dim logNoSeleccionar As Boolean = True
    
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0) 'DEMC20191025

            If Not Program.IsDesignMode() Then
                IsBusy = True
                objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_ComprobantesEgreso", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "Tesoreria_ComprobantesEgreso")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PendientesTesoreriaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerPendientesTesoreria(ByVal lo As LoadOperation(Of PendientesTesoreria))
        Try
            If lo.HasError() Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de PendientesTesoreria", _
                                            Me.ToString(), "TerminoTraerPendientesTesoreri", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            Else
                logNoSeleccionar = False
                AprobarTodos = False
                DesAprobarTodos = False
                logNoSeleccionar = True

                ListaPendientesTesoreria = lo.Entities.ToList

                If dcProxy.PendientesTesorerias.Count < 1 And logConsultarInformacion = True Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    PendientesTesoreriSelected = ListaPendientesTesoreria.FirstOrDefault
                End If
                'JFSB 20170530 Se ajusta variable de control para la xonsulta de registros
                logConsultarInformacion = True
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de PendientesTesoreria", _
                                            Me.ToString(), "TerminoTraerPendientesTesoreri", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerTesoreria(ByVal lo As LoadOperation(Of Tesoreri))
        Try
            If dcProxy.Tesoreris.Count > 0 Then
                IsBusy = True

                ListaTesoreria = dcProxy.Tesoreris
                TesoreriSelected = ListaTesoreria.First
                If Not IsNothing(ListaDetalleTesoreria) Then
                    ListaDetalleTesoreria.Clear()
                End If

                TesoreriSelected.NombreBco = PendientesTesoreriSelected.strNombreBanco

                Select Case PendientesTesoreriSelected.strTipo
                    Case "CE"
                        dcProxy.Load(dcProxy.Traer_DetalleTesoreria_TesoreriQuery(0,
                                                                              "CE",
                                                                              TesoreriSelected.NombreConsecutivo,
                                                                              TesoreriSelected.IDDocumento,
                                                                              TesoreriSelected.EstadoMC, Program.Usuario, Program.HashConexion),
                                                                              AddressOf TerminoTraerDetalleTesoreria,
                                                                              Nothing)
                    Case "N"
                        dcProxy.Load(dcProxy.Traer_DetalleTesoreria_TesoreriQuery(0,
                                                                              "N",
                                                                              TesoreriSelected.NombreConsecutivo,
                                                                              TesoreriSelected.IDDocumento,
                                                                              TesoreriSelected.EstadoMC, Program.Usuario, Program.HashConexion),
                                                                              AddressOf TerminoTraerDetalleTesoreria,
                                                                              Nothing)
                    Case "RC"
                        dcProxy.Load(dcProxy.Traer_DetalleTesoreria_TesoreriQuery(0,
                                                                              "RC",
                                                                              TesoreriSelected.NombreConsecutivo,
                                                                              TesoreriSelected.IDDocumento,
                                                                              TesoreriSelected.EstadoMC, Program.Usuario, Program.HashConexion),
                                                                              AddressOf TerminoTraerDetalleTesoreria,
                                                                              Nothing)
                End Select
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tesoreria", _
                                                             Me.ToString(), "TerminoTraerTesoreri", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerDetalleTesoreria(ByVal lo As LoadOperation(Of DetalleTesoreri))
        Try
            If lo.HasError() Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTesoreria", _
                                                                 Me.ToString(), "TerminoTraerDetalleTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
            Else
                ListaDetalleTesoreria = dcProxy.DetalleTesoreris
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTesoreria", _
                                                             Me.ToString(), "TerminoTraerDetalleTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Public Sub TerminoAprobarDocumentos(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta
                        objListaMensajes.Add(li.strMensaje)
                    Next
                    objViewImportarArchivo.ListaMensajes = objListaMensajes
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Aprobar-Desaprobar documentos"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()

                'JFSB 20170530 Se ajusta variable de control para la xonsulta de registros
                logConsultarInformacion = False
                'JFSB 20170530 Se llama método para consultar los registros
                dcProxy.Load(dcProxy.PendientesTesoreriaFiltrarQuery(ConsecutivoSeleccionado, FormaPagoSeleccionado,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPendientesTesoreria, "")

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos.", Me.ToString(), "TerminoAprobarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos.", Me.ToString(), "TerminoAprobarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim objListaConsecutivoPendientes As New List(Of OYDUtilidades.ItemCombo)
                Dim objListaFormaDePago As New List(Of OYDUtilidades.ItemCombo)

                If Not IsNothing(lo.Entities) Then
                    If lo.Entities.ToList.Where(Function(i) i.Categoria = "ConsecutivoPendientes").Count > 0 Then
                        For Each li In lo.Entities.ToList.Where(Function(i) i.Categoria = "ConsecutivoPendientes")
                            objListaConsecutivoPendientes.Add(li)
                        Next

                    End If

                    If lo.Entities.ToList.Where(Function(i) i.Categoria = "FormaPagoCETodos").Count > 0 Then
                        For Each li In lo.Entities.ToList.Where(Function(i) i.Categoria = "FormaPagoCETodos")
                            objListaFormaDePago.Add(li)
                        Next

                    End If
                End If

                ListaConsecutivoPendientes = objListaConsecutivoPendientes
                ListaFormaPago = objListaFormaDePago

                logConsultarInformacion = False
                ConsecutivoSeleccionado = String.Empty
                logConsultarInformacion = True
                FormaPagoSeleccionado = "TO"

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos", _
                                                 Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos", _
                                             Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaConsecutivoPendientes As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaConsecutivoPendientes() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaConsecutivoPendientes
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaConsecutivoPendientes = value
            MyBase.CambioItem("ListaConsecutivoPendientes")
        End Set
    End Property

    Private _ListaFormaPago As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaFormaPago() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaFormaPago
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaFormaPago = value
            MyBase.CambioItem("ListaFormaPago")
        End Set
    End Property

    Private _ListaPendientesTesoreria As New List(Of PendientesTesoreria)
    Public Property ListaPendientesTesoreria() As List(Of PendientesTesoreria)
        Get
            Return _ListaPendientesTesoreria
        End Get
        Set(ByVal value As List(Of PendientesTesoreria))
            _ListaPendientesTesoreria = value
            MyBase.CambioItem("ListaPendientesTesoreria")
            MyBase.CambioItem("ListaPendientesTesoreriaPaged")
        End Set
    End Property

    Private WithEvents _PendientesTesoreriSelected As PendientesTesoreria
    Public Property PendientesTesoreriSelected() As PendientesTesoreria
        Get
            Return _PendientesTesoreriSelected
        End Get
        Set(ByVal value As PendientesTesoreria)
            _PendientesTesoreriSelected = value
            MyBase.CambioItem("PendientesTesoreriSelected")
        End Set
    End Property

    Public ReadOnly Property ListaPendientesTesoreriaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPendientesTesoreria) Then
                Dim view = New PagedCollectionView(_ListaPendientesTesoreria)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaTesoreria As EntitySet(Of Tesoreri)
    Public Property ListaTesoreria() As EntitySet(Of Tesoreri)
        Get
            Return _ListaTesoreria
        End Get
        Set(ByVal value As EntitySet(Of Tesoreri))
            _ListaTesoreria = value
            If Not IsNothing(value) Then
                If _ListaTesoreria.Count > 0 Then
                    TesoreriSelected = _ListaTesoreria.FirstOrDefault
                Else
                    TesoreriSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTesoreria")
            MyBase.CambioItem("ListaTesoreriaPaged")
        End Set
    End Property

    Private _TesoreriSelected As Tesoreri
    Public Property TesoreriSelected() As Tesoreri
        Get
            Return _TesoreriSelected
        End Get
        Set(ByVal value As Tesoreri)
            _TesoreriSelected = value
            MyBase.CambioItem("TesoreriSelected")
        End Set
    End Property

    Private _ListaDetalleTesoreria As EntitySet(Of DetalleTesoreri)
    Public Property ListaDetalleTesoreria() As EntitySet(Of DetalleTesoreri)
        Get
            Return _ListaDetalleTesoreria
        End Get
        Set(ByVal value As EntitySet(Of DetalleTesoreri))
            _ListaDetalleTesoreria = value
            MyBase.CambioItem("ListaDetalleTesoreria")
            MyBase.CambioItem("DetalleTesoreriaPaged")
        End Set
    End Property

    Private _DetalleTesoreriSelected As DetalleTesoreri
    Public Property DetalleTesoreriSelected() As DetalleTesoreri
        Get
            Return _DetalleTesoreriSelected
        End Get
        Set(ByVal value As DetalleTesoreri)
            If Not value Is Nothing Then
                _DetalleTesoreriSelected = value
                MyBase.CambioItem("DetalleTesoreriSelected")
            End If
        End Set
    End Property

    Private _AprobarTodos As Boolean
    Public Property AprobarTodos() As Boolean
        Get
            Return _AprobarTodos
        End Get
        Set(ByVal value As Boolean)
            _AprobarTodos = value
            If logNoSeleccionar Then
                SeleccionarTodosAprobados(_AprobarTodos)
            End If

            MyBase.CambioItem("AprobarTodos")
        End Set
    End Property

    Private _DesAprobarTodos As Boolean
    Public Property DesAprobarTodos() As Boolean
        Get
            Return _DesAprobarTodos
        End Get
        Set(ByVal value As Boolean)
            _DesAprobarTodos = value
            If logNoSeleccionar Then
                SeleccionarTodosDesaprobados(_DesAprobarTodos)
            End If
            MyBase.CambioItem("DesAprobarTodos")
        End Set
    End Property

    Private _ConsecutivoSeleccionado As String
    Public Property ConsecutivoSeleccionado() As String
        Get
            Return _ConsecutivoSeleccionado
        End Get
        Set(ByVal value As String)
            _ConsecutivoSeleccionado = value
            CargarGrid()
            MyBase.CambioItem("ConsecutivoSeleccionado")
        End Set
    End Property

    Private _FormaPagoSeleccionado As String
    Public Property FormaPagoSeleccionado() As String
        Get
            Return _FormaPagoSeleccionado
        End Get
        Set(ByVal value As String)
            _FormaPagoSeleccionado = value
            CargarGrid()
            MyBase.CambioItem("FormaPagoSeleccionado")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub Filtrar()
        Try
            dcProxy.PendientesTesorerias.Clear()
            IsBusy = True
            dcProxy.Load(dcProxy.PendientesTesoreriaFiltrarQuery(String.Empty, String.Empty,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPendientesTesoreria, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            IsBusy = False
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            IsBusy = False
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Edición del registro", _
                                 Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            IsBusy = False
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Cancelación de la Edición del registro", _
                              Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            IsBusy = False
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Búsqueda del registro", _
                              Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            IsBusy = False
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el borrado del registro", _
                              Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CambiarAForma()
        Try
            If Not IsNothing(dcProxy.Tesoreris) Then
                dcProxy.Tesoreris.Clear()
            End If
            If Not IsNothing(dcProxy.DetalleTesoreris) Then
                dcProxy.DetalleTesoreris.Clear()
            End If
            ListaTesoreria = Nothing
            TesoreriSelected = Nothing
            ListaDetalleTesoreria = Nothing
            DetalleTesoreriSelected = Nothing

            Select Case PendientesTesoreriSelected.strTipo
                Case "CE"
                    dcProxy.Load(dcProxy.TesoreriaConsultarQuery(1,
                                                                 "CE",
                                                                 PendientesTesoreriSelected.strNombreConsecutivo,
                                                                 PendientesTesoreriSelected.lngIDDocumento,
                                                                 Nothing,
                                                                 "PA",
                                                                 Nothing,
                                                                 Nothing, Program.Usuario, Program.HashConexion),
                                                                 AddressOf TerminoTraerTesoreria,
                                                                 "FiltroInicial")
                Case "N"
                    dcProxy.Load(dcProxy.TesoreriaConsultarQuery(1,
                                                                 "N",
                                                                 PendientesTesoreriSelected.strNombreConsecutivo,
                                                                 PendientesTesoreriSelected.lngIDDocumento,
                                                                 Nothing,
                                                                 "PA",
                                                                 Nothing,
                                                                 Nothing, Program.Usuario, Program.HashConexion),
                                                                 AddressOf TerminoTraerTesoreria,
                                                                 "FiltroInicial")
                Case "RC"
                    dcProxy.Load(dcProxy.TesoreriaConsultarQuery(1,
                                                                 "RC",
                                                                 PendientesTesoreriSelected.strNombreConsecutivo,
                                                                 PendientesTesoreriSelected.lngIDDocumento,
                                                                 Nothing,
                                                                 "PA",
                                                                 Nothing,
                                                                 Nothing, Program.Usuario, Program.HashConexion),
                                                                 AddressOf TerminoTraerTesoreria,
                                                                 "FiltroInicial")
            End Select

            MyBase.CambiarAForma()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al visualizar el detalle.", _
                              Me.ToString(), "CambiarAForma", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Aprobando()
        Try
            If Not IsNothing(ListaPendientesTesoreria) Then
                Dim Aprobado = From ld In ListaPendientesTesoreria Where ld.Aprobar = True Or ld.AprobarDes = True
                                        Select ld
                If Aprobado.Count > 0 Then
                    autorizaciones = New Autorizaciones
                    AddHandler autorizaciones.Closed, AddressOf CerroVentana
                    Program.Modal_OwnerMainWindowsPrincipal(autorizaciones)
                    autorizaciones.ShowDialog()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe Aprobar o Desaprobar al menos un registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe Aprobar o Desaprobar al menos un registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la aprobación de los registros", _
                                             Me.ToString(), "Aprobando()", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CerroVentana()
        Try
            If autorizaciones.DialogResult = True Then
                If autorizaciones.autorizapago = True Then
                    IsBusy = True
                    Dim strRegistrosActualizar As String = String.Empty

                    For Each lp In ListaPendientesTesoreria.Where(Function(e) e.Aprobar = True Or e.AprobarDes = True)
                        If String.IsNullOrEmpty(strRegistrosActualizar) Then
                            strRegistrosActualizar = String.Format("{0},{1},{2},{3}",
                                                                   lp.intIDTesoreria,
                                                                   IIf(lp.Aprobar, "1", "0"),
                                                                   lp.strCodigoAprobacionPor,
                                                                   autorizaciones.validausuario.usuario)
                        Else
                            strRegistrosActualizar = String.Format("{0}|{1},{2},{3},{4}",
                                                                   strRegistrosActualizar,
                                                                   lp.intIDTesoreria,
                                                                   IIf(lp.Aprobar, "1", "0"),
                                                                   lp.strCodigoAprobacionPor,
                                                                   autorizaciones.validausuario.usuario)
                        End If
                    Next

                    If Not IsNothing(dcProxy.RespuestaProcesosGenericosConfirmacions) Then
                        dcProxy.RespuestaProcesosGenericosConfirmacions.Clear()
                    End If

                    dcProxy.Load(dcProxy.AprobarDesaprobarDocumentosTesoreriaQuery(strRegistrosActualizar, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoAprobarDocumentos, String.Empty)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir el evento del cierre de la ventana", _
                                            Me.ToString(), "CerroVentana()", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub SeleccionarTodosAprobados(ByVal plogAccion As Boolean)
        Try
            If Not IsNothing(ListaPendientesTesoreria) Then
                For Each ln In ListaPendientesTesoreria
                    ln.Aprobar = plogAccion
                    If plogAccion Then
                        ln.AprobarDes = False
                    End If
                Next
            End If

            If plogAccion Then
                logNoSeleccionar = False
                DesAprobarTodos = False
                logNoSeleccionar = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los registros", _
                                           Me.ToString(), "seleccionartodos()", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodosDesaprobados(ByVal plogAccion As Boolean)
        Try
            If Not IsNothing(ListaPendientesTesoreria) Then
                For Each ln In ListaPendientesTesoreria
                    ln.AprobarDes = plogAccion
                    If plogAccion Then
                        ln.Aprobar = False
                    End If
                Next
            End If

            If plogAccion Then
                logNoSeleccionar = False
                AprobarTodos = False
                logNoSeleccionar = True
            End If
            
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los registros", _
                                           Me.ToString(), "seleccionartodos()", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarGrid()
        Try
            If logConsultarInformacion Then
                IsBusy = True
                dcProxy.PendientesTesorerias.Clear()

                dcProxy.Load(dcProxy.PendientesTesoreriaFiltrarQuery(ConsecutivoSeleccionado, FormaPagoSeleccionado,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPendientesTesoreria, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llamar el servicio para la carga de los datos del grid", _
                               Me.ToString(), "CargarGrid()", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            IsBusy = False
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Creación de un Nuevo Registro.", _
                              Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _PendientesTesoreriSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _PendientesTesoreriSelected.PropertyChanged
        Try
            If e.PropertyName = "Aprobar" Then
                If _PendientesTesoreriSelected.Aprobar Then
                    _PendientesTesoreriSelected.AprobarDes = False
                End If
            ElseIf e.PropertyName = "AprobarDes" Then
                If _PendientesTesoreriSelected.AprobarDes Then
                    _PendientesTesoreriSelected.Aprobar = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_TesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class
