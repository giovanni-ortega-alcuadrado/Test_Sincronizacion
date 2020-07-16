Imports Telerik.Windows.Controls

Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class EspeciesExcluidasView
    Inherits UserControl

    ''' <summary>
    ''' ID caso de prueba:  CP001, CP002. ventana con estilos metro y modo lista 
    ''' Eventos creados para la comunicación con las clases EspeciesExcluidasView y EspeciesExcluidasViewModel
    ''' Pantalla Compañía (Cálculos Financieros)
    ''' </summary>
    ''' <remarks>Javier Pardo (Alcuadrado S.A.) - 30 de Julio 2015</remarks>

#Region "Variables"

    Private mobjVM As EspeciesExcluidasViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"

    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación.", Me.Name, "NEW", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))

            Me.DataContext = New EspeciesExcluidasViewModel
InitializeComponent()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "ATENCIÓN: No fue posible inicializar el control.", Me.Name, "NEW", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        Try
            If Not Me.DataContext Is Nothing Then
                mobjVM = CType(Me.DataContext, EspeciesExcluidasViewModel)
                mobjVM.NombreView = Me.ToString

                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

                Await mobjVM.inicializar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un iniciarlizar", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enfocar control", Me.ToString(), "seleccionarFocoControl", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                mobjVM.cb.strIDEspecie = pobjItem.IdItem 'Nemotecnico
                mobjVM.cb.strEspecie = pobjItem.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la búsqueda", Me.ToString(), "BuscadorGenericoListaButon_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ControlRefrescarCache_EventoRefrescarCombos(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM = CType(Me.DataContext, EspeciesExcluidasViewModel)
            mobjVM.NombreView = Me.ToString
            Await mobjVM.RefresacarCache()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar combos", Me.ToString(), "ControlRefrescarCache_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos detalles"

    ''' <summary>
    ''' Método para asignar la especie luego de seleccionarla a través del buscador
    ''' </summary>
    ''' <param name="pstrClaseControl"></param>
    ''' <param name="pobjItem"></param>
    ''' <remarks></remarks>
    Private Sub BuscadorGenericoListaButon__detalles_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Me.mobjVM.HabilitarEdicionDetalle Then
                    mobjVM.DetalleSeleccionado.strIDEspecie = pobjItem.IdItem 'Nemotecnico
                    'CType(Me.DataContext, EspeciesExcluidasViewModel).DetalleSeleccionado.strIDEspecie = pobjItem.IdItem
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la selección dle buscador", Me.ToString(), "BuscadorGenericoListaButon__detalles_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Manejadores error"

    Private Sub btnNuevo_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalle()
    End Sub

Private Sub btnBorrar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalle()
    End Sub

Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region
   
End Class
