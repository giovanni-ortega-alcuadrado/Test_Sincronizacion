Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConsecutivosViewModel.vb
'Generado el : 02/18/2011 09:56:35
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

Public Class CuentasBancariasPorConceptoViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCuentasBancariasPorConcepto
    Private CuentasBancariasPorConceptoPorDefecto As CuentasBancariasPorConcepto
    Private CuentasBancariasPorConceptoAnterior As CuentasBancariasPorConcepto
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
                dcProxy.Load(dcProxy.FiltrarCuentasBancariasPorConceptoQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancariasPorConcepto, "FiltroInicial")
                
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsecutivosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCuentasBancariasPorConceptoPorDefecto_Completed(ByVal lo As LoadOperation(Of CuentasBancariasPorConcepto))
        If Not lo.HasError Then
            CuentasBancariasPorConceptoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del registro por defecto", _
                                             Me.ToString(), "TerminoTraerCuentasBancariasPorConceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasBancariasPorConcepto(ByVal lo As LoadOperation(Of CuentasBancariasPorConcepto))
        If Not lo.HasError Then
            ListaCuentasBancariasPorConcepto = dcProxy.CuentasBancariasPorConceptos
            If dcProxy.CuentasBancariasPorConceptos.Count > 0 Then
                If lo.UserState = "insert" Then
                    CuentasBancariasPorConceptoSelected = ListaCuentasBancariasPorConcepto.First   ' JFSB 20160827 Se pone el primer registro que tiene la lista
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Consecutivos", _
                                             Me.ToString(), "TerminoTraerConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaCuentasBancariasPorConcepto As EntitySet(Of CuentasBancariasPorConcepto)
    Public Property ListaCuentasBancariasPorConcepto() As EntitySet(Of CuentasBancariasPorConcepto)
        Get
            Return _ListaCuentasBancariasPorConcepto
        End Get
        Set(ByVal value As EntitySet(Of CuentasBancariasPorConcepto))
            _ListaCuentasBancariasPorConcepto = value

            MyBase.CambioItem("ListaCuentasBancariasPorConcepto")
            MyBase.CambioItem("ListaCuentasBancariasPorConceptoPaged")
            If Not IsNothing(value) Then
                If IsNothing(CuentasBancariasPorConceptoAnterior) Then
                    CuentasBancariasPorConceptoSelected = _ListaCuentasBancariasPorConcepto.FirstOrDefault
                Else
                    CuentasBancariasPorConceptoSelected = CuentasBancariasPorConceptoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCuentasBancariasPorConceptoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCuentasBancariasPorConcepto) Then
                Dim view = New PagedCollectionView(_ListaCuentasBancariasPorConcepto)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CuentasBancariasPorConceptoSelected As CuentasBancariasPorConcepto
    Public Property CuentasBancariasPorConceptoSelected() As CuentasBancariasPorConcepto
        Get
            Return _CuentasBancariasPorConceptoSelected
        End Get
        Set(ByVal value As CuentasBancariasPorConcepto)
            _CuentasBancariasPorConceptoSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("CuentasBancariasPorConceptoSelected")
        End Set
    End Property

    Private _Editareg As Boolean
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
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
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewCuentasBancariasPorConcepto As New CuentasBancariasPorConcepto

            NewCuentasBancariasPorConcepto.IdCuentaBancaria = Nothing
            NewCuentasBancariasPorConcepto.NombreCuentaBancaria = String.Empty
            NewCuentasBancariasPorConcepto.IDCodigoBanco = Nothing
            NewCuentasBancariasPorConcepto.NombreBanco = String.Empty
            NewCuentasBancariasPorConcepto.IdConcepto = Nothing
            NewCuentasBancariasPorConcepto.IdConceptoAnterior = Nothing
            NewCuentasBancariasPorConcepto.DescripcionConcepto = String.Empty
            NewCuentasBancariasPorConcepto.CuentaContable = String.Empty
            NewCuentasBancariasPorConcepto.Actualizacion = Now
            NewCuentasBancariasPorConcepto.Usuario = Program.Usuario
            NewCuentasBancariasPorConcepto.Accion = "Ingresar"
            CuentasBancariasPorConceptoAnterior = CuentasBancariasPorConceptoSelected
            CuentasBancariasPorConceptoSelected = NewCuentasBancariasPorConcepto
            MyBase.CambioItem("CuentasBancariasPorConcepto")
            Editando = True
            Editareg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CuentasBancariasPorConceptos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.FiltrarCuentasBancariasPorConceptoQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancariasPorConcepto, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.FiltrarCuentasBancariasPorConceptoQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancariasPorConcepto, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IdCuentaBancaria = Nothing
        cb.CuentaContable = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If IsNothing(cb.IdCuentaBancaria) Or String.IsNullOrEmpty(cb.CuentaContable) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CuentasBancariasPorConceptos.Clear()
                CuentasBancariasPorConceptoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ConsultarCuentasBancariasPorConceptoQuery(cb.IdCuentaBancaria, cb.CuentaContable, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancariasPorConcepto, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCuentasBancariasPorConcepto
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaCuentasBancariasPorConcepto
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If IsNothing(CuentasBancariasPorConceptoSelected.IdCuentaBancaria) And String.IsNullOrEmpty(CuentasBancariasPorConceptoSelected.NombreCuentaBancaria) Then
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta bancaria es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(CuentasBancariasPorConceptoSelected.IdConcepto) Then
                A2Utilidades.Mensajes.mostrarMensaje("El concepto es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CuentasBancariasPorConceptoSelected.CuentaContable) Then
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(CuentasBancariasPorConceptoSelected.IDCodigoBanco) Then
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta bancaria no tiene correctamente configurado un banco.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(CuentasBancariasPorConceptoSelected.IdConceptoAnterior) Then
                CuentasBancariasPorConceptoSelected.IdConceptoAnterior = CuentasBancariasPorConceptoSelected.IdConcepto
            End If
            Dim origen = "Actualizar"
            ErrorForma = ""
            CuentasBancariasPorConceptoAnterior = CuentasBancariasPorConceptoSelected
            If Not ListaCuentasBancariasPorConcepto.Contains(CuentasBancariasPorConceptoSelected) Then
                origen = "Ingresar"
                ListaCuentasBancariasPorConcepto.Add(CuentasBancariasPorConceptoSelected)
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
            Dim strMsg As String = String.Empty
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "Ingresar" Or (So.UserState = "Actualizar")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    'If So.UserState = "insert" Then
                    '    ListaCuentasBancariasPorConcepto.Remove(CuentasBancariasPorConceptoSelected)
                    'End If
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
            If So.UserState = "Ingresar" Or So.UserState = "Actualizar" Then
                IsBusy = True
                dcProxy.CuentasBancariasPorConceptos.Clear()
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.FiltrarCuentasBancariasPorConceptoQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancariasPorConcepto, "Ingresar")

                Else
                    dcProxy.Load(dcProxy.FiltrarCuentasBancariasPorConceptoQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancariasPorConcepto, "Ingresar")
                End If


            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CuentasBancariasPorConceptoSelected) Then
            CuentasBancariasPorConceptoSelected.IdConceptoAnterior = CuentasBancariasPorConceptoSelected.IdConcepto
            Editando = True
            Editareg = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CuentasBancariasPorConceptoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _CuentasBancariasPorConceptoSelected.EntityState = EntityState.Detached Then
                    CuentasBancariasPorConceptoSelected = CuentasBancariasPorConceptoAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        
        'MessageBox.Show("Esta funcionalidad no esta habilitada para este maestro", "Funcionalidad ", MessageBoxButton.OK)
        Try
            If Not IsNothing(_CuentasBancariasPorConceptoSelected) Then
                dcProxy.CuentasBancariasPorConceptos.Remove(_CuentasBancariasPorConceptoSelected)
                CuentasBancariasPorConceptoSelected = _ListaCuentasBancariasPorConcepto.LastOrDefault
                IsBusy = True
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
        DicCamposTab.Add("NombreConsecutivo", 1)
        DicCamposTab.Add("Descripcion", 1)
        DicCamposTab.Add("IDOwner", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCuentasBancariasPorConcepto
    Implements INotifyPropertyChanged

    Private _IdCuentaBancaria As Nullable(Of Integer) = 0
    Public Property IdCuentaBancaria As Nullable(Of Integer)
        Get
            Return _IdCuentaBancaria
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdCuentaBancaria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdCuentaBancaria"))
        End Set
    End Property

    Private _NombreCuentaBancaria As String
    Public Property NombreCuentaBancaria() As String
        Get
            Return _NombreCuentaBancaria
        End Get
        Set(ByVal value As String)
            _NombreCuentaBancaria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCuentaBancaria"))
        End Set
    End Property

    Private _CuentaContable As String = String.Empty
    Public Property CuentaContable As String
        Get
            Return _CuentaContable
        End Get
        Set(ByVal value As String)
            _CuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaContable"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




