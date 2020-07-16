Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwConfiguracionArbitrajeView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As ConfiguracionArbitrajeViewModel
    Private mobjDetallePorDefecto As ConfiguracionArbitrajeDetalle

#End Region

#Region "Propiedades"

    Private _cmbTipo_SelectedIndex As Integer
    Public Property cmbTipo_SelectedIndex() As Integer
        Get
            Return _cmbTipo_SelectedIndex
        End Get
        Set(ByVal value As Integer)
            _cmbTipo_SelectedIndex = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("cmbTipo_SelectedIndex"))
        End Set
    End Property

    Private _strDescripcionTipo As String
    Public Property strDescripcionTipo() As String
        Get
            Return _strDescripcionTipo
        End Get
        Set(ByVal value As String)
            _strDescripcionTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionTipo"))
        End Set
    End Property

    Private Async Sub cwConfiguracionArbitrajeView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Try
            IsBusy = True

            Await inicializar()

            If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
                If mobjVM.EncabezadoSeleccionado.strTipo = "A" Then
                    cmbTipo_SelectedIndex = 1
                End If
            End If

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionTipo"))

            IsBusy = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub _strDescripcionTipo_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged

        Try
            If mobjVM.Editando Then
                If e.PropertyName = "strDescripcionTipo" Then
                    If Not IsNothing(strDescripcionTipo) Then
                        If strDescripcionTipo.ToUpper = "ESPECIE" Then
                            HabilitarNemo = True
                            HabilitarMoneda = False
                        Else
                            HabilitarNemo = False
                            HabilitarMoneda = True
                        End If
                        HabilitarValor = True
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los campos del detalle.", Me.ToString(), "_strDescripcionTipo_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private _strIDEspecie As String
    Public Property strIDEspecie() As String
        Get
            Return _strIDEspecie
        End Get
        Set(ByVal value As String)
            _strIDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
        End Set
    End Property

    Private _strISIN As String
    Public Property strISIN() As String
        Get
            Return _strISIN
        End Get
        Set(ByVal value As String)
            _strISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strISIN"))
        End Set
    End Property


    Private _dblValor As Double
    Public Property dblValor() As Double
        Get
            Return _dblValor
        End Get
        Set(ByVal value As Double)
            _dblValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValor"))
        End Set
    End Property

    Private _lngID As Integer
    Public Property lngID() As Integer
        Get
            Return _lngID
        End Get
        Set(ByVal value As Integer)
            _lngID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngID"))
        End Set
    End Property

    Private _strDescripcionMoneda As String
    Public Property strDescripcionMoneda() As String
        Get
            Return _strDescripcionMoneda
        End Get
        Set(ByVal value As String)
            _strDescripcionMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionMoneda"))
        End Set
    End Property

    Private _HabilitarTipo As Boolean = False
    Public Property HabilitarTipo() As Boolean
        Get
            Return _HabilitarTipo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarTipo"))
        End Set
    End Property

    Private _HabilitarNemo As Boolean = False
    Public Property HabilitarNemo() As Boolean
        Get
            Return _HabilitarNemo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNemo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarNemo"))
        End Set
    End Property

    Private _HabilitarValor As Boolean = False
    Public Property HabilitarValor() As Boolean
        Get
            Return _HabilitarValor
        End Get
        Set(ByVal value As Boolean)
            _HabilitarValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarValor"))
        End Set
    End Property

    Private _HabilitarMoneda As Boolean = False
    Public Property HabilitarMoneda() As Boolean
        Get
            Return _HabilitarMoneda
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarMoneda"))
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
    Private WithEvents _DetalleSeleccionado As ConfiguracionArbitrajeDetalle
    Public Property DetalleSeleccionado() As ConfiguracionArbitrajeDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ConfiguracionArbitrajeDetalle)
            _DetalleSeleccionado = value
        End Set
    End Property



#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As ConfiguracionArbitrajeViewModel, ByVal DetalleSeleccionadoVM As ConfiguracionArbitrajeDetalle, ByVal mobjDetallePorDefectoVM As ConfiguracionArbitrajeDetalle, ByVal HabilitarEdicionDetalleVM As Boolean)
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

            mobjVM = pmobjVM

            If Not IsNothing(DetalleSeleccionado) Then
                DetalleSeleccionado = DetalleSeleccionadoVM
                strDescripcionTipo = DetalleSeleccionado.strDescripcionTipo
                strIDEspecie = DetalleSeleccionado.strIDEspecie
                dblValor = CDbl(DetalleSeleccionado.dblValor)
                lngID = CInt(DetalleSeleccionado.lngIdMoneda)
                strDescripcionMoneda = DetalleSeleccionado.strDescripcionMoneda
                If mobjVM.Editando Then
                    HabilitarNemo = HabilitarEdicionDetalleVM
                    HabilitarValor = HabilitarEdicionDetalleVM
                    HabilitarMoneda = HabilitarEdicionDetalleVM
                End If
            Else
                strDescripcionTipo = String.Empty
                strIDEspecie = String.Empty
                dblValor = 0
                lngID = Nothing
                strDescripcionMoneda = String.Empty
            End If

            If mobjVM.Editando Then
                If mobjVM.EncabezadoSeleccionado.strTipo = "A" Then
                    HabilitarTipo = False
                Else
                    HabilitarTipo = True
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub

    Private Async Function inicializar() As Task
        Try
            'IsBusy = False
            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)
            'IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método inicializar.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            If ValidarDetalle() Then

                If IsNothing(DetalleSeleccionado) Then

                    Dim objNvoDetalle As New ConfiguracionArbitrajeDetalle
                    Dim objNuevaLista As New List(Of ConfiguracionArbitrajeDetalle)

                    Program.CopiarObjeto(Of ConfiguracionArbitrajeDetalle)(mobjDetallePorDefecto, objNvoDetalle)

                    objNvoDetalle.intIDConfiguracionArbitrajeDetalle = -New Random().Next(0, 1000000)

                    If IsNothing(mobjVM.ListaDetalle) Then
                        mobjVM.ListaDetalle = New List(Of ConfiguracionArbitrajeDetalle)
                    End If

                    objNuevaLista = mobjVM.ListaDetalle

                    objNvoDetalle.strDescripcionTipo = strDescripcionTipo
                    objNvoDetalle.strIDEspecie = strIDEspecie
                    objNvoDetalle.strISIN = strISIN
                    objNvoDetalle.dblValor = dblValor
                    objNvoDetalle.lngIdMoneda = lngID
                    objNvoDetalle.strDescripcionMoneda = strDescripcionMoneda

                    objNuevaLista.Add(objNvoDetalle)
                    mobjVM.ListaDetalle = objNuevaLista
                    DetalleSeleccionado = mobjVM.ListaDetalle.First

                    mobjVM.DetalleSeleccionado = DetalleSeleccionado

                Else
                    DetalleSeleccionado.strDescripcionTipo = strDescripcionTipo
                    DetalleSeleccionado.strIDEspecie = strIDEspecie
                    DetalleSeleccionado.strISIN = strISIN
                    DetalleSeleccionado.dblValor = dblValor
                    DetalleSeleccionado.lngIdMoneda = lngID
                    DetalleSeleccionado.strDescripcionMoneda = strDescripcionMoneda
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
        
            If String.IsNullOrEmpty(strDescripcionTipo) Then
                strMsg = String.Format("{0}{1} + El tipo es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not IsNothing(strDescripcionTipo) Then

                If strDescripcionTipo.ToUpper = "ESPECIE" Then
                    If String.IsNullOrEmpty(strIDEspecie) Then
                        strMsg = String.Format("{0}{1} + El nemo es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

                If strDescripcionTipo.ToUpper = "DINERO" Then
                    If IsNothing(lngID) Or lngID = 0 Then
                        strMsg = String.Format("{0}{1} + La moneda es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

            End If

            If String.IsNullOrEmpty(strISIN) And strDescripcionTipo = "ESPECIE" Then
                strMsg = String.Format("{0}{1} + El ISIN es un campo requerido.", strMsg, vbCrLf)
            End If

            If dblValor = 0 Then
                strMsg = String.Format("{0}{1} + El valor es un campo requerido.", strMsg, vbCrLf)
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ctrlBuscadorEspecies_finalizoBusqueda(pstrClaseControl As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl.ToLower()
                    Case "nemotecnico"
                        strIDEspecie = pobjEspecie.Nemotecnico
                    Case "nemotecnicobuscar"
                        'Me.mobjVM.cb.NemotecnicoSeleccionado = pobjEspecie
                        'Me.mobjVM.cb.Nemotecnico = pobjEspecie.Nemotecnico
                        Me.mobjVM.CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del nemotécnico", Me.Name, "ctrlEspecie_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlBuscadorISIN_finalizoBusqueda(pstrClaseControl As String, pobjISIN As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjISIN) Then
                Select Case pstrClaseControl.ToLower()
                    Case "isin"
                        strISIN = pobjISIN.IdItem 'pobjEspecie.Nemotecnico
                        If String.IsNullOrEmpty(strIDEspecie) Then
                            strIDEspecie = pobjISIN.InfoAdicional01
                        End If
                    Case "isinbuscar"
                        'Me.mobjVM.cb.NemotecnicoSeleccionado = pobjEspecie
                        'Me.mobjVM.cb.Nemotecnico = pobjEspecie.Nemotecnico
                        Me.mobjVM.CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del nemotécnico", Me.Name, "ctrlEspecie_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEspecie_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                'Me.mobjVM.BorrarEspecie = True
                strIDEspecie = Nothing
                'Me.mobjVM.BorrarEspecie = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarISIN_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                'Me.mobjVM.BorrarEspecie = True
                strISIN = Nothing
                'Me.mobjVM.BorrarEspecie = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub ctrlBuscadorMonedas_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "monedas"
                    If Not IsNothing(mobjVM) Then
                        'Me.mobjVM.BorrarEspecie = True
                        lngID = CInt(pobjItem.IdItem)
                        strDescripcionMoneda = pobjItem.Descripcion
                        'Me.mobjVM.BorrarEspecie = False
                    End If
            End Select
        End If
    End Sub

    Private Sub btnLimpiarMoneda_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                strDescripcionMoneda = Nothing
                lngID = Nothing
                'Me.mobjVM.BorrarEspecie = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
