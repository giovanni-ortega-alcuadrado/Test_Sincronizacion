Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web

Public Class DeshacerCierrePortafoliosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"

    Public ViewDeshacerCierrePortafolios As DeshacerCierrePortafoliosView = Nothing
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:    Creacion.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          24 de Agosto/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 24 de Agosto/2014 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarValoresPorDefecto()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            MyBase.CambioItem("dtmFechaProceso")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            MyBase.CambioItem("lngIDComitente")
        End Set
    End Property

    Private Async Sub _lngIDComitente_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        If e.PropertyName = "lngIDComitente" Then
            If String.IsNullOrEmpty(lngIDComitente) Then
                strNombreComitente = String.Empty
            Else
                Await ConsultarDatosPortafolio()
            End If
        End If
    End Sub

    Private _strNombreComitente As String = String.Empty
    Public Property strNombreComitente() As String
        Get
            Return _strNombreComitente
        End Get
        Set(ByVal value As String)
            _strNombreComitente = value
            MyBase.CambioItem("strNombreComitente")
        End Set
    End Property

    Private _Chequeado As Boolean = False
    Public Property Chequeado() As Boolean
        Get
            Return _Chequeado
        End Get
        Set(ByVal value As Boolean)
            _Chequeado = value
            MyBase.CambioItem("Chequeado")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    ''' <summary>
    ''' Lista de Deshacer Cierre Portafolios que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of DeshacerCierrePortafolios)
    Public Property ListaEncabezado() As EntitySet(Of DeshacerCierrePortafolios)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of DeshacerCierrePortafolios))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property


    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Deshacer Cierre Portafolios para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Creado por:     Germán Arbey González Osorio
    ''' Descripción:    Propiedad para almacenar el tipo de portafolio (compañía) seleccionado
    ''' Fecha:          Agosto 16/2016
    ''' ID del cambio:  GAG20160816
    ''' </summary>
    ''' <remarks></remarks>
    Private _strTipoPortafolio As String = String.Empty
    Public Property strTipoPortafolio() As String
        Get
            Return _strTipoPortafolio
        End Get
        Set(ByVal value As String)
            _strTipoPortafolio = value
            MyBase.CambioItem("strTipoPortafolio")
        End Set
    End Property

    ''' <summary>
    ''' Creado por:     Germán Arbey González Osorio
    ''' Descripción:    Propiedad para cargar la lista del tópico TIPOCOMPANIA y agregarle el item 'Todos' sin modificar datos, sino únicamente en esta pantalla
    ''' Fecha:          Agosto 16/2016
    ''' ID del cambio:  GAG20160816
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaTipoCompania
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaTipoCompania = value
            MyBase.CambioItem("ListaTipoCompania")
        End Set
    End Property
#End Region


#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ActualizarRegistro
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: 11
    ''' Descripción:       Creacion.
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de Agosto/2014
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de Agosto/2014 - Resultado Ok 
    ''' </history>
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------

            'Valida el código OyD
            If String.IsNullOrEmpty(lngIDComitente) And Not Chequeado Then
                strMsg = String.Format("{0}{1} + El código OyD es un campo requerido.", strMsg, vbCrLf)
            End If

            'Valida la fecha de proceso
            If IsNothing(dtmFechaProceso) Then
                strMsg = String.Format("{0}{1} + La fecha de proceso es un campo requerido.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Sub ConsultarValoresPorDefecto()
        'Dim objRet As LoadOperation(Of DeshacerCierrePortafolios)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            dtmFechaProceso = Date.Now()
            lngIDComitente = String.Empty
            CargaListas()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarValoresPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta los valores por defecto para el combo Tipo portafolio
    ''' </summary
    Private Sub CargaListas()
        If Application.Current.Resources.Contains(Program.NombreListaCombos) Then
            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOCOMPANIA") Then
                If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA")) Then
                    Dim objListaTipoCompania As List(Of OYDUtilidades.ItemCombo)
                    objListaTipoCompania = New List(Of OYDUtilidades.ItemCombo)(CType(Application.Current.Resources(Program.NombreListaCombos),
                                 Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOCOMPANIA"))
                    objListaTipoCompania.Add(New ItemCombo() With {
                                                                    .ID = "T",
                                                                    .Retorno = "T",
                                                                    .Descripcion = "Todos",
                                                                    .Categoria = "TIPOCOMPANIA"
                                                                })
                    Me.ListaTipoCompania = objListaTipoCompania
                    Me.strTipoPortafolio = objListaTipoCompania.LastOrDefault().ID
                End If
            End If

        End If
    End Sub

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <history>
    ''' ID caso de prueba: 11
    ''' Descripción:       Función que se activa al presionar el botón "Aceptar".
    ''' Responsable:       Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:             24 de Agosto/2014
    ''' Pruebas CB:        Jorge Peña (Alcuadrado S.A.) - 24 de Agosto/2014 - Resultado Ok 
    ''' </history>
    Public Async Function DeshacerCierrePortafolios(Confirmado As Boolean) As Task
        Try
            Dim objRet As LoadOperation(Of DeshacerCierrePortafolios)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            If Chequeado = True And Confirmado = False Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("Ha seleccionado todos los clientes para deshacer el cierre en la fecha " + dtmFechaProceso.Value.ToString("dd/MM/yyyy") + ", ¿Desea continuar?", Program.TituloSistema, ValoresUserState.Actualizar.ToString, AddressOf ConfirmarDeshacerCierrePortafolios, False)
                Exit Function
            End If

            If ValidarDatos() Then

                IsBusy = True
                mdcProxy.DeshacerCierrePortafolios.Clear()
                objRet = Await mdcProxy.Load(mdcProxy.DeshacerCierrePortafoliosQuery(dtmFechaProceso, lngIDComitente, Chequeado, strTipoPortafolio, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al deshacer el cierre de portafolios ", Me.ToString(), "DeshacerCierrePortafolios", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaEncabezado = mdcProxy.DeshacerCierrePortafolios
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al deshacer el cierre de portafolios ", Me.ToString(), "DeshacerCierrePortafolios", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Private Sub ConfirmarDeshacerCierrePortafolios(ByVal sender As Object, ByVal e As EventArgs)
        If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
            DeshacerCierrePortafolios(True)
        End If
    End Sub

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio() As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            'Dim mdcProxy As CalculosFinancierosDomainContext

            mdcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarDatosPortafolioSyncQuery(lngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        strNombreComitente = objRet.Entities.First.strNombre
                    Else
                        strNombreComitente = Nothing
                    End If
                End If
            Else
                strNombreComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ConsultarDatosPortafolio. ", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region


End Class

