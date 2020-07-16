Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: EmpleadosViewModel.vb
'Generado el : 03/14/2011 12:34:36
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
Imports System.Text.RegularExpressions
Imports A2ComunesControl
Imports A2Utilidades.Mensajes

Public Class EmpleadosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaEmpleado
    Private EmpleadoPorDefecto As Empleado
    Private EmpleadoAnterior As Empleado
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Private EmpleadoSistemaPorDefecto As EmpleadoSistema

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
                dcProxy.Load(dcProxy.EmpleadosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmpleados, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerEmpleadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmpleadosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  EmpleadosViewModel)(Me)
                ComboValorSINO.Add(New CamposComboParametros With {.ID = "SI", .Descripcion = "SI"})
                ComboValorSINO.Add(New CamposComboParametros With {.ID = "NO", .Descripcion = "NO"})
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EmpleadosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerEmpleadosPorDefecto_Completed(ByVal lo As LoadOperation(Of Empleado))
        If Not lo.HasError Then
            EmpleadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Empleado por defecto", _
                                             Me.ToString(), "TerminoTraerEmpleadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEmpleados(ByVal lo As LoadOperation(Of Empleado))
        If Not lo.HasError Then
            ListaEmpleados = dcProxy.Empleados
            If dcProxy.Empleados.Count > 0 Then
                If lo.UserState = "insert" Then
                    ' = ListaEmpleados.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Empleados", _
                                             Me.ToString(), "TerminoTraerEmpleado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Si la respuesta es Yes se procede a Cambiar el Estado del Empleado
    ''' </summary>
    ''' <remarks>SLB20120921</remarks>
    Private Sub TerminoPreguntaCambiarEstado(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            IsBusy = True
            dcProxy.CambiarEstadoEmpleado(_EmpleadoSelected.IDEmpleado,Program.Usuario, Program.HashConexion, AddressOf TerminoCambiarEstado, "")
        End If
    End Sub

    Private Sub TerminoCambiarEstado(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then
            'Filtrar()
            If Not IsNothing(_EmpleadoSelected) Then
                If _EmpleadoSelected.Activo Then
                    EmpleadoSelected.Activo = False
                Else
                    EmpleadoSelected.Activo = True
                End If
            End If
            IsBusy = False
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar el Estado del Empleado", _
                     Me.ToString(), "TerminoCambiarEstado", Application.Current.ToString(), Program.Maquina, lo.Error.InnerException)
        End If
    End Sub

    Private Sub TerminoConsultarEmpleadoSistema(ByVal lo As LoadOperation(Of EmpleadoSistema))
        If Not lo.HasError Then
            ListaEmpleadoSistema = dcProxy.EmpleadoSistemas
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EmpleadoSistema", _
                                             Me.ToString(), "TerminoConsultarEmpleadoSistema", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaEmpleados As EntitySet(Of Empleado)
    Public Property ListaEmpleados() As EntitySet(Of Empleado)
        Get
            Return _ListaEmpleados
        End Get
        Set(ByVal value As EntitySet(Of Empleado))
            _ListaEmpleados = value

            MyBase.CambioItem("ListaEmpleados")
            MyBase.CambioItem("ListaEmpleadosPaged")
            If Not IsNothing(value) Then
                If IsNothing(EmpleadoAnterior) Then
                    EmpleadoSelected = _ListaEmpleados.FirstOrDefault
                Else
                    EmpleadoSelected = EmpleadoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaEmpleadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEmpleados) Then
                Dim view = New PagedCollectionView(_ListaEmpleados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _EmpleadoSelected As Empleado
    Public Property EmpleadoSelected() As Empleado
        Get
            Return _EmpleadoSelected
        End Get
        Set(ByVal value As Empleado)
            _EmpleadoSelected = value
            If Not IsNothing(_EmpleadoSelected) Then
                ObtenerMaquinasUsuario(_EmpleadoSelected.Maquinas)
                dcProxy.EmpleadoSistemas.Clear()
                dcProxy.Load(dcProxy.EmpleadoSistemaConsultarQuery(_EmpleadoSelected.IDEmpleado, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEmpleadoSistema, Nothing)
            Else
                ListaMaquinasUsuario = Nothing
                MostrarLista = Visibility.Collapsed
                MostrarEditando = Visibility.Visible
            End If
            MyBase.CambioItem("EmpleadoSelected")
        End Set
    End Property

    Private _Desactivado As Boolean
    Public Property Desactivado() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
            _Desactivado = False
            MyBase.CambioItem("_Desactivado")
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

    Private _ListaMaquinasUsuario As List(Of MaquinasUsuario)
    Public Property ListaMaquinasUsuario() As List(Of MaquinasUsuario)
        Get
            Return _ListaMaquinasUsuario
        End Get
        Set(ByVal value As List(Of MaquinasUsuario))
            _ListaMaquinasUsuario = value
            MyBase.CambioItem("ListaMaquinasUsuario")
        End Set
    End Property

    Private _NuevaMaquina As String
    Public Property NuevaMaquina() As String
        Get
            Return _NuevaMaquina
        End Get
        Set(ByVal value As String)
            _NuevaMaquina = value
            MyBase.CambioItem("NuevaMaquina")
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

    Private _MostrarLista As Visibility = Visibility.Collapsed
    Public Property MostrarLista As Visibility
        Get
            Return _MostrarLista
        End Get
        Set(ByVal value As Visibility)
            _MostrarLista = value
            MyBase.CambioItem("MostrarLista")
        End Set
    End Property

    Private _ComboValorSINO As New List(Of CamposComboParametros)
    Public Property ComboValorSINO As List(Of CamposComboParametros)
        Get
            Return _ComboValorSINO
        End Get
        Set(value As List(Of CamposComboParametros))
            _ComboValorSINO = value
            MyBase.CambioItem("ComboValorSINO")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewEmpleado As New Empleado
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewEmpleado.IDComisionista = EmpleadoPorDefecto.IDComisionista
            NewEmpleado.IDSucComisionista = EmpleadoPorDefecto.IDSucComisionista
            NewEmpleado.IDEmpleado = EmpleadoPorDefecto.IDEmpleado
            NewEmpleado.Nombre = EmpleadoPorDefecto.Nombre
            NewEmpleado.NroDocumento = EmpleadoPorDefecto.NroDocumento
            NewEmpleado.IDReceptor = EmpleadoPorDefecto.IDReceptor
            NewEmpleado.Login = EmpleadoPorDefecto.Login
            NewEmpleado.IDCargo = EmpleadoPorDefecto.IDCargo
            NewEmpleado.AccesoOperadorBolsa = EmpleadoPorDefecto.AccesoOperadorBolsa
            NewEmpleado.Actualizacion = EmpleadoPorDefecto.Actualizacion
            NewEmpleado.Usuario = Program.Usuario
            NewEmpleado.Activo = EmpleadoPorDefecto.Activo
            NewEmpleado.TipoIdentificacion = "C"
            EmpleadoAnterior = EmpleadoSelected
            EmpleadoSelected = NewEmpleado
            PropiedadTextoCombos = ""
            MyBase.CambioItem("EmpleadoSelected")
            MostrarEditando = Visibility.Visible
            MostrarLista = Visibility.Collapsed
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
            dcProxy.Empleados.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.EmpleadosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmpleados, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.EmpleadosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmpleados, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDEmpleado <> 0 Or cb.Nombre <> String.Empty Or cb.NroDocumento <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Empleados.Clear()
                EmpleadoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDEmpleado = " &  cb.IDEmpleado.ToString() & " Nombre = " &  cb.Nombre.ToString() & " NroDocumento = " &  cb.NroDocumento.ToString() 
                dcProxy.Load(dcProxy.EmpleadosConsultarQuery(cb.IDEmpleado, cb.Nombre, cb.NroDocumento,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmpleados, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaEmpleado
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function RegistroValido()

        If Not String.IsNullOrEmpty(EmpleadoSelected.strEmail) Then
            If Not Regex.IsMatch(EmpleadoSelected.strEmail, "^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]*$") Then
                mostrarMensaje("El campo Email, no contiene el formato correcto", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
        End If

        For Each objEmpleadoSistema In ListaEmpleadoSistema
            If String.IsNullOrEmpty(objEmpleadoSistema.Sistema) Then
                mostrarMensaje("El Sistema del detalle empleado es un campo requerido", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
            If String.IsNullOrEmpty(objEmpleadoSistema.CodigoMapeo) Then
                mostrarMensaje("La Acción del detalle empleado es un campo requerido", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
            If String.IsNullOrEmpty(objEmpleadoSistema.Valor) Then
                mostrarMensaje("El Valor del detalle empleado es un campo requerido", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If

            If String.IsNullOrEmpty(EmpleadoSelected.TipoIdentificacion) Then
                mostrarMensaje("El tipo de identificacion es un campo requerido", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
                Exit Function
            End If
        Next

        For Each objEmpleadoSistema In ListaEmpleadoSistema
            Dim count As Integer = 0
            For Each objEmpleadoSistema2 In ListaEmpleadoSistema
                If objEmpleadoSistema.Sistema = objEmpleadoSistema2.Sistema And objEmpleadoSistema.CodigoMapeo = objEmpleadoSistema2.CodigoMapeo Then
                    count = count + 1
                End If
                If count > 1 Then
                    mostrarMensaje("No se pueder tener repetido la acción para un mismo sistema.", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Return False
                    Exit Function
                End If
            Next
        Next


        Return True
    End Function

    Public Overrides Sub ActualizarRegistro()
        Try
            If Not IsNothing(_ListaMaquinasUsuario) Then
                _EmpleadoSelected.Maquinas = String.Empty

                For Each li In _ListaMaquinasUsuario
                    If li.Seleccionada Then
                        If String.IsNullOrEmpty(_EmpleadoSelected.Maquinas) Then
                            _EmpleadoSelected.Maquinas = li.Maquina
                        Else
                            _EmpleadoSelected.Maquinas = String.Format("{0}|{1}", _EmpleadoSelected.Maquinas, li.Maquina)
                        End If
                    End If
                Next
            End If

            If RegistroValido() Then

                Dim origen = "update"
                ErrorForma = ""
                EmpleadoAnterior = EmpleadoSelected

                If ListaEmpleados.Where(Function(i) i.IDEmpleado = EmpleadoSelected.IDEmpleado).Count = 0 Then
                    origen = "insert"

                    ListaEmpleados.Add(EmpleadoSelected)

                    If Not IsNothing(_ListaEmpleadoSistema) Then
                        For Each li In _ListaEmpleadoSistema
                            EmpleadoSelected.EmpleadosSistemas.Add(li)
                        Next
                    End If
                End If

                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)

            End If

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
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaEmpleados.Remove(EmpleadoSelected)
                    End If
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

            MostrarEditando = Visibility.Visible

            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.Empleados.Clear()
                dcProxy.Load(dcProxy.EmpleadosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEmpleados, "insert") ' Recarga la lista para que carguen los include
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EmpleadoSelected) Then
            If _EmpleadoSelected.Activo Then
                Editando = True
                MostrarEditando = Visibility.Visible
            Else
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("Solo se pueden editar empleados que se encuentren activos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_EmpleadoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                MostrarEditando = Visibility.Collapsed
                If _EmpleadoSelected.EntityState = EntityState.Detached Then
                    EmpleadoSelected = EmpleadoAnterior
                End If

                If Not IsNothing(ListaMaquinasUsuario) Then
                    If ListaMaquinasUsuario.Count > 0 Then
                        MostrarLista = Visibility.Visible
                        MostrarEditando = Visibility.Visible
                    Else
                        MostrarLista = Visibility.Collapsed
                        MostrarEditando = Visibility.Collapsed
                    End If
                Else
                    MostrarLista = Visibility.Collapsed
                    MostrarEditando = Visibility.Collapsed
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
            'SLB se adiciona esta logica para Activar o Desactivar.
            Dim strMsg As String = String.Empty
            If Not IsNothing(_EmpleadoSelected) Then
                If _EmpleadoSelected.Activo Then
                    strMsg = "¿Está seguro de inactivar este empleado?"
                Else
                    strMsg = "¿Está seguro de activar este empleado?"
                End If
                'C1.Silverlight.C1MessageBox.Show(strMsg, "Atención", C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPreguntaCambiarEstado)
                mostrarMensajePregunta(strMsg, _
                                       Program.TituloSistema, _
                                       "INACTIVAREMPLEADO", _
                                       AddressOf TerminoPreguntaCambiarEstado, False)
            End If

            
            'If Not IsNothing(_EmpleadoSelected) Then
            '        dcProxy.Empleados.Remove(_EmpleadoSelected)
            '        EmpleadoSelected = _ListaEmpleados.LastOrDefault
            '    IsBusy = True
            '        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            'End If
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
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("NroDocumento", 1)
        DicCamposTab.Add("Login", 1)
        DicCamposTab.Add("IDCargo", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaEmpleado
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function ObtenerMaquinasUsuario(ByVal pstrMaquinas As String) As List(Of MaquinasUsuario)
        Try
            If Not String.IsNullOrEmpty(pstrMaquinas) Then
                Dim objListaMaquinas As New List(Of MaquinasUsuario)

                For Each li In pstrMaquinas.Split("|")
                    objListaMaquinas.Add(New MaquinasUsuario With {.Seleccionada = True, .Maquina = li})
                Next

                ListaMaquinasUsuario = Nothing
                ListaMaquinasUsuario = objListaMaquinas
                MostrarLista = Visibility.Visible
                MostrarEditando = Visibility.Visible
            Else
                ListaMaquinasUsuario = Nothing
                MostrarLista = Visibility.Collapsed
                MostrarEditando = Visibility.Collapsed
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener la lista de maquinas del usuario.", _
             Me.ToString(), "ObtenerMaquinasUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return ListaMaquinasUsuario
    End Function

    Public Sub AdicionarMaquina()
        Try
            If NuevaMaquina <> String.Empty And Not IsNothing(NuevaMaquina) Then
                If IsNothing(ListaMaquinasUsuario) Then
                    ListaMaquinasUsuario = New List(Of MaquinasUsuario)
                    MostrarLista = Visibility.Visible
                End If

                If Not ListaMaquinasUsuario.Where(Function(i) i.Maquina = NuevaMaquina).Count > 0 Then
                    Dim objLista As List(Of MaquinasUsuario)
                    objLista = ListaMaquinasUsuario

                    objLista.Add(New MaquinasUsuario With {.Seleccionada = True, .Maquina = NuevaMaquina})

                    ListaMaquinasUsuario = Nothing
                    ListaMaquinasUsuario = objLista
                End If

                NuevaMaquina = String.Empty
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar la maquina.", _
             Me.ToString(), "AdicionarMaquina", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EliminarMaquinasUsuario()
        Try
            Dim objLista As New List(Of MaquinasUsuario)

            For Each li In ListaMaquinasUsuario
                If li.Seleccionada Then
                    objLista.Add(li)
                End If
            Next

            ListaMaquinasUsuario = Nothing
            ListaMaquinasUsuario = objLista
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al eliminar la maquina.", _
             Me.ToString(), "EliminarMaquinasUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub




#End Region

#Region "Tablas hijas"


    '********************************************************detalle empleado
    Private _ListaEmpleadoSistema As EntitySet(Of EmpleadoSistema)
    Public Property ListaEmpleadoSistema() As EntitySet(Of EmpleadoSistema)
        Get
            Return _ListaEmpleadoSistema
        End Get
        Set(ByVal value As EntitySet(Of EmpleadoSistema))
            _ListaEmpleadoSistema = value
            MyBase.CambioItem("ListaEmpleadoSistema")
            MyBase.CambioItem("ListaEmpleadoSistemaPaged")
        End Set
    End Property



    Public ReadOnly Property ListaEmpleadoSistemaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEmpleadoSistema) Then
                Dim view = New PagedCollectionView(_ListaEmpleadoSistema)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _EmpleadoSistemaSelected As EmpleadoSistema
    Public Property EmpleadoSistemaSelected() As EmpleadoSistema
        Get
            Return _EmpleadoSistemaSelected
        End Get
        Set(ByVal value As EmpleadoSistema)
            _EmpleadoSistemaSelected = value
            MyBase.CambioItem("EmpleadoSistemaSelected")
        End Set
    End Property



    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmEmpleadoSistema"
                    Dim newEmpleadoSistema As New EmpleadoSistema
                    Dim intIDEmpleadoSistema As Integer = 0

                    If IsNothing(_ListaEmpleadoSistema) Then
                        intIDEmpleadoSistema = 0
                    Else
                        intIDEmpleadoSistema = -(_ListaEmpleadoSistema.Count)
                    End If

                    newEmpleadoSistema.ID = intIDEmpleadoSistema
                    newEmpleadoSistema.IDEmpleado = EmpleadoSelected.IDEmpleado
                    newEmpleadoSistema.Usuario = Program.Usuario
                    ListaEmpleadoSistema.Add(newEmpleadoSistema)
                    EmpleadoSistemaSelected = newEmpleadoSistema
                    MyBase.CambioItem("EmpleadoSistemaSelected")
                    MyBase.CambioItem("ListaEmpleadoSistema")

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmEmpleadoSistema"
                    If Not IsNothing(ListaEmpleadoSistema) Then
                        If Not IsNothing(ListaEmpleadoSistema) Then
                            If Not IsNothing(_EmpleadoSistemaSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(EmpleadoSistemaSelected, ListaEmpleadoSistema.ToList)

                                ListaEmpleadoSistema.Remove(_EmpleadoSistemaSelected)
                                If ListaEmpleadoSistema.Count > 0 Then
                                    Program.PosicionarItemLista(EmpleadoSistemaSelected, ListaEmpleadoSistema.ToList, intRegistroPosicionar)
                                Else
                                    EmpleadoSistemaSelected = Nothing
                                End If
                                MyBase.CambioItem("EmpleadoSistemaSelected")
                                MyBase.CambioItem("ListaEmpleadoSistema")
                            End If
                        End If

                    End If

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el borrado del detalle", _
                                  Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaEmpleado

    <Display(Name:="Código")> _
    Public Property IDEmpleado As Integer

    <StringLength(250, ErrorMessage:="La longitud máxima es de 250")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
     <Display(Name:="Número Documento")> _
    Public Property NroDocumento As String
End Class


Public Class MaquinasUsuario
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

    Private _Maquina As String
    Public Property Maquina() As String
        Get
            Return _Maquina
        End Get
        Set(ByVal value As String)
            _Maquina = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Maquina"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


