
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


#Region "JUAN CARLOS SOTO MAESTROS"

#Region "V1.0.0"

<MetadataTypeAttribute(GetType(Sucursale.SucursaleMetadata))> _
Partial Public Class Sucursale
    Friend NotInheritable Class SucursaleMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")> _
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")> _
          <Display(Name:="Código")> _
        Public Property IDSucursal As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
        <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")> _
        <Display(Name:="Nombre")> _
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje de patrimonio técnico)")> _
        <Range(0, 100, ErrorMessage:="{0} debe tener un valor entre 0 y 100")> _
        <Display(Name:="Porcentaje de patrimonio técnico", Description:="Porcentaje de patrimonio técnico")> _
        Public Property PorcentajePatrimonioTecnico As Double

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As DateTime

        '<Display(Name:="Usuario", Description:="Usuario")> _
        'Public Property Usuario As String

        <Display(Name:="Suc")> _
        Public Property IDSuc As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(TiposEntida.TiposEntidaMetadata))> _
Partial Public Class TiposEntida
    Friend NotInheritable Class TiposEntidaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")> _
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")> _
          <Display(Name:="Código")> _
        Public Property IDTipoEntidad As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
        <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")> _
        <Display(Name:="Nombre")> _
        Public Property Nombre As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

        <Display(Name:="TipoEntidadI")> _
        Public Property IdTipoEntidadI As Int16

    End Class
End Class
<MetadataTypeAttribute(GetType(ProductosValore.ProductosValoreMetadata))> _
Partial Public Class ProductosValore
    Friend NotInheritable Class ProductosValoreMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="Este campo es requerido. (Código)")> _
          <Display(Name:="Código")> _
        Public Property IDTipoProducto As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Descripción)")> _
        <StringLength(50, ErrorMessage:="El campo Descripción permite una longitud máxima de 50 caracteres.")> _
          <Display(Name:="Descripción")> _
        Public Property Descripcion As String

        <Display(Name:="Orden")> _
        Public Property Orden As Nullable(Of Integer)

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As Nullable(Of DateTime)

        <Display(Name:="ProductoValores")> _
        Public Property IdProductoValores As Int16

    End Class
End Class
<MetadataTypeAttribute(GetType(ConceptosInactivida.ConceptosInactividaMetadata))> _
Partial Public Class ConceptosInactivida
    Friend NotInheritable Class ConceptosInactividaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")> _
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")> _
          <Display(Name:="Código")> _
        Public Property ID As Integer

        <Display(Name:="Tipo")> _
        Public Property Actividad As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
        <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")> _
        <Display(Name:="Nombre")> _
        Public Property Nombre As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

        <Display(Name:="ConceptoInactividad")> _
        Public Property IdConceptoInactividad As Int16

    End Class
End Class

#End Region

#Region "V1.0.1"

<MetadataTypeAttribute(GetType(Bolsa.BolsaMetadata))> _
Partial Public Class Bolsa
    Friend NotInheritable Class BolsaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
        'Public Property IDComisionista As Integer

        '<Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        'Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (IdBolsa)")> _
        <Display(Name:="Código")> _
        Public Property IdBolsa As Integer

        <Required(ErrorMessage:="El nombre de la bolsa es un dato requerido")>
        <A2RegularExpression("[A-Za-zñÑ\w\s]{0,25}", ErrorMessage:="Existen caracteres no permitidos en el campo {0}, solo se permiten letras y números.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (Ciudad)")>
        <Display(Name:="Ciudad")>
        Public Property IDPoblacion As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nit)")>
        <Display(Name:="Nit")>
        Public Property NroDocumento As Nullable(Of Decimal)

        '<Display(Name:="Actualizacion", Description:="Actualizacion")> _
        'Public Property Actualizacion As DateTime

        '<Display(Name:="Usuario", Description:="Usuario")> _
        'Public Property Usuario As String

        <Display(Name:="Mercado Integrado")>
        Public Property MercadoIntegrado As Boolean

        '<Display(Name:="Activa", Description:="Activa")> _
        'Public Property Activa As Nullable(Of Boolean)

        '<Display(Name:="intIDBolsa", Description:="intIDBolsa")> _
        'Public Property intIDBolsa As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(BancosNacionale.BancosNacionaleMetadata))>
Partial Public Class BancosNacionale
    Friend NotInheritable Class BancosNacionaleMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Código interbancario")>
        Public Property Id As Integer

        <Display(Name:="Código ACH")>
        Public Property CodACH As String

        <Display(Name:="Dígito de Chequeo")>
        Public Property DigitoChequeo As String

        <Display(Name:="NIT")>
        Public Property NroDocumento As String

        <Required(ErrorMessage:="El nombre del banco nacional es un dato requerido.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="BancoNacional")>
        Public Property IDBancoNacional As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(RelacionesCodBanco.RelacionesCodBancoMetadata))>
Partial Public Class RelacionesCodBanco
    Friend NotInheritable Class RelacionesCodBancoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IdComisionista", Description:="IdComisionista")>
        Public Property IdComisionista As Integer

        <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")>
        Public Property IdSucComisionista As Integer

        <Display(Name:="CodBanco")>
        Public Property IdCodBanco As Integer

        <Required(ErrorMessage:="Debe elegir una relación tecnológica.")>
        <Display(Name:=" ")>
        Public Property RelTecno As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(AllowEmptyStrings:=True)>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
<MetadataTypeAttribute(GetType(Clasificacion.ClasificacionMetadata))>
Partial Public Class Clasificacion
    Friend NotInheritable Class ClasificacionMetadata
        Private Sub New()
            MyBase.New()
        End Sub



        <Required(ErrorMessage:="El nombre es un dato requerido.")>
        <StringLength(50, ErrorMessage:="El campo permite máximo 50 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="Grupo")>
        Public Property EsGrupo As Boolean
        <Display(Name:="Sector")>
        Public Property EsSector As Boolean

        <Display(Name:="Pertenece A")>
        Public Property IDPerteneceA As Integer

        <Required(ErrorMessage:="Debe indicar a quien se aplica.")>
        <Display(Name:="Aplicado A")>
        Public Property AplicaA As String

        <Display(Name:="Nemo")>
        Public Property Nemo As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Clasificacion")>
        Public Property IDClasificacion As Integer

        <Display(Name:="Subgrupo")>
        Public Property EsSubgrupo As Boolean

        <Display(Name:="Subsector")>
        Public Property EsSubsector As Boolean

    End Class
End Class

''' <history>
''' Modificado por   : Juan Carlos Soto Cruz (JCS).
''' Fecha            : Mayo 28/2013
''' Descripción      : Se adiciona la columna intIdTipoEntidad.
''' </history>
<MetadataTypeAttribute(GetType(Comisionista.ComisionistaMetadata))>
Partial Public Class Comisionista
    Friend NotInheritable Class ComisionistaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Bolsa")>
        Public Property IdBolsa As Integer

        <Display(Name:="Código")>
        Public Property ID As Integer

        <Display(Name:="Nit")>
        <DisplayFormat(DataFormatString:="d15")>
        Public Property NroDocumento As Decimal

        <Display(Name:="Nombre")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        Public Property Nombre As String

        <Display(Name:="Representante Legal")>
        Public Property RepresentanteLegal As String

        <Display(Name:="Teléfono Uno")>
        Public Property Telefono1 As String

        <Display(Name:="Teléfono Dos")>
        Public Property Telefono2 As String

        <Display(Name:="Fax Uno")>
        Public Property Fax1 As String

        <Display(Name:="Fax Dos")>
        Public Property Fax2 As String

        <Display(Name:="Dirección")>
        Public Property Direccion As String

        <Display(Name:="Página Web")>
        Public Property Internet As String

        <Display(Name:="E – Mail")>
        <A2RegularExpression("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage:="La dirección de correo electrónico ingresada no es válida.")>
        Public Property EMail As String

        <Display(Name:="Ciudad")>
        Public Property IDPoblacion As Integer

        <Display(Name:="Departamento")>
        Public Property IDDepartamento As Integer

        <Display(Name:="País")>
        Public Property IDPais As Integer

        <Display(Name:="Notas")>
        Public Property Notas As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Comisionista")>
        Public Property IDComisionista As Integer

        ' JCS Mayo 28/2013
        <Required(ErrorMessage:="El campo tipo entidad es requerido")>
        <Display(Name:="Tipo entidad")>
        Public Property intIdTipoEntidad As Nullable(Of Integer)
        ' FIN JCS
    End Class
End Class
<MetadataTypeAttribute(GetType(ConsecutivosDocumento.ConsecutivosDocumentoMetadata))>
Partial Public Class ConsecutivosDocumento
    Friend NotInheritable Class ConsecutivosDocumentoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Documento")>
        Public Property Documento As String


        <Display(Name:="Nombre")>
        Public Property NombreConsecutivo As String

        <Display(Name:="Descripción")>
        Public Property Descripcion As String

        <Display(Name:="Cta Contable")>
        Public Property CuentaContable1 As String

        <Display(Name:="SubMódulo")>
        Public Property TipoCuenta As String

        <Display(Name:="Tarifa")>
        Public Property IdTarifa As Nullable(Of Integer)

        <Display(Name:="Comprobante Contable")>
        Public Property ComprobanteContable As String

        <Display(Name:="Símbolo")>
        Public Property Signo As String

        <Display(Name:="Sucursal Conciliación")>
        Public Property sucursalConciliacion As String

        <Display(Name:="Cuenta Contable")>
        Public Property CuentaContable As Boolean

        <Display(Name:="Concepto")>
        Public Property Concepto As Nullable(Of Boolean)

        <Display(Name:="Sucursal Firma")>
        Public Property IdSucursalSuvalor As Nullable(Of Integer)

        <Display(Name:="Extracto Banco")>
        Public Property IncluidoEnExtractoBanco As Boolean

        <Display(Name:="Extracto Cliente")>
        Public Property IncluidoEnExtractoCliente As Boolean

        <Display(Name:="Cliente")>
        Public Property PermiteCliente As String

        <Display(Name:="Moneda")>
        Public Property IdMoneda As Nullable(Of Integer)

        <Display(Name:="Cliente")>
        Public Property Cliente As Boolean

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="ConsecutivoDocumento")>
        Public Property IDConsecutivoDocumento As Integer

        <Display(Name:="Compañía")>
        Public Property Compania As Nullable(Of Integer)

        <Display(Name:="Nombre Cia")>
        Public Property NombreCompania As String

    End Class
End Class
<MetadataTypeAttribute(GetType(CuentasContablesOy.CuentasContablesOyMetadata))>
Partial Public Class CuentasContablesOy
    Friend NotInheritable Class CuentasContablesOyMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Código")>
        <Required(ErrorMessage:="El campo {0} es requerido.")>
        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
        Public Property ID As String

        <Display(Name:="Nombre")>
        <Required(ErrorMessage:="El campo {0} es requerido.")>
        Public Property Nombre As String

        <Display(Name:="Naturaleza")>
        <Required(ErrorMessage:="Debe elegir una opción en el campo Naturaleza.")>
        Public Property Naturaleza As String

        <Display(Name:="Documento Asociado")>
        <Required(ErrorMessage:="Debe elegir una opción en el campo Documento Asociado.")>
        Public Property DctoAsociado As String

        <Display(Name:="actualizacion", Description:="actualizacion")>
        Public Property actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="CuentaContable")>
        Public Property IDCuentaContable As Integer

    End Class
End Class

#End Region

#Region "V1.0.2"


<MetadataTypeAttribute(GetType(DiasNoHabile.DiasNoHabileMetadata))>
Partial Public Class DiasNoHabile
    Friend NotInheritable Class DiasNoHabileMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Día no hábil")>
        <Required(ErrorMessage:="Este campo es requerido. (Día)")>
        Public Property NoHabil As DateTime?

        <Display(Name:="Estado")>
        Public Property Activo As Boolean

        <Display(Name:="Fecha Activo")>
        Public Property dActivo As DateTime

        <Display(Name:="Fecha Inactivo")>
        Public Property Inactivo As Nullable(Of DateTime)

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="País")>
        Public Property IdPais As Nullable(Of Integer)

        <Display(Name:="País")>
        Public Property strIdPais As String

        <Display(Name:="DiaNoHabil")>
        Public Property IDDiaNoHabil As Integer

    End Class
End Class

''' <history>
''' Modificado por   : Juan Carlos Soto Cruz (JCS).
''' Fecha            : Mayo 27/2013
''' Descripción      : Se adiciona la columna intIdCalificadora.
''' 
''' Modificado por   : Juan Carlos Soto Cruz (JCS).
''' Fecha            : Junio 27/2013
''' Descripción      : Se retira la propiedad intIdTipoEmisor, lo anterior por que se detecta que la funcionalidad que proporcionaria este campo ya existia en el maestro de emisores.
''' </history>
<MetadataTypeAttribute(GetType(Emisore.EmisoreMetadata))>
Partial Public Class Emisore
    Friend NotInheritable Class EmisoreMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Código")>
        Public Property ID As Decimal

        <Required(ErrorMessage:="El nombre es un dato requerido.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="El Nit es un dato requerido.")>
        <Display(Name:="NIT")>
        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
        Public Property NIT As Nullable(Of Decimal)

        <Display(Name:="Teléfono 1")>
        Public Property Telefono1 As String
        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _

        <Display(Name:="Teléfono 2")>
        Public Property Telefono2 As String
        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _

        <Display(Name:="Fax 1")>
        Public Property Fax1 As String
        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _

        <Display(Name:="Fax 2")>
        Public Property Fax2 As String
        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _

        <Display(Name:="Dirección")>
        Public Property Direccion As String

        <Display(Name:="Internet")>
        Public Property Internet As String

        '<Display(Name:="EMail")> _
        '<A2RegularExpression("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage:="La dirección de correo electrónico ingresada no es válida.")> _
        'Public Property EMail As String

        <Required(ErrorMessage:="El Grupo es requerido.")>
        <Display(Name:="Grupo")>
        Public Property IDGrupo As Integer

        <Required(ErrorMessage:="El SubGrupo es requerido.")>
        <Display(Name:="SubGrupo")>
        Public Property IDSubGrupo As Integer

        <Display(Name:="Poblacion")>
        Public Property IDPoblacion As Nullable(Of Integer)

        <Display(Name:="Departamento: ")>
        Public Property IDDepartamento As Nullable(Of Integer)

        <Display(Name:="País: ")>
        Public Property IDPais As Nullable(Of Integer)

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Contacto")>
        Public Property Contacto As String

        <Display(Name:="Mostrar")>
        Public Property Mostrar As String

        <Display(Name:="Total")>
        Public Property Total As String

        <Display(Name:="Responde")>
        Public Property Responde As String

        <Display(Name:="TipoComision", Description:="TipoComision")>
        Public Property TipoComision As String

        <Display(Name:="Emisor")>
        Public Property IdEmisor As Integer

        <Display(Name:="Código Emisor")>
        Public Property CodigoEmisor As String

        <Display(Name:="Vigilado por la superintendencia")>
        Public Property VigiladoSuper As Boolean

        <Required(ErrorMessage:="El Código ciiu es requerido.")>
        <Display(Name:="Código ciiu")>
        Public Property intIdCodigoCIIU As Nullable(Of Integer)

        <Required(ErrorMessage:="El campo tipo entidad es requerido")>
        <Display(Name:="Tipo entidad")>
        Public Property intIdTipoEntidad As Nullable(Of Integer)

        ' JCS Mayo 27/2013
        <Display(Name:="Calificadora")>
        Public Property intIdCalificadora As Nullable(Of Integer)
        ' FIN JCS 

        <Display(Name:="Fuente extranjero")>
        Public Property FuenteExtranjero As Boolean

    End Class
End Class
<MetadataTypeAttribute(GetType(Especie.EspecieMetadata))>
Partial Public Class Especie
    Friend NotInheritable Class EspecieMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Editable(True)>
        <Display(Name:="Nemotécnico", Description:="Nemotécnico")>
        Public Property Id As String

        <Display(Name:="Descripción", Description:="Descripción")>
        Public Property Nombre As String

    End Class
End Class
<MetadataTypeAttribute(GetType(TipoPersonaPorDct.TipoPersonaPorDctMetadata))>
Partial Public Class TipoPersonaPorDct
    Friend NotInheritable Class TipoPersonaPorDctMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID")>
        Public Property ID As Integer

        <Display(Name:="Descripción Tipo Dcto")>
        Public Property TipoIdentificacion As String

        <Display(Name:="Tipo Persona")>
        Public Property IDTipoPersona As Integer

        <Display(Name:="Menor de Edad")>
        Public Property menored As Nullable(Of Int16)

    End Class
End Class
<MetadataTypeAttribute(GetType(Mesa.MesaMetadata))>
Partial Public Class Mesa
    Friend NotInheritable Class MesaMetadata
        Private Sub New()
            MyBase.New()
        End Sub


        <Display(Name:="Código")>
        Public Property ID As Integer

        <Required(ErrorMessage:="El nombre es un dato requerido.")>
        <Display(Name:="         Nombre")>
        Public Property Nombre As String

        <Display(Name:="Centro costos")>
        Public Property Ccostos As String

        <Display(Name:="Cuenta Contable")>
        Public Property CuentaContable As String

        <Required(ErrorMessage:="La Ciudad es un dato requerido.")>
        <Display(Name:="Ciudad")>
        Public Property IdPoblacion As Integer

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Mesa")>
        Public Property IdMesa As Integer

        <Display(Name:="Gerente Mesa")>
        Public Property GerenteMesa As String

    End Class
End Class
<MetadataTypeAttribute(GetType(Instalacio.InstalacioMetadata))>
Partial Public Class Instalacio
    Friend NotInheritable Class InstalacioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        'Pestaña General
        <Display(Name:="Bolsa")>
        Public Property IDBolsa As Integer
        <Display(Name:="Ciudad")>
        Public Property IdPoblacion As Integer

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
        <Display(Name:="Iva Comisión (%)")>
        Public Property IvaComision As Double

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
        <Display(Name:="ReteIva (%)")>
        Public Property ReteIva As Nullable(Of Double)

        <Display(Name:="Nombre Factura")>
        Public Property NombreCuenta As String

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
        <Display(Name:="IVA (%)")>
        Public Property IVA As Integer

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
        <Display(Name:="No Constitutivo de Renta")>
        Public Property PorcentajeNoConstitutivoDeRenta As Decimal

        'El que sigue esta dudoso *************************************************************************************************************************
        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
        <Display(Name:="Retención (%)")>
        Public Property RteFuente As Integer

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
        <Display(Name:="GMF Inferior")>
        Public Property GMFInferior As Nullable(Of Double)

        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
        <Display(Name:="Nit")>
        Public Property NitComisionista As String

        'Falta el campo Código *************************************************************************************************************************
        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
        <Display(Name:="Código Entidad UIAF")>
        Public Property CodigoEntidadUIAF As String

        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
        <Display(Name:="Tipo Entidad UIAF")>
        Public Property TipoEntidadUIAF As String

        <Display(Name:="Cierre")>
        Public Property Cierre As Nullable(Of DateTime)

        <Display(Name:="Cuentas Traslado DB")>
        Public Property CtaContableTrasladoDB As String

        <Display(Name:="Cuentas Traslado CR")>
        Public Property CtaContableTrasladoCR As String
        'Fin Pestaña General

        'Pestaña Clientes
        <Display(Name:="Automático")>
        Public Property ClientesAutomatico As Boolean
        <Display(Name:="Tipo Identif. (CC, Nit, Otros)")>
        Public Property ClientesCedula As Boolean
        <Display(Name:="Utilizar concepto de cliente agrupador")>
        Public Property ClientesAgrupados As Nullable(Of Boolean)
        <Display(Name:="Cuentas Bancarias")>
        Public Property CuentasBancarias As Boolean
        <Display(Name:="Representante Legal")>
        Public Property RepresentanteLegal As Boolean
        'Fin Pestaña Clientes

        'Pestaña parametros otros temas
        <Display(Name:="Nombre Servidor Contabilidad")>
        Public Property Servidor As String
        <Display(Name:="Base de Datos de Contabilidad")>
        Public Property BaseDatos As String
        <Display(Name:="Usuario")>
        Public Property Owner As String
        <Display(Name:="Código Compañía")>
        Public Property Compania As Nullable(Of Integer)
        <Display(Name:="Cuenta Contable Clientes")>
        Public Property CtaContableClientes As String
        <Display(Name:="Ruta")>
        Public Property PathActualiza As String
        <Display(Name:="Nombre Servidor Bus")>
        Public Property ServidorBus As String
        <Display(Name:="Base de Datos de Bus")>
        Public Property BaseDatosBus As String
        <Display(Name:="Usuario")>
        Public Property OwnerBus As String
        <Display(Name:="Retención Comisión")>
        Public Property RteComision As Double
        <Display(Name:="Retención ICA")>
        Public Property RteICA As Double
        'Fin Pestaña parametros otros temas

        'Pestaña Reportes 1
        <Display(Name:="Linea")>
        Public Property Linea1 As String
        <Display(Name:="Linea1")>
        Public Property Enca11 As String
        <Display(Name:="Linea2")>
        Public Property Enca12 As String
        <Display(Name:="Linea3")>
        Public Property Enca21 As String
        <Display(Name:="Linea4")>
        Public Property Enca22 As String
        <Display(Name:="Linea5")>
        Public Property Enca31 As String
        <Display(Name:="Linea6")>
        Public Property Enca32 As String
        <Display(Name:="Linea7")>
        Public Property Enca41 As String
        <Display(Name:="Linea8")>
        Public Property Enca42 As String
        <Display(Name:="Resolución")>
        Public Property Resolucion As String
        <Display(Name:="Observ.1")>
        Public Property Observacion1 As String
        <Display(Name:="Observ.2")>
        Public Property Observacion2 As String
        <Display(Name:="Def.Cliente")>
        Public Property DefensorCliente As String
        <Display(Name:="Solicitar Rango de Clientes en Informe de Extracto")>
        Public Property ReporteExtractoClientePedirRangos As Nullable(Of Boolean)
        <Display(Name:="Usar Rangos Financieros en Pantalla de Actualización de Clientes")>
        Public Property DatosFinancieros As Nullable(Of Boolean)
        'Fin Pestaña Reportes 1

        'Pestaña Reportes 2
        <Display(Name:="Recibos Caja")>
        Public Property EncaCaj As Boolean
        <Display(Name:="Comprobantes Egreso")>
        Public Property EncaEgr As Boolean
        <Display(Name:="Notas Contables")>
        Public Property EncaNot As Boolean
        <Display(Name:="Recibos Títulos")>
        Public Property EncaTit As Boolean
        <Display(Name:="Facturas")>
        Public Property EncaFac As Boolean
        <Display(Name:="Facturas Banca")>
        Public Property EncaFacBca As Boolean
        <Display(Name:="Extractos de Cliente")>
        Public Property EncaExt As Boolean
        <Display(Name:="Custodias")>
        Public Property EncaCus As Boolean
        <Display(Name:="Recibos Caja")>
        Public Property RCLineas As Boolean
        <Display(Name:="Comprobantes Egreso")>
        Public Property CELineas As Boolean
        <Display(Name:="Notas Contables")>
        Public Property NCLineas As Boolean
        <Display(Name:="Recibos Títulos")>
        Public Property TITLineas As Boolean
        <Display(Name:="Facturas")>
        Public Property FacLineas As Boolean
        <Display(Name:="Facturas Banca")>
        Public Property FacBcaLineas As Boolean
        <Display(Name:="Extractos de Cliente")>
        Public Property EXTLineas As Boolean
        <Display(Name:="Custodias")>
        Public Property CusLineas As Boolean
        <Display(Name:="En Entregas")>
        Public Property UsuarioEntregas As Boolean
        <Display(Name:="En Recibos de Título")>
        Public Property UsuarioRecibido As Boolean
        <Display(Name:="En Custodias")>
        Public Property UsuarioCustodia As Boolean
        <Display(Name:="En Sobrantes")>
        Public Property UsuarioSobrantes As Boolean
        'El que sigue esta dudoso *************************************************************************************************************************
        <Display(Name:="En Facturas")>
        Public Property Usuario As Boolean
        <Display(Name:="URL")>
        Public Property URL As String
        <Display(Name:="Path")>
        Public Property Path As String
        'Fin Pestaña Reportes 2

        'Pestaña Bolsa
        <Display(Name:="Permitir Modificar la Fecha")>
        Public Property FechaOrden As Boolean
        <Display(Name:="Mostrar Solo los Receptores del Cliente")>
        Public Property Receptores As Boolean
        <Display(Name:="Cargar Receptor por Defecto del Cliente en la Orden")>
        Public Property CargarReceptorCliente As Boolean
        <Display(Name:="Requerir el Ingreso de Ordenantes")>
        Public Property Ordenantes As Nullable(Of Boolean)
        <Display(Name:="Servicio Fijo en Bolsa")>
        Public Property SerBolsaFijo As Nullable(Of Double)
        <Display(Name:="Servicio Bolsa Acciones")>
        Public Property SerBolsaFijoAcciones As Nullable(Of Double)
        <Display(Name:="Tope Servicio Bolsa Acciones")>
        Public Property TopeSerBolsaFijoAcciones As Nullable(Of Double)
        <Display(Name:="Fecha Límite títulos")>
        Public Property FechaLimite As Nullable(Of DateTime)
        'Fin Pestaña Bolsa

        'Pestaña OPCF
        <Display(Name:="Valor Contrato")>
        Public Property ValorContrato As Nullable(Of Double)
        <Display(Name:="Día de la Semana")>
        Public Property DiaSemana As Nullable(Of Integer)
        <Display(Name:="Porcentaje Garantía")>
        Public Property PorcentajeGarantia As Nullable(Of Double)
        <Display(Name:="Tarifa RteFuente")>
        Public Property TarifaRteFuente As Nullable(Of Double)
        'Fin Pestaña OPCF

        'Pestaña Divisas
        <Display(Name:="Código del IMC")>
        Public Property CodigoIMC As Nullable(Of Integer)
        <Display(Name:="Valor Inicial")>
        Public Property ValorInicial As Nullable(Of Double)
        <Display(Name:="Tasa Inicial")>
        Public Property TasaInicial As Nullable(Of Double)
        'Fin Pestaña Divisas

        'Pestaña Yankees
        <Display(Name:="Deposito Extranjero")>
        Public Property DepositoExtranjero As String
        <Display(Name:="Custodio Local")>
        Public Property CustodioLocal As String
        'Fin Pestaña Yankees

        'Pestaña OTC
        <Display(Name:="Permitir Aplazar Fecha de Cumplimiento")>
        Public Property AplazarOTC As Boolean
        'Fin Pestaña OTC

        'Pestaña Tesorería
        <Display(Name:="Compañia Principal")>
        Public Property CompaniaPrincipal As Nullable(Of Integer)
        <Display(Name:="Validar Giro de Cheque (Comprobante de Egreso). El cliente debe tener saldo a su favor para no sobregirarse")>
        Public Property ValSobregiroCE As Boolean
        <Display(Name:="Validar Realización de Nota Debito. El cliente debe tener saldo a su favor para no sobregirarse")>
        Public Property ValSobregiroNC As Boolean
        <Display(Name:="Imprimir por fechas, documentos de tesorería")>
        Public Property ImpDocTesoreria As Nullable(Of Boolean)
        <Display(Name:="Validar Totales en Cuentas Contables de Supervalores")>
        Public Property ValidaCuentaSuperVal As Boolean
        <Display(Name:="Registrar Manualmente la Descripción del Detalle de Tesorería (Concepto)")>
        Public Property ConceptoDetalleTesoreriaManual As Boolean
        <Required(ErrorMessage:="El Tipo es un dato requerido.")>
        <Display(Name:="Tipo")>
        Public Property Tipo As String
        <Display(Name:="Cuenta Contable")>
        Public Property CtaContable As String
        <Display(Name:="C. de Costo")>
        Public Property CCosto As String
        <Display(Name:="C. Contable Contraparte")>
        Public Property CtaContableContraparte As String
        <Display(Name:="C. de Costo Contraparte")>
        Public Property CCostoContraparte As String
        'Fin Pestaña Tesorería


        <Display(Name:="CodigoEntidad")>
        Public Property CodigoEntidad As String

        <Display(Name:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        '<Required(ErrorMessage:="El nombre es un dato requerido.")> _
        <Display(Name:="Código")>
        Public Property IdContraparteOTC As String

        <Display(Name:="GMF")>
        Public Property GMS As Nullable(Of Double)

        <Display(Name:="UltimaVersion")>
        Public Property UltimaVersion As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario")>
        Public Property strUsuario As String

        <Display(Name:="TipoEntidad")>
        Public Property TipoEntidad As String

        <Display(Name:="ReceptorSuc")>
        Public Property ReceptorSuc As String

        <Display(Name:="NroUsu")>
        Public Property NroUsu As String

        <Display(Name:="ServidorNacional")>
        Public Property ServidorNacional As String

        <Display(Name:="Compania")>
        Public Property strCompania As String

        <Display(Name:="CompaniaM")>
        Public Property CompaniaM As String

        <Display(Name:="Multicuenta")>
        Public Property Multicuenta As String

        <Display(Name:="MáximoValor")>
        Public Property MaximoValor As Nullable(Of Decimal)

        <Display(Name:="UrlReportesBus")>
        Public Property UrlReportesBus As String

        <Display(Name:="RutaReportesBus")>
        Public Property RutaReportesBus As String

        <Display(Name:="Cuentas NotasCxC")>
        Public Property CtaContableContraparteNotasCxC As String

        <Display(Name:="Tipos NotasCxC")>
        Public Property tipoNotasCxC As String

        <Display(Name:="Instalacion")>
        Public Property IDInstalacion As Integer

    End Class
End Class

#End Region



#End Region

#Region "JAQUELINE RESTREPO C. MAESTROS"

#Region "Versión 1.0.0."

<MetadataTypeAttribute(GetType(Custodi.CustodiMetadata))>
Partial Public Class Custodi
    Friend NotInheritable Class CustodiMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Código")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="Local")>
        Public Property Local As Boolean

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
<MetadataTypeAttribute(GetType(Profesione.ProfesioneMetadata))>
Partial Public Class Profesione
    Friend NotInheritable Class ProfesioneMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Código")>
        Public Property CodigoProfesion As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Profesion")>
        Public Property IDProfesion As Integer

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
<MetadataTypeAttribute(GetType(Codigos_CII.Codigos_CIIMetadata))>
Partial Public Class Codigos_CII
    Friend NotInheritable Class Codigos_CIIMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="Este campo es requerido. (Código)")>
        <StringLength(10, ErrorMessage:="El campo Código permite una longitud máxima de 10 caracteres.")>
        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
        <Display(Name:="Código")>
        Public Property Codigo As String

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(255, ErrorMessage:="El campo Nombre permite una longitud máxima de 255 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property Descripcion As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Clasificación")>
        Public Property ClasificacionCIIU As Integer

        <Display(Name:="CodigoCIIU")>
        Public Property IDCodigoCIIU As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(DoctosRequerido.DoctosRequeridoMetadata))>
Partial Public Class DoctosRequerido
    Friend NotInheritable Class DoctosRequeridoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="Este campo es requerido. (IDDocumento)")>
        <Display(Name:="Documento")>
        Public Property IDDocumento As Int16

        <Required(ErrorMessage:="Este campo es requerido. (Código)")>
        <StringLength(20, ErrorMessage:="El campo Código permite una longitud máxima de 20 caracteres.")>
        <Display(Name:="Código")>
        Public Property CodigoDocto As String

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(100, ErrorMessage:="El campo Nombre permite una longitud máxima de 100 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property NombreDocumento As String

        <Display(Name:="Requerido")>
        Public Property Requerido As Boolean

        <Display(Name:="Fecha Ini. Vigencia")>
        Public Property FechaIniVigencia As Boolean

        <Display(Name:="Fecha Fin Vigencia")>
        Public Property FechaFinVigencia As Boolean

        <Display(Name:="Activo")>
        Public Property DocuActivo As Boolean

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

    End Class
End Class
<MetadataTypeAttribute(GetType(TipoReferencia.TipoReferenciaMetadata))>
Partial Public Class TipoReferencia
    Friend NotInheritable Class TipoReferenciaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")>
        <Display(Name:="Código")>
        Public Property IDCodigo As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Retorno)")>
        <StringLength(2, ErrorMessage:="El campo {0} permite una longitud máxima de 2 caracteres.")>
        <Display(Name:="Código Retorno")>
        Public Property IDCodigoRetorno As String

        <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50 caracteres.")>
        <Display(Name:="Descripción")>
        Public Property Descripcion As String

        <StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1 caracter.")>
        <A2RegularExpression("[C|V|A|c|v|a]{1}", ErrorMessage:="El {0} debe ser Compra(C)/Venta(V)/Ambos(A)")>
        <Display(Name:="Tipo Clasificación")>
        Public Property tipoClasificacion As String

        <Display(Name:="Formulario Uno")>
        Public Property Formulario1 As Boolean

        <Display(Name:="Formulario Dos")>
        Public Property Formulario2 As Boolean

        <Display(Name:="Formulario Tres")>
        Public Property Formulario3 As Boolean

        <Display(Name:="Formulario Cuatro")>
        Public Property Formulario4 As Boolean

        <Display(Name:="Formulario Cinco")>
        Public Property Formulario5 As Boolean

        <Display(Name:="IVA")>
        Public Property CalculaIVA As Boolean

        <Display(Name:="Mensajes")>
        Public Property Mensajes As Boolean

        <Display(Name:="Retención")>
        Public Property CalculaRetencion As Boolean

        <Display(Name:="Cantidad Negociada")>
        Public Property CantidadNegociada As Nullable(Of Decimal)

        <Display(Name:="Meses Dcto Transporte")>
        Public Property NroMesesDctoTransporte As Nullable(Of Byte)

        <Required(ErrorMessage:="Este campo es requerido. (Consecutivo)")>
        <A2RegularExpression("[a-z|A-Z|ñ|Ñ]{3}[0-9]{4}[0-9]{2}", ErrorMessage:="El consecutivo debe estar conformado por 3 letras y 6 números de los cuales se recomienda que los 2 últimos hagan referencia al año. Ej. abc123411 donde 11 equivale al año 2011.")>
        <StringLength(9, ErrorMessage:="El campo {0} permite una longitud máxima de 9 caracteres.")>
        <Display(Name:="Consecutivo")>
        Public Property Consecutivo As String

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

    End Class
End Class

#End Region

#Region "V1.0.1"

<MetadataTypeAttribute(GetType(ClientesExterno.ClientesExternoMetadata))>
Partial Public Class ClientesExterno
    Friend NotInheritable Class ClientesExternoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        '<Required(ErrorMessage:="Este campo es requerido. (Código)")> _
        <Display(Name:="Código")>
        Public Property ID As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")>
        <Required(ErrorMessage:="Este campo es requerido. (Vendedor)")>
        <Display(Name:="Vendedor")>
        Public Property Vendedor As String

        '<A2RegularExpression("^[1-9]2$", ErrorMessage:="Este campo es requerido (Depósito Extranjero)")> _
        <Required(ErrorMessage:="Este campo es requerido. (Depósito Extranjero)")>
        <Display(Name:="Depósito Extranjero")>
        Public Property IDDepositoExtranjero As Integer

        <StringLength(20, ErrorMessage:="El campo {0} permite una longitud máxima de 20.")>
        <Display(Name:="Número Cuenta")>
        Public Property NumeroCuenta As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre Titular)")>
        <Display(Name:="Nombre Titular")>
        Public Property NombreTitular As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="ClienteExt")>
        Public Property IDClienteExt As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(ConceptosTesoreri.ConceptosTesoreriMetadata))>
Partial Public Class ConceptosTesoreri
    Friend NotInheritable Class ConceptosTesoreriMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Código")>
        Public Property IDConcepto As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Detalle)")>
        <Display(Name:="Detalle")>
        Public Property Detalle As String

        <Required(ErrorMessage:="Este campo es requerido. (Aplica A)")>
        <Display(Name:="Aplica A")>
        Public Property AplicaA As String

        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Cuenta Contable")>
        Public Property CuentaContable As String

        <Display(Name:="Activo")>
        Public Property Activo As Boolean

        <Display(Name:="Parámetro Contable")>
        Public Property ParametroContable As String

        <Display(Name:="Nit Tercero")>
        Public Property NitTercero As String

        <Display(Name:="Maneja Cliente")>
        Public Property ManejaCliente As String

        <Display(Name:="Tipo Movimiento Tesoreria")>
        Public Property TipoMovimientoTesoreria As String

        <Display(Name:="Retencion")>
        Public Property Retencion As String

    End Class
End Class
<MetadataTypeAttribute(GetType(Consecutivo.ConsecutivoMetadata))>
Partial Public Class Consecutivo
    Friend NotInheritable Class ConsecutivoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Módulo)")>
        <Display(Name:="Módulo")>
        Public Property IDOwner As String

        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property NombreConsecutivo As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
        <Display(Name:="Descripción")>
        Public Property Descripcion As String

        <Required(ErrorMessage:="Este campo es requerido. (Vr. Mínimo)")>
        <Range(0, 2147483647, ErrorMessage:="{0} debe ser mayor o igual que 0")>
        <Display(Name:="Vr. Mínimo")>
        Public Property Minimo As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Vr. Máximo)")>
        <Range(0, 2147483647, ErrorMessage:="{0} debe ser mayor que 0")>
        <Display(Name:="Vr. Máximo")>
        Public Property Maximo As Integer

        <Display(Name:="Vr. Actual")>
        Public Property Actual As Integer

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Activo")>
        Public Property Activo As Boolean

        '<Required(ErrorMessage:="Este campo es requerido. (IdConsecutivos)")> _
        <Display(Name:="Consecutivos")>
        Public Property IdConsecutivos As Integer

    End Class
End Class

<MetadataTypeAttribute(GetType(DepositosExtranjero.DepositosExtranjeroMetadata))>
Partial Public Class DepositosExtranjero
    Friend NotInheritable Class DepositosExtranjeroMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")>
        <Display(Name:="Código")>
        Public Property IDdeposito As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (País)")>
        <Display(Name:="País")>
        Public Property IDPais As Integer

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
<MetadataTypeAttribute(GetType(PrefijosFactura.PrefijosFacturaMetadata))>
Partial Public Class PrefijosFactura
    Friend NotInheritable Class PrefijosFacturaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <StringLength(5, ErrorMessage:="El campo {0} permite una longitud máxima de 5 caracteres.")>
        <Display(Name:="Prefijo")>
        Public Property Prefijo As String

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15 caracteres.")>
        <Display(Name:="Consecutivo")>
        Public Property NombreConsecutivo As String

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50 caracteres.")>
        <Display(Name:="Descripción")>
        Public Property Descripcion As String

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Tipo")>
        Public Property Tipo As String

        <Display(Name:="Cuenta")>
        <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25 caracteres.")>
        Public Property NombreCuenta As String

        <Display(Name:="Texto Resolución")>
        <StringLength(80, ErrorMessage:="El campo {0} permite una longitud máxima de 80 caracteres.")>
        Public Property TextoResolucion As String

        <Display(Name:="Texto Intervalo")>
        <StringLength(80, ErrorMessage:="El campo {0} permite una longitud máxima de 80 caracteres.")>
        Public Property IntervaloRes As String

        <Display(Name:="Responsabilidad IVA")>
        <StringLength(80, ErrorMessage:="El campo {0} permite una longitud máxima de 80 caracteres.")>
        Public Property ResponsabilidadIVA As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        '<Required(ErrorMessage:="Este campo es requerido. ({0})")> _
        <Display(Name:="Fecha de Vencimiento")>
        Public Property FechaVencimiento As Nullable(Of DateTime)

        <Display(Name:="Alarma activa")>
        Public Property Alarma As Nullable(Of Boolean)

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Resolución")>
        Public Property IDCodigoResolucion As Nullable(Of Integer)

        <Display(Name:="PrefijoFacturas")>
        Public Property IDPrefijoFacturas As Integer

        '<Range(1, 2147483647, ErrorMessage:="{0} debe ser mayor que 0")> _
        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Sucursal")>
        Public Property SucursalAplica As Integer

        <Display(Name:="Leyenda Facturas")>
        <StringLength(150, ErrorMessage:="El campo {0} permite una longitud máxima de 150 caracteres.")>
        Public Property Resolucion As String

        <Display(Name:="AnoRes")>
        Public Property AnoRes As String

        'cfma validamos los campos para que sean diligenciados correctamente
        <Display(Name:="Vigilado Por")>
        <StringLength(150, ErrorMessage:="El campo {0} permite una longitud máxima de 8000 caracteres.")>
        Public Property Vigiladopor As String

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Fecha Desde")>
        Public Property FechaDesde As Nullable(Of DateTime)

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Fecha Hasta")>
        Public Property FechaHasta As Nullable(Of DateTime)

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Dias Previos")>
        Public Property numDiasPreviosNoti As Integer

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Cant.Cons Previos")>
        Public Property numCantConsPrevNoti As Integer

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Días Periodicidad")>
        Public Property numDiasPeriodicidadNoti As Integer

        <Display(Name:="Destinatarios")>
        <StringLength(150, ErrorMessage:="El campo {0} permite una longitud máxima de 8000 caracteres.")>
        Public Property DestinatariosNoti As String



        'cfma




    End Class
End Class
<MetadataTypeAttribute(GetType(Empleado.EmpleadoMetadata))>
Partial Public Class Empleado
    Friend NotInheritable Class EmpleadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Código")>
        Public Property IDEmpleado As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(250, ErrorMessage:="El campo {0} permite una longitud máxima de 250 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (Número Documento)")>
        <StringLength(20, ErrorMessage:="El campo {0} permite una longitud máxima de 20 caracteres.")>
        <Display(Name:="Número Documento")>
        Public Property NroDocumento As String

        <StringLength(4, ErrorMessage:="El campo {0} permite una longitud máxima de 4 caracteres.")>
        <Display(Name:="Cod. Receptor")>
        Public Property IDReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Login OyD)")>
        <Display(Name:="Login OyD")>
        Public Property Login As String

        <Display(Name:="Cargo")>
        Public Property IDCargo As Integer

        <Display(Name:="Operador Bolsa")>
        Public Property AccesoOperadorBolsa As Nullable(Of Boolean)

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Activo")>
        Public Property Activo As Boolean

        <Display(Name:="Email", Description:="Email")>
        Public Property strEmail As String

        <Display(Name:="Tipo Identificación", Description:="Tipo Identificación")>
        Public Property TipoIdentificacion As String

    End Class
End Class
<MetadataTypeAttribute(GetType(Inhabilitado.InhabilitadoMetadata))>
Partial Public Class Inhabilitado
    Friend NotInheritable Class InhabilitadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="idcomisionista", Description:="idcomisionista")>
        Public Property idcomisionista As Integer

        <Display(Name:="idsuccomisionista", Description:="idsuccomisionista")>
        Public Property idsuccomisionista As Integer

        '<Required(ErrorMessage:="Este campo es requerido. (Tipo Identificación)")>
        <Display(Name:="Tipo Identificación")>
        Public Property tipoidentificacion As String

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")> _
        <Required(ErrorMessage:="Este campo es requerido. (Doc. Identidad)")>
        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15 caracteres.")>
        <Display(Name:="Doc. Identidad")>
        Public Property nrodocumento As String

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50 caracteres.")>
        <Display(Name:="Nombre")>
        Public Property nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (Motivo Inhabilidad)")>
        <Display(Name:="Motivo Inhabilidad")>
        Public Property idconcepto As Integer

        <Required(ErrorMessage:="Este campo es requerido. ({0})")>
        <Display(Name:="Fecha")>
        Public Property ingreso As DateTime

        <Display(Name:="actualizacion", Description:="actualizacion")>
        Public Property actualizacion As DateTime

        <Display(Name:="usuario", Description:="usuario")>
        Public Property usuario As String

        <Display(Name:="Inhabilitado")>
        Public Property IDInhabilitado As Integer

    End Class
End Class

#End Region

#End Region

#Region "JHON BAYRON TORRES MAESTROS"

#Region "V1.0.1"

<MetadataTypeAttribute(GetType(UsuariosSucursale.UsuariosSucursaleMetadata))>
Partial Public Class UsuariosSucursale
    Friend NotInheritable Class UsuariosSucursaleMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Nombre_Usuario As String

        <Required(ErrorMessage:="Este campo es requerido. (Receptor)")>
        <Display(Name:="Receptor")>
        Public Property Receptor As String

        <Display(Name:="Sucursal")>
        Public Property IDSucursal As Integer

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="UsuariosSucursales")>
        Public Property IDUsuariosSucursales As Integer

        <Display(Name:="Prioridad")>
        Public Property Prioridad As System.Nullable(Of Integer)

        <Display(Name:="Nombre Receptor")>
        Public Property Nombre_Receptor As String

    End Class
End Class
<MetadataTypeAttribute(GetType(ClientesFondosPensione.ClientesFondosPensioneMetadata))>
Partial Public Class ClientesFondosPensione
    Friend NotInheritable Class ClientesFondosPensioneMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="Este campo es requerido. (Comitente)")>
        <Display(Name:="Comitente")>
        Public Property Comitente As String

        <Display(Name:="Clientesfondospensiones")>
        Public Property IDClientesfondospensiones As Integer



    End Class
End Class
<MetadataTypeAttribute(GetType(ViewClientes_Exento.ViewClientes_ExentoMetadata))>
Partial Public Class ViewClientes_Exento


    Friend NotInheritable Class ViewClientes_ExentoMetadata

        Private Sub New()
            MyBase.New()
        End Sub
        <StringLength(17, ErrorMessage:="El campo {0} permite una longitud máxima de 17 caracteres.")>
        <Required(ErrorMessage:="Este campo es requerido. (Comitente)")>
        <Display(Name:="Comitente")>
        Public Property Comitente As String
        <Display(Name:="Nombre")>
        Public Property Nombre As String
    End Class
End Class
<MetadataTypeAttribute(GetType(ViewClientes_Exentos_Consultar.ViewClientes_Exentos_ConsultarMetadata))>
Partial Public Class ViewClientes_Exentos_Consultar


    Friend NotInheritable Class ViewClientes_Exentos_ConsultarMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Codigo", Description:="Codigo")>
        Public Property Codigo As String

        <Display(Name:="Multicuenta", Description:="Multicuenta")>
        Public Property Multicuenta As String

        <Display(Name:="Nombre", Description:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="Numero", Description:="Numero")>
        Public Property Numero As String

        <Display(Name:="T", Description:="T")>
        Public Property T As Char
    End Class
End Class


#End Region
#Region "V1.0.2"

<MetadataTypeAttribute(GetType(ConceptosConsecutivo.ConceptosConsecutivoMetadata))>
Partial Public Class ConceptosConsecutivo
    Friend NotInheritable Class ConceptosConsecutivoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")>
        Public Property IdSucComisionista As Integer

        <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre Consecutivo)")>
        <Display(Name:="Nombre Consecutivo")>
        Public Property Consecutivo As String

        <Required(ErrorMessage:="Este campo es requerido. (Concepto)")>
        <Display(Name:="Concepto")>
        Public Property Concepto As Integer

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String


        <Display(Name:="Conceptosconsecutivos")>
        Public Property IDConceptosconsecutivos As Integer




    End Class
End Class
<MetadataTypeAttribute(GetType(ConsecutivosUsuario.ConsecutivosUsuarioMetadata))>
Partial Public Class ConsecutivosUsuario
    Friend NotInheritable Class ConsecutivosUsuarioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Editable(True)>
        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Editable(True)>
        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Editable(True)>
        <Required(ErrorMessage:="Este campo es requerido. (Usuario Consecutivo)")>
        <Display(Name:="Usuario Consecutivo")>
        Public Property Usuario_Consecutivo As String

        <Editable(True)>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre Consecutivo)")>
        <Display(Name:="Nombre Consecutivo")>
        Public Property Nombre_Consecutivo As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="ConsecutivosUsuarios")>
        Public Property IDConsecutivosUsuarios As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(Tarifa.TarifaMetadata))>
Partial Public Class Tarifa
    Friend NotInheritable Class TarifaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Codigo)")>
        <Display(Name:="Código")>
        Public Property ID As Integer

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Valor")>
        Public Property Valor As Nullable(Of Double)

        <Display(Name:="Simbolo")>
        Public Property Simbolo As String

        <Display(Name:="Tarifas")>
        Public Property IDTarifas As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(DetalleTarifa.DetalleTarifaMetadata))>
Partial Public Class DetalleTarifa
    Friend NotInheritable Class DetalleTarifaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Codigo)")>
        <Display(Name:="Código")>
        Public Property Codigo As Integer

        <Required(ErrorMessage:="Este campo es requerido. (FechaValor)")>
        <Display(Name:="FechaValor")>
        Public Property FechaValor As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Valor)")>
        <Display(Name:="Valor")>
        Public Property Valor As Nullable(Of Decimal)

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="DetalleTarifas")>
        Public Property IDDetalleTarifas As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(Moneda.MonedaMetadata))>
Partial Public Class Moneda
    Friend NotInheritable Class MonedaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="Código")>
        Public Property Codigo As Integer

        <StringLength(5, ErrorMessage:="El campo {0} permite una longitud máxima de 5.")>
        <Required(ErrorMessage:="Este campo es requerido. (Codigo Internacional)")>
        <Display(Name:="Código Internacional")>
        Public Property Codigo_internacional As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Descripcion)")>
        <Display(Name:="Descripción")>
        Public Property Descripcion As String


        <Required(ErrorMessage:="Este campo es requerido. (Conversión Dólar)")>
        <Display(Name:="Conversión Dólar")>
        Public Property Convercion_Dolar As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (Nros. Decimales)")>
        <Display(Name:="Nros. Decimales")>
        Public Property Nro_Decimales As Byte

        <Required(ErrorMessage:="Este campo es requerido. (Dias Cumplimiento)")>
        <Display(Name:="Dias Cumplimiento")>
        Public Property Dias_Cumplimiento As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Valor Base IVA)")>
        <Display(Name:="Valor Base IVA")>
        Public Property ValorBase_IVA As Nullable(Of Double)

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Mercado Integrado)")>
        <Display(Name:="Mercado Integrado")>
        Public Property Mercado_Integrado As Boolean

        <Display(Name:="Código Divisas")>
        Public Property CodDivisa As DateTime


    End Class
End Class
<MetadataTypeAttribute(GetType(MonedaValo.MonedaValoMetadata))>
Partial Public Class MonedaValo
    Friend NotInheritable Class MonedaValoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")>
        <Display(Name:="Código")>
        Public Property Codigo As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Valor)")>
        <Display(Name:="Fecha Valor")>
        Public Property FechaValor As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Valor Moneda Local)")>
        <Display(Name:="Valor Moneda Local")>
        Public Property Valor_Moneda_Local As Decimal

        <Required(ErrorMessage:="Este campo es requerido. (Base IVA Diario)")>
        <Display(Name:="Base IVA Diario")>
        Public Property Base_IVA_Diario As Nullable(Of Decimal)

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Monedavalor")>
        Public Property IDMonedavalor As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(Receptore.ReceptoreMetadata))>
Partial Public Class Receptore
    Friend NotInheritable Class ReceptoreMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer


        <Required(ErrorMessage:="Este campo es requerido. (Sucursal)")>
        <Display(Name:="Sucursal", Description:="Sucursal")>
        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
        Public Property Sucursal As Nullable(Of Integer)

        <StringLength(4, ErrorMessage:="El campo {0} permite una longitud máxima de 4.")>
        <Required(ErrorMessage:="Este campo es requerido. (Codigo)")>
        <Display(Name:="Código")>
        Public Property Codigo As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (Activo)")>
        <Display(Name:="Activo")>
        Public Property Activo As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (Tipo)")>
        <Display(Name:="Tipo", Description:="Tipo")>
        Public Property Tipo As String

        <Display(Name:="Fecha", Description:="Estado")>
        Public Property Estado As Nullable(Of DateTime)

        <StringLength(20, ErrorMessage:="El campo {0} permite una longitud máxima de 20.")>
        <Display(Name:="Centro costos")>
        Public Property Centro_costos As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Login")>
        Public Property Login As String

        <Required(ErrorMessage:="Este campo es requerido. (Lider Mesa)")>
        <Display(Name:="Líder Mesa")>
        Public Property Lider_Mesa As Boolean

        <Display(Name:="Código Mesa")>
        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
        <Required(ErrorMessage:="Este campo es requerido.")>
        Public Property Codigo_Mesa As Integer

        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")>
        <Required(ErrorMessage:="Este campo es requerido.")>
        <Display(Name:="Número Documento")>
        Public Property Numero_Documento As String

        '<StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")> _
        '<A2RegularExpression("^[a-z0-9_\+-]+(\.[a-z0-9_\+-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*\.([a-z]{2,4})$", ErrorMessage:="El campo {0} Tiene un formato incorrecto")> _
        <A2RegularExpression("\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage:="La dirección de correo electrónico ingresada no es válida.")>
        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud maxima de 50.")>
        <Display(Name:="E Mail")>
        Public Property E_Mail As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        'cfma
        <Required(ErrorMessage:="Este campo es requerido. (Oficina)")>
        <Display(Name:="Oficina")>
        Public Property IdOficina As Nullable(Of Integer)
        'cfma

        <Display(Name:="Receptor")>
        Public Property IdReceptor As Nullable(Of Integer)

        <Display(Name:="NumTrader")>
        Public Property NumTrader As String

        <Display(Name:="CodSetFX")>
        Public Property CodSetFX As String

        <Display(Name:="RepresentanteLegalOtrosNegocios")>
        Public Property RepresentanteLegalOtrosNegocios As Nullable(Of Boolean)

        <Display(Name:="Receptores")>
        Public Property IdReceptores As Integer

        <StringLength(12, ErrorMessage:="El campo {0} permite una longitud máxima de 12.")>
        <Display(Name:="Código Safyr")>
        Public Property IDReceptorSafyr As String

    End Class
End Class
<MetadataTypeAttribute(GetType(ReceptoresSistema.ReceptoresSistemaMetadata))>
Partial Public Class ReceptoresSistema
    Friend NotInheritable Class ReceptoresSistemaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <StringLength(4, ErrorMessage:="El campo {0} permite una longitud máxima de 4.")>
        <Required(ErrorMessage:="Este campo es requerido. (Codigo)")>
        <Display(Name:="Receptor")>
        Public Property Codigo As String

        <StringLength(20, ErrorMessage:="El campo {0} permite una longitud máxima de 20.")>
        <Required(ErrorMessage:="Este campo es requerido. (Codigo Sistema)")>
        <Display(Name:="Sistema")>
        Public Property Codigo_Sistema As String

        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")> _

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Required(ErrorMessage:="Este campo es requerido. (Valor Sistema)")>
        <Display(Name:="Valor Sistema")>
        Public Property Valor_Sistema As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Receptores Sistemas")>
        Public Property IDReceptoresSistemas As Integer

        <Display(Name:="Receptor")>
        Public Property Nombre As String

        <Display(Name:="Monto límite")>
        Public Property MontoLimite As Double

    End Class
End Class
<MetadataTypeAttribute(GetType(UsuariosFechaCierr.UsuariosFechaCierrMetadata))>
Partial Public Class UsuariosFechaCierr
    Friend NotInheritable Class UsuariosFechaCierrMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (IDSucComisionista)")>
        <Display(Name:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre Usuario)")>
        <Display(Name:="Nombre Usuario")>
        Public Property Nombre_Usuario As String

        <Required(ErrorMessage:="Este campo es requerido. (Modulo)")>
        <Display(Name:="Módulo")>
        Public Property Modulo As String

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Cierre)")>
        <Display(Name:="Fecha abierta al usuario")>
        Public Property Fecha_Cierre As DateTime

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="ID")>
        Public Property ID As Integer

    End Class
End Class
<MetadataTypeAttribute(GetType(CodigosOtrosSistema.CodigosOtrosSistemaMetadata))>
Partial Public Class CodigosOtrosSistema
    Friend NotInheritable Class CodigosOtrosSistemaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Id")>
        Public Property Id As Integer

        <Display(Name:="IdComisionista")>
        Public Property IdComisionista As Nullable(Of Integer)

        <Display(Name:="IdSucComisionista")>
        Public Property IdSucComisionista As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Comitente)")>
        <Display(Name:="Comitente")>
        Public Property Comitente As String

        <Required(ErrorMessage:="Este campo es requerido. (Sistema)")>
        <Display(Name:="Sistema")>
        Public Property Sistema As String

        <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30")>
        <Required(ErrorMessage:="Este campo es requerido. (CodigoSistema)")>
        <Display(Name:="Código Sistema")>
        Public Property CodigoSistema As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
<MetadataTypeAttribute(GetType(CuentasDecevalPorAgrupado.CuentasDecevalPorAgrupadoMetadata))>
Partial Public Class CuentasDecevalPorAgrupado
    Friend NotInheritable Class CuentasDecevalPorAgrupadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IdComisionista", Description:="IdComisionista")>
        Public Property IdComisionista As Integer

        <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")>
        Public Property IdSucComisionista As Integer

        '<Required(ErrorMessage:="Este campo es requerido. (TipoIdComitente)")> _
        <Display(Name:="TipoIdComitente")>
        Public Property TipoIdComitente As String

        '<Required(ErrorMessage:="Este campo es requerido. (Nro. Documento)")> _
        <Display(Name:="Nro. documento")>
        Public Property NroDocumento As String

        '<Required(ErrorMessage:="Este campo es requerido. (Comitente)")> _
        <Display(Name:="Comitente")>
        Public Property Comitente As String

        <Display(Name:="Prefijo")>
        Public Property Prefijo As String

        <Required(ErrorMessage:="Este campo es requerido. (Cuenta Deceval)")>
        <Display(Name:="Cuenta depósito")>
        Public Property CuentaDeceval As Integer

        <Display(Name:="Conector 1")>
        Public Property Conector1 As String

        <Display(Name:="TipoIdBenef1")>
        Public Property TipoIdBenef1 As String

        <Display(Name:="Primer beneficiario")>
        Public Property NroDocBenef1 As Nullable(Of Decimal)

        <Display(Name:="Conector 2")>
        Public Property Conector2 As String

        <Display(Name:="TipoIdBenef2")>
        Public Property TipoIdBenef2 As String

        <Display(Name:="Segundo beneficiario")>
        Public Property NroDocBenef2 As Nullable(Of Decimal)

        <Required(ErrorMessage:="Este campo es requerido. (Depósito)")>
        <Display(Name:="Depósito")>
        Public Property Deposito As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Required(ErrorMessage:="Este campo es requerido. (Cuenta múltiple)")>
        <Display(Name:="Cuenta múltiple")>
        Public Property CuentaPorCliente As Boolean

        <Display(Name:="intermedia")>
        Public Property intermedia As Nullable(Of Integer)

        <Display(Name:="Cuenta Principal Intermediario (DCV)")>
        Public Property CuentaPrincipalDCV As String

        <Display(Name:="CuentasDeceval")>
        Public Property IDCuentasDeceval As Integer

        <Display(Name:="Nombre cliente")>
        Public Property NomCliente As String

    End Class
End Class
<MetadataTypeAttribute(GetType(ConsultaExiste.ConsultaExisteMetadata))>
Partial Public Class ConsultaExiste
    Friend NotInheritable Class ConsultaExisteMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Existe", Description:="Existe")>
        Public Property Existe As Integer

        <Display(Name:="StrId", Description:="StrId")>
        Public Property StrId As String


    End Class
End Class
<MetadataTypeAttribute(GetType(Ordene.OrdeneMetadata))>
Partial Public Class Ordene
    Friend NotInheritable Class OrdeneMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")>
        Public Property IdSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo)")>
        <Display(Name:="Tipo", Description:="Tipo")>
        Public Property Tipo As String

        <Required(ErrorMessage:="Este campo es requerido. (Clase)")>
        <Display(Name:="Clase", Description:="Clase")>
        Public Property Clase As String

        <Required(ErrorMessage:="Este campo es requerido. (ID)")>
        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Version)")>
        <Display(Name:="Version", Description:="Version")>
        Public Property Version As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Ordinaria)")>
        <Display(Name:="Ordinaria", Description:="Ordinaria")>
        Public Property Ordinaria As Boolean

        <Display(Name:="Objeto", Description:="Objeto")>
        Public Property Objeto As String

        <Required(ErrorMessage:="Este campo es requerido. (Repo)")>
        <Display(Name:="Repo", Description:="Repo")>
        Public Property Repo As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (Renovacion)")>
        <Display(Name:="Renovacion", Description:="Renovacion")>
        Public Property Renovacion As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")>
        <Display(Name:="Comitente")>
        Public Property IDComitente As String

        <Required(ErrorMessage:="Este campo es requerido. (Ordenante)")>
        <Display(Name:="Ordenante")>
        Public Property IDOrdenante As String

        <Required(ErrorMessage:="Este campo es requerido. (ComisionPactada)")>
        <Display(Name:="ComisionPactada")>
        Public Property ComisionPactada As Double

        <Required(ErrorMessage:="Este campo es requerido. (CondicionesNegociacion)")>
        <Display(Name:="CondicionesNegociacion")>
        Public Property CondicionesNegociacion As String

        <Required(ErrorMessage:="Este campo es requerido. (TipoLimite)")>
        <Display(Name:="TipoLimite")>
        Public Property TipoLimite As String

        <Display(Name:="FormaPago")>
        Public Property FormaPago As String

        <Required(ErrorMessage:="Este campo es requerido. (Orden)")>
        <Display(Name:="Orden")>
        Public Property Orden As DateTime

        <Display(Name:="VigenciaHasta")>
        Public Property VigenciaHasta As Nullable(Of DateTime)

        <Display(Name:="Instrucciones")>
        Public Property Instrucciones As String

        <Display(Name:="Notas", Description:="Notas")>
        Public Property Notas As String

        <Required(ErrorMessage:="Este campo es requerido. (Estado)")>
        <Display(Name:="Estado")>
        Public Property Estado As String

        <Required(ErrorMessage:="Este campo es requerido. (Estado)")>
        <Display(Name:="Estado")>
        Public Property Fecha_Estado As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Sistema)")>
        <Display(Name:="Sistema")>
        Public Property Sistema As DateTime

        <Display(Name:="UBICACIONTITULO")>
        Public Property UBICACIONTITULO As String

        <Display(Name:="TipoInversion")>
        Public Property TipoInversion As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Preliquidacion")>
        Public Property IDPreliquidacion As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Producto)")>
        <Display(Name:="Producto")>
        Public Property IDProducto As Integer

        <Display(Name:="CostoAdicionalesOrden")>
        Public Property CostoAdicionalesOrden As Nullable(Of Double)

        <Required(ErrorMessage:="Este campo es requerido. (Bolsa)")>
        <Display(Name:="Bolsa")>
        Public Property IdBolsa As Integer

        <Display(Name:="UsuarioIngreso")>
        Public Property UsuarioIngreso As String

        <Display(Name:="NegocioEspecial")>
        Public Property NegocioEspecial As String

        <Required(ErrorMessage:="Este campo es requerido. (Eca)")>
        <Display(Name:="Eca", Description:="Eca")>
        Public Property Eca As Boolean

        <Display(Name:="OrdenEscrito", Description:="OrdenEscrito")>
        Public Property OrdenEscrito As String

        <Display(Name:="ConsecutivoSwap", Description:="ConsecutivoSwap")>
        Public Property ConsecutivoSwap As Nullable(Of Integer)

        <Display(Name:="Ejecucion", Description:="Ejecucion")>
        Public Property Ejecucion As String

        <Display(Name:="Duracion", Description:="Duracion")>
        Public Property Duracion As String

        <Display(Name:="CantidadMinima")>
        Public Property CantidadMinima As Nullable(Of Decimal)

        <Display(Name:="PrecioStop")>
        Public Property PrecioStop As Nullable(Of Decimal)

        <Display(Name:="CantidadVisible")>
        Public Property CantidadVisible As Nullable(Of Decimal)

        <Display(Name:="HoraVigencia", Description:="HoraVigencia")>
        Public Property HoraVigencia As String

        <Required(ErrorMessage:="Este campo es requerido. (EstadoLEO)")>
        <Display(Name:="EstadoLEO", Description:="EstadoLEO")>
        Public Property EstadoLEO As String

        <Display(Name:="UsuarioOperador", Description:="UsuarioOperador")>
        Public Property UsuarioOperador As String

        <Display(Name:="CanalRecepcion", Description:="CanalRecepcion")>
        Public Property CanalRecepcion As String

        <Display(Name:="MedioVerificable", Description:="MedioVerificable")>
        Public Property MedioVerificable As String

        <Display(Name:="FechaHoraRecepcion", Description:="FechaHoraRecepcion")>
        Public Property FechaHoraRecepcion As Nullable(Of DateTime)

        <Required(ErrorMessage:="Este campo es requerido. (SitioIngreso)")>
        <Display(Name:="SitioIngreso", Description:="SitioIngreso")>
        Public Property SitioIngreso As String

        <Required(ErrorMessage:="Este campo es requerido. (Seteada)")>
        <Display(Name:="Seteada", Description:="Seteada")>
        Public Property Seteada As Integer

        <Display(Name:="Folio", Description:="Folio")>
        Public Property Folio As String

        <Display(Name:="TipoOrdenPreOrdenes", Description:="TipoOrdenPreOrdenes")>
        Public Property TipoOrdenPreOrdenes As String

        <Display(Name:="NroOrdenPreOrdenes", Description:="NroOrdenPreOrdenes")>
        Public Property NroOrdenPreOrdenes As Nullable(Of Integer)

        <Display(Name:="Impresion", Description:="Impresion")>
        Public Property Impresion As Nullable(Of Boolean)

        <Display(Name:="Impresiones", Description:="Impresiones")>
        Public Property Impresiones As Nullable(Of Integer)

        <Display(Name:="PreordenDetalle")>
        Public Property PreordenDetalle As Nullable(Of Integer)

        <Display(Name:="EstadoOrdenBus")>
        Public Property EstadoOrdenBus As String

        <Display(Name:="Ordenes")>
        Public Property IDOrdenes As Integer

        <Display(Name:="IpOrigen", Description:="IpOrigen")>
        Public Property IpOrigen As String

    End Class
End Class

<MetadataTypeAttribute(GetType(Bancos_BancosNacionalesRelacionado.Bancos_BancosNacionalesRelacionadoMetadata))>
Partial Public Class Bancos_BancosNacionalesRelacionado
    Friend NotInheritable Class Bancos_BancosNacionalesRelacionadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Banco)")>
        <Display(Name:="Banco")>
        Public Property IDBanco As Integer

        <Display(Name:="Nombre Banco")>
        Public Property NombreBanco As String

        <Required(ErrorMessage:="Este campo es requerido. (BancoNacional)")>
        <Display(Name:="BancoNacional")>
        Public Property IdBancoNacional As Integer

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="BancosNacionalesR")>
        Public Property IDBancosNacionalesR As Integer

    End Class
End Class
'<MetadataTypeAttribute(GetType(AjustesBancario.AjustesBancarioMetadata))> _
'Partial Public Class AjustesBancario
'    Friend NotInheritable Class AjustesBancarioMetadata
'        Private Sub New()
'            MyBase.New()
'        End Sub

'        '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
'        'Public Property IDComisionista As Integer

'        '<Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
'        'Public Property IDSucComisionista As Integer

'        <Display(Name:=" ")> _
'        Public Property Tipo As String

'        <Display(Name:="Tipo")> _
'        <Required(ErrorMessage:="El campo {0}, es requerido.")> _
'        Public Property NombreConsecutivo As String

'        <Display(Name:="Número")> _
'        Public Property IDDocumento As Integer

'        <Display(Name:="Banco")> _
'        Public Property IDBanco As Nullable(Of Integer)

'        <Display(Name:="Fecha")> _
'        Public Property Documento As DateTime

'        <Display(Name:="Estado")> _
'        Public Property Estado As String

'        <Display(Name:="Fecha")> _
'        Public Property FecEstado As DateTime

'        <Display(Name:=" ")> _
'        Public Property Impresiones As Integer

'    End Class
'End Class

#End Region


#End Region

#Region "JEISON RAMIREZ"

<MetadataTypeAttribute(GetType(ResolucionesFactura.ResolucionesFacturaMetadata))>
Partial Public Class ResolucionesFactura
    Friend NotInheritable Class ResolucionesFacturaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Display(Name:="ID")>
        Public Property IDCodigoResolucion As Integer

        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
        <Required(ErrorMessage:="Este campo es requerido. (Número Resolución)")>
        <Display(Name:="Número Resolución")>
        Public Property NumeroResolucion As Int64

        <StringLength(260, ErrorMessage:="El campo {0} permite una longitud máxima de 260.")>
        <Required(ErrorMessage:="Este campo es requerido. (Descripción Resolución)")>
        <Display(Name:="Descripción Resolución")>
        Public Property DescripcionResolucion As String

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime



    End Class
End Class

<MetadataTypeAttribute(GetType(EspeciesBols.EspeciesBolsMetadata))>
Partial Public Class EspeciesBols
    Friend NotInheritable Class EspeciesBolsMetadata
        Private Sub New()
            MyBase.New()
        End Sub



        <Required(ErrorMessage:="Este campo es requerido. (Bolsa)")>
        <Display(Name:="Bolsa")>
        Public Property IDBolsa As Integer


        <Required(ErrorMessage:="Este campo es requerido. (Nemotecnico)")>
        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")>
        <Display(Name:="Nemotecnico")>
        Public Property Nemotecnico As String

    End Class
End Class

<MetadataTypeAttribute(GetType(MovPreciosEspeci.MovPreciosEspeciMetadata))>
Partial Public Class MovPreciosEspeci
    Friend NotInheritable Class MovPreciosEspeciMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Editable(True)>
        <Required(ErrorMessage:="Este campo es requerido. (Bolsa)")>
        <Display(Name:="Bolsa")>
        Public Property IdBolsa As Integer


        <Required(ErrorMessage:="Este campo es requerido. (Cierre)")>
        <Display(Name:="Cierre")>
        Public Property Cierre As DateTime

        <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
        <Required(ErrorMessage:="Este campo es requerido. (Precio)")>
        <Display(Name:="Precio")>
        Public Property Precio As Decimal

    End Class
End Class

#Region "Cuentas Bancarias"
<MetadataTypeAttribute(GetType(Banco.BancoMetadata))>
Partial Public Class Banco
    Friend NotInheritable Class BancoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Código")>
        Public Property ID As Integer

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Sucursal")>
        Public Property NombreSucursal As String

        <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")>
        <Display(Name:="Cuenta No.")>
        Public Property NroCuenta As String

        <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")>
        <Display(Name:="Teléfonos")>
        Public Property Telefono1 As String

        <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")>
        <Display(Name:=" ")>
        Public Property Telefono2 As String

        <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")>
        <Display(Name:="Fax")>
        Public Property Fax1 As String

        <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")>
        <Display(Name:=" ")>
        Public Property Fax2 As String

        <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")>
        <Display(Name:="Dirección")>
        Public Property Direccion As String

        <StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")>
        <Display(Name:="Internet")>
        Public Property Internet As String

        <Display(Name:="Chequera Automática")>
        Public Property ChequeraAutomatica As Boolean

        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")>
        <Display(Name:="Consecutivo")>
        Public Property NombreConsecutivo As String

        <Display(Name:="Ciudad")>
        Public Property IDPoblacion As Integer

        <Display(Name:="Departamento")>
        Public Property IDDepartamento As Integer

        <Display(Name:="País")>
        Public Property IDPais As Integer

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Gerente")>
        Public Property NombreGerente As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Cajero")>
        Public Property NombreCajero As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Portero")>
        Public Property NombrePortero As String

        <Required(ErrorMessage:="Este campo es requerido. (Creacion)")>
        <Display(Name:="Ingreso")>
        Public Property Creacion As DateTime

        '<Required(ErrorMessage:="Este campo es requerido. (Owner)")>
        <Display(Name:="Módulo")>
        Public Property IDOwner As String

        <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")>
        <Display(Name:="Cta Contable")>
        Public Property IdCuentaCtble As String

        <Display(Name:="Activa")>
        Public Property CtaActiva As Boolean

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
        <Display(Name:="Nombre Reporte")>
        Public Property Reporte As String

        <Display(Name:="Cobro G.M.F")>
        Public Property CobroGMF As Boolean

        <Display(Name:="Tipo Cuenta")>
        Public Property TipoCta As String

        <Display(Name:="SubMódulo")>
        Public Property TipoCuenta As String

        'Se comenta el requerido para que no valide desde el RIA CVA20171221
        '<Required(ErrorMessage:="Este campo es requerido. (Banco)")>
        <Display(Name:="Banco")>
        Public Property IdCodBanco As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Banco)")>
        <Display(Name:="Banco")>
        Public Property IDBanco As Integer

        <Display(Name:="Moneda")>
        Public Property IdMoneda As Nullable(Of Integer)

        <Display(Name:="Próximo Cheque")>
        Public Property lngNumCheque As Nullable(Of Integer)

        <Display(Name:="Formato ACH")>
        Public Property IDFormato As Nullable(Of Integer)

        <Display(Name:="Compañía")>
        Public Property Compania As Nullable(Of Integer)

        <Display(Name:="Nombre Cia")>
        Public Property NombreCompania As String

    End Class
End Class

#End Region

<MetadataTypeAttribute(GetType(BancoSaldosBancoMes.BancoSaldosBancoMesMetadata))>
Partial Public Class BancoSaldosBancoMes
    Friend NotInheritable Class BancoSaldosBancoMesMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="intAnno")>
        Public Property Anno As Integer

        <Display(Name:="intMes")>
        Public Property Mes As Integer

        <Display(Name:="intDia")>
        Public Property Dia As Integer

        <Display(Name:="curValor")>
        Public Property Valor As Decimal

    End Class
End Class

<MetadataTypeAttribute(GetType(Web.MovimientosBancos.MovimientosBancosMetadata))>
Partial Public Class MovimientosBancos
    Friend NotInheritable Class MovimientosBancosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="dtmDocumento", Description:="dtmDocumento")>
        Public Property dtmDocumento As DateTime

        <Display(Name:="lngIDDocumento", Description:="lngIDDocumento")>
        Public Property lngIDDocumento As Integer

        <Display(Name:="strTipo")>
        Public Property strTipo As String

        <Display(Name:="curSumaValor")>
        Public Property curSumaValor As Decimal


    End Class
End Class

<MetadataTypeAttribute(GetType(ConfigLE.ConfigLEMetadata))>
Partial Public Class ConfigLE
    Friend NotInheritable Class ConfigLEMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (ID)")>
        <Display(Name:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Display(Name:="NombreSQL")>
        Public Property NombreSQL As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="NombreDescSQL")>
        Public Property NombreDescSQL As String


    End Class
End Class

<MetadataTypeAttribute(GetType(Usuarios_Selecciona.Usuarios_SeleccionaMetadata))>
Partial Public Class Usuarios_Selecciona
    Friend NotInheritable Class Usuarios_SeleccionaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Editable(True)>
        <Required(ErrorMessage:="Este campo es requerido. (Id)")>
        <Display(Name:="Id")>
        Public Property Id As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Login)")>
        <Display(Name:="Login")>
        Public Property Login As String

    End Class
End Class

<MetadataTypeAttribute(GetType(ClienteAgrupado.ClienteAgrupadoMetadata))>
Partial Public Class ClienteAgrupado
    Friend NotInheritable Class ClienteAgrupadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="idComisionista", Description:="idComisionista")>
        Public Property idComisionista As Integer

        <Display(Name:="idSucComisionista", Description:="idSucComisionista")>
        Public Property idSucComisionista As Integer

        <Display(Name:="Agrupador")>
        Public Property IDAgrupador As Integer

        <Required(ErrorMessage:="Este campo es requerido. (NroDocumento)")>
        <Display(Name:="Nro.de Identificación")>
        Public Property NroDocumento As String

        <Required(ErrorMessage:="Este campo es requerido. (TipoIdentificacion)")>
        <Display(Name:="Tipo Identificación")>
        Public Property TipoIdentificacion As String

        <Required(ErrorMessage:="Este campo es requerido. (ComitenteLider)")>
        <Display(Name:="Cliente Líder")>
        Public Property idComitenteLider As String

        <Required(ErrorMessage:="Este campo es requerido. (NombreAgrupador)")>
        <Display(Name:=" ")>
        Public Property NombreAgrupador As String

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (ClienteAgrupador)")>
        <Display(Name:="ClienteAgrupador")>
        Public Property IDClienteAgrupador As Integer


    End Class
End Class

<MetadataTypeAttribute(GetType(DetalleGrupoEconomico.DetalleGrupoEconomicoMetadata))>
Partial Public Class DetalleGrupoEconomico
    Friend NotInheritable Class DetalleGrupoEconomicoMetadata
        Private Sub New()
            MyBase.New()
        End Sub
        Public Property IdComitente As String
        Public Property Nombre As String
        Public Property DireccionEnvio As String
        Public Property IDReceptor As String
        Public Property IDSucCliente As String
        Public Property NroDocumento As System.Nullable(Of Decimal)
    End Class
End Class

<MetadataTypeAttribute(GetType(DetalleClienteAgrupado.DetalleClienteAgrupadoMetadata))>
Partial Public Class DetalleClienteAgrupado
    Friend NotInheritable Class DetalleClienteAgrupadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub


        <Display(Name:="Id")>
        Public Property Id As String

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (DireccionEnvio)")>
        <Display(Name:="DireccionEnvio")>
        Public Property DireccionEnvio As String

        <Required(ErrorMessage:="Este campo es requerido. (Receptor)")>
        <Display(Name:="Receptor")>
        Public Property idReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (IDSucCliente)")>
        <Display(Name:="IDSucCliente")>
        Public Property IDSucCliente As String

        <Required(ErrorMessage:="Este campo es requerido. (DetalleClienteAgrupador)")>
        <Display(Name:="DetalleClienteAgrupador")>
        Public Property IDDetalleClienteAgrupador As Integer

    End Class
End Class

<MetadataTypeAttribute(GetType(ClasificacionesCii.ClasificacionesCiiMetadata))>
Partial Public Class ClasificacionesCii
    Friend NotInheritable Class ClasificacionesCiiMetadata
        Private Sub New()
            MyBase.New()
        End Sub


        <Required(ErrorMessage:="Este campo es requerido. (ID)")>
        <Display(Name:="Código")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
        <Display(Name:="Nombre")>
        Public Property Nombre As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo)")>
        <Display(Name:="Tipo")>
        Public Property Tipo As Integer

        <Required(ErrorMessage:="Este campo es requerido. (PerteneceA)")>
        <Display(Name:="Pertenece A")>
        Public Property IDPerteneceA As Integer

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String

        <Required(ErrorMessage:="Este campo es requerido. (ClasificacionCiiu)")>
        <Display(Name:="ClasificacionCiiu")>
        Public Property IDClasificacionCiiu As Integer

    End Class
End Class

<MetadataTypeAttribute(GetType(BolsaCosto.BolsaCostoMetadata))>
Partial Public Class BolsaCosto
    Friend NotInheritable Class BolsaCostoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="Este campo es requerido. (BolsaCostos)")>
        <Display(Name:="BolsaCostos")>
        Public Property IDBolsaCostos As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Id)")>
        <Display(Name:="Id")>
        Public Property Id As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Descripcion)")>
        <Display(Name:="Descripcion")>
        Public Property Descripcion As String

        <Required(ErrorMessage:="Este campo es requerido. (PorcentajeCosto)")>
        <Display(Name:="PorcentajeCosto")>
        Public Property PorcentajeCosto As Double

        <Required(ErrorMessage:="Este campo es requerido. (CostoPesos)")>
        <Display(Name:="CostoPesos")>
        Public Property CostoPesos As Nullable(Of Double)

        <Display(Name:="Actualizado")>
        Public Property Actualizado As Nullable(Of Boolean)

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Required(ErrorMessage:="Este campo es requerido. (Actualizacion)")>
        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

    End Class
End Class

#End Region

#Region "SEBASTIAN LONDOÑO B."

<MetadataTypeAttribute(GetType(BloqueoSaldoCliente.BloqueoSaldoClienteMetadata))>
Partial Public Class BloqueoSaldoCliente
    Friend NotInheritable Class BloqueoSaldoClienteMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Registro)")>
        <Display(Name:="Registro")>
        Public Property IdRegistro As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Comitente)")>
        <Display(Name:="Comitente")>
        Public Property IDComitente As String

        <Required(ErrorMessage:="Este campo es requerido. (Motivo Bloqueo Saldo)")>
        <Display(Name:="Motivo Bloqueo Saldo")>
        Public Property MotivoBloqueoSaldo As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor Bloqueado)")>
        <Display(Name:="Valor Bloqueado")>
        Public Property ValorBloqueado As Decimal
        '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _


        <Required(ErrorMessage:="Este campo es requerido. (Naturaleza)")>
        <Display(Name:="Naturaleza")>
        Public Property Naturaleza As String

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Bloqueo)")>
        <Display(Name:="Fecha Bloqueo")>
        Public Property FechaBloqueo As Nullable(Of DateTime)

        <Required(ErrorMessage:="Este campo es requerido. (Detalle Bloqueo)")>
        <Display(Name:="Detalle Bloqueo")>
        Public Property DetalleBloqueo As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String


    End Class
End Class

<MetadataTypeAttribute(GetType(DestinoInversion.DestinoInversionMetadata))>
Partial Public Class DestinoInversion
    Friend NotInheritable Class DestinoInversionMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")>
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
        Public Property IDSucComisionista As Integer

        <Required(ErrorMessage:="Este campo es requerido.")>
        <Display(Name:="Código")>
        Public Property IDDestino As Integer

        <Required(ErrorMessage:="Este campo es requerido.")>
        <Display(Name:="Nombre")>
        Public Property NombreDestino As String

        <Display(Name:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Usuario)")>
        <Display(Name:="Usuario")>
        Public Property Usuario As String


    End Class
End Class

#End Region

#Region "SANTIAGO VERGARA"


'**************************************************************************************************
<MetadataTypeAttribute(GetType(ComisionEspecie.ComisionEspecieMetadata))>
Partial Public Class ComisionEspecie
    Friend NotInheritable Class ComisionEspecieMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Id ", Description:="Id ")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Especie)")>
        <Display(Name:="Especie")>
        Public Property IDEspecie As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor Comision)")>
        <Display(Name:="Valor Comisión")>
        Public Property Comision As Decimal

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje Comisión)")>
        <Display(Name:="Porcentaje Comisión")>
        Public Property PorcentajeComision As Decimal

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")>
        Public Property Actualizacion As DateTime

        <Display(Name:="Especie")>
        Public Property NombreEspecie As String

    End Class
End Class


#End Region

#Region "COMUNES"
'The MetadataTypeAttribute identifies AuditoriaMetadata as the class
' that carries additional metadata for the Auditoria class.
<MetadataTypeAttribute(GetType(Auditoria.AuditoriaMetadata))>
Partial Public Class Auditoria

    'This class allows you to attach custom attributes to properties
    ' of the Auditoria class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <A2RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class AuditoriaMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property Browser As String

        Public Property DirIPMaquina As String

        Public Property ErrorPersonalizado As Short

        Public Property Maquina As String

        Public Property Servidor As String

        Public Property Usuario As String
    End Class
End Class
#End Region

#Region "MAESTROS DE OYDPLUS"

'**************************************************************************************************
'Mensajes
<MetadataTypeAttribute(GetType(Mensaje.MensajeMetadata))>
Partial Public Class Mensaje
    Friend NotInheritable Class MensajeMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDMensaje", Description:="IDMensaje")>
        Public Property IDMensaje As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Mensaje)")>
        <Display(Name:="Código Mensaje", Description:="Código Mensaje")>
        Public Property CodigoMensaje As String

        <Required(ErrorMessage:="Este campo es requerido. (Mensaje)")>
        <Display(Name:="Mensaje", Description:="Mensaje para mostrar en la aplicación.")>
        Public Property Mensaje As String

        <Display(Name:="Mensaje Con Reemplazo", Description:="Mensaje para mostrar en la aplicación reemplazando algunos valores.")>
        Public Property MensajeConReempl As String

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Reglas
<MetadataTypeAttribute(GetType(Regla.ReglaMetadata))>
Partial Public Class Regla
    Friend NotInheritable Class ReglaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID Regla", Description:=" ID Regla")>
        Public Property IDRegla As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Regla)")>
        <Display(Name:="Código Regla", Description:="Código Regla")>
        Public Property CodigoRegla As String

        <Display(Name:="Descripción Regla", Description:="Descripción corta de lo que hace la Regla.")>
        Public Property DescripcionRegla As String

        <Required(ErrorMessage:="Este campo es requerido. (Modulo)")>
        <Display(Name:="Modulo", Description:="Modulo al que le aplica la regla 'ORDENES' - 'ORDENESTESORERIA'")>
        Public Property Modulo As String

        <Required(ErrorMessage:="Este campo es requerido. (Nombre Corto)")>
        <Display(Name:="Nombre Corto", Description:="Nombre Corto para mostrar en pantalla cuando se incumpla la regla.")>
        Public Property NombreCorto As String

        <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
        <Display(Name:="Prioridad", Description:="Prioridad con la cual se validara la regla de negocio.")>
        Public Property Prioridad As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Acción Regla)")>
        <Display(Name:="Acción Regla", Description:="Habilitar o deshabilitar la regla de negocio.")>
        Public Property CodigoTipoRegla As String

        <Display(Name:="Causas Justificación", Description:="Causas de la justificación para mostrar al usuario.")>
        Public Property CausasJustificacion As String

        <Display(Name:="Nombre Procedimiento", Description:="Nombre Procedimiento para validar la regla.")>
        Public Property NombreProcedimiento As String

        <Display(Name:="Parametros", Description:="Parametros que recibe el procedimiento.")>
        Public Property Parametros As String

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

        <Display(Name:="Motor calculos", Description:="Indica sí la regla se evalua por motor de calculos")>
        Public Property MotorCalculos As Boolean

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Mensajes Reglas
<MetadataTypeAttribute(GetType(MensajesRegla.MensajesReglaMetadata))>
Partial Public Class MensajesRegla
    Friend NotInheritable Class MensajesReglaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID Mensaje Regla", Description:="ID Mensaje Regla")>
        Public Property IDMensajeRegla As Integer

        <Required(ErrorMessage:="Este campo es requerido. (ID Regla)")>
        <Display(Name:="Código Regla", Description:="Código de la regla")>
        Public Property IDRegla As Integer

        <Required(ErrorMessage:="Este campo es requerido. (ID Mensaje)")>
        <Display(Name:="Código Mensaje", Description:="Código del mensaje")>
        Public Property IDMensaje As Integer

        <Required(ErrorMessage:="Este campo es requerido. (ID Tipo Mensaje)")>
        <Display(Name:="ID Tipo Mensaje", Description:="ID Tipo Mensaje")>
        Public Property IDTipoMensaje As Integer

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Tipo Negocio
<MetadataTypeAttribute(GetType(TipoNegocio.TipoNegocioMetadata))>
Partial Public Class TipoNegocio
    Friend NotInheritable Class TipoNegocioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código)")>
        <Display(Name:="Código", Description:="Código")>
        Public Property Codigo As String

        <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
        <Display(Name:="Descripción", Description:="Descripción")>
        Public Property Descripcion As String

        <Display(Name:="Configuración Menu", Description:="Configuración Menu")>
        Public Property ConfiguracionMenu As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Certificaciones x Tipo Negogcio
<MetadataTypeAttribute(GetType(CertificacionXTipoNegocio.CertificacionXTipoNegocioMetadata))>
Partial Public Class CertificacionXTipoNegocio
    Friend NotInheritable Class CertificacionXTipoNegocioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Certificacion)")>
        <Display(Name:="Código Certificacion", Description:="Código Certificacion")>
        Public Property CodigoCertificacion As String

        <Required(ErrorMessage:="Este campo es requerido. (Código Tipo Negocio)")>
        <Display(Name:="Código Tipo Negocio", Description:="Código Tipo Negocio")>
        Public Property CodigoTipoNegocio As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Documento x Tipo Negocio
<MetadataTypeAttribute(GetType(DocumentoXTipoNegocio.DocumentoXTipoNegocioMetadata))>
Partial Public Class DocumentoXTipoNegocio
    Friend NotInheritable Class DocumentoXTipoNegocioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Negocio)")>
        <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")>
        Public Property TipoNegocio As String

        <Required(ErrorMessage:="Este campo es requerido. (Cod Documento)")>
        <Display(Name:="Cod Documento", Description:="Cod Documento")>
        Public Property CodDocumento As Nullable(Of Int32)

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Especie x Tipo negocio
<MetadataTypeAttribute(GetType(TipoNegocioXEspecie.TipoNegocioXEspecieMetadata))>
Partial Public Class TipoNegocioXEspecie
    Friend NotInheritable Class TipoNegocioXEspecieMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Negocio)")>
        <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")>
        Public Property TipoNegocio As String

        <Required(ErrorMessage:="Este campo es requerido. (Especie)")>
        <Display(Name:="Especie", Description:="Especie")>
        Public Property IDEspecie As String

        <Display(Name:="Maneja ISIN", Description:="Maneja ISIN")>
        Public Property ManejaISIN As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (Max Valor Cantidad)")>
        <Display(Name:="Max Valor Cantidad", Description:="Max Valor Cantidad")>
        Public Property MaxValorCantidad As Double

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Tipo Producto x Tipo Negocio
<MetadataTypeAttribute(GetType(TipoNegocioXTipoProducto.TipoNegocioXTipoProductoMetadata))>
Partial Public Class TipoNegocioXTipoProducto
    Friend NotInheritable Class TipoNegocioXTipoProductoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Negocio)")>
        <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")>
        Public Property TipoNegocio As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Producto)")>
        <Display(Name:="Tipo Producto", Description:="Tipo Producto")>
        Public Property TipoProducto As String

        <Display(Name:="Cliente", Description:="Cliente")>
        Public Property IDComitente As String

        <Display(Name:="Perfil", Description:="Perfil")>
        Public Property Perfil As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor Max Negociación)")>
        <Display(Name:="Valor Max Negociación", Description:="Valor Max Negociación")>
        Public Property ValorMaxNegociacion As Double

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje de patrimonio técnico)")>
        <Range(0, 100, ErrorMessage:="{0} debe tener un valor entre 0 y 100")>
        <Display(Name:="Porcentaje de patrimonio técnico", Description:="Porcentaje de patrimonio técnico")>
        Public Property PorcentajePatrimonioTecnico As Double

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Certificaciones Receptor
<MetadataTypeAttribute(GetType(CertificacionesXRecepto.CertificacionesXReceptoMetadata))>
Partial Public Class CertificacionesXRecepto
    Friend NotInheritable Class CertificacionesXReceptoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
        <Display(Name:="Código Receptor", Description:="Código Receptor")>
        Public Property CodigoReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Codigo Certificación)")>
        <Display(Name:="Codigo Certificación", Description:="Codigo Certificación")>
        Public Property CodigoCertificacion As String

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Inicial)")>
        <Display(Name:="Fecha Inicial", Description:="Fecha Inicial")>
        Public Property FechaInicial As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Final)")>
        <Display(Name:="Fecha Final", Description:="Fecha Final")>
        Public Property FechaFinal As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Conceptos Receptor
<MetadataTypeAttribute(GetType(ConceptosXRecepto.ConceptosXReceptoMetadata))>
Partial Public Class ConceptosXRecepto
    Friend NotInheritable Class ConceptosXReceptoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
        <Display(Name:="Código Receptor", Description:="Código Receptor")>
        Public Property CodigoReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Concepto)")>
        <Display(Name:="Concepto", Description:="Concepto")>
        Public Property IdConcepto As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
        <Display(Name:="Prioridad", Description:="Prioridad")>
        Public Property Prioridad As Integer

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Configuraciones Adicionales Receptor
<MetadataTypeAttribute(GetType(ConfiguracionesAdicionalesRecepto.ConfiguracionesAdicionalesReceptoMetadata))>
Partial Public Class ConfiguracionesAdicionalesRecepto
    Friend NotInheritable Class ConfiguracionesAdicionalesReceptoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
        <Display(Name:="Código Receptor", Description:="Código Receptor")>
        Public Property CodigoReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Orden)")>
        <Display(Name:="Tipo Orden", Description:="Tipo Orden")>
        Public Property TipoOrden As String

        <Display(Name:="Extensión Defecto", Description:="Extensión Defecto")>
        Public Property ExtensionDefecto As String

        <Display(Name:="Tipo Orden Defecto", Description:="Tipo Orden Defecto")>
        Public Property TipoOrdenDefecto As String

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje de patrimonio técnico)")>
        <Range(0, 100, ErrorMessage:="{0} debe tener un valor entre 0 y 100")>
        <Display(Name:="Porcentaje de patrimonio técnico", Description:="Porcentaje de patrimonio técnico")>
        Public Property PorcentajePatrimonioTecnico As Double

        <Required(ErrorMessage:="Este campo es requerido. (Enruta Bus)")>
        <Display(Name:="Enruta Bus", Description:="Enruta Bus")>
        Public Property EnrutaBus As Nullable(Of Boolean)

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Parametros Receptor
<MetadataTypeAttribute(GetType(ParametrosRecepto.ParametrosReceptoMetadata))>
Partial Public Class ParametrosRecepto
    Friend NotInheritable Class ParametrosReceptoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
        <Display(Name:="Código Receptor", Description:="Código Receptor")>
        Public Property CodigoReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Topico)")>
        <Display(Name:="Topico", Description:="Topico")>
        Public Property Topico As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor)")>
        <Display(Name:="Valor", Description:="Valor")>
        Public Property Valor As String

        <Display(Name:="Prioridad", Description:="Prioridad")>
        Public Property Prioridad As Integer

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class

'**************************************************************************************************
'Receptores Clientes Autorizado
<MetadataTypeAttribute(GetType(ReceptoresClientesAutorizado.ReceptoresClientesAutorizadoMetadata))>
Partial Public Class ReceptoresClientesAutorizado
    Friend NotInheritable Class ReceptoresClientesAutorizadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Codigo Receptor Cliente)")>
        <Display(Name:="Codigo Receptor Cliente", Description:="Codigo Receptor Cliente")>
        Public Property CodigoReceptorCliente As String

        <Required(ErrorMessage:="Este campo es requerido. (Codigo Receptor Tercero)")>
        <Display(Name:="Codigo Receptor Tercero", Description:="Codigo Receptor Tercero")>
        Public Property CodigoReceptorTercero As String

        <Display(Name:="Cliente", Description:="Cliente")>
        Public Property IDComitente As String

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Inicial)")>
        <Display(Name:="Fecha Inicial", Description:="Fecha Inicial")>
        Public Property FechaInicial As DateTime

        <Required(ErrorMessage:="Este campo es requerido. (Fecha Final)")>
        <Display(Name:="Fecha Final", Description:="Fecha Final")>
        Public Property FechaFinal As DateTime

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Tipo de negocio receptor
<MetadataTypeAttribute(GetType(TipoNegocioXRecepto.TipoNegocioXReceptoMetadata))>
Partial Public Class TipoNegocioXRecepto
    Friend NotInheritable Class TipoNegocioXReceptoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
        <Display(Name:="Código Receptor", Description:="Código Receptor")>
        Public Property CodigoReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Código Tipo Negocio)")>
        <Display(Name:="Código Tipo Negocio", Description:="Código Tipo Negocio")>
        Public Property CodigoTipoNegocio As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor Max Negociación)")>
        <Display(Name:="Valor Max Negociación", Description:="Valor Max Negociación")>
        Public Property ValorMaxNegociacion As Double

        <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
        <Display(Name:="Prioridad", Description:="Prioridad")>
        Public Property Prioridad As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje Comisión)")>
        <Display(Name:="Porcentaje Comisión", Description:="Porcentaje Comisión")>
        Public Property PorcentajeComision As Nullable(Of Double)

        <Required(ErrorMessage:="Este campo es requerido. (Valor Comisión)")>
        <Display(Name:="Valor Comisión", Description:="Valor Comisión")>
        Public Property ValorComision As Nullable(Of Double)

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
'Tipo producto Receptor
<MetadataTypeAttribute(GetType(TipoProductoXRecepto.TipoProductoXReceptoMetadata))>
Partial Public Class TipoProductoXRecepto
    Friend NotInheritable Class TipoProductoXReceptoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
        <Display(Name:="Código Receptor", Description:="Código Receptor")>
        Public Property CodigoReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Producto)")>
        <Display(Name:="Tipo Producto", Description:="Tipo Producto")>
        Public Property TipoProducto As String

        <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
        <Display(Name:="Prioridad", Description:="Prioridad")>
        Public Property Prioridad As Integer

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class
'**************************************************************************************************

'**************************************************************************************************
<MetadataTypeAttribute(GetType(RN_ConfiguracionNivelAtribucio.RN_ConfiguracionNivelAtribucioMetadata))>
Partial Public Class RN_ConfiguracionNivelAtribucio
    Friend NotInheritable Class RN_ConfiguracionNivelAtribucioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Código", Description:="Código")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Regla)")>
        <Display(Name:="Regla", Description:="Regla de Negocio")>
        Public Property Regla As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Tipo Documento)")>
        <Display(Name:="Tipo Documento", Description:="Tipo Documento Autorizaciones")>
        Public Property TipoDocumento As Nullable(Of Integer)

        <Required(ErrorMessage:="Este campo es requerido. (Nivel Atribución)")>
        <Display(Name:="Nivel Atribución", Description:="Nivel Atribución que Aprobara")>
        Public Property NivelAprobacion As Nullable(Of Integer)

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class


<MetadataTypeAttribute(GetType(Costo.CostoMetadata))>
Partial Public Class Costo
    Friend NotInheritable Class CostoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Id Costo", Description:="Id Costo")>
        Public Property Idcosto As Integer

        <Required(ErrorMessage:="Este campo es requerido. (CodigoFormapago)")>
        <Display(Name:="Forma Pago", Description:="Código de Forma de Pago")>
        Public Property CodigoFormapago As String

        <Display(Name:="Tipo Cheque", Description:="Código de Tipo de Cheque")>
        Public Property CodigoTipoCheque As String

        <Display(Name:="Tipo Cruce", Description:="Código Tipo Cruce")>
        Public Property CodigoTipoCruce As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor)")>
        <Display(Name:="Valor", Description:="Valor")>
        Public Property Valor As Decimal

        '<Display(Name:="Usuario", Description:="Usuario")> _
        'Public Property Usuario As String

    End Class
End Class

'**************************************************************************************************
<MetadataTypeAttribute(GetType(TipoProductoXEspeci.TipoProductoXEspeciMetadata))>
Partial Public Class TipoProductoXEspeci
    Friend NotInheritable Class TipoProductoXEspeciMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo producto)")>
        <Display(Name:="Tipo producto", Description:="Tipo producto")>
        Public Property CodigoTipoProducto As String

        <Display(Name:="Especie", Description:="Especie")>
        Public Property IDEspecie As String

        <Required(ErrorMessage:="Este campo es requerido. (Valor Max Negociación)")>
        <Display(Name:="Valor Max Negociación", Description:="Valor Max Negociación")>
        Public Property ValorMaxNegociacion As Double

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class

'**************************************************************************************************
<MetadataTypeAttribute(GetType(Plantilla.PlantillaMetadata))>
Partial Public Class Plantilla
    Friend NotInheritable Class PlantillaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Id de plantilla", Description:="Id de plantilla")>
        Public Property intID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Código de plantilla)")>
        <Display(Name:="Código de plantilla", Description:="Código de plantilla")>
        Public Property strCodigo As String

        <Required(ErrorMessage:="Este campo es requerido. (Cuerpo de la plantilla)")>
        <Display(Name:="Plantilla", Description:="Cuerpo de la plantilla")>
        Public Property strMensaje As String

    End Class
End Class

'**************************************************************************************************
<MetadataTypeAttribute(GetType(ControlHorario.ControlHorarioMetadata))>
Partial Public Class ControlHorario
    Friend NotInheritable Class ControlHorarioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")>
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Modulo)")>
        <Display(Name:="Modulo", Description:="Modulo")>
        Public Property Modulo As System.Nullable(Of Integer)

        <Display(Name:="Tipo negocio", Description:="Tipo negocio")>
        Public Property TipoNegocio As String

        <Display(Name:="Tipo orden", Description:="Tipo orden")>
        Public Property TipoOrden As System.Nullable(Of Integer)

        <Display(Name:="Tipo producto", Description:="Tipo producto")>
        Public Property TipoProducto As System.Nullable(Of Integer)

        <Display(Name:="Instrucción", Description:="Instrucción")>
        Public Property Instruccion As System.Nullable(Of Integer)

        <Display(Name:="Hora inicio", Description:="Hora inicio")>
        <Required(ErrorMessage:="Este campo es requerido. (Hora inicio)")>
        Public Property HoraInicio As String

        <Display(Name:="Hora fin", Description:="Hora fin")>
        <Required(ErrorMessage:="Este campo es requerido. (Hora fin)")>
        Public Property HoraFin As String

        <Display(Name:="Usuario", Description:="Usuario")>
        Public Property Usuario As String

    End Class
End Class

'**************************************************************************************************
<MetadataTypeAttribute(GetType(PlantillaBanco.PlantillaBancoMetadata))>
Partial Public Class PlantillaBanco
    Friend NotInheritable Class PlantillaBancoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Id de plantilla por banco", Description:="Id de plantilla por banco")>
        Public Property Id As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Id del banco)")>
        <Display(Name:="Id del banco", Description:="Id del banco")>
        Public Property IdBanco As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Id de la plantilla)")>
        <Display(Name:="Id de la plantilla", Description:="Id de la plantilla")>
        Public Property IdPlantilla As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
        <Display(Name:="Descripción", Description:="Descripción")>
        Public Property strDescripcion As String

        '<A2RegularExpression("[A-Za-zñÑ\w\s]{0,25}", ErrorMessage:="Existen caracteres no permitidos en el campo {0}, solo se permiten letras y números.")> _
        <Required(ErrorMessage:="Este campo es requerido. (Extensión - tipo de archivo)")>
        <A2RegularExpression("^.((doc)|(docx)|(pdf)|(html))$", ErrorMessage:="Por favor ingrese una extensión de archivo válida ejm: (.doc|.docx|.pdf|.html).")>
        <Display(Name:="Extensión - tipo de archivo", Description:="Extensión - tipo de archivo")> _
        Public Property strExtension As String

        <Display(Name:="Nombre del banco", Description:="Nombre del banco")> _
        Public Property strNombre As String

        <Display(Name:="Nombre de la plantilla", Description:="Nombre de la plantilla")> _
        Public Property strCodigo As String

    End Class
End Class

<MetadataTypeAttribute(GetType(ClientesRelacionados.ClientesRelacionadosMetadata))> _
Partial Public Class ClientesRelacionados
    Friend NotInheritable Class ClientesRelacionadosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo idenficicación cliente)")> _
        <Display(Name:="Tipo idenficicación cliente")> _
        Public Property TipoIdCliente As String

        <Display(Name:="Tipo idenficicación cliente")> _
        Public Property NombreTipoIdCliente As String

        <Required(ErrorMessage:="Este campo es requerido. (Nro documento cliente)")> _
        <Display(Name:="Nro documento cliente")> _
        Public Property NroDocumentoCliente As String

        <Display(Name:="Nombre cliente")> _
        Public Property NombreCliente As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo identificación cliente relacionado)")> _
        <Display(Name:="Tipo identificación cliente relacionado")> _
        Public Property TipoIdClienteRelacionado As String

        <Display(Name:="Tipo idenficicación cliente relacionado")> _
        Public Property NombreTipoIdClienteRelacionado As String

        <Required(ErrorMessage:="Este campo es requerido. (Nro documento cliente relacionado)")> _
        <Display(Name:="Nro documento cliente relacionado")> _
        Public Property NroDocumentoClienteRelacionado As String

        <Display(Name:="Nombre cliente relacionado")> _
        Public Property NombreClienteRelacionado As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo relación)")> _
        <Display(Name:="Tipo relación")> _
        Public Property TipoRelacion As String

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String
    End Class
End Class

<MetadataTypeAttribute(GetType(ClientesRelacionadosEncabezado.ClientesRelacionadosEncabezadoMetadata))> _
Partial Public Class ClientesRelacionadosEncabezado
    Friend NotInheritable Class ClientesRelacionadosEncabezadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo idenficicación cliente)")> _
        <Display(Name:="Tipo idenficicación cliente")> _
        Public Property TipoIdCliente As String

        <Display(Name:="Tipo idenficicación cliente")> _
        Public Property NombreTipoIdCliente As String

        <Required(ErrorMessage:="Este campo es requerido. (Nro documento cliente)")> _
        <Display(Name:="Nro documento cliente")> _
        Public Property NroDocumentoCliente As String

        <Display(Name:="Nombre cliente")> _
        Public Property NombreCliente As String

    End Class
End Class

<MetadataTypeAttribute(GetType(PortalioClienteXTipoNegocio.PortalioClienteXTipoNegocioMetadata))> _
Partial Public Class PortalioClienteXTipoNegocio
    Friend NotInheritable Class PortalioClienteXTipoNegocioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Display(Name:="Tipo producto")> _
        Public Property TipoProducto As String

        <Display(Name:="Perfil riesgo")> _
        Public Property PerfilRiesgo As String

        <Display(Name:="Código OYD")> _
        Public Property CodigoOYD As String

        <Display(Name:="Nombre")> _
        Public Property NombreCodigoOYD As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo negocio)")> _
        <Display(Name:="Tipo negocio")> _
        Public Property TipoNegocio As String

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje)")> _
        <Display(Name:="Porcentaje")> _
        Public Property Porcentaje As Double

        <Required(ErrorMessage:="Este campo es requerido. (Cupo máximo)")> _
        <Display(Name:="Cupo máximo")> _
        Public Property ValorMaximoCupo As Double

    End Class
End Class


<MetadataTypeAttribute(GetType(PortalioClienteXTipoNegocioPrincipal.PortalioClienteXTipoNegocioPrincipalMetadata))> _
Partial Public Class PortalioClienteXTipoNegocioPrincipal
    Friend NotInheritable Class PortalioClienteXTipoNegocioPrincipalMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Display(Name:="Tipo producto")> _
        Public Property TipoProducto As String

        <Display(Name:="Perfil riesgo")> _
        Public Property PerfilRiesgo As String

        <Display(Name:="Código OYD")> _
        Public Property CodigoOYD As String

        <Display(Name:="Nombre")> _
        Public Property NombreCodigoOYD As String

        <Display(Name:="Perfil riesgo")> _
        Public Property NombrePerfilRiesgo As String

        <Display(Name:="Tipo producto")> _
        Public Property NombreTipoProducto As String

    End Class
End Class

<MetadataTypeAttribute(GetType(DistribucionComisionXTipoNegocio.DistribucionComisionXTipoNegocioMetadata))> _
Partial Public Class DistribucionComisionXTipoNegocio
    Friend NotInheritable Class DistribucionComisionXTipoNegocioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Tipo negocio)")> _
        <Display(Name:="Tipo negocio")> _
        Public Property TipoNegocio As String

        <Display(Name:="Tipo producto")> _
        Public Property TipoProducto As String

        <Display(Name:="Perfil riesgo")> _
        Public Property PerfilRiesgo As String

        <Display(Name:="Código OYD")> _
        Public Property CodigoOYD As String

        <Display(Name:="Nombre")> _
        Public Property NombreCodigoOYD As String

        <Required(ErrorMessage:="Este campo es requerido. (Límite inferior)")> _
        <Display(Name:="Límite inferior")> _
        Public Property LimiteInferior As Double

        <Required(ErrorMessage:="Este campo es requerido. (Límite inferior)")> _
        <Display(Name:="Límite superior")> _
        Public Property LimiteSuperior As Double

        <Required(ErrorMessage:="Este campo es requerido. (Comisión minima)")> _
        <Display(Name:="Comisión minima")> _
        Public Property ComisionMinima As Double

        <Required(ErrorMessage:="Este campo es requerido. (Comisión máxima)")> _
        <Display(Name:="Comisión máxima")> _
        Public Property ComisionMaxima As Double

        <Required(ErrorMessage:="Este campo es requerido. (Comisión en porcentaje)")> _
        <Display(Name:="Comisión en porcentaje")> _
        Public Property ComisionEnPorcentaje As Boolean


    End Class
End Class

<MetadataTypeAttribute(GetType(CupoReceptorXTipoNegocio.CupoReceptorXTipoNegocioMetadata))> _
Partial Public Class CupoReceptorXTipoNegocio
    Friend NotInheritable Class CupoReceptorXTipoNegocioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Display(Name:="Sucursal")> _
        Public Property IDSucursal As System.Nullable(Of Integer)

        <Display(Name:="Mesa")> _
        Public Property IDMesa As System.Nullable(Of Integer)

        <Display(Name:="Receptor")> _
        Public Property IDReceptor As String

        <Display(Name:="Nombre")> _
        Public Property NombreIDReceptor As String

        <Required(ErrorMessage:="Este campo es requerido. (Tipo negocio)")> _
        <Display(Name:="Tipo negocio")> _
        Public Property TipoNegocio As String

        <Required(ErrorMessage:="Este campo es requerido. (Porcentaje cupo)")> _
        <Display(Name:="Porcentaje cupo")> _
        Public Property PorcentajeCupo As Double

    End Class
End Class

<MetadataTypeAttribute(GetType(CupoReceptorXTipoNegocioPrincipal.CupoReceptorXTipoNegocioPrincipalMetadata))> _
Partial Public Class CupoReceptorXTipoNegocioPrincipal
    Friend NotInheritable Class CupoReceptorXTipoNegocioPrincipalMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Display(Name:="Sucursal")> _
        Public Property IDSucursal As System.Nullable(Of Integer)

        <Display(Name:="Mesa")> _
        Public Property IDMesa As System.Nullable(Of Integer)

        <Display(Name:="Receptor")> _
        Public Property IDReceptor As String

        <Display(Name:="Nombre")> _
        Public Property NombreReceptor As String

        <Display(Name:="Sucursal")> _
        Public Property NombreSucursal As String

        <Display(Name:="Mesa")> _
        Public Property NombreMesa As String

    End Class
End Class

'**************************************************************************************************

'**************************************************************************************************
'Alias Especies
<MetadataTypeAttribute(GetType(AliasEspecie.AliasEspecieMetadata))> _
Partial Public Class AliasEspecie
    Friend NotInheritable Class AliasEspecieMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Id especie", Description:=" Id especie")> _
        Public Property IDEspecies As Integer

        <Display(Name:="Nemotécnico", Description:="Nemotécnico.")> _
        Public Property Nemotecnico As String

        <Display(Name:="Nombre", Description:="Nombre especie")> _
        Public Property Nombre As String

        <StringLength(50, ErrorMessage:="La longitud máxima del Alias es de: 50")> _
        <Display(Name:="Alias ", Description:="Alias de la especie.")> _
        Public Property AliasEspecie As String

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

    End Class
End Class

#End Region


''' <summary>
''' Metadata para EspeciesISINFungible -- EOMC -- 08/08/2013
''' </summary>
''' <remarks></remarks>
<MetadataTypeAttribute(GetType(EspeciesISINFungible.EspeciesISINFungibleMetadata))> _
Partial Public Class EspeciesISINFungible
    Friend NotInheritable Class EspeciesISINFungibleMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        '<Required(ErrorMessage:="El ISIN es un campo requerido")> _
        <StringLength(12, ErrorMessage:="La longitud máxima es del ISIN es de: 12")> _
        <Display(Name:="ISIN", Description:="ISIN")> _
        Public Property ISIN As String

        <Display(Name:="IDFungible", Description:="IDFungible")> _
        Public Property IDFungible As Integer

        <Display(Name:="Emision", Description:="Emision")> _
        Public Property Emision As Integer

        <Display(Name:="Fecha de emision", Description:="Fecha de emision")> _
        Public Property Fecha_Emision As System.Nullable(Of Date)

        <Display(Name:="Fecha de vencimiento", Description:="Fecha de vencimiento")> _
        Public Property Fecha_Vencimiento As System.Nullable(Of Date)

        <Display(Name:="Tasa facial", Description:="Tasa facial")> _
        Public Property Tasa_Facial As System.Nullable(Of Decimal)

        <Display(Name:="Modalidad", Description:="Modalidad")> _
        Public Property Modalidad As String

        <Display(Name:="Indicador", Description:="Indicador")> _
        Public Property Indicador As String

        <Display(Name:="Puntos indicador", Description:="Puntos indicador")> _
        Public Property Puntos_Indicador As System.Nullable(Of Decimal)

        <Display(Name:="Consecutivo", Description:="Consecutivo")> _
        Public Property IDConsecutivo As Integer

        <Display(Name:="Tasa base", Description:="Tasa base")> _
        Public Property TasaBase As System.Nullable(Of Integer)

        <Display(Name:="Especie", Description:="Especie")> _
        Public Property IDEspecie As String

        <Display(Name:="Es amortizada", Description:="Es amortizada")> _
        Public Property Amortizada As System.Nullable(Of Boolean)

        <Display(Name:="Posee retención", Description:="Posee retención")> _
        Public Property logPoseeRetencion As System.Nullable(Of Boolean)

        <Display(Name:="Porcentaje de retención", Description:="Porcentaje de retención")> _
        Public Property dblPorcentajeRetencion As System.Nullable(Of System.Decimal)

        <Display(Name:="Flujos irregulares", Description:="Flujos irregulares")> _
        Public Property logFlujosIrregulares As System.Nullable(Of Boolean)

        <Display(Name:="Activo", Description:="Activo")> _
        Public Property logActivo As System.Nullable(Of Boolean)

        <Display(Name:="Sector financiero", Description:="Sector financiero")> _
        Public Property logSectorFinanciero As System.Nullable(Of Boolean)
    End Class
End Class


''' <summary>
''' Metadata para grupos económicos -- EOMC -- 12/02/2013
''' </summary>
''' <remarks></remarks>
<MetadataTypeAttribute(GetType(GrupoEconomicos.GrupoEconomicosMetadata))> _
Partial Public Class GrupoEconomicos
    Friend NotInheritable Class GrupoEconomicosMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(ErrorMessage:="El código es un campo requerido")> _
        <Display(Name:="Grupo económico", Description:="Grupo económico")> _
        Public Property IdGrupoEconomico As Integer

        <Required(ErrorMessage:="El nombre es un campo requerido")> _
        <Display(Name:="Nombre", Description:="Nombre")> _
        Public Property NombreGrupo As String

        <Required(ErrorMessage:="El comitente líder es un campo requerido")> _
        <Display(Name:="Comitente líder", Description:="Comitente líder")> _
        Public Property ComitenteLider As String

        <Required(ErrorMessage:="El nombre del líder es un campo requerido")> _
        <Display(Name:="Nombre líder", Description:="Nombre líder")> _
        Public Property NombreLider As String

    End Class



End Class


''' <summary>
''' Metadata para Clasificacion de Riesgo -- Jorge Andres Bedoya -- 23/01/2014
''' </summary>
''' <remarks></remarks>
'''
<MetadataTypeAttribute(GetType(ClasificacionRiesgo.ClasificacionRiesgoMetaData))> _
Partial Public Class ClasificacionRiesgo
    Friend NotInheritable Class ClasificacionRiesgoMetaData
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Código")> _
        Public Property IdClasificacionRiesgo As Integer

        <Required(ErrorMessage:="El tipo clasificación es un campo requerido")> _
        <Display(Name:="Tipo clasificación")> _
        Public Property IdTipoClasificacionRiesgo As Integer

        <Required(ErrorMessage:="El prefijo es un campo requerido")> _
              <Display(Name:="Prefijo")> _
        Public Property Prefijo As String

        <Required(ErrorMessage:="El Detalle es un campo requerido")> _
              <Display(Name:="Detalle")> _
        Public Property Detalle As String

        <Display(Name:="Generar alerta")> _
        Public Property GenerarAlerta As Boolean

    End Class
End Class

''' <summary>
''' Metadata para Carteras colectivas GMF -- Jorge Andres Bedoya -- 20/10/2014
''' </summary>
''' <remarks></remarks>
'''
<MetadataTypeAttribute(GetType(CarterasColectivasClientesGMF.CarterasColectivasClientesGMFMetaData))> _
Partial Public Class CarterasColectivasClientesGMF
    Friend NotInheritable Class CarterasColectivasClientesGMFMetaData
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="Código")> _
        Public Property IDCarterasColectivasClientesGMF As Integer

        <Display(Name:="Código Comitente")> _
        Public Property IDComitente As String

        <Required(ErrorMessage:="El tipo de documento es requerido")> _
        <Display(Name:="Tipo documento")> _
        Public Property TipoIdentificacion As String

        <Required(ErrorMessage:="El número de documento es requerido")> _
        <Display(Name:="Número documento")> _
        Public Property NroDocumento As String

        <Display(Name:="Nombre")> _
        Public Property Nombre As String

    End Class
End Class


''' <summary>
''' Metadata para Bancos nacionales fondos
''' </summary>
''' <remarks></remarks>
'''
<MetadataTypeAttribute(GetType(BancosNacionalesFondos.BancosNacionalesFondosMetaData))> _
Partial Public Class BancosNacionalesFondos
    Friend NotInheritable Class BancosNacionalesFondosMetaData
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID")> _
        Public Property ID As Integer

        <Required(ErrorMessage:="El banco es un campo requerido")> _
        <Display(Name:="Banco")> _
        Public Property IDBanco As Integer

        <Required(ErrorMessage:="El nro de cuenta es un campo requerido")> _
        <Display(Name:="Nro cuenta")> _
        Public Property NroCuenta As String

        <Required(ErrorMessage:="El tipo cuenta es un campo requerido")> _
        <Display(Name:="Tipo cuenta")> _
        Public Property TipoCuenta As String

        <Required(ErrorMessage:="El Tipo documento titular es un campo requerido")> _
        <Display(Name:="Tipo documento titular")> _
        Public Property TipoDocumentoTitular As String

        <Required(ErrorMessage:="El Nro documento titular es un campo requerido")> _
        <Display(Name:="Nro documento titular")> _
        Public Property NroDocumentoTitular As String

        <Required(ErrorMessage:="El Nombre titular es un campo requerido")> _
        <Display(Name:="Nombre titular")> _
        Public Property NombreTitular As String

        <Display(Name:="Código banco cartera")> _
        Public Property IDCodigoBancoSafyr As Nullable(Of Integer)

    End Class
End Class

#Region "JULIAN DAVID RINCON HENAO"

<MetadataTypeAttribute(GetType(PerfilesRiesgo.PerfilesRiesgoMetadata))> _
Partial Public Class PerfilesRiesgo
    Friend NotInheritable Class PerfilesRiesgoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="ID", Description:="ID")> _
        Public Property PerfilRiesgo As Integer

        <Display(Name:="Tipo perfil", Description:="Tipo perfil")> _
        <Required(ErrorMessage:="Este campo es requerido. (Tipo perfil)")> _
        Public Property TipoPerfil As Integer

        <Display(Name:="Descripción perfil", Description:="Descripción perfil")> _
        <Required(ErrorMessage:="Este campo es requerido. (Descripción perfil)")> _
        Public Property IDDescPerfil As String

        <Display(Name:="Calificación perfil", Description:="Calificación perfil")> _
        <Required(ErrorMessage:="Este campo es requerido. (Calificación perfil)")> _
        Public Property CalificacionPerfil As Decimal

        <Display(Name:="Actualización", Description:="Actualización")> _
        Public Property Actualizacion As String

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String
    End Class
End Class

#End Region

#Region "DECEVAL"
''' <summary>
''' Metadata para detalle empleado -- Carlos Andres Toro -- febrero/2015
''' </summary>
''' <remarks></remarks>
'''
<MetadataTypeAttribute(GetType(EmpleadoSistema.EmpleadoSistemaMetadata))> _
Partial Public Class EmpleadoSistema
    Friend NotInheritable Class EmpleadoSistemaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDEmpleado", Description:="IDEmpleado")> _
        Public Property IDEmpleado As Integer

        <Display(Name:="ID", Description:="ID")> _
        Public Property ID As Integer

        <Display(Name:="Sistema", Description:="Sistema")> _
        Public Property Usuario As String

        <Display(Name:="Código mapeo", Description:="Código mapeo")> _
        Public Property CodigoMapeo As String

        <Display(Name:="Valor", Description:="Valor")> _
        Public Property Valor As String

    End Class
End Class

#End Region

<MetadataTypeAttribute(GetType(Paise.PaiseMetadata))> _
Partial Public Class Paise
    Friend NotInheritable Class PaiseMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")> _
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        Public Property IDSucComisionista As Integer

        <Display(Name:="ID")> _
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
        <Display(Name:="Nombre")> _
        Public Property Nombre As String

        <Display(Name:="Código ISO")> _
        Public Property Codigo_ISO As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

        <Display(Name:="Pais")> _
        Public Property IDPais As Integer

        <Display(Name:="Código Dane")> _
        Public Property CodigoDane As String

    End Class
End Class

<MetadataTypeAttribute(GetType(Departamento.DepartamentoMetadata))> _
Partial Public Class Departamento
    Friend NotInheritable Class DepartamentoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")> _
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        Public Property IDSucComisionista As Integer

        <Display(Name:="Pais")> _
        Public Property IDPais As Integer

        <Display(Name:="ID")> _
        Public Property ID As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
        <StringLength(40, ErrorMessage:="El campo permite máximo 40 caracteres..")> _
        <Display(Name:="Nombre")> _
        Public Property Nombre As String

        <Display(Name:="Codigo Dane DPTO.")> _
        <StringLength(2, ErrorMessage:="El campo permite máximo 2 caracteres..")> _
        Public Property Codigo_DaneDEPTO As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

        <Display(Name:="Departamento")> _
        Public Property IDDepartamento As Integer

    End Class
End Class

<MetadataTypeAttribute(GetType(Ciudade.CiudadeMetadata))> _
Partial Public Class Ciudade
    Friend NotInheritable Class CiudadeMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Display(Name:="IDComisionista", Description:="IDComisionista")> _
        Public Property IDComisionista As Integer

        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
        Public Property IDSucComisionista As Integer


        <Required(ErrorMessage:="Este campo es requerido. (Código)")> _
          <Display(Name:="Código")> _
        Public Property IDCodigo As Integer

        <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
          <Display(Name:="Nombre")> _
        Public Property Nombre As String

        <Display(Name:="Es Capital")> _
        Public Property EsCapital As Boolean

        <Required(ErrorMessage:="Este campo es requerido. (Departamento)")> _
          <Display(Name:="Departamento")> _
        Public Property IDdepartamento As Integer

        <Display(Name:="Código DANE")> _
        Public Property CodigoDANE As String

        <Display(Name:="Actualizacion", Description:="Actualizacion")> _
        Public Property Actualizacion As DateTime

        <Display(Name:="Usuario", Description:="Usuario")> _
        Public Property Usuario As String

        <Display(Name:="Ciudad")> _
        Public Property IDCiudad As Integer

    End Class
End Class