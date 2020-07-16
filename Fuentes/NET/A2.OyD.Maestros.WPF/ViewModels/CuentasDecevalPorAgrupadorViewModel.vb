Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CuentasDecevalPorAgrupadorViewModel.vb
'Generado el : 04/29/2011 16:14:31
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class CuentasDecevalPorAgrupadorViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private CuentasDecevalPorAgrupadoPorDefecto As CuentasDecevalPorAgrupado
    Private CuentasDecevalPorAgrupadoAnterior As CuentasDecevalPorAgrupado
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Public _mlogBuscarCliente As Boolean = True

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            objProxy = New UtilidadesDomainContext()

        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCuentasDecevalPorAgrupadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupadorPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CuentasDecevalPorAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCuentasDecevalPorAgrupadorPorDefecto_Completed(ByVal lo As LoadOperation(Of CuentasDecevalPorAgrupado))
        If Not lo.HasError Then
            CuentasDecevalPorAgrupadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CuentasDecevalPorAgrupado por defecto", _
                                             Me.ToString(), "TerminoTraerCuentasDecevalPorAgrupadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasDecevalPorAgrupador(ByVal lo As LoadOperation(Of CuentasDecevalPorAgrupado))
        If Not lo.HasError Then
            ListaCuentasDecevalPorAgrupador = dcProxy.CuentasDecevalPorAgrupados
            Tabladisponibles.Clear()
            Tabladisponibles.Add(New conector1 With {.Tipo = "Y", .Descripcion = "Y"})
            Tabladisponibles.Add(New conector1 With {.Tipo = "O", .Descripcion = "O"})
            Tabladisponibles.Add(New conector1 With {.Tipo = " ", .Descripcion = " "})
            Tablaconector.Clear()
            Tablaconector.Add(New conector2 With {.Tipo = "Y", .Descripcion = "Y"})
            Tablaconector.Add(New conector2 With {.Tipo = "O", .Descripcion = "O"})
            Tablaconector.Add(New conector2 With {.Tipo = " ", .Descripcion = " "})
            If dcProxy.CuentasDecevalPorAgrupados.Count > 0 Then
                If lo.UserState = "insert" Then
                    CuentasDecevalPorAgrupadoSelected = ListaCuentasDecevalPorAgrupador.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CuentasDecevalPorAgrupador", _
                                             Me.ToString(), "TerminoTraerCuentasDecevalPorAgrupado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub TerminoTraerCuentasDeceval(ByVal lo As LoadOperation(Of CuentasDeceval))
        If Not lo.HasError Then
            ListaCuentasDecevalPorAgrupadorDeceval = dcProxy.CuentasDecevals

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresSistemas", _
                                             Me.ToString(), "TerminoTraerReceptoresSistemas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If


    End Sub
    Private Sub TerminoTraerBeneficiarios(ByVal lo As LoadOperation(Of ListaBeneficiarios))
        If Not lo.HasError Then
            ListaCuentasBeneficiarios = dcProxy.ListaBeneficiarios

            '_CuentasDecevalPorAgrupadoSelected.NroDocBenef1 = CuentasDecevalPorAgrupadoSelected.NroDocBenef1
            '_CuentasDecevalPorAgrupadoSelected.NroDocBenef2 = CuentasDecevalPorAgrupadoSelected.NroDocBenef2

            MyBase.CambioItem("ListaCuentasBeneficiarios")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresSistemas", _
                                             Me.ToString(), "TerminoTraerReceptoresSistemas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If


    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaCuentasDecevalPorAgrupador As EntitySet(Of CuentasDecevalPorAgrupado)
    Public Property ListaCuentasDecevalPorAgrupador() As EntitySet(Of CuentasDecevalPorAgrupado)
        Get
            Return _ListaCuentasDecevalPorAgrupador
        End Get
        Set(ByVal value As EntitySet(Of CuentasDecevalPorAgrupado))
            _ListaCuentasDecevalPorAgrupador = value

            MyBase.CambioItem("ListaCuentasDecevalPorAgrupador")
            MyBase.CambioItem("ListaCuentasDecevalPorAgrupadorPaged")
            If Not IsNothing(value) Then
                If IsNothing(CuentasDecevalPorAgrupadoAnterior) Then
                    CuentasDecevalPorAgrupadoSelected = _ListaCuentasDecevalPorAgrupador.FirstOrDefault
                Else
                    CuentasDecevalPorAgrupadoSelected = CuentasDecevalPorAgrupadoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCuentasDecevalPorAgrupadorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCuentasDecevalPorAgrupador) Then
                Dim view = New PagedCollectionView(_ListaCuentasDecevalPorAgrupador)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CuentasDecevalPorAgrupadoSelected As CuentasDecevalPorAgrupado
    Public Property CuentasDecevalPorAgrupadoSelected() As CuentasDecevalPorAgrupado
        Get
            Return _CuentasDecevalPorAgrupadoSelected
        End Get
        Set(ByVal value As CuentasDecevalPorAgrupado)

            dcProxy.CuentasDecevals.Clear()
            dcProxy.ListaBeneficiarios.Clear()
            If Not value Is Nothing Then
                dcProxy.Load(dcProxy.TraerBeneficiariosQuery(value.Comitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiarios, value)
                dcProxy.Load(dcProxy.TraerCuentasDecevalPorAgrupado_DecevalQuery(value.Comitente, value.TipoIdComitente, value.NroDocumento,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeceval, Nothing)
            Else
                ListaCuentasDecevalPorAgrupadorDeceval = dcProxy.CuentasDecevals
            End If
            _CuentasDecevalPorAgrupadoSelected = value
            MyBase.CambioItem("CuentasDecevalPorAgrupadoSelected")
        End Set
    End Property

    'Tblashijas
    Private _ListaCuentasDecevalPorAgrupadorDeceval As EntitySet(Of CuentasDeceval)
    Public Property ListaCuentasDecevalPorAgrupadorDeceval() As EntitySet(Of CuentasDeceval)
        Get
            Return _ListaCuentasDecevalPorAgrupadorDeceval
        End Get
        Set(ByVal value As EntitySet(Of CuentasDeceval))
            _ListaCuentasDecevalPorAgrupadorDeceval = value
            MyBase.CambioItem("ListaCuentasDecevalPorAgrupadorDeceval")
            MyBase.CambioItem("ListaCuentasDecevalPorAgrupadorDecevalPaged")
        End Set
    End Property
    Public ReadOnly Property CuentasDecevalPorAgrupadorDecevalPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCuentasDecevalPorAgrupadorDeceval) Then
                Dim view = New PagedCollectionView(_ListaCuentasDecevalPorAgrupadorDeceval)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _CuentasDecevalPorAgrupadorDecevalSelected As CuentasDeceval
    Public Property CuentasDecevalPorAgrupadorDecevalSelected() As CuentasDeceval
        Get
            Return _CuentasDecevalPorAgrupadorDecevalSelected
        End Get
        Set(ByVal value As CuentasDeceval)
            If Not value Is Nothing Then
                _CuentasDecevalPorAgrupadorDecevalSelected = value
                MyBase.CambioItem("CuentasDecevalPorAgrupadorDecevalSelected")
            End If
        End Set
    End Property


    Private _ListaCuentasBeneficiarios As EntitySet(Of ListaBeneficiarios)
    Public Property ListaCuentasBeneficiarios() As EntitySet(Of ListaBeneficiarios)
        Get
            Return _ListaCuentasBeneficiarios
        End Get
        Set(ByVal value As EntitySet(Of ListaBeneficiarios))
            _ListaCuentasBeneficiarios = value
            If Not IsNothing(value) Then

                BeneficiariosSelected = _ListaCuentasBeneficiarios.FirstOrDefault


            End If
            MyBase.CambioItem("ListaCuentasBeneficiarios")
        End Set
    End Property

    Private _BeneficiariosSelected As ListaBeneficiarios
    Public Property BeneficiariosSelected() As ListaBeneficiarios
        Get
            Return _BeneficiariosSelected
        End Get
        Set(ByVal value As ListaBeneficiarios)
            _BeneficiariosSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("BeneficiariosSelected")
            End If
        End Set
    End Property
    Private _EditaRegistro As Boolean = False
    Public Property EditaRegistro As Boolean
        Get
            Return _EditaRegistro
        End Get
        Set(ByVal value As Boolean)
            _EditaRegistro = value
            MyBase.CambioItem("EditaRegistro")
        End Set
    End Property
    Private _Checked As Boolean = False
    Public ReadOnly Property Checked() As Boolean
        Get
            Return Not Enabled
        End Get
    End Property

    Private _Enabled As Boolean = True
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
            MyBase.CambioItem("Checked")
            MyBase.CambioItem("Enabled")
        End Set
    End Property
    'Tablas hijas
    Private _Tabladisponibles As List(Of conector1) = New List(Of conector1)
    Public Property Tabladisponibles() As List(Of conector1)
        Get
            Return _Tabladisponibles
        End Get
        Set(ByVal value As List(Of conector1))
            _Tabladisponibles = value


            MyBase.CambioItem("Tabladisponibles")

        End Set

    End Property

    Private _tablaSeleccionada As conector1
    Public Property tablaSeleccionada() As conector1
        Get
            Return _tablaSeleccionada
        End Get
        Set(ByVal value As conector1)
            _tablaSeleccionada = value
            If Not value Is Nothing Then
                MyBase.CambioItem("tablaSeleccionada")
            End If
        End Set
    End Property

    Private _Tablaconector As List(Of conector2) = New List(Of conector2)
    Public Property Tablaconector() As List(Of conector2)
        Get
            Return _Tablaconector
        End Get
        Set(ByVal value As List(Of conector2))
            _Tablaconector = value


            MyBase.CambioItem("Tablaconector")

        End Set

    End Property

    Private _tablaSeleccionadaconector As conector2
    Public Property tablaSeleccionadaconector() As conector2
        Get
            Return _tablaSeleccionadaconector
        End Get
        Set(ByVal value As conector2)
            _tablaSeleccionadaconector = value
            If Not value Is Nothing Then
                MyBase.CambioItem("tablaSeleccionadaconector")
            End If
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")

        End Set
    End Property

    Private _cb As CamposBusquedaCuentasDecevalPorAgrupado = New CamposBusquedaCuentasDecevalPorAgrupado
    Public Property cb() As CamposBusquedaCuentasDecevalPorAgrupado
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCuentasDecevalPorAgrupado)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewCuentasDecevalPorAgrupado As New CuentasDecevalPorAgrupado
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCuentasDecevalPorAgrupado.IdComisionista = CuentasDecevalPorAgrupadoPorDefecto.IdComisionista
            NewCuentasDecevalPorAgrupado.IdSucComisionista = CuentasDecevalPorAgrupadoPorDefecto.IdSucComisionista
            NewCuentasDecevalPorAgrupado.TipoIdComitente = CuentasDecevalPorAgrupadoPorDefecto.TipoIdComitente
            NewCuentasDecevalPorAgrupado.NroDocumento = CuentasDecevalPorAgrupadoPorDefecto.NroDocumento
            NewCuentasDecevalPorAgrupado.Comitente = CuentasDecevalPorAgrupadoPorDefecto.Comitente
            NewCuentasDecevalPorAgrupado.CuentaDeceval = CuentasDecevalPorAgrupadoPorDefecto.CuentaDeceval
            NewCuentasDecevalPorAgrupado.Conector1 = CuentasDecevalPorAgrupadoPorDefecto.Conector1
            NewCuentasDecevalPorAgrupado.TipoIdBenef1 = CuentasDecevalPorAgrupadoPorDefecto.TipoIdBenef1
            NewCuentasDecevalPorAgrupado.NroDocBenef1 = CuentasDecevalPorAgrupadoPorDefecto.NroDocBenef1
            NewCuentasDecevalPorAgrupado.Conector2 = CuentasDecevalPorAgrupadoPorDefecto.Conector2
            NewCuentasDecevalPorAgrupado.TipoIdBenef2 = CuentasDecevalPorAgrupadoPorDefecto.TipoIdBenef2
            NewCuentasDecevalPorAgrupado.NroDocBenef2 = CuentasDecevalPorAgrupadoPorDefecto.NroDocBenef2
            NewCuentasDecevalPorAgrupado.Deposito = CuentasDecevalPorAgrupadoPorDefecto.Deposito
            NewCuentasDecevalPorAgrupado.Actualizacion = CuentasDecevalPorAgrupadoPorDefecto.Actualizacion
            NewCuentasDecevalPorAgrupado.Usuario = Program.Usuario
            NewCuentasDecevalPorAgrupado.CuentaPorCliente = CuentasDecevalPorAgrupadoPorDefecto.CuentaPorCliente
            NewCuentasDecevalPorAgrupado.intermedia = CuentasDecevalPorAgrupadoPorDefecto.intermedia
            NewCuentasDecevalPorAgrupado.CuentaPrincipalDCV = CuentasDecevalPorAgrupadoPorDefecto.CuentaPrincipalDCV
            NewCuentasDecevalPorAgrupado.IDCuentasDeceval = CuentasDecevalPorAgrupadoPorDefecto.IDCuentasDeceval
            NewCuentasDecevalPorAgrupado.Activa = True
            NewCuentasDecevalPorAgrupado.Prefijo = CuentasDecevalPorAgrupadoPorDefecto.Prefijo 'JEPM20150903
            CuentasDecevalPorAgrupadoAnterior = CuentasDecevalPorAgrupadoSelected
            CuentasDecevalPorAgrupadoSelected = NewCuentasDecevalPorAgrupado
            MyBase.CambioItem("CuentasDecevalPorAgrupador")
            Editando = True
            EditaRegistro = True
            Enabled = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CuentasDecevalPorAgrupados.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub Buscar()
        cb.Comitente = String.Empty
        cb.Deposito = String.Empty
        cb.CuentaDeceval = 0
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Comitente <> String.Empty Or cb.Deposito <> String.Empty Or cb.CuentaDeceval <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CuentasDecevalPorAgrupados.Clear()
                CuentasDecevalPorAgrupadoAnterior = Nothing
                IsBusy = True
                ' DescripcionFiltroVM = " Comitente = " & cb.Comitente.ToString()
                dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorConsultarQuery(cb.Comitente, cb.CuentaDeceval, cb.Deposito,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCuentasDecevalPorAgrupado
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            'If CuentasDecevalPorAgrupadoSelected.NroDocBenef1 <> 0 Or CuentasDecevalPorAgrupadoSelected.NroDocBenef2 <> 0 Or CuentasDecevalPorAgrupadoSelected.Conector1 <> String.Empty Or CuentasDecevalPorAgrupadoSelected.Conector2 <> String.Empty Then
            '    If CuentasDecevalPorAgrupadoSelected.NroDocBenef1 = 0 And CuentasDecevalPorAgrupadoSelected.NroDocBenef2 <> 0 Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Falta el primer beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    ElseIf CuentasDecevalPorAgrupadoSelected.Conector1 = Nothing And CuentasDecevalPorAgrupadoSelected.Conector2 <> String.Empty Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Falta el conector entre el primer y segundo beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    ElseIf ((CuentasDecevalPorAgrupadoSelected.NroDocBenef1 = 0 And CuentasDecevalPorAgrupadoSelected.NroDocBenef2 <> 0) And (CuentasDecevalPorAgrupadoSelected.NroDocBenef1 = CuentasDecevalPorAgrupadoSelected.NroDocBenef2)) Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Los Beneficiarios no pueden ser iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        Exit Sub
            '    End If
            'End If

            If String.IsNullOrEmpty(_CuentasDecevalPorAgrupadoSelected.Deposito) Then
                A2Utilidades.Mensajes.mostrarMensaje("El depósito es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(_CuentasDecevalPorAgrupadoSelected.CuentaDeceval) Or _CuentasDecevalPorAgrupadoSelected.CuentaDeceval = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta deceval es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_CuentasDecevalPorAgrupadoSelected.Comitente) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_CuentasDecevalPorAgrupadoSelected.NroDocumento) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If CuentasDecevalPorAgrupadoSelected.Conector1 = "Y" Or CuentasDecevalPorAgrupadoSelected.Conector1 = "O" Then
                If CuentasDecevalPorAgrupadoSelected.NroDocBenef1 = 0 Or IsNothing(CuentasDecevalPorAgrupadoSelected.NroDocBenef1) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Falta el primer beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            ElseIf CuentasDecevalPorAgrupadoSelected.NroDocBenef1 <> 0 And Not IsNothing(CuentasDecevalPorAgrupadoSelected.NroDocBenef1) Then
                A2Utilidades.Mensajes.mostrarMensaje("Falta el Conector entre el comitente y el primer beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If CuentasDecevalPorAgrupadoSelected.Conector2 = "Y" Or CuentasDecevalPorAgrupadoSelected.Conector2 = "O" Then
                If CuentasDecevalPorAgrupadoSelected.NroDocBenef2 = 0 Or IsNothing(CuentasDecevalPorAgrupadoSelected.NroDocBenef2) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Falta el segundo beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            ElseIf CuentasDecevalPorAgrupadoSelected.NroDocBenef2 <> 0 And Not IsNothing(CuentasDecevalPorAgrupadoSelected.NroDocBenef2) Then
                A2Utilidades.Mensajes.mostrarMensaje("Falta el Conector el primer y segundo beneficiario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If ((CuentasDecevalPorAgrupadoSelected.NroDocBenef1 = CuentasDecevalPorAgrupadoSelected.NroDocBenef2) And (CuentasDecevalPorAgrupadoSelected.NroDocBenef1 <> 0 And CuentasDecevalPorAgrupadoSelected.NroDocBenef2 <> 0)) Then
                A2Utilidades.Mensajes.mostrarMensaje("Los Beneficiarios no pueden ser iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim intNroOcurrencias As Integer

            ' JAEZ 20161130 Validar si el registro ya existe en la lista
            intNroOcurrencias = (From e In ListaCuentasDecevalPorAgrupador Where e.IDCuentasDeceval = CuentasDecevalPorAgrupadoSelected.IDCuentasDeceval Select e).Count

            Dim origen = "update"
            ErrorForma = ""
            'CuentasDecevalPorAgrupadoAnterior = CuentasDecevalPorAgrupadoSelected


            'JAEZ 20161130
            If intNroOcurrencias = 0 Then
                origen = "insert"
                ListaCuentasDecevalPorAgrupador.Add(_CuentasDecevalPorAgrupadoSelected)
            End If

            'JAEZ 20161130 Se cambia la forma de validar la existencia del registro
            'If Not ListaCuentasDecevalPorAgrupador.Contains(CuentasDecevalPorAgrupadoSelected) Then
            '    origen = "insert"
            '    ' CuentasDecevalPorAgrupadoSelected.TipoIdBenef1 = BeneficiariosSelected.TipoIdentificacion pendiente el builder de clientes 
            '    ListaCuentasDecevalPorAgrupador.Add(CuentasDecevalPorAgrupadoSelected)
            'End If

            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            'CFMA20171205
            'IsBusy = False
            'If So.HasError Then
            '    'TODO: Pendiente garantizar que Userstate no venga vacío          
            '    If So.Error.Message.Equals("Submit operation failed. Esta cuenta ya existe en este Deposito") Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Esta cuenta ya existe en este depósito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Else
            '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
            '                                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
            '    End If
            Dim strMsg As String = String.Empty
            IsBusy = False
            If So.HasError Then
                If So.UserState = "insert" Or So.UserState = "update" Then
                    If So.Error.ToString.Contains("ErrorPersonalizado") Then
                        Dim intPosIni As Integer = So.Error.ToString.IndexOf("ErrorPersonalizado,") + 19
                        Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                        strMsg = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                        Exit Sub
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                                    Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, So.Error)
                        So.MarkErrorAsHandled()
                    End If
                End If

                'CFMA20171205


                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                'EditaReg = True
                Exit Try
            End If

            If Not _CuentasDecevalPorAgrupadoSelected Is Nothing Then
                dcProxy.Load(dcProxy.TraerBeneficiariosQuery(_CuentasDecevalPorAgrupadoSelected.Comitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiarios, _CuentasDecevalPorAgrupadoSelected)
                dcProxy.Load(dcProxy.TraerCuentasDecevalPorAgrupado_DecevalQuery(_CuentasDecevalPorAgrupadoSelected.Comitente, _CuentasDecevalPorAgrupadoSelected.TipoIdComitente, _CuentasDecevalPorAgrupadoSelected.NroDocumento,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDeceval, Nothing)
            Else
                ListaCuentasDecevalPorAgrupadorDeceval = dcProxy.CuentasDecevals
            End If

            'If So.UserState = "insert" Then
            '    dcProxy.CuentasDecevalPorAgrupados.Clear()
            '    dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "insert") ' Recarga la lista para que carguen los include
            'End If
            EditaRegistro = False
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CuentasDecevalPorAgrupadoSelected) Then
            If CuentasDecevalPorAgrupadoSelected.Activa = False Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta esta inactiva, no se puede editar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                Editando = True
                EditaRegistro = False
                Enabled = False
            End If
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CuentasDecevalPorAgrupadoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaRegistro = False
                If _CuentasDecevalPorAgrupadoSelected.EntityState = EntityState.Detached Then
                    CuentasDecevalPorAgrupadoSelected = CuentasDecevalPorAgrupadoAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_CuentasDecevalPorAgrupadoSelected) Then
                If CuentasDecevalPorAgrupadoSelected.Activa = False Then
                    mostrarMensajePregunta("La cuenta ésta inactiva", _
                              Program.TituloSistema, _
                              "APROBACIONINACTIVA", _
                              AddressOf TerminovalidarGenerales, True, "Desea activarla?")
                Else
                    'dcProxy.CuentasDecevalPorAgrupados.Remove(_CuentasDecevalPorAgrupadoSelected)
                    IsBusy = True
                    'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                    dcProxy.EliminarCuentasDecevalPorAgrupador(CuentasDecevalPorAgrupadoSelected.Comitente, _CuentasDecevalPorAgrupadoSelected.CuentaDeceval, _CuentasDecevalPorAgrupadoSelected.IDCuentasDeceval, _CuentasDecevalPorAgrupadoSelected.Deposito, String.Empty,Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                If So.Value.Contains("inactivarla") Then
                    Dim mensaje = So.Value.Split(".")
                    mostrarMensajePregunta(mensaje(0), _
                                               Program.TituloSistema, _
                                               "APROBACIONACTIVA", _
                                               AddressOf TerminovalidarGenerales, True, mensaje(1))
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                If So.UserState = "borrar" Then
                    dcProxy.CuentasDecevalPorAgrupados.Clear()
                    dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub
    Private Sub TerminovalidarGenerales(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If Not IsNothing(objResultado) Then
            If Not IsNothing(objResultado.CodigoLlamado) Then
                Select Case objResultado.CodigoLlamado.ToUpper
                    Case "APROBACIONACTIVA"
                        Select Case objResultado.DialogResult
                            Case True
                                Try
                                    CuentasDecevalPorAgrupadoSelected.Activa = False
                                    IsBusy = True
                                    Program.VerificarCambiosProxyServidor(dcProxy)
                                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                                Catch ex As Exception
                                    IsBusy = False
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                                         Me.ToString(), "TerminovalidarGenerales", Application.Current.ToString(), Program.Maquina, ex)
                                End Try
                        End Select
                    Case "APROBACIONINACTIVA"
                        Select Case objResultado.DialogResult
                            Case True
                                Try
                                    CuentasDecevalPorAgrupadoSelected.Activa = True
                                    IsBusy = True
                                    Program.VerificarCambiosProxyServidor(dcProxy)
                                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                                Catch ex As Exception
                                    IsBusy = False
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                                         Me.ToString(), "TerminovalidarGenerales", Application.Current.ToString(), Program.Maquina, ex)
                                End Try
                        End Select
                End Select
            End If
        End If
    End Sub

    Sub Llamarbeneficiario()
        dcProxy.Load(dcProxy.TraerBeneficiariosQuery(CuentasDecevalPorAgrupadoSelected.Comitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBeneficiarios, Nothing)
    End Sub

    Private Sub _CuentasDecevalPorAgrupadoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CuentasDecevalPorAgrupadoSelected.PropertyChanged
        Select Case e.PropertyName
            Case "NroDocBenef1"
                CuentasDecevalPorAgrupadoSelected.TipoIdBenef1 = BeneficiariosSelected.TipoIdentificacion
            Case "NroDocBenef2"
                CuentasDecevalPorAgrupadoSelected.TipoIdBenef2 = BeneficiariosSelected.TipoIdentificacion
            Case "Comitente"
                'SLB20130930 Se adiciona la busqueda del comitente desde el control 
                If _mlogBuscarCliente Then
                    If Not String.IsNullOrEmpty(_CuentasDecevalPorAgrupadoSelected.Comitente) Then
                        buscarComitente(_CuentasDecevalPorAgrupadoSelected.Comitente, "encabezado")
                    End If
                End If
        End Select

    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Comitente", 1)
        DicCamposTab.Add("NroDocumento", 1)
        DicCamposTab.Add("Deposito", 1)
        DicCamposTab.Add("TipoIdComitente", 1)
    End Sub
#End Region

#Region "Busqueda de Comitente desde el control de la vista"

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la Tesoreria.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks>SLB20130122</remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")

        Dim strIdComitente As String = String.Empty

        Try
            If Not Me._CuentasDecevalPorAgrupadoSelected Is Nothing Then
                If Not strIdComitente.Equals(Me._CuentasDecevalPorAgrupadoSelected.Comitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me._CuentasDecevalPorAgrupadoSelected.Comitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130122</remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _CuentasDecevalPorAgrupadoSelected.Comitente = String.Empty
                    _CuentasDecevalPorAgrupadoSelected.NomCliente = String.Empty
                    _CuentasDecevalPorAgrupadoSelected.NroDocumento = String.Empty
                    _CuentasDecevalPorAgrupadoSelected.TipoIdComitente = String.Empty
                Else
                    _CuentasDecevalPorAgrupadoSelected.Comitente = lo.Entities.First.IdComitente
                    _CuentasDecevalPorAgrupadoSelected.NomCliente = lo.Entities.First.Nombre
                    _CuentasDecevalPorAgrupadoSelected.NroDocumento = lo.Entities.First.NroDocumento
                    _CuentasDecevalPorAgrupadoSelected.TipoIdComitente = lo.Entities.First.CodTipoIdentificacion
                    Llamarbeneficiario()
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _CuentasDecevalPorAgrupadoSelected.Comitente = String.Empty
                _CuentasDecevalPorAgrupadoSelected.NomCliente = String.Empty
                _CuentasDecevalPorAgrupadoSelected.NroDocumento = String.Empty
                _CuentasDecevalPorAgrupadoSelected.TipoIdComitente = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCuentasDecevalPorAgrupado
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente")> _
    Public Property Comitente() As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _Deposito As String
    <Display(Name:="Deposito")> _
   Public Property Deposito() As String
        Get
            Return _Deposito
        End Get
        Set(ByVal value As String)
            _Deposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Deposito"))
        End Set
    End Property

    Private _CuentaDeceval As Integer
    <Display(Name:="Cuenta Deceval")> _
    Public Property CuentaDeceval As Integer
        Get
            Return _CuentaDeceval
        End Get
        Set(value As Integer)
            _CuentaDeceval = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaDeceval"))
        End Set
    End Property


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


Public Class conector1

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Tipo", Description:="Tipo")> _
    Public Property Tipo As String

    <Display(Name:="Descripcion", Description:="Descripcion")> _
    Public Property Descripcion As String


End Class
Public Class conector2

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Tipo", Description:="Tipo")> _
    Public Property Tipo As String

    <Display(Name:="Descripcion", Description:="Descripcion")> _
    Public Property Descripcion As String


End Class





