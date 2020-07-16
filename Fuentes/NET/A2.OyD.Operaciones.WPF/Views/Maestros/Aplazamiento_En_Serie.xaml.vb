Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class Aplazamiento_En_Serie
    Inherits Window
    Implements INotifyPropertyChanged
    Dim Fechaliquidacion As DateTime

    Public Sub New(ByVal strTipoAplazamiento As String, ByVal dtmliquidacion As DateTime, ByVal dtmtitulo As DateTime, _
                   Optional APLAZAMIENTO_LIQUIDACIONES_AMBOS As String = "NO")
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        If (strTipoAplazamiento <> String.Empty) And (Not IsNothing(dtmliquidacion)) Then
            Aplazamiento.Title = "Aplazamiento" + IIf(strTipoAplazamiento.Equals("S"), " en Serie", " Individual").ToString
            Fechaliquidacion = dtmliquidacion
        End If

        TipoSelected.cboAplazamiento.Add(New COMBOAPLAZAMIENTO With {.Descripcion = "Titulo"})
        TipoSelected.cboAplazamiento.Add(New COMBOAPLAZAMIENTO With {.Descripcion = "Efectivo"})

        If APLAZAMIENTO_LIQUIDACIONES_AMBOS.ToUpper.Equals("SI") Then
            TipoSelected.cboAplazamiento.Add(New COMBOAPLAZAMIENTO With {.Descripcion = "Ambos"})
            TipoSelected.Descripcion = "Ambos"
        Else
            TipoSelected.Descripcion = "Titulo"
        End If

        TipoSelected.FechaAplazamiento = dtmtitulo.AddDays(1)

    End Sub



    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        If CDate(TipoSelected.FechaAplazamiento).Date < CDate(Fechaliquidacion).Date Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de aplazamiento(" & TipoSelected.FechaAplazamiento.Date.ToShortDateString & ") no debe ser inferior " & vbCrLf _
 & " a la fecha de liquidacion(" & Fechaliquidacion.Date.ToShortDateString & ")", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub
    'Private _cboAplazamiento As List(Of COMBOAPLAZAMIENTO) = New List(Of COMBOAPLAZAMIENTO)
    'Public Property cboAplazamiento As List(Of COMBOAPLAZAMIENTO)
    '    Get
    '        Return _cboAplazamiento
    '    End Get
    '    Set(ByVal value As List(Of COMBOAPLAZAMIENTO))
    '        _cboAplazamiento = value
    '        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("cboAplazamiento"))

    '    End Set
    'End Property

    Private _TipoSelected As APLAZAMIENTO = New APLAZAMIENTO
    Public Property TipoSelected As APLAZAMIENTO
        Get
            Return _TipoSelected
        End Get
        Set(ByVal value As APLAZAMIENTO)
            _TipoSelected = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoSelected"))
        End Set
    End Property

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

Public Class APLAZAMIENTO
    Implements INotifyPropertyChanged

    Private _Descripcion As String
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Private _FechaAplazamiento As DateTime
    Public Property FechaAplazamiento As DateTime
        Get
            Return _FechaAplazamiento
        End Get
        Set(ByVal value As DateTime)
            _FechaAplazamiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaAplazamiento"))
        End Set
    End Property


    Private _cboAplazamiento As List(Of COMBOAPLAZAMIENTO) = New List(Of COMBOAPLAZAMIENTO)
    Public Property cboAplazamiento As List(Of COMBOAPLAZAMIENTO)
        Get
            Return _cboAplazamiento
        End Get
        Set(ByVal value As List(Of COMBOAPLAZAMIENTO))
            _cboAplazamiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("cboAplazamiento"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class COMBOAPLAZAMIENTO
    Implements INotifyPropertyChanged

    Private _Descripcion As String
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
