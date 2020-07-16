
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports OpenRiaServices.DomainServices.EntityFramework


'TODO: Create methods containing your application logic.
<EnableClientAccess()>
Public Class OrdenesDomainServices
    Inherits DbDomainService(Of dbOrdenesEntities)

    Public Sub New()

    End Sub

#Region "UTILIDADES"

    <Invoke(HasSideEffects:=True)>
    Public Function Entidad_ValidacionesGenerales() As List(Of CPX_tblValidacionesGenerales)
        Try
            Return New List(Of CPX_tblValidacionesGenerales)
        Catch ex As Exception
            ManejarError(ex, "UTILITARIOS", "Clientes_Filtrar")
            Return Nothing
        End Try
    End Function

#End Region

End Class

