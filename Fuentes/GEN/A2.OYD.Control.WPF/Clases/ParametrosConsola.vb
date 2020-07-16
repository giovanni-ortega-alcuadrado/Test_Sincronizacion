Imports System.Collections.ObjectModel
Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades

Public Class ParametrosConsola
    Public Async Function actualizarParametrosConsola() As Task(Of Boolean)
        Return (Await Program.leerParametroAppConsola(Program.Aplicacion, Program.VersionAplicacion, Program.Division))
    End Function

    Public Async Function Plataforma_ConsultarCombosGenericos() As Task(Of Boolean)
        'Carga los combos genericos de la aplicación.
        Return Await Program.Plataforma_ConsultarCombosGenericos(Program.Aplicacion, Program.VersionAplicacion, Program.Division)
    End Function

    Public Async Function ValidarSeguridadCliente() As Task(Of String)
        Return (Await Program.Seguridad_ValidarUsuario())
    End Function

    Public Async Function ValidarIPCliente() As Task(Of String)
        Return (Await Program.Seguridad_ObtenerIP())
    End Function

    Public Async Function ValidarUsuarioCliente() As Task(Of String)
        Return (Await Program.Seguridad_ObtenerUsuario())
    End Function

End Class
