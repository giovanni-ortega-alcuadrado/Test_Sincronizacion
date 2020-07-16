Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFProcesarPortafolios
Imports System.Threading.Tasks

Public Class ProcesoCobroUtilidadesViewModel
    'Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

#Region "Variables"

    ''' <summary>
    ''' Propiedad para realizar el enlace con la capa de datos del DomainContext correspondiente
    ''' </summary>
    Private mdcProxy As ProcesarPortafoliosDomainContext

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Lista de UtilidadesCustodias que se encuentran cargadas en el grid del formulario modal (childWindow)
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Private WithEvents _ListaUtilidadesCustodias As List(Of ProcesarUtilidadesCustodias)
    Public Property ListaUtilidadesCustodias() As List(Of ProcesarUtilidadesCustodias)
        Get
            Return _ListaUtilidadesCustodias
        End Get
        Set(ByVal value As List(Of ProcesarUtilidadesCustodias))
            _ListaUtilidadesCustodias = value
            'Me.CambioItem("ListaUtilidadesCustodias")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaUtilidadesCustodias"))
            If Not IsNothing(_ListaUtilidadesCustodias) And IsNothing(_UtilidadesCustodiasSelected) Then
                UtilidadesCustodiasSelected = _ListaUtilidadesCustodias.FirstOrDefault
            End If
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para determinar la fila seleccionada y poder capturar los eventos de los campos modificados
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Private WithEvents _UtilidadesCustodiasSelected As ProcesarUtilidadesCustodias
    Public Property UtilidadesCustodiasSelected() As ProcesarUtilidadesCustodias
        Get
            Return _UtilidadesCustodiasSelected
        End Get
        Set(ByVal value As ProcesarUtilidadesCustodias)

            If Not IsNothing(_UtilidadesCustodiasSelected) AndAlso _UtilidadesCustodiasSelected.Equals(value) Then
                Exit Property
            End If

            _UtilidadesCustodiasSelected = value
            'Me.CambioItem("UtilidadesCustodiasSelected")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("UtilidadesCustodiasSelected"))

        End Set
    End Property

    Private _IsBusy As Boolean = False
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Constructor del ViewModel para asociar el DomainContext
    ''' </summary>
    Public Sub New()
        Try
            'If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            '    mdcProxy = New ProcesarPortafoliosDomainContext()
            'Else
            '    mdcProxy = New ProcesarPortafoliosDomainContext(New System.Uri(Program.RutaServicioUtilidades))
            '    'mdcProxy = New ProcesarPortafoliosDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            'End If
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
                DirectCast(mdcProxy.DomainClient, WebDomainClient(Of ProcesarPortafoliosDomainContext.IProcesarPortafoliosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 30, 0)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ProcesoCobroUtilidadesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para consultar la información de la ventana modal para cobros de utilidades
    ''' </summary>
    ''' <param name="pdtmFechaValoracion"> Parámetro de tipo Nullable(Of DateTime)</param>
    ''' <param name="strIdEspecie">Parámetro de tipo String</param>
    ''' <param name="lngIDComitente">Parámetro de tipo String</param>
    ''' <returns>Retorna una variable tipo Boolean que indica si hay errores en el proceso</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Descripción  : Método para consultar la información de la ventana modal para cobros de utilidades
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Public Async Function inicializar(ByVal pdtmFechaValoracion As Nullable(Of DateTime), ByVal strIdEspecie As String, ByVal lngIDComitente As String, ByVal strTipoCompania As String, ByVal strEstadosResultados As String) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea sincrónico
                'JAEZ 20160930  se envia el parametro  strTipoCompania
                logResultado = Await ConsultarUtilidadesCustodias(pdtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoCompania, strEstadosResultados)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logResultado

    End Function

#End Region

#Region "Métodos sincrónicos ventana modal cobro utilidades"

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de cobro
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Public Sub CobrarTodasUtilidades(blnIsChequed As Boolean)
        For Each it In ListaUtilidadesCustodias
            it.logCobro = blnIsChequed
            If it.logCobro Then
                it.logAnulado = False
                If it.dblValorCobrado = 0 Then
                    it.dblValorCobrado = it.dblValorCalculado
                End If
            End If
        Next
        'Me.CambioItem("ListaUtilidadesCustodias")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaUtilidadesCustodias"))
    End Sub

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de anular
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Public Sub AnularTodasUtilidades(blnIsChequed As Boolean)
        For Each it In ListaUtilidadesCustodias
            it.logAnulado = blnIsChequed
            If it.logAnulado Then
                it.logCobro = False
            End If
        Next
        'Me.CambioItem("ListaUtilidadesCustodias")
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaUtilidadesCustodias"))
    End Sub

    ''' <summary>
    ''' Método del botón aceptar de la ventana modal para enviar la información digitada a la base de datos
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Public Sub btnAceptar_Click(dtmFechaValoracion As System.Nullable(Of Date), strIdEspecie As String, lngIDComitente As String, strTipoPortafolio As String, strTipoProceso As String, ByRef CerrarVentana As Boolean)

        UtilidadesCustodiasActualizar(dtmFechaValoracion, strIdEspecie, lngIDComitente, strTipoPortafolio, strTipoProceso, CerrarVentana)

    End Sub

    ''' <summary>
    ''' Realiza la consulta de los registros correspondientes a los cobros de utilidades en la ventana modal (childWindow)
    ''' </summary>
    ''' <returns>Retorna una variable booleana que indica si el proceso no genero errores</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>

    Private Async Function ConsultarUtilidadesCustodias(ByVal dtmFechaValoracion As Nullable(Of DateTime), ByVal strIdEspecie As String, ByVal lngIDComitente As String, ByVal strTipoCompania As String, ByVal strEstadosResultados As String) As Task(Of Boolean)

        IsBusy = True

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ProcesarUtilidadesCustodias)

        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            mdcProxy.ProcesarUtilidadesCustodias.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.UtilidadesCustodiasConsultarQuery(pdtmFechaValoracion:=dtmFechaValoracion,
                                                                                    pstrIdEspecie:=strIdEspecie,
                                                                                    plngIDComitente:=lngIDComitente,
                                                                                    pstrTipoCompania:=strTipoCompania,
                                                                                    pstrEstado:=strEstadosResultados,
                                                                                    pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarUtilidadesCustodias", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaUtilidadesCustodias = mdcProxy.ProcesarUtilidadesCustodias.ToList
                End If
            End If

            logResultado = True

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las Utilidades de Custodias.", Me.ToString(), "ConsultarUtilidadesCustodias", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusy = False

        Return logResultado

    End Function

    ''' <summary>
    ''' Método para actualizar los datos correspondientes a los cobros de utilidades, envia al sp de 
    ''' actualizacion una variable tipo TVPUtilidadesCustodias con la información de la ventana modal
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0001, CP0002, CP0003
    ''' Creado por:         Jorge Peña (Alcuadrado S.A.)
    ''' Descripción:        Se modifica la condición del IF y se modifica el mensaje por "El valor a cobrar debe ser diferente de cero."
    ''' Fecha:              Octubre 30/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - Octubre 30/2014 - Resultado Ok 
    ''' </history>
    Public Sub UtilidadesCustodiasActualizar(dtmFechaValoracion As System.Nullable(Of Date), strIdEspecie As String, lngIDComitente As String, strTipoPortafolio As String, strTipoProceso As String, ByRef CerrarVentana As Boolean)
        Dim objRetornovalor As String = String.Empty
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyProcesarPortafolios()
            End If

            Dim xmlCompleto As String = String.Empty
            Dim xmlDetalle As String = String.Empty

            Dim ValidarCobroUtilidad As Boolean = True

            If Not IsNothing(ListaUtilidadesCustodias) Then

                xmlCompleto = "<CobroUtilidades>"

                For Each objeto In (From c In ListaUtilidadesCustodias)
                    If (objeto.logCobro And objeto.dblValorCobrado = 0) Then
                        ValidarCobroUtilidad = False
                        Exit For
                    ElseIf (objeto.logCobro Or objeto.logAnulado) Then
                        xmlDetalle = "<Detalle intID=""" & objeto.intID &
                                     """ dblValorCobrado=""" & objeto.dblValorCobrado &
                                     """ logCobrado=""" & objeto.logCobro &
                                     """ logAnulado=""" & objeto.logAnulado & """ ></Detalle>"
                        xmlCompleto = xmlCompleto & xmlDetalle
                    End If
                Next
                xmlCompleto = xmlCompleto & "</CobroUtilidades>"

            End If

            CerrarVentana = ValidarCobroUtilidad

            If ValidarCobroUtilidad Then
                mdcProxy.UtilidadesCustodiasActualizar(pxmlCobroUtilidades:=xmlCompleto, pstrUsuario:=Program.Usuario, pdtmFechaValoracion:=dtmFechaValoracion, pstrIdEspecie:=strIdEspecie, plngIDComitente:=lngIDComitente, pstrTipoPortafolio:=strTipoPortafolio, pstrTipoProceso:=strTipoProceso, pstrInfoConexion:=Program.HashConexion, callback:=AddressOf TerminoProcesarValoracion, userState:="")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El valor a cobrar debe ser diferente de cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cobro de utilidades.", Me.ToString(), "UtilidadesCustodiasActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta al finalizar el proceso de valoración
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Private Sub TerminoProcesarValoracion(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion", _
                                                 Me.ToString(), "TerminoProcesarValoracion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

            OnFinalizaValoracion(EventArgs.Empty)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion", _
                                                             Me.ToString(), "TerminoProcesarValoracion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Este evento se dispara cuando alguna propiedad de la utilidad cambia
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 03/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Private Sub _UtilidadesCustodiasSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _UtilidadesCustodiasSelected.PropertyChanged
        Try
            Select Case e.PropertyName
                Case "dblValorCobrado"
                    If UtilidadesCustodiasSelected.dblValorCobrado > UtilidadesCustodiasSelected.dblValorCalculado Then

                        'Dim objNuevoEncabezadoSeleccionado As New ProcesarUtilidadesCustodias

                        'objNuevoEncabezadoSeleccionado = UtilidadesCustodiasSelected

                        'objNuevoEncabezadoSeleccionado.dblValorCobrado = 0

                        'UtilidadesCustodiasSelected = _ListaUtilidadesCustodias.LastOrDefault

                        'Dim ListaUtilidadesCustodiasMod As New EntitySet(Of ProcesarUtilidadesCustodias)

                        'For Each li In mdcProxy.ProcesarUtilidadesCustodias
                        '    If li.intID = UtilidadesCustodiasSelected.intID Then
                        '        li.dblValorCalculado = UtilidadesCustodiasSelected.dblValorCalculado
                        '        li.dblValorCobrado = 0
                        '        li.dtmEmision = UtilidadesCustodiasSelected.dtmEmision
                        '        li.dtmFecha = UtilidadesCustodiasSelected.dtmFecha
                        '        li.dtmVencimiento = UtilidadesCustodiasSelected.dtmVencimiento
                        '        li.lngIdRecibo = UtilidadesCustodiasSelected.lngIdRecibo
                        '        li.lngSecuencia = UtilidadesCustodiasSelected.lngSecuencia
                        '        li.logAnulado = UtilidadesCustodiasSelected.logAnulado
                        '        li.logCobro = UtilidadesCustodiasSelected.logCobro
                        '        li.strEstado = UtilidadesCustodiasSelected.strEstado
                        '        li.strIdEspecie = UtilidadesCustodiasSelected.strIdEspecie
                        '    End If
                        'Next

                        'Dim ListaUtilidadesCustodiasMod As New ProcesarUtilidadesCustodias

                        'For Each li In mdcProxy.ProcesarUtilidadesCustodias
                        '    If li.intID = UtilidadesCustodiasSelected.intID Then
                        '        ListaUtilidadesCustodiasMod.dblValorCalculado = UtilidadesCustodiasSelected.dblValorCalculado
                        '        ListaUtilidadesCustodiasMod.dblValorCobrado = 0
                        '        ListaUtilidadesCustodiasMod.dtmEmision = UtilidadesCustodiasSelected.dtmEmision
                        '        ListaUtilidadesCustodiasMod.dtmFecha = UtilidadesCustodiasSelected.dtmFecha
                        '        ListaUtilidadesCustodiasMod.dtmVencimiento = UtilidadesCustodiasSelected.dtmVencimiento
                        '        ListaUtilidadesCustodiasMod.lngIdRecibo = UtilidadesCustodiasSelected.lngIdRecibo
                        '        ListaUtilidadesCustodiasMod.lngSecuencia = UtilidadesCustodiasSelected.lngSecuencia
                        '        ListaUtilidadesCustodiasMod.logAnulado = UtilidadesCustodiasSelected.logAnulado
                        '        ListaUtilidadesCustodiasMod.logCobro = UtilidadesCustodiasSelected.logCobro
                        '        ListaUtilidadesCustodiasMod.strEstado = UtilidadesCustodiasSelected.strEstado
                        '        ListaUtilidadesCustodiasMod.strIdEspecie = UtilidadesCustodiasSelected.strIdEspecie
                        '    End If
                        'Next

                        'ListaUtilidadesCustodias.Remove(UtilidadesCustodiasSelected)

                        'ListaUtilidadesCustodias.Add(ListaUtilidadesCustodiasMod)

                        'Dim test As ProcesarUtilidadesCustodias = CType(sender, ProcesarUtilidadesCustodias)

                        'test.dblValorCobrado = 0

                        'If mdcProxy.SubmitChanges.HasError Then
                        '    MessageBox.Show("Errror de entidades")
                        'End If

                        ' '' ListaUtilidadesCustodias = ListaUtilidadesCustodiasMod
                        'ListaUtilidadesCustodias = (From prueba In ListaUtilidadesCustodiasMod
                        '                            Select prueba).ToList
                        'ListaUtilidadesCustodias = CType(mdcProxy.ProcesarUtilidadesCustodias.FirstOrDefault, EntitySet(of)

                        'ListaUtilidadesCustodias.Clear()

                        '_ListaUtilidadesCustodias = ListaUtilidadesCustodiasMod

                        '_UtilidadesCustodiasSelected.dblValorCobrado = 0
                        'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("UtilidadesCustodiasSelected"))
                        'MessageBox.Show("El valor a cobrar es superior al valor calculado")
                    End If
                Case "logAnulado"
                    If _UtilidadesCustodiasSelected.logAnulado Then
                        UtilidadesCustodiasSelected.logCobro = False
                    End If

                Case "logCobro"
                    If _UtilidadesCustodiasSelected.logCobro Then
                        UtilidadesCustodiasSelected.logAnulado = False
                    End If
            End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el cambio de propiedades del encabezado.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Levanta el evento FinalizaValoracion
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 03/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Protected Overridable Sub OnFinalizaValoracion(e As EventArgs)
        RaiseEvent FinalizaValoracion(Me, e)
    End Sub

    Public Event FinalizaValoracion As EventHandler

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

#End Region

End Class
