
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq

Namespace OyDClientes

    <MetadataTypeAttribute(GetType(Cliente.ClienteMetadata))> _
    Partial Public Class Cliente
        Friend NotInheritable Class ClienteMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Display(Name:="IDComisionista")> _
            Public Property IDComisionista As Integer



            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer


            '<Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
            <StringLength(17, ErrorMessage:="El campo {0} permite una longitud maxima de 17.")> _
            <Display(Name:="Código")> _
            Public Property IDComitente As String



            '<Required(ErrorMessage:="Este campo es requerido. (IDSucCliente)")> _
            <StringLength(3, ErrorMessage:="El campo {0} permite una longitud maxima de 3.")> _
            <Display(Name:="Sucursal")> _
            Public Property IDSucCliente As String


            '<Required(ErrorMessage:="Este campo es requerido. (TipoIdentificacion)")> _
            <Display(Name:="Tipo")> _
            Public Property TipoIdentificacion As String

            '<Required(ErrorMessage:="Este campo es requerido. (TipoPersona)")> _
            <Display(Name:="Tipo")> _
            Public Property TipoPersona As Nullable(Of Integer)


            '<Required(ErrorMessage:="Este campo es requerido. (NroDocumento)")> _
            <Display(Name:="Número")> _
            Public Property NroDocumento As Decimal


            '<Required(ErrorMessage:="Este campo es requerido. (NroDocumento)")> _
            <StringLength(15, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Número")> _
            Public Property strNroDocumento As String

            <Display(Name:="PoblacionDoc")> _
            Public Property IDPoblacionDoc As Nullable(Of Integer)

            <Display(Name:="DepartamentoDoc")> _
            Public Property IDDepartamentoDoc As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (IDPaisDoc)")> _
            <Display(Name:="PaisDoc")> _
            Public Property IDPaisDoc As Integer

            <Display(Name:="Nacionalidad")> _
            Public Property IDNacionalidad As Nullable(Of Integer)


            '<Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")> _
            <Display(Name:="Nombre")> _
            Public Property Nombre As String


            '<Required(ErrorMessage:="Este campo es requerido. (TipoVinculacion)")> _
            <Display(Name:=" ")> _
            Public Property TipoVinculacion As String

            '<Required(ErrorMessage:="Este campo es requerido. (Ingreso)")> _
            <Display(Name:="Ingreso")> _
            Public Property Ingreso As DateTime

            <Display(Name:="Teléfono Ppal")> _
            Public Property Telefono1 As String

            <StringLength(25, ErrorMessage:="El campo {0} permite una longitud maxima de 25.")> _
            <Display(Name:="Teléfono Sec")> _
            Public Property Telefono2 As String

            <Display(Name:="Fax Ppal")> _
            Public Property Fax1 As String

            <StringLength(25, ErrorMessage:="El campo {0} permite una longitud maxima de 25.")> _
            <Display(Name:="Celular")> _
            Public Property Fax2 As String

            <Display(Name:="Dirección")> _
            Public Property Direccion As String

            <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")> _
            <Display(Name:="Internet")> _
            Public Property Internet As String

            <Display(Name:="Poblacion")> _
            Public Property IDPoblacion As Nullable(Of Integer)

            <Display(Name:="Departamento")> _
            Public Property IDDepartamento As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (IdPais)")> _
            <Display(Name:="País")> _
            Public Property IdPais As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (MenorEdad)")> _
            <Display(Name:="Menor Edad")> _
            Public Property MenorEdad As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (Grupo)")> _
            <Display(Name:="Grupo")> _
            Public Property IDGrupo As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (SubGrupo)")> _
            <Display(Name:="SubGrupo")> _
            Public Property IDSubGrupo As Nullable(Of Integer)

            <Display(Name:="Patrimonio")> _
            Public Property IdPatrimonio As String

            '<Required(ErrorMessage:="Este campo es requerido. (OrigenPatrimonio)")> _
            <Display(Name:="Patrimonio Extranjero")> _
            Public Property OrigenPatrimonio As Boolean


            '<Required(ErrorMessage:="Este campo es requerido. (Profesión)")> _
            <Display(Name:="Profesión")> _
            Public Property IdProfesion As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (RetFuente)")> _
            <Display(Name:="Sujeto a Retención")> _
            Public Property RetFuente As Boolean


            '<Required(ErrorMessage:="Este campo es requerido. (IngresoMensual)")> _
            '<Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Ingreso Mensual")> _
            Public Property IngresoMensual As Nullable(Of Decimal)

            '<Required(ErrorMessage:="Este campo es requerido. (Activo)")> _
            <Display(Name:="Activo")> _
            Public Property Activo As Boolean

            <Display(Name:="Causa")> _
            Public Property IDConcepto As Nullable(Of Integer)

            <Display(Name:="Fecha")> _
            Public Property Concepto As Nullable(Of DateTime)

            <Display(Name:="Notas")> _
            Public Property Notas As String

            '<Required(ErrorMessage:="Este campo es requerido. (SuperValores)")> _
            <Display(Name:="SuperValores")> _
            Public Property SuperValores As DateTime

            <Display(Name:="Pertenece")> _
            Public Property Admonvalores As String

            <Display(Name:="ContratoAV")> _
            Public Property ContratoAV As Nullable(Of DateTime)

            <Display(Name:="Notas Admon")> _
            Public Property NotasAdmon As String

            <Display(Name:="Tercero")> _
            Public Property Tercero As String

            '<Required(ErrorMessage:="Este campo es requerido. (Contribuyente)")> _
            <Display(Name:="Gran Contribuyente")> _
            Public Property Contribuyente As Boolean

            <StringLength(255, ErrorMessage:="El campo {0} permite una longitud maxima de 255.")> _
            <Display(Name:="Actividad eco.(anterior)")> _
            Public Property ActividadEconomica As String

            '<Required(ErrorMessage:="Este campo es requerido. (Excluido)")> _
            <Display(Name:="Excluir datos financieros")> _
            Public Property Excluido As Boolean

            '<Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Activos")> _
            Public Property Activos As Nullable(Of Decimal)

            ' <Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Pasivo")> _
            Public Property Pasivos As Nullable(Of Decimal)

            '<Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Patrimonio")> _
            Public Property Patrimonio As Nullable(Of Decimal)

            '<Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Utilidades")> _
            Public Property Utilidades As Nullable(Of Decimal)

            <Display(Name:="FactorComisionCliente")> _
            Public Property FactorComisionCliente As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (Sector)")> _
            <Display(Name:="Sector")> _
            Public Property IDSector As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (SubSector)")> _
            <Display(Name:="SubSector")> _
            Public Property IDSubSector As Nullable(Of Integer)

            <Display(Name:="Dir. Envío")> _
            Public Property DireccionEnvio As String

            <Display(Name:="Entregar A")> _
            Public Property EntregarA As String

            '<RegularExpression("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage:="La dirección de correo electrónico ingresada no es válida.")> _
            <StringLength(200, ErrorMessage:="El campo {0} permite una longitud maxima de 200.")> _
            <Display(Name:="EMail para envío de informacion")> _
            Public Property EMail As String

            '<RegularExpression("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage:="La dirección de correo electrónico ingresada no es válida.")> _
            <StringLength(200, ErrorMessage:="El campo {0} permite una longitud maxima de 200.")> _
            <Display(Name:="EMail para recibo de instrucciones")> _
            Public Property EMailReciboInstruccion As String

            <Display(Name:="F. Nac/Constitución")> _
            Public Property Nacimiento As Nullable(Of DateTime)

            <Display(Name:="Estado Civil")> _
            Public Property EstadoCivil As String

            <Display(Name:="Sexo")> _
            Public Property Sexo As String

            <StringLength(25, ErrorMessage:="El campo {0} permite una longitud maxima de 25.")> _
            <Display(Name:="Ocupación")> _
            Public Property Ocupacion As String

            <StringLength(25, ErrorMessage:="El campo {0} permite una longitud maxima de 25.")> _
            <Display(Name:="Cargo")> _
            Public Property Cargo As String

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")> _
            <Display(Name:="Empresa")> _
            Public Property Empresa As String

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")> _
            <Display(Name:="Secretaria")> _
            Public Property Secretaria As String

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")> _
            <Display(Name:="Nombre y Apellidos")> _
            Public Property RepresentanteLegal As String

            <Display(Name:="Identificación Tipo")> _
            Public Property TipoReprLegal As String

            '<Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Número")> _
            Public Property IDReprLegal As String

            <StringLength(25, ErrorMessage:="El campo {0} permite una longitud maxima de 25.")> _
            <Display(Name:="Cargo")> _
            Public Property CargoReprLegal As String

            <StringLength(25, ErrorMessage:="El campo {0} permite una longitud maxima de 25.")> _
            <Display(Name:="Teléfono")> _
            Public Property TelefonoReprLegal As String

            <StringLength(30, ErrorMessage:="El campo {0} permite una longitud maxima de 30.")> _
            <Display(Name:="Dirección")> _
            Public Property DireccionReprLegal As String

            '<Required(ErrorMessage:="Este campo es requerido. (IDCiudadReprLegal)")> _
            <Display(Name:="Ciudad ReprLegal")> _
            Public Property IDCiudadReprLegal As Integer

            <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")> _
            <Display(Name:="Contraseña")> _
            Public Property ClaveInternet As String

            '<Required(ErrorMessage:="Este campo es requerido. (ActivoRemoto)")> _
            <Display(Name:="Acceso Remoto")> _
            Public Property ActivoRemoto As Boolean

            <Display(Name:="Referencia")> _
            Public Property TipoReferencia As String

            <Display(Name:="Razón Vinculación")> _
            Public Property RazonVinculacion As String

            <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")> _
            <Display(Name:="Recomendado Por")> _
            Public Property RecomendadoPor As String

            <Display(Name:="F. Actualización Ficha")> _
            Public Property ActualizacionFicha As Nullable(Of DateTime)

            <Display(Name:="Forma Pago")> _
            Public Property FormaPago As String

            <Display(Name:="Nro. Base")> _
            Public Property IdCuentaCtble As String

            <Display(Name:="Título")> _
            Public Property TituloCliente As String

            <Display(Name:="No Llamar")> _
            Public Property NoLlamarAlCliente As String

            <Display(Name:="Enviar Correo")> _
            Public Property EnviarInformeEconomico As String

            <Display(Name:="Enviar Portafolio")> _
            Public Property EnviarPortafolio As String

            <StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1.")> _
            <Display(Name:="Estrato")> _
            Public Property Estrato As String

            <Display(Name:="Numero Hijos")> _
            Public Property NumeroHijos As Nullable(Of Int16)

            <Display(Name:="Fecha Nacimiento")> _
            Public Property NacimientoRepresentanteLegal As Nullable(Of DateTime)

            '<Required(ErrorMessage:="Este campo es requerido. (OrigenIngresos)")> _
            <Display(Name:="Origen Ingresos")> _
            Public Property OrigenIngresos As String

            <Display(Name:="Origen Fondo")> _
            Public Property OrigenFondo As String

            <Display(Name:="Direccion Oficina")> _
            Public Property DireccionOficina As String

            <Display(Name:="Telefono Oficina")> _
            Public Property TelefonoOficina As String

            <Display(Name:="Fax Oficina")> _
            Public Property FaxOficina As String

            <Display(Name:="ApartadoAereo")> _
            Public Property ApartadoAereo As String

            <Display(Name:="EntregaCorrespondencia")> _
            Public Property EntregaCorrespondencia As String

            <Display(Name:="Tipo Segmento")> _
            Public Property TipoSegmento As String

            <Display(Name:="RetornoNroOpTrimestre")> _
            Public Property RetornoNroOpTrimestre As String

            <Display(Name:="RetornoVlrInversionesLiquidas")> _
            Public Property RetornoVlrInversionesLiquidas As String

            <Display(Name:="RetornoPorcInversionesLiquidas")> _
            Public Property RetornoPorcInversionesLiquidas As String

            '<Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Egresos")> _
            Public Property Egresos As Nullable(Of Decimal)

            <Display(Name:="Detalle otros Ingresos")> _
            Public Property DetalleOIngresos As String

            <Display(Name:="codigoCiiu")> _
            Public Property codigoCiiu As String

            '<Required(ErrorMessage:="Este campo es requerido. (OpMonedaExtranjera)")> _
            <Display(Name:="Operación en Moneda Extranjera S/N")> _
            Public Property OpMonedaExtranjera As Boolean

            <Display(Name:="Tipo Operación")> _
            Public Property TipoOperacion As Nullable(Of Integer)

            <Display(Name:="Cual Otra")> _
            Public Property CualOtroTipoOper As String

            <Display(Name:="Nro Cuenta")> _
            Public Property NroCuentaExt As String

            <Display(Name:="Nombre Banco")> _
            Public Property BancoExt As String

            <Display(Name:="Moneda")> _
            Public Property Moneda As String

            <Display(Name:="Nombre Ciudad")> _
            Public Property CiudadExt As String

            <Display(Name:="Pais")> _
            Public Property PaisExt As String

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")> _
            <Display(Name:="Lugar de Realización de la entrevista")> _
            Public Property LugarEntrevista As String

            <StringLength(200, ErrorMessage:="El campo {0} permite una longitud maxima de 200.")> _
            <Display(Name:="Observaciones")> _
            Public Property ObservacionEnt As String

            <Display(Name:="Fecha de Realización")> _
            Public Property Entrevista As Nullable(Of DateTime)

            <Display(Name:="Receptor que certifica la entrevista")> _
            Public Property ReceptorEntrevista As String

            <Display(Name:="Telefono Residencia")> _
            Public Property TelefonoResidencia As String

            <Display(Name:="direccion Residencia")> _
            Public Property direccionResidencia As String

            <Display(Name:="Valor Semestral")> _
            Public Property RetornoValorSemestral As String

            '<Required(ErrorMessage:="Este campo es requerido. (FiguraPublica)")> _
            <Display(Name:="Figura Publica")> _
            Public Property FiguraPublica As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (PersonaAltoRiesgo)")> _
            <Display(Name:="Persona Alto Riesgo")> _
            Public Property PersonaAltoRiesgo As Boolean

            <Display(Name:="Suitability")> _
            Public Property Suitability As Nullable(Of Integer)

            <StringLength(20, ErrorMessage:="El campo {0} permite una longitud maxima de 20.")> _
            <Display(Name:="P.Apellido")> _
            Public Property Apellido1 As String

            <StringLength(20, ErrorMessage:="El campo {0} permite una longitud maxima de 20.")> _
            <Display(Name:="S.Apellido")> _
            Public Property Apellido2 As String


            <StringLength(20, ErrorMessage:="El campo {0} permite una longitud maxima de 20.")> _
            <Display(Name:="P.Nombre")> _
            Public Property Nombre1 As String

            <StringLength(20, ErrorMessage:="El campo {0} permite una longitud maxima de 20.")> _
            <Display(Name:="S.Nombre")> _
            Public Property Nombre2 As String

            <Display(Name:="Codigo Swift")> _
            Public Property CodigoSwift As String

            <Display(Name:="Movimiento Especial")> _
            Public Property MovimientoEspecial As String

            <Display(Name:="Actualización")> _
            Public Property Actualizacion As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
            <Display(Name:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="TIER")> _
            Public Property TIER As String

            <Display(Name:="Indicador Asalariado")> _
            Public Property IndicadorAsalariado As String

            <Display(Name:="Categoria Cliente")> _
            Public Property Clasificacion As String

            <Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Valor Activo Liquido")> _
            Public Property ValorActivoLiquido As Nullable(Of Decimal)

            <Display(Name:="Tip oRelacion")> _
            Public Property TipoRelacion As String

            '<Required(ErrorMessage:="Este campo es requerido. (SinAnimoDeLucro)")> _
            <Display(Name:="Sin Animo De Lucro")> _
            Public Property SinAnimoDeLucro As Boolean

            <Display(Name:="Objetivo Inversion")> _
            Public Property ObjetivoInversion As String

            <Display(Name:="Horizonte Inversion")> _
            Public Property HorizonteInversion As String

            <Display(Name:="Edad Cliente")> _
            Public Property EdadCliente As String

            <Display(Name:="Conocimiento Experiencia")> _
            Public Property ConocimientoExperiencia As String

            <Display(Name:="Clasificacion Inversionista")> _
            Public Property ClasificacionInversionista As String

            <Display(Name:="Vigilado")> _
            Public Property Superintendencia As String

            <Display(Name:="Sucursal")> _
            Public Property IDSucursal As Nullable(Of Integer)

            <Display(Name:="Estado")> _
            Public Property EstadoCliente As String

            <Display(Name:="CtaMultipdto")> _
            Public Property CtaMultipdto As String

            <Display(Name:="Periodicidad")> _
            Public Property Periodicidad As String

            <Display(Name:="Perfil Riesgo")> _
            Public Property PerfilRiesgo As String

            <Display(Name:="Tipo Producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Tipo Cliente")> _
            Public Property TipoCliente As String

            <Display(Name:="Perfil")> _
            Public Property Perfil As String

            <Display(Name:="Exceptuado")> _
            Public Property Exceptuado As String

            <Display(Name:="Categoria Cliente")> _
            Public Property CategoriaCliente As String

            <Range(0, 999999999999999, ErrorMessage:="El campo {0} permite una longitud maxima de 15.")> _
            <Display(Name:="Comision Acciones")> _
            Public Property ComisionAcciones As Nullable(Of Decimal)

            <Display(Name:="UltimoMov")> _
            Public Property UltimoMov As Nullable(Of DateTime)

            <Display(Name:="NitDepositante")> _
            Public Property NitDepositante As Nullable(Of Decimal)

            <Display(Name:="Apt")> _
            Public Property Apt As Nullable(Of Boolean)

            <Required(ErrorMessage:="Este campo es requerido. (CoopExenta)")> _
              <Display(Name:="CoopExenta")> _
            Public Property CoopExenta As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (PagoSurenta)")> _
              <Display(Name:="Pago Surenta")> _
            Public Property PagoSurenta As Boolean

            <Display(Name:="Poder Para Firmar")> _
            Public Property PoderParaFirmar As Nullable(Of Boolean)

            <Display(Name:="Prestador ServExcIVA")> _
            Public Property PrestadorServExcIVA As Nullable(Of Boolean)

            <Required(ErrorMessage:="Este campo es requerido. (SegmentoUsuarioRedes)")> _
              <Display(Name:="Segmento UsuarioRedes")> _
            Public Property SegmentoUsuarioRedes As Boolean

            <Display(Name:="Visacion")> _
            Public Property Visacion As Nullable(Of Boolean)

            <Display(Name:="Atencion Operativa")> _
            Public Property AtencionOperativa As String

            <Display(Name:="Centro Ejecucion")> _
            Public Property CentroEjecucion As String

            <Display(Name:="Clasificacion Documento")> _
            Public Property ClasificacionDocumento As String

            <Display(Name:="Dirección EnvioDane")> _
            Public Property DireccionEnvioDane As String

            <Display(Name:="Existe ContratoAdmon")> _
            Public Property ExisteContratoAdmon As String

            <Display(Name:="Justificacion Fondos")> _
            Public Property JustificacionFondos As String


            <Display(Name:="Prioridad Portafolio")> _
            Public Property PrioridadPortafolio As String

            <Display(Name:="Referencia")> _
            Public Property Referencia As String

            <Display(Name:="Retencion Fuente")> _
            Public Property RetencionFuente As String

            <Display(Name:="Sigla")> _
            Public Property Sigla As String

            <Display(Name:="Tipo Mensajeria")> _
            Public Property TipoMensajeria As String

            <Display(Name:="Tipo Regalo")> _
            Public Property TipoRegalo As String

            <Display(Name:="Usuario Ingreso")> _
            Public Property UsuarioIngreso As String

            <Display(Name:="Patrimonio Extranjero")> _
            Public Property FondoExtranjero As Nullable(Of Boolean)

            <Display(Name:="Replicar Safyr Fondos")> _
            Public Property ReplicarSafyrFondos As Nullable(Of Boolean)

            <Display(Name:="Replicar Safyr Portafolios")> _
            Public Property ReplicarSafyrPortafolios As Nullable(Of Boolean)

            <Display(Name:="Replicar Safyr Clientes")> _
            Public Property ReplicarSafyrClientes As Nullable(Of Boolean)

            <Display(Name:="Replicar Mercansoft")> _
            Public Property ReplicarMercansoft As Nullable(Of Boolean)

            <Display(Name:="Replicar Agora")> _
            Public Property ReplicarSantanderAgora As Nullable(Of Boolean)

            <Display(Name:="Declarante")> _
            Public Property Declarante As Nullable(Of Boolean)

            <Display(Name:="AutoRetenedor")> _
            Public Property AutoRetenedor As Nullable(Of Boolean)

            <Display(Name:="Exento GMF")> _
            Public Property ExentoGMF As Nullable(Of Boolean)

            <Display(Name:="Ciudad Nacimiento")> _
            Public Property IdCiudadNacimiento As Nullable(Of Integer)

            <Display(Name:="F.Expedición")> _
            Public Property FechaExpedicionDoc As Nullable(Of DateTime)

            <StringLength(10, ErrorMessage:="El campo {0} permite una longitud máxima de 10.")> _
            <Display(Name:="Indicativo Telefono")> _
            Public Property IndicativoTelefono As String

            <Display(Name:="Acepta Cruces")> _
            Public Property AceptaCruces As Nullable(Of Boolean)

            <Display(Name:="Tipo Intermediario")> _
            Public Property TipoIntermediario As Nullable(Of Integer)

            <Display(Name:="Receptor Safyr Fondos")> _
            Public Property CodReceptorSafyrFondos As String

            <Display(Name:="Receptor Safyr Portafolio")> _
            Public Property CodReceptorSafyrPortafolio As String

            <Display(Name:="Receptor Safyr Clientes")> _
            Public Property CodReceptorSafyrClientes As String

            <Display(Name:="Receptor Mercansoft")> _
            Public Property CodReceptorMercansoft As String

            <Display(Name:="Tipo IdentificacionRB")> _
            Public Property TipoIdentificacionRB As String

            <Display(Name:="NroDocumentoRB")> _
            Public Property NroDocumentoRB As String

            <Display(Name:="NombreRB")> _
            Public Property NombreRB As String

            <Display(Name:="IDClientes")> _
            Public Property IDClientes As Integer


            <Display(Name:="Embajada")> _
            Public Property Embajada As Nullable(Of Boolean)

            <Display(Name:=" ")> _
            Public Property Por_Aprobar As String

            <Display(Name:=" ")> _
            Public Property Estado As String


            <Display(Name:="Reteica")> _
            Public Property RETEICA As Nullable(Of Boolean)

            <Display(Name:="Perfil Sarlaft")> _
            Public Property PerfilSarlaft As String

            <Display(Name:="Nro de SMMLV ")> _
            Public Property NroSalarios As Integer

            <Display(Name:="Segmento Comercial")> _
            Public Property SegmentoComercial As Integer

            <Display(Name:="Descripción Cuenta")> _
            Public Property DescripcionCuenta As String

            <Display(Name:="CREE")> _
            Public Property CREE As Nullable(Of Boolean)

            <Display(Name:="Clasificación FATCA")> _
            Public Property ClasificacionFATCA As String

            <Display(Name:="Custodio")> _
            Public Property IDCustodio As Nullable(Of Integer)

        End Class
    End Class

    <MetadataTypeAttribute(GetType(CuentasCliente.CuentasClienteMetadata))> _
    Partial Public Class CuentasCliente
        Friend NotInheritable Class CuentasClienteMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (NombreBanco)")> _
       <Display(Name:="Nombre Banco")> _
            Public Property NombreBanco As String

            <Required(ErrorMessage:="Este campo es requerido. (NombreSucursal)")> _
     <Display(Name:="Nombre Sucursal")> _
            Public Property NombreSucursal As String

            <Required(ErrorMessage:="Este campo es requerido. (Cuenta)")> _
<Display(Name:="Cuenta")> _
            Public Property Cuenta As String

            <Required(ErrorMessage:="Este campo es requerido. (Titular)")> _
<Display(Name:="Titular")> _
            Public Property Titular As String

            <Required(ErrorMessage:="Este campo es requerido. (NumeroID)")> _
<Display(Name:="NumeroID")> _
            Public Property NumeroID As String


            <Required(ErrorMessage:="Este campo es requerido. (Tipo Cuenta)")> _
<Display(Name:="Tipo Cuenta")> _
            Public Property TipoCuenta As String


        End Class
    End Class

    <MetadataTypeAttribute(GetType(ClientesReceptore.ClientesReceptoreMetadata))> _
    Partial Public Class ClientesReceptore
        Friend NotInheritable Class ClientesReceptoreMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
           <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Required(ErrorMessage:="Este campo es requerido. (Receptor)")> _
      <Display(Name:="Receptor")> _
            Public Property IDReceptor As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesOrdenante.ClientesOrdenanteMetadata))> _
    Partial Public Class ClientesOrdenante
        Friend NotInheritable Class ClientesOrdenanteMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
          <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (IDOrdenante)")> _
        <Display(Name:="IDOrdenante")> _
            Public Property IDOrdenante As String



        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesBeneficiarios.ClientesBeneficiariosMetadata))> _
    Partial Public Class ClientesBeneficiarios
        Friend NotInheritable Class ClientesBeneficiariosMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (TipoID)")> _
             <Display(Name:="TipoID")> _
            Public Property TipoID As String

            <Required(ErrorMessage:="Este campo es requerido. (NroDocumento)")> _
         <Display(Name:="NroDocumento")> _
            Public Property NroDocumento As Decimal



        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesAficiones.ClientesAficionesMetadata))> _
    Partial Public Class ClientesAficiones
        Friend NotInheritable Class ClientesAficionesMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesDeportes.ClientesDeportesMetadata))> _
    Partial Public Class ClientesDeportes
        Friend NotInheritable Class ClientesDeportesMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesDireccione.ClientesDireccioneMetadata))> _
    Partial Public Class ClientesDireccione
        Friend NotInheritable Class ClientesDireccioneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDsucComisionista", Description:="IDsucComisionista")> _
            Public Property IDsucComisionista As Integer

            <Display(Name:="Consecutivo", Description:="Consecutivo")> _
            Public Property Consecutivo As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente", Description:="IDComitente")> _
            Public Property IDComitente As String

            ' <Required(ErrorMessage:="Este campo es requerido. (Direccion)")> _
            <Display(Name:="Direccion", Description:="Direccion")> _
            Public Property Direccion As String

            '<Required(ErrorMessage:="Este campo es requerido. (Telefono)")> _
            <Display(Name:="Teléfono", Description:="Telefono")> _
            Public Property Telefono As Nullable(Of Int64)

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")> _
            <Display(Name:="Fax", Description:="Fax")> _
            Public Property Fax As String

            <Display(Name:="Ciudad", Description:="Ciudad")> _
            Public Property Ciudad As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Tipo)")> _
            <Display(Name:="Tipo", Description:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="DireccionEnvio", Description:="DireccionEnvio")> _
            Public Property DireccionEnvio As Nullable(Of Boolean)

            <Display(Name:="Activo", Description:="Activo")> _
            Public Property Activo As Nullable(Of Boolean)

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")> _
            <Display(Name:="EntregaA", Description:="EntregaA")> _
            Public Property EntregaA As String

            <Display(Name:="IdConsecutivoSafyr", Description:="IdConsecutivoSafyr")> _
            Public Property IdConsecutivoSafyr As Nullable(Of Integer)

            <Display(Name:="IdConsecutivoClientes", Description:="IdConsecutivoClientes")> _
            Public Property IdConsecutivoClientes As Nullable(Of Integer)

            <Display(Name:="IdConsecutivoPortafolios", Description:="IdConsecutivoPortafolios")> _
            Public Property IdConsecutivoPortafolios As Nullable(Of Integer)

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")> _
            <Display(Name:="Extension", Description:="Extension")> _
            Public Property Extension As String




        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesDocumentosRequeridos.ClientesDocumentosRequeridosMetadata))> _
    Partial Public Class ClientesDocumentosRequeridos
        Friend NotInheritable Class ClientesDocumentosRequeridosMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente", Description:="IDComitente")> _
            Public Property IDComitente As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesLOGHistoriaIR.ClientesLOGHistoriaIRMetadata))> _
    Partial Public Class ClientesLOGHistoriaIR
        Friend NotInheritable Class ClientesLOGHistoriaIRMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente", Description:="IDComitente")> _
            Public Property ID As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesConocimientoEspecifico.ClientesConocimientoEspecificoMetadata))> _
    Partial Public Class ClientesConocimientoEspecifico
        Friend NotInheritable Class ClientesConocimientoEspecificoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente", Description:="IDComitente")> _
            Public Property ID As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(TipoClient.TipoClientMetadata))> _
    Partial Public Class TipoClient
        Friend NotInheritable Class TipoClientMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente", Description:="IDComitente")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (IDTipoEntidad)")> _
              <Display(Name:="IDTipoEntidad", Description:="IDTipoEntidad")> _
            Public Property IDTipoEntidad As Integer

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDTipoCliente", Description:="IDTipoCliente")> _
            Public Property IDTipoCliente As Integer

        End Class
    End Class
    <MetadataTypeAttribute(GetType(Portafolio.PortafoliotMetadata))> _
    Partial Public Class Portafolio
        Friend NotInheritable Class PortafoliotMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Consecutivo", Description:="Consecutivo")> _
            Public Property Consecutivo As Integer


            <Display(Name:="Portafolio", Description:="Portafolio")> _
            Public Property Portafolio As String


            <Display(Name:="Valor")> _
            Public Property Valor As Decimal


            <Display(Name:="Porcentaje")> _
            Public Property Porcentaje As Decimal


            <Display(Name:="FechaCorteHabil", Description:="FechaCorteHabil")> _
            Public Property FechaCorteHabil As DateTime


            <Display(Name:="EspeciesNoValorizadas", Description:="EspeciesNoValorizadas")> _
            Public Property EspeciesNoValorizadas As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesAccionistas.ClientesAccionistasMetadata))> _
    Partial Public Class ClientesAccionistas
        Friend NotInheritable Class ClientesAccionistasMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:="IDComitente")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (TipoIdentificacion)")> _
             <Display(Name:="TipoIdentificacion")> _
            Public Property TipoIdentificacion As String

            <Required(ErrorMessage:="Este campo es requerido. (NroDocumento)")> _
         <Display(Name:="NroDocumento")> _
            Public Property NroDocumento As String



        End Class
    End Class
    <MetadataTypeAttribute(GetType(tblConsultaSaldoRes.tblConsultaSaldoResMetadata))> _
    Partial Public Class tblConsultaSaldoRes
        Friend NotInheritable Class tblConsultaSaldoResMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Editable(True)> _
                <Display(Name:="Chequear")> _
            Public Property Chequear As Boolean
        End Class
    End Class

    <MetadataTypeAttribute(GetType(ClientesEncargos.ClientesEncargosMetadata))> _
    Partial Public Class ClientesEncargos
        Friend NotInheritable Class ClientesEncargosMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Codigo Comisionista")> _
            Public Property CodComisionista As String

            <Display(Name:="Fecha Cierre")> _
            Public Property FechaCierre As DateTime

            <Display(Name:="Saldo Anterior")> _
            Public Property SaldoAnterior As Decimal

            <Display(Name:="Valor Pesos")> _
            Public Property ValorPesos As Decimal
        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesPersonasDepEconomica.ClientesPersonasDepEconomicaMetadata))> _
    Partial Public Class ClientesPersonasDepEconomica
        Friend NotInheritable Class ClientesPersonasDepEconomicaMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Required(ErrorMessage:="Este campo es requerido. (Nro Documento)")> _
            <Display(Name:="NroDocumento")> _
            Public Property NroDocumento As String

            '<Required(ErrorMessage:="Este campo es requerido. (ComitentePerDepEco)")> _
            '<Display(Name:="IDComitentePerDepEco")> _
            'Public Property IDComitentePerDepEco As String

            <Required(ErrorMessage:="Este campo es requerido. (Parentesco)")> _
            <Display(Name:="Parentesco")> _
            Public Property Parentesco As String

        End Class
    End Class
#Region "Inactivar Clientes"
    <MetadataTypeAttribute(GetType(ListaClientesInactivar.ListaClientesInactivarMetadata))> _
    Partial Public Class ListaClientesInactivar
        Friend NotInheritable Class ListaClientesInactivarMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="FechaActFicha", Description:="FechaActFicha")> _
            Public Property FechaActFicha As Nullable(Of DateTime)

            <Display(Name:="FechaActualizacion", Description:="FechaActualizacion")> _
            Public Property FechaActualizacion As DateTime

            <Display(Name:="fechaIngreso", Description:="fechaIngreso")> _
            Public Property fechaIngreso As DateTime


            <Display(Name:="Grupo", Description:="Grupo")> _
            Public Property Grupo As String

            <Display(Name:="lngId", Description:="lngId")> _
            Public Property lngId As String

            <Display(Name:="lngIdGrupo", Description:="lngIdGrupo")> _
            Public Property lngIdGrupo As Integer

            <Display(Name:="strNombre", Description:="strNombre")> _
            Public Property strNombre As String

            <Display(Name:="strNroDocumento", Description:="strNroDocumento")> _
            Public Property strNroDocumento As String

            <Display(Name:="UltimoNegocio", Description:="UltimoNegocio")> _
            Public Property UltimoNegocio As Nullable(Of DateTime)


            <Display(Name:="Seleccionado", Description:="Seleccionado")> _
            <Editable(True, AllowInitialValue:=True)> _
            Public Property Seleccionado As Boolean

        End Class
    End Class
    <MetadataTypeAttribute(GetType(PreClientes.PreClientesMetadata))> _
    Partial Public Class PreClientes
        Friend NotInheritable Class PreClientesMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="PreCliente")> _
            Public Property IDPreCliente As Integer

            <Display(Name:="TipoIdentificacion")> _
            Public Property TipoIdentificacion As String

            <Display(Name:="NroDocumento")> _
            Public Property NroDocumento As String

            <Display(Name:="Nacionalidad")> _
            Public Property IDNacionalidad As Integer


            <Display(Name:="Nombres")> _
            Public Property Nombres As String


            <Display(Name:="Sexo")> _
            Public Property Sexo As String

            <Display(Name:="PrimerApellido")> _
            Public Property PrimerApellido As String

            <Display(Name:="Segundo Apellido")> _
            Public Property SegundoApellido As String

            <Display(Name:="Primer Nombre")> _
            Public Property PrimerNombre As String

            <Display(Name:="Segundo Nombre")> _
            Public Property SegundoNombre As String

            <Editable(True)> _
            <Display(Name:="Rechazar")> _
            Public Property Rechazar As Boolean



        End Class
    End Class
    <MetadataTypeAttribute(GetType(ClientesFicha.ClientesFichaMetadata))> _
    Partial Public Class ClientesFicha
        Friend NotInheritable Class ClientesFichaMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (CodDocumento) en el tab Ficha Cliente")> _
              <Display(Name:="CodDocumento")> _
            Public Property CodDocumento As String

            <Required(ErrorMessage:="Este campo es requerido. (TipoReferencia) en el tab Ficha Cliente")> _
             <Display(Name:="TipoReferencia")> _
            Public Property TipoReferencia As String

            '<Required(ErrorMessage:="Este campo es requerido. (Descripcion) en el tab Ficha Cliente")> _
            '<Display(Name:="Descripcion")> _
            'Public Property Descripcion As String
        End Class
    End Class

#End Region

    NotInheritable Class prueba
        Inherits Attribute
        Private _tab As Integer
        Public Property tab As Integer
            Get
                Return _tab
            End Get
            Set(ByVal value As Integer)
                _tab = value
            End Set
        End Property

    End Class

#Region "deceval"
    ''' <summary>
    ''' Metadata para detalle inversionista -- Carlos Andres Toro -- marzo/2015
    ''' </summary>
    ''' <remarks></remarks>
    '''
    <MetadataTypeAttribute(GetType(DetalleInversionistas.DetalleInversionistasMetadata))> _
    Partial Public Class DetalleInversionistas
        Friend NotInheritable Class DetalleInversionistasMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDInversionista", Description:="IDInversionista")> _
            Public Property IDInversionista As Integer

            <Display(Name:="Detalle", Description:="Detalle")> _
            Public Property Detalle As String

            <Display(Name:="Fecha", Description:="Fecha")> _
            Public Property Fecha As DateTime

        End Class
    End Class

#End Region

End Namespace

