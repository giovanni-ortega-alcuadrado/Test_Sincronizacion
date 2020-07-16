Imports Telerik.Windows.Controls
Imports Newtonsoft.Json

'Namespace A2OYDPLUSOrdenesBolsa

Public Class clsNotificacionesOYDPLUS

    <JsonProperty("IDRegistro", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property IDRegistro As Integer

    <JsonProperty("NroRegistro", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property NroRegistro As Integer

    <JsonProperty("Estado", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Estado As String

    <JsonProperty("DescripcionEstado", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property DescripcionEstado As String

    <JsonProperty("EstadoMostrar", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property EstadoMostrar As String

    <JsonProperty("ListaDescripciones", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property ListaDescripciones As String

    <JsonProperty("objListaDescripciones", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property objListaDescripciones As List(Of DetalleInformacionMostrar)

    ''' <summary>
    ''' Deserializa un objecto tipo clsNotificacionTicket serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo clsNotificacionTicket serializado con JSON</param>
    ''' <returns>Objeto tipo clsNotificacionTicket</returns>
    ''' <remarks></remarks>
    Public Shared Function Deserialize(pstrInfoNotificacion As String) As clsNotificacionesOYDPLUS
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(clsNotificacionesOYDPLUS))
    End Function

    ''' <summary>
    ''' Deserializa una lista del objecto tipo clsNotificacionTicket serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo List Of clsNotificacionTicket serializado con JSON</param>
    ''' <returns>Objeto tipo List Of clsNotificacionTicket</returns>
    ''' <remarks></remarks>
    Public Shared Function DeserializeLista(pstrInfoNotificacion As String) As List(Of clsNotificacionesOYDPLUS)
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(List(Of clsNotificacionesOYDPLUS)))
    End Function

    ''' <summary>
    ''' Retorna la instancia actual serializada con JSON.
    ''' </summary>
    ''' <returns>string que contiene la información serializada de la instancia actual</returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return Newtonsoft.Json.JsonConvert.SerializeObject(Me)
    End Function

End Class

Public Class DetalleInformacionMostrar

    <JsonProperty("Codigo", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Codigo As String

    <JsonProperty("Descripcion", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property Descripcion As String

    ''' <summary>
    ''' Deserializa un objecto tipo clsNotificacionTicket serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo clsNotificacionTicket serializado con JSON</param>
    ''' <returns>Objeto tipo clsNotificacionTicket</returns>
    ''' <remarks></remarks>
    Public Shared Function Deserialize(pstrInfoNotificacion As String) As DetalleInformacionMostrar
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(DetalleInformacionMostrar))
    End Function

    ''' <summary>
    ''' Deserializa una lista del objecto tipo clsNotificacionTicket serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo List Of clsNotificacionTicket serializado con JSON</param>
    ''' <returns>Objeto tipo List Of clsNotificacionTicket</returns>
    ''' <remarks></remarks>
    Public Shared Function DeserializeLista(pstrInfoNotificacion As String) As List(Of DetalleInformacionMostrar)
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(List(Of DetalleInformacionMostrar)))
    End Function

    ''' <summary>
    ''' Retorna la instancia actual serializada con JSON.
    ''' </summary>
    ''' <returns>string que contiene la información serializada de la instancia actual</returns>
    ''' <remarks></remarks>
    Public Overrides Function ToString() As String
        Return Newtonsoft.Json.JsonConvert.SerializeObject(Me)
    End Function

End Class

'End Namespace
