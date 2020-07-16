Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel

Public Class clsGenerales

    Shared Function CargarListas(ByVal lstCombos As List(Of CPX_ComboOrdenes)) As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Try
            Dim dicListas As New Dictionary(Of String, ObservableCollection(Of ItemLista))

            Dim listas = From l In lstCombos Select l.strTopico Distinct
            For Each lista In listas
                Dim CollListas As New ObservableCollection(Of ItemLista)

                For Each Item In (From c In lstCombos Where c.strTopico = lista Select c)

                    CollListas.Add(New ItemLista(Item.strRetorno, Item.strDescripcion))
                Next

                dicListas.Add(lista, CollListas)
            Next
            If Application.Current.Resources.Contains("Listas") Then
                Application.Current.Resources.Remove("Listas")
            End If

            Application.Current.Resources.Add("Listas", dicListas)

            Return dicListas

        Catch ex As Exception
            MessageBox.Show("Error al cargar listas: " & ex.Message)
            Return Nothing
        End Try
    End Function


    Shared Function CargarListasDivisas(ByVal lstCombos As List(Of CPX_ComboOrdenesDivisas)) As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Try
            Dim dicListas As New Dictionary(Of String, ObservableCollection(Of ItemLista))

            Dim listas = From l In lstCombos Select l.strTopico Distinct
            For Each lista In listas
                Dim CollListas As New ObservableCollection(Of ItemLista)

                For Each Item In (From c In lstCombos Where c.strTopico = lista Select c)

                    CollListas.Add(New ItemLista(Item.strRetorno, Item.strDescripcion))
                Next

                dicListas.Add(lista, CollListas)
            Next
            If Application.Current.Resources.Contains("Listas") Then
                Application.Current.Resources.Remove("Listas")
            End If

            Application.Current.Resources.Add("Listas", dicListas)

            Return dicListas

        Catch ex As Exception
            MessageBox.Show("Error al cargar listas: " & ex.Message)
            Return Nothing
        End Try
    End Function


End Class




Public Class ItemLista
    Implements INotifyPropertyChanged

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Sub New(pRetorno As String, pDescripcion As String)
        Me.Retorno = pRetorno
        Me.Descripcion = pDescripcion
    End Sub

    Private _Retorno As String
    Public Property Retorno() As String
        Get
            Return _Retorno
        End Get
        Set(ByVal Descripcion As String)
            _Retorno = Descripcion
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Retorno"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal Descripcion As String)
            _Descripcion = Descripcion
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

End Class

