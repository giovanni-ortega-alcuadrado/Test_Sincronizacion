Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.OyDBolsa

Public Class RelacionEspeciesLocalesViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private dcProxy As BolsaDomainContext
    Dim dcProxy1 As BolsaDomainContext
    Private objProxy As BolsaDomainContext
    Dim selected As Boolean
    Public Property cb As New CamposBusquedaEspeciesLocales

#Region "Propiedades"
    Private _ListaEspecies As EntitySet(Of EspeciesRelacionLocalExterior)
    Public Property ListaEspecies() As EntitySet(Of EspeciesRelacionLocalExterior)
        Get
            Return _ListaEspecies
        End Get
        Set(ByVal value As EntitySet(Of EspeciesRelacionLocalExterior))
            _ListaEspecies = value
            MyBase.CambioItem("ListaEspecies")
            MyBase.CambioItem("ListaEspeciesPaged")
        End Set
    End Property

    Private _EspecieSelected As EspeciesRelacionLocalExterior
    Public Property EspecieSelected() As EspeciesRelacionLocalExterior
        Get
            Return _EspecieSelected
        End Get
        Set(ByVal value As EspeciesRelacionLocalExterior)
            _EspecieSelected = value
            MyBase.CambioItem("EspecieSelected")
        End Set
    End Property

    Private _EspecieAnterior As EspeciesRelacionLocalExterior
    Public Property EspecieAnterior() As EspeciesRelacionLocalExterior
        Get
            Return _EspecieAnterior
        End Get
        Set(ByVal value As EspeciesRelacionLocalExterior)
            _EspecieAnterior = value
            MyBase.CambioItem("EspecieAnterior")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspecies) Then
                Dim view = New PagedCollectionView(_ListaEspecies)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

#Region "Inicio"
    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New BolsaDomainContext()
            dcProxy1 = New BolsaDomainContext()
            objProxy = New BolsaDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            mdcProxyUtilidad02 = New UtilidadesDomainContext()
        Else
            dcProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New BolsaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.EspeciesRelacionLocalExteriorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "Inicial")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "RelacionEspeciesLocalesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Metodos"
    Public Overrides Sub Filtrar()
        Try
            selected = True
            dcProxy.EspeciesRelacionLocalExteriors.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                dcProxy.Load(dcProxy.EspeciesRelacionLocalExteriorFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.EspeciesRelacionLocalExteriorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Dim NuevaRelacion As New EspeciesRelacionLocalExterior
            EspecieAnterior = EspecieSelected
            EspecieSelected = NuevaRelacion
            Editando = True
            MyBase.CambioItem("Editando")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            selected = True
            If cb.IdEspecie <> String.Empty Or cb.IdEspecieExterior <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.EspeciesRelacionLocalExteriors.Clear()
                dcProxy.Load(dcProxy.ConsultarEspeciesRelacionLocalExteriorQuery(cb.IdEspecie, cb.IdEspecieExterior, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "Busqueda")
                'IsBusy = True
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " IDComitente = " & cb.IDComitente.ToString()
                'dcProxy.Load(dcProxy.TesoreriaConsultarQuery(Filtro, CamposBusquedaTesoreria.Tipo, CamposBusquedaTesoreria.NombreConsecutivo, CamposBusquedaTesoreria.IDDocumento, IIf(CamposBusquedaTesoreria.Documento.Year < 1900, Nothing, CamposBusquedaTesoreria.Documento), estadoMC,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTesoreria, "Busqueda")
                'dcProxy.Load(dcProxy.LiquidacionesConsultarQuery(cb.ID, cb.IDComitente, cb.Tipo, cb.ClaseOrden, IIf(cb.Liquidacion.Date.Year < 1800, "1799/01/01", cb.Liquidacion), IIf(cb.CumplimientoTitulo.Date.Year < 1800, "1799/01/01", cb.CumplimientoTitulo), cb.IDOrden,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidaciones, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaEspeciesLocales
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
            Dim origen = "update"
            ErrorForma = ""
            'EspecieAnterior = EspecieSelected

            If Not ListaEspecies.Contains(EspecieSelected) Then
                origen = "insert"
                ListaEspecies.Add(EspecieSelected)
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

    ''' <summary>
    ''' Metodo encargado de lanzar la edicion del formulario.
    ''' </summary>
    ''' <remarks>        
    ''' Modificado por:	Juan Carlos Soto Cruz.
    ''' Descripción:    Se inicializan algunas propiedades de la liquidacion seleccionada de manera provisional para poder realizar los metodos CRUD de la pestaña
    '''                 Receptores. Estos se deben quitar una vez se realicen los metodos CRUD para la liquidacion. Se encuentran entre las etiquetas Jsoto 2011-08-10 y Fin Jsoto 2011-08-10 
    ''' Fecha:			Agosto 10/2011        
    ''' </remarks>
    Public Overrides Sub EditarRegistro()
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        If Not IsNothing(_EspecieSelected) Then
            'Fin Jsoto 2011-08-10
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_EspecieSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _EspecieSelected.EntityState = EntityState.Detached Then
                    EspecieSelected = EspecieAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Try
            selected = True
            If Not IsNothing(EspecieSelected) Then
                dcProxy.EspeciesRelacionLocalExteriors.Remove(EspecieSelected)
                selected = True
                EspecieSelected = ListaEspecies.LastOrDefault
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

    Public Overrides Sub Buscar()
        'cb.Liquidacion = Nothing
        'cb.CumplimientoTitulo = Nothing
        'cb.DisplayDate = Date.Now
        'cb.DisplayDate2 = Date.Now
        MyBase.Buscar()
        cb.IdEspecie = Nothing
        cb.IdEspecieExterior = Nothing
        'MessageBox.Show("Esta funcionalidad no está disponible.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

#End Region

#Region "Asincronico"
    Private Sub TerminoTraerEspecies(ByVal lo As LoadOperation(Of EspeciesRelacionLocalExterior))
        If Not lo.HasError Then
            ListaEspecies = dcProxy.EspeciesRelacionLocalExteriors
            If ListaEspecies.Count <= 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las Especies", _
                                             Me.ToString(), "TerminoTraerLiquidacionePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
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
                Dim strMsg As String = String.Empty
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" or So.UserState = "BorrarRegistro"  then
            If So.UserState = "insert" Then
                dcProxy.EspeciesRelacionLocalExteriors.Clear()
                dcProxy.Load(dcProxy.EspeciesRelacionLocalExteriorFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, So.UserState) ' Recarga la lista para que carguen los include
            End If
            If So.UserState = "BorrarRegistro" Then
                ListaEspecies = _ListaEspecies
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

Public Class CamposBusquedaEspeciesLocales
    Implements INotifyPropertyChanged

    Private _ID As Integer
    <Display(Name:="ID")> _
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _IdEspecie As String
    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
     <Display(Name:="Id Especie", Description:="Id Especie")> _
    Public Property IdEspecie() As String
        Get
            Return _IdEspecie
        End Get
        Set(ByVal value As String)
            _IdEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdEspecie"))
        End Set
    End Property

    Private _IdEspecieExterior As String
    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
     <Display(Name:="Id Especie Exterior", Description:="Id Especie Exterior")> _
    Public Property IdEspecieExterior() As String
        Get
            Return _IdEspecieExterior
        End Get
        Set(ByVal value As String)
            _IdEspecieExterior = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdEspecieExterior"))
        End Set
    End Property

    Private _NitProgramaADR As String
    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
     <Display(Name:="Nit Programa ADR", Description:="Nit Programa ADR")> _
    Public Property NitProgramaADR() As String
        Get
            Return _NitProgramaADR
        End Get
        Set(ByVal value As String)
            _NitProgramaADR = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NitProgramaADR"))
        End Set
    End Property

    Private _FactorEspecie As Integer
    <Display(Name:="FactorEspecie")> _
    Public Property FactorEspecie() As Integer
        Get
            Return _FactorEspecie
        End Get
        Set(ByVal value As Integer)
            _FactorEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FactorEspecie"))
        End Set
    End Property

    Private _FactorEspecieExterior As Integer
    <Display(Name:="FactorEspecieExterior")> _
    Public Property FactorEspecieExterior() As Integer
        Get
            Return _FactorEspecieExterior
        End Get
        Set(ByVal value As Integer)
            _FactorEspecieExterior = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FactorEspecieExterior"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
