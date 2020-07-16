Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ListasViewModel.vb
'Generado el : 01/27/2011 09:30:33
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
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros

Public Class ConfiguracionListasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaConfiguracionLista
    'Public Property cbDetalle As New CamposBusquedaListadetalle
    'Private ListaPorDefecto As ListaConfiguracion
    Private ListaDetallePorDefecto As Lista
    Private ListaAnterior As ListaConfiguracion
    Private ListaDetalleAnterior As Lista

    Dim dcProxy As MaestrosCFDomainContext
    Dim dcProxy1 As MaestrosCFDomainContext
    Dim objProxyActualizar As MaestrosCFDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosCFDomainContext()
            dcProxy1 = New MaestrosCFDomainContext()
            objProxyActualizar = New MaestrosCFDomainContext()
        Else
            dcProxy = New MaestrosCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            objProxyActualizar = New MaestrosCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ListaConfiguracionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "FiltroInicial")
                'dcProxy1.Load(dcProxy1.TraerListaConfiguracionPorDefectoQuery, AddressOf TerminoTraerListasPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerListaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListasDetallePorDefecto_Completed, "Default")

                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ListasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ListasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    'Private Sub TerminoTraerListasPorDefecto_Completed(ByVal lo As LoadOperation(Of ListaConfiguracion))
    '    If Not lo.HasError Then
    '        ListaPorDefecto = lo.Entities.FirstOrDefault
    '    Else
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Lista por defecto", _
    '                                         Me.ToString(), "TerminoTraerListaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        lo.MarkErrorAsHandled()   '????
    '    End If
    'End Sub

    Private Sub TerminoTraerListasDetallePorDefecto_Completed(ByVal lo As LoadOperation(Of Lista))
        If Not lo.HasError Then
            ListaDetallePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Lista por defecto", _
                                             Me.ToString(), "TerminoTraerListaDetallePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerListas(ByVal lo As LoadOperation(Of ListaConfiguracion))
        If Not lo.HasError Then
            ListaListas = dcProxy.ListaConfiguracions.ToList

            'If dcProxy.ListaConfiguracions.Count > 0 Then
            '    If lo.UserState = "insert" Then
            '        ListaSelected = ListaListas.Last
            '    End If
            'Else
            If dcProxy.ListaConfiguracions.Count = 0 And (lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial") Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'MyBase.Buscar()
                'MyBase.CancelarBuscar()
            End If
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Listas", _
                                         Me.ToString(), "TerminoTraerListas", Application.Current.ToString(), Program.Maquina, lo.Error)
        lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerListasDetalle(ByVal lo As LoadOperation(Of Lista))
        If Not lo.HasError Then
            ListaListasDetalle = objProxyActualizar.Listas
            For Each li In ListaListasDetalle
                li.PermiteModificar = False
                li.Seleccionado = False
            Next

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Listas", _
                                             Me.ToString(), "TerminoTraerListasDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region
#Region "Resultados Sincronos"
    Public Async Function ValidarRetornos() As System.Threading.Tasks.Task(Of Boolean)
        Dim logResultado As Boolean = False
        IsBusy = True
        Try
            Dim objRet As LoadOperation(Of Confirmacion)
            Dim strRetornos As String = ""

            ErrorForma = String.Empty

            dcProxy.Confirmacions.Clear()

            For Each li In ListaListasDetalle
                If li.Seleccionado Then
                    If String.IsNullOrEmpty(strRetornos) Then
                        strRetornos = li.Retorno
                    Else
                        strRetornos = String.Format("{0}|{1}", strRetornos, li.Retorno)
                    End If
                End If
            Next
            

            objRet = Await dcProxy.Load(dcProxy.ValidarRetornosSyncQuery(ListaSelected.Topico, strRetornos, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al validar los retornos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los retornos.", Me.ToString(), "ValidarRetornos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaResultado = objRet.Entities
                        If objListaResultado.Where(Function(i) i.Exitoso = False).Count() > 0 Then
                            If objListaResultado.Where(Function(i) i.Tipo = "NINGUNO").Count() > 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.Where(Function(i) i.Tipo = "NINGUNO").First.Mensage, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else

                                Dim strMensage As String = "Los Siguientes retornos no se pueden eliminar:"
                                For Each li In objListaResultado
                                    If li.Exitoso = False Then
                                        strMensage = String.Format("{0}{1}{2}", strMensage, vbCrLf, li.Mensage)
                                    Else

                                        If ListaListasDetalle.Where(Function(i) i.Retorno = li.Retorno).Count > 0 Then
                                            ListaListasDetalle.Remove(ListaListasDetalle.Where(Function(i) i.Retorno = li.Retorno).First)
                                        End If


                                        If ListaListasDetalle.Count > 0 Then
                                            ListaDetalleSelected = ListaListasDetalle.FirstOrDefault
                                        End If

                                    End If
                                Next
                                A2Utilidades.Mensajes.mostrarMensaje(strMensage, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        


                        Else
                            Dim objListaRemover As New List(Of Lista)

                            For Each li In ListaListasDetalle
                                If li.Seleccionado Then
                                    objListaRemover.Add(li)
                                End If
                            Next

                            For Each li In objListaRemover
                                If ListaListasDetalle.Where(Function(i) i.IDLista = li.IDLista).Count > 0 Then
                                    ListaListasDetalle.Remove(ListaListasDetalle.Where(Function(i) i.IDLista = li.IDLista).First)
                                End If
                            Next

                            If ListaListasDetalle.Count > 0 Then
                                ListaDetalleSelected = ListaListasDetalle.FirstOrDefault
                            End If

                         

                        End If
                    End If
                    Dim objListaEliminarVacios As New List(Of Lista)

                    For Each li In ListaListasDetalle
                        If li.Seleccionado And IsNothing(li.Retorno) Then
                            objListaEliminarVacios.Add(li)
                        End If
                    Next

                    For Each li In objListaEliminarVacios
                        If ListaListasDetalle.Where(Function(i) i.IDLista = li.IDLista).Count > 0 Then
                            ListaListasDetalle.Remove(ListaListasDetalle.Where(Function(i) i.IDLista = li.IDLista).First)
                        End If
                    Next

                    If ListaListasDetalle.Count > 0 Then
                        ListaDetalleSelected = ListaListasDetalle.FirstOrDefault
                    End If

                    MyBase.CambioItem("ListaDetalleSelected")
                    MyBase.CambioItem("ListaListasDetalle")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los retornos.", Me.ToString(), "ValidarRetornos", Application.Current.ToString(), Program.Maquina, ex)
            logResultado = False
        Finally
        End Try
        IsBusy = False
        Return (logResultado)
    End Function

#End Region


#Region "Propiedades"
    'Lista Configuracion
    Private _ListaListas As List(Of ListaConfiguracion)
    Public Property ListaListas() As List(Of ListaConfiguracion)
        Get
            Return _ListaListas
        End Get
        Set(ByVal value As List(Of ListaConfiguracion))
            _ListaListas = value
            If Not IsNothing(value) Then
                If IsNothing(ListaAnterior) Then
                    ListaSelected = _ListaListas.FirstOrDefault
                Else
                    ListaSelected = ListaAnterior
                End If
            End If
            MyBase.CambioItem("ListaListas")
            MyBase.CambioItem("ListaListasPaged")
        End Set
    End Property

    Public ReadOnly Property ListaListasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaListas) Then
                Dim view = New PagedCollectionView(_ListaListas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


    Private _ListaSelected As ListaConfiguracion
    Public Property ListaSelected() As ListaConfiguracion
        Get
            Return _ListaSelected
        End Get
        Set(ByVal value As ListaConfiguracion)
            _ListaSelected = value
            If Not value Is Nothing Then
                ConsultarDetalle()
                HabilitarControlesDetalle()

            End If
            MyBase.CambioItem("ListaSelected")
        End Set
    End Property


    Private _Editareg As Boolean = False
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property

    Private _VisibleTexto As Visibility = Visibility.Collapsed
    Public Property VisibleTexto() As Visibility
        Get
            Return _VisibleTexto
        End Get
        Set(ByVal value As Visibility)
            _VisibleTexto = value
            MyBase.CambioItem("VisibleTexto")
        End Set
    End Property

    Private _VisibleEntero As Visibility = Visibility.Collapsed
    Public Property VisibleEntero() As Visibility
        Get
            Return _VisibleEntero
        End Get
        Set(ByVal value As Visibility)
            _VisibleEntero = value
            MyBase.CambioItem("VisibleEntero")
        End Set
    End Property

    Private _VisibleDecimal As Visibility = Visibility.Collapsed
    Public Property VisibleDecimal() As Visibility
        Get
            Return _VisibleDecimal
        End Get
        Set(ByVal value As Visibility)
            _VisibleDecimal = value
            MyBase.CambioItem("VisibleDecimal")
        End Set
    End Property

    Private _VisibleFecha As Visibility = Visibility.Collapsed
    Public Property VisibleFecha() As Visibility
        Get
            Return _VisibleFecha
        End Get
        Set(ByVal value As Visibility)
            _VisibleFecha = value
            MyBase.CambioItem("VisibleFecha")
        End Set
    End Property

    Private _MaximoLongitudCampos As String
    Public Property MaximoLongitudCampos() As String
        Get
            Return _MaximoLongitudCampos
        End Get
        Set(ByVal value As String)
            _MaximoLongitudCampos = value
            MyBase.CambioItem("MaximoLongitudCampos")
        End Set
    End Property

    Private _MaximoLongitudCamposNegativos As String
    Public Property MaximoLongitudCamposNegativos() As String
        Get
            Return _MaximoLongitudCamposNegativos
        End Get
        Set(ByVal value As String)
            _MaximoLongitudCamposNegativos = value
            MyBase.CambioItem("MaximoLongitudCamposNegativos")
        End Set
    End Property

    Private _MaximoLongitudDescripcionCampos As String
    Public Property MaximoLongitudDescripcionCampos() As String
        Get
            Return _MaximoLongitudDescripcionCampos
        End Get
        Set(ByVal value As String)
            _MaximoLongitudDescripcionCampos = value

            MyBase.CambioItem("MaximoLongitudDescripcionCampos")
        End Set
    End Property

    Private _FechaActual As DateTime = Now.Date
    Public Property FechaActual() As DateTime
        Get
            Return _FechaActual
        End Get
        Set(ByVal value As DateTime)
            _FechaActual = value
            MyBase.CambioItem("FechaActual")
        End Set
    End Property


#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                                 Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ListaConfiguracions.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ListaConfiguracionFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ListaConfiguracionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Topico <> String.Empty Or cb.Descripcion <> String.Empty Or cb.TipoDato <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ListaConfiguracions.Clear()
                ListaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Topico = " &  cb.Topico.ToString() & " Descripcion = " &  cb.Descripcion.ToString() & " Retorno = " &  cb.Retorno.ToString() 
                dcProxy.Load(dcProxy.ListaConfiguracionConsultarQuery(cb.Topico, cb.Descripcion, cb.TipoDato, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConfiguracionLista
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
            If IsNothing(ListaListasDetalle) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de haber por lo menos un registro configurado en el detalle .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim numeroErrores = (From lr In ListaListasDetalle Where lr.HasValidationErrors = True).Count
            If numeroErrores <> 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle que hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaListasDetalle.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de haber por lo menos un registro configurado en el detalle .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            If ListaSelected.longitud > 80 Then
                A2Utilidades.Mensajes.mostrarMensaje("La longitud de la configuracion es mayor a la registrada en base datos, por favor comuniquese con el administrador .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaSelected.LongitudDescripcion > 200 Then
                A2Utilidades.Mensajes.mostrarMensaje("La Longitud de la descripcion configurada es mayor a la registrada en base datos, por favor comuniquese con el administrador .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            For Each li In ListaListasDetalle
                li.HabilitarConfiguracionLista = True
                If ListaSelected.TipoDato = "TIPO_DATO_FECHA" Then
                    If Not String.IsNullOrEmpty(li.RetornoFecha) Then
                        li.Retorno = li.RetornoFecha
                    End If
                ElseIf ListaSelected.TipoDato = "TIPO_DATO_ENTERO" Then
                    If Not String.IsNullOrEmpty(li.RetornoEntero) Then
                        li.Retorno = li.RetornoEntero
                    End If
                ElseIf ListaSelected.TipoDato = "TIPO_DATO_DECIMAL" Then
                    If Not String.IsNullOrEmpty(li.RetornoDecimal) Then
                        li.Retorno = li.RetornoDecimal
                    End If
                ElseIf ListaSelected.TipoDato = "TIPO_DATO_TEXTO" Then
                    If Not String.IsNullOrEmpty(li.RetornoTexto) Then
                        li.Retorno = li.RetornoTexto
                    End If
                End If

            Next

            For Each li In ListaListasDetalle
                If String.IsNullOrEmpty(li.Retorno) Or String.IsNullOrEmpty(li.Descripcion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El retorno y la descripcion son requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            Next

            Dim logRepetido As Boolean = False
            Dim strRetornoRepetido As String = String.Empty

            For Each li In ListaListasDetalle
                If ListaListasDetalle.Where(Function(i) i.Retorno = li.Retorno And i.IDLista <> li.IDLista).Count > 0 Then
                    logRepetido = True
                    strRetornoRepetido = li.Retorno
                    Exit For
                End If
            Next

            If logRepetido Then
                A2Utilidades.Mensajes.mostrarMensaje("Esta intentado registrar un retorno duplicado,  (" + strRetornoRepetido + " ) por favor revise la informacion antes de guardarla .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If



            Dim origen = "update"
            ErrorForma = ""
            'ListaAnterior = ListaSelected
            'If Not ListaListas.Contains(ListaSelected) Then
            '    origen = "insert"
            '    ListaListas.Add(ListaSelected)
            'End If


            IsBusy = True
            Program.VerificarCambiosProxyServidor(objProxyActualizar)
            objProxyActualizar.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
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
                'Dim numeroErrores = (From lr In ListaListasDetalle Where lr.HasValidationErrors = True).Count
                'If numeroErrores <> 0 Then
                '    A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle que hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    So.MarkErrorAsHandled()
                '    Exit Sub
                'End If
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "insert" Or So.UserState = "update") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    'If So.UserState = "insert" Then
                    '    ListaListas.Remove(ListaSelected)
                    '    ListaListasDetalle.Remove(ListaDetalleSelected)
                    'End If

                    If So.Error.Message.Contains("ErrorPersonalizado,") = True Then
                        objProxyActualizar.RejectChanges()
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    objProxyActualizar.RejectChanges()
                End If

                So.MarkErrorAsHandled()

                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.Listas.Clear()
            '    dcProxy.Load(dcProxy.ListasFiltrarQuery(""), AddressOf TerminoTraerListas, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
            Editareg = False
            ListaDetalleSelected.PermiteModificar = False
            ConsultarDetalle()
            HabilitarControlesDetalle()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ListaSelected) Then
            'HabilitarActivo = True
            Editando = True
            ' Editareg = False
            If ListaSelected.Modificable = True Then
                Editareg = True
            Else
                Editareg = False
            End If
            MyBase.CambioItem("Editando")

        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ListaSelected) Then
                objProxyActualizar.RejectChanges()

                If _ListaSelected.EntityState = EntityState.Detached Then
                    ListaSelected = ListaAnterior
                Else
                    ConsultarDetalle()
                    HabilitarControlesDetalle()
                End If
            Else
                ListaDetalleSelected = Nothing
                ListaListasDetalle = Nothing
            End If
            Editando = False
            Editareg = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaConfiguracionLista
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '  Try
        '      If Not IsNothing(_ListaSelected) Then
        '              dcProxy.Listas.Remove(_ListaSelected)
        '              ListaSelected = _ListaListas.LastOrDefault
        '          IsBusy = True
        '              dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '      End If
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '       Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub

    'Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
    '    If DicCamposTab.ContainsKey(pstrNombreCampo) Then
    '        Dim miTab = DicCamposTab(pstrNombreCampo)
    '        TabSeleccionadaFinanciero = miTab
    '    End If

    'End Sub

    'Public Sub llenarDiccionario()
    '    DicCamposTab.Add("Topico", 1)
    '    DicCamposTab.Add("TipoDato", 1)
    '    DicCamposTab.Add("Descripcion", 1)
    'End Sub

    Public Sub ConsultarDetalle()
        Try
            If Not IsNothing(ListaSelected) Then
                ErrorForma = ""
                objProxyActualizar.Listas.Clear()
                objProxyActualizar.Load(objProxyActualizar.ListasConsultarQuery(ListaSelected.Topico, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerListasDetalle, "Busqueda")

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub HabilitarControlesDetalle()
        Try
            If Not IsNothing(ListaSelected) Then
                MaximoLongitudDescripcionCampos = ListaSelected.LongitudDescripcion.ToString
                If ListaSelected.TipoDato = "TIPO_DATO_TEXTO" Then
                    VisibleTexto = Visibility.Visible
                    VisibleDecimal = Visibility.Collapsed
                    VisibleEntero = Visibility.Collapsed
                    VisibleFecha = Visibility.Collapsed

                    MaximoLongitudCampos = ListaSelected.longitud.ToString


                ElseIf ListaSelected.TipoDato = "TIPO_DATO_ENTERO" Then
                    VisibleTexto = Visibility.Collapsed
                    VisibleDecimal = Visibility.Collapsed
                    VisibleEntero = Visibility.Visible
                    VisibleFecha = Visibility.Collapsed
                    MaximoLongitudCampos = ListaSelected.longitud.ToString

                    'Dim intContador As Integer = 1
                    'Dim strLongitud As String = ""

                    'While intContador <= ListaSelected.longitud
                    '    If String.IsNullOrEmpty(strLongitud) Then
                    '        strLongitud = "9"
                    '    Else
                    '        strLongitud = String.Format("{0}9", strLongitud)
                    '    End If
                    '    intContador += 1
                    'End While

                    'MaximoLongitudCampos = strLongitud
                    'MaximoLongitudCamposNegativos = String.Format("-{0}", strLongitud)

                ElseIf ListaSelected.TipoDato = "TIPO_DATO_DECIMAL" Then
                    VisibleTexto = Visibility.Collapsed
                    VisibleDecimal = Visibility.Visible
                    VisibleEntero = Visibility.Collapsed
                    VisibleFecha = Visibility.Collapsed

                    Dim intContador As Integer = 1
                    Dim strLongitud As String = ""

                    While intContador <= ListaSelected.longitud
                        If String.IsNullOrEmpty(strLongitud) Then
                            strLongitud = "9"
                        Else
                            strLongitud = String.Format("{0}9", strLongitud)
                        End If
                        intContador += 1
                    End While

                    MaximoLongitudCampos = strLongitud
                    MaximoLongitudCamposNegativos = String.Format("-{0}", strLongitud)

                ElseIf ListaSelected.TipoDato = "TIPO_DATO_FECHA" Then
                    VisibleTexto = Visibility.Collapsed
                    VisibleDecimal = Visibility.Collapsed
                    VisibleEntero = Visibility.Collapsed
                    VisibleFecha = Visibility.Visible

                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


#Region "Tablas hijas"

    Private _ListaListasDetalle As EntitySet(Of Lista)
    Public Property ListaListasDetalle() As EntitySet(Of Lista)
        Get
            Return _ListaListasDetalle
        End Get
        Set(ByVal value As EntitySet(Of Lista))
            _ListaListasDetalle = value

            MyBase.CambioItem("ListaListasDetalle")
            MyBase.CambioItem("ListaListasDetallePaged")
            If Not IsNothing(value) Then
                If IsNothing(ListaDetalleAnterior) Then
                    ListaDetalleSelected = _ListaListasDetalle.FirstOrDefault
                Else
                    ListaDetalleSelected = ListaDetalleAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaListasDetallePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaListasDetalle) Then
                Dim view = New PagedCollectionView(_ListaListasDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaDetalleSelected As Lista
    Public Property ListaDetalleSelected() As Lista
        Get
            Return _ListaDetalleSelected
        End Get
        Set(ByVal value As Lista)
            _ListaDetalleSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("ListaDetalleSelected")
        End Set
    End Property

    'Private _Longitud As Integer = 0
    'Public Property Longitud() As Integer
    '    Get
    '        Return _Longitud
    '    End Get
    '    Set(ByVal value As Integer)
    '        _Longitud = value
    '        MyBase.CambioItem("Longitud")
    '    End Set
    'End Property





#End Region

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmListas"

                    '              
                    Dim NewListaDetalle As New Lista
                    If IsNothing(ListaListasDetalle) Then
                        ListaListasDetalle = objProxyActualizar.Listas
                    End If
                    If ListaListasDetalle.Count > 0 Then
                        NewListaDetalle.IDLista = -(ListaListasDetalle.Count + 1)

                    End If
                    NewListaDetalle.Topico = ListaSelected.Topico
                    NewListaDetalle.Descripcion = ListaDetallePorDefecto.Descripcion
                    NewListaDetalle.Retorno = ListaDetallePorDefecto.Retorno
                    NewListaDetalle.Comentario = ListaDetallePorDefecto.Comentario
                    NewListaDetalle.PermiteModificar = True
                    NewListaDetalle.Seleccionado = False
                    NewListaDetalle.Usuario = Program.Usuario
                    NewListaDetalle.Activo = True
                    NewListaDetalle.RetornoTexto = String.Empty
                    NewListaDetalle.RetornoEntero = String.Empty
                    NewListaDetalle.RetornoDecimal = "0"
                    NewListaDetalle.RetornoFecha = Now.Date.ToString("yyyy-MM-dd")

                    ListaListasDetalle.Add(NewListaDetalle)
                    ListaDetalleSelected = NewListaDetalle


                    MyBase.CambioItem("ListaDetalleSelected")
                    MyBase.CambioItem("ListaListasDetalle")
                    'MyBase.CambioItem("Editando")

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al añadir un registro", _
             Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está habilitada para este maestro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'Select Case NombreColeccionDetalle
            '    Case "cmListas"
            '        If Not IsNothing(ListaListasDetalle) Then
            '            If Not IsNothing(_ListaDetalleSelected) Then
            '                Dim objListaRemover As New List(Of Lista)

            '                For Each li In ListaListasDetalle
            '                    If li.Seleccionado Then
            '                        objListaRemover.Add(li)
            '                    End If
            '                Next
            '                If objListaRemover.Count > 0 Then
            '                    A2Utilidades.Mensajes.mostrarMensajePregunta("¿Esta seguro que desea eliminar los registros?", _
            '                          Program.TituloSistema, _
            '                          "ELIMINARDETALLE", _
            '                          AddressOf TerminoMensajePregunta, False)
            '                Else
            '                    A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione uno o mas registros para eliminar ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '                End If

            '            End If
            '        End If
            'End Select

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al eliminar un registro", _
            Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Async Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "ELIMINARDETALLE"
                            If objResultado.DialogResult Then
                                Await ValidarRetornos()
                            Else
                                IsBusy = False
                            End If
                    End Select
                End If
            Else
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje pregunta.", Me.ToString(), "TerminoMensajePregunta", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaConfiguracionLista

    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
     <Display(Name:="Tópico")> _
    Public Property Topico As String

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Descripción")> _
    Public Property Descripcion As String

    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")>
    <Display(Name:="Tipo de dato")>
    Public Property TipoDato As String

End Class

'Public Class CamposBusquedaListadetalle

'    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
'     <Display(Name:="Tópico")> _
'    Public Property Topico As String

'    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
'     <Display(Name:="Nombre")> _
'    Public Property Descripcion As String

'    <StringLength(80, ErrorMessage:="La longitud máxima es de 80")> _
'     <Display(Name:="Retorno")> _
'    Public Property Retorno As String


'End Class




