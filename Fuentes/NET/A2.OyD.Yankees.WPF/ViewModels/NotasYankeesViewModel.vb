Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: LiquidacionesViewModel.vb
'Generado el : 05/30/2011 09:18:58
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
Imports A2.OyD.OYDServer.RIA.Web.OyDYankees
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Public Class NotasYankeesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private NotasYankeesAnterior As NotasYankees
    Public Property cb As New CamposBusquedaNotasYankees
    Dim objProxy1 As UtilidadesDomainContext
    Dim objProxyTesoreria As TesoreriaDomainContext
    Dim dcProxy As YankeesDomainContext
    Dim dcProxy1 As YankeesDomainContext
    Dim fechaCierre As DateTime
    Dim intBasecalculoInteres As Integer
    Dim stralianza As String
    Dim intSecuencia As Integer
    Public Const GSTR_ESTADO_PENDIENTE = "P"
    Public Const GSTR_ESTADO_ANULADO = "A"
    Public Const GSTR_ESTADO_IMPRESO = "I"
    Public Const GSTR_TEXTO_CONSECUTIVO = "Ajustar Clientes Yankees"
    Dim Numero As Integer = 0
    Dim intContadorVerificacion As Integer
    Public _mlogBuscarCliente As Boolean = True
    Public _mlogBuscarCuentaContable As Boolean = True
    Dim _lngIdDocumento As Integer

#Region "Inicializaciones"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New YankeesDomainContext()
            dcProxy1 = New YankeesDomainContext()
            objProxy1 = New UtilidadesDomainContext()
            objProxyTesoreria = New TesoreriaDomainContext()
        Else
            dcProxy = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy1 = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            objProxyTesoreria = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioTesoreria)))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "NotasYankees.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "ResultadosAsincronicos"

    ''' <summary>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 17/2013 - Resultado Ok 
    ''' </summary>
    ''' <param name="lo"></param>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If (_mlogBuscarCliente) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        IsBusyComitente = False
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        SelectedNotasYankeesDetalle.strNombre = String.Empty
                        SelectedNotasYankeesDetalle.strIdComitente = String.Empty
                    Else
                        Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                    End If
                Else
                    IsBusyComitente = False
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    SelectedNotasYankeesDetalle.strNombre = String.Empty

                End If
            End If
        Catch ex As Exception
            IsBusyComitente = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para verificar si el reporte se imprimio correctamente, si se imprimio correctamente se cambia el estado del documento de Tesorería a Impreso y en
    ''' caso contrario se vuelve a verificar si ya se imprimio el reporte, si ha pasado un numero de veces predeterminado quiere decir que ocurrio un error con
    ''' el reporte.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130312</remarks>
    Private Sub TeminoVericarEstadoImpresion(ByVal lo As InvokeOperation(Of Boolean))
        Try
            IsBusy = False
            If Not lo.HasError Then
                If lo.Value Then
                    _lngIdDocumento = SelectedNotasYankees.lngIdDocumento
                    dcProxy.NotasYankees.Clear()
                    dcProxy.NotasYankeesDetalles.Clear()
                    IsBusy = True
                    dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "TERMINO_IMPRIMIR")
                Else
                    If intContadorVerificacion < 10 Then
                        System.Threading.Thread.Sleep(5000)
                        IsBusy = True
                        intContadorVerificacion = intContadorVerificacion + 1
                        dcProxy.Verificar_EstadoImpresion(_SelectedNotasYankees.lngIdDocumento, _SelectedNotasYankees.strNombreConsecutivo, "N", Program.Usuario, Program.HashConexion, AddressOf TeminoVericarEstadoImpresion, "verificar")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Se presento un error con el reporte de impresion del documento de Nota yankees", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TeminoVericarEstadoImpresion", _
                                  Me.ToString(), "TeminoVericarEstadoImpresion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TeminoVericarEstadoImpresion", _
                     Me.ToString(), "TeminoVericarEstadoImpresion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerNotasYankees(ByVal lo As LoadOperation(Of NotasYankees))
        Try
            If Not lo.HasError Then
                If dcProxy.NotasYankees.Count > 0 Then
                    ListaNotasYankees = dcProxy.NotasYankees.ToList
                    If lo.UserState = "TERMINO_IMPRIMIR" Or lo.UserState = "TERMINO_ANULAR" Then
                        SelectedNotasYankees = (From C In ListaNotasYankees Where C.lngIdDocumento = _lngIdDocumento Select C).FirstOrDefault
                        _lngIdDocumento = Nothing
                        MyBase.CambioItem("SelectedNotasYankees")
                    End If
                    MyBase.CambiarFormulario_Forma_Manual()
                Else
                    ListaNotasYankees = Nothing
                    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroInicial" Or lo.UserState = "FiltroVM" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        visNavegando = "Collapsed"
                        MyBase.CambioItem("visNavegando")
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Notas Yankees", _
                                                 Me.ToString(), "TerminoTraerNotasYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False
            cb = New CamposBusquedaNotasYankees
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Notas Yankees", _
                                                      Me.ToString(), "TerminoTraerNotasYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub

    Private Sub TerminoTraerNotasYankeesDetalle(ByVal lo As LoadOperation(Of NotasYankeesDetalle))
        Try
            If Not lo.HasError Then
                If dcProxy.NotasYankeesDetalles.Count > 0 Then
                    ListaNotasYankeesDetalle = dcProxy.NotasYankeesDetalles.ToList
                Else
                    ListaNotasYankeesDetalle = Nothing
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista detelle Notas Yankees", _
                                                 Me.ToString(), "TerminoTraerNotasYankeesDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista detelle Notas Yankees", _
                                                         Me.ToString(), "TerminoTraerNotasYankeesDetalle", Application.Current.ToString(), Program.Maquina, lo.Error)

        End Try
    End Sub

    Private Sub TerminoActualizarNotasYankees(lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
                Editando = False
                MyBase.CambioItem("Editando")
                IsBusy = True
                If lo.UserState = "ING" Then
                    dcProxy.NotasYankees.Clear()
                    dcProxy.NotasYankeesDetalles.Clear()
                    dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, lo.UserState)
                Else
                    IsBusy = False
                    Editando = False
                    MyBase.CambioItem("Editando")
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualizacion del registro", _
                                                Me.ToString(), "TerminoActualizarNotasYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualizacion del registro", _
                                 Me.ToString(), "TerminoActualizarNotasYankees.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoEliminarNotasYankees(lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
                IsBusy = False
                mostrarMensaje("Se anuló correctamente el registro", "", A2Utilidades.wppMensajes.TiposMensaje.Exito)
                _lngIdDocumento = SelectedNotasYankees.lngIdDocumento
                dcProxy.NotasYankees.Clear()
                dcProxy.NotasYankeesDetalles.Clear()
                IsBusy = True
                dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "TERMINO_ANULAR")
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la anulación del registro", _
                                                Me.ToString(), "TerminoEliminarNotasYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la anulación del registro", _
                                 Me.ToString(), "TerminoEliminarNotasYankees.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades"

    Private _FechaBusqueda As DateTime = Now
    Public Property FechaBusqueda() As DateTime
        Get
            Return _FechaBusqueda
        End Get
        Set(ByVal value As DateTime)
            _FechaBusqueda = value
            MyBase.CambioItem("FechaBusqueda")
        End Set
    End Property


    Private _ListaNotasYankees As List(Of NotasYankees)
    Public Property ListaNotasYankees() As List(Of NotasYankees)
        Get
            Return _ListaNotasYankees
        End Get
        Set(ByVal value As List(Of NotasYankees))
            _ListaNotasYankees = value
            'If Not IsNothing(_ListaNotasYankees) Then
            '    SelectedNotasYankees = _ListaNotasYankees.FirstOrDefault
            'End If
            MyBase.CambioItem("ListaNotasYankees")
            MyBase.CambioItem("ListaNotasYankeesPaged")
        End Set
    End Property
    Public ReadOnly Property ListaNotasYankeesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaNotasYankees) Then
                Dim view = New PagedCollectionView(_ListaNotasYankees)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _SelectedNotasYankees As NotasYankees
    Public Property SelectedNotasYankees() As NotasYankees
        Get
            Return _SelectedNotasYankees
        End Get
        Set(ByVal value As NotasYankees)
            _SelectedNotasYankees = value
            If Not IsNothing(_SelectedNotasYankees) Then
                ValidarEstado(_SelectedNotasYankees.ValorEstado)
                dcProxy.NotasYankeesDetalles.Clear()
                NombreColeccionDetalle = "cmDetalleTesoreriYankees"
                dcProxy.Load(dcProxy.NotasYankeesFiltrarDetalleQuery(_SelectedNotasYankees.strNombreConsecutivo, _SelectedNotasYankees.lngIdDocumento, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankeesDetalle, "")
            End If
            MyBase.CambioItem("SelectedNotasYankees")
        End Set
    End Property

    Private _ListaNotasYankeesDetalle As List(Of NotasYankeesDetalle)
    Public Property ListaNotasYankeesDetalle() As List(Of NotasYankeesDetalle)
        Get
            Return _ListaNotasYankeesDetalle
        End Get
        Set(ByVal value As List(Of NotasYankeesDetalle))
            _ListaNotasYankeesDetalle = value
            If Not IsNothing(_ListaNotasYankeesDetalle) Then
                SelectedNotasYankeesDetalle = _ListaNotasYankeesDetalle.FirstOrDefault
            End If

            MyBase.CambioItem("ListaNotasYankeesDetalle")
            MyBase.CambioItem("ListaNotasYankeesDetallePaged")
        End Set
    End Property
    Public ReadOnly Property ListaNotasYankeesDetallePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaNotasYankeesDetalle) Then
                Dim view = New PagedCollectionView(_ListaNotasYankeesDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private WithEvents _SelectedNotasYankeesDetalle As NotasYankeesDetalle
    Public Property SelectedNotasYankeesDetalle() As NotasYankeesDetalle
        Get
            Return _SelectedNotasYankeesDetalle
        End Get
        Set(ByVal value As NotasYankeesDetalle)
            _SelectedNotasYankeesDetalle = value

            MyBase.CambioItem("SelectedNotasYankeesDetalle")
        End Set
    End Property
    Private _Pendiente As Boolean
    Public Property Pendiente() As Boolean
        Get
            Return _Pendiente
        End Get
        Set(ByVal value As Boolean)
            _Pendiente = value
            MyBase.CambioItem("Pendiente")
        End Set
    End Property
    Private _Anulado As Boolean
    Public Property Anulado() As Boolean
        Get
            Return _Anulado
        End Get
        Set(ByVal value As Boolean)
            _Anulado = value
            MyBase.CambioItem("Anulado")
        End Set
    End Property
    Private _Impreso As Boolean
    Public Property Impreso() As Boolean
        Get
            Return _Impreso
        End Get
        Set(ByVal value As Boolean)
            _Impreso = value
            MyBase.CambioItem("Impreso")
        End Set
    End Property
    Private _IsBusyComitente As Boolean = False
    Public Property IsBusyComitente() As Boolean
        Get
            Return _IsBusyComitente
        End Get
        Set(ByVal value As Boolean)
            _IsBusyComitente = value
            MyBase.CambioItem("IsBusyComitente")
        End Set
    End Property

#End Region

#Region "Metodos"

    ''' <summary>Autocompletar información del cliente</summary>
    ''' <param name="pobjComitente"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Según el código del cliente digitado por el usuario, se autocompleta el nombre.
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub ComitenteSeleccionadoM(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try

            If Not IsNothing(pobjComitente) Then
                If (_mlogBuscarCliente) Then
                    SelectedNotasYankeesDetalle.strNombre = pobjComitente.Nombre
                    IsBusyComitente = False
                End If
            End If
        Catch ex As Exception
            IsBusyComitente = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la asignación del nombre del comitente", _
                                                         Me.ToString(), "ComitenteSeleccionadoM", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidarEstado(pstrEstado As String)
        Try
            If pstrEstado = GSTR_ESTADO_PENDIENTE Then
                Pendiente = True
                Impreso = False
                Anulado = False
            ElseIf pstrEstado = GSTR_ESTADO_IMPRESO Then
                Pendiente = False
                Impreso = True
                Anulado = False
            ElseIf pstrEstado = GSTR_ESTADO_ANULADO Then
                Pendiente = False
                Impreso = False
                Anulado = True

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar Estado del Documento", _
                                 Me.ToString(), "ValidarEstado", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            NotasYankeesAnterior = SelectedNotasYankees
            Dim NewNota As New NotasYankees
            NewNota.lngIdDocumento = Nothing
            NewNota.lngImpresiones = 0
            NewNota.ValorEstado = GSTR_ESTADO_PENDIENTE
            NewNota.dtmDocumento = Date.Now
            NewNota.dtmEstado = Date.Now
            NewNota.strDescripcion = GSTR_TEXTO_CONSECUTIVO
            NewNota.strEstado = GSTR_ESTADO_PENDIENTE
            NewNota.strOpcion = "ING"
            SelectedNotasYankees = NewNota

            Editando = True
            MyBase.CambioItem("Editando")

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaNotasYankees
        CambioItem("cb")
        MyBase.Buscar()

    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.lngIDDocumento <> 0 Or Not IsNothing(cb.dtmDocumento) Then
                IsBusy = True
                ErrorForma = ""
                NotasYankeesAnterior = Nothing
                If Not IsNothing(dcProxy.NotasYankees) Then
                    dcProxy.NotasYankees.Clear()
                End If
                ListaNotasYankeesDetalle = Nothing
                dcProxy.Load(dcProxy.NotasYankeesConsultarQuery(cb.lngIDDocumento, cb.dtmDocumento, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaNotasYankees
                CambioItem("cb")
                FechaBusqueda = Now
            End If
        Catch ex As Exception
            IsBusy = False
        End Try

    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.NotasYankees.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "FiltroVM")
            End If

            'If FiltroVM.Length > 0 Then
            '    If Regex.IsMatch(FiltroVM, "^[a-z A-Z 0-9_-]*$") Then
            '        IsBusy = True
            '        dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "FiltroVM")
            '    Else
            '        IsBusy = False
            '        FiltroVM = String.Empty
            '        mostrarMensaje("¡La opción filtrar no se puede realizar, el filtro que ingreso posee caracteres NO válidos!", "Filtrar", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    End If
            'Else
            '    IsBusy = True
            '    dcProxy.Load(dcProxy.NotasYankeesFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNotasYankees, "FiltroVM")

            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", Me.ToString(), "Filtrar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub EditarRegistro()

        Try
            If SelectedNotasYankees Is Nothing Then
                MyBase.RetornarValorEdicionNavegacion()
                Exit Sub
            End If


            NotasYankeesAnterior = SelectedNotasYankees

            If _SelectedNotasYankees.ValorEstado = GSTR_ESTADO_PENDIENTE Then
                SelectedNotasYankees.strOpcion = "MOD"
                SelectedNotasYankees.Usuario = Program.Usuario
                Numero = SelectedNotasYankees.lngIdDocumento

                Editando = True
                MyBase.CambioItem("Editando")
            ElseIf _SelectedNotasYankees.ValorEstado = GSTR_ESTADO_IMPRESO Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque ya fue impreso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("El documento no se puede editar porque ya fue anulado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar de un registro", _
                                                         Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try

            SelectedNotasYankees = NotasYankeesAnterior


            Editando = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub BorrarRegistro()
        Try
            If Not IsNothing(SelectedNotasYankees) Then

                If SelectedNotasYankees.ValorEstado = GSTR_ESTADO_IMPRESO Then
                    If Not Await Tiene_Permiso() Then
                        mostrarMensaje("Señor usuario, usted no posee permisos para realizar esta operación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    dcProxy.NotasYankeesEliminar(SelectedNotasYankees.lngIdDocumento, SelectedNotasYankees.strNombreConsecutivo, Program.Usuario, Program.HashConexion, AddressOf TerminoEliminarNotasYankees, "")
                Else
                    mostrarMensaje("Para Eliminar/Anular Nota de Yankees es necesario que se encuentre en un estado impreso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                mostrarMensaje("Para Eliminar/Anular Nota de Yankees es necesario que existan registros", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function Tiene_Permiso() As Task(Of Boolean)
        Dim objRetorno As Boolean = True
        Try
            Dim objRet As LoadOperation(Of OYDUtilidades.ItemCombo)

            objProxy1.ItemCombos.Clear()

            objRet = Await objProxy1.Load(objProxy1.cargarCombosEspecificosQuery("Tesoreria_Notas", Program.Usuario, Program.HashConexion)).AsTask()

            If objRet.HasError Then
                If objRet.Error Is Nothing Then
                    A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "GenerarDocumentosTesoreria", Program.TituloSistema, Program.Maquina, objRet.Error)
                End If
                objRet.MarkErrorAsHandled()
                Return False
            Else
                Dim listConsecutivos = objProxy1.ItemCombos.Where(Function(y) y.Categoria = "ConsecutivosTesoreriaNotas" And y.Descripcion = GSTR_TEXTO_CONSECUTIVO).ToList
                If listConsecutivos.Count = 0 Then
                    objRetorno = False
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "Tiene_Permiso", Application.Current.ToString(), Program.Maquina, ex)
            objRetorno = False
        End Try
        Return objRetorno
    End Function

    Public Overrides Sub ActualizarRegistro()
        Try
            IsBusy = True
            Dim curCredito As Double = 0
            Dim curDebito As Double = 0
            Dim logpermitirThen = True
            Dim strRegistrosDetalle = String.Empty
            Dim lngIDDocumento = 0

            If Not IsNothing(SelectedNotasYankees) Then
                If SelectedNotasYankees.strOpcion = "ING" Then
                    lngIDDocumento = 0
                Else
                    lngIDDocumento = SelectedNotasYankees.lngIdDocumento
                End If
                If Not IsNothing(ListaNotasYankeesDetalle) Then
                    For Each x In ListaNotasYankeesDetalle.Where(Function(i) Not IsNothing(i.curValorCredito)).ToList
                        curCredito = curCredito + x.curValorCredito
                    Next
                    For Each x In ListaNotasYankeesDetalle.Where(Function(i) Not IsNothing(i.curValorDebito)).ToList
                        curDebito = curDebito + x.curValorDebito
                    Next
                    If Validaciones(curCredito, curDebito) Then
                        If ListaNotasYankeesDetalle.Count > 0 Then
                            For Each li In ListaNotasYankeesDetalle

                                If logpermitirThen Then
                                    strRegistrosDetalle = String.Format("%{0}%**%{1}%**%{2}%**%{3}%**%{4}%", li.strIdComitente, li.strDetalle, li.curValorCredito, li.curValorDebito, li.strIDCuentaContable)
                                    logpermitirThen = False
                                Else
                                    strRegistrosDetalle = String.Format("{0}|%{1}%**%{2}%**%{3}%**%{4}%**%{5}%", strRegistrosDetalle, li.strIdComitente, li.strDetalle, li.curValorCredito, li.curValorDebito, li.strIDCuentaContable)
                                End If
                            Next
                            logpermitirThen = False
                        End If
                        If ListaNotasYankeesDetalle.Count > 0 Then
                            dcProxy.NotasYankeesActualizar(lngIDDocumento, SelectedNotasYankees.strNombreConsecutivo, SelectedNotasYankees.dtmDocumento, SelectedNotasYankees.ValorEstado, SelectedNotasYankees.strOpcion, strRegistrosDetalle, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarNotasYankees, SelectedNotasYankees.strOpcion)
                        Else
                            mostrarMensaje("Debe ingresar el detalle del documento", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If

                    End If
                Else
                    IsBusy = False
                    mostrarMensaje("Debe ingresar el detalle del documento", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                End If

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

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Dim obListaDetalle = New List(Of NotasYankeesDetalle)
            If Not IsNothing(ListaNotasYankeesDetalle) Then
                obListaDetalle = ListaNotasYankeesDetalle
            End If
            ListaNotasYankeesDetalle = Nothing
            obListaDetalle.Add(New NotasYankeesDetalle)

            ListaNotasYankeesDetalle = obListaDetalle
            MyBase.CambioItem("ListaNotasYankeesDetalle")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al adicionar nuevo registro de Detalle", _
                                            Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub
    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Dim obListaDetalle = New List(Of NotasYankeesDetalle)
            If Not IsNothing(SelectedNotasYankeesDetalle) Then
                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(SelectedNotasYankeesDetalle, ListaNotasYankeesDetalle)

                obListaDetalle = ListaNotasYankeesDetalle
                obListaDetalle.Remove(SelectedNotasYankeesDetalle)
                ListaNotasYankeesDetalle = Nothing
                ListaNotasYankeesDetalle = obListaDetalle

                Program.PosicionarItemLista(SelectedNotasYankeesDetalle, ListaNotasYankeesDetalle, intRegistroPosicionar)

                MyBase.CambioItem("ListaNotasYankeesDetalle")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Borrar registro de Detalle", _
                                            Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    ''' <summary>
    ''' Metodo para realizar el proceso de validaciones
    ''' </summary>
    ''' <remarks>
    ''' metodo          : Validaciones()
    ''' Se encarga de    : de recorrer las diferentes caso para notificar proceso de grabado
    ''' Modificado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function Validaciones(pcurValorCredito As Double, pcurValorDebito As Double) As Boolean
        Try
            Dim logValidacion As Boolean = True
            Dim strMensajeValidacion As String = String.Empty


            If pcurValorCredito <> pcurValorDebito Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} Suma de Valores Crédito y Débito: El valor total de los créditos no es igual al de los debitos." & vbCrLf & _
                               "NC=$" + pcurValorCredito.ToString + "  ND=$" + pcurValorDebito.ToString, strMensajeValidacion, vbCrLf)
            End If
            If pcurValorCredito = 0 And pcurValorDebito = 0 Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} Valores en Cero: Los valores crédito y débito deben ser mayores a 0", strMensajeValidacion, vbCrLf)
            End If
            For Each x In ListaNotasYankeesDetalle
                If String.IsNullOrEmpty(x.strDetalle) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} Detalle: Debe ingresar el valor en el campo detalle para cada registro", strMensajeValidacion, vbCrLf)
                    Exit For
                End If
            Next
            For Each x In ListaNotasYankeesDetalle
                If String.IsNullOrEmpty(x.strIDCuentaContable) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} Cuenta Contable: Debe seleccionar un valor para la cuenta contable.", strMensajeValidacion, vbCrLf)
                    Exit For
                End If
            Next
            For Each x In ListaNotasYankeesDetalle
                If String.IsNullOrEmpty(x.strIdComitente) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} Comitente: Debe seleccionar un valor para el campo código que pertenece al comitente.", strMensajeValidacion, vbCrLf)
                    Exit For
                End If
            Next
            For Each x In ListaNotasYankeesDetalle
                If String.IsNullOrEmpty(x.strNombre) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} Nombre: El comitente Seleccionado debe tener un nombre", strMensajeValidacion, vbCrLf)
                    Exit For
                End If
            Next

            If SelectedNotasYankees.dtmDocumento > Now Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} Fecha Documento: el valor de la fecha debe ser menor o igual a la fecha actual", strMensajeValidacion, vbCrLf)
            End If
            If logValidacion = False Then
                IsBusy = False
                mostrarMensaje("Validaciones:" & vbCrLf & strMensajeValidacion, "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If

        Catch ex As Exception
            IsBusy = False
            Return False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en las Validaciones.", _
                                Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Function

    ''' <summary>
    ''' Metodo para imprimir reportes de los formularios de NOTAS YANKEES
    ''' </summary>
    ''' <remarks>JDO20130626</remarks>
    ''' 
    Public Sub ImprimirReporteYanKees(ByVal strNombreReporte As String)
        Try
            Dim strParametros As String = String.Empty
            Dim strReporte As String = String.Empty
            Dim strNroVentana As String = String.Empty

            If Not IsNothing(_SelectedNotasYankees) Then
                If _SelectedNotasYankees.ValorEstado = "P" Then
                    If Application.Current.Resources.Contains("A2VReporteImprimirNotaContableYankees") = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("El reporte para imprimir la nota contable yankees no está configurado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    strReporte = Application.Current.Resources("A2VReporteImprimirNotaContableYankees").ToString.Trim()
                    strParametros = "&plngNotaDesde=" & _SelectedNotasYankees.lngIdDocumento & "&plngNotaHasta=" & _SelectedNotasYankees.lngIdDocumento & "&pstrTipoConsecutivo=" & _SelectedNotasYankees.strNombreConsecutivo & "&pstrEstado=" & _SelectedNotasYankees.ValorEstado

                    MostrarReporte(strParametros, Me.ToString, strReporte)
                    If strNombreReporte = "imprimirNotaYankees" Then
                        System.Threading.Thread.Sleep(5000)
                        IsBusy = True
                        intContadorVerificacion = 0
                        'procedimiento para evaluar si ya se lanzo el reporte
                        dcProxy.Verificar_EstadoImpresion(_SelectedNotasYankees.lngIdDocumento, _SelectedNotasYankees.strNombreConsecutivo, "N", Program.Usuario, Program.HashConexion, AddressOf TeminoVericarEstadoImpresion, "verificar")
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Solo se pueden imprimir documentos pendientes", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir.", _
                                Me.ToString(), "ImprimirReporteYanKees", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Buscar los datos del comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : 
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty
        Try
            If (_mlogBuscarCliente) Then
                If Not Me.SelectedNotasYankeesDetalle Is Nothing Then
                    If Not strIdComitente.Equals(Me.SelectedNotasYankeesDetalle.strIdComitente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.SelectedNotasYankeesDetalle.strIdComitente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If
            If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                IsBusyComitente = True
                objProxy1.BuscadorClientes.Clear()
                objProxy1.Load(objProxy1.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub




#End Region

#Region "Buscador Generico"

    ''' <summary>
    ''' Buscar el centro de costos, cuenta contable y Nits seleccionado.
    ''' </summary>
    ''' <param name="pstrCentroCostos"></param>
    ''' <param name="pstrBusqueda"></param>
    ''' <remarks>SLB20130710</remarks>
    Friend Sub buscarGenerico(Optional ByVal pstrCentroCostos As String = "", Optional ByVal pstrBusqueda As String = "")
        Try
            objProxy1.BuscadorGenericos.Clear()
            objProxy1.Load(objProxy1.buscarItemEspecificoQuery(pstrBusqueda, pstrCentroCostos, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBuscadorGenerico, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método recibe del buscador generico.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130710</remarks>
    Private Sub TerminoTraerBuscadorGenerico(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "CuentasContables"
                        If lo.Entities.ToList.Count > 0 Then
                            _SelectedNotasYankeesDetalle.strIDCuentaContable = lo.Entities.First.IdItem
                        Else
                            'sw = 1
                            A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _SelectedNotasYankeesDetalle.strIDCuentaContable = Nothing
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos", _
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(), _
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "PropertyChanged"

    Private Sub _SelectedNotasYankeesDetalle_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _SelectedNotasYankeesDetalle.PropertyChanged
        If Not IsNothing(_SelectedNotasYankeesDetalle) Then

            If e.PropertyName = "curValorCredito" Then
                If _SelectedNotasYankeesDetalle.curValorCredito > 0 Then
                    _SelectedNotasYankeesDetalle.curValorDebito = 0
                End If
            End If
            If e.PropertyName = "curValorDebito" Then
                If _SelectedNotasYankeesDetalle.curValorDebito > 0 Then
                    _SelectedNotasYankeesDetalle.curValorCredito = 0
                End If
            End If
            If e.PropertyName = "strIdComitente" Then
                If Not String.IsNullOrEmpty(_SelectedNotasYankeesDetalle.strIdComitente) And _mlogBuscarCliente Then
                    buscarComitente(_SelectedNotasYankeesDetalle.strIdComitente)
                End If
            End If

            'SLB20130710 Se adiciona la busqueda de la cuenta contable desde el control 
            If e.PropertyName = "strIDCuentaContable" And _mlogBuscarCuentaContable Then
                If Not String.IsNullOrEmpty(_SelectedNotasYankeesDetalle.strIDCuentaContable) Then
                    buscarGenerico(_SelectedNotasYankeesDetalle.strIDCuentaContable, "CuentasContables")
                End If
            End If

        End If
    End Sub


#End Region


End Class


Public Class CamposBusquedaNotasYankees

    Implements INotifyPropertyChanged

    Private _lngIDDocumento As Integer
    Public Property lngIDDocumento As Integer
        Get
            Return _lngIDDocumento
        End Get
        Set(ByVal value As Integer)
            _lngIDDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDDocumento"))
        End Set
    End Property

    Private _dtmDocumento As DateTime? = Nothing
    <Display(Name:="Fecha Documento")> _
    Public Property dtmDocumento As DateTime?
        Get
            Return _dtmDocumento
        End Get
        Set(ByVal value As DateTime?)
            _dtmDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmDocumento"))
        End Set
    End Property

    Private _strIdComitente As String
    <Display(Name:="Comitente")> _
    Public Property strIdComitente As String
        Get
            Return _strIdComitente
        End Get
        Set(ByVal value As String)
            _strIdComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIdComitente"))
        End Set
    End Property

    Private _strNombre As String
    <Display(Name:="Nombre")> _
    Public Property strNombre As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
