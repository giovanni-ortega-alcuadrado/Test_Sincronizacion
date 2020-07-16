Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports A2Utilidades.Mensajes
Imports A2OyDImprimirCheques
'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class RepetirChequesViewModel.vb
'Propiedad de Alcuadrado S.A. 
'Junio 28/2013
Public Class RepetirChequesViewModel

    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxyTesoreria As TesoreriaDomainContext
    Public moduloTesoreria As String
    Dim objProxy As UtilidadesDomainContext
    Dim strNombreConsecutivo As String = String.Empty
    Public logConsultarCompania As Boolean = True
    Public intIDCompaniaFirma As Integer = 0
    Public strNroDocumentoCompaniaFirma As String = String.Empty
    Public strNombreCortoCompaniaFirma As String = String.Empty
    Public strNombreCompaniaFirma As String = String.Empty

#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxyTesoreria = New TesoreriaDomainContext()
                objProxy = New UtilidadesDomainContext
            Else
                dcProxyTesoreria = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If

            IsBusy = True
            dcProxyTesoreria.Load(dcProxyTesoreria.TraerBolsaQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBolsa, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                     Me.ToString(), "RepetirChequesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerBolsa(ByVal lo As LoadOperation(Of TraerBolsa))
        Try
            If Not lo.HasError Then
                If Not IsNothing(dcProxyTesoreria.TraerBolsas.First.lngIDCiaPrincipal) Then
                    intIDCompaniaFirma = dcProxyTesoreria.TraerBolsas.First.lngIDCiaPrincipal
                    strNroDocumentoCompaniaFirma = dcProxyTesoreria.TraerBolsas.First.strNumeroDocumentoCiaPrincipal
                    strNombreCortoCompaniaFirma = dcProxyTesoreria.TraerBolsas.First.strNombreCorto
                    strNombreCompaniaFirma = dcProxyTesoreria.TraerBolsas.First.strNombreCiaPrincipal

                    IDCompania = intIDCompaniaFirma
                    NombreCompania = strNombreCompaniaFirma

                    ConsultarConsecutivosCompania()
                Else
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerBolsa",
                                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TraerBolsa",
                                                 Me.ToString(), "TerminoTraerBolsa", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Consulta los Cheques para repetir impresión
    ''' </summary>
    ''' <remarks>SV20130627</remarks>
    Sub BuscarChequeRepetir()
        Try
            If strConsecutivo <> String.Empty Then
                If intNumero <> 0 Then
                    IsBusy = True
                    dcProxyTesoreria.ImprimirCheques.Clear()
                    dcProxyTesoreria.Load(dcProxyTesoreria.ConsultarRepetirChequesQuery(strConsecutivo, intNumero, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarRepetirCheques, "Consultar")
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese un número de comprobante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione un consecutivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar los cheques",
               Me.ToString(), "BuscarChequeRepetir", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se encarga de imprimir cheques.
    ''' Tomado de la forma Imprimir Cheques
    ''' </summary>
    ''' <remarks>SV20130627</remarks>
    Public Sub ImprimirCheques()
        Try
            If IsNothing(_ChequesxImprimirSelected) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el cheque a imprimir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_ChequesxImprimirSelected.FormatoBanco) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe registrar el reporte para el banco " & _ChequesxImprimirSelected.NombreBanco, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            mostrarMensajePregunta("Se imprimirán los Cheques directamente por la impresora : " & vbCr & "Asegurese de tener preparada la impresora antes de Aceptar ",
                                   Program.TituloSistema,
                                   "IMPRIMIRANCHEQUES",
                                   AddressOf TerminoPregunta, False)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir el cheque",
               Me.ToString(), "ImprimirCheques", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub


    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Invocar_Pantalla_Impresion()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir el cheque",
               Me.ToString(), "TerminoPregunta", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub Invocar_Pantalla_Impresion()
        Try
            Dim strServidorRS As String = String.Empty
            Dim strCarpetaReportes As String = String.Empty

            If Application.Current.Resources.Contains("A2VServicioRS") = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque no se tiene configurado el nombre del servidor de Reportes.", Program.TituloSistema)
                IsBusy = False
                Exit Sub
            End If
            strServidorRS = Application.Current.Resources("A2VServicioRS").ToString.Trim()
            strServidorRS = strServidorRS.Substring(0, strServidorRS.LastIndexOf("/"))

            If strServidorRS.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque el nombre del servidor de Reportes está en blanco.", Program.TituloSistema)
                IsBusy = False
                Exit Sub
            End If

            If Application.Current.Resources.Contains("A2CarpetaReportes") = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque no se tiene el nombre de la carpeta de los reportes.", Program.TituloSistema)
                IsBusy = False
                Exit Sub
            End If
            strCarpetaReportes = Application.Current.Resources("A2CarpetaReportes").ToString.Trim()
            If strCarpetaReportes.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque el nombre de la carpeta de los Reportes está en blanco.", Program.TituloSistema)
                IsBusy = False
                Exit Sub
            End If

            Dim objListaArgumentos As New List(Of String)
            objListaArgumentos.Add(strServidorRS)
            objListaArgumentos.Add(strCarpetaReportes & Replace(_ChequesxImprimirSelected.FormatoBanco, " ", "+"))
            objListaArgumentos.Add("plngIDBanco," & CStr(_ChequesxImprimirSelected.IdBanco))
            objListaArgumentos.Add("pstrConsecutivo," & _ChequesxImprimirSelected.NombreConsecutivo)
            objListaArgumentos.Add("plngIDDocumento," & CStr(_intNumero))
            objListaArgumentos.Add("pintNroImpresion," & CStr(2))
            'objListaArgumentos.Add("Credenciales")

            Dim clsimprimircheque As ImpresionCheque = New ImpresionCheque(objListaArgumentos)
            AddHandler clsimprimircheque.Closed, AddressOf TerminoImprimirCheque
            'clsimprimircheque.StrArgumentos = """" & "" & """ """ & strServidorRS & """ """ & strCarpetaReportes & Replace(_ChequesxImprimirSelected.FormatoBanco, " ", "+") _
            '                                   & """ ""plngIDBanco," & CStr(_ChequesxImprimirSelected.IdBanco) _
            '                                   & ":pstrConsecutivo," & _ChequesxImprimirSelected.NombreConsecutivo _
            '                                   & ":plngIDDocumento," & CStr(_intNumero) _
            '                                   & ":pintNroImpresion," & CStr(2) _
            '                                   & """ "

            clsimprimircheque.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir el cheque",
               Me.ToString(), "Invocar_Pantalla_Impresion", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoImprimirCheque(ByVal sender As Object, ByVal e As EventArgs)
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir el cheque",
               Me.ToString(), "TerminoImprimirCheque", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Metodo que permite ejecutar el ImprimirReporte.exe
    ''' Tomado de la forma Imprimir Cheques
    ''' </summary>
    ''' <remarks>SV20130627</remarks>
    Public Sub ConsultarConsecutivosCompania(Optional ByVal pstrUserState As String = "CONSECUTIVOSCOMPANIA")
        Try
            IsBusy = True
            If Not IsNothing(objProxy.ItemCombos) Then
                objProxy.ItemCombos.Clear()
            End If

            objProxy.Load(objProxy.cargarCombosEspecificosQuery("Tesoreria_ComprobantesEgreso", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConsecutivos, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al consultar los consecutivos.", Me.ToString(), "ConsultarConsecutivosCompania", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Friend Sub buscarCompania(Optional ByVal pintCompania As Integer = 0, Optional ByVal pstrBusqueda As String = "")
        Dim intCompania As Integer = 1

        Try
            IsBusy = True
            objProxy.BuscadorGenericos.Clear()
            objProxy.Load(objProxy.buscarItemsQuery(pintCompania, "compania", "T", "incluircompaniasclasestodasconfirma", String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf buscarCompaniaCompleted, pstrBusqueda)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la compañia", Me.ToString(), "buscarCompania", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarCompaniaCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If Not IsNothing(IDCompania) Then
                    If lo.Entities.Where(Function(i) i.IdItem = IDCompania).Count > 0 Then
                        IDCompania = lo.Entities.Where(Function(i) i.IdItem = IDCompania.ToString).First.IdItem
                        NombreCompania = lo.Entities.Where(Function(i) i.IdItem = IDCompania.ToString).First.Nombre
                        ConsultarConsecutivosCompania()
                    Else
                        IsBusy = False
                        IDCompania = Nothing
                        NombreCompania = String.Empty
                        LimpiarConsecutivoCompania()
                        A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    IsBusy = False
                    IDCompania = Nothing
                    NombreCompania = String.Empty
                    LimpiarConsecutivoCompania()
                    A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                IsBusy = False
                IDCompania = Nothing
                NombreCompania = String.Empty
                LimpiarConsecutivoCompania()
                A2Utilidades.Mensajes.mostrarMensaje("La compañia ingresada no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "buscarCompaniaCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarConsecutivoCompania()
        Try
            If Not IsNothing(IDCompania) Then
                strNombreConsecutivo = String.Empty
            End If
            listConsecutivos = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar al limpiar la compañia.", Me.ToString(), "LimpiarConsecutivoCompania", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

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

    Public _IDCompania As Integer
    Public Property IDCompania() As Integer
        Get
            Return _IDCompania
        End Get
        Set(ByVal value As Integer)
            _IDCompania = value
            MyBase.CambioItem("IDCompania")
        End Set
    End Property

    Public _NombreCompania As String
    Public Property NombreCompania() As String
        Get
            Return _NombreCompania
        End Get
        Set(ByVal value As String)
            _NombreCompania = value
            MyBase.CambioItem("NombreCompania")
        End Set
    End Property

    Private Sub TerminoTraerConsecutivos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                Dim objListaConsecutivos As New List(Of OYDUtilidades.ItemCombo)

                objListaConsecutivos = objProxy.ItemCombos.Where(Function(y) y.Categoria = "NombreConsecutivoCE").ToList
                If Not IsNothing(IDCompania) Then
                    strConsecutivo = String.Empty
                    listConsecutivos = objListaConsecutivos.Where(Function(i) i.Retorno = IDCompania.ToString).ToList

                    If listConsecutivos.Count = 1 Then
                        strConsecutivo = listConsecutivos.First.ID
                    End If
                Else
                    listConsecutivos = Nothing
                    strConsecutivo = String.Empty
                End If
                IsBusy = False
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los documentos por aprobar.",
                           Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo validar los consecutivos del usuario.",
                                     Me.ToString(), "TerminoTraerConsecutivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos Asincrónicos"

    Private Sub TerminoConsultarRepetirCheques(ByVal lo As LoadOperation(Of ImprimirCheques))
        IsBusy = False
        If Not lo.HasError Then
            ListaChequesxImprimir = dcProxyTesoreria.ImprimirCheques
            If ListaChequesxImprimir.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El comprobante Nro. " & intNumero & " del consecutivo " & strConsecutivo & " no existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                intNumero = Nothing
            End If
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _strConsecutivo As String = ""
    Public Property strConsecutivo As String
        Get
            Return _strConsecutivo
        End Get
        Set(ByVal value As String)
            _strConsecutivo = value
            MyBase.CambioItem("strConsecutivo")
        End Set
    End Property

    Private _intNumero As Integer
    Public Property intNumero() As Integer
        Get
            Return _intNumero
        End Get
        Set(ByVal value As Integer)
            _intNumero = value
            MyBase.CambioItem("intNumero")
        End Set
    End Property

    Private _ListaChequesxImprimir As EntitySet(Of ImprimirCheques)
    Public Property ListaChequesxImprimir() As EntitySet(Of ImprimirCheques)
        Get
            Return _ListaChequesxImprimir
        End Get
        Set(ByVal value As EntitySet(Of ImprimirCheques))
            _ListaChequesxImprimir = value
            MyBase.CambioItem("ListaChequesxImprimir")
        End Set
    End Property

    Private _ChequesxImprimirSelected As ImprimirCheques
    Public Property ChequesxImprimirSelected As ImprimirCheques
        Get
            Return _ChequesxImprimirSelected
        End Get
        Set(ByVal value As ImprimirCheques)
            _ChequesxImprimirSelected = value
            MyBase.CambioItem("ChequesxImprimirSelected")
        End Set
    End Property


#End Region

End Class
