
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


<EnableClientAccess()>
Public Class OrdenesDivisasDomainServices
    Inherits DbDomainService(Of dbOrdenesDivisasEntities)

    Public Sub New()

    End Sub

#Region "UTILIDADES"

    <Invoke(HasSideEffects:=True)>
    Public Function Entidad_ValidacionesGenerales() As List(Of CPX_tblValidacionesOrdenes)
        Try
            Return New List(Of CPX_tblValidacionesOrdenes)
        Catch ex As Exception
            ManejarError(ex, "UTILITARIOS", "Clientes_Filtrar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "ORDENES"

    ''' <summary>
    ''' Traer el tipo complex de ordenes por defecto
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetOrdenes() As CPX_tblOrdenes
        Try
            Return New CPX_tblOrdenes
        Catch ex As Exception
            ManejarError(ex, "Ordenes", "GetOrdenes")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer la entidad de ordenes por defecto 
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    Public Function EntidadGettblOrdenes() As tblOrdenes
        Try
            Return New tblOrdenes
        Catch ex As Exception
            ManejarError(ex, "Ordenes", "EntidadGettblOrdenes")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para validar y actualizar los datos de la orden
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrDatosNegocio"></param>
    ''' <param name="pstrConfirmaciones"></param>
    ''' <param name="plogSoloValidar"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pdtmActualizacion"></param>
    ''' <param name="pstrProceso"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_Actualizar(ByVal pintID As Integer _
                                    , ByVal pstrProducto As String _
                                    , ByVal pstrDatosNegocio As String _
                                    , ByVal pstrConfirmaciones As String _
                                    , ByVal plogSoloValidar As Boolean _
                                    , ByVal pstrUsuario As String _
                                    , ByVal pdtmActualizacion As Date _
                                    , ByVal pstrProceso As String) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Ordenes_Validar(pintID _
                                                        , pstrProducto _
                                                        , pstrDatosNegocio _
                                                        , pdtmActualizacion _
                                                        , pstrConfirmaciones _
                                                        , plogSoloValidar _
                                                        , pstrUsuario _
                                                        , strInfoSession _
                                                        , False _
                                                        , pstrProceso
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "Ordenes_Actualizar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Llamado al procedimiento para validar procesos especiales de la pantalla
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrProceso"></param>
    ''' <param name="pstrConfirmaciones"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_Validar_Procesos(ByVal pintID As Integer _
                                    , ByVal pstrProducto As String _
                                    , ByVal pstrProceso As String _
                                    , ByVal pstrConfirmaciones As String _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Ordenes_Validar_Procesos(pintID _
                                                        , pstrProducto _
                                                        , pstrProceso _
                                                        , pstrConfirmaciones _
                                                        , pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "Ordenes_Validar_Procesos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer los datos por defecto para la creación de un nuevo registro de ordenes
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_Defecto(ByVal pstrUsuario As String) As tblOrdenes
        Try

            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_ConsultarID(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "Ordenes_Defecto")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Llamado al procedimiento para filtrar las órdenes
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblOrdenes)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "ORDENES_Filtrar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Llamado al procedimiento para consulta de las ordenes
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pintConsecutivo"></param>
    ''' <param name="pintVersion"></param>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pintIDComitente"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pdtmOrden"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_Consultar(ByVal pintID As Integer? _
                                    , ByVal pintConsecutivo As Long? _
                                    , ByVal pintVersion As Integer? _
                                    , ByVal pstrProducto As String _
                                    , ByVal pintIDComitente As String _
                                    , ByVal pstrTipo As String _
                                    , ByVal pdtmOrden As Date? _
                                    , ByVal pstrEstado As String _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblOrdenes)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblOrdenes)

            objRetorno = DbContext.usp_Ordenes_Consultar(pintID, pintConsecutivo, pintVersion, pstrProducto, pintIDComitente, pstrTipo, pdtmOrden, pstrEstado, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "Ordenes_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Llamado al procedimiento para consultar un registro de ordenes por ID
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_ID(ByVal pintID As Integer, ByVal pstrUsuario As String) As tblOrdenes
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_ConsultarID(pintID, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "Ordenes_ID")
            Return Nothing
        End Try
    End Function




    ''' <summary>
    ''' JAPC20200408
    ''' 'JAPC20200408_CC20200306-04: Funcion para consultar devaluacion forward segun curvas de la moneda en los dias de cumplimiento
    ''' </summary>
    ''' <param name="pintIDMoneda"></param>
    ''' <param name="pintDiasCumplimiento"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasDevaluacion_Consultar(ByVal pintIDMoneda As Integer, ByVal pintDiasCumplimiento As Integer, ByVal pdtmFecha As DateTime, ByVal pstrTipo As String, ByVal plogRetornarCurvas As Boolean, ByVal plogRetornarSelect As Boolean, ByVal pstrUsuario As String) As CPX_tblOrdenesDivisasDevaluacion
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESFORWARDDEVALUACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_ConsultarDevaluacionForward(pintIDMoneda, pintDiasCumplimiento, pdtmFecha, pstrTipo, plogRetornarCurvas, plogRetornarSelect, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "OrdenesDivisasDevaluacion_Consultar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Carga de combos órdenes"

    ''' <summary>
    ''' Proceso para la carga de combos de la pantalla de órdenes
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesCombos(ByVal pstrUsuario As String) As List(Of CPX_ComboOrdenes)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "COMBOSORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_Combos("", Nothing, Nothing, 0, 0, True, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "COMBOSORDENES", "OrdenesCombos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la carga de los combos específicos de la pantalla de órdenes
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesCombosEspecificos(ByVal pstrProducto As String,
                                             ByVal pstrCondicionTexto1 As String,
                                             ByVal pstrCondicionTexto2 As String,
                                             ByVal pstrCondicionEntero1 As Integer,
                                             ByVal pstrCondicionEntero2 As Integer,
                                             ByVal pstrUsuario As String) As List(Of CPX_ComboOrdenes)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "COMBOSORDENESESPECIFICOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_Combos(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, True, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "COMBOSORDENESESPECIFICOS", "OrdenesCombosEspecificos")
            Return Nothing
        End Try
    End Function

#End Region


#Region "Procesos ordenes"

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Proceso de cálculo de dias de vigencia de la órden
    ''' Se añade la moneda para el cálculo con el pais de la moneda
    ''' </summary>
    ''' <param name="pstrTipoCalculo"></param>
    ''' <param name="pdtmFechaInicial"></param>
    ''' <param name="pdtmFechaFinal"></param>
    ''' <param name="pintNroDias"></param>
    ''' <param name="pintMoneda"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Procesos_CalcularDiasOrden(ByVal pstrTipoCalculo As String,
                                             ByVal pdtmFechaInicial As DateTime?,
                                             ByVal pdtmFechaFinal As DateTime?,
                                             ByVal pintNroDias As Integer?,
                                               ByVal pintMoneda As Integer?,
                                             ByVal pstrUsuario As String) As List(Of CPX_CalculoDiasOrden)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "CalcularDiasOrden")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_Procesos_CalcularDiasOrden(pstrTipoCalculo, pdtmFechaInicial, pdtmFechaFinal, pintNroDias, pintMoneda, False, pstrUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CalcularDiasOrden", "Procesos_CalcularDiasOrden")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    '''  metodo para invokar  sp verificar la hora que se ingresa la orden para verificar 
    '''  que no se pase de la hora de control divisas
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>JAPC20181123_VALIDACIONESORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Procesos_VerificarFechaHoraOrden(ByVal pstrUsuario As String) As Boolean
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "VerificarFechaHoraOrden")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_VerificarFechaHoraOrden(pstrUsuario, strInfoSession).FirstOrDefault.Value
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "VerificarFechaHoraOrden", "Procesos_VerificarFechaHoraOrden")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' metodo para invokar sp para verificar que ya exista un cierre de operaciones 
    ''' del ultimo dia anterior al actual para poder realizar las operaciones del dia actual
    ''' </summary>
    ''' <param name="pstrIDMoneda"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>JAPC20181123_VALIDACIONESORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Procesos_VerificarFechaCierreAnterior(pstrIDMoneda As String, ByVal pstrUsuario As String) As Boolean
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "VerificarFechaCierreAnterior")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.uspVerificarCierreDivisasAnt(pstrIDMoneda, pstrUsuario, strInfoSession).FirstOrDefault.Value
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "VerificarFechaCierreAnterior", "Procesos_VerificarFechaCierreAnterior")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' metodo para invokar sp para verificar que ya exista un cierre de operaciones y si van a anular una orden cumplida no la deje borrar
    ''' 
    ''' </summary>
    ''' <param name="pstrIDMoneda"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20200625_VALIDACIONESORDENES_Antes de borrar</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Procesos_VerificarFechaCierreBorrado(pstrIDMoneda As String, ByVal pstrUsuario As String) As Boolean
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "VerificarFechaCierreBorrado")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_VerificarCierreDivisas_Validar(pstrIDMoneda, pstrUsuario, strInfoSession).FirstOrDefault.Value
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "VerificarFechaCierreBorrado", "Procesos_VerificarFechaCierreBorrado")
            Return Nothing
        End Try
    End Function

#End Region


#Region "Carga de combos órdenes de divisas"

    ''' <summary>
    ''' Proceso para la carga de los combos de órdenes de divisas
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasCombos(ByVal pstrUsuario As String) As List(Of CPX_ComboOrdenesDivisas)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "COMBOSORDENESDIVISAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisas_Combos("", Nothing, Nothing, 0, 0, True, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "COMBOSORDENESDIVISAS", "OrdenesDivisasCombos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la carga de combos específocos y en cascada para la pantalla de órdenes de divisas
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasCombosEspecificos(ByVal pstrProducto As String,
                                             ByVal pstrCondicionTexto1 As String,
                                             ByVal pstrCondicionTexto2 As String,
                                             ByVal pstrCondicionEntero1 As Integer,
                                             ByVal pstrCondicionEntero2 As Integer,
                                             ByVal pstrUsuario As String) As List(Of CPX_ComboOrdenesDivisas)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "COMBOSORDENESDIVISASESPECIFICOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisas_Combos(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2,
                                                                         pstrCondicionEntero1, pstrCondicionEntero2, True, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "COMBOSORDENESDIVISASESPECIFICOS", "OrdenesCombosEspecificos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "tblOrdenesReceptores"

    ''' <summary>
    ''' Traer la entidad complex de receptores de la orden
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetReceptoresOrdenes() As CPX_tblOrdenesReceptores
        Try
            Return New CPX_tblOrdenesReceptores
        Catch ex As Exception
            ManejarError(ex, "RECEPTORESORDENES", "GetReceptoresOrdenes")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la consulta de los receptores de una orden
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesReceptores_Consultar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_tblOrdenesReceptores)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "RECEPTORESORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblOrdenesReceptores)

            objRetorno = DbContext.usp_Ordenes_OrdenesReceptores_Consultar(pintID, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "RECEPTORESORDENES", "OrdenesReceptores_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer los datos por defecto para la creación de una registro de receptores de la orden
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesReceptores_Defecto(ByVal pstrUsuario As String) As CPX_tblOrdenesReceptores
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "RECEPTORESORDENES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesReceptores_Consultar(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "RECEPTORESORDENES", "OrdenesReceptores_Defecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "tblOrdenesInstrucciones"


    ''' <summary>
    ''' Consulta de la entidad complex de ordenes instrucciones
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetInstruccionesOrdenes() As CPX_tblOrdenesInstrucciones
        Try
            Return New CPX_tblOrdenesInstrucciones
        Catch ex As Exception
            ManejarError(ex, "Ordenes", "GetOrdenes")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la consulta de las instrucciones de una orden
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesInstrucciones_Consultar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_tblOrdenesInstrucciones)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesInstrucciones_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblOrdenesInstrucciones)

            objRetorno = DbContext.usp_Ordenes_OrdenesInstrucciones_Consultar(pintID, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "InstruccionesOrdenes", "OrdenesInstrucciones_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de los datos de intrucciónes por id
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesInstrucciones_ConsultarID(ByVal pintID As Integer, ByVal pstrUsuario As String) As CPX_tblOrdenesInstrucciones
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesInstrucciones_ConsultarID")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesInstrucciones_Consultar(pintID, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "InstruccionesOrdenes", "OrdenesInstrucciones_ConsultarID")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de los datos por defecto para la creación de una nueva instricción de la orden
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesInstrucciones_Defecto(ByVal pstrUsuario As String) As CPX_tblOrdenesInstrucciones
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesInstrucciones_Defecto")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesInstrucciones_Consultar(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "InstruccionesOrdenes", "OrdenesInstrucciones_Defecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de los datos de intrucciónes para la validación del GMF
    ''' </summary>
    ''' <param name="pintIdInstruccion"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pintComitente"></param>
    ''' <param name="pstrCuenta"></param>
    ''' <param name="pstrTipoReferencia"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function InstruccionesGMF_Validar(ByVal pintIdInstruccion As Integer, ByVal pstrTipo As String, ByVal pintComitente As String, ByVal pstrCuenta As String, ByVal pstrTipoReferencia As String, ByVal pstrUsuario As String) As CPX_InstruccionesGMF
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "InstruccionesGMF_Validar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_InstruccionesGMF_Validar(pintIdInstruccion, pstrTipo, pintComitente, pstrCuenta, pstrTipoReferencia, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "InstruccionesGMF", "InstruccionesGMF_Validar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "OrdenesDivisas"

    ''' <summary>
    ''' Traer la entidad de ordenes de divisas por defecto 
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    Public Function EntidadGettblOrdenesDivisas() As tblOrdenesDivisas
        Try
            Return New tblOrdenesDivisas
        Catch ex As Exception
            ManejarError(ex, "tblOrdenesDivisas", "EntidadGettblOrdenesDivisas")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de los datos de la orden de divisas
    ''' </summary>
    ''' <param name="pintIDOrden"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisas_Consultar(ByVal pintIDOrden As Integer, ByVal pstrUsuario As String) As List(Of tblOrdenesDivisas)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesDivisasDetalle_Consultar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisas_Consultar(pintIDOrden, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "OrdenesDivisas", "OrdenesDivisasDetalle_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer los datos por defecto para la creación de un detalle de pordenes de divisas
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisas_Defecto(ByVal pstrUsuario As String) As List(Of tblOrdenesDivisas)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesDivisasDetalle_Defecto")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisas_Consultar(-1, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "OrdenesDivisas", "OrdenesDivisasDetalle_Defecto")
            Return Nothing
        End Try
    End Function

#End Region


#Region "OrdenesPendientes"

    ''' <summary>
    ''' Consulta de las ordenes pendientes por cumplir
    ''' SV20181031_CUMPLIMIENTOOPERACIONES
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_PendientesCumplimiento_Consultar(ByVal pstrUsuario As String) As List(Of CPX_OrdenesPendientesCumplir)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESPENDIENTES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_OrdenesPendientesCumplir)

            objRetorno = DbContext.usp_Ordenes_PendientesCumplimiento_Consultar(pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESPENDIENTES", "Ordenes_PendientesCumplimiento_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para generar el cumplimiento de las órdenes pendientes
    ''' SV20181031_CUMPLIMIENTOOPERACIONES
    ''' </summary>
    ''' <param name="pintIdOrden"></param>
    ''' <param name="pstrFolio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_Pendientes_Cumplir(ByVal pintIdOrden As Integer,
                                               ByVal pstrFolio As String,
                                               ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESPENDIENTES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Ordenes_Pendientes_Cumplir(pintIdOrden _
                                                                    , pstrFolio _
                                                                    , pstrUsuario _
                                                                    , strInfoSession
                                                                    ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESPENDIENTES", "Ordenes_PendientesCumplimiento_Consultar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Aprobación de Preordenes"
    ''' <summary>
    ''' Consulta de las ordenes que estan en estado preorden
    ''' RABP20181203_APROBACIONPREORDEN
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_AprobacionPreordenes_Consultar(ByVal pstrUsuario As String) As List(Of CPX_AprobacionPreordenesConsultar)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "APROBACIONPREORDEN")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_AprobacionPreordenesConsultar)

            objRetorno = DbContext.usp_Ordenes_AprobacionPreordenes_Consultar(pstrUsuario _
                                                                              , strInfoSession
                                                                             ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "APROBACIONPREORDEN", "Ordenes_AprobacionPreordenes_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para generar el cambio de preorden a Pendiente
    ''' RABP20181203_APROBAR
    ''' </summary>
    ''' <param name="pintIdOrden"></param>
    ''' <param name="pdtmfechaorden"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_AprobacionPreordenes_Aprobar(ByVal pintIdOrden As Integer,
                                               ByVal pdtmfechaorden As DateTime,
                                               ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "APROBACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Ordenes_AprobacionPreordenes_Aprobar(pintIdOrden _
                                                                    , pdtmfechaorden _
                                                                    , pstrUsuario _
                                                                    , strInfoSession
                                                                    ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESPENDIENTES", "Ordenes_AprobacionPreordenes_Aprobar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Proceso para generar el cambio de preorden a desaprobada
    ''' JAPC20200212 - C-20190368
    ''' </summary>
    ''' <param name="pintIdOrden"></param>
    ''' <param name="pdtmfechaorden"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Ordenes_DesaprobacionPreordenes_Desaprobar(ByVal pintIdOrden As Integer,
                                               ByVal pdtmfechaorden As DateTime,
                                               ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "APROBACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Ordenes_AprobacionPreordenes_Aprobar(pintIdOrden _
                                                                    , pdtmfechaorden _
                                                                    , pstrUsuario _
                                                                    , strInfoSession
                                                                    ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESPENDIENTES", "Ordenes_AprobacionPreordenes_Aprobar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' metodo para consultar los correos de los receptores
    ''' </summary>
    ''' <returns>JAPC20201202</returns>
    ''' <remarks>C-20190368</remarks>
    <Invoke(HasSideEffects:=True)>
    Public Function GetCorreosReceptores_Consultar() As List(Of clx_CorreosPersonas)
        Dim objRetorno As List(Of clx_CorreosPersonas)
        objRetorno = Me.DbContext.usp_Divisas_CorreosReceptores_Consultar().ToList
        Return objRetorno
    End Function


#End Region

#Region "Cargue Operaciones"

    ''' <summary>
    ''' SV20181113: Ajustes a importacion para el manejo de preordenes
    ''' Desarrollo de pantalla para obtener los resultados de la importación
    ''' </summary>
    ''' <param name="plogSoloValidar"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function CargarArchivoOperaciones_Validar(ByVal plogSoloValidar As Boolean _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesCargaOperaciones)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OPERACIONES")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesCargaOperaciones)

            objRetorno = DbContext.usp_Ordenes_CargarArchivoOperaciones_Validar(plogSoloValidar _
                                                        , pstrUsuario _
                                                        , strInfoSession
                                                      ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "OPERACIONES", "CargarArchivoOperaciones_Validar")
            Return Nothing
        End Try
    End Function

#End Region


#Region "Modulos"

    ''' <summary>
    ''' Trear la entidad de módulos complex
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetModulos() As CPX_tblModulos
        Try
            Return New CPX_tblModulos
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "GetModulos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer la entidad de modulos
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    Public Function EntidadGettblModulos() As tblModulos
        Try
            Return New tblModulos
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "EntidadGettblModulos")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Proceso para validar y actualizar un registro de modulo
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrModulo"></param>
    ''' <param name="pstrSubModulo"></param>
    ''' <param name="pstrDescripcion"></param>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pdtmActualizacion"></param>
    ''' <param name="pstrModulosEstados"></param>
    ''' <param name="pstrModulosEstadosEliminar"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="plogSoloValidar"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Modulos_Actualizar(ByVal pintID As Integer _
                                    , ByVal pstrModulo As String _
                                    , ByVal pstrSubModulo As String _
                                    , ByVal pstrDescripcion As String _
                                     , ByVal pstrNombreConsecutivo As String _
                                    , ByVal pdtmActualizacion As Date _
                                    , ByVal pstrModulosEstados As String _
                                    , ByVal pstrModulosEstadosEliminar As String _
                                    , ByVal pstrUsuario As String _
                                    , ByVal plogSoloValidar As Boolean) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Maestros_Modulos_Validar(pintID _
                                                       , pstrModulo _
                                                       , pstrSubModulo _
                                                       , pstrDescripcion _
                                                       , pstrNombreConsecutivo _
                                                       , pdtmActualizacion _
                                                       , pstrModulosEstados _
                                                       , pstrModulosEstadosEliminar _
                                                       , plogSoloValidar _
                                                       , pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "Modulos_Actualizar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para traer un registro por defecto para la creación de un módulo
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Modulos_Defecto(ByVal pstrUsuario As String) As tblModulos
        Try

            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Modulos_ConsultarID(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "Modulos_Defecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para filtrar  los registros del maestro de mósulos
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Modulos_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblModulos)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Modulos_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "Modulos_Filtrar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Proceso para consultar los registros del maestro de módulos
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrModulo"></param>
    ''' <param name="pstrSubModulo"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Modulos_Consultar(ByVal pintID As Integer _
                                    , ByVal pstrModulo As String _
                                    , ByVal pstrSubModulo As String _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblModulos)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblModulos)

            objRetorno = DbContext.usp_Maestros_Modulos_Consultar(pintID, pstrModulo, pstrSubModulo, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "Modulos_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para consulta de módulos por ID
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function Modulos_ID(ByVal pintID As Integer, ByVal pstrUsuario As String) As tblModulos
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_Modulos_ConsultarID(pintID, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOS", "Modulos_ID")
            Return Nothing
        End Try
    End Function


#End Region

#Region "ModulosEstados"

    ''' <summary>
    ''' Traer la entidad complex de módulos estados
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetModulosEstados() As CPX_tblModulosEstados
        Try
            Return New CPX_tblModulosEstados
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOS", "GetModulosEstados")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los estados de un módulo
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstados_Consultar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_tblModulosEstados)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblModulosEstados)

            objRetorno = DbContext.usp_Maestros_ModulosEstados_Consultar(pintID, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOS", "ModulosEstados_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer los datos por defecto para la creación de un detalle de estado del módulo
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstados_Defecto(ByVal pstrUsuario As String) As CPX_tblModulosEstados
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ModulosEstados_Consultar(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOS", "ModulosEstados_Defecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "ModulosEstadosConfiguracion"

    ''' <summary>
    ''' Traer la entidad complex de configuración de módulo estados
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetModulosEstadosConfiguracion() As CPX_tblModulosEstadosConfiguracion
        Try
            Return New CPX_tblModulosEstadosConfiguracion
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "Exception")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Trear la entidad de configuración de módulos estados
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <returns></returns>
    Public Function EntidadGettblModulosEstadosConfiguracion() As tblModulosEstadosConfiguracion
        Try
            Return New tblModulosEstadosConfiguracion
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "EntidadGettblModulosEstadosConfiguracion")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para validar y/o actualizar un registro de configuración de módulos estados
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pintIdModulo"></param>
    ''' <param name="pstrEstadoInicial"></param>
    ''' <param name="pstrEstadoPermitido"></param>
    ''' <param name="pdtmActualizacion"></param>
    ''' <param name="pstrEstadoRegistro"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="plogSoloValidar"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstadosConfiguracion_Actualizar(ByVal pintID As Integer _
                                    , ByVal pintIdModulo As Integer _
                                    , ByVal pstrEstadoInicial As String _
                                    , ByVal pstrEstadoPermitido As String _
                                    , ByVal pdtmActualizacion As Date _
                                    , ByVal pstrEstadoRegistro As String _
                                    , ByVal pstrUsuario As String _
                                    , ByVal plogSoloValidar As Boolean) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOSCONFIGURACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_Maestros_ModulosEstadosConfiguracion_Validar(pintID _
                                                       , pintIdModulo _
                                                       , pstrEstadoInicial _
                                                       , pstrEstadoPermitido _
                                                       , pdtmActualizacion _
                                                       , pstrEstadoRegistro _
                                                       , plogSoloValidar _
                                                       , pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "ModulosEstadosConfiguracion_Actualizar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer los datos por defectos para la creación de un registro de configuración de módulos estados
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstadosConfiguracion_Defecto(ByVal pstrUsuario As String) As tblModulosEstadosConfiguracion
        Try

            'TODO Asegurar que la fecha retornada solo sea hasta el dia, y no incluya horas, minutos, segundos
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOSCONFIGURACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ModulosEstadosConfiguracion_ConsultarID(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "ModulosEstadosConfiguracion_Defecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Filtrar registro de configuración de móduloes estados
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstadosConfiguracion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblModulosEstadosConfiguracion)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOSCONFIGURACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ModulosEstadosConfiguracion_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "ModulosEstadosConfiguracion_Filtrar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de registros de configuración de módulos estados
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pintIdModulo"></param>
    ''' <param name="pstrEstadoInicial"></param>
    ''' <param name="pstrEstadoPermitido"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstadosConfiguracion_Consultar(ByVal pintID As Integer _
                                    , ByVal pintIdModulo As Integer _
                                    , ByVal pstrEstadoInicial As String _
                                    , ByVal pstrEstadoPermitido As String _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblModulosEstadosConfiguracion)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOSCONFIGURACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblModulosEstadosConfiguracion)

            objRetorno = DbContext.usp_Maestros_ModulosEstadosConfiguracion_Consultar(pintID, pintIdModulo, pstrEstadoInicial, pstrEstadoPermitido, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "ModulosEstadosConfiguracion_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de un registro de consiguración de módulos estados por ID
    ''' SV20180711_ORDENES
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function ModulosEstadosConfiguracion_ID(ByVal pintID As Integer, ByVal pstrUsuario As String) As tblModulosEstadosConfiguracion
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MODULOSESTADOSCONFIGURACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Maestros_ModulosEstadosConfiguracion_ConsultarID(pintID, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MODULOSESTADOSCONFIGURACION", "ModulosEstadosConfiguracion_ID")
            Return Nothing
        End Try
    End Function

#End Region

#Region "AJUSTESMESAS"

    ''' <summary>
    ''' Función para traer los ajustes de las mesas
    ''' </summary>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetAjustesMesas() As CPX_tblAjustesMesas
        Try
            Return New CPX_tblAjustesMesas
        Catch ex As Exception
            ManejarError(ex, "AJUSTESMESAS", "GetAjustesMesas")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer una nueva entidad de ajustes mesas
    ''' </summary>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    Public Function EntidadGettblAjustesMesas() As tblAjustesMesas
        Try
            Return New tblAjustesMesas
        Catch ex As Exception
            ManejarError(ex, "AJUSTESMESAS", "EntidadGettblAjustesMesas")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función para llamar el procedimiento de validar y grabar un registro
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pdtmAjuste"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrNaturaleza"></param>
    ''' <param name="pstrMesaDesde"></param>
    ''' <param name="pstrMesaHacia"></param>
    ''' <param name="pintIdBanco"></param>
    ''' <param name="pdblCantidad"></param>
    ''' <param name="pdblTasaPromedio"></param>
    ''' <param name="pstrEstado"></param>
    ''' <param name="pstrObservaciones"></param>
    ''' <param name="pdtmActualizacion"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="plogSoloValidar"></param>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function AjustesMesas_Actualizar(ByVal pintID As Integer _
                                    , ByVal pdtmAjuste As DateTime _
                                    , ByVal pstrTipo As String _
                                    , ByVal pstrNaturaleza As String _
                                    , ByVal pstrMesaDesde As String _
                                    , ByVal pstrMesaHacia As String _
                                    , ByVal pintIdBanco As Integer? _
                                    , ByVal pdblCantidad As Double _
                                    , ByVal pdblTasaPromedio As Double _
                                    , ByVal pstrEstado As String _
                                    , ByVal pstrObservaciones As String _
                                    , ByVal pstrConfirmaciones As String _
                                    , ByVal pdtmActualizacion As DateTime _
                                    , ByVal pstrUsuario As String _
                                    , ByVal plogSoloValidar As Boolean) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "AJUSTESMESAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_AjustesMesas_Validar(pintID _
                                                       , pdtmAjuste _
                                                       , pstrTipo _
                                                       , pstrNaturaleza _
                                                       , pstrMesaDesde _
                                                       , pstrMesaHacia _
                                                       , pintIdBanco _
                                                       , pdblCantidad _
                                                       , pdblTasaPromedio _
                                                       , pstrEstado _
                                                       , pstrObservaciones _
                                                       , pdtmActualizacion _
                                                       , pstrConfirmaciones _
                                                       , plogSoloValidar _
                                                       , pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "AJUSTESMESAS", "AjustesMesas_Actualizar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función para validar otros procesos en la pantalla de ajustes de mesas
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrProceso"></param>
    ''' <param name="pstrConfirmaciones"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function AjustesMesas_Validar_Procesos(ByVal pintID As Integer _
                                    , ByVal pstrProceso As String _
                                    , ByVal pstrConfirmaciones As String _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "AJUSTESMESAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_AjustesMesas_Validar_Procesos(pintID _
                                                        , pstrProceso _
                                                        , pstrConfirmaciones _
                                                        , pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "AJUSTESMESAS", "AjustesMesas_Validar_Procesos")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consultar los datos por defecto para la creación de un nuevo registro de ajustes de mesas
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function AjustesMesas_Defecto(ByVal pstrUsuario As String) As tblAjustesMesas
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "AjustesMesas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_AjustesMesas_ConsultarID(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "AjustesMesas", "AjustesMesas_Defecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función para filtrado de registros de ajustes de mesas
    ''' </summary>
    ''' <param name="pstrFiltro"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function AjustesMesas_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String) As List(Of CPX_tblAjustesMesas)
        Try
            pstrFiltro = System.Web.HttpUtility.UrlDecode(pstrFiltro)
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "AjustesMesas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_AjustesMesas_Filtrar(pstrFiltro, strUsuario, strInfoSession).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "AjustesMesas", "AjustesMesas_Filtrar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para consultar un registro de ajustes de mesas por unos parámetros específicos
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pdtmAjuste"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrNaturaleza"></param>
    ''' <param name="pstrMesaDesde"></param>
    ''' <param name="pstrMesaHacia"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function AjustesMesas_Consultar(ByVal pintID As Integer? _
                                    , ByVal pdtmAjuste As DateTime? _
                                    , ByVal pstrTipo As String _
                                    , ByVal pstrNaturaleza As String _
                                    , ByVal pstrMesaDesde As String _
                                    , ByVal pstrMesaHacia As String _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblAjustesMesas)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "AjustesMesas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblAjustesMesas)

            objRetorno = DbContext.usp_AjustesMesas_Consultar(pintID, pdtmAjuste, pstrTipo, pstrNaturaleza, pstrMesaDesde, pstrMesaHacia, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "AjustesMesas", "AjustesMesas_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para consultar un registro de ajustes de mesas mandandole el ID
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180710_AJUSTESMESAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function AjustesMesas_ID(ByVal pintID As Integer, ByVal pstrUsuario As String) As tblAjustesMesas
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "AjustesMesas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_AjustesMesas_ConsultarID(pintID, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "AjustesMesas", "AjustesMesas_ID")
            Return Nothing
        End Try
    End Function

#End Region

#Region "CIERREDIVISAS"

    ''' <summary>
    ''' Función para traer los cierres de divisas
    ''' </summary>
    ''' <returns>SV20180717_CIERREDIVISAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetCierreDivisas() As CPX_tblCierreDivisas
        Try
            Return New CPX_tblCierreDivisas
        Catch ex As Exception
            ManejarError(ex, "CierreDivisas", "GetCierreDivisas")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Procedimiento para validar y actualizar los cierres de divisas
    ''' </summary>
    ''' <param name="pdtmFecha"></param>
    ''' <param name="pintIDMoneda"></param>
    ''' <param name="pstrMesa"></param>
    ''' <param name="pstrConfirmaciones"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="plogSoloValidar"></param>
    ''' <returns>SV20180717_CIERREDIVISAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function CierreDivisas_Actualizar(ByVal pdtmFecha As Date _
                                    , ByVal pintIDMoneda As Integer _
                                    , ByVal pstrMesa As String _
                                    , ByVal pstrConfirmaciones As String _
                                    , ByVal pstrUsuario As String _
                                     , ByVal plogCierre As Boolean _
                                    , ByVal plogSoloValidar As Boolean) As List(Of CPX_tblValidacionesOrdenes)


        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "CierreDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblValidacionesOrdenes)

            objRetorno = DbContext.usp_CierreDivisas_Validar(pdtmFecha _
                                                       , pintIDMoneda _
                                                       , pstrMesa _
                                                       , pstrConfirmaciones _
                                                       , plogCierre _
                                                       , plogSoloValidar _
                                                       , pstrUsuario _
                                                        , strInfoSession
                                                        ).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CierreDivisas", "CierreDivisas_Actualizar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Procedimiento para consultar los cierres de divisas
    ''' </summary>
    ''' <param name="pdtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20180717_CIERREDIVISAS</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function CierreDivisas_Consultar(ByVal pdtmFecha As Date? _
                                    , ByVal pstrUsuario As String) As List(Of CPX_tblCierreDivisas)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "CierreDivisas")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblCierreDivisas)

            objRetorno = DbContext.usp_CierreDivisas_Consultar(pdtmFecha, pstrUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "CierreDivisas", "CierreDivisas_Consultar")
            Return Nothing
        End Try
    End Function



#End Region

#Region "OrdenesDivisas Importacion Sector real"

    ''' <summary>
    ''' RABP20190627: metodo para consultar las ordenes del serctor real Formulario 4
    ''' </summary>
    ''' <param name="dtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasSectorReal_Importar(dtmFecha As DateTime,
                                                      pstrUsuario As String) As List(Of CPX_OrdenesDivisasSectorReal)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesDivisasSectorReal_Importar")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_OrdenesDivisasSectorReal)

            objRetorno = DbContext.usp_Ordenes_OrdenesDivisasSectorReal_Importar(dtmFecha,
                                                                                 pstrUsuario,
                                                                                 strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "OrdenesDivisasSectorReal_Importar", "OrdenesDivisasSectorReal_Importar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' RABP20190627: metodo para consultar las ordenes del serctor real Formulario 4 y exportarlo a XML
    ''' </summary>
    ''' <param name="dtmFecha"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasSectorReal_Importar_XML(dtmFecha As DateTime,
                                                          pstrUsuario As String,
                                                          intConsecutivo As Integer) As List(Of CPX_OrdenesDivisasSectorReal_XML)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesDivisasSectorReal_Importar_XML")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_OrdenesDivisasSectorReal_XML)

            objRetorno = DbContext.usp_Ordenes_OrdenesDivisasSectorReal_Importar_XML(dtmFecha,
                                                                                     pstrUsuario,
                                                                                     intConsecutivo,
                                                                                     strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "OrdenesDivisasSectorReal_Importar_XML", "OrdenesDivisasSectorReal_Importar_XML")
            Return Nothing
        End Try
    End Function

#End Region

#Region "tblOrdenesDivisasMultimonedas"

    ''' <summary>
    ''' Traer la entidad complex de ordenes divisas multimoneda
    ''' </summary>
    ''' <returns>RABP20190719_Dllo de Multimoneda</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetOrdenesDivisasMultimonedas() As CPX_tblOrdenesDivisasMultimoneda
        Try
            Return New CPX_tblOrdenesDivisasMultimoneda
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASMULTIMONEDAS", "GetOrdenesDivisasMultimonedas")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la consultaR Las ordenes divisas multimonedas
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20190719_Dllo de Multimoneda</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasMultimonedas_Consultar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_tblOrdenesDivisasMultimoneda)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDIVISASMULTIMONEDAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_tblOrdenesDivisasMultimoneda)

            objRetorno = DbContext.usp_Ordenes_OrdenesDivisasMultimoneda_Consultar(pintID, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASMULTIMONEDAS", "OrdenesDivisasMultimonedas_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Traer los datos por defecto para la creación de una registro de receptores de la orden
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20190719_Dllo de Multimoneda</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasMultimoneda_Defecto(ByVal pstrUsuario As String) As CPX_tblOrdenesDivisasMultimoneda
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDIVISASMULTIMONEDAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisasMultimoneda_Consultar(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASMULTIMONEDAS", "OrdenesDivisasMultimoneda_Defecto")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Proceso para la consultaR Las ordenes divisas multimonedas
    ''' </summary>
    ''' <param name="pintIDMoneda"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20190723_Dllo de Multimoneda</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function MultimonedasCambioCruzado_Consultar(ByVal pintIDMoneda As Integer, ByVal pstrUsuario As String) As List(Of CPX_MultimonedaCambioCruzado)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "MULTIMONEDASCAMBIOCRUZADO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_MultimonedaCambioCruzado)

            objRetorno = DbContext.usp_Ordenes_MultimonedaCambioCruzado_Consultar(pintIDMoneda, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "MULTIMONEDASCAMBIOCRUZADO", "MultimonedasCambioCruzado_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "tblOrdenesDivisasDatosGiros"

    ''' <summary>
    ''' Traer la entidad complex de ordenes divisas Datos Giros
    ''' </summary>
    ''' <returns>RABP20191119_Dllo de datos Giros</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function GetOrdenesDivisasDatosGiros() As CPX_DatosGiros
        Try
            Return New CPX_DatosGiros
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASDatosGiros", "GetOrdenesDivisasDatosGiros")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la consulta de los datos de giros y datos de los beneficiarios
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20191119_Dllo de datos Giros</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasDatosGiros_Consultar(ByVal pintID As Integer, ByVal pstrUsuario As String) As List(Of CPX_DatosGiros)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDIVISASDATOSGIROS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno As New List(Of CPX_DatosGiros)

            objRetorno = DbContext.usp_Ordenes_OrdenesDivisasDatosGiros_Consultar(pintID, strUsuario, strInfoSession).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASDATOSGIROS", "OrdenesDivisasDatosGiros_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para la consulta de los datos de giros y datos de los beneficiarios por defecto
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20191119_Dllo de datos Giros</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasDatosGiros_Defecto(ByVal pstrUsuario As String) As CPX_DatosGiros
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDIVISASDATOSGIROSDEFECTO")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisasDatosGiros_Consultar(-1, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASDATOSGIROSDEFECTO", "OrdenesDivisasDatosGiros_Defecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta de los datos de intrucciónes por id
    ''' </summary>
    ''' <param name="pintID"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasDatosGiros_ConsultarID(ByVal pintID As Integer, ByVal pstrUsuario As String) As CPX_DatosGiros
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "OrdenesDivisasDatosGiros_ConsultarID")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Ordenes_OrdenesDivisasDatosGiros_Consultar(pintID, strUsuario, strInfoSession).FirstOrDefault
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "InstruccionesOrdenes", "OrdenesDivisasDatosGiros_ConsultarID")
            Return Nothing
        End Try
    End Function
#End Region


#Region "ControlCierreDivisas"

    ''' <summary>
    ''' Proceso para realizar la consulta del control de cierre de divisas.
    ''' </summary>
    ''' <returns>RABP20200529_Consulta de control cierre de Divisas</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasControlCierre_Consultar() As List(Of CPX_ControlCierreOPeracionesDivisasConsultar)
        Try
            Dim objRetorno As New List(Of CPX_ControlCierreOPeracionesDivisasConsultar)

            objRetorno = DbContext.usp_Divisas_ControlCierreOPeracionesDivisas_Consultar().ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASCONTROLCIERRECONSULTAR", "OrdenesDivisasControlCierre_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Proceso para realizar el cierre del ingreso de ordenes, para no dejar crear mas ordenes.
    ''' </summary>
    ''' <param name="pstrHoraMinuto"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>RABP20200529 Desarrollo para el tener control de cierre de las opearciones de Divisas</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasControlCierre_Actualizar(ByVal pstrHoraMinuto As String, ByVal pstrUsuario As String) As List(Of CPX_ControlCierreOPeracionesDivisasActualizar)
        Try

            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDIVISASCONTROLCIERRE")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim intErrorPersonalizado As Int32 = 0
            Dim objRetorno As New List(Of CPX_ControlCierreOPeracionesDivisasActualizar)

            objRetorno = DbContext.usp_Divisas_ControlCierreOPeracionesDivisas_Actualizar(pstrHoraMinuto, strUsuario, strInfoSession, intErrorPersonalizado).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASCONTROLCIERREACTUALIZAR", "OrdenesDivisasControlCierre_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Historico Valoración"

    ''' <summary>
    ''' Proceso para realizar la consulta del control de cierre de divisas.
    ''' </summary>
    ''' <returns>RABP20200529_Consulta de control cierre de Divisas</returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasHistoricoValoracion_Consultar(ByVal pintIDConsecutivo As Integer, ByVal pstrUsuario As String) As List(Of CPX_OrdenesValoracionHistoricoConsultar)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDIVISASCONTROLCIERRE")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim intErrorPersonalizado As Int32 = 0
            Dim objRetorno As New List(Of CPX_OrdenesValoracionHistoricoConsultar)

            objRetorno = DbContext.usp_OrdenesValoracionHistorico_Consultar(pintIDConsecutivo, pstrUsuario, strInfoSession, intErrorPersonalizado).ToList

            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENESDIVISASCONTROLCIERRECONSULTAR", "OrdenesDivisasControlCierre_Consultar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Valoracion"

    ''' <summary>
    ''' JAPC20200630_C-20200440_Metodo para realizar proceso de cierre y valoracion de operaciones forward - divisas a una fecha de corte, moneda y una clasificacion negocio (forward,nextday,spot)
    ''' </summary>
    ''' <param name="pdtmFechaCorte"></param>
    ''' <param name="pintIDMoneda"></param>
    ''' <param name="pstrClasificacionNegocio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasValoracion_Procesar(ByVal pdtmFechaCorte As Date, ByVal pintIDMoneda As Integer, ByVal pstrClasificacionNegocio As String, ByVal pstrUsuario As String) As List(Of CPX_MensajesGenerico)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESPROCESARVALORACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_OrdenesFORWARD_Valoracion(pdtmFechaCorte, pintIDMoneda, pstrClasificacionNegocio, strUsuario, strInfoSession, 0).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "OrdenesDivisasValoracion_Procesar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' JAPC20200630_C-20200440_Funcion para deshacer cierre de operaciones
    ''' </summary>
    ''' <param name="pdtmFechaCorte"></param>
    ''' <param name="pstrClasificacionNegocio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OrdenesDivisasDeshacerCierre_Procesar(ByVal pdtmFechaCorte As Date, ByVal pstrClasificacionNegocio As String, ByVal pstrUsuario As String) As List(Of CPX_MensajesGenerico)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESDESHACERVALORACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_Operaciones_DeshacerCierre(pdtmFechaCorte, pstrClasificacionNegocio, strUsuario, strInfoSession, 0).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "OrdenesDivisasDeshacerCierre_Procesar")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' 'JAPC20200630_C-20200440: Metodo para consultar operaciones forward o divisas pendientes por cerrar a una fecha de corte
    ''' </summary>
    ''' <param name="pdtmFechaCorte"></param>
    ''' <param name="pintIDMoneda"></param>
    ''' <param name="pstrClasificacionNegocio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OperacionesPendientes_Consultar(ByVal pdtmFechaCorte As Date, ByVal pintIDMoneda As Integer, ByVal pstrClasificacionNegocio As String, ByVal pstrUsuario As String) As List(Of CPX_OperacionesPendientes)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESPENDIENTESVALORACION")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_OperacionesPendientes_Valoracion_Consultar(pdtmFechaCorte, pintIDMoneda, pstrClasificacionNegocio, strUsuario, strInfoSession, 0).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "OperacionesPendientes_Consultar")
            Return Nothing
        End Try
    End Function



    ''' <summary>
    ''' JAPC20200630_C-20200440_Funcion para consultar operaciones cerradas
    ''' </summary>
    ''' <param name="pdtmFechaCorte"></param>
    ''' <param name="pintIDMoneda"></param>
    ''' <param name="pstrClasificacionNegocio"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    <Invoke(HasSideEffects:=True)>
    Public Function OperacionesValoradas_Consultar(ByVal pdtmFechaCorte As Date, ByVal pintIDMoneda As Integer, ByVal pstrClasificacionNegocio As String, ByVal pstrUsuario As String) As List(Of CPX_OperacionesValoracion)
        Try
            Dim strInfoSession As String = DemeInfoSesion(pstrUsuario, "ORDENESCERRADAS")
            Dim strUsuario As String = DemeUsuario(pstrUsuario)
            Dim objRetorno = DbContext.usp_OperacionValoracion_Consultar(pdtmFechaCorte, pintIDMoneda, pstrClasificacionNegocio, strUsuario, strInfoSession, 0).ToList
            Return objRetorno
        Catch ex As Exception
            ManejarError(ex, "ORDENES", "OperacionesValoradas_Consultar")
            Return Nothing
        End Try
    End Function


#End Region
End Class

