Imports Microsoft.Reporting.WinForms
Imports System.ComponentModel
Imports System.Windows.Threading
Imports System.Windows.Controls
Imports System.Drawing
Imports System.Collections.Generic
Imports System.Net
Imports A2Utilidades

Partial Public Class ImpresionCheque
    Inherits Window
    Private _VisorReporte As ReportViewer
    Public Event RenderingComplete As RenderingCompleteEventHandler
    Public argumentos As List(Of String) = New List(Of String)
    Private logerror As Boolean
    Dim timer As DispatcherTimer = New DispatcherTimer()

    Public Property VisorReporte() As ReportViewer
        Get
            Return Me._VisorReporte
        End Get
        Set(value As ReportViewer)
            Try
                If Me._VisorReporte IsNot Nothing Then
                    RemoveHandler Me._VisorReporte.RenderingComplete, AddressOf Me.VisorReporte_RenderingComplete
                    RemoveHandler Me._VisorReporte.RenderingBegin, AddressOf Me.VisorReporte_RenderingBegin
                End If
                Me._VisorReporte = value
                If Me._VisorReporte IsNot Nothing Then
                    AddHandler _VisorReporte.RenderingComplete, AddressOf VisorReporte_RenderingComplete
                    AddHandler _VisorReporte.RenderingBegin, AddressOf VisorReporte_RenderingBegin
                    Return
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No fue posible inicializar el control VisorReporte.",
               Me.ToString(), "VisorReporte", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        End Set
    End Property
    Private Sub VisorReporte_RenderingBegin(ByVal sender As System.Object, ByVal e As CancelEventArgs)
    End Sub
    Public Sub New(ByVal pobjArgumentos As List(Of String))
        Try
            InitializeComponent()

            argumentos = pobjArgumentos

            AddHandler timer.Tick, AddressOf Timer_Tick
            timer.Interval = New TimeSpan(1000)
            timer.Start()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No fue posible inicializar el formulario de ImprimirCheques.",
               Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
            Me.logerror = True
        End Try
    End Sub
    Private Sub VisorReporte_RenderingComplete(ByVal sender As System.Object, ByVal e As RenderingCompleteEventArgs)
        Try
            Me.myBusyIndicator.BusyContent = "Termino impresión..."
            Me.Close()
            Dim num As Integer = Integer.Parse(Me.VisorReporte.PrintDialog())
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la impresión",
               Me.ToString(), "VisorReporte_RenderingComplete", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        Try
            If Me.logerror Then
                Me.Close()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No fue posible establecer los parametros o comunicación con el servidor.",
               Me.ToString(), "Window_Loaded", Application.Current.ToString(), Program.Maquina, ex)
            Me.Close()
        End Try
    End Sub

    Public Sub Timer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        Try
            timer.Stop()
            VisorReporte = New ReportViewer()
            'SuspendLayout()
            'VisorReporte.Location = New Point(21, 17)
            VisorReporte.Name = "ReportViewer"
            Dim Tamano1 As Size = New Size(396, 246)
            Dim Tamano2 As Size = Tamano1
            'VisorReporte.Size = Tamano1
            VisorReporte.TabIndex = 0
            'AutoScaleDimensions = New SizeF(6.0F, 13.0F)
            'AutoScaleMode = AutoScaleMode.Font
            Tamano1 = New Size(252, 101)
            'ClientSize = Tamano1
            Opacity = 0.0
            'Text = "Imprimir"

            Dim strArray2() As String = argumentos(2).Split(",")
            Dim strArray3() As String = argumentos(3).Split(",")
            Dim strArray4() As String = argumentos(4).Split(",")
            Dim strArray5() As String = argumentos(5).Split(",")
            Dim reportParameterArray() As ReportParameter = {
                    New ReportParameter(strArray2(0), strArray2(1)),
                    New ReportParameter(strArray3(0), strArray3(1)),
                    New ReportParameter(strArray4(0), strArray4(1)),
                    New ReportParameter(strArray5(0), strArray5(1))
                }
            'PrinterSettings().PrinterName = "Microsoft XPS Document Writer"
            Dim Str As String = argumentos(1).Replace("+", " ")
            If Strings.Left(Str, 1) <> "/" Then
                Str = "/" & Str
            End If

            If argumentos.Count() > 6 Then
                Dim strArrayCredenciales() As String = argumentos(6).Split(":")
                Dim myCred As NetworkCredential = New NetworkCredential(strArrayCredenciales(0).Split(",")(1), strArrayCredenciales(1).Split(",")(1), strArrayCredenciales(2).Split(",")(1))
                VisorReporte.ServerReport.ReportServerCredentials.NetworkCredentials = myCred
                VisorReporte.Refresh()
            End If

            VisorReporte.ServerReport.ReportServerUrl = New Uri(argumentos(0))
            VisorReporte.ServerReport.ReportPath = Str
            VisorReporte.ServerReport.SetParameters(CType(reportParameterArray, IEnumerable(Of ReportParameter)))
            VisorReporte.ProcessingMode = ProcessingMode.Remote
            VisorReporte.ServerReport.Refresh()
            VisorReporte.RefreshReport()

            Me.myBusyIndicator.BusyContent = "Imprimiendo cheques..."
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No fue posible establecer los parametros o comunicación con el servidor.",
               Me.ToString(), "Timer_Tick", Application.Current.ToString(), Program.Maquina, ex)
            Me.Close()
        End Try
    End Sub
End Class
