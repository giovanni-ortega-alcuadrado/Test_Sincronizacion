'Ricardo Barrientos Pérez
'Junio 03/ 2020
'RABP20200603_Desarrollo de Historial de Valoración 

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.WEB

Public Class HistorialValoracionViewModel
    Inherits A2ControlMenu.A2ViewModel
#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public objViewPrincipal As HistorialValoracionView


#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub


    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True
                If Not IsNothing(RegistroEncabezado) Then
                    consultarHistoricosValoracion(RegistroEncabezado.intConsecutivo)
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private WithEvents _RegistroEncabezado As tblOrdenes
    Public Property RegistroEncabezado() As tblOrdenes
        Get
            Return _RegistroEncabezado
        End Get
        Set(ByVal value As tblOrdenes)
            _RegistroEncabezado = value
            If Not IsNothing(_RegistroEncabezado) Then
                consultarHistoricosValoracion(_RegistroEncabezado.intConsecutivo)
            End If

            MyBase.CambioItem("RegistroEncabezado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As CPX_OrdenesValoracionHistoricoConsultar
    Public Property EncabezadoEdicionSeleccionado() As CPX_OrdenesValoracionHistoricoConsultar
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As CPX_OrdenesValoracionHistoricoConsultar)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private _lstHistoricoValoracion As List(Of CPX_OrdenesValoracionHistoricoConsultar)
    Public Property lstHistoricoValoracion() As List(Of CPX_OrdenesValoracionHistoricoConsultar)
        Get
            Return _lstHistoricoValoracion
        End Get
        Set(ByVal value As List(Of CPX_OrdenesValoracionHistoricoConsultar))
            _lstHistoricoValoracion = value
            MyBase.CambioItem("lstHistoricoValoracion")
        End Set
    End Property



#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Proceso para la consulta de la ordenes pendintes por cumplir
    ''' </summary>
    Public Async Sub consultarHistoricosValoracion(ByVal pintIDConsecutivo As Integer)
        Try
            IsBusy = True

            Dim objRespuesta = Await mdcProxy.OrdenesDivisasHistoricoValoracion_ConsultarAsync(pintIDConsecutivo, Program.Usuario)
            If Not IsNothing(objRespuesta) Then
                lstHistoricoValoracion = objRespuesta.Value
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(),
                                                     "consultarOrdenesPendientes", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub




    Public Sub RefrescarOrden()
        Try
            consultarHistoricosValoracion(RegistroEncabezado.intConsecutivo)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
#End Region
End Class
