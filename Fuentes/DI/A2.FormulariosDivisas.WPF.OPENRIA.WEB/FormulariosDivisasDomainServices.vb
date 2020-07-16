
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
Imports A2.OyD.Infraestructura
Imports System.Data.SqlClient
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.IO



<EnableClientAccess()>
Public Class FormulariosDivisasDomainServices
	Inherits DbDomainService(Of dbFormulariosDivisasEntities)

	Public Sub New()

	End Sub

#Region "UTILIDADES"
	<Invoke(HasSideEffects:=True)>
	Public Function Filtrar_Clientes(pstrFiltro As String, pstrUsuario As String) As List(Of CPX_Clientes)
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "CLIENTES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_BuscadorClientes(pstrFiltro, strUsuario, strInfoSession).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "UTILITARIOS", "Clientes_Filtrar")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Entidad_ValidacionesGenerales() As List(Of CPX_tblValidacionesGenerales)
		Try
			Return New List(Of CPX_tblValidacionesGenerales)
		Catch ex As Exception
			ManejarError(ex, "UTILITARIOS", "Clientes_Filtrar")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Utilitarios_ConsultarCombos(ByVal pstrUsuario As String) As List(Of CPX_tblCombos)
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "UTILITARIOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Combos1("", "", "", 1, 1, True, strUsuario, strInfoSession).ToList
            Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "UTILITARIOS", "Clientes_Filtrar")
			Return Nothing
		End Try
	End Function

#End Region

#Region "Numerales Cambiarios"
	<Invoke(HasSideEffects:=True)>
	Public Function NumeralesCambiarios_Filtrar(ByVal pstrFiltro As String, ByVal pintFormulario As Integer, ByVal pstrUsuario As String) As List(Of CPX_NumeralesCambiarios)
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "NUMERALESCAMBIARIOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_NumeralesCambiarios_Filtrar(pstrFiltro, pintFormulario, strUsuario, strInfoSession).ToList
            Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "NUMERALESCAMBIARIOS", "NumeralesCambiarios_Filtrar")
			Return Nothing
		End Try
	End Function
#End Region

#Region "Formulario1"
	<Invoke(HasSideEffects:=True)>
	Public Function GetFormulario1() As CPX_Formulario1
		Try
			Return New CPX_Formulario1
		Catch ex As Exception
			ManejarError(ex, "Formularios", "GetFormulario1")
			Return Nothing
		End Try
	End Function

    Public Function EntidadGetFormulario1() As tblFormulariosBcoRepublica1
        Try
            Return New tblFormulariosBcoRepublica1
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario1")
            Return Nothing
        End Try
    End Function



    <Invoke(HasSideEffects:=True)>
    Public Function Formulario1_Actualizar(plngNumeroDecl As Nullable(Of Integer), plngFormulario As Nullable(Of Integer), pstrCondicionNegocio As String, pintTipoOperacion As Nullable(Of Short), plngCiudad As Nullable(Of Integer), plngNit As String, pdtmFecha As Nullable(Of Date), plngNitAnterior As String, pdtmFechaAnterior As Nullable(Of Date), plngNumeroDeclAnterior As Nullable(Of Integer), pstrTipoDocumento As String, plngNumeroidentificacion As String, plngDigitoVerificacion As String, pRazonSocial As String, plngCiudadDomicilio As Nullable(Of Integer), pstrCondiccionespago As String, pstrCondiccionesDespacho As String, pstrNombreDeclarante As String, plngNumeroIdentDeclarante As String, pblnEnviado As Nullable(Of Boolean), pstrObservaciones As String, pdblTotalvalorFOB As Nullable(Of Decimal), pdblTotalGastosExportacion As Nullable(Of Decimal), pdblDeducciones As Nullable(Of Decimal), pdblReintegroNeto As Nullable(Of Decimal), pstrTelefono As String, pstrDireccion As String, pstrCodigoMoneda1 As String, pdblTipoCambioMoneda1 As Nullable(Of Decimal), plngNumeralcambiario1 As Nullable(Of Integer), pdblValormoneda1 As Nullable(Of Decimal), pdblValorUSD1 As Nullable(Of Decimal), pstrCodigoMoneda2 As String, pdblTipoCambioMoneda2 As Nullable(Of Decimal), plngNumeralcambiario2 As Nullable(Of Integer), pdblValormoneda2 As Nullable(Of Decimal), pdblValorUSD2 As Nullable(Of Decimal), pstrGlobal As String, pstrCorreccion As String, pstrIngresoEgreso As String, plngIDOperacion As Nullable(Of Integer), pstrNombreDeclaranteFirma As String, plngNumeroDeclAnteriorFirma As String, pstrEstado As String, pstrTipoReferencia As String, pdtmFechaActualizacion As Nullable(Of Date), pblnEnviadoDian As Nullable(Of Boolean), pstrCorreoElectronicoDecl As String, pstrCodigoCiudadDecl As String, plogFirmaFormPend As Nullable(Of Boolean), plngDeclaracionConsolidado As Nullable(Of Integer), pintNumeroBcoRep As Nullable(Of Integer), pdtmFechaBcoRep As Nullable(Of Date), pstrDetalleFormulario1 As String, pstrDetalleFormulario1Eliminar As String, plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario1_Validar(plngNumeroDecl,
                                                                        plngFormulario,
                                                                        pstrCondicionNegocio,
                                                                        pintTipoOperacion,
                                                                        plngCiudad,
                                                                        plngNit,
                                                                        pdtmFecha,
                                                                        plngNitAnterior,
                                                                        pdtmFechaAnterior,
                                                                        plngNumeroDeclAnterior,
                                                                        pstrTipoDocumento,
                                                                        plngNumeroidentificacion,
                                                                        plngDigitoVerificacion,
                                                                        pRazonSocial,
                                                                        plngCiudadDomicilio,
                                                                        pstrCondiccionespago,
                                                                        pstrCondiccionesDespacho,
                                                                        pstrNombreDeclarante,
                                                                        plngNumeroIdentDeclarante,
                                                                        pblnEnviado,
                                                                        pstrObservaciones,
                                                                        pdblTotalvalorFOB,
                                                                        pdblTotalGastosExportacion,
                                                                        pdblDeducciones,
                                                                        pdblReintegroNeto,
                                                                        pstrTelefono,
                                                                        pstrDireccion,
                                                                        pstrCodigoMoneda1,
                                                                        pdblTipoCambioMoneda1,
                                                                        plngNumeralcambiario1,
                                                                        pdblValormoneda1,
                                                                        pdblValorUSD1,
                                                                        pstrCodigoMoneda2,
                                                                        pdblTipoCambioMoneda2,
                                                                        plngNumeralcambiario2,
                                                                        pdblValormoneda2,
                                                                        pdblValorUSD2,
                                                                        pstrGlobal,
                                                                        pstrCorreccion,
                                                                        pstrIngresoEgreso,
                                                                        plngIDOperacion,
                                                                        pstrNombreDeclaranteFirma,
                                                                        plngNumeroDeclAnteriorFirma,
                                                                        pstrEstado,
                                                                        pstrTipoReferencia,
                                                                        pdtmFechaActualizacion,
                                                                        pblnEnviadoDian,
                                                                        pstrCorreoElectronicoDecl,
                                                                        pstrCodigoCiudadDecl,
                                                                        plogFirmaFormPend,
                                                                        plngDeclaracionConsolidado,
                                                                        pintNumeroBcoRep,
                                                                        pdtmFechaBcoRep,
                                                                        pstrDetalleFormulario1,
                                                                        pstrDetalleFormulario1Eliminar,
                                                                        plogSoloValidar,
                                                                        pstrUsuario,
                                                                        strInfoSession
                                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO1", "Formulario1_Actualizar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario1_Defecto(ByVal pstrUsuario As String) As tblFormulariosBcoRepublica1
        Try
            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario1_ConsultarID(-1, Nothing, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Formulario1", "Formulario1_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
	Public Function Formulario1_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario1)
		Try
			pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario1_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO1", "Formulario1_Filtrar")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario1_Consultar(ByVal plngNumeroDecl As String, ByVal pdtmFecha As Nullable(Of Date), ByVal pstrNombre As String, ByVal pintOrden As String, ByVal pstrNumeroIdentificacion As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario1)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_Formulario1)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario1_Consultar(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO1", "Formulario1_Consultar")
			Return Nothing
		End Try
	End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario1_ID(ByVal plngNumeroDecl As Integer, ByVal pdtmFecha As Date, ByVal pstrUsuario As String) As tblFormulariosBcoRepublica1
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario1_ConsultarID(plngNumeroDecl, pdtmFecha, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO1", "Formulario1_ID")
            Return Nothing
        End Try
    End Function


    <Invoke(HasSideEffects:=True)>
	Public Function Detalle1Formulario_Consultar(ByVal pintID As Integer, ByVal plnNumeroDecl As Nullable(Of Integer), ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As List(Of CPX_tblFormulario1DetalleDian)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE1FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_tblFormulario1DetalleDian)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario1DescripcionOp_Consultar(pintID, plnNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "DETALLE1FORMULARIO", "Detalle1Formulario_Consultar")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Detalle1Formulario_ConsultarID(ByVal pintID As Integer, ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As CPX_tblFormulario1DetalleDian
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario1DescripcionOp_Consultar(pintID, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "Detalle2Formulario", "Detalle2Formulario_ConsultarID")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Detalle1Formulario_Defecto(ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As CPX_tblFormulario1DetalleDian
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario1DescripcionOp_Consultar(-1, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "Detalle1Formulario", "Detalle1Formulario_Defecto")
			Return Nothing
		End Try
	End Function


#End Region

#Region "Formulario2"



	<Invoke(HasSideEffects:=True)>
	Public Function GetFormulario2() As CPX_Formulario2
		Try
			Return New CPX_Formulario2
		Catch ex As Exception
			ManejarError(ex, "Formularios", "GetFormulario2")
			Return Nothing
		End Try
	End Function

    Public Function EntidadGetFormulario2() As tblFormulariosBcoRepublica1
        Try
            Return New tblFormulariosBcoRepublica1
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario2")
            Return Nothing
        End Try
    End Function



    <Invoke(HasSideEffects:=True)>
    Public Function Formulario2_Actualizar(plngNumeroDecl As Nullable(Of Integer), plngFormulario As Nullable(Of Integer), pstrCondicionNegocio As String, pintTipoOperacion As Nullable(Of Short), plngCiudad As Nullable(Of Integer), plngNit As String, pdtmFecha As Nullable(Of Date), plngNitAnterior As String, pdtmFechaAnterior As Nullable(Of Date), plngNumeroDeclAnterior As Nullable(Of Integer), pstrTipoDocumento As String, plngNumeroidentificacion As String, plngDigitoVerificacion As String, pRazonSocial As String, plngCiudadDomicilio As Nullable(Of Integer), pstrCondiccionespago As String, pstrCondiccionesDespacho As String, pstrNombreDeclarante As String, plngNumeroIdentDeclarante As String, pblnEnviado As Nullable(Of Boolean), pstrObservaciones As String, pdblTotalvalorFOB As Nullable(Of Decimal), pdblTotalGastosExportacion As Nullable(Of Decimal), pdblDeducciones As Nullable(Of Decimal), pdblReintegroNeto As Nullable(Of Decimal), pstrTelefono As String, pstrDireccion As String, pstrCodigoMoneda1 As String, pdblTipoCambioMoneda1 As Nullable(Of Decimal), plngNumeralcambiario1 As Nullable(Of Integer), pdblValormoneda1 As Nullable(Of Decimal), pdblValorUSD1 As Nullable(Of Decimal), pstrCodigoMoneda2 As String, pdblTipoCambioMoneda2 As Nullable(Of Decimal), plngNumeralcambiario2 As Nullable(Of Integer), pdblValormoneda2 As Nullable(Of Decimal), pdblValorUSD2 As Nullable(Of Decimal), pstrGlobal As String, pstrCorreccion As String, pstrIngresoEgreso As String, plngIDOperacion As Nullable(Of Integer), pstrNombreDeclaranteFirma As String, plngNumeroDeclAnteriorFirma As String, pstrEstado As String, pstrTipoReferencia As String, pdtmFechaActualizacion As Nullable(Of Date), pblnEnviadoDian As Nullable(Of Boolean), pstrCorreoElectronicoDecl As String, pstrCodigoCiudadDecl As String, plogFirmaFormPend As Nullable(Of Boolean), plngDeclaracionConsolidado As Nullable(Of Integer), pintNumeroBcoRep As Nullable(Of Integer), pdtmFechaBcoRep As Nullable(Of Date), pstrDetalleFormulario2 As String, pstrDetalleFormulario2Eliminar As String, pstrDetalleFormulario2DIAN As String, pstrDetalleFormulario2DIANEliminar As String, plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario2_Validar(plngNumeroDecl,
                                                                        plngFormulario,
                                                                        pstrCondicionNegocio,
                                                                        pintTipoOperacion,
                                                                        plngCiudad,
                                                                        plngNit,
                                                                        pdtmFecha,
                                                                        plngNitAnterior,
                                                                        pdtmFechaAnterior,
                                                                        plngNumeroDeclAnterior,
                                                                        pstrTipoDocumento,
                                                                        plngNumeroidentificacion,
                                                                        plngDigitoVerificacion,
                                                                        pRazonSocial,
                                                                        plngCiudadDomicilio,
                                                                        pstrCondiccionespago,
                                                                        pstrCondiccionesDespacho,
                                                                        pstrNombreDeclarante,
                                                                        plngNumeroIdentDeclarante,
                                                                        pblnEnviado,
                                                                        pstrObservaciones,
                                                                        pdblTotalvalorFOB,
                                                                        pdblTotalGastosExportacion,
                                                                        pdblDeducciones,
                                                                        pdblReintegroNeto,
                                                                        pstrTelefono,
                                                                        pstrDireccion,
                                                                        pstrCodigoMoneda1,
                                                                        pdblTipoCambioMoneda1,
                                                                        plngNumeralcambiario1,
                                                                        pdblValormoneda1,
                                                                        pdblValorUSD1,
                                                                        pstrCodigoMoneda2,
                                                                        pdblTipoCambioMoneda2,
                                                                        plngNumeralcambiario2,
                                                                        pdblValormoneda2,
                                                                        pdblValorUSD2,
                                                                        pstrGlobal,
                                                                        pstrCorreccion,
                                                                        pstrIngresoEgreso,
                                                                        plngIDOperacion,
                                                                        pstrNombreDeclaranteFirma,
                                                                        plngNumeroDeclAnteriorFirma,
                                                                        pstrEstado,
                                                                        pstrTipoReferencia,
                                                                        pdtmFechaActualizacion,
                                                                        pblnEnviadoDian,
                                                                        pstrCorreoElectronicoDecl,
                                                                        pstrCodigoCiudadDecl,
                                                                        plogFirmaFormPend,
                                                                        plngDeclaracionConsolidado,
                                                                        pintNumeroBcoRep,
                                                                        pdtmFechaBcoRep,
                                                                        pstrDetalleFormulario2,
                                                                        pstrDetalleFormulario2Eliminar,
                                                                        pstrDetalleFormulario2DIAN,
                                                                        pstrDetalleFormulario2DIANEliminar,
                                                                        plogSoloValidar,
                                                                        pstrUsuario,
                                                                        strInfoSession
                                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO2", "Formulario2_Actualizar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario2_Defecto(ByVal pstrUsuario As String) As tblFormulariosBcoRepublica1
        Try
            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2_ConsultarID(-1, Nothing, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Formulario2", "Formulario2_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
	Public Function Formulario2_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario2)
		Try
			pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO2", "Formulario2_Filtrar")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario2_Consultar(ByVal plngNumeroDecl As String, ByVal pdtmFecha As Nullable(Of Date), ByVal pstrNombre As String, ByVal pintOrden As String, ByVal pstrNumeroIdentificacion As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario2)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_Formulario2)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario2_Consultar(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO2", "Formulario2_Consultar")
			Return Nothing
		End Try
	End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario2_ID(ByVal plngNumeroDecl As Integer, ByVal pdtmFecha As Date, ByVal pstrUsuario As String) As tblFormulariosBcoRepublica1
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2_ConsultarID(plngNumeroDecl, pdtmFecha, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO2", "Formulario2_ID")
            Return Nothing
        End Try
    End Function


    Public Function EntidadGetDetalle2Formulario() As tblDetalle2FormulariosBcoRepublica
        Try
            Return New tblDetalle2FormulariosBcoRepublica
        Catch ex As Exception
            ManejarError(ex, "Formularios", "EntidadGetDetalle2Formulario")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle2Formulario_Consultar(ByVal pintID As Integer, ByVal plnNumeroDecl As Nullable(Of Integer), ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As List(Of CPX_DIVISAS_tblDetalle2FormularioBcoRepublica)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_DIVISAS_tblDetalle2FormularioBcoRepublica)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOp_Consultar(pintID, plnNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLE2FORMULARIO", "Detalle2Formulario_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle2Formulario_ConsultarID(ByVal pintID As Integer, ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle2FormulariosBcoRepublica
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOp_ConsultarID(pintID, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Detalle2Formulario", "Detalle2Formulario_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle2Formulario_Defecto(ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle2FormulariosBcoRepublica
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOp_ConsultarID(-1, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Detalle2Formulario", "Detalle2Formulario_Defecto")
            Return Nothing
        End Try
    End Function

#Region "Detalle Formulario 2 DIAN"
    Public Function EntidadGetDetalle2FormularioDIAN() As tblDetalle2FormulariosBcoRepublica_DIAN
        Try
            Return New tblDetalle2FormulariosBcoRepublica_DIAN
        Catch ex As Exception
            ManejarError(ex, "Formularios", "EntidadGetDetalle2FormularioDIAN")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle2FormularioDIAN_Consultar(ByVal pintID As Integer, ByVal plnNumeroDecl As Nullable(Of Integer), ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As List(Of CPX_tblDetalleFormulario2DIAN)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblDetalleFormulario2DIAN)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOpDIAN_Consultar(pintID, plnNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLE2FORMULARIODIAN", "Detalle2FormularioDIAN_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle2FormularioDIAN_ConsultarID(ByVal pintID As Integer, ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle2FormulariosBcoRepublica_DIAN
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIODIAN")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOpDIAN_ConsultarID(pintID, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Detalle2Formulario", "Detalle2Formulario_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle2FormularioDIAN_Defecto(ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle2FormulariosBcoRepublica_DIAN
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIODIAN")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOpDIAN_ConsultarID(-1, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Detalle2FormularioDIAN", "Detalle2FormularioDIAN_Defecto")
            Return Nothing
        End Try
    End Function
#End Region

#End Region

#Region "Fomulario3"
    <Invoke(HasSideEffects:=True)>
    Public Function GetFormulario3() As CPX_Formulario3
        Try
            Return New CPX_Formulario3
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario3")
            Return Nothing
        End Try
    End Function

    Public Function EntidadGetFormulario3() As tblFormulariosBcoRepublica1
        Try
            Return New tblFormulariosBcoRepublica1
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario3")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario3_Actualizar(plngNumeroDecl As Nullable(Of Integer), plngFormulario As Nullable(Of Integer), pstrCondicionNegocio As String, pintTipoOperacion As Nullable(Of Short), plngCiudad As Nullable(Of Integer), plngNit As String, pdtmFecha As Nullable(Of Date), plngNitAnterior As String, pdtmFechaAnterior As Nullable(Of Date), plngNumeroDeclAnterior As Nullable(Of Integer), pstrTipoDocumento As String, plngNumeroidentificacion As String, plngDigitoVerificacion As String, pRazonSocial As String, plngCiudadDomicilio As Nullable(Of Integer), pstrCondiccionespago As String, pstrNombreDeclarante As String, plngNumeroIdentDeclarante As String, pblnEnviado As Nullable(Of Boolean), pstrTelefono As String, pstrDireccion As String, pstrCodigoMoneda1 As String, pdblTipoCambioMoneda1 As Nullable(Of Decimal), plngNumeralcambiario1 As Nullable(Of Integer), pdblValormoneda1 As Nullable(Of Decimal), pdblValorUSD1 As Nullable(Of Decimal), pstrCodigoMoneda2 As String, pdblTipoCambioMoneda2 As Nullable(Of Decimal), plngNumeralcambiario2 As Nullable(Of Integer), pdblValormoneda2 As Nullable(Of Decimal), pdblValorUSD2 As Nullable(Of Decimal), pstrIngresoEgreso As String, plngIDOperacion As Nullable(Of Integer), pstrNombreDeclaranteFirma As String, plngNumeroDeclAnteriorFirma As String, pstrEstado As String, pblnEnviadoDian As Nullable(Of Boolean), plogFirmaFormPend As Nullable(Of Boolean), pstrNumeroPrestamoOp As String, pstrTipoDocumentoOp As String, pstrNumeroidentificacionOp As String, pstrDigitoVerificacionEmpOp As String, pstrNombreAcreedor As String, pintNumeroBcoRep As Nullable(Of Integer), pdtmFechaBcoRep As Nullable(Of Date), pstrDetalleFormulario3 As String, pstrDetalleFormulario3Eliminar As String, plogSoloValidar As Nullable(Of Boolean), pdtmFechaActualizacion As Nullable(Of Date), pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario3_Validar(plngNumeroDecl,
                                                                        plngFormulario,
                                                                        pstrCondicionNegocio,
                                                                        pintTipoOperacion,
                                                                        plngCiudad,
                                                                        plngNit,
                                                                        pdtmFecha,
                                                                        plngNitAnterior,
                                                                        pdtmFechaAnterior,
                                                                        plngNumeroDeclAnterior,
                                                                        pstrTipoDocumento,
                                                                        plngNumeroidentificacion,
                                                                        plngDigitoVerificacion,
                                                                        pRazonSocial,
                                                                        plngCiudadDomicilio,
                                                                        pstrCondiccionespago,
                                                                        pstrNombreDeclarante,
                                                                        plngNumeroIdentDeclarante,
                                                                        pblnEnviado,
                                                                        pstrTelefono,
                                                                        pstrDireccion,
                                                                        pstrCodigoMoneda1,
                                                                        pdblTipoCambioMoneda1,
                                                                        plngNumeralcambiario1,
                                                                        pdblValormoneda1,
                                                                        pdblValorUSD1,
                                                                        pstrCodigoMoneda2,
                                                                        pdblTipoCambioMoneda2,
                                                                        plngNumeralcambiario2,
                                                                        pdblValormoneda2,
                                                                        pdblValorUSD2,
                                                                        pstrIngresoEgreso,
                                                                        plngIDOperacion,
                                                                        pstrNombreDeclaranteFirma,
                                                                        plngNumeroDeclAnteriorFirma,
                                                                        pstrEstado,
                                                                        pblnEnviadoDian,
                                                                        plogFirmaFormPend,
                                                                        pstrNumeroPrestamoOp,
                                                                        pstrTipoDocumentoOp,
                                                                        pstrNumeroidentificacionOp,
                                                                        pstrDigitoVerificacionEmpOp,
                                                                        pstrNombreAcreedor,
                                                                        pintNumeroBcoRep,
                                                                        pdtmFechaBcoRep,
                                                                        pstrDetalleFormulario3,
                                                                        pstrDetalleFormulario3Eliminar,
                                                                        plogSoloValidar,
                                                                        pdtmFechaActualizacion,
                                                                        pstrUsuario,
                                                                        strInfoSession
                                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO3", "Formulario3_Actualizar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario3_Defecto(ByVal pstrUsuario As String) As CPX_Formulario3
        Try
            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario3_ConsultarID(-1, Now, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Formulario3", "Formulario3_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario3_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario3)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario3_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO2", "Formulario3_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario3_Consultar(ByVal plngNumeroDecl As String, ByVal pdtmFecha As Nullable(Of Date), ByVal pstrNombre As String, ByVal pintOrden As String, ByVal pstrNumeroIdentificacion As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario3)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formulario3)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario3_Consultar(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO3", "Formulario3_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario3_ID(ByVal plngNumeroDecl As Integer, ByVal pdtmFecha As Date, ByVal pstrUsuario As String) As CPX_Formulario3
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario3_ConsultarID(plngNumeroDecl, pdtmFecha, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO3", "Formulario3_ID")
            Return Nothing
        End Try
    End Function


    Public Function EntidadGetDetalle1Formulario() As tblDetalle1FormulariosBcoRepublica
        Try
            Return New tblDetalle1FormulariosBcoRepublica
        Catch ex As Exception
            ManejarError(ex, "Formularios", "EntidadGetDetalle1Formulario")
            Return Nothing
        End Try
    End Function

    Public Function EntidadGetDetalle5Formulario() As tblDetalle5FormulariosBcoRepublica
        Try
            Return New tblDetalle5FormulariosBcoRepublica
        Catch ex As Exception
            ManejarError(ex, "Formularios", "EntidadGetDetalle5Formulario")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
	Public Function Detalle3Formulario_Consultar(ByVal pintID As Integer, ByVal plnNumeroDecl As Nullable(Of Integer), ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As List(Of CPX_tblDetalle1FormulariosBcoRepublica)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE3FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_tblDetalle1FormulariosBcoRepublica)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario3DescripcionOp_Consultar(pintID, plnNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "DETALLE1FORMULARIO", "Detalle1Formulario_Consultar")
			Return Nothing
		End Try
	End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle3Formulario_ConsultarID(ByVal pintID As Integer, ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle1FormulariosBcoRepublica
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLEFORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario3DescripcionOp_ConsultarID(pintID, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DetalleFormulario3", "DetalleFormulario3_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle3Formulario_Defecto(ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle1FormulariosBcoRepublica
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLEFORMULARIO3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario3DescripcionOp_ConsultarID(-1, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DetalleFormulario3", "DetalleFormulario3_Defecto")
            Return Nothing
        End Try
    End Function

#End Region

    <Invoke(HasSideEffects:=True)>
	Public Function ConsecutivoFormulario() As Nullable(Of Integer)
		Dim objRetorno = DbContext.usp_Formularios_Consecutivo()
		Return objRetorno.FirstOrDefault
	End Function
#Region "Formulario 4"
    <Invoke(HasSideEffects:=True)>
    Public Function GetFormulario4() As CPX_Formulario4
        Try
            Return New CPX_Formulario4
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario4")
            Return Nothing
        End Try
    End Function

    Public Function EntidadGetFormulario4() As tblFormulariosBcoRepublica1
        Try
            Return New tblFormulariosBcoRepublica1
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario4")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario4_Actualizar(plngNumeroDecl As Nullable(Of Integer),
                                           plngFormulario As Nullable(Of Integer),
                                           pstrCondicionNegocio As String,
                                           pintTipoOperacion As Nullable(Of Short),
                                           plngCiudad As Nullable(Of Integer),
                                           plngNit As String,
                                           pdtmFecha As Nullable(Of Date),
                                           plngNitAnterior As String,
                                           pdtmFechaAnterior As Nullable(Of Date),
                                           plngNumeroDeclAnterior As Nullable(Of Integer),
                                           pstrTipoDocumento As String,
                                           plngNumeroidentificacion As String,
                                           plngDigitoVerificacion As String,
                                           pRazonSocial As String,
                                           plngCiudadDomicilio As Nullable(Of Integer),
                                           pstrCondiccionespago As String,
                                           pstrCondiccionesDespacho As String,
                                           pstrNombreDeclarante As String,
                                           plngNumeroIdentDeclarante As String,
                                           pblnEnviado As Nullable(Of Boolean),
                                           pstrObservaciones As String,
                                           pdblTotalvalorFOB As Nullable(Of Decimal),
                                           pdblTotalGastosExportacion As Nullable(Of Decimal),
                                           pdblDeducciones As Nullable(Of Decimal),
                                           pdblReintegroNeto As Nullable(Of Decimal),
                                           pstrTelefono As String,
                                           pstrDireccion As String,
                                          pstrCodigoMoneda1 As String,
                                          pdblTipoCambioMoneda1 As Nullable(Of Decimal),
                                          plngNumeralcambiario1 As Nullable(Of Integer),
                                          pdblValormoneda1 As Nullable(Of Decimal),
                                          pdblValorUSD1 As Nullable(Of Decimal),
                                          pstrCodigoMoneda2 As String,
                                          pdblTipoCambioMoneda2 As Nullable(Of Decimal),
                                          plngNumeralcambiario2 As Nullable(Of Integer),
                                          pdblValormoneda2 As Nullable(Of Decimal),
                                          pdblValorUSD2 As Nullable(Of Decimal),
                                          pstrGlobal As String,
                                          pstrCorreccion As String,
                                          pstrIngresoEgreso As String,
                                          plngIDOperacion As Nullable(Of Integer),
                                          pstrNombreDeclaranteFirma As String,
                                          plngNumeroDeclAnteriorFirma As String,
                                          pstrEstado As String,
                                          pstrTipoReferencia As String,
                                          pblnEnviadoDian As Nullable(Of Boolean),
                                          pstrCorreoElectronicoDecl As String,
                                          pstrCodigoCiudadDecl As String,
                                          plogFirmaFormPend As Nullable(Of Boolean),
                                          plngDeclaracionConsolidado As Nullable(Of Integer),
                                          PstrTipoDocumentoEmpRec As String,
                                          plngNumeroidentificacionEmpRec As String,
                                          plngDigitoVerificacionEmpRec As Nullable(Of Integer),
                                          pstrNombreEmpReceptora As String,
                                          pintPaisEmpReceptora As Nullable(Of Integer),
                                          pstrTipoDocumentoInv As String,
                                          plngNumeroidentificacionInv As String,
                                          plngDigitoVerificacionInv As Nullable(Of Integer),
                                          pstrNombreInversionista As String,
                                          pstrEntidadInversionista As String,
                                          pintPaisInversionista As Nullable(Of Integer),
                                          pdtmFechaInversionista As Nullable(Of Date),
                                          plngNumeroInversionista As Nullable(Of Integer),
                                          plngNumeralcambiarioOperacion As Nullable(Of Integer),
                                          pstrCodigoMonedaOperacion As String,
                                          pdblValorMonedaGiroOperacion As Nullable(Of Decimal),
                                          pdblTipoCambioUSDOperacion As Nullable(Of Decimal),
                                          pdblValorUSDOperacion As Nullable(Of Decimal),
                                          pdblTipoCambioPesosOperacion As Nullable(Of Decimal),
                                          pdblValorPesosOperacion As Nullable(Of Decimal),
                                          pstrDestinoOperacion As Nullable(Of Integer),
                                          pstrPaisInv As String,
                                          pstrCiiuInv As String,
                                          pstrPaisRec As String,
                                          pstrCiiuRec As String,
                                          pstrInversionPlazo As String,
                                          pstrCiudadEmpRec As String,
                                          pstrTelefonoEmpRec As String,
                                          pstrAccionesOCuotas As String,
                                          pintNumeroBcoRep As Nullable(Of Integer),
                                          pintNumeroBcoRepAnterior As Nullable(Of Integer),
                                          pdtmFechaBcoRep As Nullable(Of Date),
                                          pdtmFechaBcoRepAnterior As Nullable(Of Date),
                                          pstrDetalleFormulario4 As String,
                                          pstrDetalleFormulario4Eliminar As String,
                                          plogSoloValidar As Nullable(Of Boolean),
                                          pdtmFechaActualizacion As Nullable(Of Date),
                                          pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO4")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario4_Validar(plngNumeroDecl,
                                                                        plngFormulario,
                                                                        pstrCondicionNegocio,
                                                                        pintTipoOperacion,
                                                                        plngCiudad,
                                                                        plngNit,
                                                                        pdtmFecha,
                                                                        plngNitAnterior,
                                                                        pdtmFechaAnterior,
                                                                        plngNumeroDeclAnterior,
                                                                        pstrTipoDocumento,
                                                                        plngNumeroidentificacion,
                                                                        plngDigitoVerificacion,
                                                                        pRazonSocial,
                                                                        plngCiudadDomicilio,
                                                                        pstrCondiccionespago,
                                                                        pstrCondiccionesDespacho,
                                                                        pstrNombreDeclarante,
                                                                        plngNumeroIdentDeclarante,
                                                                        pblnEnviado,
                                                                        pstrObservaciones,
                                                                        pdblTotalvalorFOB,
                                                                        pdblTotalGastosExportacion,
                                                                        pdblDeducciones,
                                                                        pdblReintegroNeto,
                                                                        pstrTelefono,
                                                                        pstrDireccion,
                                                                        pstrCodigoMoneda1,
                                                                        pdblTipoCambioMoneda1,
                                                                        plngNumeralcambiario1,
                                                                        pdblValormoneda1,
                                                                        pdblValorUSD1,
                                                                        pstrCodigoMoneda2,
                                                                        pdblTipoCambioMoneda2,
                                                                        plngNumeralcambiario2,
                                                                        pdblValormoneda2,
                                                                        pdblValorUSD2,
                                                                        pstrGlobal,
                                                                        pstrCorreccion,
                                                                        pstrIngresoEgreso,
                                                                        plngIDOperacion,
                                                                        pstrNombreDeclaranteFirma,
                                                                        plngNumeroDeclAnteriorFirma,
                                                                        pstrEstado,
                                                                        pstrTipoReferencia,
                                                                        pblnEnviadoDian,
                                                                        pstrCorreoElectronicoDecl,
                                                                        pstrCodigoCiudadDecl,
                                                                        plogFirmaFormPend,
                                                                        plngDeclaracionConsolidado,
                                                                        PstrTipoDocumentoEmpRec,
                                                                        plngNumeroidentificacionEmpRec,
                                                                        plngDigitoVerificacionEmpRec,
                                                                        pstrNombreEmpReceptora,
                                                                        pintPaisEmpReceptora,
                                                                        pstrTipoDocumentoInv,
                                                                        plngNumeroidentificacionInv,
                                                                        plngDigitoVerificacionInv,
                                                                        pstrNombreInversionista,
                                                                        pstrEntidadInversionista,
                                                                        pintPaisInversionista,
                                                                        pdtmFechaInversionista,
                                                                        plngNumeroInversionista,
                                                                        plngNumeralcambiarioOperacion,
                                                                        pstrCodigoMonedaOperacion,
                                                                        pdblValorMonedaGiroOperacion,
                                                                        pdblTipoCambioUSDOperacion,
                                                                        pdblValorUSDOperacion,
                                                                        pdblTipoCambioPesosOperacion,
                                                                        pdblValorPesosOperacion,
                                                                        pstrDestinoOperacion,
                                                                        pstrPaisInv,
                                                                        pstrCiiuInv,
                                                                        pstrPaisRec,
                                                                        pstrCiiuRec,
                                                                        pstrInversionPlazo,
                                                                        pstrCiudadEmpRec,
                                                                        pstrTelefonoEmpRec,
                                                                        pstrAccionesOCuotas,
                                                                        pintNumeroBcoRep,
                                                                        pintNumeroBcoRepAnterior,
                                                                        pdtmFechaBcoRep,
                                                                        pdtmFechaBcoRepAnterior,
                                                                        pstrDetalleFormulario4,
                                                                        pstrDetalleFormulario4Eliminar,
                                                                        plogSoloValidar,
                                                                        pdtmFechaActualizacion,
                                                                        pstrUsuario,
                                                                        strInfoSession
                                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO5", "Formulario4_Actualizar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario4_Defecto(ByVal pstrUsuario As String) As CPX_Formulario4
        Try
            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario4_ConsultarID(-1, Nothing, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Formulario4", "Formulario4_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario4_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario4)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO4")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario4_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO4", "Formulario4_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario4_Consultar(ByVal plngNumeroDecl As String, ByVal pdtmFecha As Nullable(Of Date), ByVal pstrNombre As String, ByVal pintOrden As String, ByVal pstrNumeroIdentificacion As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario4)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO4")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formulario4)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario4_Consultar(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO4", "Formulario4_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario4_ID(ByVal plngNumeroDecl As Integer, ByVal pdtmFecha As Date, ByVal pstrUsuario As String) As CPX_Formulario4
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO4")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario4_ConsultarID(plngNumeroDecl, pdtmFecha, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO4", "Formulario4_ID")
            Return Nothing
        End Try
    End Function


    Public Function EntidadGetDetalle4Formulario4() As tblDetalle4FormulariosBcoRepublica
        Try
            Return New tblDetalle4FormulariosBcoRepublica
        Catch ex As Exception
            ManejarError(ex, "Formularios", "EntidadGetDetalle4Formulario")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Formulario 5"
    <Invoke(HasSideEffects:=True)>
    Public Function GetFormulario5() As CPX_Formulario5
        Try
            Return New CPX_Formulario5
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario5")
            Return Nothing
        End Try
    End Function

    Public Function EntidadGetFormulario5() As tblFormulariosBcoRepublica1
        Try
            Return New tblFormulariosBcoRepublica1
        Catch ex As Exception
            ManejarError(ex, "Formularios", "GetFormulario5")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario5_Actualizar(plngNumeroDecl As Nullable(Of Integer), plngFormulario As Nullable(Of Integer), pstrCondicionNegocio As String, pintTipoOperacion As Nullable(Of Short), plngCiudad As Nullable(Of Integer), plngNit As String, pdtmFecha As Nullable(Of Date), plngNitAnterior As String, pdtmFechaAnterior As Nullable(Of Date), plngNumeroDeclAnterior As Nullable(Of Integer), pstrTipoDocumento As String, plngNumeroidentificacion As String, plngDigitoVerificacion As String, pRazonSocial As String, plngCiudadDomicilio As Nullable(Of Integer), pstrCondiccionespago As String, pstrCondiccionesDespacho As String, pstrNombreDeclarante As String, plngNumeroIdentDeclarante As String, pblnEnviado As Nullable(Of Boolean), pstrObservaciones As String, pdblTotalvalorFOB As Nullable(Of Decimal), pdblTotalGastosExportacion As Nullable(Of Decimal), pdblDeducciones As Nullable(Of Decimal), pdblReintegroNeto As Nullable(Of Decimal), pstrTelefono As String, pstrDireccion As String, pstrCodigoMoneda1 As String, pdblTipoCambioMoneda1 As Nullable(Of Decimal), plngNumeralcambiario1 As Nullable(Of Integer), pdblValormoneda1 As Nullable(Of Decimal), pdblValorUSD1 As Nullable(Of Decimal), pstrCodigoMoneda2 As String, pdblTipoCambioMoneda2 As Nullable(Of Decimal), plngNumeralcambiario2 As Nullable(Of Integer), pdblValormoneda2 As Nullable(Of Decimal), pdblValorUSD2 As Nullable(Of Decimal), pstrGlobal As String, pstrCorreccion As String, pstrIngresoEgreso As String, plngIDOperacion As Nullable(Of Integer), pstrNombreDeclaranteFirma As String, plngNumeroDeclAnteriorFirma As String, pstrEstado As String, pstrTipoReferencia As String, pdtmFechaActualizacion As Nullable(Of Date), pblnEnviadoDian As Nullable(Of Boolean), pstrCorreoElectronicoDecl As String, pstrCodigoCiudadDecl As String, plogFirmaFormPend As Nullable(Of Boolean), plngDeclaracionConsolidado As Nullable(Of Integer), pintNumeroBcoRep As Nullable(Of Integer), pintNumeroBcoRepAnterior As Nullable(Of Integer), pdtmFechaBcoRep As Nullable(Of Date), pdtmFechaBcoRepAnterior As Nullable(Of Date), pstrDetalleFormulario5 As String, pstrDetalleFormulario5Eliminar As String, plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario5_Validar(plngNumeroDecl,
                                                                        plngFormulario,
                                                                        pstrCondicionNegocio,
                                                                        pintTipoOperacion,
                                                                        plngCiudad,
                                                                        plngNit,
                                                                        pdtmFecha,
                                                                        plngNitAnterior,
                                                                        pdtmFechaAnterior,
                                                                        plngNumeroDeclAnterior,
                                                                        pstrTipoDocumento,
                                                                        plngNumeroidentificacion,
                                                                        plngDigitoVerificacion,
                                                                        pRazonSocial,
                                                                        plngCiudadDomicilio,
                                                                        pstrCondiccionespago,
                                                                        pstrCondiccionesDespacho,
                                                                        pstrNombreDeclarante,
                                                                        plngNumeroIdentDeclarante,
                                                                        pblnEnviado,
                                                                        pstrObservaciones,
                                                                        pdblTotalvalorFOB,
                                                                        pdblTotalGastosExportacion,
                                                                        pdblDeducciones,
                                                                        pdblReintegroNeto,
                                                                        pstrTelefono,
                                                                        pstrDireccion,
                                                                        pstrCodigoMoneda1,
                                                                        pdblTipoCambioMoneda1,
                                                                        plngNumeralcambiario1,
                                                                        pdblValormoneda1,
                                                                        pdblValorUSD1,
                                                                        pstrCodigoMoneda2,
                                                                        pdblTipoCambioMoneda2,
                                                                        plngNumeralcambiario2,
                                                                        pdblValormoneda2,
                                                                        pdblValorUSD2,
                                                                        pstrGlobal,
                                                                        pstrCorreccion,
                                                                        pstrIngresoEgreso,
                                                                        plngIDOperacion,
                                                                        pstrNombreDeclaranteFirma,
                                                                        plngNumeroDeclAnteriorFirma,
                                                                        pstrEstado,
                                                                        pstrTipoReferencia,
                                                                        pdtmFechaActualizacion,
                                                                        pblnEnviadoDian,
                                                                        pstrCorreoElectronicoDecl,
                                                                        pstrCodigoCiudadDecl,
                                                                        plogFirmaFormPend,
                                                                        plngDeclaracionConsolidado,
                                                                        pintNumeroBcoRep,
                                                                        pintNumeroBcoRepAnterior,
                                                                        pdtmFechaBcoRep,
                                                                        pdtmFechaBcoRepAnterior,
                                                                        pstrDetalleFormulario5,
                                                                        pstrDetalleFormulario5Eliminar,
                                                                        plogSoloValidar,
                                                                        pstrUsuario,
                                                                        strInfoSession
                                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO5", "Formulario5_Actualizar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario5_Defecto(ByVal pstrUsuario As String) As CPX_Formulario5
        Try
            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario5_ConsultarID(-1, Nothing, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Formulario5", "Formulario5_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario5_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario5)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario5_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO5", "Formulario5_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario5_Consultar(ByVal plngNumeroDecl As String, ByVal pdtmFecha As Nullable(Of Date), ByVal pstrNombre As String, ByVal pintOrden As String, ByVal pstrNumeroIdentificacion As String, ByVal pstrUsuario As String) As List(Of CPX_Formulario5)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formulario5)

            objRetorno = DbContext.usp_FORMULARIOS_Formulario5_Consultar(plngNumeroDecl, pdtmFecha, pstrNombre, pintOrden, pstrNumeroIdentificacion, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO5", "Formulario5_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Formulario5_ID(ByVal plngNumeroDecl As Integer, ByVal pdtmFecha As Date, ByVal pstrUsuario As String) As CPX_Formulario5
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario5_ConsultarID(plngNumeroDecl, pdtmFecha, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMULARIO5", "Formulario5_ID")
            Return Nothing
        End Try
    End Function


    Public Function EntidadGetDetalle2Formulario5() As tblDetalle2FormulariosBcoRepublica
        Try
            Return New tblDetalle2FormulariosBcoRepublica
        Catch ex As Exception
            ManejarError(ex, "Formularios", "EntidadGetDetalle5Formulario")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle5Formulario_Consultar(ByVal pintID As Integer, ByVal plnNumeroDecl As Nullable(Of Integer), ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As List(Of CPX_DIVISAS_tblDetalle2FormularioBcoRepublica)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE5FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_DIVISAS_tblDetalle2FormularioBcoRepublica)

            'RABP20180810_El 1 que tiene al final se debe a que es con schema Divisas
            objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOp_Consultar(pintID, plnNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DETALLE5FORMULARIO", "Detalle5Formulario_Consultar")
            Return Nothing
        End Try
    End Function


    <Invoke(HasSideEffects:=True)>
    Public Function Detalle5Formulario_ConsultarID(ByVal pintID As Integer, ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle2FormulariosBcoRepublica
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            'RABP20180810_El 1 que tiene al final se debe a que es con schema Divisas
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOp_ConsultarID(pintID, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Detalle2Formulario", "Detalle2Formulario_ConsultarID")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function Detalle5Formulario_Defecto(ByVal plngNumeroDecl As Integer, ByVal pdtmFechaDecl As Date, ByVal pstrUsuario As String) As tblDetalle2FormulariosBcoRepublica
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario2DescripcionOp_ConsultarID(-1, plngNumeroDecl, pdtmFechaDecl, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "Detalle2Formulario", "Detalle2Formulario_Defecto")
            Return Nothing
        End Try
    End Function
#End Region


#Region "Formulario6"

    Public Function EntidadGetFormulario6() As tblFormularioEndeudamientoExterno
		Try
			Return New tblFormularioEndeudamientoExterno
		Catch ex As Exception
			ManejarError(ex, "Formularios", "GetFormulario6")
			Return Nothing
		End Try
	End Function



	<Invoke(HasSideEffects:=True)>
	Public Function Formulario6_Actualizar(pintID As Nullable(Of Integer), pintFormulario As Nullable(Of Short), plogEnviado As Nullable(Of Boolean), pintTipoOperacion As Nullable(Of Short), pdtmFecha As Nullable(Of Date), pintNumeroPrestamo As Nullable(Of Integer), pstrNumeroIdentifiacion As String, plogDesembolso As Nullable(Of Boolean), pintNumeroDeclaracion As Nullable(Of Integer), pintNumeral As Nullable(Of Integer), pintIDMoneda As Nullable(Of Integer), pdblValorMonedaNegociada As Nullable(Of Decimal), pdblValorUSD As Nullable(Of Decimal), pstrTipoIdentificacionDeudor As String, pstrNumeroIdentificacionDeudor As String, pintDigitoVerificacionDeudor As Nullable(Of Short), pstrNombreDeudor As String, pintIDCiudadDeudor As Nullable(Of Integer), pstrDireccionDeudor As String, pstrTelefonoDeudor As String, pstrEmailDeudor As String, pstrIDCodigoCIIUDeudor As String, pstrCodigoBanRepAcreedor As String, pstrNombreAcreedor As String, pstrPais As String, pstrTipoAcreedor As String, pstrCodigoPropositoPrestamo As String, pintIDMonedaPrestamo As Nullable(Of Integer), pdblMontoContratado As Nullable(Of Decimal), pstrTasaInteres As String, pdblSpreadValorInteres As Nullable(Of Double), pstrNumeroDepositoFinan As String, plogIndexacion As Nullable(Of Boolean), pintIDMonedaIndexacion As Nullable(Of Integer), plogSustitucion As Nullable(Of Boolean), plogFraccionamiento As Nullable(Of Boolean), plogConsolidacion As Nullable(Of Boolean), pintNUmeroIDCreditoAnterior1 As Nullable(Of Integer), pintIDMonedaAnterior1 As Nullable(Of Integer), pdblValorAnterior1 As Nullable(Of Double), pintNUmeroIDCreditoAnterior2 As Nullable(Of Integer), pintIDMonedaAnterior2 As Nullable(Of Integer), pdblValorAnterior2 As Nullable(Of Double), pstrNombre As String, pstrNumeroIDDeclarante As String, pstrFirma As String, pdtmActualizacion As Nullable(Of Date), pstrDetalleFormulario6 As String, pstrDetalleFormulario6Eliminar As String, plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO6")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario6_Validar(pintID,
																	   pintFormulario,
																	   plogEnviado,
																	   pintTipoOperacion,
																	   pdtmFecha,
																	   pintNumeroPrestamo,
																	   pstrNumeroIdentifiacion,
																	   plogDesembolso,
																	   pintNumeroDeclaracion,
																	   pintNumeral,
																	   pintIDMoneda,
																	   pdblValorMonedaNegociada,
																	   pdblValorUSD,
																	   pstrTipoIdentificacionDeudor,
																	   pstrNumeroIdentificacionDeudor,
																	   pintDigitoVerificacionDeudor,
																	   pstrNombreDeudor,
																	   pintIDCiudadDeudor,
																	   pstrDireccionDeudor,
																	   pstrTelefonoDeudor,
																	   pstrEmailDeudor,
																	   pstrIDCodigoCIIUDeudor,
																	   pstrCodigoBanRepAcreedor,
																	   pstrNombreAcreedor,
																	   pstrPais,
																	   pstrTipoAcreedor,
																	   pstrCodigoPropositoPrestamo,
																	   pintIDMonedaPrestamo,
																	   pdblMontoContratado,
																	   pstrTasaInteres,
																	   pdblSpreadValorInteres,
																	   pstrNumeroDepositoFinan,
																	   plogIndexacion,
																	   pintIDMonedaIndexacion,
																	   plogSustitucion,
																	   plogFraccionamiento,
																	   plogConsolidacion,
																	   pintNUmeroIDCreditoAnterior1,
																	   pintIDMonedaAnterior1,
																	   pdblValorAnterior1,
																	   pintNUmeroIDCreditoAnterior2,
																	   pintIDMonedaAnterior2,
																	   pdblValorAnterior2,
																	   pstrNombre,
																	   pstrNumeroIDDeclarante,
																	   pstrFirma,
																	   pdtmActualizacion,
																	   pstrDetalleFormulario6,
																	   pstrDetalleFormulario6Eliminar,
																		plogSoloValidar,
																		pstrUsuario,
																		strInfoSession
																		).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO6", "Formulario6_Actualizar")
			Return Nothing
		End Try
	End Function



	<Invoke(HasSideEffects:=True)>
	Public Function Formulario6_ID(ByVal pintID As Integer, ByVal pstrUsuario As String) As tblFormularioEndeudamientoExterno
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO6")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario6_ConsultarID(pintID, strUsuario, strInfoSession).First
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO6", "Formulario6_ID")
			Return Nothing
		End Try
	End Function


	<Invoke(HasSideEffects:=True)>
	Public Function Formulario6_Defecto(ByVal pstrUsuario As String) As tblFormularioEndeudamientoExterno
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO6")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario6_ConsultarID(-1, strUsuario, strInfoSession).First
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "Formulario6", "Formulario6_Defecto")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario6_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblFormularioEndeudamientoExterno)
		Try
			pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO6")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario6_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "Formulario6", "Formulario6_Defecto")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario6_Consultar(pintID As String, pdtmFecha As Nullable(Of Date), pstrNombre As String, pstrNumeroIDDeclarante As String, pstrUsuario As String) As List(Of CPX_tblFormularioEndeudamientoExterno)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_tblFormularioEndeudamientoExterno)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario6_Consultar(pintID, pdtmFecha, pstrNombre, pstrNumeroIDDeclarante, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO2", "Formulario2_Consultar")
			Return Nothing
		End Try
	End Function

	Public Function EntidadGetDetalle6Formulario() As CPX_FormularioEndeudamientoDetalle
		Try
			Return New CPX_FormularioEndeudamientoDetalle
		Catch ex As Exception
			ManejarError(ex, "Formularios", "EntidadGetDetalle6Formulario")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Detalle6Formulario_Consultar(ByVal pintIDFormulario As Integer, ByVal pstrUsuario As String) As List(Of CPX_FormularioEndeudamientoDetalle)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_FormularioEndeudamientoDetalle)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario6Detalle_Consultar(pintIDFormulario, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "DETALLE6FORMULARIO", "Detalle6Formulario_Consultar")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario6_Eliminar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario6_Eliminar(pintID, strUsuario, strInfoSession).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIOS", "Formulario6_Eliminar")
			Return Nothing
		End Try
	End Function
#End Region

#Region "Formulario7"

	Public Function EntidadGetFormulario7() As tblFormularioEndeudamientoExterno
		Try
			Return New tblFormularioEndeudamientoExterno
		Catch ex As Exception
			ManejarError(ex, "Formularios", "GetFormulario7")
			Return Nothing
		End Try
	End Function



	<Invoke(HasSideEffects:=True)>
	Public Function Formulario7_Actualizar(pintID As Nullable(Of Integer), pintFormulario As Nullable(Of Short), plogEnviado As Nullable(Of Boolean), pintTipoOperacion As Nullable(Of Short), pdtmFecha As Nullable(Of Date), pintNumeroPrestamo As Nullable(Of Integer), pstrNumeroIdentifiacion As String, plogDesembolso As Nullable(Of Boolean), pintNumeroDeclaracion As Nullable(Of Integer), pintNumeral As Nullable(Of Integer), pintIDMoneda As Nullable(Of Integer), pdblValorMonedaNegociada As Nullable(Of Decimal), pdblValorUSD As Nullable(Of Decimal), pstrTipoIdentificacionDeudor As String, pstrNumeroIdentificacionDeudor As String, pintDigitoVerificacionDeudor As Nullable(Of Short), pstrNombreDeudor As String, pintIDCiudadDeudor As Nullable(Of Integer), pstrDireccionDeudor As String, pstrTelefonoDeudor As String, pstrEmailDeudor As String, pstrIDCodigoCIIUDeudor As String, pstrCodigoBanRepAcreedor As String, pstrNombreAcreedor As String, pstrPais As String, pstrTipoAcreedor As String, pstrCodigoPropositoPrestamo As String, pintIDMonedaPrestamo As Nullable(Of Integer), pdblMontoContratado As Nullable(Of Decimal), pstrTasaInteres As String, pdblSpreadValorInteres As Nullable(Of Double), pstrNumeroDepositoFinan As String, plogIndexacion As Nullable(Of Boolean), pintIDMonedaIndexacion As Nullable(Of Integer), plogSustitucion As Nullable(Of Boolean), plogFraccionamiento As Nullable(Of Boolean), plogConsolidacion As Nullable(Of Boolean), pintNUmeroIDCreditoAnterior1 As Nullable(Of Integer), pintIDMonedaAnterior1 As Nullable(Of Integer), pdblValorAnterior1 As Nullable(Of Double), pintNUmeroIDCreditoAnterior2 As Nullable(Of Integer), pintIDMonedaAnterior2 As Nullable(Of Integer), pdblValorAnterior2 As Nullable(Of Double), pstrNombre As String, pstrNumeroIDDeclarante As String, pstrFirma As String, pdtmActualizacion As Nullable(Of Date), pstrDetalleFormulario7 As String, pstrDetalleFormulario7Eliminar As String, plogSoloValidar As Nullable(Of Boolean), pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO7")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario7_Validar(pintID,
																	   pintFormulario,
																	   plogEnviado,
																	   pintTipoOperacion,
																	   pdtmFecha,
																	   pintNumeroPrestamo,
																	   pstrNumeroIdentifiacion,
																	   plogDesembolso,
																	   pintNumeroDeclaracion,
																	   pintNumeral,
																	   pintIDMoneda,
																	   pdblValorMonedaNegociada,
																	   pdblValorUSD,
																	   pstrTipoIdentificacionDeudor,
																	   pstrNumeroIdentificacionDeudor,
																	   pintDigitoVerificacionDeudor,
																	   pstrNombreDeudor,
																	   pintIDCiudadDeudor,
																	   pstrDireccionDeudor,
																	   pstrTelefonoDeudor,
																	   pstrEmailDeudor,
																	   pstrIDCodigoCIIUDeudor,
																	   pstrCodigoBanRepAcreedor,
																	   pstrNombreAcreedor,
																	   pstrPais,
																	   pstrTipoAcreedor,
																	   pstrCodigoPropositoPrestamo,
																	   pintIDMonedaPrestamo,
																	   pdblMontoContratado,
																	   pstrTasaInteres,
																	   pdblSpreadValorInteres,
																	   pstrNumeroDepositoFinan,
																	   plogIndexacion,
																	   pintIDMonedaIndexacion,
																	   plogSustitucion,
																	   plogFraccionamiento,
																	   plogConsolidacion,
																	   pintNUmeroIDCreditoAnterior1,
																	   pintIDMonedaAnterior1,
																	   pdblValorAnterior1,
																	   pintNUmeroIDCreditoAnterior2,
																	   pintIDMonedaAnterior2,
																	   pdblValorAnterior2,
																	   pstrNombre,
																	   pstrNumeroIDDeclarante,
																	   pstrFirma,
																	   pdtmActualizacion,
																	   pstrDetalleFormulario7,
																	   pstrDetalleFormulario7Eliminar,
																		plogSoloValidar,
																		pstrUsuario,
																		strInfoSession
																		).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO7", "Formulario7_Actualizar")
			Return Nothing
		End Try
	End Function



	<Invoke(HasSideEffects:=True)>
	Public Function Formulario7_ID(ByVal pintID As Integer, ByVal pstrUsuario As String) As tblFormularioEndeudamientoExterno
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO7")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario7_ConsultarID(pintID, strUsuario, strInfoSession).First
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO7", "Formulario7_ID")
			Return Nothing
		End Try
	End Function


	<Invoke(HasSideEffects:=True)>
	Public Function Formulario7_Defecto(ByVal pstrUsuario As String) As tblFormularioEndeudamientoExterno
		Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO7")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario7_ConsultarID(-1, strUsuario, strInfoSession).First
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "Formulario7", "Formulario7_Defecto")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario7_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblFormularioEndeudamientoExterno)
		Try
			pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO7")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno = DbContext.usp_FORMULARIOS_Formulario7_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "Formulario7", "Formulario7_Defecto")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Formulario7_Consultar(pintID As String, pdtmFecha As Nullable(Of Date), pstrNombre As String, pstrNumeroIDDeclarante As String, pstrUsuario As String) As List(Of CPX_tblFormularioEndeudamientoExterno)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "FORMULARIO2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_tblFormularioEndeudamientoExterno)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario7_Consultar(pintID, pdtmFecha, pstrNombre, pstrNumeroIDDeclarante, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "FORMULARIO2", "Formulario2_Consultar")
			Return Nothing
		End Try
	End Function

	Public Function EntidadGetDetalle7Formulario() As CPX_FormularioEndeudamientoDetalle
		Try
			Return New CPX_FormularioEndeudamientoDetalle
		Catch ex As Exception
			ManejarError(ex, "Formularios", "EntidadGetDetalle7Formulario")
			Return Nothing
		End Try
	End Function

	<Invoke(HasSideEffects:=True)>
	Public Function Detalle7Formulario_Consultar(ByVal pintIDFormulario As Integer, ByVal pstrUsuario As String) As List(Of CPX_FormularioEndeudamientoDetalle)
		Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DETALLE2FORMULARIO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
			Dim objRetorno As New List(Of CPX_FormularioEndeudamientoDetalle)

			objRetorno = DbContext.usp_FORMULARIOS_Formulario6Detalle_Consultar(pintIDFormulario, strUsuario, strInfoSession).ToList

			Return objRetorno
		Catch ex As Exception
			ManejarError(ex, "DETALLE7FORMULARIO", "Detalle7Formulario_Consultar")
			Return Nothing
		End Try
	End Function
#End Region


#Region "DestinosInvFormulariosDivisas"

    <Invoke(HasSideEffects:=True)>
    Public Function GetDestinosInvFormulariosDivisas() As CPX_tblDestinosInvFormulariosDivisas
        Try
            Return New CPX_tblDestinosInvFormulariosDivisas
        Catch ex As Exception
            ManejarError(ex, "DestinosInversionFormularios", "GetDestinosInvFormulariosDivisas")
            Return Nothing
        End Try
    End Function

    Public Function EntidadGetDestinosInvFormulariosDivisas() As tblDestinosInvFormulariosDivisas
        Try
            Return New tblDestinosInvFormulariosDivisas
        Catch ex As Exception
            ManejarError(ex, "DestinosInversionFormularios", "GetDestinosInvFormulariosDivisas")
            Return Nothing
        End Try
    End Function



    <Invoke(HasSideEffects:=True)>
    Public Function DestinosInvFormulariosDivisas_Actualizar(pintId As Nullable(Of Integer),
                                                                pintIdFormulario As Nullable(Of Integer),
                                                                pintIdDestinoInversion As Nullable(Of Integer),
                                                                pstrGrupoFormulario As String,
                                                                pintNroCampoFormulario As Nullable(Of Integer),
                                                                pstrNombreCampo As String,
                                                                pintIdNumeralCambiario As Nullable(Of Integer),
                                                                plogEditable As Boolean?,
                                                                pstrNombreCampoFormulario As String,
                                                                plogEsRequerido As Boolean?,
                                                                plogAplicatodosDestinosInversion As Boolean?,
                                                                plogAplicatodosFormularios As Boolean?,
                                                                plogPermitido As Boolean?,
                                                                plogAplicatodosNumerales As Boolean?,
                                                                pdtmActualizacion As Date,
                                                                plogSoloValidar As Boolean?,
                                                                pstrUsuario As String) As List(Of CPX_tblValidacionesGenerales)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DestinosInvFormulariosDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesGenerales)

            objRetorno = DbContext.usp_DestinosInvFormulariosDivisas_Validar(pintId,
                                                                            pintIdFormulario,
                                                                            pintIdDestinoInversion,
                                                                            pstrGrupoFormulario,
                                                                            pintNroCampoFormulario,
                                                                            pstrNombreCampo,
                                                                            pintIdNumeralCambiario,
                                                                            plogEditable,
                                                                            pstrNombreCampoFormulario,
                                                                            plogEsRequerido,
                                                                            plogAplicatodosDestinosInversion,
                                                                            plogAplicatodosFormularios,
                                                                            plogPermitido,
                                                                            plogAplicatodosNumerales,
                                                                            pdtmActualizacion,
                                                                            plogSoloValidar,
                                                                            pstrUsuario,
                                                                            strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DestinosInvFormulariosDivisas", "DestinosInvFormulariosDivisas_Actualizar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DestinosInvFormulariosDivisas_Defecto(ByVal pstrUsuario As String) As tblDestinosInvFormulariosDivisas
        Try
            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DestinosInvFormulariosDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_DestinosInvFormulariosDivisas_ConsultarID(-1, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DestinosInvFormulariosDivisas", "DestinosInvFormulariosDivisas_Defecto")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DestinosInvFormulariosDivisas_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblDestinosInvFormulariosDivisas)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DestinosInvFormulariosDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_DestinosInvFormulariosDivisas_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DestinosInvFormulariosDivisas", "DestinosInvFormulariosDivisas_Filtrar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DestinosInvFormulariosDivisas_Consultar(pintId As Integer?,
                                                            pintIdFormulario As Integer?,
                                                            pintIdDestinoInversion As Integer?,
                                                            pstrGrupoFormulario As String,
                                                            pintNroCampoFormulario As Integer?,
                                                            pstrNombreCampo As String,
                                                            pintIdNumeralCambiario As Integer?,
                                                            pstrUsuario As String) As List(Of CPX_tblDestinosInvFormulariosDivisas)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DestinosInvFormulariosDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblDestinosInvFormulariosDivisas)

            objRetorno = DbContext.usp_DestinosInvFormulariosDivisas_Consultar(pintId,
                                                                                pintIdFormulario,
                                                                                pintIdDestinoInversion,
                                                                                pstrGrupoFormulario,
                                                                                pintNroCampoFormulario,
                                                                                pstrNombreCampo,
                                                                                pintIdNumeralCambiario,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DestinosInvFormulariosDivisas", "DestinosInvFormulariosDivisas_Consultar")
            Return Nothing
        End Try
    End Function

    <Invoke(HasSideEffects:=True)>
    Public Function DestinosInvFormulariosDivisas_ID(ByVal pintId As Integer, ByVal pstrUsuario As String) As tblDestinosInvFormulariosDivisas
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "DestinosInvFormulariosDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_DestinosInvFormulariosDivisas_ConsultarID(pintId, strUsuario, strInfoSession).First
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "DestinosInvFormulariosDivisas", "DestinosInvFormulariosDivisas_ID")
            Return Nothing
        End Try
    End Function

#End Region


#Region "Generar Banco República"


    ''' <summary>
    ''' Consulta de la información para el reporte legal del formulario 1
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublicaF1_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublicaF1)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublicaF1)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublicaF1(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublicaF1", "ReporteriaLegal_BcoRepublicaF1_Consultar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Consulta de la información para el reporte legal del formulario 2
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublicaF2_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublicaF2)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublicaF2)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublicaF2(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublicaF2", "ReporteriaLegal_BcoRepublicaF2_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de la información para el reporte legal del formulario 2
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublicaF3_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublicaF3)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF3")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublicaF3)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublicaF3(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublicaF3", "ReporteriaLegal_BcoRepublicaF3_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de la información para el reporte legal del formulario 4
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublicaF4_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublicaF4)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF4")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublicaF4)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublicaF4(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublicaF4", "ReporteriaLegal_BcoRepublicaF4_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de la información para el reporte legal del formulario 5
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublicaF5_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublicaF5)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF5")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublicaF5)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublicaF5(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublicaF5", "ReporteriaLegal_BcoRepublicaF5_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de la información para el reporte legal de banco repúblia detalle de formulario 2 y 5
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublica_Detalle2_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublica_Detalle2)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublica_Detalle2")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublica_Detalle2)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublica_Detalle2(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublica_Detalle2", "ReporteriaLegal_BcoRepublica_Detalle2_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de la información para el reporte legal de banco repúblia detalle de formulario 3
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="strTipoEnviado"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublicaF3_Detalle1_Consultar(pintFormulario As Integer,
                                                            dtmFechaInicial As DateTime,
                                                            dtmFechaFinal As DateTime,
                                                            strTipoEnviado As String,
                                                            pstrUsuario As String) As List(Of CPX_tblBcoRepublicaF3_Detalle1)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF3_Detalle1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblBcoRepublicaF3_Detalle1)

            objRetorno = DbContext.usp_ReporteriaLegal_BcoRepublicaF3_Detalle1(pintFormulario,
                                                                                dtmFechaInicial,
                                                                                dtmFechaFinal,
                                                                                strTipoEnviado,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublicaF3_Detalle1", "ReporteriaLegal_BcoRepublicaF3_Detalle1_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Actualiza como enviados los registros de los formularios al generar el informe de banco repúbluca
    ''' SV20181009_BCOREPUBLICA
    ''' </summary>
    ''' <param name="pintFormulario"></param>
    ''' <param name="pstrCadenaActualizar"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_BcoRepublica_Actualizar(pintFormulario As Integer,
                                                            pstrCadenaActualizar As String,
                                                            pstrUsuario As String) As Boolean
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublica_Actualizar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)

            DbContext.usp_ReporteriaLegal_BcoRepublica_Actualizar(pintFormulario,
                                                                  pstrCadenaActualizar,
                                                                                pstrUsuario,
                                                                                strInfoSession)

            Return True
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_BcoRepublica_Actualizar", "ReporteriaLegal_BcoRepublica_Actualizar")
            Return False
        End Try
    End Function

#End Region

#Region "Generar DIAN"

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales DIAN formato 1062 desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1062_Consultar(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_ArchivosLegalesDIAN_Formato1062)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1062_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ArchivosLegalesDIAN_Formato1062)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1062(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1062_Consultar", "ReporteriaLegal_DIANF1062_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales DIAN formato 1059 desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1059_Consultar(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_ArchivosLegalesDIAN_Formato1059)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1059_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ArchivosLegalesDIAN_Formato1059)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1059(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1059_Consultar", "ReporteriaLegal_DIANF1059_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales DIAN formato 1060 desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1060_Consultar(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_ArchivosLegalesDIAN_Formato1060)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1059_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ArchivosLegalesDIAN_Formato1060)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1060(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1060_Consultar", "ReporteriaLegal_DIANF1060_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales DIAN formato 1061 desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1061_Consultar(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_ArchivosLegalesDIAN_Formato1061)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1061_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ArchivosLegalesDIAN_Formato1061)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1061(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1061_Consultar", "ReporteriaLegal_DIANF1061_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales DIAN formato 1063 desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1063_Consultar(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_ArchivosLegalesDIAN_Formato1063)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1063_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ArchivosLegalesDIAN_Formato1063)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1063(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1063_Consultar", "ReporteriaLegal_DIANF1063_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales DIAN formato 1064 desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1064_Consultar(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_ArchivosLegalesDIAN_Formato1064)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1064_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ArchivosLegalesDIAN_Formato1064)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1064(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1064_Consultar", "ReporteriaLegal_DIANF1064_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales  DIAN formato 1062 en archivo XML  desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1062_GenerarXML(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_Formato_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1062_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formato_XML)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1062_XML(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1062_Consultar", "ReporteriaLegal_DIANF1062_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales  DIAN formato 1059 en archivo XML  desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1059_GenerarXML(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_Formato_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1059_GenerarXML")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formato_XML)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1059_XML(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1059_GenerarXML", "ReporteriaLegal_DIANF1059_GenerarXML")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales  DIAN formato 1060 en archivo XML  desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1060_GenerarXML(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_Formato_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1060_GenerarXML")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formato_XML)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1060_XML(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1060_GenerarXML", "ReporteriaLegal_DIANF1060_GenerarXML")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales  DIAN formato 1061 en archivo XML  desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1061_GenerarXML(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_Formato_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1061_GenerarXML")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formato_XML)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1061_XML(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1061_GenerarXML", "ReporteriaLegal_DIANF1061_GenerarXML")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales  DIAN formato 1063 en archivo XML  desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1063_GenerarXML(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_Formato_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1063_GenerarXML")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formato_XML)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1063_XML(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1063_GenerarXML", "ReporteriaLegal_DIANF1063_GenerarXML")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de movimientos legales  DIAN formato 1064 en archivo XML  desde base datos con los siguientes parametros
    ''' </summary>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="dtmFechaInicial"></param>
    ''' <param name="dtmFechaFinal"></param>
    ''' <param name="pintValorBase"></param>
    ''' <param name="pintValorInicio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_DIANF1064_GenerarXML(pintIDComitente As String,
                                                        dtmFechaInicial As DateTime,
                                                        dtmFechaFinal As DateTime,
                                                        pintValorBase As Integer,
                                                        pintValorInicio As Integer,
                                                        pstrUsuario As String) As List(Of CPX_Formato_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_DIANF1064_GenerarXML")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_Formato_XML)

            objRetorno = DbContext.usp_ReporteriaLegal_GenerarArchivosLegalesDIAN_Formato1064_XML(pintIDComitente,
                                                                                              dtmFechaInicial,
                                                                                              dtmFechaFinal,
                                                                                              pintValorBase,
                                                                                              pintValorInicio,
                                                                                              pstrUsuario,
                                                                                              strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_DIANF1064_GenerarXML", "ReporteriaLegal_DIANF1064_GenerarXML")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' JAPC20181009: metodo para traer consulta de iformacion formato seleccionado desde base datos
    ''' </summary>
    ''' <param name="pintFormato"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_Formatos_Consultar(pintFormato As Integer,
                                                       pstrUsuario As String) As CPX_FormatosDianXML
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_Formatos_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New CPX_FormatosDianXML

            objRetorno = DbContext.usp_ReporteriaLegal_tblFormatosDianXML_Consultar(pintFormato,
                                                                                    pstrUsuario,
                                                                                    strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_Formatos_Consultar", "ReporteriaLegal_Formatos_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Reportes UIAF"
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_MensualCompraVentaUIAF_Consultar(dtmFechaCorte As DateTime,
                                                                      pstrUsuario As String) As List(Of CPX_ReporteUIAF)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_MensualCompraVentaUIAF")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_ReporteUIAF)

            objRetorno = DbContext.usp_DIVISAS_MensualCompraVentaUIAF_CONSULTAR(dtmFechaCorte,
                                                                                pstrUsuario,
                                                                                strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_UIAF", "ReporteriaLegal_MensualCompraVentaUIAF_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  RABP20181017
    '''  Proceso para generar el archivo plano a partir de una ruta configurada
    ''' </summary>
    ''' <param name="pdtmFechaCorte"></param>
    ''' <param name="pstrRuta"></param>
    ''' <param name="pstrUsuario"></param>
    <Invoke(HasSideEffects:=True)>
    Public Function ReporteriaLegal_MensualCompraVentaUIAF_Exportar(pdtmFechaCorte As Date,
                                                                    pstrRuta As String,
                                                                    pstrUsuario As String,
                                                                    ByVal pstrInfoConexion As String) As List(Of CPX_RespuestaArchivoImportacion)
        Try
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[Divisas].[usp_DIVISAS_MensualCompraVentaUIAF_Exportar]"
            Dim objListaRetorno As New List(Of CPX_RespuestaArchivoImportacion)
            Dim strNombreArchivo As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_MensualCompraVentaUIAF")


            Dim objListaParametros As New List(Of SqlParameter)
            objListaParametros.Add(CrearSQLParameter("pdtmFechaCorte", pdtmFechaCorte, SqlDbType.DateTime))
            objListaParametros.Add(CrearSQLParameter("pstrRuta", pstrRuta, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", strInfoSession, SqlDbType.VarChar))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    strNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
            Next

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                If Not Directory.Exists(strRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(strRutaArchivoGeneracion)
                End If

                strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, strNombreArchivo & "_" & Now.ToString("yyyyMMdd"), ".txt", String.Empty)
            End If

            If Right(strRutaArchivoGeneracion, 1) <> "\" Then
                strRutaArchivoGeneracion = strRutaArchivoGeneracion & "\"
            End If

            If Not String.IsNullOrEmpty(strArchivoRutaGeneracion) Then
                objListaRetorno.Add(New CPX_RespuestaArchivoImportacion With {.Campo = "",
                                                                      .Columna = 0,
                                                                      .Exitoso = True,
                                                                      .Fila = 1,
                                                                      .ID = 2,
                                                                      .Mensaje = "El archivo se generó en la ruta " & strRutaArchivoGeneracion & strArchivoRutaGeneracion,
                                                                      .Tipo = ""})
            End If

            Return objListaRetorno
        Catch ex As Exception
            ManejarError(ex, "ReporteriaLegal_UIAF", "ReporteriaLegal_MensualCompraVentaUIAF_Exportar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Formato 395 y 102"
    ''' <summary>
    ''' Generar formato 395
    ''' RABP20181109_Generación de archivo de formato 395
    ''' </summary>
    ''' <param name="pdtmFechaInicial"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Formato395_Generar(pdtmFechaInicial As DateTime,
                                       pstrUsuario As String) As List(Of CPX_FORMATO_395)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_FORMATO_395)

            objRetorno = DbContext.usp_DIVISAS_DiarioCompraVenta395_Exportar(pdtmFechaInicial,
                                                                             pstrUsuario,
                                                                             strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMATO395", "Formato395_Generar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta los datos basicos como entidad, palabra clave  y fecha no habíl
    ''' RABP20181109_Generación de archivo de formato 395
    ''' </summary>
    ''' <param name="pdtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Formato395_Validar(pdtmFecha As DateTime,
                                       pstrUsuario As String) As List(Of CPX_FORMATO395_Validar)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_FORMATO395_Validar)

            objRetorno = DbContext.usp_DIVISAS_FORMATO395_Validar(pdtmFecha,
                                                                  pstrUsuario,
                                                                  strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMATO395", "Formato395_Validar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Generar formato 102
    ''' RABP20181109_Generación de archivo de formato 395
    ''' </summary>
    ''' <param name="pdtmFechaInicial"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Formato102_Generar(pdtmFechaInicial As DateTime,
                                       pstrUsuario As String) As List(Of CPX_FORMATO_102)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ReporteriaLegal_BcoRepublicaF1")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_FORMATO_102)

            objRetorno = DbContext.usp_DIVISAS_DiarioCompraVenta102_Exportar(pdtmFechaInicial,
                                                                             pstrUsuario,
                                                                             strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "FORMATO395", "Formato102_Generar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Generar Formatos 388 - 389 - 390 - 391 - 393"

    ''' <summary>
    ''' RABP201930: metodo para traer consulta de movimientos de los formatos 388-389-390 -393 en archivo .txt
    ''' </summary>
    ''' <param name="intNroMes"></param>
    ''' <param name="plogExcel"></param>
    ''' <param name="strAno"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function TransaccionesConsolidado_Generar(intNroMes As Integer,
                                                        plogExcel As Boolean,
                                                        strAno As String,
                                                        pstrUsuario As String) As List(Of CPX_TransaccionesConsolidado)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "TransaccionesConsolidado_Generar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_TransaccionesConsolidado)

            objRetorno = DbContext.usp_DIVISAS_TransaccionesConsolidado_Generar(intNroMes, plogExcel, strAno, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ransaccionesConsolidado_Generar", "ransaccionesConsolidado_Generar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' RABP20191001: metodo para traer consulta de movimientos de los formatos 388-389-390 -393 en Excel
    ''' </summary>
    ''' <param name="intNroMes"></param>
    ''' <param name="plogExcel"></param>
    ''' <param name="strAno"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function TransaccionesConsolidado_Excel(intNroMes As Integer,
                                                   plogExcel As Boolean,
                                                   strAno As String,
                                                   pstrUsuario As String) As List(Of CPX_TransaccionesConsolidado_EXCEL)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "TransaccionesConsolidado_Excel")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_TransaccionesConsolidado_EXCEL)

            objRetorno = DbContext.usp_DIVISAS_TransaccionesConsolidado_EXCEL(intNroMes, plogExcel, strAno, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "TransaccionesConsolidado_EXCEL", "TransaccionesConsolidado_EXCEL")
            Return Nothing
        End Try
    End Function

#End Region

End Class
