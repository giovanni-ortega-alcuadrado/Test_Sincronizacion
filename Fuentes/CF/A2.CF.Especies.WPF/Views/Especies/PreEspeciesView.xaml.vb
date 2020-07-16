Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2.OYD.OYDServer.RIA.Web



Partial Public Class PreEspeciesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases PreEspeciesView y PreEspeciesViewModel
    ''' Pantalla ChoquesTasasInteres (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As PreEspeciesViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

#End Region

#Region "Inicializacion"
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

        Me.DataContext = New PreEspeciesViewModel
InitializeComponent()
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
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, PreEspeciesViewModel)
            mobjVM.NombreView = Me.ToString

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            Await mobjVM.inicializar()
        End If
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

        End Try
    End Sub
#End Region

#Region "Manejadores error"

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

#Region "Eventos para asignar emisor y/o clase"
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idemisor"
                    CType(Me.DataContext, PreEspeciesViewModel).EncabezadoSeleccionado.lngIdEmisor = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, PreEspeciesViewModel).EncabezadoSeleccionado.strDescripcionEmisor = pobjItem.Nombre
                Case "idemisorbuscar"
                    CType(Me.DataContext, PreEspeciesViewModel).cb.lngIdEmisor = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, PreEspeciesViewModel).cb.strDescripcionEmisor = pobjItem.Nombre

                Case "idclase"
                    CType(Me.DataContext, PreEspeciesViewModel).EncabezadoSeleccionado.lngIDClase = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, PreEspeciesViewModel).EncabezadoSeleccionado.strDescripcionClase = pobjItem.Nombre
                Case "idclasebuscar"
                    CType(Me.DataContext, PreEspeciesViewModel).cb.lngIDClase = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, PreEspeciesViewModel).cb.strDescripcionClase = pobjItem.Nombre

            End Select
        End If
    End Sub


    Private Sub Buscadorclaseespecie_GotFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)

        CType(gridEd.FindName("Buscadorclaseespecie"), BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, PreEspeciesViewModel).Clase()

    End Sub


    Private Sub txtEmisor_KeyDown(sender As Object, e As KeyEventArgs)
        'Try
        '    If Not IsNothing(mobjVM) Then
        '        If e.Key = Key.Enter Then
        '            CType(gridEd.FindName("ctlBuscadorEmisor"), BuscadorGenericoListaButon).Focus()
        '        End If
        '    End If
        'Catch ex As Exception
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error consultar el emisor", Me.Name, "txtEmisor_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        'End Try
    End Sub

    Private Sub txtClase_KeyDown(sender As Object, e As KeyEventArgs)
        'Try
        '    If Not IsNothing(mobjVM) Then
        '        If e.Key = Key.Enter Then
        '            CType(gridEd.FindName("ctlBuscadorEmisor"), BuscadorGenericoListaButon).Focus()
        '        End If
        '    End If
        'Catch ex As Exception
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error consultar el emisor", Me.Name, "txtEmisor_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        'End Try
    End Sub

    Private Sub txtEmisor_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim dcProxy As UtilidadesDomainContext

            dcProxy = inicializarProxyUtilidadesOYD()

            If Not IsNothing(mobjVM) Then
                If Not String.IsNullOrEmpty(CStr(txtEmisor.Value)) Then
                    mobjVM.TraerDatosBuscador("Emisor", CStr(txtEmisor.Value))

                    'Await dcProxy.Load(dcProxy.buscarItemEspecificoQuery("Emisor", txtEmisor.Text, Program.Usuario, Program.HashConexion)).AsTask
                Else
                    If Not String.IsNullOrEmpty(CStr(mobjVM.EncabezadoSeleccionado.lngIdEmisor)) Then
                        mobjVM.EncabezadoSeleccionado.lngIdEmisor = 0
                        'mobjVM.EncabezadoSeleccionado.IdComprobante = Nothing
                        mobjVM.EncabezadoSeleccionado.strDescripcionEmisor = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error consultar el emisor", Me.Name, "txtEmisor_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtClase_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim dcProxy As UtilidadesDomainContext

            dcProxy = inicializarProxyUtilidadesOYD()

            If Not IsNothing(mobjVM) Then
                If Not String.IsNullOrEmpty(CStr(txtClase.Value)) Then
                    mobjVM.TraerDatosBuscador("Clase", CStr(txtClase.Value))
                    'Await dcProxy.Load(dcProxy.buscarItemEspecificoQuery("Emisor", txtEmisor.Text, Program.Usuario, Program.HashConexion)).AsTask
                Else
                    If Not String.IsNullOrEmpty(CStr(mobjVM.EncabezadoSeleccionado.lngIDClase)) Then
                        mobjVM.EncabezadoSeleccionado.lngIDClase = 0
                        'mobjVM.EncabezadoSeleccionado.IdComprobante = Nothing
                        mobjVM.EncabezadoSeleccionado.strDescripcionClase = String.Empty
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error consultar la clase", Me.Name, "txtClase_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region


End Class
