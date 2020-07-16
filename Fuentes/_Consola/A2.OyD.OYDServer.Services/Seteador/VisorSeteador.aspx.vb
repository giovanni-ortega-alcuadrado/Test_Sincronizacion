Imports A2Utilidades.Cifrar
Public Class VisorSeteador
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim strStringConn = System.Web.Configuration.WebConfigurationManager.ConnectionStrings("A2.OyD.OYDServer.RIA.Web.My.MySettings.dbOYDConnectionString").ConnectionString

            dsDatosVisor.ConnectionString = descifrar(strStringConn)

            If Not Request.QueryString("idOrden") Is Nothing Then
                Dim strIdOrden = Request.QueryString("idOrden").ToString()
                dsDatosVisor.SelectParameters(0).DefaultValue = Integer.Parse(strIdOrden)
                lblServicioOK.Visible = False
            Else
                lblServicioOK.Visible = True
            End If

        Catch ex As Exception
            lblServicioOK.Visible = False
            Response.Write(ex.Message)
        End Try
    End Sub

End Class