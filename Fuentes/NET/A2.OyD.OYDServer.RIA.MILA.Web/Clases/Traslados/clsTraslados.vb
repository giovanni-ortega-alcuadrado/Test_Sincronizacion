Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.IO

Public Class clsTraslados

    Private L2SDC As New OyDBolsaDatacontext
    Private Mensajes As String = String.Empty
    Private gStringErrores As New StringBuilder

    Private Const MSGERROR_INICIO_TRASLADO As String = "Inicio del traslado"
    Private Const MSGERROR_FIN_TRASLADO As String = "Fin del traslado"

    Public Property gstrUser As String
    Public Property gbtyIDBOLSA As Byte

    'Public Function TrasladarLiquidaciones(pstrRutaArchivo As String, pstrNombreContacto As String, pstrClase As String, pstrUsuario As String, plngIdSucursal As Integer) As String

    '    Reportar(MSGERROR_INICIO_TRASLADO)

    '    Try

    '        'If Trim(pstrRutaArchivo) = "" Then
    '    '    Return "Ingrese una ruta para los archivos de control, Notificación a Clientes!"
    '    '    Exit Function
    '    'End If
    '        If (pstrNombreContacto).Trim() = "" Then
    '            Reportar("No hay nombre de contacto para el archivo de control")
    '            Return gStringErrores.ToString()
    '            Exit Function
    '        End If

    '    'Call GenerarPlano()
    '    Dim ret = FnTrasladarLiquidaciones(pstrClase, pstrUsuario, plngIdSucursal)
    '    Call EscribirArchivoParametrosNotasPortafolio(pstrNombreContacto) 'Grabar datos en el *.INI

    '        Reportar("Respuesta de SP: " & ret.ToString())
    '        Reportar(MSGERROR_INICIO_TRASLADO)



    '    Catch ex As Exception
    '        Reportar("Errores presentados: " & ex.Message.ToString())
    '    End Try

    '    Return gStringErrores.ToString()

    'End Function

    Friend Sub GenerarPlano()
        ''        'Anexar codigo para generar el archivo plano  -- OK
        ''        'Validar que tengan datos el nombre del contacto y la ruta -- OK
        ''        'strLineas = Indice
        ''On Error GoTo Error

        ''        Dim mpstTrasladarLiq      As rdoPreparedStatement
        ''        Dim strSQL As String
        ''        Dim rst As rdoResultset
        ''        '------
        'Dim lngIDAnterior As String
        'Dim lngIDActual As String
        'Dim gBblnGenFile As Boolean
        ''        '----------
        'Dim dtmFechaServer As String
        'Dim CodClienteCtrl As String
        'Dim strContenido As String
        'Dim strLinea As String
        ''Dim gstrsql As String
        'Dim gstrsqlCliente As String
        'Dim strClase As String
        'Dim lngIDSucursal As Long
        'Dim strCodigoSWIFT As String
        'Dim strNombreCliente As String

        'Dim lngVlrGiro As String
        'Dim strNomEmisor As String
        'Dim strDescClase As String
        'Dim strDescTipo As String

        ''        gstrsql = "SELECT dtmFecha = CONVERT(char(12), GETDATE(), 102)"

        'dtmFechaServer = Now.Year.ToString() & Now.Month.ToString("00") & Now.Day.ToString("00").ToString()
        ''        dtmFechaServer = ReemplazarcaracterEnCadena(dtmFechaServer)
        'Mensajes = ""
        'lngIDAnterior = ""
        'lngIDActual = ""
        'gBblnGenFile = False

        ''Se evalua la clase seleccionada
        ''Comenta Rafael Cordero, ya que se enviara por defecto la opción Ambas "T".
        ''If optTipo(ACCION).Value = True Then
        ''    strClase = "A"
        ''Else
        ''    If optTipo(RENTAFIJA).Value = True Then
        ''        strClase = "C"
        ''    Else
        ''        strClase = "T"
        ''    End If
        ''End If

        'strClase = "T" 'Se envia la opción Ambas por Defecto.

        ''Se comenta para enviar por defecto la opcion todas las sucursales "-1"
        ''        If cboSucursal.ListIndex <> -1 Then
        ''            If cboSucursal.Text <> STR_TODOS Then
        ''                lngIDSucursal = cboSucursal.ItemData(cboSucursal.ListIndex)
        ''            Else
        ''                lngIDSucursal = -1
        ''            End If
        ''        Else
        ''            Call Mensaje(S_MENSAJE_ESCRIBE, "Debe suministrar una sucursal", S_MENSAJE_ERROR)
        ''            cboSucursal.SetFocus()
        ''            Exit Sub
        ''        End If

        'lngIDSucursal = -1 'Se envia la opción -1 el cual indica que son todas.

        ''        If MsgBox("¿Genera el archivo plano para el Cliente? ", vbQuestion + vbYesNo + vbDefaultButton2, _
        ''                            "Generar Archivos de Confirmacion") = vbYes Then
        ''            Dim i As Long

        ''            ''    gBblnGenFile = True
        ''            Load(frmLogImportacion)
        ''            frmLogImportacion.Caption = "Generación de Archivos Planos - Confirmación Clientes"
        ''            Mensajes = MSG_INICIO_GENPLANO & vbCrLf & MSG_GENPLANO_ENCABEZADO
        ''            Call Reportar(Mensajes)

        ''            If mpstmGeneraPlano Is Nothing Then
        ''                gstrsql = "{CALL dbo.sp_TrasladarLiq_GeneraPlano(?, ?, ?,?,?)}"
        ''                mpstmGeneraPlano = gdb.CreatePreparedStatement("", gstrsql)
        ''            End If

        ''            With mpstmGeneraPlano
        ''                .rdoParameters(0).Value = glngIDComisionista
        ''                .rdoParameters(1).Value = glngIDSucComisionista
        ''                .rdoParameters(2).Value = strClase
        ''                .rdoParameters(3).Value = gstrUser
        ''                .rdoParameters(4).Value = lngIDSucursal
        ''                .Execute()
        ''                rdoErrors.Clear()
        ''                rst = .OpenResultset(rdOpenStatic, rdConcurReadOnly)
        ''            End With

        ''            With rst
        ''                If .EOF And .BOF Then
        ''                Else
        ''                    .MoveFirst()

        ''                    For i = 0 To .RowCount - 1
        ''                        'TOMAR CODIGO DEL RESULTSET Y REALIZAR UNA CONSULTA A TBLCLIENTES,
        ''                        'SI EL VALOR NO ES NULO LLAME EL GENERAR ARCHIVO
        ''Inicio:
        ''                        gstrsqlCliente = "SELECT strCodigoSwift FROM tblclientes "
        ''                        gstrsqlCliente = gstrsqlCliente & "WHERE lngId = " & !lngIDComitente
        ''                        'FUNCION DATOSQL()
        ''                        strCodigoSWIFT = IIf(IsNull(DatoSQL(gstrsqlCliente)), "", (DatoSQL(gstrsqlCliente)))
        ''                        If Not (strCodigoSWIFT) = "" Then
        ''                            'No tiene Codigo SWIFT, no genera plano.
        ''                            Select Case (!strClaseOrden)
        ''                                Case "A"
        ''                                    strDescClase = "Acciones"
        ''                                Case "C"
        ''                                    strDescClase = "Renta Fija"
        ''                                Case Else
        ''                                    strDescClase = "Desconocido"
        ''                            End Select

        ''                            Select Case (!strTipo)
        ''                                Case "C"
        ''                                    strDescTipo = "COMPRA"
        ''                                Case "V"
        ''                                    strDescTipo = "VENTA"
        ''                                Case "S"
        ''                                    strDescTipo = "REVENTA"
        ''                                Case "R"
        ''                                    strDescTipo = "RECOMPRA"
        ''                                Case Else
        ''                            End Select
        ''                            strNombreCliente = !strNomCliente
        ''                            '---    Emisor de la especie
        ''                            gstrsqlCliente = ""
        ''                            gstrsqlCliente = "SELECT Emi.strNombre"
        ''                            gstrsqlCliente = gstrsqlCliente & " FROM  tblEspecies Esp LEFT JOIN tblEmisores Emi"
        ''                            gstrsqlCliente = gstrsqlCliente & " ON Esp.lngIdEmisor = Emi.lngID"
        ''                            gstrsqlCliente = gstrsqlCliente & " WHERE (Esp.strId = " & "'" & !strIDEspecie & "') "
        ''                            gstrsqlCliente = gstrsqlCliente & " AND Esp.lngIDComisionista  = " & !lngIDComisionista
        ''                            gstrsqlCliente = gstrsqlCliente & " AND Esp.lngIDSucComisionista = " & !lngIDSucComisionista
        ''                            strNomEmisor = IIf(IsNull(DatoSQL(gstrsqlCliente)), "", (DatoSQL(gstrsqlCliente)))

        ''                            '----   Calculo de Valor Giro Contraparte.
        ''                            'lngVlrGiro = CLng(!curSubTotalLiq) + CLng(!curComision) + CLng(!curValorIva)
        ''                            '-------------------------------------------------------------------
        ''                            CodClienteCtrl = !lngIDComitente
        ''                            CodClienteCtrl = ReemplazarcaracterEnCadena(CodClienteCtrl)

        ''                            If lngIDAnterior = !lngIDComitente Then
        ''                                GoTo Transacciones
        ''                            End If
        ''                            '------------------- ENCABEZADO DEL ARCHIVO DE CONTROL
        ''encabezado:
        ''                            strContenido = vbCrLf & vbCrLf & vbCrLf & IndArchCtrl & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & "TO " & CStr(txtNomContact.Text)
        ''                            strContenido = strContenido & vbCrLf & !strNomCliente & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & vbCrLf & ":20:" & vbTab & dtmFechaServer
        ''                            strContenido = strContenido & vbCrLf & ":79:" & vbTab & "NARRATIVA"
        ''                            strContenido = strContenido & vbCrLf & vbCrLf & "Atención Sr. "
        ''                            strContenido = strContenido & !strRepresentanteLegal & vbCrLf & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & "Confirmamos que CITIVALORES COMISIONISTA DE BOLSA, ha cerrado la(s) siguiente(s) "
        ''                            strContenido = strContenido & "operación(es) ante la Bolsa de Valores de Colombia en nombre de "
        ''                            strContenido = strContenido & !strNomCliente
        ''                            'Continua en Transacciones:
        ''                            '------------------ INICIO DE SECCION DE OPERACIONES, REPETIR CON EL # DE OP.
        ''Transacciones:
        ''                            strContenido = strContenido & vbCrLf & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & "Compra / Venta" & vbTab & vbTab
        ''                            strContenido = strContenido & strDescTipo & vbCrLf & vbCrLf  'TOMAR TIPO OPERACION
        ''                            strContenido = strContenido & "Fecha Operación" & vbTab & vbTab
        ''                            strContenido = strContenido & !dtmLiquidacion & vbCrLf & vbCrLf 'TOMAR FECHA OPERACION
        ''                            strContenido = strContenido & "Fecha Cumplimiento" & vbTab
        ''                            strContenido = strContenido & !dtmCumplimiento & vbCrLf & vbCrLf  'TOMAR FECHA CUMPLIMIENTO
        ''                            strContenido = strContenido & "TITULO NEGOCIADO" & vbTab
        ''                            strContenido = strContenido & strDescClase & vbCrLf & vbCrLf  'ACCIONES O RTA FIJA
        ''                            strContenido = strContenido & "Emisor" & vbTab & vbTab & vbTab
        ''                            strContenido = strContenido & strNomEmisor & vbCrLf & vbCrLf     'EMISOR DEL TITULO
        ''                            strContenido = strContenido & "Cantidad Negociado" & vbTab
        ''                            strContenido = strContenido & !dblCantidad & vbCrLf  'TOMAR LA CANTIDAD DE ACCIONES.
        ''                            strContenido = strContenido & vbCrLf
        ''                            strContenido = strContenido & "Valor" & vbTab & vbTab & vbTab
        ''                            strContenido = strContenido & "COP " & !curSubTotalLiq 'VALOR COP?
        ''                            strContenido = strContenido & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & "Comision" & vbTab & vbTab
        ''                            strContenido = strContenido & "COP " & !curComision  'VALOR COMISION COP
        ''                            strContenido = strContenido & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & "IVA Comision" & vbTab & vbTab
        ''                            strContenido = strContenido & "COP " & !curValorIva  'VALOR COP IVA COMISION
        ''                            strContenido = strContenido & vbCrLf & vbCrLf
        ''                            strContenido = strContenido & "valor giro contraparte " & vbTab
        ''                            ''strContenido = strContenido & "COP " & !curTotalLiq   'VALOR COP GIRO -CGA, 2007/10/30
        ''                            strContenido = strContenido & "COP " & (!curTotalLiq - !curComision - !curValorIva)     'VALOR COP GIRO +CGA, 2007/10/30
        ''                            strContenido = strContenido & vbCrLf & vbCrLf

        ''                            '----------------- Seccion Creación y vaciado a Archivo
        ''                        Else
        ''                            Mensajes = Mensajes & vbCrLf & "El Cliente con Código <" & Trim(CStr(!lngIDComitente)) & "> No tiene Código Swift, por esta razón no tendrá archivo plano."
        ''                            Call Reportar(Mensajes)
        ''                        End If
        ''                        lngIDAnterior = CStr(!lngIDComitente)
        ''                        .MoveNext()
        ''                        If rst.EOF = True Then
        ''                            lngIDActual = lngIDAnterior
        ''                        Else
        ''                            lngIDActual = CStr(!lngIDComitente)
        ''                        End If
        ''                        '--  SI EL LNGIDCOMITENTE ES DIFERENTE CREE EL PLANO SI NO, CONTINUE
        ''                        If ((lngIDAnterior <> lngIDActual) Or (rst.EOF = True)) And Not (strCodigoSWIFT) = "" Then
        ''                            'Si se generó el archivo?
        ''                            gBblnGenFile = True '+CGA, 20070918
        ''30:                         If ExisteArchivo(txtRutaArchivo & "\" & dtmFechaServer & "-" & CodClienteCtrl & ".txt") Then
        ''                                mintArchivoParamNotas = FreeFile
        ''                        Open txtRutaArchivo & "\" & dtmFechaServer & "-" & CodClienteCtrl & ".txt" For Input As mintArchivoParamNotas

        ''                                If Not EOF(mintArchivoParamNotas) Then
        ''                            Input #mintArchivoParamNotas, strContenido
        ''                                    '            txtNomContact.Text = strLinea
        ''                                    '            strLinea = ""
        ''                                Else
        ''                                    txtNomContact.Text = strContenido & ""
        ''                                End If

        ''                        Close #mintArchivoParamNotas
        ''                            Else
        ''                                mintArchivoParamNotas = FreeFile
        ''                        Open txtRutaArchivo.Text & "\" & dtmFechaServer & "-" & CodClienteCtrl & ".txt" For Output As mintArchivoParamNotas
        ''                        Print #mintArchivoParamNotas, strContenido
        ''                        Close #mintArchivoParamNotas
        ''                            End If

        ''                            Mensajes = Mensajes & vbCrLf & "Archivo Generado : " & dtmFechaServer & "-" & CodClienteCtrl & ".txt"
        ''                            Call Reportar(Mensajes)

        ''                        End If    'DE LA PREGUNTA SI LNGIDCOMITENTE DIFERENTE.

        ''                    Next

        ''                End If
        ''                .Cancel()
        ''                .Close()
        ''            End With

        ''        End If


        ''EXIT_GenerarPlano:
        ''        If gBblnGenFile Then
        ''            Mensajes = Mensajes & vbCrLf & "Proceso finalizado!"
        ''            Mensajes = Mensajes & vbCrLf & vbCrLf & "Los archivos han sido generados en la ruta " & txtRutaArchivo.Text
        ''            Call Reportar(Mensajes)
        ''        Else
        ''            'Unload frmLogImportacion '?
        ''            BarraProgreso(pstrTipo:=S_BARRA_INVISIBLE)
        ''        End If


    End Sub

    Public Function FnTrasladarLiquidaciones(pTipoMercado As String, pstrUsuario As String, pintSucursal As Integer, pstrContacto As String, plogActualizarCostos As Boolean) As String

        Try

            Reportar(MSGERROR_INICIO_TRASLADO)

            If (pstrContacto).Trim() = "" Then
                Reportar("No hay nombre de contacto para el archivo de control")
                Return gStringErrores.ToString()
                Exit Function
            End If

            'On Error GoTo Err_TrasladarLiquidaciones
            'Dim mpstTrasladarLiq As rdoPreparedStatement  'Datos se homologa con RIA
            'Dim strSQL As String
            'Dim Message As String
            'Dim strClase As String
            Dim lngIDSucursal As Long
            Dim intRespuesta As Integer = 0

            lngIDSucursal = pintSucursal

            Call PrEjecutarTraslado(pTipoMercado, pstrUsuario, pintSucursal, plogActualizarCostos) 'JAG 20140311 se agrega el envio del parametro plogActualizarCostos
            Call EscribirArchivoParametrosNotasPortafolio(pstrContacto) 'Grabar datos en el *.INI

            Reportar(MSGERROR_FIN_TRASLADO)

        Catch ex As Exception
            Reportar("Errores presentados: " & ex.Message.ToString())
        End Try

        'Se elimina esta validación ya que esta validación se hace en la forma.
        'If cboSucursal.ListIndex <> -1 Then
        '    If cboSucursal.Text <> STR_TODOS Then
        '        lngIDSucursal = cboSucursal.ItemData(cboSucursal.ListIndex)
        '    Else
        '        lngIDSucursal = -1
        '    End If
        'Else
        '    Call Mensaje(S_MENSAJE_ESCRIBE, "Debe suministrar una sucursal", S_MENSAJE_ERROR)
        '    cboSucursal.SetFocus()
        '    Exit Sub
        'End If


        'If MsgBox("¿Traslada las liquidaciones? ", vbQuestion + vbYesNo + vbDefaultButton2, _
        '                    "Trasladar Liquidaciones") = vbYes Then
        '    If mpstTrasladarLiq Is Nothing Then
        '        strSQL = "{? = call sp_TrasladarLiquidacion (?,?,?,?,?)}"
        '        mpstTrasladarLiq = gdb.CreatePreparedStatement("", strSQL)
        '    End If
        '    With mpstTrasladarLiq
        '        .rdoParameters(0).Direction = rdParamReturnValue
        '        .rdoParameters(1).Value = glngIDComisionista
        '        .rdoParameters(2).Value = glngIDSucComisionista
        '        .rdoParameters(3).Value = strClase
        '        .rdoParameters(4).Value = gstrUser
        '        .rdoParameters(5).Value = lngIDSucursal
        '        .Execute()
        '        If (.rdoParameters(0).Value = 0) Or (.rdoParameters(0).Value = Null) Then
        '            MsgBox("  No hay Liquidaciones para trasladar  ", vbInformation + vbOKOnly)
        '        Else
        '            Message = "   Se Trasladaron " & Trim(CStr(.rdoParameters(0).Value)) & _
        '                      " Liquidaciones   "
        '            MsgBox(Message, vbInformation + vbOKOnly)
        '        End If
        '        .Close()
        '    End With
        '    mpstTrasladarLiq = Nothing
        'End If




        'If (res = 0) Or IsNothing(res) Then
        '    MsgBox("  No hay Liquidaciones para trasladar  ", vbInformation + vbOKOnly)
        'Else
        '    Message = "   Se Trasladaron " & res & _
        '              " Liquidaciones   "
        '    MsgBox(Message, vbInformation + vbOKOnly)
        'End If

        Return gStringErrores.ToString()

    End Function

    Friend Sub EscribirArchivoParametrosNotasPortafolio(pstrNombreContacto As String)

        Dim mintArchivoParamNotas As String = String.Empty

        'Antes de salirnos de la impresion de portafolio de inversion se graban las notas
        'En un archivo contacto.ini.

        Dim fs As New FileStream(My.Application.Info.DirectoryPath & "\contacto.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        'declaring a FileStream and creating a document file named file with 
        'access mode of writing
        Dim s As New StreamWriter(fs)
        'creating a new StreamWriter and passing the filestream object fs as argument
        s.Write(pstrNombreContacto)
        'writing text to the newly created file
        s.Close()
        'closing the file

        Exit Sub
ErrorControlado:

    End Sub

    Private Sub PrEjecutarTraslado(pstrClase As String, pstrUsuario As String, plngIdSucursal As Integer, plogActualizarCostos As Boolean)

        Dim ret = L2SDC.spTrasladarLiquidaciones(pstrClase, pstrUsuario, plngIdSucursal, DemeInfoSesion("PrEjecutarTraslado"), 0, plogActualizarCostos) 'JAG 20140311 se agrega el envio del parametro plogActualizarCostos
        Reportar("Registros trasladados: [" & ret & "].")

    End Sub

    Private Sub Reportar(ByVal pstrMensaje As String)
        On Error GoTo Err_Reportar

        '   frmLogImportacion.lstLogErrores.AddItem pstrMensaje
        '   frmLogImportacion.lstLogErrores.Refresh
        '    frmLogImportacion.txtLogErrores.Text = frmLogImportacion.txtLogErrores.Text & pstrMensaje & vbCrLf
        '    frmLogImportacion.txtLogErrores.Refresh
        gStringErrores.AppendLine(String.Format("{0} {1}", Now.ToLongTimeString, pstrMensaje))
        'frmLogImportacion.txtLogErrores.AddItem(pstrMensaje)
        'frmLogImportacion.txtLogErrores.Refresh()


Exit_Reportar:
        Exit Sub

Err_Reportar:
        'LogErrores(Me.Name & ".Reportar", Err.Number, Err.Description)
        'Resume Exit_Reportar
    End Sub

End Class
