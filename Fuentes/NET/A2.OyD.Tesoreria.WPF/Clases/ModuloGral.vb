Imports Telerik.Windows.Controls
Module ModuloGral

    Public decSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator
    Public milSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator
  

    Public Const GSTR_TODOS = "(TODOS)"
    Public Const GSTR_TODOS_RETORNO = "TDS"
    Public Const GSTR_NOMBRE_TODOS = "(TODOS)"
    Public Const GSTR_ID_TODOS = "-1"
    Public GSTR_CLIENTE As String
    Public GSTR_TERCERO As String
    Public GSTR_CUENTA_REGISTRADA As String
    Public GSTR_CUENTA_NO_REGISTRADA As String
    Public Const GSTR_DIRECCION_INSCRITA As String = "DirI"
    Public Const GSTR_DIRECCION_NO_INSCRITA As String = "DirNI"
    Public Const GSTR_CHEQUE As String = "C"
    Public Const GSTR_EFECTIVO As String = "E"
    Public Const GSTR_CONSIGNACION_AMBAS As String = "A"
    Public Const GSTR_TRANSFERENCIA As String = "T"
    Public Const GSTR_CONSIGNACIONES As String = "CG"
    Public Const GSTR_CARTERASCOLECTIVAS As String = "CC"
    Public Const GSTR_OYD As String = "OYD"
    Public Const GSTR_INTERNOS As String = "I"
    Public Const GSTR_BLOQUEO_RECURSOS As String = "BS"
    Public Const GSTR_OPERACIONES_ESPECIALES As String = "OE"
    Public Const GSTR_CHEQUE_GERENCIA As String = "CG"
    Public Const GSTR_ORDENGIRO As String = "CE"
    Public Const GSTR_ORDENFONDOS As String = "FO"
    Public Const GSTR_ORDENRECIBO As String = "RC"
    Public Const GSTR_ORDENRECIBO_CARGOPAGOA As String = "CP"
    Public Const GSTR_ORDENRECIBO_CARGOPAGOAFONDOS As String = "CPF"
    Public Const GSTR_ORDENRECIBO_CARGOPAGOAFONDOSADICION As String = "CPFAD"
    Public Const GSTR_ORDENRECIBO_CARGOPAGOAFONDOSCONSTITUCION As String = "CPFCO"
    Public strFormapagoImportacion As String
    Public strNombreArchivoCarga As String
    Public GSTR_SERVICIO_DOCUMENTOS As String = ""
    Public GSTR_CONTROL_CANJE_CHEQUE As String = ""
    Public dblGMF_E As Decimal = 0
    Public dblGMF_D As Decimal = 0
    Public Const GSTR_EDITARDETALLE_Plus = "EDITAR"
    Public Const GSTR_NUEVODETALLE_Plus = "NUEVO"
    Public Const GSTR_PENDIENTEAPROBACION_Plus = "V"
    Public Const GSTR_CUMPLIDA_Plus = "C"
    Public Const GSTR_PENDIENTE_Plus = "P"
    Public Const GSTR_FUTURA_Plus = "F"
    Public Const GSTR_ANULADA_Plus = "A"
    Public Const GSTR_PROGRAMADA_Plus = "R"
    Public Const GSTR_DIVIDENDOS_Plus = "D"
    Public Const GSTR_PENDIENTE_Plus_Detalle = "Pendiente"
    Public Const GSTR_DESCRIPCION_CHEQUE_GERENCIA = "Cheque Gerencia"
    Public Const GSTR_DESCRIPCION_CHEQUE = "Cheque"
    Public Const GSTR_DESCRIPCION_TRANSFERENCIA = "Transferencia"
    Public Const GSTR_DESCRIPCION_CARTERACOLECTIVA = "Cartera Colectiva"
    Public Const GSTR_DESCRIPCION_INTERNO = "Interno"
    Public Const GSTR_CRUCE_SENCILLO = "CS"
    Public Const GSTR_CRUCE_SENCILLO_DESCRIPCION = "Cruce Sencillo"
    Public Const GSTR_CRUCE_PRIMERBENEFICIARIO = "CB"
    Public Const GSTR_CRUCE_PRIMERBENEFICIARIO_DESCRIPCION = "Para Consignar a 1er Beneficiario"
    Public Const GSTR_CRUCE_VENTANILLA_PRIMERBENEFICIARIO = "CV"
    Public Const GSTR_CRUCE_VENTANILLA_PRIMERBENEFICIARIO_DESCRIPCION = "Ventanilla 1er Beneficiario"
    Public Const GSTR_GMF_ENCIMA = "E"
    Public Const GSTR_GMF_DEBAJO = "D"
    Public Const GSTR_OT = "OT"
    Public Const GSTR_OTD = "OTD"
    Public Const GSTR_PENDIENTES = "Pendientes"
    Public Const GSTR_POR_APROBAR = "Por Aprobar"
    Public Const GSTR_ANULADAS = "Anuladas"
    Public Const GSTR_DUPLICAR = "Duplicar"
    Public Const GSTR_PROGRAMADAS = "Programadas"
    Public Const GSTR_DIVIDENDOS = "Dividendos"
    Public Const GSTR_CLIENTETRAECHEQUE = "Cliente trae el cheque"
    Public Const GSTR_OTRADIRECCIONCHEQUE = "Recoger CHEQUE en otra dirección"
    Public Const GSTR_DESCRIPCION_EFECTIVO = "Efectivo"
    Public Const GSTR_FONDOS_TIPOPRODUCTO = "F"
    Public Const GSTR_FONDOS_TITULO_CARTERASCOLECTIVAS = "Carteras Colectivas"
    Public Const GSTR_FONDOS_TITULO_OYD = "OyD"
    Public Const GSTR_FONDOS_TITULO_BLOQUEORECURSOS = "Bloqueo o liberar recursos"
    Public Const GSTR_FONDOS_TITULO_OPERACIONESESPECIALES = "Operaciones especiales"
    Public Const GSTR_FONDOS_TIPOACCION_CONSTITUCION = "CO"
    Public Const GSTR_FONDOS_TIPOACCION_ADICION = "AD"
    Public Const GSTR_TIPOOPEESPECIAL_OPERACIONESPECIAL = "OE"


    Public Enum EstadosAdmonFacturasFirmasContraparte
        I '//Impresa
        A '// Anulada
        P '// Pendiente
    End Enum

End Module

