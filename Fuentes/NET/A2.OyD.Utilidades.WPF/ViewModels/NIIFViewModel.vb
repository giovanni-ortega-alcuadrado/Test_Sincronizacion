Imports Telerik.Windows.Controls
'-------------------------------------------------------------------------------------
'Descripción:       ViewModel para la pantalla de NIIF matricular los códigos de conceptos
'Desarrollado por:  Ricardo Barrientos Pérez
'Fecha:             Noviembre 29/2013
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

Public Class NIIFViewModel
    Implements INotifyPropertyChanged

    Dim dcProxy As UtilidadesDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.NIIF_ConsultaInicialQuery(Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoCargaNIIF, "CARGAINICIAL")

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "NIIFViewModel.New", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub

#Region "Declaraciones y Propiedades"

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

    Private _logOperaciones As Boolean
    Public Property logOperaciones() As Boolean
        Get
            Return _logOperaciones
        End Get
        Set(ByVal value As Boolean)
            _logOperaciones = value
            CambioPropiedad("logOperaciones")
        End Set
    End Property
    Private _logTesoreria As Boolean
    Public Property logTesoreria() As Boolean
        Get
            Return _logTesoreria
        End Get
        Set(ByVal value As Boolean)
            _logTesoreria = value
            CambioPropiedad("logTesoreria")
        End Set
    End Property

    Private _intIdConcepto As Integer
    Public Property intIdConcepto() As Integer
        Get
            Return _intIdConcepto
        End Get
        Set(ByVal value As Integer)
            _intIdConcepto = value
            If lstValorCriterio.Count > 0 Then
                lstValorCriterio = New List(Of OYDUtilidades.ItemCombo)
            End If
            IsBusy = True
            dcProxy.Load(dcProxy.NIIF_ConsultaInicialQuery(_intIdConcepto, Program.Usuario, Program.HashConexion), AddressOf TerminoCargaNIIF, "CARGA")
        End Set
    End Property

    Private _intIdCriterio As Integer
    Public Property intIdCriterio() As Integer
        Get
            Return _intIdCriterio
        End Get
        Set(ByVal value As Integer)
            _intIdCriterio = value
            CargarGrigConsulta()
        End Set
    End Property

    Private _lstConceptos As New List(Of OYDUtilidades.ItemCombo)
    Public Property lstConceptos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _lstConceptos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _lstConceptos = value
            CambioPropiedad("lstConceptos")
        End Set
    End Property

    Private _lstCriterios As New List(Of OYDUtilidades.ItemCombo)
    Public Property lstCriterios() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _lstCriterios
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _lstCriterios = value

            CambioPropiedad("lstCriterios")
        End Set
    End Property

    Private _lstValorCriterio As New List(Of OYDUtilidades.ItemCombo)
    Public Property lstValorCriterio() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _lstValorCriterio
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _lstValorCriterio = value
            CambioPropiedad("lstValorCriterio")
            CambioPropiedad("lstValorCriterioPaged")
        End Set
    End Property

    Private _ActivarComboCriterio As Boolean
    Public Property ActivarComboCriterio() As Boolean
        Get
            Return _ActivarComboCriterio
        End Get
        Set(ByVal value As Boolean)
            _ActivarComboCriterio = value
            CambioPropiedad("ActivarComboCriterio")
        End Set
    End Property

    Private _SegmentoDefecto As String
    Public Property SegmentoDefecto() As String
        Get
            Return _SegmentoDefecto
        End Get
        Set(ByVal value As String)
            _SegmentoDefecto = value
            CambioPropiedad("SegmentoDefecto")
        End Set
    End Property

    Public ReadOnly Property lstValorCriterioPaged() As PagedCollectionView
        Get
            If Not IsNothing(_lstValorCriterio) Then
                Dim view = New PagedCollectionView(_lstValorCriterio)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ActivarGrupos As Boolean
    Public Property ActivarGrupos() As Boolean
        Get
            Return _ActivarGrupos
        End Get
        Set(ByVal value As Boolean)
            _ActivarGrupos = value
            CambioPropiedad("ActivarGrupos")
        End Set
    End Property

#End Region

#Region "Métodos"

    Private Sub CargarGrigConsulta()
        Try
            lstValorCriterio = New List(Of OYDUtilidades.ItemCombo)
            If _intIdCriterio <> 0 Then
                IsBusy = True
                dcProxy.Load(dcProxy.cargarCombosNiffQuery("CARGARGRIDCONSULTA", _intIdCriterio, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "CARGARGRIDCONSULTA")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga del grid", Me.ToString(), "CargarGrigConsulta", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub CargarCriterios()
        Try
            IsBusy = True
            dcProxy.Load(dcProxy.cargarCombosNiffQuery("CRITERIOS", _intIdConcepto, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "CRITERIOS")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga del combo de Criterios", Me.ToString(), "CargarCriterios", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub GuardarCambios()
        Dim xmlCompleto As String
        Dim xmlDetalle As String

        Try


            xmlCompleto = "<DatosNIIF>"

            For Each objeto In (From c In _lstValorCriterio Where Not c.Retorno = String.Empty Select c)

                xmlDetalle = "<Detalle IdCampo=""" & _intIdCriterio & """ lngCodCampo=""" & CStr(objeto.ID) & """ strCodSegNeg=""" & CStr(objeto.Retorno) & """></Detalle>"

                xmlCompleto = xmlCompleto & xmlDetalle
            Next

            xmlCompleto = xmlCompleto & "</DatosNIIF>"

            IsBusy = True

            dcProxy.ActualizarSegmentosNegocios(xmlCompleto, _intIdConcepto, _logOperaciones, _logTesoreria, _SegmentoDefecto, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizar_ActualizarSegmentosNegocios, "grabar")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los Segmentos", Me.ToString(), "GuardarCambios", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Public Sub ActivarDesactivarComboCriterio()

        If _ActivarComboCriterio = False Then
            ActivarComboCriterio = True

        End If

    End Sub

    Public Sub CancelarCambios()
        Try
            CargarGrigConsulta()
            A2Utilidades.Mensajes.mostrarMensaje("Se cancelo el proceso", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

            ActivarComboCriterio = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar el proceso", Me.ToString(), "CancelarCambios", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoActualizar_ActualizarSegmentosNegocios(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not lo.HasError Then
                A2Utilidades.Mensajes.mostrarMensaje("Los cambios se grabaron exitosamente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                ActivarComboCriterio = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar los códigos de Segmentos", _
                                                   Me.ToString(), "TerminoActualizar_ActualizarSegmentosNegocios" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de actualizar los códigos de segmento", Me.ToString(), "TerminoActualizar_ActualizarSegmentosNegocios", Application.Current.ToString(), Program.Maquina, ex)

        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoCargaNIIF(ByVal lo As LoadOperation(Of OYDUtilidades.NIIFInicial))
        Try
            If Not lo.HasError Then
                IsBusy = False
                ActivarGrupos = True

                If dcProxy.NIIFInicials.Count > 0 Then

                    With dcProxy.NIIFInicials.FirstOrDefault
                        _intIdConcepto = .Concepto
                        _intIdCriterio = .Criterio
                        logOperaciones = .Operaciones
                        logTesoreria = .Tesoreria
                        SegmentoDefecto = .strConceptoPorDefecto
                    End With

                    If _logOperaciones = True Or _logTesoreria = True Then
                        ActivarGrupos = False
                    End If

                    dcProxy.NIIFInicials.Clear()

                    ActivarComboCriterio = False
                Else
                    _intIdCriterio = 0
                    logOperaciones = False
                    logTesoreria = False
                    SegmentoDefecto = String.Empty
                    ActivarComboCriterio = True
                End If

                CambioPropiedad("intIdConcepto")
                CambioPropiedad("intIdCriterio")

                If lo.UserState = "CARGAINICIAL" Then
                    IsBusy = True
                    dcProxy.Load(dcProxy.cargarCombosNiffQuery("CARGARCONCEPTOS", 0, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "CARGARCONCEPTOS")
                ElseIf lo.UserState = "CARGA" Then
                    CargarCriterios()
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga inicial", _
                     Me.ToString(), "TerminoCargaInicial", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga inicial", _
                       Me.ToString(), "TerminoCargaInicial", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If Not lo.HasError Then
                IsBusy = False

                Select Case lo.UserState
                    Case "CARGARCONCEPTOS"
                        If dcProxy.ItemCombos.Count > 0 Then
                            lstConceptos = dcProxy.ItemCombos.ToList
                            CambioPropiedad("intIdConcepto")
                            dcProxy.ItemCombos.Clear()
                            CargarCriterios()
                        End If

                    Case "CRITERIOS"
                        If dcProxy.ItemCombos.Count > 0 Then
                            lstCriterios = dcProxy.ItemCombos.ToList
                            CambioPropiedad("intIdCriterio")
                            dcProxy.ItemCombos.Clear()
                            CargarGrigConsulta()
                        End If

                    Case "CARGARGRIDCONSULTA"
                        If dcProxy.ItemCombos.Count > 0 Then
                            lstValorCriterio = dcProxy.ItemCombos.ToList
                            dcProxy.ItemCombos.Clear()
                        End If

                End Select

                dcProxy.ItemCombos.Clear()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                     Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos", _
                       Me.ToString(), "TerminoCargarCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Notificar Cambio"

    Public Sub CambioPropiedad(ByVal strPropiedad As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(strPropiedad))

    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

End Class
