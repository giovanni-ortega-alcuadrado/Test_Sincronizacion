Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: BancosNacionalesViewModel.vb
'Generado el : 03/07/2011 12:15:57
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web

Public Class BancosNacionalesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBancosNacionale
    Private BancosNacionalePorDefecto As BancosNacionale
    Private BancosNacionaleAnterior As BancosNacionale
    Private RelacionesCodBancoPorDefecto As RelacionesCodBanco

    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.BancosNacionalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionales, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerBancosNacionalePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionalesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  BancosNacionalesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "BancosNacionalesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerBancosNacionalesPorDefecto_Completed(ByVal lo As LoadOperation(Of BancosNacionale))
        If Not lo.HasError Then
            BancosNacionalePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la BancosNacionale por defecto", _
                                             Me.ToString(), "TerminoTraerBancosNacionalePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBancosNacionales(ByVal lo As LoadOperation(Of BancosNacionale))
        If Not lo.HasError Then
            ListaBancosNacionales = dcProxy.BancosNacionales
            If dcProxy.BancosNacionales.Count > 0 Then
                If lo.UserState = "insert" Then
                    'BancosNacionaleSelected = ListaBancosNacionales.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If ListaRelacionesCodBancos.Count > 0 Then
                        ListaRelacionesCodBancos.Clear()
                    End If
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de BancosNacionales", Me.ToString, "TerminoTraerBancosNacionale", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BancosNacionales", _
            '                                 Me.ToString(), "TerminoTraerBancosNacionale", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


    Private Sub TerminoTraerRelacionesCodBancos(ByVal lo As LoadOperation(Of RelacionesCodBanco))
        If Not lo.HasError Then
            ListaRelacionesCodBancos = dcProxy.RelacionesCodBancos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de RelacionesCodBancos", _
                                             Me.ToString(), "TerminoTraerRelacionesCodBancos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerRelacionesCodBancosPorDefecto_Completed(ByVal lo As LoadOperation(Of RelacionesCodBanco))
        If Not lo.HasError Then
            RelacionesCodBancoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las RelacionesCodBanco por defecto", _
                                             Me.ToString(), "TerminoTraerRelacionesCodBancosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.BancosNacionales.Clear()
                    dcProxy.Load(dcProxy.BancosNacionalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionales, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub
#End Region

#Region "Propiedades"

    Private _ListaBancosNacionales As EntitySet(Of BancosNacionale)
    Public Property ListaBancosNacionales() As EntitySet(Of BancosNacionale)
        Get
            Return _ListaBancosNacionales
        End Get
        Set(ByVal value As EntitySet(Of BancosNacionale))
            _ListaBancosNacionales = value

            MyBase.CambioItem("ListaBancosNacionales")
            MyBase.CambioItem("ListaBancosNacionalesPaged")
            If Not IsNothing(value) Then
                If IsNothing(BancosNacionaleAnterior) Then
                    BancosNacionaleSelected = _ListaBancosNacionales.FirstOrDefault
                Else
                    BancosNacionaleSelected = BancosNacionaleAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBancosNacionalesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBancosNacionales) Then
                Dim view = New PagedCollectionView(_ListaBancosNacionales)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BancosNacionaleSelected As BancosNacionale
    Public Property BancosNacionaleSelected() As BancosNacionale
        Get
            Return _BancosNacionaleSelected
        End Get
        Set(ByVal value As BancosNacionale)
            _BancosNacionaleSelected = value
            If Not value Is Nothing Then
                dcProxy.RelacionesCodBancos.Clear()
                dcProxy.Load(dcProxy.Traer_RelacionesCodBancos_BancosNacionaleQuery(BancosNacionaleSelected.Id,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRelacionesCodBancos, Nothing)
            End If
            MyBase.CambioItem("BancosNacionaleSelected")
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

    Private _HabilitarCodigo As Boolean
    Public Property HabilitarCodigo As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
        End Set
    End Property
#End Region


#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewBancosNacionale As New BancosNacionale
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewBancosNacionale.IDComisionista = BancosNacionalePorDefecto.IDComisionista
            NewBancosNacionale.IDSucComisionista = BancosNacionalePorDefecto.IDSucComisionista
            NewBancosNacionale.Id = 0
            NewBancosNacionale.CodACH = BancosNacionalePorDefecto.CodACH
            NewBancosNacionale.Nombre = BancosNacionalePorDefecto.Nombre
            NewBancosNacionale.Actualizacion = BancosNacionalePorDefecto.Actualizacion
            NewBancosNacionale.Usuario = Program.Usuario
            'NewBancosNacionale.IDBancoNacional = BancosNacionalePorDefecto.IDBancoNacional
            NewBancosNacionale.IDBancoNacional = -1
            BancosNacionaleAnterior = BancosNacionaleSelected
            BancosNacionaleSelected = NewBancosNacionale
            MyBase.CambioItem("BancosNacionales")
            Editando = True
            HabilitarCodigo = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.BancosNacionales.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.BancosNacionalesFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionales, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.BancosNacionalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionales, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.Id = -1
        cb.Nombre = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Id <> -1 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.BancosNacionales.Clear()
                BancosNacionaleAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Id = " &  cb.Id.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.BancosNacionalesConsultarQuery(cb.Id, cb.Nombre,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionales, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBancosNacionale
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()

        Dim numeroErrores = (From lr In ListaRelacionesCodBancos Where lr.HasValidationErrors = True).Count
        If numeroErrores <> 0 Then
            MessageBox.Show("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", "Alerta", MessageBoxButton.OK)
            Exit Sub
        End If

        Try
            If BancosNacionaleSelected.Id < 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Código es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If


            For Each led In ListaRelacionesCodBancos


                ' Se valida que la fecha de inicio pago del registro especifico no este repetida en otro de los registros
                Dim RelacionInicio = led.RelTecno
                Dim fechaInicioRepetida = From ld In ListaRelacionesCodBancos Where ld.RelTecno = RelacionInicio
                                          Select ld

                If fechaInicioRepetida.Count > 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Existen elementos duplicados en Relaciones Tecnológicas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            Next


            Dim origen = "update"
            ErrorForma = ""
            'BancosNacionaleAnterior = BancosNacionaleSelected
            If Not ListaBancosNacionales.Contains(BancosNacionaleSelected) Or BancosNacionaleSelected.Id = -1 Then
                origen = "insert"
                ListaBancosNacionales.Add(BancosNacionaleSelected)
                For Each detalle In ListaRelacionesCodBancos
                    BancosNacionaleSelected.RelacionesCodBancos.Add(detalle)
                Next
            End If
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
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaEmpleados.Remove(EmpleadoSelected)
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.BancosNacionales.Clear()
            '    dcProxy.Load(dcProxy.BancosNacionalesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacionales, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_BancosNacionaleSelected) Then
            Editando = True
            HabilitarCodigo = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_BancosNacionaleSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                HabilitarCodigo = False
                If _BancosNacionaleSelected.EntityState = EntityState.Detached Then
                    BancosNacionaleSelected = BancosNacionaleAnterior
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
            If Not IsNothing(_BancosNacionaleSelected) Then
                'dcProxy.BancosNacionales.Remove(_BancosNacionaleSelected)
                'BancosNacionaleSelected = _ListaBancosNacionales.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarBancosNacionales(BancosNacionaleSelected.IDBancoNacional, BancosNacionaleSelected.Usuario, String.Empty, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
    End Sub
#End Region

#Region "Tablas hijas"

    '******************************************************** RelacionesCodBancos 
    Private _ListaRelacionesCodBancos As EntitySet(Of RelacionesCodBanco)
    Public Property ListaRelacionesCodBancos() As EntitySet(Of RelacionesCodBanco)
        Get
            Return _ListaRelacionesCodBancos
        End Get
        Set(ByVal value As EntitySet(Of RelacionesCodBanco))
            _ListaRelacionesCodBancos = value
            MyBase.CambioItem("ListaRelacionesCodBancos")
            MyBase.CambioItem("ListaRelacionesCodBancosPaged")
        End Set
    End Property

    Public ReadOnly Property RelacionesCodBancosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaRelacionesCodBancos) Then
                Dim view = New PagedCollectionView(_ListaRelacionesCodBancos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _RelacionesCodBancoSelected As RelacionesCodBanco
    Public Property RelacionesCodBancoSelected() As RelacionesCodBanco
        Get
            Return _RelacionesCodBancoSelected
        End Get
        Set(ByVal value As RelacionesCodBanco)
            _RelacionesCodBancoSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("RelacionesCodBancoSelected")
            End If
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmRelacionesCodBanco"
                Dim NewRelacionesCodBanco As New RelacionesCodBanco

                NewRelacionesCodBanco.IdComisionista = BancosNacionaleSelected.IDComisionista
                NewRelacionesCodBanco.IdSucComisionista = BancosNacionaleSelected.IDSucComisionista
                NewRelacionesCodBanco.IdCodBanco = BancosNacionaleSelected.Id
                NewRelacionesCodBanco.RelTecno = "TR"
                NewRelacionesCodBanco.Actualizacion = DateTime.Now
                NewRelacionesCodBanco.Usuario = String.Empty
                NewRelacionesCodBanco.InfoSesion = String.Empty
                'NewRelacionesCodBanco.BancosNacionale = BancosNacionaleSelected
                NewRelacionesCodBanco.intIDRelacionCodBanco = -1
                ListaRelacionesCodBancos.Add(NewRelacionesCodBanco)
                RelacionesCodBancoSelected = NewRelacionesCodBanco
                MyBase.CambioItem("RelacionesCodBancoSelected")
                MyBase.CambioItem("ListaRelacionesCodBanco")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmRelacionesCodBanco"
                If Not IsNothing(ListaRelacionesCodBancos) Then
                    If Not IsNothing(ListaRelacionesCodBancos) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(RelacionesCodBancoSelected, ListaRelacionesCodBancos.ToList)
                        _RelacionesCodBancoSelected.RelTecno = 0
                        ListaRelacionesCodBancos.Remove(_RelacionesCodBancoSelected)
                        If ListaRelacionesCodBancos.Count > 0 Then
                            Program.PosicionarItemLista(RelacionesCodBancoSelected, ListaRelacionesCodBancos.ToList, intRegistroPosicionar)
                        Else
                            RelacionesCodBancoSelected = Nothing
                        End If
                        MyBase.CambioItem("RelacionesCodBancoSelected")
                        MyBase.CambioItem("ListaRelacionesCodBancos")
                    End If
                End If

        End Select
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBancosNacionale
    Implements INotifyPropertyChanged

    '<Display(Name:="Código")> _
    'Public Property Id As Integer

    Private _Id As Integer
    <Display(Name:="Código")> _
    Public Property Id As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Id"))
        End Set
    End Property

    '<Display(NAme:="Nombre")> _
    'Public Property Nombre As String

    Private _Nombre As String = String.Empty
    <Display(Name:="Nombre")> _
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




