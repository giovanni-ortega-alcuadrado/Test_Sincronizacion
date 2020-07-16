Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data

Imports A2Utilidades.Mensajes

Public Class ImportarRecibosCajaRecaudosViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim dcArchivosProxy As ImportacionesDomainContext
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Private strArchivoBorrar As String


    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcArchivosProxy = New ImportacionesDomainContext
                dcProxyUtilidades = New UtilidadesDomainContext
            Else
                dcArchivosProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(dcArchivosProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)


            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_IMP_RC_RECAUDO_RESPUESTA, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                ConsultarParametros()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), " .New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Propiedades"

    Public _ListaArchivosGuardados As EntitySet(Of OyDImportaciones.Archivo)
    Public Property ListaArchivosGuardados As EntitySet(Of OyDImportaciones.Archivo)
        Get
            Return _ListaArchivosGuardados
        End Get
        Set(ByVal value As EntitySet(Of OyDImportaciones.Archivo))
            _ListaArchivosGuardados = value
            CambioItem("ListaArchivosGuardados")
            CambioItem("ListaArchivosGuardadosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaArchivosGuardadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(ListaArchivosGuardados) Then
                Dim view = New PagedCollectionView(ListaArchivosGuardados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _intCodigoBancoPorDefecto As System.Nullable(Of Integer)
    Public Property intCodigoBancoPorDefecto As System.Nullable(Of Integer)
        Get
            Return _intCodigoBancoPorDefecto
        End Get
        Set(value As System.Nullable(Of Integer))
            _intCodigoBancoPorDefecto = value
            MyBase.CambioItem("intCodigoBancoPorDefecto")
        End Set
    End Property

    Private _intCodigoBanco As System.Nullable(Of Integer)
    Public Property intCodigoBanco As System.Nullable(Of Integer)
        Get
            Return _intCodigoBanco
        End Get
        Set(value As System.Nullable(Of Integer))
            _intCodigoBanco = value
            MyBase.CambioItem("intCodigoBanco")
        End Set
    End Property

    Private _strNombreBanco As String
    Public Property strNombreBanco As String
        Get
            Return _strNombreBanco
        End Get
        Set(value As String)
            _strNombreBanco = value
            MyBase.CambioItem("strNombreBanco")
        End Set
    End Property

    Private _strNombrearchivo As String
    Public Property strNombrearchivo As String
        Get
            Return _strNombrearchivo
        End Get
        Set(value As String)
            _strNombrearchivo = value
            MyBase.CambioItem("strNombrearchivo")
        End Set
    End Property

#End Region

#Region "Métodos"

    Friend Sub BuscarBancos(Optional ByVal plngIdBanco As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Try
            dcProxyUtilidades.BuscadorGenericos.Clear()
            dcProxyUtilidades.Load(dcProxyUtilidades.buscarItemEspecificoQuery("cuentasbancarias", plngIdBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasBancarias, "cuentasbancarias")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los bancos", Me.ToString(), "buscarBancos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerCuentasBancarias(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "cuentasbancarias"
                        If lo.Entities.ToList.Count > 0 Then
                            If lo.Entities.First.InfoAdicional02.Equals("1") Then
                                intCodigoBanco = lo.Entities.First.IdItem
                                strNombreBanco = lo.Entities.First.Nombre
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("El banco se encuentra inactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                intCodigoBanco = Nothing
                                strNombreBanco = String.Empty
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("El banco ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            intCodigoBanco = Nothing
                            strNombreBanco = String.Empty
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos",
                                             Me.ToString(), "TerminoTraerBanco", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el banco", Me.ToString(),
                                                             "TerminoTraerCuentasBancarias", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub


    Public Sub CargarArchivo()
        Try

            If IsNothing(strNombrearchivo) Then
                A2Utilidades.Mensajes.mostrarMensaje("El archivo es requerido para poder realizar el cargue de RC", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(intCodigoBanco) Then
                A2Utilidades.Mensajes.mostrarMensaje("El banco es requerido ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            If Not IsNothing(dcArchivosProxy.RespuestaArchivoImportacions) Then
                dcArchivosProxy.RespuestaArchivoImportacions.Clear()
            End If
            dcArchivosProxy.Load(dcArchivosProxy.LeerArchivoRCQuery(strNombrearchivo, CSTR_NOMBREPROCESO_IMP_RC_RECAUDO, CSTR_NOMBREPROCESO_IMP_RC_RECAUDO_RESPUESTA, Program.Usuario, intCodigoBanco, Program.HashConexion), AddressOf TerminoLeerRC, Nothing)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.",
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoLeerRC(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try

            If lo.HasError = False Then

                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, objListaRespuesta.First.Mensaje.ToString, Me.ToString(), "TerminoGenerarComprobantes", Application.Current.ToString(), Program.Maquina, lo.Error)
                    Else
                        dcArchivosProxy.Archivos.Clear()
                        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_IMP_RC_RECAUDO_RESPUESTA, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se obtuvieron registros para procesar", Me.ToString(), "TerminoLeerRC", Application.Current.ToString(), Program.Maquina, lo.Error)
                End If
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()

            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)

        End Try
    End Sub


    Private Sub TerminoTraerArchivos(ByVal lo As LoadOperation(Of OyDImportaciones.Archivo))
        Try
            If Not lo.HasError Then
                ListaArchivosGuardados = dcArchivosProxy.Archivos
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos",
                                                 Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception

            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos",
                                                             Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Public Sub BorrarArchivo(ByVal archivo As OyDImportaciones.Archivo)
        If Not IsNothing(archivo) Then
            strArchivoBorrar = archivo.Nombre
            mostrarMensajePregunta("¿Está seguro de borrar este archivo?",
                                          Program.TituloSistema,
                                          "BORRAR",
                                          AddressOf TerminoPreguntaBorrar, False)
        End If
    End Sub

    Private Sub TerminoPreguntaBorrar(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            dcArchivosProxy.BorrarArchivo(strArchivoBorrar, CSTR_NOMBREPROCESO_IMP_RC_RECAUDO_RESPUESTA, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
        End If
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        If Not IsNothing(e.Error) Then
            MessageBox.Show(e.Error.Message)
            e.MarkErrorAsHandled()
        End If
        dcArchivosProxy.Archivos.Clear()
        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_IMP_RC_RECAUDO_RESPUESTA, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
    End Sub


    Public Sub ConsultarParametros()
        Try
            dcProxyUtilidades.Verificaparametro("TESORERIA_RC_CARGA_BANCO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "TESORERIA_RC_CARGA_BANCO")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros de Tesorería",
                                 Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState
                Case "TESORERIA_RC_CARGA_BANCO"
                    intCodigoBancoPorDefecto = lo.Value.ToString
                    BuscarBancos(intCodigoBancoPorDefecto, "consultarbancos")
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros",
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

End Class
