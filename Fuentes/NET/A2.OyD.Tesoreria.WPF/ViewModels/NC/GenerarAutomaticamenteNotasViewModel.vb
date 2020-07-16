Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class GenerarAutomaticamenteViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2Utilidades.Mensajes

Public Class GenerarAutomaticamenteNotasViewModel
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

                Dim NewValores As New ConfiguracionNota

                NewValores.NombreConsecutivo = String.Empty
                NewValores.FechaDocumento = Date.Now
                NewValores.RegistroNinguno = String.Empty
                NewValores.ContraParteNinguno = String.Empty
                NewValores.LogRegistroBanco = False
                NewValores.LogContraParteBanco = False
                NewValores.LogRegistroCliente = False
                NewValores.LogContraParteCliente = False
                NewValores.RegistroCliente = String.Empty
                NewValores.ContraParteCliente = String.Empty
                NewValores.RegistroBanco = String.Empty
                NewValores.ContraParteBanco = String.Empty
                NewValores.RegistroCuentaContable = String.Empty
                NewValores.ContraParteCuentaContable = String.Empty
                NewValores.RegistroCCostos = String.Empty
                NewValores.ContraParteCCostos = String.Empty
                NewValores.HabilitarRegistroBanco = True
                NewValores.HabilitarContraParteBanco = True
                NewValores.HabilitarRegistroCliente = True
                NewValores.HabilitarContraParteCliente = True

                ValoresSelected = NewValores

                'JCM20160216
                If Not IsNothing(objProxy.ItemCombos) Then
                    objProxy.ItemCombos.Clear()
                End If
                objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_Notas", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, Nothing)

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
    Public Sub GenerarNC()
        Try
            If IsNothing(_ValoresSelected.FechaDocumento) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la fecha de la nota contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_ValoresSelected.NombreConsecutivo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un consecutivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Validaciones() Then
                IsBusy = False
                mostrarMensajePregunta("¿ Desea generar la nota contable ?",
                                   Program.TituloSistema,
                                   "GENERACIONNOTAS",
                                   AddressOf TerminoPregunta, True, "¿Generar?")
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
            GuardarNC()
        End If
    End Sub

    ''' <summary>
    ''' Método encargado de Guardar los CE.
    ''' </summary>
    ''' <param name="GraboEncabezado"></param>
    ''' <param name="GraboDetalle"></param>
    ''' <remarks>SLB20130422</remarks>
    Private Sub GuardarNC(Optional GraboEncabezado As Boolean = False, Optional GraboDetalle As Boolean = False)
        Try

            If ListaResultadoGeneAutoNotas.Where(Function(i) i.Seleccionado).Count > 0 Then

                Dim strXMLDetTesoreria As String

                strXMLDetTesoreria = "<NotasMasivas>  "

                For Each objDet In ListaResultadoGeneAutoNotas.Where(Function(i) i.Seleccionado = True)
                    Dim strXMLDetalle = <Detalle intIdDetalle=<%= objDet.intId %>
                                            lngIdComitente=<%= objDet.lngIDComitente %>
                                            strDetalle=<%= objDet.strDetalle %>
                                            curValor=<%= objDet.curTotalLiq %>
                                            strNaturaleza=<%= _ParametrosConsultaSelected.Naturaleza %>>
                                        </Detalle>

                    strXMLDetTesoreria = strXMLDetTesoreria & strXMLDetalle.ToString
                Next

                strXMLDetTesoreria = strXMLDetTesoreria & " </NotasMasivas>"

                Dim strXMLDetTesoreriaSeguro As String = System.Web.HttpUtility.HtmlEncode(strXMLDetTesoreria)

                IsBusy = True

                If Not IsNothing(dcProxy.RespuestaProcesosGenericosConfirmacions) Then
                    dcProxy.RespuestaProcesosGenericosConfirmacions.Clear()
                End If

                dcProxy.Load(dcProxy.ResultadoGeneracionAutomaticaNotasQuery(strXMLDetTesoreriaSeguro, _ValoresSelected.NombreConsecutivo, _ValoresSelected.FechaDocumento,
                                                                _ValoresSelected.RegistroCliente, _ValoresSelected.ContraParteCliente, _ValoresSelected.RegistroBanco,
                                                                _ValoresSelected.ContraParteBanco, _ValoresSelected.RegistroCuentaContable, _ValoresSelected.ContraParteCuentaContable,
                                                                _ValoresSelected.RegistroCCostos, _ValoresSelected.ContraParteCCostos, IIf(_ValoresSelected.RegistroNinguno = "0", True, False),
                                                                IIf(_ValoresSelected.ContraParteNinguno = "0", True, False), _ValoresSelected.LogRegistroBanco, _ValoresSelected.LogContraParteBanco, _ValoresSelected.LogRegistroCliente,
                                                                _ValoresSelected.LogContraParteCliente, Program.Usuario, Program.HashConexion), AddressOf TerminoGuardarNC, "Guadar")

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
            dcProxy.ResultadoGeneAutoNotas.Clear()
            dcProxy.Load(dcProxy.GeneracionAutomaticaNotasConsultarQuery(_ParametrosConsultaSelected.Operaciones, _ParametrosConsultaSelected.Clase,
                                                                         _ParametrosConsultaSelected.Naturaleza, _ParametrosConsultaSelected.TipoOperacion,
                                                                         _ParametrosConsultaSelected.Fecha, _ParametrosConsultaSelected.FechaDe, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarPendientesTesoreria, strAccion)

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
        If IsNothing(_ListaResultadoGeneAutoNotas) Then
            A2Utilidades.Mensajes.mostrarMensaje("No existen detalles para la generación de la nota. Por favor haga la respectiva consulta. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        Else
            If _ListaResultadoGeneAutoNotas.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No existen detalles para la generación de la nota. Por favor haga la respectiva consulta. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If
        End If

        If _ListaResultadoGeneAutoNotas.Count > 0 Then
            If _ListaResultadoGeneAutoNotas.Where(Function(i) i.Seleccionado = True).Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No existen registros seleccionados para realizar la nota de tesorería. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If
        End If

        If _ListaResultadoGeneAutoNotas.Count > 0 Then
            If String.IsNullOrEmpty(_ValoresSelected.RegistroBanco) And _ValoresSelected.LogRegistroBanco Then
                A2Utilidades.Mensajes.mostrarMensaje("Para la generación de la nota debe seleccionar un banco en el primer registro. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Validaciones = False
                Return Validaciones
            End If

        End If

        If String.IsNullOrEmpty(_ValoresSelected.ContraParteBanco) And _ValoresSelected.LogContraParteBanco Then
            A2Utilidades.Mensajes.mostrarMensaje("Para la generación de la nota debe de seleccionar un banco en la contraparte. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_ValoresSelected.RegistroCuentaContable) Then
            A2Utilidades.Mensajes.mostrarMensaje("Para la generación de la nota contable debe de seleccionar la cuenta contable. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(_ValoresSelected.ContraParteCuentaContable) Then
            A2Utilidades.Mensajes.mostrarMensaje("Para la generación de la nota debe de seleccionar la cuenta contable contraparte. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

    End Function

    Public Sub ModificacionParametros()
        Try
            Select Case ValoresSelected.RegistroNinguno
                Case "0" 'Ninguno
                    ValoresSelected.HabilitarRegistroBanco = False
                    ValoresSelected.HabilitarRegistroCliente = False
                    ValoresSelected.RegistroCliente = String.Empty
                    ValoresSelected.RegistroBanco = Nothing
                    ValoresSelected.LogRegistroBanco = False
                    ValoresSelected.LogRegistroCliente = False
                Case "1" 'Banco
                    ValoresSelected.HabilitarRegistroBanco = True
                    ValoresSelected.HabilitarRegistroCliente = False
                    ValoresSelected.RegistroCliente = String.Empty
                    ValoresSelected.LogRegistroBanco = True
                    ValoresSelected.LogRegistroCliente = False
                Case "2" 'Cliente
                    ValoresSelected.HabilitarRegistroBanco = False
                    ValoresSelected.HabilitarRegistroCliente = True
                    ValoresSelected.RegistroBanco = String.Empty
                    ValoresSelected.LogRegistroBanco = False
                    ValoresSelected.LogRegistroCliente = True
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ModificacionParametrosContraParte()
        Try
            Select Case ValoresSelected.ContraParteNinguno
                Case "0" 'Ninguno
                    ValoresSelected.HabilitarContraParteBanco = False
                    ValoresSelected.HabilitarContraParteCliente = False
                    ValoresSelected.ContraParteCliente = String.Empty
                    ValoresSelected.ContraParteBanco = Nothing
                    ValoresSelected.LogContraParteBanco = False
                    ValoresSelected.LogContraParteCliente = False
                Case "1" 'Banco
                    ValoresSelected.HabilitarContraParteBanco = True
                    ValoresSelected.HabilitarContraParteCliente = False
                    ValoresSelected.ContraParteCliente = String.Empty
                    ValoresSelected.LogContraParteBanco = True
                    ValoresSelected.LogContraParteCliente = False
                Case "2" 'Cliente
                    ValoresSelected.HabilitarContraParteBanco = False
                    ValoresSelected.HabilitarContraParteCliente = True
                    ValoresSelected.ContraParteBanco = String.Empty
                    ValoresSelected.LogContraParteBanco = False
                    ValoresSelected.LogContraParteCliente = True
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_DetalleTesoreriSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodosChecked(ByVal pbolCheked As System.Nullable(Of Boolean))
        Try
            If Not pbolCheked Is Nothing Then
                If Not IsNothing(_ListaResultadoGeneAutoNotas) Then
                    For Each Lista In _ListaResultadoGeneAutoNotas
                        Lista.Seleccionado = pbolCheked
                    Next
                End If
            Else
                SeleccionarTodos = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar los registros",
                                 Me.ToString(), "SeleccionarTodosChecked", Application.Current.ToString(), Program.Maquina, ex)
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
            If ValoresSelected.LogRegistroBanco Then
                objProxy.Load(objProxy.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "cuentasbancariasRegistro")
            End If

            If ValoresSelected.LogContraParteBanco Then
                objProxy.Load(objProxy.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "cuentasbancariasContraParte")
            End If

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
            If ValoresSelected.RegistroCCostos Then
                objProxy.Load(objProxy.buscarItemEspecificoQuery("CentrosCosto", pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "CentrosCostoRegistro")
            End If

            If ValoresSelected.ContraParteCCostos Then
                objProxy.Load(objProxy.buscarItemEspecificoQuery("CentrosCosto", pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "CentrosCostoContraParte")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


    'JMC20160216
    Public Sub LimpiarBanco(Optional ByVal pstrTopico As String = "")
        If pstrTopico = "cuentasbancariasRegistro" Then
            _ValoresSelected.RegistroBanco = Nothing
        End If

        If pstrTopico = "cuentasbancariasContraParte" Then
            _ValoresSelected.ContraParteBanco = Nothing
        End If

        strCompaniaBanco = String.Empty
    End Sub

    Public Sub ControlSeleccionado()
        Select Case _ParametrosConsultaSelected.Operaciones
            Case "CONVENIDAS"
                _ValoresSelected.RegistroNinguno = "1"
                ModificacionParametros()

                _ValoresSelected.ContraParteNinguno = "0"
                ModificacionParametrosContraParte()

            Case "CRUZADAS"
                _ValoresSelected.RegistroNinguno = "1"
                ModificacionParametros()

                _ValoresSelected.ContraParteNinguno = "2"
                ModificacionParametrosContraParte()

            Case "OTRAFIRMA"
                _ValoresSelected.RegistroNinguno = "1"
                ModificacionParametros()

                _ValoresSelected.ContraParteNinguno = "2"
                ModificacionParametrosContraParte()

            Case "DIVISAS"
                _ValoresSelected.RegistroNinguno = "1"
                ModificacionParametros()

                _ValoresSelected.ContraParteNinguno = "2"
                ModificacionParametrosContraParte()

            Case "DIARIA"
                _ValoresSelected.RegistroNinguno = "1"
                ModificacionParametros()

                _ValoresSelected.ContraParteNinguno = "0"
                ModificacionParametrosContraParte()

        End Select
    End Sub


#End Region

#Region "MetodosAsincronicos"

    ''' <summary>
    ''' Método encargado de recibir la lista de las liquidaciones pendientes de Tesorería.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130418</remarks>
    Private Sub TerminoConsultarPendientesTesoreria(ByVal lo As LoadOperation(Of OyDTesoreria.ResultadoGeneAutoNotas))
        IsBusy = False
        If Not lo.HasError Then
            If dcProxy.ResultadoGeneAutoNotas.Count > 0 Then
                ListaResultadoGeneAutoNotas = lo.Entities.ToList
                ControlSeleccionado()
            Else
                If Not lo.UserState = "Grabar" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No hay operaciones para crear Notas contables con los parámetros especificados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
            SeleccionarTodos = False
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los pendientes de tesorería.",
                     Me.ToString(), "TerminoConsultarPendientesTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

    ''' <summary>
    ''' Método que recibe la respuesta de la inserción del encabezado del CE
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130422</remarks>
    Private Sub TerminoGuardarNC(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        IsBusy = False
        If Not lo.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(lo.Entities.FirstOrDefault.strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

            Dim objlitasTemoporal As New List(Of ResultadoGeneAutoNotas)

            For Each objLista In ListaResultadoGeneAutoNotas
                If objLista.Seleccionado = False Then
                    objlitasTemoporal.Add(objLista)

                End If
            Next

            ListaResultadoGeneAutoNotas = objlitasTemoporal

            SeleccionarTodos = False
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar la nota de tesorería. ",
                 Me.ToString(), "TerminoInsertarCE", Application.Current.ToString(), Program.Maquina, lo.Error)
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
                    listConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = "ConsecutivosTesoreriaNotas").ToList
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False
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
                    Case "cuentasbancariasRegistro"
                        If lo.Entities.ToList.Count > 0 Then
                            '------------------------------------------------------------------------------------------------------------------------------------------------
                            'Validar si la compañia del consecutivo es la misma compañia del banco
                            'ID caso de prueba:  CP001
                            '------------------------------------------------------------------------------------------------------------------------------------------------
                            'JCM20160216
                            If Not IsNothing(_ValoresSelected.NombreConsecutivo) Then
                                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _ValoresSelected.NombreConsecutivo Select c).FirstOrDefault.Retorno

                                If lo.Entities.First.InfoAdicional02.Equals("1") Then
                                    If lo.Entities.First.InfoAdicional03.Equals(strCompaniaConsecutivo) Then
                                        _ValoresSelected.RegistroBanco = lo.Entities.First.IdItem
                                    Else
                                        '------------------------------------------------------------------------------------------------------------------------------------------------
                                        'L acompañia del banco no coincide con la compañia del consecutivo
                                        'ID caso de prueba:  CP002
                                        '------------------------------------------------------------------------------------------------------------------------------------------------
                                        A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra asociado a una compañía diferente a la del consecutivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        LimpiarBanco("cuentasbancariasRegistro")
                                    End If
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    LimpiarBanco("cuentasbancariasRegistro")
                                End If
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            LimpiarBanco("cuentasbancariasRegistro")
                        End If
                    Case "cuentasbancariasContraParte"
                        If lo.Entities.Count > 0 Then
                            If Not IsNothing(_ValoresSelected.NombreConsecutivo) Then
                                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _ValoresSelected.NombreConsecutivo Select c).FirstOrDefault.Retorno

                                If lo.Entities.First.InfoAdicional02.Equals("1") Then
                                    If lo.Entities.First.InfoAdicional03.Equals(strCompaniaConsecutivo) Then
                                        _ValoresSelected.ContraParteBanco = lo.Entities.First.IdItem
                                    Else
                                        '------------------------------------------------------------------------------------------------------------------------------------------------
                                        'L acompañia del banco no coincide con la compañia del consecutivo
                                        'ID caso de prueba:  CP002
                                        '------------------------------------------------------------------------------------------------------------------------------------------------
                                        A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra asociado a una compañía diferente a la del consecutivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        LimpiarBanco("cuentasbancariasContraParte")
                                    End If
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    LimpiarBanco("cuentasbancariasContraParte")
                                End If
                            End If
                        End If
                    Case "CentrosCostoRegistro"
                        If lo.Entities.ToList.Count > 0 Then
                            _ValoresSelected.RegistroCCostos = lo.Entities.First.IdItem
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El centro de costos ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ValoresSelected.RegistroCCostos = ""
                        End If
                    Case "CentrosCostoContraParte"
                        If lo.Entities.ToList.Count > 0 Then
                            _ValoresSelected.ContraParteCCostos = lo.Entities.First.IdItem
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El centro de costos ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _ValoresSelected.ContraParteCCostos = ""
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

    Private WithEvents _ValoresSelected As ConfiguracionNota
    Public Property ValoresSelected As ConfiguracionNota
        Get
            Return _ValoresSelected
        End Get
        Set(ByVal value As ConfiguracionNota)
            _ValoresSelected = value
            MyBase.CambioItem("ValoresSelected")
        End Set
    End Property

    Private _SeleccionarTodos As System.Nullable(Of Boolean)
    Public Property SeleccionarTodos() As System.Nullable(Of Boolean)
        Get
            If _SeleccionarTodos Is Nothing Then
                _SeleccionarTodos = False
            End If
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As System.Nullable(Of Boolean))
            _SeleccionarTodos = value
            SeleccionarTodosChecked(_SeleccionarTodos)
            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _ListaResultadoGeneAutoNotas As List(Of ResultadoGeneAutoNotas)
    Public Property ListaResultadoGeneAutoNotas As List(Of ResultadoGeneAutoNotas)
        Get
            Return _ListaResultadoGeneAutoNotas
        End Get
        Set(ByVal value As List(Of ResultadoGeneAutoNotas))
            _ListaResultadoGeneAutoNotas = value
            MyBase.CambioItem("ListaResultadoGeneAutoNotas")
            MyBase.CambioItem("ListaResultadoGeneAutoNotasPaged")
        End Set
    End Property
    Private _ResultadoGeneAutoNotasSelected As ResultadoGeneAutoNotas
    Public Property ResultadoGeneAutoNotasSelected() As ResultadoGeneAutoNotas
        Get
            Return _ResultadoGeneAutoNotasSelected
        End Get
        Set(ByVal value As ResultadoGeneAutoNotas)
            _ResultadoGeneAutoNotasSelected = value
            MyBase.CambioItem("ResultadoGeneAutoNotasSelected")
        End Set
    End Property


    Public ReadOnly Property ListaResultadoGeneAutoNotasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultadoGeneAutoNotas) Then
                Dim view = New PagedCollectionView(_ListaResultadoGeneAutoNotas)
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

    Private _ParametrosConsultaSelected As New ParametrosConsultaNC
    Public Property ParametrosConsultaSelected As ParametrosConsultaNC
        Get
            Return _ParametrosConsultaSelected
        End Get
        Set(ByVal value As ParametrosConsultaNC)
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


    Private Sub _ValoresSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _ValoresSelected.PropertyChanged
        If e.PropertyName = "NombreConsecutivo" Then
            If Not IsNothing(_ValoresSelected.NombreConsecutivo) Then
                strCompaniaConsecutivo = (From c In listConsecutivos Where c.ID = _ValoresSelected.NombreConsecutivo Select c).FirstOrDefault.Retorno
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
Public Class ParametrosConsultaNC
    Implements INotifyPropertyChanged

    Private _Operaciones As String = "CRUZADAS"
    Public Property Operaciones() As String
        Get
            Return _Operaciones
        End Get
        Set(ByVal value As String)
            _Operaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Operaciones"))
        End Set
    End Property

    Private _Clase As String = "A"
    Public Property Clase As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    Private _TipoOperacion As String = "C"
    Public Property TipoOperacion As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _Naturaleza As String = "NC"
    Public Property Naturaleza() As String
        Get
            Return _Naturaleza
        End Get
        Set(ByVal value As String)
            _Naturaleza = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Naturaleza"))
        End Set
    End Property

    Private _Fecha As DateTime = Date.Now
    Public Property Fecha() As DateTime
        Get
            Return _Fecha
        End Get
        Set(ByVal value As DateTime)
            _Fecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fecha"))
        End Set
    End Property

    Private _FechaDe As String = "C"
    Public Property FechaDe() As String
        Get
            Return _FechaDe
        End Get
        Set(ByVal value As String)
            _FechaDe = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaDe"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class


Public Class ConfiguracionNota
    Implements INotifyPropertyChanged

    Private _NombreConsecutivo As String = String.Empty
    Public Property NombreConsecutivo() As String
        Get
            Return _NombreConsecutivo
        End Get
        Set(ByVal value As String)
            _NombreConsecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreConsecutivo"))

        End Set
    End Property

    Private _FechaDocumento As DateTime
    Public Property FechaDocumento() As DateTime
        Get
            Return _FechaDocumento
        End Get
        Set(ByVal value As DateTime)
            _FechaDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaDocumento"))

        End Set
    End Property

    Private _RegistroNinguno As String
    Public Property RegistroNinguno() As String
        Get
            Return _RegistroNinguno
        End Get
        Set(ByVal value As String)
            _RegistroNinguno = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistroNinguno"))
        End Set
    End Property

    Private _ContraParteNinguno As String
    Public Property ContraParteNinguno() As String
        Get
            Return _ContraParteNinguno
        End Get
        Set(ByVal value As String)
            _ContraParteNinguno = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ContraParteNinguno"))
        End Set
    End Property

    Private _RegistroBanco As String = String.Empty
    Public Property RegistroBanco() As String
        Get
            Return _RegistroBanco
        End Get
        Set(ByVal value As String)
            _RegistroBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistroBanco"))
        End Set
    End Property

    Private _ContraParteBanco As String = String.Empty
    Public Property ContraParteBanco() As String
        Get
            Return _ContraParteBanco
        End Get
        Set(ByVal value As String)
            _ContraParteBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ContraParteBanco"))

        End Set
    End Property

    Private _RegistroCliente As String = String.Empty
    Public Property RegistroCliente() As String
        Get
            Return _RegistroCliente
        End Get
        Set(ByVal value As String)
            _RegistroCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistroCliente"))

        End Set
    End Property

    Private _ContraParteCliente As String = String.Empty
    Public Property ContraParteCliente() As String
        Get
            Return _ContraParteCliente
        End Get
        Set(ByVal value As String)
            _ContraParteCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ContraParteCliente"))
        End Set
    End Property

    Private _RegistroCuentaContable As String = String.Empty
    Public Property RegistroCuentaContable() As String
        Get
            Return _RegistroCuentaContable
        End Get
        Set(ByVal value As String)
            _RegistroCuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistroCuentaContable"))

        End Set
    End Property

    Private _ContraParteCuentaContable As String = String.Empty
    Public Property ContraParteCuentaContable() As String
        Get
            Return _ContraParteCuentaContable
        End Get
        Set(ByVal value As String)
            _ContraParteCuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ContraParteCuentaContable"))

        End Set
    End Property

    Private _RegistroCCostos As String = String.Empty
    Public Property RegistroCCostos() As String
        Get
            Return _RegistroCCostos
        End Get
        Set(ByVal value As String)
            _RegistroCCostos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RegistroCCostos"))
        End Set
    End Property

    Private _ContraParteCCostos As String = String.Empty
    Public Property ContraParteCCostos() As String
        Get
            Return _ContraParteCCostos
        End Get
        Set(ByVal value As String)
            _ContraParteCCostos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ContraParteCCostos"))
        End Set
    End Property

    Private _LogRegistroBanco As Boolean = False
    Public Property LogRegistroBanco() As Boolean
        Get
            Return _LogRegistroBanco
        End Get
        Set(ByVal value As Boolean)
            _LogRegistroBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LogRegistroBanco"))
        End Set
    End Property

    Private _LogContraParteBanco As Boolean = False
    Public Property LogContraParteBanco() As Boolean
        Get
            Return _LogContraParteBanco
        End Get
        Set(ByVal value As Boolean)
            _LogContraParteBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LogContraParteBanco"))
        End Set
    End Property

    Private _LogRegistroCliente As Boolean = False
    Public Property LogRegistroCliente() As Boolean
        Get
            Return _LogRegistroCliente
        End Get
        Set(ByVal value As Boolean)
            _LogRegistroCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LogRegistroCliente"))
        End Set
    End Property

    Private _LogContraParteCliente As Boolean = False
    Public Property LogContraParteCliente() As Boolean
        Get
            Return _LogContraParteCliente
        End Get
        Set(ByVal value As Boolean)
            _LogContraParteCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LogContraParteCliente"))
        End Set
    End Property

    Private _HabilitarRegistroBanco As Boolean = True
    Public Property HabilitarRegistroBanco As Boolean
        Get
            Return _HabilitarRegistroBanco
        End Get
        Set(ByVal value As Boolean)
            _HabilitarRegistroBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarRegistroBanco"))
        End Set
    End Property

    Private _HabilitarContraParteBanco As Boolean = True
    Public Property HabilitarContraParteBanco As Boolean
        Get
            Return _HabilitarContraParteBanco
        End Get
        Set(ByVal value As Boolean)
            _HabilitarContraParteBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarContraParteBanco"))
        End Set
    End Property

    Private _HabilitarRegistroCliente As Boolean = True
    Public Property HabilitarRegistroCliente As Boolean
        Get
            Return _HabilitarRegistroCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarRegistroCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarRegistroCliente"))
        End Set
    End Property

    Private _HabilitarContraParteCliente As Boolean = True
    Public Property HabilitarContraParteCliente As Boolean
        Get
            Return _HabilitarContraParteCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarContraParteCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarContraParteCliente"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Sub New()

    End Sub
End Class
