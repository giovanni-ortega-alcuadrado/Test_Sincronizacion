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
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports A2Utilidades.Mensajes

Public Class UnificarISINViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
        End If
        Try
            If Not Program.IsDesignMode() Then
                'IsBusy = True
                'dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery(""), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "FiltroInicial")
                'dcProxy1.Load(dcProxy1.TraerCuentasDecevalPorAgrupadoPorDefectoQuery, AddressOf TerminoTraerCuentasDecevalPorAgrupadorPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "UnificarISINViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Propiedades"

    Private WithEvents _unifica As ClaseUnificarISIN = New ClaseUnificarISIN
    Public Property unifica() As ClaseUnificarISIN
        Get
            Return _unifica
        End Get
        Set(ByVal value As ClaseUnificarISIN)
            _unifica = value
            MyBase.CambioItem("unifica")
        End Set
    End Property

    Private _Habilitaboton As Boolean = False
    Public Property Habilitaboton() As Boolean
        Get
            Return _Habilitaboton
        End Get
        Set(ByVal value As Boolean)
            _Habilitaboton = value
            MyBase.CambioItem("Habilitaboton")
        End Set
    End Property

#End Region

#Region "Métodos"

    Private Sub _unifica_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _unifica.PropertyChanged

        If e.PropertyName.Equals("ISINRetira") Then
            If unifica.ISINRetira <> String.Empty Then
                IsBusy = True
                dcProxy.UnificarEspecieISIN(unifica.ISINRetira, "B", "", unifica.Nemotecnico, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerIsin, "ISINRetira")
            End If
        ElseIf e.PropertyName.Equals("ISINUnifica") Then
            If unifica.ISINUnifica <> String.Empty Then
                IsBusy = True
                dcProxy.UnificarEspecieISIN(unifica.ISINUnifica, "B", "", unifica.Nemotecnico, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerIsin, "ISINUnifica")
            End If
        End If

    End Sub

    Sub ActualizaRegistro()
        'C1.Silverlight.C1MessageBox.Show("Desea unificar el ISIN: " + unifica.ISINRetira + vbCr + "Con el ISIN: " + unifica.ISINUnifica + vbCr + "De la Especie: " + unifica.Nemotecnico, _
        '                                 Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, _
        '                                 AddressOf Terminopregunta)
        mostrarMensajePregunta("Desea unificar el ISIN: " + unifica.ISINRetira + vbCr + "Con el ISIN: " + unifica.ISINUnifica + vbCr + "De la Especie: " + unifica.Nemotecnico, _
                               Program.TituloSistema, _
                               "UNIFICARISIN", _
                               AddressOf Terminopregunta, True, "¿Desea continuar?")
    End Sub

    Private Sub Terminopregunta(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            If (unifica.ISINRetira = unifica.ISINUnifica) Then
                A2Utilidades.Mensajes.mostrarMensaje("Los ISIN deben ser diferentes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Try
                IsBusy = True
                dcProxy.UnificarEspecieISIN(unifica.ISINRetira, "U", unifica.ISINUnifica, unifica.Nemotecnico, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerIsin, "ProcesoUnificar")
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                     Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        End If
    End Sub

    Sub cancelar()
        ErrorForma = ""
        unifica.Nemotecnico = String.Empty
        unifica.ISINRetira = String.Empty
        unifica.NombreISINRetira = String.Empty
        unifica.ISINUnifica = String.Empty
        unifica.NombreISINUnifica = String.Empty
        unifica.HabilitaISIN = False
        Editando = False
        Habilitaboton = False
    End Sub

#End Region

#Region "Resultados Asincrónicos"

    ''' <summary>
    ''' Métedo que recibe la respuesta si el ISIN existe, tambien la respuesta cuando se unifican el ISIN.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130321</remarks>
    Private Sub TerminoTraerIsin(ByVal lo As InvokeOperation(Of String))
        Try
            IsBusy = False
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "ISINRetira"
                        If lo.Value.ToString = "No existe" Then
                            A2Utilidades.Mensajes.mostrarMensaje("El ISIN no existe para la especie seleccionda. Por favor verique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            unifica.ISINRetira = String.Empty
                            unifica.NombreISINRetira = String.Empty
                            Habilitaboton = False
                        Else
                            unifica.NombreISINRetira = lo.Value.ToString
                            If unifica.ISINUnifica <> String.Empty Then Habilitaboton = True
                        End If

                    Case "ISINUnifica"
                        If lo.Value.ToString = "No existe" Then
                            A2Utilidades.Mensajes.mostrarMensaje("El ISIN no existe para la especie seleccionda. Por favor verique!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            unifica.ISINUnifica = String.Empty
                            unifica.NombreISINUnifica = String.Empty
                            Habilitaboton = False
                        Else
                            unifica.NombreISINUnifica = lo.Value.ToString
                            If unifica.ISINRetira <> String.Empty Then Habilitaboton = True
                        End If

                    Case "ProcesoUnificar"
                        A2Utilidades.Mensajes.mostrarMensaje("La unificación del ISIN fué exitosa", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                        cancelar()
                End Select
            Else
                If lo.UserState.ToString = "ProcesoUnificar" Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "La unificación del ISIN fué fallo", _
                         Me.ToString(), "TerminoTraerIsin", Application.Current.ToString(), Program.Maquina, lo.Error)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificación del Isin", _
                         Me.ToString(), "TerminoTraerIsin", Application.Current.ToString(), Program.Maquina, lo.Error)
                End If
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                 Me.ToString(), "TerminoTraerIsin", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


Public Class ClaseUnificarISIN
    Implements INotifyPropertyChanged

    Private _ISINRetira As String
    <Display(Name:="ISIN a retirar")> _
    Public Property ISINRetira As String
        Get
            Return _ISINRetira
        End Get
        Set(ByVal value As String)
            _ISINRetira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ISINRetira"))
        End Set
    End Property

    Private _ISINUnifica As String
    <Display(Name:="ISIN a unificar")> _
    Public Property ISINUnifica As String
        Get
            Return _ISINUnifica
        End Get
        Set(ByVal value As String)
            _ISINUnifica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ISINUnifica"))
        End Set
    End Property

    Private _NombreISINRetira As String
    <Display(Name:="Nombre")> _
    Public Property NombreISINRetira As String
        Get
            Return _NombreISINRetira
        End Get
        Set(ByVal value As String)
            _NombreISINRetira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreISINRetira"))
        End Set
    End Property

    Private _NombreISINUnifica As String
    <Display(Name:="Nombre")> _
    Public Property NombreISINUnifica As String
        Get
            Return _NombreISINUnifica
        End Get
        Set(ByVal value As String)
            _NombreISINUnifica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreISINUnifica"))
        End Set
    End Property

    Private _Nemotecnico As String
    <Display(Name:="Especie")> _
    Public Property Nemotecnico As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
        End Set
    End Property

    Private _HabilitaISIN As Boolean = False
    Public Property HabilitaISIN As Boolean
        Get
            Return _HabilitaISIN
        End Get
        Set(ByVal value As Boolean)
            _HabilitaISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitaISIN"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
