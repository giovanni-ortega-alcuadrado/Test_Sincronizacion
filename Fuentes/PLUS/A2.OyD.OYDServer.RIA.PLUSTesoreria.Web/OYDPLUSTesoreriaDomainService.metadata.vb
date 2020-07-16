Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System.ComponentModel.DataAnnotations

Namespace OyDPLUSTesoreria

#Region "OyD Plus"

    <MetadataTypeAttribute(GetType(TesoreriaOrdenesEncabezado.TesoreriaOrdenesEncabezadoMetadata))> _
    Partial Public Class TesoreriaOrdenesEncabezado
        Friend NotInheritable Class TesoreriaOrdenesEncabezadoMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="ID Orden de Tesoreria", Description:="ID Orden de Tesoreria")> _
            Property lngID As Integer

            <Display(Name:="Tipo Producto", Description:="Tipo Producto")> _
            Property strTipoProducto As String

            <Display(Name:="Código Receptor", Description:="Código Receptor")> _
            Property strCodigoReceptor As String

            ', Description:="Nro Documento")> _
            <Display(Name:="Nro Documento")> _
            Property strNroDocumento As String

            <Display(Name:="Nro Documento", Description:="Nro Documento")> _
            Property lngNroDocumento As String

            <Display(Name:="Tipo Identificación", Description:="Tipo Identificación")> _
            Property strTipoIdentificacion As String

            ', Description:="Nombre"
            <Display(Name:="Nombre")> _
            Property strNombre As String

            <Display(Name:="Valor", Description:="Valor")> _
            Property curValor As Double

            ', Description:="ID Comitente"
            <Display(Name:="ID Comitente")> _
            Property strIDComitente As String

            <Display(Name:="Fecha Documento", Description:="FEcha Documento")> _
            Property dtmDocumento As Date

            <Display(Name:="Estado Orden Tesoreria", Description:="Estado Orden Tesoreria")> _
            Property strEstado As String

            <Display(Name:="Usuario", Description:="Usuario")> _
            Property strUsuario As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(TesoreriaOrdenesDetalleChequesOyDPlus.TesoreriaOrdenesDetalleChequesOyDPlusMetadata))> _
    Partial Public Class TesoreriaOrdenesDetalleChequesOyDPlus
        Friend NotInheritable Class TesoreriaOrdenesDetalleChequesOyDPlusMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="ID Tesoreria Detalle", Description:="ID Tesoreria Detalle")> _
            Property lngID As Integer

            <Display(Name:="ID Tesoreria Encabezado", Description:="ID Tesoreria Encabezado")> _
            Property lngIDTesoreriaOrdenes As Integer

            <Display(Name:="Tipo", Description:="Tipo")> _
            Property strTipo As String

            <Display(Name:="Forma Pago", Description:="Forma Pago")> _
            Property strFormaPago As String

            <Display(Name:="ID Concepto", Description:="ID Concepto")> _
            Property lngIDConcepto As String

            <Display(Name:="Concepto", Description:="Concepto")> _
            Property strDetalleConcepto As String

            <Display(Name:="Nro Documento", Description:="Nro Documento")> _
            Property strNroDocumento As String

            <Display(Name:="Tipo Documento", Description:="Tipo Documento")> _
            Property strTipoDocumento As String

            <Display(Name:="Beneficiario", Description:="Beneficiario")> _
            Property strNombre As String

            <Display(Name:="Es Tercero", Description:="Es Tercero")> _
            Property strEsTercero As String

            <Display(Name:="Valor", Description:="Valor")> _
            Property curValor As Double

            <Display(Name:="Tipo Cobro GMF", Description:="Tipo Cobro GMF")> _
            Property strTipoCobroGMF As String

            <Display(Name:="Tipo Cheque", Description:="Tipo Cheque")> _
            Property strTipoCheque As String

            <Display(Name:="Nro Cheque", Description:="Nro Cheque")> _
            Property lngNroCheque As Integer

            <Display(Name:="Es Cuenta Inscrita", Description:="Es Cuenta Inscrita")> _
            Property strEsCuentaRegistrada As String

            <Display(Name:="Cuenta", Description:="Cuenta")> _
            Property strCuenta As String

            <Display(Name:="Tipo Cuenta", Description:="Tipo Cuenta")> _
            Property strTipoCuenta As String

            <Display(Name:="Banco", Description:="Banco")> _
            Property lngIdBanco As Integer

            <Display(Name:="Nombre Titular", Description:="Nombre Titular")> _
            Property strNombreTitular As String

            <Display(Name:="Tipo Documento Titular", Description:="Tipo Documento Titular")> _
            Property strTipoDocumentoTitular As String

            <Display(Name:="Nro Documento Titular", Description:="Nro Documento Titular")> _
            Property strNroDocumentoTitular As String

            <Display(Name:="Valor GMF", Description:="Valor GMF")> _
            Property curValorGMF As Double

            <Display(Name:="Fecha Actualización", Description:="Fecha Actualización")> _
            Property dtmFechaActualizacion As DateTime

            <Display(Name:="InfoSesion", Description:="InfoSesion")> _
            Property InfoSesion As String
        End Class
    End Class

#End Region

End Namespace

