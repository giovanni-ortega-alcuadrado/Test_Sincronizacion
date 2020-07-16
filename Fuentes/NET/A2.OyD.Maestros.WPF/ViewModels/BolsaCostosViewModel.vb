Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: BolsaCostosViewModel.vb
'Generado el : 04/09/2012 13:57:25
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

Public Class BolsaCostosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBolsaCosto
    Private BolsaCostoPorDefecto As BolsaCosto
    Private BolsaCostoAnterior As BolsaCosto
    Private BolsasAnterior As Bolsa
    Private BolsasPorDefecto As Bolsa
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)


    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxy1 = New MaestrosDomainContext()
            Else
                dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerBolsaCostoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsaCostosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "BolsaCostosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerBolsaCostosPorDefecto_Completed(ByVal lo As LoadOperation(Of BolsaCosto))
        If Not lo.HasError Then
            BolsaCostoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la BolsaCosto por defecto", _
                                             Me.ToString(), "TerminoTraerBolsaCostoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBolsaCostos(ByVal lo As LoadOperation(Of BolsaCosto))
        If Not lo.HasError Then
            ListaBolsaCostos = dcProxy.BolsaCostos
            If dcProxy.BolsaCostos.Count > 0 Then
                If lo.UserState = "insert" Then
                    BolsaCostoSelected = ListaBolsaCostos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BolsaCostos", _
                                             Me.ToString(), "TerminoTraerBolsaCosto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerBolsas(ByVal lo As LoadOperation(Of Bolsa))
        If Not lo.HasError Then
            ListaBolsas = dcProxy.Bolsas
            If ListaBolsas.Count = 0 Then
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    BolsasSelected = Nothing
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Bolsas", Me.ToString, "TerminoTraerBolsas", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bolsas", _
            '                                 Me.ToString(), "TerminoTraerBolsas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    'Tablas padres

#End Region

#Region "Propiedades"

    Private _ListaBolsaCostos As EntitySet(Of BolsaCosto)
    Public Property ListaBolsaCostos() As EntitySet(Of BolsaCosto)
        Get
            Return _ListaBolsaCostos
        End Get
        Set(ByVal value As EntitySet(Of BolsaCosto))
            _ListaBolsaCostos = value

            MyBase.CambioItem("ListaBolsaCostos")
            MyBase.CambioItem("ListaBolsaCostosPaged")
            If Not IsNothing(value) Then
                If IsNothing(BolsaCostoAnterior) Then
                    BolsaCostoSelected = _ListaBolsaCostos.FirstOrDefault
                Else
                    BolsaCostoSelected = BolsaCostoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBolsaCostosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBolsaCostos) Then
                Dim view = New PagedCollectionView(_ListaBolsaCostos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BolsaCostoSelected As BolsaCosto
    Public Property BolsaCostoSelected() As BolsaCosto
        Get
            Return _BolsaCostoSelected
        End Get
        Set(ByVal value As BolsaCosto)
            _BolsaCostoSelected = value
            MyBase.CambioItem("BolsaCostoSelected")
        End Set
    End Property

    Private _ListaBolsas As EntitySet(Of Bolsa)
    Public Property ListaBolsas() As EntitySet(Of Bolsa)
        Get
            Return _ListaBolsas
        End Get
        Set(ByVal value As EntitySet(Of Bolsa))
            _ListaBolsas = value
            If Not IsNothing(value) Then
                If IsNothing(BolsasAnterior) Then
                    BolsasSelected = _ListaBolsas.FirstOrDefault
                Else
                    BolsasSelected = BolsasAnterior
                End If
            End If
            MyBase.CambioItem("ListaBolsas")
            MyBase.CambioItem("ListaBolsasPaged")
        End Set
    End Property

    Private _BolsasSelected As Bolsa
    Public Property BolsasSelected() As Bolsa
        Get
            Return _BolsasSelected
        End Get
        Set(ByVal value As Bolsa)
            _BolsasSelected = value
            If Not value Is Nothing Then
                dcProxy.BolsaCostos.Clear()
                dcProxy.Load(dcProxy.BolsaCostosFiltrarQuery(BolsasSelected.IdBolsa,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsaCostos, Nothing)
            Else
                dcProxy.BolsaCostos.Clear()
                ListaBolsaCostos = dcProxy.BolsaCostos
            End If
            MyBase.CambioItem("BolsasSelected")
        End Set
    End Property

    Public ReadOnly Property ListaBolsasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBolsas) Then
                Dim view = New PagedCollectionView(_ListaBolsas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _lista As New EntitySet(Of Bolsa)
    Public Property lista() As EntitySet(Of Bolsa)
        Get
            Return _lista
        End Get
        Set(ByVal value As EntitySet(Of Bolsa))
            _lista = value
            MyBase.CambioItem("lista")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero As Integer
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property

    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '          Dim NewBolsaCosto As New BolsaCosto
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            'NewBolsaCosto.IDBolsaCostos = BolsaCostoPorDefecto.IDBolsaCostos
            'NewBolsaCosto.IDComisionista = BolsaCostoPorDefecto.IDComisionista
            'NewBolsaCosto.IDSucComisionista = BolsaCostoPorDefecto.IDSucComisionista
            'NewBolsaCosto.Id = BolsaCostoPorDefecto.Id
            'NewBolsaCosto.Descripcion = BolsaCostoPorDefecto.Descripcion
            'NewBolsaCosto.PorcentajeCosto = BolsaCostoPorDefecto.PorcentajeCosto
            'NewBolsaCosto.CostoPesos = BolsaCostoPorDefecto.CostoPesos
            'NewBolsaCosto.Actualizado = BolsaCostoPorDefecto.Actualizado
            'NewBolsaCosto.Usuario = Program.Usuario
            'NewBolsaCosto.Actualizacion = BolsaCostoPorDefecto.Actualizacion
            '      BolsaCostoAnterior = BolsaCostoSelected
            '      BolsaCostoSelected = NewBolsaCosto
            '      MyBase.CambioItem("BolsaCostos")
            '      Editando = True
            '      MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            'dcProxy.BolsaCostos.Clear()
            'IsBusy = True
            'If FiltroVM.Length > 0 Then
            '    dcProxy.Load(dcProxy.BolsaCostosFiltrarQuery(FiltroVM,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsaCostos, Nothing)
            'Else
            '    dcProxy.Load(dcProxy.BolsaCostosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsaCostos, Nothing)
            'End If
            dcProxy.Bolsas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.BolsasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, Nothing)
            Else
                dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.Id = 0
        cb.Nombre = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Id <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Bolsas.Clear()
                BolsaCostoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.BolsasConsultarQuery(cb.Id, cb.Nombre, 0, 0, Nothing,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBolsaCosto
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
            If Not IsNothing(BolsaCostoSelected) Then
                For Each led In ListaBolsaCostos
                    If IsNothing(led.Descripcion) Or led.Descripcion = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("Existe un registro con el campo Descripción vacío y es requerido. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
                For Each lid In ListaBolsaCostos
                    If IsNothing(lid.PorcentajeCosto) Or lid.PorcentajeCosto = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Existe un registro con el campo valor(%) vacío y es requerido. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
                For Each lod In ListaBolsaCostos
                    If IsNothing(lod.CostoPesos) Or lod.CostoPesos = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Existe un registro con el campo valor($) vacío y es requerido. Por favor verifique", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
            End If
            Dim origen = "update"
            ErrorForma = ""
            BolsasAnterior = BolsasSelected
            If Not ListaBolsas.Contains(BolsasSelected) Then
                origen = "insert"
                ListaBolsas.Add(BolsasSelected)
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
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_BolsasSelected) Then
            Editando = True
            EditandoDetalle = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_BolsasSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditandoDetalle = True
                If BolsasSelected.EntityState = EntityState.Detached Then
                    BolsasSelected = BolsasAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
        dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "FiltroInicial")
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
        dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "FiltroInicial")
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            'If Not IsNothing(_BolsaCostoSelected) Then
            '    dcProxy.BolsaCostos.Remove(_BolsaCostoSelected)
            '   BolsaCostoSelected = _ListaBolsaCostos.LastOrDefault  'Dic202011  nueva
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            'End If
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Tablas Hijas"

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Dim NewBolsaCosto As New BolsaCosto
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewBolsaCosto.IDBolsaCostos = BolsaCostoPorDefecto.IDBolsaCostos
            NewBolsaCosto.Id = BolsasSelected.IdBolsa
            NewBolsaCosto.Descripcion = BolsaCostoPorDefecto.Descripcion
            NewBolsaCosto.PorcentajeCosto = BolsaCostoPorDefecto.PorcentajeCosto
            NewBolsaCosto.CostoPesos = BolsaCostoPorDefecto.CostoPesos
            'NewBolsaCosto.Actualizado = BolsaCostoPorDefecto.Actualizado
            NewBolsaCosto.Actualizado = False
            NewBolsaCosto.Usuario = Program.Usuario
            NewBolsaCosto.Actualizacion = BolsaCostoPorDefecto.Actualizacion
            BolsaCostoAnterior = BolsaCostoSelected
            BolsaCostoSelected = NewBolsaCosto
            ListaBolsaCostos.Add(NewBolsaCosto)
            MyBase.CambioItem("BolsaCostoSelected")
            MyBase.CambioItem("ListaBolsaCostos")
            MyBase.CambioItem("BolsaCostos")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            If Not IsNothing(ListaBolsaCostos) Then
                If ListaBolsaCostos.Count > 0 Then
                    Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(BolsaCostoSelected, ListaBolsaCostos.ToList)
                    ListaBolsaCostos.Remove(BolsaCostoSelected)
                    Program.PosicionarItemLista(BolsaCostoSelected, ListaBolsaCostos.ToList, intRegistroPosicionar)
                    MyBase.CambioItem("ListaBolsaCostos")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBolsaCosto
    Implements INotifyPropertyChanged

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




