Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo
Imports A2.OYD.OYDServer.RIA.Web.OyDTesoreria
Imports A2ComunesImportaciones

Public Class ProcesosTrasladosInternosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"

    Public ViewTrasladosTesoreria_Importacion As ProcesosTrasladosInternosView = Nothing
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext  ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private dcProxyImportaciones As ImportacionesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private mobjDetallePorDefecto As TrasladosTesoreria_Importacion
#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:    Creacion.
    ''' Responsable:    Catalina Dávila (IOSoft S.A.)
    ''' Fecha:          29 de Agosto/2016
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

                dtmFechaProceso = Date.Now

                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificosQuery("TrasladosTesoreria", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _DiccionarioCarga As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCarga() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCarga
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCarga = value
            MyBase.CambioItem("DiccionarioCarga")
        End Set
    End Property

    Private _ConsecutivoSeleccionado As String
    Public Property ConsecutivoSeleccionado() As String
        Get
            Return _ConsecutivoSeleccionado
        End Get
        Set(ByVal value As String)
            _ConsecutivoSeleccionado = value
            MyBase.CambioItem("ConsecutivoSeleccionado")
        End Set
    End Property

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            MyBase.CambioItem("dtmFechaProceso")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
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

    ''' <summary>
    ''' Lista de Deshacer Cierre Portafolios que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaDetalle As List(Of TrasladosTesoreria_Importacion)
    Public Property ListaDetalle() As List(Of TrasladosTesoreria_Importacion)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of TrasladosTesoreria_Importacion))
            _ListaDetallePaginada = Nothing
            _ListaDetalle = value

            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    Private Sub _ListaDetalle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        If e.PropertyName = "ListaDetalle" Then
            If Not IsNothing(ListaDetalle) Then
                dblTotal = ListaDetalle.Sum(Function(i) i.dblValor)
            End If
        End If
    End Sub

    Private _ListaDetallePaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Deshacer Cierre Portafolios para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                If IsNothing(_ListaDetallePaginada) Then
                    Dim view = New PagedCollectionView(_ListaDetalle)
                    _ListaDetallePaginada = view
                    Return view
                Else
                    Return (_ListaDetallePaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _strFormaPago As String
    Public Property strFormaPago() As String
        Get
            Return _strFormaPago
        End Get
        Set(ByVal value As String)
            _strFormaPago = value
            MyBase.CambioItem("strFormaPago")
        End Set
    End Property

    Private _strAlConsecutivo As String = String.Empty
    Public Property strAlConsecutivo() As String
        Get
            Return _strAlConsecutivo
        End Get
        Set(ByVal value As String)
            _strAlConsecutivo = value
            intAlBanco = Nothing
            strNombreAlBanco = String.Empty
            strCuentaAlBanco = String.Empty
            MyBase.CambioItem("strAlConsecutivo")
        End Set
    End Property

    Private _strDelConsecutivo As String = String.Empty
    Public Property strDelConsecutivo() As String
        Get
            Return _strDelConsecutivo
        End Get
        Set(ByVal value As String)
            _strDelConsecutivo = value
            intDelBanco = Nothing
            strNombreDelBanco = String.Empty
            strCuentaDelBanco = String.Empty
            MyBase.CambioItem("strDelConsecutivo")
        End Set
    End Property

    Private _intAlBanco As Integer
    Public Property intAlBanco() As Integer
        Get
            Return _intAlBanco
        End Get
        Set(ByVal value As Integer)
            _intAlBanco = value
            MyBase.CambioItem("intAlBanco")
        End Set
    End Property

    Private _intDelBanco As Integer
    Public Property intDelBanco() As Integer
        Get
            Return _intDelBanco
        End Get
        Set(ByVal value As Integer)
            _intDelBanco = value
            MyBase.CambioItem("intDelBanco")
        End Set
    End Property

    Private _strCuentaAlBanco As String = String.Empty
    Public Property strCuentaAlBanco() As String
        Get
            Return _strCuentaAlBanco
        End Get
        Set(ByVal value As String)
            _strCuentaAlBanco = value
            MyBase.CambioItem("strCuentaAlBanco")
        End Set
    End Property

    Private _strCuentaDelBanco As String = String.Empty
    Public Property strCuentaDelBanco() As String
        Get
            Return _strCuentaDelBanco
        End Get
        Set(ByVal value As String)
            _strCuentaDelBanco = value
            MyBase.CambioItem("strCuentaDelBanco")
        End Set
    End Property

    Private _strNombreAlBanco As String = String.Empty
    Public Property strNombreAlBanco() As String
        Get
            Return _strNombreAlBanco
        End Get
        Set(ByVal value As String)
            _strNombreAlBanco = value
            MyBase.CambioItem("strNombreAlBanco")
        End Set
    End Property

    Private _strNombreDelBanco As String = String.Empty
    Public Property strNombreDelBanco() As String
        Get
            Return _strNombreDelBanco
        End Get
        Set(ByVal value As String)
            _strNombreDelBanco = value
            MyBase.CambioItem("strNombreDelBanco")
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

    Private _strRuta As String
    Public Property strRuta() As String
        Get
            Return _strRuta
        End Get
        Set(ByVal value As String)
            _strRuta = value
            MyBase.CambioItem("strRuta")
        End Set
    End Property

    Private _strExtensionesPermitidas As String = "Archivo CSV|*.csv"
    Public Property strExtensionesPermitidas() As String
        Get
            Return _strExtensionesPermitidas
        End Get
        Set(ByVal value As String)
            _strExtensionesPermitidas = value
            MyBase.CambioItem("strExtensionesPermitidas")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As TrasladosTesoreria_Importacion
    Public Property DetalleSeleccionado() As TrasladosTesoreria_Importacion
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As TrasladosTesoreria_Importacion)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _dblTotal As System.Nullable(Of Double)
    Public Property dblTotal() As System.Nullable(Of Double)
        Get
            Return _dblTotal
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblTotal = value
            MyBase.CambioItem("dblTotal")
        End Set
    End Property

    Private _HabilitarEdicionContenido As Boolean = True
    Public Property HabilitarEdicionContenido() As Boolean
        Get
            Return _HabilitarEdicionContenido
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionContenido = value
            MyBase.CambioItem("HabilitarEdicionContenido")
        End Set
    End Property

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = String.Empty

        Try
            If Not lo.HasError Then
                'Obtiene los valores del UserState
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New List(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New List(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))

                        dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCarga = dicListaCombos

                    If DiccionarioCarga.ContainsKey("PAGODEPARA") Then
                        If DiccionarioCarga("PAGODEPARA").Count > 0 Then
                            strFormaPago = DiccionarioCarga("PAGODEPARA").First.ID.ToString
                        End If
                    End If

                    If DiccionarioCarga.ContainsKey("TIPOCONSECUTIVOEGRESOS") Then
                        If DiccionarioCarga("TIPOCONSECUTIVOEGRESOS").Count > 0 Then
                            strDelConsecutivo = DiccionarioCarga("TIPOCONSECUTIVOEGRESOS").First.ID.ToString
                        End If
                    End If

                    If DiccionarioCarga.ContainsKey("TIPOCONSECUTIVOCAJA") Then
                        If DiccionarioCarga("TIPOCONSECUTIVOCAJA").Count > 0 Then
                            strAlConsecutivo = DiccionarioCarga("TIPOCONSECUTIVOCAJA").First.ID
                        End If
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ActualizarRegistro
    ''' </summary>
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------

            'Valida la forma de pago
            If String.IsNullOrEmpty(strFormaPago) Then
                strMsg = String.Format("{0}{1} + La forma de pago es un campo requerido.", strMsg, vbCrLf)
            End If

            'Valida el consecutivo
            If String.IsNullOrEmpty(strAlConsecutivo) Then
                strMsg = String.Format("{0}{1} + Al consecutivo es un campo requerido.", strMsg, vbCrLf)
            End If

            'Valida el consecutivo
            If String.IsNullOrEmpty(strDelConsecutivo) Then
                strMsg = String.Format("{0}{1} + Del consecutivo es un campo requerido.", strMsg, vbCrLf)
            End If

            'Valida el banco
            If (intAlBanco) = 0 Then
                strMsg = String.Format("{0}{1} + Al banco es un campo requerido.", strMsg, vbCrLf)
            End If

            'Valida el banco
            If (intDelBanco) = 0 Then
                strMsg = String.Format("{0}{1} + Del banco es un campo requerido.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If IsNothing(_ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            ElseIf _ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "validarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

#Region "Métodos públicos del encabezado"

    Public Sub CargarArchivo(pstrModulo As String, pstrNombreCompletoArchivo As String)
        Try
            ViewImportarArchivo.IsBusy = True
            strRuta = pstrNombreCompletoArchivo
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If
            dcProxyImportaciones.Load(dcProxyImportaciones.TrasladosTesoreria_ImportarQuery(pstrNombreCompletoArchivo, "TrasladosTesoreria", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)
        Catch ex As Exception
            ViewImportarArchivo.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al subir el archivo.", _
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                        'objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "RESULTADOS" Then
                                objListaMensajes.Add(li.Mensaje)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        'objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                End If

                Await RegistrosImportados()

                ViewImportarArchivo.IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de generar el archivo. ", Me.ToString(), "TerminoCargarArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de generar el archivo. ", Me.ToString(), "TerminoCargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        ViewImportarArchivo.IsBusy = False
    End Sub

    Private Sub _DetalleSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleSeleccionado.PropertyChanged
        Try
            If e.PropertyName = "dblValor" Then
                If Not IsNothing(ListaDetalle) Then
                    dblTotal = ListaDetalle.Sum(Function(i) i.dblValor)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos públicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' </summary>
    Public Sub IngresarDetalle()
        Try
            Dim objNvoDetalle As New TrasladosTesoreria_Importacion
            Dim objNuevaLista As New List(Of TrasladosTesoreria_Importacion)

            objNvoDetalle.intID = -New Random().Next(0, 1000000)
            objNvoDetalle.lngIDComitente = String.Empty
            objNvoDetalle.strNombre = String.Empty
            objNvoDetalle.dblValor = 0

            If Not IsNothing(ListaDetalle) Then
                objNuevaLista = ListaDetalle
            End If

            objNuevaLista.Add(objNvoDetalle)
            ListaDetalle = objNuevaLista
            DetalleSeleccionado = _ListaDetalle.Last

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de la operación interbancaria", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    Public Sub BorrarDetalle()
        Try

            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).First)
                End If

                Dim objNuevaListaDetalle As New List(Of TrasladosTesoreria_Importacion)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible borrar este detalle o no existe.", "BorrarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle de configuración arbitraje.", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosComitente(ByVal pstrIDComitente As String, ByVal pintIDDetalle As Integer)
        Try
            IsBusy = True
            If Not IsNothing(mdcProxyUtilidad.BuscadorClientes) Then
                mdcProxyUtilidad.BuscadorClientes.Clear()
            End If

            mdcProxyUtilidad.Load(mdcProxyUtilidad.buscarClienteEspecificoQuery(pstrIDComitente, Program.Usuario, "idcomitente", Program.HashConexion), AddressOf TerminoConsultarComitente, pintIDDetalle)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosComitente. ", Me.ToString(), "ConsultarDatosComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarComitente(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente no existe, por favor verifique.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If Not IsNothing(_ListaDetalle) Then
                        For Each li In _ListaDetalle
                            If li.intID = CInt(lo.UserState) Then
                                li.lngIDComitente = String.Empty
                                li.strNombre = String.Empty
                            End If
                        Next
                    End If
                Else
                    If Not IsNothing(_ListaDetalle) Then
                        For Each li In _ListaDetalle
                            If li.intID = CInt(lo.UserState) Then
                                li.lngIDComitente = lo.Entities.First.IdComitente
                                li.strNombre = lo.Entities.First.Nombre
                            End If
                        Next
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitenteDetalle", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta de los combos.", Me.ToString(), "TerminoConsultarComitenteDetalle", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

    Private Async Function RegistrosImportados() As Task
        Try
            Dim objRegistrosImportados As LoadOperation(Of TrasladosTesoreria_Importacion)

            objRegistrosImportados = Await mdcProxy.Load(mdcProxy.TrasladosTesoreria_ConsultarSyncQuery(Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask

            If Not objRegistrosImportados Is Nothing Then
                If objRegistrosImportados.HasError Then
                    If objRegistrosImportados.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar las liquidaciones de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las liquidaciones de compra.", Me.ToString(), "ConsultarLiquidacionesCompra", Program.TituloSistema, Program.Maquina, objRegistrosImportados.Error)
                    End If

                    objRegistrosImportados.MarkErrorAsHandled()
                Else
                    HabilitarEdicionContenido = False
                    ListaDetalle = objRegistrosImportados.Entities.ToList
                End If
            Else
                'ListaDetalle = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros importados.", _
                               Me.ToString(), "RegistrosImportados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Sub Procesar()
        Try
            If ValidarDatos() Then
                If ValidarDetalle() Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Realmente está seguro de realizar el proceso ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarProcesar)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al procesar la información.", _
                               Me.ToString(), "Procesar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ConfirmarProcesar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then


                Dim objRegistrosProcesados As LoadOperation(Of RespuestaProcesosGenericosConfirmacion)
                Dim strRegistrosModificados As String
                Dim strDetalle As String

                strRegistrosModificados = String.Empty

                IsBusy = True


                If Not IsNothing(ListaDetalle) Then
                    For Each li In ListaDetalle
                        If String.IsNullOrEmpty(strRegistrosModificados) Then
                            strDetalle = li.intID & "|" & li.lngIDComitente & "|" & li.dblValor
                        Else
                            strDetalle = "*" & li.intID & "|" & li.lngIDComitente & "|" & li.dblValor
                        End If

                        strRegistrosModificados = strRegistrosModificados & strDetalle
                    Next
                    'JFSB 20180320 Se limpia la entidad para que reciba nuevamente la información
                    mdcProxy.RespuestaProcesosGenericosConfirmacions.clear()

                    objRegistrosProcesados = Await mdcProxy.Load(mdcProxy.TrasladosTesoreria_ProcesarSyncQuery(dtmFechaProceso, strFormaPago, strDelConsecutivo, strAlConsecutivo, intDelBanco, intAlBanco, strCuentaDelBanco, strCuentaAlBanco, strRegistrosModificados, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask

                    If Not objRegistrosProcesados Is Nothing Then
                        If objRegistrosProcesados.HasError Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar las liquidaciones de compra.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)

                            objRegistrosProcesados.MarkErrorAsHandled()
                        Else
                            Dim objListaRespuesta As List(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion)
                            Dim objListaMensajes As New List(Of String)
                            Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                            objListaRespuesta = objRegistrosProcesados.Entities.ToList

                            For Each li In objListaRespuesta
                                objListaMensajes.Add(li.strMensaje)
                            Next

                            objViewImportarArchivo.ListaMensajes = objListaMensajes

                            objViewImportarArchivo.Title = "Traslados tesorería"
                            Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                            objViewImportarArchivo.ShowDialog()
                        End If
                    End If
                End If

                HabilitarEdicionContenido = True
                ListaDetalle = Nothing
                dblTotal = 0
                strRuta = String.Empty

                If DiccionarioCarga.ContainsKey("PAGODEPARA") Then
                    If DiccionarioCarga("PAGODEPARA").Count > 0 Then
                        strFormaPago = DiccionarioCarga("PAGODEPARA").First.ID.ToString
                    End If
                End If

                If DiccionarioCarga.ContainsKey("TIPOCONSECUTIVOEGRESOS") Then
                    If DiccionarioCarga("TIPOCONSECUTIVOEGRESOS").Count > 0 Then
                        strDelConsecutivo = DiccionarioCarga("TIPOCONSECUTIVOEGRESOS").First.ID.ToString
                    End If
                End If

                If DiccionarioCarga.ContainsKey("TIPOCONSECUTIVOCAJA") Then
                    If DiccionarioCarga("TIPOCONSECUTIVOCAJA").Count > 0 Then
                        strAlConsecutivo = DiccionarioCarga("TIPOCONSECUTIVOCAJA").First.ID
                    End If
                End If

                IsBusy = False

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "ConfirmarProcesar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

