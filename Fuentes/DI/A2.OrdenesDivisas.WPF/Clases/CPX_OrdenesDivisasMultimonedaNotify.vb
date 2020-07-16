Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class CPX_OrdenesDivisasMultimonedaNotify
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub NotifyPropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private _intIDMonedaIntermediaM As Integer
    Public Property intIDMonedaIntermediaM() As Integer
        Get
            Return _intIDMonedaIntermediaM
        End Get
        Set(ByVal value As Integer)
            _intIDMonedaIntermediaM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblCantidadMultimoneda As Nullable(Of Double)
    Public Property dblCantidadMultimoneda() As Nullable(Of Double)
        Get
            Return _dblCantidadMultimoneda
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblCantidadMultimoneda = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblPrecioIntermedioM As Nullable(Of Decimal)
    Public Property dblPrecioIntermedioM() As Nullable(Of Decimal)
        Get
            Return _dblPrecioIntermedioM
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblPrecioIntermedioM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblSpreadComisionM As Nullable(Of Decimal)
    Public Property dblSpreadComisionM() As Nullable(Of Decimal)
        Get
            Return _dblSpreadComisionM
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblSpreadComisionM = value
            NotifyPropertyChanged()
        End Set
    End Property


    Private _dblValorBrutoMultimoneda As Nullable(Of Decimal)
    Public Property dblValorBrutoMultimoneda() As Nullable(Of Decimal)
        Get
            Return _dblValorBrutoMultimoneda
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValorBrutoMultimoneda = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblPrecioMonedaNegociadaM As Nullable(Of Decimal)
    Public Property dblPrecioMonedaNegociadaM() As Nullable(Of Decimal)
        Get
            Return _dblPrecioMonedaNegociadaM
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblPrecioMonedaNegociadaM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblComisionUSD As Nullable(Of Decimal)
    Public Property dblComisionUSD() As Nullable(Of Decimal)
        Get
            Return _dblComisionUSD
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblComisionUSD = value
            NotifyPropertyChanged()
        End Set
    End Property


    Private _dblCantidadM As Nullable(Of Double)
    Public Property dblCantidadM() As Nullable(Of Double)
        Get
            Return _dblCantidadM
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblCantidadM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblPrecioM As Nullable(Of Double)
    Public Property dblPrecioM() As Nullable(Of Double)
        Get
            Return _dblPrecioM
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblPrecioM = value
            NotifyPropertyChanged()
        End Set
    End Property


    Private _dblValorBrutoM As Nullable(Of Decimal)
    Public Property dblValorBrutoM() As Nullable(Of Decimal)
        Get
            Return _dblValorBrutoM
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValorBrutoM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblValorNetoM As Nullable(Of Decimal)
    Public Property dblValorNetoM() As Nullable(Of Decimal)
        Get
            Return _dblValorNetoM
        End Get
        Set(ByVal value As Nullable(Of Decimal))
            _dblValorNetoM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblComisionCOP As Nullable(Of Double)
    Public Property dblComisionCOP() As Nullable(Of Double)
        Get
            Return _dblComisionCOP
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblComisionCOP = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblValorRteFuenteM As Nullable(Of Double)
    Public Property dblValorRteFuenteM() As Nullable(Of Double)
        Get
            Return _dblValorRteFuenteM
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblValorRteFuenteM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblValorGMFM As Nullable(Of Double)
    Public Property dblValorGMFM() As Nullable(Of Double)
        Get
            Return _dblValorGMFM
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblValorGMFM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblBaseIVAM As Nullable(Of Double)
    Public Property dblBaseIVAM() As Nullable(Of Double)
        Get
            Return _dblBaseIVAM
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblBaseIVAM = value
            NotifyPropertyChanged()
        End Set
    End Property

    Private _dblValorIVAM As Nullable(Of Double)
    Public Property dblValorIVAM() As Nullable(Of Double)
        Get
            Return _dblValorIVAM
        End Get
        Set(ByVal value As Nullable(Of Double))
            _dblValorIVAM = value
            NotifyPropertyChanged()
        End Set
    End Property

End Class
