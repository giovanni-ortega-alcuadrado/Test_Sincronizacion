Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: EmisoresViewModel.vb
'Generado el : 04/19/2011 10:28:38
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
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports System.Text.RegularExpressions

''' <history>
''' Modificado por   : Juan Carlos Soto Cruz (JCS).
''' Fecha            : Junio 27/2013
''' Descripción      : Se retira todo lo referente a Tipos Emisores de este maestro, lo anterior por que se detecta que la funcionalidad que proporcionaria este campo ya existia en el maestro de emisores.
''' </history>
Public Class EmisoresViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaEmisore
    Private EmisorePorDefecto As Emisore
    Private EmisoreAnterior As Emisore
    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim dcProxyMaestros As MaestrosCFDomainContext

    ''' <history>
    ''' Modificado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Mayo 27/2013
    ''' Descripción      : Se adiciona la consulta de Calificadoras.
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Mayo 27/2013 - Resultado Ok
    '''  
    ''' Modificado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Junio 27/2013
    ''' Descripción      : Se retira la consulta TiposEmisoresFiltrarQuery, lo anterior por que se detecta que la funcionalidad que proporcionaria este campo ya existia en el maestro de emisores.
    ''' </history>
    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            dcProxyMaestros = New MaestrosCFDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            dcProxyMaestros = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True

                dcProxyMaestros.Load(dcProxyMaestros.ClasificacionesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, Nothing)
                dcProxyMaestros.Load(dcProxyMaestros.TiposEntidadFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposEntidad, Nothing)
                dcProxyMaestros.Load(dcProxyMaestros.Codigos_CIIUFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosCii, Nothing)

                ' JCS Mayo 27/2013
                dcProxyMaestros.Load(dcProxyMaestros.FiltrarCalificadorasQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCalificadoras, Nothing)
                ' FIN JCS

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EmisoresViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerEmisoresPorDefecto_Completed(ByVal lo As LoadOperation(Of Emisore))
        If Not lo.HasError Then
            EmisorePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Emisore por defecto", _
                                             Me.ToString(), "TerminoTraerEmisorePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEmisores(ByVal lo As LoadOperation(Of Emisore))
        If Not lo.HasError Then
            ListaEmisores = dcProxy.Emisores
            If dcProxy.Emisores.Count > 0 Then
                If lo.UserState = "insert" Then
                    EmisoreSelected = ListaEmisores.FirstOrDefault
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'Se comentan estas dos lineas para que no vuelva a regarcar la lista si no encuentra ningun registro.
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Emisores", Me.ToString, "TerminoTraerEspecie", lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerEspecies(ByVal lo As LoadOperation(Of Especie))
        If Not lo.HasError Then
            ListaEspecies = dcProxy.Especies
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Especies", _
                                             Me.ToString(), "TerminoTraerEspecies", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCiudades(ByVal lo As LoadOperation(Of Ciudade))
        If Not lo.HasError Then
            ListaCiudades = dcProxyMaestros.Ciudades
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ciudades", _
                                             Me.ToString(), "TerminoTraerCiudades", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDepartamentos(ByVal lo As LoadOperation(Of Departamento))
        If Not lo.HasError Then
            ListaDepartamentos = dcProxyMaestros.Departamentos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Departamentos", _
                                             Me.ToString(), "TerminoTraerDepartamentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClasificaciones(ByVal lo As LoadOperation(Of Clasificacion))
        Try
            If Not lo.HasError Then

                'ListaClasificacionesC = dcProxy.Clasificacions
                ListaClasificacionesC = dcProxyMaestros.Clasificacions
                '_EmisoreSelected.IDSubGrupo = EmisoreSelected.IDSubGrupo
                MyBase.CambioItem("EmisoreSelected")
                For Each lc In ListaClasificacionesC
                    'lc.IDClasificacion.ToString()
                    lc.Código.ToString()
                Next
                If ListaClasificacionesC.Count <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Este Grupo no tiene SubGrupos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                'EmisoreSelected.IDSubGrupo=(ListaClasificaciones.Where(Function(E) E.IDPerteneceA=EmisoreSelected.IDGrupo).FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificaciones", _
                                                 Me.ToString(), "TerminoTraerClasificaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificaciones", _
                                                 Me.ToString(), "TerminoTraerClasificaciones", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerTiposEntidad(ByVal lo As LoadOperation(Of TiposEntida))
        Try
            If Not lo.HasError Then
                ListaTiposEntidad = dcProxyMaestros.TiposEntidas

                If ListaTiposEntidad.Count <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existen tipos de entidad", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                dcProxy.Load(dcProxy.EmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerEmisorePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisoresPorDefecto_Completed, "Default")
                'EmisoreSelected.IDSubGrupo=(ListaClasificaciones.Where(Function(E) E.IDPerteneceA=EmisoreSelected.IDGrupo).FirstOrDefault
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

    Private Sub TerminoTraerCodigosCii(ByVal lo As LoadOperation(Of Codigos_CII))
        Try
            If Not lo.HasError Then
                ListaCodigosCii = dcProxyMaestros.Codigos_CIIs

                If ListaCodigosCii.Count <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existen códigos ciiu", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                'EmisoreSelected.IDSubGrupo=(ListaClasificaciones.Where(Function(E) E.IDPerteneceA=EmisoreSelected.IDGrupo).FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de códigos ciiu", _
                                                 Me.ToString(), "TerminoTraerCodigosCii", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de códigos ciiu", _
                                                 Me.ToString(), "TerminoTraerCodigosCii", Application.Current.ToString(), Program.Maquina, ex.InnerException)

        End Try
    End Sub
    ''' <summary>
    ''' Operacion asincrona para finalizacion de consulta de Calificadoras.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz (JCS).
    ''' Fecha            : Mayo 27/2013
    ''' Descripción      : Creado.
    ''' </history>    
    Private Sub TerminoTraerCalificadoras(ByVal lo As LoadOperation(Of Calificadoras))
        Try
            If Not lo.HasError Then

                ListaCalificadoras = dcProxyMaestros.Calificadoras

                If ListaCalificadoras.Count <= 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No existen Calificadoras", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Calificadoras", _
                                                 Me.ToString(), "TerminoTraerCalificadoras", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Calificadoras", _
                                                 Me.ToString(), "TerminoTraerCalificadoras", Application.Current.ToString(), Program.Maquina, ex.InnerException)

        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaEmisores As EntitySet(Of Emisore)
    Public Property ListaEmisores() As EntitySet(Of Emisore)
        Get
            Return _ListaEmisores
        End Get
        Set(ByVal value As EntitySet(Of Emisore))
            _ListaEmisores = value

            MyBase.CambioItem("ListaEmisores")
            MyBase.CambioItem("ListaEmisoresPaged")
            If Not IsNothing(value) Then
                If IsNothing(EmisoreAnterior) Then
                    EmisoreSelected = _ListaEmisores.FirstOrDefault
                Else
                    EmisoreSelected = EmisoreAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaEmisoresPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEmisores) Then
                Dim view = New PagedCollectionView(_ListaEmisores)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _EmisoreSelected As Emisore
    Public Property EmisoreSelected() As Emisore
        Get
            Return _EmisoreSelected
        End Get
        Set(ByVal value As Emisore)
            _EmisoreSelected = value
            If Not value Is Nothing Then
                dcProxy.Especies.Clear()
                dcProxyMaestros.Ciudades.Clear()
                dcProxyMaestros.Departamentos.Clear()
                
                If Not IsNothing(ListaClasificaciones) Then
                    ListaClasificaciones.Clear()
                End If
                If Not IsNothing(ListaClasificacionesC) Then
                    ListaClasificaciones = ListaClasificacionesC.Where(Function(di) di.EsGrupo = False And di.IDPerteneceA = IIf(IsNothing(value.IDGrupo), 0, value.IDGrupo)).ToList
                    _EmisoreSelected.IDSubGrupo = value.IDSubGrupo
                    MyBase.CambioItem("ListaClasificaciones")
                End If
                buscarItem("ciudades")
                dcProxy.Load(dcProxy.Traer_Especies_EmisoreQuery(value.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "ConsultaEspecies")
                dcProxyMaestros.Load(dcProxyMaestros.CiudadesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCiudades, "ConsultaCiudades")
                dcProxyMaestros.Load(dcProxyMaestros.DepartamentosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, "ConsultaDepartamentos")

                'dcProxy.Load(dcProxy.PaisesFiltrarQuery(""), AddressOf TerminoTraerPaises, "ConsultaPaises")

                intIDTipoEntidad = _EmisoreSelected.intIdTipoEntidad

            End If
                MyBase.CambioItem("EmisoreSelected")
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

    Private _ListaCiudades As IEnumerable(Of Ciudade)
    Public Property ListaCiudades() As IEnumerable(Of Ciudade)
        Get
            Return _ListaCiudades
        End Get
        Set(ByVal value As IEnumerable(Of Ciudade))
            _ListaCiudades = value
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

    Private _CiudadesClaseSelected As CiudadesClase = New CiudadesClase
    Public Property CiudadesClaseSelected As CiudadesClase
        Get
            Return _CiudadesClaseSelected
        End Get
        Set(ByVal value As CiudadesClase)
            _CiudadesClaseSelected = value
            MyBase.CambioItem("CiudadesClaseSelected")
        End Set
    End Property

    Private _intIDTipoEntidad As Nullable(Of Integer)
    Public Property intIDTipoEntidad() As Nullable(Of Integer)
        Get
            Return _intIDTipoEntidad
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _intIDTipoEntidad = value
            MyBase.CambioItem("intIDTipoEntidad")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewEmisore As New Emisore
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewEmisore.IDComisionista = EmisorePorDefecto.IDComisionista
            NewEmisore.IDSucComisionista = EmisorePorDefecto.IDSucComisionista
            NewEmisore.ID = EmisorePorDefecto.ID
            NewEmisore.Nombre = EmisorePorDefecto.Nombre
            NewEmisore.NIT = EmisorePorDefecto.NIT
            NewEmisore.Telefono1 = EmisorePorDefecto.Telefono1
            NewEmisore.Telefono2 = EmisorePorDefecto.Telefono2
            NewEmisore.Fax1 = EmisorePorDefecto.Fax1
            NewEmisore.Fax2 = EmisorePorDefecto.Fax2
            NewEmisore.Direccion = EmisorePorDefecto.Direccion
            NewEmisore.Internet = EmisorePorDefecto.Internet
            NewEmisore.EMail = EmisorePorDefecto.EMail
            NewEmisore.IDGrupo = Nothing
            NewEmisore.IDSubGrupo = Nothing
            NewEmisore.IDPoblacion = EmisorePorDefecto.IDPoblacion
            NewEmisore.IDDepartamento = EmisorePorDefecto.IDDepartamento
            NewEmisore.IDPais = EmisorePorDefecto.IDPais
            NewEmisore.Actualizacion = Now
            NewEmisore.Usuario = EmisorePorDefecto.Usuario
            NewEmisore.Contacto = EmisorePorDefecto.Contacto
            NewEmisore.Mostrar = EmisorePorDefecto.Mostrar
            NewEmisore.Total = EmisorePorDefecto.Total
            NewEmisore.Responde = EmisorePorDefecto.Responde
            NewEmisore.TipoComision = EmisorePorDefecto.TipoComision
            NewEmisore.IdEmisor = EmisorePorDefecto.IdEmisor
            NewEmisore.logActivo = EmisorePorDefecto.logActivo
            NewEmisore.VigiladoSuper = EmisorePorDefecto.VigiladoSuper
            NewEmisore.logEsPatrimonioAutonomo = EmisorePorDefecto.logEsPatrimonioAutonomo
            NewEmisore.FuenteExtranjero = EmisorePorDefecto.FuenteExtranjero
            EmisoreAnterior = EmisoreSelected
            EmisoreSelected = NewEmisore
            MyBase.CambioItem("Emisores")
            Editando = True
            MyBase.CambioItem("Editando")
            CiudadesClaseSelected.Ciudad = String.Empty
            CiudadesClaseSelected.Departamento = String.Empty
            CiudadesClaseSelected.Pais = String.Empty
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Emisores.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.EmisoresFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.EmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Emisores.Clear()
                EmisoreAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " Nombre = " & cb.Nombre.ToString()
                dcProxy.Load(dcProxy.EmisoresConsultarQuery(cb.ID, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaEmisore
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
            If Not IsNothing(EmisoreSelected) Then
                Select Case EmisoreSelected.IDPoblacion
                    Case 0
                        A2Utilidades.Mensajes.mostrarMensaje("Debe Seleccionar una Ciudad", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                End Select
            End If

            If EmisoreSelected.EMail <> "" Then
                If validar_Mail(EmisoreSelected.EMail) = False Then
                    A2Utilidades.Mensajes.mostrarMensaje("La dirección de correo electrónico ingresada no es válida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If IsNothing(EmisoreSelected.intIdCodigoCIIU) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un código CIIU.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

            If IsNothing(EmisoreSelected.IDGrupo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un grupo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(EmisoreSelected.IDSubGrupo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un grupo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            'EmisoreAnterior = EmisoreSelected
            If Not ListaEmisores.Contains(EmisoreSelected) Then
                origen = "insert"
                ListaEmisores.Add(EmisoreSelected)
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
                'TODO: Pendiente garantizar que Userstate no venga vacío

                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "BorrarRegistro") Or (So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                    'EmisoreSelected = EmisoreAnterior
                End If
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState = "BorrarRegistro" Then ' Se hace esto para cuando borre vuelva a traér toda la lista y llene el formulario y no se pierda  JRP 15/80/2012
                    MyBase.QuitarFiltroDespuesGuardar()
                    dcProxy.Load(dcProxy.EmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "FiltroInicial")
                End If
            End If

            'If So.UserState = "insert" Then
            '    dcProxy.Emisores.Clear()
            '    dcProxy.Load(dcProxy.EmisoresFiltrarQuery(""), AddressOf TerminoTraerEmisores, "insert") ' Recarga la lista para que carguen los include
            'End If

            MyBase.TerminoSubmitChanges(So)

            ' JCS Mayo 28/2013
            dcProxy1.Emisores.Clear()
            If Not IsNothing(ListaEmisores) Then
                ListaEmisores.Clear()
            End If
            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.EmisoresFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmisores, "FiltroInicial")
            ' FIN JCS

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EmisoreSelected) Then
            EmisoreSelected.TipoComision = "0"
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_EmisoreSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _EmisoreSelected.EntityState = EntityState.Detached Then
                    EmisoreSelected = EmisoreAnterior
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
            If Not IsNothing(_EmisoreSelected) Then
                EmisoreAnterior = _EmisoreSelected
                dcProxy.Emisores.Remove(_EmisoreSelected)
                EmisoreSelected = _ListaEmisores.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _EmisoreSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EmisoreSelected.PropertyChanged
        If Editando Then
            Dim DepartamentoId = From lc In ListaCiudades
                            Where lc.IDCodigo = EmisoreSelected.IDPoblacion
                            Select lc.IDdepartamento

            If DepartamentoId.Count > 0 Then
                EmisoreSelected.IDDepartamento = DepartamentoId.First
                Dim CodigoPais = From ld In ListaDepartamentos
                                 Where ld.ID = DepartamentoId.First
                                 Select ld.IDPais

                If CodigoPais.Count > 0 Then
                    EmisoreSelected.IDPais = CodigoPais.First
                End If
            End If
        End If

        Select Case e.PropertyName
            Case "IDGrupo"
                'dcProxy.Clasificacions.Clear()
                EmisoreSelected.IDSubGrupo.Equals("")
                If Not IsNothing(ListaClasificaciones) Then
                    ListaClasificaciones.Clear()
                End If
                ListaClasificaciones = ListaClasificacionesC.Where(Function(di) di.EsGrupo = False And di.IDPerteneceA = IIf(IsNothing(EmisoreSelected.IDGrupo), 0, EmisoreSelected.IDGrupo)).ToList
                MyBase.CambioItem("ListaClasificaciones")
                'dcProxy.Load(dcProxy.ClasificacionesConsultarQuery(0,
                '                                                   String.Empty,
                '                                                   String.Empty,
                '                                                   0,
                '                                                   0,
                '                                                   EmisoreSelected.IDGrupo),
                '                                                   AddressOf TerminoTraerClasificaciones, "")
                'dcProxy.Load(dcProxy.ClasificacionesFiltrarQuery(""), AddressOf TerminoTraerClasificaciones, "FiltroInicial")
                'A2Utilidades.Mensajes.mostrarMensaje("as", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

        End Select
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("IDPoblacion", 1)
        DicCamposTab.Add("IDDepartamento", 1)
        DicCamposTab.Add("IDPais", 1)
        DicCamposTab.Add("IDGrupo", 1)
        DicCamposTab.Add("IDSubGrupo", 1)
        DicCamposTab.Add("NIT", 1)
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.EmisoreSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "ciudades"


                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then

                            If Not IsNothing(Me.EmisoreSelected.IDPoblacion) Then
                                strIdItem = Me.EmisoreSelected.IDPoblacion
                            End If
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not strIdItem.Equals(String.Empty) Then
                            logConsultar = True
                        End If
                        If logConsultar Then
                            mdcProxyUtilidad01.BuscadorGenericos.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        End If
                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
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
                Select Case strTipoItem.ToLower()
                    Case "ciudades"
                        Me.CiudadesClaseSelected.Ciudad = lo.Entities.ToList.Item(0).Nombre
                        Me.CiudadesClaseSelected.Departamento = lo.Entities.ToList.Item(0).CodigoAuxiliar
                        Me.CiudadesClaseSelected.Pais = lo.Entities.ToList.Item(0).InfoAdicional02
                End Select
            Else
                Me.CiudadesClaseSelected.Ciudad = String.Empty
                Me.CiudadesClaseSelected.Departamento = String.Empty
                Me.CiudadesClaseSelected.Pais = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        cb.ID = Nothing
        cb.Nombre = String.Empty
        MyBase.Buscar()
    End Sub
#End Region

#Region "Tablas hijas"


    '******************************************************** Especies 
    Private _ListaEspecies As EntitySet(Of Especie)
    Public Property ListaEspecies() As EntitySet(Of Especie)
        Get
            Return _ListaEspecies
        End Get
        Set(ByVal value As EntitySet(Of Especie))
            _ListaEspecies = value
            MyBase.CambioItem("ListaEspecies")
            MyBase.CambioItem("ListaEspeciesPaged")
        End Set
    End Property

    Public ReadOnly Property EspeciesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspecies) Then
                Dim view = New PagedCollectionView(_ListaEspecies)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _EspecieSelected As Especie
    Public Property EspecieSelected() As Especie
        Get
            Return _EspecieSelected
        End Get
        Set(ByVal value As Especie)
            _EspecieSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("EspecieSelected")
            End If
        End Set
    End Property

    Private _ListaClasificaciones As List(Of Clasificacion)
    Public Property ListaClasificaciones() As List(Of Clasificacion)
        Get
            Return _ListaClasificaciones
        End Get
        Set(ByVal value As List(Of Clasificacion))
            _ListaClasificaciones = value
            MyBase.CambioItem("ListaClasificaciones")
        End Set
    End Property
    Private _ListaClasificacionesC As EntitySet(Of Clasificacion)
    Public Property ListaClasificacionesC() As EntitySet(Of Clasificacion)
        Get
            Return _ListaClasificacionesC
        End Get
        Set(ByVal value As EntitySet(Of Clasificacion))
            _ListaClasificacionesC = value
            MyBase.CambioItem("ListaClasificacionesC")
        End Set
    End Property

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

    Private _ListaCodigosCii As EntitySet(Of Codigos_CII)
    Public Property ListaCodigosCii() As EntitySet(Of Codigos_CII)
        Get
            Return _ListaCodigosCii
        End Get
        Set(ByVal value As EntitySet(Of Codigos_CII))
            _ListaCodigosCii = value
            MyBase.CambioItem("ListaCodigosCii")
        End Set
    End Property

    ''' <summary>
    ''' Lista para almacenar las calificadoras (Calculos Financieros).
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por   : Juan Carlos Soto Cruz (JCS).
    ''' Fecha        : Mayo 27/2013
    ''' Descripción  : Creado.
    ''' </history>    
    Private _ListaCalificadoras As EntitySet(Of Calificadoras)
    Public Property ListaCalificadoras() As EntitySet(Of Calificadoras)
        Get
            Return _ListaCalificadoras
        End Get
        Set(ByVal value As EntitySet(Of Calificadoras))
            _ListaCalificadoras = value
            MyBase.CambioItem("ListaCalificadoras")
        End Set
    End Property

    Private Function validar_Mail(ByVal sMail As String) As Boolean
        ' retorna true o false   
        Return Regex.IsMatch(EmisoreSelected.EMail, _
                "^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$")
    End Function
    'Public Overrides Sub NuevoRegistroDetalle()
    '    Select Case NombreColeccionDetalle
    '        Case "cmEspecie"
    '            Dim NewEspecie As New Especie
    '            NewEspecie.IdEmisor = EmisoreSelected.ID
    '            ListaEspecies.Add(NewEspecie)
    '            EspecieSelected = NewEspecie
    '            MyBase.CambioItem("EspecieSelected")
    '            MyBase.CambioItem("ListaEspecie")

    '    End Select
    'End Sub

    'Public Overrides Sub BorrarRegistroDetalle()
    '    Select Case NombreColeccionDetalle
    '        Case "cmEspecie"
    '            If Not IsNothing(ListaEspecies) Then
    '                If Not IsNothing(ListaEspecies) Then
    '                    ListaEspecies.Remove(_EspecieSelected)
    '                    If ListaEspecies.Count > 0 Then
    '                        EspecieSelected = ListaEspecies.FirstOrDefault
    '                    End If
    '                    MyBase.CambioItem("EspecieSelected")
    '                    MyBase.CambioItem("ListaEspecies")
    '                End If
    '            End If

    '    End Select
    'End Sub
#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaEmisore

    Implements INotifyPropertyChanged
    Private _ID As Integer
    <Display(Name:="Código")> _
    Public Property ID As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property
    Private _Nombre As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

'Clase base para el builder de Ciudad
Public Class CiudadesEmisores
    Implements INotifyPropertyChanged


    Private _Ciudad As String
    <Display(Name:="Ciudad")> _
    Public Property Ciudad As String
        Get
            Return _Ciudad
        End Get
        Set(ByVal value As String)
            _Ciudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Ciudad"))
        End Set
    End Property


    Private _Departamento As String
    <Display(Name:="Departamento")> _
    Public Property Departamento As String
        Get
            Return _Departamento
        End Get
        Set(ByVal value As String)
            _Departamento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Departamento"))
        End Set
    End Property


    Private _Pais As String
    <Display(Name:="País")> _
    Public Property Pais As String
        Get
            Return _Pais
        End Get
        Set(ByVal value As String)
            _Pais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Pais"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


