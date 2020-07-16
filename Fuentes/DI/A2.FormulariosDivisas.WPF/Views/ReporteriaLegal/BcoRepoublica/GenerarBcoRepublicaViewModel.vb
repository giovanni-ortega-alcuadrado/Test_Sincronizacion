
'SV20181009_BCOREPUBLICA: Desarrollo de pantalla de generación de archivos de banco república

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO
Imports System.Text


Public Class GenerarBcoRepublicaViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As FormulariosDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Public objViewPrincipal As GenerarBcoRepublicaView

    Dim mstrNitIMC As String
    Dim mstrDetalle As String

#End Region


#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub


    Public Sub inicializar()

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()
            PrepararNuevaBusqueda()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de datos del formulario 1
    ''' </summary>
    Private _ListaBcoRepublicaF1 As List(Of CPX_tblBcoRepublicaF1)
    Public Property ListaBcoRepublicaF1() As List(Of CPX_tblBcoRepublicaF1)
        Get
            Return _ListaBcoRepublicaF1
        End Get
        Set(ByVal value As List(Of CPX_tblBcoRepublicaF1))
            _ListaBcoRepublicaF1 = value
            MyBase.CambioItem("ListaBcoRepublicaF1")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del formulario 2
    ''' </summary>
    Private _ListaBcoRepublicaF2 As List(Of CPX_tblBcoRepublicaF2)
    Public Property ListaBcoRepublicaF2() As List(Of CPX_tblBcoRepublicaF2)
        Get
            Return _ListaBcoRepublicaF2
        End Get
        Set(ByVal value As List(Of CPX_tblBcoRepublicaF2))
            _ListaBcoRepublicaF2 = value
            MyBase.CambioItem("ListaBcoRepublicaF2")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del formulario 3
    ''' </summary>
    Private _ListaBcoRepublicaF3 As List(Of CPX_tblBcoRepublicaF3)
    Public Property ListaBcoRepublicaF3() As List(Of CPX_tblBcoRepublicaF3)
        Get
            Return _ListaBcoRepublicaF3
        End Get
        Set(ByVal value As List(Of CPX_tblBcoRepublicaF3))
            _ListaBcoRepublicaF3 = value
            MyBase.CambioItem("ListaBcoRepublicaF3")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del formulario 4
    ''' </summary>
    Private _ListaBcoRepublicaF4 As List(Of CPX_tblBcoRepublicaF4)
    Public Property ListaBcoRepublicaF4() As List(Of CPX_tblBcoRepublicaF4)
        Get
            Return _ListaBcoRepublicaF4
        End Get
        Set(ByVal value As List(Of CPX_tblBcoRepublicaF4))
            _ListaBcoRepublicaF4 = value
            MyBase.CambioItem("ListaBcoRepublicaF4")
        End Set
    End Property

    ''' <summary>
    ''' Lista de datos del formulario 5
    ''' </summary>
    Private _ListaBcoRepublicaF5 As List(Of CPX_tblBcoRepublicaF5)
    Public Property ListaBcoRepublicaF5() As List(Of CPX_tblBcoRepublicaF5)
        Get
            Return _ListaBcoRepublicaF5
        End Get
        Set(ByVal value As List(Of CPX_tblBcoRepublicaF5))
            _ListaBcoRepublicaF5 = value
            MyBase.CambioItem("ListaBcoRepublicaF5")
        End Set
    End Property

    ''' <summary>
    ''' Nombre del archivo generado
    ''' </summary>
    Private _strNombreArchivo As String
    Public Property strNombreArchivo() As String
        Get
            Return _strNombreArchivo
        End Get
        Set(ByVal value As String)
            _strNombreArchivo = value
            MyBase.CambioItem("strNombreArchivo")
        End Set
    End Property


    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaBcoRepublica
    Public Property cb() As CamposBusquedaBcoRepublica
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaBcoRepublica)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region


#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Preparar nueva busqueda de registros
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaBcoRepublica
            objCB.dtmFechaDesde = Date.Now
            objCB.dtmFechaHasta = Date.Now
            objCB.strTipoEnviado = "T"
            cb = objCB

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos públicos del encabezado"

    ''' <summary>
    ''' Consultar todos los datos de los formularios
    ''' </summary>
    Public Async Sub ConsultarBancoRepublica()
        Try
            If IsNothing(cb.dtmFechaDesde) Or IsNothing(cb.dtmFechaHasta) Or IsNothing(cb.strTipoEnviado) Then
                MessageBox.Show((DiccionarioEtiquetasPantalla("FALTANFILTROS").Titulo))
            Else

                Dim objRespuesta1 = Await mdcProxy.ReporteriaLegal_BcoRepublicaF1_ConsultarAsync(Formulario.Formulario1, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                If Not IsNothing(objRespuesta1) Then
                    ListaBcoRepublicaF1 = objRespuesta1.Value
                    If Not IsNothing(ListaBcoRepublicaF1) AndAlso ListaBcoRepublicaF1.Count > 0 Then
                        mstrNitIMC = ListaBcoRepublicaF1.FirstOrDefault.NIT_IMC
                    End If
                End If

                Dim objRespuesta2 = Await mdcProxy.ReporteriaLegal_BcoRepublicaF2_ConsultarAsync(Formulario.Formulario2, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                If Not IsNothing(objRespuesta2) Then
                    ListaBcoRepublicaF2 = objRespuesta2.Value
                    If Not IsNothing(ListaBcoRepublicaF2) AndAlso ListaBcoRepublicaF2.Count > 0 Then
                        mstrNitIMC = ListaBcoRepublicaF2.FirstOrDefault.NIT_IMC
                    End If
                End If

                Dim objRespuesta3 = Await mdcProxy.ReporteriaLegal_BcoRepublicaF3_ConsultarAsync(Formulario.Formulario3, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                If Not IsNothing(objRespuesta3) Then
                    ListaBcoRepublicaF3 = objRespuesta3.Value
                    If Not IsNothing(ListaBcoRepublicaF3) AndAlso ListaBcoRepublicaF3.Count > 0 Then
                        mstrNitIMC = ListaBcoRepublicaF3.FirstOrDefault.NIT_IMC
                    End If
                End If

                Dim objRespuesta4 = Await mdcProxy.ReporteriaLegal_BcoRepublicaF4_ConsultarAsync(Formulario.Formulario4, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                If Not IsNothing(objRespuesta4) Then
                    ListaBcoRepublicaF4 = objRespuesta4.Value
                    If Not IsNothing(ListaBcoRepublicaF4) AndAlso ListaBcoRepublicaF4.Count > 0 Then
                        mstrNitIMC = ListaBcoRepublicaF4.FirstOrDefault.NIT_IMC
                    End If
                End If

                Dim objRespuesta5 = Await mdcProxy.ReporteriaLegal_BcoRepublicaF5_ConsultarAsync(Formulario.Formulario5, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                If Not IsNothing(objRespuesta5) Then
                    ListaBcoRepublicaF5 = objRespuesta5.Value
                    If Not IsNothing(ListaBcoRepublicaF5) AndAlso ListaBcoRepublicaF5.Count > 0 Then
                        mstrNitIMC = ListaBcoRepublicaF5.FirstOrDefault.NIT_IMC
                    End If
                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "AbrirDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Generación del archivo
    ''' </summary>
    Private Async Sub GrabarArchivo()
        Try

            'Validar que exista información de los formularios
            If ListaBcoRepublicaF1.Count = 0 And ListaBcoRepublicaF2.Count = 0 And ListaBcoRepublicaF3.Count = 0 And ListaBcoRepublicaF4.Count = 0 And ListaBcoRepublicaF5.Count = 0 Then
                MessageBox.Show((DiccionarioEtiquetasPantalla("SININFORMACIONFORMULARIOS").Titulo))
                Exit Sub
            End If

            Dim STR_ARCHIVO_DEFECTO As String
            Dim intTipoOperacion As Integer
            Dim lngTotalRegistros As Long
            Dim ValorUSD1 As String
            Dim ValorUSD2 As String
            Dim TotalRegistros As Long
            Dim decSumaTotal As Decimal
            Dim Registros As String
            Dim strTIPO_REGISTRO_DETALLE As String = "2"
            Dim strCadenaActualizar As String = ""

            'Se obtiene el separados de decimale 
            Dim strSeparadorDecimal As String = Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator

            'variable que se llena copn las líneas del archivo
            Dim lstLineasArchivo As New List(Of String) ' Arreglo con el string para escribir en el archivo

            STR_ARCHIVO_DEFECTO = CStr(Format$(Year(cb.dtmFechaDesde), "0000")) + CStr(Format$(Month(cb.dtmFechaDesde), "00")) + CStr(Format$(Day(cb.dtmFechaDesde), "00")) + CStr(mstrNitIMC) + ".txt"

            Dim fileDialog = New System.Windows.Forms.SaveFileDialog()
            fileDialog.DefaultExt = ".txt"
            fileDialog.Filter = "Archivos *.txt (*.txt)|*.txt"
            fileDialog.FileName = STR_ARCHIVO_DEFECTO

            'Dialog para obtener la ruta de guardado del archivo
            Dim result = fileDialog.ShowDialog()
            Select Case result
                Case System.Windows.Forms.DialogResult.OK

                    strNombreArchivo = fileDialog.FileName

                    Dim objStream = fileDialog.OpenFile()

                    ' Se hace la construcción de las líneas del archivo
                    Using archivo As StreamWriter = New StreamWriter(objStream)

                        mstrDetalle = ""
                        lngTotalRegistros = 0

                        intTipoOperacion = 1 ' Se genera el archivo plano primero con los registros de operacion 1

                        '------------------------------------------------------------------------------------------
                        '----------------------------------- FORMULARIO UNO ---------------------------------------
                        '------------------------------------------------------------------------------------------

                        For intContReg = 0 To 3  'RECORRER POR TIPO DE OPERACION (INICIAL,MODIFICA,CAMBIO)
                            For Each objRegistro In ListaBcoRepublicaF1
                                If objRegistro.EXPORTAR = True Then ' Esta seleccionado para ser importado
                                    If objRegistro.TIPO_OPERACION = intTipoOperacion Then

                                        mstrDetalle = objRegistro.TIPO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_REGISTRO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_OPERACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_IMP
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_IDENT_IMP_DVIMPORTADOR
                                        mstrDetalle = mstrDetalle & objRegistro.NOM_IMPORTADOR
                                        mstrDetalle = mstrDetalle & objRegistro.COD_MONEDA_GIRO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_CAMBIO_A_USD
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERAL_CAMB_1
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_MONEDA_GIRO_1
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_USD_1
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERAL_CAMB_2
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_MONEDA_GIRO_2
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_USD_2
                                        mstrDetalle = mstrDetalle & objRegistro.ES_GLOBAL_S_N
                                        mstrDetalle = mstrDetalle & " "

                                        lstLineasArchivo.Add(mstrDetalle)

                                        TotalRegistros = TotalRegistros + 1
                                        'Total de VLR USD1
                                        ValorUSD1 = CStr(objRegistro.VALOR_USD_1)

                                        decSumaTotal = decSumaTotal + CDec(Strings.Left(ValorUSD1, 12) & strSeparadorDecimal & Strings.Right(ValorUSD1, 2))
                                        'Total de VLR USD2
                                        ValorUSD2 = CStr(objRegistro.VALOR_USD_2)
                                        decSumaTotal = decSumaTotal + CDec(Strings.Left(ValorUSD2, 12) & strSeparadorDecimal & Strings.Right(ValorUSD2, 2))

                                        'Se actualiza blnEnviado para indicar que el registro ya fue enviado
                                        strCadenaActualizar = strCadenaActualizar & objRegistro.NUMERO_DECLARACION & "|"

                                    End If
                                End If

                            Next

                            intTipoOperacion = intTipoOperacion + 1
                        Next

                        'Se actualizan los registros enviados
                        Await mdcProxy.ReporteriaLegal_BcoRepublica_ActualizarAsync(Formulario.Formulario1, strCadenaActualizar, Program.Usuario)

                        strCadenaActualizar = ""
                        intTipoOperacion = 1

                        '------------------------------------------------------------------------------------------
                        '----------------------------------- FORMULARIO DOS ---------------------------------------
                        '------------------------------------------------------------------------------------------

                        Dim ListaBcoRepublicaF2_Detalle2 As New List(Of CPX_tblBcoRepublica_Detalle2)

                        Dim objRespuesta2 = Await mdcProxy.ReporteriaLegal_BcoRepublica_Detalle2_ConsultarAsync(Formulario.Formulario2, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                        If Not IsNothing(objRespuesta2) Then
                            ListaBcoRepublicaF2_Detalle2 = objRespuesta2.Value
                        End If


                        For intContReg = 0 To 3  'RECORRER POR TIPO DE OPERACION (INICIAL,MODIFICA,CAMBIO)
                            For Each objRegistro In ListaBcoRepublicaF2
                                If objRegistro.EXPORTAR = True Then ' Esta seleccionado para ser importado
                                    If objRegistro.TIPO_OPERACION = intTipoOperacion Then

                                        mstrDetalle = objRegistro.TIPO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_REGISTRO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_OPERACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_EXP
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_IDENT_IMP_DVIMPORTADOR
                                        mstrDetalle = mstrDetalle & objRegistro.DVIMPORTADOR
                                        mstrDetalle = mstrDetalle & objRegistro.NOM_EXPORTADOR
                                        mstrDetalle = mstrDetalle & objRegistro.COD_MONEDA_REINT
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_MONEDA_REINT
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_CAMBIO_MON_A_USD
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_FOB
                                        mstrDetalle = mstrDetalle & objRegistro.VLR_REINT_X_REEMB_USD
                                        mstrDetalle = mstrDetalle & objRegistro.VLR_DEDUCCIONES
                                        mstrDetalle = mstrDetalle & objRegistro.VLR_REINT_NETO_USD
                                        mstrDetalle = mstrDetalle & objRegistro.ES_GLOBAL_S_N
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_DETALLES
                                        mstrDetalle = mstrDetalle & objRegistro.CORRECION_S_N

                                        lstLineasArchivo.Add(mstrDetalle)

                                        TotalRegistros = TotalRegistros + 1
                                        'Total de VLR USD1
                                        ValorUSD1 = CStr(objRegistro.VALOR_MONEDA_REINT)
                                        decSumaTotal = decSumaTotal + CDec(Strings.Left(ValorUSD1, 12) & strSeparadorDecimal & Strings.Right(ValorUSD1, 2))

                                        'Se actualiza blnEnviado para indicar que el registro ya fue enviado
                                        strCadenaActualizar = strCadenaActualizar & objRegistro.NUMERO_DECLARACION & "|"

                                        '------------------------------------------------------------------------------------------
                                        '----------------------------------- FORMULARIO DOS DETALLE -------------------------------
                                        '------------------------------------------------------------------------------------------

                                        For Each objRegistroDetalle In (From c In ListaBcoRepublicaF2_Detalle2 Where c.NUMERO_DECLARACION = objRegistro.NUMERO_DECLARACION Select c)
                                            mstrDetalle = objRegistro.TIPO_DECLARACION
                                            mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                            mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                            mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                            mstrDetalle = mstrDetalle & strTIPO_REGISTRO_DETALLE ' TIPO_REGISTRO PARA EL DETALLE ES 2

                                            'Campos del detalle
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.NUMERAL_CAMB
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.VALOR_REINT_USD

                                            lstLineasArchivo.Add(mstrDetalle)

                                            TotalRegistros = TotalRegistros + 1

                                        Next

                                    End If
                                End If

                            Next

                            intTipoOperacion = intTipoOperacion + 1
                        Next

                        'Se actualizan los registros enviados
                        Await mdcProxy.ReporteriaLegal_BcoRepublica_ActualizarAsync(Formulario.Formulario2, strCadenaActualizar, Program.Usuario)

                        strCadenaActualizar = ""
                        intTipoOperacion = 1

                        '------------------------------------------------------------------------------------------
                        '----------------------------------- FORMULARIO TRES --------------------------------------
                        '------------------------------------------------------------------------------------------

                        Dim ListaBcoRepublicaF3_Detalle1 As New List(Of CPX_tblBcoRepublicaF3_Detalle1)

                        Dim objRespuesta3 = Await mdcProxy.ReporteriaLegal_BcoRepublicaF3_Detalle1_ConsultarAsync(Formulario.Formulario3, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                        If Not IsNothing(objRespuesta3) Then
                            ListaBcoRepublicaF3_Detalle1 = objRespuesta3.Value
                        End If

                        For intContReg = 0 To 3  'RECORRER POR TIPO DE OPERACION (INICIAL,MODIFICA,CAMBIO)
                            For Each objRegistro In ListaBcoRepublicaF3
                                If objRegistro.EXPORTAR = True Then ' Esta seleccionado para ser importado
                                    If objRegistro.TIPO_OPERACION = intTipoOperacion Then

                                        mstrDetalle = objRegistro.TIPO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_REGISTRO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_OPERACION
                                        mstrDetalle = mstrDetalle & objRegistro.INGRESO_EGRESO_I_E
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_PRESTAMO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_REGISTRADO
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_IDENT_REG_DVREGISTRADO
                                        mstrDetalle = mstrDetalle & objRegistro.COD_MONEDA_CONTRAT
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_TOTAL_MON_CONTRAT
                                        mstrDetalle = mstrDetalle & objRegistro.COD_MONEDA_NEGOCIACION
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_TOTAL_MON_NEGOCIACION
                                        mstrDetalle = mstrDetalle & objRegistro.BASE_INTERES_360_365
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_CAM_MON_NEG_A_USD
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_TOTALUSD
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_DETALLES
                                        mstrDetalle = mstrDetalle & objRegistro.CORRECION_S_N

                                        lstLineasArchivo.Add(mstrDetalle)

                                        TotalRegistros = TotalRegistros + 1
                                        'Total de VLR USD1
                                        ValorUSD1 = CStr(objRegistro.VALOR_TOTALUSD)
                                        decSumaTotal = decSumaTotal + CDec(Strings.Left(ValorUSD1, 12) & strSeparadorDecimal & Strings.Right(ValorUSD1, 2))

                                        'Se actualiza blnEnviado para indicar que el registro ya fue enviado
                                        strCadenaActualizar = strCadenaActualizar & objRegistro.NUMERO_DECLARACION & "|"

                                        '------------------------------------------------------------------------------------------
                                        '----------------------------------- FORMULARIO TRES DETALLE ------------------------------
                                        '------------------------------------------------------------------------------------------

                                        For Each objRegistroDetalle In (From c In ListaBcoRepublicaF3_Detalle1 Where c.NUMERO_DECLARACION = objRegistro.NUMERO_DECLARACION Select c)
                                            mstrDetalle = objRegistro.TIPO_DECLARACION
                                            mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                            mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                            mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                            mstrDetalle = mstrDetalle & strTIPO_REGISTRO_DETALLE ' TIPO_REGISTRO PARA EL DETALLE ES 2

                                            'Campos del detalle
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.NUMERAL_CAMB

                                            ' Valor moneda Neg
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.VALOR_MONEDA_NEG

                                            ' Valor Mon contrat
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.VALOR_MONEDA_CONTRAT

                                            ' Valor USD
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.VALOR_USD

                                            mstrDetalle = mstrDetalle & objRegistroDetalle.Blancos   ' BLANCOS

                                            ' Valor base moneda contr
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.VLR_BASE_MONEDA_CONTR

                                            mstrDetalle = mstrDetalle & objRegistroDetalle.FECHA_INICIAL
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.FECHA_FINAL
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.NRO_DIAS

                                            'Tasa interes
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.TASA_INTERES

                                            lstLineasArchivo.Add(mstrDetalle)

                                            TotalRegistros = TotalRegistros + 1

                                        Next

                                    End If
                                End If

                            Next

                            intTipoOperacion = intTipoOperacion + 1
                        Next

                        'Se actualizan los registros enviados
                        Await mdcProxy.ReporteriaLegal_BcoRepublica_ActualizarAsync(Formulario.Formulario3, strCadenaActualizar, Program.Usuario)

                        strCadenaActualizar = ""
                        intTipoOperacion = 1

                        '------------------------------------------------------------------------------------------
                        '----------------------------------- FORMULARIO CUATRO ------------------------------------
                        '------------------------------------------------------------------------------------------

                        For intContReg = 0 To 3  'RECORRER POR TIPO DE OPERACION (INICIAL,MODIFICA,CAMBIO)
                            For Each objRegistro In ListaBcoRepublicaF4
                                If objRegistro.EXPORTAR = True Then ' Esta seleccionado para ser importado
                                    If objRegistro.TIPO_OPERACION = intTipoOperacion Then

                                        mstrDetalle = objRegistro.TIPO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_REGISTRO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_OPERACION
                                        mstrDetalle = mstrDetalle & objRegistro.INGRESO_EGRESO_I_E
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_DECLARANTE
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_IDENT_DEC_DVDECLARANTE
                                        mstrDetalle = mstrDetalle & objRegistro.NOMBRE_DECLARANTE
                                        mstrDetalle = mstrDetalle & objRegistro.TELEFONO_DECLARANTE
                                        mstrDetalle = mstrDetalle & objRegistro.DIRECCION_DECLARANTE
                                        mstrDetalle = mstrDetalle & objRegistro.CODIGO_CIUDAD_DECLARANTE
                                        mstrDetalle = mstrDetalle & objRegistro.CORREO_ELECTRONICO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_EMPRESA_RECEPTORA
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_EMPRESA_RECEPTORA_DV_RECEPTORA
                                        mstrDetalle = mstrDetalle & objRegistro.NOMBRE_EMP_RECEP_FONDO
                                        mstrDetalle = mstrDetalle & objRegistro.CODIGO_PAIS_EMP_REC
                                        mstrDetalle = mstrDetalle & objRegistro.CODIGO_CIUDAD_EMPR_RECEP_FONDO
                                        mstrDetalle = mstrDetalle & objRegistro.TELEFONO_EMPR_RECEP_FONDO
                                        mstrDetalle = mstrDetalle & objRegistro.CODIGO_CIIU_EMPR_RECEP_FONDO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_INVERSIONISTA
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_INVERSIONISTA_DV_INVERSIONISTA
                                        mstrDetalle = mstrDetalle & objRegistro.NOMBRE_INVERSIONISTA
                                        mstrDetalle = mstrDetalle & objRegistro.CODIGO_PAIS_INVERSIONISTA
                                        mstrDetalle = mstrDetalle & objRegistro.CODIGO_CIIU_INVERSIONISTA
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERAL_CAMB
                                        mstrDetalle = mstrDetalle & objRegistro.COD_MONEDA_GIRO_REINT
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_MONEDA_GIRO_REINT
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_ACCIONES
                                        mstrDetalle = mstrDetalle & objRegistro.INVERSION_PLAZOS
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_CAMBIO_DIV_USD
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_USD
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_CAMBIO_USD_COP
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_PESOS
                                        mstrDetalle = mstrDetalle & objRegistro.DESTINO_INVERSION
                                        mstrDetalle = mstrDetalle & " "

                                        'If Len(mstrDetalle) <> 440 Then
                                        '    MessageBox.Show("Error al generar el archivo F4, longitud debe ser de 440, actual: " & Len(mstrDetalle))
                                        'End If

                                        lstLineasArchivo.Add(mstrDetalle)

                                        TotalRegistros = TotalRegistros + 1
                                        'Total de VLR USD1
                                        ValorUSD1 = CStr(objRegistro.VALOR_USD)
                                        decSumaTotal = decSumaTotal + CDec(Strings.Left(ValorUSD1, 12) & strSeparadorDecimal & Strings.Right(ValorUSD1, 2))

                                        'Se actualiza blnEnviado para indicar que el registro ya fue enviado
                                        strCadenaActualizar = strCadenaActualizar & objRegistro.NUMERO_DECLARACION & "|"

                                    End If
                                End If

                            Next

                            intTipoOperacion = intTipoOperacion + 1
                        Next

                        'Se actualizan los registros enviados
                        Await mdcProxy.ReporteriaLegal_BcoRepublica_ActualizarAsync(Formulario.Formulario4, strCadenaActualizar, Program.Usuario)

                        strCadenaActualizar = ""
                        intTipoOperacion = 1

                        '------------------------------------------------------------------------------------------
                        '----------------------------------- FORMULARIO CINCO -------------------------------------
                        '------------------------------------------------------------------------------------------

                        Dim ListaBcoRepublicaF5_Detalle2 As New List(Of CPX_tblBcoRepublica_Detalle2)

                        Dim objRespuesta5 = Await mdcProxy.ReporteriaLegal_BcoRepublica_Detalle2_ConsultarAsync(Formulario.Formulario5, cb.dtmFechaDesde, cb.dtmFechaHasta, cb.strTipoEnviado, Program.Usuario)
                        If Not IsNothing(objRespuesta5) Then
                            ListaBcoRepublicaF5_Detalle2 = objRespuesta5.Value
                        End If


                        For intContReg = 0 To 3  'RECORRER POR TIPO DE OPERACION (INICIAL,MODIFICA,CAMBIO)
                            For Each objRegistro In ListaBcoRepublicaF5
                                If objRegistro.EXPORTAR = True Then ' Esta seleccionado para ser importado
                                    If objRegistro.TIPO_OPERACION = intTipoOperacion Then

                                        mstrDetalle = objRegistro.TIPO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_REGISTRO
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_OPERACION
                                        mstrDetalle = mstrDetalle & objRegistro.INGRESO_EGRESO_I_E
                                        mstrDetalle = mstrDetalle & objRegistro.NIT_IMC_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.FECHA_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECL_ANT
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_IDENT_COMPRA_VENDE
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_IDENT_COMPRA_VENDE
                                        mstrDetalle = mstrDetalle & objRegistro.NOMBRE_COMPRA_VENDE
                                        mstrDetalle = mstrDetalle & objRegistro.TELEFONO_COMPRA_VENDE
                                        mstrDetalle = mstrDetalle & objRegistro.DIRECCION_COMPRA_VENDE
                                        mstrDetalle = mstrDetalle & objRegistro.COD_MONEDA_GIRO_REINT
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_MONEDA_GIRO_REINT
                                        mstrDetalle = mstrDetalle & objRegistro.TIPO_CAMBIO_DIV_USD
                                        mstrDetalle = mstrDetalle & objRegistro.VALOR_TOTAL_USD
                                        mstrDetalle = mstrDetalle & objRegistro.ES_GLOBAL_S_N
                                        mstrDetalle = mstrDetalle & objRegistro.NUM_DETALLES
                                        mstrDetalle = mstrDetalle & objRegistro.CORRECION_S_N

                                        lstLineasArchivo.Add(mstrDetalle)

                                        TotalRegistros = TotalRegistros + 1

                                        ValorUSD1 = CStr(objRegistro.VALOR_TOTAL_USD)
                                        decSumaTotal = decSumaTotal + CDec(Strings.Left(ValorUSD1, 12) & strSeparadorDecimal & Strings.Right(ValorUSD1, 2))

                                        'Se actualiza blnEnviado para indicar que el registro ya fue enviado
                                        strCadenaActualizar = strCadenaActualizar & objRegistro.NUMERO_DECLARACION & "|"

                                        '------------------------------------------------------------------------------------------
                                        '----------------------------------- FORMULARIO CINCO DETALLE -----------------------------
                                        '------------------------------------------------------------------------------------------

                                        For Each objRegistroDetalle In (From c In ListaBcoRepublicaF5_Detalle2 Where c.NUMERO_DECLARACION = objRegistro.NUMERO_DECLARACION Select c)
                                            mstrDetalle = objRegistro.TIPO_DECLARACION
                                            mstrDetalle = mstrDetalle & objRegistro.NIT_IMC
                                            mstrDetalle = mstrDetalle & objRegistro.FECHA_DECLARACION
                                            mstrDetalle = mstrDetalle & objRegistro.NUMERO_DECLARACION
                                            mstrDetalle = mstrDetalle & strTIPO_REGISTRO_DETALLE ' TIPO_REGISTRO PARA EL DETALLE ES 2

                                            'Campos del detalle
                                            mstrDetalle = mstrDetalle & objRegistroDetalle.NUMERAL_CAMB

                                            mstrDetalle = mstrDetalle & objRegistroDetalle.VALOR_REINT_USD

                                            lstLineasArchivo.Add(mstrDetalle)

                                            TotalRegistros = TotalRegistros + 1

                                        Next

                                    End If
                                End If

                            Next

                            intTipoOperacion = intTipoOperacion + 1
                        Next

                        'Se actualizan los registros enviados
                        Await mdcProxy.ReporteriaLegal_BcoRepublica_ActualizarAsync(Formulario.Formulario5, strCadenaActualizar, Program.Usuario)

                        'REGISTRO DE CONTROL
                        mstrDetalle = "02"

                        mstrDetalle = mstrDetalle &
                               CStr(Format$(Year(cb.dtmFechaDesde), "0000")) + CStr(Format$(Month(cb.dtmFechaDesde), "00")) + CStr(Format$(Day(cb.dtmFechaDesde), "00")) _
                               + mstrNitIMC ' NIT_IMC

                        ' se coloca el total de registros
                        Registros = Format$(TotalRegistros, "000000")

                        mstrDetalle = mstrDetalle & Registros

                        ' Se coloca la suma total
                        Dim arrString As String() = Split(decSumaTotal.ToString, strSeparadorDecimal)

                        mstrDetalle = mstrDetalle & Strings.Right(Format(Int(arrString(0)), "000000000000"), 12) + Strings.Left(arrString(1), 2)


                        archivo.WriteLine(mstrDetalle)

                        For Each strLinea In lstLineasArchivo
                            archivo.WriteLine(strLinea)
                        Next

                    End Using

                    MessageBox.Show("El archivo se generó con éxito.")

                    Exit Select
                Case System.Windows.Forms.DialogResult.Cancel
                    Exit Select
                Case Else
            End Select
        Catch ex As Exception
            MessageBox.Show("Se presentó un error al grabar el archivo. " & ex.Message, "GrabarArchivo")
        End Try
    End Sub

#End Region


#Region "Comandos"

    ''' <summary>
    ''' Comando del botón para consultar la información
    ''' </summary>
    Private WithEvents _ConsultarCmd As RelayCommand
    Public ReadOnly Property ConsultarCmd() As RelayCommand
        Get
            If _ConsultarCmd Is Nothing Then
                _ConsultarCmd = New RelayCommand(AddressOf ConsultarBancoRepublica)
            End If
            Return _ConsultarCmd
        End Get
    End Property

    ''' <summary>
    ''' Comando para la generación del archivo
    ''' </summary>
    Private WithEvents _ArchivoCmd As RelayCommand
    Public ReadOnly Property ArchivoCmd() As RelayCommand
        Get
            If _ArchivoCmd Is Nothing Then
                _ArchivoCmd = New RelayCommand(AddressOf GrabarArchivo)
            End If
            Return _ArchivoCmd
        End Get
    End Property

#End Region



End Class

''' <summary>
''' Enumerador con los formularios
''' </summary>
Public Enum Formulario As Integer
    Formulario1 = 1
    Formulario2 = 2
    Formulario3 = 3
    Formulario4 = 4
    Formulario5 = 5
End Enum

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' <history>
''' 
''' </history>
Public Class CamposBusquedaBcoRepublica
    Implements INotifyPropertyChanged

    Private _dtmFechaDesde As DateTime
    Public Property dtmFechaDesde() As DateTime
        Get
            Return _dtmFechaDesde
        End Get
        Set(ByVal value As DateTime)
            _dtmFechaDesde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaDesde"))
        End Set
    End Property

    Private _dtmFechaHasta As DateTime
    Public Property dtmFechaHasta() As DateTime
        Get
            Return _dtmFechaHasta
        End Get
        Set(ByVal value As DateTime)
            _dtmFechaHasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaHasta"))
        End Set
    End Property

    Private _strTipoEnviado As String
    Public Property strTipoEnviado() As String
        Get
            Return _strTipoEnviado
        End Get
        Set(ByVal value As String)
            _strTipoEnviado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoEnviado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
