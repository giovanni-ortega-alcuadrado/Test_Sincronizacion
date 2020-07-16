Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: BloqueoSaldoClientesViewModel.vb
'Generado el : 04/11/2012 08:34:27
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
Imports A2Utilidades.Mensajes
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic.CompilerServices

Public Class BloqueoSaldoClientesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBloqueoSaldoCliente
    Private BloqueoSaldoClientePorDefecto As BloqueoSaldoCliente
    Private BloqueoSaldoClienteAnterior As BloqueoSaldoCliente
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Public Const GSTR_NATURALEZA_CREDITO = "CR"
    ''JCM20170727
    Private mdcProxyUtilidad03 As MaestrosDomainContext
    Private Const PARAM_STR_CALCULOS_FINANCIEROS As String = "CF_UTILIZACALCULOSFINANCIEROS"
    Private Const PARAM_STR_UTILIZA_PASIVA As String = "CF_UTILIZAPASIVA_A2"
    Dim UtilizaPasiva As Boolean

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxy1 = New MaestrosDomainContext()
                mdcProxyUtilidad03 = New MaestrosDomainContext()
            Else
                dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.BloqueoSaldoClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBloqueoSaldoClientes, "")
                dcProxy1.Load(dcProxy1.TraerBloqueoSaldoClientePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBloqueoSaldoClientesPorDefecto_Completed, "Default")

                dcProxy1.VerificaParametros(PARAM_STR_UTILIZA_PASIVA, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroPasiva, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "BloqueoSaldoClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerBloqueoSaldoClientesPorDefecto_Completed(ByVal lo As LoadOperation(Of BloqueoSaldoCliente))
        If Not lo.HasError Then
            BloqueoSaldoClientePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la BloqueoSaldoCliente por defecto",
                                             Me.ToString(), "TerminoTraerBloqueoSaldoClientePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBloqueoSaldoClientes(ByVal lo As LoadOperation(Of BloqueoSaldoCliente))
        If Not lo.HasError Then
            ListaBloqueoSaldoClientes = dcProxy.BloqueoSaldoClientes
            If dcProxy.BloqueoSaldoClientes.Count > 0 Then
                If lo.UserState = "insert" Then
                    BloqueoSaldoClienteSelected = ListaBloqueoSaldoClientes.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MessageBox.Show("No se encontro ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de BloqueoSaldoClientes",
                                             Me.ToString(), "TerminoTraerBloqueoSaldoCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Public Sub TerminValidarSaldoBloqueo(ByVal lo As InvokeOperation(Of Decimal))
        Try
            If Not lo.HasError Then
                If BloqueoSaldoClienteSelected.ValorBloqueado = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un Valor a Bloquear", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    If BloqueoSaldoClienteSelected.Naturaleza = GSTR_NATURALEZA_CREDITO Then
                        'JCM20170808, Esta validación solo debe hacer cuando es un saldo bloqueado de comisión y no se ejecuta cuando es un saldo bloquedo por encargo
                        If BloqueoSaldoClienteSelected.ValorBloqueado > lo.Value And ((IsNothing(BloqueoSaldoClienteSelected.intIDCompania) Or BloqueoSaldoClienteSelected.intIDCompania < 0) And (IsNothing(BloqueoSaldoClienteSelected.intIDEncargo) Or BloqueoSaldoClienteSelected.intIDEncargo < 0)) Then
                            IsBusy = False
                            A2Utilidades.Mensajes.mostrarMensaje("No se puede bloquear el Valor: " & "$" & BloqueoSaldoClienteSelected.ValorBloqueado.ToString & " es mayor al saldo del cliente: " & "$" & lo.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        Else
                            Dim origen = "update"
                            ErrorForma = ""
                            BloqueoSaldoClienteAnterior = BloqueoSaldoClienteSelected
                            If Not ListaBloqueoSaldoClientes.Contains(BloqueoSaldoClienteSelected) Then
                                origen = "insert"
                                ListaBloqueoSaldoClientes.Add(BloqueoSaldoClienteSelected)
                            End If
                            IsBusy = True
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

                            IsBusy = False
                        End If
                    Else
                        If BloqueoSaldoClienteSelected.FechaBloqueo.Value.Year < 1900 Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha de bloqueo no puede ser menor al año de 1900", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        Dim origen = "update"
                        ErrorForma = ""
                        BloqueoSaldoClienteAnterior = BloqueoSaldoClienteSelected
                        If Not ListaBloqueoSaldoClientes.Contains(BloqueoSaldoClienteSelected) Then
                            origen = "insert"
                            ListaBloqueoSaldoClientes.Add(BloqueoSaldoClienteSelected)
                        End If
                        IsBusy = True
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

                        IsBusy = False
                    End If

                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al Eliminar registro",
                                            Me.ToString(), "TerminoEliminarEncabezado", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If



            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del saldo de BloqueoSaldoClientes",
                                            Me.ToString(), "TerminValidarSaldoBloqueo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub


    ''' <summary>
    ''' Carga el valor del parámetro CF_CF_UTILIZAPASIVA_A2 -- JCM20170728
    ''' 
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminotraerparametroPasiva(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroPasiva", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "SI" Then
                UtilizaPasiva = True
                MostrarCamposPasivo = Visibility.Visible
            Else
                UtilizaPasiva = False
                MostrarCamposPasivo = Visibility.Collapsed
            End If
        End If
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaBloqueoSaldoClientes As EntitySet(Of BloqueoSaldoCliente)
    Public Property ListaBloqueoSaldoClientes() As EntitySet(Of BloqueoSaldoCliente)
        Get
            Return _ListaBloqueoSaldoClientes
        End Get
        Set(ByVal value As EntitySet(Of BloqueoSaldoCliente))
            _ListaBloqueoSaldoClientes = value

            MyBase.CambioItem("ListaBloqueoSaldoClientes")
            MyBase.CambioItem("ListaBloqueoSaldoClientesPaged")
            If Not IsNothing(value) Then
                If IsNothing(BloqueoSaldoClienteAnterior) Then
                    BloqueoSaldoClienteSelected = _ListaBloqueoSaldoClientes.FirstOrDefault
                Else
                    BloqueoSaldoClienteSelected = BloqueoSaldoClienteAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBloqueoSaldoClientesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBloqueoSaldoClientes) Then
                Dim view = New PagedCollectionView(_ListaBloqueoSaldoClientes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _BloqueoSaldoClienteSelected As BloqueoSaldoCliente
    Public Property BloqueoSaldoClienteSelected() As BloqueoSaldoCliente
        Get
            Return _BloqueoSaldoClienteSelected
        End Get
        Set(ByVal value As BloqueoSaldoCliente)
            _BloqueoSaldoClienteSelected = value
            MyBase.CambioItem("BloqueoSaldoClienteSelected")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad Mostrar u oculta las opciones cuando este el parametro de maneja pasiva prendido o apagado
    ''' </summary>
    Private _MostrarCamposPasivo As Visibility = Visibility.Collapsed
    Public Property MostrarCamposPasivo() As Visibility
        Get
            Return _MostrarCamposPasivo

        End Get
        Set(value As Visibility)
            _MostrarCamposPasivo = value
            MyBase.CambioItem("MostrarCamposPasivo")
        End Set
    End Property



#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewBloqueoSaldoCliente As New BloqueoSaldoCliente
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewBloqueoSaldoCliente.IDComisionista = BloqueoSaldoClientePorDefecto.IDComisionista
            NewBloqueoSaldoCliente.IDSucComisionista = BloqueoSaldoClientePorDefecto.IDSucComisionista
            NewBloqueoSaldoCliente.IdRegistro = BloqueoSaldoClientePorDefecto.IdRegistro
            NewBloqueoSaldoCliente.IDComitente = BloqueoSaldoClientePorDefecto.IDComitente
            NewBloqueoSaldoCliente.MotivoBloqueoSaldo = BloqueoSaldoClientePorDefecto.MotivoBloqueoSaldo
            NewBloqueoSaldoCliente.ValorBloqueado = BloqueoSaldoClientePorDefecto.ValorBloqueado
            NewBloqueoSaldoCliente.Naturaleza = BloqueoSaldoClientePorDefecto.Naturaleza
            NewBloqueoSaldoCliente.FechaBloqueo = BloqueoSaldoClientePorDefecto.FechaBloqueo
            NewBloqueoSaldoCliente.DetalleBloqueo = BloqueoSaldoClientePorDefecto.DetalleBloqueo
            NewBloqueoSaldoCliente.Actualizacion = BloqueoSaldoClientePorDefecto.Actualizacion
            NewBloqueoSaldoCliente.Usuario = Program.Usuario
            NewBloqueoSaldoCliente.intIDCompania = BloqueoSaldoClientePorDefecto.intIDCompania
            NewBloqueoSaldoCliente.intIDEncargo = BloqueoSaldoClientePorDefecto.intIDEncargo
            BloqueoSaldoClienteAnterior = BloqueoSaldoClienteSelected
            BloqueoSaldoClienteSelected = NewBloqueoSaldoCliente
            MyBase.CambioItem("BloqueoSaldoClientes")
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
            dcProxy.BloqueoSaldoClientes.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.BloqueoSaldoClientesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBloqueoSaldoClientes, Nothing)
            Else
                dcProxy.Load(dcProxy.BloqueoSaldoClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBloqueoSaldoClientes, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDComitente <> String.Empty Or cb.Naturaleza <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.BloqueoSaldoClientes.Clear()
                BloqueoSaldoClienteAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDComitente = " &  cb.IDComitente.ToString() & " Naturaleza = " &  cb.Naturaleza.ToString() 
                dcProxy.Load(dcProxy.BloqueoSaldoClientesConsultarQuery(cb.IDComitente, cb.Naturaleza, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBloqueoSaldoClientes, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBloqueoSaldoCliente
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
            If UtilizaPasiva = False Then
                dcProxy.ValidarSaldoBloqueo(BloqueoSaldoClienteSelected.IDComitente, 0, BloqueoSaldoClienteSelected.FechaBloqueo, Program.Usuario, Program.HashConexion, AddressOf TerminValidarSaldoBloqueo, "")
            Else
                If Not IsNothing(BloqueoSaldoClienteSelected.intIDEncargo) Then
                    dcProxy.ValidarSaldoBloqueo(BloqueoSaldoClienteSelected.IDComitente, BloqueoSaldoClienteSelected.intIDEncargo, BloqueoSaldoClienteSelected.FechaBloqueo, Program.Usuario, Program.HashConexion, AddressOf TerminValidarSaldoBloqueo, "")
                Else
                    dcProxy.ValidarSaldoBloqueo(BloqueoSaldoClienteSelected.IDComitente, 0, BloqueoSaldoClienteSelected.FechaBloqueo, Program.Usuario, Program.HashConexion, AddressOf TerminValidarSaldoBloqueo, "")
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
            MyBase.QuitarFiltroDespuesGuardar()
            IsBusy = True
            dcProxy.BloqueoSaldoClientes.Clear()
            dcProxy.Load(dcProxy.BloqueoSaldoClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBloqueoSaldoClientes, "insert")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        'If Not IsNothing(_BloqueoSaldoClienteSelected) Then
        '    Editando = True
        'End If
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_BloqueoSaldoClienteSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _BloqueoSaldoClienteSelected.EntityState = EntityState.Detached Then
                    BloqueoSaldoClienteSelected = BloqueoSaldoClienteAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        '  Try
        '      If Not IsNothing(_BloqueoSaldoClienteSelected) Then
        '          dcProxy.BloqueoSaldoClientes.Remove(_BloqueoSaldoClienteSelected)
        '          IsBusy = True
        '          dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
        '      End If
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '       Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    End Sub
    Public Overrides Sub Buscar()
        cb.IDComitente = String.Empty
        cb.Naturaleza = String.Empty
        cb.NombreComitente = String.Empty
        cb.intiDCompania = String.Empty
        cb.strNombreCompania = String.Empty
        cb.intIDEncargo = String.Empty
        cb.strDetalleEncargo = String.Empty
        MyBase.Buscar()
    End Sub

    Public Async Sub ObtenerValorSaldoEncargo()
        Try
            If UtilizaPasiva = True Then
                If BloqueoSaldoClienteSelected.intIDEncargo > 0 Then
                    Dim objRet As InvokeOperation(Of Double)


                    objRet = Await dcProxy.ValidarSaldoEncargo(BloqueoSaldoClienteSelected.intIDEncargo, BloqueoSaldoClienteSelected.FechaBloqueo, Program.Usuario).AsTask

                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then

                        'si no hay saldo retorna 0
                        If objRet.Value.ToString() = "0" Then
                            BloqueoSaldoClienteSelected.ValorBloqueado = 0
                        Else
                            BloqueoSaldoClienteSelected.ValorBloqueado = objRet.Value
                        End If

                    End If
                Else
                    BloqueoSaldoClienteSelected.ValorBloqueado = 0

                End If
            Else
                BloqueoSaldoClienteSelected.ValorBloqueado = 0
            End If



        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del saldo del encargo",
                                 Me.ToString(), "ObtenerValorSaldoEncargo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarCamposPasivo(pstrTipo As String)
        If UtilizaPasiva = True Then
            BloqueoSaldoClienteSelected.intIDEncargo = Nothing
            BloqueoSaldoClienteSelected.strDetalleEncargo = Nothing
            BloqueoSaldoClienteSelected.IDComitente = Nothing
            BloqueoSaldoClienteSelected.NombreComitente = Nothing

            If pstrTipo.ToLower = "compania" Then
                BloqueoSaldoClienteSelected.intIDCompania = Nothing
                BloqueoSaldoClienteSelected.strNombreCompania = Nothing
            End If



        End If
    End Sub


#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBloqueoSaldoCliente
    Implements INotifyPropertyChanged

    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")>
    <Display(Name:="IDComitente", Description:="IDComitente")>
    Private Property _IDComitente As String
    Public Property IDComitente As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property


    Private _NombreComitente As String
    Public Property NombreComitente() As String
        Get
            Return _NombreComitente
        End Get
        Set(ByVal value As String)
            _NombreComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreComitente"))
        End Set
    End Property


    Private _Naturaleza As String
    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")>
    <Display(Name:="Naturaleza")>
    Public Property Naturaleza As String
        Get
            Return _Naturaleza
        End Get
        Set(ByVal value As String)
            _Naturaleza = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Naturaleza"))
        End Set
    End Property

    <StringLength(4, ErrorMessage:="La longitud máxima es de 4")>
    <Display(Name:="intiDCompania", Description:="intiDCompania")>
    Private Property _intiDCompania As String
    Public Property intiDCompania As String
        Get
            Return _intiDCompania
        End Get
        Set(ByVal value As String)
            _intiDCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intiDCompania"))
        End Set
    End Property

    Private _strNombreCompania As String
    Public Property strNombreCompania() As String
        Get
            Return _strNombreCompania
        End Get
        Set(ByVal value As String)
            _strNombreCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreCompania"))
        End Set
    End Property


    <StringLength(4, ErrorMessage:="La longitud máxima es de 4")>
    <Display(Name:="intIDEncargo", Description:="intIDEncargo")>
    Private Property _intIDEncargo As String
    Public Property intIDEncargo As String
        Get
            Return _intIDEncargo
        End Get
        Set(ByVal value As String)
            _intIDEncargo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDEncargo"))
        End Set
    End Property

    Private _strDetalleEncargo As String
    Public Property strDetalleEncargo() As String
        Get
            Return _strDetalleEncargo
        End Get
        Set(ByVal value As String)
            _strDetalleEncargo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDetalleEncargo"))
        End Set
    End Property


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




