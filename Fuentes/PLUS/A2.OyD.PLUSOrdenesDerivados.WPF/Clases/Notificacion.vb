Imports Telerik.Windows.Controls
Imports Newtonsoft.Json

'Namespace A2OYDPLUSOrdenesDerivados

Public Class clsSAENotificacionOperacion

    <JsonProperty("CR", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strCodigoReceptor As String

    <JsonProperty("CO", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strClaseOrden As String

    <JsonProperty("TO", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strTipoOperacion As String

    ''' <summary>
    ''' Deserializa un objecto tipo clsSAENotificacionOperacion serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo clsSAENotificacionOperacion serializado con JSON</param>
    ''' <returns>Objeto tipo clsSAENotificacionOperacion</returns>
    ''' <remarks></remarks>
    Public Shared Function Deserialize(pstrInfoNotificacion As String) As clsSAENotificacionOperacion
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(clsSAENotificacionOperacion))
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

Public Class clsNotificacionTicket

    <JsonProperty("RE", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strIDReceptor As String

    <JsonProperty("EM", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property intIdEmisor As Integer

    <JsonProperty("ME", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property intIdMesa As Integer

    <JsonProperty("ES", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strEspecie As String

    <JsonProperty("CU", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property dtmCumplimiento As DateTime

    <JsonProperty("TP", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strTipoProducto As String

    <JsonProperty("CL", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strIDCliente As String

    <JsonProperty("RB", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property strIDReceptorB As String

    ''' <summary>
    ''' Deserializa un objecto tipo clsNotificacionTicket serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo clsNotificacionTicket serializado con JSON</param>
    ''' <returns>Objeto tipo clsNotificacionTicket</returns>
    ''' <remarks></remarks>
    Public Shared Function Deserialize(pstrInfoNotificacion As String) As clsNotificacionTicket
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(clsNotificacionTicket))
    End Function

    ''' <summary>
    ''' Deserializa una lista del objecto tipo clsNotificacionTicket serializado con JSON.
    ''' </summary>
    ''' <param name="pstrInfoNotificacion">Objeto tipo List Of clsNotificacionTicket serializado con JSON</param>
    ''' <returns>Objeto tipo List Of clsNotificacionTicket</returns>
    ''' <remarks></remarks>
    Public Shared Function DeserializeLista(pstrInfoNotificacion As String) As List(Of clsNotificacionTicket)
        Return Newtonsoft.Json.JsonConvert.DeserializeObject(pstrInfoNotificacion, GetType(List(Of clsNotificacionTicket)))
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
