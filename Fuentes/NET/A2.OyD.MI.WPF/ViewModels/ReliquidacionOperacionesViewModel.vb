Imports Telerik.Windows.Controls
'-------------------------------------------------------------------------------------
'Descripción:       ViewModel para la pantalla de Reliquidaciòn de Operaciones
'Desarrollado por:  Santiago Alexander Vergara Orrego
'Fecha:             Noviembre 05/2013
'--------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports System.Text
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Public Class ReliquidacionOperacionesViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As MILADomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MILADomainContext()
        Else
            dcProxy = New MILADomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Try
            If Not Program.IsDesignMode() Then
                
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "ReliquidacionOperacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub


#Region "Declaraciones y Propiedades"

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            CambioPropiedad("strNombre")
        End Set
    End Property

    Private _lngIdComitente As String
    Public Property lngIdComitente() As String
        Get
            Return _lngIdComitente
        End Get
        Set(ByVal value As String)
            _lngIdComitente = value
            CambioPropiedad("lngIdComitente")
        End Set
    End Property

    Private _TipoFecha As String
    Public Property TipoFecha() As String
        Get
            Return _TipoFecha
        End Get
        Set(ByVal value As String)
            _TipoFecha = value
            CambioPropiedad("TipoFecha")
        End Set
    End Property

    Private _dtmFecha As DateTime
    Public Property dtmFecha() As DateTime
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As DateTime)
            _dtmFecha = value
            CambioPropiedad("dtmFecha")
        End Set
    End Property

    Private _dblComisionPesos As Double
    Public Property dblComisionPesos() As Double
        Get
            Return _dblComisionPesos
        End Get
        Set(ByVal value As Double)
            _dblComisionPesos = value
            CambioPropiedad("dblComisionPesos")
        End Set
    End Property

    Private _dblComisionPorcentaje As Double
    Public Property dblComisionPorcentaje() As Double
        Get
            Return _dblComisionPorcentaje
        End Get
        Set(ByVal value As Double)
            _dblComisionPorcentaje = value
            CambioPropiedad("dblComisionPorcentaje")
        End Set
    End Property

    Private _logSeleccionarTodos As Boolean
    Public Property logSeleccionarTodos() As Boolean
        Get
            Return _logSeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionarTodos = value
            CambioPropiedad("logSeleccionarTodos")
        End Set
    End Property

    Private _IsBusy As Boolean
    Public Property IsBusy() As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            CambioPropiedad("IsBusy")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub ConsultarLiquidaciones()

    End Sub

    'Public Sub GuardarCambios()
    '    Dim strUsuariosConcatenados As String = String.Empty

    '    Try

    '        For Each objeto In lstUsuariosAutorizados

    '            If strUsuariosConcatenados = String.Empty Then
    '                strUsuariosConcatenados = objeto.Descripcion
    '            Else
    '                strUsuariosConcatenados = strUsuariosConcatenados & "|" & objeto.Descripcion
    '            End If
    '        Next

    '        IsBusy = True
    '        dcProxy.UsuariosDTS_Actualizar(_intIdDTS, strUsuariosConcatenados, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizar_UsuariosDTS, "grabar")

    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los usuarios autorizados", Me.ToString(), "GuardarCambios", Application.Current.ToString(), Program.Maquina, ex)
    '        IsBusy = False
    '    End Try

    'End Sub

#End Region

#Region "Resultados Asincrónicos"

    'Private Sub TerminoActualizar_UsuariosDTS(ByVal lo As InvokeOperation(Of Boolean))
    '    Try
    '        If Not lo.HasError Then
    '            A2Utilidades.Mensajes.mostrarMensaje("Los cambios se grabaron exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los usuarios DTS", _
    '                                               Me.ToString(), "TerminoActualizar_UsuariosDTS" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
    '            lo.MarkErrorAsHandled()
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de actualizar los usuarios DTS", Me.ToString(), "TerminoActualizar_UsuariosDTS", Application.Current.ToString(), Program.Maquina, ex)

    '    End Try
    '    IsBusy = False
    'End Sub

    'Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
    '    Dim _lstUsuariosTemporal As New List(Of OYDUtilidades.ItemCombo)
    '    Try
    '        If Not lo.HasError Then


    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
    '                 Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
    '                   Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
    '    End Try
    '    IsBusy = False
    'End Sub

#End Region

#Region "Notificar Cambio"

    Public Sub CambioPropiedad(ByVal strPropiedad As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(strPropiedad))

    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

End Class






