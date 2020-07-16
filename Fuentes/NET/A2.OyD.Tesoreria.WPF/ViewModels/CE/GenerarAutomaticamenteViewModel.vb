Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class GenerarAutomaticamenteViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2Utilidades.Mensajes

Public Class GenerarAutomaticamenteViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim FechaCierre As DateTime
    Dim NroRegistros As Integer
    Dim Mensaje As String
    Dim NroAccionesGen As Integer
    Dim NroRentaGen As Integer

    Dim intNroGuadar As Integer = 0
    Dim intRegistrosInsetados As Integer = 0
    Dim intNroCE, intSecuencia As Integer
    Dim NroDocumento, NombreCliente, Comitente As String
    Dim ValorTotalRegistrosInsertados As Double

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()
            dcProxy1 = New TesoreriaDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                'System.Threading.Thread.Sleep(4000)
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")


                Dim NewUltimosValores As New OyDTesoreria.UltimosValores

                NewUltimosValores.TipoIdentificacion = "N"
                NewUltimosValores.Documento = Now.Date
                NewUltimosValores.IdBanco = Nothing
                NewUltimosValores.FormaPagoCE = "C"
                NewUltimosValores.ClienteRegistrar = True
                'If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("NombreConsecutivoCE")) Then
                '    NewUltimosValores.NombreConsecutivo = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("NombreConsecutivoCE").FirstOrDefault.ID
                'End If
                UltimosValoresSelected = NewUltimosValores

                'JCM20160216
                If Not IsNothing(objProxy.ItemCombos) Then
                    objProxy.ItemCombos.Clear()
                End If
                objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_ComprobantesEgreso", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, Nothing)


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"

    ''' <summary>
    ''' Método generaración los Comprobantes de Egreso.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub GenerarCE()
        Try
            If IsNothing(_UltimosValoresSelected.Documento) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la fecha del comprobante de egreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If validaFechaCierre("Actualizar") Then
                If Validaciones() Then
                    IsBusy = False
                    dcProxy.Load(dcProxy.ConsultarUltimosValoresQuery("ING", "CE", _UltimosValoresSelected.NombreConsecutivos, _UltimosValoresSelected.CuentaContable, _UltimosValoresSelected.FechaCorte, _UltimosValoresSelected.Recibi, _UltimosValoresSelected.TipoId,
                                                      _UltimosValoresSelected.NumeroId, _UltimosValoresSelected.IdBanco, _UltimosValoresSelected.Banco, _UltimosValoresSelected.CCosto, _UltimosValoresSelected.FormaPago, _UltimosValoresSelected.NroCheque, _UltimosValoresSelected.BancoGirador,
                                                      _UltimosValoresSelected.FechaConsignacion, _UltimosValoresSelected.ClienteRegistrar, _UltimosValoresSelected.Observaciones, _UltimosValoresSelected.FormaPagoCE, _UltimosValoresSelected.TipoIdentificacion, _UltimosValoresSelected.NombreConsecutivo,
                                                      _UltimosValoresSelected.NroDocumento, _UltimosValoresSelected.IDCuentaContable, _UltimosValoresSelected.NumCheque, _UltimosValoresSelected.Documento, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUltimosValores, "Grabar")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            intNroGuadar = 0
            intRegistrosInsetados = 0
            intSecuencia = 1
            ValorTotalRegistrosInsertados = 0
            GuardarCE()
        End If
    End Sub

    ''' <summary>
    ''' Método encargado de Guardar los CE.
    ''' </summary>
    ''' <param name="GraboEncabezado"></param>
    ''' <param name="GraboDetalle"></param>
    ''' <remarks>SLB20130422</remarks>
    Private Sub GuardarCE(Optional GraboEncabezado As Boolean = False, Optional GraboDetalle As Boolean = False)
        Try

            If ListaTesoreriaGA.ElementAt(intNroGuadar).Seleccionado Then

                'Inserta el encabezo del CE.
                If Not GraboEncabezado And intSecuencia = 1 Then

                    If _EspecificacionesCESelected.NroDetalles = "1" Then
                        If Not IsNothing(_ListaTesoreriaGA.ElementAt(intNroGuadar).NroDocumento) Then
                            NroDocumento = _ListaTesoreriaGA.ElementAt(intNroGuadar).NroDocumento
                        Else
                            NroDocumento = 0
                        End If
                    Else
                        NroDocumento = _UltimosValoresSelected.NroDocumento
                    End If

                    If _EspecificacionesCESelected.NroDetalles = "1" Then
                        NombreCliente = _ListaTesoreriaGA.ElementAt(intNroGuadar).Nombre
                    Else
                        NombreCliente = _UltimosValoresSelected.Recibi
                    End If

                    IsBusy = True
                    dcProxy.InsertarCE(_UltimosValoresSelected.NombreConsecutivo, _UltimosValoresSelected.TipoIdentificacion, NroDocumento,
                                       NombreCliente, _UltimosValoresSelected.IdBanco, _UltimosValoresSelected.NumCheque, _UltimosValoresSelected.Documento,
                                       Program.Usuario, _UltimosValoresSelected.FormaPagoCE, Nothing, Nothing, Program.HashConexion, AddressOf TerminoInsertarCE, "Guadar")

                    Exit Sub
                End If

                'Inserta los detalles del CE.
                If Not GraboDetalle Then

                    If _UltimosValoresSelected.ClienteRegistrar Then
                        If Not IsNothing(_ListaTesoreriaGA.ElementAt(intNroGuadar).CodComitente) Then
                            Comitente = _ListaTesoreriaGA.ElementAt(intNroGuadar).CodComitente
                        Else
                            Comitente = _UltimosValoresSelected.NroDocumento
                        End If
                    Else
                        Comitente = Nothing
                    End If

                    IsBusy = True
                    dcProxy.InsertarCEDetalle(_UltimosValoresSelected.NombreConsecutivo, intNroCE, intSecuencia, Comitente, _ListaTesoreriaGA.ElementAt(intNroGuadar).TotalLiquidacion,
                                              _UltimosValoresSelected.IDCuentaContable, _ListaTesoreriaGA.ElementAt(intNroGuadar).Detalle,
                                              _ListaTesoreriaGA.ElementAt(intNroGuadar).NroDocumento, _UltimosValoresSelected.CCosto, Program.Usuario,
                                              Nothing, Nothing, 1, Program.HashConexion, AddressOf TeminoGrabarCEDetalle, "Guardar")
                    ValorTotalRegistrosInsertados = ValorTotalRegistrosInsertados + _ListaTesoreriaGA.ElementAt(intNroGuadar).TotalLiquidacion
                    Exit Sub
                End If

                intSecuencia = intSecuencia + 1
            End If

            'Recorrer el grid de Pendientes de Tesorería hasta terminar.
            If intNroGuadar < (ListaTesoreriaGA.Count - 1) Then
                intNroGuadar = intNroGuadar + 1
                If intSecuencia > EspecificacionesCESelected.NroDetalles Then 'SLB20130509 Manejo para saber cuando se tiene que generar un nuevo encabezado, dependiendo del Nro de detalles por encabezado (Digitado en pantalla)
                    intSecuencia = 1
                End If
                GuardarCE()
            Else
                IsBusy = False
                GeneroExitosamente()
                'A2Utilidades.Mensajes.mostrarMensaje("Comprobante de Egreso insertados: " & CStr(intRegistrosInsetados) & vbCr & "Valor de: $" & CStr(ValorTotalRegistrosInsertados), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                'ConsultarPendientesTesoreraia("Grabar")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "GuardarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que consulta los documentos de Tesorería que estan pendientes por Generar .
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub ConsultarPendientesTesoreraia(Optional ByVal strAccion As String = " ")
        Try
            IsBusy = True
            dcProxy.TesoreriaGAs.Clear()
            dcProxy.Load(dcProxy.ConsultarPendientesTesoreriaQuery(_ParametrosConsultaSelected.Cumplimiento, _ParametrosConsultaSelected.FormadePago,
                                                                   _ParametrosConsultaSelected.Receptores, _ParametrosConsultaSelected.Modulo, _ParametrosConsultaSelected.TipoCliente,
                                                                   _ParametrosConsultaSelected.Sistema, _ParametrosConsultaSelected.TipoOperacion, _ParametrosConsultaSelected.Clase,
                                                                   _ParametrosConsultaSelected.Tipo, "CE", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPendientesTesoreria, strAccion)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConsultarUltimosValores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que muestra el resultado de la generación automatica del CE.
    ''' </summary>
    ''' <remarks>SLB20130624</remarks>
    Private Sub GeneroExitosamente()
        A2Utilidades.Mensajes.mostrarMensaje("Comprobante de Egreso insertados: " & CStr(intRegistrosInsetados) & vbCr & "Valor de: " & FormatCurrency(CStr(ValorTotalRegistrosInsertados), 2, , , TriState.True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
        Dim lstReceptoresSel As New List(Of Object)

        For Each objLista In ListaTesoreriaGA
            If objLista.Seleccionado Then
                lstReceptoresSel.Add(objLista)
            End If
        Next

        For Each objLista In lstReceptoresSel
            ListaTesoreriaGA.Remove(objLista)
        Next

    End Sub

    ''' <summary>
    ''' Método que consulta los utlimos valores con los cuales se generó CE Automaticos.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub ConsultarUltimosValores()
        Try
            IsBusy = True
            dcProxy.UltimosValores.Clear()
            dcProxy.Load(dcProxy.ConsultarUltimosValoresQuery("CON", "CE", Nothing, Nothing, Nothing, Nothing, Nothing,
                                                                  Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                                                                  Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                                                                  Nothing, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUltimosValores, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConsultarUltimosValores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método de las validaciones para generar los Comprobantes de Egreso.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Function Validaciones() As Boolean
        Validaciones = True

        If _UltimosValoresSelected.NombreConsecutivo = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un consecutivo de tesorería.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_UltimosValoresSelected.IDCuentaContable) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un cuenta contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_UltimosValoresSelected.Recibi) Then
            If _EspecificacionesCESelected.NroDetalles <> 1 Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo a Favor de no puede estar vacío.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If
        End If

        If String.IsNullOrEmpty(_UltimosValoresSelected.TipoIdentificacion) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_UltimosValoresSelected.NroDocumento) Then
            If _EspecificacionesCESelected.NroDetalles <> 1 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe digitar el número de documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If
        End If

        If IsNothing(_UltimosValoresSelected.FormaPagoCE) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la forma de pago.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_UltimosValoresSelected.FormaPagoCE) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la forma de pago.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If IsNothing(_UltimosValoresSelected.IdBanco) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_UltimosValoresSelected.Banco) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If _EspecificacionesCESelected.ChequeraAutomatica Then
            _UltimosValoresSelected.NumCheque = 0
            _EspecificacionesCESelected.HabilitarCheque = False
        Else
            If _UltimosValoresSelected.FormaPagoCE = "C" Then
                If IsNothing(_UltimosValoresSelected.NumCheque) Or _UltimosValoresSelected.NumCheque = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número del cheque para poder grabar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Validaciones = False
                    Return Validaciones
                End If
            End If
        End If


        If _EspecificacionesCESelected.NroDetalles <= 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe digitar un número de detalles para los comprobantes de egreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        Dim Seleccionado As Boolean = False
        If Not IsNothing(_ListaTesoreriaGA) Then
            If ListaTesoreriaGA.Count > 0 Then
                For Each objLista In ListaTesoreriaGA.Where(Function(i) i.Seleccionado = True)
                    If objLista.TotalLiquidacion = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No existe valor operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = False
                        Return Validaciones
                    End If

                    If objLista.Detalle = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("No existe detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = False
                        Return Validaciones
                    End If

                    If objLista.Seleccionado Then
                        Seleccionado = True
                    End If
                Next
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No existen detalles para hacer los comprobantes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If
        Else
            A2Utilidades.Mensajes.mostrarMensaje("No existen detalles para hacer los comprobantes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If Not Seleccionado Then
            A2Utilidades.Mensajes.mostrarMensaje("No existen detalles seleccionados para hacer los comprobantes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If
    End Function

    ''' <summary>
    ''' Valida la Fecha de Cierre del Sistema
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>SLB20121005</remarks>
    Private Function validaFechaCierre(ByVal pstrAccion As String) As Boolean
        validaFechaCierre = True
        If Format(CType(_UltimosValoresSelected.Documento, Date).Date, "yyyy/MM/dd") <= Format(FechaCierre, "yyyy/MM/dd") Then 'Intentan registrar un documento con fecha inferior a la fecha de cierre registrada en tblInstalacion
            If Format(FechaCierre, "yyyy/mm/dd") <> "1900/01/01" Then
                Select Case pstrAccion
                    Case "Actualizar"
                        A2Utilidades.Mensajes.mostrarMensaje("La operación con fecha (" & CType(_UltimosValoresSelected.Documento, Date).Date.ToLongDateString & ") no puede ser ingresada o modificada porque su fecha es inferior a la fecha de cierre (" & FechaCierre.Date.ToLongDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End Select
                validaFechaCierre = False
            End If
        End If
        Return validaFechaCierre
    End Function

    ''' <summary>
    ''' Método para seleccionar el total de los registros de los pendientes de tesorería.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub SeleccionarTodos()
        If Not IsNothing(_ListaTesoreriaGA) Then
            For Each led In ListaTesoreriaGA
                led.Seleccionado = True
            Next
        End If
    End Sub

    ''' <summary>
    ''' Método para des seleccionar el total de los registros de los pendientes de tesorería.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub DesseleccionarTodos()
        If Not IsNothing(_ListaTesoreriaGA) Then
            For Each led In ListaTesoreriaGA
                led.Seleccionado = False
            Next
        End If
    End Sub

    ''' <summary>
    ''' Método para totalizar el total de los registros de los pendientes de tesorería.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub Totalizar()
        If Not IsNothing(_ListaTesoreriaGA) Then
            TotalGeneral = 0
            TotalSeleccionado = 0
            For Each led In ListaTesoreriaGA
                TotalGeneral = TotalGeneral + led.TotalLiquidacion
                If led.Seleccionado = True Then
                    TotalSeleccionado = TotalSeleccionado + led.TotalLiquidacion
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Método que modica los parametros de consulta dependiendo del módulo seleccionado.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub ModificacionParametros()
        Try
            Select Case _ParametrosConsultaSelected.Modulo
                Case "0" 'OTC
                    ParametrosConsultaSelected.HabilitarSistema = True
                    ParametrosConsultaSelected.HabilitarFormaPago = False
                    ParametrosConsultaSelected.HabilitarFormaPago = False
                    ParametrosConsultaSelected.HabilitarTipoCliente = True
                    HabilitarNroDetalles = True
                    ParametrosConsultaSelected.HabilitarReceptores = True
                Case "1" 'Bolsa
                    ParametrosConsultaSelected.HabilitarSistema = False
                    ParametrosConsultaSelected.HabilitarFormaPago = True
                    ParametrosConsultaSelected.HabilitarFormaPago = True
                    ParametrosConsultaSelected.HabilitarTipoCliente = True
                    HabilitarNroDetalles = True
                    ParametrosConsultaSelected.HabilitarReceptores = True
                Case "2" 'Otros Firmas
                    ParametrosConsultaSelected.HabilitarSistema = False
                    ParametrosConsultaSelected.HabilitarFormaPago = True
                    ParametrosConsultaSelected.HabilitarTipoCliente = False
                    HabilitarNroDetalles = True
                    ParametrosConsultaSelected.HabilitarReceptores = True
                    ParametrosConsultaSelected.TipoCliente = "2"
                Case "3" 'Internacionales
                    ParametrosConsultaSelected.HabilitarSistema = True
                    ParametrosConsultaSelected.HabilitarFormaPago = False
                    ParametrosConsultaSelected.HabilitarTipoCliente = True
                    HabilitarNroDetalles = False
                    ParametrosConsultaSelected.HabilitarReceptores = True
                    _EspecificacionesCESelected.NroDetalles = 1
                Case "4" 'Divisas
                    ParametrosConsultaSelected.HabilitarSistema = False
                    ParametrosConsultaSelected.HabilitarFormaPago = False
                    ParametrosConsultaSelected.HabilitarTipoCliente = False
                    HabilitarNroDetalles = False
                    ParametrosConsultaSelected.HabilitarReceptores = False
                    _EspecificacionesCESelected.NroDetalles = 1
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del banco seleccionado en el encabezado y en el detalle de Tesoreria
    ''' </summary>
    ''' <param name="plngIdBanco">Codigo del banco el cual se va a realizar la búsqueda</param>
    ''' <remarks>SLB20130312</remarks>
    Friend Sub buscarBancos(Optional ByVal plngIdBanco As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "cuentasbancarias")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar el centro de costos seleccionado.
    ''' </summary>
    ''' <param name="pstrCentroCostos"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>SLB20130312</remarks>
    Friend Sub buscarCentroCostos(Optional ByVal pstrCentroCostos As String = "", Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("CentrosCosto", pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "CentrosCosto")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    'JMC20160216
    Public Sub LimpiarBanco()
        _UltimosValoresSelected.IdBanco = Nothing
        _UltimosValoresSelected.Banco = String.Empty
        EspecificacionesCESelected.HabilitarCheque = True
        strCompaniaBanco = String.Empty
    End Sub


#End Region

#Region "MetodosAsincronicos"

    ''' <summary>
    ''' Método encargado de recibir la lista de los ultimos valores sugeridos y tambien cuando se esta actualizando.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130418</remarks>
    Private Sub TerminoTraerUltimosValores(ByVal lo As LoadOperation(Of OyDTesoreria.UltimosValores))
        IsBusy = False
        If Not lo.HasError Then
            If lo.UserState.ToString = "Grabar" Then
                'C1.Silverlight.C1MessageBox.Show("Desea generar los Comprobantes de Egreso", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPregunta)
                mostrarMensajePregunta("Desea generar los Comprobantes de Egreso",
                                       Program.TituloSistema,
                                       "ULTIMOSVALORES",
                                       AddressOf TerminoPregunta, True, "¿Generar?")
            Else
                If dcProxy.UltimosValores.Count > 0 Then
                    UltimosValoresSelected = dcProxy.UltimosValores.FirstOrDefault
                    buscarBancos(_UltimosValoresSelected.IdBanco)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se dispone de datos de ultimos valores", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Ultimos Valores Sugeridos",
                     Me.ToString(), "TerminoTraerUltimosValores", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método encargado de recibir la lista de las liquidaciones pendientes de Tesorería.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130418</remarks>
    Private Sub TerminoConsultarPendientesTesoreria(ByVal lo As LoadOperation(Of OyDTesoreria.TesoreriaGA))
        IsBusy = False
        If Not lo.HasError Then
            If dcProxy.TesoreriaGAs.Count > 0 Then
                ListaTesoreriaGA = dcProxy.TesoreriaGAs
            Else
                If Not lo.UserState = "Grabar" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No hay operaciones para crear Comprobantes de Egreso con los parámetros especificados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los pendientes de tesorería.",
                     Me.ToString(), "TerminoConsultarPendientesTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        IsBusy = False
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            FechaCierre = obj.Value
        End If
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta de la inserción del encabezado del CE
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130422</remarks>
    Private Sub TerminoInsertarCE(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            intNroCE = lo.Value
            intRegistrosInsetados = intRegistrosInsetados + 1
            If _ParametrosConsultaSelected.Modulo = "3" Then 'Se procesa si es yankees
                'Javier S. Enero de 2008 Actualizamos la tabla de tblliquidacionesTesoreria para controlar en yankees que no muestre los registros que ya fueron generados
                IsBusy = True
                dcProxy.InsertarLiquidacionesTesoreria(_ListaTesoreriaGA.ElementAt(intNroGuadar).LiquidacionId, _ListaTesoreriaGA.ElementAt(intNroGuadar).SecuenciaF,
                                                       intNroCE, _UltimosValoresSelected.NombreConsecutivo, "CE", Program.Usuario, Program.HashConexion, AddressOf TeminoGrabarYankees, "")
            Else
                GuardarCE(True)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el encabezado del CE.",
                     Me.ToString(), "TerminoInsertarCE", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta de la inserción del detalle del CE
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130422</remarks>
    Private Sub TeminoGrabarCEDetalle(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            GuardarCE(True, True)
        Else
            'SLB20130422 En caso de fallar de deben borrar los encabezados y detalles previamente insertados.
            dcProxy.BorrarDocumentoTesoreria(intNroCE, _UltimosValoresSelected.NombreConsecutivo, "CE", Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarTesoreria, "borrar")
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el detalle del CE.",
                     Me.ToString(), "TeminoGrabarCEDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta de la inserción de Yankees
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130422</remarks>
    Private Sub TeminoGrabarYankees(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            GuardarCE(True)
        Else
            'SLB20130422 En caso de fallar de deben borrar los encabezados y detalles previamente insertados.
            dcProxy.BorrarDocumentoTesoreria(intNroCE, _UltimosValoresSelected.NombreConsecutivo, "CE", Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarTesoreria, "borrar")
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar en Yankees.",
                     Me.ToString(), "TeminoGrabarYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta del borrado de los encabezados y detalles previamente insertados.
    ''' </summary>
    ''' <remarks>SLB20130422</remarks>
    Private Sub TerminoBorrarTesoreria(ByVal lo As InvokeOperation(Of Integer))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar los documentos de tesorería insertados.",
                     Me.ToString(), "TerminoBorrarTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta del combo para los consecutivos, solo se muestran los consecutivos por usuario.
    ''' </summary>
    ''' <remarks> JCM20160216</remarks>

    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If objProxy.ItemCombos.Count > 0 Then
                    listConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = "NombreConsecutivoCE").ToList
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Método recibe la respuesta si el banco existe o no
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>20130201</remarks>
    Private Sub TerminoTraerCuentasBancarias(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "cuentasbancarias"
                        If lo.Entities.ToList.Count > 0 Then
                            '------------------------------------------------------------------------------------------------------------------------------------------------
                            'Validar si la compañia del consecutivo es la misma compañia del banco
                            'ID caso de prueba:  CP001
                            '------------------------------------------------------------------------------------------------------------------------------------------------
                            'JCM20160216
                            If Not IsNothing(_UltimosValoresSelected.NombreConsecutivo) Then
                                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _UltimosValoresSelected.NombreConsecutivo Select c).FirstOrDefault.Retorno

                                If lo.Entities.First.InfoAdicional02.Equals("1") Then
                                    If lo.Entities.First.InfoAdicional03.Equals(strCompaniaConsecutivo) Then
                                        _UltimosValoresSelected.IdBanco = lo.Entities.First.IdItem
                                        _UltimosValoresSelected.Banco = lo.Entities.First.Nombre
                                        _EspecificacionesCESelected.ChequeraAutomatica = lo.Entities.First.Estado
                                        If _EspecificacionesCESelected.ChequeraAutomatica Then
                                            EspecificacionesCESelected.HabilitarCheque = False
                                            _UltimosValoresSelected.NumCheque = 0
                                        Else
                                            EspecificacionesCESelected.HabilitarCheque = True
                                        End If
                                    Else
                                        '------------------------------------------------------------------------------------------------------------------------------------------------
                                        'L acompañia del banco no coincide con la compañia del consecutivo
                                        'ID caso de prueba:  CP002
                                        '------------------------------------------------------------------------------------------------------------------------------------------------

                                        A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra asociado a una compañia diferente a la del consecutivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        LimpiarBanco()
                                    End If
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    LimpiarBanco()
                                End If
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            LimpiarBanco()
                        End If
                    Case "CentrosCosto"
                        If lo.Entities.ToList.Count > 0 Then
                            _UltimosValoresSelected.CCosto = lo.Entities.First.IdItem
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El centro de costos ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _UltimosValoresSelected.CCosto = Nothing
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Propiedades"

    Private WithEvents _UltimosValoresSelected As OyDTesoreria.UltimosValores
    Public Property UltimosValoresSelected As OyDTesoreria.UltimosValores
        Get
            Return _UltimosValoresSelected
        End Get
        Set(ByVal value As OyDTesoreria.UltimosValores)
            _UltimosValoresSelected = value
            MyBase.CambioItem("UltimosValoresSelected")
        End Set
    End Property


    Private _ListaTesoreriaGA As EntitySet(Of OyDTesoreria.TesoreriaGA)
    Public Property ListaTesoreriaGA As EntitySet(Of OyDTesoreria.TesoreriaGA)
        Get
            Return _ListaTesoreriaGA
        End Get
        Set(ByVal value As EntitySet(Of OyDTesoreria.TesoreriaGA))
            _ListaTesoreriaGA = value
            MyBase.CambioItem("ListaTesoreriaGA")
            MyBase.CambioItem("ListaTesoreriaGAPaged")
        End Set
    End Property

    Public ReadOnly Property ListaTesoreriaGAPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTesoreriaGA) Then
                Dim view = New PagedCollectionView(_ListaTesoreriaGA)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TotalSeleccionado As Decimal
    Public Property TotalSeleccionado As Decimal
        Get
            Return _TotalSeleccionado
        End Get
        Set(ByVal value As Decimal)
            _TotalSeleccionado = value
            MyBase.CambioItem("TotalSeleccionado")
        End Set
    End Property

    Private _TotalGeneral As Decimal
    Public Property TotalGeneral As Decimal
        Get
            Return _TotalGeneral
        End Get
        Set(ByVal value As Decimal)
            _TotalGeneral = value
            MyBase.CambioItem("TotalGeneral")
        End Set
    End Property

    Private _ParametrosConsultaSelected As New ParametrosConsulta
    Public Property ParametrosConsultaSelected As ParametrosConsulta
        Get
            Return _ParametrosConsultaSelected
        End Get
        Set(ByVal value As ParametrosConsulta)
            _ParametrosConsultaSelected = value
            MyBase.CambioItem("ParametrosConsultaSelected")
        End Set
    End Property

    Private _HabilitarNroDetalles As Boolean = True
    Public Property HabilitarNroDetalles As Boolean
        Get
            Return _HabilitarNroDetalles
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNroDetalles = value
            MyBase.CambioItem("HabilitarNroDetalles")
        End Set
    End Property

    Private _EspecificacionesCESelected As New EspecificacionesCE
    Public Property EspecificacionesCESelected As EspecificacionesCE
        Get
            Return _EspecificacionesCESelected
        End Get
        Set(ByVal value As EspecificacionesCE)
            _EspecificacionesCESelected = value
            MyBase.CambioItem("EspecificacionesCESelected")
        End Set
    End Property

    'JCM20160215
    'Propiedades para cargar los consecutivos por el usuario
    Private _listConsecutivos As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivos = value
            MyBase.CambioItem("listConsecutivos")
        End Set
    End Property


    'Capturar la compañia asociada al consecutivo seleccionado
    Private _strCompaniaConsecutivo As String = String.Empty
    Public Property strCompaniaConsecutivo() As String
        Get
            Return _strCompaniaConsecutivo
        End Get
        Set(ByVal value As String)
            _strCompaniaConsecutivo = value
            MyBase.CambioItem("strCompaniaConsecutivo")
        End Set
    End Property

    Public Property strCompaniaBanco As String = String.Empty

#End Region

    '------------------------------------------------------------------------------------------------------------------------------------------------
    'Validar si la compañia del consecutivo es la misma compañia del banco
    'ID caso de prueba:  CP001
    '------------------------------------------------------------------------------------------------------------------------------------------------


    Private Sub _UltimosValoresSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _UltimosValoresSelected.PropertyChanged
        If e.PropertyName = "NombreConsecutivo" Then
            If Not IsNothing(_UltimosValoresSelected.NombreConsecutivo) Then
                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _UltimosValoresSelected.NombreConsecutivo Select c).FirstOrDefault.Retorno
            Else
                strCompaniaConsecutivo = String.Empty
            End If
            If strCompaniaBanco <> strCompaniaConsecutivo Then
                LimpiarBanco()
            End If
        End If
    End Sub




End Class

''' <summary>
''' Clase para el manejo de los parametros de consulta de los CE pendientes.
''' </summary>
''' <remarks>SLB20130417</remarks>
Public Class ParametrosConsulta
    Implements INotifyPropertyChanged

    Private _Cumplimiento As Date = Now.Date
    Public Property Cumplimiento As Date
        Get
            Return _Cumplimiento
        End Get
        Set(ByVal value As Date)
            _Cumplimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Cumplimiento"))
        End Set
    End Property

    Private _FormadePago As String = "TO"
    Public Property FormadePago As String
        Get
            Return _FormadePago
        End Get
        Set(ByVal value As String)
            _FormadePago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FormadePago"))
        End Set
    End Property

    Private _Receptores As String = "T"
    Public Property Receptores As String
        Get
            Return _Receptores
        End Get
        Set(ByVal value As String)
            _Receptores = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptores"))
        End Set
    End Property

    Private _Modulo As String = "1"
    Public Property Modulo As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modulo"))
        End Set
    End Property

    Private _TipoCliente As String = "0"
    Public Property TipoCliente As String
        Get
            Return _TipoCliente
        End Get
        Set(ByVal value As String)
            _TipoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoCliente"))
        End Set
    End Property

    Private _Sistema As String = "Citinfo"
    Public Property Sistema As String
        Get
            Return _Sistema
        End Get
        Set(ByVal value As String)
            _Sistema = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Sistema"))
        End Set
    End Property

    Private _Tipo As String = "V"
    Public Property Tipo As String
        Get
            Return _Tipo
        End Get
        Set(ByVal value As String)
            _Tipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tipo"))
        End Set
    End Property

    Private _Clase As String = "T"
    Public Property Clase As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    Private _TipoOperacion As String = "TO"
    Public Property TipoOperacion As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _HabilitarSistema As Boolean = False
    Public Property HabilitarSistema As Boolean
        Get
            Return _HabilitarSistema
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSistema = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarSistema"))
        End Set
    End Property

    Private _HabilitarFormaPago As Boolean = False
    Public Property HabilitarFormaPago As Boolean
        Get
            Return _HabilitarFormaPago
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFormaPago = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarFormaPago"))
        End Set
    End Property

    Private _HabilitarTipoCliente As Boolean = True
    Public Property HabilitarTipoCliente As Boolean
        Get
            Return _HabilitarTipoCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarTipoCliente"))
        End Set
    End Property

    Private _HabilitarReceptores As Boolean = True
    Public Property HabilitarReceptores As Boolean
        Get
            Return _HabilitarReceptores
        End Get
        Set(ByVal value As Boolean)
            _HabilitarReceptores = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarReceptores"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class


Public Class EspecificacionesCE
    Implements INotifyPropertyChanged

    Private _NroDetalles As Integer = 100
    Public Property NroDetalles As Integer
        Get
            Return _NroDetalles
        End Get
        Set(ByVal value As Integer)
            _NroDetalles = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroDetalles"))
        End Set
    End Property

    Private _ChequeraAutomatica As Boolean = False
    Public Property ChequeraAutomatica As Boolean
        Get
            Return _ChequeraAutomatica
        End Get
        Set(ByVal value As Boolean)
            _ChequeraAutomatica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ChequeraAutomatica"))
        End Set
    End Property

    Private _HabilitarCheque As Boolean = True
    Public Property HabilitarCheque As Boolean
        Get
            Return _HabilitarCheque
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCheque = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarCheque"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
