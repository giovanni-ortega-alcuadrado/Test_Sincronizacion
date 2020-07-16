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
    Public Const ERROR_PERSONALIZADO_SQLSERVER As Byte = A2Utilidades.Utilidades.GINT_INICIO_MSGERR_SQL_PERSONALIZADO  ' Contante definida para el código del error personalizado en los procedimientos almacenados.
    Public Const CONSULTAR_DATOS_POR_DEFECTO As String = "DEFAULT"   ' Constante definida para enviar como parámetro cuando se solicitan los datos por defecto para un nuevo registro
    Public Const IDENTIFICADOR_VALIDACION_A2 As String = "validacionA2"   ' Constante definida marcar los errores generados a partir de la validación de inconsistencias en procedimientos almacenados
    Public Const IDENTIFICADOR_EXITOSO_MENSAJE As String = "EXITOSO"   ' Constante definida identificar sí el mensaje de validación retorna un mensaje exitoso.
#End Region

End Class
