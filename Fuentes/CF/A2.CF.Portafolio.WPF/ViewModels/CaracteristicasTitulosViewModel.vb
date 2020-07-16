Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio

Public Class CaracteristicasTitulosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"
    Public Property cb As New CamposBusquedaCaracteristicasTitulos
    Private CaracteristicasTitulosAnterior As CFPortafolio.CaracteristicasTitulos
    Dim dcProxy As PortafolioDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcProxyUtil As UtilidadesDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim fechacierre As DateTime
    Dim strcomitente, strnombre As String
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim LiquidacionesClienteSf As LiquidacionesCliente
    Dim cuentaDeceval As Integer = 0
    Private STRTRAZABILIDADTITULOS As String = "NO"
#End Region

    ''' <history>
    ''' ID de cambio: CP0006
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New PortafolioDomainContext()
            objProxy = New UtilidadesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            dcProxyUtil = New UtilidadesDomainContext
        Else
            dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            dcProxyUtil = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CaracteristicasTitulosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCaracteristicasTitulos, "FiltroInicial")
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  CustodiaViewModel)(Me)
                ConsultarParametros()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CustodiaViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincronicos"
    Private Sub TerminoTraerCaracteristicasTitulos(ByVal lo As LoadOperation(Of CFPortafolio.CaracteristicasTitulos))
        If Not lo.HasError Then
            ListaCaracteristicasTitulos = dcProxy.CaracteristicasTitulos
            If Not CaracteristicasTitulosSelected Is Nothing Then
                strEspecieAnterior = CaracteristicasTitulosSelected.IdEspecie
            End If
            If dcProxy.CaracteristicasTitulos.Count > 0 Then
                If lo.UserState = "insert" Then
                    CaracteristicasTitulosSelected = ListaCaracteristicasTitulos.Last
                End If

            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MessageBox.Show("No se encontro ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CaracteristicasTitulos", _
                                             Me.ToString(), "TerminoTraerCaracteristicasTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            fechacierre = obj.Value
        End If
    End Sub
    Private Sub TerminoTraerISIN(ByVal lo As LoadOperation(Of CFPortafolio.ListadoISIN))
        If Not lo.HasError Then
            listaISIN = dcProxy.ListadoISINs.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CaracteristicasTitulos", _
                                             Me.ToString(), "TerminoTraerISIN", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Private Sub TerminoTraerCuentas(ByVal lo As LoadOperation(Of CFPortafolio.ListadoCuenta))
        If Not lo.HasError Then
            'JBT 15 de agosto de 2013 se agrega condicional en la linea donde se asigna primeraCuenta.IdCuentaDeceval
            listaCuentas = dcProxy.ListadoCuentas.ToList
            If (listaCuentas.Count > 0) Then
                Dim primeraCuenta = listaCuentas.FirstOrDefault
                'If Editando Then
                _CaracteristicasTitulosSelected.IdCuentaDeceval = primeraCuenta.IdCuentaDeceval
                'End If

                'MyBase.CambioItem("listaCuentas")
                'PropiedadTextoCombos = ""
                'MyBase.CambioItem("CaracteristicasTitulosSelected")
            Else
                If Editando Then
                    _CaracteristicasTitulosSelected.IdCuentaDeceval = Nothing
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CaracteristicasTitulos", _
                                             Me.ToString(), "TerminoTraerCaracteristicasTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Valida cada uno de los campos del selected item que se desea modificar.
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 18/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 18/2013 - Resultado Ok
    ''' </history>       
    Private Sub TerminoTraerTipoDocumento(ByVal obj As LoadOperation(Of CFPortafolio.ValidacionesCaracteristica))
        If Not obj.HasError Then
            Dim validaciones As Boolean = False
            ListaValidacionesCaracteristicas = dcProxy.ValidacionesCaracteristicas

            If strEspecieAnterior <> CaracteristicasTitulosSelected.IdEspecie Then
                A2Utilidades.Mensajes.mostrarMensaje("El isin no corresponde a la especie " + strEspecieAnterior, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validaciones = False
                Exit Sub
            End If

            If Not IsNothing(CaracteristicasTitulosSelected.IdCuentaDeceval) Then
                If Len(CaracteristicasTitulosSelected.IdCuentaDeceval) > 9 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Cuenta de Deceval Invalida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    validaciones = False
                    Exit Sub
                End If
            End If

            If Len(CaracteristicasTitulosSelected.ISIN) > 12 Then
                A2Utilidades.Mensajes.mostrarMensaje("ISIN Invalido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                validaciones = False
                Exit Sub
            End If

            'SM20150916
            If STRTRAZABILIDADTITULOS = "SI" Then
                If String.IsNullOrEmpty(CaracteristicasTitulosSelected.strConceptoDesmaterializacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El concepto de actualización es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    validaciones = False
                    Exit Sub
                End If

                If IsNothing(CaracteristicasTitulosSelected.dtmFechaDesmaterializacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de desmaterialización es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    validaciones = False
                    Exit Sub
                End If

                If CaracteristicasTitulosSelected.dtmFechaDesmaterializacion > Now.Date Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha de desmaterialización no puede ser mayor a la fecha actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    validaciones = False
                    Exit Sub
                End If

                If Not IsNothing(CaracteristicasTitulosSelected.dtmFechaCierrePortafolio) Then
                    If CaracteristicasTitulosSelected.dtmFechaDesmaterializacion < CaracteristicasTitulosSelected.dtmFechaCierrePortafolio Or CaracteristicasTitulosSelected.dtmFechaDesmaterializacion < CaracteristicasTitulosSelected.dtmRecibo_Custodia Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de desmaterialización no puede ser menor a la fecha de cierre de portafolio o a la fecha de recibo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        validaciones = False
                        Exit Sub
                    End If
                End If
            End If
            'FIN SM20150916
            If dcProxy.ValidacionesCaracteristicas.Count > 0 Then
                For Each res In ListaValidacionesCaracteristicas

                    'Validamos que la cuenta de Deceval Matriculada exista en la especie
                    'Descripcion: Cuando la ubicacion del titulo es en el exterior, no es obligatorio seleccionar la cuenta.
                    'Modificado por: Jorge Pena (Alcuadrado s.a)
                    'Fecha: 13 de Febrero de 2012
                    If res.TipoDocumento <> "X" And res.TipoDocumento <> "F" Then
                        If Not res.ExisteCuentaDeceval Then
                            A2Utilidades.Mensajes.mostrarMensaje("La Cuenta del Fondo No existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            validaciones = False
                            Exit Sub
                        End If
                    End If

                    If Not String.IsNullOrEmpty(CaracteristicasTitulosSelected.ISIN) Then
                        If Not res.ExisteIsin Then
                            A2Utilidades.Mensajes.mostrarMensaje("El ISIN matriculado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            CaracteristicasTitulosSelected.ISIN = Nothing
                            validaciones = False
                            Exit Sub
                        End If
                    End If
                    If res.TipoDocumento <> "X" And res.TipoDocumento <> "F" Then
                        If CaracteristicasTitulosSelected.IdCuentaDeceval = 0 Or IsNothing(CaracteristicasTitulosSelected.IdCuentaDeceval) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Seleccione la cuenta del deposito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            validaciones = False
                            Exit Sub
                        End If
                    End If
                Next
            End If

            'CORREC_CITI_SV_2014
            'With CaracteristicasTitulosSelected.Liquidacion
            '    If Not IsDate(CaracteristicasTitulosSelected.Liquidacion) Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la fecha de la Liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        validaciones = False
            '        Exit Sub
            '    End If
            'End With


            'With CaracteristicasTitulosSelected.CumplimientoTitulo
            '    If Not IsDate(CaracteristicasTitulosSelected.CumplimientoTitulo) Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Debe  ingresar la fecha de cumplimiento del título de la liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        validaciones = False
            '        Exit Sub
            '    End If
            '    If Not CaracteristicasTitulosSelected.CumplimientoTitulo >= CaracteristicasTitulosSelected.Liquidacion Then
            '        A2Utilidades.Mensajes.mostrarMensaje("La fecha de cumplimiento debe ser mayor o igual que la de Liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        validaciones = False
            '        Exit Sub
            '    End If
            'End With

            'CORREC_CITI_SV_2014
            If IsDate(CaracteristicasTitulosSelected.Liquidacion) AndAlso IsDate(CaracteristicasTitulosSelected.CumplimientoTitulo) AndAlso Not CaracteristicasTitulosSelected.CumplimientoTitulo >= CaracteristicasTitulosSelected.Liquidacion Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de cumplimiento debe ser mayor o igual que la de Liquidación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validaciones = False
                Exit Sub
            End If

            If CaracteristicasTitulosSelected.Fondo = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione el deposito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                validaciones = False
                Exit Sub
            End If

            validaciones = True

            If Not validaciones Then Exit Sub
            Dim origen = "update"
            ErrorForma = ""
            CaracteristicasTitulosAnterior = CaracteristicasTitulosSelected
            If Not ListaCaracteristicasTitulos.Contains(CaracteristicasTitulosSelected) Then
                origen = "insert"
                ListaCaracteristicasTitulos.Add(CaracteristicasTitulosSelected)
            End If
            IsBusy = True
            CaracteristicasTitulosSelected.Usuario = Program.Usuario
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el Tipo de documento", Me.ToString(), "TerminoTraerTipoDocumento", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        End If

    End Sub
#End Region

#Region "Propiedades"

    Private _ListaValidacionesCaracteristicas As EntitySet(Of CFPortafolio.ValidacionesCaracteristica)
    Public Property ListaValidacionesCaracteristicas() As EntitySet(Of CFPortafolio.ValidacionesCaracteristica)
        Get
            Return _ListaValidacionesCaracteristicas
        End Get
        Set(ByVal value As EntitySet(Of CFPortafolio.ValidacionesCaracteristica))
            _ListaValidacionesCaracteristicas = value
            MyBase.CambioItem("ListaValidacionesCaracteristicas")
        End Set
    End Property

    Private _ListaCaracteristicasTitulos As EntitySet(Of CFPortafolio.CaracteristicasTitulos)
    Public Property ListaCaracteristicasTitulos() As EntitySet(Of CFPortafolio.CaracteristicasTitulos)
        Get
            Return _ListaCaracteristicasTitulos
        End Get
        Set(ByVal value As EntitySet(Of CFPortafolio.CaracteristicasTitulos))
            _ListaCaracteristicasTitulos = value
            If Not IsNothing(value) Then
                If IsNothing(CaracteristicasTitulosAnterior) Then
                    CaracteristicasTitulosSelected = _ListaCaracteristicasTitulos.FirstOrDefault
                Else
                    CaracteristicasTitulosSelected = CaracteristicasTitulosAnterior
                End If
            End If
            MyBase.CambioItem("ListaCaracteristicasTitulos")
            MyBase.CambioItem("ListaCaracteristicasTitulosPaged")
        End Set
    End Property


    Private WithEvents _CaracteristicasTitulosSelected As CFPortafolio.CaracteristicasTitulos
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz.
    ''' Descripción      : Se adiciona el Fondo para la consulta de TraerCuentas y se envia en cero el IdCuentaDeceval, en Visual Basic genrico se envia siempre en NULL.
    ''' Fecha            : Abril 03/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Abril 03/2013 - Resultado Ok 
    ''' </history> 
    ''' <history>
    ''' ID de cambio: CP0007
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    ''' <history>
    ''' Responsable:  Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Descripción:  Al seleccionar registros en la vista lista, al pasar a la vista forma siempre traia la primer especie, por lo tanto se asigna nuevamente
    ''' Fecha:        Febrero 8/2016
    ''' Pruebas CB:   Germán Arbey González Osorio (Alcuadrado S.A.) - Febrero 8/2016 - OK
    ''' </history>
    Public Property CaracteristicasTitulosSelected() As CFPortafolio.CaracteristicasTitulos
        Get
            Return _CaracteristicasTitulosSelected
        End Get
        Set(ByVal value As CFPortafolio.CaracteristicasTitulos)
            If Not value Is Nothing Then
                _CaracteristicasTitulosSelected = value
                dcProxy.ListadoCuentas.Clear()

                strEspecieAnterior = CaracteristicasTitulosSelected.IdEspecie

                If Not IsNothing(CaracteristicasTitulosSelected.IdCuentaDeceval) Then
                    cuentaDeceval = CaracteristicasTitulosSelected.IdCuentaDeceval
                End If

                HabilitarCamposRF = CaracteristicasTitulosSelected.ClaseLiquidacion <> "A"
                CaracteristicasTitulosSelected.HabilitarModalidad = False
                'dcProxy.Load(dcProxy.TraerCuentasConsultarQuery(CaracteristicasTitulosSelected.IdCuentaDeceval, CaracteristicasTitulosSelected.Comitente), AddressOf TerminoTraerCuentas, Nothing)
                dcProxy.Load(dcProxy.TraerCuentasConsultarQuery(cuentaDeceval,
                                                                CaracteristicasTitulosSelected.Comitente,
                                                                CaracteristicasTitulosSelected.Fondo,
                                                                Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentas, Nothing)
                buscarItem("Comitente")
                dcProxy.ListadoISINs.Clear()
                dcProxy.Load(dcProxy.TraerISINConsultarQuery(Nothing, CaracteristicasTitulosSelected.IdEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerISIN, Nothing)
                '_CaracteristicasTitulosSelected.IdCuentaDeceval = value.IdCuentaDeceval
                MyBase.CambioItem("listaCuentas")
                MyBase.CambioItem("HabilitarCamposRF")
                PropiedadTextoCombos = ""
            End If
            MyBase.CambioItem("CaracteristicasTitulosSelected")
        End Set
    End Property

    Private Sub _CaracteristicasTitulosSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CaracteristicasTitulosSelected.PropertyChanged
        Try
            'If e.PropertyName.Equals("Fondo") Then

            'End If
            If Editando Then
                Select Case e.PropertyName
                    Case "Fondo"
                        dcProxy.ListadoCuentas.Clear()
                        dcProxy.Load(dcProxy.TraerCuentasConsultarQuery(0,
                                                                        CaracteristicasTitulosSelected.Comitente,
                                                                        CaracteristicasTitulosSelected.Fondo,
                                                                        Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentas, Nothing)
                        HabilitarModalidad()
                End Select
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición", _
                                 Me.ToString(), "_CaracteristicasTitulosSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Sub HabilitarModalidad()
        If IsNothing(CaracteristicasTitulosSelected.ISIN) And Not IsNothing(CaracteristicasTitulosSelected.Modalidad) Then
            CaracteristicasTitulosSelected.HabilitarModalidad = True
        Else
            CaracteristicasTitulosSelected.HabilitarModalidad = False
        End If
    End Sub

    Public ReadOnly Property ListaCaracteristicasTitulosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCaracteristicasTitulos) Then
                Dim view = New PagedCollectionView(_ListaCaracteristicasTitulos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _listaISIN As List(Of CFPortafolio.ListadoISIN)
    Public Property listaISIN As List(Of CFPortafolio.ListadoISIN)
        Get
            Return _listaISIN
        End Get
        Set(ByVal value As List(Of CFPortafolio.ListadoISIN))
            _listaISIN = value
            MyBase.CambioItem("listaISIN")
        End Set
    End Property

    Private _listaCuentas As List(Of CFPortafolio.ListadoCuenta)
    Public Property listaCuentas As List(Of CFPortafolio.ListadoCuenta)
        Get
            Return _listaCuentas
        End Get
        Set(ByVal value As List(Of CFPortafolio.ListadoCuenta))
            _listaCuentas = value
            MyBase.CambioItem("listaCuentas")
        End Set
    End Property
    Private _Editareg As Boolean = False
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property

    Private _Busqueda As CamposBusquedaCaracteristicasTitulos = New CamposBusquedaCaracteristicasTitulos
    Public Property Busqueda As CamposBusquedaCaracteristicasTitulos
        Get
            Return _Busqueda
        End Get
        Set(ByVal value As CamposBusquedaCaracteristicasTitulos)
            _Busqueda = value
            MyBase.CambioItem("Busqueda")
        End Set
    End Property

    Private _ComitenteDescripcion As ComitenteNombre = New ComitenteNombre
    Public Property ComitenteDescripcion As ComitenteNombre
        Get
            Return _ComitenteDescripcion
        End Get
        Set(ByVal value As ComitenteNombre)
            _ComitenteDescripcion = value
            MyBase.CambioItem("ComitenteDescripcion")
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

    ''' <summary>
    ''' Propiedad para obtener o asignar el atributo IsEnabled de un control. 
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       :  
    ''' </history>
    Private _LiquidacionesClienteHabilitado As New LiquidacionesClienteHabilitado
    Public Property LiquidacionesClienteHabilitado As LiquidacionesClienteHabilitado
        Get
            Return _LiquidacionesClienteHabilitado
        End Get
        Set(value As LiquidacionesClienteHabilitado)
            _LiquidacionesClienteHabilitado = value
            MyBase.CambioItem("LiquidacionesClienteHabilitado")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para obtener o asignar el atributo Visibility de un control. 
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       :  
    ''' </history>''' 
    Private _LiquidacionesClienteVisibilidad As New LiquidacionesClienteVisibilidad
    Public Property LiquidacionesClienteVisibilidad As LiquidacionesClienteVisibilidad
        Get
            Return _LiquidacionesClienteVisibilidad
        End Get
        Set(value As LiquidacionesClienteVisibilidad)
            _LiquidacionesClienteVisibilidad = value
            MyBase.CambioItem("LiquidacionesClienteVisibilidad")
        End Set
    End Property

    Private _HabilitarCamposRF As Boolean
    Public Property HabilitarCamposRF() As Boolean
        Get
            Return _HabilitarCamposRF
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposRF = value
            MyBase.CambioItem(_HabilitarCamposRF)
        End Set
    End Property

    Private _VisibilidadCamposDesmaterializacion As Visibility = Visibility.Collapsed
    Public Property VisibilidadCamposDesmaterializacion() As Visibility
        Get
            Return _VisibilidadCamposDesmaterializacion
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadCamposDesmaterializacion = value
            MyBase.CambioItem("VisibilidadCamposDesmaterializacion")
        End Set
    End Property

    Private _strEspecieAnterior As String
    Public Property strEspecieAnterior() As String
        Get
            Return _strEspecieAnterior
        End Get
        Set(ByVal value As String)
            _strEspecieAnterior = value
            MyBase.CambioItem("strEspecieAnterior")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        A2Utilidades.Mensajes.mostrarMensaje("Esta Funcionalidad No esta Habilitada para este Formulario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Try
            'Dim NewCustodi As New A2.OyD.OYDServer.RIA.Web.OyDTitulos.CaracteristicasTitulos
            ''TODO: Verificar cuales son los campos que deben inicializarse
            'NewCustodi.IdComisionista = CustodiPorDefecto.IdComisionista
            'NewCustodi.IdSucComisionista = CustodiPorDefecto.IdSucComisionista
            'NewCustodi.IdRecibo = CustodiPorDefecto.IdRecibo
            'NewCustodi.Comitente = CustodiPorDefecto.Comitente
            'NewCustodi.TipoIdentificacion = CustodiPorDefecto.TipoIdentificacion
            'NewCustodi.NroDocumento = CustodiPorDefecto.NroDocumento
            'NewCustodi.Nombre = CustodiPorDefecto.Nombre
            'NewCustodi.Telefono1 = CustodiPorDefecto.Telefono1
            'NewCustodi.Direccion = CustodiPorDefecto.Direccion
            'NewCustodi.Recibo = CustodiPorDefecto.Recibo
            'NewCustodi.Estado = "P"
            'NewCustodi.Fecha_Estado = CustodiPorDefecto.Fecha_Estado
            'NewCustodi.ConceptoAnulacion = CustodiPorDefecto.ConceptoAnulacion
            'NewCustodi.Notas = CustodiPorDefecto.Notas
            'NewCustodi.NroLote = CustodiPorDefecto.NroLote
            'NewCustodi.Elaboracion = CustodiPorDefecto.Elaboracion
            'NewCustodi.Actualizacion = CustodiPorDefecto.Actualizacion
            'NewCustodi.Usuario = Program.Usuario
            'NewCustodi.IDCustodia = CustodiPorDefecto.IDCustodia
            'NewCustodi.Aprobacion = 0
            'CustodiAnterior = CustodiSelected
            'CustodiSelected = NewCustodi
            'MyBase.CambioItem("Custodias")

            'Editando = True
            'Editarcampos = True
            'Editareg = True
            'Read = False
            'ConceptoA = False
            'MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' ID de cambio: CP0008
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Overrides Sub Filtrar()
        Try
            dcProxy.CaracteristicasTitulos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CaracteristicasTitulosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCaracteristicasTitulos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CaracteristicasTitulosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCaracteristicasTitulos, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID de cambio: CP0009
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Overrides Sub ConfirmarBuscar()
        Try
            If Busqueda.IdRecibo <> 0 Or Busqueda.Comitente <> String.Empty Or Busqueda.Especie <> String.Empty Or Busqueda.NroTitulo <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CaracteristicasTitulos.Clear()
                CaracteristicasTitulosAnterior = Nothing
                IsBusy = True
                ' DescripcionFiltroVM = " IdRecibo = " & cb.IdRecibo.ToString() & " Comitente = " & cb.Comitente.ToString()
                dcProxy.Load(dcProxy.CaracteristicasTitulosConsultarQuery(Busqueda.IdRecibo, Busqueda.Comitente, Busqueda.Especie, Busqueda.NroTitulo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCaracteristicasTitulos, "Busqueda")
                MyBase.ConfirmarBuscar()
                Busqueda = New CamposBusquedaCaracteristicasTitulos
                'cb = New CamposBusquedaCaracteristicasTitulos
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se encarga del manejo del evento de cancelacion en el formulario de busqueda.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 04/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Abril 04/2013 - Resultado Ok
    ''' </history> 
    Public Overrides Sub CancelarBuscar()
        Busqueda.Comitente = String.Empty
        Busqueda.Deposito = String.Empty
        Busqueda.Especie = String.Empty
        Busqueda.IdRecibo = 0
        Busqueda.Nombre = String.Empty
        Busqueda.NroTitulo = String.Empty

        MyBase.CancelarBuscar()
    End Sub

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se modifica para que ejecute el proceso de validaciones para la pantalla antes de la edicion.
    ''' Fecha            : Marzo 18/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 18/2013 - Resultado Ok
    ''' </history>    
    Public Overrides Sub ActualizarRegistro()
        Try
            dcProxy.ValidacionesCaracteristicas.Clear()
            If Not IsNothing(CaracteristicasTitulosSelected.IdCuentaDeceval) Then
                cuentaDeceval = CaracteristicasTitulosSelected.IdCuentaDeceval
            End If
            dcProxy.Load(dcProxy.ValidacionesCaracteristicasConsultarQuery("UBICACIONTITULO", True, CaracteristicasTitulosSelected.Fondo,
                                                                           cuentaDeceval, CaracteristicasTitulosSelected.Comitente,
                                                                           CaracteristicasTitulosSelected.ISIN,
                                                                           CaracteristicasTitulosSelected.IdEspecie,
                                                                           Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoDocumento, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    '''<remarks>    
    '''JCS Marzo 13/2013 Se cambia propiedad de visibilidad de ChildWindow.
    '''</remarks>
    ''' <history>
    ''' Modificado por   : Juan Carlos Soto.
    ''' Descripción      : Se agrega linea para cambiar propiedad de visibilidad de la ChildWindow de Liquidaciones cuando se termino de editar.
    ''' Fecha            : Marzo 13/2013
    ''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 13/2013 - Resultado Ok
    ''' </history>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If

            MyBase.TerminoSubmitChanges(So)

            Filtrar()

            'JCS Marzo 13/2013 Se cambia propiedad de visibilidad de ChildWindow.
            LiquidacionesClienteVisibilidad.LiquidacionesClienteVisible = Visibility.Collapsed
            'FIN JCS Marzo 13/2013 Se cambia propiedad de visibilidad de ChildWindow.

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz.
    ''' Descripción      : Se cambia el valor de las propiedades LiquidacionesClienteHabilitado y LiquidacionesClienteVisibilidad.
    '''                    se identifica el cambio en el codigo asi: JCS - Marzo 07/2013 - Adicion
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok  
    ''' </history> 
    ''' <history>
    ''' ID de cambio: CP0010, CP0011, CP0012, CP0013, CP0014
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        Agosto 13/2015
    ''' Pruebas CB:   Jorge Peña (Alcuadrado S.A.) - Agosto 13/2015 - OK
    ''' </history>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CaracteristicasTitulosSelected) Then
            Editando = True
            Editareg = True
            'JCS - Marzo 07/2013 - Adicion
            If (_CaracteristicasTitulosSelected.IDLiquidacion = 0 Or IsNothing(_CaracteristicasTitulosSelected)) Then
                LiquidacionesClienteHabilitado.HabilitarLiquidacionesCliente = True
                LiquidacionesClienteVisibilidad.LiquidacionesClienteVisible = Visibility.Visible
            End If
            'FIN JCS - Marzo 07/2013 - Adicion

            CaracteristicasTitulosSelected.strConceptoDesmaterializacion = String.Empty
            CaracteristicasTitulosSelected.dtmFechaDesmaterializacion = Now.Date
            HabilitarModalidad()
        End If
    End Sub


    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz.
    ''' Descripción      : Se cambia el valor de la propiedad LiquidacionesClienteVisible para que se oculte el boton de consulta de Liquidaciones Clientes.
    '''                    se identifica el cambio en el codigo asi: JCS - Marzo 07/2013 - Adicion
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok  
    ''' </history>    
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            CaracteristicasTitulosSelected.strConceptoDesmaterializacion = String.Empty
            CaracteristicasTitulosSelected.dtmFechaDesmaterializacion = Now.Date

            If Not IsNothing(_CaracteristicasTitulosSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                'JCS - Marzo 07/2013 - Adicion
                LiquidacionesClienteVisibilidad.LiquidacionesClienteVisible = Visibility.Collapsed
                'FIN JCS - Marzo 07/2013 - Adicion
                If _CaracteristicasTitulosSelected.EntityState = EntityState.Detached Then
                    CaracteristicasTitulosSelected = CaracteristicasTitulosAnterior
                    ComitenteDescripcion.Comitente = strcomitente
                    ComitenteDescripcion.Nombre = strnombre
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()

        A2Utilidades.Mensajes.mostrarMensaje("Esta Funcionalidad No esta Habilitada para este Formulario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'Try
        '    If Not IsNothing(_CustodiSelected) Then
        '        CustodiSelected.ConceptoAnulacion = "jjjjj"
        '        '  dcProxy.Custodias.Remove(_CustodiSelected)
        '        CustodiSelected = _ListaCustodia.LastOrDefault
        '        IsBusy = True
        '        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '    End If
        'Catch ex As Exception
        '    IsBusy = False
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.CaracteristicasTitulosSelected Is Nothing Then
                Select Case pstrTipoItem
                    Case "Comitente"

                        If Not IsNothing(CaracteristicasTitulosSelected.Comitente) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.CaracteristicasTitulosSelected.Comitente
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad01.BuscadorClientes.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(strIdItem, Program.Usuario, "IdComitenteLectura", Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If

                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la ciudad", Me.ToString(), "Buscar ciudad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Select Case strTipoItem
                    Case "Comitente"

                        Me.ComitenteDescripcion.Comitente = lo.Entities.ToList.Item(0).IdComitente
                        Me.ComitenteDescripcion.Nombre = lo.Entities.ToList.Item(0).Nombre
                        strcomitente = lo.Entities.ToList.Item(0).NombreCodigoOYD
                        strnombre = lo.Entities.ToList.Item(0).Nombre


                End Select

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("idliquidacion", 1)
        DicCamposTab.Add("parcial", 1)
    End Sub

    ''' <summary>
    ''' Funcion para levantar una ventana modal en donde se pueda seleccionar el numero de liquidacion.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok  
    ''' </history>
    Sub LiquidacionesCliente()
        Try
            LiquidacionesClienteSf = New LiquidacionesCliente(CaracteristicasTitulosSelected.Comitente, CaracteristicasTitulosSelected.IdEspecie)
            AddHandler LiquidacionesClienteSf.Closed, AddressOf CerroVentana
            Program.Modal_OwnerMainWindowsPrincipal(LiquidacionesClienteSf)
            LiquidacionesClienteSf.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el numero de liquidacion.", Me.ToString(), "LiquidacionesCliente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Funcion para manejar el cierre de la ventana modal de seleccion de numero de liquidacion.
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 07/2013
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok  
    ''' </history>
    Private Sub CerroVentana()
        Try
            If LiquidacionesClienteSf.DialogResult = True Then
                If (Not IsNothing(LiquidacionesClienteSf.ListaLiquidacionesClienteSelected)) Then
                    CaracteristicasTitulosSelected.IDLiquidacion = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Liquidacion()
                    CaracteristicasTitulosSelected.Liquidacion = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Fecha_Liquidacion()
                    CaracteristicasTitulosSelected.TotalLiq = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Total()
                    CaracteristicasTitulosSelected.Parcial = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Parcial()
                    CaracteristicasTitulosSelected.CumplimientoTitulo = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Fecha_Cumplimiento()
                    CaracteristicasTitulosSelected.Modalidad = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Modalidad()
                    CaracteristicasTitulosSelected.ClaseLiquidacion = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Clase()
                    CaracteristicasTitulosSelected.TasaDescuento = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Tasa_Real()
                    CaracteristicasTitulosSelected.IndicadorEconomico = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Indicador()
                    CaracteristicasTitulosSelected.TipoLiquidacion = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Tipo()
                    CaracteristicasTitulosSelected.TasaCompraVende = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Tasa_Facial()
                    CaracteristicasTitulosSelected.PuntosIndicador = LiquidacionesClienteSf.ListaLiquidacionesClienteSelected.Puntos()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana para seleccionar el numero de liquidacion.", Me.ToString(), "CerroVentana", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para consultar los parámetros para recargar y habilitar automáticamente el proceso de valoración.
    ''' </summary>
    Private Sub ConsultarParametros()
        Try
            dcProxyUtil.Verificaparametro("TRAZABILIDADTITULOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "TRAZABILIDADTITULOS")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "TRAZABILIDADTITULOS"
                    STRTRAZABILIDADTITULOS = lo.Value.ToString
                    If STRTRAZABILIDADTITULOS = "NO" Then
                        VisibilidadCamposDesmaterializacion = Visibility.Collapsed
                    Else
                        VisibilidadCamposDesmaterializacion = Visibility.Visible
                    End If
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region

#Region "Métodos para controlar cambio de campos asociados a buscadores"
    ''' <summary>
    ''' Actualizar el nemotécnico de la orden con los datos del nemotecnico recibido como parámetro
    ''' </summary>
    ''' <param name="pobjNemotecnico">Nemotécnico enviado como parámetro</param>
    Public Sub TraerCaracteristicasNemotecnico(ByVal pobjNemotecnico As OYDUtilidades.BuscadorEspecies)
        Try
            If Editando Then
                CaracteristicasTitulosSelected.ISIN = pobjNemotecnico.ISIN
                CaracteristicasTitulosSelected.IdEspecie = pobjNemotecnico.Nemotecnico
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las características del Nemotecnico.", Me.ToString(), "TraerCaracteristicasNemotecnico", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
#End Region

End Class


''' <summary>
''' Clase base para forma de búsquedas.
''' </summary>
''' <remarks></remarks>
''' <history>
''' Modificado por   : Juan Carlos Soto.
''' Descripción      : Se modifican los Display para las propiedades IdRecibo, NroTitulo, Comitente y Especie.
''' Fecha            : Marzo 04/2013
''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 04/2013 - Resultado Ok 
''' 
''' Modificado por   : Juan Carlos Soto.
''' Descripción      : Se adiciona la propiedad para el Nombre del Comitente.
'''                    se identifica asi: JCS Marzo 08/2013 Adicion.
''' Fecha            : Marzo 08/2013
''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 08/2013 - Resultado Ok 
''' 
''' Modificado por   : Juan Carlos Soto.
''' Descripción      : Se ajusta la propiedad NroTitulo para que notifique el cambio que se le indica en el evento CancelarBuscar().                    
''' Fecha            : Abril 04/2013
''' Pruebas Negocio  : Juan Carlos Soto Cruz - Abril 04/2013 - Resultado Ok 
''' </history>
Public Class CamposBusquedaCaracteristicasTitulos
    Implements INotifyPropertyChanged

    Private _IdRecibo As Integer
    <Display(Name:="Recibo Nro")> _
    Public Property IdRecibo As Integer
        Get
            Return _IdRecibo
        End Get
        Set(ByVal value As Integer)
            _IdRecibo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdRecibo"))
        End Set
    End Property

    Private _NroTitulo As String
    <Display(Name:="Titulo Nro")> _
    Public Property NroTitulo As String
        Get
            Return _NroTitulo
        End Get
        Set(ByVal value As String)
            _NroTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroTitulo"))
        End Set
    End Property

    Private _Comitente As String
    <Display(Name:="Cliente")> _
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    Private _Especie As String
    <Display(Name:="Especie")> _
    Public Property Especie As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    <Display(Name:="Deposito")> _
    Public Property Deposito As String

    'JCS Marzo 08/2013 Adicion.
    Private _Nombre As String
    <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")> _
    <Display(Name:="")> _
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property
    'FIN JCS Marzo 08/2013 Adicion.

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

''' <history>
''' Modificado por   : Juan Carlos Soto.
''' Descripción      : Se modifican los Display para las propiedades Comitente y Nombre.
''' Fecha            : Marzo 04/2013
''' Pruebas Negocio  : Juan Carlos Soto Cruz - Marzo 04/2013 - Resultado Ok 
''' </history>
Public Class ComitenteNombre
    Implements INotifyPropertyChanged

    Private _Comitente As String
    <Display(Name:="Cliente")> _
    Public Property Comitente As String
        Get
            Return _Comitente
        End Get
        Set(ByVal value As String)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property

    <Display(Name:=" ")> _
    Public Property Nombre As String


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

''' <summary>
''' Clase para manejar los cambios en las propiedades de habilitacion de controles.
''' </summary>
''' <remarks></remarks>
''' <history>
''' Creado por       : Juan Carlos Soto Cruz.
''' Descripción      : Creacion.
''' Fecha            : Marzo 07/2013
''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok   
''' </history>
Public Class LiquidacionesClienteHabilitado
    Implements INotifyPropertyChanged

    Private _HabilitarLiquidacionesCliente As Boolean = False
    Public Property HabilitarLiquidacionesCliente As Boolean
        Get
            Return _HabilitarLiquidacionesCliente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarLiquidacionesCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarLiquidacionesCliente"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

''' <summary>
''' Clase para manejar los cambios en las propiedades de visibilidad.
''' </summary>
''' <remarks></remarks>
''' <history>
''' Creado por       : Juan Carlos Soto Cruz.
''' Descripción      : Creacion.
''' Fecha            : Marzo 07/2013
''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 07/2013 - Resultado Ok   
''' </history>
Public Class LiquidacionesClienteVisibilidad
    Implements INotifyPropertyChanged

    Private _LiquidacionesClienteVisible As Visibility = Visibility.Collapsed
    Public Property LiquidacionesClienteVisible As Visibility
        Get
            Return _LiquidacionesClienteVisible
        End Get
        Set(value As Visibility)
            _LiquidacionesClienteVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LiquidacionesClienteVisible"))
        End Set
    End Property
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
