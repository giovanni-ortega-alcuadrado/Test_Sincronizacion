Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CPX_Formulario5INotify
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler _
    Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub


    Private _intNumeroDecl As Integer
    Public Property intNumeroDecl() As Integer
        Get
            Return _intNumeroDecl
        End Get
        Set(ByVal value As Integer)
            _intNumeroDecl = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngFormulario As Integer
    Public Property lngFormulario() As Integer
        Get
            Return _lngFormulario
        End Get
        Set(ByVal value As Integer)
            _lngFormulario = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _intNumeral As Nullable(Of Integer)
    Public Property intNumeral() As Nullable(Of Integer)
        Get
            Return _intNumeral
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intNumeral = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblValorUSD As Nullable(Of Decimal)
    Public Property dblValorUSD() As Nullable(Of Decimal)
        Get
            Return _dblValorUSD
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValorUSD = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCondicionNegocio As String
    Public Property strCondicionNegocio() As String
        Get
            Return _strCondicionNegocio
        End Get
        Set(ByVal value As String)
            _strCondicionNegocio = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intTipoOperacion As Nullable(Of Short)
    Public Property intTipoOperacion() As Nullable(Of Short)
        Get
            Return _intTipoOperacion
        End Get
        Set(ByVal value As Nullable(Of Short))
            _intTipoOperacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngIDOrden As Integer
    Public Property lngIDOrden() As Integer
        Get
            Return _lngIDOrden
        End Get
        Set(ByVal value As Integer)
            _lngIDOrden = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intCiudad As Nullable(Of Integer)
    Public Property intCiudad() As Nullable(Of Integer)
        Get
            Return _intCiudad
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intCiudad = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNit As String
    Public Property strNit() As String
        Get
            Return _strNit
        End Get
        Set(ByVal value As String)
            _strNit = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dtmFecha As Date
    Public Property dtmFecha() As Date
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As Date)
            _dtmFecha = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNitAnterior As String
    Public Property strNitAnterior() As String
        Get
            Return _strNitAnterior
        End Get
        Set(ByVal value As String)
            _strNitAnterior = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dtmFechaAnterior As Nullable(Of Date)
    Public Property dtmFechaAnterior() As Nullable(Of Date)
        Get
            Return _dtmFechaAnterior
        End Get
        Set(ByVal value As Nullable(Of Date))
            _dtmFechaAnterior = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intNumeroDeclAnterior As Nullable(Of Integer)
    Public Property intNumeroDeclAnterior() As Nullable(Of Integer)
        Get
            Return _intNumeroDeclAnterior
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intNumeroDeclAnterior = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intCiudadDomicilio As Nullable(Of Integer)
    Public Property intCiudadDomicilio() As Nullable(Of Integer)
        Get
            Return _intCiudadDomicilio
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intCiudadDomicilio = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCondiccionespago As String
    Public Property strCondiccionespago() As String
        Get
            Return _strCondiccionespago
        End Get
        Set(ByVal value As String)
            _strCondiccionespago = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCondiccionesDespacho As String
    Public Property strCondiccionesDespacho() As String
        Get
            Return _strCondiccionesDespacho
        End Get
        Set(ByVal value As String)
            _strCondiccionesDespacho = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNombreDeclarante As String
    Public Property strNombreDeclarante() As String
        Get
            Return _strNombreDeclarante
        End Get
        Set(ByVal value As String)
            _strNombreDeclarante = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNumeroIdentDeclarante As String
    Public Property strNumeroIdentDeclarante() As String
        Get
            Return _strNumeroIdentDeclarante
        End Get
        Set(ByVal value As String)
            _strNumeroIdentDeclarante = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _blnEnviado As Nullable(Of Boolean)
    Public Property blnEnviado() As Nullable(Of Boolean)
        Get
            Return _blnEnviado
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _blnEnviado = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strTelefono As String
    Public Property strTelefono() As String
        Get
            Return _strTelefono
        End Get
        Set(ByVal value As String)
            _strTelefono = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strDireccion As String
    Public Property strDireccion() As String
        Get
            Return _strDireccion
        End Get
        Set(ByVal value As String)
            _strDireccion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCodigoMoneda1 As String
    Public Property strCodigoMoneda1() As String
        Get
            Return _strCodigoMoneda1
        End Get
        Set(ByVal value As String)
            _strCodigoMoneda1 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblTipoCambioMoneda1 As Nullable(Of Decimal)
    Public Property dblTipoCambioMoneda1() As Nullable(Of Decimal)
        Get
            Return _dblTipoCambioMoneda1
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblTipoCambioMoneda1 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngNumeralcambiario1 As Nullable(Of Integer)
    Public Property lngNumeralcambiario1() As Nullable(Of Integer)
        Get
            Return _lngNumeralcambiario1
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _lngNumeralcambiario1 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblValormoneda1 As Nullable(Of Decimal)
    Public Property dblValormoneda1() As Nullable(Of Decimal)
        Get
            Return _dblValormoneda1
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValormoneda1 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblValorUSD1 As Nullable(Of Decimal)
    Public Property dblValorUSD1() As Nullable(Of Decimal)
        Get
            Return _dblValorUSD1
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValorUSD1 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCodigoMoneda2 As String
    Public Property strCodigoMoneda2() As String
        Get
            Return _strCodigoMoneda2
        End Get
        Set(ByVal value As String)
            _strCodigoMoneda2 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strIngresoEgreso As String
    Public Property strIngresoEgreso() As String
        Get
            Return _strIngresoEgreso
        End Get
        Set(ByVal value As String)
            _strIngresoEgreso = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngIDOperacion As Nullable(Of Integer)
    Public Property lngIDOperacion() As Nullable(Of Integer)
        Get
            Return _lngIDOperacion
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _lngIDOperacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNombreDeclaranteFirma As String
    Public Property strNombreDeclaranteFirma() As String
        Get
            Return _strNombreDeclaranteFirma
        End Get
        Set(ByVal value As String)
            _strNombreDeclaranteFirma = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intNumeroDeclAnteriorFirma As String
    Public Property intNumeroDeclAnteriorFirma() As String
        Get
            Return _intNumeroDeclAnteriorFirma
        End Get
        Set(ByVal value As String)
            _intNumeroDeclAnteriorFirma = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strEstado As String
    Public Property strEstado() As String
        Get
            Return _strEstado
        End Get
        Set(ByVal value As String)
            _strEstado = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _blnEnviadoDian As Boolean
    Public Property blnEnviadoDian() As Boolean
        Get
            Return _blnEnviadoDian
        End Get
        Set(ByVal value As Boolean)
            _blnEnviadoDian = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _logFirmaFormPend As Nullable(Of Boolean)
    Public Property logFirmaFormPend() As Nullable(Of Boolean)
        Get
            Return _logFirmaFormPend
        End Get
        Set(ByVal value As Nullable(Of Boolean))
            _logFirmaFormPend = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngDeclaracionConsolidado As Nullable(Of Integer)
    Public Property lngDeclaracionConsolidado() As Nullable(Of Integer)
        Get
            Return _lngDeclaracionConsolidado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _lngDeclaracionConsolidado = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dtmFechaActualizacion As Date
    Public Property dtmFechaActualizacion() As Date
        Get
            Return _dtmFechaActualizacion
        End Get
        Set(ByVal value As Date)
            _dtmFechaActualizacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strUsuario As String
    Public Property strUsuario() As String
        Get
            Return _strUsuario
        End Get
        Set(ByVal value As String)
            _strUsuario = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngIDComisionista As Integer
    Public Property lngIDComisionista() As Integer
        Get
            Return _lngIDComisionista
        End Get
        Set(ByVal value As Integer)
            _lngIDComisionista = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngIDSucComisionista As Integer
    Public Property lngIDSucComisionista() As Integer
        Get
            Return _lngIDSucComisionista
        End Get
        Set(ByVal value As Integer)
            _lngIDSucComisionista = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strTipoDocumento As String
    Public Property strTipoDocumento() As String
        Get
            Return _strTipoDocumento
        End Get
        Set(ByVal value As String)
            _strTipoDocumento = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intNumeroidentificacion As String
    Public Property intNumeroidentificacion() As String
        Get
            Return _intNumeroidentificacion
        End Get
        Set(ByVal value As String)
            _intNumeroidentificacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intDigitoVerificacion As String
    Public Property intDigitoVerificacion() As String
        Get
            Return _intDigitoVerificacion
        End Get
        Set(ByVal value As String)
            _intDigitoVerificacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strRazonSocial As String
    Public Property strRazonSocial() As String
        Get
            Return _strRazonSocial
        End Get
        Set(ByVal value As String)
            _strRazonSocial = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strObservaciones As String
    Public Property strObservaciones() As String
        Get
            Return _strObservaciones
        End Get
        Set(ByVal value As String)
            _strObservaciones = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblTotalvalorFOB As Nullable(Of Decimal)
    Public Property dblTotalvalorFOB() As Nullable(Of Decimal)
        Get
            Return _dblTotalvalorFOB
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblTotalvalorFOB = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblTotalGastosExportacion As Nullable(Of Decimal)
    Public Property dblTotalGastosExportacion() As Nullable(Of Decimal)
        Get
            Return _dblTotalGastosExportacion
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblTotalGastosExportacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblDeducciones As Nullable(Of Decimal)
    Public Property dblDeducciones() As Nullable(Of Decimal)
        Get
            Return _dblDeducciones
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblDeducciones = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblReintegroNeto As Nullable(Of Decimal)
    Public Property dblReintegroNeto() As Nullable(Of Decimal)
        Get
            Return _dblReintegroNeto
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblReintegroNeto = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblTipoCambioMoneda2 As Nullable(Of Decimal)
    Public Property dblTipoCambioMoneda2() As Nullable(Of Decimal)
        Get
            Return _dblTipoCambioMoneda2
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblTipoCambioMoneda2 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngNumeralcambiario2 As Nullable(Of Integer)
    Public Property lngNumeralcambiario2() As Nullable(Of Integer)
        Get
            Return _lngNumeralcambiario2
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _lngNumeralcambiario2 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblValormoneda2 As Nullable(Of Decimal)
    Public Property dblValormoneda2() As Nullable(Of Decimal)
        Get
            Return _dblValormoneda2
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValormoneda2 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dblValorUSD2 As Nullable(Of Decimal)
    Public Property dblValorUSD2() As Nullable(Of Decimal)
        Get
            Return _dblValorUSD2
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValorUSD2 = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strGlobal As String
    Public Property strGlobal() As String
        Get
            Return _strGlobal
        End Get
        Set(ByVal value As String)
            _strGlobal = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCorreccion As String
    Public Property strCorreccion() As String
        Get
            Return _strCorreccion
        End Get
        Set(ByVal value As String)
            _strCorreccion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strTipoReferencia As String
    Public Property strTipoReferencia() As String
        Get
            Return _strTipoReferencia
        End Get
        Set(ByVal value As String)
            _strTipoReferencia = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCorreoElectronicoDecl As String
    Public Property strCorreoElectronicoDecl() As String
        Get
            Return _strCorreoElectronicoDecl
        End Get
        Set(ByVal value As String)
            _strCorreoElectronicoDecl = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strCodigoCiudadDecl As String
    Public Property strCodigoCiudadDecl() As String
        Get
            Return _strCodigoCiudadDecl
        End Get
        Set(ByVal value As String)
            _strCodigoCiudadDecl = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNumeroPrestamoOp As String
    Public Property strNumeroPrestamoOp() As String
        Get
            Return _strNumeroPrestamoOp
        End Get
        Set(ByVal value As String)
            _strNumeroPrestamoOp = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strTipoDocumentoOp As String
    Public Property strTipoDocumentoOp() As String
        Get
            Return _strTipoDocumentoOp
        End Get
        Set(ByVal value As String)
            _strTipoDocumentoOp = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngNumeroidentificacionOp As String
    Public Property lngNumeroidentificacionOp() As String
        Get
            Return _lngNumeroidentificacionOp
        End Get
        Set(ByVal value As String)
            _lngNumeroidentificacionOp = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _lngDigitoVerificacionEmpOp As String
    Public Property lngDigitoVerificacionEmpOp() As String
        Get
            Return _lngDigitoVerificacionEmpOp
        End Get
        Set(ByVal value As String)
            _lngDigitoVerificacionEmpOp = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNombreOperacion As String
    Public Property strNombreOperacion() As String
        Get
            Return _strNombreOperacion
        End Get
        Set(ByVal value As String)
            _strNombreOperacion = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _strNombreAcreedor As String
    Public Property strNombreAcreedor() As String
        Get
            Return _strNombreAcreedor
        End Get
        Set(ByVal value As String)
            _strNombreAcreedor = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intNumeroBcoRep As Nullable(Of Integer)
    Public Property intNumeroBcoRep() As Nullable(Of Integer)
        Get
            Return _intNumeroBcoRep
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intNumeroBcoRep = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _intNumeroBcoRepAnterior As Nullable(Of Integer)
    Public Property intNumeroBcoRepAnterior() As Nullable(Of Integer)
        Get
            Return _intNumeroBcoRepAnterior
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intNumeroBcoRepAnterior = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dtmFechaBcoRep As Nullable(Of Date)
    Public Property dtmFechaBcoRep() As Nullable(Of Date)
        Get
            Return _dtmFechaBcoRep
        End Get
        Set(ByVal value As Nullable(Of Date))
            _dtmFechaBcoRep = value
            NotifyPropertyChanged()
        End Set
    End Property
    Private _dtmFechaBcoRepAnterior As Nullable(Of Date)
    Public Property dtmFechaBcoRepAnterior() As Nullable(Of Date)
        Get
            Return _dtmFechaBcoRepAnterior
        End Get
        Set(ByVal value As Nullable(Of Date))
            _dtmFechaBcoRepAnterior = value
            NotifyPropertyChanged()
        End Set
    End Property
End Class
