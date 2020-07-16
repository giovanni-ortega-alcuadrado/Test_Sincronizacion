Imports Telerik.Windows.Controls
'Codigo Creado
'Archivo    : EntregaDeCustodiasViewModel.vb
'Por        : Rafael Cordero
'Creado el  : 08/17/2011 04:58:21AM
'Propiedad de Alcuadrado S.A. 2011

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio

Public Class EntregaDeCustodiasViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"
    Public Property cb As New CamposBusquedaCustodi
    Private CustodiPorDefecto As CFPortafolio.Custodia
    Private CustodiAnterior As CFPortafolio.Custodia
    Private DetalleCustodiaPorDefecto As CFPortafolio.DetalleCustodia
    Dim dcProxy As PortafolioDomainContext
    Dim dcProxy1 As PortafolioDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim objProxyUtilidades As UtilidadesCFDomainContext
    Private Const NOMBRE_CONSECUTIVO_ENTREGA_CUSTODIA As String = "ENTCUS"
    Private Const STR_OWNERADVALOR As String = "A"  '"ADVALOR "
    Private Const STRCOMA As String = ","
    Private Const STR_COMILLA_SIMPLE As String = "'"
    Private Const STR_ENSECUENCIA As String = "S"
    Private Const STR_FUERA_DE_SECUENCIA As String = "F"
    Private Const STR_TIPODOC_ENTREGA_CUSTODIA As String = "ET"
    Private Const STR_NOEXISTE_CONSECUTIVO As String = "El Consecutivo de Entregas de Custodias No existe"
    Private lngEntregaConsecActual As Long = 0
    Private intNroRegistrosProcesados As Integer = 0
    Private intTotalRegSel As Integer = 0
    Private lngIDReciboCustodia As Long = 0
    Private lngIDReciboCustodiaAnterior As Long = 0
    Dim ejecutareporte As Boolean
    Dim CiudadInstalacion As String
    Public _mlogBuscarISINFungible As Boolean = True
    Dim STR_ENTREGA_CUS_IMPRIMIR_REPORTE As String = "NO"
#End Region

#Region "Métodos"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New PortafolioDomainContext()
                dcProxy1 = New PortafolioDomainContext()
                objProxy = New UtilidadesDomainContext()
                objProxyUtilidades = New UtilidadesCFDomainContext
            Else
                dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                dcProxy1 = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                objProxyUtilidades = New UtilidadesCFDomainContext(New System.Uri((Program.RutaServicioUtilidadesCF)))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                MensajeOcupado = "Procesando operaciones, por favor espere..."
                objProxyUtilidades.Load(objProxyUtilidades.ConsultarTablaInstalacionQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacion, "FiltroInicial")
                dcProxy.TraerCiudadInstalacion(" ", Program.Usuario, Program.HashConexion, AddressOf terminotraerciudad, "")
                objProxy.Load(objProxy.listaVerificaparametroQuery("", "Custodias", Program.Usuario, Program.HashConexion), AddressOf Terminotraerparametrolista, Nothing) 'SV20150305
                ConsultarParametros()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "InstalacionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub EjecutarOperaciones()
        Try
            ejecutareporte = True
            intTotalRegSel = 0
            intTotalRegSel = Aggregate tot In _ListaCustodiaMostrar Where tot.ObjParaEntregarAlCliente.Equals(True) Into Count(tot.ObjParaEntregarAlCliente)

            If intTotalRegSel > 0 Then
                MensajeOcupado = "Procesando operaciones, por favor espere..."
                IsBusy = True
                objProxy.consultarConsecutivo(NOMBRE_CONSECUTIVO_ENTREGA_CUSTODIA, STR_OWNERADVALOR, Program.Usuario, Program.HashConexion, AddressOf TerminaConsultarConsecutivo, "")
            Else
                ejecutareporte = False
                A2Utilidades.Mensajes.mostrarMensaje("No hay registros seleccionados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            MyBase.CambioItem("ListaCustodiaPaged")
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                 Me.ToString(), "EntregaDeCustodiasViewModel.EjecutarOperaciones", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub

    Public Sub ConsultarCustodiasComitente()
        Try

            If String.IsNullOrEmpty(_strIdComitente) And String.IsNullOrEmpty(_strIsinFungible) Then
                ejecutareporte = False
                A2Utilidades.Mensajes.mostrarMensaje("No hay cliente o ISIN seleccionado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If _FechaCustodias > Now.Date Then
                ejecutareporte = False
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de entrega seleccionada (" + _FechaCustodias + ") no puede ser superior a la actual (" + Now.Date + "), se cambiará. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                FechaCustodias = Now.Date
                Exit Sub
            End If


            MensajeOcupado = "Consultando custodias del cliente o el ISIN, por favor espere..."
            IsBusy = True


            dcProxy.ExistenCustodiasClientePendientesPorAprobar(_strIdComitente, Program.Usuario, Program.HashConexion, AddressOf ExistenCustodiasClientePendientesPorAprobarCompleted, "")
            EstadoSalida = Nothing
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                 Me.ToString(), "EntregaDeCustodiasViewModel.ConsultarCustodiasComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarItemCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.UserState = "ISINFUNGIBLE" Then
                If lo.Entities.ToList.Count > 0 Then
                    _mlogBuscarISINFungible = False
                    ISINFungibleSeleccionada(lo.Entities.First)
                    _mlogBuscarISINFungible = True
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("la isin ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ISINFungibleSeleccionada(ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                strIsinFungible = pobjItem.IdItem
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al el Isin Fungible", _
                                 Me.ToString(), "ISINFungibleSeleccionada", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultarParametros()
        Try
            objProxy.Verificaparametro("ENTREGA_CUS_IMPRIMIR_REPORTE", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ENTREGA_CUS_IMPRIMIR_REPORTE")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "ENTREGA_CUS_IMPRIMIR_REPORTE"
                    STR_ENTREGA_CUS_IMPRIMIR_REPORTE = lo.Value.ToString
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    Private Sub ExistenCustodiasClientePendientesPorAprobarCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Boolean)))
        Try
            If Not obj.HasError Then
                If Not obj.Value Then
                    '?????
                End If
            Else
                Throw New Exception("Validando si el cliente tiene custodias pendientes por aprobar ")
            End If


            If Not dcProxy.ListadoCustodiasEntregas Is Nothing Then
                dcProxy.ListadoCustodiasEntregas.Clear()
            End If

            If Not _ListaCustodia Is Nothing Then
                _ListaCustodia.Clear()
            End If

            If Not _ListaCustodiaMostrar Is Nothing Then
                _ListaCustodiaMostrar.Clear()
            End If

            dcProxy.Load(dcProxy.TraerCustodiasParaEntregaComitenteQuery(_strIdComitente, _strIsinFungible, Program.Usuario, Program.HashConexion), AddressOf TerminaTraerCustodiasComitente, "")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                 Me.ToString(), "EntregaDeCustodiasViewModel.ExistenCustodiasClientePendientesPorAprobarCompleted", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    '''<remarks>    
    '''JCS Marzo 13/2013 Se agrega manejo de error.
    '''</remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Cuando la operacion tiene errores, se agrega manejo de error mediante mensaje.
    ''' Fecha            : Marzo 13/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 13/2013 - Resultado Ok
    ''' </history>
    Private Sub TerminaTraerCustodiasComitente(lo As LoadOperation(Of CFPortafolio.ListadoCustodiasEntrega))
        Try
            If Not lo.HasError Then
                ListaCustodia = dcProxy.ListadoCustodiasEntregas

                For Each li In _ListaCustodia
                    _ListaCustodiaMostrar.Add(New CFPortafolio.ListadoCustodiasEntrega With {.ObjParaEntregarAlCliente = False, _
                                                                                            .cantidad = li.cantidad, _
                                                                                            .ClaseLiquidacion = li.ClaseLiquidacion, _
                                                                                            .CumplimientoTitulo = li.CumplimientoTitulo, _
                                                                                            .curTotalLiq = li.curTotalLiq, _
                                                                                            .DescEstado = li.DescEstado, _
                                                                                            .DiasVencimiento = li.DescEstado, _
                                                                                            .Direccion = li.Direccion, _
                                                                                            .Emision = li.Emision, _
                                                                                            .EstadoActual = li.EstadoActual, _
                                                                                            .FechasPagoRendimientos = li.FechasPagoRendimientos, _
                                                                                            .Fondo = li.Fondo, _
                                                                                            .Fungible = li.Fungible, _
                                                                                            .IdAgenteRetenedor = li.IdAgenteRetenedor, _
                                                                                            .IdCuentaDeceval = li.IdCuentaDeceval, _
                                                                                            .IDCustodio = li.IDCustodio, _
                                                                                            .IDDepositoExtranjero = li.IDDepositoExtranjero, _
                                                                                            .IDEspecie = li.IDEspecie, _
                                                                                            .IDLiquidacion = li.IDLiquidacion, _
                                                                                            .idRecibo = li.idRecibo, _
                                                                                            .IndicadorEconomico = li.IndicadorEconomico, _
                                                                                            .ISIN = li.ISIN, _
                                                                                            .Liquidacion = li.Liquidacion, _
                                                                                            .Modalidad = li.Modalidad, _
                                                                                            .Nombre = li.Nombre, _
                                                                                            .NroDocumento = li.NroDocumento, _
                                                                                            .NroRefFondo = li.NroRefFondo, _
                                                                                            .Nrotitulo = li.Nrotitulo, _
                                                                                            .ObjCancelacion = li.ObjCancelacion, _
                                                                                            .ObjCobroIntDiv = li.ObjCobroIntDiv, _
                                                                                            .ObjRenovReinv = li.ObjRenovReinv, _
                                                                                            .ObjSuscripcion = li.ObjSuscripcion, _
                                                                                            .ObjVenta = li.ObjVenta, _
                                                                                            .Parcial = li.Parcial, _
                                                                                            .PorcRendimiento = li.PorcRendimiento, _
                                                                                            .PuntosIndicador = li.PuntosIndicador, _
                                                                                            .ReciboTR = li.ReciboTR, _
                                                                                            .Reinversion = li.Reinversion, _
                                                                                            .RentaVariable = li.RentaVariable, _
                                                                                            .Retencion = li.Retencion, _
                                                                                            .Secuencia = li.Secuencia, _
                                                                                            .Sellado = li.Sellado, _
                                                                                            .Sellado1 = li.Sellado1, _
                                                                                            .TasaCompraVende = li.TasaCompraVende, _
                                                                                            .TasaDescuento = li.TasaDescuento, _
                                                                                            .TasaInteres = li.TasaInteres, _
                                                                                            .TasaRetencion = li.TasaRetencion, _
                                                                                            .telefono1 = li.telefono1, _
                                                                                            .TipoIdentificacion = li.TipoIdentificacion, _
                                                                                            .TipoLiquidacion = li.TipoLiquidacion, _
                                                                                            .TipoValor = li.TipoValor, _
                                                                                            .TitularCustodio = li.TitularCustodio, _
                                                                                            .ValorRetencion = li.ValorRetencion, _
                                                                                            .Vencimiento = li.Vencimiento, _
                                                                                            .CantidadDevolver = li.cantidad, _
                                                                                            .fechaRecibo = li.fechaRecibo, _
                                                                                            .lngIDComitente = li.lngIDComitente})
                Next

                MyBase.CambioItem("ListaCustodiaMostrar")
                MyBase.CambioItem("ListaCustodiaPaged")
                If ejecutareporte And STR_ENTREGA_CUS_IMPRIMIR_REPORTE = "SI" Then
                    imprimirCustodia()
                    ejecutareporte = False
                End If

            Else
                'JCS Marzo 13/2013 Se agrega manejo de error.
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                     Me.ToString(), "EntregaDeCustodiasViewModel.TerminaTraerCustodiasComitente", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                'FIN JCS Marzo 13/2013 Se agrega manejo de error.
                IsBusy = False
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                 Me.ToString(), "EntregaDeCustodiasViewModel.TerminaTraerCustodiasComitente", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    '''<remarks>
    '''JCS Febrero 27/2013 Se valida si la fecha de entrega es menor a la de recibo. 
    '''JCS Marzo 13/2013 Se agrega manejo de error.
    '''</remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se agrega la validacion por fecha de entrega.
    ''' Fecha            : Febrero 27/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Febrero 27/2013 - Resultado Ok 
    ''' 
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Cuando la operacion tiene errores, se agrega manejo de error mediante mensaje.
    ''' Fecha            : Marzo 13/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 13/2013 - Resultado Ok
    ''' </history>
    Private Async Function TerminaConsultarConsecutivo(ByVal obj As InvokeOperation(Of System.Nullable(Of Integer))) As System.Threading.Tasks.Task
        Dim bolPresentaInconsistencias As Boolean = False
        Dim strMensaje As String = String.Empty
        intNroRegistrosProcesados = 0

        Try
            If Not obj.HasError Then

                If obj.Value.HasValue Then
                    lngEntregaConsecActual = obj.Value


                    For Each li In _ListaCustodiaMostrar.OrderBy(Function(tit) tit.idRecibo) 'Comienza recorrido de los registros del Grid

                        If li.ObjParaEntregarAlCliente Then 'Se valida si el registro actual esta seleccionado.

                            'Se valida si la cantidad a devolver es mayor que la cantidad del titulo.

                            'lngIDReciboCustodiaAnterior = li.idRecibo

                            If li.CantidadDevolver <= 0 Then
                                ejecutareporte = False
                                strMensaje = strMensaje & " *No existe valor a devolver para la custodia [" & li.idRecibo & "] y ID Recibo [" & li.Nrotitulo & "]." & vbCrLf
                                bolPresentaInconsistencias = True
                                Exit For
                            End If

                            'JCS Febrero 27/2013 Se valida si la fecha de entrega es menor a la de recibo
                            If (li.fechaRecibo > FechaCustodias) Then
                                ejecutareporte = False
                                strMensaje = strMensaje & "La fecha de entrega seleccionada [" & FechaCustodias & "] es mayor a la fecha de recibo de la custodia [" & li.fechaRecibo & "]" & vbCrLf
                                bolPresentaInconsistencias = True
                                Exit For
                            End If
                            'FIN JCS Febrero 27/2013 Se valida si la fecha de entrega es menor a la de recibo

                            If IsNothing(li.EstadoSalida) And li.ObjParaEntregarAlCliente Then
                                ejecutareporte = False
                                strMensaje = strMensaje & "Debe seleccionar un estado de salida para la custodia [" & li.idRecibo & "] y secuencia [" & li.Secuencia & "]" & vbCrLf
                                bolPresentaInconsistencias = True
                                Exit For
                            End If

                            If li.CantidadDevolver > li.cantidad Then
                                ejecutareporte = False
                                strMensaje = strMensaje & " *Se ha detectado para la custodia [" & li.idRecibo & "] y ID Recibo [" & li.Nrotitulo & "], El valor a devolver es mayor que la cantidad del título" & vbCrLf
                                bolPresentaInconsistencias = True
                                Exit For
                            End If
                        End If
                    Next

                Else
                    strMensaje = strMensaje & " *" & STR_NOEXISTE_CONSECUTIVO
                    bolPresentaInconsistencias = True
                End If

                If bolPresentaInconsistencias Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                'JCS Marzo 13/2013 Se agrega manejo de error.
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                     Me.ToString(), "EntregaDeCustodiasViewModel.TerminaConsultarConsecutivo", Application.Current.ToString(), Program.Maquina, obj.Error)
                obj.MarkErrorAsHandled()
                'FIN JCS Marzo 13/2013 Se agrega manejo de error.
                IsBusy = False
            End If

            Dim objRet As InvokeOperation(Of System.Nullable(Of Integer))

            If Not bolPresentaInconsistencias Then

                For Each li In _ListaCustodiaMostrar.OrderBy(Function(tit) tit.idRecibo) 'Comienza recorrido de los registros del Grid

                    If li.ObjParaEntregarAlCliente Then 'Se valida si el registro actual esta seleccionado.

                        objRet = Await dcProxy.EntregaCustodias_Procesar(li.idRecibo, li.Secuencia, FechaCustodias, _strNotas, li.EstadoSalida, li.CantidadDevolver, Program.Usuario, Program.HashConexion).AsTask()

                        If Not objRet.HasError Then
                            If obj.Value >= 1 Then

                                li.CantidadDevolver = li.cantidad
                                intNroRegistrosProcesados = intNroRegistrosProcesados + 1

                                If intTotalRegSel = intNroRegistrosProcesados Then
                                    Call ConsultarCustodiasComitente()
                                End If

                            ElseIf obj.Value = -1 Then
                                IsBusy = False
                                ejecutareporte = False
                                A2Utilidades.Mensajes.mostrarMensaje("No se pudo guardar el movimiento de la custodia " & li.idRecibo & "-" & li.Secuencia & " el registro ya existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                IsBusy = False
                                ejecutareporte = False
                                A2Utilidades.Mensajes.mostrarMensaje("No se pudo guardar el movimiento de la custodia " & li.idRecibo & "-" & li.Secuencia, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                                 Me.ToString(), "TerminaConsultarConsecutivo", Application.Current.ToString(), Program.Maquina, obj.Error)
                            obj.MarkErrorAsHandled()
                            IsBusy = False
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método", _
                                 Me.ToString(), "TerminaConsultarConsecutivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Function

    Public Sub imprimirCustodia()
        Dim strParametros As String = String.Empty
        Dim strReporte As String = String.Empty
        Dim strNroVentana As String = String.Empty

        Try
            ' If Not _ListaCustodiaSeleccionada Is Nothing Then
            If Application.Current.Resources.Contains(Program.REPORTE_ENTREGA_CUSTODIAS) = False Or Application.Current.Resources.Contains(Program.REPORTE_ENTREGA_CUSTODIAS_LIN) = False Then
                A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir la orden no está configurado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If Not IsNothing(ListaInstalacion) Then
                Dim li = ListaInstalacion.FirstOrDefault

                If li.TITLineas Then
                    strReporte = Application.Current.Resources(Program.REPORTE_ENTREGA_CUSTODIAS_LIN).ToString.Trim()
                    strParametros = "&plngEntregaInicialM=" & lngEntregaConsecActual & "&plngEntregaFinalM=" & lngEntregaConsecActual & "&pstrTipoDoc=" & STR_TIPODOC_ENTREGA_CUSTODIA & "&pstrUsuarioMaquina=" & Program.Usuario & "&pstrCiudadImpresion=" & CiudadInstalacion & "&plogCopia=" & 0
                    MostrarReporte(strParametros, Me.ToString, strReporte)
                Else
                    strReporte = Application.Current.Resources(Program.REPORTE_ENTREGA_CUSTODIAS).ToString.Trim()
                    strParametros = "&plngEntregaInicialM=" & lngEntregaConsecActual & "&plngEntregaFinalM=" & lngEntregaConsecActual & "&pstrTipoDoc=" & STR_TIPODOC_ENTREGA_CUSTODIA & "&pstrUsuarioMaquina=" & Program.Usuario & "&pstrCiudadImpresion=" & CiudadInstalacion & "&plogCopia=" & 0
                    MostrarReporte(strParametros, Me.ToString, strReporte)
                End If
            End If


            ' End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la impresión del reporte", Me.ToString(), "imprimirReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoTraerInstalacion(ByVal lo As LoadOperation(Of CFUtilidades.Instalacio))
        If Not lo.HasError Then
            IsBusy = False
            ListaInstalacion = objProxyUtilidades.Instalacios
            'If dcProxy2.Instalacios.Count > 0 Then
            '    If lo.UserState = "insert" Then
            '        InstalacioSelected = ListaInstalacion.Last
            '    End If
            'Else
            '    If lo.UserState = "Busqueda" Then
            '        'MessageBox.Show("No se encontro ningún registro")
            '        A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        MyBase.Buscar()
            '        MyBase.CancelarBuscar()
            '    End If
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Instalacion", _
                                             Me.ToString(), "TerminoTraerInstalacion", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub terminotraerciudad(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            IsBusy = False
            CiudadInstalacion = lo.Value
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Instalacion", _
                                             Me.ToString(), "terminotraerciudad", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de parámetros
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks>SV20150305</remarks>
    Private Sub Terminotraerparametrolista(ByVal obj As LoadOperation(Of OYDUtilidades.valoresparametro))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            'Dim lista = obj.Entities.ToList
            For Each li In obj.Entities.ToList
                Select Case li.Parametro
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES"
                        If li.Valor = "SI" Then
                            logVisualizarMasDecimalesCantidad = True
                        Else
                            logVisualizarMasDecimalesCantidad = False
                        End If
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES_CONSULTA"
                        If li.Valor = "SI" Then
                            logVisualizarMasDecimalesCantidadConsulta = True
                        Else
                            logVisualizarMasDecimalesCantidadConsulta = False
                        End If
                End Select
            Next
        End If
    End Sub

    ''' <summary>
    ''' Cuanto recibe plogTodos y pLogLimpiar limpia el campo "Estado salida" en el grid, cuando solo viene plogTodos evalua cual registro ha sido seleccionado 
    ''' y si el campo "Estado salida" no tiene valor le asigna el que tenga el combo "Estado salida"
    ''' </summary>
    ''' <param name="plogTodos"></param>
    ''' <param name="pLogLimpiar"></param>
    Private Sub AsignarEstadoEntrega(ByRef plogTodos As Boolean, ByRef pLogLimpiar As Boolean)
        Try
            If Not IsNothing(ListaCustodiaMostrar) Then
                If Not IsNothing(EstadoSalida) Then
                    Dim li As List(Of ListadoCustodiasEntrega)
                    li = ListaCustodiaMostrar.Where(Function(I) I.ObjParaEntregarAlCliente = True).ToList()

                    If plogTodos And pLogLimpiar Then
                        li = ListaCustodiaMostrar.Where(Function(I) I.ObjParaEntregarAlCliente = False).ToList()
                        For Each obj In li
                            obj.EstadoSalida = Nothing
                        Next
                    Else
                        For Each obj In li
                            If obj.ObjParaEntregarAlCliente Then
                                If IsNothing(obj.EstadoSalida) Then
                                    obj.EstadoSalida = EstadoSalida
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema tratando de asignar el estado de entrega", _
                                            Me.ToString(), "AsignarEstadoEntrega", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _strMensajeOcupado As String
    Public Property MensajeOcupado() As String
        Get
            Return _strMensajeOcupado
        End Get
        Set(ByVal value As String)
            _strMensajeOcupado = value
            MyBase.CambioItem("MensajeOcupado")
        End Set
    End Property

    Private _strIdComitente As String
    Public Property IdComitente() As String
        Get
            Dim ret As String = String.Empty
            If Not String.IsNullOrEmpty(_strIdComitente) Then
                ret = _strIdComitente.Trim()
            End If
            Return ret
        End Get
        Set(ByVal value As String)
            _strIdComitente = value.Trim.PadLeft(17, " ")
            If _strIdComitente.Trim.Equals(String.Empty) Then _strIdComitente = String.Empty
            MyBase.CambioItem("IdComitente")
        End Set
    End Property

    Private _strNotas As String
    Public Property Notas() As String
        Get
            Return _strNotas
        End Get
        Set(ByVal value As String)
            _strNotas = value
            MyBase.CambioItem("Notas")
        End Set
    End Property

    Private _ListaCustodia As EntitySet(Of CFPortafolio.ListadoCustodiasEntrega)
    Public Property ListaCustodia() As EntitySet(Of CFPortafolio.ListadoCustodiasEntrega)
        Get
            Return _ListaCustodia
        End Get
        Set(ByVal value As EntitySet(Of CFPortafolio.ListadoCustodiasEntrega))
            _ListaCustodia = value
            MyBase.CambioItem("ListaCustodia")
        End Set
    End Property

    Private _ListaCustodiaMostrar As New List(Of CFPortafolio.ListadoCustodiasEntrega)
    Public Property ListaCustodiaMostrar() As List(Of CFPortafolio.ListadoCustodiasEntrega)
        Get
            Return _ListaCustodiaMostrar
        End Get
        Set(ByVal value As List(Of CFPortafolio.ListadoCustodiasEntrega))
            _ListaCustodiaMostrar = value
            MyBase.CambioItem("ListaCustodiaMostrar")
            MyBase.CambioItem("ListaCustodiaPaged")
        End Set
    End Property

    Private _ListaCustodiaPaged As PagedCollectionView
    Public Property ListaCustodiaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCustodiaMostrar) Then
                Dim _ListaCustodiaPaged = New PagedCollectionView(_ListaCustodiaMostrar.OrderBy(Function(tit) tit.idRecibo))
                Return _ListaCustodiaPaged
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As PagedCollectionView)
            _ListaCustodiaPaged = value
            MyBase.CambioItem("ListaCustodiaPaged")
        End Set
    End Property

    Private _ListaCustodiaSeleccionada As CFPortafolio.ListadoCustodiasEntrega
    Public Property ListaCustodiaSeleccionada() As CFPortafolio.ListadoCustodiasEntrega
        Get
            Return _ListaCustodiaSeleccionada
        End Get
        Set(ByVal value As CFPortafolio.ListadoCustodiasEntrega)
            _ListaCustodiaSeleccionada = value

            If Not IsNothing(ListaCustodiaSeleccionada) Then
                If ListaCustodiaSeleccionada.ObjParaEntregarAlCliente Then
                    If Not IsNothing(EstadoSalida) And IsNothing(ListaCustodiaSeleccionada.EstadoSalida) Then
                        ListaCustodiaSeleccionada.EstadoSalida = EstadoSalida
                    End If
                Else
                    ListaCustodiaSeleccionada.EstadoSalida = Nothing
                End If
            End If
            MyBase.CambioItem("ListaCustodiaSeleccionada")
        End Set
    End Property

    Private _ListaInstalacion As EntitySet(Of CFUtilidades.Instalacio)
    Public Property ListaInstalacion() As EntitySet(Of CFUtilidades.Instalacio)
        Get
            Return _ListaInstalacion
        End Get
        Set(ByVal value As EntitySet(Of CFUtilidades.Instalacio))
            _ListaInstalacion = value
            MyBase.CambioItem("ListaInstalacion")
        End Set
    End Property

    ''' <summary>
    '''  Propiedad para la fecha de custodias.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 26/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Febrero 26/2013 - Resultado Ok 
    ''' </history>
    Private _FechaCustodias As DateTime = Now.Date
    Public Property FechaCustodias As DateTime
        Get
            Return _FechaCustodias
        End Get
        Set(ByVal value As DateTime)
            _FechaCustodias = value
            MyBase.CambioItem("FechaCustodias")
        End Set
    End Property

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para la edición y cuando está prendido el parámetro CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES
    Private _logVisualizarMasDecimalesCantidad As Boolean = False
    Public Property logVisualizarMasDecimalesCantidad() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidad
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidad = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidad")
        End Set
    End Property

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para el modo de consulta CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES_CONSULTA
    Private _logVisualizarMasDecimalesCantidadConsulta As Boolean = False
    Public Property logVisualizarMasDecimalesCantidadConsulta() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidadConsulta
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidadConsulta = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidadConsulta")
        End Set
    End Property

    Private _strIsinFungible As String
    Public Property strIsinFungible() As String
        Get
            Return _strIsinFungible
        End Get
        Set(ByVal value As String)
            _strIsinFungible = value
            If _strIsinFungible.Trim.Equals(String.Empty) Then _strIsinFungible = String.Empty
            MyBase.CambioItem("strIsinFungible")
        End Set
    End Property

    'Se controla el checkbox "SeleccionarTodos" desde esta propiedad
    Private _SeleccionarTodos As Boolean = False
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value

            If Not IsNothing(_ListaCustodiaMostrar) Then
                If SeleccionarTodos Then
                    For Each led In ListaCustodiaMostrar
                        led.ObjParaEntregarAlCliente = True
                    Next
                    AsignarEstadoEntrega(True, False)

                Else
                    For Each led In ListaCustodiaMostrar
                        led.ObjParaEntregarAlCliente = False
                    Next
                    AsignarEstadoEntrega(True, True)
                End If

            End If

                MyBase.CambioItem("strIsinFungible")
        End Set
    End Property

    Private _EstadoSalida As String
    Public Property EstadoSalida() As String
        Get
            Return _EstadoSalida
        End Get
        Set(ByVal value As String)
            _EstadoSalida = value
            AsignarEstadoEntrega(False, False)
            MyBase.CambioItem("EstadoSalida")
        End Set
    End Property

#End Region

End Class