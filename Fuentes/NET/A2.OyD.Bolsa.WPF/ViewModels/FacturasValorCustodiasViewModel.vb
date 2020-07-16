Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: FacturasBancaInvViewModel.vb
'Generado el : 02/24/2012 07:45:33
'Propiedad de Alcuadrado S.A. 2010

'******Modificaciones*******

'Se modifica lógica para recibir la lista de pagos por lotes, y se añaden mas propiedades a la lista 
'para identifigar los pagos al momento de generarlos y marcarlos como pagados
'Modificado por: Santiago Alexander Vergara Orrego
'Fecha: Julio 12/2013
'Cambio: SA_CONSULTARPAGOSLOTES

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports System.Text
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports System.Xml.Linq
Imports System.Core


Public Class FacturasValorCustodiasViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As BolsaDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of BolsaDomainContext.IBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)
        Try
            If Not Program.IsDesignMode() Then
                Fecha = Date.Now
                EspecieInicial = "0"
                EspecieFinal = "ZZZZ"
                CodigoOyDInicial = "0"
                CodigoOyDFinal = "99999999999999999"
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "FacturasBancaInvViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoConsultarPagosComisiones(ByVal lo As LoadOperation(Of PagosComisiones))

        If Not lo.HasError Then

            'Modificado por: Santiago Alexander Vergara Orrego
            'Fecha: Julio 12/2013
            'Cambio: SA_CONSULTARPAGOSLOTES
            If dcProxy.PagosComisiones.Count = 0 Then
                If _ListaFacturasValorCustodiasTemporal.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron Registros", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                Else
                    ListaFacturasValorCustodias = (From c In _ListaFacturasValorCustodiasTemporal
                                                   Order By c.Nombre, c.CodigoOyD
                                                   Select c).ToList

                End If
                IsBusy = False
            Else

                _ListaFacturasValorCustodiasTemporal.AddRange(dcProxy.PagosComisiones.ToList)

                _intUltimoId = (From c In _ListaFacturasValorCustodiasTemporal
                                Select c.Id).ToList.Max

                _strProceso = (From c In _ListaFacturasValorCustodiasTemporal
                                Select c).ToList.FirstOrDefault.Proceso

                dcProxy.PagosComisiones.Clear()
                dcProxy.Load(dcProxy.ConsultarPagosComisionesAdmonQuery(_CodigoOyDInicial, _CodigoOyDFinal, _EspecieInicial, _EspecieFinal, _Fondo, _Fecha, _intUltimoId, _strProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPagosComisiones, "")

            End If
            'Fin Cambio: SA_CONSULTARPAGOSLOTES
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Comisiones por administración Valores", _
                                             Me.ToString(), "TerminoConsultarPagosComisiones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoFacturarPagos(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then

            If Not IsNothing(_ListaFacturasValorCustodiasSeleccionadas) Then
                For Each obj In _ListaFacturasValorCustodiasSeleccionadas
                    ListaFacturasValorCustodias.Remove(obj)
                Next
            End If

            _ListaFacturasValorCustodiasTemporal = ListaFacturasValorCustodias
            ListaFacturasValorCustodias = Nothing
            ListaFacturasValorCustodias = _ListaFacturasValorCustodiasTemporal

            marcarDesmarcarTodos(False)

            _ListaFacturasValorCustodiasSeleccionadas = Nothing

            A2Utilidades.Mensajes.mostrarMensaje("Las facturas se generaron correctamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito, "")
            logPuedeEnviar = False

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del resultado de facturar pagos", _
                     Me.ToString(), "TerminoFacturarPagos", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    'Modificado por: Santiago Alexander Vergara Orrego
    'Fecha: Julio 12/2013
    'Cambio: SA_CONSULTARPAGOSLOTES
    Private _strProceso As String
    Private _intUltimoId As Integer
    Private _ListaFacturasValorCustodiasTemporal As New List(Of PagosComisiones)
    'Fin Cambio: SA_CONSULTARPAGOSLOTES
    Private _ListaFacturasValorCustodiasSeleccionadas As List(Of PagosComisiones)

    Private _ListaFacturasValorCustodias As New List(Of PagosComisiones)
    Public Property ListaFacturasValorCustodias() As List(Of PagosComisiones)
        Get
            Return _ListaFacturasValorCustodias
        End Get
        Set(ByVal value As List(Of PagosComisiones))
            _ListaFacturasValorCustodias = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaFacturasValorCustodias"))
        End Set
    End Property

    Private _FacturasValorCustodiasSelected As PagosComisiones
    Public Property FacturasValorCustodiasSelected() As PagosComisiones
        Get
            Return _FacturasValorCustodiasSelected
        End Get
        Set(ByVal value As PagosComisiones)
            _FacturasValorCustodiasSelected = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FacturasValorCustodiasSelected"))
        End Set
    End Property

    Private _marcarTodos As Boolean
    Public Property marcarTodos() As Boolean
        Get
            Return _marcarTodos
        End Get
        Set(ByVal value As Boolean)
            marcarDesmarcarTodos(value)
        End Set
    End Property

    Private _EspecieInicial As String
    Public Property EspecieInicial() As String
        Get
            Return _EspecieInicial
        End Get
        Set(ByVal value As String)
            _EspecieInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspecieInicial"))
        End Set
    End Property

    Private _EspecieFinal As String
    Public Property EspecieFinal() As String
        Get
            Return _EspecieFinal
        End Get
        Set(ByVal value As String)
            _EspecieFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspecieFinal"))
        End Set
    End Property

    Private _CodigoOyDInicial As String
    Public Property CodigoOyDInicial() As String
        Get
            Return _CodigoOyDInicial
        End Get
        Set(ByVal value As String)
            _CodigoOyDInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoOyDInicial"))
        End Set
    End Property

    Private _CodigoOyDFinal As String
    Public Property CodigoOyDFinal() As String
        Get
            Return _CodigoOyDFinal
        End Get
        Set(ByVal value As String)
            _CodigoOyDFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoOyDFinal"))
        End Set
    End Property

    Private _Fecha As DateTime
    Public Property Fecha() As DateTime
        Get
            Return _Fecha
        End Get
        Set(ByVal value As DateTime)
            _Fecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fecha"))
        End Set
    End Property

    Private _Fondo As String
    Public Property Fondo() As String
        Get
            Return _Fondo
        End Get
        Set(ByVal value As String)
            _Fondo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fondo"))
        End Set
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _logPuedeEnviar As Boolean
    Public Property logPuedeEnviar() As Boolean
        Get
            Return _logPuedeEnviar
        End Get
        Set(ByVal value As Boolean)
            _logPuedeEnviar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logPuedeEnviar"))
        End Set
    End Property


#End Region

#Region "Métodos"

    'para marcar o desmarcar todos los registros dependiendo de la seleccion del usuario 
    Public Sub marcarDesmarcarTodos(ByVal logMarcar As Boolean)

        Try
            IsBusy = True

            If Not IsNothing(ListaFacturasValorCustodias) Then
                If ListaFacturasValorCustodias.Count > 0 Then
                    For Each facturaPago In ListaFacturasValorCustodias
                        facturaPago.Marcar = logMarcar
                    Next
                    _marcarTodos = logMarcar
                Else
                    _marcarTodos = False
                End If
            Else
                _marcarTodos = False
            End If

            logPuedeEnviar = _marcarTodos

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("marcarTodos"))

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al marcar o desmarcar todos los registros", _
             Me.ToString(), "marcarDesmarcarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    'Consulta las facturas de Valor en custodias según los parámetros de busqueda seleccionados
    Public Sub ConsultarPagos()

        Try
            If _CodigoOyDInicial <> String.Empty And _CodigoOyDFinal <> String.Empty And _EspecieInicial <> String.Empty And _EspecieFinal <> String.Empty And _Fondo <> String.Empty And Not IsNothing(_Fecha) Then
                If _Fecha.Year >= 1753 Then
                    dcProxy.PagosComisiones.Clear()

                    'Modificado por: Santiago Alexander Vergara Orrego
                    'Fecha: Julio 12/2013
                    'Cambio: SA_CONSULTARPAGOSLOTES
                    ListaFacturasValorCustodias = Nothing
                    _ListaFacturasValorCustodiasTemporal.Clear()
                    _intUltimoId = 0
                    _strProceso = Nothing
                    'Fin Cambio: SA_CONSULTARPAGOSLOTES

                    logPuedeEnviar = False
                    marcarTodos = False
                    dcProxy.Load(dcProxy.ConsultarPagosComisionesAdmonQuery(_CodigoOyDInicial, _CodigoOyDFinal, _EspecieInicial, _EspecieFinal, _Fondo, _Fecha, 0, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPagosComisiones, "")
                    IsBusy = True
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione todos los parámetros para realizar la busqueda", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las comisiones por administración valores", _
             Me.ToString(), "ConsultarPagos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub LimpiarControles()
        Try
            Fecha = Date.Now
            Fondo = Nothing
            EspecieInicial = "0"
            EspecieFinal = "ZZZZ"
            CodigoOyDInicial = "0"
            CodigoOyDFinal = "99999999999999999"
            dcProxy.PagosComisiones.Clear()
            ListaFacturasValorCustodias = Nothing
            _FacturasValorCustodiasSelected = Nothing
            logPuedeEnviar = False
            marcarTodos = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los controles", _
             Me.ToString(), "LimpiarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub FacturarPagos()

        Try
            IsBusy = True
            _ListaFacturasValorCustodiasSeleccionadas = New List(Of PagosComisiones)
            For Each obj In _ListaFacturasValorCustodias
                If obj.Marcar = True Then
                    _ListaFacturasValorCustodiasSeleccionadas.Add(obj)
                End If
            Next

            If Not IsNothing(_ListaFacturasValorCustodiasSeleccionadas) Then

                Dim strXMLCompleto As String

                strXMLCompleto = "<Comisiones>   "

                For Each obj In _ListaFacturasValorCustodiasSeleccionadas

                    Dim strXML = <Datos CodigoOyD=<%= obj.CodigoOyD %>
                                     CuentaDeposito=<%= obj.CuentaDeposito %>
                                     ValorPagado=<%= obj.ValorPagado %>
                                     Especie=<%= obj.Especie %>
                                     ValorCobroComision=<%= obj.ValorCobroComision %>
                                     AutoRetenedor=<%= obj.AutoRetenedor %>
                                     IVA=<%= obj.IVA %>
                                     ValorIVA=<%= obj.ValorIVA %>
                                     Retension=<%= obj.Retension %>
                                     ValorRetension=<%= obj.ValorRetension %>
                                     PorcentajeVariable=<%= obj.PorcentajeVariable %>
                                     ValorFijoComision=<%= obj.ValorFijoComision %>
                                     ValorComision=<%= obj.ValorComision %>
                                     ValorNetoComision=<%= obj.ValorNeto %>>
                                 </Datos>

                    strXMLCompleto = strXMLCompleto & strXML.ToString

                Next

                strXMLCompleto = strXMLCompleto & " </Comisiones>"

                'Modificado por: Santiago Alexander Vergara Orrego
                'Fecha: Septiembre 02/2013
                'Cambio: SA_CONSULTARPAGOSLOTES
                dcProxy.FacturarPagosComisiones(strXMLCompleto, "A", _Fecha, Program.Usuario, Program.HashConexion, AddressOf TerminoFacturarPagos, "")
                'Fin Cambio: SA_CONSULTARPAGOSLOTES
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enviar los registros", _
             Me.ToString(), "EnviarFacturas", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub HabilitarFacturar()
        Try
            If Not IsNothing(ListaFacturasValorCustodias) Then
                Dim CantidadMarcados = (From c In ListaFacturasValorCustodias _
                            Where c.Marcar = True Select c).Count

                If CantidadMarcados > 0 Then
                    logPuedeEnviar = True
                Else
                    logPuedeEnviar = False
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar o deshabilitar el boton de generación de facturas", _
             Me.ToString(), "HabilitarFacturar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class