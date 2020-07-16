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
Imports A2Utilidades.Mensajes

Public Class UnificarCuentaViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaUnificar
    Private CuentasDecevalPorAgrupadoPorDefecto As CuentasDecevalPorAgrupado
    Private CuentasDecevalPorAgrupadoAnterior As CuentasDecevalPorAgrupado
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Public result As MessageBoxResult
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
                'IsBusy = True
                'dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "FiltroInicial")
                'dcProxy1.Load(dcProxy1.TraerCuentasDecevalPorAgrupadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupadorPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CuentasDecevalPorAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

#End Region

#Region "Propiedades"

    Private _ListaCuentasUnificar As EntitySet(Of ConsultaNombre)
    Public Property ListaCuentasUnificar() As EntitySet(Of ConsultaNombre)
        Get
            Return _ListaCuentasUnificar
        End Get
        Set(ByVal value As EntitySet(Of ConsultaNombre))
            _ListaCuentasUnificar = value
            If Not IsNothing(value) Then

                CuentasUnificarSelected = _ListaCuentasUnificar.FirstOrDefault

            End If
            MyBase.CambioItem("ListaCuentasUnificar")
        End Set
    End Property
    Private _CuentasUnificarSelected As ConsultaNombre
    Public Property CuentasUnificarSelected() As ConsultaNombre
        Get
            Return _CuentasUnificarSelected
        End Get
        Set(ByVal value As ConsultaNombre)
            _CuentasUnificarSelected = value
            If Not value Is Nothing Then


            End If
            MyBase.CambioItem("CuentasUnificarSelected")
        End Set
    End Property


    Private WithEvents _unifica As Unificar = New Unificar
    Public Property unifica() As Unificar
        Get
            Return _unifica
        End Get
        Set(ByVal value As Unificar)
            _unifica = value
            MyBase.CambioItem("unifica")
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

    Private Sub _unifica_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _unifica.PropertyChanged

        If e.PropertyName.Equals("CuentaRetira") Then
            If Not IsNothing(unifica.CuentaRetira) Then
                unifica.Accion = CType("B", Char?)
                dcProxy.ConsultaNombres.Clear()
                dcProxy.Load(dcProxy.TraerUnificarCuentaQuery(unifica.CuentaRetira, unifica.Accion, unifica.Deposito,Program.Usuario, Program.HashConexion), AddressOf terminotraernombre, Nothing)
                IsBusy = True
            End If
        ElseIf e.PropertyName.Equals("CuentaUnifica") Then
            If (IsNothing(unifica.CuentaRetira) And (Not IsNothing(unifica.CuentaUnifica))) Then

                A2Utilidades.Mensajes.mostrarMensaje("Digite primero la cuenta a retirar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            ElseIf Not IsNothing(unifica.CuentaUnifica) Then

                unifica.Accion = CType("B", Char?)
                dcProxy.ConsultaNombres.Clear()
                dcProxy.Load(dcProxy.TraerUnificarCuentaQuery(unifica.CuentaUnifica, unifica.Accion, unifica.Deposito,Program.Usuario, Program.HashConexion), AddressOf terminotraer, Nothing)
                IsBusy = True
            End If

        End If

    End Sub
    Sub ActualizaRegistro()
        'C1.Silverlight.C1MessageBox.Show("Realmente desea unificar la cuenta:   " + unifica.CuentaRetira.ToString + "   " + "Con la cuenta   " + unifica.CuentaUnifica.ToString, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminopregunta)
        mostrarMensajePregunta("Realmente desea unificar la cuenta:   " + unifica.CuentaRetira.ToString + "   " + "Con la cuenta   " + unifica.CuentaUnifica.ToString, _
                               Program.TituloSistema, _
                               "ACTUALIZARREGISTRO", _
                               AddressOf Terminopregunta, True, "¿Unificar?")
    End Sub
    Private Sub Terminopregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If (unifica.CuentaRetira = unifica.CuentaUnifica) Then
                A2Utilidades.Mensajes.mostrarMensaje("Los codigos de las cuentas deben ser diferentes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                cancelar()
                Exit Sub
            End If
            If (unifica.NombreClienteR <> unifica.NombreClienteU) Then
                A2Utilidades.Mensajes.mostrarMensaje("Las cuentas deben pertenecer al mismo cliente .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                cancelar()
                Exit Sub
            End If
            Try
                Dim origen = "update"
                ErrorForma = ""
                origen = "insert"
                unifica.Accion = "U"
                dcProxy.Load(dcProxy.UnificarCuentaQuery(unifica.CuentaRetira, unifica.Accion, unifica.CuentaUnifica, unifica.Deposito,Program.Usuario, Program.HashConexion), AddressOf TerminoSubmit, "insert")

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

    Private Sub TerminoSubmit(ByVal lo As LoadOperation(Of ConsultaNombre))
        Try
            IsBusy = False
            If lo.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
                If lo.UserState = "insert" Then
                    dcProxy.RejectChanges()
                End If
                lo.MarkErrorAsHandled()
                Exit Try
            End If
            If lo.UserState = "insert" Then
                cancelar()
                A2Utilidades.Mensajes.mostrarMensaje("La unificación de la cuenta fue exitosa.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub cancelar()
        ErrorForma = ""
        unifica.Accion = ""
        unifica.CuentaRetira = Nothing
        unifica.CuentaUnifica = Nothing
        unifica.NombreClienteR = ""
        unifica.NombreClienteU = ""
        Editando = False
        Habilitaboton = False
    End Sub

    Public Sub terminotraernombre(ByVal lo As LoadOperation(Of ConsultaNombre))


        If Not lo.HasError Then

            ListaCuentasUnificar = dcProxy.ConsultaNombres

            If ListaCuentasUnificar.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El número de cuenta no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CancelarEditarRegistro()

            Else
                unifica.NombreClienteR = CuentasUnificarSelected.Nombre
                MyBase.CambioItem("unifica")
                MyBase.CambioItem("NombreClienteR")
            End If


        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresSistemas", _
                                             Me.ToString(), "TerminoTraerReceptoresSistemas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    Public Sub terminotraer(ByVal lo As LoadOperation(Of ConsultaNombre))


        If Not lo.HasError Then
            ListaCuentasUnificar = dcProxy.ConsultaNombres
            If ListaCuentasUnificar.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El número de cuenta no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CancelarEditarRegistro()
            Else
                Habilitaboton = True
                unifica.NombreClienteU = CuentasUnificarSelected.Nombre
                MyBase.CambioItem("unifica")
                MyBase.CambioItem("NombreClienteU")
            End If


        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresSistemas", _
                                             Me.ToString(), "TerminoTraerReceptoresSistemas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
#End Region
End Class
Public Class Unificar
    Implements INotifyPropertyChanged




    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    Private _CuentaRetira As System.Nullable(Of Integer)
    <RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")> _
    <Required(ErrorMessage:="Este campo es requerido. (CuentaRetira)")> _
    <Display(Name:="Cuenta a Retirar")> _
     Public Property CuentaRetira As System.Nullable(Of Integer)
        Get
            Return _CuentaRetira
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _CuentaRetira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaRetira"))
        End Set
    End Property

    Private _CuentaUnifica As System.Nullable(Of Integer)
    <RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")> _
    <Required(ErrorMessage:="Este campo es requerido. (CuentaUnifica)")> _
    <Display(Name:="Cuenta a Unificar")> _
     Public Property CuentaUnifica As System.Nullable(Of Integer)
        Get
            Return _CuentaUnifica
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _CuentaUnifica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaUnifica"))
        End Set
    End Property
    Private _NombreClienteR As String
    <Display(Name:="Nombre")> _
    Public Property NombreClienteR As String
        Get
            Return _NombreClienteR
        End Get
        Set(ByVal value As String)
            _NombreClienteR = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreClienteR"))
        End Set
    End Property
    Private _NombreClienteU As String
    <Display(Name:="Nombre")> _
    Public Property NombreClienteU As String
        Get
            Return _NombreClienteU
        End Get
        Set(ByVal value As String)
            _NombreClienteU = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreClienteU"))
        End Set
    End Property


    <Display(Name:="Depósito")> _
    Public Property Deposito As String

    <Display(Name:="Accion")> _
    Public Property Accion As Char


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class


Public Class CamposBusquedaUnificar

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente", Description:="Comitente")> _
    Public Property Comitente As String

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
   <Display(Name:="Depósito", Description:="Deposito")> _
    Public Property Deposito As String
End Class