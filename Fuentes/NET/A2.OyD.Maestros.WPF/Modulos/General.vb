Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web

Module General

    Public Enum EnumVersionAplicacionCliente
        G 'Genérico
        C 'City
    End Enum

    Friend Function inicializarProxyMaestros() As MaestrosDomainContext
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            Return (New MaestrosDomainContext())
        Else
            Return (New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocioMaestros(Program.TipoServicio.URLServicioMaestros.ToString()))))
        End If
    End Function

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

End Module
