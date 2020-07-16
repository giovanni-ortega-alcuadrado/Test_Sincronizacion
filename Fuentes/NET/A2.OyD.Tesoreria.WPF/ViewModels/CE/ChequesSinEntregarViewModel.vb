Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class ChequesSinEntregarViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel

Public Class ChequesSinEntregarViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext
    Public Const COMPROBANTE_EGRESO As String = "CE"
    Dim intcontadorinsertados As Integer

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()

        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))

        End If
        Try
            If Not Program.IsDesignMode() Then

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ChequesSinEntregarViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Metodos"


    ''' <summary>
    ''' proceso para la inserción de los cheques que se seleccionan en la pantalla como entregados
    ''' </summary>
    ''' <param name="intComprobante">Indica el ultimo comprobante ingresado para porder diferenciar el siguiente en insentarse</param>
    ''' <remarks>SV20130516</remarks>
    Public Sub GuardarChequesEntregados(Optional intComprobante As Integer = 0)
        Try

            Dim objCheque = (From C In ListaChequesSinEntrega Where C.Entregado = True And C.NroComprobanteM > intComprobante Select C).Take(1).Min

            If Not IsNothing(objCheque) Then

                'Se cuentan los registros insertados
                If intComprobante = 0 Then
                    intcontadorinsertados = 1
                Else
                    intcontadorinsertados = intcontadorinsertados + 1
                End If

                IsBusy = True
                intComprobante = objCheque.NroComprobanteM
                dcProxy.ComprobanteEgreso_ControlCheques_Ingresar(objCheque.NroComprobanteM, objCheque.ConsecutivoComprobanteM, COMPROBANTE_EGRESO _
                                                                  , objCheque.NroChequeM, objCheque.Entregado, objCheque.FechaServidorM _
                                                                   , objCheque.RecibidoPorM, Program.Usuario, Program.HashConexion, AddressOf TerminoGuardarCheque, intComprobante)

            Else
                If intcontadorinsertados > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Se actualizaron (" & intcontadorinsertados & ") registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    intcontadorinsertados = 0
                    ConsultarChequesSinEntregar()
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No hay registros seleccionados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                End If

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registros", _
                                 Me.ToString(), "GuardarChequesEntregados", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Proceso para consultar los cheques que estan sin entregar dependiendo del consecutivo seleccionado en la pantalla
    ''' </summary>
    ''' <remarks>SV20130516</remarks>
    Public Sub ConsultarChequesSinEntregar()
        Try
            IsBusy = True
            dcProxy.ChequesSinEntregas.Clear()
            dcProxy.Load(dcProxy.ComprobanteEgreso_ControlCheques_ConsultarQuery(Nothing, Nothing, _ParametrosConsultaSelected.Consecutivo, 2,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerChequesSinEntrega, "Consulta")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los cheques sin entregar", _
                                 Me.ToString(), "ConsultarChequesSinEntregar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "ResultadosAsincronicos"

    ''' <summary>
    ''' Proceso donde se recibe el resultado de los cheques sin entregar consultados
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20130516</remarks>
    Private Sub TerminoTraerChequesSinEntrega(ByVal lo As LoadOperation(Of OyDTesoreria.ChequesSinEntrega))
        IsBusy = False
        If Not lo.HasError Then
            If dcProxy.ChequesSinEntregas.Count > 0 Then
                ListaChequesSinEntrega = dcProxy.ChequesSinEntregas
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron cheques sin entregar para el consecutivo seleccionado", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los cheques sin entregar", _
                     Me.ToString(), "TerminoTraerChequesSinEntrega", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub


    ''' <summary>
    ''' Proceso donde se recibe la respuesta de la inserción del cheque
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20130516</remarks>
    Private Sub TerminoGuardarCheque(ByVal lo As InvokeOperation(Of Integer))
        IsBusy = False
        If Not lo.HasError Then
            GuardarChequesEntregados(lo.UserState)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el cheque", _
                     Me.ToString(), "TerminoGuardarCheque", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _ParametrosConsultaSelected As New ParametrosConsultaCheques
    Public Property ParametrosConsultaSelected As ParametrosConsultaCheques
        Get
            Return _ParametrosConsultaSelected
        End Get
        Set(ByVal value As ParametrosConsultaCheques)
            _ParametrosConsultaSelected = value
            MyBase.CambioItem("ParametrosConsultaSelected")
        End Set
    End Property

    Private _ListaChequesSinEntrega As EntitySet(Of OyDTesoreria.ChequesSinEntrega)
    Public Property ListaChequesSinEntrega As EntitySet(Of OyDTesoreria.ChequesSinEntrega)
        Get
            Return _ListaChequesSinEntrega
        End Get
        Set(ByVal value As EntitySet(Of OyDTesoreria.ChequesSinEntrega))
            _ListaChequesSinEntrega = value
            MyBase.CambioItem("ListaChequesSinEntrega")
            MyBase.CambioItem("ListaChequesSinEntregaPaged")
        End Set
    End Property

    Public ReadOnly Property ListaChequesSinEntregaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaChequesSinEntrega) Then
                Dim view = New PagedCollectionView(_ListaChequesSinEntrega)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

End Class

'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class ParametrosConsultaCheques.vb
'Propiedad de Alcuadrado S.A. 2013
Public Class ParametrosConsultaCheques
    Implements INotifyPropertyChanged

    Private _Consecutivo As String = "T"
    Public Property Consecutivo As String
        Get
            Return _Consecutivo
        End Get
        Set(ByVal value As String)
            _Consecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Consecutivo"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
