'IA2CacheData
'Archivo: IA2CacheData.vb
'Creado el : 31/10/2014 08:00am
'Propiedad de Alcuadrado S.A. 2014
'
'Creado por Jose Walter Sierra 

'Interfaz con metodos para el manejo de cache, sin importar la estrategia de guardado (IsolatedStorage, database, FileSystem)

Public Interface IA2CacheData

    ''' <summary>
    ''' Funcion anonima para leer un valor de la cache
    ''' </summary>
    ''' <typeparam name="Tipo">Tipo de dato esperado</typeparam>
    ''' <param name="key">Item</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function LeerItem(Of Tipo)(ByVal key As String) As Tipo
    ''' <summary>
    ''' Funcion para verificar si existe un valor en la cache
    ''' </summary>
    ''' <param name="key">Item</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ExisteItem(ByVal key As String) As Boolean
    ''' <summary>
    ''' Procedimiento anonimo para Escribir un valor en la cache
    ''' </summary>
    ''' <typeparam name="Tipo">Tipo de dato a grabar</typeparam>
    ''' <param name="key">Item</param>
    ''' <param name="valor">valor a grabar</param>
    ''' <remarks></remarks>
    Sub EscribeItem(Of Tipo)(ByVal key As String, ByVal valor As Tipo)
    ''' <summary>
    ''' Procedimiento para eliminar un valor de la cache
    ''' </summary>
    ''' <param name="key">Item</param>
    ''' <remarks></remarks>
    Sub EliminaItem(ByVal key As String)

End Interface
