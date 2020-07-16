'A2CacheIsolatedStorage
'Archivo: A2CacheIsolatedStorage.vb
'Creado el : 31/10/2014 08:00am
'Propiedad de Alcuadrado S.A. 2014
'
'Creado por Jose Walter Sierra 

'Clase que implemeta la interfaz IA2CacheData, tomando como estrategia de guardado el IsolatedStorage

Imports System.IO.IsolatedStorage
Imports Newtonsoft.Json

'clase para usar la cache con el IsolatedStorage
Public Class A2CacheIsolatedStorage
    Implements IA2CacheData

#Region "IA2CacheData"

    ''' <summary>
    ''' Funcion anonima para leer un valor de la cache
    ''' </summary>
    ''' <typeparam name="Tipo">Tipo de dato esperado</typeparam>
    ''' <param name="key">Item</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LeerItem(Of Tipo)(ByVal key As String) As Tipo Implements IA2CacheData.LeerItem
        Return LeerItemData(Of Tipo)(Format(key))
    End Function

    ''' <summary>
    ''' Funcion anonima para verificar si existe un valor en la cache
    ''' </summary>
    ''' <param name="key">Item</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExisteItem(ByVal key As String) As Boolean Implements IA2CacheData.ExisteItem
        Return ExisteItemData(Format(key))
    End Function

    ''' <summary>
    ''' Procedimiento anonimo para Escribir un valor en la cache
    ''' </summary>
    ''' <typeparam name="Tipo">Tipo de dato a grabar</typeparam>
    ''' <param name="key">Item</param>
    ''' <param name="valor">valor a grabar</param>
    ''' <remarks></remarks>
    Public Sub EscribeItem(Of Tipo)(ByVal key As String, ByVal valor As Tipo) Implements IA2CacheData.EscribeItem
        EscribeItemData(Format(key), valor)
    End Sub

    ''' <summary>
    ''' Procedimiento para eliminar un valor de la cache
    ''' </summary>
    ''' <param name="key">Item</param>
    ''' <remarks></remarks>
    Public Sub EliminaItem(ByVal key As String) Implements IA2CacheData.EliminaItem
        EliminaItemData(Format(key))
    End Sub

#End Region

    'function para concatenar al key de la cache el programa
    'esto con el objetivo de usar cache por cada programa registrado en la consola
    Private Function Format(ByVal key As String) As String
        Return String.Concat(key, "_", A2Utilidades.Program.Aplicacion)
    End Function

    Private Function ExisteItemData(ByVal key As String) As Boolean
        If IsolatedStorageFile.IsEnabled Then
            'verifico si el valor existe
            Dim userSettings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings
            Return userSettings.Contains(key)
        Else
            Return False
        End If
    End Function

    Private Function LeerItemData(Of Tipo)(ByVal key As String) As Tipo
        If IsolatedStorageFile.IsEnabled Then
            Dim userSettings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings
            If userSettings.Contains(key) Then
                'leo el valor de la cache
                Return DeserializeSetting(Of Tipo)(userSettings(key))
            Else
                Return Nothing
            End If
        End If
    End Function

    Private Sub EscribeItemData(Of Tipo)(ByVal key As String, ByVal value As Tipo)
        If IsolatedStorageFile.IsEnabled Then
            Dim userSettings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings
            'escribo el valor en la cache
            If userSettings.Contains(key) Then
                userSettings(key) = SerializeSetting(value)
            Else
                userSettings.Add(key, SerializeSetting(value))
            End If
            GuardarSetting(userSettings)
        End If
    End Sub

    Private Sub EliminaItemData(key As String)
        If IsolatedStorageFile.IsEnabled Then
            Dim userSettings As IsolatedStorageSettings = IsolatedStorageSettings.ApplicationSettings
            'elimino un valor de la cache
            userSettings.Remove(key)
            GuardarSetting(userSettings)
        End If
    End Sub

    'funcion que determina si es necesario aumentar el espacio de quota del isolated storage
    Private Sub GuardarSetting(ByRef storage As IsolatedStorageSettings)
        Try
            Using store = IsolatedStorageFile.GetUserStoreForApplication()
                'aumento 20MB
                Dim spaceToAdd As Int64 = 20971520
                Dim curAvail As Int64 = store.AvailableFreeSpace
                'si el espacio disponible es menor a 1MB incremento quota
                If (curAvail < 1048576) Then
                    'solicito el incremento
                    If store.IncreaseQuotaTo((store.Quota + spaceToAdd)) Then
                        'si el usuario aprueba el incremento aumento la cuota
                        'si no se aprueba el valor no se guarda
                        storage.Save()
                    End If
                Else
                    storage.Save()
                End If
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub

    'funcion para serializar en json y encriptar el texto resultante
    Private Function SerializeSetting(ByVal data As Object) As String
        'convierto el texto a json
        Dim strData As String = JsonConvert.SerializeObject(data)
        Return strData
    End Function

    'funcion para desencriptar el texto guardado y deserializar el json a un tipo epecifico
    Private Function DeserializeSetting(Of Tipo)(ByVal data As Object) As Tipo
        'deserializo de json a un tipo especifico
        Return JsonConvert.DeserializeObject(Of Tipo)(data)
    End Function

End Class

