Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: SaldarPagosDECEVALViewModel.vb
'Generado el : 03/22/2012 16:36:27
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
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio

Public Class SaldarPagosDECEVALViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaSaldarPagosDECEVA
    Private SaldarPagosDECEVAPorDefecto As SaldarPagosDECEVA
    Private SaldarPagosDECEVAAnterior As SaldarPagosDECEVA
    Dim dcProxy As New PortafolioDomainContext
    Dim dcProxy1 As New PortafolioDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New PortafolioDomainContext()
                dcProxy1 = New PortafolioDomainContext()
            Else
                dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                dcProxy1 = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.SaldarPagosDECEVALFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSaldarPagosDECEVAL, "")
                dcProxy1.Load(dcProxy1.TraerSaldarPagosDECEVAPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSaldarPagosDECEVALPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  SaldarPagosDECEVALViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "SaldarPagosDECEVALViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerSaldarPagosDECEVALPorDefecto_Completed(ByVal lo As LoadOperation(Of SaldarPagosDECEVA))
        If Not lo.HasError Then
            SaldarPagosDECEVAPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la SaldarPagosDECEVA por defecto", _
                                             Me.ToString(), "TerminoTraerSaldarPagosDECEVAPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerSaldarPagosDECEVAL(ByVal lo As LoadOperation(Of SaldarPagosDECEVA))
        If Not lo.HasError Then
            ListaSaldarPagosDECEVAL = dcProxy.SaldarPagosDECEVAs
            If dcProxy.SaldarPagosDECEVAs.Count > 0 Then
                If lo.UserState = "insert" Then
                    SaldarPagosDECEVASelected = ListaSaldarPagosDECEVAL.Last
                End If
            Else
                If lo.UserState = "Busqueda" Then
                    MessageBox.Show("No se encontro ningún registro")
                    MyBase.Buscar()
                    MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de SaldarPagosDECEVAL", _
                                             Me.ToString(), "TerminoTraerSaldarPagosDECEVA", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaSaldarPagosDECEVAL As EntitySet(Of SaldarPagosDECEVA)
    Public Property ListaSaldarPagosDECEVAL() As EntitySet(Of SaldarPagosDECEVA)
        Get
            Return _ListaSaldarPagosDECEVAL
        End Get
        Set(ByVal value As EntitySet(Of SaldarPagosDECEVA))
            _ListaSaldarPagosDECEVAL = value

            MyBase.CambioItem("ListaSaldarPagosDECEVAL")
            MyBase.CambioItem("ListaSaldarPagosDECEVALPaged")
            If Not IsNothing(value) Then
                If IsNothing(SaldarPagosDECEVAAnterior) Then
                    SaldarPagosDECEVASelected = _ListaSaldarPagosDECEVAL.FirstOrDefault
                Else
                    SaldarPagosDECEVASelected = SaldarPagosDECEVAAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaSaldarPagosDECEVALPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaSaldarPagosDECEVAL) Then
                Dim view = New PagedCollectionView(_ListaSaldarPagosDECEVAL)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _SaldarPagosDECEVASelected As SaldarPagosDECEVA
    Public Property SaldarPagosDECEVASelected() As SaldarPagosDECEVA
        Get
            Return _SaldarPagosDECEVASelected
        End Get
        Set(ByVal value As SaldarPagosDECEVA)
            _SaldarPagosDECEVASelected = value
            MyBase.CambioItem("SaldarPagosDECEVASelected")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewSaldarPagosDECEVA As New SaldarPagosDECEVA
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewSaldarPagosDECEVA.Tesoreria = SaldarPagosDECEVAPorDefecto.Tesoreria
            NewSaldarPagosDECEVA.FechaUno = SaldarPagosDECEVAPorDefecto.FechaUno
            NewSaldarPagosDECEVA.FechaDos = SaldarPagosDECEVAPorDefecto.FechaDos
            NewSaldarPagosDECEVA.ConsecutivoUno = SaldarPagosDECEVAPorDefecto.ConsecutivoUno
            NewSaldarPagosDECEVA.Numero = SaldarPagosDECEVAPorDefecto.Numero
            NewSaldarPagosDECEVA.haElaboracion = SaldarPagosDECEVAPorDefecto.haElaboracion
            NewSaldarPagosDECEVA.ConsecutivoDos = SaldarPagosDECEVAPorDefecto.ConsecutivoDos
            NewSaldarPagosDECEVA.Banco = SaldarPagosDECEVAPorDefecto.Banco
            NewSaldarPagosDECEVA.CuentaContable = SaldarPagosDECEVAPorDefecto.CuentaContable
            NewSaldarPagosDECEVA.IDSaldarPagosDECEVAL = SaldarPagosDECEVAPorDefecto.IDSaldarPagosDECEVAL
            SaldarPagosDECEVAAnterior = SaldarPagosDECEVASelected
            SaldarPagosDECEVASelected = NewSaldarPagosDECEVA
            MyBase.CambioItem("SaldarPagosDECEVAL")
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
            dcProxy.SaldarPagosDECEVAs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                dcProxy.Load(dcProxy.SaldarPagosDECEVALFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSaldarPagosDECEVAL, Nothing)
            Else
                dcProxy.Load(dcProxy.SaldarPagosDECEVALFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSaldarPagosDECEVAL, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            SaldarPagosDECEVAAnterior = SaldarPagosDECEVASelected
            If Not ListaSaldarPagosDECEVAL.Contains(SaldarPagosDECEVASelected) Then
                origen = "insert"
                ListaSaldarPagosDECEVAL.Add(SaldarPagosDECEVASelected)
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
        If Not IsNothing(_SaldarPagosDECEVASelected) Then
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_SaldarPagosDECEVASelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _SaldarPagosDECEVASelected.EntityState = EntityState.Detached Then
                    SaldarPagosDECEVASelected = SaldarPagosDECEVAAnterior
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
            If Not IsNothing(_SaldarPagosDECEVASelected) Then
                dcProxy.SaldarPagosDECEVAs.Remove(_SaldarPagosDECEVASelected)
                SaldarPagosDECEVASelected = _ListaSaldarPagosDECEVAL.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
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
Public Class CamposBusquedaSaldarPagosDECEVA
End Class




