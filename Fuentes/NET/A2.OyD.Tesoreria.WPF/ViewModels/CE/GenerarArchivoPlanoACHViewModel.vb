Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class GenerarArchivoPlanoACHViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.Web
Imports System.Text.RegularExpressions
Imports A2Utilidades.Mensajes

Public Class GenerarArchivoPlanoACHViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcArchivosProxy As ImportacionesDomainContext
    Dim FechaCierre As DateTime
    Dim NroRegistros As Integer
    Dim Mensaje As String
    Dim NroAccionesGen As Integer
    Dim NroRentaGen As Integer
    Dim intNroGuadar As Integer = 0
    Dim intRegistrosInsetados As Integer = 0
    Dim intNroCE, intSecuencia As Integer
    Dim strCadena As String = String.Empty
    Dim NroDocumento, NombreCliente, Comitente As String
    Dim ValorTotalRegistrosInsertados As Double
    Dim strFormato As String
    Dim cwCuentasBancarias As cwCuentasBancariasClientes
    Public _mlogBuscarBancos As Boolean = True
    Public Const CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO = "ArchivoPlanoACH"
    Dim sw As Byte = 0
    Dim strBorrar As String = String.Empty
    Dim _mlogValidoNombrePlano As Boolean = True

#Region "Inicializaciones"

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()
            dcProxy1 = New TesoreriaDomainContext()
            objProxy = New UtilidadesDomainContext()
            dcArchivosProxy = New ImportacionesDomainContext()
        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            dcArchivosProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 1800)
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                ConsultarPendientesTesoreraia()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Método generaración del Plano ACH.
    ''' </summary>
    ''' <remarks>SLB20130530</remarks>
    Public Sub GuardarPlanoACH()
        Try
            If Validaciones() Then
                'C1.Silverlight.C1MessageBox.Show("Está seguro de generar el archivo plano", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
                '                                 C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPregunta)
                mostrarMensajePregunta("Está seguro de generar el archivo plano", _
                                       Program.TituloSistema, _
                                       "GUARDARPLANO", _
                                       AddressOf TerminoPregunta, True, "¿Generar?")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            intNroGuadar = 0
            intRegistrosInsetados = 0
            intSecuencia = 0
            ValorTotalRegistrosInsertados = 0
            strCadena = "<root> "
            strBorrar = String.Empty
            GenerarPlano()
        End If
    End Sub

    ''' <summary>
    ''' Método encargado de Insertar en las tablas temporales para posteriormente generar el plano.
    ''' </summary>
    ''' <param name="GraboRCExportados"></param>
    ''' <remarks>SLB20130530</remarks>
    Private Sub GenerarPlano(Optional GraboRCExportados As Boolean = False)
        Try
            If ListaTesoreriaGA.ElementAt(intNroGuadar).Seleccionado Then

                'Inserta en la tabla RCExportados.
                If Not GraboRCExportados Then
                    IsBusy = True
                    dcProxy.InsertaRCExportados("CE", _ListaTesoreriaGA.ElementAt(intNroGuadar).NombreConsecutivo, _ListaTesoreriaGA.ElementAt(intNroGuadar).Documento, _
                                                _ListaTesoreriaGA.ElementAt(intNroGuadar).NroComprobante, Program.Usuario, Program.HashConexion, AddressOf TerminoInsertarRCExportados, "Guadar")
                    Exit Sub
                End If
                strBorrar = strBorrar & _ListaTesoreriaGA.ElementAt(intNroGuadar).NombreConsecutivo & "*" & _ListaTesoreriaGA.ElementAt(intNroGuadar).NroComprobante.ToString & ";"

                strCadena = strCadena & "<Datos "
                strCadena = strCadena & " NroDctoTesoreria = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).NroComprobante & ChrW(34)
                strCadena = strCadena & " Secuencia = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).Secuencia & ChrW(34)
                strCadena = strCadena & " NombreConsecutivo = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).NombreConsecutivo & ChrW(34)
                strCadena = strCadena & " TipoDctoTesoreria = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).Tipo & ChrW(34)
                strCadena = strCadena & " CodigoBancoNal = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).IDBanco & ChrW(34)
                strCadena = strCadena & " NombreBanco = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).NombreBanco & ChrW(34)
                strCadena = strCadena & " SucursalBanco = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).NombreSucursal & ChrW(34)
                strCadena = strCadena & " NroCtaBancaria = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).Cuenta & ChrW(34)
                strCadena = strCadena & " TipoCuenta = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).TipoCuenta & ChrW(34)
                strCadena = strCadena & " CodigoACHBanco = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).CodigoACH & ChrW(34)
                strCadena = strCadena & " Titular = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).Titular & ChrW(34)
                strCadena = strCadena & " TipoIDTitular =" & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).TipoID & ChrW(34)
                strCadena = strCadena & " NroIDTitular = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).NumeroID & ChrW(34)
                strCadena = strCadena & " EsDeTransferencia = " & ChrW(34) & _ListaTesoreriaGA.ElementAt(intNroGuadar).TransferirA & ChrW(34)
                strCadena = strCadena & " />"

                intSecuencia = intSecuencia + 1
            End If

            'Recorrer el grid de Pendientes de Tesorería hasta terminar.
            If intNroGuadar < (ListaTesoreriaGA.Count - 1) Then
                intNroGuadar = intNroGuadar + 1
                GenerarPlano()
            Else
                strCadena = Replace(strCadena, "Ñ", "N")
                strCadena = Replace(strCadena, "Á", "A")
                strCadena = Replace(strCadena, "É", "E")
                strCadena = Replace(strCadena, "Í", "I")
                strCadena = Replace(strCadena, "Ó", "O")
                strCadena = Replace(strCadena, "Ú", "U")
                strCadena = Replace(strCadena, "Ü", "U")

                'Descripción:   Se agregan caracteres especiales porque en faltan algunos por validar
                'Creado Por:    Jorge Peña (Alcuadrado S.A.)
                'Fecha:         2 de Junio 2011

                '            strCadena = Replace(strCadena, "¿", "")
                '            strCadena = Replace(strCadena, "¡", "")
                '            strCadena = Replace(strCadena, "¢", "")
                '
                '            'Caracteres especiales ASCII del 192 al 255
                strCadena = Replace(strCadena, "À", "A")
                strCadena = Replace(strCadena, "Â", "A")
                strCadena = Replace(strCadena, "Ã", "A")
                strCadena = Replace(strCadena, "Ä", "A")
                strCadena = Replace(strCadena, "Å", "A")
                strCadena = Replace(strCadena, "Æ", "")
                strCadena = Replace(strCadena, "Ç", "C")
                strCadena = Replace(strCadena, "È", "E")
                strCadena = Replace(strCadena, "Ê", "E")
                strCadena = Replace(strCadena, "Ë", "E")
                strCadena = Replace(strCadena, "Ì", "I")
                strCadena = Replace(strCadena, "Î", "I")
                strCadena = Replace(strCadena, "Ï", "I")
                strCadena = Replace(strCadena, "Ð", "")
                strCadena = Replace(strCadena, "Ò", "O")
                strCadena = Replace(strCadena, "Ô", "O")
                strCadena = Replace(strCadena, "Õ", "O")
                strCadena = Replace(strCadena, "Ö", "O")
                strCadena = Replace(strCadena, "¥", "")
                strCadena = Replace(strCadena, "×", "")
                strCadena = Replace(strCadena, "Ø", "")
                strCadena = Replace(strCadena, "Ù", "U")
                strCadena = Replace(strCadena, "Û", "U")
                strCadena = Replace(strCadena, "Ü", "U")
                strCadena = Replace(strCadena, "Ý", "Y")
                strCadena = Replace(strCadena, "Þ", "")
                strCadena = Replace(strCadena, "ß", "")
                strCadena = Replace(strCadena, "à", "a")
                strCadena = Replace(strCadena, "â", "a")
                strCadena = Replace(strCadena, "á", "a")
                strCadena = Replace(strCadena, "ã", "a")
                strCadena = Replace(strCadena, "ä", "a")
                strCadena = Replace(strCadena, "å", "a")
                strCadena = Replace(strCadena, "æ", "")
                strCadena = Replace(strCadena, "ç", "c")
                strCadena = Replace(strCadena, "è", "e")
                strCadena = Replace(strCadena, "é", "e")
                strCadena = Replace(strCadena, "ê", "e")
                strCadena = Replace(strCadena, "ë", "e")
                strCadena = Replace(strCadena, "ì", "i")
                strCadena = Replace(strCadena, "í", "i")
                strCadena = Replace(strCadena, "î", "i")
                strCadena = Replace(strCadena, "ï", "i")
                strCadena = Replace(strCadena, "ð", "")
                strCadena = Replace(strCadena, "ò", "o")
                strCadena = Replace(strCadena, "ó", "o")
                strCadena = Replace(strCadena, "ô", "o")
                strCadena = Replace(strCadena, "õ", "o")
                strCadena = Replace(strCadena, "ö", "o")
                strCadena = Replace(strCadena, "÷", "")
                strCadena = Replace(strCadena, "ø", "")
                strCadena = Replace(strCadena, "ù", "u")
                strCadena = Replace(strCadena, "ú", "u")
                strCadena = Replace(strCadena, "û", "u")
                strCadena = Replace(strCadena, "ü", "u")
                strCadena = Replace(strCadena, "ý", "y")
                strCadena = Replace(strCadena, "þ", "")
                strCadena = Replace(strCadena, "ÿ", "")
                strCadena = Replace(strCadena, "ñ", "n")

                strCadena = Replace(strCadena, "`", "")
                strCadena = Replace(strCadena, "´", "")
                strCadena = Replace(strCadena, ".", "")
                strCadena = Replace(strCadena, "&", "")
                strCadena = Replace(strCadena, "*", "")
                strCadena = Replace(strCadena, "-", "")
                strCadena = Replace(strCadena, ",", "")
                strCadena = Replace(strCadena, "(", "")
                strCadena = Replace(strCadena, ")", "")
                strCadena = Replace(strCadena, "\", "")
                strCadena = Replace(strCadena, "'", "")
                strCadena = Replace(strCadena, "?", "")
                '/******************************************************************************************
                strCadena = Replace(strCadena, "&", " ")
                '/* Realizado Por : Ricardo peña (Alcuadrado)
                '/* fecha : 07-05-2010.
                '/******************************************************************************************
                strCadena = strCadena & "</root> "

                DescripcionFormato()
                'Inserta en las tablas temporlas para generar el plano.
                IsBusy = True
                dcProxy.InsertarDatosPlanoACH(_ParametrosConsultaSelected.Formato, strCadena, strFormato, Program.Usuario, Program.HashConexion, AddressOf TerminoInsertarDatosPlanoACH, "GenerarACH")
                Exit Sub
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "GuardarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que consulta los documentos de Tesorería que estan pendientes por Generar .
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Sub ConsultarPendientesTesoreraia()
        Try
            Dim Desde, Hasta As Date?
            If Not _ParametrosConsultaSelected.Elaboracion Then
                Desde = Nothing
                Hasta = Nothing
            Else
                Desde = _ParametrosConsultaSelected.Desde
                Hasta = _ParametrosConsultaSelected.Hasta
            End If

            DescripcionFormato()

            IsBusy = True
            dcProxy.TesoreriaACHPendientes.Clear()
            dcProxy.Load(dcProxy.ConsultarPendientesACHQuery(Desde, Hasta, _
                                                             _ParametrosConsultaSelected.NombreConsecutivo, _ParametrosConsultaSelected.NroDocumento, _
                                                             _ParametrosConsultaSelected.IDBanco, strFormato, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPendientesTesoreria, "Consulta")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsultarUltimosValores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Devuelve la descripcion del formato seleccionado.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub DescripcionFormato()
        If Not String.IsNullOrEmpty(_ParametrosConsultaSelected.Formato) Then
            strFormato = (From dc In dicListaCombos Where dc.ID = _ParametrosConsultaSelected.Formato Select dc.Descripcion).FirstOrDefault
        Else
            strFormato = String.Empty
        End If
    End Sub

    ''' <summary>
    ''' Método de las validaciones para generar los Comprobantes de Egreso.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Function Validaciones() As Boolean
        Validaciones = True

        If IsNothing(_ParametrosConsultaSelected.IDBanco) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe elegir un banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If


        If _ParametrosConsultaSelected.Formato = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe elegir un formato.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        Dim Seleccionado As Boolean = False
        If Not IsNothing(_ListaTesoreriaGA) Then
            If ListaTesoreriaGA.Count > 0 Then
                For Each objLista In ListaTesoreriaGA
                    If objLista.Seleccionado Then
                        Seleccionado = True
                    End If
                Next
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Deben existir datos de los comprobantes de Egreso. Cambie los filtros de consulta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If
        Else
            A2Utilidades.Mensajes.mostrarMensaje("Deben existir datos de los comprobantes de Egreso. Cambie los filtros de consulta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If Not Seleccionado Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar por lo menos un comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(RutaArchivoPlano) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe definir el nombre del archivo plano.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(ExtensionPlano) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la extensión del archivo plano.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If Not _mlogValidoNombrePlano Then
            A2Utilidades.Mensajes.mostrarMensaje("El nombre del archivo plano posee caractares no válidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

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
    ''' Buscar los datos del banco seleccionado en el encabezado y en el detalle de Tesoreria
    ''' </summary>
    ''' <param name="plngIdBanco">Codigo del banco el cual se va a realizar la búsqueda</param>
    ''' <remarks>SLB20130312</remarks>
    Friend Sub buscarBancos(Optional ByVal plngIdBanco As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo encargado de buscar los formatos asociados al banco seleccionado.
    ''' </summary>
    ''' <remarks>SLB20130529</remarks>
    Public Sub buscarFormatosBanco()
        Try
            IsBusy = True
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemEspecificoQuery("FormatosBancos", _ParametrosConsultaSelected.IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, "Formatos")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo encargado de levantar el pop up de la cuentas bancarias de los clientes.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130605</remarks>
    Public Sub CuentasBancariasClientes_Mostar(ByVal lo As OyDTesoreria.TesoreriaACHPendiente)
        Try
            TesoreriaSelected = lo
            cwCuentasBancarias = New cwCuentasBancariasClientes(LTrim(RTrim(_TesoreriaSelected.IDCliente)))
            AddHandler cwCuentasBancarias.Closed, AddressOf CerroVentanaCuentasBancariasClientes
            Program.Modal_OwnerMainWindowsPrincipal(cwCuentasBancarias)
            cwCuentasBancarias.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar un nuevo detalle de los Pendientes", _
                                 Me.ToString(), "OrdenesPendientesTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo encargado de recibir la respuesta del pop up del buildar de las cuentas bancarias de los clientes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>SLB20130605</remarks>
    Private Sub CerroVentanaCuentasBancariasClientes(sender As Object, e As EventArgs)
        Try
            If cwCuentasBancarias.DialogResult.Value Then
                If Not IsNothing(cwCuentasBancarias.CuentasClientesSelected) Then
                    _TesoreriaSelected.IDBanco = cwCuentasBancarias.CuentasClientesSelected.IdItem
                    _TesoreriaSelected.NombreBanco = cwCuentasBancarias.CuentasClientesSelected.CodItem
                    _TesoreriaSelected.NombreSucursal = cwCuentasBancarias.CuentasClientesSelected.Nombre
                    _TesoreriaSelected.Cuenta = cwCuentasBancarias.CuentasClientesSelected.InfoAdicional01
                    _TesoreriaSelected.TipoCuenta = cwCuentasBancarias.CuentasClientesSelected.Descripcion
                    _TesoreriaSelected.CodigoACH = cwCuentasBancarias.CuentasClientesSelected.EtiquetaIdItem
                    _TesoreriaSelected.Titular = cwCuentasBancarias.CuentasClientesSelected.Clasificacion
                    _TesoreriaSelected.TipoID = cwCuentasBancarias.CuentasClientesSelected.EtiquetaCodItem
                    _TesoreriaSelected.NroDctoCliente = cwCuentasBancarias.CuentasClientesSelected.CodigoAuxiliar
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana del las Cuentas Bancarias de los clientes", _
                     Me.ToString(), "CerroVentanaCuentasBancariasClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub NavegarArchivo(ByVal archivo As OyDImportaciones.Archivo)
    '    If Not IsNothing(archivo) Then
    '        Dim strNroVentana = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

    '        If (Application.Current.IsRunningOutOfBrowser) Then
    '            'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
    '            Dim button As New MyHyperlinkButton
    '            button.NavigateUri = New Uri(archivo.RutaWeb)
    '            button.TargetName = "vtnNva" & "00" & strNroVentana
    '            button.ClickMe()
    '        Else
    '            HtmlPage.Window.Navigate(New Uri(archivo.RutaWeb), "vtnNva" & "00" & strNroVentana, "height=550,width=750,top=25,left=25,toolbar=1,resizable=1")
    '        End If
    '    End If
    'End Sub

    Public Sub BorrarArchivo(ByVal archivo As OyDImportaciones.Archivo)
        If Not IsNothing(archivo) Then
            'If MessageBox.Show("¿Está seguro de borrar este archivo?", "Borrar", MessageBoxButton.OKCancel) Then
            dcArchivosProxy.BorrarArchivo(archivo.Nombre, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
            'End If
        End If
    End Sub

#End Region

#Region "Metodos Asincronicos"

    ''' <summary>
    ''' Método encargado de recibir la lista de las liquidaciones pendientes de Tesorería.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130418</remarks>
    Private Sub TerminoConsultarPendientesTesoreria(ByVal lo As LoadOperation(Of OyDTesoreria.TesoreriaACHPendiente))
        IsBusy = False
        If Not lo.HasError Then
            If dcProxy.TesoreriaACHPendientes.Count > 0 Then
                ListaTesoreriaGA = dcProxy.TesoreriaACHPendientes
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Pendientes de Tesorería.", _
                     Me.ToString(), "TerminoConsultarPendientesTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
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
    ''' Método que recibe la respuesta de la inserción del CE exportado.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130530</remarks>
    Private Sub TerminoInsertarRCExportados(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            intRegistrosInsetados = intRegistrosInsetados + 1
            GenerarPlano(True)
        Else
            dcProxy.BorrarRCExportados(strBorrar, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarRCExportados, "Borrar")
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el encabezado del CE.", _
                     Me.ToString(), "TerminoInsertarRCExportados", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    Private Sub TerminoBorrarRCExportados(ByVal lo As InvokeOperation(Of Boolean))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el encabezado del CE.", _
                     Me.ToString(), "TerminoInsertarRCExportados", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Metodo que recibe la respuesta de los registros incertados en las tablas temporales para generar el Archivo Plano ACH, 
    ''' Tambien se lanza la función GenerarArchivoPlanoACH que se encarga el archivo plano.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130605</remarks>
    Private Sub TerminoInsertarDatosPlanoACH(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            RutaArchivoPlano = RutaArchivoPlano & "." & ExtensionPlano

            IsBusy = True
            dcProxy.GenerarArchivoPlanoACH(_ParametrosConsultaSelected.Formato, _ParametrosConsultaSelected.IDBanco, strFormato, RutaArchivoPlano, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, _
                                           Program.Usuario, Program.HashConexion, AddressOf TerminoGenerarArchivoPlano, "GenerarPlano")
        Else
            dcProxy.BorrarRCExportados(strBorrar, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarRCExportados, "Borrar")
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el encabezado del CE.", _
                     Me.ToString(), "TerminoInsertarDatosPlanoACH", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Metodo que recibe la respuesta de la generación del archivo Plano ACH.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130605</remarks>
    Private Sub TerminoGenerarArchivoPlano(ByVal lo As InvokeOperation(Of Boolean))
        Try
            IsBusy = False
            If Not lo.HasError Then

                A2Utilidades.Mensajes.mostrarMensaje("El proceso generó " & CStr(intSecuencia) & " detalles para el archivo plano.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                ConsultarPendientesTesoreraia()
                RutaArchivoPlano = String.Empty
                dcArchivosProxy.Archivos.Clear()
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
            Else
                dcProxy.BorrarRCExportados(strBorrar, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarRCExportados, "Borrar")
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Generación de los Comprobantes", _
                                                 Me.ToString(), "TerminoGenerarComprobantes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Generación de los Comprobantes", _
                                                             Me.ToString(), "TerminoGenerarComprobantes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método recibe la respuesta del buscador generico.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>20130201</remarks>
    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If lo.UserState.ToString = "Formatos" Then
                    _ParametrosConsultaSelected.Formato = String.Empty
                    Dim ListaFormatos = lo.Entities.ToList
                    Dim objdicListaCombos = New List(Of OYDUtilidades.ItemCombo)

                    For Each objFormato In ListaFormatos
                        objdicListaCombos.Add(New OYDUtilidades.ItemCombo With {.intID = objFormato.Id, .ID = objFormato.IdItem,
                                                                             .Descripcion = objFormato.Nombre, .Categoria = "FormatosBancos"})
                    Next

                    dicListaCombos = objdicListaCombos
                    ConsultarPendientesTesoreraia()
                End If

                If lo.UserState.ToString = "Bancos" Then
                    _mlogBuscarBancos = False
                    If lo.Entities.ToList.Count > 0 Then
                        If lo.Entities.First.InfoAdicional02.Equals("1") Then
                            _ParametrosConsultaSelected.IDBanco = lo.Entities.First.IdItem
                            buscarFormatosBanco()
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ParametrosConsultaSelected.IDBanco = Nothing
                            _ParametrosConsultaSelected.Formato = String.Empty
                            dicListaCombos = Nothing
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        _ParametrosConsultaSelected.IDBanco = Nothing
                        _ParametrosConsultaSelected.Formato = String.Empty
                        dicListaCombos = Nothing
                    End If
                    _mlogBuscarBancos = True
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos", _
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(), _
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        If Not IsNothing(e.Error) Then
            MessageBox.Show(e.Error.Message)
            e.MarkErrorAsHandled()
        End If
        dcArchivosProxy.Archivos.Clear()
        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
    End Sub

    Private Sub TerminoTraerArchivos(ByVal lo As LoadOperation(Of OyDImportaciones.Archivo))
        Try
            If Not lo.HasError Then
                ListaArchivosGuardados = dcArchivosProxy.Archivos
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos", _
                                                 Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos", _
                                                             Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        ExtensionPlano = "txt"
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaTesoreriaGA As EntitySet(Of OyDTesoreria.TesoreriaACHPendiente)
    Public Property ListaTesoreriaGA As EntitySet(Of OyDTesoreria.TesoreriaACHPendiente)
        Get
            Return _ListaTesoreriaGA
        End Get
        Set(ByVal value As EntitySet(Of OyDTesoreria.TesoreriaACHPendiente))
            _ListaTesoreriaGA = value
            MyBase.CambioItem("ListaTesoreriaGA")
            MyBase.CambioItem("ListaTesoreriaGAPaged")
        End Set
    End Property

    Private _TesoreriaSelected As OyDTesoreria.TesoreriaACHPendiente
    Public Property TesoreriaSelected As OyDTesoreria.TesoreriaACHPendiente
        Get
            Return _TesoreriaSelected
        End Get
        Set(ByVal value As OyDTesoreria.TesoreriaACHPendiente)
            _TesoreriaSelected = value
            MyBase.CambioItem("TesoreriaSelected")
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

    Private WithEvents _ParametrosConsultaSelected As New ParametrosConsultaACH
    Public Property ParametrosConsultaSelected As ParametrosConsultaACH
        Get
            Return _ParametrosConsultaSelected
        End Get
        Set(ByVal value As ParametrosConsultaACH)
            _ParametrosConsultaSelected = value
            MyBase.CambioItem("ParametrosConsultaSelected")
        End Set
    End Property

    Private _dicListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
    Public Property dicListaCombos As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _dicListaCombos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _dicListaCombos = value
            MyBase.CambioItem("dicListaCombos")
        End Set
    End Property

    Public _ListaArchivosGuardados As EntitySet(Of OyDImportaciones.Archivo)
    Public Property ListaArchivosGuardados As EntitySet(Of OyDImportaciones.Archivo)
        Get
            Return _ListaArchivosGuardados
        End Get
        Set(ByVal value As EntitySet(Of OyDImportaciones.Archivo))
            _ListaArchivosGuardados = value
            CambioItem("ListaArchivosGuardados")
            CambioItem("ListaArchivosGuardadosPaged")
        End Set
    End Property

    Private _RutaArchivoPlano As String
    Public Property RutaArchivoPlano As String
        Get
            Return _RutaArchivoPlano
        End Get
        Set(ByVal value As String)
            _RutaArchivoPlano = value
            If Not String.IsNullOrEmpty(_RutaArchivoPlano) Then
                If Not Regex.IsMatch(_RutaArchivoPlano, "^[a-z A-ZÑ 0-9 á-ú ._-]*$") Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nombre del archivo plano posee caractares no válidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _mlogValidoNombrePlano = False
                Else
                    _mlogValidoNombrePlano = True
                End If
            End If
            CambioItem("RutaArchivoPlano")
        End Set
    End Property

    Private _ExtensionPlano As String
    Public Property ExtensionPlano As String
        Get
            Return _ExtensionPlano
        End Get
        Set(ByVal value As String)
            _ExtensionPlano = value
            CambioItem("ExtensionPlano")
        End Set
    End Property

#End Region

#Region "PropertyChanged"

    Private Sub _ParametrosConsultaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ParametrosConsultaSelected.PropertyChanged
        Try
            If e.PropertyName = "IDBanco" And _mlogBuscarBancos Then
                If Not IsNothing(_ParametrosConsultaSelected.IDBanco) Then
                    buscarBancos(_ParametrosConsultaSelected.IDBanco, "Bancos")
                Else
                    ConsultarPendientesTesoreraia()
                End If
            End If

            If sw = 0 Then
                Select Case e.PropertyName
                    Case "Consecutivo"
                        If Not _ParametrosConsultaSelected.Consecutivo Then
                            sw = 1
                            _ParametrosConsultaSelected.NombreConsecutivo = Nothing
                        End If
                        ConsultarPendientesTesoreraia()
                    Case "NombreConsecutivo"
                        If _ParametrosConsultaSelected.Consecutivo Then
                            ConsultarPendientesTesoreraia()
                        End If
                    Case "Numero"
                        If Not _ParametrosConsultaSelected.Numero Then
                            sw = 1
                            _ParametrosConsultaSelected.NroDocumento = Nothing
                        End If
                        ConsultarPendientesTesoreraia()
                    Case "NroDocumento"
                        If _ParametrosConsultaSelected.Numero Then
                            ConsultarPendientesTesoreraia()
                        End If
                    Case "Elaboracion", "Desde", "Hasta"
                        ConsultarPendientesTesoreraia()
                    Case "Formato"
                        If Not String.IsNullOrEmpty(_ParametrosConsultaSelected.Formato) Then
                            ConsultarPendientesTesoreraia()
                        End If
                End Select
            Else
                sw = 0
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region



End Class

''' <summary>
''' Clase para el manejo de los parametros de consulta de los CE pendientes.
''' </summary>
''' <remarks>SLB20130528</remarks>
Public Class ParametrosConsultaACH
    Implements INotifyPropertyChanged

    Private _Elaboracion As Boolean = True
    Public Property Elaboracion As Boolean
        Get
            Return _Elaboracion
        End Get
        Set(ByVal value As Boolean)
            _Elaboracion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Elaboracion"))
        End Set
    End Property

    Private _Desde As Date = Now.Date
    Public Property Desde As Date
        Get
            Return _Desde
        End Get
        Set(ByVal value As Date)
            _Desde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Desde"))
        End Set
    End Property

    Private _Hasta As Date = Now.Date
    Public Property Hasta As Date
        Get
            Return _Hasta
        End Get
        Set(ByVal value As Date)
            _Hasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Hasta"))
        End Set
    End Property

    Private _Numero As Boolean = False
    Public Property Numero As Boolean
        Get
            Return _Numero
        End Get
        Set(ByVal value As Boolean)
            _Numero = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Numero"))
        End Set
    End Property

    Private _NroDocumento As Integer? = Nothing
    Public Property NroDocumento As Integer?
        Get
            Return _NroDocumento
        End Get
        Set(ByVal value As Integer?)
            _NroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroDocumento"))
        End Set
    End Property

    Private _IDBanco As Integer? = Nothing
    Public Property IDBanco As Integer?
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Integer?)
            _IDBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))
        End Set
    End Property

    Private _Formato As String = String.Empty
    Public Property Formato As String
        Get
            Return _Formato
        End Get
        Set(ByVal value As String)
            _Formato = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Formato"))
        End Set
    End Property

    Private _Consecutivo As Boolean = False
    Public Property Consecutivo As Boolean
        Get
            Return _Consecutivo
        End Get
        Set(ByVal value As Boolean)
            _Consecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Consecutivo"))
        End Set
    End Property

    Private _NombreConsecutivo As String
    Public Property NombreConsecutivo As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class

