Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class CalculadoraFinancieraBasicaView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As CalculadoraBasicaViewModel
    ' Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para la calculadora financiera básica (aplica los estilos a la pantalla)
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New CalculadoraBasicaViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                inicializar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Realiza un llamado sincrono al proceso inicializar del ViewModel para obtener el resultado de la calculadora basica
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Async Sub inicializar()
        Try
            If Not Me.DataContext Is Nothing Then
                mobjVM = CType(Me.DataContext, CalculadoraBasicaViewModel)
                mobjVM.NombreView = Me.ToString

                mobjVM.inicializar()
                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización", Me.Name, "inicializar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

#End Region

#Region "Eventos de controles"

    ''' <summary>
    ''' Selecciona todo el texto que se encuentre en la caja de texto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "Eventos para asignar especie"

    ''' <summary>
    ''' Obtiene la propiedad "strIDEspecie"
    ''' </summary>
    ''' <param name="pstrNemotecnico"></param>
    ''' <param name="pstrNombreEspecie"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Sub BuscadorEspecie_nemotecnicoAsignado_1(pstrNemotecnico As String, pstrNombreEspecie As String)
        Try
            If Not String.IsNullOrEmpty(pstrNemotecnico) Then
                Me.mobjVM.LimpiarObjeto()
                Me.mobjVM.TraerNemotecnico(pstrNemotecnico)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene todas las propiedades del nemotectico (especie seleccionada por el usuario)
    ''' </summary>
    ''' <param name="pstrClaseControl"></param>
    ''' <param name="pobjEspecie"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Me.mobjVM.LimpiarObjeto()
                Me.mobjVM.TraerCaracteristicasNemotecnico(pobjEspecie)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Evento que dispara la lupa para limpiar los datos de la especie (strIDEspecie, strISIN)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Sub btnProcesar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Calcular()
    End Sub

Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.LimpiarBusqueda()
    End Sub

Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarEspecie = True
                Me.mobjVM.CB.strIDEspecie = Nothing
                Me.mobjVM.BorrarEspecie = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
