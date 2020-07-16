Imports System.Collections.ObjectModel
Imports System.Configuration
Imports System.Security.Principal
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Recursos

Public Class clsRecursos
    Public Shared Function SimularURL_ServiciosRIA(ByVal pstrTipoServicio As String) As String
        Dim strUrlRespuesta As String = "http://a2webdllo:5025/OYDServiciosRIA/"

        Select Case pstrTipoServicio
            Case "URLServicioMaestros"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-MaestrosDomainService.svc"
            Case "URLServicioCustodias"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-TitulosDomainService.svc"
            Case "URLServicioBolsa"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-BolsaDomainService.svc"
            Case "URLServicioClientes"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-ClientesDomainService.svc"
            Case "URLServicioTesoreria"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-TesoreriaDomainService.svc"
            Case "URLServicioUtilidadesOYD"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-UtilidadesDomainService.svc"
            Case "URLServicioImportaciones"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-ImportacionesDomainService.svc"
            Case "URLServicioOrdenes"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OrdenesDomainService.svc"
            Case "URLServicioCitiBank"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-CitiBankDomainService.svc"
            Case "URLServicioMILA"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-MILADomainService.svc"
            Case "URLServicioOTC"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OTCDomainService.svc"
            Case "URLVisorSeteador"
                strUrlRespuesta += "OYDPLUS/DatosVisorPage.aspx?IdOrden="
            Case "URLServicioYankees"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-YankeesDomainService.svc"
            Case "URLServicioPLUSMaestros"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSMaestrosDomainService.svc"
            Case "URLServicioPLUSOrdenes"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSOrdenesDomainService.svc"
            Case "URLServicioPLUSOrdenesBolsa"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSOrdenesBolsaDomainService.svc"
            Case "URLServicioPLUSOrdenesOF"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSOrdenesOFDomainService.svc"
            Case "URLServicioPLUSOrdenesDerivados"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSOrdenesDerivadosDomainService.svc"
            Case "URLServicioPLUSOrdenesDivisas"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSOrdenesDivisasDomainService.svc"
            Case "URLServicioPLUSTesoreria"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSTesoreriaDomainService.svc"
            Case "URLServicioPLUSUtilidades"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OYDPLUSUtilidadesDomainService.svc"
            Case "URLServicioADINOrdenes"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-ADINOrdenesBolsaDomainService.svc"
            Case "URLServicioADINPortafolio"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-PortafolioDomainService.svc"
            Case "URLServicioPLUSGarantias"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OyDPLUSGarantiasDomainService.svc"
            Case "URLServicioCFCalculosFinancieros"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-CalculosFinancierosDomainService.svc"
            Case "URLServicioCFCodificacionContable"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-CodificacionContableDomainService.svc"
            Case "URLServicioCFUtilidades"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-UtilidadesCFDomainService.svc"
            Case "URLServicioCFEspecies"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-EspeciesCFDomainService.svc"
            Case "URLServicioCFMaestros"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-MaestrosCFDomainService.svc"
            Case "URLServicioCFPortafolio"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-PortafolioDomainService.svc"
            Case "URLServicioCFTitulosNet"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-TitulosNetDomainService.svc"
            Case "URLServicioCFOperaciones"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OperacionesCFDomainService.svc"
            Case "URLServicioOperaciones"
                strUrlRespuesta += "Services/A2-OyD-OYDServer-RIA-Web-OperacionesDomainService.svc"
        End Select

        Return strUrlRespuesta
    End Function
End Class
