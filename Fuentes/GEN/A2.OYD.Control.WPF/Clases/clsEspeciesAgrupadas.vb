Public Class EspeciesAgrupadas

    Public Property Nemotecnico As String
    Public Property Especie As String
    Public Property Emisor As String
    Public Property Mercado As String
    Public Property EsAccion As Boolean
    Public Property CodTipoTasaFija As String
    Public Property TipoTasaFija As String
    Public Property IdIndicador As String
    Public Property Indicador As String

    Public Sub New(ByVal pstrNemotecnico As String,
                   ByVal pstrNombre As String,
                   ByVal pstrEmisor As String,
                   ByVal pstrMercado As String)
        Nemotecnico = pstrNemotecnico
        Especie = pstrNombre
        Emisor = pstrEmisor
        Mercado = pstrMercado
    End Sub

    Public Sub New(ByVal pstrNemotecnico As String,
                   ByVal pstrNombre As String,
                   ByVal pstrEmisor As String,
                   ByVal pstrMercado As String,
                   ByVal plogEsAccion As Boolean,
                   ByVal pstrCodTipoTasaFija As String,
                   ByVal pstrTipoTasaFija As String,
                   ByVal pstrIndicador As String,
                   ByVal pstrDescripcionIndicador As String)
        Nemotecnico = pstrNemotecnico
        Especie = pstrNombre
        Emisor = pstrEmisor
        Mercado = pstrMercado
        EsAccion = plogEsAccion
        CodTipoTasaFija = pstrCodTipoTasaFija
        TipoTasaFija = pstrTipoTasaFija
        IdIndicador = pstrIndicador
        Indicador = pstrDescripcionIndicador
    End Sub

End Class
