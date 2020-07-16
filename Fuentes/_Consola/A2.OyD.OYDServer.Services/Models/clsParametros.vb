Imports Newtonsoft.Json

Public Class clsParam_Scripts

    <JsonProperty("IDC", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIdCompania As Nullable(Of Integer)

    <JsonProperty("IDS", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pintIDScript As Integer

    <JsonProperty("NOM", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrNombreScript As String

    <JsonProperty("PAR", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrParametros As String

    <JsonProperty("US", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrUsuario As String

    <JsonProperty("MA", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrMaquina As String

    <JsonProperty("INF", NullValueHandling:=NullValueHandling.Ignore)>
    Public Property pstrInfoConexion As String

End Class
