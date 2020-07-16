
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
Public Class PersonasDomainServices
    Inherits DbDomainService(Of dbPersonasEntities)

    Public Sub New()

    End Sub

#Region "UTILIDADES"

    <Invoke(HasSideEffects:=True)>
    Public Function Entidad_ValidacionesGenerales() As List(Of CPX_tblValidacionesPersonas)
        Try
            Return New List(Of CPX_tblValidacionesPersonas)
        Catch ex As Exception
            ManejarError(ex, "UTILITARIOS", "Clientes_Filtrar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "BuscadorPersonas"
    ''' <summary>
    ''' JAPC20180926: proceso para traer nuevo complex CPX_BuscadorPersonas
    ''' </summary>
    ''' <returns></returns>
    Public Function EntidadBuscadorPersonas() As CPX_BuscadorPersonas
		Return New CPX_BuscadorPersonas
	End Function

    ''' <summary>
    ''' JAPC20180926: metodo para buscar personas por medio del rol  y un filtro 
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrRol"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Personas_Buscar(ByVal pstrFiltro As String, ByVal pstrRol As String, ByVal pstrUsuario As String) As List(Of CPX_BuscadorPersonas)
        Try
            'TODO Agregar el estado para el filtro de personas
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "BUSCADOR_PERSONAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Personas_Buscador_Personas(pstrFiltro, pstrRol, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "BUSCADOR_PERSONAS", "Personas_Filtrar")
            Return Nothing
        End Try
    End Function

#End Region

End Class

