Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports A2.OyD.Infraestructura

Friend Class clsImportarLiquidaciones
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
    Private rstConsultarDatosLiquidacion As List(Of usp_Bolsa_ConsultarOperacionExistente_VALIDARResult)

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
    Private Const STR_EU As String = "X"

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
    Private Const MSGERROR_VALORTOTAL As String = "El curTotalLiq de la liquidación '"
    Private Const MSGERROR_TABLA_LIQUIDACIONES_VALORTOTAL As String = "' Es un valor negativo"

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

        Dim ret = (L2SDC.uspOyDNet_Importaciones_VerificarOrdenLiq(pstrTipo, pstrClase, CType(CLng(pstrOrden), Integer?), CStr(pstrEspecie), CStr(pstrNroDocumento))).ToList
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


    Private Function VerificarSiEspecieExiste(ByVal plngIDBolsa As Long,
                                      ByRef pstrIdEspecie As String,
                                      ByRef pstrlogEsAccion As Boolean) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function VerificarEspecie(plngIDBolsa As Long, ByRef pstrIdEspecie As String,  ByRef pstrlogEsAccion As Boolean) As Boolean
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


        ' Dim rst As rdoResultset
        VerificarSiEspecieExiste = True
        'Try
        Dim ret = (L2SDC.uspOyDNet_Importaciones_VerificarEspecies(Trim(pstrIdEspecie), CType(plngIDBolsa, Integer?))).ToList
        If ret.Count > 0 Then
            Dim PrimerRegistro = ret(0)
            pstrIdEspecie = PrimerRegistro.strIdEspecie
            pstrlogEsAccion = PrimerRegistro.logEsAccion
        Else
            pstrIdEspecie = Trim(pstrIdEspecie)
            pstrlogEsAccion = CBool(0)
            VerificarSiEspecieExiste = False
        End If

        Return VerificarSiEspecieExiste
    End Function


    Private Function ExisteLiquidacion(ByVal pdtmLiquidacion As Date, ByVal plngIDLiq As Long, ByVal plngParcial As Long,
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

        ExisteLiquidacion = False
        'Try
        Dim plogexiste As Boolean
        Dim ret = L2SDC.uspOyDNet_Importaciones_BuscarLiquidacion(CDate(pdtmLiquidacion), CType(plngIDLiq, Integer?), CType(plngParcial, Integer?), pstrTipo, pstrClase, CType(plngIDBolsa, Integer?), CType(plogexiste, Integer?)).ToList

        If ret.Count <= 0 Then 'No Existe
            ExisteLiquidacion = False
        Else
            ExisteLiquidacion = True
        End If

        'Catch ex As Exception
        '    MessageBox.Show(ex.Message)
        '    ExisteLiquidacion = False
        '    'LogErrores(Me.Name & ".ExisteLiquidacion", Err.Number, Err.Description)
        'End Try
        Return ExisteLiquidacion
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


    Private Function ExisteDetalleLiq(ByVal plngIDLiq As Long, ByVal plngParcial As Long, ByVal pstrTipo As String,
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

    Public Function BorrarEncabezados() As String
        Dim retorno As String = ""
        Try
            Dim res = L2SDC.ExecuteQuery(Of Object)("delete from tblImportacionliq")

        Catch ex As Exception
            retorno = "Error: " & ex.Message
        End Try
        Return retorno
    End Function
    Public Function BorrarEncabezadosCompensacion() As String
        Dim retorno As String = ""
        Try
            Dim res = L2SDC.ExecuteQuery(Of Object)("delete from tblLiqCompensacion")
        Catch ex As Exception
            retorno = "Error: " & ex.Message
        End Try
        Return retorno
    End Function

    Private Sub Validar_Campo_a_Campo(ByVal pdtmLiquidacion As Date, ByVal plngIDLiq As Long, ByVal plngParcial As Long,
                                        ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDBolsa As Long)

        Dim ret As List(Of usp_Bolsa_ConsultarOperacionExistente_VALIDARResult) = L2SDC.uspOyDNet_Importaciones_ConsultarOperacionExistente(CDate(pdtmLiquidacion), CType(plngIDLiq, Integer?), CType(plngParcial, Integer?), pstrTipo,
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

    Public Function EvaluarLineaColombia(ByVal pNombreEncabezado As String, ByVal pEstrActual As Boolean, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal pstrUsuario As String, ByVal pdtmFechaCierre As DateTime) As String
        '     '/******************************************************************************************
        '     '/* INICIO DOCUMENTO
        '     '/* Sub EvaluarLineaColombia()
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

            If objReader.Peek() <> -1 Then
                Dim primeraLinea = objReader.ReadLine()
                If pEstrActual = True Then
                    If Len(primeraLinea) > 613 Then
                        objReader.Close()
                        Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                        Exit Function
                    End If
                Else 'ES ESTRUCTURA NUEVA
                    If Len(primeraLinea) < 613 Then
                        objReader.Close()
                        Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                        Exit Function
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
                If pEstrActual = True Then
                    ExtraerDatosColombiaNuevo(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario, pdtmFechaCierre)
                Else
                    ExtraerDatosColombiaNuevo(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario, pdtmFechaCierre)
                End If
                intCual = intCual + 1
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

    Public Function EvaluarLineaColombiaCompensacion(ByVal pNombreEncabezado As String, ByVal pEstrActual As Boolean, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal pstrUsuario As String, ByVal pbAcciones As Boolean, ByVal pbCrediticio As Boolean, ByVal pbAmbos As Boolean) As String
        '     '/******************************************************************************************
        '     '/* INICIO DOCUMENTO
        '     '/* Sub EvaluarLineaColombia()
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
        Dim objReader As System.IO.StreamReader = Nothing
        Try
            Dim strdatos As String
            Dim intCual As Integer
            'Dim objReader As System.IO.StreamReader = Nothing

            Dim FILE_NAME As String = pNombreEncabezado
            'Abrimos el archivo de encabezados
            objReader = New System.IO.StreamReader(FILE_NAME)
            If objReader.Peek() <> -1 Then
                Dim primeraLinea = objReader.ReadLine()
                If pEstrActual = True Then
                    If Len(primeraLinea) > 624 Then
                        objReader.Close()
                        Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                        Exit Function
                    End If
                Else 'ES ESTRUCTURA NUEVA
                    If Len(primeraLinea) < 625 Then
                        objReader.Close()
                        Return "El archivo seleccionado no corresponde con la estructura necesaria para ser importado, verifique si el archivo a importar corresponde con la estructura deseada."
                        Exit Function
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
                If pEstrActual = True Then
                    ExtraerDatosColombiaNuevoCompensacion(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario, pbAcciones, pbCrediticio, pbAmbos)
                Else
                    ExtraerDatosColombiaNuevoCompensacion(intCual, strdatos, pDateDesde, pDateHasta, pstrUsuario, pbAcciones, pbCrediticio, pbAmbos)
                End If
                intCual = intCual + 1
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

    Private Sub ExtraerDatosColombiaNuevo(ByVal pintCual As Integer, ByVal pstrDatos As String, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal pstrUsuario As String, ByVal pFechaCierre As DateTime)
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Sub ExtraerDatosColombiaNuevo(pintCual As Integer, pstrDatos As String)
        '/* Alcance     :   Private
        '/* Descripción :   A partir de una hilera de caracteres separa en variables los datos
        '/*                 de una Liquidación de la Bolsa de Colombia, con previo conocimiento
        '/*                 de las posiciones donde empiezan y donde terminan el valor de cada
        '/*                 variable. Y luego las graba en tblImportacionLiq.
        '/*                 Mayo 8/2008 Luis Fernando Alvarez se actualiza dicho procedimiento por cambio en BVC para nro Liq de 9 digitos
        '/*                 En general el nro liquidacion queda de la siguiente manera xx-xxxxxxxx-xxxxxxxxx
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
        Try


            Dim logEsAccion As Boolean

            Dim strObjeto As String = String.Empty
            Dim ExisteOrden As Boolean
            Dim lngOrden As Long
            Dim dblTotalFuturoRepo As Double
            Dim dblTotalRecompras As Double
            Dim dblTotalRecomprasImportadas As Double
            Dim dblTotalFuturoOrdenesRepos As Double

            Dim lngID As System.Nullable(Of Integer),
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
                 dblTasaDescuento As System.Nullable(Of Double),
                 dblTasaCompraVende As System.Nullable(Of Double),
                 strModalidad As String,
                 strIndicador As String,
                 dblPuntos As System.Nullable(Of Double),
                 lngPlazo As System.Nullable(Of Integer),
                 dtmLiquidacion As System.Nullable(Of Date),
                 dtmCumplimiento As System.Nullable(Of Date),
                 dtmEmision As System.Nullable(Of Date),
                 dtmVencimiento As System.Nullable(Of Date),
                 logOtraPlaza As System.Nullable(Of Boolean),
                 strPlaza As String,
                 lngIDComisionistaLocal As System.Nullable(Of Integer),
                 lngIDComisionistaOtraPlaza As Integer,
                 lngIDCiudadOtraPlaza As System.Nullable(Of Integer),
                 dblTasaEfectiva As System.Nullable(Of Double),
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
                 lngPlazoOriginal As System.Nullable(Of Integer),
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
                 lngDiasContado As System.Nullable(Of Integer),
                 logOrdinaria As System.Nullable(Of Boolean) = False,
                 strObjetoOrdenExtraOrdinaria As String = String.Empty,
                 lngNumPadre As Integer,
                 lngParcialPadre As Integer,
                 dtmOperacionPadre As System.Nullable(Of Date),
                 lngDiasTramo As System.Nullable(Of Integer) = 0,
                 dblValorTraslado As System.Nullable(Of Double),
                 dblValorBrutoCompraVencida As System.Nullable(Of Double) = 0,
                 strAutoRetenedor As String = String.Empty,
                 strSujeto As String = String.Empty,
                 dblPcRenEfecCompraRet As System.Nullable(Of Double) = 0,
                 dblPcRenEfecVendeRet As System.Nullable(Of Double) = 0,
                 strReinversion As String,
                 strSwap As String,
                 lngNroSwap As Integer,
                 strCertificacion As String = String.Empty,
                 dblDescuentoAcumula As System.Nullable(Of Double) = 0,
                 dblPctRendimiento As System.Nullable(Of Double) = 0,
                 dtmFechaCompraVencido As System.Nullable(Of Date) = Nothing,
                 dblPrecioCompraVencido As System.Nullable(Of Double) = 0,
                 strConstanciaEnajenacion As String = String.Empty,
                 strRepoTitulo As String,
                 strUsuario As String = pstrUsuario,
                 dblServBolsaVble As System.Nullable(Of Double),
                 dblServBolsaFijo As System.Nullable(Of Double),
                 strTraslado As String = String.Empty,
                 curValorIva As System.Nullable(Of Double),
                 strUBICACIONTITULO As String,
                 strTipoIdentificacion As System.Nullable(Of Char),
                 strNroDocumento As String,
                 dblValorEntregaContraPago As System.Nullable(Of Double),
                 strAquienSeEnviaRetencion As System.Nullable(Of Char),
                 strBaseDias As System.Nullable(Of Char),
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
                 dblPorcIvaComision As System.Nullable(Of Double),
                 strIndicadorAcciones As System.Nullable(Of Char),
                 strOperacionNegociada As System.Nullable(Of Char),
                 dtmFechaConstancia As System.Nullable(Of Date),
                 dblValorConstancia As System.Nullable(Of Double),
                 strGeneraConstancia As System.Nullable(Of Char),
                 strCodigoIntermediario As String = String.Empty,
                 curValorExtemporaneo As System.Nullable(Of Double),
                 strPosicionExtemporaneo As String,
                 strRueda As String = String.Empty


            'lngIDBolsa = cmbvBolsas.ItemData(cmbvBolsas.ListIndex)
            Dim retorno = (L2SDC.uspA2utils_CargarCombos("UBICACIONTITULO_EXTERIOR", pstrUsuario.ToString)).ToList

            lngIDBolsa = 4
            gbtyIDBOLSA = 4

            strUBICACIONTITULO = UCase(Mid$(pstrDatos, 281, 3))
            Select Case strUBICACIONTITULO
                Case Is <> Nothing
                    Dim strUBICACIONTITULO2 = retorno.Where(Function(i) i.ID = strUBICACIONTITULO.ToString()).Select(Function(i) i.Descripcion).FirstOrDefault

                    strUBICACIONTITULO = strUBICACIONTITULO2

                    If String.IsNullOrEmpty(strUBICACIONTITULO) Then
                        strUBICACIONTITULO = STR_FISICO
                    End If

                Case Else
                    strUBICACIONTITULO = STR_FISICO
            End Select


            'La fecha de liquidacion no cambia porque el nro de liquidacion inicia en la posicion 17
            dtmLiquidacion = DateSerial(CInt(Mid$(pstrDatos, 1, 4)), CInt(Mid$(pstrDatos, 5, 2)), CInt(Mid$(pstrDatos, 7, 2)))
            If Not IsDate(dtmLiquidacion) Then
                Call LogImportacion(pintCual, dtmLiquidacion, "La fecha de liquidación tiene un formato no válido")
                Exit Sub
            End If

            If ComparaFechas(dtmLiquidacion, pDateDesde, "<") Or
                ComparaFechas(dtmLiquidacion, pDateHasta, ">") Then
                Call LogImportacion(pintCual, Mid$(pstrDatos, 1, 8), MSGERROR_RANGO_FECHAS)
                Exit Sub
            End If

            If dtmLiquidacion.Value.ToShortDateString <= pFechaCierre Then
                Call LogImportacion(pintCual, dtmLiquidacion, "La fecha de liquidación es menor o igual a la fecha de cierre (" & pFechaCierre.ToShortDateString() & ").")
                Exit Sub
            End If

            If Not IsNumeric(Right(Mid$(pstrDatos, 17, 17), 9)) Then
                Call LogImportacion(pintCual, Right(Mid$(pstrDatos, 17, 17), 9), "Número de operación incorrecto")
                Exit Sub
            End If

            lngID = Right(Mid$(pstrDatos, 17, 17), 9) 'Número de la liquidación

            If Not IsNumeric(Mid$(pstrDatos, 34, 3)) Then
                Call LogImportacion(pintCual, Mid$(pstrDatos, 34, 3), "Número del parcial incorrecto")
                Exit Sub
            End If

            lngParcial = Mid$(pstrDatos, 34, 3)
            strTipo = Mid$(pstrDatos, 37, 1)
            strClaseOrden = Mid$(pstrDatos, 39, 1)
            'Con los TES viene en clase la D
            If strClaseOrden = "R" Or strClaseOrden = "D" Then
                strClaseOrden = "C" 'Liquidación de Renta Fija, C:Indica Crediticio
            Else
                strClaseOrden = "A" 'Liquidación de Acciones
            End If

            If strClaseOrden = "C" Then
                If Trim(strNemotecnico) = "" Then strNemotecnico = ValorParametro("LeerEspecieArchivoBolsaPrimero", "NO")

                If (strNemotecnico = "SI") And (Len(Trim(Mid$(pstrDatos, 616, 12))) >= 2) Then
                    strIDEspecie = Mid$(pstrDatos, 616, 12)
                Else : strIDEspecie = Mid$(pstrDatos, 40, 10)
                End If
            Else
                strIDEspecie = Mid$(pstrDatos, 40, 10)
            End If

            If strTipo = "C" Then
                lngIDComisionistaLocal = Mid$(pstrDatos, 53, 3)
            Else
                lngIDComisionistaLocal = Mid$(pstrDatos, 50, 3)
            End If

            If Not IsNumeric(Mid$(pstrDatos, 62, 18)) Then
                Call LogImportacion(pintCual, Mid$(pstrDatos, 62, 18), "Cantidad de la operación incorrecta")
                Exit Sub
            End If

            If Not IsNumeric(Mid$(pstrDatos, 299, 19)) Then
                Call LogImportacion(pintCual, Mid$(pstrDatos, 299, 19), "Cantidad de la transacción incorrecta")
                Exit Sub
            End If

            If Not VerificarSiEspecieExiste(gbtyIDBOLSA, strIDEspecie, logEsAccion) Then
                Call LogImportacion(pintCual, strIDEspecie, MSGERROR_ESPECIE & strIDEspecie & vbTab & MSGERROR_TABLA_ESPECIES)
                Exit Sub
            End If

            If lngParcial = 0 Then
                dblCantidad = Mid$(pstrDatos, 62, 18)
                curTransaccion = Mid$(pstrDatos, 108, 19)
            Else
                dblCantidad = Mid$(pstrDatos, 284, 15)
                curTransaccion = Mid$(pstrDatos, 299, 19)
            End If

            If Not IsNumeric(Mid$(pstrDatos, 80, 5)) Then
                Call LogImportacion(pintCual, Mid$(pstrDatos, 80, 5), "Días al vencimiento incorrecto")
                Exit Sub
            End If
            If strClaseOrden = "C" Then ' Crediticio / Renta Fija
                lngDiasVencimiento = Mid$(pstrDatos, 80, 5)
                curPrecio = Mid$(pstrDatos, 85, 7)
            Else
                curPrecio = Mid$(pstrDatos, 127, 13)
                lngDiasVencimiento = 0
            End If

            Dim strEmision As String

            strEmision = Mid$(pstrDatos, 140, 8)
            If strEmision <> "" And strEmision <> "00000000" Then
                dtmEmision = DateSerial(CInt(Mid$(strEmision, 1, 4)), CInt(Mid$(strEmision, 5, 2)), CInt(Mid$(strEmision, 7, 2)))
            Else
                dtmEmision = ValidaFecha("")
            End If

            Dim strVencimiento As String

            strVencimiento = Mid$(pstrDatos, 148, 8)
            If Not logEsAccion Then
                If strVencimiento <> "" And strVencimiento <> "00000000" Then
                    dtmVencimiento = DateSerial(CInt(Mid$(strVencimiento, 1, 4)), CInt(Mid$(strVencimiento, 5, 2)), CInt(Mid$(strVencimiento, 7, 2)))
                Else
                    dtmVencimiento = ValidaFecha("")
                End If
                If dtmVencimiento Is Nothing Then
                    Call LogImportacion(pintCual, strVencimiento, "Falta la fecha de Vencimiento")
                    Exit Sub
                ElseIf Not IsDate(dtmVencimiento) Then
                    Call LogImportacion(pintCual, dtmVencimiento, "La fecha de Vencimiento tiene un formato no válido")
                    Exit Sub
                End If
            End If

            lngPlazoOriginal = Mid$(pstrDatos, 156, 5)

            strIndicador = Mid$(pstrDatos, 161, 1)

            strBaseDias = Mid$(pstrDatos, 194, 1)

            dblPuntos = Mid$(pstrDatos, 162, 16)
            dblTasaDescuento = Mid$(pstrDatos, 178, 16)
            dblTasaEfectiva = Mid$(pstrDatos, 92, 16)
            strReinversion = Mid$(pstrDatos, 197, 1)
            lngDiasContado = IIf(Trim(Mid$(pstrDatos, 200, 3)) <> "", CType((Mid$(pstrDatos, 200, 3)), System.Nullable(Of Integer)), 0)


            strTipoDeOferta = IIf(Trim(Mid$(pstrDatos, 215, 1)) <> "", CType(Mid$(pstrDatos, 215, 1), System.Nullable(Of Char)), CChar("?"))
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

            strCumplimiento = Mid$(pstrDatos, 203, 8)
            dtmCumplimiento = DateSerial(CInt(Mid$(strCumplimiento, 1, 4)), CInt(Mid$(strCumplimiento, 5, 2)), CInt(Mid$(strCumplimiento, 7, 2)))
            If strClaseOrden = "C" Then   'CFMA20151120 
                strModalidad = Mid$(pstrDatos, 195, 1) & Mid$(pstrDatos, 196, 1)
                strMercado = Mid$(pstrDatos, 216, 1)
                If strModalidad = "PA" Then
                    strModalidad = "UA"
                ElseIf strModalidad = "NO" Then
                    strModalidad = "NO"
                Else
                    If strModalidad = "PV" Then
                        strModalidad = "UV"
                    End If
                End If   'CFMA20151120 
            Else
                strModalidad = ""
                strMercado = ""
            End If

            lngPlazo = IIf(Trim(strMercado) <> "", IIf(IsNumeric(Mid$(pstrDatos, 200, 3)), CType(Mid$(pstrDatos, 200, 3), System.Nullable(Of Integer)), 0), 0)
            strPosicionPropia = Mid$(pstrDatos, 214, 1)
            strRepoTitulo = Mid$(pstrDatos, 215, 1)
            If (strRepoTitulo = "R" Or strRepoTitulo = "A") Then

                If CDate(dtmLiquidacion) <> CDate(dtmCumplimiento) Then strTipo = IIf(strTipo = "C", "R", "S") 'Jairo Gonzalez 2007/06/20

                dblTasaEfectiva = Mid$(pstrDatos, 226, 16)
            End If

            strSwap = Mid$(pstrDatos, 217, 1)

            If strSwap = "S" Then lngNroSwap = Mid$(pstrDatos, 218, 8)
            'Estos valores no son actualizados
            strPlaza = "LOC"             '2001/05/25   Siempre seran locales con una unica Bolsa de valores
            logOtraPlaza = False         '2001/05/25   Siempre seran locales con una unica Bolsa de valores
            lngIDCiudadOtraPlaza = 585   '2001/05/25   Siempre seran locales con una unica Bolsa de valores
            bytIDRueda = 1               '2001/05/25   Siempre seran locales con una unica Bolsa de valores

            dblFactorComisionPactada = Mid$(pstrDatos, 318, 8)
            curComision = Mid$(pstrDatos, 326, 13)

            If dblPorcentajeIvaComision = -1 Then
                dblPorcentajeIvaComision = CampoTabla("1", "dblIvaComision", "tblinstalacion", "1")
                dblPorcentajeIvaComision = dblPorcentajeIvaComision / 100
            End If

            If IsNumeric(curComision) And curComision > 0 Then
                curValorIva = Format((dblPorcentajeIvaComision * curComision) - 0.005, "#,#.00")
            Else
                curValorIva = 0
            End If

            If Not IsNumeric(Mid$(pstrDatos, 346, 16)) Then
                dblTasaCompraVende = 0
            Else
                dblTasaCompraVende = Mid$(pstrDatos, 346, 16)
            End If

            If Not IsNumeric(Mid$(pstrDatos, 381, 2)) Then
                lngImpresiones = 0
            Else
                lngImpresiones = Mid$(pstrDatos, 381, 2)
            End If

            If Not IsNumeric(Mid$(pstrDatos, 383, 12)) Then
                dblValBolsa = 0
            Else
                dblValBolsa = Mid$(pstrDatos, 383, 12)
            End If
            curRetencion = 0
            dblValorTraslado = 0

            curRetencion = IIf(Mid$(pstrDatos, 497, 13) = "", 0, CType(Mid$(pstrDatos, 497, 13), System.Nullable(Of Double)))
            dblValorTraslado = IIf(Mid$(pstrDatos, 531, 13) = "", 0, CType(Mid$(pstrDatos, 531, 13), System.Nullable(Of Double))) 'En la posición 12 se evalua el signo

            If Not IsNumeric(Mid$(pstrDatos, 395, 12)) Then
                dblServBolsaVble = 0
            Else
                dblServBolsaVble = CType(Mid$(pstrDatos, 395, 12), System.Nullable(Of Double))
            End If

            strIndicadorAcciones = Mid$(pstrDatos, 564, 1)
            If strTipo = "C" Or strTipo = "R" Then
                curSubTotalLiq = curTransaccion + curComision + curValorIva
            Else
                curSubTotalLiq = curTransaccion - curComision - curValorIva
            End If
            If strTipo = "C" Or strTipo = "R" Or strMercado = "P" Then
                curTotalLiq = curSubTotalLiq + curRetencion '+ dblValorTraslado '+ IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
            Else
                'JAEZ 20170126 se le resta el  dblValorTraslado
                curTotalLiq = curSubTotalLiq + curRetencion '- dblValorTraslado '- IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
            End If

            'RBP20160725
            If curTotalLiq < 0 Then
                Call LogImportacion(pintCual, curTotalLiq, MSGERROR_VALORTOTAL & lngID & MSGERROR_TABLA_LIQUIDACIONES_VALORTOTAL)
                Exit Sub
            End If

            If Mid$(pstrDatos, 474, 1) = "S" Then
                logOrdinaria = False
                strObjetoOrdenExtraOrdinaria = STRCARRUSEL
                lngNumPadre = lngID
                lngParcialPadre = lngParcial
                dtmOperacionPadre = CDate(dtmLiquidacion)
            Else
                dtmOperacionPadre = ValidaFecha("")
            End If

            strTipoIdentificacion = Mid$(pstrDatos, 426, 1)

            If strTipoIdentificacion = "N" Then
                strNroDocumento = Trim(Mid$(pstrDatos, 427, 12))
            Else
                strNroDocumento = Trim(Mid$(pstrDatos, 427, 12))
            End If

            'strNroDocumento = ReemplazarcaracterEnCadena(strNroDocumento)

            If Not IsNumeric(Mid$(pstrDatos, 594, 19)) Then
                dblValorEntregaContraPago = 0
            Else
                dblValorEntregaContraPago = IIf(Mid$(pstrDatos, 594, 19) = "", 0, CType(Mid$(pstrDatos, 594, 19), System.Nullable(Of Double)))
            End If

            strAquienSeEnviaRetencion = Mid$(pstrDatos, 613, 1)

            If ExisteLiquidacion(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
                'Existe Exactamente la liquidación - con strTipo = " "
                Call LogImportacion(pintCual, strIDEspecie, MSGERROR_LIQUIDACION & lngID & MSGERROR_TABLA_LIQUIDACIONES)
                'Valido los campos que vienen contra los que existen
                Call Validar_Campo_a_Campo(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa)


                If IsNothing(rstConsultarDatosLiquidacion) Or rstConsultarDatosLiquidacion.Count = 0 Then
                    Exit Sub
                End If


                With rstConsultarDatosLiquidacion(0)

                    If IIf(IsNothing(.dtmVencimiento), Nothing, .dtmVencimiento) <> dtmVencimiento Then
                        Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
                    End If
                    If IIf(IsNothing(.dblServBolsaVble), Nothing, .dblServBolsaVble) <> dblServBolsaVble Then
                        Call LogDiferencias(.dblServBolsaVble, "Servicio Bolsa - Vlr en el Plano : " & dblServBolsaVble)
                    End If
                    If IIf(IsNothing(.curTotalLiq), Nothing, .curTotalLiq) <> curTotalLiq Then
                        Call LogDiferencias(.curTotalLiq, "Total operación - Vlr en el Plano : " & curTotalLiq)
                    End If
                    If IIf(IsNothing(.curTotalLiq), Nothing, .curTotalLiq) <> curTotalLiq Then
                        Call LogDiferencias(.curTotalLiq, "Total operación - Vlr en el Plano : " & curTotalLiq)
                    End If
                    If IIf(IsNothing(.dblValorTraslado), Nothing, .dblValorTraslado) <> dblValorTraslado Then
                        Call LogDiferencias(.dblValorTraslado, "Vlr Traslado - Vlr en el Plano : " & dblValorTraslado)
                    End If
                    If IIf(IsNothing(.curRetencion), Nothing, .curRetencion) <> curRetencion Then
                        Call LogDiferencias(.curRetencion, "Retención - Vlr en el Plano : " & curRetencion)
                    End If
                    If IIf(IsNothing(.dblValBolsa), Nothing, .dblValBolsa) <> dblValBolsa Then
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
                    If IIf(IsNothing(.dtmVencimiento), Nothing, .dtmVencimiento) <> dtmVencimiento Then
                        Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
                    End If
                    If IIf(IsNothing(.dtmEmision), Nothing, .dtmEmision) <> dtmEmision Then
                        Call LogDiferencias(.dtmEmision, "Emisión - Vlr en el Plano : " & dtmEmision)
                    End If
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
                If ExisteLiquidacion(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
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

            lngOrden = CDbl(CStr(Now.Year) & Format$(Mid$(pstrDatos, 418, 8), "000000"))

            Dim strOrden As String

            Dim strDepositoOrden As String = String.Empty 'Jorge Arango 2009/11/03 Ubicacion del titulo de la orden
            Dim strEstadoAprobOrden As String = String.Empty
            Dim strEstadoAprobCliente As String = String.Empty
            Dim strEstadoOrden As String = String.Empty

            'JAEZ 20170126 se valida si tiene datos en esa posicion
            If Not IsNothing(Mid$(pstrDatos, 418, 8)) And Mid$(pstrDatos, 418, 8).Trim <> "" Then

                strOrden = Now.Year.ToString & Format$(Val(Mid$(pstrDatos, 418, 8)), "000000")  ' AFG

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

            If strTipo = "R" And dblPatrimonioTecnicoFirma > 0 And ExisteOrden Then
                Call dblTotalValorFuturoRepo(fmtCodigo(lngIDComitente),
                                             dtmCumplimiento,
                                             curTotalLiq,
                                             dblTotalRecompras,
                                             dblTotalFuturoOrdenesRepos,
                                             dblTotalRecomprasImportadas,
                                             dblPatrimonioTecnicoFirma,
                                             dblSuperaPatrimonioTecnico)
                If dblSuperaPatrimonioTecnico > 0 Then
                    dblTotalFuturoRepo = dblTotalRecompras +
                                         dblTotalFuturoOrdenesRepos +
                                         dblTotalRecomprasImportadas +
                                         curTotalLiq
                    lngOrden = 0
                    lngIDComitente = "0"
                    lngIDOrdenante = "0"
                    Call LogImportacion(pintCual, strUBICACIONTITULO, "La orden # " & strOrden & " no puede ser asociada. El acumulado del cliente (" & Format(dblTotalFuturoRepo, "$,##,###.00") & ") supera el patrimonio técnico de la Firma.")
                End If
            End If


            strHoraGrabacion = Trim(Mid$(pstrDatos, 9, 8))
            strOrigenOperacion = Mid$(pstrDatos, 38, 1)
            lngCodigoOperadorCompra = Mid$(pstrDatos, 56, 3)
            lngCodigoOperadorVende = Mid$(pstrDatos, 59, 3)
            strIdentificacionRemate = Mid$(pstrDatos, 198, 2)
            strModalidaOperacion = Mid$(pstrDatos, 211, 1)
            strIndicadorPrecio = Mid$(pstrDatos, 212, 1)
            strPeriodoExdividendo = Mid$(pstrDatos, 213, 1)
            lngPlazoOperacionRepo = Mid$(pstrDatos, 242, 5)
            dblValorCaptacionRepo = IIf(Mid$(pstrDatos, 247, 15) = "", 0, CType(Mid$(pstrDatos, 247, 15), System.Nullable(Of Double)))
            lngVolumenCompraRepo = IIf(Mid$(pstrDatos, 262, 19) = "", 0, CType(Mid$(pstrDatos, 262, 19), System.Nullable(Of Double)))
            dblPrecioNetoFraccion = IIf(Mid$(pstrDatos, 339, 7) = "", 0, CType(Mid$(pstrDatos, 339, 7), System.Nullable(Of Double)))
            lngVolumenNetoFraccion = IIf(Mid$(pstrDatos, 362, 19) = "", 0, CType(Mid$(pstrDatos, 362, 19), System.Nullable(Of Double)))
            strCodigoContactoComercial = Mid$(pstrDatos, 407, 8) 'El campo pasó de ser numérico a string. CGA:  (16 de Junio 06).
            lngNroFraccionOperacion = CType(Mid$(pstrDatos, 415, 3), System.Nullable(Of Integer))
            strIdentificacionPatrimonio1 = Mid$(pstrDatos, 439, 3)   'espacios en blanco 7
            strTipoidentificacionCliente2 = Mid$(pstrDatos, 442, 1)
            strNitCliente2 = Mid$(pstrDatos, 443, 12)
            strIdentificacionPatrimonio2 = Mid$(pstrDatos, 455, 3)
            strTipoIdentificacionCliente3 = Mid$(pstrDatos, 458, 1)
            strNitCliente3 = Mid$(pstrDatos, 459, 12)
            strIdentificacionPatrimonio3 = Mid$(pstrDatos, 471, 3)
            strIndicadorOperacion = Mid$(pstrDatos, 475, 1)
            dblBaseRetencion = IIf(Mid$(pstrDatos, 476, 15) = "", 0, CType(Mid$(pstrDatos, 476, 15), System.Nullable(Of Double)))
            dblPorcRetencion = IIf(Mid$(pstrDatos, 491, 6) = "", 0, CType(Mid$(pstrDatos, 491, 6), System.Nullable(Of Double)))
            dblBaseRetencionTranslado = IIf(Mid$(pstrDatos, 510, 15) = "", 0, CType(Mid$(pstrDatos, 510, 15), System.Nullable(Of Double)))
            dblPorcRetencionTranslado = IIf(Mid$(pstrDatos, 525, 6) = "", 0, CType(Mid$(pstrDatos, 525, 6), System.Nullable(Of Double)))
            dblPorcIvaComision = IIf(Mid$(pstrDatos, 544, 7) = "", 0, CType(Mid$(pstrDatos, 544, 7), System.Nullable(Of Double)))
            strOperacionNegociada = Mid$(pstrDatos, 565, 1)
            Dim dtmFechaConstancia1 As String
            dtmFechaConstancia1 = Mid$(pstrDatos, 566, 8)
            'If dtmFechaConstancia1 <> "" And dtmFechaConstancia1 <> "00000000" Then
            If dtmFechaConstancia1 <> "" Then
                dtmFechaConstancia = DateSerial(CInt(Mid$(dtmFechaConstancia1, 1, 4)), CInt(Mid$(dtmFechaConstancia1, 5, 2)), CInt(Mid$(dtmFechaConstancia1, 7, 2)))
            Else
                dtmFechaConstancia = ValidaFecha("")
            End If
            dblValorConstancia = IIf(Mid$(pstrDatos, 574, 19) = "", 0, CType(Mid$(pstrDatos, 574, 19), System.Nullable(Of Double)))
            strGeneraConstancia = CType(Mid$(pstrDatos, 593, 1), System.Nullable(Of Char))
            lngImpresiones = CType(Mid$(pstrDatos, 381, 2), System.Nullable(Of Integer))

            curValorExtemporaneo = 0
            Try
                If Not IsNothing(Mid$(pstrDatos, 628, 19)) Then
                    If Not String.IsNullOrEmpty(Trim(Mid$(pstrDatos, 628, 19))) Then
                        curValorExtemporaneo = CType(Mid$(pstrDatos, 628, 19), System.Nullable(Of Double))
                    End If
                End If
            Catch ex As Exception

            End Try

            strPosicionExtemporaneo = String.Empty
            Try
                If Not IsNothing(Mid$(pstrDatos, 647, 1)) Then
                    If Not String.IsNullOrEmpty(Trim(Mid$(pstrDatos, 647, 1))) Then
                        strPosicionExtemporaneo = CType(Mid$(pstrDatos, 647, 1), String)
                    End If
                End If
            Catch ex As Exception

            End Try

            'Dim strContraparteCRCC As String = String.Empty
            Dim strEsReinversion As String = String.Empty
            Dim strContratoCuentaMargen As String = String.Empty

            If strTipoDeOferta = "5" Or strTipoDeOferta = "6" Then
                lngIDComisionistaOtraPlaza = -999999999
            End If

            'Try
            '    If Not IsNothing(Mid$(pstrDatos, 648, 4)) Then
            '        If Not String.IsNullOrEmpty(Trim(Mid$(pstrDatos, 648, 4))) Then
            '            strContraparteCRCC = CType(Mid$(pstrDatos, 648, 4), String)
            '            If strContraparteCRCC = "CRCC" Then
            '                lngIDComisionistaOtraPlaza = -999999999
            '            End If
            '        End If
            '    End If
            'Catch ex As Exception

            'End Try

            Try
                If Not IsNothing(Mid$(pstrDatos, 649, 1)) Then
                    If Not String.IsNullOrEmpty(Trim(Mid$(pstrDatos, 649, 1))) Then
                        strEsReinversion = CType(Mid$(pstrDatos, 649, 1), String)
                        If (strEsReinversion.ToUpper) <> "S" And (strEsReinversion.ToUpper) <> "N" Then
                            strEsReinversion = String.Empty
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try

            Try
                If Not IsNothing(Mid$(pstrDatos, 648, 1)) Then
                    If Not String.IsNullOrEmpty(Trim(Mid$(pstrDatos, 648, 1))) Then
                        strContratoCuentaMargen = CType(Mid$(pstrDatos, 648, 1), String)
                        If (strContratoCuentaMargen.ToUpper) <> "S" And (strContratoCuentaMargen.ToUpper) <> "N" Then
                            strContratoCuentaMargen = String.Empty
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try

            'JFSB 20170614
            Try
                If Not IsNothing(Mid$(pstrDatos, 650, 4)) Then
                    If Not String.IsNullOrEmpty(Mid$(pstrDatos, 650, 4)) Then
                        strRueda = CType(Mid$(pstrDatos, 650, 4), String)
                    End If
                Else
                    strRueda = String.Empty

                End If
            Catch ex As Exception

            End Try

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
                                                    strIDEspecie,
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
                                                    String.Empty,
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
                                                    strCodigoIntermediario,
                                                    DemeInfoSesion(pstrUsuario, "ExtraerDatosColombiaNuevo"),
                                                    ClsConstantes.GINT_ErrorPersonalizado,
                                                    curValorExtemporaneo,
                                                    strPosicionExtemporaneo,
                                                    strEsReinversion,
                                                    strContratoCuentaMargen,
                                                    strRueda)


            RegBuenos = RegBuenos + 1

        Catch ex As NullReferenceException

            ManejarError(ex, Me.ToString(), "ExtraerDatosColombiaNuevo")
        Catch A As Exception
            ManejarError(A, Me.ToString(), "ExtraerDatosColombiaNuevo")
        End Try

    End Sub
    Private Sub ExtraerDatosColombiaNuevoCompensacion(ByVal pintCual As Integer, ByVal pstrDatos As String, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal pstrUsuario As String, ByVal pbAcciones As Boolean, ByVal pbCrediticio As Boolean, ByVal pbAmbos As Boolean)
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Sub ExtraerDatosColombiaNuevo(pintCual As Integer, pstrDatos As String)
        '/* Alcance     :   Private
        '/* Descripción :   A partir de una hilera de caracteres separa en variables los datos
        '/*                 de una Liquidación de la Bolsa de Colombia, con previo conocimiento
        '/*                 de las posiciones donde empiezan y donde terminan el valor de cada
        '/*                 variable. Y luego las graba en tblImportacionLiq.
        '/*                 Mayo 8/2008 Luis Fernando Alvarez se actualiza dicho procedimiento por cambio en BVC para nro Liq de 9 digitos
        '/*                 En general el nro liquidacion queda de la siguiente manera xx-xxxxxxxx-xxxxxxxxx
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
        Try


            Dim logEsAccion As Boolean

            Dim strObjeto As String = String.Empty
            Dim ExisteOrden As Boolean
            Dim lngOrden As Long
            Dim dblTotalFuturoRepo As Double
            Dim dblTotalRecompras As Double
            Dim dblTotalRecomprasImportadas As Double
            Dim dblTotalFuturoOrdenesRepos As Double

            Dim lngID As System.Nullable(Of Integer),
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
                 dblTasaDescuento As System.Nullable(Of Double),
                 dblTasaCompraVende As System.Nullable(Of Double),
                 strModalidad As String,
                 strIndicador As String,
                 dblPuntos As System.Nullable(Of Double),
                 lngPlazo As System.Nullable(Of Integer),
                 dtmLiquidacion As System.Nullable(Of Date),
                 dtmCumplimiento As System.Nullable(Of Date),
                 dtmEmision As System.Nullable(Of Date),
                 dtmVencimiento As System.Nullable(Of Date),
                 logOtraPlaza As System.Nullable(Of Boolean),
                 strPlaza As String,
                 lngIDComisionistaLocal As System.Nullable(Of Integer),
                 lngIDComisionistaOtraPlaza As Integer,
                 lngIDCiudadOtraPlaza As System.Nullable(Of Integer),
                 dblTasaEfectiva As System.Nullable(Of Double),
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
                 lngPlazoOriginal As System.Nullable(Of Integer),
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
                 lngDiasContado As System.Nullable(Of Integer),
                 logOrdinaria As System.Nullable(Of Boolean) = False,
                 strObjetoOrdenExtraOrdinaria As String = String.Empty,
                 lngNumPadre As Integer,
                 lngParcialPadre As Integer,
                 dtmOperacionPadre As System.Nullable(Of Date),
                 lngDiasTramo As System.Nullable(Of Integer) = 0,
                 logVendido As System.Nullable(Of Boolean) = False,
                 dtmVendido As System.Nullable(Of Date) = Nothing,
                 dblValorTraslado As System.Nullable(Of Double),
                 dblValorBrutoCompraVencida As System.Nullable(Of Double) = 0,
                 strAutoRetenedor As String = String.Empty,
                 strSujeto As String = String.Empty,
                 dblPcRenEfecCompraRet As System.Nullable(Of Double) = 0,
                 dblPcRenEfecVendeRet As System.Nullable(Of Double) = 0,
                 strReinversion As String,
                 strSwap As String,
                 lngNroSwap As Integer,
                 strCertificacion As String = String.Empty,
                 dblDescuentoAcumula As System.Nullable(Of Double) = 0,
                 dblPctRendimiento As System.Nullable(Of Double) = 0,
                 dtmFechaCompraVencido As System.Nullable(Of Date) = Nothing,
                 dblPrecioCompraVencido As System.Nullable(Of Double) = 0,
                 strConstanciaEnajenacion As String = String.Empty,
                 strRepoTitulo As String,
                 strUsuario As String = pstrUsuario,
                 dblServBolsaVble As System.Nullable(Of Double),
                 dblServBolsaFijo As System.Nullable(Of Double),
                 strTraslado As String = String.Empty,
                 curValorIva As System.Nullable(Of Double),
                 strUBICACIONTITULO As String,
                 strTipoIdentificacion As System.Nullable(Of Char),
                 strNroDocumento As String,
                 dblValorEntregaContraPago As System.Nullable(Of Double),
                 strAquienSeEnviaRetencion As System.Nullable(Of Char),
                 strBaseDias As System.Nullable(Of Char),
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
                 dblPorcIvaComision As System.Nullable(Of Double),
                 strIndicadorAcciones As System.Nullable(Of Char),
                 strOperacionNegociada As System.Nullable(Of Char),
                 dtmFechaConstancia As System.Nullable(Of Date),
                 dblValorConstancia As System.Nullable(Of Double),
                 strGeneraConstancia As System.Nullable(Of Char),
                 strCodigoIntermediario As String = String.Empty,
                 strNemotecnico As String = String.Empty


            'lngIDBolsa = cmbvBolsas.ItemData(cmbvBolsas.ListIndex)
            Dim retorno = (L2SDC.uspA2utils_CargarCombos("UBICACIONTITULO_EXTERIOR", pstrUsuario.ToString)).ToList

            lngIDBolsa = 4
            gbtyIDBOLSA = 4

            strUBICACIONTITULO = UCase(Mid$(pstrDatos, 281, 3))
            Select Case strUBICACIONTITULO
                Case Is <> Nothing
                    Dim strUBICACIONTITULO2 = retorno.Where(Function(i) i.ID = strUBICACIONTITULO.ToString()).Select(Function(i) i.Descripcion).FirstOrDefault

                    strUBICACIONTITULO = strUBICACIONTITULO2

                    If String.IsNullOrEmpty(strUBICACIONTITULO) Then
                        strUBICACIONTITULO = STR_FISICO
                    End If

                Case Else
                    strUBICACIONTITULO = STR_FISICO
            End Select
            'Select Case strUBICACIONTITULO
            '    Case "DVL"
            '        strUBICACIONTITULO = STR_DECEVAL
            '    Case "DCV"
            '        strUBICACIONTITULO = STR_DCV
            '    Case "EU$"
            '        strUBICACIONTITULO = STR_EU
            '    Case "CE$"
            '        strUBICACIONTITULO = STR_EU
            '    Case Else
            '        strUBICACIONTITULO = STR_FISICO
            'End Select

            'La fecha de liquidacion no cambia porque el nro de liquidacion inicia en la posicion 17
            dtmLiquidacion = DateSerial(CInt(Mid$(pstrDatos, 1, 4)), CInt(Mid$(pstrDatos, 5, 2)), CInt(Mid$(pstrDatos, 7, 2)))
            If Not IsDate(dtmLiquidacion) Then
                Call LogImportacion(pintCual, dtmLiquidacion, "La fecha de liquidación tiene un formato no válido")
                Exit Sub
            End If

            If ComparaFechas(dtmLiquidacion, pDateDesde, "<") Or
                ComparaFechas(dtmLiquidacion, pDateHasta, ">") Then
                Call LogImportacion(pintCual, Mid$(pstrDatos, 1, 8), MSGERROR_RANGO_FECHAS)
                Exit Sub
            End If

            'If Not IsNumeric(Right(Mid$(pstrDatos, 17, 17), 9)) Then
            '    Call LogImportacion(pintCual, Right(Mid$(pstrDatos, 17, 17), 9), "Número de operación incorrecto")
            '    Exit Sub
            'End If

            lngID = Right(Mid$(pstrDatos, 17, 17), 9) 'Número de la liquidación

            'If Not IsNumeric(Mid$(pstrDatos, 34, 3)) Then
            '    Call LogImportacion(pintCual, Mid$(pstrDatos, 34, 3), "Número del parcial incorrecto")
            '    Exit Sub
            'End If

            lngParcial = Mid$(pstrDatos, 34, 3)
            strTipo = Mid$(pstrDatos, 37, 1)
            strClaseOrden = Mid$(pstrDatos, 39, 1)
            'Con los TES viene en clase la D
            If strClaseOrden = "R" Or strClaseOrden = "D" Then
                strClaseOrden = "C" 'Liquidación de Renta Fija, C:Indica Crediticio
            Else
                strClaseOrden = "A" 'Liquidación de Acciones
            End If

            If pbAcciones And strClaseOrden <> "A" Then
                Exit Sub
            ElseIf pbCrediticio And strClaseOrden <> "C" Then
                Exit Sub
            End If

            If strClaseOrden = "C" Then
                If Trim(strNemotecnico) = "" Then strNemotecnico = ValorParametro("LeerEspecieArchivoBolsaPrimero", "NO")

                If (strNemotecnico = "SI") And (Len(Trim(Mid$(pstrDatos, 616, 12))) >= 2) Then
                    strIDEspecie = Mid$(pstrDatos, 616, 12)
                Else : strIDEspecie = Mid$(pstrDatos, 40, 10)
                End If
            Else
                strIDEspecie = Mid$(pstrDatos, 40, 10)
            End If

            If strTipo = "C" Then
                lngIDComisionistaLocal = Mid$(pstrDatos, 53, 3)
            Else
                lngIDComisionistaLocal = Mid$(pstrDatos, 50, 3)
            End If

            'If Not IsNumeric(Mid$(pstrDatos, 62, 18)) Then
            '    Call LogImportacion(pintCual, Mid$(pstrDatos, 62, 18), "Cantidad de la operación incorrecta")
            '    Exit Sub
            'End If

            'If Not IsNumeric(Mid$(pstrDatos, 299, 19)) Then
            '    Call LogImportacion(pintCual, Mid$(pstrDatos, 299, 19), "Cantidad de la transacción incorrecta")
            '    Exit Sub
            'End If

            If Not VerificarSiEspecieExiste(gbtyIDBOLSA, strIDEspecie, logEsAccion) Then
                Call LogImportacion(pintCual, strIDEspecie, MSGERROR_ESPECIE & strIDEspecie & vbTab & MSGERROR_TABLA_ESPECIES)
                Exit Sub
            End If

            If lngParcial = 0 Then
                dblCantidad = Mid$(pstrDatos, 62, 18)
                curTransaccion = Mid$(pstrDatos, 108, 19)
            Else
                dblCantidad = Mid$(pstrDatos, 284, 15)
                curTransaccion = Mid$(pstrDatos, 299, 19)
            End If

            'If Not IsNumeric(Mid$(pstrDatos, 80, 5)) Then
            '    Call LogImportacion(pintCual, Mid$(pstrDatos, 80, 5), "Días al vencimiento incorrecto")
            '    Exit Sub
            'End If
            If strClaseOrden = "C" Then ' Crediticio / Renta Fija
                lngDiasVencimiento = Mid$(pstrDatos, 76, 5)
                curPrecio = Mid$(pstrDatos, 85, 7)
            Else
                curPrecio = Mid$(pstrDatos, 127, 13)
                lngDiasVencimiento = 0
            End If

            Dim strEmision As String

            strEmision = Mid$(pstrDatos, 140, 8)
            If strEmision <> "" And strEmision <> "00000000" Then
                dtmEmision = DateSerial(CInt(Mid$(strEmision, 1, 4)), CInt(Mid$(strEmision, 5, 2)), CInt(Mid$(strEmision, 7, 2)))
            Else
                dtmEmision = ValidaFecha("")
            End If

            Dim strVencimiento As String

            strVencimiento = Mid$(pstrDatos, 148, 8)
            If Not logEsAccion Then
                If strVencimiento <> "" And strVencimiento <> "00000000" Then
                    dtmVencimiento = DateSerial(CInt(Mid$(strVencimiento, 1, 4)), CInt(Mid$(strVencimiento, 5, 2)), CInt(Mid$(strVencimiento, 7, 2)))
                Else
                    dtmVencimiento = ValidaFecha("")
                End If
                If dtmVencimiento Is Nothing Then
                    Call LogImportacion(pintCual, strVencimiento, "Falta la fecha de Vencimiento")
                    Exit Sub
                ElseIf Not IsDate(dtmVencimiento) Then
                    Call LogImportacion(pintCual, dtmVencimiento, "La fecha de Vencimiento tiene un formato no válido")
                    Exit Sub
                End If
            End If



            lngPlazoOriginal = Mid$(pstrDatos, 156, 5)

            strIndicador = Mid$(pstrDatos, 161, 1)

            strBaseDias = Mid$(pstrDatos, 194, 1)

            dblPuntos = Mid$(pstrDatos, 162, 16)
            dblTasaDescuento = Mid$(pstrDatos, 178, 16)
            dblTasaEfectiva = Mid$(pstrDatos, 92, 16)
            strReinversion = Mid$(pstrDatos, 197, 1)
            lngDiasContado = IIf(Trim(Mid$(pstrDatos, 200, 3)) <> "", CType((Mid$(pstrDatos, 200, 3)), System.Nullable(Of Integer)), 0)


            strTipoDeOferta = IIf(Trim(Mid$(pstrDatos, 215, 1)) <> "", CType(Mid$(pstrDatos, 215, 1), System.Nullable(Of Char)), CChar("?"))
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

            strCumplimiento = Mid$(pstrDatos, 203, 8)
            dtmCumplimiento = DateSerial(CInt(Mid$(strCumplimiento, 1, 4)), CInt(Mid$(strCumplimiento, 5, 2)), CInt(Mid$(strCumplimiento, 7, 2)))
            If strClaseOrden = "C" Then
                strModalidad = Mid$(pstrDatos, 195, 1) & Mid$(pstrDatos, 196, 1)
                strMercado = Mid$(pstrDatos, 216, 1)
                If strModalidad = "PA" Then
                    strModalidad = "UA"
                ElseIf strModalidad = "NO" Then
                    strModalidad = "NO"
                Else
                    If strModalidad = "PV" Or Trim(strModalidad) = "" Or IsNothing(strModalidad) Then
                        strModalidad = "UV"
                    End If
                End If
            Else
                strModalidad = ""
                strMercado = ""
            End If

            lngPlazo = IIf(Trim(strMercado) <> "", IIf(IsNumeric(Mid$(pstrDatos, 200, 3)), CType(Mid$(pstrDatos, 200, 3), System.Nullable(Of Integer)), 0), 0)
            strPosicionPropia = Mid$(pstrDatos, 214, 1)
            strRepoTitulo = Mid$(pstrDatos, 215, 1)
            If (strRepoTitulo = "R" Or strRepoTitulo = "A") Then

                If CDate(dtmLiquidacion) <> CDate(dtmCumplimiento) Then strTipo = IIf(strTipo = "C", "R", "S") 'Jairo Gonzalez 2007/06/20

                dblTasaEfectiva = Mid$(pstrDatos, 226, 16)
            End If

            strSwap = Mid$(pstrDatos, 217, 1)

            If strSwap = "S" Then lngNroSwap = Mid$(pstrDatos, 218, 8)
            'Estos valores no son actualizados
            strPlaza = "LOC"             '2001/05/25   Siempre seran locales con una unica Bolsa de valores
            logOtraPlaza = False         '2001/05/25   Siempre seran locales con una unica Bolsa de valores
            lngIDCiudadOtraPlaza = 585   '2001/05/25   Siempre seran locales con una unica Bolsa de valores
            bytIDRueda = 1               '2001/05/25   Siempre seran locales con una unica Bolsa de valores

            dblFactorComisionPactada = Mid$(pstrDatos, 318, 8)
            curComision = Mid$(pstrDatos, 326, 13)

            If dblPorcentajeIvaComision = -1 Then
                dblPorcentajeIvaComision = CampoTabla("1", "dblIvaComision", "tblinstalacion", "1")
                dblPorcentajeIvaComision = dblPorcentajeIvaComision / 100
            End If

            If IsNumeric(curComision) And curComision > 0 Then
                curValorIva = Format((dblPorcentajeIvaComision * curComision) - 0.005, "#,#.00")
            Else
                curValorIva = 0
            End If

            If Not IsNumeric(Mid$(pstrDatos, 346, 16)) Then
                dblTasaCompraVende = 0
            Else
                dblTasaCompraVende = Mid$(pstrDatos, 346, 16)
            End If

            If Not IsNumeric(Mid$(pstrDatos, 381, 2)) Then
                lngImpresiones = 0
            Else
                lngImpresiones = Mid$(pstrDatos, 381, 2)
            End If

            If Not IsNumeric(Mid$(pstrDatos, 383, 12)) Then
                dblValBolsa = 0
            Else
                dblValBolsa = Mid$(pstrDatos, 383, 12)
            End If
            curRetencion = 0
            dblValorTraslado = 0

            curRetencion = IIf(Mid$(pstrDatos, 497, 13) = "", 0, CType(Mid$(pstrDatos, 497, 13), System.Nullable(Of Double)))
            dblValorTraslado = IIf(Mid$(pstrDatos, 531, 13) = "", 0, CType(Mid$(pstrDatos, 531, 13), System.Nullable(Of Double))) 'En la posición 12 se evalua el signo

            If Not IsNumeric(Mid$(pstrDatos, 395, 12)) Then
                dblServBolsaVble = 0
            Else
                dblServBolsaVble = CType(Mid$(pstrDatos, 395, 12), System.Nullable(Of Double))
            End If

            strIndicadorAcciones = Mid$(pstrDatos, 564, 1)
            If strTipo = "C" Or strTipo = "R" Then
                curSubTotalLiq = curTransaccion + curComision + curValorIva
            Else
                curSubTotalLiq = curTransaccion - curComision - curValorIva
            End If
            If strTipo = "C" Or strTipo = "R" Or strMercado = "P" Then
                curTotalLiq = curSubTotalLiq + curRetencion '+ dblValorTraslado '+ IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
            Else
                'JAEZ 20170126 se le resta el  dblValorTraslado
                curTotalLiq = curSubTotalLiq + curRetencion '- dblValorTraslado '- IIf(strIndicadorAcciones = "S", dblServBolsaVble, 0) 'Pendiente de verificar con la Bolsa si ya viene el campo con S y correponde a quitar el servicio de bolsa en las operaciones
            End If

            If Mid$(pstrDatos, 474, 1) = "S" Then
                logOrdinaria = False
                strObjetoOrdenExtraOrdinaria = STRCARRUSEL
                lngNumPadre = lngID
                lngParcialPadre = lngParcial
                dtmOperacionPadre = CDate(dtmLiquidacion)
            Else
                dtmOperacionPadre = ValidaFecha("")
            End If

            strTipoIdentificacion = Mid$(pstrDatos, 426, 1)

            If strTipoIdentificacion = "N" Then
                strNroDocumento = Trim(Mid$(pstrDatos, 427, 12))
            Else
                strNroDocumento = Trim(Mid$(pstrDatos, 427, 12))
            End If

            strNroDocumento = ReemplazarcaracterEnCadena(strNroDocumento)

            If Not IsNumeric(Mid$(pstrDatos, 594, 19)) Then
                dblValorEntregaContraPago = 0
            Else
                dblValorEntregaContraPago = IIf(Mid$(pstrDatos, 594, 19) = "", 0, CType(Mid$(pstrDatos, 594, 19), System.Nullable(Of Double)))
            End If

            strAquienSeEnviaRetencion = Mid$(pstrDatos, 613, 1)
            If Not logEsAccion Then
                strVencimiento = dtmVencimiento
            Else
                strVencimiento = String.Empty
            End If
            'If ExisteLiquidacion(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
            '    'Existe Exactamente la liquidación - con strTipo = " "
            '    Call LogImportacion(pintCual, strIDEspecie, MSGERROR_LIQUIDACION & lngID & MSGERROR_TABLA_LIQUIDACIONES)
            '    'Valido los campos que vienen contra los que existen
            '    Call Validar_Campo_a_Campo(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa)


            '    If IsNothing(rstConsultarDatosLiquidacion) Or rstConsultarDatosLiquidacion.Count = 0 Then
            '        Exit Sub
            '    End If


            '    With rstConsultarDatosLiquidacion(0)

            '        If IIf(IsNothing(.dtmVencimiento), Nothing, .dtmVencimiento) <> dtmVencimiento Then
            '            Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
            '        End If
            '        If IIf(IsNothing(.dblServBolsaVble), Nothing, .dblServBolsaVble) <> dblServBolsaVble Then
            '            Call LogDiferencias(.dblServBolsaVble, "Servicio Bolsa - Vlr en el Plano : " & dblServBolsaVble)
            '        End If
            '        If IIf(IsNothing(.curTotalLiq), Nothing, .curTotalLiq) <> curTotalLiq Then
            '            Call LogDiferencias(.curTotalLiq, "Total operación - Vlr en el Plano : " & curTotalLiq)
            '        End If
            '        If IIf(IsNothing(.dblValorTraslado), Nothing, .dblValorTraslado) <> dblValorTraslado Then
            '            Call LogDiferencias(.dblValorTraslado, "Vlr Traslado - Vlr en el Plano : " & dblValorTraslado)
            '        End If
            '        If IIf(IsNothing(.curRetencion), Nothing, .curRetencion) <> curRetencion Then
            '            Call LogDiferencias(.curRetencion, "Retención - Vlr en el Plano : " & curRetencion)
            '        End If
            '        If IIf(IsNothing(.dblValBolsa), Nothing, .dblValBolsa) <> dblValBolsa Then
            '            Call LogDiferencias(.dblValBolsa, "Servicio Bolsa - Vlr en el Plano : " & dblValBolsa)
            '        End If
            '        If .dblTasaCompraVende <> dblTasaCompraVende Then
            '            Call LogDiferencias(.dblTasaCompraVende, "Tasa Neta Fracción - Vlr en el Plano : " & dblTasaCompraVende)
            '        End If
            '        If .curValorIva <> curValorIva Then
            '            Call LogDiferencias(.curValorIva, "IVA Comisión - Vlr en el Plano : " & curValorIva)
            '        End If
            '        If .curComision <> curComision Then
            '            Call LogDiferencias(.curComision, "Comisión - Vlr en el Plano : " & curComision)
            '        End If
            '        If .dblFactorComisionPactada <> dblFactorComisionPactada Then
            '            Call LogDiferencias(.strModalidad, "Modalidad - Vlr en el Plano : " & strModalidad)
            '        End If
            '        If (.strModalidad <> strModalidad) And logEsAccion = False Then
            '            Call LogDiferencias(.strModalidad, "Modalidad - Vlr en el Plano : " & strModalidad)
            '        End If
            '        If .dtmCumplimiento <> dtmCumplimiento Then
            '            Call LogDiferencias(.dtmCumplimiento, "Fecha Cumplimiento - Vlr en el Plano : " & dtmCumplimiento)
            '        End If
            '        If .strTipoDeOferta <> strTipoDeOferta Then
            '            Call LogDiferencias(.strTipoDeOferta, "Tipo Oferta - Vlr en el Plano : " & strTipoDeOferta)
            '        End If
            '        If .dblTasaEfectiva <> dblTasaEfectiva Then
            '            Call LogDiferencias(.dblTasaEfectiva, "Tasa Efectiva - Vlr en el Plano : " & dblTasaEfectiva)
            '        End If
            '        If .dblPuntosIndicador <> dblPuntos Then
            '            Call LogDiferencias(.dblPuntosIndicador, "Puntos sobre el Indicador - Vlr en el Plano : " & dblPuntos)
            '        End If
            '        If .strIndicadorEconomico <> strIndicador Then
            '            Call LogDiferencias(.strIndicadorEconomico, "Indicador Economico - Vlr en el Plano : " & strIndicador)
            '        End If
            '        If .lngPlazoOriginal <> lngPlazoOriginal Then
            '            Call LogDiferencias(.lngPlazoOriginal, "Plazo - Vlr en el Plano : " & lngPlazoOriginal)
            '        End If
            '        If IIf(IsNothing(.dtmVencimiento), Nothing, .dtmVencimiento) <> dtmVencimiento Then
            '            Call LogDiferencias(.dtmVencimiento, "Vencimiento - Vlr en el Plano : " & dtmVencimiento)
            '        End If
            '        If IIf(IsNothing(.dtmEmision), Nothing, .dtmEmision) <> dtmEmision Then
            '            Call LogDiferencias(.dtmEmision, "Emisión - Vlr en el Plano : " & dtmEmision)
            '        End If
            '        If .curPrecio <> curPrecio Then
            '            Call LogDiferencias(.curPrecio, "Precio - Vlr en el Plano : " & curPrecio)
            '        End If
            '        If .lngDiasVencimiento <> lngDiasVencimiento Then
            '            Call LogDiferencias(.lngDiasVencimiento, "Dias al Vencimiento - Vlr en el Plano : " & lngDiasVencimiento)
            '        End If
            '        If .curTransaccion <> curTransaccion Then
            '            Call LogDiferencias(.curTransaccion, "Transacción - Vlr en el Plano : " & curTransaccion)
            '        End If
            '        If .dblCantidad <> dblCantidad Then
            '            Call LogDiferencias(.dblCantidad, "Cantidad - Vlr en el Plano : " & dblCantidad)
            '        End If
            '        If .dblTasaDescuento <> dblTasaDescuento Then
            '            Call LogDiferencias(.dblTasaDescuento, "Tasa Nominal - Vlr en el Plano : " & dblTasaDescuento)
            '        End If

            '    End With

            '    Exit Sub
            'Else
            '    If ExisteLiquidacion(CDate(dtmLiquidacion), lngID, lngParcial, strTipo, strClaseOrden, lngIDBolsa) Then
            '        If strTipo = "C" Then
            '            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_COMPRA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
            '        ElseIf strTipo = "V" Then
            '            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_VENTA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
            '        ElseIf strTipo = "R" Then
            '            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_RECOMPRA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
            '        ElseIf strTipo = "S" Then
            '            Call LogImportacion(pintCual, strIDEspecie, MSGERROR_EXISTEN_LIQUIDACIONES_REVENTA & lngID & MSGERROR_PARCIAL & lngParcial & "'")
            '        End If
            '        Exit Sub
            '    End If
            'End If



            lngOrden = 0
            lngIDComitente = "0"
            lngIDOrdenante = "0"

            lngOrden = CDbl(CStr(Now.Year) & Format$(Mid$(pstrDatos, 414, 8), "000000"))

            Dim strOrden As String

            Dim strDepositoOrden As String = String.Empty 'Jorge Arango 2009/11/03 Ubicacion del titulo de la orden
            Dim strEstadoAprobOrden As String = String.Empty
            Dim strEstadoAprobCliente As String = String.Empty
            Dim strEstadoOrden As String = String.Empty

            'JAEZ 20170126 se valida si tiene datos en esa posicion
            If Not IsNothing(Mid$(pstrDatos, 418, 8)) And Mid$(pstrDatos, 418, 8) <> "" Then

                strOrden = Now.Year.ToString & Format$(Val(Mid$(pstrDatos, 418, 8)), "000000")  ' AFG

                If IsNumeric(strOrden) And Len(Trim(strOrden)) = 10 Then
                    lngOrden = CLng(strOrden)
                    If strTipoDeOferta = "2" Or strTipoDeOferta = "4" Or ((strTipoDeOferta = "R" Or strTipoDeOferta = "A") And (strTipo = "R" Or strTipo = "S")) Then
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
            End If


            If strTipo = "R" And dblPatrimonioTecnicoFirma > 0 And ExisteOrden Then
                Call dblTotalValorFuturoRepo(fmtCodigo(lngIDComitente),
                                             dtmCumplimiento,
                                             curTotalLiq,
                                             dblTotalRecompras,
                                             dblTotalFuturoOrdenesRepos,
                                             dblTotalRecomprasImportadas,
                                             dblPatrimonioTecnicoFirma,
                                             dblSuperaPatrimonioTecnico)
                If dblSuperaPatrimonioTecnico > 0 Then
                    dblTotalFuturoRepo = dblTotalRecompras +
                                         dblTotalFuturoOrdenesRepos +
                                         dblTotalRecomprasImportadas +
                                         curTotalLiq
                    lngOrden = 0
                    lngIDComitente = "0"
                    lngIDOrdenante = "0"
                    Call LogImportacion(pintCual, strUBICACIONTITULO, "La orden # " & strOrden & " no puede ser asociada. El acumulado del cliente (" & Format(dblTotalFuturoRepo, "$,##,###.00") & ") supera el patrimonio técnico de la Firma.")
                End If
            End If


            strHoraGrabacion = Trim(Mid$(pstrDatos, 9, 8))
            strOrigenOperacion = Mid$(pstrDatos, 38, 1)
            lngCodigoOperadorCompra = Mid$(pstrDatos, 56, 3)
            lngCodigoOperadorVende = Mid$(pstrDatos, 59, 3)
            strIdentificacionRemate = Mid$(pstrDatos, 198, 2)
            strModalidaOperacion = Mid$(pstrDatos, 211, 1)
            strIndicadorPrecio = Mid$(pstrDatos, 212, 1)
            strPeriodoExdividendo = Mid$(pstrDatos, 213, 1)
            lngPlazoOperacionRepo = Mid$(pstrDatos, 242, 5)
            dblValorCaptacionRepo = IIf(Mid$(pstrDatos, 247, 15) = "", 0, CType(Mid$(pstrDatos, 247, 15), System.Nullable(Of Double)))
            lngVolumenCompraRepo = IIf(Mid$(pstrDatos, 262, 19) = "", 0, CType(Mid$(pstrDatos, 262, 19), System.Nullable(Of Double)))
            dblPrecioNetoFraccion = IIf(Mid$(pstrDatos, 339, 7) = "", 0, CType(Mid$(pstrDatos, 339, 7), System.Nullable(Of Double)))
            lngVolumenNetoFraccion = IIf(Mid$(pstrDatos, 362, 19) = "", 0, CType(Mid$(pstrDatos, 362, 19), System.Nullable(Of Double)))
            strCodigoContactoComercial = Mid$(pstrDatos, 407, 8) 'El campo pasó de ser numérico a string. CGA:  (16 de Junio 06).
            lngNroFraccionOperacion = CType(Mid$(pstrDatos, 415, 3), System.Nullable(Of Integer))
            strIdentificacionPatrimonio1 = Mid$(pstrDatos, 439, 3)   'espacios en blanco 7
            strTipoidentificacionCliente2 = Mid$(pstrDatos, 442, 1)
            strNitCliente2 = Mid$(pstrDatos, 443, 12)
            strIdentificacionPatrimonio2 = Mid$(pstrDatos, 455, 3)
            strTipoIdentificacionCliente3 = Mid$(pstrDatos, 458, 1)
            strNitCliente3 = Mid$(pstrDatos, 459, 12)
            strIdentificacionPatrimonio3 = Mid$(pstrDatos, 471, 3)
            strIndicadorOperacion = Mid$(pstrDatos, 475, 1)
            dblBaseRetencion = IIf(Mid$(pstrDatos, 476, 15) = "", 0, CType(Mid$(pstrDatos, 476, 15), System.Nullable(Of Double)))
            dblPorcRetencion = IIf(Mid$(pstrDatos, 491, 6) = "", 0, CType(Mid$(pstrDatos, 491, 6), System.Nullable(Of Double)))
            dblBaseRetencionTranslado = IIf(Mid$(pstrDatos, 510, 15) = "", 0, CType(Mid$(pstrDatos, 510, 15), System.Nullable(Of Double)))
            dblPorcRetencionTranslado = IIf(Mid$(pstrDatos, 525, 6) = "", 0, CType(Mid$(pstrDatos, 525, 6), System.Nullable(Of Double)))
            dblPorcIvaComision = IIf(Mid$(pstrDatos, 544, 7) = "", 0, CType(Mid$(pstrDatos, 544, 7), System.Nullable(Of Double)))
            strOperacionNegociada = Mid$(pstrDatos, 565, 1)
            Dim dtmFechaConstancia1 As String
            dtmFechaConstancia1 = Mid$(pstrDatos, 566, 8)
            'If dtmFechaConstancia1 <> "" And dtmFechaConstancia1 <> "00000000" Then
            If dtmFechaConstancia1 <> "" Then
                dtmFechaConstancia = DateSerial(CInt(Mid$(dtmFechaConstancia1, 1, 4)), CInt(Mid$(dtmFechaConstancia1, 5, 2)), CInt(Mid$(dtmFechaConstancia1, 7, 2)))
            Else
                dtmFechaConstancia = ValidaFecha("")
            End If
            dblValorConstancia = IIf(Mid$(pstrDatos, 574, 19) = "", 0, CType(Mid$(pstrDatos, 574, 19), System.Nullable(Of Double)))
            strGeneraConstancia = CType(Mid$(pstrDatos, 593, 1), System.Nullable(Of Char))
            lngImpresiones = CType(Mid$(pstrDatos, 381, 2), System.Nullable(Of Integer))


            Dim ret = L2SDC.uspOyDNet_Importaciones_GrabarLiquidacionesCompensacion(lngID,
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
                                                    strIDEspecie,
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
                                                    String.Empty,
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
                                                    strCodigoIntermediario,
                                                    DemeInfoSesion(pstrUsuario, "ExtraerDatosColombiaNuevoCompensacion"),
                                                    ClsConstantes.GINT_ErrorPersonalizado)


            RegBuenos = RegBuenos + 1

        Catch ex As NullReferenceException

            ManejarError(ex, Me.ToString(), "ExtraerDatosColombiaNuevoCompensacion")
        Catch A As Exception
            ManejarError(A, Me.ToString(), "ExtraerDatosColombiaNuevoCompensacion")
        End Try

    End Sub

    Private Function ValidaFecha(pstrFechaArchivo As String) As System.Nullable(Of Date)
        Dim dtmRetorno As System.Nullable(Of Date) = Nothing
        If Not String.IsNullOrEmpty(pstrFechaArchivo) Then
            dtmRetorno = CDate(pstrFechaArchivo)
        End If
        Return dtmRetorno
    End Function

    Public Function EjecutaprocesoServicioBolsa(ByVal pNombreEncabezado As String, ByVal pDateDesde As DateTime, ByVal pDateHasta As DateTime, ByVal ReemplazaValor As Boolean, ByVal strusuario As String) As String
        '     '/******************************************************************************************
        '     '/* INICIO DOCUMENTO
        '     '/* Sub EjecutaprocesoServicioBolsa()
        '     '/* Alcance     :   Private
        '     '/* Descripción :   Ejecuta los procesos que hacen que la importación de los servicios de bolsa
        '     '/* Parámetros  :
        '     '/* Valores de retorno:
        '     '/* FIN DOCUMENTO
        '     '/******************************************************************************************
        'Dim sb As New StringBuilder
        Dim objReader As System.IO.StreamReader = Nothing
        Try
            Dim intCual As Integer

            Dim strlinea As String

            Dim FILE_NAME As String = pNombreEncabezado
            'Abrimos el archivo de encabezados
            objReader = New System.IO.StreamReader(FILE_NAME)

            Dim aText() As String, sExt As String
            aText = Split(FILE_NAME, ".")
            sExt = aText(UBound(aText))

            If objReader.Peek() = -1 Then
                Return "El archivo está vacio."
            ElseIf Not objReader.ReadLine().Contains(";") Then
                Return "La estructura del archivo no corresponde."
            ElseIf sExt <> "txt" Then
                Return "El formato del archivo no corresponde."
            End If

            objReader.Close()


            'Abrimos el archivo de encabezados
            objReader = New System.IO.StreamReader(FILE_NAME)
            intCual = 1
            RegBuenos = 0
            'Recorremos el archivo de encabezados

            Do While objReader.Peek <> -1
                strlinea = objReader.ReadLine()
                Dim strdatos = Split(strlinea, ";")
                If Not strlinea = String.Empty Then
                    Dim DsctoServBolsaFijo As Double = 0
                    Dim DsctoServBolsaVble As Double = 0
                    Dim DsctoTotalServBolsa As Double = 0
                    Dim curValorExtemporaneo As Double = 0
                    Dim strPosicionExtemporaneo As String

                    Try
                        If Not IsNothing(strdatos(12)) Then
                            If Not String.IsNullOrEmpty(Trim(strdatos(12))) Then
                                DsctoServBolsaFijo = CDbl(Trim(strdatos(12).Replace(".", "").Replace(",", ".")))
                            End If
                        End If
                    Catch ex As Exception

                    End Try


                    Try
                        If Not IsNothing(strdatos(13)) Then
                            If Not String.IsNullOrEmpty(Trim(strdatos(13))) Then
                                DsctoServBolsaVble = CDbl(Trim(strdatos(13).Replace(".", "").Replace(",", ".")))
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    Try
                        If Not IsNothing(strdatos(14)) Then
                            If Not String.IsNullOrEmpty(Trim(strdatos(14))) Then
                                DsctoTotalServBolsa = CDbl(Trim(strdatos(14).Replace(".", "").Replace(",", ".")))
                            End If
                        End If
                    Catch ex As Exception

                    End Try

                    Dim ret = L2SDC.usp_InsertarServicioBolsa(strdatos(0),
                    strdatos(1),
                    strdatos(2),
                    strdatos(3),
                    strdatos(4),
                    CDate(Mid(strdatos(5), 1, 4) & "/" & Mid(strdatos(5), 5, 2) & "/" & Mid(strdatos(5), 7, 2)),
                    CDate(Mid(strdatos(6), 1, 4) & "/" & Mid(strdatos(6), 5, 2) & "/" & Mid(strdatos(6), 7, 2)),
                    strdatos(7),
                    CDbl(Trim(strdatos(8).Replace(".", "").Replace(",", "."))),
                    CDbl(Trim(strdatos(9).Replace(".", "").Replace(",", "."))),
                    CDbl(Trim(strdatos(10).Replace(".", "").Replace(",", "."))),
                    CDbl(Trim(strdatos(11).Replace(".", "").Replace(",", "."))),
                    DsctoServBolsaFijo,
                    DsctoServBolsaVble,
                    DsctoTotalServBolsa,
                    strusuario)
                End If
            Loop

            objReader.Close()

            Return Nothing
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EjecutaprocesoServicioBolsa")
            Return Nothing
        Finally
            If Not IsNothing(objReader) Then
                objReader.Dispose()
            End If
        End Try
    End Function

#Region "Importar Precios Especies"

    ''' <summary>
    ''' Función para leer el archivo para actualizar el precio de la especie.
    ''' </summary>
    ''' <param name="pNombreEncabezado"></param>
    ''' <param name="pDateDesde"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130320</remarks>
    Public Function EvaluarPreciosEspecie(ByVal pNombreEncabezado As String, ByVal pDateDesde As DateTime, ByVal pstrUsuario As String) As String
        Dim objReader As System.IO.StreamReader = Nothing
        Try
            Dim intCual As Integer
            'Dim objReader As System.IO.StreamReader = Nothing

            Dim FILE_NAME As String = pNombreEncabezado
            'Abrimos el archivo de encabezados
            objReader = New System.IO.StreamReader(FILE_NAME)
            If objReader.Peek() <> -1 Then
                Dim primeraLinea = objReader.ReadLine()
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

            Dim strDatosEspecies() As String

            'Recorremos el archivo de encabezados
            Do While objReader.Peek() <> -1
                strDatosEspecies = objReader.ReadLine().Split(",")

                If strDatosEspecies.Length.Equals(4) Then
                    ExtraerDatosEspecies(intCual, strDatosEspecies, pDateDesde, pstrUsuario)
                Else
                    Call LogImportacion(intCual, "Estructura", "Error en la estructura del registro")
                End If
                intCual = intCual + 1
            Loop

            objReader.Close()

            Reportar(MSGERROR_TOTAL_LEIDOS & CStr(intCual - 1))
            Reportar(MSGERROR_TOTAL_IMPORTADOS & CStr(RegBuenos))

            If RegBuenos > 0 Then
                L2SDC.uspOyDNet_Importaciones_MovPreciosEspecies("", pDateDesde, 0, "V", pstrUsuario, DemeInfoSesion(pstrUsuario, "EvaluarPreciosEspecie"),
                                                                     ClsConstantes.GINT_ErrorPersonalizado)
            End If

            Return gStringErrores.ToString()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EvaluarPreciosEspecie")
            Return Nothing
        Finally
            objReader.Dispose()
        End Try
    End Function

    ''' <summary>
    ''' Método que lee cada registro del archivo de Movimiento de Precios de Especies.
    ''' </summary>
    ''' <param name="intCual"></param>
    ''' <param name="strDatosEspecies"></param>
    ''' <param name="pDateDesde"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <remarks>SLB20130320</remarks>
    Private Sub ExtraerDatosEspecies(ByVal intCual As Integer, ByVal strDatosEspecies() As String, ByVal pDateDesde As Date, ByVal pstrUsuario As String)
        Try
            Dim strIdEspecie, strFechaCierre, strPrecio, strTipo As String
            Dim dtmCierre As Date
            Dim logEsAccion As Boolean? = Nothing
            Dim strValorAccion As String = String.Empty

            strIdEspecie = strDatosEspecies(0)
            strFechaCierre = strDatosEspecies(1)
            strPrecio = strDatosEspecies(2)
            strTipo = strDatosEspecies(3)

            If Trim(strIdEspecie) <> String.Empty Then
                If UCase(strTipo) = "E" Or UCase(strTipo) = "T" Then
                    If Not IsNumeric(strPrecio) Then
                        Call LogImportacion(intCual, strPrecio, "Error en tipo de precio")
                        Exit Sub
                    End If
                    If IsNothing(strIdEspecie) Then
                        Call LogImportacion(intCual, strIdEspecie, "Error en la especie")
                        Exit Sub
                    End If

                    dtmCierre = DateSerial(CInt(Mid$(strFechaCierre, 7, 4)), CInt(Mid$(strFechaCierre, 4, 2)), CInt(Mid$(strFechaCierre, 1, 2)))
                    If Not IsDate(dtmCierre) Then
                        Call LogImportacion(intCual, strFechaCierre, "Error en tipo de fecha")
                        Exit Sub
                    Else
                        If Not dtmCierre.Date = pDateDesde.Date Then
                            Call LogImportacion(intCual, CStr(dtmCierre), "Error en el rango de la fecha")
                            Exit Sub
                        End If
                    End If

                    'If Not VerificarSiEspecieExiste(gbtyIDBOLSA, strIdEspecie, logEsAccion) Then
                    '    Call LogImportacion(intCual, strIdEspecie, "La especie no existe")
                    '    Exit Sub
                    'End If

                    strValorAccion = CampoTabla(strIdEspecie, "logEsAccion", "tblEspecies", "strId")

                    If String.IsNullOrEmpty(strValorAccion) Then
                        logEsAccion = False
                    Else
                        logEsAccion = CBool(strValorAccion)
                    End If

                    If UCase(strTipo) = "E" Then
                        If IsNothing(logEsAccion) Then
                            Call LogImportacion(intCual, strIdEspecie, "La especie no existe")
                            Exit Sub
                        End If
                    End If

                    If UCase(strTipo) = "E" Then
                        If Not logEsAccion Then
                            Call LogImportacion(intCual, strIdEspecie, "La especie no es de acciones")
                            Exit Sub
                        End If
                    End If

                    Select Case UCase(strTipo)
                        Case "E"
                            strTipo = "I"
                        Case "T"
                            strTipo = "T"
                    End Select

                    L2SDC.uspOyDNet_Importaciones_MovPreciosEspecies(strIdEspecie, dtmCierre, CDec(strPrecio), strTipo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ExtraerDatosEspecies"),
                                                                     ClsConstantes.GINT_ErrorPersonalizado)

                    RegBuenos = RegBuenos + 1

                End If
            End If

        Catch ex As NullReferenceException
            ManejarError(ex, Me.ToString(), "ExtraerDatosEspecies")
        Catch A As Exception
            ManejarError(A, Me.ToString(), "ExtraerDatosEspecies")
        End Try
    End Sub

#End Region

End Class

