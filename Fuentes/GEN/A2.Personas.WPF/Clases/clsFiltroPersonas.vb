Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.WEB
Imports System.Linq


Public Class clsFiltroPersonas
	Inherits FilteringBehavior

	Public Overrides Function FindMatchingItems(searchText As String, items As IList, escapedItems As IEnumerable(Of Object), textSearchPath As String, textSearchMode As TextSearchMode) As IEnumerable(Of Object)
		Dim results = items.OfType(Of CPX_BuscadorPersonas).Where(Function(x) x.strCadenaBusqueda.ToLower.Contains(searchText.ToLower))
		Return results.Where(Function(x) Not escapedItems.Contains(x))
	End Function

End Class
