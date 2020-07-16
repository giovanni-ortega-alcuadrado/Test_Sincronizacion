Imports A2Utilidades.Cifrar
Public Class DatosVisorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim strStringConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("A2.OyD.OYDServer.RIA.Web.My.MySettings.dbOYDConnectionString").ConnectionString

            dsDatosVisor.ConnectionString = descifrar(strStringConn)

            If Not Request.QueryString("idOrden") Is Nothing Then
                dsDatosVisor.SelectParameters(0).DefaultValue = Integer.Parse(Request.QueryString("idOrden").ToString())
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


    End Sub

End Class