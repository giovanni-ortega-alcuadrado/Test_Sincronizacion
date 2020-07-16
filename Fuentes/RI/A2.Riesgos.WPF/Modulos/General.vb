Imports Telerik.Windows.Controls
Imports A2.Riesgos.RIA.Web
Imports OpenRiaServices.DomainServices.Client

Module General
    Friend Enum ValoresUserState
        Combos
        CombosEspecificos
        Listar
        Consultar
        Filtrar
        Buscar
        Ingresar
        Actualizar
        Borrar
        PorDefecto
    End Enum

    Friend Function inicializarProxyRiesgos() As RiesgosDomainContext
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            Return (New RiesgosDomainContext())
        Else
            Return (New RiesgosDomainContext(New System.Uri(Program.RutaServicioRiesgos)))
        End If
    End Function

End Module

