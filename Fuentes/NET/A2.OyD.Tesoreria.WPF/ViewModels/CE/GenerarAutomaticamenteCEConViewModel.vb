﻿Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class GenerarAutomaticamenteCEConViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2Utilidades.Mensajes

Public Class GenerarAutomaticamenteCEConViewModel
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
    Private Sub GuardarCE(Optional GraboEncabezado As Boolean = False, Optional GraboLiquidacionBolsa As Boolean = False, Optional GraboDetalle As Boolean = False)
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

                'SLB20130515 Inserta la Liquidación de Bolsa.
                If Not GraboLiquidacionBolsa Then
                    IsBusy = True
                    dcProxy.InsertarLiquidacionesBolsa_Tesoreria(_ListaTesoreriaGA.ElementAt(intNroGuadar).LiquidacionId, _ListaTesoreriaGA.ElementAt(intNroGuadar).Parcial,
                                                 _ListaTesoreriaGA.ElementAt(intNroGuadar).Liquidacion, _ListaTesoreriaGA.ElementAt(intNroGuadar).Tipo,
                                                 _ListaTesoreriaGA.ElementAt(intNroGuadar).ClaseOrden, _ListaTesoreriaGA.ElementAt(intNroGuadar).IDBolsa,
                                                 intNroCE, intSecuencia, _UltimosValoresSelected.NombreConsecutivo, "CE", Program.Usuario, Program.HashConexion, AddressOf TeminoGrabarCEDetalle, "GrabarLiquidacionBolsa")
                    Exit Sub
                End If


                'Inserta los detalles del CE.
                If Not GraboDetalle Then

                    If _UltimosValoresSelected.ClienteRegistrar Then
                        If Not IsNothing(_ListaTesoreriaGA.ElementAt(intNroGuadar).CodComitente) Then
                            Comitente = _ListaTesoreriaGA.ElementAt(intNroGuadar).CodComitente
                        Else
                            Comitente = Nothing
                        End If
                    Else
                        Comitente = Nothing
                    End If

                    IsBusy = True
                    dcProxy.InsertarCEDetalle(_UltimosValoresSelected.NombreConsecutivo, intNroCE, intSecuencia, Comitente, _ListaTesoreriaGA.ElementAt(intNroGuadar).TotalLiquidacion,
                                              _UltimosValoresSelected.IDCuentaContable, _ListaTesoreriaGA.ElementAt(intNroGuadar).Detalle,
                                              _ListaTesoreriaGA.ElementAt(intNroGuadar).NroDocumento, _UltimosValoresSelected.CCosto, Program.Usuario,
                                              Nothing, Nothing, 1, Program.HashConexion, AddressOf TeminoGrabarCEDetalle, "GuardarCEDeetalle")
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
                A2Utilidades.Mensajes.mostrarMensaje("Comprobante de Egreso insertados: " & CStr(intRegistrosInsetados) & vbCr & "Valor de: $" & CStr(ValorTotalRegistrosInsertados), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                ConsultarPendientesTesoreraia("Grabar")
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
            dcProxy.TesoreriaGAConvenidas.Clear()
            dcProxy.Load(dcProxy.ConsultarPendientesConvenidasTesoreriaQuery(_ParametrosConsultaSelected.Cumplimiento, _ParametrosConsultaSelected.FormadePago,
                                                                   _ParametrosConsultaSelected.Receptores, "CE", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPendientesTesoreria, strAccion)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConsultarUltimosValores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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
            If IsNothing(_UltimosValoresSelected.NumCheque) And _UltimosValoresSelected.FormaPagoCE = "C" Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número del cheque para poder grabar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
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
                For Each objLista In ListaTesoreriaGA.Where(Function(i) i.Seleccionado = True) 'Se verifica si el registro que se esta validando de la lista se encutra seleccionado. CVA20171214
                    If objLista.TotalLiquidacion = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No existe valor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Validaciones = False
                        Return Validaciones
                    End If

                    'Se cambia la validacion del campo detalle ya sea que se encuentre vacio o nulado. CVA20171214
                    If String.IsNullOrEmpty(objLista.Detalle) Then
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
                                       "TRAERULTIMOSVALORES",
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
    Private Sub TerminoConsultarPendientesTesoreria(ByVal lo As LoadOperation(Of OyDTesoreria.TesoreriaGAConvenida))
        IsBusy = False
        If Not lo.HasError Then
            If dcProxy.TesoreriaGAConvenidas.Count > 0 Then
                ListaTesoreriaGA = dcProxy.TesoreriaGAConvenidas
            Else
                If Not lo.UserState = "Grabar" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existen operaciones de compra para los parámetros dados o ya fueron creados los comprobantes de egreso respectivos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Pendientes de Tesorería.",
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
            GuardarCE(True)
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
            Select Case lo.UserState.ToString
                Case "GrabarLiquidacionBolsa"
                    GuardarCE(True, True)
                Case "GuardarCEDeetalle"
                    GuardarCE(True, True, True)
            End Select
        Else
            'SLB20130422 En caso de fallar de deben borrar los encabezados y detalles previamente insertados.
            dcProxy.BorrarDocumentoTesoreria(intNroCE, _UltimosValoresSelected.NombreConsecutivo, "CE", Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarTesoreria, "borrar")
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al " & lo.UserState.ToString & ".",
                     Me.ToString(), "TeminoGrabarCEDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)
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
    ''' Método recibe la respuesta si el banco existe o no
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>20130201</remarks>
    Private Sub TerminoTraerCuentasBancarias(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "cuentasbancarias"
                        '_mlogBuscarBancos = False
                        If lo.Entities.ToList.Count > 0 Then
                            If lo.Entities.First.InfoAdicional02.Equals("1") Then
                                _UltimosValoresSelected.IdBanco = lo.Entities.First.IdItem
                                _UltimosValoresSelected.Banco = lo.Entities.First.Nombre
                                If lo.Entities.First.Estado Then
                                    EspecificacionesCESelected.HabilitarCheque = False
                                    _UltimosValoresSelected.NumCheque = 0
                                Else
                                    EspecificacionesCESelected.HabilitarCheque = True
                                End If
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                _UltimosValoresSelected.IdBanco = Nothing
                                _UltimosValoresSelected.Banco = String.Empty
                                EspecificacionesCESelected.HabilitarCheque = True
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _UltimosValoresSelected.IdBanco = Nothing
                            _UltimosValoresSelected.Banco = String.Empty
                            EspecificacionesCESelected.HabilitarCheque = True
                        End If
                        '_mlogBuscarBancos = True
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

    Private _UltimosValoresSelected As OyDTesoreria.UltimosValores
    Public Property UltimosValoresSelected As OyDTesoreria.UltimosValores
        Get
            Return _UltimosValoresSelected
        End Get
        Set(ByVal value As OyDTesoreria.UltimosValores)
            _UltimosValoresSelected = value
            MyBase.CambioItem("UltimosValoresSelected")
        End Set
    End Property


    Private _ListaTesoreriaGA As EntitySet(Of OyDTesoreria.TesoreriaGAConvenida)
    Public Property ListaTesoreriaGA As EntitySet(Of OyDTesoreria.TesoreriaGAConvenida)
        Get
            Return _ListaTesoreriaGA
        End Get
        Set(ByVal value As EntitySet(Of OyDTesoreria.TesoreriaGAConvenida))
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

    Private _ParametrosConsultaSelected As New ParametrosConsultaCECon
    Public Property ParametrosConsultaSelected As ParametrosConsultaCECon
        Get
            Return _ParametrosConsultaSelected
        End Get
        Set(ByVal value As ParametrosConsultaCECon)
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

#End Region


End Class

''' <summary>
''' Clase para el manejo de los parametros de consulta de los CE pendientes.
''' </summary>
''' <remarks>SLB20130417</remarks>
Public Class ParametrosConsultaCECon
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

    Private _FormadePago As String = "C"
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

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

