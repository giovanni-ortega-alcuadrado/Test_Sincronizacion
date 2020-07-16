Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: FacturasBancaInvViewModel.vb
'Generado el : 02/24/2012 07:45:33
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2ComunesImportaciones

Public Class ConstanciaOperacionViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As OTCDomainContext
    Dim dcProxyImporta As ImportacionesDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New OTCDomainContext()
            dcProxyImporta = New ImportacionesDomainContext()
        Else
            dcProxy = New OTCDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxyImporta = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OTCDomainContext.IOTCDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(1, 0, 0)
        Try
            LimpiarControles()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConstanciaOperacionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Métodos"

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega línea de código para llamar el método PlantillaConstanciaOperacionOTC.
    ''' Fecha            : Mayo 15/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 15/2013 - Resultado Ok 
    ''' </history>
    Public Sub ConsultarLiquidaciones()
        Try
            If Not IsNothing(_IdInicial) And Not IsNothing(_IdFinal) And _IdFinal <> 0 And Not IsNothing(_FechaInicial) And Not IsNothing(_FechaFinal) Then
                If _FechaInicial.Year >= 1753 And _FechaFinal.Year >= 1753 Then
                    dcProxy.Liquidaciones_OTs.Clear()
                    dcProxy.PlantillaConstanciaOperacionOTC(_IdInicial, _IdFinal, _FechaInicial, _FechaFinal, Program.Usuario, Program.HashConexion, AddressOf TerminoPlantillaConstanciaOperacionOTC, "")
                    IsBusy = True
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El valor mínimo permitido para la fecha es '01/01/1753'", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione todos los parámetros para realizar la busqueda", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los liquidaciones", _
             Me.ToString(), "ConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub LimpiarControles()
        Try
            _FechaInicial = Date.Now
            _FechaFinal = Date.Now
            _IdFinal = 999999
            _IdInicial = 0
            ListaLiquidaciones = Nothing
            _LiquidacionSelected = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los controles", _
             Me.ToString(), "LimpiarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoConsultarLiquidaciones(ByVal lo As LoadOperation(Of Liquidaciones_OT))

        If Not lo.HasError Then
            If dcProxy.Liquidaciones_OTs.Count < 1 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron Registros", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            Else
                ListaLiquidaciones = dcProxy.Liquidaciones_OTs.ToList
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de liquidaciones", _
                                             Me.ToString(), "TerminoConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub


    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se hace el llamado a un método ubicado en el proxy de importanciones, con el fin de generar el archivo de word.
    ''' Fecha            : Junio 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 12/2013 - Resultado Ok 
    ''' </history>
    Private Sub TerminoPlantillaConstanciaOperacionOTC(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If Not String.IsNullOrEmpty(lo.Value) Then
                dcProxyImporta.Guardar_ArchivoServidorWord("Carta_Operacion_OTC", Program.Usuario, String.Format("Carta_OTC.doc", Now.ToString("yyyy-mm-dd")), lo.Value, Program.HashConexion, AddressOf TerminoCrearArchivo, True)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El reporte no generó resultados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un probl.ema en la obtención de las plantillas de constancia de operación OTC", _
                                             Me.ToString(), "TerminoPlantillaConstanciaOperacionOTC", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se encarga de mostrar la ventana popup para seleccionar el archivo de word generado.
    ''' Fecha            : Junio 12/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 12/2013 - Resultado Ok 
    ''' </history>
    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New ListarArchivosDirectorioView("Carta_Operacion_OTC")
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al intentar mostrar la ventana de visualización de los archivos", _
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaLiquidaciones As List(Of Liquidaciones_OT)
    Public Property ListaLiquidaciones() As List(Of Liquidaciones_OT)
        Get
            Return _ListaLiquidaciones
        End Get
        Set(ByVal value As List(Of Liquidaciones_OT))
            _ListaLiquidaciones = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidaciones"))
        End Set
    End Property

    Private _LiquidacionSelected As Liquidaciones_OT
    Public Property LiquidacionSelected() As Liquidaciones_OT
        Get
            Return _LiquidacionSelected
        End Get
        Set(ByVal value As Liquidaciones_OT)
            _LiquidacionSelected = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("LiquidacionSelected"))
        End Set
    End Property

    Private _IdInicial As Integer
    Public Property IdInicial() As Integer
        Get
            Return _IdInicial
        End Get
        Set(ByVal value As Integer)
            _IdInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdInicial"))
        End Set
    End Property

    Private _IdFinal As Integer
    Public Property IdFinal() As Integer
        Get
            Return _IdFinal
        End Get
        Set(ByVal value As Integer)
            _IdFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdFinal"))
        End Set
    End Property

    Private _FechaInicial As DateTime
    Public Property FechaInicial() As DateTime
        Get
            Return _FechaInicial
        End Get
        Set(ByVal value As DateTime)
            _FechaInicial = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaInicial"))
        End Set
    End Property

    Private _FechaFinal As DateTime
    Public Property FechaFinal() As DateTime
        Get
            Return _FechaFinal
        End Get
        Set(ByVal value As DateTime)
            _FechaFinal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaFinal"))
        End Set
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    Private _logPuedeEnviar As Boolean
    Public Property logPuedeEnviar() As Boolean
        Get
            Return _logPuedeEnviar
        End Get
        Set(ByVal value As Boolean)
            _logPuedeEnviar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logPuedeEnviar"))
        End Set
    End Property

    Private _RutaArchivo As String
    Public Property RutaArchivo() As String
        Get
            Return _RutaArchivo
        End Get
        Set(ByVal value As String)
            _RutaArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RutaArchivo"))
        End Set
    End Property

#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class






