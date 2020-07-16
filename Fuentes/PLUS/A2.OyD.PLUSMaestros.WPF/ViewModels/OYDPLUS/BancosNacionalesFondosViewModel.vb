Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CostosViewModel.vb
'Generado el : 11/15/2012 07:29:09
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class BancosNacionalesFondosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBancosNacionalesFondos
    Private BancoNacionalFondoPorDefecto As BancosNacionalesFondos
    Private BancoNacionalFondoAnterior As BancosNacionalesFondos
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext
    Dim IdItemActualizar As Integer = 0

    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                objProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.BancosNacionalesFondosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondos, "")
                dcProxy1.Load(dcProxy1.TraerBancosNacionalesFondosPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CostosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerBancosNacioanlesFondosPorDefecto_Completed(ByVal lo As LoadOperation(Of BancosNacionalesFondos))
        If Not lo.HasError Then
            BancoNacionalFondoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del banco nacional por defecto", _
                                             Me.ToString(), "TerminoTraerBancosNacioanlesFondosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBancosNacioanlesFondos(ByVal lo As LoadOperation(Of BancosNacionalesFondos))
        If Not lo.HasError Then
            ListaBancosNacionalesFondos = dcProxy.BancosNacionalesFondos
            If dcProxy.BancosNacionalesFondos.Count > 0 Then
                If lo.UserState = "insert" Then
                    BancoNacionalFondoSelected = ListaBancosNacionalesFondos.Last
                ElseIf lo.UserState = "Actualizar" Then
                    If ListaBancosNacionalesFondos.Where(Function(i) i.ID = IdItemActualizar).Count > 0 Then
                        BancoNacionalFondoSelected = ListaBancosNacionalesFondos.Where(Function(i) i.ID = IdItemActualizar).FirstOrDefault
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Costos", _
                                             Me.ToString(), "TerminoTraerCosto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaBancosNacionalesFondos As EntitySet(Of BancosNacionalesFondos)
    Public Property ListaBancosNacionalesFondos() As EntitySet(Of BancosNacionalesFondos)
        Get
            Return _ListaBancosNacionalesFondos
        End Get
        Set(ByVal value As EntitySet(Of BancosNacionalesFondos))
            _ListaBancosNacionalesFondos = value

            MyBase.CambioItem("ListaBancosNacionalesFondos")
            MyBase.CambioItem("ListaBancosNacionalesFondosPaged")
            If Not IsNothing(_ListaBancosNacionalesFondos) Then
                If _ListaBancosNacionalesFondos.Count > 0 Then
                    _BancoNacionalFondoSelected = _ListaBancosNacionalesFondos.FirstOrDefault
                Else
                    _BancoNacionalFondoSelected = Nothing
                End If
            Else
                _BancoNacionalFondoSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBancosNacionalesFondosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBancosNacionalesFondos) Then
                Dim view = New PagedCollectionView(_ListaBancosNacionalesFondos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _BancoNacionalFondoSelected As BancosNacionalesFondos
    Public Property BancoNacionalFondoSelected() As BancosNacionalesFondos
        Get
            Return _BancoNacionalFondoSelected
        End Get
        Set(ByVal value As BancosNacionalesFondos)
            _BancoNacionalFondoSelected = value
            MyBase.CambioItem("BancoNacionalFondoSelected")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewBancoNacional As New BancosNacionalesFondos
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewBancoNacional.ID = BancoNacionalFondoPorDefecto.ID
            NewBancoNacional.IDBanco = 0
            NewBancoNacional.NombreBanco = BancoNacionalFondoPorDefecto.NombreBanco
            NewBancoNacional.NombreTipoCuenta = BancoNacionalFondoPorDefecto.NombreTipoCuenta
            NewBancoNacional.NombreTipoDocumentoTitular = BancoNacionalFondoPorDefecto.NombreTipoDocumentoTitular
            NewBancoNacional.NombreTitular = "_"
            NewBancoNacional.NroCuenta = "_"
            NewBancoNacional.NroDocumentoTitular = "_"
            NewBancoNacional.TipoCuenta = "A"
            NewBancoNacional.TipoDocumentoTitular = "C"
            NewBancoNacional.Usuario = Program.Usuario

            ObtenerRegistroAnterior()
            BancoNacionalFondoSelected = NewBancoNacional
            MyBase.CambioItem("BancoNacionalFondoSelected")
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
            dcProxy.BancosNacionalesFondos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.BancosNacionalesFondosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.BancosNacionalesFondosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondos, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaBancosNacionalesFondos()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDBanco <> 0 Or Not String.IsNullOrEmpty(cb.Nombre) Or Not String.IsNullOrEmpty(cb.NroCuenta) Or Not String.IsNullOrEmpty(cb.NroDocumento) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.BancosNacionalesFondos.Clear()
                BancoNacionalFondoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " CodigoFormapago = " &  cb.CodigoFormapago.ToString() & " CodigoTipoCheque = " &  cb.CodigoTipoCheque.ToString() & " CodigoTipoCruce = " &  cb.CodigoTipoCruce.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.BancosNacionalesFondosConsultarQuery(cb.IDBanco, cb.NroCuenta, cb.NroDocumento, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBancosNacionalesFondos
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
            If IsNothing(_BancoNacionalFondoSelected.IDBanco) Or _BancoNacionalFondoSelected.IDBanco = 0 Then
                mostrarMensaje("El banco es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_BancoNacionalFondoSelected.NroCuenta) Or _BancoNacionalFondoSelected.NroCuenta = "_" Then
                mostrarMensaje("El nro de cuenta es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_BancoNacionalFondoSelected.NroDocumentoTitular) Or _BancoNacionalFondoSelected.NroDocumentoTitular = "_" Then
                mostrarMensaje("El nro documento titular es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_BancoNacionalFondoSelected.NombreTitular) Or _BancoNacionalFondoSelected.NombreTitular = "_" Then
                mostrarMensaje("El nombre del titular es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            If Not ListaBancosNacionalesFondos.Where(Function(i) i.ID = _BancoNacionalFondoSelected.ID).Count > 0 Then
                origen = "insert"
                ListaBancosNacionalesFondos.Add(BancoNacionalFondoSelected)
            End If

            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                objProxy.ValidacionEliminarRegistros.Clear()
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
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            If So.UserState <> "BorrarRegistro" Then
                If Not IsNothing(_BancoNacionalFondoSelected) Then
                    IdItemActualizar = _BancoNacionalFondoSelected.ID
                End If

                If Not IsNothing(dcProxy.BancosNacionalesFondos) Then
                    dcProxy.BancosNacionalesFondos.Clear()
                End If

                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.Load(dcProxy.BancosNacionalesFondosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondos, "Actualizar")
            Else
                If Not IsNothing(dcProxy.BancosNacionalesFondos) Then
                    dcProxy.BancosNacionalesFondos.Clear()
                End If

                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.Load(dcProxy.BancosNacionalesFondosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancosNacioanlesFondos, So.UserState)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_BancoNacionalFondoSelected) Then
            Editando = True
            ObtenerRegistroAnterior()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_BancoNacionalFondoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                BancoNacionalFondoSelected = BancoNacionalFondoAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_BancoNacionalFondoSelected) Then
                'dcProxy.BancosNacionalesFondos.Remove(_BancoNacionalFondoSelected)
                dcProxy.BancosNacionalesFondos.Remove(dcProxy.BancosNacionalesFondos.Where(Function(i) i.ID = _BancoNacionalFondoSelected.ID).First())

                BancoNacionalFondoSelected = _ListaBancosNacionalesFondos.LastOrDefault  'Dic202011  nueva
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

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objBancoNacional As New BancosNacionalesFondos
            If Not IsNothing(_BancoNacionalFondoSelected) Then
                objBancoNacional.ID = _BancoNacionalFondoSelected.ID
                objBancoNacional.IDBanco = _BancoNacionalFondoSelected.IDBanco
                objBancoNacional.NombreBanco = _BancoNacionalFondoSelected.NombreBanco
                objBancoNacional.NombreTipoCuenta = _BancoNacionalFondoSelected.NombreTipoCuenta
                objBancoNacional.NombreTipoDocumentoTitular = _BancoNacionalFondoSelected.NombreTipoDocumentoTitular
                objBancoNacional.NombreTitular = _BancoNacionalFondoSelected.NombreTitular
                objBancoNacional.NroCuenta = _BancoNacionalFondoSelected.NroCuenta
                objBancoNacional.NroDocumentoTitular = _BancoNacionalFondoSelected.NroDocumentoTitular
                objBancoNacional.TipoCuenta = _BancoNacionalFondoSelected.TipoCuenta
                objBancoNacional.TipoDocumentoTitular = _BancoNacionalFondoSelected.TipoDocumentoTitular
                objBancoNacional.Usuario = _BancoNacionalFondoSelected.Usuario
                objBancoNacional.IDCodigoBancoSafyr = _BancoNacionalFondoSelected.IDCodigoBancoSafyr
                objBancoNacional.CarteraColectiva = _BancoNacionalFondoSelected.CarteraColectiva
            End If

            BancoNacionalFondoAnterior = Nothing
            BancoNacionalFondoAnterior = objBancoNacional
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _BancoNacionalFondoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _BancoNacionalFondoSelected.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_BancoNacionalFondoSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBancosNacionalesFondos
    Implements INotifyPropertyChanged


    <Display(Name:="Banco", Description:="Banco")> _
    Private _IDBanco As Nullable(Of Integer) = 0
    Public Property IDBanco() As Nullable(Of Integer)
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IDBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))
        End Set
    End Property


    Private _Banco As String = String.Empty
    <Display(Name:="Nombre banco", Description:="Nombre banco")> _
    Public Property Banco() As String
        Get
            Return _Banco
        End Get
        Set(ByVal value As String)
            _Banco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Banco"))
        End Set
    End Property

    Private _NroCuenta As String = String.Empty
    <Display(Name:="Nro cuenta", Description:="Nro cuenta")> _
    Public Property NroCuenta() As String
        Get
            Return _NroCuenta
        End Get
        Set(ByVal value As String)
            _NroCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroCuenta"))
        End Set
    End Property

    Private _NroDocumento As String = String.Empty
    <Display(Name:="Nro documento", Description:="Nro documento")> _
    Public Property NroDocumento() As String
        Get
            Return _NroDocumento
        End Get
        Set(ByVal value As String)
            _NroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroDocumento"))
        End Set
    End Property

    Private _Nombre As String = String.Empty
    <Display(Name:="Nombre", Description:="Nombre")> _
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class




