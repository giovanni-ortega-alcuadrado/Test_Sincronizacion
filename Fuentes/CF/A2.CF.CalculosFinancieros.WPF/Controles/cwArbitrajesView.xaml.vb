Imports Telerik.Windows.Controls

Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwArbitrajesView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As ArbitrajesViewModel
    Private mobjDetallePorDefecto As ArbitrajesDetalle

#End Region

#Region "Propiedades"

    Private Async Sub cwArbitrajesView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            IsBusy = True

            'Await inicializar()

            If Not IsNothing(DetalleSeleccionado) Then
                If Not String.IsNullOrEmpty(DetalleSeleccionado.lngIDComitenteReplica) Then
                    Await Me.mobjVM.ConsultarDatosPortafolio(DetalleSeleccionado.lngIDComitenteReplica, False)
                End If

                If Not String.IsNullOrEmpty(DetalleSeleccionado.lngIDComitenteTraslado) Then
                    Await Me.mobjVM.ConsultarDatosPortafolio(DetalleSeleccionado.lngIDComitenteTraslado, True)
                End If
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el método cwArbitrajesView_Loaded", Me.Name, "cwArbitrajesView_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _strDatosOperacion As String
    Public Property strDatosOperacion() As String
        Get
            Return _strDatosOperacion
        End Get
        Set(ByVal value As String)
            _strDatosOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDatosOperacion"))
        End Set
    End Property

    Private _strIDLiquidacionesOperacionOrigen As String
    Public Property strIDLiquidacionesOperacionOrigen() As String
        Get
            Return _strIDLiquidacionesOperacionOrigen
        End Get
        Set(ByVal value As String)
            _strIDLiquidacionesOperacionOrigen = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDLiquidacionesOperacionOrigen"))
        End Set
    End Property

    Private _strDatosOperacionCruce As String
    Public Property strDatosOperacionCruce() As String
        Get
            Return _strDatosOperacionCruce
        End Get
        Set(ByVal value As String)
            _strDatosOperacionCruce = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDatosOperacionCruce"))
        End Set
    End Property

    Private _strOrigenOperacion As String
    Public Property strOrigenOperacion() As String
        Get
            Return _strOrigenOperacion
        End Get
        Set(ByVal value As String)
            _strOrigenOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strOrigenOperacion"))
        End Set
    End Property

    Private _strOrigenCruce As String
    Public Property strOrigenCruce() As String
        Get
            Return _strOrigenCruce
        End Get
        Set(ByVal value As String)
            _strOrigenCruce = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strOrigenCruce"))
        End Set
    End Property

    Private _intIDLiquidacionesCruce As Integer
    Public Property intIDLiquidacionesCruce() As Integer
        Get
            Return _intIDLiquidacionesCruce
        End Get
        Set(ByVal value As Integer)
            _intIDLiquidacionesCruce = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDLiquidacionesCruce"))
        End Set
    End Property

    Private _lngIDComitenteReplica As String
    Public Property lngIDComitenteReplica() As String
        Get
            Return _lngIDComitenteReplica
        End Get
        Set(ByVal value As String)
            _lngIDComitenteReplica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitenteReplica"))
        End Set
    End Property

    Private _strNombreComitente As String
    Public Property strNombreComitente() As String
        Get
            Return _strNombreComitente
        End Get
        Set(ByVal value As String)
            _strNombreComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreComitente"))
        End Set
    End Property

    Private _lngIDComitenteTraslado As String
    Public Property lngIDComitenteTraslado() As String
        Get
            Return _lngIDComitenteTraslado
        End Get
        Set(ByVal value As String)
            _lngIDComitenteTraslado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitenteTraslado"))
        End Set
    End Property

    Private _strNombreComitenteTraslado As String
    Public Property strNombreComitenteTraslado() As String
        Get
            Return _strNombreComitenteTraslado
        End Get
        Set(ByVal value As String)
            _strNombreComitenteTraslado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreComitenteTraslado"))
        End Set
    End Property

    Private _IsBusy As Boolean = False
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As ArbitrajesDetalle
    Public Property DetalleSeleccionado() As ArbitrajesDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ArbitrajesDetalle)
            _DetalleSeleccionado = value
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarEncabezado"))
        End Set
    End Property

    Private _strDatosEncabezado As String
    Public Property strDatosEncabezado() As String
        Get
            Return _strDatosEncabezado
        End Get
        Set(ByVal value As String)
            _strDatosEncabezado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDatosEncabezado"))
        End Set
    End Property

    Private _strAccion As String
    Public Property strAccion() As String
        Get
            Return _strAccion
        End Get
        Set(ByVal value As String)
            _strAccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strAccion"))
        End Set
    End Property

    Private _dblNominalOperacion As System.Nullable(Of System.Double)
    Public Property dblNominalOperacion() As System.Nullable(Of System.Double)
        Get
            Return _dblNominalOperacion
        End Get
        Set(ByVal value As System.Nullable(Of System.Double))
            _dblNominalOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblNominalOperacion"))
        End Set
    End Property

    Private _lngIDComitenteOperacion As String
    Public Property lngIDComitenteOperacion() As String
        Get
            Return _lngIDComitenteOperacion
        End Get
        Set(ByVal value As String)
            _lngIDComitenteOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitenteOperacion"))
        End Set
    End Property

    Private _intIDLiquidacionesOperacion As Integer
    Public Property intIDLiquidacionesOperacion() As Integer
        Get
            Return _intIDLiquidacionesOperacion
        End Get
        Set(ByVal value As Integer)
            _intIDLiquidacionesOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDLiquidacionesOperacion"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As ArbitrajesViewModel, ByVal DetalleSeleccionadoVM As ArbitrajesDetalle, ByVal mobjDetallePorDefectoVM As ArbitrajesDetalle, ByVal EncabezadoSeleccionadoVM As Arbitrajes, ByVal HabilitarEncabezadoVM As Boolean, ByVal strDatosPortafolioADRVM As String)
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

        Try

            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.DataContext = Me

            DetalleSeleccionado = DetalleSeleccionadoVM
            mobjDetallePorDefecto = mobjDetallePorDefectoVM
            HabilitarEncabezado = HabilitarEncabezadoVM

            mobjVM = pmobjVM

            If Not IsNothing(EncabezadoSeleccionadoVM) Then

                If EncabezadoSeleccionadoVM.logConstruir Then
                    strAccion = "1"
                Else
                    strAccion = "0"
                End If

                strDatosEncabezado = Format(EncabezadoSeleccionadoVM.dtmFechaProceso, "yyyyMMdd") & "!" & strAccion & "!" & EncabezadoSeleccionadoVM.strIDEspecie & strDatosPortafolioADRVM

            End If


            If Not IsNothing(DetalleSeleccionado) Then

                DetalleSeleccionado = DetalleSeleccionadoVM

                strDatosOperacion = DetalleSeleccionado.strDatosOperacion
                strIDLiquidacionesOperacionOrigen = DetalleSeleccionado.strOrigenOperacion + "!" + CStr(DetalleSeleccionado.intIDLiquidacionesOperacion)
                strDatosOperacionCruce = DetalleSeleccionado.strDatosOperacionCruce

                lngIDComitenteReplica = DetalleSeleccionado.lngIDComitenteReplica
                lngIDComitenteTraslado = DetalleSeleccionado.lngIDComitenteTraslado

            End If

        Catch ex As Exception
            IsBusy = False
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

    'Private Function inicializar() As Task
    '    Try
    'IsBusy = False
    'Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)
    'IsBusy = False
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método inicializar.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Function

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            If ValidarDetalle() Then

                If IsNothing(DetalleSeleccionado) Then

                    Dim objNvoDetalle As New ArbitrajesDetalle
                    Dim objNuevaLista As New List(Of ArbitrajesDetalle)

                    Program.CopiarObjeto(Of ArbitrajesDetalle)(mobjDetallePorDefecto, objNvoDetalle)

                    objNvoDetalle.intIDArbitrajesDetalle = -New Random().Next(0, 1000000)

                    If IsNothing(mobjVM.ListaDetalle) Then
                        mobjVM.ListaDetalle = New List(Of ArbitrajesDetalle)
                    End If

                    objNuevaLista = mobjVM.ListaDetalle

                    objNvoDetalle.strDatosOperacion = strDatosOperacion
                    objNvoDetalle.strDatosOperacionCruce = strDatosOperacionCruce
                    objNvoDetalle.intIDLiquidacionesOperacion = intIDLiquidacionesOperacion
                    objNvoDetalle.intIDLiquidacionesCruce = intIDLiquidacionesCruce
                    objNvoDetalle.strOrigenOperacion = strOrigenOperacion
                    objNvoDetalle.strOrigenCruce = strOrigenCruce
                    objNvoDetalle.lngIDComitenteReplica = lngIDComitenteReplica
                    objNvoDetalle.lngIDComitenteTraslado = lngIDComitenteTraslado
                    objNvoDetalle.logSeleccionado = True
                    objNvoDetalle.dblCantidadOperacion = dblNominalOperacion

                    objNuevaLista.Add(objNvoDetalle)
                    mobjVM.ListaDetalle = objNuevaLista
                    DetalleSeleccionado = mobjVM.ListaDetalle.First

                    mobjVM.DetalleSeleccionado = DetalleSeleccionado

                Else
                    DetalleSeleccionado.strDatosOperacion = strDatosOperacion
                    DetalleSeleccionado.strDatosOperacionCruce = strDatosOperacionCruce
                    DetalleSeleccionado.intIDLiquidacionesOperacion = intIDLiquidacionesOperacion
                    DetalleSeleccionado.intIDLiquidacionesCruce = intIDLiquidacionesCruce
                    DetalleSeleccionado.strOrigenOperacion = strOrigenOperacion
                    DetalleSeleccionado.strOrigenCruce = strOrigenCruce
                    DetalleSeleccionado.lngIDComitenteReplica = lngIDComitenteReplica
                    DetalleSeleccionado.lngIDComitenteTraslado = lngIDComitenteTraslado
                    DetalleSeleccionado.logSeleccionado = True
                    DetalleSeleccionado.dblCantidadOperacion = dblNominalOperacion
                End If

                Me.DialogResult = True

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------

            If String.IsNullOrEmpty(strDatosOperacion) Then
                strMsg = String.Format("{0}{1} + La operación es un campo requerido.", strMsg, vbCrLf)
            End If

            If String.IsNullOrEmpty(lngIDComitenteReplica) Then
                strMsg = String.Format("{0}{1} + El portafolio replica es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "ValidarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnCerrar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub seleccionarFocoControl(sender As Object, e As RoutedEventArgs)
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

    Private Sub BuscadorGenerico_finalizo_Dataform_Edicion(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "OperacionesCB"
                        If Not IsNothing(mobjVM) Then
                            strDatosOperacion = pobjItem.Descripcion
                            strIDLiquidacionesOperacionOrigen = pobjItem.InfoAdicional02 + "!" + pobjItem.IdItem
                            dblNominalOperacion = CType(pobjItem.InfoAdicional01, Double?)
                            intIDLiquidacionesOperacion = CInt(pobjItem.IdItem)
                            strOrigenOperacion = pobjItem.InfoAdicional02
                        End If
                    Case "CruceCB"
                        If Not IsNothing(mobjVM) Then
                            strDatosOperacionCruce = pobjItem.Descripcion
                            intIDLiquidacionesCruce = CInt(pobjItem.IdItem)
                            strOrigenCruce = pobjItem.InfoAdicional01

                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al finalizar la búsqueda genérica.", Me.Name, "BuscadorGenerico_finalizo_Dataform_Edicion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                Select Case pstrClaseControl
                    Case "IDComitente"
                        If Not IsNothing(mobjVM) Then
                            lngIDComitenteReplica = pobjComitente.CodigoOYD
                            strNombreComitente = pobjComitente.NombreCodigoOYD
                        End If
                    Case "IDComitenteTraslado"
                        If Not IsNothing(mobjVM) Then
                            lngIDComitenteTraslado = pobjComitente.CodigoOYD
                            strNombreComitenteTraslado = pobjComitente.NombreCodigoOYD
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al finalizar la búsqueda del comitente.", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarDatosOperacion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                strDatosOperacion = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar los datos de la operación.", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarDatosCruce_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                strDatosOperacionCruce = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar los datos del cruce.", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                lngIDComitenteReplica = Nothing
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar los datos del código OyD.", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarClienteTraslado_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                lngIDComitenteTraslado = Nothing
                strNombreComitenteTraslado = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar los datos del traslado.", Me.Name, "btnLimpiarClienteTraslado_Dataform_Edicion_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtCodigoOyD_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not String.IsNullOrEmpty(txtCodigoOyD.Text) Then
                Await Me.mobjVM.ConsultarDatosPortafolio(txtCodigoOyD.Text, False)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al perder el foco del código OyD.", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub txtCodigoOyDTraslado_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not String.IsNullOrEmpty(txtCodigoOyDTraslado.Text) Then
                Await Me.mobjVM.ConsultarDatosPortafolio(txtCodigoOyDTraslado.Text, True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al perder el foco del traslado.", Me.Name, "txtCodigoOyDTraslado_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class