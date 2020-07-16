Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ComisionistasViewModel.vb
'Generado el : 03/02/2011 17:36:04
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

Public Class ComisionistasViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaComisionista
	Private ComisionistaPorDefecto As Comisionista
    Private ComisionistaAnterior As Comisionista
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim strciudad, strdepartamento, strpais As String
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Mayo 28/2013
    ''' Descripción      : Se adiciona la consulta de Tipos de Entidad.
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Mayo 28/2013 - Resultado Ok 
    ''' </history>
    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ComisionistasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionistas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerComisionistaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionistasPorDefecto_Completed, "Default")
                dcProxy.Load(dcProxy.CiudadesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "ConsultaCiudades")
                dcProxy.Load(dcProxy.DepartamentosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, "ConsultaDepartamentos")

                ' JCS Mayo 28/2013
                dcProxy.Load(dcProxy.TiposEntidadFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, Nothing)
                ' FIN JCS

                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ComisionistasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ComisionistasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerComisionistasPorDefecto_Completed(ByVal lo As LoadOperation(Of Comisionista))
        If Not lo.HasError Then
            ComisionistaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Comisionista por defecto", _
                                             Me.ToString(), "TerminoTraerComisionistaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerComisionistas(ByVal lo As LoadOperation(Of Comisionista))
        If Not lo.HasError Then
            ListaComisionistas = dcProxy.Comisionistas
            If dcProxy.Comisionistas.Count > 0 Then
                If lo.UserState = "insert" Then
                    ComisionistaSelected = ListaComisionistas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Comisionistas", _
                                             Me.ToString(), "TerminoTraerComisionista", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerCiudades(ByVal lo As LoadOperation(Of Ciudade))
        If Not lo.HasError Then
            ListaCiudades = dcProxy.Ciudades
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ciudades", _
                                             Me.ToString(), "TerminoTraerCiudades", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDepartamentos(ByVal lo As LoadOperation(Of Departamento))
        If Not lo.HasError Then
            ListaDepartamentos = dcProxy.Departamentos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Departamentos", _
                                             Me.ToString(), "TerminoTraerDepartamentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Me.CiudadSelected.strPoblacion = lo.Entities.ToList.Item(0).Nombre
                Me.CiudadSelected.strdepartamento = lo.Entities.ToList.Item(0).CodigoAuxiliar
                Me.CiudadSelected.strPais = lo.Entities.ToList.Item(0).InfoAdicional02
                strciudad = lo.Entities.ToList.Item(0).Nombre
                strdepartamento = lo.Entities.ToList.Item(0).CodigoAuxiliar
                strpais = lo.Entities.ToList.Item(0).InfoAdicional02
            Else
                Me.CiudadSelected.strPoblacion = String.Empty
                Me.CiudadSelected.strdepartamento = String.Empty
                Me.CiudadSelected.strPais = String.Empty
                strciudad = String.Empty
                strdepartamento = String.Empty
                strpais = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "Borrar" Then
                    dcProxy.Comisionistas.Clear()
                    dcProxy.Load(dcProxy.ComisionistasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionistas, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Operacion asincrona para finalizacion de consulta de Tipos de Entidad.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Mayo 28/2013
    ''' Descripción      : Creado.
    ''' </history> 
    Private Sub TerminoTraerTiposEntidad(ByVal lo As LoadOperation(Of TiposEntida))
        Try
            If Not lo.HasError Then
                ListaTiposEntidad = dcProxy.TiposEntidas

                If ListaTiposEntidad.Count <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existen tipos de entidad", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de tipos entidad", _
                                                 Me.ToString(), "TerminoTraerTiposEntidad", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de tipos entidad", _
                                                 Me.ToString(), "TerminoTraerTiposEntidad", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub
#End Region

#Region "Propiedades"

    Private _ListaComisionistas As EntitySet(Of Comisionista)
    Public Property ListaComisionistas() As EntitySet(Of Comisionista)
        Get
            Return _ListaComisionistas
        End Get
        Set(ByVal value As EntitySet(Of Comisionista))
            _ListaComisionistas = value
            MyBase.CambioItem("ListaComisionistas")
            MyBase.CambioItem("ListaComisionistasPaged")
            If Not IsNothing(value) Then
                If IsNothing(ComisionistaAnterior) Then
                    ComisionistaSelected = _ListaComisionistas.FirstOrDefault
                Else
                    'ComisionistaSelected = ComisionistaAnterior
                    If Not ComisionistaSelected.IDComisionista.Equals(_ListaComisionistas.FirstOrDefault.IDComisionista) Then
                        ComisionistaSelected = _ListaComisionistas.FirstOrDefault
                    Else
                        ComisionistaSelected = ComisionistaAnterior
                    End If
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaComisionistasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaComisionistas) Then
                Dim view = New PagedCollectionView(_ListaComisionistas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ComisionistaSelected As Comisionista
    Public Property ComisionistaSelected() As Comisionista
        Get
            Return _ComisionistaSelected
        End Get
        Set(ByVal value As Comisionista)
            _ComisionistaSelected = value
            If Not value Is Nothing Then
                '_ComisionistaSelected = value
                buscarItem("ciudadesdoc")
            End If
            MyBase.CambioItem("ComisionistaSelected")
        End Set
    End Property

    Private _ListaCiudades As IEnumerable(Of Ciudade)
    Public Property ListaCiudades() As IEnumerable(Of Ciudade)
        Get
            Return _ListaCiudades
        End Get
        Set(ByVal value As IEnumerable(Of Ciudade))
            _ListaCiudades = value
        End Set
    End Property

    Private _ListaDepartamentos As IEnumerable(Of Departamento)
    Public Property ListaDepartamentos() As IEnumerable(Of Departamento)
        Get
            Return _ListaDepartamentos
        End Get
        Set(ByVal value As IEnumerable(Of Departamento))
            _ListaDepartamentos = value
        End Set
    End Property

    Private _CiudadSelected As Ciudadseleccionada = New Ciudadseleccionada
    Public Property CiudadSelected As Ciudadseleccionada
        Get
            Return _CiudadSelected
        End Get
        Set(ByVal value As Ciudadseleccionada)
            _CiudadSelected = value
            MyBase.CambioItem("CiudadSelected")
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


    Private _habilitar As Boolean = False
    Public Property habilitar() As Boolean
        Get
            Return _habilitar
        End Get
        Set(ByVal value As Boolean)
            _habilitar = value
            MyBase.CambioItem("habilitar")
        End Set
    End Property

    ''' <summary>
    ''' Lista para almacenar los Tipos de Entidad (Calculos Financieros).
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha        : Mayo 28/2013
    ''' Descripción  : Creado.
    ''' </history>
    Private _ListaTiposEntidad As EntitySet(Of TiposEntida)
    Public Property ListaTiposEntidad() As EntitySet(Of TiposEntida)
        Get
            Return _ListaTiposEntidad
        End Get
        Set(ByVal value As EntitySet(Of TiposEntida))
            _ListaTiposEntidad = value
            MyBase.CambioItem("ListaTiposEntidad")
        End Set
    End Property
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewComisionista As New Comisionista
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewComisionista.IdBolsa = Nothing
            NewComisionista.ID = 0
            NewComisionista.NroDocumento = ComisionistaPorDefecto.NroDocumento
            NewComisionista.Nombre = ComisionistaPorDefecto.Nombre
            NewComisionista.RepresentanteLegal = ComisionistaPorDefecto.RepresentanteLegal
            NewComisionista.Telefono1 = ComisionistaPorDefecto.Telefono1
            NewComisionista.Telefono2 = ComisionistaPorDefecto.Telefono2
            NewComisionista.Fax1 = ComisionistaPorDefecto.Fax1
            NewComisionista.Fax2 = ComisionistaPorDefecto.Fax2
            NewComisionista.Direccion = ComisionistaPorDefecto.Direccion
            NewComisionista.Internet = ComisionistaPorDefecto.Internet
            NewComisionista.EMail = ComisionistaPorDefecto.EMail
            'NewComisionista.IDPoblacion = ComisionistaPorDefecto.IDPoblacion
            NewComisionista.IDPoblacion = Nothing
            NewComisionista.IDDepartamento = Nothing
            NewComisionista.IDPais = Nothing
            NewComisionista.Notas = ComisionistaPorDefecto.Notas
            NewComisionista.Actualizacion = ComisionistaPorDefecto.Actualizacion
            NewComisionista.Usuario = Program.Usuario
            NewComisionista.IDComisionista = ComisionistaPorDefecto.IDComisionista
            ComisionistaAnterior = ComisionistaSelected
            ComisionistaSelected = NewComisionista
            MyBase.CambioItem("Comisionistas")
            Editando = True
            MyBase.CambioItem("Editando")
            CiudadSelected.strdepartamento = String.Empty
            CiudadSelected.strPais = String.Empty
            CiudadSelected.strPoblacion = String.Empty
            PropiedadTextoCombos = ""
            habilitar = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Comisionistas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ComisionistasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionistas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ComisionistasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionistas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Or cb.pstrRepresentanteLegal <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Comisionistas.Clear()
                ComisionistaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.ComisionistasConsultarQuery(cb.ID, cb.Nombre, cb.pstrRepresentanteLegal,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComisionistas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaComisionista
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
            If ComisionistaSelected.ID = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Código es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If ComisionistaSelected.IdBolsa = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La Bolsa es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaComisionistas.Count = 0 Then
            Else
                For Each led In ListaComisionistas
                    Dim Nombre = ComisionistaSelected.Nombre
                    Dim Bolsa = ComisionistaSelected.IdBolsa
                    Dim Repetida = From ld In ListaComisionistas Where ld.Nombre.ToUpper = Nombre.ToUpper And ld.IdBolsa = Bolsa
                                              Select ld

                    If Repetida.Count > 1 Then
                        A2Utilidades.Mensajes.mostrarMensaje("El Nombre " + ComisionistaSelected.Nombre + " Con la Bolsa seleccionada Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
            End If

            If ComisionistaSelected.NroDocumento = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Nit es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If ComisionistaSelected.IDPoblacion = 0 Or ComisionistaSelected.IDPoblacion = Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("La ciudad es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            'ComisionistaAnterior = ComisionistaSelected
            If Not ListaComisionistas.Contains(ComisionistaSelected) Then
                origen = "insert"
                ListaComisionistas.Add(ComisionistaSelected)
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

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Mayo 28/2013
    ''' Descripción      : Se adicionan lineas de codigo necesarias para recargar la informacion del grid despues de insertar o actualizar.
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Mayo 28/2013 - Resultado Ok 
    ''' </history>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
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
   
            MyBase.TerminoSubmitChanges(So)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ComisionistaSelected) Then
            Editando = True
            habilitar = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ComisionistaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ComisionistaSelected.EntityState = EntityState.Detached Then
                    ComisionistaSelected = ComisionistaAnterior
                End If
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ComisionistaSelected) Then
                'dcProxy.Comisionistas.Remove(_ComisionistaSelected)
                'ComisionistaSelected = _ListaComisionistas.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarComisionista(ComisionistaSelected.IDComisionista, String.Empty,Program.Usuario, Program.HashConexion, AddressOf TerminoEliminar, "Borrar")

            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ComisionistaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ComisionistaSelected.PropertyChanged
        If Editando Then
            Dim DepartamentoId = From lc In ListaCiudades
                            Where lc.IDCodigo = ComisionistaSelected.IDPoblacion
                            Select lc.IDdepartamento

            If DepartamentoId.Count > 0 Then
                ComisionistaSelected.IDDepartamento = DepartamentoId.First
                Dim CodigoPais = From ld In ListaDepartamentos
                                 Where ld.ID = DepartamentoId.First
                                 Select ld.IDPais

                If CodigoPais.Count > 0 Then
                    ComisionistaSelected.IDPais = CodigoPais.First
                End If
            End If
        End If
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.ComisionistaSelected Is Nothing Then
                If Not IsNothing(ComisionistaSelected.IDPoblacion) Then
                    pstrIdItem = pstrIdItem.Trim()
                    If pstrIdItem.Equals(String.Empty) Then
                        strIdItem = Me.ComisionistaSelected.IDPoblacion
                    Else
                        strIdItem = pstrIdItem
                    End If
                    If Not strIdItem.Equals(String.Empty) Then
                        logConsultar = True
                    Else
                        logConsultar = False
                    End If
                    If logConsultar Then
                        mdcProxyUtilidad01.BuscadorGenericos.Clear()
                        mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery("ciudades", strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la ciudad", Me.ToString(), "Buscar ciudad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaComisionista
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaComisionista
 	
    <Display(Name:="Código")> _
    Public Property ID As Integer
 	
    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <Display(Name:="Representante Legal")> _
    Public Property pstrRepresentanteLegal As String
End Class

Public Class Ciudadseleccionada
    Implements INotifyPropertyChanged

    Private _strPoblacion As String
    <Display(Name:="Ciudad")> _
    Public Property strPoblacion As String
        Get
            Return _strPoblacion
        End Get
        Set(ByVal value As String)
            _strPoblacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPoblacion"))
        End Set
    End Property
    Private _strdepartamento As String
    <Display(Name:="Departamento")> _
    Public Property strdepartamento As String
        Get
            Return _strdepartamento
        End Get
        Set(ByVal value As String)
            _strdepartamento = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strdepartamento"))

        End Set
    End Property
    Private _strPais As String
    <Display(Name:="Pais")> _
    Public Property strPais As String
        Get
            Return _strPais
        End Get
        Set(ByVal value As String)
            _strPais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPais"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


