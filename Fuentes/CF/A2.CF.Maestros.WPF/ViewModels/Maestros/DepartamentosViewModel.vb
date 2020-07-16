Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: DepartamentosViewModel.vb
'Generado el : 04/12/2011 16:59:11
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
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros

Public Class DepartamentosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaDepartamento
    Private DepartamentoPorDefecto As Departamento
    Private DepartamentoAnterior As Departamento
    Dim dcProxy As MaestrosCFDomainContext
    Dim dcProxy1 As MaestrosCFDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosCFDomainContext()
                dcProxy1 = New MaestrosCFDomainContext()
            Else
                dcProxy = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
                dcProxy1 = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.DepartamentosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, "")
                dcProxy1.Load(dcProxy1.TraerDepartamentoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  DepartamentosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "DepartamentosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerDepartamentosPorDefecto_Completed(ByVal lo As LoadOperation(Of Departamento))
        If Not lo.HasError Then
            DepartamentoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Departamento por defecto", _
                                             Me.ToString(), "TerminoTraerDepartamentoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDepartamentos(ByVal lo As LoadOperation(Of Departamento))
        If Not lo.HasError Then
            ListaDepartamentos = dcProxy.Departamentos
            If dcProxy.Departamentos.Count > 0 Then
                If lo.UserState = "insert" Then
                    DepartamentoSelected = ListaDepartamentos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Departamentos", _
                                             Me.ToString(), "TerminoTraerDepartamento", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaDepartamentos As EntitySet(Of Departamento)
    Public Property ListaDepartamentos() As EntitySet(Of Departamento)
        Get
            Return _ListaDepartamentos
        End Get
        Set(ByVal value As EntitySet(Of Departamento))
            _ListaDepartamentos = value

            MyBase.CambioItem("ListaDepartamentos")
            MyBase.CambioItem("ListaDepartamentosPaged")
            If Not IsNothing(value) Then
                If IsNothing(DepartamentoAnterior) Then
                    DepartamentoSelected = _ListaDepartamentos.FirstOrDefault
                Else
                    DepartamentoSelected = DepartamentoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDepartamentosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDepartamentos) Then
                Dim view = New PagedCollectionView(_ListaDepartamentos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DepartamentoSelected As Departamento
    Public Property DepartamentoSelected() As Departamento
        Get
            Return _DepartamentoSelected
        End Get
        Set(ByVal value As Departamento)
            _DepartamentoSelected = value
            MyBase.CambioItem("DepartamentoSelected")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewDepartamento As New Departamento
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewDepartamento.IDComisionista = DepartamentoPorDefecto.IDComisionista
            NewDepartamento.IDSucComisionista = DepartamentoPorDefecto.IDSucComisionista
            NewDepartamento.IDPais = DepartamentoPorDefecto.IDPais
            NewDepartamento.ID = DepartamentoPorDefecto.ID
            NewDepartamento.Nombre = DepartamentoPorDefecto.Nombre
            NewDepartamento.Codigo_DaneDEPTO = DepartamentoPorDefecto.Codigo_DaneDEPTO
            NewDepartamento.Actualizacion = DepartamentoPorDefecto.Actualizacion
            NewDepartamento.Usuario = Program.Usuario
            NewDepartamento.IDDepartamento = DepartamentoPorDefecto.IDDepartamento
            DepartamentoAnterior = DepartamentoSelected
            DepartamentoSelected = NewDepartamento
            MyBase.CambioItem("Departamentos")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Departamentos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.DepartamentosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, Nothing)
            Else
                dcProxy.Load(dcProxy.DepartamentosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Departamentos.Clear()
                DepartamentoAnterior = Nothing
                IsBusy = True
                Dim DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " Nombre = " & cb.Nombre.ToString()
                dcProxy.Load(dcProxy.DepartamentosConsultarQuery(cb.ID, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaDepartamento
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
            DepartamentoAnterior = DepartamentoSelected
            If Not ListaDepartamentos.Contains(DepartamentoSelected) Then
                origen = "insert"
                ListaDepartamentos.Add(DepartamentoSelected)
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
        If Not IsNothing(_DepartamentoSelected) Then
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_DepartamentoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _DepartamentoSelected.EntityState = EntityState.Detached Then
                    DepartamentoSelected = DepartamentoAnterior
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
            If Not IsNothing(_DepartamentoSelected) Then
                dcProxy.Departamentos.Remove(_DepartamentoSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaDepartamento

    <Display(Name:="ID", Description:="ID")> _
    Public Property ID As Integer

    <StringLength(40, ErrorMessage:="La longitud máxima es de 40")> _
     <Display(Name:="Nombre", Description:="Nombre")> _
    Public Property Nombre As String
End Class




