Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class GenerarOrdenDesdeLiquidacionViewModel.vb
'Propiedad de Alcuadrado S.A. 2012
Imports A2.OyD.Control.WPF
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2Utilidades.Mensajes

Public Class GenerarOrdenDesdeLiquidacionViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As BolsaDomainContext
    Dim dcProxy1 As BolsaDomainContext
    Public Property SelectedParametrosSeleccion As New ParametrosSeleccion
    Public Property ListaLiquidacionesGapOrdenesEscogidas As New List(Of LiquidacionesGapOrdenes)
    Public Property ListaLiquidacionesGapOrdenesGrabar As New List(Of LiquidacionesGapOrdenes)
    Public ValidarMaestroLista() As String
    Public ValidarInfoL() As String
    Dim NroRegistros As Integer
    Dim Mensaje As String
    Dim NroAccionesGen As Integer
    Dim NroRentaGen As Integer
    Dim MostrarResultado As ResultadoGenerarOrdenLiqui

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
            dcProxy1 = New BolsaDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                SelectedParametrosSeleccion.EspeciesS = True
                SelectedParametrosSeleccion.ClientesS = True
                'Lista.Add(1, "Ambos")
                'Lista.Add(2, "Venta")
                SelectedParametrosSeleccion.ListaClase.Add(New ListaCombo With {.ID = 1, .Descripcion = "Acciones"})
                SelectedParametrosSeleccion.ListaClase.Add(New ListaCombo With {.ID = 2, .Descripcion = "Renta Fija"})
                SelectedParametrosSeleccion.ListaClase.Add(New ListaCombo With {.ID = 3, .Descripcion = "Ambos"})

                SelectedParametrosSeleccion.ListaTipo.Add(New ListaCombo With {.ID = 1, .Descripcion = "Compra"})
                SelectedParametrosSeleccion.ListaTipo.Add(New ListaCombo With {.ID = 2, .Descripcion = "Venta"})
                SelectedParametrosSeleccion.ListaTipo.Add(New ListaCombo With {.ID = 3, .Descripcion = "Ambos"})
                dcProxy.Load(dcProxy.ConsultarLiquidacionesGapOrdenesQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesGapOrdenes, "FiltroInicial")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"

    Public Sub SeleccionarPorParametros()
        Try
            Dim Tipo As String
            Dim Clase As String
            Select Case SelectedParametrosSeleccion.tipo
                Case 1
                    Tipo = "C"
                Case 2
                    Tipo = "V"
                Case 3
                    Tipo = "T"
            End Select
            Select Case SelectedParametrosSeleccion.clase
                Case 1
                    Clase = "A"
                Case 2
                    Clase = "C"
                Case 3
                    Clase = "T"
            End Select
            '/******************************************************************************************
            '/* INICIO DOCUMENTO
            '/* Procedimiento:  Validar_Registros
            '/* Alcance     :   Public
            '/* Descripción :   Valida los registros que cumplan con las condicionesw seleccionadas
            '/* Parametros  :   Posicion del registro a validar
            '/* Retorno     :   Retorna Si o No si este cumple con las condiciones
            '/* FIN DOCUMENTO
            '/******************************************************************************************
            For Each led In ListaLiquidacionesGapOrdenes

                Dim Validar_Registros As Boolean = False
                If (Clase = "A") And (led.ClaseOrden = "A") Then
                    Validar_Registros = True
                Else
                    If (Clase = "C") And (led.ClaseOrden = "C") Then
                        Validar_Registros = True
                    Else
                        If (Clase = "T") Then
                            Validar_Registros = True
                        End If
                    End If
                End If

                If (Validar_Registros = True) Then
                    If (Tipo = "C") And ((led.Tipo = "C") Or (led.Tipo = "R")) Then
                        Validar_Registros = True
                    ElseIf (Tipo = "V") And ((led.Tipo = "V") Or (led.Tipo = "S")) Then
                        Validar_Registros = True
                    ElseIf (Tipo = "T") Then
                        Validar_Registros = True
                    Else
                        Validar_Registros = False
                    End If
                End If

                If (Validar_Registros = True) Then
                    If Not (SelectedParametrosSeleccion.EspeciesS = True) And (SelectedParametrosSeleccion.Especie = led.Especie) Then
                        Validar_Registros = True
                    ElseIf (SelectedParametrosSeleccion.EspeciesS = False) And ((SelectedParametrosSeleccion.Especie <> led.Especie) And SelectedParametrosSeleccion.Especie <> "") Then
                        Validar_Registros = False
                    ElseIf (SelectedParametrosSeleccion.EspeciesS = True) Then
                        Validar_Registros = True
                        '        Else
                        '              Validar_Registros = False
                    End If
                End If

                If (Validar_Registros = True) Then
                    If Not (SelectedParametrosSeleccion.ClientesS = True) And (SelectedParametrosSeleccion.CodigoCliente = led.Comitente) Then
                        Validar_Registros = True
                    ElseIf (SelectedParametrosSeleccion.ClientesS = False) And ((SelectedParametrosSeleccion.CodigoCliente <> led.Comitente) And SelectedParametrosSeleccion.CodigoCliente <> "") Then
                        Validar_Registros = False
                    ElseIf (SelectedParametrosSeleccion.ClientesS = True) Then
                        Validar_Registros = True
                        '        Else
                        '            Validar_Registros = False
                    End If
                End If

                If Validar_Registros Then
                    led.Activo = True
                Else
                    led.Activo = False
                End If

            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Seleccion de los registros", _
                 Me.ToString(), "SeleccionarPorParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodos()
        For Each led In ListaLiquidacionesGapOrdenes
            led.Activo = True
        Next
    End Sub

    Public Sub DesseleccionarTodos()
        For Each led In ListaLiquidacionesGapOrdenes
            led.Activo = False
        Next
    End Sub

    Public Sub GenerarOrden()
        Dim HaySeleccionados As Boolean = False
        For Each led In ListaLiquidacionesGapOrdenes
            If led.Activo = True Then
                HaySeleccionados = True
            End If
        Next
        If Not HaySeleccionados Then
            'C1.Silverlight.C1MessageBox.Show("No seleccionó ningún registro", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.OK, C1.Silverlight.C1MessageBoxIcon.Error)
            mostrarMensaje("No seleccionó ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        'C1.Silverlight.C1MessageBox.Show("¿Desear Generar las Ordenes?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPregunta)
        mostrarMensajePregunta("¿Desear Generar las Ordenes?", _
                               Program.TituloSistema, _
                               "GENERARORDENES", _
                               AddressOf TerminoPregunta, False)
    End Sub

    Public Sub DisGenerarOrden()
        If ValidarMaestroListaM() Then
            ValidacionesRegistrosSeleccionados()
        Else
            IsBusy = False
        End If
    End Sub

    Public Function ValidarMaestroListaM() As Boolean
        Try
            ValidarMaestroListaM = True
            If Not ValidarMaestroLista(0) = "TIP" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro TIPOLIMITE No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'Call mensaje(S_MENSAJE_INFORMATIVO, "El maestro " & "TIPOLIMITE" & MSGVALIDARMAESTRO, MSGERROR)
                'Screen.MousePointer = vbDefault: DoEvents: Exit Sub
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(1) = "CON" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro CONDICNEGOCIACION No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(2) = "FOR" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro FORMAPAGO No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(3) = "INV" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro TIPOINVERSION No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(4) = "CAN" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro CANALLEO No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(5) = "MED" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro MEDIOVERLEO No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(6) = "EJE" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro EJECUCION No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            If Not ValidarMaestroLista(7) = "DUR" Then
                A2Utilidades.Mensajes.mostrarMensaje("El maestro DURACION No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
            'Validar_UsuarioRecepcion
            If Not (ValidarMaestroLista(8) = "True") Then
                A2Utilidades.Mensajes.mostrarMensaje("El usuario que se tiene configurado en el parámetro" + vbCrLf + " MILA_CREAORDEN_USUARIO_RECEPCION_ORDEN" + vbCrLf + " no esta activo en el maestro de empleados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ValidarMaestroListaM = False
                Exit Function
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificacion de la Liquidacion", _
                Me.ToString(), "ValidarMaestroListaM", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
            Return Nothing
        End Try
    End Function

    Public Sub ValidacionesRegistrosSeleccionados()
        Try
            For Each led In ListaLiquidacionesGapOrdenes
                If led.Activo Then
                    ListaLiquidacionesGapOrdenesEscogidas.Add(led)
                End If
            Next
            NroRegistros = 0
            dcProxy1.ValidarInfoLiquidacionGapOrdenes(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).Comitente, _
                                                      ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).UBICACIONTITULO, Program.Usuario, Program.HashConexion, AddressOf TerminoValidarInfo, "validacion")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificacion de la Liquidacion", _
                Me.ToString(), "ValidacionesRegistrosSeleccionados", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub SalvarDatos()
        Try
            ListaLiquidacionesGapOrdenesGrabar.Clear()
            For Each led In ListaLiquidacionesGapOrdenesEscogidas
                If led.Activo = True Then
                    ListaLiquidacionesGapOrdenesGrabar.Add(led)
                End If
            Next
            If ListaLiquidacionesGapOrdenesGrabar.Count > 0 Then
                NroRegistros = 0
                NroAccionesGen = 0
                NroRentaGen = 0
                dcProxy.SalvarDatos(ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Tipo, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ClaseOrden, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ID, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaLiquidacion, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaCumplimiento, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Comitente, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Parcial, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).UBICACIONTITULO, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).CuentaDeceval, Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarRegistro, "grabar")
            Else
                MostrarResultado = New ResultadoGenerarOrdenLiqui(Mensaje) ', 0, 0)
                Program.Modal_OwnerMainWindowsPrincipal(MostrarResultado)
                MostrarResultado.ShowDialog()
                'A2Utilidades.Mensajes.mostrarMensaje(Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Mensaje = String.Empty
                IsBusy = False
            End If
            ListaLiquidacionesGapOrdenesEscogidas.Clear()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Generar la Orden desde la Liquidacion", _
                Me.ToString(), "SalvarDatos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "MetodosAsincronicos"

    Private Sub TerminoTraerLiquidacionesGapOrdenes(ByVal lo As LoadOperation(Of LiquidacionesGapOrdenes))
        If Not lo.HasError Then
            ListaLiquidacionesGapOrdenes = dcProxy.LiquidacionesGapOrdenes
            If ListaLiquidacionesGapOrdenes.Count > 0 Then
                HabilitarBotones = True
            Else
                HabilitarBotones = False
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de LiquidacionesGapOrdenes", _
                     Me.ToString(), "TerminoTraerLiquidacionesGapOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            dcProxy1.ValidarMaestroLista("", "", "", "", "", "", "", "", Program.Usuario, Program.HashConexion, AddressOf TerminoValidarMaestroLista, "consulta")
        End If
    End Sub

    Private Sub TerminoValidarMaestroLista(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            ValidarMaestroLista = Split(lo.Value, ",")
            DisGenerarOrden()
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de LiquidacionesGapOrdenes", _
                     Me.ToString(), "TerminoValidarMaestroLista", Application.Current.ToString(), Program.Maquina, lo.Error)
            IsBusy = False
        End If
    End Sub

    Private Sub TerminoValidarInfo(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            ValidarInfoL = Split(lo.Value, ",")
            Dim blnValidacion As Boolean = True
            ' Validar_Cliente
            If Not (LTrim(RTrim(ValidarInfoL(1))) = "A" And ValidarInfoL(2) = "True") Then
                Mensaje = String.Format("{0}Línea {1}: Cliente Invalido o Inactivo {2} (Liquidación: {3}) {4}", Mensaje, _
                                        ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).intIDLiquidacionesGapOrdenes, _
                                        vbTab, ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).ID, vbCr)
                blnValidacion = False
            End If
            'Validar_Ordenante
            If Not (ValidarInfoL(3) = "True") Then
                Mensaje = String.Format("{0}Línea {1}: El Comitente ({2}) no cuenta con ordenantes asociados {3} (Liquidación: {4}) {5}", Mensaje, _
                                        ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).intIDLiquidacionesGapOrdenes, _
                                        LTrim(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).Comitente), _
                                        vbTab, ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).ID, vbCr)
                blnValidacion = False
            End If
            'Validar_Receptor
            If Not (ValidarInfoL(4) = "True") Then
                Mensaje = String.Format("{0}Línea {1}: El Comitente ({2}) no cuenta con Receptores asociados {3} (Liquidación: {4}) {5}", Mensaje, _
                                        ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).intIDLiquidacionesGapOrdenes, _
                                        LTrim(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).Comitente), _
                                        vbTab, ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).ID, vbCr)
                blnValidacion = False
            End If
            'Existe en el maestro de empleados
            If Not (ValidarInfoL(5) = "True") Then
                Mensaje = String.Format("{0}Línea {1}: El Receptor ({2}) asociado al comitente ({3}) no existe en el maestro de empleados {4} (Liquidación: {5}) {6}", Mensaje, _
                                        ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).intIDLiquidacionesGapOrdenes, ValidarInfoL(6), _
                                        LTrim(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).Comitente), _
                                        vbTab, ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).ID, vbCr)
                blnValidacion = False
            End If
            'Validar_CuentaDeposito
            If IsNothing(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).CuentaDeceval) Then
                If UCase(Trim(ValidarInfoL(0))) <> "X" And UCase(Trim(ValidarInfoL(0))) <> "F" Then
                    Mensaje = String.Format("{0}Línea {1}: El Cliente no tiene cuenta DECEVAL {2} (Liquidación: {3}) {4}", Mensaje, _
                                            ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).intIDLiquidacionesGapOrdenes, _
                                            vbTab, ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).ID, vbCr)
                    blnValidacion = False
                End If
            End If
            If Not blnValidacion Then
                'ListaLiquidacionesGapOrdenesEscogidas.Remove(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros))
                Mensaje = String.Format("{0} {1}", Mensaje, vbCr)
                ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).Activo = False
            End If

            NroRegistros = NroRegistros + 1
            If NroRegistros < ListaLiquidacionesGapOrdenesEscogidas.Count Then
                dcProxy1.ValidarInfoLiquidacionGapOrdenes(ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).Comitente, _
                                          ListaLiquidacionesGapOrdenesEscogidas.Item(NroRegistros).UBICACIONTITULO, Program.Usuario, Program.HashConexion, AddressOf TerminoValidarInfo, "validacion")
            Else
                IsBusy = True
                SalvarDatos()
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de LiquidacionesGapOrdenes", _
                     Me.ToString(), "TerminoValidarMaestroLista", Application.Current.ToString(), Program.Maquina, lo.Error)
            IsBusy = False
        End If
    End Sub

    Private Sub TerminoGrabarRegistro(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Dim ClaseOrden As String
            Dim Tipo As String
            Select Case ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ClaseOrden
                Case "A"
                    ClaseOrden = "Acción"
                Case "C"
                    ClaseOrden = "Renta Fija"
            End Select

            Select Case ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Tipo
                Case "C"
                    Tipo = "Compra"
                Case "R"
                    Tipo = "Recompra"
                Case "V"
                    Tipo = "Venta"
                Case "S"
                    Tipo = "Reventa"
            End Select

            If lo.Value = "3" Then
                Mensaje = String.Format("{0}No se encontro orden para relacionar la liquidación: {1} TIPO: {2} CLASE: {3} FECHA LIQUIDACION: {4} TIPO OFERTA: {5} LINEA: {6} {7}{8}", _
                                        Mensaje, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ID, Tipo, _
                                        ClaseOrden, Format(ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaLiquidacion, "yyyy/MM/dd"), _
                                        ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).TipoOferta, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).intIDLiquidacionesGapOrdenes, vbCr, vbCr)
                'Select Case ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ClaseOrden
                '    Case "A"
                '        NroAccionesGen = NroAccionesGen - 1
                '    Case "C"
                '        NroRentaGen = NroRentaGen - 1
                'End Select
            ElseIf lo.Value = "4" Then
                Mensaje = String.Format("{0}Se generó orden para liquidación: {1} TIPO: {2} CLASE: {3} FECHA LIQUIDACION: {4} TIPO OFERTA: {5} LINEA: {6} {7}{8}", _
                        Mensaje, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ID, Tipo, _
                        ClaseOrden, Format(ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaLiquidacion, "yyyy/MM/dd"), _
                        ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).TipoOferta, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).intIDLiquidacionesGapOrdenes, vbCr, vbCr)

                Select Case ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ClaseOrden
                    Case "A"
                        NroAccionesGen = NroAccionesGen + 1
                    Case "C"
                        NroRentaGen = NroRentaGen + 1
                End Select
            Else
                Mensaje = String.Format("{0}No Se generó orden para liquidación: {1} TIPO: {2} CLASE: {3} FECHA LIQUIDACION: {4} TIPO OFERTA: {5} LINEA: {6} Error:{7} {8}{9}", _
                        Mensaje, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ID, Tipo, _
                        ClaseOrden, Format(ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaLiquidacion, "yyyy/MM/dd"), _
                        ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).TipoOferta, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).intIDLiquidacionesGapOrdenes, lo.Value.ToString, vbCr, vbCr)
            End If

            NroRegistros = NroRegistros + 1
            If NroRegistros < ListaLiquidacionesGapOrdenesGrabar.Count Then
                dcProxy.SalvarDatos(ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Tipo, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ClaseOrden, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).ID, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaLiquidacion, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).dtmFechaCumplimiento, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Comitente, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).Parcial, ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).UBICACIONTITULO, _
                                    ListaLiquidacionesGapOrdenesGrabar.Item(NroRegistros).CuentaDeceval, Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarRegistro, "grabar")
            Else
                'IsBusy = False
                MostrarResultado = New ResultadoGenerarOrdenLiqui(Mensaje) ', NroAccionesGen, NroRentaGen)
                Program.Modal_OwnerMainWindowsPrincipal(MostrarResultado)
                MostrarResultado.ShowDialog()
                'A2Utilidades.Mensajes.mostrarMensaje(Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                A2Utilidades.Mensajes.mostrarMensaje("Se generó un total de: " & CStr(Int(NroAccionesGen + NroRentaGen)) & " ordenes satisfactoriamente " & vbCrLf & vbCrLf & " -Total Ordenes Acciones: " & NroAccionesGen & vbCrLf & " -Total Ordenes Renta Fija: " & NroRentaGen, _
                                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                Mensaje = String.Empty
                IsBusy = True
                dcProxy.LiquidacionesGapOrdenes.Clear()
                dcProxy.Load(dcProxy.ConsultarLiquidacionesGapOrdenesQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesGapOrdenes, "FiltroInicial")
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de LiquidacionesGapOrdenes", _
                 Me.ToString(), "TerminoGrabarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
            IsBusy = True
        End If
        'IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _LiquidacionGapOrdenesSelected As LiquidacionesGapOrdenes
    Public Property LiquidacionGapOrdenesSelected() As LiquidacionesGapOrdenes
        Get
            Return _LiquidacionGapOrdenesSelected
        End Get
        Set(value As LiquidacionesGapOrdenes)
            _LiquidacionGapOrdenesSelected = value
            CambioItem("LiquidacionGapOrdenesSelected")
        End Set
    End Property

    Private _ListaLiquidacionesGapOrdenes As EntitySet(Of LiquidacionesGapOrdenes)
    Public Property ListaLiquidacionesGapOrdenes() As EntitySet(Of LiquidacionesGapOrdenes)
        Get
            Return _ListaLiquidacionesGapOrdenes
        End Get
        Set(ByVal value As EntitySet(Of LiquidacionesGapOrdenes))
            _ListaLiquidacionesGapOrdenes = value
            MyBase.CambioItem("ListaLiquidacionesGapOrdenes")
            MyBase.CambioItem("ListaLiquidacionesGapOrdenesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaLiquidacionesGapOrdenesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidacionesGapOrdenes) Then
                Dim view = New PagedCollectionView(_ListaLiquidacionesGapOrdenes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


    Private _HabilitarBotones As Boolean
    Public Property HabilitarBotones As Boolean
        Get
            Return _HabilitarBotones
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotones = value
            MyBase.CambioItem("HabilitarBotones")
        End Set
    End Property


#End Region

    Public Class ListaCombo
        Private _Descripcion As String
        Public Property Descripcion() As String
            Get
                Return _Descripcion
            End Get
            Set(ByVal value As String)
                _Descripcion = value
            End Set
        End Property

        Private _ID As Integer
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

    End Class

    Public Class ParametrosSeleccion
        Implements INotifyPropertyChanged

        Private _tipo As Integer = 3
        Public Property tipo As Integer
            Get
                Return _tipo
            End Get
            Set(ByVal value As Integer)
                _tipo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("tipo"))
                'MyBase.CambioItem("tipo")
            End Set
        End Property

        Private _clase As Integer = 3
        Public Property clase As Integer
            Get
                Return _clase
            End Get
            Set(ByVal value As Integer)
                _clase = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("clase"))
                'MyBase.CambioItem("clase")
            End Set
        End Property

        Private _CodigoCliente As String
        Public Property CodigoCliente() As String
            Get
                Return _CodigoCliente
            End Get
            Set(ByVal value As String)
                _CodigoCliente = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoCliente"))
                'MyBase.CambioItem("CodigoCliente")
            End Set
        End Property

        Private _NombreCliente As String
        Public Property NombreCliente() As String
            Get
                Return _NombreCliente
            End Get
            Set(ByVal value As String)
                _NombreCliente = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
                'MyBase.CambioItem("NombreCliente")
            End Set
        End Property

        Private _Nemotecnico As String
        Public Property Nemotecnico() As String
            Get
                Return _Nemotecnico
            End Get
            Set(ByVal value As String)
                _Nemotecnico = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
                'MyBase.CambioItem("Nemotecnico")
            End Set
        End Property

        Private _Especie As String
        Public Property Especie() As String
            Get
                Return _Especie
            End Get
            Set(ByVal value As String)
                _Especie = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
                'MyBase.CambioItem("Especie")
            End Set
        End Property

        Private _EspeciesS As Boolean
        Public Property EspeciesS() As Boolean
            Get
                Return _EspeciesS
            End Get
            Set(ByVal value As Boolean)
                _EspeciesS = value
                If value = True Then
                    Nemotecnico = String.Empty
                    Especie = String.Empty
                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspeciesS"))
                'MyBase.CambioItem("EspeciesS")
            End Set
        End Property

        Private _ClientesS As Boolean
        Public Property ClientesS() As Boolean
            Get
                Return _ClientesS
            End Get
            Set(ByVal value As Boolean)
                _ClientesS = value
                If value = True Then
                    CodigoCliente = String.Empty
                    NombreCliente = String.Empty
                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClientesS"))
                'MyBase.CambioItem("ClientesS")
            End Set
        End Property

        'Private _Lista As New Dictionary(Of Integer, String)
        'Public Property Lista() As Dictionary(Of Integer, String)
        '    Get
        '        Return _Lista
        '    End Get
        '    Set(ByVal value As Dictionary(Of Integer, String))
        '        _Lista = value
        '        MyBase.CambioItem("Lista")
        '    End Set
        'End Property

        Private _ListaClase As New List(Of ListaCombo)
        Public Property ListaClase() As List(Of ListaCombo)
            Get
                Return _ListaClase
            End Get
            Set(ByVal value As List(Of ListaCombo))
                _ListaClase = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaClase"))
                'MyBase.CambioItem("ListaClase")
            End Set
        End Property

        Private _ListaTipo As New List(Of ListaCombo)
        Public Property ListaTipo() As List(Of ListaCombo)
            Get
                Return _ListaTipo
            End Get
            Set(ByVal value As List(Of ListaCombo))
                _ListaTipo = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaTipo"))
                'MyBase.CambioItem("ListaTipo")
            End Set
        End Property

        Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    End Class

End Class
