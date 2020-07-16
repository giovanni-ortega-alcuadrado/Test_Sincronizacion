Imports Telerik.Windows.Controls
'Archivo:           BrokerComisionesViewModel.vb
'Desarrollado por:	Sebastian Londoño Benitez.
'Fecha:				Mayo 23/2011
'Propiedad de Alcuadrado S.A. 2014

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports System.Text.RegularExpressions

Public Class BrokerComisionesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBrokerComisiones
    'Private TipoPersonaPorDctPorDefecto As TipoPersonaPorDct
    Private ComisionesBrokerAnterior As ComisionesBroker
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim expresionemail As String = Program.ExpresionRegularEmail
    Const SEPARADORMAIL As String = ";"

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
                dcProxy.Load(dcProxy.ComisionesBrokerFiltrarQuery("", "F", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionesBroker, "FiltroInicial")
                'dcProxy1.Load(dcProxy1.TraerTipoPersonaPorDctPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoPersonaPorDctoPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "BrokerComisionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    'Private Sub TerminoTraerTipoPersonaPorDctoPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoPersonaPorDct))
    '    If Not lo.HasError Then
    '        TipoPersonaPorDctPorDefecto = lo.Entities.FirstOrDefault
    '    Else
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoPersonaPorDct por defecto", _
    '                                         Me.ToString(), "TerminoTraerTipoPersonaPorDctPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        lo.MarkErrorAsHandled()   '????
    '    End If
    'End Sub

    Private Sub TerminoTraerComisionesBroker(ByVal lo As LoadOperation(Of ComisionesBroker))
        If Not lo.HasError Then
            ListaComisionesBroker = dcProxy.ComisionesBrokers
            If ListaComisionesBroker.Count > 0 Then
                'If lo.UserState = "insert" Then
                '    TipoPersonaPorDctSelected = ListaTipoPersonaPorDcto.Last
                'End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de ComisionesBroker", Me.ToString, "TerminoTraerComisionesBroker", lo.Error)

            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ComisionesBroker", _
            '                                 Me.ToString(), "TerminoTraerComisionesBroker", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaComisionesBroker As EntitySet(Of ComisionesBroker)
    Public Property ListaComisionesBroker() As EntitySet(Of ComisionesBroker)
        Get
            Return _ListaComisionesBroker
        End Get
        Set(ByVal value As EntitySet(Of ComisionesBroker))
            _ListaComisionesBroker = value
            MyBase.CambioItem("ListaComisionesBroker")
            MyBase.CambioItem("ListaComisionesBrokerPaged")
            If Not IsNothing(value) Then
                If IsNothing(ComisionesBrokerAnterior) Then
                    ComisionesBrokerSelected = _ListaComisionesBroker.FirstOrDefault
                Else
                    ComisionesBrokerSelected = ComisionesBrokerAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaComisionesBrokerPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaComisionesBroker) Then
                Dim view = New PagedCollectionView(_ListaComisionesBroker)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ComisionesBrokerSelected As ComisionesBroker
    Public Property ComisionesBrokerSelected() As ComisionesBroker
        Get
            Return _ComisionesBrokerSelected
        End Get
        Set(ByVal value As ComisionesBroker)
            _ComisionesBrokerSelected = value
            MyBase.CambioItem("ComisionesBrokerSelected")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ComisionesBrokers.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ComisionesBrokerFiltrarQuery(TextoFiltroSeguro, "F", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionesBroker, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ComisionesBrokerFiltrarQuery("", "F", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionesBroker, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.Broker = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not String.IsNullOrEmpty(cb.Broker) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ComisionesBrokers.Clear()
                ComisionesBrokerAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ComisionesBrokerFiltrarQuery(cb.Broker, "C", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionesBroker, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBrokerComisiones
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
            If Not String.IsNullOrEmpty(_ComisionesBrokerSelected.Email) Then
                Dim emails = _ComisionesBrokerSelected.Email.Split(SEPARADORMAIL)
                For Each li In emails
                    If Not IsValidmail(Trim(li)) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El e-mail " & li & " no es valido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
            End If

            Dim origen = "update"
            ErrorForma = ""
            _ComisionesBrokerSelected.Usuario = Program.Usuario
            ComisionesBrokerAnterior = ComisionesBrokerSelected
            If Not ListaComisionesBroker.Contains(_ComisionesBrokerSelected) Then
                origen = "insert"
                ListaComisionesBroker.Add(ComisionesBrokerSelected)
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

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

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
        If Not IsNothing(_ComisionesBrokerSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ComisionesBrokerSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ComisionesBrokerSelected.EntityState = EntityState.Detached Then
                    _ComisionesBrokerSelected = ComisionesBrokerAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    ''' <summary>
    ''' Funcion para verificar email correcto
    ''' </summary>
    ''' <remarks>JBT20140201</remarks>
    Public Function IsValidmail(emailaddress As String) As Boolean
        Try
            Return Regex.IsMatch(emailaddress, expresionemail)
        Catch generatedExceptionName As FormatException
            Return False
        End Try
    End Function

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBrokerComisiones

    Implements INotifyPropertyChanged

    Private _Broker As String = String.Empty
    <Display(Name:="Broker")> _
    Public Property Broker As String
        Get
            Return _Broker
        End Get
        Set(ByVal value As String)
            _Broker = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Broker"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class




