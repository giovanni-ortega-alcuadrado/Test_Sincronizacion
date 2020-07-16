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
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports A2Utilidades.Mensajes

Public Class UnificarEspecieViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext
    Public result As MessageBoxResult

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of EspeciesCFDomainContext.IEspeciesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        Try
            If Not Program.IsDesignMode() Then

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "UnificarespeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

#End Region

#Region "Propiedades"

    Private _ListaEspecieUnificar As EntitySet(Of ConsultaNombreEspecie)
    Public Property ListaEspecieUnificar() As EntitySet(Of ConsultaNombreEspecie)
        Get
            Return _ListaEspecieUnificar
        End Get
        Set(ByVal value As EntitySet(Of ConsultaNombreEspecie))
            _ListaEspecieUnificar = value
            If Not IsNothing(value) Then

                NombreEspecieSelected = _ListaEspecieUnificar.FirstOrDefault

            End If
            MyBase.CambioItem("ListaEspecieUnificar")
        End Set
    End Property
    Private _NombreEspecieSelected As ConsultaNombreEspecie
    Public Property NombreEspecieSelected() As ConsultaNombreEspecie
        Get
            Return _NombreEspecieSelected
        End Get
        Set(ByVal value As ConsultaNombreEspecie)
            _NombreEspecieSelected = value
            If Not value Is Nothing Then


            End If
            MyBase.CambioItem("NombreEspecieSelected")
        End Set
    End Property


    Private WithEvents _unificaEspecie As ClaseUnificarEspecie = New ClaseUnificarEspecie
    Public Property unificaEspecie() As ClaseUnificarEspecie
        Get
            Return _unificaEspecie
        End Get
        Set(ByVal value As ClaseUnificarEspecie)
            _unificaEspecie = value
            MyBase.CambioItem("unificaEspecie")
        End Set
    End Property


    Private _Habilitaboton As Boolean = False
    Public Property Habilitaboton() As Boolean
        Get
            Return _Habilitaboton
        End Get
        Set(ByVal value As Boolean)
            _Habilitaboton = value
            MyBase.CambioItem("Habilitaboton")
        End Set
    End Property



#End Region

#Region "Métodos"

    Private Sub _unifica_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _unificaEspecie.PropertyChanged

        If e.PropertyName.Equals("EspecieRetira") Then
            If unificaEspecie.EspecieRetira <> String.Empty Then
                unificaEspecie.Accion = CType("B", Char?)
                dcProxy.ConsultaNombreEspecies.Clear()
                dcProxy.Load(dcProxy.TraerUnificarEspecieQuery(unificaEspecie.EspecieRetira, unificaEspecie.Accion, Program.Usuario, Program.HashConexion), AddressOf terminotraernombre, Nothing)
                IsBusy = True
            End If
        ElseIf e.PropertyName.Equals("EspecieUnifica") Then
            If ((unificaEspecie.EspecieRetira = String.Empty) And (unificaEspecie.EspecieUnifica <> String.Empty)) Then
                If Editando = True Then
                    A2Utilidades.Mensajes.mostrarMensaje("Digite primero la cuenta a retirar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    CancelarEditarRegistro()

                End If
            ElseIf unificaEspecie.EspecieUnifica <> String.Empty Then

                unificaEspecie.Accion = CType("B", Char?)
                dcProxy.ConsultaNombreEspecies.Clear()
                dcProxy.Load(dcProxy.TraerUnificarEspecieQuery(unificaEspecie.EspecieUnifica, unificaEspecie.Accion, Program.Usuario, Program.HashConexion), AddressOf terminotraer, Nothing)
                IsBusy = True
            End If

        End If

    End Sub
    Sub ActualizaRegistro()
        'C1.Silverlight.C1MessageBox.Show("Realmente desea unificar la especie:   " + unificaEspecie.EspecieRetira _
        '                                 + "   " + "Con la especie   " + unificaEspecie.EspecieUnifica, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf Terminopregunta)
        mostrarMensajePregunta("Realmente desea unificar la especie:   " + unificaEspecie.EspecieRetira _
                                         + "   " + "Con la especie   " + unificaEspecie.EspecieUnifica, _
                                Program.TituloSistema, _
                                "UNIFICARESPECIE", _
                                AddressOf Terminopregunta, True, "¿Unificar?")
    End Sub
    Private Sub Terminopregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If (unificaEspecie.EspecieRetira = unificaEspecie.EspecieUnifica) Then
                A2Utilidades.Mensajes.mostrarMensaje("Los codigos de las especies deben ser diferentes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                cancelar()
                Exit Sub
            End If
            If (unificaEspecie.ClaseR <> unificaEspecie.ClaseU) Then
                A2Utilidades.Mensajes.mostrarMensaje("Las clases de las especies deben ser iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                cancelar()
                Exit Sub
            End If
            Try
                Dim origen = "update"
                ErrorForma = ""
                origen = "insert"
                unificaEspecie.Accion = "U"
                dcProxy.Load(dcProxy.UnificarEspecieQuery(unificaEspecie.EspecieRetira, unificaEspecie.Accion, unificaEspecie.EspecieUnifica, Program.Usuario, Program.HashConexion), AddressOf TerminoSubmit, "insert")
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                     Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try

        Else
            cancelar()
        End If
    End Sub

    Private Sub TerminoSubmit(ByVal lo As LoadOperation(Of ConsultaNombreEspecie))
        Try
            IsBusy = False
            If lo.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de unificación de las especies", _
                                               Me.ToString(), "TerminoSubmitChanges" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
                'A2Utilidades.Mensajes.mostrarMensaje("La unificacion de la especie fallo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                If lo.UserState = "insert" Then
                    dcProxy.RejectChanges()
                End If
                lo.MarkErrorAsHandled()
                Exit Try
            End If
            If lo.UserState = "insert" Then
                cancelar()
                A2Utilidades.Mensajes.mostrarMensaje("La unificación de la especie fue exitosa.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub cancelar()
        ErrorForma = ""
        unificaEspecie.Accion = ""
        unificaEspecie.EspecieRetira = Nothing
        unificaEspecie.EspecieUnifica = Nothing
        unificaEspecie.NombreEspecieR = ""
        unificaEspecie.NombreEspecieU = ""
        Editando = False
        Habilitaboton = False
    End Sub

    Public Sub terminotraernombre(ByVal lo As LoadOperation(Of ConsultaNombreEspecie))


        If Not lo.HasError Then

            ListaEspecieUnificar = dcProxy.ConsultaNombreEspecies

            If ListaEspecieUnificar.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La especie no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CancelarEditarRegistro()
                Habilitaboton = False
                _unificaEspecie.EspecieRetira = String.Empty
                _unificaEspecie.NombreEspecieR = String.Empty
            Else
                unificaEspecie.NombreEspecieR = NombreEspecieSelected.Nombre
                unificaEspecie.ClaseR = NombreEspecieSelected.Clase
                If _unificaEspecie.EspecieUnifica <> String.Empty Then Habilitaboton = True
                MyBase.CambioItem("unificaEspecie")
                MyBase.CambioItem("NombreEspecieR")
            End If


        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de especies", _
                                             Me.ToString(), "TerminoTraerespecies", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Public Sub terminotraer(ByVal lo As LoadOperation(Of ConsultaNombreEspecie))


        If Not lo.HasError Then
            ListaEspecieUnificar = dcProxy.ConsultaNombreEspecies
            If ListaEspecieUnificar.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La especie no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CancelarEditarRegistro()
                Habilitaboton = False
                _unificaEspecie.EspecieUnifica = String.Empty
                _unificaEspecie.NombreEspecieU = String.Empty
            Else
                Habilitaboton = True
                unificaEspecie.NombreEspecieU = NombreEspecieSelected.Nombre
                unificaEspecie.ClaseU = NombreEspecieSelected.Clase
                If _unificaEspecie.EspecieRetira <> String.Empty Then Habilitaboton = True
                MyBase.CambioItem("unificaEspecie")
                MyBase.CambioItem("NombreEspecieU")
            End If


        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de especies", _
                                             Me.ToString(), "TerminoTraerespecies", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
#End Region
End Class
Public Class ClaseUnificarEspecie
    Implements INotifyPropertyChanged




    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    Private _EspecieRetira As String
    <Required(ErrorMessage:="Este campo es requerido. (EspecieRetira)")> _
    <Display(Name:="EspecieRetira")> _
    Public Property EspecieRetira As String
        Get
            Return _EspecieRetira
        End Get
        Set(ByVal value As String)
            _EspecieRetira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspecieRetira"))
        End Set
    End Property

    Private _EspecieUnifica As String
    <Required(ErrorMessage:="Este campo es requerido. (EspecieUnifica)")> _
    <Display(Name:="EspecieUnifica")> _
    Public Property EspecieUnifica As String
        Get
            Return _EspecieUnifica
        End Get
        Set(ByVal value As String)
            _EspecieUnifica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EspecieUnifica"))
        End Set
    End Property
    Private _NombreEspecieR As String
    <Display(Name:="Nombre")> _
    Public Property NombreEspecieR As String
        Get
            Return _NombreEspecieR
        End Get
        Set(ByVal value As String)
            _NombreEspecieR = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEspecieR"))
        End Set
    End Property
    Private _NombreEspecieU As String
    <Display(Name:="Nombre")> _
    Public Property NombreEspecieU As String
        Get
            Return _NombreEspecieU
        End Get
        Set(ByVal value As String)
            _NombreEspecieU = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreEspecieU"))
        End Set
    End Property


    <Display(Name:="Accion")> _
    Public Property Accion As Char

    <Display(Name:="ClaseR")> _
    Public Property ClaseR As Boolean

    <Display(Name:="ClaseU")> _
    Public Property ClaseU As Boolean

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
Public Class CamposBusquedaUnificarEspecie

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente", Description:="Comitente")> _
    Public Property Comitente As String

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
   <Display(Name:="Deposito", Description:="Deposito")> _
    Public Property Deposito As String
End Class