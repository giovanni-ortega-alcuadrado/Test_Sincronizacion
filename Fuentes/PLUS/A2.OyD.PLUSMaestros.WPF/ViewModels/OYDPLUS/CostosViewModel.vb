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

Public Class CostosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCosto
    Private CostoPorDefecto As Costo
    Private CostoAnterior As Costo
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext
    Dim IdItemActualizar As Integer = 0
    Private FORMAPAGO_CHEQUE As String = ""
    Private TIPOCHEQUE_CHEQUE As String = ""
    Private TIPOCHEQUE_CHEQUEGERENCIA As String = ""

    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                objProxy = New OyDPLUSutilidadesDomainContext()

                FORMAPAGO_CHEQUE = "C"
                TIPOCHEQUE_CHEQUE = "C"
                TIPOCHEQUE_CHEQUEGERENCIA = "CG"
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))

                FORMAPAGO_CHEQUE = RetornarValorProgram(Program.FormaPago_Cheque, "C")
                TIPOCHEQUE_CHEQUE = RetornarValorProgram(Program.TipoCheque_Cheque, "C")
                TIPOCHEQUE_CHEQUEGERENCIA = RetornarValorProgram(Program.TipoCheque_ChequeGerencia, "CG")
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CostosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCostos, "")
                dcProxy1.Load(dcProxy1.TraerCostoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCostosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  CostosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
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

    Private Sub TerminoTraerCostosPorDefecto_Completed(ByVal lo As LoadOperation(Of Costo))
        If Not lo.HasError Then
            CostoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Costo por defecto",
                                             Me.ToString(), "TerminoTraerCostoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCostos(ByVal lo As LoadOperation(Of Costo))
        If Not lo.HasError Then
            ListaCostos = dcProxy.Costos
            If dcProxy.Costos.Count > 0 Then
                If lo.UserState = "insert" Then
                    CostoSelected = ListaCostos.Last
                ElseIf lo.UserState = "update" Then
                    If ListaCostos.Where(Function(i) i.Idcosto = IdItemActualizar).Count > 0 Then
                        CostoSelected = ListaCostos.Where(Function(i) i.Idcosto = IdItemActualizar).FirstOrDefault
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Costos",
                                             Me.ToString(), "TerminoTraerCosto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaCostos As EntitySet(Of Costo)
    Public Property ListaCostos() As EntitySet(Of Costo)
        Get
            Return _ListaCostos
        End Get
        Set(ByVal value As EntitySet(Of Costo))
            _ListaCostos = value

            MyBase.CambioItem("ListaCostos")
            MyBase.CambioItem("ListaCostosPaged")
            If Not IsNothing(_ListaCostos) Then
                If _ListaCostos.Count > 0 Then
                    _CostoSelected = _ListaCostos.FirstOrDefault
                Else
                    _CostoSelected = Nothing
                End If
            Else
                _CostoSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCostosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCostos) Then
                Dim view = New PagedCollectionView(_ListaCostos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CostoSelected As Costo
    Public Property CostoSelected() As Costo
        Get
            Return _CostoSelected
        End Get
        Set(ByVal value As Costo)
            _CostoSelected = value
            If Not IsNothing(_CostoSelected) Then
                If _CostoSelected.CodigoFormapago = "C" Then
                    HabilitarCheque = True
                Else
                    HabilitarCheque = False
                End If
            End If
            MyBase.CambioItem("CostoSelected")
        End Set
    End Property

    Private _HabilitarCheque As Boolean
    Public Property HabilitarCheque() As Boolean
        Get
            Return _HabilitarCheque
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCheque = value
            MyBase.CambioItem("HabilitarCheque")
        End Set
    End Property

    Private _HabilitarTipoCruce As Boolean
    Public Property HabilitarTipoCruce() As Boolean
        Get
            Return _HabilitarTipoCruce
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoCruce = value
            MyBase.CambioItem("HabilitarTipoCruce")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            ObtenerRegistroAnterior()

            Dim NewCosto As New Costo
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCosto.Idcosto = CostoPorDefecto.Idcosto
            NewCosto.CodigoFormapago = CostoPorDefecto.CodigoFormapago
            NewCosto.CodigoTipoCheque = CostoPorDefecto.CodigoTipoCheque
            NewCosto.CodigoTipoCruce = CostoPorDefecto.CodigoTipoCruce
            NewCosto.Valor = CostoPorDefecto.Valor
            NewCosto.Usuario = Program.Usuario
            CostoAnterior = _CostoSelected
            _CostoSelected = NewCosto
            MyBase.CambioItem("CostoSelected")
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
            dcProxy.Costos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CostosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCostos, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CostosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCostos, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaCosto()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.CodigoFormapago <> String.Empty Or cb.CodigoTipoCheque <> String.Empty Or cb.CodigoTipoCruce <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Costos.Clear()
                CostoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " CodigoFormapago = " &  cb.CodigoFormapago.ToString() & " CodigoTipoCheque = " &  cb.CodigoTipoCheque.ToString() & " CodigoTipoCruce = " &  cb.CodigoTipoCruce.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.CostosConsultarQuery(cb.CodigoFormapago, cb.CodigoTipoCheque, cb.CodigoTipoCruce, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCostos, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCosto
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
            Dim origen = "update"
            ErrorForma = ""
            If Not ListaCostos.Where(Function(i) i.Idcosto = _CostoSelected.Idcosto).Count > 0 Then
                origen = "insert"
            End If

            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                objProxy.ValidacionEliminarRegistros.Clear()
            End If

            If origen = "insert" Then
                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblCostos", "'strCodigoFormapago'|'strCodigoTipoCheque'|'strCodigoTipoCruce'", String.Format("'{0}'|'{1}'|'{2}'", _CostoSelected.CodigoFormapago, IIf(_CostoSelected.CodigoTipoCheque Is Nothing, "NULL", _CostoSelected.CodigoTipoCheque), IIf(_CostoSelected.CodigoTipoCruce Is Nothing, "NULL", _CostoSelected.CodigoTipoCruce)), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
            Else
                If CostoAnterior.CodigoFormapago <> _CostoSelected.CodigoFormapago Or CostoAnterior.CodigoTipoCheque <> _CostoSelected.CodigoTipoCheque _
                   Or CostoAnterior.CodigoTipoCruce <> _CostoSelected.CodigoTipoCruce Then
                    objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblCostos", "'strCodigoFormapago'|'strCodigoTipoCheque'|'strCodigoTipoCruce'", String.Format("'{0}'|'{1}'|'{2}'", _CostoSelected.CodigoFormapago, IIf(_CostoSelected.CodigoTipoCheque Is Nothing, "NULL", _CostoSelected.CodigoTipoCheque), IIf(_CostoSelected.CodigoTipoCruce Is Nothing, "NULL", _CostoSelected.CodigoTipoCruce)), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                Else
                    If origen = "insert" Then
                        ListaCostos.Add(_CostoSelected)
                    End If
                    IsBusy = True
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

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then

                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Dim origen = "update"
                            ErrorForma = ""
                            If Not ListaCostos.Where(Function(i) i.Idcosto = _CostoSelected.Idcosto).Count > 0 Then
                                origen = "insert"
                                ListaCostos.Add(_CostoSelected)
                            End If
                            IsBusy = True
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.",
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.",
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
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


            If So.UserState = "update" Then
                IdItemActualizar = _CostoSelected.Idcosto
            End If

            If Not IsNothing(dcProxy.Costos) Then
                dcProxy.Costos.Clear()
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.CostosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCostos, So.UserState)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(__CostoSelected) Then
            Editando = True
            If _CostoSelected.CodigoFormapago = FORMAPAGO_CHEQUE Then
                HabilitarCheque = True
                If _CostoSelected.CodigoTipoCheque = TIPOCHEQUE_CHEQUE Then
                    HabilitarTipoCruce = True
                ElseIf _CostoSelected.CodigoTipoCheque = TIPOCHEQUE_CHEQUEGERENCIA Then
                    HabilitarTipoCruce = False
                End If
            Else
                HabilitarCheque = False
                HabilitarTipoCruce = False
            End If
            ObtenerRegistroAnterior()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CostoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                CostoSelected = CostoAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_CostoSelected) Then
                'dcProxy.Costos.Remove(_CostoSelected)
                dcProxy.Costos.Remove(dcProxy.Costos.Where(Function(i) i.Idcosto = _CostoSelected.Idcosto).First())

                _CostoSelected = _ListaCostos.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objCosto As New Costo
            If Not IsNothing(_CostoSelected) Then
                objCosto.Idcosto = _CostoSelected.Idcosto
                objCosto.CodigoFormapago = _CostoSelected.CodigoFormapago
                objCosto.CodigoTipoCheque = _CostoSelected.CodigoTipoCheque
                objCosto.CodigoTipoCruce = _CostoSelected.CodigoTipoCruce
                objCosto.NombreFormaPago = _CostoSelected.NombreFormaPago
                objCosto.NombreTipoCheque = _CostoSelected.NombreTipoCheque
                objCosto.NombreTipoCruce = _CostoSelected.NombreTipoCruce
                objCosto.Usuario = _CostoSelected.Usuario
                objCosto.Valor = _CostoSelected.Valor
            End If

            CostoAnterior = Nothing
            CostoAnterior = objCosto
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.",
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _CostoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CostoSelected.PropertyChanged
        Try
            If e.PropertyName.Equals("CodigoFormapago") Then
                If Editando Then
                    If Not IsNothing(_CostoSelected) Then
                        If _CostoSelected.CodigoFormapago = FORMAPAGO_CHEQUE Then
                            HabilitarCheque = True
                        Else
                            HabilitarCheque = False
                            HabilitarTipoCruce = False
                            _CostoSelected.CodigoTipoCheque = Nothing
                            _CostoSelected.CodigoTipoCruce = Nothing
                        End If
                    End If
                End If
            ElseIf e.PropertyName.Equals("CodigoTipoCheque") Then
                If Editando Then
                    If Not IsNothing(_CostoSelected) Then
                        If _CostoSelected.CodigoTipoCheque = TIPOCHEQUE_CHEQUE Then
                            HabilitarTipoCruce = True
                        ElseIf _CostoSelected.CodigoTipoCheque = TIPOCHEQUE_CHEQUEGERENCIA Then
                            HabilitarTipoCruce = False
                            _CostoSelected.CodigoTipoCruce = Nothing
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.",
             Me.ToString(), "_CostoSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCosto

    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="Forma Pago", Description:="Código de Forma de Pago")>
    Public Property CodigoFormapago As String = String.Empty

    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="Tipo Cheque", Description:="Código de Tipo de Cheque")>
    Public Property CodigoTipoCheque As String = String.Empty

    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="Tipo Cruce", Description:="Código Tipo Cruce")>
    Public Property CodigoTipoCruce As String = String.Empty
End Class




