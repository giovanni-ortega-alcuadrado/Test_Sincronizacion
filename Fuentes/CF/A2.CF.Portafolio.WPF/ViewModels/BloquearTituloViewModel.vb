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
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports A2Utilidades.Mensajes

Public Class BloquearTituloViewModel

    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged
    Dim dcProxy As PortafolioDomainContext
    Private objProxy As UtilidadesDomainContext
    Dim BuscarTitulo As String 'B Indica que busca los Titulos Que Actualmente estan Bloqueados y L Busca los Titulos que Estan Libres y que podrian ser bloqueados
    Public _mlogBuscarCliente As Boolean = True
    Public _mlogBuscarEspecie As Boolean = True
    Public _mlogBuscarISINFungible As Boolean = True
    Public strISINFungible As String = ""
    Public strEspecie1 As String = ""

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New PortafolioDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        IsBusy = True
        objProxy.Load(objProxy.listaVerificaparametroQuery("", "Custodias", Program.Usuario, Program.HashConexion), AddressOf Terminotraerparametrolista, Nothing) 'SV20150305
        FechaBusqueda = Now.Date
        NombreColumna = "Cantidad a Bloquear"
        TextoGrid = "Títulos Encontrados"
        HabilitadoModificar = False
        HabilitarBuscEspe = False
        logActivo = True
        IsBusy = False

    End Sub


#Region "Metodos"

    Sub BuscarCustadias()
        IsBusy = True
        SeleccionarTodos = False
        strMotivoBloqueo = Nothing
        dblCantidadTotal = 0

        If logActivo Then
            BuscarTitulo = "L"
            NombreColumna = "Cantidad a Bloquear"
        Else
            BuscarTitulo = "B"
            NombreColumna = "Cantidad a Liberar"
        End If
        Try
            dcProxy.CustodiasClientes.Clear()
            dcProxy.Load(dcProxy.ConsultarCustodiasClienteQuery(CamposBusquedaSelected.CodigoCliente.PadLeft(17), CamposBusquedaSelected.Nemotecnico, BuscarTitulo, CamposBusquedaSelected.IsinFungible, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCustodiasCliente, "Consultar")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar las custodias asociadas al cliente",
                Me.ToString(), "BloquearTitulosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se realizan ajustes para validar que se cambie el estado a por lo menos uno de los títulos. 
    ''' Fecha            : Abril 10/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 10/2013 - Resultado Ok 
    ''' </history>
    Sub CambiarEstadoCustodias()
        Dim HayCambios As Boolean = False
        Dim intTotal As Integer = 0
        intTotal = Aggregate tot In _ListaCustodiasCliente Where tot.ObjParaBloquear.Equals(True) Into Count(tot.ObjParaBloquear)

        If intTotal = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje(String.Format("Debe seleccionar al menos un registro", vbCrLf), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If

        For Each led In ListaCustodiasCliente.Where(Function(I) I.ObjParaBloquear = True).ToList()
            ' Se valida 
            led.pdtmRecibo = FechaBusqueda
            If led.ObjParaBloquear Then
                'Si el motivo de bloqueo es NO BLOQUEADO y el RadioButton es libre para bloquear muestra el mensaje.
                If (IsNothing(led.strMotivoBloqueo) Or led.strMotivoBloqueo = "X") And logActivo_Consultado = True Then
                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("Debe ingresar el motivo del bloqueo {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If led.CantidadBloquear > led.Cantidad Then
                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cantidad a Bloquear o Liberar no puede ser superior a la de la Custodia {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If led.CantidadBloquear < 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cantidad Bloquear o Liberar no puede ser negativa {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If led.dtmRecibo > FechaBusqueda Then
                    A2Utilidades.Mensajes.mostrarMensaje(String.Format("La fecha no puede ser menor a la fecha de recibo {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                If Not IsNothing(led.dtmFechaCierrePortafolio) Then
                    If FechaBusqueda <= led.dtmFechaCierrePortafolio Then
                        A2Utilidades.Mensajes.mostrarMensaje(String.Format("La fecha de bloqueo/desbloqueo no puede ser inferior a la fecha de portafolio {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                HayCambios = True
            End If
            'If (Not led.strMotivoBloqueo = "X") And (led.CantidadBloquear = 0) Then
            '    A2Utilidades.Mensajes.mostrarMensaje(String.Format("Debe ingresar la cantidad a Bloquear o Liberar {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            'If led.CantidadBloquear < 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje(String.Format("La cantidad Bloquear o Liberar no puede ser negativa {0}Custodia: {1}", vbCrLf, led.Custodia), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            'Dim fechaInicioPago = led.InicioPago.Date
            'Dim fechaInicioRepetida = From ld In ListaEspeciesDividendos Where ld.InicioPago.Date = fechaInicioPago
            '                          Select ld
        Next
        If HayCambios Then
            'C1.Silverlight.C1MessageBox.Show("¿Desea cambiar el estado de las custodias?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf ResultadoConfirmar)
            mostrarMensajePregunta("¿Desea cambiar el estado de las custodias?",
                                   Program.TituloSistema,
                                   "ESTADOCUSTODIA",
                                   AddressOf ResultadoConfirmar, False)
        Else
            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la cantidad a bloquear o liberar de al menos una custodia y completar los datos requeridos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub

    Public Async Sub ConfirmarCambiarEstado()
        Try
            Dim objRet As InvokeOperation(Of System.Nullable(Of Integer))

            BuscarTitulo = If(BuscarTitulo = "B", "L", "B") ' intercambio los valores debido a que el campo se utiliza para el radio button de la pantalla  
            '(L :libres para bloquear pero debo bloquear cuando se selecciona, por eso mando B, B :bloqueados pero debo liberar cuando se selecciona, por eso mando L)

            For Each led In ListaCustodiasCliente.Where(Function(I) I.ObjParaBloquear = True).ToList()
                led.pdtmRecibo = FechaBusqueda
                objRet = Await dcProxy.BloqueoCustodias_Procesar(led.Custodia,
                                                                    led.Secuencia,
                                                                    led.strMotivoBloqueo,
                                                                    led.strNotasBloqueo,
                                                                    led.CantidadBloquear,
                                                                    BuscarTitulo,
                                                                    Program.Usuario,
                                                                    FechaBusqueda,
                                                                    Program.HashConexion).AsTask()
            Next
            BuscarCustadias()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema tratando en el llamado ConfirmarCambiarEstado",
                                           Me.ToString(), "ConfirmarCambiarEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ResultadoConfirmar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                ConfirmarCambiarEstado()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema tratando en el llamado ResultadoConfirmar",
                                           Me.ToString(), "ResultadoConfirmar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub CambiarEstadoCusto()
        Program.VerificarCambiosProxyServidor(dcProxy)
        dcProxy.SubmitChanges(AddressOf TerminoSubmitChangesGrabar, "insert")
    End Sub

    ''' <history>
    ''' ID Modificación  : 000001
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega al case la opcion Comitente, para permitir digitar el código del cliente en el detalle.
    ''' Fecha            : Abril 10/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 10/2013 - Resultado Ok 
    ''' 
    ''' </history>

    Private Sub _CamposBusquedaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CamposBusquedaSelected.PropertyChanged
        Try
            Select Case e.PropertyName

                Case "CodigoCliente"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected.CodigoCliente) And _mlogBuscarCliente Then
                        buscarComitente(_CamposBusquedaSelected.CodigoCliente)
                    End If
                Case "Nemotecnico"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected.Nemotecnico) And _mlogBuscarEspecie Then
                        BuscarEspecie(_CamposBusquedaSelected.Nemotecnico)
                    End If

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro",
                                 Me.ToString(), "_CustodiSelected.PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ISINFungibleSeleccionada(ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                strISINFungible = pobjItem.IdItem
                CamposBusquedaSelected.IsinFungible = pobjItem.IdItem
                CamposBusquedaSelected.DescripcionIsinFungible = pobjItem.Descripcion

                If CamposBusquedaSelected.Nemotecnico <> pobjItem.InfoAdicional01 Then
                    strEspecie1 = pobjItem.InfoAdicional01
                    CamposBusquedaSelected.Nemotecnico = pobjItem.InfoAdicional01
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al el Isin Fungible",
                                 Me.ToString(), "ISINFungibleSeleccionada", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub AsignarEstadoBloqueo(ByRef plogTodos As Boolean, ByRef pLogLimpiar As Boolean)
        Try
            If Not IsNothing(ListaCustodiasCliente) Then
                If Not IsNothing(strMotivoBloqueo) Then
                    Dim li As List(Of CustodiasCliente)
                    li = ListaCustodiasCliente.Where(Function(I) I.ObjParaBloquear = True).ToList()

                    If plogTodos And pLogLimpiar Then
                        li = ListaCustodiasCliente.Where(Function(I) I.ObjParaBloquear = False).ToList()
                        For Each obj In li
                            obj.strMotivoBloqueo = Nothing
                        Next
                    Else
                        For Each obj In li
                            If obj.ObjParaBloquear Then
                                If IsNothing(obj.strMotivoBloqueo) Then
                                    obj.strMotivoBloqueo = strMotivoBloqueo()
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema tratando de asignar el estado de bloqueo",
                                            Me.ToString(), "AsignarEstadoBloqueo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AsignarMotivoBloqueo()
        If Not IsNothing(ListaCustodiaSeleccionada) Then
            If ListaCustodiaSeleccionada.ObjParaBloquear Then
                If Not IsNothing(strMotivoBloqueo) And IsNothing(ListaCustodiaSeleccionada.strMotivoBloqueo) Then
                    ListaCustodiaSeleccionada.strMotivoBloqueo = strMotivoBloqueo
                End If
            ElseIf logActivo Then
                ListaCustodiaSeleccionada.strMotivoBloqueo = Nothing
            End If
        End If
        MyBase.CambioItem("ListaCustodiaSeleccionada")
    End Sub
#End Region

#Region "Métodos Autocompletar Cliente"
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
                If Not Me.CamposBusquedaSelected Is Nothing Then
                    If Not strIdComitente.Equals(Me.CamposBusquedaSelected.CodigoCliente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.CamposBusquedaSelected.CodigoCliente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If

            If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                objProxy.BuscadorClientes.Clear()
                objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

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
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCamposBusqueda()
                    Else
                        Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusqueda()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

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
        If Not IsNothing(pobjComitente) Then
            If (_mlogBuscarCliente) Then
                _CamposBusquedaSelected.NombreCliente = pobjComitente.Nombre
                HabilitarBuscEspe = True
            End If
        End If
    End Sub

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : LimpiarCamposBusqueda()
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub LimpiarCamposBusqueda()
        CamposBusquedaSelected.CodigoCliente = String.Empty
        CamposBusquedaSelected.NombreCliente = String.Empty
        CamposBusquedaSelected.Nemotecnico = String.Empty
        CamposBusquedaSelected.Especie = String.Empty
        ListaCustodiasCliente = Nothing
        HabilitarBuscEspe = False
        HabilitadoModificar = False
        TextoGrid = String.Empty
    End Sub
#End Region

#Region "Métodos Autocompletar Especie"

    ''' <summary>
    ''' Buscar los datos de la especie según el ID digitado por el usuario.
    ''' </summary>
    ''' <param name="pstrIdEspecie">ID de la Especie que se debe buscar.</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : En el método buscarNemotecnicoEspecificoQuery se envía el valor "T" en el parametro mstrAdmonValores
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Friend Sub BuscarEspecie(Optional ByVal pstrIdEspecie As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdEspecie As String = String.Empty
        Try
            If Not Me.CamposBusquedaSelected Is Nothing Then
                If Not strIdEspecie.Equals(Me.CamposBusquedaSelected.Nemotecnico) Then
                    If pstrIdEspecie.Trim.Equals(String.Empty) Then
                        strIdEspecie = Me.CamposBusquedaSelected.Nemotecnico
                    Else
                        strIdEspecie = pstrIdEspecie
                    End If

                    If Not strIdEspecie Is Nothing AndAlso Not strIdEspecie.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorEspecies.Clear()

                        'ID Modificación  : 000001
                        objProxy.Load(objProxy.buscarNemotecnicoEspecificoQuery("T", strIdEspecie, Program.Usuario, Program.HashConexion), AddressOf BuscarEspecieCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la especie", Me.ToString(), "BuscarEspecie", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar la especie cuando se digita el ID en el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Private Sub BuscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If lo.Entities.ToList.Count > 0 Then
                Me.EspecieSeleccionadaM(lo.Entities.ToList.Item(0))
            Else
                A2Utilidades.Mensajes.mostrarMensaje("La especie ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                CamposBusquedaSelected.Nemotecnico = String.Empty
                CamposBusquedaSelected.Especie = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "BuscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se valida si la especie existe y se autocompleta el campo Especie.
    ''' </summary>
    ''' <param name="pobjEspecie"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub EspecieSeleccionadaM(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                CamposBusquedaSelected.Especie = pobjEspecie.Especie
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la especie",
                                 Me.ToString(), "EspecieSeleccionadaM", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
#End Region

#Region "Metodos Asincronos"

    Private Sub TerminoTraerCustodiasCliente(ByVal lo As LoadOperation(Of CustodiasCliente))
        If Not lo.HasError Then
            ListaCustodiasCliente = dcProxy.CustodiasClientes
            If ListaCustodiasCliente.Count = 0 Then
                HabilitadoModificar = False
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron Títulos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TextoGrid = "Títulos Encontrados: 0 registros"
                logActivo_Consultado = logActivo
            Else
                TextoGrid = String.Format("Títulos Encontrados: {0} registros", ListaCustodiasCliente.Count)
                HabilitadoModificar = True
                logActivo_Consultado = logActivo
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes Custodias",
                                 Me.ToString(), "TerminoTraerCustodiasCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Public Sub TerminoSubmitChangesGrabar(ByVal So As SubmitOperation)
        Try
            If So.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se modificaron correctamente las custodias", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                IsBusy = True
                BuscarCustadias()
                'dcProxy.CustodiasClientes.Clear()
                'dcProxy.Load(dcProxy.ConsultarCustodiasClienteQuery(CodigoCliente, Nemotecnico, BuscarTitulo), AddressOf TerminoTraerCustodiasCliente, "Consultar")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Recibe el resultado de la consulta de parámetros
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks>SV20150305</remarks>
    Private Sub Terminotraerparametrolista(ByVal obj As LoadOperation(Of OYDUtilidades.valoresparametro))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            'Dim lista = obj.Entities.ToList
            For Each li In obj.Entities.ToList
                Select Case li.Parametro
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES"
                        If li.Valor = "SI" Then
                            logVisualizarMasDecimalesCantidad = True
                        Else
                            logVisualizarMasDecimalesCantidad = False
                        End If
                    Case "CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES_CONSULTA"
                        If li.Valor = "SI" Then
                            logVisualizarMasDecimalesCantidadConsulta = True
                        Else
                            logVisualizarMasDecimalesCantidadConsulta = False
                        End If
                End Select
            Next
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _FechaBusqueda As Date
    Public Property FechaBusqueda() As Date
        Get
            Return _FechaBusqueda
        End Get
        Set(ByVal value As Date)
            _FechaBusqueda = value
            MyBase.CambioItem("FechaBusqueda")
        End Set
    End Property

    Private _ListaCustodiasCliente As EntitySet(Of CustodiasCliente)
    Public Property ListaCustodiasCliente() As EntitySet(Of CustodiasCliente)
        Get
            Return _ListaCustodiasCliente
        End Get
        Set(ByVal value As EntitySet(Of CustodiasCliente))
            _ListaCustodiasCliente = value
            MyBase.CambioItem("ListaCustodiasCliente")
        End Set
    End Property


    Private _logActivo As Boolean
    Public Property logActivo() As Boolean
        Get
            Return _logActivo
        End Get
        Set(ByVal value As Boolean)
            _logActivo = value
        End Set
    End Property
    ''' <summary>
    ''' Se alamacena esta propiedad para controlar el mensaje de validacion cuando el usuario elige el motivo NO BLOQUEADO y va a liberar o bloquear.
    ''' Nota: Se crea una propiedad a parte ya que el usuario puedo manipular los optionButton despues de haber consultado.
    ''' Santiago Mazo 01/10/2015
    ''' </summary>
    Private _logActivo_Consultado As Boolean
    Public Property logActivo_Consultado() As Boolean
        Get
            Return _logActivo_Consultado
        End Get
        Set(ByVal value As Boolean)
            _logActivo_Consultado = value
            MyBase.CambioItem("logActivo_Consultado")
        End Set
    End Property

    Private _NombreColumna As String
    Public Property NombreColumna() As String
        Get
            Return _NombreColumna
        End Get
        Set(ByVal value As String)
            _NombreColumna = value
            MyBase.CambioItem("NombreColumna")
        End Set
    End Property

    Private _HabilitadoModificar As Boolean
    Public Property HabilitadoModificar() As Boolean
        Get
            Return _HabilitadoModificar
        End Get
        Set(ByVal value As Boolean)
            _HabilitadoModificar = value
            MyBase.CambioItem("HabilitadoModificar")
        End Set
    End Property


    Private _TextoGrid As String
    Public Property TextoGrid() As String
        Get
            Return _TextoGrid
        End Get
        Set(ByVal value As String)
            _TextoGrid = value
            MyBase.CambioItem("TextoGrid")
        End Set
    End Property

    Private _HabilitarBuscEspe As Boolean
    Public Property HabilitarBuscEspe() As Boolean
        Get
            Return _HabilitarBuscEspe
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBuscEspe = value
            MyBase.CambioItem("HabilitarBuscEspe")
        End Set
    End Property

    Private WithEvents _CamposBusquedaSelected As New CamposBusqueda
    Public Property CamposBusquedaSelected() As CamposBusqueda
        Get
            Return _CamposBusquedaSelected
        End Get
        Set(ByVal value As CamposBusqueda)
            _CamposBusquedaSelected = value
            MyBase.CambioItem("CamposBusquedaSelected")
        End Set
    End Property

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para la edición y cuando está prendido el parámetro CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES
    Private _logVisualizarMasDecimalesCantidad As Boolean = False
    Public Property logVisualizarMasDecimalesCantidad() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidad
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidad = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidad")
        End Set
    End Property

    'SV20150305
    'Indica si el campo cantidad se visualiza con mas decimales, esto es para el modo de consulta CUSTODIAS_CANTIDAD_INCREMENTAR_DECIMALES_CONSULTA
    Private _logVisualizarMasDecimalesCantidadConsulta As Boolean = False
    Public Property logVisualizarMasDecimalesCantidadConsulta() As Boolean
        Get
            Return _logVisualizarMasDecimalesCantidadConsulta
        End Get
        Set(ByVal value As Boolean)
            _logVisualizarMasDecimalesCantidadConsulta = value
            MyBase.CambioItem("logVisualizarMasDecimalesCantidadConsulta")
        End Set
    End Property

    Private _SeleccionarTodos As Boolean = False
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value

            If Not IsNothing(_ListaCustodiasCliente) Then
                dblCantidadTotal = 0
                If SeleccionarTodos Then
                    For Each led In ListaCustodiasCliente
                        led.ObjParaBloquear = True
                        dblCantidadTotal = dblCantidadTotal + led.CantidadBloquear
                    Next
                    AsignarEstadoBloqueo(True, False)

                Else
                    For Each led In ListaCustodiasCliente
                        led.ObjParaBloquear = False
                    Next
                    dblCantidadTotal = 0
                    AsignarEstadoBloqueo(True, True)
                End If

            End If

            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _strMotivoBloqueo As String
    Public Property strMotivoBloqueo() As String
        Get
            Return _strMotivoBloqueo
        End Get
        Set(ByVal value As String)
            _strMotivoBloqueo = value
            AsignarEstadoBloqueo(False, False)
            MyBase.CambioItem("strMotivoBloqueo")
        End Set
    End Property

    Private _ListaCustodiaSeleccionada As CustodiasCliente
    Public Property ListaCustodiaSeleccionada() As CustodiasCliente
        Get
            Return _ListaCustodiaSeleccionada
        End Get
        Set(ByVal value As CustodiasCliente)
            _ListaCustodiaSeleccionada = value

            AsignarMotivoBloqueo()
            MyBase.CambioItem("ListaCustodiaSeleccionada")
        End Set
    End Property

    Private _dblCantidadTotal As Double = 0
    Public Property dblCantidadTotal() As Double
        Get
            Return _dblCantidadTotal
        End Get
        Set(ByVal value As Double)
            _dblCantidadTotal = value
            MyBase.CambioItem("dblCantidadTotal")
        End Set
    End Property

#End Region

End Class

#Region "Propiedades Campos Búsqueda"
Public Class CamposBusqueda
    Implements INotifyPropertyChanged

    Private _CodigoCliente As String
    Public Property CodigoCliente() As String
        Get
            Return _CodigoCliente
        End Get
        Set(ByVal value As String)
            _CodigoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoCliente"))
        End Set
    End Property


    Private _NombreCliente As String
    Public Property NombreCliente() As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property

    Private _Nemotecnico As String
    Public Property Nemotecnico() As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
        End Set
    End Property


    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _IsinFungible As String
    Public Property IsinFungible() As String
        Get
            Return _IsinFungible
        End Get
        Set(ByVal value As String)
            _IsinFungible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsinFungible"))
        End Set
    End Property

    Private _DescripcionIsinFungible As String
    Public Property DescripcionIsinFungible() As String
        Get
            Return _DescripcionIsinFungible
        End Get
        Set(ByVal value As String)
            _DescripcionIsinFungible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionIsinFungible"))
        End Set
    End Property

    'Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
#End Region