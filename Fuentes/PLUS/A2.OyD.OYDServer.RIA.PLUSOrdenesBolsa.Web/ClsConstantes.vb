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

End Class
