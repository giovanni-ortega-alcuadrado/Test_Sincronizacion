'A2SingletonObject
'Archivo: A2SingletonObject.vb
'Creado el : 31/10/2014 08:00am
'Propiedad de Alcuadrado S.A. 2014
'
'Creado por Jose Walter Sierra 

'El patrón de diseño singleton (instancia única) está diseñado para restringir la creación de objetos pertenecientes 
'a una clase o el valor de un tipo a un único objeto. 
'Su intención consiste en garantizar que una clase sólo tenga una instancia y proporcionar un punto de acceso global a ella.

''' <summary>
''' Clase que implementa el patrón singleton
''' </summary>
''' <remarks></remarks>
Public Class A2SingletonObject
    Private Shared m_instance As A2SingletonObject = Nothing
    Private Shared ReadOnly padlock As New Object()

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance() As A2SingletonObject
        Get
            If m_instance Is Nothing Then
                'evitar errores de multihilo
                SyncLock padlock
                    If m_instance Is Nothing Then
                        m_instance = New A2SingletonObject()
                    End If
                End SyncLock
            End If

            Return m_instance
        End Get
    End Property

    'propiedad para manejar todo el tema de cache en el IsolatedStorage
    'Private _cache As IA2CacheData
    'Public ReadOnly Property A2Cache As IA2CacheData
    '    Get
    '        If _cache Is Nothing Then
    '            _cache = New A2CacheIsolatedStorage()
    '        End If
    '        Return _cache
    '    End Get
    'End Property

End Class
