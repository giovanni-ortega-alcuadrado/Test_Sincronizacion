Imports System.Data
Imports Newtonsoft
Imports Newtonsoft.Json

Public Class EjecutarScriptGridPantallaView
    Inherits Window

    Private mobjVM As EjecutarScriptGridPantallaViewModel
    Private logCargoPantalla As Boolean = False

    Public Sub New(ByVal pstrParametros As String)
        Try
            ' This call is required by the designer.
            InitializeComponent()

            Dim objParametrosScript As New clsParametrosEjecutarScript

            If Not String.IsNullOrEmpty(pstrParametros) Then
                objParametrosScript = Json.JsonConvert.DeserializeObject(Of clsParametrosEjecutarScript)(pstrParametros)
            End If

            If Not IsNothing(objParametrosScript) Then
                ' Add any initialization after the InitializeComponent() call.
                mobjVM = New EjecutarScriptGridPantallaViewModel
                mobjVM.intIDScript = objParametrosScript.pintIDScript
                mobjVM.intIDCompania = objParametrosScript.pintIDCompania
                mobjVM.strNombreScript = objParametrosScript.pstrNombreScript
                mobjVM.strDescripcionScript = objParametrosScript.pstrDescripcionScript
                If Not String.IsNullOrEmpty(objParametrosScript.pstrNombreExportacion) Then
                    mobjVM.strNombreExportacion = objParametrosScript.pstrNombreExportacion
                Else
                    mobjVM.strNombreExportacion = objParametrosScript.pstrNombreScript
                End If
                If String.IsNullOrEmpty(objParametrosScript.pstrParametrosFiltro) Then
                    mobjVM.strParametrosFiltro = " "
                Else
                    mobjVM.strParametrosFiltro = objParametrosScript.pstrParametrosFiltro
                End If

                mobjVM.strParametrosVisualizar = objParametrosScript.pstrParametrosVisualizar

                If objParametrosScript.pintIDDisenoDefecto > 0 Then
                    mobjVM.intIDDisenoDefecto = objParametrosScript.pintIDDisenoDefecto
                End If

                Me.DataContext = mobjVM
                mobjVM.View_EjecutarScriptGridPantalla = Me
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se recibieron los parametros de los filtros para la ejecución del Generador de consultas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al abrir al inicializar el control.", Me.ToString(), "Inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            Await mobjVM.inicializar()
            logCargoPantalla = True
            OrganizarAltoControlGrid()
            AddHandler Me.SizeChanged, AddressOf CambioTamanoPantalla
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al abrir al inicializar el control.", Me.ToString(), "Window_Loaded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CambioTamanoPantalla(ByVal sender As Object, ByVal e As EventArgs)
        Try
            OrganizarAltoControlGrid()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al abrir al inicializar el control.", Me.ToString(), "CambioTamanoPantalla", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub OrganizarAltoControlGrid()
        If logCargoPantalla Then
            Dim intAltoSugerido As Double = 500
            If Not IsNothing(mobjVM) Then
                intAltoSugerido = IIf(Me.Height.Equals(Double.NaN), 650, Me.Height) _
                - IIf(mobjVM.ExpanderTitulo, 100, 75) _
                - IIf(mobjVM.ExpanderDiseno, 250, 70)
            End If

            ctlControlFiltro.Height = intAltoSugerido
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM.ListaEncabezado) Then
                Dim intIDSeleccionado As Integer = CInt(CType(sender, Button).Tag)
                If mobjVM.ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDSeleccionado).Count > 0 Then
                    If IsNothing(mobjVM.EncabezadoSeleccionado) Then
                        mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDSeleccionado).First
                    Else
                        If mobjVM.EncabezadoSeleccionado.IDScriptDiseno <> intIDSeleccionado Then
                            mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDSeleccionado).First
                        End If
                    End If

                    mobjVM.EliminarDiseno()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al seleccionar el registro.", Me.ToString(), "btnEliminar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM.ListaEncabezado) Then
                Dim intIDSeleccionado As Integer = CInt(CType(sender, Button).Tag)
                If mobjVM.ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDSeleccionado).Count > 0 Then
                    If IsNothing(mobjVM.EncabezadoSeleccionado) Then
                        mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDSeleccionado).First
                    Else
                        If mobjVM.EncabezadoSeleccionado.IDScriptDiseno <> intIDSeleccionado Then
                            mobjVM.EncabezadoSeleccionado = mobjVM.ListaEncabezado.Where(Function(i) i.IDScriptDiseno = intIDSeleccionado).First
                        End If
                    End If

                    mobjVM.SeleccionarDiseno()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al seleccionar el registro.", Me.ToString(), "btnSeleccionar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ctlControlFiltro_FiltroDiseno_Modificado(pintIDRegistro As Integer, pstrNombreDiseno As String, pstrFiltroDiseno As String)
        Try
            mobjVM.AbrirActualizarDiseno(pintIDRegistro, pstrNombreDiseno, pstrFiltroDiseno)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al seleccionar el registro.", Me.ToString(), "ctlControlFiltro_FiltroDiseno_Modificado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ctlControlFiltro_FiltroDiseno_Borrado()
        Try
            mobjVM.EncabezadoSeleccionado = Nothing
            mobjVM.SeleccionarDiseno()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al seleccionar el registro.", Me.ToString(), "ctlControlFiltro_FiltroDiseno_Modificado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ExpanderControl_Expanded(sender As Object, e As RoutedEventArgs)
        Try
            ExpanderDisenos.IsExpanded = False
            ExpanderControl.IsExpanded = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al seleccionar el registro.", Me.ToString(), "ExpanderControl_Expanded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EjecutarAltoControl()
        Try
            OrganizarAltoControlGrid()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al seleccionar el registro.", Me.ToString(), "EjecutarAltoContro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnRecargarConsultaBD_Click(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.IsBusyControlGrid = False Then
                mobjVM.IsBusyControlGrid = True
                mobjVM.ReiniciaTimer()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al refrescar la consulta de base de datos.", Me.ToString(), "btnRecargarConsultaBD_Click", Application.Current.ToString(), Program.Maquina, ex)
            mobjVM.IsBusyControlGrid = False
        End Try
    End Sub

    Private Async Sub btnRecargarDisenosBD_Click(sender As Object, e As RoutedEventArgs)
        Try
            Await mobjVM.ConsultarListaDisenos()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al refrescar la consulta de los diseños.", Me.ToString(), "btnRecargarDisenosBD_Click", Application.Current.ToString(), Program.Maquina, ex)
            mobjVM.IsBusyControlGrid = False
        End Try
    End Sub

    Private Sub dtgGridUsuarioTodos_ItemsSourceChanged(sender As Object, e As EventArgs)
        Try
            If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
                ReorganizarColoresGridDisenos(mobjVM.ObtenerContadorSeleccionado())
            Else
                ReorganizarColoresGridDisenos()
            End If

            For Each li In dtgGridUsuarioTodos.Columns
                If li.ColumnName = "UsuarioApp" And mobjVM.AdministradorDisenosScript = False Then
                    li.Visible = False
                End If
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al refrescar la consulta de los diseños.", Me.ToString(), "dtgGridUsuarioTodos_ItemsSourceChanged", Application.Current.ToString(), Program.Maquina, ex)
            mobjVM.IsBusyControlGrid = False
        End Try
    End Sub

    Public Sub ReorganizarColoresGridDisenos(Optional ByVal intSeleccionado As Integer = -1)
        Try
            For Each li In dtgGridUsuarioTodos.Rows
                If intSeleccionado <> -1 Then
                    If intSeleccionado = li.Index Then
                        li.Background = Brushes.LightBlue
                    Else
                        li.Background = Brushes.White
                    End If
                Else
                    li.Background = Brushes.White
                End If
            Next
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al refrescar la consulta de los diseños.", Me.ToString(), "ReorganizarColoresGridDisenos", Application.Current.ToString(), Program.Maquina, ex)
            mobjVM.IsBusyControlGrid = False
        End Try
    End Sub


End Class