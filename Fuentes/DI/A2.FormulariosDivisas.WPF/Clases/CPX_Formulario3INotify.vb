Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CPX_Formulario3INotify
	Implements INotifyPropertyChanged

	Public Event PropertyChanged As PropertyChangedEventHandler _
		Implements INotifyPropertyChanged.PropertyChanged

	Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
		RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
	End Sub


	Private _lngNumeroDecl As Integer
	Public Property lngNumeroDecl() As Integer
		Get
			Return _lngNumeroDecl
		End Get
		Set(ByVal value As Integer)
			_lngNumeroDecl = value
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
	Private _lngCiudad As Nullable(Of Integer)
	Public Property lngCiudad() As Nullable(Of Integer)
		Get
			Return _lngCiudad
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_lngCiudad = value
			NotifyPropertyChanged()
		End Set
	End Property
	Private _lngNit As String
	Public Property lngNit() As String
		Get
			Return _lngNit
		End Get
		Set(ByVal value As String)
			_lngNit = value
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
	Private _lngNitAnterior As String
	Public Property lngNitAnterior() As String
		Get
			Return _lngNitAnterior
		End Get
		Set(ByVal value As String)
			_lngNitAnterior = value
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
	Private _lngNumeroDeclAnterior As Nullable(Of Integer)
	Public Property lngNumeroDeclAnterior() As Nullable(Of Integer)
		Get
			Return _lngNumeroDeclAnterior
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_lngNumeroDeclAnterior = value
			NotifyPropertyChanged()
		End Set
	End Property
	Private _lngCiudadDomicilio As Nullable(Of Integer)
	Public Property lngCiudadDomicilio() As Nullable(Of Integer)
		Get
			Return _lngCiudadDomicilio
		End Get
		Set(ByVal value As Nullable(Of Integer))
			_lngCiudadDomicilio = value
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
	Private _lngNumeroIdentDeclarante As String
	Public Property lngNumeroIdentDeclarante() As String
		Get
			Return _lngNumeroIdentDeclarante
		End Get
		Set(ByVal value As String)
			_lngNumeroIdentDeclarante = value
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
	Private _lngNumeroDeclAnteriorFirma As String
	Public Property lngNumeroDeclAnteriorFirma() As String
		Get
			Return _lngNumeroDeclAnteriorFirma
		End Get
		Set(ByVal value As String)
			_lngNumeroDeclAnteriorFirma = value
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
	Private _lngNumeroidentificacion As String
	Public Property lngNumeroidentificacion() As String
		Get
			Return _lngNumeroidentificacion
		End Get
		Set(ByVal value As String)
			_lngNumeroidentificacion = value
			NotifyPropertyChanged()
		End Set
	End Property
	Private _lngDigitoVerificacion As String
	Public Property lngDigitoVerificacion() As String
		Get
			Return _lngDigitoVerificacion
		End Get
		Set(ByVal value As String)
			_lngDigitoVerificacion = value
			NotifyPropertyChanged()
		End Set
	End Property
	Private _RazonSocial As String
	Public Property RazonSocial() As String
		Get
			Return _RazonSocial
		End Get
		Set(ByVal value As String)
			_RazonSocial = value
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
