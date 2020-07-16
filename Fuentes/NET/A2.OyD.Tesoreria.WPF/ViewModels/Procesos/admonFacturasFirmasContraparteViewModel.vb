Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Juan David Osorio 
'
'Junion 2013
'Propiedad de Alcuadrado S.A.
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
'Imports Microsoft.VisualBasic.ac
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports A2ComunesImportaciones



Public Class admonFacturasFirmasContraparteViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim objAnterior As New AdmonFacturasFirmasContraparte
    Public Const CSTR_NOMBREPROCESO_ADMONFACTURASFIRMAS = "TESORERIA_ADMONFACTURASFIRMAS"



    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
                dcProxy1 = New TesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext()

            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy1 = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If
            IsBusy = True

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
            dcProxy.Load(dcProxy.TraerAdmonFacturasFirmasContraparteQuery(Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdmonFacturasFirmas, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "admonFactirasFirmasContraparteViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#Region "Metodos"
    ''' <summary>
    ''' Metodo para realizar el proceso de validaciones
    ''' </summary>
    ''' <remarks>
    ''' metodo          : Validaciones()
    ''' Se encarga de    : Valida si se han seleccionado forma de pago, consecutivo de recibo, banco girador y consignacion, detalle.
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function Validaciones() As Boolean
        Try
            Dim logValidacion As Boolean = True
            Dim strMensajeValidacion As String = String.Empty


            If Selected.Saldo = 0 Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} - Saldo por Pagar: No se puede modificar el valor del saldo por pagar es igual a 0", strMensajeValidacion, vbCrLf)
            End If

            If SelectedDETALLE.curValorAbonar < 0 Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} - Valor abonar: el valor ingresado no puede ser menor que 0", strMensajeValidacion, vbCrLf)
            Else
                If SelectedDETALLE.curValorAbonar > Selected.Saldo Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} - Valor abonar: el valor ingresado es mayor al saldo por pagar de la factura", strMensajeValidacion, vbCrLf)
                End If
            End If

            If String.IsNullOrEmpty(strFormaPago) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} - Forma de Pago: es necesario seleccionar una forma de pago", strMensajeValidacion, vbCrLf)

            End If
            If String.IsNullOrEmpty(NombreConsecutivo) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} - Consecutivo Recibo de Caja: es necesario seleccionar un Consecutivo de Recibo de Caja", strMensajeValidacion, vbCrLf)
            End If
            If String.IsNullOrEmpty(BancoConsignacion) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} - Banco consignación: es necesario seleccionar un Banco de consignación", strMensajeValidacion, vbCrLf)
            End If
            If String.IsNullOrEmpty(strDetalle) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} -Es necesario ingresar el detalle", strMensajeValidacion, vbCrLf)
            End If
            If String.IsNullOrEmpty(strBancoGirador) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} -Es necesario ingresar el Banco Girador", strMensajeValidacion, vbCrLf)
            End If

            If logValidacion = False Then
                mostrarMensaje("Validaciones:" & vbCrLf & strMensajeValidacion, "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If

        Catch ex As Exception
            IsBusy = False
            Return False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en las Validaciones.", _
                                Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Function

    ''' <summary>
    ''' Metodo para realizar el proceso de Grabacion
    ''' </summary>
    ''' <remarks>
    ''' metodo          : ActualizarRegistro()
    ''' Se encarga de    : de hacer el llamado a las funciones de Grabacion y generacion de documentos
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Overrides Sub ActualizarRegistro()
        Try
            IsBusy = True
            IsBusyDetalles = True
            If Validaciones() Then
                dcProxy.Load(dcProxy.ActualizarAdmonFacturasFirmasContraparteQuery(Selected.lngID, Selected.dtmDocumento, Selected.lngIDComitente, _
                NombreConsecutivo, Selected.strNombre, Selected.ValorTipoIdentificacion, Selected.strNroIdentificacion, SelectedDETALLE.curValorAbonar, strFormaPago, _
                lngCodigoBancoConsignacion, strBancoGirador, FechaConsignacion, strDetalle, Nrocheque, strObservaciones, Selected.strPrefijo, Program.Usuario, Program.HashConexion),
                            AddressOf TerminoActualizarRegistro, String.Empty)
            Else
                IsBusyDetalles = False
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Actualizar.", _
                                Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub




    ''' <summary>
    ''' Metodo para realizar el proceso de edicion
    ''' </summary>
    ''' <remarks>
    ''' metodo          : EditarRegistro()
    ''' Se encarga de    : de habilitar la forma para ingreso de datos a Editar y generar documentos
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Overrides Sub EditarRegistro()

        Try
            If Not IsNothing(_Selected) Then
                If _Selected.ValorEstado = GSTR_PENDIENTE_Plus And _Selected.Saldo <> 0 Then
                    objAnterior = _Selected


                    Editando = True
                    MyBase.CambioItem("Editando")
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                    mostrarMensaje("No se puede Editar verifique que el estado sea pendiente y que el saldo por pagar sea mayor a 0", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                End If
            Else
                'No existe registro para actualizar
            End If

        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Editar", _
                                Me.ToString(), "Editar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub


    ''' <summary>
    ''' Metodo para cancelar la edicion
    ''' </summary>
    ''' <remarks>
    ''' metodo          : CancelarEditarRegistro()
    ''' Se encarga de    :se encarga de cancelar la opcion de edicion y inhabilitar la forma
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            If Not IsNothing(objAnterior) Then
                Selected = objAnterior
                BancoConsignacion = String.Empty
                strDetalle = String.Empty
                strFormaPago = String.Empty
                strObservaciones = String.Empty
                NombreConsecutivo = String.Empty
                strBancoGirador = String.Empty
                FechaConsignacion = DateTime.Now
                Nrocheque = 0
                BancoConsignacionDescripcion = String.Empty
                strObservaciones = String.Empty
            End If

            Editando = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cancelar Edición.", _
                                Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub


    ''' <summary>
    ''' funcion para  evaluar si una variable es numerica
    ''' </summary>
    ''' <remarks>
    ''' funcion          : IsNumeric()
    ''' Se encarga de    :se encarga de evaluar cuando una variable es numerica devolviendo valor booleano
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function IsNumeric(ByVal Str As String)
        Try
            Return Regex.IsMatch(Str, "\d+")
        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar sí el número era valido.", _
                                 Me.ToString(), "IsNumeric", Application.Current.ToString(), Program.Maquina, ex)

            Return False
        End Try
    End Function



    ''' <summary>
    ''' Metodo para refrescar la pantalla y haciendo llamado de listar los registros
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : Refrescar()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Sub Refrescar()
        Try
            IsBusyDetalles = True
            IsBusy = True
            dcProxy.AdmonFacturasFirmasContrapartes.Clear()
            dcProxy.RetornoAdmonFacturasFirmas.Clear()
            dcProxy.Load(dcProxy.TraerAdmonFacturasFirmasContraparteQuery(Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdmonFacturasFirmas, "")

        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Refrescar Pantalla.", _
                                Me.ToString(), "Refrescar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Metodo para realizar filtrado por el numero del registro
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : Filtrar()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            If Not IsNothing(dcProxy.AdmonFacturasFirmasContrapartes) Then
                dcProxy.AdmonFacturasFirmasContrapartes.Clear()
            End If

            If FiltroVM.Length > 0 Then
                If Regex.IsMatch(FiltroVM, "^[0-9]*$") Then

                    dcProxy.Load(dcProxy.TraerAdmonFacturasFirmasContraparteQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdmonFacturasFirmas, "")

                Else
                    IsBusy = False
                    FiltroVM = String.Empty
                    mostrarMensaje("¡La opción filtrar no se puede realizar, el filtro que ingreso posee caracteres NO válidos!", "Filtrar", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                End If
            Else
                IsBusy = False
                IsBusyDetalles = False
                dcProxy.Load(dcProxy.TraerAdmonFacturasFirmasContraparteQuery(Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdmonFacturasFirmas, "")

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Metodo para realizar Busqueda por el numero del registro
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : ConfirmarBuscar()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.lngID <> 0 Or Not IsNothing(cb.lngID) Then
                IsBusy = True
                ErrorForma = ""
                objAnterior = Nothing
                If Not IsNothing(dcProxy.AdmonFacturasFirmasContrapartes) Then
                    dcProxy.AdmonFacturasFirmasContrapartes.Clear()
                End If

                dcProxy.Load(dcProxy.TraerAdmonFacturasFirmasContraparteQuery(cb.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdmonFacturasFirmas, "")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaAdmonFacturasFirmasContraparte
            Else
                mostrarMensaje("Para Realizar la Busqueda ingrese los Datos Correspondientes Número.", "Confirmar Buscar", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
        End Try

    End Sub



    ''' <summary>
    ''' Metodo para realizar exportacion de consulta a Excel
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : EjecutarConsulta()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Sub EjecutarConsulta()
        Try
            mostrarMensajePregunta("Se va generar el Reporte de Facturas pendientes por pagar", _
                                   Program.TituloSistema, _
                                   "Información", _
                                   AddressOf TerminoPreguntarConfirmacion, _
                                   True, _
                                   "¿Desea continuar?")
       
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al ejecutar la consulta.", _
                                 Me.ToString(), "ReporteExcelTitulosViewModel.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "ResultadosAsincronicos"

    Private Sub TerminoPreguntarConfirmacion(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                IsBusy = True
                dcProxy.Traer_ReporteExcelAdmonFacturasFirmas(Program.Usuario, Program.HashConexion, AddressOf TerminoTraerReporteExcel, "csv")
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al confirmar.", _
                                Me.ToString(), "TerminoPreguntarConfirmacion.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Metodo asincronico que trae los resultado de los registros de encabezado
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : TerminoTraerAdmonFacturasFirmas()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Sub TerminoTraerAdmonFacturasFirmas(ByVal lo As LoadOperation(Of OyDTesoreria.AdmonFacturasFirmasContraparte))
        Try
            If Not lo.HasError Then
                If dcProxy.AdmonFacturasFirmasContrapartes.Count > 0 Then
                    ListaAdmonFacturasFirmas = dcProxy.AdmonFacturasFirmasContrapartes.ToList
                    IsBusy = False
                Else
                    IsBusy = False
                    ListaAdmonFacturasFirmas = Nothing
                    'mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista", _
                                                 Me.ToString(), "TerminoTraerAdmonFacturasFirmas", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista ", _
                                                             Me.ToString(), "TerminoTraerAdmonFacturasFirmas", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub


    ''' <summary>
    ''' Metodo asincronico que trae los resultado de los registros de DEtalle
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : TerminoTraerAdmonFacturasFirmasDetalle()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Sub TerminoTraerAdmonFacturasFirmasDetalle(ByVal lo As LoadOperation(Of OyDTesoreria.AdmonFacturasFirmasContraparteDetalle))
        Try
            If Not lo.HasError Then
                If dcProxy.AdmonFacturasFirmasContraparteDetalles.Count > 0 Then
                    ListaAdmonFacturasFirmasDETALLE = dcProxy.AdmonFacturasFirmasContraparteDetalles.ToList
                    IsBusy = False
                    IsBusyDetalles = False
                Else
                    IsBusy = False
                    IsBusyDetalles = False
                    ListaAdmonFacturasFirmasDETALLE = Nothing
                    'mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                IsBusy = False
                IsBusyDetalles = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista", _
                                                 Me.ToString(), "TerminoTraerAdmonFacturasFirmasDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista ", _
                                                             Me.ToString(), "TerminoTraerAdmonFacturasFirmasDetalle", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub



    ''' <summary>
    ''' Metodo asincronico que trae los resultado de los documentos generados
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : TerminoActualizarRegistro()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Sub TerminoActualizarRegistro(ByVal lo As LoadOperation(Of RetornoAdmonFacturasFirmas))
        Try
            If Not lo.HasError Then
                IsBusy = False
                IsBusyDetalles = False
                If dcProxy.RetornoAdmonFacturasFirmas.Count > 0 Then
                    For Each li In dcProxy.RetornoAdmonFacturasFirmas
                        mostrarMensaje(li.strMensaje, "", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    Next
                    Selected = objAnterior
                    BancoConsignacion = String.Empty
                    strDetalle = String.Empty
                    strFormaPago = String.Empty
                    strObservaciones = String.Empty
                    NombreConsecutivo = String.Empty
                    strBancoGirador = String.Empty
                    Nrocheque = 0
                    FechaConsignacion = DateTime.Now
                    strObservaciones = String.Empty
                    BancoConsignacionDescripcion = String.Empty

                    Editando = False
                    MyBase.CambioItem("Editando")
                    Refrescar()

                Else
                    IsBusy = False
                    IsBusyDetalles = False
                End If
            Else
                IsBusy = False
                IsBusyDetalles = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar valores de Actualización", _
                                                 Me.ToString(), "TerminoActualizarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)

            End If
        Catch ex As Exception
            IsBusy = False
            IsBusyDetalles = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar valores de Actualización ", _
                                                             Me.ToString(), "TerminoActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub

    ''' <summary>
    ''' Metodo asincronico que trae los resultados de la exportacion a Excel
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : TerminoTraerReporteExcel()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Private Sub TerminoTraerReporteExcel(ByVal lo As InvokeOperation(Of System.Nullable(Of Boolean)))

        Try
            If Not lo.HasError Then

                TerminoCrearArchivo()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte ", _
                                 Me.ToString(), "TerminoTraerReporteExcel", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte ", _
                                 Me.ToString(), "TerminoTraerReporteExcel", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' Metodo que genera y abre el archivo de Excel
    ''' </summary>
    ''' <remarks>
    ''' Metodo          : TerminoCrearArchivo()
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_ADMONFACTURASFIRMAS)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos", _
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

#End Region

#Region "Propiedades"
    Private _cb As CamposBusquedaAdmonFacturasFirmasContraparte = New CamposBusquedaAdmonFacturasFirmasContraparte
    Public Property cb() As CamposBusquedaAdmonFacturasFirmasContraparte
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaAdmonFacturasFirmasContraparte)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property
    Private _strObservaciones As String
    Public Property strObservaciones() As String
        Get
            Return _strObservaciones
        End Get
        Set(ByVal value As String)
            _strObservaciones = value
            MyBase.CambioItem("strObservaciones")
        End Set
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
    Private _Nrocheque As Integer
    Public Property Nrocheque() As Integer
        Get
            Return _Nrocheque
        End Get
        Set(ByVal value As Integer)
            _Nrocheque = value
            MyBase.CambioItem("Nrocheque")
        End Set
    End Property
    Private _NombreConsecutivo As String
    Public Property NombreConsecutivo() As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            MyBase.CambioItem("NombreConsecutivo")
        End Set
    End Property
    Private _strDetalle As String
    Public Property strDetalle() As String
        Get
            Return _strDetalle
        End Get
        Set(ByVal value As String)
            _strDetalle = value
            MyBase.CambioItem("strDetalle")
        End Set
    End Property
    Private _strBancoGirador As String
    Public Property strBancoGirador() As String
        Get
            Return _strBancoGirador
        End Get
        Set(ByVal value As String)
            _strBancoGirador = value
            MyBase.CambioItem("strBancoGirador")
        End Set
    End Property
    Private _FechaConsignacion As DateTime = DateTime.Now
    Public Property FechaConsignacion() As DateTime
        Get
            Return _FechaConsignacion
        End Get
        Set(ByVal value As DateTime)
            _FechaConsignacion = value
            MyBase.CambioItem("FechaConsignacion")
        End Set
    End Property



    Private _HabilitarModificar As Boolean = False
    Public Property HabilitarModificar() As Boolean
        Get
            Return _HabilitarModificar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarModificar = value
            MyBase.CambioItem("HabilitarModificar")
        End Set
    End Property


    Private _ListaAdmonFacturasFirmas As List(Of AdmonFacturasFirmasContraparte)
    Public Property ListaAdmonFacturasFirmas() As List(Of AdmonFacturasFirmasContraparte)
        Get
            Return _ListaAdmonFacturasFirmas
        End Get
        Set(ByVal value As List(Of AdmonFacturasFirmasContraparte))
            _ListaAdmonFacturasFirmas = value

            MyBase.CambioItem("ListaAdmonFacturasFirmas")
            MyBase.CambioItem("ListaAdmonFacturasFirmas_Paged")

            If Not IsNothing(_ListaAdmonFacturasFirmas) Then
                Selected = _ListaAdmonFacturasFirmas.First
            End If
        End Set
    End Property

    Private _Selected As AdmonFacturasFirmasContraparte
    Public Property Selected() As AdmonFacturasFirmasContraparte
        Get
            Return _Selected
        End Get
        Set(ByVal value As AdmonFacturasFirmasContraparte)
            _Selected = value
            If Not IsNothing(_Selected) Then


                dcProxy.AdmonFacturasFirmasContraparteDetalles.Clear()
                dcProxy.Load(dcProxy.TraerAdmonFacturasFirmasContraparteDETALLEQuery(_Selected.lngID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAdmonFacturasFirmasDetalle, "")
                'IsBusyDetalles = True

            End If
            MyBase.CambioItem("Selected")
        End Set
    End Property
    Public ReadOnly Property ListaAdmonFacturasFirmas_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAdmonFacturasFirmas) Then
                Dim view = New PagedCollectionView(_ListaAdmonFacturasFirmas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaAdmonFacturasFirmasDETALLE As List(Of AdmonFacturasFirmasContraparteDetalle)
    Public Property ListaAdmonFacturasFirmasDETALLE() As List(Of AdmonFacturasFirmasContraparteDetalle)
        Get
            Return _ListaAdmonFacturasFirmasDETALLE
        End Get
        Set(ByVal value As List(Of AdmonFacturasFirmasContraparteDetalle))
            _ListaAdmonFacturasFirmasDETALLE = value
            If Not IsNothing(_ListaAdmonFacturasFirmasDETALLE) Then
                SelectedDETALLE = _ListaAdmonFacturasFirmasDETALLE.FirstOrDefault
            End If
            MyBase.CambioItem("ListaAdmonFacturasFirmasDETALLE")
            MyBase.CambioItem("ListaAdmonFacturasFirmasDETALLE_Paged")
        End Set
    End Property

    Private _SelectedDETALLE As AdmonFacturasFirmasContraparteDetalle
    Public Property SelectedDETALLE() As AdmonFacturasFirmasContraparteDetalle
        Get
            Return _SelectedDETALLE
        End Get
        Set(ByVal value As AdmonFacturasFirmasContraparteDetalle)
            _SelectedDETALLE = value

            MyBase.CambioItem("SelectedDETALLE")
        End Set
    End Property
    Public ReadOnly Property ListaAdmonFacturasFirmasDETALLE_Paged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAdmonFacturasFirmasDETALLE) Then
                Dim view = New PagedCollectionView(_ListaAdmonFacturasFirmasDETALLE)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property




    Private _strTipoIdentificacion As String
    Public Property strTipoIdentificacion() As String
        Get
            Return _strTipoIdentificacion
        End Get
        Set(ByVal value As String)
            _strTipoIdentificacion = value
            MyBase.CambioItem("strTipoIdentificacion")
        End Set
    End Property
    Private _RbPendiente As Boolean
    Public Property RbPendiente() As Boolean
        Get
            Return _RbPendiente
        End Get
        Set(ByVal value As Boolean)
            _RbPendiente = value
            If _RbPendiente Then
                RbAnulada = False
            End If

            MyBase.CambioItem("RbPendiente")
        End Set
    End Property
    Private _RbAnulada As Boolean
    Public Property RbAnulada() As Boolean
        Get
            Return _RbAnulada
        End Get
        Set(ByVal value As Boolean)
            _RbAnulada = value
            MyBase.CambioItem("RbAnulada")
        End Set
    End Property
    Private _RbImpresa As Boolean
    Public Property RbImpresa() As Boolean
        Get
            Return _RbImpresa
        End Get
        Set(ByVal value As Boolean)
            _RbImpresa = value
            MyBase.CambioItem("RbImpresa")
        End Set
    End Property

    Private _IsBusyDetalles As Boolean = False
    Public Property IsBusyDetalles() As Boolean
        Get
            Return _IsBusyDetalles
        End Get
        Set(ByVal value As Boolean)
            _IsBusyDetalles = value
            MyBase.CambioItem("IsBusyDetalles")
        End Set
    End Property

    Private _BancoConsignacion As String
    Public Property BancoConsignacion() As String
        Get
            Return _BancoConsignacion
        End Get
        Set(ByVal value As String)
            _BancoConsignacion = value
            MyBase.CambioItem("BancoConsignacion")
        End Set
    End Property
    Private _BancoConsignacionDescripcion As String
    Public Property BancoConsignacionDescripcion() As String
        Get
            Return _BancoConsignacionDescripcion
        End Get
        Set(ByVal value As String)
            _BancoConsignacionDescripcion = value
            MyBase.CambioItem("BancoConsignacionDescripcion")
        End Set
    End Property

    Private _lngCodigoBancoConsignacion As Integer
    Public Property lngCodigoBancoConsignacion() As Integer
        Get
            Return _lngCodigoBancoConsignacion
        End Get
        Set(ByVal value As Integer)
            _lngCodigoBancoConsignacion = value
            MyBase.CambioItem("lngCodigoBancoConsignacion")
        End Set
    End Property







#End Region

    Public Class CamposBusquedaAdmonFacturasFirmasContraparte
        Implements INotifyPropertyChanged

        Private _lngID As Integer
        <Display(Name:="Número", Description:="Número")> _
        Property lngID As Integer
            Get
                Return _lngID
            End Get
            Set(ByVal value As Integer)
                _lngID = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngID"))
            End Set
        End Property

        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

        Public Sub New()

        End Sub
    End Class
End Class




