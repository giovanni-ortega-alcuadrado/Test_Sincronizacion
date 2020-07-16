Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web

Public Class ModificarOperacionCumplidaViewModel
    Inherits A2ControlMenu.A2ViewModel

    ''' <summary>
    ''' ViewModel para la pantalla Modificar Operacion Cumplida.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 22/2014
    ''' Pruebas CB       : Germán Arbey González Osorio - Abril 22/2014 - Resultado OK
    ''' </history>

#Region "Constantes"

    ' Las constantes y el enumerador se utilizan para realizar la exportación de los datos
    Private Const STR_CARPETA As String = "FORMATOSGENERADOS"
    Private Const STR_NOMBREARCHIVO As String = "OperacionesCumplidas"
    Private Const STR_PARAMETROS_EXPORTAR As String = "[USUARIO]=[[USUARIO]]"
    Private Enum PARAMETROSEXPORTAR
        USUARIO
    End Enum

#End Region

#Region "Variables - REQUERIDO"

    '------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CodificacionContableDomainContext
    Private mdcProxyUtilidades As UtilidadesDomainContext
    Public ViewModificarOperacionCumplida As ModificarOperacionCumplidaView = Nothing

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de operaciones de la entidad en este caso OperacionesCumplidas
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private _ListaOperaciones As List(Of CFCodificacionContable.OperacionesCumplidas)
    Public Property ListaOperaciones() As List(Of CFCodificacionContable.OperacionesCumplidas)
        Get
            Return _ListaOperaciones
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.OperacionesCumplidas))
            _ListaOperaciones = value
            MyBase.CambioItem("ListaOperaciones")
            MyBase.CambioItem("ListaOperacionesPaginadas")
        End Set
    End Property

    ''' <summary>
    ''' Pagina la lista operaciones. Se presenta en el grid de las operaciones 
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public ReadOnly Property ListaOperacionesPaginadas() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOperaciones) Then
                Dim view = New PagedCollectionView(_ListaOperaciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de las operaciones está seleccionado
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private WithEvents _OperacionSeleccionada As CFCodificacionContable.OperacionesCumplidas
    Public Property OperacionSeleccionada() As CFCodificacionContable.OperacionesCumplidas
        Get
            Return _OperacionSeleccionada
        End Get
        Set(ByVal value As CFCodificacionContable.OperacionesCumplidas)
            _OperacionSeleccionada = value
            MyBase.CambioItem("OperacionSeleccionada")
        End Set
    End Property

    ''' <summary>
    ''' Indica la extensión del archivo en que se exportarán los datos
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private _ExtensionArchivo As String = String.Empty
    Public Property ExtensionArchivo() As String
        Get
            Return _ExtensionArchivo
        End Get
        Set(ByVal value As String)
            _ExtensionArchivo = value
            MyBase.CambioItem("ExtensionArchivo")
        End Set
    End Property

#End Region

#Region "Inicialización - REQUERIDO"

    Public Sub New()
        Try
            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la construcción del objeto de negocio.", Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns>Retorna un booleano indicando si el proceso es exitoso</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                logResultado = Await ConsultarOperacionesCumplidas()
            End If
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos propios de la clase"

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de actualizar
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Public Sub SeleccionarTodasOperaciones(blnIsChequed As Boolean)
        For Each Operacion In ListaOperaciones
            Operacion.logActualizar = blnIsChequed
        Next
        Me.CambioItem("ListaOperaciones")
    End Sub

#End Region

#Region "Métodos sincrónicos"

    ''' <summary>
    ''' Realiza la consulta de los registros correspondientes a los cobros de utilidades en la ventana modal (childWindow)
    ''' </summary>
    ''' <returns>Retorna una variable booleana que indica si el proceso no genero errores</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 04/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 04/2014 - Resultado OK
    ''' </history>
    Private Async Function ConsultarOperacionesCumplidas() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.OperacionesCumplidas)

        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.OperacionesCumplidas.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.OperacionesCumplidasConsultarQuery(pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para obtener los registros de las operaciones cumplidas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de las operaciones cumplidas.", Me.ToString(), "ConsultarOperacionesCumplidas", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaOperaciones = mdcProxy.OperacionesCumplidas.ToList
                End If
            End If

            logResultado = True

        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las Utilidades de Custodias.", Me.ToString(), "ConsultarUtilidadesCustodias", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return logResultado

    End Function

    ''' <summary>
    ''' Método que se ejecuta al presionar el botón "Exportar"
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Async Sub ExportarOperaciones()

        Try
            IsBusy = True

            If ValidarExportarArchivo() Then

                Dim strParametros As String = STR_PARAMETROS_EXPORTAR
                Dim objRet As LoadOperation(Of GenerarArchivosPlanos)

                mdcProxyUtilidades = inicializarProxyUtilidadesOYD()

                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

                objRet = Await mdcProxyUtilidades.Load(mdcProxyUtilidades.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_NOMBREARCHIVO, strParametros, STR_NOMBREARCHIVO, String.Empty, _ExtensionArchivo, Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            Mensajes.mostrarMensaje("Se generó un problema al ejecutar la exportación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la exportación del archivo.", Me.ToString(), "ExportarArchivo", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                        IsBusy = False
                        objRet.MarkErrorAsHandled()
                    Else
                        If objRet.Entities.Count > 0 Then
                            Dim objResultado = objRet.Entities.First

                            If objResultado.Exitoso Then
                                Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano)
                            Else
                                Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la exportación del archivo.", "ExportarArchivo", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        Finally
            IsBusy = False
        End Try

    End Sub

    ''' <summary>
    ''' Valida que el usuario seleccione una extensión antes de exportar el archivo
    ''' </summary>
    ''' <returns>Retorna un booleano que indica si el usuario seleciono una extensión</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Function ValidarExportarArchivo() As Boolean

        Try
            Dim objResultado As Boolean = True
            Dim strMensaje As String = String.Empty

            If String.IsNullOrEmpty(ExtensionArchivo) Then
                strMensaje = strMensaje & vbCrLf & "  + Debe seleccionar la extensión"
            End If

            If Not String.IsNullOrEmpty(strMensaje) Then
                strMensaje = "No es posible realizar el proceso de exportación porque tiene las siguientes inconsistencias: " & vbCrLf & strMensaje
                Mensajes.mostrarMensajeResultadoAsincronico(strMensaje, Program.TituloSistema, AddressOf TerminoMostrarMensajeUsuario, "ValidarExportarArchivo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objResultado = False
                IsBusy = False
            End If

            Return objResultado
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el archivo.", "ValidarExportarArchivo", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Se ejecuta al finalizar el mensaje de validación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Sub TerminoMostrarMensajeUsuario(ByVal sender As Object, ByVal e As EventArgs)
        Try
            'Dim objResultado = CType(sender, A2Utilidades.wcpMensajes)

            'If Not String.IsNullOrEmpty(strControlAposicionar) Then
            '    Select Case objResultado.CodigoLlamado.ToLower
            '        Case "exportar"
            '    End Select
            'End If

        Catch ex As Exception
            IsBusy = False
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del mensaje usuario.", Me.ToString(), "TerminoMostrarMensajeUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Reemplaza la cadena del parámetro de consulta para realizar la exportación
    ''' </summary>
    ''' <param name="pintTipoCampo">Parametro tipo PARAMETROSEXPORTAR </param>
    ''' <returns>Retorna la cadena modificada con el valor requerido</returns>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

    ''' <summary>
    ''' Método que se ejecuta al presionar el botón "Actualizar"
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Public Sub ActualizarOperaciones()

        Mensajes.mostrarMensajePregunta("¿Esta seguro de actualizar los registros?", Program.TituloSistema, ValoresUserState.Consultar.ToString(), AddressOf ActualizarRegistrosConfirmado)

    End Sub

    ''' <summary>
    ''' Se ejecuta si el usuario confirma la actualización de los datos en la tabla "tblLiquidacion"
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Creación.
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Async Sub ActualizarRegistrosConfirmado(ByVal sender As Object, ByVal e As EventArgs)

        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                Dim Resultado As Boolean = False
                Dim xmlDetalle As String = String.Empty
                Dim xmlCompleto As String = String.Empty

                If mdcProxy Is Nothing Then
                    mdcProxy = inicializarProxyCodificacionContable()
                End If

                If Not IsNothing(ListaOperaciones) Then

                    xmlCompleto = "<ActualizarOperacionesCumplidas>"

                    For Each objeto In (From c In ListaOperaciones)
                        If (objeto.logActualizar) Then

                            Resultado = True
                            xmlDetalle = "<Registro lngID=""" & objeto.lngID &
                                         """ lngParcial=""" & objeto.lngParcial &
                                         """ dtmLiquidacion=""" & objeto.dtmLiquidacion &
                                         """ strTipo=""" & objeto.strTipo &
                                         """ strClaseOrden=""" & objeto.strClaseOrden &
                                         """ ></Registro>"
                            xmlCompleto = xmlCompleto & xmlDetalle

                        End If

                    Next

                    xmlCompleto = xmlCompleto & "</ActualizarOperacionesCumplidas>"

                End If

                If Resultado Then

                    Dim objRet As InvokeOperation(Of Integer)

                    objRet = Await mdcProxy.OperacionesCumplidasActualizarSync(pxmlOperacionesCumplidas:=xmlCompleto, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion).AsTask()

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                Mensajes.mostrarMensaje("Se generó un problema al ejecutar la actualización de las operaciones cumplidas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la actualización de las operaciones cumplidas.", Me.ToString(), "ActualizarOperacionesCumplidas", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If
                            objRet.MarkErrorAsHandled()
                        Else
                            Await ConsultarOperacionesCumplidas()
                            Mensajes.mostrarMensaje("A finalizado el proceso de actualización con exito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                Else
                    Mensajes.mostrarMensaje("No se ha seleccionado ningún registro para actualizar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If

        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar las operaciones cumplidas.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

    End Sub

#End Region

End Class
