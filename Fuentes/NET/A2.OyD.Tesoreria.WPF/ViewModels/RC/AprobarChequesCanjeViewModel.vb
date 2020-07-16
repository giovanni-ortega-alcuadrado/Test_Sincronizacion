Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria


Public Class AprobarChequesCanjeViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged
    Dim dcProxyTesoreria As TesoreriaDomainContext
    Private objProxyUtilidades As UtilidadesDomainContext

#Region "Declaraciones"
    Public intIDCompaniaFirma As Integer = 0
    Dim dcProxy As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
#End Region

#Region "Inicialización"
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()
            dcProxyTesoreria = New TesoreriaDomainContext()
            objProxyUtilidades = New UtilidadesDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            dcProxyTesoreria = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        FechaBusqueda = Now.Date
        VerificarParametroCanje("INICIO")

        dcProxy.Load(dcProxy.TraerBolsaQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsa, Nothing)
    End Sub
#End Region

#Region "Métodos"

    Public Sub VerificarParametroCanje(ByVal pstrUserState As String)
        Try
            objProxyUtilidades.Verificaparametro("CONTROL_CANJE_CHEQUES",Program.Usuario, Program.HashConexion, AddressOf TerminoVerificarParametro, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el parametro de canje.", _
               Me.ToString(), "VerificarParametroCanje.Metodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Consulta los cheques pendientes por aprobar.
    ''' Fecha            : Mayo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    ''' </history>
    Sub BuscarChequesPorAprobar()
        IsBusy = True
        Try
            dcProxyTesoreria.ChequesPorAprobars.Clear()
            dcProxyTesoreria.Load(dcProxyTesoreria.ConsultarChequesPorAprobarQuery(FechaBusqueda, Program.Usuario, IDCompania, Program.HashConexion), AddressOf TerminoConsultarChequesPorAprobar, "Consultar")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar los cheques por aprobar", _
               Me.ToString(), "AprobarChequesCanjeViewModel.Metodos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Método para aprobar cheques.
    ''' Fecha            : Mayo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    ''' </history>
    ''' Modificado por   : Yessid Andrés Paniagua Pabón. Id Cambio: YAPP20160422
    ''' Descripción      : Control para evitar que se muestre mensaje varias veces el mensaje: "No se encontraron cheques por aprobar" .
    ''' Fecha            : Abril 22/2016
    ''' Pruebas CB       : Yessid Andrés Paniagua Pabón. - Abril 22/2016 - Resultado Ok 

    Dim logNotificado As Boolean = False 'YAPP20160422
    Sub AprobarCheques()
        Try
            Dim logHabilitarEspera As Boolean = False
            logNotificado = True 'YAPP20160422
            If Not IsNothing(ListaChequesxAprobar) Then
                For Each Linea In ListaChequesxAprobar
                    If Linea.ActualizarEstado Then
                        logHabilitarEspera = True
                        dcProxyTesoreria.Cheques_Aprobar(Linea.NombreConsecutivo, Linea.Documento, Linea.Secuencia, Linea.FechaConsignacion, Program.Usuario, Program.HashConexion, AddressOf TerminoAprobarCheques, "AprobarCheques")
                    End If
                Next
            End If

            If logHabilitarEspera Then
                IsBusy = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de aprobación de cheques", _
               Me.ToString(), "AprobarChequesCanjeViewModel.Metodos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Método para desaprobar cheques.
    ''' Fecha            : Mayo 06/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    ''' </history>
    ''' 

    Sub DesaprobarCheques()
        Try
            Dim logHabilitarEspera As Boolean = False
            logNotificado = True 'JABG 20160428
            If Not IsNothing(ListaChequesxAprobar) Then
                For Each Linea In ListaChequesxAprobar
                    If Linea.ActualizarEstado Then
                        If intIDCompaniaFirma <> IDCompania Then
                            logHabilitarEspera = True
                            dcProxyTesoreria.Cheques_Desaprobar(Linea.Tipo, Linea.Documento, Linea.Secuencia, Linea.NombreConsecutivo, Linea.CodComitente, ConsecutivoNota, Linea.FechaConsignacion, Program.Usuario, Program.HashConexion, AddressOf TerminoDesaprobarCheques, "DesaprobarCheques")
                        Else
                            If Not String.IsNullOrEmpty(ConsecutivoNota) Then
                                logHabilitarEspera = True
                                dcProxyTesoreria.Cheques_Desaprobar(Linea.Tipo, Linea.Documento, Linea.Secuencia, Linea.NombreConsecutivo, Linea.CodComitente, ConsecutivoNota, Linea.FechaConsignacion, Program.Usuario, Program.HashConexion, AddressOf TerminoDesaprobarCheques, "DesaprobarCheques")
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje(String.Format("Por favor seleccione un nombre de consecutivo de notas contables válido"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If

            If logHabilitarEspera Then
                IsBusy = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso de desaprobación de cheques",
               Me.ToString(), "AprobarChequesCanjeViewModel.Metodos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerBolsa(ByVal lo As LoadOperation(Of TraerBolsa))
        Try
            If Not lo.HasError Then
                ListaTraerBolsas = dcProxy.TraerBolsas
                If Not IsNothing(ListaTraerBolsas.First.lngIDCiaPrincipal) Then
                    intIDCompaniaFirma = ListaTraerBolsas.First.lngIDCiaPrincipal
                    IDCompania = ListaTraerBolsas.First.lngIDCiaPrincipal
                    NombreCompania = ListaTraerBolsas.First.strNombreCiaPrincipal
                End If

                RecargarComboConsecutivo()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerBolsa",
                                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerBolsa",
                                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub buscarCompania(Optional ByVal pintCompania As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Dim intCompania As Integer = 1

        Try
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemsQuery(pintCompania, "compania", "T", "incluircompaniasclaseshijasconfirma", String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf buscarCompaniaCompleted, pstrBusqueda)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la compañia", Me.ToString(), "buscarCompania", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarCompaniaCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.UserState.ToString = "buscarCompaniaEncabezado" Then
                    If Not IsNothing(IDCompania) Then
                        If lo.Entities.Where(Function(i) i.IdItem = IDCompania).Count > 0 Then
                            IDCompania = lo.Entities.Where(Function(i) i.IdItem = IDCompania.ToString).First.IdItem
                            NombreCompania = lo.Entities.Where(Function(i) i.IdItem = IDCompania.ToString).First.Nombre
                        Else
                            IDCompania = Nothing
                            NombreCompania = String.Empty
                            A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        IDCompania = Nothing
                        NombreCompania = String.Empty
                        A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                If lo.UserState.ToString = "buscarCompaniaEncabezado" Then
                    IDCompania = Nothing
                    NombreCompania = String.Empty
                End If
                A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "buscarCompaniaCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            Dim strNombreConsecutivo As String
            strNombreConsecutivo = "ConsecutivosNotasFirma"

            If Not lo.HasError Then
                Dim objListaConsecutivos As New List(Of OYDUtilidades.ItemCombo)

                objListaConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = strNombreConsecutivo).ToList

                If Not IsNothing(IDCompania) Then
                    listConsecutivos = Nothing
                    listConsecutivos = objListaConsecutivos.Where(Function(i) i.Retorno = IDCompania.ToString).ToList

                    If listConsecutivos.Count = 1 Then
                        ConsecutivoNota = listConsecutivos.First.ID
                    End If
                Else
                    listConsecutivos = Nothing
                End If
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub RecargarComboConsecutivo()
        Try
            IsBusy = True
            ListaChequesxAprobar = Nothing
            objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_Notas", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, "")
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recargar el combo consecutivo notas.",
                                     Me.ToString(), "RecargarComboConsecutivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos Asincrónicos"

    Private Sub TerminoConsultarChequesPorAprobar(ByVal lo As LoadOperation(Of ChequesPorAprobar))
        IsBusy = False
        If Not lo.HasError Then
            ListaChequesxAprobar = dcProxyTesoreria.ChequesPorAprobars
            If ListaChequesxAprobar.Count = 0 And logNotificado = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron cheques por aprobar o desaprobar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
            logNotificado = False 'YAPP20160422
        End If
    End Sub

    Private Sub TerminoAprobarCheques(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al aprobar los cheques seleccionados.", _
                     Me.ToString(), "TerminoAprobarCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
        Else
            BuscarChequesPorAprobar()
            ConsecutivoNota = Nothing
        End If
    End Sub

    Private Sub TerminoDesaprobarCheques(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema desaprobar los cheques seleccionados.", _
                     Me.ToString(), "TerminoDesaprobarCheques", Application.Current.ToString(), Program.Maquina, lo.Error)
        Else
            BuscarChequesPorAprobar()
            ConsecutivoNota = Nothing
        End If
    End Sub

    Private Sub TerminoVerificarParametro(ByVal lo As InvokeOperation(Of String))
        IsBusy = False
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema verificar el parametro.", _
                     Me.ToString(), "TerminoVerificarParametro", Application.Current.ToString(), Program.Maquina, lo.Error)
        Else
            If Not IsNothing(lo.Value) Then
                If lo.Value.ToUpper() = "SI" Then
                    If lo.UserState = "BUSCAR" Then
                        BuscarChequesPorAprobar()
                    ElseIf lo.UserState = "APROBAR" Then
                        AprobarCheques()
                    ElseIf lo.UserState = "DESAPROBAR" Then
                        DesaprobarCheques()
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La firma no maneja el control de canje de cheques, por favor verifique el párametro que maneja dicho control.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
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

    Private _ListaChequesxAprobar As EntitySet(Of ChequesPorAprobar)
    Public Property ListaChequesxAprobar() As EntitySet(Of ChequesPorAprobar)
        Get
            Return _ListaChequesxAprobar
        End Get
        Set(ByVal value As EntitySet(Of ChequesPorAprobar))
            _ListaChequesxAprobar = value
            MyBase.CambioItem("ListaChequesxAprobar")
        End Set
    End Property

    Private _Seleccionado As Boolean
    Public Property Seleccionado() As Boolean
        Get
            Return _Seleccionado
        End Get
        Set(ByVal value As Boolean)
            _Seleccionado = value
            MyBase.CambioItem("Seleccionado")
        End Set
    End Property

    Private _Check As Boolean
    Public Property Check() As Boolean
        Get
            Return _Check
        End Get
        Set(ByVal value As Boolean)
            _Check = value
            MyBase.CambioItem("Check")
            'JFSB 20160916 - Se ajusta la lógica para marcar o desmarcar todos los registros
                If ListaChequesxAprobar Is Nothing Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe buscar primero cheques en canje.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _Check = False
                ElseIf Not IsNothing(ListaChequesxAprobar) Then
                    For Each Linea In ListaChequesxAprobar
                        Linea.ActualizarEstado = _Check
                    Next
                End If
        End Set
    End Property

    Private _ConsecutivoNota As String
    Public Property ConsecutivoNota() As String
        Get
            Return _ConsecutivoNota
        End Get
        Set(ByVal value As String)
            _ConsecutivoNota = value
            MyBase.CambioItem("ConsecutivoNota")
        End Set
    End Property

    Private _IDCompania As System.Nullable(Of System.Int32)
    Public Property IDCompania() As System.Nullable(Of System.Int32)
        Get
            Return _IDCompania
        End Get
        Set(ByVal value As System.Nullable(Of System.Int32))
            _IDCompania = value
            If _IDCompania = intIDCompaniaFirma Then
                MostrarInfoCompaniaFirma = Visibility.Visible
                MostrarInfoCompaniaFondos = Visibility.Collapsed
            Else
                MostrarInfoCompaniaFirma = Visibility.Collapsed
                MostrarInfoCompaniaFondos = Visibility.Visible
            End If
            MyBase.CambioItem("IDCompania")
        End Set
    End Property

    Private _ListaTraerBolsas As EntitySet(Of TraerBolsa)
    Public Property ListaTraerBolsas() As EntitySet(Of TraerBolsa)
        Get
            Return _ListaTraerBolsas
        End Get
        Set(ByVal value As EntitySet(Of TraerBolsa))
            _ListaTraerBolsas = value
            MyBase.CambioItem("ListaTraerBolsas")
        End Set
    End Property

    Private _NombreCompania As String
    Public Property NombreCompania() As String
        Get
            Return _NombreCompania
        End Get
        Set(ByVal value As String)
            _NombreCompania = value
            MyBase.CambioItem("NombreCompania")
        End Set
    End Property

    Private _listConsecutivos As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property listConsecutivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listConsecutivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listConsecutivos = value
            MyBase.CambioItem("listConsecutivos")
        End Set
    End Property

    Private _MostrarInfoCompaniaFondos As Visibility = Visibility.Collapsed
    Public Property MostrarInfoCompaniaFondos() As Visibility
        Get
            Return _MostrarInfoCompaniaFondos
        End Get
        Set(ByVal value As Visibility)
            _MostrarInfoCompaniaFondos = value
            MyBase.CambioItem("MostrarInfoCompaniaFondos")
        End Set
    End Property

    Private _MostrarInfoCompaniaFirma As Visibility = Visibility.Visible
    Public Property MostrarInfoCompaniaFirma() As Visibility
        Get
            Return _MostrarInfoCompaniaFirma
        End Get
        Set(ByVal value As Visibility)
            _MostrarInfoCompaniaFirma = value
            MyBase.CambioItem("MostrarInfoCompaniaFirma")
        End Set
    End Property


#End Region

End Class
