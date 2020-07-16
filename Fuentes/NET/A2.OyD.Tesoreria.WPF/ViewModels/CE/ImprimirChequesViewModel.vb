Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports A2Utilidades.Mensajes
Imports A2OyDImprimirCheques




Public Class ImprimirChequesViewModel

    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxyTesoreria As TesoreriaDomainContext
    Dim lngNroChequeFinal As Integer

#Region "Inicialización"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxyTesoreria = New TesoreriaDomainContext()
            Else
                dcProxyTesoreria = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
            BuscarChequesxImprimir()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                     Me.ToString(), "ImprimirChequesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos"

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Consulta los cheques pendientes por imprimir.
    ''' Fecha            : Mayo 28/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 28/2013 - Resultado Ok 
    ''' </history>
    Sub BuscarChequesxImprimir()
        Try
            IsBusy = True
            dcProxyTesoreria.ImprimirCheques.Clear()
            dcProxyTesoreria.Load(dcProxyTesoreria.ConsultarChequesxImprimirQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarChequesPorImprimir, "Consultar")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en consultar los cheques por aprobar", _
               Me.ToString(), "ImprimirChequesViewModel.Metodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método que se encarga de imprimir cheques.
    ''' </summary>
    ''' <remarks>SLB20130620</remarks>
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

            If Not IsNothing(_ChequesxImprimirSelected.Maximo) Then
                lngNroChequeFinal = _ChequesxImprimirSelected.lngActual + _ChequesxImprimirSelected.ChequeXImprimir - 1
                If lngNroChequeFinal > _ChequesxImprimirSelected.Maximo Then
                    A2Utilidades.Mensajes.mostrarMensaje("Excede en " & CStr(lngNroChequeFinal - _ChequesxImprimirSelected.Maximo) & " cheque(s), la máxima numeración del consecutivo " & _ChequesxImprimirSelected.NombreConsecutivo & ". Máximo [" & _ChequesxImprimirSelected.Maximo & "]", _
                                                         Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            mostrarMensajePregunta("Desea imprimir los cheques", _
                                   Program.TituloSistema, _
                                   "IMPRIMIR", _
                                   AddressOf TerminoPregunta, True, "¿Imprimir?")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir el cheque", _
               Me.ToString(), "ImprimirCheques", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub


    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                Invocar_Pantalla_Impresion()
                IsBusy = False
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
            objListaArgumentos.Add("plngIDDocumento," & CStr(0))
            objListaArgumentos.Add("pintNroImpresion," & CStr(1))
            'objListaArgumentos.Add("Credenciales")

            Dim clsimprimircheque As ImpresionCheque = New ImpresionCheque(objListaArgumentos)
            AddHandler clsimprimircheque.Closed, AddressOf TerminoImprimirCheque
            'clsimprimircheque.StrArgumentos = """" & "" & """ """ & strServidorRS & """ """ & strCarpetaReportes & Replace(_ChequesxImprimirSelected.FormatoBanco, " ", "+") _
            '                                   & """ ""plngIDBanco," & CStr(_ChequesxImprimirSelected.IdBanco) _
            '                                   & ":pstrConsecutivo," & _ChequesxImprimirSelected.NombreConsecutivo _
            '                                   & ":plngIDDocumento," & CStr(0) _
            '                                   & ":pintNroImpresion," & CStr(1) _
            '                                   & """ "

            clsimprimircheque.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Error Al levantar la pantalla Decimal impresion ERROR: " + ex.Message.ToString)
        End Try
    End Sub

    Private Sub TerminoImprimirCheque(ByVal sender As Object, ByVal e As EventArgs)
        Try
            BuscarChequesxImprimir()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al imprimir el cheque",
               Me.ToString(), "TerminoImprimirCheque", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Métodos Asincrónicos"

    Private Sub TerminoConsultarChequesPorImprimir(ByVal lo As LoadOperation(Of ImprimirCheques))
        IsBusy = False
        If Not lo.HasError Then
            ListaChequesxImprimir = dcProxyTesoreria.ImprimirCheques
            If ListaChequesxImprimir.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay cheques pendientes para imprimir", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If
    End Sub

#End Region

#Region "Propiedades"

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
