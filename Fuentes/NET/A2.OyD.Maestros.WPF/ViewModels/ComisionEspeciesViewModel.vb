Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ComisionEspeciesViewModel.vb
'Generado el : 06/13/2012 16:10:41
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

Public Class ComisionEspeciesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaComisionEspecie
    Private ComisionEspeciePorDefecto As ComisionEspecie
    Private ComisionEspecieAnterior As ComisionEspecie
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext


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
                dcProxy.Load(dcProxy.ComisionEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionEspecies, "")
                dcProxy1.Load(dcProxy1.TraerComisionEspeciePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionEspeciesPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ComisionEspeciesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ComisionEspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerComisionEspeciesPorDefecto_Completed(ByVal lo As LoadOperation(Of ComisionEspecie))
        If Not lo.HasError Then
            ComisionEspeciePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ComisionEspecie por defecto",
                                             Me.ToString(), "TerminoTraerComisionEspeciePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerComisionEspecies(ByVal lo As LoadOperation(Of ComisionEspecie))
        If Not lo.HasError Then
            ListaComisionEspecies = dcProxy.ComisionEspecies
            If dcProxy.ComisionEspecies.Count > 0 Then
                If lo.UserState = "insert" Then
                    ComisionEspecieSelected = ListaComisionEspecies.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ComisionEspecies",
                                             Me.ToString(), "TerminoTraerComisionEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaComisionEspecies As EntitySet(Of ComisionEspecie)
    Public Property ListaComisionEspecies() As EntitySet(Of ComisionEspecie)
        Get
            Return _ListaComisionEspecies
        End Get
        Set(ByVal value As EntitySet(Of ComisionEspecie))
            _ListaComisionEspecies = value

            MyBase.CambioItem("ListaComisionEspecies")
            MyBase.CambioItem("ListaComisionEspeciesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ComisionEspecieAnterior) Then
                    ComisionEspecieSelected = _ListaComisionEspecies.FirstOrDefault
                Else
                    ComisionEspecieSelected = ComisionEspecieAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaComisionEspeciesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaComisionEspecies) Then
                Dim view = New PagedCollectionView(_ListaComisionEspecies)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ComisionEspecieSelected As ComisionEspecie
    Public Property ComisionEspecieSelected() As ComisionEspecie
        Get
            Return _ComisionEspecieSelected
        End Get
        Set(ByVal value As ComisionEspecie)
            _ComisionEspecieSelected = value
            MyBase.CambioItem("ComisionEspecieSelected")
        End Set
    End Property

    Private _Nemotecnico As String
    Public Property Nemotecnico() As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            ComisionEspecieSelected.IDEspecie = value
            MyBase.CambioItem("Nemotecnico")
            MyBase.CambioItem("ComisionEspecieSelected")
        End Set
    End Property


    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            MyBase.CambioItem("Especie")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewComisionEspecie As New ComisionEspecie
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewComisionEspecie.ID = ComisionEspeciePorDefecto.ID
            NewComisionEspecie.IDEspecie = ComisionEspeciePorDefecto.IDEspecie
            NewComisionEspecie.Comision = ComisionEspeciePorDefecto.Comision
            NewComisionEspecie.PorcentajeComision = ComisionEspeciePorDefecto.PorcentajeComision
            NewComisionEspecie.Usuario = Program.Usuario
            NewComisionEspecie.Actualizacion = ComisionEspeciePorDefecto.Actualizacion
            ComisionEspecieAnterior = ComisionEspecieSelected
            ComisionEspecieSelected = NewComisionEspecie
            MyBase.CambioItem("ComisionEspecies")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ComisionEspecies.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ComisionEspeciesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionEspecies, Nothing)
            Else
                dcProxy.Load(dcProxy.ComisionEspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionEspecies, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDEspecie <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ComisionEspecies.Clear()
                ComisionEspecieAnterior = Nothing
                IsBusy = True
                DescripcionFiltroVM = " IDEspecie = " & cb.IDEspecie.ToString()
                dcProxy.Load(dcProxy.ComisionEspeciesConsultarQuery(cb.IDEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionEspecies, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaComisionEspecie
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()

        Try
            If ValidarIngreso() Then

                Dim CantRegistros As Integer = (From Lista In ListaComisionEspecies
                                                Where LCase(Lista.IDEspecie) = LCase(ComisionEspecieSelected.IDEspecie)
                                                Select Lista).Count()

                Dim origen = "update"
                ErrorForma = ""

                If Not ListaComisionEspecies.Contains(ComisionEspecieSelected) Then
                    origen = "insert"
                End If

                If ((CantRegistros > 1 And origen = "update") Or (CantRegistros > 0 And origen = "insert")) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La especie ya tiene una comisión configurada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    If (origen = "insert") Then
                        ListaComisionEspecies.Add(ComisionEspecieSelected)
                    End If
                    IsBusy = True
                    ComisionEspecieAnterior = ComisionEspecieSelected
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ComisionEspecieSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ComisionEspecieSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ComisionEspecieSelected.EntityState = EntityState.Detached Then
                    ComisionEspecieSelected = ComisionEspecieAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        cb = New CamposBusquedaComisionEspecie
        CambioItem("cb")
        MyBase.CancelarBuscar()
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ComisionEspecieSelected) Then
                dcProxy.ComisionEspecies.Remove(_ComisionEspecieSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                ComisionEspecieSelected = ListaComisionEspecies.LastOrDefault
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function ValidarIngreso() As Boolean

        Dim retorno As Boolean = True

        Try

            If ComisionEspecieSelected.Comision = 0 And ComisionEspecieSelected.PorcentajeComision = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar al menos una de las comisiones para la Especie", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                retorno = False
            End If

            Return retorno
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el ingreso",
             Me.ToString(), "ValidarIngreso", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try

    End Function

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaComisionEspecie
    Implements INotifyPropertyChanged

    Private _IDEspecie As String
    <Display(Name:="Id Especie", Description:="Id Especie")>
    Public Property IDEspecie As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecie"))
        End Set
    End Property


    Private _NombreEspecie As String
    <Display(Name:="Especie", Description:="Especie")>
    Public Property NombreEspecie As String
        Get
            Return _NombreEspecie
        End Get
        Set(ByVal value As String)
            _NombreEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class


