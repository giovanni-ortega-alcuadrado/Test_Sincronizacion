Option Strict On

Imports A2Utilidades.Utilidades

''' <summary>
''' Nombre:                 ClsConstantes
''' Objetivo:               Centralizar la declaracion de las constantes del proyecto.
''' Condiciones especiales: Se define Option Strict On para se deba declarar las constantes explícitamente especificando un tipo de datos. 
''' Desarrollado por:       Juan Carlos Soto Cruz. 2011/02/04.
''' </summary>
''' <remarks></remarks>
Public Class ClsConstantes

#Region "Constantes para maestros"
    ''' <summary>
    ''' Nombre:                 GByte_ErrorPersonalizado
    ''' Objetivo:               Contante definida para almacenar el codigo requerido por los Sp's para el error personalizado.
    ''' Condiciones especiales: 
    ''' Desarrollado por:       Juan Carlos Soto Cruz. 2011/02/04.
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GINT_ErrorPersonalizado As Byte = A2Utilidades.Utilidades.GINT_INICIO_MSGERR_SQL_PERSONALIZADO
#End Region

#Region "Constantes para Maker and Checker"
    Public Const GINT_MAKER_AND_CHEKER_POR_APROBAR As Byte = 0
    Public Const GINT_MAKER_AND_CHEKER_RECHAZADO As Byte = 1
    Public Const GINT_MAKER_AND_CHEKER_APROBADO As Byte = 2

    'ESTADOS

    Public Const DESC_ESTADO_1_INGRESADO_PENDIENTE_POR_APROBACION As String = "01-Ingresado, Pendiente por aprobacion"
    Public Const DESC_ESTADO_2_APROBADO_LUEGO_DE_INGRESO As String = "02-Aprobado, luego de ingreso"
    Public Const DESC_ESTADO_3_RECHAZADO_LUEGO_DE_INGRESO As String = "03-No aprobado, luego de ingresado"
    Public Const DESC_ESTADO_4_MODIFICADO_PENDIENTE_POR_APROBACION As String = "04-Modificado, Pendiente por aprobacion"
    Public Const DESC_ESTADO_5_APROBADO_LUEGO_DE_MODIFICADO As String = "05-Aprobado, luego de modificado"
    Public Const DESC_ESTADO_6_RECHAZADO_LUEGO_DE_MODIFICADO As String = "06-No aprobado, luego de modificado"
    Public Const DESC_ESTADO_7_RETIRADO_PENDIENTE_POR_APROBACION As String = "07-Retirado, pendiente de aprobacion"
    Public Const DESC_ESTADO_8_APROBADO_LUEGO_DE_RETIRO As String = "08-Aprobado, luego de retiro"
    Public Const DESC_ESTADO_9_RECHAZADO_LUEGO_DE_RETIRO As String = "09-No aprobado, luego de retirado"
    Public Const DESC_ESTADO_10_ACTIVADO_PENDIENTE_POR_APROBACION As String = "10-Activado, Pendiente de aprobacion"
    Public Const DESC_ESTADO_11_APROBADO_LUEGO_DE_ACTIVADO As String = "11-Aprobado, Luego de activado"
    Public Const DESC_ESTADO_12_RECHAZADO_LUEGO_DE_ACTIVADO As String = "12-No aprobado, luego de activado"

#End Region

#Region "Constantes para Tesoreria"
    Public Const GINT_TESORERIA_RECIBO_CAJA As String = "RC"
    Public Const GINT_TESORERIA_COMPROBANTE_EGRESO As String = "CE"
    Public Const GINT_TESORERIA_NOTAS_CONTABLES As String = "N"

    Public Const FORMA_PAGO_RECIBO_CAJA_PAGO_DECEVAL As String = "T" 'Antes "C" (Cheque). Ahora Transferencia, por solicitud de la Firma. CGA, 12.FEbrero.10

#End Region

#Region "Tipo Documento por Defecto"
    Public Const GSTR_TIPO_DOCUMENTO_NIT As String = "N"
#End Region



End Class
