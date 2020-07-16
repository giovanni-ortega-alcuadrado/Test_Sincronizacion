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
Imports A2ComunesImportaciones
Imports A2.OyD.OYDServer.RIA.Web.OyDYankees

Public Class ConstanciaOperacionYankeesViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As YankeesDomainContext
    Dim dcProxyImporta As ImportacionesDomainContext
    Dim logDatosValidos As Boolean = True

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New YankeesDomainContext()
            dcProxyImporta = New ImportacionesDomainContext()
        Else
            dcProxy = New YankeesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxyImporta = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If

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
    ''' Descripción      : Se método para validar la información ingresada en los campos de búsqueda.
    ''' Fecha            : Junio 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 17/2013 - Resultado Ok 
    ''' </history>
    Public Sub Validaciones()
        Try
            logDatosValidos = True
            If Liquidacion Then
                'If Trim(IdInicial) = "" Then
                '    logDatosValidos = False
                '    A2Utilidades.Mensajes.mostrarMensaje("Se requiere un valor válido para la liquidación inicial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                '    Exit Sub
                'End If
                If Trim(IdFinal) = "" Or IdFinal = 0 Then
                    logDatosValidos = False
                    A2Utilidades.Mensajes.mostrarMensaje("Se requiere un valor válido para la liquidación final", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
                If IdInicial > IdFinal Then
                    logDatosValidos = False
                    A2Utilidades.Mensajes.mostrarMensaje("El valor de la liquidación final debe ser mayor al de la inicial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
            Else
                If FechaInicial > FechaFinal Then
                    logDatosValidos = False
                    A2Utilidades.Mensajes.mostrarMensaje("El valor de la fecha final debe ser mayor al de la fecha inicial", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                    Exit Sub
                End If
                IdInicial = 0
                IdFinal = 0
                Liquidacion = False
            End If

            'If (logDatosValidos) Then
            dcProxy.tblLiquidaciones_YANKEEs.Clear()
            dcProxy.PlantillaConfirmacionOperacionYankees(_IdInicial, _IdFinal, _FechaInicial, _FechaFinal, _TipoOperacion, Program.Usuario, Program.HashConexion, AddressOf TerminoPlantillaConfirmacionOperacionYankees, "")
            IsBusy = True
            'Else
            'Validaciones()
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las liquidaciones", _
             Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)
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
            Liquidacion = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar los controles", _
             Me.ToString(), "LimpiarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Resultados Asincrónicos"

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se hace el llamado a un método ubicado en el proxy de importanciones, con el fin de generar el archivo de word.
    ''' Fecha            : Junio 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 17/2013 - Resultado Ok 
    ''' </history>
    Private Sub TerminoPlantillaConfirmacionOperacionYankees(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If Not String.IsNullOrEmpty(lo.Value) Then
                dcProxyImporta.Guardar_ArchivoServidorWord("Confirmación_Operacion_Yankees", Program.Usuario, String.Format("Confirmación_Operacion_Yankees.doc", Now.ToString("yyyy-mm-dd")), lo.Value.ToString, Program.HashConexion, AddressOf TerminoCrearArchivo, True)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El reporte no generó resultados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las plantillas de confirmación operación Yankees", _
                                             Me.ToString(), "TerminoPlantillaConfirmacionOperacionYankees", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub

    ''' <history>
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se encarga de mostrar la ventana popup para seleccionar el archivo de word generado.
    ''' Fecha            : Junio 17/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Junio 17/2013 - Resultado Ok 
    ''' </history>
    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New ListarArchivosDirectorioView("Confirmación_Operacion_Yankees")
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

    Private _ListaLiquidaciones As List(Of tblLiquidaciones_YANKEE)
    Public Property ListaLiquidaciones() As List(Of tblLiquidaciones_YANKEE)
        Get
            Return _ListaLiquidaciones
        End Get
        Set(ByVal value As List(Of tblLiquidaciones_YANKEE))
            _ListaLiquidaciones = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaLiquidaciones"))
        End Set
    End Property

    Private _LiquidacionSelected As tblLiquidaciones_YANKEE
    Public Property LiquidacionSelected() As tblLiquidaciones_YANKEE
        Get
            Return _LiquidacionSelected
        End Get
        Set(ByVal value As tblLiquidaciones_YANKEE)
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

    Private _IdFinal As Integer = 999999
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

    Private _Liquidacion As Boolean
    Public Property Liquidacion() As Boolean
        Get
            Return _Liquidacion
        End Get
        Set(ByVal value As Boolean)
            _Liquidacion = value
            If Not IsNothing(value) Then
                If value = True Then
                    IsEnabledLiquidacion = True
                    IsEnabledFecha = False
                Else
                    IsEnabledFecha = True
                    IsEnabledLiquidacion = False
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Liquidacion"))
        End Set
    End Property

    Private _TipoOperacion As String
    Public Property TipoOperacion() As String
        Get
            Return _TipoOperacion
        End Get
        Set(ByVal value As String)
            _TipoOperacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoOperacion"))
        End Set
    End Property

    Private _IsEnabledLiquidacion As Boolean
    Public Property IsEnabledLiquidacion() As Boolean
        Get
            Return _IsEnabledLiquidacion
        End Get
        Set(ByVal value As Boolean)
            _IsEnabledLiquidacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsEnabledLiquidacion"))
        End Set
    End Property

    Private _IsEnabledFecha As Boolean
    Public Property IsEnabledFecha() As Boolean
        Get
            Return _IsEnabledFecha
        End Get
        Set(ByVal value As Boolean)
            _IsEnabledFecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsEnabledFecha"))
        End Set
    End Property



#End Region

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

