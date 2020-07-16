Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OyD.Infraestructura

Friend Class clsImportarLiquidaciones_MI
    Private L2SDC As New OyD_ImportacionesDataContext
    Public Property gstrUser As String
    Public Property gbtyIDBOLSA As Byte
    Private gStringErrores As New StringBuilder

    Private mintEstado As Integer
    Private mlogFiltro As Boolean
    Private mlogDirty As Boolean
    Private mstrCaption As String

    'Constante para identificar los carruseles
    Public Const STRCARRUSEL As String = "CRR"
    Private rstConsultarDatosLiquidacion As List(Of usp_Bolsa_ConsultarOperacionExistente_VALIDAR_MIResult)

    Private RegBuenos As Long
    Private mstrEspecie As String
    Private mdtmCumplimiento As Date

    Private dtmFechaAbiertaUsuario As Date   ' Fecha abierta para un usuario (tblFechaCierreUsuario)
    Private dblPatrimonioTecnicoFirma As Double
    Private dblSuperaPatrimonioTecnico As Double
    Dim dblPorcentajeIvaComision As Double = -1 'Se inicializa en -1 para no consultar por cada registro
    Dim strNemotecnico As String

#Region "Constantes"

    Private Const PORC_RETFTE As Double = 0.02
    Private Const IDBOLSABOG As Byte = 1
    Private Const IDBOLSAMED As Byte = 2
    Private Const IDBOLSAOCC As Byte = 3
    Private Const IDOTROSMED As Byte = 5
    Private Const IDBOLSACOL As Byte = 4

    Private Const STRMOVIMI As String = "Otros Med"
    Private Const STR_ESPECIE_TES As String = "TESB"
    Private Const STR_FISICO As String = "F"
    Private Const STR_DECEVAL As String = "D"
    Private Const STR_DCV As String = "V"

    'Constantes para los mensajes de error
    Private Const MSGERROR_SERVIDOROCUPADO As String = "El servidor se encuentra ocupado.  Desea intentarlo de nuevo?"
    Private Const MSGERROR_FECHAS As String = "La fecha inicial debe ser menor que la fecha final"
    Private Const MSGERROR_INGRESAR_NOMBREARCHIVO As String = "Debe ingresar el nombre del archivo"
    'Private Const MSGERROR_INGRESAR_NOMBREARCHIVO_DETALLE As String = "Debe ingresar el nombre del archivo de detalle"
    Private Const MSGERROR_ABRIR_ARCHIVO As String = "Error tratando de abrir el archivo"
    Private Const MSGERROR_IMPORTACION As String = "Importación de Liquidaciones"
    Private Const MSGERROR_INICIO_IMPORTACION As String = "Inicio de la importación"
    Private Const MSGERROR_FIN_IMPORTACION As String = "Fin de la Importación"
    Private Const MSGERROR_ENCABEZADO As String = "   1. Archivo de Encabezado : "
    Private Const MSGERROR_DETALLES As String = "   2. Archivo de Detalle : "
    Private Const MSGERROR_TOTAL_LEIDOS As String = "   Total de Registros Leidos  "
    Private Const MSGERROR_TOTAL_IMPORTADOS As String = "   Total de Registros Importados   "
    Private Const MSGERROR_EXISTEN_LIQUIDACIONES As String = "Existen liquidaciones de Compra y de Venta para la liquidación  '"
    Private Const MSGERROR_EXISTEN_LIQUIDACIONES_COMPRA As String = "Existen liquidaciones de Compra para la liquidación  '"
    Private Const MSGERROR_EXISTEN_LIQUIDACIONES_VENTA As String = "Existen liquidaciones de Venta para la liquidación  '"
    Private Const MSGERROR_EXISTEN_LIQUIDACIONES_RECOMPRA As String = "Existen liquidaciones de ReCompra para la liquidación  '"
    Private Const MSGERROR_EXISTEN_LIQUIDACIONES_REVENTA As String = "Existen liquidaciones de ReVenta para la liquidación  '"
    Private Const MSGERROR_PARCIAL As String = "' y parcial  '"
    Private Const MSGERROR_RANGO_FECHAS As String = "Esta liquidación NO se encuentra en el rango de fechas dado"
    Private Const MSGERROR_EXISTE_DETALLE As String = "El detalle existe en Liquidaciones"
    Private Const MSGERROR_ESPECIE As String = "La especie '"
    Private Const MSGERROR_TABLA_ESPECIES As String = "' no existe en la relación de Especies con Bolsa"
    Private Const MSGERROR_LIQUIDACION As String = "La Liquidación '"
    Private Const MSGERROR_TABLA_LIQUIDACIONES As String = "' ya existe en la tabla de Liquidaciones"

    'Constantes para los mensajes de error de las conversiones
    Private Const MSGERROR_CONVERSION_TRANSACCION As String = "Error de conversión del 'Número Transacción'"
    Private Const MSGERROR_CONVERSION_ORDEN_COMPRA As String = "Error de conversión de la 'Orden de compra'"
    Private Const MSGERROR_CONVERSION_ORDEN_VENTA As String = "Error de conversión de la 'Orden de venta'"
    Private Const MSGERROR_CONVERSION_FECHA_NEGOCIO As String = "Error de conversión de la 'Fecha de negocio'"
    Private Const MSGERROR_CONVERSION_ESPECIE As String = "Error de conversión de la 'Especie'"
    Private Const MSGERROR_CONVERSION_FECHA_CUMPLIMIENTO As String = "Error de conversión de la 'Fecha de cumplimiento'"
    Private Const MSGERROR_CONVERSION_CANTIDAD As String = "Error de conversión de la 'Cantidad'"
    Private Const MSGERROR_CONVERSION_PRECIO As String = "Error de conversión del 'Precio'"
    Private Const MSGERROR_CONVERSION_MERCADO As String = "Error de conversión del 'Mercado (P/S)'"
    Private Const MSGERROR_CONVERSION_FORMA_NEGOCIO As String = "Error de conversión de la 'Forma Negocio (N/R/C/M/O)'"
    Private Const MSGERROR_CONVERSION_NEGOCIO_PLAZO As String = "Error de conversión del 'Negocio a plazo (S/N)'"
    Private Const MSGERROR_CONVERSION_OPERADOR_COMPRA As String = "Error de conversión del 'Operador que compra'"
    Private Const MSGERROR_CONVERSION_OPERADOR_VENDE As String = "Error de conversión del 'Operador que vende'"
    Private Const MSGERROR_CONVERSION_VALOR_BRUTO As String = "Error de conversión del 'Valor bruto'"
    Private Const MSGERROR_CONVERSION_TASA_EFECTIVA As String = "Error de conversión de 'Tasa Efectiva'"
    Private Const MSGERROR_CONVERSION_OTRA_PLAZA As String = "Error de conversión de la 'Otra plaza (LO/BO/OC)'"
    Private Const MSGERROR_CONVERSION_HORA_NEGOCIO As String = "Error de conversión de la 'Hora del negocio HHMMSSmmm'"
    Private Const MSGERROR_CONVERSION_FIRMA_OTRA_PLAZA As String = "Error de conversión de la 'Firma otra plaza'"
    Private Const MSGERROR_CONVERSION_CIUDAD_ORIGEN As String = "Error de conversión de la 'Ciudad Origen'"
    Private Const MSGERROR_CONVERSION_CIUDAD_CUMPLIMIENTO As String = "Error de conversión de la 'Ciudad Cumplimiento'"
    Private Const MSGERROR_CONVERSION_NUMERO_ADICIONAL As String = "Error de conversión del 'Número de Adicional'"
    Private Const MSGERROR_CONVERSION_TIPO_ADICIONAL As String = "Error de conversión del 'Tipo de Adicional'"
    Private Const MSGERROR_CONVERSION_TIPO_LIQUIDACION As String = "Error de conversión del 'Tipo de Liquidacion'"
    Private Const MSGERROR_CONVERSION_OPERADOR As String = "Error de conversión del 'Operador'"
    Private Const MSGERROR_CONVERSION_PORCENTAJE_COMISION As String = "Error de conversión del 'Porcentaje de Comisión'"
    Private Const MSGERROR_CONVERSION_VALOR_COMISION As String = "Error de conversión del 'Valor de Comisión'"
    Private Const MSGERROR_CONVERSION_VALOR_NETO As String = "Error de conversión del 'Valor Neto'"
    Private Const MSGERROR_CONVERSION_RETEFUENTE_COMISION As String = "Error de conversión de la 'ReteFuente por Comision'"
    Private Const MSGERROR_CONVERSION_RETEFUENTE_BOLSA As String = "Error de conversión de la 'ReteFuente por Servicio Bolsa'"
    Private Const MSGERROR_CONVERSION_RETEFUENTE As String = "Error de conversión de la 'Retención en la Fuente'"
    Private Const MSGERROR_CONVERSION_IVA As String = "Error de conversión del 'IVA'"
    Private Const MSGERROR_CONVERSION_POSICION_PROPIA As String = "Error de conversión de la 'Posición Propia S/N'"
    Private Const MSGERROR_CONVERSION_DECEVAL As String = "Error de conversión del 'Deceval S/N'"
    Private Const MSGERROR_CONVERSION_DIAS_VENCIMIENTO As String = "Error de conversión de los días al Vencimiento"
    Private Const MSGERROR_CONVERSION_PLAZA As String = "Error de conversión de la 'plaza'"
    Private Const MSGERROR_CONVERSION_MODALIDAD_PLAZA As String = "Error de conversión de la Modalidad Otra Plaza"
    Private Const MSGERROR_CONVERSION_REPO As String = "Error de conversión de Identificador de 'Repo S/N'"
    Private Const MSGERROR_CONVERSION_MARTILLO As String = "Error de conversión de Identificador de 'Martillo S/N'"
    Private Const MSGERROR_CONVERSION_CARRUSEL As String = "Error de conversión de Identificador de 'Carrusel S/N'"
    Private Const MSGERROR_CONVERSION_FECHA_VENCIMIENTO As String = "Error de conversión de la 'Fecha Vencimiento'"
    Private Const MSGERROR_CONVERSION_FECHA_EMISION As String = "Error de conversión de la 'Fecha Emisión'"
    Private Const MSGERROR_CONVERSION_PERIODO_PAGO As String = "Error de conversion del 'Período Pago M/B/T/C/S/A/ /U'"
    Private Const MSGERROR_CONVERSION_MODALIDAD_PAGO As String = "Error de conversión de 'Modalidad de Pago'"
    Private Const MSGERROR_CONVERSION_CIUDAD_EXPEDICION As String = "Error de conversión de 'Ciudad de Expedición'"
    Private Const MSGERROR_CONVERSION_APLAZAMIENTO As String = "Error de conversión del 'Aplazamiento'"
    Private Const MSGERROR_CONVERSION_RENTABILIDAD_COMPRADOR As String = "Error de conversión de la 'Rentabilidad del Comprador'"
    Private Const MSGERROR_CONVERSION_RENTABILIDAD_VENDEDOR As String = "Error de conversión de la 'Rentabilidad del Vendedor'"
    Private Const MSGERROR_CONVERSION_VALOR_BOLSA As String = "Error de conversión del 'Valor de la Bolsa'"
    Private Const MSGERROR_CONVERSION_FECHA_TRANSACCION As String = "Error de conversión de la 'Fecha de Transacción'"
    Private Const MSGERROR_CONVERSION_CIUDAD_TRANSACCION As String = "Error de conversión de la 'Ciudad de Transacción'"
    Private Const MSGERROR_CONVERSION_PUNTOS_ADICIONALES As String = "Error de conversión de los 'Puntos Adicionales'"
    Private Const MSGERROR_CONVERSION_PORRETEFUENTE As String = "Error de conversión del 'Porcentaje de Retención en la Fuente'"
    Private Const MSGERROR_CONVERSION_PORSERVBOLSA As String = "Error de conversión del 'Porcentaje de Servicio de la Bolsa'"
    Private Const MSGERROR_CONVERSION_PORRENTANOMINAL As String = "Error de conversión del 'Porcentaje de Rentabilidad Nominal'"
    Private Const MSGERROR_CONVERSION_DEPOSITO_VALORES As String = "Error de conversión del 'Depósito de Valores DEC/DCV/' ''"
    Private Const MSGERROR_CONVERSION_INDICADOR As String = "Error de conversión del 'Indicador DTF/TCC/IPC/CM/TRM'"
    Private Const MSGERROR_CONVERSION_BENEFICIARIO As String = "Error de conversión del 'Beneficiario de la Especie'"
    Private Const MSGERROR_CONVERSION_DIAS_INTERESES As String = "Error de conversión de los 'Días de Interés'"
    Private Const MSGERROR_CONVERSION_FECHA_RECOMPRA As String = "Error de conversión de la 'Fecha de Recompra en Repo'"
    Private Const MSGERROR_CONVERSION_PRECIO_RECOMPRA As String = "Error de conversión del 'Precio de Recompra en Repo'"
    Private Const MSGERROR_CONVERSION_RENTA_RECOMPRA As String = "Error de conversión de la 'Rentabilidad de Remate en Repo'"
    Private Const MSGERROR_CONVERSION_DIAS_REPO As String = "Error de conversión de los 'Días Repo'"
    Private Const MSGERROR_CONVERSION_REINVERSION As String = "Error de conversión de la 'Reinversión'"
    Private Const MSGERROR_CONVERSION_SWAP As String = "Error de conversión del 'Swap'"
    Private Const MSGERROR_CONVERSION_CERTIFICACION As String = "Error de conversión de la 'Certificación'"
    Private Const MSGERROR_CONVERSION_DESCUENTO_ACUMULA As String = "Error de conversión del 'Descuento Acumula'"
    Private Const MSGERROR_CONVERSION_PCTRENDIMIENTO As String = "Error de conversión del 'Porcentaje Rendimiento'"
    Private Const MSGERROR_CONVERSION_FECHA_COMPRA_VENCIDO As String = "Error de conversión de la 'Fecha Compra Vencido'"
    Private Const MSGERROR_CONVERSION_PRECIO_COMPRA_VENCIDO As String = "Error de conversión del 'Precio Compra Vencido'"
    Private Const MSGERROR_CONVERSION_CONSTANCIA_ENAJENACION As String = "Error de conversión de la 'Constancia Enajenación'"
    Private Const MSGERROR_CONVERSION_REPO_TITULO As String = "Error de conversión del 'Repo Título'"
    Private Const MSGERROR_CONVERSION_VALOR_BRUTO_COMPRA_VENCIDA As String = "Error de conversión del 'Valor Bruto Compra Vencida'"
    Private Const MSGERROR_CONVERSION_AUTORETENEDOR As String = "Error de conversión del 'AutoRetenedor'"
    Private Const MSGERROR_CONVERSION_SUJETO As String = "Error de conversión del 'Sujeto'"
    Private Const MSGERROR_CONVERSION_PCREN_EFEC_COMPRA_RET As String = "Error de conversión del 'Pc Ren Efec Compra Ret'"
    Private Const MSGERROR_CONVERSION_PCREN_EFEC_VENDE_RET As String = "Error de conversión del 'Pc Ren Efec Vende Ret'"
    Private Const MSGERROR_CONVERSION_NUMERO_OPERACION As String = "Error de conversión del 'Número de Operación'"
    Private Const MSGERROR_CONVERSION_POS_PROPIA As String = "Error de conversión de la 'Posición Propia'"
    Private Const MSGERROR_CONVERSION_PLAZO_TITULO As String = "Error de conversión del 'Plazo del título'"
    Private Const MSGERROR_CONVERSION_INTERES_NOMINAL As String = "Error de conversión del 'Interés nominal'"
    Private Const MSGERROR_CONVERSION_COMPRA_VENTA As String = "Error de conversión del 'Precio de Compra o Venta'"
    Private Const MSGERROR_CONVERSION_TIPO As String = "Error de conversión del 'Tipo de Liquidación'"
    Private Const MSGERROR_CONVERSION_COMISIONISTA_LOCAL As String = "Error de conversión del 'Comisionista Local'"
    Private Const MSGERROR_CONVERSION_INTERESES As String = "Error de conversión de los 'Intereses'"
    Private Const MSGERROR_CONVERSION_RETENCION As String = "Error de conversión de la 'Retención'"
    Private Const MSGERROR_CONVERSION_COMISION As String = "Error de conversión de la 'Comisión'"
    Private Const MSGERROR_CONVERSION_TOTAL As String = "Error de conversión del 'Total'"
    Private Const MSGERROR_CONVERSION_PARCIAL As String = "Error de conversión del 'Parcial'"
    Private Const MSGERROR_CONVERSION_TIPO_OPERACION As String = "Error de conversión del 'Tipo de Operación'"

    'Constantes para los estados del maker and checker
    Private Const ESTADO_PENDIENTE_POR_APROBACION As String = "PA"
    Private Const ESTADO_PENDIENTE_POR_APROBACION_CLIENTE As String = "por aprobar"
    Private Const ESTADO_7_RETIRADO_PENDIENTE_POR_APROBACION As String = "07"
    Private Const ESTADO_8_APROBADO_LUEGO_DE_RETIRO As String = "08"
    Private Const ESTADO_10_ACTIVADO_PENDIENTE_POR_APROBACION As String = "10"
    Private Const ESTADO_12_RECHAZADO_LUEGO_DE_ACTIVADO As String = "12"

#End Region

    ''' <summary>
    '''  Consulta la existencia de una orden que cumpla con los parametros dados.
    ''' </summary>
    ''' <param name="pstrOrden">Numero de la Orden.</param>
    ''' <param name="pstrTipo">Tipo de la orden.</param>
    ''' <param name="pstrClase"> Clase de la orden.</param>
    ''' <param name="pstrEspecie">Nemotecnico de la Especie de la orden.</param>
    ''' <param name="pstrNroDocumento">Numero de identificación que viene del archivo plano.</param>
    ''' <param name="plngIdComitente">Código del cliente en la orden (SI ESTA EXISTE).</param>
    ''' <param name="plngIdOrdenante">Código del ordenante en la orden (SI ESTA EXISTE).</param>
    ''' <param name="plogOrdinaria">TRUE indica Liq Ordinaria FALSE Liq Extraordinaria (SI ESTA EXISTE).</param>
    ''' <param name="pstrObjeto">Nombre del objeto que tiene la liq (SI ESTA EXISTE).</param>
    ''' <param name="pstrDeposito">Nombre del deposito que tiene la liq (SI ESTA EXISTE).</param>
    ''' <param name="pstrEstadoAprobOrden"></param>
    ''' <param name="pstrEstadoAprobCliente"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks>
    ''' Modificado Por	:	Juan Carlos Soto Cruz.
    ''' Descripcion		:	Se adicionan la asignacion a los campos pstrEstadoAprobOrden y pstrEstadoAprobCliente
    ''' Fecha			:	Octubre 05/2011
    ''' </remarks>
    Private Function VerificarOrden(ByVal pstrOrden As String,
                                    ByVal pstrTipo As String,
                                    ByVal pstrClase As String,
                                    ByVal pstrEspecie As String,
                                    ByVal pstrNroDocumento As String,
                                    ByRef plngIdComitente As String,
                                    ByRef plngIdOrdenante As String,
                                    ByRef plogOrdinaria As Boolean,
                                    ByRef pstrObjeto As String,
                                    ByRef pstrDeposito As String,
                                    ByRef pstrEstadoAprobOrden As String,
                                    ByRef pstrEstadoAprobCliente As String,
                                    ByRef pstrEstadoOrden As String) As Boolean
        VerificarOrden = True

        Dim ret = (L2SDC.uspOyDNet_Importaciones_VerificarOrdenLiq_MI(pstrTipo, pstrClase, CType(CLng(pstrOrden), Integer?), CStr(pstrEspecie), CStr(pstrNroDocumento))).ToList
        If ret.Count > 0 Then
            Dim PrimerRegistro = ret(0)
            VerificarOrden = True
            plngIdComitente = PrimerRegistro.lngIDComitente
            plngIdOrdenante = PrimerRegistro.lngIDOrdenante
            plogOrdinaria = CBool(IIf(PrimerRegistro.logOrdinaria, True, False))
            pstrObjeto = PrimerRegistro.strObjeto & ""
            pstrDeposito = PrimerRegistro.strUBICACIONTITULO
            pstrEstadoAprobOrden = PrimerRegistro.strEstadoMakerChecker
            pstrEstadoAprobCliente = PrimerRegistro.strEstadoAprobacionCliente
            pstrEstadoOrden = PrimerRegistro.strEstado
        Else
            VerificarOrden = False
        End If
    End Function

    Private Function VerificarSiEspecieExiste_MI(ByRef plngIdBolsa As Integer, ByRef pstrIdEspecie As String, _
                                      ByRef pstrlogEsAccion As Boolean) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function VerificarSiEspecieExiste_MI(plngIDBolsa As Long, ByRef pstrIdEspecie As String,  ByRef pstrlogEsAccion As Boolean) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Verificamos que la especie exista en la base de datos
        '/* Parámetros  :
        '/* Por             Nombre              Tipo        Descripción
        '/* Referencia      plngIDBolsa         Long        Bolsa donde se negocia la especie
        '/* Referencia      pstrIdEspecie       String      Nemotecnico de la especie
        '/* Referencia      pstrlogEsAccion     Boolean     Indica si la Especie es Accion o es Renta fija
        '/* Valores de retorno:
        '/* VerificarEspecie: Devuelve TRUE si la especie existe
        '/*                   Devuelve FALSE si NO existe.
        '/* FIN DOCUMENTO
        '/******************************************************************************************
        'On Error GoTo Err_VerificarEspecie

        VerificarSiEspecieExiste_MI = True
        'SLB se adiciona el retorno de la bolsa cuando se verifica la especie
        Dim ret = (L2SDC.uspOyDNet_Importaciones_VerificarEspecies_MI(Trim(pstrIdEspecie))).ToList
        If ret.Count > 0 Then
            Dim PrimerRegistro = ret(0)
            pstrIdEspecie = PrimerRegistro.strIdEspecie
            pstrlogEsAccion = PrimerRegistro.logEsAccion
            plngIdBolsa = PrimerRegistro.lngidBolsa
        Else
            pstrIdEspecie = Trim(pstrIdEspecie)
            pstrlogEsAccion = CBool(0)
            plngIdBolsa = 0
            VerificarSiEspecieExiste_MI = False
        End If

        Return VerificarSiEspecieExiste_MI
    End Function

    Private Function ExisteLiquidacion_MI(ByVal pdtmLiquidacion As Date, ByVal plngIDLiq As Long, ByVal plngParcial As Long, _
        ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDBolsa As Long) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function ExisteLiquidacion(pdtmLiquidacion As Date, plngIDLiq As Long, plngParcial As Long,  pstrTipo As String, pstrClase As String, plngIDBolsa As Long) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Verifica que una liquidación exista en la base de datos
        '/* Parámetros  :
        '/* Por             Nombre              Tipo        Descripción
        '/* Referencia      pdtmLiquidacion     Date        fecha de elaboracion de la Liquidación
        '/* Referencia      plngIDLiq           Long        Numero de la Liquidación
        '/* Referencia      plngParcial         Long        Parcial de la liquidación
        '/* Referencia      pstrTipo            string      Tipo de la Liquidación
        '/* Referencia      pstrClase           string      Clase de la Liquidación
        '/* Referencia      plngIDBolsa         Long        Codigo de la Bolsa
        '/* Valores de retorno:
        '/* ExisteLiquidacion:  Devuelve TRUE si la liquidación existe
        '/*                     Devulve FALSE si la Liquidacion NO existe.
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        ExisteLiquidacion_MI = False
        'Try
        Dim plogexiste As Boolean
        Dim ret = L2SDC.uspOyDNet_Importaciones_BuscarLiquidacion_MI(CDate(pdtmLiquidacion), CType(plngIDLiq, Integer?), CType(plngParcial, Integer?), pstrTipo, pstrClase, CType(plngIDBolsa, Integer?), CType(plogexiste, Integer?)).ToList

        If ret.Count <= 0 Then 'No Existe
            ExisteLiquidacion_MI = False
        Else
            ExisteLiquidacion_MI = True
        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        '    ExisteLiquidacion = False
        '    'LogErrores(Me.Name & ".ExisteLiquidacion", Err.Number, Err.Description)
        'End Try
        Return ExisteLiquidacion_MI
    End Function

    Private Function ExisteDetalleLiqSinClase(ByVal plngIDLiq As Long, ByVal plngParcial As Long, ByVal pstrTipo As String, ByVal plngIDBolsa As Long) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function ExisteDetalleLiqSinClase(plngIDLiq As Long, plngParcial As Long, pstrTipo As String) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Se busca si existe un registro con las mismas características de los parametros
        '/*                 en tblImportacionLiq.
        '/* Parámetros  :
        '/* Por             Nombre              Tipo        Descripción
        '/* Referencia      plngIDLiq           long        Número de la Liquidación
        '/* Referencia      plngParcial         long        Numero del parcial de la liquidación
        '/* Referencia      pstrTipo            string      Tipo de operación
        '/* Valores de retorno:
        '/* ExisteDetalleLiq:   Devuelve TRUE si la Liquidacion con esas caracteristicas YA EXISTE
        '/*                     Devuelve FALSE si la Liquidación NO existe.
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        ExisteDetalleLiqSinClase = False
        'Try
        Dim ret = L2SDC.uspOyDNet_Importaciones_LiquidacionesImpSinClase(CType(plngIDLiq, Integer?), CType(plngParcial, Integer?), CStr(pstrTipo), CType(plngIDBolsa, Integer?)).ToList
        If Not ret Is Nothing Then
            If ret.Count = 0 Then
                ExisteDetalleLiqSinClase = False
            Else
                ExisteDetalleLiqSinClase = True
            End If
        Else
            ExisteDetalleLiqSinClase = False
        End If

        Return ExisteDetalleLiqSinClase
    End Function

    Private Function ExisteDetalleLiqSinClaseNew(ByVal plngIDLiq As Long, ByVal pstrTipo As String, ByVal plngIDBolsa As Long) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function ExisteDetalleLiqSinClaseNew(plngIDLiq As Long, pstrTipo As String) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Se busca si existe un registro con las mismas características de los parametros
        '/*                 en tblImportacionLiq. Es decir que exista el cruce de una operación.
        '/*                 El cruce de una venta es la compra y viceversa.
        '/* Parámetros  :
        '/* Por             Nombre              Tipo        Descripción
        '/* Referencia      plngIDLiq           long        Número de la Liquidación
        '/* Referencia      pstrTipo            string      Tipo de operación
        '/* Valores de retorno:
        '/* ExisteDetalleLiq:   Devuelve TRUE si la Liquidacion con esas caracteristicas YA EXISTE
        '/*                     Devuelve FALSE si la Liquidación NO existe.
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        ExisteDetalleLiqSinClaseNew = False
        'Try
        'Dim plogexiste As Boolean
        Dim ret = L2SDC.uspOyDNet_Importaciones_LiquidacionesImpSinClaseNew(CType(plngIDLiq, Integer?), pstrTipo, CType(plngIDBolsa, Integer?)).ToList
        If Not ret Is Nothing Then
            If ret.Count = 0 Then
                ExisteDetalleLiqSinClaseNew = False
            Else
                ExisteDetalleLiqSinClaseNew = True
            End If
        Else
            ExisteDetalleLiqSinClaseNew = False
        End If

        Return ExisteDetalleLiqSinClaseNew
    End Function

    Private Function ExisteDetalleLiq(ByVal plngIDLiq As Long, ByVal plngParcial As Long, ByVal pstrTipo As String, _
        ByVal pstrClase As String) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function ExisteDetalleLiq(plngIDLiq As Long, plngParcial As Long, pstrTipo As String,pstrClase As String) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Se busca si existe un registro con las mismas características de los parametros
        '/*                 en tblImportacionLiq.
        '/* Parámetros  :
        '/* Por             Nombre              Tipo        Descripción
        '/* Referencia      plngIDLiq           long        Número de la Liquidación
        '/* Referencia      plngParcial         long        Numero del parcial de la liquidación
        '/* Referencia      pstrTipo            string      Tipo de operación
        '/* Referencia      pstrClase           string      Clase de operación.
        '/* Valores de retorno:
        '/* ExisteDetalleLiq:   Devuelve TRUE si la Liquidacion con esas caracteristicas YA EXISTE
        '/*                     Devuelve FALSE si la Liquidación NO existe.
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        ExisteDetalleLiq = False

        Dim ret = L2SDC.uspOyDNet_Importaciones_LiquidacionesImportExisteDet(CType(plngIDLiq, Integer?), CType(plngParcial, Integer?), pstrTipo, pstrClase).ToList
        If Not ret Is Nothing Then
            If ret.Count <= 0 Then 'No Existe
                ExisteDetalleLiq = False
            Else
                ExisteDetalleLiq = True
            End If
        Else
            ExisteDetalleLiq = False
        End If

        Return ExisteDetalleLiq
    End Function

    Private Sub CrearPreparedStatements()

    End Sub

    Public Function BorrarEncabezados_MI() As String
        Dim retorno As String = ""
        Try
            Dim res = L2SDC.ExecuteQuery(Of Object)("delete from tblImportacionLiq_MI")
            retorno = "Se eliminaron correctamente todas las Importaciones"
        Catch ex As Exception
            retorno = "Error: " & ex.Message
        End Try
        Return retorno
    End Function

    Private Sub Validar_Campo_a_Campo(ByVal pdtmLiquidacion As Date, ByVal plngIDLiq As Long, ByVal plngParcial As Long, _
                                        ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDBolsa As Long)

        Dim ret As List(Of usp_Bolsa_ConsultarOperacionExistente_VALIDAR_MIResult) = L2SDC.uspOyDNet_Importaciones_ConsultarOperacionExistente_MI(CDate(pdtmLiquidacion), CType(plngIDLiq, Integer?), CType(plngParcial, Integer?), pstrTipo, _
                                                pstrClase, CType(plngIDBolsa, Integer?)).ToList

        rstConsultarDatosLiquidacion = ret

    End Sub

    Sub LogDiferencias(ByVal pstrDato As String, ByVal pstrMensaje As String)
        On Error GoTo Err_LogDiferencias

        Call Reportar(vbTab & "ERROR-DIFERENCIA" & vbTab & pstrMensaje & vbTab & " (Dato en OyD: " & pstrDato & ")")

Exit_LogDiferencias:
        Exit Sub

Err_LogDiferencias:

    End Sub

    Sub LogImportacion(ByVal pintLinea As Integer, ByVal pstrDato As String, ByVal pstrMensaje As String)
        On Error GoTo Err_LogImportacion

        Call Reportar(" Línea " & pintLinea & ": " & vbTab & pstrMensaje & vbTab & " (Dato: " & pstrDato & ")")

Exit_LogImportacion:
        Exit Sub

Err_LogImportacion:

    End Sub

    Private Function EstaEn(ByVal pstrValor As String, ByVal pstrOpciones As String) As Boolean
        On Error GoTo Err_EstaEn

        pstrOpciones = pstrOpciones & "/"

        EstaEn = (InStr(pstrOpciones, pstrValor & "/") > 0)

Exit_EstaEn:
        Exit Function

Err_EstaEn:

    End Function

    Private Sub Reportar(ByVal pstrMensaje As String)
        On Error GoTo Err_Reportar

        gStringErrores.AppendLine(String.Format("{0} {1}", Now.ToLongTimeString, pstrMensaje))

Exit_Reportar:
        Exit Sub

Err_Reportar:

    End Sub

    Private Function FileExist(ByVal pstrFileName As String) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function FileExist(pstrFileName As String) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Evalua la existencia de un nombre de archivo.
        '/* Parámetros  :
        '/* Por         Nombre          Tipo            Descripción
        '/* Referencia  pstrFileName    string          Nombre del archivo.
        '/* Valores de retorno:

        '/* FileExist:  Devuelve TRUE si el archivo existe.
        '/*             Devuelve FALSE si el archivo NO existe.
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        If FileExist(pstrFileName) Then
            FileExist = True
        Else
            FileExist = False
        End If

    End Function

    Public Function EvaluarLineaColombia_MI(ByVal pNombreEncabezado As String, ByVal pEstrActual As Boolean, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal FirmaExtrangera As Boolean, _
                                            ByVal pstrUsuario As String) As String
        '     '/******************************************************************************************
        '     '/* INICIO DOCUMENTO
        '     '/* Sub EvaluarLineaColombia_MI()
        '     '/* Alcance     :   Private
        '     '/* Descripción :   Ejecuta los procesos que hacen que la importación de las operaciones de
        '     '/*                 bolsa de Colombia hacia la firma se lleve satisfactoriamente.  Ademas
        '     '/*                 presenta un formulario que imprimira un reporte de los errores y total de
        '     '/*                 de los registros leidos e importados del archivo.
        '     '/*                 A partir de una hilera de caracteres separa en variables los datos
        '     '/*                 de una Liquidación de la Bolsa de Colombia , con previo conocimiento
        '     '/*                 de las posiciones donde empiezan y donde terminan el valor de cada
        '     '/*                 variable. Y luego las graba en tblImportacionLiq.
        '     '/* Parámetros  :
        '     '/* Valores de retorno:
        '     '/* FIN DOCUMENTO
        '     '/******************************************************************************************
        'Dim sb As New StringBuilder
        Dim objReader As System.IO.StreamReader = Nothing
        Try
            Dim strdatos As String
            Dim intCual As Integer
            'Dim objReader As System.IO.StreamReader = Nothing

            Dim FILE_NAME As String = pNombreEncabezado
            'Abrimos el archivo de encabezados
            objReader = New System.IO.StreamReader(FILE_NAME)

            'SLB Verifica el formato del archivo, puede ser .dat o .txt
            Dim aText() As String, sExt As String
            aText = Split(FILE_NAME, ".")
            sExt = aText(UBound(aText))

            If objReader.Peek() = -1 Then
                Return "El archivo está vacio."
                objReader.Close()
                Exit Function
            ElseIf sExt <> "dat" And sExt <> "txt" Then
                Return "La extención del archivo no corresponde, debe tener extención .txt ó .dat ."
                objReader.Close()
                Exit Function
            End If

            If objReader.Peek() <> -1 Then
                Dim primeraLinea = objReader.ReadLine()
                If pEstrActual = True Then
                    If Len(primeraLinea) > 624 Then
                        objReader.Close()
                        Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                        Exit Function
                    End If
                Else 'ES ESTRUCTURA NUEVA
                    'If Len(primeraLinea) < 625 Then
                    If FirmaExtrangera Then
                        If Len(primeraLinea) <> 391 Then
                            objReader.Close()
                            Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                            Exit Function
                        End If
                    Else
                        If Len(primeraLinea) <> 406 Then 'SLB tamaño de las filas del archivo de las operaciones de colombianos en el extranjero
                            objReader.Close()
                            Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                            Exit Function
                        End If
                    End If

                End If
            Else
                'Archivo no tiene contenido
                Return "El archivo está vacio."
            End If

            objReader.Close()

            Reportar(MSGERROR_INICIO_IMPORTACION)
            Reportar(MSGERROR_ENCABEZADO)
            Reportar("Archivo:  " & pNombreEncabezado)

            'Abrimos el archivo de encabezados
            objReader = New System.IO.StreamReader(FILE_NAME)
            intCual = 1
            RegBuenos = 0
            'Recorremos el archivo de encabezados
            Do While objReader.Peek() <> -1
                strdatos = objReader.ReadLine()
                If Not strdatos = String.Empty Then
                    If FirmaExtrangera Then
                        If pEstrActual = True Then
                            ExtraerDatosColombiaNuevoFirmaExtrangera(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario)
                        Else
                            ExtraerDatosColombiaNuevoFirmaExtrangera(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario)
                        End If
                        intCual = intCual + 1
                    Else 'SLB extaer los datos de las operaciones de colombianos en el extranjero
                        If pEstrActual = True Then
                            ExtraerDatosColombiaNuevo(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario)
                        Else
                            ExtraerDatosColombiaNuevo(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario)
                        End If
                        intCual = intCual + 1
                    End If
                End If
            Loop

            objReader.Close()

            Reportar(MSGERROR_TOTAL_LEIDOS & CStr(intCual - 1))
            Reportar(MSGERROR_TOTAL_IMPORTADOS & CStr(RegBuenos))

            Return gStringErrores.ToString()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EvaluarLineaColombia")
            Return Nothing
        Finally
            objReader.Dispose()
        End Try
    End Function

    Private Function EsAccion(ByVal prstrEspecie As String, ByVal prlngIdBolsa As Long) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function EsAccion(ByVal prstrEspecie As String, prlngIdBolsa As Long) As Boolean
        '/* Alcance     :   Private
        '/* Descripción :   Consulta el Nemotecnico y el campo LogEsAccion de una especie para
        '/*                 para conocer si esta es una Accion o una Renta fija.
        '/* Parámetros  :
        '/* Por             Nombre          Tipo            Descripción.
        '/* Valor           prstrEspecie    string          Nemotecnico de la especie
        '/* Referencia      prlngIdBolsa    Long            Bolsa de valores donde se negocia la especie
        '/* Valores de retorno:
        '/* EsAccion: Devuelve TRUE si la especie consultada es una ACCION
        '/*           Devuelve FALSE si la especie consultada NO es una acción
        '/* FIN DOCUMENTO
        '/******************************************************************************************
        EsAccion = True
        'Try
        Dim ret = (L2SDC.uspOyDNet_Importaciones_VerificarEspecies(Trim(prstrEspecie), prlngIdBolsa)).ToList
        If ret.Count > 0 Then
            EsAccion = True
        Else
            EsAccion = False
        End If

    End Function

    Private Sub ExtraerDatosColombiaNuevo(ByVal pintCual As Integer, ByVal pstrDatos As String, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, _
                                          ByVal pstrUsuario As String)
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Sub ExtraerDatosColombiaNuevo(pintCual As Integer, pstrDatos As String)
        '/* Alcance     :   Private
        '/* Descripción :   A partir de una hilera de caracteres separa en variables los datos
        '/*                 de una Liquidación de las operaciones que realizan los colombianos en las Bolsas
        '/*                 Extrajenras, con previo conocimiento las posiciones donde empiezan y donde terminan el valor de cada
        '/*                 variable. Y luego las graba en tblImportacionLiq_MI.
        '/*                 Mayo 2012 Sebastian Londoño Benitez
        '/*                                                                     prefijo-Fecha-Nro Liquidacion
        '/* Parámetros  :
        '/* Por         Nombre          Tipo        Descripción
        '/* Referencia  pintCual        integer     Número de la Linea a ser evaluada en el archivo
        '/*                                         texto.
        '/* Referencia  pstrDato        string      Hilera de caracteres donde vienen datos de una
        '/*                                         liquidación
        '/* Valores de retorno:
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        Dim logEsAccion As Boolean

        Dim strObjeto As String = String.Empty
        Dim ExisteOrden As Boolean
        Dim lngOrden As Long
        Dim dblTotalFuturoRepo As Double
        Dim dblTotalRecompras As Double
        Dim dblTotalRecomprasImportadas As Double
        Dim dblTotalFuturoOrdenesRepos As Double

        Dim lngID As System.Nullable(Of Integer), _
             lngParcial As System.Nullable(Of Integer), _
             strTipo As String, _
             strClaseOrden As String, _
             strIDEspecie As String, _
             lngIDOrden As System.Nullable(Of Integer) = 0, _
             lngIDComitente As String, _
             lngIDOrdenante As String, _
             lngIDBolsa As System.Nullable(Of Integer), _
             bytIDRueda As System.Nullable(Of Short), _
             dblValBolsa As System.Nullable(Of Double), _
             dblTasaDescuento As Double = 0, _
             dblTasaCompraVende As System.Nullable(Of Double), _
             strModalidad As String, _
             strIndicador As String = "", _
             dblPuntos As Double = 0, _
             lngPlazo As System.Nullable(Of Integer), _
             dtmLiquidacion As System.Nullable(Of Date), _
             dtmCumplimiento As System.Nullable(Of Date), _
             dtmEmision As System.Nullable(Of Date), _
             dtmVencimiento As System.Nullable(Of Date), _
             logOtraPlaza As System.Nullable(Of Boolean), _
             strPlaza As String, _
             lngIDComisionistaLocal As System.Nullable(Of Integer), _
             lngIDComisionistaOtraPlaza As Integer = 0, _
             lngIDCiudadOtraPlaza As System.Nullable(Of Integer), _
             dblTasaEfectiva As Double = 0, _
             dblCantidad As System.Nullable(Of Double), _
             curPrecio As System.Nullable(Of Double), _
             curTransaccion As System.Nullable(Of Double), _
             curSubTotalLiq As System.Nullable(Of Double), _
             curTotalLiq As System.Nullable(Of Double), _
             curComision As System.Nullable(Of Double), _
             curRetencion As System.Nullable(Of Double), _
             curIntereses As System.Nullable(Of Double) = 0, _
             lngDiasIntereses As System.Nullable(Of Integer) = 0, _
             dblFactorComisionPactada As System.Nullable(Of Double), _
             strMercado As String, _
             strNroTitulo As String = String.Empty, _
             lngIDCiudadExpTitulo As System.Nullable(Of Integer) = 0, _
             lngPlazoOriginal As Integer = 0, _
             logAplazamiento As System.Nullable(Of Boolean) = False, _
             bytVersionPapeleta As System.Nullable(Of Short) = 0, _
             dtmEmisionOriginal As System.Nullable(Of Date) = Nothing, _
             dtmVencimientoOriginal As System.Nullable(Of Date) = Nothing, _
             lngImpresiones As System.Nullable(Of Integer), _
             strFormaPago As String = String.Empty, _
             lngCtrlImpPapeleta As System.Nullable(Of Integer) = 0, _
             lngDiasVencimiento As System.Nullable(Of Integer), _
             strPosicionPropia As String, _
             strTransaccion As String = String.Empty, _
             strTipoOperacion As String = String.Empty, _
             lngDiasContado As Integer = 0, _
             logOrdinaria As System.Nullable(Of Boolean) = False, _
             strObjetoOrdenExtraOrdinaria As String = String.Empty, _
             lngNumPadre As Integer = 0, _
             lngParcialPadre As Integer = 0, _
             dtmOperacionPadre As System.Nullable(Of Date), _
             lngDiasTramo As System.Nullable(Of Integer) = 0, _
             dblValorTraslado As System.Nullable(Of Double), _
             dblValorBrutoCompraVencida As System.Nullable(Of Double) = 0, _
             strAutoRetenedor As String = String.Empty, _
             strSujeto As String = String.Empty, _
             dblPcRenEfecCompraRet As System.Nullable(Of Double) = 0, _
             dblPcRenEfecVendeRet As System.Nullable(Of Double) = 0, _
             strReinversion As String = "", _
             strSwap As String, _
             lngNroSwap As Integer = 0, _
             strCertificacion As String = String.Empty, _
             dblDescuentoAcumula As System.Nullable(Of Double) = 0, _
             dblPctRendimiento As System.Nullable(Of Double) = 0, _
             dtmFechaCompraVencido As System.Nullable(Of Date) = Nothing, _
             dblPrecioCompraVencido As System.Nullable(Of Double) = 0, _
             strConstanciaEnajenacion As String = String.Empty, _
             strRepoTitulo As String, _
             strUsuario As String = pstrUsuario, _
             dblServBolsaVble As System.Nullable(Of Double), _
             dblServBolsaFijo As System.Nullable(Of Double), _
             strTraslado As String = String.Empty, _
             curValorIva As System.Nullable(Of Double), _
             strUBICACIONTITULO As String, _
             strTipoIdentificacion As System.Nullable(Of Char), _
             strNroDocumento As String, _
             dblValorEntregaContraPago As Double = 0, _
             strAquienSeEnviaRetencion As Char = "", _
             strBaseDias As Char = "", _
             strTipoDeOferta As System.Nullable(Of Char), _
             strHoraGrabacion As String, _
             strOrigenOperacion As String, _
             lngCodigoOperadorCompra As System.Nullable(Of Integer), _
             lngCodigoOperadorVende As System.Nullable(Of Integer), _
             strIdentificacionRemate As String, _
             strModalidaOperacion As System.Nullable(Of Char), _
             strIndicadorPrecio As System.Nullable(Of Char), _
             strPeriodoExdividendo As System.Nullable(Of Char), _
             lngPlazoOperacionRepo As System.Nullable(Of Integer), _
             dblValorCaptacionRepo As System.Nullable(Of Double), _
             lngVolumenCompraRepo As System.Nullable(Of Double), _
             dblPrecioNetoFraccion As System.Nullable(Of Double), _
             lngVolumenNetoFraccion As System.Nullable(Of Double), _
             strCodigoContactoComercial As String, _
             lngNroFraccionOperacion As System.Nullable(Of Integer), _
             strIdentificacionPatrimonio1 As String, _
             strTipoidentificacionCliente2 As System.Nullable(Of Char), _
             strNitCliente2 As String, _
             strIdentificacionPatrimonio2 As String, _
             strTipoIdentificacionCliente3 As System.Nullable(Of Char), _
             strNitCliente3 As String, _
             strIdentificacionPatrimonio3 As String, _
             strIndicadorOperacion As System.Nullable(Of Char), _
             dblBaseRetencion As System.Nullable(Of Double), _
             dblPorcRetencion As System.Nullable(Of Double), _
             dblBaseRetencionTranslado As System.Nullable(Of Double), _
             dblPorcRetencionTranslado As System.Nullable(Of Double), _
             dblPorcIvaComision As Double = 0, _
             strIndicadorAcciones As System.Nullable(Of Char), _
             strOperacionNegociada As System.Nullable(Of Char), _
             dtmFechaConstancia As System.Nullable(Of Date), _
             dblValorConstancia As System.Nullable(Of Double), _
             strGeneraConstancia As System.Nullable(Of Char), _
             strCodigoIntermediario As String = String.Empty, _
             strOperacionEnMercadoExtranjero As String

        'lngIDBolsa = cmbvBolsas.ItemData(cmbvBolsas.ListIndex)

        lngIDBolsa = 0

        strUBICACIONTITULO = UCase(Mid$(pstrDatos, 182, 3))
        Select Case strUBICACIONTITULO
            Case "DVL"
                strUBICACIONTITULO = STR_DECEVAL
            Case "DCV"
                strUBICACIONTITULO = STR_DCV
            Case Else
                strUBICACIONTITULO = STR_FISICO
        End Select

        'La fecha de liquidacion no cambia porque el nro de liquidacion inicia en la posicion 17
        dtmLiquidacion = DateSerial(CInt(Mid$(pstrDatos, 17, 4)), CInt(Mid$(pstrDatos, 21, 2)), CInt(Mid$(pstrDatos, 23, 2)))
        If Not IsDate(dtmLiquidacion) Then
            Call LogImportacion(pintCual, dtmLiquidacion, "La fecha de liquidación tiene un formato no válido")
            Exit Sub
        End If

        If ComparaFechas(dtmLiquidacion, pDateDesde, "<") Or _
            ComparaFechas(dtmLiquidacion, pDateHasta, ">") Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 17, 8), MSGERROR_RANGO_FECHAS)
            Exit Sub
        End If

        If Not IsNumeric(Mid$(pstrDatos, 40, 9)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 40, 9), "Número de operación incorrecto")
            Exit Sub
        End If

        lngID = Mid$(pstrDatos, 40, 9) 'Número de la liquidación

        If Trim(Mid$(pstrDatos, 25, 15)) = "" Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 25, 15), "Sin número de Operación en Mercado Extranjero")
            Exit Sub
        End If
        strOperacionEnMercadoExtranjero = Mid$(pstrDatos, 25, 15)

        If Not IsNumeric(Mid$(pstrDatos, 49, 3)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 49, 3), "Secuencia o número de fracción incorrecto")
        End If
        lngParcial = Mid$(pstrDatos, 49, 3)

        'Punto de la operación(7)
        strTipo = Mid$(pstrDatos, 52, 1)

        'Mercado(9)
        strClaseOrden = Mid$(pstrDatos, 54, 1)

        If strClaseOrden = "R" Then 'Or strClaseOrden = "D" Then (Con los TES viene en clase la D)
            strClaseOrden = "C" 'Liquidación de Renta Fija, C:Indica Crediticio
        Else
            strClaseOrden = "A" 'Liquidación de Acciones
        End If

        strIDEspecie = Trim(Mid$(pstrDatos, 55, 10)) 'Nemotécnico de la especie (10)

        If strTipo = "C" Then
            'Código del operador comprador(13)
            lngIDComisionistaLocal = Mid$(pstrDatos, 71, 3)
        Else
            'Código del operador vendedor(13)
            lngIDComisionistaLocal = Mid$(pstrDatos, 74, 3)
        End If

        'Cantidad o valor nominal(15)
        If Not IsNumeric(Mid$(pstrDatos, 77, 18)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 77, 18), "Cantidad de la operación incorrecta")
            Exit Sub
        End If

        If lngParcial = 0 Then
            dblCantidad = Mid$(pstrDatos, 77, 18) 'Cantidad o Valor Nominal(15)
            'curTransaccion = Mid$(pstrDatos, 108, 19)
        Else
            dblCantidad = Mid$(pstrDatos, 185, 15) 'Cantidad de la fraccion(30)
            'curTransaccion = Mid$(pstrDatos, 299, 19)
        End If

        curTransaccion = Mid$(pstrDatos, 200, 19)

        'Días al vencimiento (16)
        If Not IsNumeric(Mid$(pstrDatos, 95, 5)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 95, 5), "Días al vencimiento incorrecto")
            Exit Sub
        End If
        lngDiasVencimiento = Mid$(pstrDatos, 95, 5)

        'Precio unitario(17)
        curPrecio = Mid$(pstrDatos, 100, 13)

        Dim strEmision As String

        Dim strVencimiento As String

        'Tipo de oferta (23)
        strTipoDeOferta = IIf(Trim(Mid$(pstrDatos, 125, 1)) <> "", CType(Mid$(pstrDatos, 125, 1), System.Nullable(Of Char)), CChar("?"))
        'strtipoDeOferta  puede tener estos valores:
        'N: Oferta Normal
        'R: Repo
        'A: Repo en Acciones
        'P: OPAS
        'M: Martillo     Si Mercado ="A"
        'M: Simultanea  Si Mercado = "R"
        'O: Opciones  Si Mercado = "R"
        'D: Forward registro
        'F: Fondeo
        'S: Subasta
        'C: CARRUSEL
        'I: Interbancario
        'W: Swap
        'X: Otras
        '1: Simultáneas de salida
        '2: Simultáneas de regreso
        '3: TTV de salida
        '4: TTV de regreso

        Dim strCumplimiento As String

        strCumplimiento = Mid$(pstrDatos, 113, 8) 'Por el momento, tomar "Fecha de liquidación(18)"

        dtmCumplimiento = DateSerial(CInt(Mid$(strCumplimiento, 1, 4)), CInt(Mid$(strCumplimiento, 5, 2)), CInt(Mid$(strCumplimiento, 7, 2)))
        If strClaseOrden = "C" Then
            'strModalidad = Mid$(pstrDatos, 195, 1) & Mid$(pstrDatos, 196, 1)
            'strMercado = Mid$(pstrDatos, 216, 1)
            'If strModalidad = "PA" Then
            '    strModalidad = "UA"
            'Else
            '    If strModalidad = "PV" Or Trim(strModalidad) = "" Or IsNothing(strModalidad) Or strModalidad = "NO" Then
            '        strModalidad = "UV"
            '    End If
            'End If
        Else
            strModalidad = ""
            strMercado = ""
        End If

        lngPlazo = 0

        strPosicionPropia = Mid$(pstrDatos, 124, 1) 'Indicador de origen(22)

        strRepoTitulo = strTipoDeOferta
        If (strRepoTitulo = "R" Or strRepoTitulo = "A") Then
            If CDate(dtmLiquidacion) <> CDate(dtmCumplimiento) Then strTipo = IIf(strTipo = "C", "R", "S") 'Jairo Gonzalez 2007/06/20
            dblTasaEfectiva = Mid$(pstrDatos, 127, 16) 'Tasa para la operación repo(25)
        End If

        'strSwap = Mid$(pstrDatos, 217, 1)
        strSwap = ""

        'Estos valores no son actualizados
        strPlaza = "LOC"             '2001/05/25   Siempre seran locales con una unica Bolsa de valores
        logOtraPlaza = False         '2001/05/25   Siempre seran locales con una unica Bolsa de valores
        lngIDCiudadOtraPlaza = 585   '2001/05/25   Siempre seran locales con una unica Bolsa de valores
        bytIDRueda = 1               '2001/05/25   Siempre seran locales con una unica Bolsa de valores

        dblFactorComisionPactada = Mid$(pstrDatos, 219, 8) '% de la comisión (32)

        'Valor de la comisión (33)
        curComision = Mid$(pstrDatos, 227, 13)

        If dblPorcentajeIvaComision = -1 Then
            dblPorcentajeIvaComision = CampoTabla("1", "dblIvaComision", "tblinstalacion", "1")
            dblPorcentajeIvaComision = dblPorcentajeIvaComision / 100
        End If

        If IsNumeric(curComision) And curComision > 0 Then
            curValorIva = Format((dblPorcentajeIvaComision * curComision) - 0.005, "#,#.00")
        Else
            curValorIva = 0
        End If

        If Not IsNumeric(Mid$(pstrDatos, 247, 16)) Then
            dblTasaCompraVende = 0
        Else
            dblTasaCompraVende = Mid$(pstrDatos, 247, 16) 'Tasa neta de la fracción(35)
        End If

        'Número de veces de impresión(37)
        If Not IsNumeric(Mid$(pstrDatos, 282, 2)) Then
            lngImpresiones = 0
        Else
            lngImpresiones = Mid$(pstrDatos, 282, 2)
        End If

        If Not IsNumeric(Mid$(pstrDatos, 284, 12)) Then
            dblValBolsa = 0
        Else
            dblValBolsa = Mid$(pstrDatos, 284, 12) 'Servicio en bolsa(Cargo por volumen),Fijo(38)
        End If

        curRetencion = 0
        dblValorTraslado = 0

        If Not IsNumeric(Mid$(pstrDatos, 296, 12)) Then
            dblServBolsaVble = 0
        Else
            dblServBolsaVble = CType(Mid$(pstrDatos, 296, 12), System.Nullable(Of Double)) 'Servicio en bolsa (Cargo por volumen), VARIABLE(39)
        End If

        strIndicadorAcciones = "S"

        If strTipo = "C" Or strTipo = "R" Then
            curSubTotalLiq = curTransaccion + curComision + curValorIva
        Else
            curSubTotalLiq = curTransaccion - curComision - curValorIva
        End If
        If strTipo = "C" Or strTipo = "R" Or strMercado = "P" Then
            curTotalLiq = curSubTotalLiq + curRetencion + dblValorTraslado '+ IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
        Else
            curTotalLiq = curSubTotalLiq + curRetencion + dblValorTraslado '- IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
        End If

        'Tipo de identificación del cliente 1(43)
        strTipoIdentificacion = Mid$(pstrDatos, 327, 1)

        'Identificacion del cliente 1(44)
        strNroDocumento = Trim(Mid$(pstrDatos, 328, 12))

        strNroDocumento = ReemplazarcaracterEnCadena(strNroDocumento)

        If Not VerificarSiEspecieExiste_MI(lngIDBolsa, strIDEspecie, logEsAccion) Then
            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_ESPECIE & strIDEspecie & vbTab & MSGERROR_TABLA_ESPECIES)
            Exit Sub
        End If

        If Not logEsAccion Then
            strVencimiento = dtmVencimiento
        Else
            strVencimiento = ""
        End If

        If ExisteLiquidacion_MI(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
            'Existe Exactamente la liquidación - con strTipo = " "
            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_LIQUIDACION & lngID & MSGERROR_TABLA_LIQUIDACIONES)
            'Valido los campos que vienen contra los que existen
            Call Validar_Campo_a_Campo(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa)

            If IsNothing(rstConsultarDatosLiquidacion) Or rstConsultarDatosLiquidacion.Count = 0 Then
                Exit Sub
            End If

            With rstConsultarDatosLiquidacion(0)
                'If .dtmVencimiento <> dtmVencimiento Then
                '    Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
                'End If
                If .dblServBolsaVble <> dblServBolsaVble Then
                    Call LogDiferencias(.dblServBolsaVble, "Servicio Bolsa - Vlr en el Plano : " & dblServBolsaVble)
                End If
                If .curTotalLiq <> curTotalLiq Then
                    Call LogDiferencias(.curTotalLiq, "Total operación - Vlr en el Plano : " & curTotalLiq)
                End If
                If .dblValorTraslado <> dblValorTraslado Then
                    Call LogDiferencias(.dblValorTraslado, "Vlr Traslado - Vlr en el Plano : " & dblValorTraslado)
                End If
                If .curRetencion <> curRetencion Then
                    Call LogDiferencias(.curRetencion, "Retención - Vlr en el Plano : " & curRetencion)
                End If
                If .dblValBolsa <> dblValBolsa Then
                    Call LogDiferencias(.dblValBolsa, "Servicio Bolsa - Vlr en el Plano : " & dblValBolsa)
                End If
                If .dblTasaCompraVende <> dblTasaCompraVende Then
                    Call LogDiferencias(.dblTasaCompraVende, "Tasa Neta Fracción - Vlr en el Plano : " & dblTasaCompraVende)
                End If
                If .curValorIva <> curValorIva Then
                    Call LogDiferencias(.curValorIva, "IVA Comisión - Vlr en el Plano : " & curValorIva)
                End If
                If .curComision <> curComision Then
                    Call LogDiferencias(.curComision, "Comisión - Vlr en el Plano : " & curComision)
                End If
                If .dblFactorComisionPactada <> dblFactorComisionPactada Then
                    Call LogDiferencias(.strModalidad, "Modalidad - Vlr en el Plano : " & strModalidad)
                End If
                If (.strModalidad <> strModalidad) And logEsAccion = False Then
                    Call LogDiferencias(.strModalidad, "Modalidad - Vlr en el Plano : " & strModalidad)
                End If
                If .dtmCumplimiento <> dtmCumplimiento Then
                    Call LogDiferencias(.dtmCumplimiento, "Fecha Cumplimiento - Vlr en el Plano : " & dtmCumplimiento)
                End If
                If .strTipoDeOferta <> strTipoDeOferta Then
                    Call LogDiferencias(.strTipoDeOferta, "Tipo Oferta - Vlr en el Plano : " & strTipoDeOferta)
                End If
                If .dblTasaEfectiva <> dblTasaEfectiva Then
                    Call LogDiferencias(.dblTasaEfectiva, "Tasa Efectiva - Vlr en el Plano : " & dblTasaEfectiva)
                End If
                If .dblPuntosIndicador <> dblPuntos Then
                    Call LogDiferencias(.dblPuntosIndicador, "Puntos sobre el Indicador - Vlr en el Plano : " & dblPuntos)
                End If
                If .strIndicadorEconomico <> strIndicador Then
                    Call LogDiferencias(.strIndicadorEconomico, "Indicador Economico - Vlr en el Plano : " & strIndicador)
                End If
                If .lngPlazoOriginal <> lngPlazoOriginal Then
                    Call LogDiferencias(.lngPlazoOriginal, "Plazo - Vlr en el Plano : " & lngPlazoOriginal)
                End If
                If .dtmVencimiento <> dtmVencimiento Then
                    Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
                End If
                'If .dtmEmision <> dtmEmision Then
                '    Call LogDiferencias(.dtmEmision, "Emisión - Vlr en el Plano : " & dtmEmision)
                'End If
                If .curPrecio <> curPrecio Then
                    Call LogDiferencias(.curPrecio, "Precio - Vlr en el Plano : " & curPrecio)
                End If
                If .lngDiasVencimiento <> lngDiasVencimiento Then
                    Call LogDiferencias(.lngDiasVencimiento, "Dias al Vencimiento - Vlr en el Plano : " & lngDiasVencimiento)
                End If
                If .curTransaccion <> curTransaccion Then
                    Call LogDiferencias(.curTransaccion, "Transacción - Vlr en el Plano : " & curTransaccion)
                End If
                If .dblCantidad <> dblCantidad Then
                    Call LogDiferencias(.dblCantidad, "Cantidad - Vlr en el Plano : " & dblCantidad)
                End If
                If .dblTasaDescuento <> dblTasaDescuento Then
                    Call LogDiferencias(.dblTasaDescuento, "Tasa Nominal - Vlr en el Plano : " & dblTasaDescuento)
                End If

            End With

            Exit Sub
        Else
            If ExisteLiquidacion_MI(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
                If strTipo = "C" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_COMPRA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                ElseIf strTipo = "V" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_VENTA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                ElseIf strTipo = "R" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_RECOMPRA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                ElseIf strTipo = "S" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_REVENTA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                End If
                Exit Sub
            End If
        End If

        lngOrden = 0
        lngIDComitente = "0"
        lngIDOrdenante = "0"

        Dim strOrden As String

        Dim strDepositoOrden As String = String.Empty 'Jorge Arango 2009/11/03 Ubicacion del titulo de la orden
        Dim strEstadoAprobOrden As String = String.Empty
        Dim strEstadoAprobCliente As String = String.Empty
        Dim strEstadoOrden As String = String.Empty

        'strOrden = "" 'El archivo plano no especifica número de orden 

        '15 de Octubre/2019 Jorge Peña - Se agrega funcionalidad ya que no estaba.
        If Not IsNothing(Mid$(pstrDatos, 319, 8)) And Mid$(pstrDatos, 319, 8).Trim <> "" Then

            strOrden = Now.Year.ToString & Format$(Val(Mid$(pstrDatos, 319, 8)), "000000")  ' AFG

            If IsNumeric(strOrden) And Len(Trim(strOrden)) = 10 Then
                lngOrden = CLng(strOrden)
                If strTipoDeOferta = "2" Or strTipoDeOferta = "4" Or strTipoDeOferta = "6" Or
                       ((strTipoDeOferta = "R" Or strTipoDeOferta = "A") And (strTipo = "R" Or strTipo = "S")) Then
                    lngOrden = lngOrden + 1     'Cuando hay complementacion de Simultaneas en BVC, solo se puede realizar
                    'en las operaciones de salida(de hoy para hoy) y el stma coloca el mismo
                    'numero de Orden de la operacion de salida a la operacion de regreso y por ello
                    'aca se les suma uno (ya que OyD las ingresa consecutivamente)
                    strOrden = CStr(lngOrden)
                End If

                ExisteOrden = VerificarOrden(strOrden, strTipo, strClaseOrden, CStr(strIDEspecie), strNroDocumento, lngIDComitente, lngIDOrdenante, logOrdinaria, strObjeto, strDepositoOrden, strEstadoAprobOrden, strEstadoAprobCliente, strEstadoOrden)

                If Not ExisteOrden Then
                    lngOrden = 0
                Else 'Si existe la orden pero se debe validar que si tenga el mismo deposito de la liquidacion Jorge Arango 2009/11/03
                    If strDepositoOrden <> strUBICACIONTITULO Then
                        Call LogImportacion(pintCual, strUBICACIONTITULO, "Los depósitos de la orden #" & strOrden & " y la liquidación no concuerdan, por favor verifique. " & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                        Exit Sub
                    End If
                    If strEstadoOrden = "A" Then 'RBP20160719_Si la orden esta anulda no se debe dejar subir el archivo
                        Call LogImportacion(pintCual, "Estado de la Orden " & lngOrden, ", está anulada" & strEstadoOrden)
                        Exit Sub
                    End If
                    If Not IsNothing(strEstadoAprobCliente) Then
                        If (strEstadoAprobOrden = ESTADO_PENDIENTE_POR_APROBACION) Then
                            Call LogImportacion(pintCual, Now.Year & lngOrden, "La Orden no ha sido aprobada")
                            Exit Sub
                        End If

                        If Mid(strEstadoAprobCliente, 1, 2) = ESTADO_7_RETIRADO_PENDIENTE_POR_APROBACION Or Mid(strEstadoAprobCliente, 1, 2) = ESTADO_8_APROBADO_LUEGO_DE_RETIRO Or
                               Mid(strEstadoAprobCliente, 1, 2) = ESTADO_10_ACTIVADO_PENDIENTE_POR_APROBACION Or Mid(strEstadoAprobCliente, 1, 2) = ESTADO_12_RECHAZADO_LUEGO_DE_ACTIVADO Or
                               strEstadoAprobCliente.ToLower() = ESTADO_PENDIENTE_POR_APROBACION_CLIENTE Then
                            Call LogImportacion(pintCual, strNroDocumento, "El cliente está inactivo o está pendiente la aprobación de su inactivacion")
                            lngOrden = 0
                            Exit Sub
                        End If
                    End If

                End If
            End If
        Else
            lngOrden = 0
        End If
        '15 de Octubre/2019 Jorge Peña - Se agrega funcionalidad ya que no estaba.


        strHoraGrabacion = Trim(Mid$(pstrDatos, 9, 8)) 'Hora de grabación (2)
        strOrigenOperacion = Mid$(pstrDatos, 53, 1)    'Tipo de operación (8)
        lngCodigoOperadorCompra = Mid$(pstrDatos, 71, 3) 'Código del operador comprador (13)
        lngCodigoOperadorVende = Mid$(pstrDatos, 74, 3)  'Código del operador vendedor (14)
        strIdentificacionRemate = ""
        strModalidaOperacion = Mid$(pstrDatos, 121, 1) 'Modalidad de liquidación (19)
        strIndicadorPrecio = Mid$(pstrDatos, 122, 1)      'Indica si fijo precio en acciones (20)
        strPeriodoExdividendo = Mid$(pstrDatos, 123, 1)   'Indica si es exdividendo (21)
        lngPlazoOperacionRepo = Mid$(pstrDatos, 143, 5)   'Plazo de la operación repo (26)
        dblValorCaptacionRepo = IIf(Mid$(pstrDatos, 148, 15) = "", 0, CDbl(Mid$(pstrDatos, 148, 15))) 'Valor de captación del repo (27)
        lngVolumenCompraRepo = IIf(Mid$(pstrDatos, 163, 19) = "", 0, CDbl(Mid$(pstrDatos, 163, 19))) 'Volumen de recompra del repo (28)
        dblPrecioNetoFraccion = IIf(Mid$(pstrDatos, 240, 7) = "", 0, CDbl(Mid$(pstrDatos, 240, 7)))  'Precio neto de la fracción (34)
        lngVolumenNetoFraccion = IIf(Mid$(pstrDatos, 263, 19) = "", 0, CDbl(Mid$(pstrDatos, 263, 19))) 'Volumen neto de la fracción (36)
        strCodigoContactoComercial = Mid$(pstrDatos, 308, 8) 'Código del trader (40)
        lngNroFraccionOperacion = Mid$(pstrDatos, 316, 3)      'Número de fracciones (41)
        strIdentificacionPatrimonio1 = Mid$(pstrDatos, 340, 3)   'Identificación patrimonio autónomo 1 (45)
        strTipoidentificacionCliente2 = Mid$(pstrDatos, 343, 1)  'Tipo de identificación del cliente 2 (46)
        strNitCliente2 = Mid$(pstrDatos, 344, 12)               'Identificación del cliente 2 (47)
        strIdentificacionPatrimonio2 = Mid$(pstrDatos, 356, 3)  'Identificación patrimonio autónomo 2 (48)
        strTipoIdentificacionCliente3 = Mid$(pstrDatos, 359, 1) 'Tipo de identificación del cliente 3 (49)
        strNitCliente3 = Mid$(pstrDatos, 360, 12)               'Identificación del cliente 3 (50)
        strIdentificacionPatrimonio3 = Mid$(pstrDatos, 372, 3)  'Identificación patrimonio autónomo 3 (51)
        strIndicadorOperacion = Mid$(pstrDatos, 375, 1)         'Indicador de la operación (52)
        dblBaseRetencion = 0 '
        dblPorcRetencion = 0 '
        dblBaseRetencionTranslado = 0  '
        dblPorcRetencionTranslado = 0 '
        strOperacionNegociada = "" '
        Dim dtmFechaConstancia1 As String
        dtmFechaConstancia1 = "" '
        'dtmFechaConstancia = ""
        dtmFechaConstancia = Nothing
        dblValorConstancia = 0 '
        strGeneraConstancia = "" '
        ''''lngImpresiones = 0 '
        strCodigoIntermediario = Trim(Mid$(pstrDatos, 398, 9)) 'Código Intermediario extranjero (57)

        'SLB se grabar en la tabla tblimportacionliq_mi
        Dim ret = L2SDC.sp_GrabarLiquidacion_MI(lngID, _
                                        lngParcial, _
                                        strTipo, _
                                        strClaseOrden, _
                                        strIDEspecie, _
                                        lngOrden, _
                                        lngIDComitente, _
                                        lngIDOrdenante, _
                                        lngIDBolsa, _
                                        bytIDRueda, _
                                        dblValBolsa, _
                                        dblTasaDescuento, _
                                        dblTasaCompraVende, _
                                        strModalidad, _
                                        strIndicador, _
                                        dblPuntos, _
                                        lngPlazo, _
                                        dtmLiquidacion, _
                                        dtmCumplimiento, _
                                        dtmEmision, _
                                        dtmVencimiento, _
                                        logOtraPlaza, _
                                        strPlaza, _
                                        lngIDComisionistaLocal, _
                                        lngIDComisionistaOtraPlaza, _
                                        lngIDCiudadOtraPlaza, _
                                        dblTasaEfectiva, _
                                        dblCantidad, _
                                        curPrecio, _
                                        curTransaccion, _
                                        curSubTotalLiq, _
                                        curTotalLiq, _
                                        curComision, _
                                        curRetencion, _
                                        0, _
                                        0, _
                                        dblFactorComisionPactada, _
                                        strMercado, _
                                        strIDEspecie, _
                                        0, _
                                        lngPlazoOriginal, _
                                        0, _
                                        0, _
                                        dtmEmisionOriginal, _
                                        dtmVencimientoOriginal, _
                                        lngImpresiones, _
                                        String.Empty, _
                                        0, _
                                        lngDiasVencimiento, _
                                        strPosicionPropia, _
                                        String.Empty, _
                                        String.Empty, _
                                        lngDiasContado, _
                                        logOrdinaria, _
                                        IIf(strObjetoOrdenExtraOrdinaria <> "", strObjetoOrdenExtraOrdinaria, strObjeto), _
                                        lngNumPadre, _
                                        lngParcialPadre, _
                                        dtmOperacionPadre, _
                                        0, _
                                        dblValorTraslado, _
                                        0, _
                                        String.Empty, _
                                        String.Empty, _
                                        0, _
                                        0, _
                                        strReinversion, _
                                        strSwap, _
                                        lngNroSwap, _
                                        String.Empty, _
                                        0, _
                                        0, _
                                        dtmFechaCompraVencido, _
                                        0, _
                                        String.Empty, _
                                        String.Empty, _
                                        strUsuario, _
                                        dblServBolsaVble, _
                                        0, _
                                        String.Empty, _
                                        curValorIva, _
                                        strUBICACIONTITULO, _
                                        strTipoIdentificacion, _
                                        strNroDocumento, _
                                        dblValorEntregaContraPago, _
                                        strAquienSeEnviaRetencion, _
                                        strBaseDias, _
                                        strTipoDeOferta, _
                                        strHoraGrabacion, _
                                        strOrigenOperacion, _
                                        lngCodigoOperadorCompra, _
                                        lngCodigoOperadorVende, _
                                        strIdentificacionRemate, _
                                        strModalidaOperacion, _
                                        strIndicadorPrecio, _
                                        strPeriodoExdividendo, _
                                        lngPlazoOperacionRepo, _
                                        dblValorCaptacionRepo, _
                                        lngVolumenCompraRepo, _
                                        dblPrecioNetoFraccion, _
                                        lngVolumenNetoFraccion, _
                                        strCodigoContactoComercial, _
                                        lngNroFraccionOperacion, _
                                        strIdentificacionPatrimonio1, _
                                        strTipoidentificacionCliente2, _
                                        strNitCliente2, _
                                        strIdentificacionPatrimonio2, _
                                        strTipoIdentificacionCliente3, _
                                        strNitCliente3, _
                                        strIdentificacionPatrimonio3, _
                                        strIndicadorOperacion, _
                                        dblBaseRetencion, _
                                        dblPorcRetencion, _
                                        dblBaseRetencionTranslado, _
                                        dblPorcRetencionTranslado, _
                                        dblPorcIvaComision, _
                                        strIndicadorAcciones, _
                                        strOperacionNegociada, _
                                        dtmFechaConstancia, _
                                        dblValorConstancia, _
                                        strGeneraConstancia, _
                                        strCodigoIntermediario, _
                                        strOperacionEnMercadoExtranjero, _
                                        DemeInfoSesion(pstrUsuario, "ExtraerDatosColombiaNuevo"), _
                                        ClsConstantes.GINT_ErrorPersonalizado)


        'Dim ret = L2SDC.sp_GrabarLiquidacion_MI(lngID, _
        '                                        lngParcial, _
        '                                        strTipo, _
        '                                        strClaseOrden, _
        '                                        strIDEspecie, _
        '                                        lngOrden, _
        '                                        lngIDComitente, _
        '                                        lngIDOrdenante, _
        '                                        lngIDBolsa, _
        '                                        bytIDRueda, _
        '                                        dblValBolsa, _
        '                                        dblTasaDescuento, _
        '                                        dblTasaCompraVende, _
        '                                        strModalidad, _
        '                                        strIndicador, _
        '                                        dblPuntos, _
        '                                        lngPlazo, _
        '                                        dtmLiquidacion, _
        '                                        dtmCumplimiento, _
        '                                        dtmEmision, _
        '                                        dtmVencimiento, _
        '                                        logOtraPlaza, _
        '                                        strPlaza, _
        '                                        lngIDComisionistaLocal, _
        '                                        lngIDComisionistaOtraPlaza, _
        '                                        lngIDCiudadOtraPlaza, _
        '                                        dblTasaEfectiva, _
        '                                        dblCantidad, _
        '                                        curPrecio, _
        '                                        curTransaccion, _
        '                                        curSubTotalLiq, _
        '                                        curTotalLiq, _
        '                                        curComision, _
        '                                        curRetencion, _
        '                                        curIntereses, _
        '                                        lngDiasIntereses, _
        '                                        dblFactorComisionPactada, _
        '                                        strMercado, _
        '                                        strNroTitulo, _
        '                                        lngIDCiudadExpTitulo, _
        '                                        lngPlazoOriginal, _
        '                                        logAplazamiento, _
        '                                        bytVersionPapeleta, _
        '                                        dtmEmisionOriginal, _
        '                                        dtmVencimientoOriginal, _
        '                                        lngImpresiones, _
        '                                        strFormaPago, _
        '                                        lngCtrlImpPapeleta, _
        '                                        lngDiasVencimiento, _
        '                                        strPosicionPropia, _
        '                                        strTransaccion, _
        '                                        strTipoOperacion, _
        '                                        lngDiasContado, _
        '                                        logOrdinaria, _
        '                                        strObjetoOrdenExtraOrdinaria, _
        '                                        lngNumPadre, _
        '                                        lngParcialPadre, _
        '                                        dtmOperacionPadre, _
        '                                        lngDiasTramo, _
        '                                        dblValorTraslado, _
        '                                        dblValorBrutoCompraVencida, _
        '                                        strAutoRetenedor, _
        '                                        strSujeto, _
        '                                        dblPcRenEfecCompraRet, _
        '                                        dblPcRenEfecVendeRet, _
        '                                        strReinversion, _
        '                                        strSwap, _
        '                                        lngNroSwap, _
        '                                        strCertificacion, _
        '                                        dblDescuentoAcumula, _
        '                                        dblPctRendimiento, _
        '                                        dtmFechaCompraVencido, _
        '                                        dblPrecioCompraVencido, _
        '                                        strConstanciaEnajenacion, _
        '                                        strRepoTitulo, _
        '                                        strUsuario, _
        '                                        dblServBolsaVble, _
        '                                        dblServBolsaFijo, _
        '                                        strTraslado, _
        '                                        curValorIva, _
        '                                        strUBICACIONTITULO, _
        '                                        strTipoIdentificacion, _
        '                                        strNroDocumento, _
        '                                        dblValorEntregaContraPago, _
        '                                        strAquienSeEnviaRetencion, _
        '                                        strBaseDias, _
        '                                        strTipoDeOferta, _
        '                                        strHoraGrabacion, _
        '                                        strOrigenOperacion, _
        '                                        lngCodigoOperadorCompra, _
        '                                        lngCodigoOperadorVende, _
        '                                        strIdentificacionRemate, _
        '                                        strModalidaOperacion, _
        '                                        strIndicadorPrecio, _
        '                                        strPeriodoExdividendo, _
        '                                        lngPlazoOperacionRepo, _
        '                                        dblValorCaptacionRepo, _
        '                                        lngVolumenCompraRepo, _
        '                                        dblPrecioNetoFraccion, _
        '                                        lngVolumenNetoFraccion, _
        '                                        strCodigoContactoComercial, _
        '                                        lngNroFraccionOperacion, _
        '                                        strIdentificacionPatrimonio1, _
        '                                        strTipoidentificacionCliente2, _
        '                                        strNitCliente2, _
        '                                        strIdentificacionPatrimonio2, _
        '                                        strTipoIdentificacionCliente3, _
        '                                        strNitCliente3, _
        '                                        strIdentificacionPatrimonio3, _
        '                                        strIndicadorOperacion, _
        '                                        dblBaseRetencion, _
        '                                        dblPorcRetencion, _
        '                                        dblBaseRetencionTranslado, _
        '                                        dblPorcRetencionTranslado, _
        '                                        dblPorcIvaComision, _
        '                                        strIndicadorAcciones, _
        '                                        strOperacionNegociada, _
        '                                        dtmFechaConstancia, _
        '                                        dblValorConstancia, _
        '                                        strGeneraConstancia, _
        '                                        0, _
        '                                        strOperacionEnMercadoExtranjero, _
        '                                        DemeInfoSesion(pstrUsuario, "ExtraerDatosColombiaNuevo"), _
        '                                        ClsConstantes.GINT_ErrorPersonalizado)

        RegBuenos = RegBuenos + 1

    End Sub

    Private Sub ExtraerDatosColombiaNuevoFirmaExtrangera(ByVal pintCual As Integer, ByVal pstrDatos As String, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal pstrUsuario As String)
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Sub ExtraerDatosColombiaNuevoFirmaExtrangera(pintCual As Integer, pstrDatos As String)
        '/* Alcance     :   Private
        '/* Descripción :   A partir de una hilera de caracteres separa en variables los datos
        '/*                 de una Liquidación de la Bolsa de Colombia, con previo conocimiento
        '/*                 de las posiciones donde empiezan y donde terminan el valor de cada
        '/*                 variable. Y luego las graba en tblImportacionLiq.
        '/*                 Mayo 8/2008 Luis Fernando Alvarez se actualiza dicho procedimiento por cambio en BVC para nro Liq de 9 digitos
        '/*                 En general el nro liquidacion queda de la siguiente manera xx-xxxxxxxx-xxxxxxxxx
        '/*                                                                     prefijo-Fecha-Nro Liquidacion
        '/* FIN DOCUMENTO
        '/******************************************************************************************

        Dim logEsAccion As Boolean

        Dim strObjeto As String = String.Empty
        Dim ExisteOrden As Boolean
        Dim lngOrden As Long
        'Dim dblTotalFuturoRepo As Double
        'Dim dblTotalRecompras As Double
        'Dim dblTotalRecomprasImportadas As Double
        'Dim dblTotalFuturoOrdenesRepos As Double

        Dim intEncabezado As System.Nullable(Of Integer),
            strIndicadorEconomico As String,
            dblPuntosIndicador As System.Nullable(Of Double),
            logVendido As Boolean,
            dtmVendido As String,
            dtmActualizacion As String,
            dblIvaComision As System.Nullable(Of Double),
            strIDBaseDias As String,
             lngID As System.Nullable(Of Integer),
             lngParcial As System.Nullable(Of Integer),
             strTipo As String,
             strClaseOrden As String,
             strIDEspecie As String,
             lngIDOrden As System.Nullable(Of Integer) = 0,
             lngIDComitente As String,
             lngIDOrdenante As String,
             lngIDBolsa As System.Nullable(Of Integer),
             bytIDRueda As System.Nullable(Of Short),
             dblValBolsa As System.Nullable(Of Double),
             dblTasaDescuento As Double = 0,
             dblTasaCompraVende As System.Nullable(Of Double),
             strModalidad As String,
             strIndicador As String = "",
             dblPuntos As Double = 0,
             lngPlazo As System.Nullable(Of Integer),
             dtmLiquidacion As System.Nullable(Of Date),
             dtmCumplimiento As System.Nullable(Of Date),
             dtmEmision As System.Nullable(Of Date),
             dtmVencimiento As System.Nullable(Of Date),
             logOtraPlaza As System.Nullable(Of Boolean),
             strPlaza As String,
             lngIDComisionistaLocal As System.Nullable(Of Integer),
             lngIDComisionistaOtraPlaza As Integer = 0,
             lngIDCiudadOtraPlaza As System.Nullable(Of Integer),
             dblTasaEfectiva As Double = 0,
             dblCantidad As System.Nullable(Of Double),
             curPrecio As System.Nullable(Of Double),
             curTransaccion As System.Nullable(Of Double),
             curSubTotalLiq As System.Nullable(Of Double),
             curTotalLiq As System.Nullable(Of Double),
             curComision As System.Nullable(Of Double),
             curRetencion As System.Nullable(Of Double),
             curIntereses As System.Nullable(Of Double) = 0,
             lngDiasIntereses As System.Nullable(Of Integer) = 0,
             dblFactorComisionPactada As System.Nullable(Of Double),
             strMercado As String,
             strNroTitulo As String = String.Empty,
             lngIDCiudadExpTitulo As System.Nullable(Of Integer) = 0,
             lngPlazoOriginal As Integer = 0,
             logAplazamiento As System.Nullable(Of Boolean) = False,
             bytVersionPapeleta As System.Nullable(Of Short) = 0,
             dtmEmisionOriginal As System.Nullable(Of Date) = Nothing,
             dtmVencimientoOriginal As System.Nullable(Of Date) = Nothing,
             lngImpresiones As System.Nullable(Of Integer),
             strFormaPago As String = String.Empty,
             lngCtrlImpPapeleta As System.Nullable(Of Integer) = 0,
             lngDiasVencimiento As System.Nullable(Of Integer),
             strPosicionPropia As String,
             strTransaccion As String = String.Empty,
             strTipoOperacion As String = String.Empty,
             lngDiasContado As Integer = 0,
             logOrdinaria As System.Nullable(Of Boolean) = True,
             strObjetoOrdenExtraOrdinaria As String = String.Empty,
             lngNumPadre As Integer = 0,
             lngParcialPadre As Integer = 0,
             dtmOperacionPadre As System.Nullable(Of Date),
             lngDiasTramo As System.Nullable(Of Integer) = 0,
             dblValorTraslado As System.Nullable(Of Double),
             dblValorBrutoCompraVencida As System.Nullable(Of Double) = 0,
             strAutoRetenedor As String = String.Empty,
             strSujeto As String = String.Empty,
             dblPcRenEfecCompraRet As System.Nullable(Of Double) = 0,
             dblPcRenEfecVendeRet As System.Nullable(Of Double) = 0,
             strReinversion As String = "",
             strSwap As String,
             lngNroSwap As Integer = 0,
             strCertificacion As String = String.Empty,
             dblDescuentoAcumula As System.Nullable(Of Double) = 0,
             dblPctRendimiento As System.Nullable(Of Double) = 0,
             dtmFechaCompraVencido As System.Nullable(Of Date) = Nothing,
             dblPrecioCompraVencido As System.Nullable(Of Double) = 0,
             strConstanciaEnajenacion As String = String.Empty,
             strRepoTitulo As String,
             strUsuario As String = String.Empty,
             dblServBolsaVble As System.Nullable(Of Double),
             dblServBolsaFijo As System.Nullable(Of Double),
             strTraslado As String = String.Empty,
             curValorIva As System.Nullable(Of Double),
             strUBICACIONTITULO As String,
             strTipoIdentificacion As System.Nullable(Of Char),
             strNroDocumento As String,
             dblValorEntregaContraPago As Double = 0,
             strAquienSeEnviaRetencion As Char = "",
             strBaseDias As Char = "",
             strTipoDeOferta As System.Nullable(Of Char),
             strHoraGrabacion As String,
             strOrigenOperacion As String,
             lngCodigoOperadorCompra As System.Nullable(Of Integer),
             lngCodigoOperadorVende As System.Nullable(Of Integer),
             strIdentificacionRemate As String,
             strModalidaOperacion As System.Nullable(Of Char),
             strIndicadorPrecio As System.Nullable(Of Char),
             strPeriodoExdividendo As System.Nullable(Of Char),
             lngPlazoOperacionRepo As System.Nullable(Of Integer),
             dblValorCaptacionRepo As System.Nullable(Of Double),
             lngVolumenCompraRepo As System.Nullable(Of Double),
             dblPrecioNetoFraccion As System.Nullable(Of Double),
             lngVolumenNetoFraccion As System.Nullable(Of Double),
             strCodigoContactoComercial As String,
             lngNroFraccionOperacion As System.Nullable(Of Integer),
             strIdentificacionPatrimonio1 As String,
             strTipoidentificacionCliente2 As System.Nullable(Of Char),
             strNitCliente2 As String,
             strIdentificacionPatrimonio2 As String,
             strTipoIdentificacionCliente3 As System.Nullable(Of Char),
             strNitCliente3 As String,
             strIdentificacionPatrimonio3 As String,
             strIndicadorOperacion As System.Nullable(Of Char),
             dblBaseRetencion As System.Nullable(Of Double),
             dblPorcRetencion As System.Nullable(Of Double),
             dblBaseRetencionTranslado As System.Nullable(Of Double),
             dblPorcRetencionTranslado As System.Nullable(Of Double),
             dblPorcIvaComision As Double = 0,
             strIndicadorAcciones As System.Nullable(Of Char),
             strOperacionNegociada As System.Nullable(Of Char),
             dtmFechaConstancia As System.Nullable(Of Date),
             dblValorConstancia As System.Nullable(Of Double),
             strGeneraConstancia As System.Nullable(Of Char),
             strCodigoIntermediario As String = String.Empty,
             strOperacionEnMercadoExtranjero As String

        'lngIDBolsa = cmbvBolsas.ItemData(cmbvBolsas.ListIndex)

        lngIDBolsa = 4
        'gbtyIDBOLSA = 4

        strUBICACIONTITULO = UCase(Mid$(pstrDatos, 167, 3))
        Select Case strUBICACIONTITULO
            Case "DVL"
                strUBICACIONTITULO = STR_DECEVAL
            Case "DCV"
                strUBICACIONTITULO = STR_DCV
            Case Else
                strUBICACIONTITULO = STR_FISICO
        End Select

        'La fecha de liquidacion no cambia porque el nro de liquidacion inicia en la posicion 17
        dtmLiquidacion = DateSerial(CInt(Mid$(pstrDatos, 17, 4)), CInt(Mid$(pstrDatos, 21, 2)), CInt(Mid$(pstrDatos, 23, 2)))
        If Not IsDate(dtmLiquidacion) Then
            Call LogImportacion(pintCual, dtmLiquidacion, "La fecha de liquidación tiene un formato no válido")
            Exit Sub
        End If


        If ComparaFechas(dtmLiquidacion, pDateDesde, "<") Or
            ComparaFechas(dtmLiquidacion, pDateHasta, ">") Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 17, 8), MSGERROR_RANGO_FECHAS)
            Exit Sub
        End If

        If Not IsNumeric(Mid$(pstrDatos, 25, 9)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 25, 9), "Número de operación incorrecto")
            Exit Sub
        End If

        lngID = Mid$(pstrDatos, 25, 9) 'Número de la liquidación

        If Not IsNumeric(Mid$(pstrDatos, 34, 3)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 34, 3), "Secuencia o número de fracción incorrecto")
        End If
        lngParcial = Mid$(pstrDatos, 34, 3)

        'Punto de la operación(6)
        strTipo = Mid$(pstrDatos, 37, 1)

        'Mercado(8)
        strClaseOrden = Mid$(pstrDatos, 39, 1)

        'Con los TES viene en clase la D
        If strClaseOrden = "R" Or strClaseOrden = "D" Then
            strClaseOrden = "C" 'Liquidación de Renta Fija, C:Indica Crediticio
        Else
            strClaseOrden = "A" 'Liquidación de Acciones
        End If

        strIDEspecie = Trim(Mid$(pstrDatos, 40, 10)) 'Nemotécnico de la especie (10)

        If strTipo = "C" Then
            'Código del operador comprador(12)
            lngIDComisionistaLocal = Mid$(pstrDatos, 56, 3)
        Else
            'Código del operador vendedor(13)
            lngIDComisionistaLocal = Mid$(pstrDatos, 59, 3)
        End If

        'Cantidad o valor nominal(14)
        If Not IsNumeric(Mid$(pstrDatos, 62, 18)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 62, 18), "Cantidad de la operación incorrecta")
            Exit Sub
        End If

        dblCantidad = Mid$(pstrDatos, 62, 18)

        'If Not IsNumeric(Mid$(pstrDatos, 299, 19)) Then
        '    Call LogImportacion(pintCual, Mid$(pstrDatos, 299, 19), "Cantidad de la transacción incorrecta")
        '    Exit Sub
        'End If

        If lngParcial = 0 Then
            dblCantidad = Mid$(pstrDatos, 62, 18) 'Cantidad o Valor Nominal(15)
            'curTransaccion = Mid$(pstrDatos, 108, 19)
        Else
            dblCantidad = Mid$(pstrDatos, 170, 15) 'Cantidad de la fraccion(30)
            'curTransaccion = Mid$(pstrDatos, 299, 19)
        End If

        curTransaccion = Mid$(pstrDatos, 185, 19)

        'Días al vencimiento (15)
        If Not IsNumeric(Mid$(pstrDatos, 80, 5)) Then
            Call LogImportacion(pintCual, Mid$(pstrDatos, 80, 5), "Días al vencimiento incorrecto")
            Exit Sub
        End If
        lngDiasVencimiento = Mid$(pstrDatos, 80, 5)

        'Precio unitario(17)
        curPrecio = Mid$(pstrDatos, 85, 13)

        Dim strEmision As String

        Dim strVencimiento As String

        'Tipo de oferta (23)
        strTipoDeOferta = IIf(Trim(Mid$(pstrDatos, 110, 1)) <> "", CType(Mid$(pstrDatos, 110, 1), System.Nullable(Of Char)), CChar("?"))
        'strtipoDeOferta  puede tener estos valores:
        'N: Oferta Normal
        'R: Repo
        'A: Repo en Acciones
        'P: OPAS
        'M: Martillo     Si Mercado ="A"
        'M: Simultanea  Si Mercado = "R"
        'O: Opciones  Si Mercado = "R"
        'D: Forward registro
        'F: Fondeo
        'S: Subasta
        'C: CARRUSEL
        'I: Interbancario
        'W: Swap
        'X: Otras
        '1: Simultáneas de salida
        '2: Simultáneas de regreso
        '3: TTV de salida
        '4: TTV de regreso

        Dim strCumplimiento As String

        strCumplimiento = Mid$(pstrDatos, 98, 8) 'Por el momento, tomar "Fecha de liquidación(18)"

        dtmCumplimiento = DateSerial(CInt(Mid$(strCumplimiento, 1, 4)), CInt(Mid$(strCumplimiento, 5, 2)), CInt(Mid$(strCumplimiento, 7, 2)))
        If strClaseOrden = "C" Then
            'strModalidad = Mid$(pstrDatos, 195, 1) & Mid$(pstrDatos, 196, 1)
            'strMercado = Mid$(pstrDatos, 216, 1)
            'If strModalidad = "PA" Then
            '    strModalidad = "UA"
            'Else
            '    If strModalidad = "PV" Or Trim(strModalidad) = "" Or IsNothing(strModalidad) Or strModalidad = "NO" Then
            '        strModalidad = "UV"
            '    End If
            'End If
        Else
            strModalidad = ""
            strMercado = ""
        End If

        'lngPlazo = IIf(Trim(strMercado) <> "", IIf(IsNumeric(Mid$(pstrDatos, 200, 3)), CType(Mid$(pstrDatos, 200, 3), System.Nullable(Of Integer)), 0), 0)
        lngPlazo = 0

        strPosicionPropia = Mid$(pstrDatos, 109, 1) 'Indicador de origen(22)

        strRepoTitulo = IIf(Trim(Mid$(pstrDatos, 110, 1)) <> "", Mid$(pstrDatos, 110, 1), "?") 'strTipoOferta
        If (strRepoTitulo = "R" Or strRepoTitulo = "A") Then
            If CDate(dtmLiquidacion) <> CDate(dtmCumplimiento) Then strTipo = IIf(strTipo = "C", "R", "S") 'Jairo Gonzalez 2007/06/20
            dblTasaEfectiva = Mid$(pstrDatos, 127, 16) 'Tasa para la operación repo(25)
        End If

        'strSwap = Mid$(pstrDatos, 217, 1)
        strSwap = ""

        'If strSwap = "S" Then lngNroSwap = Mid$(pstrDatos, 218, 8)
        'Estos valores no son actualizados
        strPlaza = "LOC"             '2001/05/25   Siempre seran locales con una unica Bolsa de valores
        logOtraPlaza = False         '2001/05/25   Siempre seran locales con una unica Bolsa de valores
        lngIDCiudadOtraPlaza = 585   '2001/05/25   Siempre seran locales con una unica Bolsa de valores
        bytIDRueda = 1               '2001/05/25   Siempre seran locales con una unica Bolsa de valores

        dblFactorComisionPactada = Mid$(pstrDatos, 204, 8) '% de la comisión (31)

        'Valor de la comisión (32)
        curComision = Mid$(pstrDatos, 212, 13)

        If dblPorcentajeIvaComision = -1 Then
            dblPorcentajeIvaComision = CampoTabla("1", "dblIvaComision", "tblinstalacion", "1")
            dblPorcentajeIvaComision = dblPorcentajeIvaComision / 100
        End If

        If IsNumeric(curComision) And curComision > 0 Then
            curValorIva = Format((dblPorcentajeIvaComision * curComision) - 0.005, "#,#.00")
        Else
            curValorIva = 0
        End If

        If Not IsNumeric(Mid$(pstrDatos, 232, 16)) Then
            dblTasaCompraVende = 0
        Else
            dblTasaCompraVende = Mid$(pstrDatos, 232, 16) 'Tasa neta de la fracción(35)
        End If

        'Número de veces de impresión(36)
        If Not IsNumeric(Mid$(pstrDatos, 267, 2)) Then
            lngImpresiones = 0
        Else
            lngImpresiones = Mid$(pstrDatos, 267, 2)
        End If

        If Not IsNumeric(Mid$(pstrDatos, 269, 12)) Then
            dblValBolsa = 0
        Else
            dblValBolsa = Mid$(pstrDatos, 269, 12) 'Servicio en bolsa(Cargo por volumen),Fijo(38)
        End If

        curRetencion = 0
        dblValorTraslado = 0

        'curRetencion = IIf(Mid$(pstrDatos, 497, 13) = "", 0, CType(Mid$(pstrDatos, 497, 13), System.Nullable(Of Double)))
        'dblValorTraslado = IIf(Mid$(pstrDatos, 531, 13) = "", 0, CType(Mid$(pstrDatos, 531, 13), System.Nullable(Of Double))) 'En la posición 12 se evalua el signo

        If Not IsNumeric(Mid$(pstrDatos, 281, 12)) Then
            dblServBolsaVble = 0
        Else
            dblServBolsaVble = CType(Mid$(pstrDatos, 281, 12), System.Nullable(Of Double)) 'Servicio en bolsa (Cargo por volumen), VARIABLE(39)
        End If

        'strIndicadorAcciones = Mid$(pstrDatos, 564, 1)
        strIndicadorAcciones = "S"

        If strTipo = "C" Or strTipo = "R" Then
            curSubTotalLiq = curTransaccion + curComision + curValorIva
        Else
            curSubTotalLiq = curTransaccion - curComision - curValorIva
        End If
        If strTipo = "C" Or strTipo = "R" Or strMercado = "P" Then
            curTotalLiq = curSubTotalLiq + curRetencion + dblValorTraslado '+ IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
        Else
            curTotalLiq = curSubTotalLiq + curRetencion + dblValorTraslado '- IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
        End If

        'If Mid$(pstrDatos, 474, 1) = "S" Then
        '    logOrdinaria = False
        '    strObjetoOrdenExtraOrdinaria = STRCARRUSEL
        '    lngNumPadre = lngID
        '    lngParcialPadre = lngParcial
        '    dtmOperacionPadre = CDate(dtmLiquidacion)
        'Else
        '    dtmOperacionPadre = ValidaFecha("")
        'End If

        'Tipo de identificación del cliente 1(42)
        strTipoIdentificacion = Mid$(pstrDatos, 312, 1)

        'If strTipoIdentificacion = "N" Then
        '    strNroDocumento = Trim(Mid$(pstrDatos, 427, 12))
        'Else
        '    strNroDocumento = Trim(Mid$(pstrDatos, 427, 12))
        'End If

        'Identificacion del cliente 1(43)
        strNroDocumento = Trim(Mid$(pstrDatos, 313, 12))

        strNroDocumento = ReemplazarcaracterEnCadena(strNroDocumento)

        'If Not IsNumeric(Mid$(pstrDatos, 594, 19)) Then
        '    dblValorEntregaContraPago = 0
        'Else
        '    dblValorEntregaContraPago = IIf(Mid$(pstrDatos, 594, 19) = "", 0, CType(Mid$(pstrDatos, 594, 19), System.Nullable(Of Double)))
        'End If

        'strAquienSeEnviaRetencion = Mid$(pstrDatos, 613, 1)

        If Not VerificarSiEspecieExiste_MI(lngIDBolsa, strIDEspecie, logEsAccion) Then
            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_ESPECIE & strIDEspecie & vbTab & MSGERROR_TABLA_ESPECIES)
            Exit Sub
        End If

        If Not logEsAccion Then
            strVencimiento = dtmVencimiento
        Else
            strVencimiento = ""
        End If

        If ExisteLiquidacion_MI(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
            'Existe Exactamente la liquidación - con strTipo = " "
            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_LIQUIDACION & lngID & MSGERROR_TABLA_LIQUIDACIONES)
            'Valido los campos que vienen contra los que existen
            Call Validar_Campo_a_Campo(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa)


            If IsNothing(rstConsultarDatosLiquidacion) Or rstConsultarDatosLiquidacion.Count = 0 Then
                Exit Sub
            End If


            With rstConsultarDatosLiquidacion(0)

                'If .dtmVencimiento <> dtmVencimiento Then
                '    Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
                'End If
                If .dblServBolsaVble <> dblServBolsaVble Then
                    Call LogDiferencias(.dblServBolsaVble, "Servicio Bolsa - Vlr en el Plano : " & dblServBolsaVble)
                End If
                If .curTotalLiq <> curTotalLiq Then
                    Call LogDiferencias(.curTotalLiq, "Total operación - Vlr en el Plano : " & curTotalLiq)
                End If
                If .dblValorTraslado <> dblValorTraslado Then
                    Call LogDiferencias(.dblValorTraslado, "Vlr Traslado - Vlr en el Plano : " & dblValorTraslado)
                End If
                If .curRetencion <> curRetencion Then
                    Call LogDiferencias(.curRetencion, "Retención - Vlr en el Plano : " & curRetencion)
                End If
                If .dblValBolsa <> dblValBolsa Then
                    Call LogDiferencias(.dblValBolsa, "Servicio Bolsa - Vlr en el Plano : " & dblValBolsa)
                End If
                If .dblTasaCompraVende <> dblTasaCompraVende Then
                    Call LogDiferencias(.dblTasaCompraVende, "Tasa Neta Fracción - Vlr en el Plano : " & dblTasaCompraVende)
                End If
                If .curValorIva <> curValorIva Then
                    Call LogDiferencias(.curValorIva, "IVA Comisión - Vlr en el Plano : " & curValorIva)
                End If
                If .curComision <> curComision Then
                    Call LogDiferencias(.curComision, "Comisión - Vlr en el Plano : " & curComision)
                End If
                If .dblFactorComisionPactada <> dblFactorComisionPactada Then
                    Call LogDiferencias(.strModalidad, "Modalidad - Vlr en el Plano : " & strModalidad)
                End If
                If (.strModalidad <> strModalidad) And logEsAccion = False Then
                    Call LogDiferencias(.strModalidad, "Modalidad - Vlr en el Plano : " & strModalidad)
                End If
                If .dtmCumplimiento <> dtmCumplimiento Then
                    Call LogDiferencias(.dtmCumplimiento, "Fecha Cumplimiento - Vlr en el Plano : " & dtmCumplimiento)
                End If
                If .strTipoDeOferta <> strTipoDeOferta Then
                    Call LogDiferencias(.strTipoDeOferta, "Tipo Oferta - Vlr en el Plano : " & strTipoDeOferta)
                End If
                If .dblTasaEfectiva <> dblTasaEfectiva Then
                    Call LogDiferencias(.dblTasaEfectiva, "Tasa Efectiva - Vlr en el Plano : " & dblTasaEfectiva)
                End If
                If .dblPuntosIndicador <> dblPuntos Then
                    Call LogDiferencias(.dblPuntosIndicador, "Puntos sobre el Indicador - Vlr en el Plano : " & dblPuntos)
                End If
                If .strIndicadorEconomico <> strIndicador Then
                    Call LogDiferencias(.strIndicadorEconomico, "Indicador Economico - Vlr en el Plano : " & strIndicador)
                End If
                If .lngPlazoOriginal <> lngPlazoOriginal Then
                    Call LogDiferencias(.lngPlazoOriginal, "Plazo - Vlr en el Plano : " & lngPlazoOriginal)
                End If
                If .dtmVencimiento <> dtmVencimiento Then
                    Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
                End If
                'If .dtmEmision <> dtmEmision Then
                '    Call LogDiferencias(.dtmEmision, "Emisión - Vlr en el Plano : " & dtmEmision)
                'End If
                If .curPrecio <> curPrecio Then
                    Call LogDiferencias(.curPrecio, "Precio - Vlr en el Plano : " & curPrecio)
                End If
                If .lngDiasVencimiento <> lngDiasVencimiento Then
                    Call LogDiferencias(.lngDiasVencimiento, "Dias al Vencimiento - Vlr en el Plano : " & lngDiasVencimiento)
                End If
                If .curTransaccion <> curTransaccion Then
                    Call LogDiferencias(.curTransaccion, "Transacción - Vlr en el Plano : " & curTransaccion)
                End If
                If .dblCantidad <> dblCantidad Then
                    Call LogDiferencias(.dblCantidad, "Cantidad - Vlr en el Plano : " & dblCantidad)
                End If
                If .dblTasaDescuento <> dblTasaDescuento Then
                    Call LogDiferencias(.dblTasaDescuento, "Tasa Nominal - Vlr en el Plano : " & dblTasaDescuento)
                End If

            End With

            Exit Sub
        Else
            If ExisteLiquidacion_MI(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
                If strTipo = "C" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_COMPRA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                ElseIf strTipo = "V" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_VENTA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                ElseIf strTipo = "R" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_RECOMPRA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                ElseIf strTipo = "S" Then
                    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_REVENTA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
                End If
                Exit Sub
            End If
        End If

        lngOrden = 0
        lngIDComitente = "0"
        lngIDOrdenante = "0"

        'lngOrden = CDbl(CStr(Now.Year) & Format$(Mid$(pstrDatos, 414, 8), "000000"))

        Dim strOrden As String

        Dim strDepositoOrden As String = String.Empty 'Jorge Arango 2009/11/03 Ubicacion del titulo de la orden
        Dim strEstadoAprobOrden As String = String.Empty
        Dim strEstadoAprobCliente As String = String.Empty

        'strOrden = Now.Year.ToString & Format$(Val(Mid$(pstrDatos, 418, 8)), "000000")  ' AFG
        strOrden = "" 'El archivo plano no especifica número de orden 

        strHoraGrabacion = Trim(Mid$(pstrDatos, 9, 8)) 'Hora de grabación (2)
        strOrigenOperacion = Mid$(pstrDatos, 38, 1)    'Tipo de operación (7)
        lngCodigoOperadorCompra = Mid$(pstrDatos, 56, 3) 'Código del operador comprador (12)
        lngCodigoOperadorVende = Mid$(pstrDatos, 59, 3)  'Código del operador vendedor (13)
        strIdentificacionRemate = ""
        strModalidaOperacion = Mid$(pstrDatos, 106, 1) 'Modalidad de liquidación (18)
        strIndicadorPrecio = Mid$(pstrDatos, 107, 1)      'Indica si fijo precio en acciones (19)
        strPeriodoExdividendo = Mid$(pstrDatos, 108, 1)   'Indica si es exdividendo (20)
        lngPlazoOperacionRepo = Mid$(pstrDatos, 128, 5)   'Plazo de la operación repo (25)
        dblValorCaptacionRepo = CDbl(IIf(Mid$(pstrDatos, 148, 15) = "", 0, Mid$(pstrDatos, 133, 15))) 'Valor de captación del repo (26)
        lngVolumenCompraRepo = CDbl(IIf(Mid$(pstrDatos, 163, 19) = "", 0, Mid$(pstrDatos, 148, 19))) 'Volumen de recompra del repo (27)
        dblPrecioNetoFraccion = CDbl(IIf(Mid$(pstrDatos, 240, 7) = "", 0, Mid$(pstrDatos, 225, 7)))  'Precio neto de la fracción (33)
        lngVolumenNetoFraccion = CDbl(IIf(Mid$(pstrDatos, 263, 19) = "", 0, Mid$(pstrDatos, 248, 19))) 'Volumen neto de la fracción (35)
        strCodigoContactoComercial = Mid$(pstrDatos, 293, 8) 'Código del trader (39)
        lngNroFraccionOperacion = Mid$(pstrDatos, 301, 3)      'Número de fracciones (40)
        strIdentificacionPatrimonio1 = Mid$(pstrDatos, 325, 3)   'Identificación patrimonio autónomo 1 (44)
        strTipoidentificacionCliente2 = Mid$(pstrDatos, 328, 1)  'Tipo de identificación del cliente 2 (45)
        strNitCliente2 = Mid$(pstrDatos, 329, 12)               'Identificación del cliente 2 (46)
        strIdentificacionPatrimonio2 = Mid$(pstrDatos, 341, 3)  'Identificación patrimonio autónomo 2 (47)
        strTipoIdentificacionCliente3 = Mid$(pstrDatos, 344, 1) 'Tipo de identificación del cliente 3 (48)
        strNitCliente3 = Mid$(pstrDatos, 345, 12)               'Identificación del cliente 3 (49)
        strIdentificacionPatrimonio3 = Mid$(pstrDatos, 357, 3)  'Identificación patrimonio autónomo 3 (50)
        strIndicadorOperacion = Mid$(pstrDatos, 360, 1)         'Indicador de la operación (51)
        dblBaseRetencion = 0 '
        dblPorcRetencion = 0 '
        dblBaseRetencionTranslado = 0  '
        dblPorcRetencionTranslado = 0 '
        strOperacionNegociada = "" '
        Dim dtmFechaConstancia1 As String
        dtmFechaConstancia1 = "" '
        'dtmFechaConstancia = ""
        dblValorConstancia = 0 '
        strGeneraConstancia = "" '
        ''''lngImpresiones = 0 '

        '------------------------------------------------------------------------------------------------
        'TEMPORAL:
        Dim strCodIntermediario As String 'Último campo del archivo plano.
        Dim strObjetoOrden As String
        Dim strCodigoCliente As String = String.Empty
        Dim strCodigoOrdenante As String = String.Empty
        Dim strCodigoReceptor As String = String.Empty
        Dim strCodFondo As String = String.Empty
        Dim strCuentaFondo As String


        '---En este correo
        'From: Luis N. Rivera Saldarriaga
        'Sent: Wednesday, November 03, 2010 9:34 AM
        'To: Jose Alberto Gomez Velasquez
        'Cc: Elizabeth Arias Posada; Carlos Alberto Gómez Aristizabal; Jorge Andrés Bedoya Garcés
        'Subject: RE: Proyecto Mercado Integrado - Inquietudes sobre archivos planos
        '--- se especifica:
        '"Respecto al código del intermedio extranjero, este se compone de 9 caracteres
        'donde los 3 primeros indican la Bolsa de Origen, los 3 siguientes el código del afiliado y los 3 últimos el código del operador (este campo se registra en ceros debido a que no se tiene la información). Para aclarar la duda, este campo es ALFANUMERICO por lo que sí es correcto que se registre con el espacio que ustedes observan."

        strCodIntermediario = Mid$(pstrDatos, 383, 9)
        strCodIntermediario = Mid$(strCodIntermediario, 4, 3) '(Código del afiliado): Temporalmente, mientras se define cuál grupo corresponde al Cliente.


        'ESTO ES UNA PRUEBA JBT
        'strCodIntermediario = "9090"
        ''''strCodIntermediario = "33" '(PRUEBAS)

        'ojjooo: ¿Cómo identificar si el código del Intermediario es del mercado chileno o peruano?
        Call ObtenerCodigoCliente(strCodIntermediario, strCodigoCliente, strCodigoOrdenante, strCodigoReceptor)

        If Trim(strCodigoCliente) = "" Then
            Call LogImportacion(pintCual, strCodIntermediario, "Intermediario no matriculado en Códigos otros sistemas.")
            Exit Sub
        End If
        If Trim(strCodigoOrdenante) = "" Then
            Call LogImportacion(pintCual, strCodigoCliente, "El cliente carece de un ordenante líder.")
            Exit Sub
        End If


        strObjetoOrden = IIf(strObjetoOrdenExtraOrdinaria <> "", strObjetoOrdenExtraOrdinaria, strObjeto)

        strCuentaFondo = CuentaFondo(strCodigoCliente, strUBICACIONTITULO)

        ' Ojjooo: Pendiente consultar....
        'If Trim(strValorParametroCUENTADEPOSITOORDENES) = "" Then strValorParametroCUENTADEPOSITOORDENES = ValorParametro("CUENTADEPOSITOORDENES", "NO")
        'If strValorParametroCUENTADEPOSITOORDENES = "SI" Then
        '   If (strUBICACIONTITULO <> "F") And Trim(strCuentaFondo) = "" Then
        '      If strUBICACIONTITULO <> "" Then
        '         Call LogImportacion(pintCual, strCodigoCliente, "Este cliente no tiene cuenta de depósito.")
        '         Exit Sub
        '      End If
        '   End If
        'End If

        If strClaseOrden = "A" Then
            Call CrearOrden(strTipo, strRepoTitulo, strCodigoCliente, strCodigoOrdenante, curComision,
                            strUBICACIONTITULO, strIDEspecie, dblCantidad, curPrecio, CDate(dtmCumplimiento), strCuentaFondo,
                            strCodigoReceptor, lngIDOrden)
            If lngIDOrden <> -1 Then
                lngOrden = lngIDOrden
                lngIDComitente = strCodigoCliente
                lngIDOrdenante = strCodigoOrdenante
            Else
                Call LogImportacion(pintCual, strCodigoCliente, "No fue posible crear la orden para este cliente.")
                Exit Sub
            End If
        End If

        Dim ret = L2SDC.sp_GrabarLiquidacion(lngID,
                                                lngParcial,
                                                strTipo,
                                                strClaseOrden,
                                                strIDEspecie,
                                                lngOrden,
                                                lngIDComitente,
                                                lngIDOrdenante,
                                                lngIDBolsa,
                                                bytIDRueda,
                                                dblValBolsa,
                                                dblTasaDescuento,
                                                dblTasaCompraVende,
                                                strModalidad,
                                                strIndicador,
                                                dblPuntos,
                                                lngPlazo,
                                                dtmLiquidacion,
                                                dtmCumplimiento,
                                                dtmEmision,
                                                dtmVencimiento,
                                                logOtraPlaza,
                                                strPlaza,
                                                lngIDComisionistaLocal,
                                                lngIDComisionistaOtraPlaza,
                                                lngIDCiudadOtraPlaza,
                                                dblTasaEfectiva,
                                                dblCantidad,
                                                curPrecio,
                                                curTransaccion,
                                                curSubTotalLiq,
                                                curTotalLiq,
                                                curComision,
                                                curRetencion,
                                                curIntereses,
                                                lngDiasIntereses,
                                                dblFactorComisionPactada,
                                                strMercado,
                                                strNroTitulo,
                                                lngIDCiudadExpTitulo,
                                                lngPlazoOriginal,
                                                logAplazamiento,
                                                bytVersionPapeleta,
                                                dtmEmisionOriginal,
                                                dtmVencimientoOriginal,
                                                lngImpresiones,
                                                strFormaPago,
                                                lngCtrlImpPapeleta,
                                                lngDiasVencimiento,
                                                strPosicionPropia,
                                                strTransaccion,
                                                strTipoOperacion,
                                                lngDiasContado,
                                                logOrdinaria,
                                                strObjetoOrdenExtraOrdinaria,
                                                lngNumPadre,
                                                lngParcialPadre,
                                                dtmOperacionPadre,
                                                lngDiasTramo,
                                                dblValorTraslado,
                                                dblValorBrutoCompraVencida,
                                                strAutoRetenedor,
                                                strSujeto,
                                                dblPcRenEfecCompraRet,
                                                dblPcRenEfecVendeRet,
                                                strReinversion,
                                                strSwap,
                                                lngNroSwap,
                                                strCertificacion,
                                                dblDescuentoAcumula,
                                                dblPctRendimiento,
                                                dtmFechaCompraVencido,
                                                dblPrecioCompraVencido,
                                                strConstanciaEnajenacion,
                                                strRepoTitulo,
                                                strUsuario,
                                                dblServBolsaVble,
                                                dblServBolsaFijo,
                                                strTraslado,
                                                curValorIva,
                                                strUBICACIONTITULO,
                                                strTipoIdentificacion,
                                                strNroDocumento,
                                                dblValorEntregaContraPago,
                                                strAquienSeEnviaRetencion,
                                                strBaseDias,
                                                strTipoDeOferta,
                                                strHoraGrabacion,
                                                strOrigenOperacion,
                                                lngCodigoOperadorCompra,
                                                lngCodigoOperadorVende,
                                                strIdentificacionRemate,
                                                strModalidaOperacion,
                                                strIndicadorPrecio,
                                                strPeriodoExdividendo,
                                                lngPlazoOperacionRepo,
                                                dblValorCaptacionRepo,
                                                lngVolumenCompraRepo,
                                                dblPrecioNetoFraccion,
                                                lngVolumenNetoFraccion,
                                                strCodigoContactoComercial,
                                                lngNroFraccionOperacion,
                                                strIdentificacionPatrimonio1,
                                                strTipoidentificacionCliente2,
                                                strNitCliente2,
                                                strIdentificacionPatrimonio2,
                                                strTipoIdentificacionCliente3,
                                                strNitCliente3,
                                                strIdentificacionPatrimonio3,
                                                strIndicadorOperacion,
                                                dblBaseRetencion,
                                                dblPorcRetencion,
                                                dblBaseRetencionTranslado,
                                                dblPorcRetencionTranslado,
                                                dblPorcIvaComision,
                                                strIndicadorAcciones,
                                                strOperacionNegociada,
                                                dtmFechaConstancia,
                                                dblValorConstancia,
                                                strGeneraConstancia,
                                                0,
                                                DemeInfoSesion(pstrUsuario, "ExtraerDatosColombiaNuevo"),
                                                ClsConstantes.GINT_ErrorPersonalizado,
                                                Nothing,
                                                Nothing,
                                                Nothing,
                                                Nothing,
                                                Nothing)



        RegBuenos = RegBuenos + 1

    End Sub
    Private Sub ObtenerCodigoCliente(ByVal pstrCodigoOtroSistema As String, _
                               ByRef pstrCodigoCliente As String, _
                               ByRef pstrCodigoOrdenante As String, _
                               ByRef pstrCodigoReceptor As String)
        'Obtener el codigo del cliente para crear la orden posteriormente
        Try
            pstrCodigoCliente = ""
            pstrCodigoOrdenante = ""
            pstrCodigoReceptor = ""
            L2SDC.usp_ConsultarCliente_OyDNet(pstrCodigoOtroSistema, pstrCodigoCliente, pstrCodigoOrdenante, pstrCodigoReceptor)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ObtenerCodigoCliente")
        End Try


    End Sub
    Private Function CuentaFondo(ByVal plngIdComitente As String, ByVal pstrFondo As String) As String
        Try
            'obtiene la cuenta para insertarla posteriormente en la orden
            Dim a As String = "0"
            L2SDC.spValidarCuentasDEC_OyDNet(a, plngIdComitente, pstrFondo)
            Return a
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentaFondo")
        End Try
        Return Nothing
    End Function
    Private Sub CrearOrden(ByVal pstrTipo As String, _
                       ByVal pstrRepoTitulo As String, ByVal strCodigoCliente As String, ByVal strCodigoOrdenante As String, _
                       ByVal pcurComision As Double, ByVal pstrUBICACIONTITULO As String, ByVal pstrIdEspecie As String, _
                       ByVal pdblCantidad As Double, ByVal pcurPrecio As Double, ByVal pdtmCumplimiento As Date, _
                       ByVal pstrCuentaFondo As String, ByVal pstrCodigoReceptor As String, ByRef plngIDOrden As Long)
        Try
            'crear la orden a partir de la liquidacion
            Dim cuentafondo As Long
            If Trim(pstrCuentaFondo) = "" Or Not IsNumeric(pstrCuentaFondo) Then
                cuentafondo = Nothing
            Else
                cuentafondo = (CLng(pstrCuentaFondo))
            End If
            L2SDC.usp_CrearOrden_OyDNet(pstrTipo, IIf((pstrRepoTitulo = "R" Or pstrRepoTitulo = "A"), True, False), _
            fmtCodigo(strCodigoCliente), fmtCodigo(strCodigoOrdenante), pcurComision, Now, _
             Now, "P", "A", pstrUBICACIONTITULO, gstrUser, Now, _
            gstrUser, Now, pstrIdEspecie, pdblCantidad, pcurPrecio, pcurPrecio, pdtmCumplimiento, _
            cuentafondo, pstrCodigoReceptor, 1, 100, plngIDOrden)

        Catch ex As Exception
            plngIDOrden = -1
            ManejarError(ex, Me.ToString(), "CrearOrden")

        End Try
    End Sub



    Private Function ValidaFecha(ByVal pstrFechaArchivo As String) As System.Nullable(Of Date)
        Dim dtmRetorno As System.Nullable(Of Date) = Nothing
        If Not String.IsNullOrEmpty(pstrFechaArchivo) Then
            dtmRetorno = CDate(pstrFechaArchivo)
        End If
        Return dtmRetorno
    End Function



End Class
