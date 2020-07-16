Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel

Public Class PagosLibranzasViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Pagos Libranzasperteneciente al proyecto de Cálculos Financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Catalina Dávila (IOsoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 4 de Mayo/2016
    ''' Pruebas CB       : Catalina Dávila - 4 de Mayo/2016 - Resultado Ok 
    ''' </history>

#Region "Constantes"
    Private Const strNombreArchivo As String = "InformePagosLibranzas"
#End Region

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjDetallePorDefecto As ListaDatosExcluir
    Public ViewPagosLibranzas As PagosLibranzasView = Nothing
    Dim objNvoDetalle As New ListaDatosExcluir
    Dim objNuevaLista As New List(Of ListaDatosExcluir)
    Dim logExisteLibranzaExcluir As Boolean = False
    Dim logExisteComitenteExcluir As Boolean = False
    Dim logExisteEmisorExcluir As Boolean = False
    Dim logExistePagadorExcluir As Boolean = False
    Dim logExisteCustodioExcluir As Boolean = False

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        dtmFechaVencimiento = Date.Now()
        intIDLibranza = Nothing
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
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
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _dtmFechaVencimiento As System.Nullable(Of System.DateTime) = Date.Now()
    Public Property dtmFechaVencimiento() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaVencimiento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaVencimiento = value
            MyBase.CambioItem("dtmFechaVencimiento")
        End Set
    End Property

    Private _intIDLibranza As Integer
    Public Property intIDLibranza() As Integer
        Get
            Return _intIDLibranza
        End Get
        Set(ByVal value As Integer)

            _intIDLibranza = value

            MyBase.CambioItem("intIDLibranza")
        End Set
    End Property

    Private _intIDEmisor As Integer
    Public Property intIDEmisor() As Integer
        Get
            Return _intIDEmisor
        End Get
        Set(ByVal value As Integer)

            _intIDEmisor = value

            MyBase.CambioItem("intIDEmisor")
        End Set
    End Property

    Private _strNroDocumentoEmisor As String = String.Empty
    Public Property strNroDocumentoEmisor() As String
        Get
            Return _strNroDocumentoEmisor
        End Get
        Set(ByVal value As String)
            _strNroDocumentoEmisor = value

            MyBase.CambioItem("strNroDocumentoEmisor")
        End Set
    End Property

    Private _strNombreEmisor As String = String.Empty
    Public Property strNombreEmisor() As String
        Get
            Return _strNombreEmisor
        End Get
        Set(ByVal value As String)
            _strNombreEmisor = value
            MyBase.CambioItem("strNombreEmisor")
        End Set
    End Property

    Private _intIDPagador As Integer
    Public Property intIDPagador() As Integer
        Get
            Return _intIDPagador
        End Get
        Set(ByVal value As Integer)

            _intIDPagador = value

            MyBase.CambioItem("intIDPagador")
        End Set
    End Property

    Private _strNroDocumentoPagador As String = String.Empty
    Public Property strNroDocumentoPagador() As String
        Get
            Return _strNroDocumentoPagador
        End Get
        Set(ByVal value As String)
            _strNroDocumentoPagador = value

            MyBase.CambioItem("strNroDocumentoPagador")
        End Set
    End Property

    Private _strNombrePagador As String = String.Empty
    Public Property strNombrePagador() As String
        Get
            Return _strNombrePagador
        End Get
        Set(ByVal value As String)
            _strNombrePagador = value
            MyBase.CambioItem("strNombrePagador")
        End Set
    End Property

    Private _intIDCustodio As Integer
    Public Property intIDCustodio() As Integer
        Get
            Return _intIDCustodio
        End Get
        Set(ByVal value As Integer)

            _intIDCustodio = value

            MyBase.CambioItem("intIDCustodior")
        End Set
    End Property

    Private _strNroDocumentoCustodio As String = String.Empty
    Public Property strNroDocumentoCustodio() As String
        Get
            Return _strNroDocumentoCustodio
        End Get
        Set(ByVal value As String)
            _strNroDocumentoCustodio = value

            MyBase.CambioItem("strNroDocumentoCustodio")
        End Set
    End Property

    Private _strNombreCustodio As String = String.Empty
    Public Property strNombreCustodio() As String
        Get
            Return _strNombreCustodio
        End Get
        Set(ByVal value As String)
            _strNombreCustodio = value
            MyBase.CambioItem("strNombreCustodio")
        End Set
    End Property

    Private _intCantidadMarcados As Integer
    Public Property intCantidadMarcados() As Integer
        Get
            Return _intCantidadMarcados
        End Get
        Set(ByVal value As Integer)

            _intCantidadMarcados = value

            MyBase.CambioItem("intCantidadMarcados")
        End Set
    End Property

    Private _intCantidadExcluidos As Integer
    Public Property intCantidadExcluidos() As Integer
        Get
            Return _intCantidadExcluidos
        End Get
        Set(ByVal value As Integer)

            _intCantidadExcluidos = value

            MyBase.CambioItem("intCantidadExcluidos")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = LTrim(RTrim(value))

            MyBase.CambioItem("lngIDComitente")
        End Set
    End Property

    Private _strNombreComitente As String = String.Empty
    Public Property strNombreComitente() As String
        Get
            Return _strNombreComitente
        End Get
        Set(ByVal value As String)
            _strNombreComitente = value
            MyBase.CambioItem("strNombreComitente")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of ListaDatosExcluir)
    Public Property ListaDetalle() As List(Of ListaDatosExcluir)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of ListaDatosExcluir))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    ''' <summary>
    ''' Pagina la lista detalles. Se presenta en el grid del detalle 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                Dim view = New PagedCollectionView(_ListaDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As ListaDatosExcluir
    Public Property DetalleSeleccionado() As ListaDatosExcluir
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ListaDatosExcluir)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio(ByVal plngIDComitente As String) As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(plngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        lngIDComitente = Nothing
                        strNombreComitente = Nothing
                    End If
                End If
            Else
                lngIDComitente = Nothing
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Async Function ConsultarDatosGenericos(ByVal strCondicionFiltro As String, ByVal strTipoItem As String) As Task
        Try
            Dim objRet As LoadOperation(Of DatosGenericos)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosGenericosSyncQuery(strCondicionFiltro, strTipoItem, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosGenericos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        If strTipoItem = "Emisor" Then
                            strNombreEmisor = objRet.Entities.First.strNombre
                            intIDEmisor = objRet.Entities.First.intID
                        ElseIf strTipoItem = "Pagador" Then
                            strNombrePagador = objRet.Entities.First.strNombre
                            intIDPagador = objRet.Entities.First.intID
                        ElseIf strTipoItem = "Custodio" Then
                            strNombreCustodio = objRet.Entities.First.strNombre
                            intIDCustodio = objRet.Entities.First.intID
                        End If
                    Else
                        If strTipoItem = "Emisor" Then
                            strNroDocumentoEmisor = Nothing
                            strNombreEmisor = Nothing
                            intIDEmisor = Nothing
                        ElseIf strTipoItem = "Pagador" Then
                            strNroDocumentoPagador = Nothing
                            strNombrePagador = Nothing
                            intIDPagador = Nothing
                        ElseIf strTipoItem = "Custodio" Then
                            strNroDocumentoCustodio = Nothing
                            strNombreCustodio = Nothing
                            intIDCustodio = Nothing
                        End If
                    End If
                End If
            Else
                If strTipoItem = "Emisor" Then
                    strNroDocumentoEmisor = Nothing
                    strNombreEmisor = Nothing
                    intIDEmisor = Nothing
                ElseIf strTipoItem = "Pagador" Then
                    strNroDocumentoPagador = Nothing
                    strNombrePagador = Nothing
                    intIDPagador = Nothing
                ElseIf strTipoItem = "Custodio" Then
                    strNroDocumentoCustodio = Nothing
                    strNombreCustodio = Nothing
                    intIDCustodio = Nothing
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosGenericos. ", Me.ToString(), "ConsultarDatosGenericos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Sub Excluir()
        Try

            If IsNothing(ListaDetalle) Then
                ListaDetalle = New List(Of ListaDatosExcluir)
            End If

            logExisteLibranzaExcluir = False
            logExisteComitenteExcluir = False
            logExisteEmisorExcluir = False
            logExistePagadorExcluir = False
            logExisteCustodioExcluir = False

            If Not IsNothing(ListaDetalle) Then
                For Each li In ListaDetalle

                    If li.strCampoExcluir = "LIBRANZA" And li.strValorExcluir = CType(intIDLibranza, String) Then
                        logExisteLibranzaExcluir = True
                    ElseIf li.strCampoExcluir = "COMITENTE" And li.strValorExcluir = lngIDComitente Then
                        logExisteComitenteExcluir = True
                    ElseIf li.strCampoExcluir = "EMISOR" And li.strValorExcluir = strNroDocumentoEmisor Then
                        logExisteEmisorExcluir = True
                    ElseIf li.strCampoExcluir = "PAGADOR" And li.strValorExcluir = strNroDocumentoPagador Then
                        logExistePagadorExcluir = True
                    ElseIf li.strCampoExcluir = "CUSTODIO" And li.strValorExcluir = strNroDocumentoCustodio Then
                        logExisteCustodioExcluir = True
                    End If

                Next
            End If

            If Not logExisteLibranzaExcluir And intIDLibranza > 0 Then

                objNvoDetalle = New ListaDatosExcluir
                objNvoDetalle.intIDDatosExcluir = -New Random().Next(0, 1000000)
                objNvoDetalle.strCampoExcluir = "LIBRANZA"
                objNvoDetalle.strValorExcluir = CStr(intIDLibranza)
                objNvoDetalle.intID = intIDLibranza

                AgregarRegistro()

            End If

            If Not logExisteComitenteExcluir And Not String.IsNullOrEmpty(lngIDComitente) Then

                objNvoDetalle = New ListaDatosExcluir
                objNvoDetalle.intIDDatosExcluir = -New Random().Next(0, 100000)
                objNvoDetalle.strCampoExcluir = "COMITENTE"
                objNvoDetalle.strValorExcluir = lngIDComitente
                objNvoDetalle.intID = CInt(lngIDComitente)

                AgregarRegistro()

            End If

            If Not logExisteEmisorExcluir And Not String.IsNullOrEmpty(strNroDocumentoEmisor) Then

                objNvoDetalle = New ListaDatosExcluir
                objNvoDetalle.intIDDatosExcluir = -New Random().Next(0, 10000)
                objNvoDetalle.strCampoExcluir = "EMISOR"
                objNvoDetalle.strValorExcluir = strNroDocumentoEmisor
                objNvoDetalle.intID = intIDEmisor

                AgregarRegistro()

            End If

            If Not logExistePagadorExcluir And Not String.IsNullOrEmpty(strNroDocumentoPagador) Then

                objNvoDetalle = New ListaDatosExcluir
                objNvoDetalle.intIDDatosExcluir = -New Random().Next(0, 1000)
                objNvoDetalle.strCampoExcluir = "PAGADOR"
                objNvoDetalle.strValorExcluir = strNroDocumentoPagador
                objNvoDetalle.intID = intIDPagador

                AgregarRegistro()

            End If

            If Not logExisteCustodioExcluir And Not String.IsNullOrEmpty(strNroDocumentoCustodio) Then

                objNvoDetalle = New ListaDatosExcluir
                objNvoDetalle.intIDDatosExcluir = -New Random().Next(0, 100)
                objNvoDetalle.strCampoExcluir = "CUSTODIO"
                objNvoDetalle.strValorExcluir = strNroDocumentoCustodio
                objNvoDetalle.intID = intIDCustodio

                AgregarRegistro()

            End If

            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallesPaginada")
            MyBase.CambioItem("DetalleSeleccionado")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle", Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AgregarRegistro()
        Try
            objNuevaLista = ListaDetalle
            objNuevaLista.Add(objNvoDetalle)

            ListaDetalle = objNuevaLista
            DetalleSeleccionado = _ListaDetalle.First
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al agregar registros. ", Me.ToString(), "AgregarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Consultar()
        Try
            ExportarExcel("C")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Consultar. ", Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub MarcarComoPagados()
        Try
            ExportarExcel("P")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón MarcarComoPagados. ", Me.ToString(), "MarcarComoPagados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub ExportarExcel(ByVal strTipoGeneracion As String)
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of PagosLibranzas)
            Dim mdcProxyConsultar As CalculosFinancierosDomainContext
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaVencimiento As String = String.Empty
            Dim strFiltrosExcluirCompleto As String
            Dim strFiltrosExcluir As String = String.Empty

            If Not ValidarDatos() Then Exit Sub

            mdcProxyConsultar = inicializarProxyCalculosFinancieros()


            If (dtmFechaVencimiento.Value.Day < 10) Then
                dia = "0" + dtmFechaVencimiento.Value.Day.ToString
            Else
                dia = dtmFechaVencimiento.Value.Day.ToString
            End If

            If (dtmFechaVencimiento.Value.Month < 10) Then
                mes = "0" + dtmFechaVencimiento.Value.Month.ToString
            Else
                mes = dtmFechaVencimiento.Value.Month.ToString
            End If

            ano = dtmFechaVencimiento.Value.Year.ToString

            strFechaVencimiento = ano + mes + dia

            strFiltrosExcluirCompleto = String.Empty

            If Not IsNothing(ListaDetalle) Then
                For Each objeto In (From c In ListaDetalle)

                    If strFiltrosExcluir = String.Empty Then
                        strFiltrosExcluir = objeto.strCampoExcluir & "=" & objeto.intID
                    Else
                        strFiltrosExcluir = strFiltrosExcluir & "|" & objeto.strCampoExcluir & "=" & objeto.intID
                    End If

                Next
            End If

            objRet = Await mdcProxyConsultar.Load(mdcProxyConsultar.PagosLibranzas_ConsultarSyncQuery(strFiltrosExcluir, strTipoGeneracion, strFechaVencimiento, Program.Usuario, strNombreArchivo, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "InformePagosLibranzas", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                    IsBusy = False
                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objResultado = objRet.Entities.First

                        If objResultado.logExitoso Then
                            intCantidadMarcados = CInt(objResultado.intCantidadMarcados)
                            intCantidadExcluidos = CInt(objResultado.intCantidadExcluidos)

                            Program.VisorArchivosWeb_DescargarURL(objResultado.strRutaArchivo & "?date=" & dtmFechaVencimiento & DateTime.Now.ToString("HH:mm:ss"))
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(objResultado.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                    IsBusy = False
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método ExportarExcel", _
                                                             Me.ToString(), "ExportarExcel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarDetalle()
        Try
            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intIDDatosExcluir = _DetalleSeleccionado.intIDDatosExcluir).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDDatosExcluir = _DetalleSeleccionado.intIDDatosExcluir).First)
                End If

                Dim objNuevaListaDetalle As New List(Of ListaDatosExcluir)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDDatosExcluir = _DetalleSeleccionado.intIDDatosExcluir).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDDatosExcluir = _DetalleSeleccionado.intIDDatosExcluir).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al eliminar un registro del grid.", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            If IsNothing(dtmFechaVencimiento) Then
                strMsg = String.Format("{0}{1} + La fecha de vencimiento es un campo requerido.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then

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

#End Region

    'Public Event PropertyChanged1(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class

Public Class ListaDatosExcluir
    Implements INotifyPropertyChanged

    Private _intIDDatosExcluir As Integer
    Public Property intIDDatosExcluir() As Integer
        Get
            Return _intIDDatosExcluir
        End Get
        Set(ByVal value As Integer)
            _intIDDatosExcluir = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDDatosExcluir"))
        End Set
    End Property

    Private _strCampoExcluir As String
    Public Property strCampoExcluir() As String
        Get
            Return _strCampoExcluir
        End Get
        Set(ByVal value As String)
            _strCampoExcluir = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strCampoExcluir"))
        End Set
    End Property

    Private _strValorExcluir As String
    Public Property strValorExcluir() As String
        Get
            Return _strValorExcluir
        End Get
        Set(ByVal value As String)
            _strValorExcluir = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strValorExcluir"))
        End Set
    End Property

    Private _intID As Integer
    Public Property intID() As Integer
        Get
            Return _intID
        End Get
        Set(ByVal value As Integer)
            _intID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intID"))
        End Set
    End Property

    Private _logExisteDatoExcluir As System.Nullable(Of System.Boolean)
    Public Property logExisteDatoExcluir() As System.Nullable(Of System.Boolean)
        Get
            Return _logExisteDatoExcluir
        End Get
        Set(ByVal value As System.Nullable(Of System.Boolean))
            _logExisteDatoExcluir = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logExisteDatoExcluir"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class





