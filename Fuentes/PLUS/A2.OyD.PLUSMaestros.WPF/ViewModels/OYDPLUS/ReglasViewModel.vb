Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ReglasViewModel.vb
'Generado el : 11/04/2012 17:13:35
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

Public Class ReglasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ReglaPorDefecto As Regla
    Private ReglaAnterior As Regla
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext
    Dim IdOrdenActualizar As Integer = 0

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
                dcProxy.Load(dcProxy.ReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, "")
                dcProxy1.Load(dcProxy1.TraerReglaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglasPorDefecto_Completed, "Default")

                ListaTipoRegla = New List(Of TipoRegla)
                ListaTipoRegla.Add(New TipoRegla With {.Codigo = "REGLADESABILITADA", .Descripcion = "NO Validar"})
                ListaTipoRegla.Add(New TipoRegla With {.Codigo = "DETIENEINGRESONOCONTINUA", .Descripcion = "No Continua (Detiene el Proceso)"})
                ListaTipoRegla.Add(New TipoRegla With {.Codigo = "REQUIERECONFIRMACION", .Descripcion = "Genera Advertencia"})
                ListaTipoRegla.Add(New TipoRegla With {.Codigo = "REQUIEREJUSTIFICACION", .Descripcion = "Requiere Justificación"})
                ListaTipoRegla.Add(New TipoRegla With {.Codigo = "REQUIEREAPROBACION", .Descripcion = "Requiere Aprobación"})

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ReglasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerReglasPorDefecto_Completed(ByVal lo As LoadOperation(Of Regla))
        If Not lo.HasError Then
            ReglaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Regla por defecto",
                                             Me.ToString(), "TerminoTraerReglaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerReglas(ByVal lo As LoadOperation(Of Regla))
        If Not lo.HasError Then
            ListaReglas = dcProxy.Reglas
            If dcProxy.Reglas.Count > 0 Then
                If lo.UserState = "insert" Or lo.UserState = "update" Then
                    If ListaReglas.Where(Function(i) i.IDRegla = IdOrdenActualizar).Count > 0 Then
                        ReglaSelected = ListaReglas.Where(Function(i) i.IDRegla = IdOrdenActualizar).FirstOrDefault
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Then
                    mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Reglas",
                                             Me.ToString(), "TerminoTraerRegla", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then

                            If dcProxy.Reglas.Where(Function(i) i.IDRegla = _ReglaSelected.IDRegla).Count > 0 Then
                                dcProxy.Reglas.Remove(dcProxy.Reglas.Where(Function(i) i.IDRegla = _ReglaSelected.IDRegla).First)
                            End If

                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Dim origen = "update"
                            If Not ListaReglas.Contains(ReglaSelected) Then
                                origen = "insert"
                                ListaReglas.Add(ReglaSelected)
                            End If
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

#End Region

#Region "Propiedades"

    Private _ListaReglas As EntitySet(Of Regla)
    Public Property ListaReglas() As EntitySet(Of Regla)
        Get
            Return _ListaReglas
        End Get
        Set(ByVal value As EntitySet(Of Regla))
            _ListaReglas = value

            MyBase.CambioItem("ListaReglas")
            MyBase.CambioItem("ListaReglasPaged")
            If Not IsNothing(_ListaReglas) Then
                If _ListaReglas.Count Then
                    ReglaSelected = _ListaReglas.FirstOrDefault
                Else
                    ReglaSelected = Nothing
                End If
            Else
                ReglaSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaReglasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReglas) Then
                Dim view = New PagedCollectionView(_ListaReglas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ReglaSelected As Regla
    Public Property ReglaSelected() As Regla
        Get
            Return _ReglaSelected
        End Get
        Set(ByVal value As Regla)
            _ReglaSelected = value
            If Not IsNothing(_ReglaSelected) Then
                ObtenerCausaJustificacion(_ReglaSelected.CausasJustificacion)
            Else
                ListaCausasJustificacion = Nothing
            End If
            MyBase.CambioItem("ReglaSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaRegla
    Public Property cb() As CamposBusquedaRegla
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaRegla)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _ListaCausasJustificacion As List(Of CausasJustificación)
    Public Property ListaCausasJustificacion() As List(Of CausasJustificación)
        Get
            Return _ListaCausasJustificacion
        End Get
        Set(ByVal value As List(Of CausasJustificación))
            _ListaCausasJustificacion = value
            MyBase.CambioItem("ListaCausasJustificacion")
        End Set
    End Property

    Private _MostrarEditando As Visibility = Visibility.Collapsed
    Public Property MostrarEditando() As Visibility
        Get
            Return _MostrarEditando
        End Get
        Set(ByVal value As Visibility)
            _MostrarEditando = value
            MyBase.CambioItem("MostrarEditando")
        End Set
    End Property

    Private _NuevaJustificacion As String
    Public Property NuevaJustificacion() As String
        Get
            Return _NuevaJustificacion
        End Get
        Set(ByVal value As String)
            _NuevaJustificacion = value
            MyBase.CambioItem("NuevaJustificacion")
        End Set
    End Property

    Private _MostrarJustificaciones As Visibility = Visibility.Collapsed
    Public Property MostrarJustificaciones() As Visibility
        Get
            Return _MostrarJustificaciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarJustificaciones = value
            MyBase.CambioItem("MostrarJustificaciones")
        End Set
    End Property

    Private _HabilitarCampos As Boolean
    Public Property HabilitarCampos() As Boolean
        Get
            Return _HabilitarCampos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCampos = value
            MyBase.CambioItem("HabilitarCampos")
        End Set
    End Property

    Private _ListaTipoRegla As List(Of TipoRegla)
    Public Property ListaTipoRegla() As List(Of TipoRegla)
        Get
            Return _ListaTipoRegla
        End Get
        Set(ByVal value As List(Of TipoRegla))
            _ListaTipoRegla = value
            MyBase.CambioItem("ListaTipoRegla")
        End Set
    End Property

    Private _TipoReglaSelected As String
    <Display(Name:="Acción Regla")>
    Public Property TipoReglaSelected() As String
        Get
            Return _TipoReglaSelected
        End Get
        Set(ByVal value As String)
            _TipoReglaSelected = value
            MyBase.CambioItem("TipoReglaSelected")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewRegla As New Regla
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewRegla.IDRegla = -1
            NewRegla.CodigoRegla = "_"
            NewRegla.DescripcionRegla = String.Empty
            NewRegla.Modulo = "ORDENES"
            NewRegla.NombreCorto = "_"
            NewRegla.Prioridad = 0
            NewRegla.CodigoTipoRegla = "DETIENEINGRESONOCONTINUA"
            NewRegla.DescripcionCodigoTipoRegla = "No Continua (Detiene el Proceso)"
            NewRegla.CausasJustificacion = String.Empty
            NewRegla.NombreProcedimiento = String.Empty
            NewRegla.Parametros = String.Empty
            NewRegla.Usuario = Program.Usuario
            NewRegla.MotorCalculos = True
            NuevaJustificacion = String.Empty
            ListaCausasJustificacion = Nothing

            MostrarEditando = Visibility.Visible
            ObtenerRegistroAnterior()
            ReglaSelected = NewRegla

            MyBase.CambioItem("ReglaSelected")
            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarCampos = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.Reglas) Then
                dcProxy.Reglas.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ReglasFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, Nothing)
            Else
                dcProxy.Load(dcProxy.ReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If Not IsNothing(_ReglaSelected) Then
                If String.IsNullOrEmpty(_ReglaSelected.CodigoRegla) Or _ReglaSelected.CodigoRegla = "_" Then
                    mostrarMensaje("Debe de ingresar el código de la regla", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If String.IsNullOrEmpty(_ReglaSelected.Modulo) Then
                    mostrarMensaje("Debe de ingresar el modulo de la regla", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If String.IsNullOrEmpty(_ReglaSelected.NombreCorto) Or _ReglaSelected.NombreCorto = "_" Then
                    mostrarMensaje("Debe de ingresar el nombre corto de la regla", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                If String.IsNullOrEmpty(_ReglaSelected.CodigoTipoRegla) Then
                    mostrarMensaje("Debe de ingresar el tipo de validación de la regla", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                Dim logPermitirguardar As Boolean = True

                Dim strCodigoRegla As String = String.Empty, strCodigoTipoRegla As String = String.Empty, blnExiste As Boolean = False

                If _ReglaSelected.CodigoTipoRegla = "REQUIEREAPROBACION" Or _ReglaSelected.CodigoTipoRegla = "REQUIEREJUSTIFICACION" Then
                    _ReglaSelected.CausasJustificacion = String.Empty

                    If Not IsNothing(ListaCausasJustificacion) Then
                        For Each li In ListaCausasJustificacion
                            If li.Seleccionada Then
                                If String.IsNullOrEmpty(_ReglaSelected.CausasJustificacion) Then
                                    _ReglaSelected.CausasJustificacion = li.Causa
                                Else
                                    _ReglaSelected.CausasJustificacion = String.Format("{0}|{1}", _ReglaSelected.CausasJustificacion, li.Causa)
                                End If
                            End If
                        Next
                    End If

                    If String.IsNullOrEmpty(_ReglaSelected.CausasJustificacion) Then
                        mostrarMensaje("Debe ingresar al menos una causa de justificación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logPermitirguardar = False
                    End If
                End If

                'Reorganizar las prioridad de las reglas.
                If ListaReglas.Where(Function(i) IIf(i.Prioridad Is Nothing, -999, i.Prioridad) = _ReglaSelected.Prioridad And i.IDRegla <> _ReglaSelected.IDRegla And i.Modulo = _ReglaSelected.Modulo).Count > 0 Then
                    Dim UltimaPrioridad As Integer = 0

                    For Each li In ListaReglas.Where(Function(i) IIf(i.Prioridad Is Nothing, -999, i.Prioridad) >= _ReglaSelected.Prioridad And i.IDRegla <> _ReglaSelected.IDRegla And i.Modulo = _ReglaSelected.Modulo)
                        If li.Prioridad = _ReglaSelected.Prioridad Then
                            li.Prioridad = li.Prioridad + 1
                            UltimaPrioridad = li.Prioridad
                        Else
                            If li.Prioridad = UltimaPrioridad Then
                                li.Prioridad = li.Prioridad + 1
                                UltimaPrioridad = li.Prioridad
                            End If
                        End If
                    Next
                End If

                strCodigoRegla = _ReglaSelected.CodigoRegla
                strCodigoTipoRegla = _ReglaSelected.CodigoTipoRegla

                If strCodigoRegla = "PATRIMONIOTECNICO" Then
                    If strCodigoTipoRegla = "REGLADESABILITADA" Then
                        blnExiste = ListaReglas.Any(Function(i) (i.CodigoRegla = "PATRIMONIOTECNICORECEPTOR" Or i.CodigoRegla = "PATRIMONIOTECNICOSUCURSAL" _
                                                                 Or i.CodigoRegla = "PATRIMONIOTECNICOCLIENTE") _
                                                        And i.CodigoTipoRegla <> strCodigoTipoRegla)
                        If blnExiste Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se deben modificar antes las reglas 'Patrimonio tecnico receptor', " +
                                "'Patrimonio tecnico sucursal', 'Patrimonio tecnico cliente' antes de modificar la regla " +
                                "'Patrimonio Tecnico'.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logPermitirguardar = False
                        End If
                    End If
                Else
                    If (strCodigoRegla = "PATRIMONIOTECNICORECEPTOR" Or strCodigoRegla = "PATRIMONIOTECNICOSUCURSAL" Or strCodigoRegla = "PATRIMONIOTECNICOCLIENTE") _
                        And strCodigoTipoRegla = "DETIENEINGRESONOCONTINUA" Then
                        blnExiste = ListaReglas.Any(Function(i) i.CodigoRegla = "PATRIMONIOTECNICO" And i.CodigoTipoRegla <> "DETIENEINGRESONOCONTINUA")
                        If blnExiste Then

                            A2Utilidades.Mensajes.mostrarMensaje("Se deben modificar antes la regla 'Patrimonio Tecnico' antes de modificar la regla " +
                                _ReglaSelected.NombreCorto, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logPermitirguardar = False
                        End If
                    End If
                End If

                If logPermitirguardar Then
                    Dim origen = "update"
                    ErrorForma = ""

                    If ListaReglas.Where(Function(i) i.IDRegla = _ReglaSelected.IDRegla).Count = 0 Then
                        origen = "insert"
                    End If
                    IsBusy = True

                    If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                        objProxy.ValidacionEliminarRegistros.Clear()
                    End If

                    If origen = "insert" Then
                        objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblReglas", "'strCodigoRegla'", String.Format("'{0}'", _ReglaSelected.CodigoRegla), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                    Else
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                        'If ReglaAnterior.CodigoRegla <> _ReglaSelected.CodigoRegla Then
                        '    dcProxy.Load(dcProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblReglas", "'strCodigoRegla'", String.Format("'{0}'", _ReglaSelected.CodigoRegla), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                        'Else
                        '    IdOrdenActualizar = 0
                        '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                        'End If
                    End If
                End If
            End If
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
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            MostrarEditando = Visibility.Collapsed
            If Not IsNothing(_ReglaSelected) Then
                ListaCausasJustificacion = ObtenerCausaJustificacion(_ReglaSelected.CausasJustificacion)
            End If
            HabilitarCampos = False

            If So.UserState = "update" Or So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()

                IdOrdenActualizar = _ReglaSelected.IDRegla
                If Not IsNothing(dcProxy.Reglas) Then
                    dcProxy.Reglas.Clear()
                End If

                dcProxy.Load(dcProxy.ReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, So.UserState)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ReglaSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
            MostrarEditando = Visibility.Visible
            NuevaJustificacion = String.Empty
            ObtenerCausaJustificacion(_ReglaSelected.CausasJustificacion)
            HabilitarCampos = False
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            MostrarEditando = Visibility.Collapsed
            NuevaJustificacion = String.Empty
            HabilitarCampos = False
            If Not IsNothing(_ReglaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                ReglaSelected = ReglaAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ReglaSelected) Then
                If _ReglaSelected.MotorCalculos = False Then
                    mostrarMensaje("Solo las reglas marcadas como motor de calculos pueden ser eliminadas, sí desea deshabilite la regla colocando la opción No validar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borrar el registro seleccionado. ¿Confirma el borrado de esta regla?", Program.TituloSistema, "borrar", AddressOf TerminoMensajePregunta)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    ''' 
    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Select Case CType(sender, A2Utilidades.wppMensajePregunta).CodigoLlamado.ToLower
                    Case "borrar"
                        If Not IsNothing(_ReglaSelected) Then
                            IsBusy = True
                            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                                objProxy.ValidacionEliminarRegistros.Clear()
                            End If
                            objProxy.Load(objProxy.ValidarEliminarRegistroQuery("'OYDPLUS.tblMensajesReglas'", "'intIDRegla'", String.Format("'{0}'", _ReglaSelected.IDRegla), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ELIMINAR")
                        End If
                End Select
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "TerminoMensajePregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            ErrorForma = ""
            If Not IsNothing(dcProxy.Reglas) Then
                dcProxy.Reglas.Clear()
            End If

            ReglaAnterior = Nothing
            IsBusy = True
            'DescripcionFiltroVM = " IdBolsa = " &  cb.IdBolsa.ToString() & " Nombre = " &  cb.Nombre.ToString() 
            If Not IsNothing(dcProxy.Reglas) Then
                dcProxy.Reglas.Clear()
            End If
            Dim TextoFiltroSeguroCodigo = System.Web.HttpUtility.UrlEncode(cb.CodigoRegla)
            Dim TextoFiltroSeguroNombre = System.Web.HttpUtility.UrlEncode(cb.NombreCorto)

            dcProxy.Load(dcProxy.ReglasConsultarQuery(TextoFiltroSeguroCodigo, TextoFiltroSeguroNombre, cb.MotorCalculos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, "Busqueda")
            MyBase.ConfirmarBuscar()
            PrepararNuevaBusqueda()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaRegla
            objCB.CodigoRegla = String.Empty
            objCB.NombreCorto = String.Empty
            objCB.MotorCalculos = False

            cb = objCB
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta",
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objRegla As New Regla
            If Not IsNothing(_ReglaSelected) Then
                objRegla.IDRegla = _ReglaSelected.IDRegla
                objRegla.CausasJustificacion = _ReglaSelected.CausasJustificacion
                objRegla.CodigoRegla = _ReglaSelected.CodigoRegla
                objRegla.CodigoTipoRegla = _ReglaSelected.CodigoTipoRegla
                objRegla.DescripcionRegla = _ReglaSelected.DescripcionRegla
                objRegla.DescripcionCodigoTipoRegla = _ReglaSelected.DescripcionCodigoTipoRegla
                objRegla.Modulo = _ReglaSelected.Modulo
                objRegla.NombreCorto = _ReglaSelected.NombreCorto
                objRegla.NombreProcedimiento = _ReglaSelected.NombreProcedimiento
                objRegla.Parametros = _ReglaSelected.Parametros
                objRegla.Prioridad = _ReglaSelected.Prioridad
                objRegla.Usuario = _ReglaSelected.Usuario
            End If
            ReglaAnterior = Nothing
            ReglaAnterior = objRegla
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.",
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function ObtenerCausaJustificacion(ByVal pstrCausasJustificacion As String) As List(Of CausasJustificación)
        Try
            If Not String.IsNullOrEmpty(pstrCausasJustificacion) Then
                Dim objListaCausas As New List(Of CausasJustificación)

                For Each li In pstrCausasJustificacion.Split("|")
                    objListaCausas.Add(New CausasJustificación With {.Seleccionada = True, .Causa = li})
                Next

                ListaCausasJustificacion = Nothing
                ListaCausasJustificacion = objListaCausas
            Else
                ListaCausasJustificacion = Nothing
            End If

            If _ReglaSelected.CodigoTipoRegla = "REQUIEREAPROBACION" Or _ReglaSelected.CodigoTipoRegla = "REQUIEREJUSTIFICACION" Then
                MostrarJustificaciones = Visibility.Visible
            Else
                MostrarJustificaciones = Visibility.Collapsed
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la lista de causas de justificación.",
             Me.ToString(), "ObtenerCausaJustificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return ListaCausasJustificacion
    End Function

    Public Sub AdicionarJustificacion()
        Try
            If NuevaJustificacion <> String.Empty And Not IsNothing(NuevaJustificacion) Then
                If IsNothing(ListaCausasJustificacion) Then
                    ListaCausasJustificacion = New List(Of CausasJustificación)
                End If

                If Not ListaCausasJustificacion.Where(Function(i) i.Causa = NuevaJustificacion).Count > 0 Then
                    Dim objLista As List(Of CausasJustificación)
                    objLista = ListaCausasJustificacion

                    objLista.Add(New CausasJustificación With {.Seleccionada = True, .Causa = NuevaJustificacion})

                    ListaCausasJustificacion = Nothing
                    ListaCausasJustificacion = objLista
                End If

                NuevaJustificacion = String.Empty
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EliminarJustificacion()
        Try
            Dim objLista As New List(Of CausasJustificación)

            For Each li In ListaCausasJustificacion
                If li.Seleccionada Then
                    objLista.Add(li)
                End If
            Next

            ListaCausasJustificacion = Nothing
            ListaCausasJustificacion = objLista
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _ReglaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ReglaSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("CodigoTipoRegla") Then
                    If _ReglaSelected.CodigoTipoRegla = "REQUIEREAPROBACION" Or _ReglaSelected.CodigoTipoRegla = "REQUIEREJUSTIFICACION" Then
                        MostrarJustificaciones = Visibility.Visible
                    Else
                        MostrarJustificaciones = Visibility.Collapsed
                    End If
                End If
            End If
            'If Editando Then
            '    If _ReglaSelected.CodigoTipoRegla.RequiereJustificacion Or _ReglaSelected.RequiereAprobacion Then
            '        MostrarJustificaciones = Visibility.Visible
            '    Else
            '        MostrarJustificaciones = Visibility.Collapsed
            '    End If

            '    If _ReglaSelected.RequiereJustificacion Then
            '        CambiarBooleanos("REQUIEREJUSTIFICACION")
            '    End If
            'End If
            'ElseIf e.PropertyName.Equals("DetieneIngreso") Then
            '    If Editando Then
            '        If _ReglaSelected.DetieneIngreso Then
            '            CambiarBooleanos("DETIENEINGRESO")
            '        End If
            '    End If
            'ElseIf e.PropertyName.Equals("ContinuarMostrandoAdvertencia") Then
            '    If Editando Then
            '        If _ReglaSelected.ContinuarMostrandoAdvertencia Then
            '            CambiarBooleanos("CONTINUARMOSTRANDOMENSAJE")
            '        End If
            '    End If
            'ElseIf e.PropertyName.Equals("RequiereConfirmacion") Then
            '    If Editando Then
            '        If _ReglaSelected.RequiereConfirmacion Then
            '            CambiarBooleanos("REQUIERECONFIRMACION")
            '        End If
            '    End If
            'ElseIf e.PropertyName.Equals("RequiereAprobacion") Then
            '    If Editando Then
            '        If _ReglaSelected.RequiereJustificacion Or _ReglaSelected.RequiereAprobacion Then
            '            MostrarJustificaciones = Visibility.Visible
            '        Else
            '            MostrarJustificaciones = Visibility.Collapsed
            '        End If

            '        If _ReglaSelected.RequiereAprobacion Then
            '            CambiarBooleanos("REQUIEREAPROBACION")
            '        End If
            '    End If
            'ElseIf e.PropertyName.Equals("ContinuaValidacion") Then
            '    If Editando Then
            '        If _ReglaSelected.ContinuaValidacion Then
            '            _ReglaSelected.DetieneIngreso = True
            '            HabilitarCampos = False
            '        Else
            '            HabilitarCampos = True
            '        End If
            '    End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_ReglaSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaRegla
    Implements INotifyPropertyChanged

    Private _CodigoRegla As String
    <Display(Name:="Código regla")> _
    Public Property CodigoRegla() As String
        Get
            Return _CodigoRegla
        End Get
        Set(ByVal value As String)
            _CodigoRegla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoRegla"))
        End Set
    End Property

    Private _NombreCorto As String
    <Display(Name:="Nombre corto")> _
    Public Property NombreCorto() As String
        Get
            Return _NombreCorto
        End Get
        Set(ByVal value As String)
            _NombreCorto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCorto"))
        End Set
    End Property

    Private _MotorCalculos As Boolean
    <Display(Name:="Motor calculos")> _
    Public Property MotorCalculos() As Boolean
        Get
            Return _MotorCalculos
        End Get
        Set(ByVal value As Boolean)
            _MotorCalculos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("MotorCalculos"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class CausasJustificación
    Implements INotifyPropertyChanged

    Private _Seleccionada As Boolean
    Public Property Seleccionada() As Boolean
        Get
            Return _Seleccionada
        End Get
        Set(ByVal value As Boolean)
            _Seleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Seleccionada"))
        End Set
    End Property

    Private _Causa As String
    Public Property Causa() As String
        Get
            Return _Causa
        End Get
        Set(ByVal value As String)
            _Causa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Causa"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class TipoRegla
    Implements INotifyPropertyChanged

    Private _Codigo As String
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class