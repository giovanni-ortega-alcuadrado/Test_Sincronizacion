Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.OyDYankees
Imports System.Windows.Data
Imports System.Globalization

Public Class EnviarAdministracionValoresViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim dcProxy As YankeesDomainContext
    Dim dcProxy1 As YankeesDomainContext

    Private intContadorEnvio As Integer
    Private intContadorRegresos As Integer
    Private intContadorAEnviar As Integer
    Private strErrores As String = String.Empty

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New YankeesDomainContext()
            dcProxy1 = New YankeesDomainContext()
        Else
            dcProxy = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Try
            'IsBusy = True
            FechaIni = Date.Now
            FechaHasta = Date.Now
            Enabled = True
            PuedeEnviar = False
            'dcProxy.Load(dcProxy.LiquidacionesEnviarAdmonQuery(FechaIni, FechaHasta,Program.Usuario, Program.HashConexion), AddressOf terminoCargarLiquidaciones, "")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EnviarAdministracionValoresViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private _ListaLiquidacionesYankees As List(Of LiquidacionesYankees)
    Public Property ListaLiquidacionesYankees() As List(Of LiquidacionesYankees)
        Get
            Return _ListaLiquidacionesYankees
        End Get
        Set(ByVal value As List(Of LiquidacionesYankees))
            _ListaLiquidacionesYankees = value
            MyBase.CambioItem("ListaLiquidacionesYankees")
            MyBase.CambioItem("ListaLiquidacionesYankeesPaged")
        End Set
    End Property
    Public ReadOnly Property ListaLiquidacionesYankeesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidacionesYankees) Then
                Dim view = New PagedCollectionView(_ListaLiquidacionesYankees)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private WithEvents _LiquidacionesYankeesSelected As LiquidacionesYankees
    Public Property LiquidacionesYankeesSelected() As LiquidacionesYankees
        Get
            Return _LiquidacionesYankeesSelected
        End Get
        Set(ByVal value As LiquidacionesYankees)
            _LiquidacionesYankeesSelected = value

            MyBase.CambioItem("LiquidacionesYankeesSelected")
        End Set
    End Property

    Private _fechaIni As Date
    Public Property FechaIni() As Date
        Get
            Return _fechaIni
        End Get
        Set(ByVal value As Date)
            _fechaIni = value
            _enabled = True
            MyBase.CambioItem("FechaIni")
            MyBase.CambioItem("Enabled")
        End Set
    End Property

    Private _fechaHasta As Date
    Public Property FechaHasta() As Date
        Get
            Return _fechaHasta
        End Get
        Set(ByVal value As Date)
            _fechaHasta = value
            _enabled = True
            MyBase.CambioItem("FechaHasta")
            MyBase.CambioItem("Enabled")
        End Set
    End Property

    Private _enabled As Boolean
    Public Property Enabled() As Boolean
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
            MyBase.CambioItem("Enabled")
        End Set
    End Property

    Private _puedeEnviar As Boolean
    Public Property PuedeEnviar() As Boolean
        Get
            Return _puedeEnviar
        End Get
        Set(ByVal value As Boolean)
            _puedeEnviar = value
            MyBase.CambioItem("PuedeEnviar")
        End Set
    End Property

    Private WithEvents _filtrarTitulos As RelayCommand
    Public ReadOnly Property FiltrarTitulos() As RelayCommand
        Get
            If _filtrarTitulos Is Nothing Then
                _filtrarTitulos = New RelayCommand(AddressOf consultarLiquidacionesEnviar)
            End If
            Return _filtrarTitulos
        End Get
    End Property

    Private WithEvents _enviarTitulos As RelayCommand
    Public ReadOnly Property EnviarTitulos() As RelayCommand
        Get
            If _enviarTitulos Is Nothing Then
                _enviarTitulos = New RelayCommand(AddressOf EnviarLiquidacionesEnviar)
            End If
            Return _enviarTitulos
        End Get
    End Property

    Sub SeleccionarTodas(check As Boolean)
        If Not IsNothing(ListaLiquidacionesYankees) Then
            For Each it In ListaLiquidacionesYankees
                it.blnEnviar = check
            Next
        End If

        MyBase.CambioItem("ListaLiquidacionesYankees")
        MyBase.CambioItem("ListaLiquidacionesYankeesPaged")
    End Sub

    Private Sub terminoCargarLiquidaciones(ByVal lo As LoadOperation(Of OyDYankees.Liquidaciones_Yankee_Admon))
        Try
            IsBusy = False

            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    ListaLiquidacionesYankees = Alista(dcProxy.Liquidaciones_Yankee_Admons)
                    LiquidacionesYankeesSelected = ListaLiquidacionesYankees.ToList().FirstOrDefault()
                    PuedeEnviar = True
                Else
                    If Not IsNothing(ListaLiquidacionesYankees) Then
                        ListaLiquidacionesYankees.Clear()
                        LiquidacionesYankeesSelected = Nothing
                    End If
                    PuedeEnviar = False
                    End If
            Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de liquidaciones para enviar a administración.", Me.ToString(), "terminoCargarLiquidaciones", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                    IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de liquidaciones para enviar a administración.", Me.ToString(), "terminoCargarLiquidaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Function Alista(entitySet As EntitySet(Of Liquidaciones_Yankee_Admon)) As List(Of LiquidacionesYankees)
        Dim lst As New List(Of LiquidacionesYankees)
        Try
            For Each it As OyDYankees.Liquidaciones_Yankee_Admon In entitySet
                lst.Add(New LiquidacionesYankees() With {.intIDLiquidaciones_Yankees = it.intIDLiquidaciones_Yankees,
                    .lngID = it.lngID,
                    .lngIDComitente = it.lngIDComitente,
                    .strComitente = it.strComitente,
                    .strTipoIdentificacion = it.strTipoIdentificacion,
                    .lngNroDocumento = it.lngNroDocumento,
                    .strDireccion = it.strDireccion,
                    .strTelefono1 = it.strTelefono1,
                    .dtmRegistro = it.dtmRegistro,
                    .dtmCumplimiento = it.dtmCumplimiento,
                    .strIsin = it.strIsin,
                    .strIDEspecie = it.strIDEspecie,
                    .dtmEmision = it.dtmEmision,
                    .dtmVencimiento = it.dtmVencimiento,
                    .strModalidadTitulo = it.strModalidadTitulo,
                    .lngIDCertcus = it.lngIDCertcus,
                    .lngIDNroCustodia = it.lngIDNroCustodia,
                    .dblValorNominal = it.dblValorNominal,
                    .dblValorTotal = it.dblValorTotal,
                    .lngIDSecuencia = it.lngIDSecuencia,
                    .blnEnviar = True})
            Next

        Catch ex As Exception
            Throw ex
        End Try

        Return lst

    End Function

    Private Sub consultarLiquidacionesEnviar()
        Try
            If FechaIni <= FechaHasta Then
                IsBusy = True
                Enabled = False
                strErrores = String.Empty
                dcProxy.Load(dcProxy.LiquidacionesEnviarAdmonQuery(FechaIni, FechaHasta,Program.Usuario, Program.HashConexion), AddressOf terminoCargarLiquidaciones, "")
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La fecha inicial no debe ser mayor que la fecha final.", Program.TituloSistema)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las liquidaciones para enviar a administración.", Me.ToString(), "consultarLiquidacionesEnviar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub EnviarLiquidacionesEnviar()
        Try
            intContadorEnvio = 0
            intContadorRegresos = 0
            PuedeEnviar = False
            Dim strDatos As String = String.Empty
            Dim strMensajeError As String
            strErrores = String.Empty
            intContadorAEnviar = ListaLiquidacionesYankees.Where(Function(i) i.blnEnviar = True).Count
            If intContadorAEnviar > 0 Then
                IsBusy = True
                For Each it As LiquidacionesYankees In ListaLiquidacionesYankees
                    If it.blnEnviar Then
                        strMensajeError = String.Empty
                        intContadorEnvio += 1
                        strDatos = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", it.lngID.ToString(), it.lngIDSecuencia.ToString(),
                            it.lngIDComitente, it.strIDEspecie, it.lngIDNroCustodia.ToString(), it.strTipoIdentificacion, it.strModalidadTitulo, it.dtmEmision.ToString(),
                            it.dtmVencimiento.ToString(), it.dblValorNominal.ToString(), it.lngNroDocumento.ToString(), it.strIsin, IIf(IsNothing(it.lngIDDepositoExtranjero), "0", it.lngIDDepositoExtranjero.ToString()).ToString(),
                            it.dblValorTotal.ToString(), it.dtmCumplimiento.ToString())

                        If IsNothing(it.lngIDComitente) Or IsNothing(it.strTipoIdentificacion) Or IsNothing(it.lngNroDocumento) Or
                            IsNothing(it.strComitente) Or IsNothing(it.strTelefono1) Or IsNothing(it.strDireccion) Or IsNothing(it.dtmRegistro) Then
                            strMensajeError = "La operación " + it.lngID.ToString() + " tiene información incompleta." + vbNewLine +
                                "Por favor revise los datos de comitente, tipo de identificación, documento, teléfono, dirección y/o fecha de registro"
                        End If

                        If strMensajeError.Length > 0 Then
                            strErrores += strMensajeError + vbNewLine
                        Else
                            dcProxy.EnviarLiquidacionesCrear(-1, it.lngIDComitente, it.strTipoIdentificacion, it.lngNroDocumento, it.strComitente, it.strTelefono1, it.strDireccion, it.dtmRegistro, "P", Nothing, Nothing, String.Empty, Nothing, Program.Usuario, Program.HashConexion, AddressOf terminoEnviarLiquidaciones, strDatos)
                        End If
                    End If
                Next
                If intContadorAEnviar = intContadorEnvio And strErrores.Length > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje(strErrores, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    PuedeEnviar = True
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No fue seleccionada ninguna operación", Program.TituloSistema)
                IsBusy = False
                PuedeEnviar = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al enviar las liquidaciones a administración.", Me.ToString(), "EnviarLiquidacionesEnviar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub terminoEnviarLiquidaciones(ByVal lo As InvokeOperation(Of Integer))
        Dim intIdOrden, indIdSecuencia, plngNroDocumento, plngIDDepositoExtranjero As Integer
        Dim pdblCantidad, pcurTotalLiq As Double
        Dim lngIdComitente, strIdEspecie, pstrNroTitulo, pstrTipoIdentificacion, pstrModalidad, strIsin As String
        Dim pdtmEmision, pdtmVencimiento, pdtmCumplimientoTitulo As Date
        Dim strState() As String
        Dim strDatos As String = String.Empty
        Try
            If Not lo.HasError Then

                Dim intIdRecibo As Integer = lo.Value

                strState = lo.UserState.Split(CChar("|"))
                intIdOrden = Integer.Parse(strState(0).ToString())
                indIdSecuencia = Integer.Parse(strState(1).ToString())
                lngIdComitente = strState(2).ToString()
                strIdEspecie = strState(3).ToString()
                pstrNroTitulo = strState(4).ToString()
                pstrTipoIdentificacion = strState(5).ToString()
                pstrModalidad = strState(6).ToString()
                pdtmEmision = Date.Parse(strState(7).ToString(), CultureInfo.CurrentCulture)
                pdtmVencimiento = Date.Parse(strState(8).ToString(), CultureInfo.CurrentCulture)
                pdblCantidad = Double.Parse(strState(9).ToString())
                plngNroDocumento = Integer.Parse(strState(10).ToString())
                strIsin = strState(11).ToString()
                plngIDDepositoExtranjero = Integer.Parse(strState(12).ToString())
                pcurTotalLiq = Double.Parse(strState(13).ToString())
                pdtmCumplimientoTitulo = Date.Parse(strState(14).ToString(), CultureInfo.CurrentCulture)


                strDatos = String.Format("{0}|{1}|{2}|{3}", indIdSecuencia.ToString(), plngNroDocumento.ToString(), intIdOrden.ToString(), lngIdComitente)

                dcProxy.IngresarDetalleCustodiaYankee(intIdRecibo, indIdSecuencia, lngIdComitente, strIdEspecie, pstrNroTitulo, pstrTipoIdentificacion, pstrModalidad, pdtmEmision,
                        pdtmVencimiento, pdblCantidad, plngNroDocumento, strIsin, Program.Usuario, plngIDDepositoExtranjero, pcurTotalLiq, pdtmCumplimientoTitulo, Program.HashConexion, AddressOf terminoEnviarDetalleLiquidaciones, strDatos)
            Else
                strErrores += lo.Error.Message + vbNewLine
            End If
        Catch ex As Exception
            strErrores += ex.Message + vbNewLine
        End Try
    End Sub

    Private Sub terminoEnviarDetalleLiquidaciones(ByVal lo As InvokeOperation(Of Integer))

        Dim indIdSecuencia, plngNroDocumento, intIdOrden, lngIdComitente As Integer
        Dim strState() As String

        Try
            If Not lo.HasError Then

                strState = lo.UserState.Split(CChar("|"))

                indIdSecuencia = Integer.Parse(strState(0).ToString())
                plngNroDocumento = Integer.Parse(strState(1).ToString())
                intIdOrden = Integer.Parse(strState(2).ToString())
                lngIdComitente = Integer.Parse(strState(3).ToString())

                dcProxy.Custodia_Beneficiario_Yankee_Modificar(indIdSecuencia, plngNroDocumento, intIdOrden, lngIdComitente, Program.Usuario, Program.HashConexion, AddressOf terminoActualizarCustodiaBeneficiario, String.Empty)

            Else
                strErrores += lo.Error.Message + vbNewLine
            End If
        Catch ex As Exception
            strErrores += ex.Message + vbNewLine
        End Try

    End Sub

    Private Sub terminoActualizarCustodiaBeneficiario(ByVal lo As InvokeOperation(Of Integer))
        Try
            intContadorRegresos += 1
            If Not lo.HasError Then
                If intContadorAEnviar = intContadorRegresos Then
                    A2Utilidades.Mensajes.mostrarMensaje("El proceso de envío ha terminado", Program.TituloSistema)

                    Dim items = ListaLiquidacionesYankees.Where(Function(i) i.blnEnviar = True)

                    ListaLiquidacionesYankees.RemoveAll(Function(i) items.Contains(i))
                    MyBase.CambioItem("ListaLiquidacionesYankees")
                    MyBase.CambioItem("ListaLiquidacionesYankeesPaged")

                    IsBusy = False
                End If
            Else
                If intContadorAEnviar = intContadorRegresos And strErrores.Length > 0 Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentaron problemas al enviar las liquidaciones a administración." + vbNewLine + strErrores, Me.ToString(), "terminoActualizarCustodiaBeneficiario", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                    IsBusy = False
                Else
                    strErrores += lo.Error.Message + vbNewLine
                End If
            End If
            PuedeEnviar = True
        Catch ex As Exception
            If intContadorEnvio = intContadorRegresos Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentaron problemas al enviar las liquidaciones a administración." + vbNewLine + strErrores, Me.ToString(), "terminoActualizarCustodiaBeneficiario", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
                IsBusy = False
            Else
                strErrores += lo.Error.Message + vbNewLine
            End If
        End Try
    End Sub

End Class


Public Class LiquidacionesYankees
    Implements INotifyPropertyChanged

    Private _intIDLiquidaciones_Yankees As Integer
    Public Property intIDLiquidaciones_Yankees() As Integer
        Get
            Return _intIDLiquidaciones_Yankees
        End Get
        Set(ByVal value As Integer)
            _intIDLiquidaciones_Yankees = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDLiquidaciones_Yankees"))
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

    Private _lngIDComitente As String
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private _strComitente As String
    Public Property strComitente() As String
        Get
            Return _strComitente
        End Get
        Set(ByVal value As String)
            _strComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strComitente"))
        End Set
    End Property

    Private _strTipoIdentificacion As System.Nullable(Of Char)
    Public Property strTipoIdentificacion() As System.Nullable(Of Char)
        Get
            Return _strTipoIdentificacion
        End Get
        Set(ByVal value As System.Nullable(Of Char))
            _strTipoIdentificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoIdentificacion"))
        End Set
    End Property

    Private _lngNroDocumento As System.Nullable(Of Decimal)
    Public Property lngNroDocumento() As System.Nullable(Of Decimal)
        Get
            Return _lngNroDocumento
        End Get
        Set(ByVal value As System.Nullable(Of Decimal))
            _lngNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngNroDocumento"))
        End Set
    End Property

    Private _strDireccion As String
    Public Property strDireccion() As String
        Get
            Return _strDireccion
        End Get
        Set(ByVal value As String)
            _strDireccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDireccion"))
        End Set
    End Property

    Private _strTelefono1 As String
    Public Property strTelefono1() As String
        Get
            Return _strTelefono1
        End Get
        Set(ByVal value As String)
            _strTelefono1 = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTelefono1"))
        End Set
    End Property

    Private _dtmRegistro As Date
    Public Property dtmRegistro() As Date
        Get
            Return _dtmRegistro
        End Get
        Set(ByVal value As Date)
            _dtmRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmRegistro"))
        End Set
    End Property

    Private _dtmCumplimiento As Date
    Public Property dtmCumplimiento() As Date
        Get
            Return _dtmCumplimiento
        End Get
        Set(ByVal value As Date)
            _dtmCumplimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmCumplimiento"))
        End Set
    End Property

    Private _strIsin As String
    Public Property strIsin() As String
        Get
            Return _strIsin
        End Get
        Set(ByVal value As String)
            _strIsin = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIsin"))
        End Set
    End Property

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

    Private _dtmEmision As System.Nullable(Of Date)
    Public Property dtmEmision() As System.Nullable(Of Date)
        Get
            Return _dtmEmision
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _dtmEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmEmision"))
        End Set
    End Property

    Private _dtmVencimiento As System.Nullable(Of Date)
    Public Property dtmVencimiento() As System.Nullable(Of Date)
        Get
            Return _dtmVencimiento
        End Get
        Set(ByVal value As System.Nullable(Of Date))
            _dtmVencimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmVencimiento"))
        End Set
    End Property

    Private _strModalidadTitulo As String
    Public Property strModalidadTitulo() As String
        Get
            Return _strModalidadTitulo
        End Get
        Set(ByVal value As String)
            _strModalidadTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strModalidadTitulo"))
        End Set
    End Property

    Private _lngIDCertcus As System.Nullable(Of Integer)
    Public Property lngIDCertcus() As System.Nullable(Of Integer)
        Get
            Return _lngIDCertcus
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDCertcus = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDCertcus"))
        End Set
    End Property

    Private _lngIDNroCustodia As System.Nullable(Of Integer)
    Public Property lngIDNroCustodia() As System.Nullable(Of Integer)
        Get
            Return _lngIDNroCustodia
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDNroCustodia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDNroCustodia"))
        End Set
    End Property

    Private _dblValorNominal As System.Nullable(Of Double)
    Public Property dblValorNominal() As System.Nullable(Of Double)
        Get
            Return _dblValorNominal
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValorNominal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorNominal"))
        End Set
    End Property

    Private _dblValorTotal As System.Nullable(Of Double)
    Public Property dblValorTotal() As System.Nullable(Of Double)
        Get
            Return _dblValorTotal
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValorTotal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorTotal"))
        End Set
    End Property

    Private _lngIDSecuencia As Integer
    Public Property lngIDSecuencia() As Integer
        Get
            Return _lngIDSecuencia
        End Get
        Set(ByVal value As Integer)
            _lngIDSecuencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDSecuencia"))
        End Set
    End Property

    Private _lngIDDepositoExtranjero As System.Nullable(Of Integer)
    Public Property lngIDDepositoExtranjero() As System.Nullable(Of Integer)
        Get
            Return _lngIDDepositoExtranjero
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDDepositoExtranjero = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDDepositoExtranjero"))
        End Set
    End Property

    Private _blnEnviar As Boolean
    Public Property blnEnviar() As Boolean
        Get
            Return _blnEnviar
        End Get
        Set(ByVal value As Boolean)
            _blnEnviar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("blnEnviar"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class