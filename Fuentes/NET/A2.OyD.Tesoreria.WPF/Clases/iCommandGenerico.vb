Imports Telerik.Windows.Controls
Imports System.Windows.Input

''' <summary> 
''' Genérico ICommand implementación para ayudar con el procesamiento de órdenes en MVVM
''' </summary> 
Public Class DelegateCommandGenerico(Of T)
    Implements ICommand

#Region "Propiedades"

    ''' <summary> 
    ''' Define el subprograma para la acción a ejecutar. 
    ''' </summary> 
    Private ReadOnly executeAction As Action(Of T)

    ''' <summary> 
    ''' Define la función que determina si la acción puede ser ejecutada.
    ''' </summary> 
    Private ReadOnly canExecuteAction As Func(Of T, Boolean)

#End Region

#Region "Eventos"

    ''' <summary> 
    ''' Define un evento para evaluar, cuando los valores que afectan a "CanExecute" se cambian
    ''' </summary> 
    Public Event CanExecuteChanged(ByVal sender As Object,
                          ByVal e As System.EventArgs) _
         Implements System.Windows.Input.ICommand.CanExecuteChanged

#End Region

#Region "Métodos"

    ''' <summary> 
    ''' Construye un objeto que siempre se puede ejecutar. 
    ''' </summary> 
    Public Sub New(ByVal currentExecuteAction As Action(Of T))
        Me.New(currentExecuteAction, Nothing)
    End Sub

    ''' <summary> 
    ''' Construye un objeto a ejecutar. 
    ''' </summary> 
    Public Sub New(ByVal currentExecuteAction As Action(Of T),
            ByVal currentCanExecuteAction As Func(Of T, Boolean))
        executeAction = currentExecuteAction
        canExecuteAction = currentCanExecuteAction
    End Sub

    ''' <summary> 
    ''' Define el método que determina si el comando se puede ejecutar en su estado actual.
    ''' </summary> 
    Public Function CanExecute(ByVal parameter As Object) As Boolean _
                  Implements System.Windows.Input.ICommand.CanExecute
        If canExecuteAction IsNot Nothing Then
            Return canExecuteAction(DirectCast(parameter, T))
        Else
            Return True
        End If
    End Function

    ''' <summary> 
    ''' Define el método que se llamará cuando el comando se invoca.
    ''' </summary> 
    Public Sub Execute(ByVal parameter As Object) _
              Implements System.Windows.Input.ICommand.Execute
        If CanExecute(parameter) Then
            executeAction(DirectCast(parameter, T))
        End If
    End Sub

#End Region


End Class


