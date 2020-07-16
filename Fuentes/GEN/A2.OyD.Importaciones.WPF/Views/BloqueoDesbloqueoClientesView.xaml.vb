'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClientesViewModel.vb
'Generado el : 07/08/2011 09:34:53
'Propiedad de Alcuadrado S.A. 2010
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data

Imports A2.OyD.OYDServer.RIA.Web
Imports OYDUtilidades
Imports Microsoft.VisualBasic.CompilerServices

Partial Public Class BloqueoDesbloqueoClientesView
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New()
        InitializeComponent()
        Bloqueod.DataContext = Me
    End Sub
#Region "Propiedades"
    Private _IDMotivo As Nullable(Of Integer) = Nothing
    Public Property IDMotivo As Nullable(Of Integer)
        Get
            Return _IDMotivo
        End Get
        Set(value As Nullable(Of Integer))
            _IDMotivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDMotivo"))
        End Set
    End Property
    Private _logBloqueo As Boolean
    Public Property logBloqueo As Boolean
        Get
            Return _logBloqueo
        End Get
        Set(value As Boolean)
            Try
                _logBloqueo = value
                If _logBloqueo Then
                    If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                        Dim objListaTipoID As New ObservableCollection(Of OYDUtilidades.ItemCombo)
                        For Each l In CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ConceptoInactividad")
                            objListaTipoID.Add(l)
                        Next
                        objTipoId = objListaTipoID
                    End If
                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logBloqueo"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle", _
                         Me.ToString(), "logBloqueo", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            End Try
        End Set
    End Property
    Private _logdesBloqueo As Boolean
    Public Property logdesBloqueo As Boolean
        Get
            Return _logdesBloqueo
        End Get
        Set(value As Boolean)
            Try
                _logdesBloqueo = value
                If _logdesBloqueo Then
                    If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                        Dim objListaTipoID As New ObservableCollection(Of OYDUtilidades.ItemCombo)
                        For Each l In CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ConceptoActividad")
                            objListaTipoID.Add(l)
                        Next

                        objTipoId = objListaTipoID
                    End If
                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logdesBloqueo"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle", _
                         Me.ToString(), "logdesBloqueo", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            End Try
        End Set
    End Property
    Private _objTipoId As ObservableCollection(Of OYDUtilidades.ItemCombo) = New ObservableCollection(Of OYDUtilidades.ItemCombo)
    Public Property objTipoId() As ObservableCollection(Of OYDUtilidades.ItemCombo)
        Get
            Return _objTipoId
        End Get
        Set(ByVal value As ObservableCollection(Of OYDUtilidades.ItemCombo))
            _objTipoId = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("objTipoId"))
        End Set
    End Property

    Private _NombreArchivo As String
    Public Property NombreArchivo As String
        Get
            Return _NombreArchivo
        End Get
        Set(value As String)
            _NombreArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreArchivo"))
        End Set
    End Property
#End Region
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs) Handles btnAceptar.Click
        If logBloqueo = False And logdesBloqueo = False Then
            A2Utilidades.Mensajes.mostrarMensaje("El campo Bloqueo ó Desbloqueo es requerido. Favor diligenciarlo para continuar con el proceso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If IsNothing(IDMotivo) Then
            A2Utilidades.Mensajes.mostrarMensaje("El campo Motivo es requerido. Favor diligenciarlo para continuar con el proceso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        Me.DialogResult = True
        Me.Close()
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'App.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub
End Class
