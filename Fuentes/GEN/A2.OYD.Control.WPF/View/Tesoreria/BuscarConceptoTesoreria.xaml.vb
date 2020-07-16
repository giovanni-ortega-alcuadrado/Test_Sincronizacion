Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Text.RegularExpressions

Partial Public Class BuscarConceptoTesoreria
    Inherits Window
    Implements INotifyPropertyChanged

    Dim strDetalleConceptoIngresado As String
    Dim strTipoItemConsultaConceptos As String
    Dim strTipoTesoreria As String
    Dim logClienteRequerido As Boolean
    Dim logPermitirGuardarSinConcepto As Boolean = False
    Dim logConceptoNotasSoloCuentas As Boolean = False
    Dim objProxy As UtilidadesDomainContext

    Public Sub New(ByVal pobjTipoTesoreria As String,
                   ByVal pstrConsecutivo As String,
                   ByVal pintIDCompaniaConsecutivo As Integer,
                   ByVal pintIDConcepto As Nullable(Of Integer),
                   ByVal pstrDetalleConcepto As String,
                   Optional ByVal plogMostrarSeleccionCliente As Boolean = False,
                   Optional ByVal plogClienteRequerido As Boolean = False,
                   Optional ByVal plogDescripcionCliente As Boolean = False,
                   Optional ByVal plogMostrarCamposNotas As Boolean = True,
                   Optional ByVal plogPermitirGuardarSinConcepto As Boolean = False,
                   Optional ByVal plogConceptoNotasSoloCuentas As Boolean = False)
        Try
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me
            ctlBuscadorConcepto.Agrupamiento = pstrConsecutivo
            IDCompaniaConsecutivo = pintIDCompaniaConsecutivo

            logPermitirGuardarSinConcepto = plogPermitirGuardarSinConcepto
            logConceptoNotasSoloCuentas = plogConceptoNotasSoloCuentas

            txtConcepto.IsEnabled = True

            If plogMostrarSeleccionCliente Then
                MostrarSeleccionCliente = Visibility.Visible
            Else
                MostrarSeleccionCliente = Visibility.Collapsed
            End If

            logClienteRequerido = plogClienteRequerido

            strTipoTesoreria = pobjTipoTesoreria

            If pobjTipoTesoreria = "N" Then
                strTipoItemConsultaConceptos = "ConceptoTesoNotas"
            ElseIf pobjTipoTesoreria = "CE" Then
                strTipoItemConsultaConceptos = "ConceptoTesoEgresos"
            ElseIf pobjTipoTesoreria = "RC" Then
                strTipoItemConsultaConceptos = "ConceptoTesoCaja"
            End If

            ctlBuscadorConcepto.TipoItem = strTipoItemConsultaConceptos

            If plogDescripcionCliente = False Then
                MostrarDescripcionCliente = Visibility.Collapsed
            End If

            'SOLO PARA NOTAS
            If pobjTipoTesoreria = "N" And plogMostrarCamposNotas Then
                MostrarCamposNotas = Visibility.Visible
                MostrarCuentaContable = Visibility.Collapsed
            Else
                MostrarCamposNotas = Visibility.Collapsed
                MostrarCuentaContable = Visibility.Visible
            End If

            strDetalleConceptoIngresado = pstrDetalleConcepto
            'LLAMAR BUSCAR ESPECIFICO CUANDO CONCEPTO SEA MAYOR A CERO
            If pintIDConcepto > 0 Then
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))

                'DirectCast(objProxy.DomainClient, WebDomainClient(Of A2.OYD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)

                objProxy.BuscadorGenericos.Clear()
                objProxy.Load(objProxy.buscarItemEspecificoQuery(strTipoItemConsultaConceptos, pintIDConcepto.ToString, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, String.Empty)
            Else
                If Not String.IsNullOrEmpty(strDetalleConceptoIngresado) Then
                    If strDetalleConceptoIngresado.Contains("(") And strDetalleConceptoIngresado.Contains(")") Then
                        Dim intInicio As Integer = strDetalleConceptoIngresado.IndexOf("(")

                        txtConcepto.Text = strDetalleConceptoIngresado.Substring(intInicio + 1, strDetalleConceptoIngresado.IndexOf(")", intInicio + 1) - intInicio - 1)
                    Else
                        txtConcepto.Text = strDetalleConceptoIngresado
                    End If

                    LlevarValoresItemACampos(Nothing)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el concepto.", _
                                                 Me.ToString(), "BuscarConceptoTesoreria", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"

    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    LlevarValoresItemACampos(lo.Entities.First)

                    If Not String.IsNullOrEmpty(strDetalleConceptoIngresado) Then
                        If strDetalleConceptoIngresado.Contains("(") And strDetalleConceptoIngresado.Contains(")") Then
                            Dim intInicio As Integer = strDetalleConceptoIngresado.IndexOf("(")

                            txtConcepto.Text = strDetalleConceptoIngresado.Substring(intInicio + 1, strDetalleConceptoIngresado.IndexOf(")", intInicio + 1) - intInicio - 1)
                        Else
                            txtConcepto.Text = String.Empty
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos", _
                                             Me.ToString(), "TerminoTraerBuscadorGenerico", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos", Me.ToString(), _
                                                             "TerminoTraerBuscadorGenerico", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub
    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try

            If logPermitirGuardarSinConcepto = False Then
                If IsNothing(IDConcepto) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el concepto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If logConceptoNotasSoloCuentas Then
                    If IDTipoMovimiento = "BB" Or IDTipoMovimiento = "BBA" Or IDTipoMovimiento = "BP" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El tipo de movimiento del concepto seleccionado debe mover dos cuentas contables.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            Else 'para que permita ingresar en el detalle cuando no hay concepto
                If IsNothing(IDConcepto) And String.IsNullOrEmpty(txtConcepto.Text) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el concepto o el detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If MostrarCamposNotas = Visibility.Visible Then
                If strTipoTesoreria = "N" Then
                    If String.IsNullOrEmpty(IDTipoMovimiento) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Como el concepto es de notas se debe tener matriculado el campo tipo movimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            End If

            If logClienteRequerido Then
                If String.IsNullOrEmpty(ClienteSeleccionado) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            Me.DialogResult = CType(True, MessageBoxResult)
            Me.Close()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el concepto.", _
                                                 Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.DialogResult = CType(False, MessageBoxResult)
            Me.Close()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la selección del concepto.", _
                                                 Me.ToString(), "btnCancelar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub ctlBuscadorConcepto_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If logConceptoNotasSoloCuentas Then
                    If pobjItem.InfoAdicional07 = "BB" Or pobjItem.InfoAdicional07 = "BBA" Or pobjItem.InfoAdicional07 = "BP" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El tipo de movimiento del concepto seleccionado debe mover dos cuentas contables.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                LlevarValoresItemACampos(pobjItem)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el concepto.",
                                                Me.ToString(), "ctlBuscadorConcepto_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub Buscar_finalizoBusquedaCliente(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                ClienteSeleccionado = pobjComitente.IdComitente
                NombreCliente = pobjComitente.Nombre
                NroDocumentoCliente = pobjComitente.NroDocumento
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el cliente.", _
                                                Me.ToString(), "Buscar_finalizoBusquedaCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub LlevarValoresItemACampos(ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                IDConcepto = CType(pobjItem.IdItem, Integer?)
                IDCuentaContable = pobjItem.InfoAdicional03
                IDCuentaContableAux = pobjItem.InfoAdicional04
                IDManejaCliente = pobjItem.InfoAdicional05
                DescripcionManejaCliente = pobjItem.InfoAdicional06
                IDTipoMovimiento = pobjItem.InfoAdicional07
                DescripcionTipoMovimiento = pobjItem.InfoAdicional08
                IDRetencion = pobjItem.InfoAdicional09
                DescripcionRetencion = pobjItem.InfoAdicional10
                DetalleConcepto = pobjItem.Nombre
                If pobjItem.InfoAdicional11 = "1" Then
                    ManejaDiferido = True
                Else
                    ManejaDiferido = False
                End If
                IDTipoMovimientoDiferido = pobjItem.InfoAdicional12
                DescripcionTipoMovimientoDiferido = pobjItem.InfoAdicional13
                IDCuentaContableCRDiferido = pobjItem.InfoAdicional14
                IDCuentaContableDBDiferido = pobjItem.InfoAdicional15
                CantidadMaximaParaTextoDigitado(pobjItem.Nombre)
                ConcatenarConceptoYDetalle(DetalleConcepto, txtConcepto.Text)
            Else
                IDConcepto = Nothing
                IDCuentaContable = String.Empty
                IDCuentaContableAux = String.Empty
                IDManejaCliente = String.Empty
                DescripcionManejaCliente = String.Empty
                IDTipoMovimiento = String.Empty
                DescripcionTipoMovimiento = String.Empty
                IDRetencion = String.Empty
                DescripcionRetencion = String.Empty
                DetalleConcepto = String.Empty
                ManejaDiferido = False
                IDTipoMovimientoDiferido = String.Empty
                DescripcionTipoMovimientoDiferido = String.Empty
                IDCuentaContableCRDiferido = String.Empty
                IDCuentaContableDBDiferido = String.Empty
                CantidadMaximaParaTextoDigitado(String.Empty)
                ConcatenarConceptoYDetalle(String.Empty, txtConcepto.Text)
            End If
            txtConcepto.IsEnabled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llevar los valores a los campos.",
                                                Me.ToString(), "LlevarValoresItemACampos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtConcepto_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            ConcatenarConceptoYDetalle(DetalleConcepto, txtConcepto.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al digitar el concepto.", _
                                                Me.ToString(), "txtConcepto_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CantidadMaximaParaTextoDigitado(ByVal pstrDetalleConcepto As String)
        Try
            If Not String.IsNullOrEmpty(pstrDetalleConcepto) Then
                If Len(pstrDetalleConcepto) >= 77 Then
                    txtConcepto.MaxLength = 0
                Else
                    txtConcepto.MaxLength = 77 - Len(pstrDetalleConcepto)
                End If
            Else
                txtConcepto.MaxLength = 77
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al digitar el concepto.", _
                                               Me.ToString(), "CantidadMaximaParaTextoDigitado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConcatenarConceptoYDetalle(ByVal pstrDetalleConcepto As String, ByVal pstrTextoDigitado As String)
        Try
            If Not String.IsNullOrEmpty(pstrDetalleConcepto) Then
                Dim strConceptoConcatenado As String
                Dim intCantidadPermitida As Integer
                If Len(pstrDetalleConcepto) >= 77 Then
                    DetalleConceptoCompleto = pstrDetalleConcepto
                Else
                    If Not String.IsNullOrEmpty(pstrTextoDigitado) Then
                        strConceptoConcatenado = pstrDetalleConcepto & "("
                        intCantidadPermitida = 77 - Len(pstrDetalleConcepto)

                        If Len(pstrTextoDigitado) <= intCantidadPermitida Then
                            strConceptoConcatenado += pstrTextoDigitado
                        Else
                            strConceptoConcatenado += pstrTextoDigitado.Substring(0, intCantidadPermitida)
                        End If

                        strConceptoConcatenado += ")"

                        DetalleConceptoCompleto = strConceptoConcatenado
                    Else
                        DetalleConceptoCompleto = pstrDetalleConcepto
                    End If
                End If
            Else
                DetalleConceptoCompleto = pstrTextoDigitado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concetenar el concepto.", _
                                                Me.ToString(), "ConcatenarConceptoYDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        LlevarValoresItemACampos(Nothing)
    End Sub

    Private Sub btnLimpiarCiente_Click(sender As Object, e As RoutedEventArgs)
        ClienteSeleccionado = String.Empty
        NombreCliente = String.Empty
        NroDocumentoCliente = String.Empty
    End Sub

#End Region

#Region "Propiedades"

    Private _IDConcepto As Nullable(Of Integer)
    Public Property IDConcepto() As Nullable(Of Integer)
        Get
            Return _IDConcepto
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDConcepto"))
        End Set
    End Property
    Private _IDCuentaContable As String
    Public Property IDCuentaContable() As String
        Get
            Return _IDCuentaContable
        End Get
        Set(ByVal value As String)
            _IDCuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCuentaContable"))
        End Set
    End Property
    Private _IDCuentaContableAux As String
    Public Property IDCuentaContableAux() As String
        Get
            Return _IDCuentaContableAux
        End Get
        Set(ByVal value As String)
            _IDCuentaContableAux = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCuentaContableAux"))
        End Set
    End Property
    Private _MostrarCuentaContable As Visibility = Visibility.Collapsed
    Public Property MostrarCuentaContable() As Visibility
        Get
            Return _MostrarCuentaContable
        End Get
        Set(ByVal value As Visibility)
            _MostrarCuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarCuentaContable"))
        End Set
    End Property

    Private _IDManejaCliente As String
    Public Property IDManejaCliente() As String
        Get
            Return _IDManejaCliente
        End Get
        Set(ByVal value As String)
            _IDManejaCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDManejaCliente"))
        End Set
    End Property
    Private _DescripcionManejaCliente As String
    Public Property DescripcionManejaCliente() As String
        Get
            Return _DescripcionManejaCliente
        End Get
        Set(ByVal value As String)
            _DescripcionManejaCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionManejaCliente"))
        End Set
    End Property
    Private _IDTipoMovimiento As String
    Public Property IDTipoMovimiento() As String
        Get
            Return _IDTipoMovimiento
        End Get
        Set(ByVal value As String)
            _IDTipoMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDTipoMovimiento"))
        End Set
    End Property
    Private _DescripcionTipoMovimiento As String
    Public Property DescripcionTipoMovimiento() As String
        Get
            Return _DescripcionTipoMovimiento
        End Get
        Set(ByVal value As String)
            _DescripcionTipoMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionTipoMovimiento"))
        End Set
    End Property
    Private _IDRetencion As String
    Public Property IDRetencion() As String
        Get
            Return _IDRetencion
        End Get
        Set(ByVal value As String)
            _IDRetencion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDRetencion"))
        End Set
    End Property
    Private _DescripcionRetencion As String
    Public Property DescripcionRetencion() As String
        Get
            Return _DescripcionRetencion
        End Get
        Set(ByVal value As String)
            _DescripcionRetencion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionRetencion"))
        End Set
    End Property
    Private _DetalleConcepto As String
    Public Property DetalleConcepto() As String
        Get
            Return _DetalleConcepto
        End Get
        Set(ByVal value As String)
            _DetalleConcepto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DetalleConcepto"))
        End Set
    End Property
    Private _DetalleConceptoCompleto As String
    Public Property DetalleConceptoCompleto() As String
        Get
            Return _DetalleConceptoCompleto
        End Get
        Set(ByVal value As String)
            _DetalleConceptoCompleto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DetalleConceptoCompleto"))
        End Set
    End Property
    Private _MostrarCamposNotas As Visibility = Visibility.Collapsed
    Public Property MostrarCamposNotas() As Visibility
        Get
            Return _MostrarCamposNotas
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposNotas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarCamposNotas"))
        End Set
    End Property
    Private _ClienteSeleccionado As String
    Public Property ClienteSeleccionado() As String
        Get
            Return _ClienteSeleccionado
        End Get
        Set(ByVal value As String)
            _ClienteSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClienteSeleccionado"))
        End Set
    End Property
    Private _NombreCliente As String
    Public Property NombreCliente() As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property
    Private _NroDocumentoCliente As String
    Public Property NroDocumentoCliente() As String
        Get
            Return _NroDocumentoCliente
        End Get
        Set(ByVal value As String)
            _NroDocumentoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroDocumentoCliente"))
        End Set
    End Property
    Private _MostrarSeleccionCliente As Visibility = Visibility.Collapsed
    Public Property MostrarSeleccionCliente() As Visibility
        Get
            Return _MostrarSeleccionCliente
        End Get
        Set(ByVal value As Visibility)
            _MostrarSeleccionCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarSeleccionCliente"))
        End Set
    End Property
    Private _MostrarDescripcionCliente As Visibility = Visibility.Visible
    Public Property MostrarDescripcionCliente() As Visibility
        Get
            Return _MostrarDescripcionCliente
        End Get
        Set(ByVal value As Visibility)
            _MostrarDescripcionCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MostrarDescripcionCliente"))
        End Set
    End Property
    Private _IDCompaniaConsecutivo As Integer
    Public Property IDCompaniaConsecutivo() As Integer
        Get
            Return _IDCompaniaConsecutivo
        End Get
        Set(ByVal value As Integer)
            _IDCompaniaConsecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCompaniaConsecutivo"))
        End Set
    End Property
    Private _ManejaDiferido As Boolean
    Public Property ManejaDiferido() As Boolean
        Get
            Return _ManejaDiferido
        End Get
        Set(ByVal value As Boolean)
            _ManejaDiferido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ManejaDiferido"))
        End Set
    End Property
    Private _IDTipoMovimientoDiferido As String
    Public Property IDTipoMovimientoDiferido() As String
        Get
            Return _IDTipoMovimientoDiferido
        End Get
        Set(ByVal value As String)
            _IDTipoMovimientoDiferido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDTipoMovimientoDiferido"))
        End Set
    End Property
    Private _DescripcionTipoMovimientoDiferido As String
    Public Property DescripcionTipoMovimientoDiferido() As String
        Get
            Return _DescripcionTipoMovimientoDiferido
        End Get
        Set(ByVal value As String)
            _DescripcionTipoMovimientoDiferido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionTipoMovimientoDiferido"))
        End Set
    End Property
    Private _IDCuentaContableCRDiferido As String
    Public Property IDCuentaContableCRDiferido() As String
        Get
            Return _IDCuentaContableCRDiferido
        End Get
        Set(ByVal value As String)
            _IDCuentaContableCRDiferido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCuentaContableCRDiferido"))
        End Set
    End Property
    Private _IDCuentaContableDBDiferido As String
    Public Property IDCuentaContableDBDiferido() As String
        Get
            Return _IDCuentaContableDBDiferido
        End Get
        Set(ByVal value As String)
            _IDCuentaContableDBDiferido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCuentaContableDBDiferido"))
        End Set
    End Property
#End Region


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


End Class
