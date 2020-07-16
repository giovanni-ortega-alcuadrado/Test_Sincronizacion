Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Web

''' <summary>
''' ViewModel para la pantalla Unificar Formatos perteneciente al proyecto de Cálculos Financieros.
''' </summary>
''' <history>
''' Creado por       : Jeison Ramírez Pino  - IOsoft S.A.S.
''' Descripción      : Creacion.
''' Fecha            : Octubre 09/2016
''' Pruebas CB       : Jeison Ramírez Pino - Octubre 09/2016 - Resultado Ok 
''' </history>

Public Class UnificarFormatosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables"

    ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxy As CalculosFinancierosDomainContext

#End Region

#Region "Propiedades"

    Private _UnificarFormatosSelected As New CamposUnificarFormatos
    Public Property UnificarFormatosSelected() As CamposUnificarFormatos
        Get
            Return _UnificarFormatosSelected
        End Get
        Set(ByVal value As CamposUnificarFormatos)
            _UnificarFormatosSelected = value
        End Set
    End Property

#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Inicializa el objeto y activa el control que bloquea la pantalla mientras se está procesando
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán González  - Alcuadrado S.A.
    ''' Descripción      : Creacion.
    ''' Fecha            : Junio 17/2014
    ''' Pruebas CB       : Germán González - Junio 17/2014 - Resultado Ok 
    ''' </history>
    Public Sub New()
        IsBusy = True
    End Sub

    ''' <summary>
    ''' Carga inicial de datos
    ''' </summary>
    ''' <returns>Retorna un objeto de tipo boolean</returns>
    ''' <history>
    ''' Creado por       : Jeison Ramírez Pino  - IOsoft S.A.S.
    ''' Descripción      : Creacion.
    ''' Fecha            : Octubre 10/2016
    ''' Pruebas CB       : Jeison Ramírez Pino - Octubre 10/2016 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                    mdcProxy = New CalculosFinancierosDomainContext()
                Else
                    mdcProxy = New CalculosFinancierosDomainContext(New System.Uri((Program.RutaServicioCalculosFinancieros)))
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Métodos privados- REQUERIDOS"

    ''' <summary>
    ''' Establece los valores por defecto en la clase que tiene los campos de los Formatos
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jeison Ramírez Pino - IOsoft S.A.S.
    ''' Descripción      : Creacion.
    ''' Fecha            : Octubre 11/2016
    ''' Pruebas CB       : Jeison Ramírez Pino - Octubre 11/2016 - Resultado Ok 
    ''' </history>
    Private Sub Preparar()
        Try

            Dim objCB As New CamposUnificarFormatos

            objCB.strFormatoPrincipal = String.Empty
            objCB.strFormatoProductos = String.Empty
            UnificarFormatosSelected = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos publicos - REQUERIDOS"
    ''' <summary>
    ''' Metodo del boton Unificar.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jeison Ramírez Pino  - IOsoft S.A.S.
    ''' Descripción      : Creacion.
    ''' Fecha            : Octubre 10/2016
    ''' Pruebas CB       : Jeison Ramírez Pino - Octubre 10/2016 - Resultado Ok 
    ''' </history>
    Public Sub Unificar()
        Try
            If String.IsNullOrEmpty(UnificarFormatosSelected.strFormatoPrincipal) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el formato principal.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(UnificarFormatosSelected.strFormatoProductos) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el formato producto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            mdcProxy.RetornoInformacionArchivos.Clear()
            mdcProxy.Load(mdcProxy.UnificarFormatos_ImportarExportarQuery(UnificarFormatosSelected.strFormatoPrincipal, UnificarFormatosSelected.strFormatoProductos, "ImpUnificarFormatos", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoTraerUnificarFormatos, "Consulta")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Unificar los formatos", Me.ToString(), "Unificar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoTraerUnificarFormatos(ByVal lo As LoadOperation(Of CFCalculosFinancieros.RetornoInformacionArchivo))
        Try
            If Not lo.HasError Then
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim objListaRespuesta As List(Of CFCalculosFinancieros.RetornoInformacionArchivo)
                Dim strUrlArchivo As String = String.Empty

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta
                        objListaMensajes.Add(li.strInformacionGenerar)

                        If String.IsNullOrEmpty(strUrlArchivo) And Not String.IsNullOrEmpty(li.URLArchivo) Then
                            strUrlArchivo = li.URLArchivo
                        End If
                    Next
                End If

                objViewImportarArchivo.ListaMensajes = objListaMensajes

                objViewImportarArchivo.Title = "Unificar formatos"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()

                If Not String.IsNullOrEmpty(strUrlArchivo) Then
                    Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

                    Program.VisorArchivosWeb_DescargarURL(strUrlArchivo)
                End If

                UnificarFormatosSelected.strFormatoPrincipal = String.Empty
                UnificarFormatosSelected.strFormatoProductos = String.Empty

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema", Me.ToString(), "TerminoTraerUnificarFormatos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema.", Me.ToString(), "TerminoTraerUnificarFormatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub
#End Region

#Region "Clases adicionales para captura de datos"

    '    ''' <summary>
    '    ''' Clase de administrar los campos de Unificar Formatos.
    '    ''' </summary>
    '    ''' <history>
    '    ''' Creado por       : Jeison Ramírez Pino - IOsoft S.A.S.
    '    ''' Descripción      : Creacion.
    '    ''' Fecha            : Octubre 11/2016
    '    ''' Pruebas CB       : Jeison Ramírez Pino - Octubre 11/2016 - Resultado Ok 
    '    ''' </history>
    Public Class CamposUnificarFormatos
        Implements INotifyPropertyChanged

        Private _strFormatoPrincipal As String
        Public Property strFormatoPrincipal() As String
            Get
                Return _strFormatoPrincipal
            End Get
            Set(ByVal value As String)
                _strFormatoPrincipal = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strFormatoPrincipal"))
            End Set
        End Property

        Private _strFormatoProductos As String
        Public Property strFormatoProductos() As String
            Get
                Return _strFormatoProductos
            End Get
            Set(ByVal value As String)
                _strFormatoProductos = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strFormatoProductos"))
            End Set
        End Property

        Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    End Class

#End Region

End Class
