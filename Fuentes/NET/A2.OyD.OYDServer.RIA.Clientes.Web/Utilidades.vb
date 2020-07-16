Imports System.IO
Imports A2Utilidades.Utilidades
Imports System.Web
Imports System.Data.SqlClient




Module Utilidades

    Public Function CampoTabla(ByVal pstrIdCondicion As String, ByVal pstrCampo As String, ByVal pstrTabla As String, ByVal pstrCondicion As String) As String
        Dim dcUtil As New OyDClientesDataContext
        Dim strRetorno As String = String.Empty

        Dim obj = dcUtil.uspOyDNet_Utilidades_CampoTabla(pstrIdCondicion, pstrCampo, pstrTabla, pstrCondicion).FirstOrDefault()

        If Not obj Is Nothing Then
            strRetorno = obj.Resultado.ToString()
        End If

        Return strRetorno
    End Function

End Module
