Imports Telerik.Windows.Controls
Imports System.Threading.Tasks

Public Class ParametrosConsola
    Public Async Function actualizarParametrosConsola() As Task(Of Boolean)
        Return (Await Program.leerParametroAppConsola(Program.Aplicacion, Program.VersionAplicacion, Program.Division))
    End Function

End Class
