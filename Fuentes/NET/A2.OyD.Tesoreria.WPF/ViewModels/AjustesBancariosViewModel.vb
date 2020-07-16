Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: BolsasViewModel.vb
'Generado el : 02/09/2011 11:50:52
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class AjustesBancariosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaAjustesBancarios
    Private AjustesBancariosPorDefecto As AjustesBancario
    Private AjustesBancariosAnterior As AjustesBancario
    Public NombreDiccionarioCombosAjustes As String = "Combos_A2OYDTesoreria.AjustesBancariosView"
    Dim dcProxy As TesoreriaDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim strNombreConsecutivo As String = String.Empty
    Dim fechaCierre, fechadocumento As DateTime


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()
            dcProxy1 = New TesoreriaDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            'dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            'dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If


        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.AjustesBancariosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAjustesBancarios, "FiltroInicial")
                dcProxy1.Load(dcProxy1.Traer_AjustesBancariosPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAjustesBancariosPorDefecto_Completed, "Default")
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then

                'End If
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  BolsasViewModel)(Me)
            End If
            strNombreConsecutivo = "ConsecutivosAjustes"
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "AjustesBancariosViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerAjustesBancariosPorDefecto_Completed(ByVal lo As LoadOperation(Of AjustesBancario))
        If Not lo.HasError Then
            AjustesBancariosPorDefecto = lo.Entities.FirstOrDefault
            'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
            '    listapoblacion = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Ciudades").ToList
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los ajustes bancarios por defecto",
                                             Me.ToString(), "TerminoTraerAjustesBancariosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerAjustesBancarios(ByVal lo As LoadOperation(Of AjustesBancario))
        If Not lo.HasError Then
            Try
                ListaAjustesBancarios = dcProxy.AjustesBancarios
                If dcProxy.AjustesBancarios.Count > 0 Then
                    If lo.UserState = "insert" Then
                        AjustesBancarioSelected = ListaAjustesBancarios.Last
                    End If
                    If Application.Current.Resources.Contains(NombreDiccionarioCombosAjustes) Then
                        If CType(Application.Current.Resources(NombreDiccionarioCombosAjustes), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(strNombreConsecutivo) Then
                            listConsecutivos = CType(Application.Current.Resources(NombreDiccionarioCombosAjustes), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                        End If
                    End If
                Else
                    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'Se comentan estas dos lineas para que no vuelva a regarcar la lista si no encuentra ningun registro.
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de tesoreria",
                                     Me.ToString(), "TerminoTraerAjustesBancarios", Application.Current.ToString(), Program.Maquina, ex.InnerException)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de tesoreria",
                                             Me.ToString(), "TerminoTraerAjustesBancarios", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
        IsBusy = False
    End Sub
    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                If objProxy.ItemCombos.Count > 0 Then
                    Dim hayConsecutivos As Boolean = False
                    listConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = strNombreConsecutivo).ToList
                    If listConsecutivos.Count <> 0 Then
                        If lo.UserState <> "Nuevo" Then
                            For Each Consecutivo In listConsecutivos
                                If Consecutivo.ID = AjustesBancarioSelected.NombreConsecutivo Then
                                    hayConsecutivos = True
                                End If
                            Next
                        Else
                            hayConsecutivos = True
                        End If
                    Else
                        hayConsecutivos = False
                    End If
                    If hayConsecutivos = True Then
                        If lo.UserState = "Nuevo" Then
                            Nuevo()
                        ElseIf lo.UserState = "Editar" Then
                            Editar()
                        ElseIf lo.UserState = "Borrar" Then
                            Borrar()
                        End If
                    Else
                        If lo.UserState = "Editar" Then
                            MyBase.RetornarValorEdicionNavegacion()
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje("Señor usuario, usted no posee permisos para realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        listConsecutivos = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                    End If
                Else
                    If lo.UserState = "Editar" Then
                        MyBase.RetornarValorEdicionNavegacion()
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("Señor usuario, usted no posee permisos para realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    listConsecutivos = CType(Application.Current.Resources(Program.NOMBRE_DICCIONARIO_COMBOSESPECIFICOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                End If

                'Else
                '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los documentos por aprobar.", _
                '                         Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        Try
            If obj.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                fechaCierre = obj.Value
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema",
                                                 Me.ToString(), "consultarFechaCierreCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#End Region

#Region "Propiedades"

    Private _ListaAjustesBancarios As EntitySet(Of AjustesBancario)
    Public Property ListaAjustesBancarios() As EntitySet(Of AjustesBancario)
        Get
            Return _ListaAjustesBancarios
        End Get
        Set(ByVal value As EntitySet(Of AjustesBancario))
            _ListaAjustesBancarios = value

            MyBase.CambioItem("ListaAjustesBancarios")
            MyBase.CambioItem("ListaAjustesBancariosPaged")
            If Not IsNothing(value) Then
                If IsNothing(AjustesBancariosAnterior) Then
                    AjustesBancarioSelected = _ListaAjustesBancarios.FirstOrDefault
                Else
                    AjustesBancarioSelected = AjustesBancariosAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaAjustesBancariosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAjustesBancarios) Then
                Dim view = New PagedCollectionView(_ListaAjustesBancarios)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _AjustesBancarioSelected As AjustesBancario
    Public Property AjustesBancarioSelected() As AjustesBancario
        Get
            Return _AjustesBancarioSelected
        End Get
        Set(ByVal value As AjustesBancario)
            _AjustesBancarioSelected = value
            'If Not value Is Nothing Then
            '    _BolsaSelected = value
            'End If
            MyBase.CambioItem("AjustesBancarioSelected")
        End Set
    End Property
    'Propiedad para cargar los Consecutivos de los usuarios.
    Private _listConsecutivos As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivos = value
            MyBase.CambioItem("listConsecutivos")
        End Set
    End Property
#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            ValidarUsuario("Tesoreria_Ajustesbancarios", "Nuevo")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub Nuevo()
        Try
            Dim NewAjustesBancario As New AjustesBancario
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewAjustesBancario.IDBanco = AjustesBancariosPorDefecto.IDBanco
            NewAjustesBancario.IDDocumento = AjustesBancariosPorDefecto.IDDocumento
            NewAjustesBancario.IDTesoreria = AjustesBancariosPorDefecto.IDTesoreria
            NewAjustesBancario.Impresiones = AjustesBancariosPorDefecto.Impresiones
            NewAjustesBancario.FecEstado = Now
            NewAjustesBancario.Documento = Now
            NewAjustesBancario.Estado = "P"

            AjustesBancarioSelected = NewAjustesBancario
            'If Not IsNothing(listapoblacion) Then
            '    listapoblacion.Clear()
            'End If
            'PropiedadTextoCombos = ""
            'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
            '    listapoblacion = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Ciudades").ToList
            'End If
            'MyBase.CambioItem("listapoblacion")
            'BolsaSelected.IDPoblacion = Nothing
            MyBase.CambioItem("AjustesBancarioSelected")
            Editando = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.AjustesBancarios.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.AjustesBancariosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAjustesBancarios, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.AjustesBancariosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAjustesBancarios, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        'cb.DisplayDate = Now.Date
        'cb.Documento = Nothing
        'cb.Tipo = String.Empty
        'cb.NombreConsecutivo = String.Empty
        'cb.IDDocumento = 0
        'cb.NombreBanco = String.Empty
        cb = New CamposBusquedaAjustesBancarios()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDBanco <> 0 Or cb.Tipo <> String.Empty Or cb.IDDocumento <> 0 Or cb.NombreConsecutivo <> String.Empty Or Not IsNothing(cb.Documento) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.AjustesBancarios.Clear()
                AjustesBancariosAnterior = Nothing
                IsBusy = True
                If IsNothing(cb.Documento) Then
                    fechadocumento = Nothing
                Else
                    fechadocumento = cb.Documento
                End If
                'DescripcionFiltroVM = " IdBolsa = " &  cb.IdBolsa.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.AjustesBancariosConsultarQuery(cb.Tipo, cb.NombreConsecutivo, cb.IDDocumento, IIf(fechadocumento.Date.Year < 1800, "1799/01/01", fechadocumento), cb.IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAjustesBancarios, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaAjustesBancarios
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
            'If BolsaSelected.NroDocumento = 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("El Nit de la bolsa es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    IsBusy = False
            '    Exit Sub
            'End If
            'If BolsaSelected.IDPoblacion = 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("La ciudad de la bolsa es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    IsBusy = False
            'Else
            Dim origen = "update"
            ErrorForma = ""
            If AjustesBancarioSelected.Documento.ToShortDateString <= fechaCierre Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha del documento no puede ser menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If AjustesBancarioSelected.IDBanco = 0 Or IsNothing(AjustesBancarioSelected.IDBanco) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un banco", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If AjustesBancarioSelected.Valor = 0 Or IsNothing(AjustesBancarioSelected.Valor) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un Valor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If AjustesBancarioSelected.Tipo = String.Empty Or IsNothing(AjustesBancarioSelected.Tipo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la naturaleza débito o crédito del ajuste", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            AjustesBancariosAnterior = AjustesBancarioSelected
            If Not ListaAjustesBancarios.Contains(AjustesBancarioSelected) Then
                origen = "insert"
                ListaAjustesBancarios.Add(AjustesBancarioSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.Bolsas.Clear()
            '    dcProxy.Load(dcProxy.AjustesBancariosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAjustesBancarios, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_AjustesBancarioSelected) Then
            If AjustesBancarioSelected.Estado = "A" Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("El ajuste no se puede editar, ya se encuentra anulado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If AjustesBancarioSelected.Documento.ToShortDateString <= fechaCierre Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La fecha del documento no puede ser menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            ValidarUsuario("Tesoreria_Ajustesbancarios", "Editar")
        End If
    End Sub
    Public Sub Editar()
        If Not IsNothing(_AjustesBancarioSelected) Then
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_AjustesBancarioSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _AjustesBancarioSelected.EntityState = EntityState.Detached Then
                    AjustesBancarioSelected = AjustesBancariosAnterior
                End If
            End If
            If Application.Current.Resources.Contains(NombreDiccionarioCombosAjustes) Then
                If CType(Application.Current.Resources(NombreDiccionarioCombosAjustes), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(strNombreConsecutivo) Then
                    listConsecutivos = CType(Application.Current.Resources(NombreDiccionarioCombosAjustes), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(strNombreConsecutivo)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_AjustesBancarioSelected) Then
                If AjustesBancarioSelected.Documento.ToShortDateString <= fechaCierre Then
                    A2Utilidades.Mensajes.mostrarMensaje("La fecha del documento no puede ser menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If AjustesBancarioSelected.Estado = "A" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El ajuste ya se encuentra anulado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                ValidarUsuario("Tesoreria_Ajustesbancarios", "Borrar")
                'If Not IsNothing(_AjustesBancarioSelected) Then
                '    dcProxy.Bolsas.Remove(_AjustesBancarioSelected)
                '    'AjustesBancarioSelected = _ListaBolsas.LastOrDefault
                '    IsBusy = True
                '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                '    'dcProxy.EliminarBolsa(BolsaSelected.intIDBolsa, BolsaSelected.Nombre, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
                'End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminaPreguntaanular(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            'configurarNuevaTesoreria(_TesoreriSelected, True)
            'dcProxy.EliminarTarifas(TarifaSelected.Aprobacion, TarifaSelected.Nombre, TarifaSelected.Usuario, TarifaSelected.ID, String.Empty, TarifaSelected.IDTarifas,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")        
            AjustesBancarioSelected.Estado = "A"
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        Else
            Exit Sub
        End If
    End Sub
    Public Sub Borrar()
        Try
            If Not IsNothing(_AjustesBancarioSelected) Then
                If AjustesBancarioSelected.Estado = "I" Then
                    'C1.Silverlight.C1MessageBox.Show("¿Desea Anular este documento?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminaPreguntaanular)
                    mostrarMensajePregunta("¿Desea Anular este documento?",
                                           Program.TituloSistema,
                                           "ANULARDOCUMENTO",
                                           AddressOf TerminaPreguntaanular, False)
                    Exit Sub
                End If
                'dcProxy.Bolsas.Remove(_AjustesBancarioSelected)
                'AjustesBancarioSelected = _ListaBolsas.LastOrDefault
                AjustesBancarioSelected.Estado = "A"
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                'dcProxy.EliminarBolsa(BolsaSelected.intIDBolsa, BolsaSelected.Nombre, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub ValidarUsuario(ByVal pstrClaseCombos As String, ByVal pstrTipo As String)
        '// Ejecutar la carga de los combos y luego valida los datos con el metodo de ValidarConsecutivos
        objProxy.Load(objProxy.cargarCombosEspecificosQuery(pstrClaseCombos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, pstrTipo)
    End Sub
    'Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
    '    If So.HasError Then
    '        A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '    Else
    '        If Not (So.Value) = String.Empty Then
    '            A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '        Else
    '            If So.UserState = "borrar" Then
    '                dcProxy.Bolsas.Clear()
    '                dcProxy.Load(dcProxy.BolsasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsas, "insert") ' Recarga la lista para que carguen los include
    '            End If
    '        End If
    '    End If
    '    IsBusy = False
    'End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaAjustesBancarios
    Implements INotifyPropertyChanged

    <Display(Name:=" ")>
    Public Property Tipo As String

    <Display(Name:="Tipo")>
    Public Property NombreConsecutivo As String

    <Display(Name:="Número")>
    Public Property IDDocumento As Integer


    Private _IDBanco As Integer
    <Display(Name:="Banco")>
    Public Property IDBanco As Integer
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Integer)
            If Not IsNothing(value) Then
                _IDBanco = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))
        End Set
    End Property

    Private _NombreBanco As String
    <Display(Name:=" ")>
    Public Property NombreBanco As String
        Get
            Return _NombreBanco
        End Get
        Set(ByVal value As String)
            If Not IsNothing(value) Then
                _NombreBanco = value
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreBanco"))
        End Set
    End Property

    Private _Documento As System.Nullable(Of System.DateTime)
    <Display(Name:="Fecha")>
    Public Property Documento As System.Nullable(Of System.DateTime)
        Get
            Return _Documento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            ' If Not IsNothing(value) Then
            _Documento = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Documento"))
        End Set
    End Property

    <Display(Name:="Fecha")>
    Public Property DisplayDate As DateTime

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class