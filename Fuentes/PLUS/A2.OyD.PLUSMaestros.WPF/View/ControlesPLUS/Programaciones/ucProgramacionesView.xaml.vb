Imports Telerik.Windows.Controls
Imports System.Collections.ObjectModel
Imports A2Utilidades

Partial Public Class ucProgramacionesView
    Inherits Window

    Private mobjVM As New ProgramacionViewModel

    Public Sub New()
        Try
            If Application.Current.Resources.Contains("ProgViewModel") Then
                Application.Current.Resources.Remove("ProgViewModel")
            End If
            Application.Current.Resources.Add("ProgViewModel", mobjVM)

            InitializeComponent()
            Me.DataContext = mobjVM
            mobjVM.viewProgramaciones = Me
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al instanciar el formulario de programaciones.", _
                                                         Me.ToString(), "ucProgramacionesView", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Shared ReadOnly ModuloDep As DependencyProperty = DependencyProperty.Register("Modulo", GetType(String), GetType(ucProgramacionesView), New PropertyMetadata("", New PropertyChangedCallback(AddressOf ModuloChanged)))
    Public Property Modulo As String
        Get
            Return CStr(GetValue(ModuloDep))
        End Get
        Set(ByVal value As String)
            SetValue(ModuloDep, value)
        End Set
    End Property

    Private Shared Sub ModuloChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ucProgramacionesView = DirectCast(d, ucProgramacionesView)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.Modulo = obj.Modulo
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlProgramaciones", "ModuloChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Shared ReadOnly NroDocumentoDep As DependencyProperty = DependencyProperty.Register("NroDocumento", GetType(Integer), GetType(ucProgramacionesView), New PropertyMetadata(0, New PropertyChangedCallback(AddressOf NroDocumentoChanged)))
    Public Property NroDocumento As Integer
        Get
            Return CInt(GetValue(NroDocumentoDep))
        End Get
        Set(ByVal value As Integer)
            SetValue(NroDocumentoDep, value)
        End Set
    End Property

    Private Shared Sub NroDocumentoChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Try
            Dim obj As ucProgramacionesView = DirectCast(d, ucProgramacionesView)
            If Not IsNothing(obj.mobjVM) Then
                obj.mobjVM.NroDocumento = obj.NroDocumento
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al realizar el cambio de propiedad.", "ControlProgramaciones", "NroDocumentoChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles btnCancelar.Click
        Me.DialogResult = True
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Private Sub btnAceptar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            mobjVM.RealizarLlamadosSincronicos(ProgramacionViewModel.TipoConsulta.ActualizarProgramacion)
        End If
    End Sub

    Private Sub btnInactivar_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            If Not IsNothing(mobjVM.ProgramacionSeleccionado) Then
                If mobjVM.ProgramacionSeleccionado.ID <> 0 Then
                    mobjVM.RealizarLlamadosSincronicos(ProgramacionViewModel.TipoConsulta.InactivarProgramacion)
                Else
                    Mensajes.mostrarMensaje("La programación aun no ha sido guardada por lo tanto no tiene datos a inactivar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        End If
    End Sub

    Private Sub btnSimularFechas_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            mobjVM.RealizarLlamadosSincronicos(ProgramacionViewModel.TipoConsulta.GenerarFechas)
        End If
    End Sub

    Private Sub btnFechasGeneradas_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(mobjVM) Then
            If Not IsNothing(mobjVM.ProgramacionSeleccionado) Then
                If mobjVM.ProgramacionSeleccionado.ID <> 0 Then
                    mobjVM.RealizarLlamadosSincronicos(ProgramacionViewModel.TipoConsulta.Fechas)
                Else
                    Mensajes.mostrarMensaje("La programación aun no ha sido guardada por lo tanto no tiene datos en las fechas.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        End If
    End Sub
End Class







