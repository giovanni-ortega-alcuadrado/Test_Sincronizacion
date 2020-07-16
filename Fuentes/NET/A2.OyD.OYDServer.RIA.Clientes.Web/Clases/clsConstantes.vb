''' <summary>
''' Constantes globales para los servicios de datos RIA
''' </summary>
''' <history>
''' Creado por       : Juan Carlos Soto.
''' Descripción      : Creacion.
''' Fecha            : Enero 23/2014
''' Pruebas CB       : Juan Carlos Soto Cruz - Enero 23/2014 - Resultado Ok 
''' </history>
Public Class Constantes

#Region "Constantes generales"

    Public Const ERROR_PERSONALIZADO_SQLSERVER As Byte = A2Utilidades.Utilidades.GINT_INICIO_MSGERR_SQL_PERSONALIZADO  ' Contante definida para el código del error personalizado en los procedimientos almacenados.
    Public Const CONSULTAR_DATOS_POR_DEFECTO As String = "DEFAULT"   ' Constante definida para enviar como parámetro cuando se solicitan los datos por defecto para un nuevo registro
    Public Const IDENTIFICADOR_VALIDACION_A2 As String = "validacionA2"   ' Constante definida marcar los errores generados a partir de la validación de inconsistencias en procedimientos almacenados
    Public Const IDENTIFICADOR_EXITOSO_MENSAJE As String = "EXITOSO"   ' Constante definida identificar sí el mensaje de validación retorna un mensaje exitoso.

#End Region

End Class
