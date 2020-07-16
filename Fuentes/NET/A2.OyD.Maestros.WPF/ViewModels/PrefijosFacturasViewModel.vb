Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: PrefijosFacturasViewModel.vb
'Generado el : 03/04/2011 15:46:43
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
'CFMA20172510
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions
Imports System.Collections
'CFMA20172510

Public Class PrefijosFacturasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaPrefijosFactura
    Private PrefijosFacturaPorDefecto As PrefijosFactura
    Private PrefijosFacturaAnterior As PrefijosFactura
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    'CFMA20172510
    Dim SEPARADORMAIL As String = ","
    Dim expresionemail As String = Program.ExpresionRegularEmail
    'CFMA20172510

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
                dcProxy.Load(dcProxy.PrefijosFacturasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPrefijosFacturas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerPrefijosFacturaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPrefijosFacturasPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  PrefijosFacturasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PrefijosFacturasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerPrefijosFacturasPorDefecto_Completed(ByVal lo As LoadOperation(Of PrefijosFactura))
        If Not lo.HasError Then
            PrefijosFacturaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la PrefijosFactura por defecto", _
                                             Me.ToString(), "TerminoTraerPrefijosFacturaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerPrefijosFacturas(ByVal lo As LoadOperation(Of PrefijosFactura))
        If Not lo.HasError Then
            ListaPrefijosFacturas = dcProxy.PrefijosFacturas
            If dcProxy.PrefijosFacturas.Count > 0 Then
                If lo.UserState = "insert" Then
                    PrefijosFacturaSelected = ListaPrefijosFacturas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de PrefijosFacturas", _
                                             Me.ToString(), "TerminoTraerPrefijosFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaPrefijosFacturas As EntitySet(Of PrefijosFactura)
    Public Property ListaPrefijosFacturas() As EntitySet(Of PrefijosFactura)
        Get
            Return _ListaPrefijosFacturas
        End Get
        Set(ByVal value As EntitySet(Of PrefijosFactura))
            _ListaPrefijosFacturas = value

            MyBase.CambioItem("ListaPrefijosFacturas")
            MyBase.CambioItem("ListaPrefijosFacturasPaged")
            If Not IsNothing(value) Then
                If IsNothing(PrefijosFacturaAnterior) Then
                    PrefijosFacturaSelected = _ListaPrefijosFacturas.FirstOrDefault
                Else
                    PrefijosFacturaSelected = PrefijosFacturaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaPrefijosFacturasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPrefijosFacturas) Then
                Dim view = New PagedCollectionView(_ListaPrefijosFacturas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _PrefijosFacturaSelected As PrefijosFactura
    Public Property PrefijosFacturaSelected() As PrefijosFactura
        Get
            Return _PrefijosFacturaSelected
        End Get
        Set(ByVal value As PrefijosFactura)
            _PrefijosFacturaSelected = value
            MyBase.CambioItem("PrefijosFacturaSelected")
        End Set
    End Property

    Private _EditaReg As Boolean
    Public Property EditaReg() As Boolean
        Get
            Return _EditaReg
        End Get
        Set(ByVal value As Boolean)

            _EditaReg = value
            MyBase.CambioItem("EditaReg")
        End Set
    End Property
    Private _Enabled As Boolean
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            If EditaReg = False Then
                value = False
            Else
                value = True
            End If
            _Enabled = value
            MyBase.CambioItem("Enabled")
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


    Private _LimpiarTexto As String
    Public Property LimpiarTexto() As String
        Get
            Return _LimpiarTexto
        End Get
        Set(ByVal value As String)
            _LimpiarTexto = value
            MyBase.CambioItem("LimpiarTexto")
        End Set
    End Property

    'CFMA20172510
    Public Function IsValidmail(emailaddress As String) As Boolean
        Try
            'Dim match As Match = regex.Match(emailaddress, "^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+.)+[A-Z]{2,4}$")
            'If match.Success Then
            '    Return True
            'Else
            '    Return False
            'End If
            'Return Regex.IsMatch(emailaddress, "^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
            Return Regex.IsMatch(emailaddress, expresionemail)
        Catch generatedExceptionName As FormatException
            Return False
        End Try
    End Function



#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewPrefijosFactura As New PrefijosFactura
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewPrefijosFactura.IDComisionista = PrefijosFacturaPorDefecto.IDComisionista
            NewPrefijosFactura.IDSucComisionista = PrefijosFacturaPorDefecto.IDSucComisionista
            NewPrefijosFactura.Prefijo = PrefijosFacturaPorDefecto.Prefijo
            NewPrefijosFactura.NombreConsecutivo = PrefijosFacturaPorDefecto.NombreConsecutivo
            NewPrefijosFactura.Descripcion = PrefijosFacturaPorDefecto.Descripcion
            NewPrefijosFactura.Tipo = PrefijosFacturaPorDefecto.Tipo
            NewPrefijosFactura.NombreCuenta = PrefijosFacturaPorDefecto.NombreCuenta
            NewPrefijosFactura.TextoResolucion = PrefijosFacturaPorDefecto.TextoResolucion
            NewPrefijosFactura.IntervaloRes = PrefijosFacturaPorDefecto.IntervaloRes
            NewPrefijosFactura.ResponsabilidadIVA = PrefijosFacturaPorDefecto.ResponsabilidadIVA
            NewPrefijosFactura.Actualizacion = PrefijosFacturaPorDefecto.Actualizacion
            NewPrefijosFactura.Usuario = Program.Usuario
            NewPrefijosFactura.FechaVencimiento = PrefijosFacturaPorDefecto.FechaVencimiento
            NewPrefijosFactura.Alarma = PrefijosFacturaPorDefecto.Alarma
            ' NewPrefijosFactura.IDCodigoResolucion = Nothing
            NewPrefijosFactura.IDCodigoResolucion = PrefijosFacturaPorDefecto.IDCodigoResolucion
            NewPrefijosFactura.IDPrefijoFacturas = PrefijosFacturaPorDefecto.IDPrefijoFacturas
            NewPrefijosFactura.SucursalAplica = PrefijosFacturaPorDefecto.SucursalAplica
            NewPrefijosFactura.Resolucion = PrefijosFacturaPorDefecto.Resolucion
            NewPrefijosFactura.AnoRes = PrefijosFacturaPorDefecto.AnoRes
            'CFMA20172510
            NewPrefijosFactura.Vigiladopor = PrefijosFacturaPorDefecto.Vigiladopor
            NewPrefijosFactura.FechaDesde = PrefijosFacturaPorDefecto.FechaDesde
            NewPrefijosFactura.FechaHasta = PrefijosFacturaPorDefecto.FechaHasta
            NewPrefijosFactura.numDiasPreviosNoti = PrefijosFacturaPorDefecto.numDiasPreviosNoti
            NewPrefijosFactura.numCantConsPrevNoti = PrefijosFacturaPorDefecto.numCantConsPrevNoti
            NewPrefijosFactura.numDiasPeriodicidadNoti = PrefijosFacturaPorDefecto.numDiasPeriodicidadNoti
            NewPrefijosFactura.DestinatariosNoti = PrefijosFacturaPorDefecto.DestinatariosNoti
            NewPrefijosFactura.FechaUltNoti = PrefijosFacturaPorDefecto.FechaUltNoti
            'CFMA20172510
            PrefijosFacturaAnterior = PrefijosFacturaSelected
            PrefijosFacturaSelected = NewPrefijosFactura
            MyBase.CambioItem("PrefijosFacturas")
            LimpiarTexto = String.Empty
            Editando = True
            EditaReg = True
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
            dcProxy.PrefijosFacturas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.PrefijosFacturasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPrefijosFacturas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.PrefijosFacturasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPrefijosFacturas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Prefijo <> String.Empty Or cb.Descripcion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.PrefijosFacturas.Clear()
                PrefijosFacturaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Prefijo = " &  cb.Prefijo.ToString() & " Descripcion = " &  cb.Descripcion.ToString() 
                dcProxy.Load(dcProxy.PrefijosFacturasConsultarQuery(cb.Prefijo, cb.Descripcion,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPrefijosFacturas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaPrefijosFactura
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
            'If PrefijosFacturaSelected.SucursalAplica <= 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una Sucursal", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            If PrefijosFacturaSelected.IDCodigoResolucion <= 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar una Resolución", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If PrefijosFacturaSelected.FechaVencimiento < Date.Now() Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha vencimiento debe ser mayor a la fecha actual: " + Date.Now.ToString("MMM dd, yyyy") + "", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            'CFMA20172510
            If PrefijosFacturaSelected.FechaDesde Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha desde no puede ser vacía, por favor insertar la fecha: " + Date.Now.ToString("MMM dd, yyyy") + "", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If PrefijosFacturaSelected.FechaDesde > PrefijosFacturaSelected.FechaHasta Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha Desde debe ser menor que la fecha Hasta: " + Date.Now.ToString("MMM dd, yyyy") + "", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If PrefijosFacturaSelected.FechaHasta Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha hasta no puede ser vacía, por favor insertar la fecha: " + Date.Now.ToString("MMM dd, yyyy") + "", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If PrefijosFacturaSelected.FechaHasta < PrefijosFacturaSelected.FechaDesde Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha Hasta no debe ser menor que la fecha desde: " + Date.Now.ToString("MMM dd, yyyy") + "", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            If Not String.IsNullOrEmpty(PrefijosFacturaSelected.DestinatariosNoti) Then
                Dim emails = PrefijosFacturaSelected.DestinatariosNoti.Split(SEPARADORMAIL.Trim)
                For Each li In emails
                    If Not IsValidmail(li) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El e-mail " & li & " no es válido, inserta dirección de correo valida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
            End If


            'CFMA20172510



            Dim origen = "update"
            ErrorForma = ""
            PrefijosFacturaAnterior = PrefijosFacturaSelected
            If Not ListaPrefijosFacturas.Contains(PrefijosFacturaSelected) Then
                origen = "insert"
                ListaPrefijosFacturas.Add(PrefijosFacturaSelected)
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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaPrefijosFacturas.Remove(PrefijosFacturaSelected)
                    End If
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
            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.PrefijosFacturas.Clear()
                dcProxy.Load(dcProxy.PrefijosFacturasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPrefijosFacturas, "insert") ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_PrefijosFacturaSelected) Then
            Editando = True
            EditaReg = False
            Enabled = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_PrefijosFacturaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                Enabled = False
                If _PrefijosFacturaSelected.EntityState = EntityState.Detached Then
                    PrefijosFacturaSelected = PrefijosFacturaAnterior
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
            If Not IsNothing(_PrefijosFacturaSelected) Then
                dcProxy.PrefijosFacturas.Remove(_PrefijosFacturaSelected)
                PrefijosFacturaSelected = _ListaPrefijosFacturas.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
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
        DicCamposTab.Add("Tipo", 1)
        DicCamposTab.Add("Prefijo", 1)
        DicCamposTab.Add("Descripcion", 1)
        DicCamposTab.Add("NombreConsecutivo", 1)
    End Sub
    Public Overrides Sub Buscar()
        cb.Descripcion = String.Empty
        cb.Prefijo = String.Empty
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaPrefijosFactura
    Implements INotifyPropertyChanged
    Private _Prefijo As String
    <StringLength(5, ErrorMessage:="La longitud máxima es de 5")> _
     <Display(Name:="Prefijo")> _
    Public Property Prefijo As String
        Get
            Return _Prefijo
        End Get
        Set(ByVal value As String)
            _Prefijo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Prefijo"))
        End Set
    End Property
    Private _Descripcion As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Descripción")> _
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class



