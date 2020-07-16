Imports Telerik.Windows.Controls

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
Public Class DuplicarClientesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Dim dcProxy As ClientesDomainContext
    Dim objParametros As List(Of OYDUtilidades.ItemCombo) = Nothing
    Private _logPersonaJuridica As Boolean = False

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New ClientesDomainContext()
                dcProxyUtilidades = New UtilidadesDomainContext()
            Else
                dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el objeto.", _
                                Me.ToString(), "DuplicarClientesViewModel.new", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#Region "Propiedades"

    Private _DuplicarDatosClientesSelected As DuplicarDatosClientes = New DuplicarDatosClientes
    Public Property DuplicarDatosClientesSelected() As DuplicarDatosClientes
        Get
            Return _DuplicarDatosClientesSelected
        End Get
        Set(ByVal value As DuplicarDatosClientes)
            _DuplicarDatosClientesSelected = value
            MyBase.CambioItem("DuplicarDatosClientesSelected")
        End Set
    End Property

    Private _MostrarNombreCompleto As Boolean = False
    Public Property MostrarNombreCompleto() As Boolean
        Get
            Return _MostrarNombreCompleto
        End Get
        Set(ByVal value As Boolean)
            _MostrarNombreCompleto = value
            MyBase.CambioItem("MostrarNombreCompleto")
        End Set
    End Property

    Private _HabilitarNroIdentificacion As Boolean = True
    Public Property HabilitarNroIdentificacion() As Boolean
        Get
            Return _HabilitarNroIdentificacion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNroIdentificacion = value
            MyBase.CambioItem("HabilitarNroIdentificacion")
        End Set
    End Property

    Private _NroDocumentoConsulta As String
    Public Property NroDocumentoConsulta() As String
        Get
            Return _NroDocumentoConsulta
        End Get
        Set(ByVal value As String)
            _NroDocumentoConsulta = value
            MyBase.CambioItem("NroDocumentoConsulta")
        End Set
    End Property

    Private _VisualizarDuplicar As Visibility = Visibility.Visible
    Public Property VisualizarDuplicar() As Visibility
        Get
            Return _VisualizarDuplicar
        End Get
        Set(ByVal value As Visibility)
            _VisualizarDuplicar = value
            MyBase.CambioItem("VisualizarDuplicar")
        End Set
    End Property

    Private _VisualizarAceptar As Visibility = Visibility.Collapsed
    Public Property VisualizarAceptar() As Visibility
        Get
            Return _VisualizarAceptar
        End Get
        Set(ByVal value As Visibility)
            _VisualizarAceptar = value
            MyBase.CambioItem("VisualizarAceptar")
        End Set
    End Property

#End Region

#Region "Metodos"



    Public Sub ActualizarDatosCliente(ByVal pobjCliente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjCliente) Then
                If IsNothing(_DuplicarDatosClientesSelected) Then
                    DuplicarDatosClientesSelected = New DuplicarDatosClientes
                End If

                _DuplicarDatosClientesSelected.NroIdentificacion = pobjCliente.NroDocumento
                _DuplicarDatosClientesSelected.CodigoOYD = pobjCliente.CodigoOYD
                _DuplicarDatosClientesSelected.CodigoLiderAgrupador = pobjCliente.CodigoOYD
                _DuplicarDatosClientesSelected.TipoIdentificacion = pobjCliente.CodTipoIdentificacion

                If (From c In CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2")
                             Where c.ID = pobjCliente.CodTipoIdentificacion Select c).Count >= 1 Then
                    _DuplicarDatosClientesSelected.PrimerNombre = pobjCliente.Nombre
                    MostrarNombreCompleto = True
                    _logPersonaJuridica = True
                Else
                    _DuplicarDatosClientesSelected.PrimerNombre = pobjCliente.PrimerNombre
                    _DuplicarDatosClientesSelected.SegundoNombre = pobjCliente.SegundoNombre
                    _DuplicarDatosClientesSelected.PrimerApellido = pobjCliente.PrimerApellido
                    _DuplicarDatosClientesSelected.SegundoApellido = pobjCliente.SegundoApellido
                    MostrarNombreCompleto = False
                    _logPersonaJuridica = False
                End If

                _DuplicarDatosClientesSelected.DireccionEnvio = pobjCliente.DireccionEnvio
                _DuplicarDatosClientesSelected.TipoCliente = pobjCliente.CodTipoCliente
                _DuplicarDatosClientesSelected.TipoProducto = pobjCliente.CodTipoProducto
                _DuplicarDatosClientesSelected.TelefonoPrincipal = pobjCliente.Telefono

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los datos del cliente.", _
                                 Me.ToString(), "ActualizarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarDatosCliente()
        Try
            If Not IsNothing(_DuplicarDatosClientesSelected) Then
                If NroDocumentoConsulta = _DuplicarDatosClientesSelected.NroIdentificacion Or dcProxyUtilidades.IsLoading Then
                    Exit Sub
                End If

                If String.IsNullOrEmpty(NroDocumentoConsulta) Or NroDocumentoConsulta = "0" Then
                    LimpiarDatosCliente()
                    Exit Sub
                End If
            End If
            IsBusy = True

            If Not IsNothing(dcProxyUtilidades.BuscadorClientes) Then
                dcProxyUtilidades.BuscadorClientes.Clear()
            End If

            dcProxyUtilidades.Load(dcProxyUtilidades.buscarClientesQuery(NroDocumentoConsulta, "A", String.Empty, "clienteagrupador", Program.Usuario, False, 0, Program.HashConexion), AddressOf TerminoConsultarDatosCliente, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente.", _
                                 Me.ToString(), "ConsultarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DuplicarRegistro()
        Try
            If String.IsNullOrEmpty(NroDocumentoConsulta) Or NroDocumentoConsulta = "0" Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                HabilitarNroIdentificacion = False
                Editando = True
                MyBase.CambioItem("Editando")
                VisualizarAceptar = Visibility.Visible
                VisualizarDuplicar = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al dupicar el registro.", _
                                Me.ToString(), "DuplicarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AceptarCambios()
        Try
            If Editando = False Then
                Exit Sub
            End If

            If String.IsNullOrEmpty(NroDocumentoConsulta) Or NroDocumentoConsulta = "0" Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                ValidarDuplicarRegistro()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al dupicar el registro.", _
                                Me.ToString(), "DuplicarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se crea método para cancelar los cambios cuando no se desea duplicar el cliente 
    ''' </summary>
    ''' <remarks>SV20150122</remarks>
    Public Sub CancelarCambios()
        Try
            LimpiarDatosCliente()
            Editando = False
            NroDocumentoConsulta = "0"
            VisualizarAceptar = Visibility.Collapsed
            VisualizarDuplicar = Visibility.Visible
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar los cambios.", _
                                Me.ToString(), "CancelarCambios", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ValidarDuplicarRegistro()
        Dim logPasoValidacion As Boolean = True
        Dim strMensaje As String = "Hay algunas validaciones:"
        Try
            If _logPersonaJuridica = False Then
                If String.IsNullOrEmpty(_DuplicarDatosClientesSelected.PrimerApellido) Then
                    strMensaje = String.Format("{0}{1}  + Debe especificar el primer apellido.", strMensaje, vbCrLf)
                    logPasoValidacion = False
                End If

                If String.IsNullOrEmpty(_DuplicarDatosClientesSelected.PrimerNombre) Then
                    strMensaje = String.Format("{0}{1}  + Debe especificar el primer nombre.", strMensaje, vbCrLf)
                    logPasoValidacion = False
                End If
            Else
                If String.IsNullOrEmpty(_DuplicarDatosClientesSelected.PrimerNombre) Then
                    strMensaje = String.Format("{0}{1}  + Debe especificar el nombre completo.", strMensaje, vbCrLf)
                    logPasoValidacion = False
                End If
            End If

            If String.IsNullOrEmpty(_DuplicarDatosClientesSelected.TipoProducto) Then
                strMensaje = String.Format("{0}{1}  + Debe seleccionar el tipo de producto.", strMensaje, vbCrLf)
                logPasoValidacion = False
            End If

            If String.IsNullOrEmpty(_DuplicarDatosClientesSelected.TipoCliente) Then
                strMensaje = String.Format("{0}{1}  + Debe seleccionar el tipo de cliente.", strMensaje, vbCrLf)
                logPasoValidacion = False
            End If

            If logPasoValidacion = False Then
                mostrarMensaje(strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            Else
                mostrarMensajePregunta("Está seguro de duplicar el registro del cliente.", Program.TituloSistema, "DUPLICARCLIENTE", AddressOf TerminoConfirmarDuplicar)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos del registro.", _
                                Me.ToString(), "ValidarDuplicarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub LimpiarDatosCliente()
        Try
            If IsNothing(_DuplicarDatosClientesSelected) Then
                DuplicarDatosClientesSelected = New DuplicarDatosClientes
            End If

            _DuplicarDatosClientesSelected.NroIdentificacion = String.Empty
            _DuplicarDatosClientesSelected.CodigoOYD = String.Empty
            _DuplicarDatosClientesSelected.CodigoLiderAgrupador = String.Empty
            _DuplicarDatosClientesSelected.TipoIdentificacion = String.Empty
            _DuplicarDatosClientesSelected.PrimerNombre = String.Empty
            _DuplicarDatosClientesSelected.SegundoNombre = String.Empty
            _DuplicarDatosClientesSelected.PrimerApellido = String.Empty
            _DuplicarDatosClientesSelected.SegundoApellido = String.Empty
            _DuplicarDatosClientesSelected.DireccionEnvio = String.Empty
            _DuplicarDatosClientesSelected.TipoCliente = String.Empty
            _DuplicarDatosClientesSelected.TipoProducto = String.Empty
            _DuplicarDatosClientesSelected.TelefonoPrincipal = String.Empty
            _logPersonaJuridica = False
            HabilitarNroIdentificacion = True
            Editando = False
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los datos del cliente.", _
                                Me.ToString(), "LimpiarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConfirmarDuplicar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                ConsultarParametro()
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar sí queria duplicar el registro.", _
                                Me.ToString(), "TerminoConfirmarDuplicar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultarParametro()
        Try
            dcProxyUtilidades.Verificaparametro("VALIDA_DOC_TIPOPRODUCTO", Program.Usuario, Program.HashConexion, AddressOf TerminoTraerParametro, "VALIDADOCUMENTO")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el parametro.", _
                                Me.ToString(), "ConsultarParametro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub RealizarDuplicacionCliente()
        Try
            'Modificado por JDCP20141215
            'Solución de error al llevar el nombre completo del cliente
            '********************************************************************
            Dim strNombreCompleto As String = String.Empty

            If _logPersonaJuridica Then
                strNombreCompleto = _DuplicarDatosClientesSelected.PrimerNombre
            Else
                strNombreCompleto = _DuplicarDatosClientesSelected.PrimerApellido + " " + _DuplicarDatosClientesSelected.SegundoApellido + " " + _DuplicarDatosClientesSelected.PrimerNombre + " " + _DuplicarDatosClientesSelected.SegundoNombre
            End If

            dcProxy.DuplicarCliente(_DuplicarDatosClientesSelected.NroIdentificacion, _DuplicarDatosClientesSelected.PrimerNombre, _DuplicarDatosClientesSelected.SegundoNombre, _DuplicarDatosClientesSelected.PrimerApellido, _DuplicarDatosClientesSelected.SegundoApellido, strNombreCompleto, _DuplicarDatosClientesSelected.TipoProducto, _DuplicarDatosClientesSelected.TipoCliente, Program.Usuario, Program.HashConexion, AddressOf TerminoDuplicarCliente, String.Empty)
            '********************************************************************
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al duplicar el cliente.", _
                                Me.ToString(), "RealizarDuplicacionCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincronicos"

    Private Sub TerminoConsultarDatosCliente(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    NroDocumentoConsulta = lo.Entities.FirstOrDefault.NroDocumento
                    ActualizarDatosCliente(lo.Entities.First)
                Else
                    LimpiarDatosCliente()
                    NroDocumentoConsulta = "0"
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningun registro con estas caracteristicas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos del cliente.", _
                                 Me.ToString(), "TerminoConsultarDatosCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos del cliente.", _
                                 Me.ToString(), "TerminoConsultarDatosCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoTraerParametro(ByVal lo As InvokeOperation(Of String))
        Try
            If lo.HasError = False Then
                If Not String.IsNullOrEmpty(lo.Value) Then
                    If lo.UserState = "VALIDADOCUMENTO" Then
                        If lo.Value = "SI" Or lo.Value = "1" Then
                            If Not String.IsNullOrEmpty(_DuplicarDatosClientesSelected.TipoProducto) Then
                                dcProxy.ValidarClienteTipoProducto(_DuplicarDatosClientesSelected.NroIdentificacion, _DuplicarDatosClientesSelected.TipoProducto, "0", Program.Usuario, Program.HashConexion, AddressOf TerminoValidarTipoProducto, String.Empty)
                            Else
                                mostrarMensaje("El tipo de producto no existe en la lista.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                            End If
                        Else
                            RealizarDuplicacionCliente()
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener obtener el parametro.", _
                                 Me.ToString(), "TerminoTraerParametro", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener obtener el parametro.", _
                                 Me.ToString(), "TerminoTraerParametro", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoValidarTipoProducto(ByVal lo As InvokeOperation(Of Integer))
        Try
            If lo.HasError = False Then
                If lo.Value > 0 Then
                    'Obtiene la descripción del tipo de producto
                    If IsNothing(objParametros) Then
                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOPRODUCTO") Then
                            objParametros = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOPRODUCTO")
                        End If
                    End If

                    Dim strDescripcionTipoProducto As String = String.Empty

                    If objParametros.Where(Function(i) i.ID = _DuplicarDatosClientesSelected.TipoProducto).Count > 0 Then
                        strDescripcionTipoProducto = objParametros.Where(Function(i) i.ID = _DuplicarDatosClientesSelected.TipoProducto).First.Descripcion
                    End If

                    mostrarMensaje("Ya existe " & lo.Value.ToString & " con el mismo tipo de producto, para este cliente " & " Nro. Documento " & _DuplicarDatosClientesSelected.NroIdentificacion & " Tipo producto: " & strDescripcionTipoProducto, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                Else
                    RealizarDuplicacionCliente()
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el tipo de producto.", _
                                 Me.ToString(), "TerminoValidarTipoProducto", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el tipo de producto.", _
                                 Me.ToString(), "TerminoValidarTipoProducto", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoDuplicarCliente(ByVal lo As InvokeOperation(Of String))
        Try
            If lo.HasError = False Then
                If Not String.IsNullOrEmpty(lo.Value) Then
                    mostrarMensaje("Se duplicó el registro con el código " & LTrim(RTrim(lo.Value)), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarDatosCliente()
                    NroDocumentoConsulta = "0"
                    VisualizarAceptar = Visibility.Collapsed
                    VisualizarDuplicar = Visibility.Visible
                Else
                    mostrarMensaje("No se pudo duplicar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el tipo de producto.", _
                                 Me.ToString(), "TerminoValidarTipoProducto", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el tipo de producto.", _
                                 Me.ToString(), "TerminoValidarTipoProducto", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

End Class


Public Class DuplicarDatosClientes
    Implements INotifyPropertyChanged

    Private _CodigoOYD As String
    Public Property CodigoOYD() As String
        Get
            Return _CodigoOYD
        End Get
        Set(ByVal value As String)
            _CodigoOYD = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoOYD"))
        End Set
    End Property

    Private _NroIdentificacion As String
    Public Property NroIdentificacion() As String
        Get
            Return _NroIdentificacion
        End Get
        Set(ByVal value As String)
            _NroIdentificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NroIdentificacion"))
        End Set
    End Property

    Private _CodigoLiderAgrupador As String
    Public Property CodigoLiderAgrupador() As String
        Get
            Return _CodigoLiderAgrupador
        End Get
        Set(ByVal value As String)
            _CodigoLiderAgrupador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoLiderAgrupador"))
        End Set
    End Property

    Private _TipoIdentificacion As String
    Public Property TipoIdentificacion() As String
        Get
            Return _TipoIdentificacion
        End Get
        Set(ByVal value As String)
            _TipoIdentificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoIdentificacion"))
        End Set
    End Property

    Private _PrimerNombre As String
    Public Property PrimerNombre() As String
        Get
            Return _PrimerNombre
        End Get
        Set(ByVal value As String)
            _PrimerNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PrimerNombre"))
        End Set
    End Property

    Private _SegundoNombre As String
    Public Property SegundoNombre() As String
        Get
            Return _SegundoNombre
        End Get
        Set(ByVal value As String)
            _SegundoNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SegundoNombre"))
        End Set
    End Property

    Private _PrimerApellido As String
    Public Property PrimerApellido() As String
        Get
            Return _PrimerApellido
        End Get
        Set(ByVal value As String)
            _PrimerApellido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PrimerApellido"))
        End Set
    End Property

    Private _SegundoApellido As String
    Public Property SegundoApellido() As String
        Get
            Return _SegundoApellido
        End Get
        Set(ByVal value As String)
            _SegundoApellido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SegundoApellido"))
        End Set
    End Property

    Private _DireccionEnvio As String
    Public Property DireccionEnvio() As String
        Get
            Return _DireccionEnvio
        End Get
        Set(ByVal value As String)
            _DireccionEnvio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DireccionEnvio"))
        End Set
    End Property

    Private _TelefonoPrincipal As String
    Public Property TelefonoPrincipal() As String
        Get
            Return _TelefonoPrincipal
        End Get
        Set(ByVal value As String)
            _TelefonoPrincipal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TelefonoPrincipal"))
        End Set
    End Property

    Private _TipoProducto As String
    Public Property TipoProducto() As String
        Get
            Return _TipoProducto
        End Get
        Set(ByVal value As String)
            _TipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoProducto"))
        End Set
    End Property

    Private _TipoCliente As String
    Public Property TipoCliente() As String
        Get
            Return _TipoCliente
        End Get
        Set(ByVal value As String)
            _TipoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoCliente"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
