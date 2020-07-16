Imports Telerik.Windows.Controls
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Public Class CiudadesClase
    Implements INotifyPropertyChanged

    Private _Ciudad As String
    <Display(Name:="Ciudad")> _
    Public Property Ciudad As String
        Get
            Return _Ciudad
        End Get
        Set(ByVal value As String)
            _Ciudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ciudad"))
        End Set
    End Property

    Private _Departamento As String
    <Display(Name:="Departamento")> _
    Public Property Departamento As String
        Get
            Return _Departamento
        End Get
        Set(ByVal value As String)
            _Departamento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Departamento"))
        End Set
    End Property

    Private _Pais As String
    <Display(Name:="Pais")> _
    Public Property Pais As String
        Get
            Return _Pais
        End Get
        Set(ByVal value As String)
            _Pais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Pais"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class